using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using IPA.Config;
using IPA.Logging;
using IPA.Old;
using IPA.Utilities;
using IPA.Utilities.Async;
using Mono.Cecil;
using UnityEngine;

namespace IPA.Loader
{
	/// <summary>
	/// The manager class for all plugins.
	/// </summary>
	// Token: 0x0200004C RID: 76
	public static class PluginManager
	{
		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000223 RID: 547 RVA: 0x0000B0B0 File Offset: 0x000092B0
		internal static IEnumerable<PluginExecutor> BSMetas
		{
			get
			{
				return PluginManager._bsPlugins;
			}
		}

		/// <summary>
		/// Gets info about the enabled plugin with the specified name.
		/// </summary>
		/// <param name="name">the name of the plugin to get (must be an exact match)</param>
		/// <returns>the plugin metadata for the requested plugin or <see langword="null" /> if it doesn't exist or is disabled</returns>
		// Token: 0x06000224 RID: 548 RVA: 0x0000B0B8 File Offset: 0x000092B8
		public static PluginMetadata GetPlugin(string name)
		{
			return PluginManager.BSMetas.Select((PluginExecutor p) => p.Metadata).FirstOrDefault((PluginMetadata p) => p.Name == name);
		}

		/// <summary>
		/// Gets info about the enabled plugin with the specified ID.
		/// </summary>
		/// <param name="name">the ID name of the plugin to get (must be an exact match)</param>
		/// <returns>the plugin metadata for the requested plugin or <see langword="null" /> if it doesn't exist or is disabled</returns>
		// Token: 0x06000225 RID: 549 RVA: 0x0000B10C File Offset: 0x0000930C
		public static PluginMetadata GetPluginFromId(string name)
		{
			return PluginManager.BSMetas.Select((PluginExecutor p) => p.Metadata).FirstOrDefault((PluginMetadata p) => p.Id == name);
		}

		/// <summary>
		/// Gets a disabled plugin's metadata by its name.
		/// </summary>
		/// <param name="name">the name of the disabled plugin to get</param>
		/// <returns>the metadata for the corresponding plugin</returns>
		// Token: 0x06000226 RID: 550 RVA: 0x0000B160 File Offset: 0x00009360
		public static PluginMetadata GetDisabledPlugin(string name)
		{
			return PluginManager.DisabledPlugins.FirstOrDefault((PluginMetadata p) => p.Name == name);
		}

		/// <summary>
		/// Gets a disabled plugin's metadata by its ID.
		/// </summary>
		/// <param name="name">the ID of the disabled plugin to get</param>
		/// <returns>the metadata for the corresponding plugin</returns>
		// Token: 0x06000227 RID: 551 RVA: 0x0000B190 File Offset: 0x00009390
		public static PluginMetadata GetDisabledPluginFromId(string name)
		{
			return PluginManager.DisabledPlugins.FirstOrDefault((PluginMetadata p) => p.Id == name);
		}

		/// <summary>
		/// Creates a new transaction for mod enabling and disabling mods simultaneously.
		/// </summary>
		/// <returns>a new <see cref="T:IPA.Loader.StateTransitionTransaction" /> that captures the current state of loaded mods</returns>
		// Token: 0x06000228 RID: 552 RVA: 0x0000B1C0 File Offset: 0x000093C0
		public static StateTransitionTransaction PluginStateTransaction()
		{
			return new StateTransitionTransaction(PluginManager.EnabledPlugins, PluginManager.DisabledPlugins);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000B1D4 File Offset: 0x000093D4
		internal static Task CommitTransaction(StateTransitionTransaction transaction)
		{
			if (!transaction.HasStateChanged)
			{
				return Task.WhenAll(Array.Empty<Task>());
			}
			if (!UnityGame.OnMainThread)
			{
				StateTransitionTransaction transactionCopy = transaction.Clone();
				transaction.Dispose();
				return UnityMainThreadTaskScheduler.Factory.StartNew<Task>(() => PluginManager.CommitTransaction(transactionCopy)).Unwrap();
			}
			object obj = PluginManager.commitTransactionLockObject;
			Task task;
			lock (obj)
			{
				if (transaction.CurrentlyEnabled.Except(PluginManager.EnabledPlugins).Concat(PluginManager.EnabledPlugins.Except(transaction.CurrentlyEnabled)).Any<PluginMetadata>() || transaction.CurrentlyDisabled.Except(PluginManager.DisabledPlugins).Concat(PluginManager.DisabledPlugins.Except(transaction.CurrentlyDisabled)).Any<PluginMetadata>())
				{
					transaction.Dispose();
					throw new InvalidOperationException("Transaction no longer resembles the current state of plugins");
				}
				PluginManager.<>c__DisplayClass9_1 CS$<>8__locals2;
				CS$<>8__locals2.toEnable = transaction.ToEnable;
				IEnumerable<PluginMetadata> toDisable = transaction.ToDisable;
				transaction.Dispose();
				using (DisabledConfig.Instance.ChangeTransaction())
				{
					List<PluginMetadata> list = new List<PluginMetadata>();
					PluginManager.<CommitTransaction>g__DeTree|9_2(list, CS$<>8__locals2.toEnable, ref CS$<>8__locals2);
					using (List<PluginMetadata>.Enumerator enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							PluginMetadata meta = enumerator.Current;
							PluginExecutor executor = PluginManager.runtimeDisabledPlugins.FirstOrDefault((PluginExecutor e) => e.Metadata == meta);
							if (meta.RuntimeOptions == RuntimeOptions.DynamicInit)
							{
								if (executor != null)
								{
									PluginManager.runtimeDisabledPlugins.Remove(executor);
								}
								else
								{
									executor = PluginLoader.InitPlugin(meta, PluginManager.EnabledPlugins);
								}
								if (executor == null)
								{
									continue;
								}
							}
							PluginLoader.DisabledPlugins.Remove(meta);
							DisabledConfig.Instance.DisabledModIds.Remove(meta.Id ?? meta.Name);
							PluginManager.PluginEnableDelegate pluginEnabled = PluginManager.PluginEnabled;
							if (pluginEnabled != null)
							{
								pluginEnabled(meta, meta.RuntimeOptions != RuntimeOptions.DynamicInit);
							}
							if (meta.RuntimeOptions == RuntimeOptions.DynamicInit)
							{
								PluginManager._bsPlugins.Add(executor);
								try
								{
									executor.Enable();
								}
								catch (Exception e)
								{
									Logger.loader.Error("Error while enabling " + meta.Id + ":");
									Exception e2;
									Logger.loader.Error(e2);
								}
							}
						}
					}
					Task result = Task.WhenAll(Array.Empty<Task>());
					PluginExecutor[] disableExecs = toDisable.Select((PluginMetadata m) => PluginManager.BSMetas.FirstOrDefault((PluginExecutor e) => e.Metadata == m)).NonNull<PluginExecutor>().ToArray<PluginExecutor>();
					foreach (PluginExecutor exec in disableExecs)
					{
						PluginLoader.DisabledPlugins.Add(exec.Metadata);
						DisabledConfig.Instance.DisabledModIds.Add(exec.Metadata.Id ?? exec.Metadata.Name);
						if (exec.Metadata.RuntimeOptions == RuntimeOptions.DynamicInit)
						{
							PluginManager.runtimeDisabledPlugins.Add(exec);
							PluginManager._bsPlugins.Remove(exec);
						}
						PluginManager.PluginDisableDelegate pluginDisabled = PluginManager.PluginDisabled;
						if (pluginDisabled != null)
						{
							pluginDisabled(exec.Metadata, exec.Metadata.RuntimeOptions != RuntimeOptions.DynamicInit);
						}
					}
					IEnumerable<PluginManager.DisableExecutor> enumerable = disableExecs.Select(new Func<PluginExecutor, PluginManager.DisableExecutor>(PluginManager.<CommitTransaction>g__MakeDisableExec|9_4));
					Dictionary<PluginExecutor, Task> disabled = new Dictionary<PluginExecutor, Task>();
					result = Task.WhenAll(enumerable.Select((PluginManager.DisableExecutor d) => PluginManager.<CommitTransaction>g__Disable|9_6(d, disabled)));
					PluginManager.OnAnyPluginsStateChangedDelegate onAnyPluginsStateChanged = PluginManager.OnAnyPluginsStateChanged;
					if (onAnyPluginsStateChanged != null)
					{
						onAnyPluginsStateChanged(result, CS$<>8__locals2.toEnable, toDisable);
					}
					if (CS$<>8__locals2.toEnable.Concat(toDisable).Any((PluginMetadata m) => m.RuntimeOptions == RuntimeOptions.DynamicInit))
					{
						Action<Task> onPluginsStateChanged = PluginManager.OnPluginsStateChanged;
						if (onPluginsStateChanged != null)
						{
							onPluginsStateChanged(result);
						}
					}
					task = result;
				}
			}
			return task;
		}

		/// <summary>
		/// Checks if a given plugin is disabled.
		/// </summary>
		/// <param name="meta">the plugin to check</param>
		/// <returns><see langword="true" /> if the plugin is disabled, <see langword="false" /> otherwise.</returns>
		// Token: 0x0600022A RID: 554 RVA: 0x0000B648 File Offset: 0x00009848
		public static bool IsDisabled(PluginMetadata meta)
		{
			return PluginManager.DisabledPlugins.Contains(meta);
		}

		/// <summary>
		/// Checks if a given plugin is enabled.
		/// </summary>
		/// <param name="meta">the plugin to check</param>
		/// <returns><see langword="true" /> if the plugin is enabled, <see langword="false" /> otherwise.</returns>
		// Token: 0x0600022B RID: 555 RVA: 0x0000B658 File Offset: 0x00009858
		public static bool IsEnabled(PluginMetadata meta)
		{
			return PluginManager.BSMetas.Any((PluginExecutor p) => p.Metadata == meta);
		}

		/// <summary>
		/// Called whenever a plugin is enabled, before the plugin in question is enabled.
		/// </summary>
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600022C RID: 556 RVA: 0x0000B688 File Offset: 0x00009888
		// (remove) Token: 0x0600022D RID: 557 RVA: 0x0000B6BC File Offset: 0x000098BC
		public static event PluginManager.PluginEnableDelegate PluginEnabled;

		/// <summary>
		/// Called whenever a plugin is disabled, before the plugin in question is enabled.
		/// </summary>
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600022E RID: 558 RVA: 0x0000B6F0 File Offset: 0x000098F0
		// (remove) Token: 0x0600022F RID: 559 RVA: 0x0000B724 File Offset: 0x00009924
		public static event PluginManager.PluginDisableDelegate PluginDisabled;

		/// <summary>
		/// Called whenever any plugins have their state changed at runtime with the <see cref="T:System.Threading.Tasks.Task" /> representing that state change.
		/// </summary>
		/// <remarks>
		/// Note that this is called on the Unity main thread, and cannot therefore block, as the <see cref="T:System.Threading.Tasks.Task" />
		/// provided represents operations that also run on the Unity main thread.
		/// </remarks>
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000230 RID: 560 RVA: 0x0000B758 File Offset: 0x00009958
		// (remove) Token: 0x06000231 RID: 561 RVA: 0x0000B78C File Offset: 0x0000998C
		public static event Action<Task> OnPluginsStateChanged;

		/// <summary>
		/// Called whenever any plugins, regardless of whether or not their change occurs during runtime, have their state changed.
		/// </summary>
		/// <remarks>
		/// Note that this is called on the Unity main thread, and cannot therefore block, as the <see cref="T:System.Threading.Tasks.Task" />
		/// provided represents operations that also run on the Unity main thread.
		/// </remarks>
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000232 RID: 562 RVA: 0x0000B7C0 File Offset: 0x000099C0
		// (remove) Token: 0x06000233 RID: 563 RVA: 0x0000B7F4 File Offset: 0x000099F4
		public static event PluginManager.OnAnyPluginsStateChangedDelegate OnAnyPluginsStateChanged;

		/// <summary>
		/// Gets a list of all enabled BSIPA plugins. Use <see cref="P:IPA.Loader.PluginManager.EnabledPlugins" /> instead of this.
		/// </summary>
		/// <value>a collection of all enabled plugins as <see cref="T:IPA.Loader.PluginMetadata" />s</value>
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000234 RID: 564 RVA: 0x0000B827 File Offset: 0x00009A27
		[Obsolete("This is an old name that no longer accurately represents its value. Use EnabledPlugins instead.")]
		public static IEnumerable<PluginMetadata> AllPlugins
		{
			get
			{
				return PluginManager.EnabledPlugins;
			}
		}

		/// <summary>
		/// Gets a collection of all enabled plugins, as represented by <see cref="T:IPA.Loader.PluginMetadata" />.
		/// </summary>
		/// <value>a collection of all enabled plugins</value>
		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000235 RID: 565 RVA: 0x0000B82E File Offset: 0x00009A2E
		public static IEnumerable<PluginMetadata> EnabledPlugins
		{
			get
			{
				return PluginManager.BSMetas.Select((PluginExecutor p) => p.Metadata);
			}
		}

		/// <summary>
		/// Gets a list of disabled BSIPA plugins.
		/// </summary>
		/// <value>a collection of all disabled plugins as <see cref="T:IPA.Loader.PluginMetadata" /></value>
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000236 RID: 566 RVA: 0x0000B859 File Offset: 0x00009A59
		public static IEnumerable<PluginMetadata> DisabledPlugins
		{
			get
			{
				return PluginLoader.DisabledPlugins;
			}
		}

		/// <summary>
		/// Gets a read-only dictionary of an ignored plugin to the reason it was ignored, as an <see cref="T:IPA.Loader.IgnoreReason" />.
		/// </summary>
		/// <value>a dictionary of <see cref="T:IPA.Loader.PluginMetadata" /> to <see cref="T:IPA.Loader.IgnoreReason" /> of ignored plugins</value>
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000237 RID: 567 RVA: 0x0000B860 File Offset: 0x00009A60
		public static IReadOnlyDictionary<PluginMetadata, IgnoreReason> IgnoredPlugins
		{
			get
			{
				return PluginLoader.ignoredPlugins;
			}
		}

		/// <summary>
		/// An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of old IPA plugins.
		/// </summary>
		/// <value>all legacy plugin instances</value>
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000238 RID: 568 RVA: 0x0000B867 File Offset: 0x00009A67
		[Obsolete("This exists only to provide support for legacy IPA plugins based on the IPlugin interface.")]
		public static IEnumerable<IPlugin> Plugins
		{
			get
			{
				return PluginManager._ipaPlugins;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000239 RID: 569 RVA: 0x0000B86E File Offset: 0x00009A6E
		// (set) Token: 0x0600023A RID: 570 RVA: 0x0000B875 File Offset: 0x00009A75
		internal static IConfigProvider SelfConfigProvider { get; set; }

		// Token: 0x0600023B RID: 571 RVA: 0x0000B880 File Offset: 0x00009A80
		internal static void Load()
		{
			string pluginDirectory = UnityGame.PluginsPath;
			string exeName = Path.GetFileNameWithoutExtension(PluginManager.AppInfo.StartupPath);
			PluginManager._bsPlugins = new List<PluginExecutor>();
			PluginManager._ipaPlugins = new List<IPlugin>();
			if (!Directory.Exists(pluginDirectory))
			{
				return;
			}
			string cacheDir = Path.Combine(pluginDirectory, ".cache");
			string[] array;
			if (!Directory.Exists(cacheDir))
			{
				Directory.CreateDirectory(cacheDir);
			}
			else
			{
				array = Directory.GetFiles(cacheDir, "*");
				for (int i = 0; i < array.Length; i++)
				{
					File.Delete(array[i]);
				}
			}
			PluginManager._bsPlugins.AddRange(PluginLoader.LoadPlugins());
			List<string> metadataPaths = PluginLoader.PluginsMetadata.Select((PluginMetadata m) => m.File.FullName).ToList<string>();
			List<string> ignoredPaths = PluginLoader.ignoredPlugins.Select((KeyValuePair<PluginMetadata, IgnoreReason> m) => m.Key.File.FullName).Concat(PluginLoader.ignoredPlugins.SelectMany((KeyValuePair<PluginMetadata, IgnoreReason> m) => m.Key.AssociatedFiles.Select((FileInfo f) => f.FullName))).ToList<string>();
			List<string> disabledPaths = PluginManager.DisabledPlugins.Select((PluginMetadata m) => m.File.FullName).ToList<string>();
			foreach (string s in Directory.GetFiles(pluginDirectory, "*.dll"))
			{
				if (!metadataPaths.Contains(s) && !ignoredPaths.Contains(s) && !disabledPaths.Contains(s))
				{
					string pluginCopy = Path.Combine(cacheDir, Path.GetFileName(s));
					ModuleDefinition module = ModuleDefinition.ReadModule(Path.Combine(pluginDirectory, s));
					foreach (AssemblyNameReference @ref in module.AssemblyReferences)
					{
						if (@ref.Name == "IllusionPlugin" || @ref.Name == "IllusionInjector")
						{
							@ref.Name = "IPA.Loader";
						}
					}
					foreach (TypeReference ref2 in module.GetTypeReferences())
					{
						if (ref2.FullName == "IllusionPlugin.IPlugin")
						{
							ref2.Namespace = "IPA.Old";
						}
						if (ref2.FullName == "IllusionPlugin.IEnhancedPlugin")
						{
							ref2.Namespace = "IPA.Old";
						}
						if (ref2.FullName == "IllusionPlugin.IniFile")
						{
							ref2.Namespace = "IPA.Config";
						}
						if (ref2.FullName == "IllusionPlugin.IModPrefs")
						{
							ref2.Namespace = "IPA.Config";
						}
						if (ref2.FullName == "IllusionPlugin.ModPrefs")
						{
							ref2.Namespace = "IPA.Config";
						}
						if (ref2.FullName == "IllusionPlugin.Utils.ReflectionUtil")
						{
							ref2.Namespace = "IPA.Utilities";
						}
						if (ref2.FullName == "IllusionPlugin.Logging.Logger")
						{
							ref2.Namespace = "IPA.Logging";
						}
						if (ref2.FullName == "IllusionPlugin.Logging.LogPrinter")
						{
							ref2.Namespace = "IPA.Logging";
						}
						if (ref2.FullName == "IllusionInjector.PluginManager")
						{
							ref2.Namespace = "IPA.Loader";
						}
						if (ref2.FullName == "IllusionInjector.PluginComponent")
						{
							ref2.Namespace = "IPA.Loader";
						}
						if (ref2.FullName == "IllusionInjector.CompositeBSPlugin")
						{
							ref2.Namespace = "IPA.Loader.Composite";
						}
						if (ref2.FullName == "IllusionInjector.CompositeIPAPlugin")
						{
							ref2.Namespace = "IPA.Loader.Composite";
						}
						if (ref2.FullName == "IllusionInjector.Logging.UnityLogInterceptor")
						{
							ref2.Namespace = "IPA.Logging";
						}
						if (ref2.FullName == "IllusionInjector.Logging.StandardLogger")
						{
							ref2.Namespace = "IPA.Logging";
						}
						if (ref2.FullName == "IllusionInjector.Updating.SelfPlugin")
						{
							ref2.Namespace = "IPA.Updating";
						}
						if (ref2.FullName == "IllusionInjector.Updating.Backup.BackupUnit")
						{
							ref2.Namespace = "IPA.Updating.Backup";
						}
						if (ref2.Namespace == "IllusionInjector.Utilities")
						{
							ref2.Namespace = "IPA.Utilities";
						}
						if (ref2.Namespace == "IllusionInjector.Logging.Printers")
						{
							ref2.Namespace = "IPA.Logging.Printers";
						}
					}
					module.Write(pluginCopy);
				}
			}
			array = Directory.GetFiles(cacheDir, "*.dll");
			for (int i = 0; i < array.Length; i++)
			{
				IEnumerable<IPlugin> result = PluginManager.LoadPluginsFromFile(array[i]);
				if (result != null)
				{
					PluginManager._ipaPlugins.AddRange(result.NonNull<IPlugin>());
				}
			}
			Logger.log.Info(exeName);
			Logger.log.Info("Running on Unity " + Application.unityVersion);
			Logger.log.Info(string.Format("Game version {0}", UnityGame.GameVersion));
			Logger.log.Info("-----------------------------");
			Logger.log.Info(string.Format("Loading plugins from {0} and found {1}", Utils.GetRelativePath(pluginDirectory, Environment.CurrentDirectory), PluginManager._bsPlugins.Count + PluginManager._ipaPlugins.Count));
			Logger.log.Info("-----------------------------");
			foreach (PluginExecutor plugin in PluginManager._bsPlugins)
			{
				Logger.log.Info(string.Format("{0} ({1}): {2}", plugin.Metadata.Name, plugin.Metadata.Id, plugin.Metadata.Version));
			}
			Logger.log.Info("-----------------------------");
			foreach (IPlugin plugin2 in PluginManager._ipaPlugins)
			{
				Logger.log.Info(plugin2.Name + ": " + plugin2.Version);
			}
			Logger.log.Info("-----------------------------");
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000BF24 File Offset: 0x0000A124
		private static IEnumerable<IPlugin> LoadPluginsFromFile(string file)
		{
			PluginManager.<>c__DisplayClass45_0 CS$<>8__locals1;
			CS$<>8__locals1.file = file;
			List<IPlugin> ipaPlugins = new List<IPlugin>();
			if (!File.Exists(CS$<>8__locals1.file) || !CS$<>8__locals1.file.EndsWith(".dll", true, null))
			{
				return ipaPlugins;
			}
			try
			{
				Type[] types = Assembly.LoadFrom(CS$<>8__locals1.file).GetTypes();
				for (int i = 0; i < types.Length; i++)
				{
					IPlugin ipaPlugin = PluginManager.<LoadPluginsFromFile>g__OptionalGetPlugin|45_0<IPlugin>(types[i], ref CS$<>8__locals1);
					if (ipaPlugin != null)
					{
						ipaPlugins.Add(ipaPlugin);
					}
				}
			}
			catch (ReflectionTypeLoadException e)
			{
				Logger.loader.Error("Could not load the following types from " + Path.GetFileName(CS$<>8__locals1.file) + ":");
				Logger loader = Logger.loader;
				string text = "  ";
				string text2 = "\n  ";
				Exception[] loaderExceptions = e.LoaderExceptions;
				IEnumerable<string> enumerable;
				if (loaderExceptions == null)
				{
					enumerable = null;
				}
				else
				{
					enumerable = loaderExceptions.Select(delegate(Exception e1)
					{
						if (e1 == null)
						{
							return null;
						}
						return e1.Message;
					}).StrJP();
				}
				loader.Error(text + string.Join(text2, enumerable ?? Array.Empty<string>()));
			}
			catch (Exception e2)
			{
				Logger.loader.Error("Could not load " + Path.GetFileName(CS$<>8__locals1.file) + "!");
				Logger.loader.Error(e2);
			}
			return ipaPlugins;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000C090 File Offset: 0x0000A290
		[CompilerGenerated]
		internal static void <CommitTransaction>g__DeTree|9_2(List<PluginMetadata> into, IEnumerable<PluginMetadata> tree, ref PluginManager.<>c__DisplayClass9_1 A_2)
		{
			foreach (PluginMetadata st in tree)
			{
				if (A_2.toEnable.Contains(st) && !into.Contains(st))
				{
					PluginManager.<CommitTransaction>g__DeTree|9_2(into, st.Dependencies, ref A_2);
					into.Add(st);
				}
			}
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000C0FC File Offset: 0x0000A2FC
		[CompilerGenerated]
		internal static PluginManager.DisableExecutor <CommitTransaction>g__MakeDisableExec|9_4(PluginExecutor e)
		{
			return new PluginManager.DisableExecutor
			{
				Executor = e,
				Dependents = PluginManager.BSMetas.Where((PluginExecutor f) => f.Metadata.Dependencies.Contains(e.Metadata)).Select(new Func<PluginExecutor, PluginManager.DisableExecutor>(PluginManager.<CommitTransaction>g__MakeDisableExec|9_4))
			};
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000C15C File Offset: 0x0000A35C
		[CompilerGenerated]
		internal static Task <CommitTransaction>g__Disable|9_6(PluginManager.DisableExecutor exec, Dictionary<PluginExecutor, Task> alreadyDisabled)
		{
			Task task;
			if (alreadyDisabled.TryGetValue(exec.Executor, out task))
			{
				return task;
			}
			if (exec.Executor.Metadata.RuntimeOptions != RuntimeOptions.DynamicInit)
			{
				return Task.FromException(new CannotRuntimeDisableException(exec.Executor.Metadata));
			}
			Task res = Task.WhenAll(exec.Dependents.Select((PluginManager.DisableExecutor d) => PluginManager.<CommitTransaction>g__Disable|9_6(d, alreadyDisabled))).ContinueWith<Task>(delegate(Task t)
			{
				if (!t.IsFaulted)
				{
					return exec.Executor.Disable();
				}
				return Task.WhenAll(new Task[]
				{
					t,
					Task.FromException(new CannotRuntimeDisableException(exec.Executor.Metadata, "Dependents cannot be disabled for plugin"))
				});
			}, UnityMainThreadTaskScheduler.Default).Unwrap();
			alreadyDisabled.Add(exec.Executor, res);
			return res;
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000C224 File Offset: 0x0000A424
		[CompilerGenerated]
		internal static T <LoadPluginsFromFile>g__OptionalGetPlugin|45_0<T>(Type t, ref PluginManager.<>c__DisplayClass45_0 A_1) where T : class
		{
			if (t.FindInterfaces((Type t, object o) => t == o as Type, typeof(T)).Length != 0)
			{
				try
				{
					return Activator.CreateInstance(t) as T;
				}
				catch (Exception e)
				{
					Logger.loader.Error(string.Format("Could not load plugin {0} in {1}! {2}", t.FullName, Path.GetFileName(A_1.file), e));
				}
			}
			return default(T);
		}

		// Token: 0x040000DE RID: 222
		private static List<PluginExecutor> _bsPlugins;

		// Token: 0x040000DF RID: 223
		private static readonly object commitTransactionLockObject = new object();

		// Token: 0x040000E4 RID: 228
		private static readonly HashSet<PluginExecutor> runtimeDisabledPlugins = new HashSet<PluginExecutor>();

		// Token: 0x040000E5 RID: 229
		private static List<IPlugin> _ipaPlugins;

		// Token: 0x02000105 RID: 261
		private struct DisableExecutor
		{
			// Token: 0x040003A1 RID: 929
			public PluginExecutor Executor;

			// Token: 0x040003A2 RID: 930
			public IEnumerable<PluginManager.DisableExecutor> Dependents;
		}

		/// <summary>
		/// An invoker for the <see cref="E:IPA.Loader.PluginManager.PluginEnabled" /> event.
		/// </summary>
		/// <param name="plugin">the plugin that was enabled</param>
		/// <param name="needsRestart">whether it needs a restart to take effect</param>
		// Token: 0x02000106 RID: 262
		// (Invoke) Token: 0x06000564 RID: 1380
		public delegate void PluginEnableDelegate(PluginMetadata plugin, bool needsRestart);

		/// <summary>
		/// An invoker for the <see cref="E:IPA.Loader.PluginManager.PluginDisabled" /> event.
		/// </summary>
		/// <param name="plugin">the plugin that was disabled</param>
		/// <param name="needsRestart">whether it needs a restart to take effect</param>
		// Token: 0x02000107 RID: 263
		// (Invoke) Token: 0x06000568 RID: 1384
		public delegate void PluginDisableDelegate(PluginMetadata plugin, bool needsRestart);

		/// <summary>
		/// A delegate representing a state change event for any plugin.
		/// </summary>
		/// <param name="changeTask">the <see cref="T:System.Threading.Tasks.Task" /> representing the change</param>
		/// <param name="enabled">the plugins that were enabled in the change</param>
		/// <param name="disabled">the plugins that were disabled in the change</param>
		// Token: 0x02000108 RID: 264
		// (Invoke) Token: 0x0600056C RID: 1388
		public delegate void OnAnyPluginsStateChangedDelegate(Task changeTask, IEnumerable<PluginMetadata> enabled, IEnumerable<PluginMetadata> disabled);

		// Token: 0x02000109 RID: 265
		internal static class AppInfo
		{
			// Token: 0x0600056F RID: 1391
			[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
			private static extern int GetModuleFileName(HandleRef hModule, StringBuilder buffer, int length);

			// Token: 0x170000D1 RID: 209
			// (get) Token: 0x06000570 RID: 1392 RVA: 0x00017148 File Offset: 0x00015348
			public static string StartupPath
			{
				get
				{
					StringBuilder stringBuilder = new StringBuilder(260);
					PluginManager.AppInfo.GetModuleFileName(PluginManager.AppInfo.NullHandleRef, stringBuilder, stringBuilder.Capacity);
					return stringBuilder.ToString();
				}
			}

			// Token: 0x040003A3 RID: 931
			private static HandleRef NullHandleRef = new HandleRef(null, IntPtr.Zero);
		}
	}
}
