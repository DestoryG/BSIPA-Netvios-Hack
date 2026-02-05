using System;

namespace IPA
{
	/// <summary>
	/// Indicates that the target method should be called when the plugin is enabled.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This attribute is interchangable with <see cref="T:IPA.OnStartAttribute" />, and is treated identically.
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
	/// <seealso cref="T:IPA.OnStartAttribute" />
	// Token: 0x02000006 RID: 6
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public sealed class OnEnableAttribute : Attribute, IEdgeLifecycleAttribute
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000004 RID: 4 RVA: 0x00002060 File Offset: 0x00000260
		EdgeLifecycleType IEdgeLifecycleAttribute.Type
		{
			get
			{
				return EdgeLifecycleType.Enable;
			}
		}
	}
}
