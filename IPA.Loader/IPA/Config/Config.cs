using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using IPA.Config.Providers;
using IPA.Utilities;

namespace IPA.Config
{
	/// <summary>
	/// An abstraction of a config file on disk, which handles synchronizing between a memory representation and the
	/// disk representation.
	/// </summary>
	// Token: 0x0200005B RID: 91
	public class Config
	{
		// Token: 0x06000286 RID: 646 RVA: 0x0000CEE2 File Offset: 0x0000B0E2
		static Config()
		{
			JsonConfigProvider.RegisterConfig();
		}

		/// <summary>
		/// Registers a <see cref="T:IPA.Config.IConfigProvider" /> to use for configs.
		/// </summary>
		/// <typeparam name="T">the type to register</typeparam>
		// Token: 0x06000287 RID: 647 RVA: 0x0000CEF3 File Offset: 0x0000B0F3
		public static void Register<T>() where T : IConfigProvider
		{
			Config.Register(typeof(T));
		}

		/// <summary>
		/// Registers a <see cref="T:IPA.Config.IConfigProvider" /> to use for configs.
		/// </summary>
		/// <param name="type">the type to register</param>
		// Token: 0x06000288 RID: 648 RVA: 0x0000CF04 File Offset: 0x0000B104
		public static void Register(Type type)
		{
			IConfigProvider inst = Activator.CreateInstance(type) as IConfigProvider;
			if (inst == null)
			{
				throw new ArgumentException("Type not an IConfigProvider");
			}
			if (Config.registeredProviders.ContainsKey(inst.Extension))
			{
				throw new InvalidOperationException("Extension provider for " + inst.Extension + " already exists");
			}
			Config.registeredProviders.Add(inst.Extension, inst);
		}

		/// <summary>
		/// Gets a <see cref="T:IPA.Config.Config" /> object using the specified list of preferred config types.
		/// </summary>
		/// <param name="configName">the name of the mod for this config</param>
		/// <param name="extensions">the preferred config types to try to get</param>
		/// <returns>a <see cref="T:IPA.Config.Config" /> using the requested format, or of type JSON.</returns>
		// Token: 0x06000289 RID: 649 RVA: 0x0000CF6C File Offset: 0x0000B16C
		public static Config GetConfigFor(string configName, params string[] extensions)
		{
			string chosenExt = extensions.FirstOrDefault((string s) => Config.registeredProviders.ContainsKey(s)) ?? "json";
			IConfigProvider provider = Config.registeredProviders[chosenExt];
			string filename = Path.Combine(UnityGame.UserDataPath, configName + "." + provider.Extension);
			Config config = new Config(configName, provider, new FileInfo(filename));
			ConfigRuntime.RegisterConfig(config);
			return config;
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0000CFE4 File Offset: 0x0000B1E4
		internal static Config GetConfigFor(string modName, ParameterInfo info)
		{
			string[] prefs = Array.Empty<string>();
			Config.PreferAttribute prefer = info.GetCustomAttribute<Config.PreferAttribute>();
			if (prefer != null)
			{
				prefs = prefer.PreferenceOrder;
			}
			Config.NameAttribute name = info.GetCustomAttribute<Config.NameAttribute>();
			if (name != null)
			{
				modName = name.Name;
			}
			return Config.GetConfigFor(modName, prefs);
		}

		/// <summary>
		/// Gets the name associated with this <see cref="T:IPA.Config.Config" /> object.
		/// </summary>
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600028B RID: 651 RVA: 0x0000D021 File Offset: 0x0000B221
		public string Name { get; }

		/// <summary>
		/// Gets the <see cref="T:IPA.Config.IConfigProvider" /> associated with this <see cref="T:IPA.Config.Config" /> object.
		/// </summary>
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600028C RID: 652 RVA: 0x0000D029 File Offset: 0x0000B229
		public IConfigProvider Provider { get; }

		/// <summary>
		/// Sets this object's <see cref="T:IPA.Config.IConfigStore" />. Can only be called once.
		/// </summary>
		/// <param name="store">the <see cref="T:IPA.Config.IConfigStore" /> to add to this instance</param>
		/// <exception cref="T:System.InvalidOperationException">If this was called before.</exception>
		// Token: 0x0600028D RID: 653 RVA: 0x0000D031 File Offset: 0x0000B231
		public void SetStore(IConfigStore store)
		{
			if (this.Store != null)
			{
				throw new InvalidOperationException("SetStore can only be called once");
			}
			this.Store = store;
			ConfigRuntime.ConfigChanged();
		}

		/// <summary>
		/// Forces a synchronous load from disk.
		/// </summary>
		// Token: 0x0600028E RID: 654 RVA: 0x0000D052 File Offset: 0x0000B252
		public void LoadSync()
		{
			this.LoadAsync().Wait();
		}

		/// <summary>
		/// Forces an asynchronous load from disk.
		/// </summary>
		// Token: 0x0600028F RID: 655 RVA: 0x0000D05F File Offset: 0x0000B25F
		public Task LoadAsync()
		{
			return ConfigRuntime.TriggerFileLoad(this);
		}

		// Token: 0x06000290 RID: 656 RVA: 0x0000D067 File Offset: 0x0000B267
		private Config(string name, IConfigProvider provider, FileInfo file)
		{
			this.Name = name;
			this.Provider = provider;
			this.File = file;
			this.configProvider = new ConfigProvider(file, provider);
		}

		// Token: 0x040000ED RID: 237
		private static readonly Dictionary<string, IConfigProvider> registeredProviders = new Dictionary<string, IConfigProvider>();

		// Token: 0x040000F0 RID: 240
		internal IConfigStore Store;

		// Token: 0x040000F1 RID: 241
		internal readonly FileInfo File;

		// Token: 0x040000F2 RID: 242
		internal readonly ConfigProvider configProvider;

		// Token: 0x040000F3 RID: 243
		internal int Writes;

		/// <summary>
		/// Specifies that a particular parameter is preferred to use a particular <see cref="T:IPA.Config.IConfigProvider" />. 
		/// If it is not available, also specifies backups. If none are available, the default is used.
		/// </summary>
		// Token: 0x0200011F RID: 287
		[AttributeUsage(AttributeTargets.Parameter)]
		public sealed class PreferAttribute : Attribute
		{
			/// <summary>
			/// The order of preference for the config type. 
			/// </summary>
			/// <value>the list of config extensions in order of preference</value>
			// Token: 0x170000D2 RID: 210
			// (get) Token: 0x060005AF RID: 1455 RVA: 0x00017499 File Offset: 0x00015699
			// (set) Token: 0x060005B0 RID: 1456 RVA: 0x000174A1 File Offset: 0x000156A1
			public string[] PreferenceOrder { get; private set; }

			/// <inheritdoc />
			/// <summary>
			/// Constructs the attribute with a specific preference list. Each entry is the extension without a '.'
			/// </summary>
			/// <param name="preference">The preferences in order of preference.</param>
			// Token: 0x060005B1 RID: 1457 RVA: 0x000174AA File Offset: 0x000156AA
			public PreferAttribute(params string[] preference)
			{
				this.PreferenceOrder = preference;
			}
		}

		/// <summary>
		/// Specifies a preferred config name, instead of using the plugin's name.
		/// </summary>
		// Token: 0x02000120 RID: 288
		[AttributeUsage(AttributeTargets.Parameter)]
		public sealed class NameAttribute : Attribute
		{
			/// <summary>
			/// The name to use for the config.
			/// </summary>
			/// <value>the name to use for the config</value>
			// Token: 0x170000D3 RID: 211
			// (get) Token: 0x060005B2 RID: 1458 RVA: 0x000174B9 File Offset: 0x000156B9
			// (set) Token: 0x060005B3 RID: 1459 RVA: 0x000174C1 File Offset: 0x000156C1
			public string Name { get; private set; }

			/// <inheritdoc />
			/// <summary>
			/// Constructs the attribute with a specific name.
			/// </summary>
			/// <param name="name">the name to use for the config.</param>
			// Token: 0x060005B4 RID: 1460 RVA: 0x000174CA File Offset: 0x000156CA
			public NameAttribute(string name)
			{
				this.Name = name;
			}
		}
	}
}
