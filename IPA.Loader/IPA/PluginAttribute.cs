using System;

namespace IPA
{
	/// <summary>
	/// Marks a class as being a BSIPA plugin.
	/// </summary>
	/// <seealso cref="T:IPA.InitAttribute" />
	/// <seealso cref="T:IPA.OnEnableAttribute" />
	/// <seealso cref="T:IPA.OnDisableAttribute" />
	/// <seealso cref="T:IPA.OnStartAttribute" />
	/// <seealso cref="T:IPA.OnExitAttribute" />
	// Token: 0x0200000A RID: 10
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class PluginAttribute : Attribute
	{
		/// <summary>
		/// The <see cref="T:IPA.RuntimeOptions" /> passed into the constructor of this attribute.
		/// </summary>
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000C RID: 12 RVA: 0x0000208C File Offset: 0x0000028C
		public RuntimeOptions RuntimeOptions { get; }

		/// <summary>
		/// Initializes a <see cref="T:IPA.PluginAttribute" /> with the given <see cref="T:IPA.RuntimeOptions" />
		/// to indicate the runtime capabilities of the plugin.
		/// </summary>
		/// <param name="runtimeOptions">the options to use for this plugin</param>
		// Token: 0x0600000D RID: 13 RVA: 0x00002094 File Offset: 0x00000294
		public PluginAttribute(RuntimeOptions runtimeOptions)
		{
			this.RuntimeOptions = runtimeOptions;
		}
	}
}
