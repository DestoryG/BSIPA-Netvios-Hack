using System;

namespace IPA
{
	/// <summary>
	/// Indicates that the target method should be called when the game starts.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This attribute is interchangable with <see cref="T:IPA.OnEnableAttribute" />, and is treated identically.
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
	/// <seealso cref="T:IPA.OnEnableAttribute" />
	// Token: 0x02000007 RID: 7
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public sealed class OnStartAttribute : Attribute, IEdgeLifecycleAttribute
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000006 RID: 6 RVA: 0x0000206B File Offset: 0x0000026B
		EdgeLifecycleType IEdgeLifecycleAttribute.Type
		{
			get
			{
				return EdgeLifecycleType.Enable;
			}
		}
	}
}
