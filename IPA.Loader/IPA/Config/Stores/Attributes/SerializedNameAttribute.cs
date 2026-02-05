using System;

namespace IPA.Config.Stores.Attributes
{
	/// <summary>
	/// Specifies a name for the serialized field or property in an object being wrapped by
	/// <see cref="M:IPA.Config.Stores.GeneratedStore.Generated``1(IPA.Config.Config,System.Boolean)" /> that is different from the member name itself.
	/// </summary>
	/// <example>
	/// <para>
	/// When serializing the following object, we might get the JSON that follows.
	/// <code>
	/// public class PluginConfig
	/// {
	///     public virtual bool BooleanField { get; set; } = true;
	/// }
	/// </code>
	/// <code>
	/// {
	///     "BooleanField": true
	/// }
	/// </code>
	/// </para>
	/// <para>
	/// However, if we were to add a <see cref="T:IPA.Config.Stores.Attributes.SerializedNameAttribute" /> to that field, we would get the following.
	/// <code>
	/// public class PluginConfig
	/// {
	///     [SerializedName("bool")]
	///     public virtual bool BooleanField { get; set; } = true;
	/// }
	/// </code>
	/// <code>
	/// {
	///     "bool": true
	/// }
	/// </code>
	/// </para>
	/// </example>
	// Token: 0x02000093 RID: 147
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public sealed class SerializedNameAttribute : Attribute
	{
		/// <summary>
		/// Gets the name to replace the member name with.
		/// </summary>
		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600039F RID: 927 RVA: 0x0001322C File Offset: 0x0001142C
		// (set) Token: 0x060003A0 RID: 928 RVA: 0x00013234 File Offset: 0x00011434
		public string Name { get; private set; }

		/// <summary>
		/// Creates a new <see cref="T:IPA.Config.Stores.Attributes.SerializedNameAttribute" /> with the given <see cref="P:IPA.Config.Stores.Attributes.SerializedNameAttribute.Name" />.
		/// </summary>
		/// <param name="name">the value to assign to <see cref="P:IPA.Config.Stores.Attributes.SerializedNameAttribute.Name" /></param>
		// Token: 0x060003A1 RID: 929 RVA: 0x0001323D File Offset: 0x0001143D
		public SerializedNameAttribute(string name)
		{
			this.Name = name;
		}
	}
}
