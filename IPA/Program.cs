using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using IPA.Patcher;

namespace IPA
{
	// Token: 0x02000005 RID: 5
	public static class Program
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00002895 File Offset: 0x00000A95
		public static Version Version
		{
			get
			{
				return Assembly.GetEntryAssembly().GetName().Version;
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000028A8 File Offset: 0x00000AA8
		[STAThread]
		public static void Main()
		{
			Arguments.CmdLine.Flags(new ArgumentFlag[]
			{
				Program.ArgHelp,
				Program.ArgWaitFor,
				Program.ArgForce,
				Program.ArgRevert,
				Program.ArgNoWait,
				Program.ArgStart,
				Program.ArgLaunch,
				Program.ArgNoRevert
			}).Process();
			if (Program.ArgHelp)
			{
				Arguments.CmdLine.PrintHelp();
				return;
			}
			try
			{
				Program.<>c__DisplayClass12_0 CS$<>8__locals1 = new Program.<>c__DisplayClass12_0();
				if (Program.ArgWaitFor.HasValue)
				{
					int num = int.Parse(Program.ArgWaitFor.Value);
					try
					{
						Process processById = Process.GetProcessById(num);
						Console.WriteLine(string.Format("Waiting for parent ({0}) process to die...", num));
						processById.WaitForExit();
					}
					catch (Exception)
					{
					}
				}
				CS$<>8__locals1.context = null;
				AppDomain.CurrentDomain.AssemblyResolve += CS$<>8__locals1.<Main>g__AssemblyLibLoader|0;
				string text = Arguments.CmdLine.PositionalArgs.FirstOrDefault((string s) => s.EndsWith(".exe"));
				if (text == null)
				{
					CS$<>8__locals1.context = PatchContext.Create(new DirectoryInfo(Directory.GetCurrentDirectory()).GetFiles().First((FileInfo o) => o.Extension == ".exe" && o.FullName != Assembly.GetEntryAssembly().Location).FullName);
				}
				else
				{
					CS$<>8__locals1.context = PatchContext.Create(text);
				}
				Program.Validate(CS$<>8__locals1.context);
				if (Program.ArgRevert || Program.Keyboard.IsKeyDown(Keys.LMenu))
				{
					Program.Revert(CS$<>8__locals1.context);
				}
				else
				{
					Program.Install(CS$<>8__locals1.context);
					Program.StartIfNeedBe(CS$<>8__locals1.context);
				}
			}
			catch (Exception ex)
			{
				Program.Fail(ex.Message);
			}
			Program.WaitForEnd();
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002A98 File Offset: 0x00000C98
		private static void WaitForEnd()
		{
			if (!Program.ArgNoWait)
			{
				Console.ForegroundColor = ConsoleColor.DarkYellow;
				Console.WriteLine("[Press any key to continue]");
				Console.ResetColor();
				Console.ReadKey();
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002AC4 File Offset: 0x00000CC4
		private static void Validate(PatchContext c)
		{
			if (!Directory.Exists(c.DataPathDst) || !File.Exists(c.EngineFile))
			{
				Program.Fail("Game does not seem to be a Unity project. Could not find the libraries to patch.");
				Console.WriteLine("DataPath: " + c.DataPathDst);
				Console.WriteLine("EngineFile: " + c.EngineFile);
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002B20 File Offset: 0x00000D20
		private static void Install(PatchContext context)
		{
			try
			{
				bool flag = true;
				if (File.Exists(Path.Combine(context.ProjectRoot, "winhttp.dll")))
				{
					string text = Path.Combine(context.ManagedPath, "IPA.Injector.dll");
					if (File.Exists(text) && new Version(FileVersionInfo.GetVersionInfo(text).FileVersion) > Program.Version)
					{
						flag = false;
					}
				}
				if (flag || Program.ArgForce)
				{
					BackupUnit backupUnit = new BackupUnit(context);
					if (!Program.ArgNoRevert)
					{
						Console.ForegroundColor = ConsoleColor.Cyan;
						Console.WriteLine("Restoring old version... ");
						if (BackupManager.HasBackup(context))
						{
							BackupManager.Restore(context);
						}
					}
					string text2 = Path.Combine(context.DataPathDst, "Plugins");
					if (Directory.Exists(text2))
					{
						Directory.GetFiles(text2).Any((string f) => f.EndsWith(".dll"));
					}
					bool flag2 = !BackupManager.HasBackup(context) || Program.ArgForce;
					Program.DetectArchitecture(context.Executable);
					Console.ForegroundColor = ConsoleColor.DarkCyan;
					Console.WriteLine("Installing files... ");
					Program.CopyAll(new DirectoryInfo(context.DataPathSrc), new DirectoryInfo(context.DataPathDst), flag2, backupUnit, null, true);
					Program.CopyAll(new DirectoryInfo(context.LibsPathSrc), new DirectoryInfo(context.LibsPathDst), flag2, backupUnit, null, true);
					Program.CopyAll(new DirectoryInfo(context.IPARoot), new DirectoryInfo(context.ProjectRoot), flag2, backupUnit, null, false);
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine("Not copying files because newer version already installed");
				}
				if (!Directory.Exists(context.PluginsFolder))
				{
					Console.ForegroundColor = ConsoleColor.DarkYellow;
					Console.WriteLine("Creating plugins folder... ");
					Directory.CreateDirectory(context.PluginsFolder);
					Console.ResetColor();
				}
			}
			catch (Exception ex)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				string text3 = "Oops! This should not have happened.\n\n";
				Exception ex2 = ex;
				Program.Fail(text3 + ((ex2 != null) ? ex2.ToString() : null));
			}
			Console.ResetColor();
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002D28 File Offset: 0x00000F28
		private static void Revert(PatchContext context)
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.Write("Restoring backup... ");
			if (BackupManager.Restore(context))
			{
				Console.WriteLine("Done!");
			}
			else
			{
				Console.WriteLine("Already vanilla or you removed your backups!");
			}
			if (File.Exists(context.ShortcutPath))
			{
				Console.WriteLine("Deleting shortcut...");
				File.Delete(context.ShortcutPath);
			}
			Console.WriteLine("");
			Console.WriteLine("--- Done reverting ---");
			Console.ResetColor();
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002DA0 File Offset: 0x00000FA0
		private static void StartIfNeedBe(PatchContext context)
		{
			if (Program.ArgStart.HasValue)
			{
				Process.Start(context.Executable, Program.ArgStart.Value);
				return;
			}
			List<string> list = Arguments.CmdLine.PositionalArgs.ToList<string>();
			list.Remove(context.Executable);
			if (Program.ArgLaunch)
			{
				Process.Start(context.Executable, Program.Args(list.ToArray()));
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002E10 File Offset: 0x00001010
		public static void ClearLine()
		{
			if (Program.IsConsole)
			{
				Console.SetCursorPosition(0, Console.CursorTop);
				int cursorTop = Console.CursorTop;
				Console.Write(new string(' ', Console.WindowWidth));
				Console.SetCursorPosition(0, cursorTop);
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002E4D File Offset: 0x0000104D
		private static IEnumerable<FileInfo> PassThroughInterceptor(FileInfo from, FileInfo to)
		{
			yield return to;
			yield break;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002E60 File Offset: 0x00001060
		public static void CopyAll(DirectoryInfo source, DirectoryInfo target, bool aggressive, BackupUnit backup, Func<FileInfo, FileInfo, IEnumerable<FileInfo>> interceptor = null, bool recurse = true)
		{
			if (interceptor == null)
			{
				interceptor = new Func<FileInfo, FileInfo, IEnumerable<FileInfo>>(Program.PassThroughInterceptor);
			}
			foreach (FileInfo fileInfo in source.GetFiles())
			{
				foreach (FileInfo fileInfo2 in interceptor(fileInfo, new FileInfo(Path.Combine(target.FullName, fileInfo.Name))))
				{
					if (!fileInfo2.Exists || !(fileInfo2.LastWriteTimeUtc >= fileInfo.LastWriteTimeUtc) || aggressive)
					{
						DirectoryInfo directory = fileInfo2.Directory;
						if (directory != null)
						{
							directory.Create();
						}
						Program.LineBack();
						Program.ClearLine();
						Console.WriteLine("Copying {0}", fileInfo2.FullName);
						backup.Add(fileInfo2);
						fileInfo.CopyTo(fileInfo2.FullName, true);
					}
				}
			}
			if (!recurse)
			{
				return;
			}
			foreach (DirectoryInfo directoryInfo in source.GetDirectories())
			{
				DirectoryInfo directoryInfo2 = new DirectoryInfo(Path.Combine(target.FullName, directoryInfo.Name));
				Program.CopyAll(directoryInfo, directoryInfo2, aggressive, backup, interceptor, true);
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002FA0 File Offset: 0x000011A0
		private static void Fail(string message)
		{
			Console.Error.WriteLine("ERROR: " + message);
			Program.WaitForEnd();
			Environment.Exit(1);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002FC2 File Offset: 0x000011C2
		public static string Args(params string[] args)
		{
			return string.Join(" ", args.Select(new Func<string, string>(Program.EncodeParameterArgument)).ToArray<string>());
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00002FE5 File Offset: 0x000011E5
		public static string EncodeParameterArgument(string original)
		{
			if (string.IsNullOrEmpty(original))
			{
				return original;
			}
			return Regex.Replace(Regex.Replace(original, "(\\\\*)\"", "$1\\$0"), "^(.*\\s.*?)(\\\\*)$", "\"$1$2$2\"");
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00003010 File Offset: 0x00001210
		public static Program.Architecture DetectArchitecture(string assembly)
		{
			Program.Architecture architecture;
			using (BinaryReader binaryReader = new BinaryReader(File.OpenRead(assembly)))
			{
				if (binaryReader.ReadUInt16() == 23117)
				{
					binaryReader.BaseStream.Seek(60L, SeekOrigin.Begin);
					uint num = binaryReader.ReadUInt32();
					binaryReader.BaseStream.Seek((long)((ulong)(num + 4U)), SeekOrigin.Begin);
					ushort num2 = binaryReader.ReadUInt16();
					if (num2 == 34404)
					{
						architecture = Program.Architecture.x64;
					}
					else if (num2 == 332)
					{
						architecture = Program.Architecture.x86;
					}
					else if (num2 == 512)
					{
						architecture = Program.Architecture.x64;
					}
					else
					{
						architecture = Program.Architecture.Unknown;
					}
				}
				else
				{
					architecture = Program.Architecture.Unknown;
				}
			}
			return architecture;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000030AC File Offset: 0x000012AC
		public static void ResetLine()
		{
			if (Program.IsConsole)
			{
				Console.CursorLeft = 0;
				return;
			}
			Console.Write("\r");
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000030C6 File Offset: 0x000012C6
		public static void LineBack()
		{
			if (Program.IsConsole)
			{
				Console.CursorTop--;
				return;
			}
			Console.Write("\u001b[1A");
		}

		// Token: 0x06000046 RID: 70
		[DllImport("kernel32.dll")]
		private static extern IntPtr GetConsoleWindow();

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000047 RID: 71 RVA: 0x000030E6 File Offset: 0x000012E6
		public static bool IsConsole
		{
			get
			{
				if (Program.isConsole == null)
				{
					Program.isConsole = new bool?(Program.GetConsoleWindow() != IntPtr.Zero);
				}
				return Program.isConsole.Value;
			}
		}

		// Token: 0x0400001C RID: 28
		public const string FileVersion = "4.0.6.0";

		// Token: 0x0400001D RID: 29
		public static readonly ArgumentFlag ArgHelp = new ArgumentFlag(new string[] { "--help", "-h" })
		{
			DocString = "prints this message"
		};

		// Token: 0x0400001E RID: 30
		public static readonly ArgumentFlag ArgWaitFor = new ArgumentFlag(new string[] { "--waitfor", "-w" })
		{
			DocString = "waits for the specified PID to exit",
			ValueString = "PID"
		};

		// Token: 0x0400001F RID: 31
		public static readonly ArgumentFlag ArgForce = new ArgumentFlag(new string[] { "--force", "-f" })
		{
			DocString = "forces the operation to go through"
		};

		// Token: 0x04000020 RID: 32
		public static readonly ArgumentFlag ArgRevert = new ArgumentFlag(new string[] { "--revert", "-r" })
		{
			DocString = "reverts the IPA installation"
		};

		// Token: 0x04000021 RID: 33
		public static readonly ArgumentFlag ArgNoRevert = new ArgumentFlag(new string[] { "--no-revert", "-R" })
		{
			DocString = "prevents a normal installation from first reverting"
		};

		// Token: 0x04000022 RID: 34
		public static readonly ArgumentFlag ArgNoWait = new ArgumentFlag(new string[] { "--nowait", "-n" })
		{
			DocString = "doesn't wait for user input after the operation"
		};

		// Token: 0x04000023 RID: 35
		public static readonly ArgumentFlag ArgStart = new ArgumentFlag(new string[] { "--start", "-s" })
		{
			DocString = "uses the specified arguments to start the game after the patch/unpatch",
			ValueString = "ARGUMENTS"
		};

		// Token: 0x04000024 RID: 36
		public static readonly ArgumentFlag ArgLaunch = new ArgumentFlag(new string[] { "--launch", "-l" })
		{
			DocString = "uses positional parameters as arguments to start the game after patch/unpatch"
		};

		// Token: 0x04000025 RID: 37
		private static bool? isConsole;

		// Token: 0x0200000A RID: 10
		public enum Architecture
		{
			// Token: 0x04000032 RID: 50
			x86,
			// Token: 0x04000033 RID: 51
			x64,
			// Token: 0x04000034 RID: 52
			Unknown
		}

		// Token: 0x0200000B RID: 11
		internal static class Keyboard
		{
			// Token: 0x0600005C RID: 92
			[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
			private static extern short GetKeyState(int keyCode);

			// Token: 0x0600005D RID: 93 RVA: 0x00003840 File Offset: 0x00001A40
			private static Program.Keyboard.KeyStates KeyState(Keys key)
			{
				Program.Keyboard.KeyStates keyStates = Program.Keyboard.KeyStates.None;
				short keyState = Program.Keyboard.GetKeyState((int)key);
				if (((int)keyState & 32768) == 32768)
				{
					keyStates |= Program.Keyboard.KeyStates.Down;
				}
				if ((keyState & 1) == 1)
				{
					keyStates |= Program.Keyboard.KeyStates.Toggled;
				}
				return keyStates;
			}

			// Token: 0x0600005E RID: 94 RVA: 0x00003871 File Offset: 0x00001A71
			public static bool IsKeyDown(Keys key)
			{
				return Program.Keyboard.KeyStates.Down == (Program.Keyboard.KeyState(key) & Program.Keyboard.KeyStates.Down);
			}

			// Token: 0x0600005F RID: 95 RVA: 0x0000387E File Offset: 0x00001A7E
			public static bool IsKeyToggled(Keys key)
			{
				return Program.Keyboard.KeyStates.Toggled == (Program.Keyboard.KeyState(key) & Program.Keyboard.KeyStates.Toggled);
			}

			// Token: 0x02000012 RID: 18
			[Flags]
			private enum KeyStates
			{
				// Token: 0x04000043 RID: 67
				None = 0,
				// Token: 0x04000044 RID: 68
				Down = 1,
				// Token: 0x04000045 RID: 69
				Toggled = 2
			}
		}
	}
}
