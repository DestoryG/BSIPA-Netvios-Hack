using System;

namespace IPA.Old
{
	/// <summary>
	/// Interface for generic Illusion unity plugins. Every class that implements this will be loaded if the DLL is placed in
	/// Plugins.
	/// </summary>
	// Token: 0x02000027 RID: 39
	[Obsolete("When building plugins for Beat Saber, use the plugin attributes starting with PluginAttribute")]
	public interface IPlugin
	{
		/// <summary>
		/// Gets the name of the plugin.
		/// </summary>
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000DA RID: 218
		string Name { get; }

		/// <summary>
		/// Gets the version of the plugin.
		/// </summary>
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000DB RID: 219
		string Version { get; }

		/// <summary>
		/// Gets invoked when the application is started.
		/// </summary>
		// Token: 0x060000DC RID: 220
		void OnApplicationStart();

		/// <summary>
		/// Gets invoked when the application is closed.
		/// </summary>
		// Token: 0x060000DD RID: 221
		void OnApplicationQuit();

		/// <summary>
		/// Gets invoked whenever a level is loaded.
		/// </summary>
		/// <param name="level"></param>
		// Token: 0x060000DE RID: 222
		void OnLevelWasLoaded(int level);

		/// <summary>
		/// Gets invoked after the first update cycle after a level was loaded.
		/// </summary>
		/// <param name="level"></param>
		// Token: 0x060000DF RID: 223
		void OnLevelWasInitialized(int level);

		/// <summary>
		/// Gets invoked on every graphic update.
		/// </summary>
		// Token: 0x060000E0 RID: 224
		void OnUpdate();

		/// <summary>
		/// Gets invoked on ever physics update.
		/// </summary>
		// Token: 0x060000E1 RID: 225
		void OnFixedUpdate();
	}
}
