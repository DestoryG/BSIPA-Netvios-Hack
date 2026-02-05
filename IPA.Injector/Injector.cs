using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IPA.Config;
using IPA.Injector.Backups;
using IPA.Loader;
using IPA.Logging;
using IPA.Utilities;
using Mono.Cecil;
using Mono.Cecil.Cil;
using UnityEngine;

namespace IPA.Injector
{
	// Token: 0x02000005 RID: 5
	internal static class Injector
	{
		// Token: 0x06000011 RID: 17 RVA: 0x00002388 File Offset: 0x00000588
		internal static void Main(string[] args)
		{
			try
			{
				if (Environment.GetCommandLineArgs().Contains("--verbose"))
				{
					WinConsole.Initialize(true);
				}
				Injector.SetupLibraryLoading();
				Injector.EnsureDirectories();
				Logger.LogLevel printFilter = StandardLogger.PrintFilter;
				Logger.log.Debug("Initializing logger");
				SelfConfig.ReadCommandLine(Environment.GetCommandLineArgs());
				SelfConfig.Load();
				DisabledConfig.Load();
				if (AntiPiracy.IsInvalid(Environment.CurrentDirectory))
				{
					Logger.log.Error("Invalid installation; please buy the game to run BSIPA.");
				}
				else
				{
					CriticalSection.Configure();
					Logger.injector.Debug("Prepping bootstrapper");
					Injector.InstallBootstrapPatch();
					GameVersionEarly.Load();
					Updates.InstallPendingUpdates();
					LibLoader.SetupAssemblyFilenames(true);
					Injector.pluginAsyncLoadTask = PluginLoader.LoadTask();
					Injector.permissionFixTask = PermissionFix.FixPermissions(new DirectoryInfo(Environment.CurrentDirectory));
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000245C File Offset: 0x0000065C
		private static void EnsureDirectories()
		{
			string path;
			if (!Directory.Exists(path = Path.Combine(Environment.CurrentDirectory, "UserData")))
			{
				Directory.CreateDirectory(path);
			}
			if (!Directory.Exists(path = Path.Combine(Environment.CurrentDirectory, "Plugins")))
			{
				Directory.CreateDirectory(path);
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000024A7 File Offset: 0x000006A7
		private static void SetupLibraryLoading()
		{
			if (Injector.loadingDone)
			{
				return;
			}
			Injector.loadingDone = true;
			LibLoader.Configure();
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000024BC File Offset: 0x000006BC
		private static void InstallHarmonyProtections()
		{
			HarmonyProtectorProxy.ProtectNull();
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000024C4 File Offset: 0x000006C4
		private static void InstallBootstrapPatch()
		{
			AssemblyName cAsmName = Assembly.GetExecutingAssembly().GetName();
			string managedPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string dataDir = new DirectoryInfo(managedPath).Parent.Name;
			string gameName = dataDir.Substring(0, dataDir.Length - 5);
			Logger.injector.Debug("Finding backup");
			string backupPath = Path.Combine(Environment.CurrentDirectory, "IPA", "Backups", gameName);
			BackupUnit bkp = BackupManager.FindLatestBackup(backupPath);
			if (bkp == null)
			{
				Logger.injector.Warn("No backup found! Was BSIPA installed using the installer?");
			}
			Logger.injector.Debug("Ensuring patch on UnityEngine.CoreModule exists");
			string unityPath = Path.Combine(managedPath, "UnityEngine.CoreModule.dll");
			using (CriticalSection.ExecuteSection())
			{
				using (AssemblyDefinition unityAsmDef = AssemblyDefinition.ReadAssembly(unityPath, new ReaderParameters
				{
					ReadWrite = false,
					InMemory = true,
					ReadingMode = ReadingMode.Immediate
				}))
				{
					ModuleDefinition unityModDef = unityAsmDef.MainModule;
					bool modified = false;
					foreach (AssemblyNameReference asmref in unityModDef.AssemblyReferences)
					{
						if (asmref.Name == cAsmName.Name && asmref.Version != cAsmName.Version)
						{
							asmref.Version = cAsmName.Version;
							modified = true;
						}
					}
					TypeDefinition application = unityModDef.GetType("UnityEngine", "Camera");
					if (application == null)
					{
						Logger.injector.Critical("UnityEngine.CoreModule doesn't have a definition for UnityEngine.Camera!Nothing to patch to get ourselves into the Unity run cycle!");
					}
					else
					{
						MethodDefinition cctor = null;
						foreach (MethodDefinition i in application.Methods)
						{
							if (i.IsRuntimeSpecialName && i.Name == ".cctor")
							{
								cctor = i;
							}
						}
						MethodReference cbs = unityModDef.ImportReference(new Action(Injector.CreateBootstrapper).Method);
						if (cctor == null)
						{
							cctor = new MethodDefinition(".cctor", Mono.Cecil.MethodAttributes.Static | Mono.Cecil.MethodAttributes.SpecialName | Mono.Cecil.MethodAttributes.RTSpecialName, unityModDef.TypeSystem.Void);
							application.Methods.Add(cctor);
							modified = true;
							ILProcessor ilprocessor = cctor.Body.GetILProcessor();
							ilprocessor.Emit(OpCodes.Call, cbs);
							ilprocessor.Emit(OpCodes.Ret);
						}
						else
						{
							ILProcessor ilp = cctor.Body.GetILProcessor();
							for (int j = 0; j < Math.Min(2, cctor.Body.Instructions.Count); j++)
							{
								Instruction ins = cctor.Body.Instructions[j];
								if (j != 0)
								{
									if (j == 1)
									{
										if (ins.OpCode != OpCodes.Ret)
										{
											ilp.Replace(ins, ilp.Create(OpCodes.Ret));
											modified = true;
										}
									}
								}
								else if (ins.OpCode != OpCodes.Call)
								{
									ilp.Replace(ins, ilp.Create(OpCodes.Call, cbs));
									modified = true;
								}
								else
								{
									MethodReference methodReference = ins.Operand as MethodReference;
									if (((methodReference != null) ? methodReference.FullName : null) != cbs.FullName)
									{
										ilp.Replace(ins, ilp.Create(OpCodes.Call, cbs));
										modified = true;
									}
								}
							}
						}
						if (modified)
						{
							BackupUnit bkp3 = bkp;
							if (bkp3 != null)
							{
								bkp3.Add(unityPath);
							}
							unityAsmDef.Write(unityPath);
						}
					}
				}
			}
			Logger.injector.Debug("Ensuring game assemblies are virtualized");
			bool isFirst = true;
			foreach (string name in SelfConfig.GameAssemblies_)
			{
				string ascPath = Path.Combine(managedPath, name);
				using (CriticalSection.ExecuteSection())
				{
					try
					{
						Logger.injector.Debug("Virtualizing " + name);
						using (VirtualizedModule ascModule = VirtualizedModule.Load(ascPath))
						{
							ascModule.Virtualize(cAsmName, delegate
							{
								BackupUnit bkp2 = bkp;
								if (bkp2 == null)
								{
									return;
								}
								bkp2.Add(ascPath);
							});
						}
					}
					catch (Exception e)
					{
						Logger.injector.Error("Could not virtualize " + ascPath);
						if (SelfConfig.Debug_.ShowHandledErrorStackTraces_)
						{
							Logger.injector.Error(e);
						}
					}
					if (isFirst)
					{
						try
						{
							Logger.injector.Debug("Applying anti-yeet patch");
							using (AssemblyDefinition ascAsmDef = AssemblyDefinition.ReadAssembly(ascPath, new ReaderParameters
							{
								ReadWrite = false,
								InMemory = true,
								ReadingMode = ReadingMode.Immediate
							}))
							{
								ascAsmDef.MainModule.GetType("IPAPluginsDirDeleter").Methods.Clear();
								ascAsmDef.Write(ascPath);
								isFirst = false;
							}
						}
						catch (Exception e2)
						{
							Logger.injector.Warn("Could not apply anti-yeet patch to " + ascPath);
							if (SelfConfig.Debug_.ShowHandledErrorStackTraces_)
							{
								Logger.injector.Warn(e2);
							}
						}
					}
				}
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002ADC File Offset: 0x00000CDC
		private static void CreateBootstrapper()
		{
			if (Injector.bootstrapped)
			{
				return;
			}
			Injector.bootstrapped = true;
			Application.logMessageReceived += delegate(string condition, string stackTrace, LogType type)
			{
				Logger.Level level = UnityLogRedirector.LogTypeToLevel(type);
				UnityLogProvider.UnityLogger.Log(level, condition ?? "");
				UnityLogProvider.UnityLogger.Log(level, stackTrace ?? "");
			};
			StdoutInterceptor.RedirectConsole();
			Injector.InstallHarmonyProtections();
			new GameObject("NonDestructiveBootstrapper").AddComponent<Bootstrapper>().Destroyed += Injector.Bootstrapper_Destroyed;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002B48 File Offset: 0x00000D48
		private static void Bootstrapper_Destroyed()
		{
			try
			{
				Injector.pluginAsyncLoadTask.Wait();
				Injector.permissionFixTask.Wait();
				Logger.log.Debug("Plugins loaded");
				Logger.log.Debug(string.Join(", ", PluginLoader.PluginsMetadata.StrJP<PluginMetadata>()));
				PluginComponent.Create();
			}
			catch (Exception e)
			{
				Logger.log.Debug(e);
				Process.GetCurrentProcess().Kill();
			}
		}

		// Token: 0x04000002 RID: 2
		private static Task pluginAsyncLoadTask;

		// Token: 0x04000003 RID: 3
		private static Task permissionFixTask;

		// Token: 0x04000004 RID: 4
		private static bool bootstrapped;

		// Token: 0x04000005 RID: 5
		private static bool loadingDone;
	}
}
