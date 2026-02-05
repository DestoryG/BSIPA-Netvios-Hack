using System;

namespace IPA.Config.Stores.Attributes
{
	/// <summary>
	/// Indicates that a field or property in an object being wrapped by <see cref="M:IPA.Config.Stores.GeneratedStore.Generated``1(IPA.Config.Config,System.Boolean)" />
	/// that would otherwise be nullable (i.e. a reference type or a <see cref="T:System.Nullable`1" /> type) should never be null, and the
	/// member will be ignored if the deserialized value is <see langword="null" />.
	/// </summary>
	// Token: 0x02000091 RID: 145
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public sealed class NonNullableAttribute : Attribute
	{
	}
}
