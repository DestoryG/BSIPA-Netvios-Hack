using System;

namespace IPA.Config.Stores.Attributes
{
	/// <summary>
	/// Causes a field or property in an object being wrapped by <see cref="M:IPA.Config.Stores.GeneratedStore.Generated``1(IPA.Config.Config,System.Boolean)" /> to be
	/// ignored during serialization and deserialization.
	/// </summary>
	// Token: 0x02000090 RID: 144
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public sealed class IgnoreAttribute : Attribute
	{
	}
}
