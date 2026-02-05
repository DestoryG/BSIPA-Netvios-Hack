using System;

namespace IPA
{
	/// <summary>
	/// Indicates that the target method should be called when the plugin is disabled.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This attribute is interchangable with <see cref="T:IPA.OnExitAttribute" />, and is treated identically.
	/// They are seperate to allow plugin code to more clearly describe the intent of the methods.
	/// </para>
	/// <para>
	/// Typically, this will be used when the <see cref="T:IPA.RuntimeOptions" /> parameter of the plugins's
	/// <see cref="T:IPA.PluginAttribute" /> is <see cref="F:IPA.RuntimeOptions.DynamicInit" />.
	/// </para>
	/// <para>
	/// The method marked by this attribute will always be called from the Unity main thread.
	/// </para>
	/// </remarks>
	/// <seealso cref="T:IPA.PluginAttribute" />
	/// <seealso cref="T:IPA.OnExitAttribute" />
	// Token: 0x02000008 RID: 8
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public sealed class OnDisableAttribute : Attribute, IEdgeLifecycleAttribute
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00002076 File Offset: 0x00000276
		EdgeLifecycleType IEdgeLifecycleAttribute.Type
		{
			get
			{
				return EdgeLifecycleType.Disable;
			}
		}
	}
}
