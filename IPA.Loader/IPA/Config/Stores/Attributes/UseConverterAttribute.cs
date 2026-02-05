using System;
using System.Linq;

namespace IPA.Config.Stores.Attributes
{
	/// <summary>
	/// Indicates that a given field or property in an object being wrapped by <see cref="M:IPA.Config.Stores.GeneratedStore.Generated``1(IPA.Config.Config,System.Boolean)" />
	/// should be serialized and deserialized using the provided converter instead of the default mechanism.
	/// </summary>
	// Token: 0x02000092 RID: 146
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public sealed class UseConverterAttribute : Attribute
	{
		/// <summary>
		/// Gets whether or not to use the default converter for the member type instead of the specified type.
		/// </summary>
		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000399 RID: 921 RVA: 0x00013127 File Offset: 0x00011327
		public bool UseDefaultConverterForType { get; }

		/// <summary>
		/// Gets the type of the converter to use.
		/// </summary>
		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600039A RID: 922 RVA: 0x0001312F File Offset: 0x0001132F
		public Type ConverterType { get; }

		/// <summary>
		/// Gets the target type of the converter if it is avaliable at instantiation time, otherwise
		/// <see langword="null" />.
		/// </summary>
		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600039B RID: 923 RVA: 0x00013137 File Offset: 0x00011337
		public Type ConverterTargetType { get; }

		/// <summary>
		/// Gets whether or not this converter is a generic <see cref="T:IPA.Config.Stores.ValueConverter`1" />.
		/// </summary>
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600039C RID: 924 RVA: 0x0001313F File Offset: 0x0001133F
		public bool IsGenericConverter
		{
			get
			{
				return this.ConverterTargetType != null;
			}
		}

		/// <summary>
		/// Creates a new <see cref="T:IPA.Config.Stores.Attributes.UseConverterAttribute" /> specifying to use the default converter type for the target member.
		/// </summary>
		// Token: 0x0600039D RID: 925 RVA: 0x0001314D File Offset: 0x0001134D
		public UseConverterAttribute()
		{
			this.UseDefaultConverterForType = true;
		}

		/// <summary>
		/// Creates a new <see cref="T:IPA.Config.Stores.Attributes.UseConverterAttribute" /> with a  given <see cref="P:IPA.Config.Stores.Attributes.UseConverterAttribute.ConverterType" />.
		/// </summary>
		/// <param name="converterType">the type to assign to <see cref="P:IPA.Config.Stores.Attributes.UseConverterAttribute.ConverterType" /></param>
		// Token: 0x0600039E RID: 926 RVA: 0x0001315C File Offset: 0x0001135C
		public UseConverterAttribute(Type converterType)
		{
			this.UseDefaultConverterForType = false;
			this.ConverterType = converterType;
			Type baseT = this.ConverterType.BaseType;
			while (baseT != null && baseT != typeof(object) && (!baseT.IsGenericType || baseT.GetGenericTypeDefinition() != typeof(ValueConverter<>)))
			{
				baseT = baseT.BaseType;
			}
			if (baseT == typeof(object))
			{
				this.ConverterTargetType = null;
			}
			else
			{
				this.ConverterTargetType = baseT.GetGenericArguments()[0];
			}
			bool implInterface = this.ConverterType.GetInterfaces().Contains(typeof(IValueConverter));
			if (this.ConverterTargetType == null && !implInterface)
			{
				throw new ArgumentException("Type is not a value converter!");
			}
		}
	}
}
