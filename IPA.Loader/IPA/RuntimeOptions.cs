using System;

namespace IPA
{
	/// <summary>
	/// Options that a plugin must specify to describe how it expects to be run.
	/// </summary>
	/// <seealso cref="T:IPA.PluginAttribute" />
	/// <seealso cref="T:IPA.InitAttribute" />
	/// <seealso cref="T:IPA.OnEnableAttribute" />
	/// <seealso cref="T:IPA.OnDisableAttribute" />
	/// <seealso cref="T:IPA.OnStartAttribute" />
	/// <seealso cref="T:IPA.OnExitAttribute" />
	// Token: 0x0200000B RID: 11
	public enum RuntimeOptions
	{
		/// <summary>
		/// <para>
		/// Indicates that this plugin expects to be initialized and enabled with the game, and disabled with the game.
		/// </para>
		/// <para>
		/// With this option set, whether or not the plugin is disabled during a given run is constant for that entire run.
		/// </para>
		/// </summary>
		// Token: 0x04000006 RID: 6
		SingleStartInit,
		/// <summary>
		/// <para>
		/// Indicates that this plugin supports runtime enabling and disabling.
		/// </para>
		/// <para>
		/// When this is set, the plugin may be disabled at reasonable points during runtime. As with <see cref="F:IPA.RuntimeOptions.SingleStartInit" />,
		/// it will be initialized and enabled with the game if it is enabled on startup, and disabled with the game if it is enabled
		/// on shutdown.
		/// </para>
		/// <para>
		/// When a plugin with this set is enabled mid-game, the first time it is enabled, its initialization methods will be called,
		/// then its enable methods. All subsequent enables will <b>NOT</b> re-initialize, however the enable methods will be called.
		/// </para>
		/// <para>
		/// When a plugin with this set is disabled mid-game, the plugin instance will <b>NOT</b> be destroyed, and will instead be
		/// re-used for subsequent enables. The plugin is expected to handle this gracefully, and behave in a way that makes sense.
		/// </para>
		/// </summary>
		// Token: 0x04000007 RID: 7
		DynamicInit
	}
}
