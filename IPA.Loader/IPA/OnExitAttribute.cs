using System;

namespace IPA
{
	/// <summary>
	/// Indicates that the target method should be called when the game exits.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This attribute is interchangable with <see cref="T:IPA.OnDisableAttribute" />, and is treated identically.
	/// They are seperate to allow plugin code to more clearly describe the intent of the methods.
	/// </para>
	/// <para>
	/// Typically, this will be used when the <see cref="T:IPA.RuntimeOptions" /> parameter of the plugins's
	/// <see cref="T:IPA.PluginAttribute" /> is <see cref="F:IPA.RuntimeOptions.SingleStartInit" />.
	/// </para>
	/// <para>
	/// The method marked by this attribute will always be called from the Unity main thread.
	/// </para>
	/// </remarks>
	/// <seealso cref="T:IPA.PluginAttribute" />
	/// <seealso cref="T:IPA.OnDisableAttribute" />
	// Token: 0x02000009 RID: 9
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public sealed class OnExitAttribute : Attribute, IEdgeLifecycleAttribute
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000A RID: 10 RVA: 0x00002081 File Offset: 0x00000281
		EdgeLifecycleType IEdgeLifecycleAttribute.Type
		{
			get
			{
				return EdgeLifecycleType.Disable;
			}
		}
	}
}
