using System;

namespace IPA
{
	/// <summary>
	/// Marks a method or a constructor as an inialization method.
	/// </summary>
	/// <remarks>
	/// <para>
	/// If more than one constructor is marked with this attribute, the one with the most parameters, whether or not they can be injected, will be used.
	/// </para>
	/// <para>
	/// Parameter injection is done with <see cref="T:IPA.Loader.PluginInitInjector" />.
	/// </para>
	/// </remarks>
	/// <seealso cref="T:IPA.PluginAttribute" />
	/// <seealso cref="T:IPA.Loader.PluginInitInjector" />
	// Token: 0x0200000C RID: 12
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public sealed class InitAttribute : Attribute
	{
	}
}
