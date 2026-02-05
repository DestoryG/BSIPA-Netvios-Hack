using System;

namespace System.ComponentModel
{
	// Token: 0x02000554 RID: 1364
	[AttributeUsage(AttributeTargets.All)]
	public sealed class ExtenderProvidedPropertyAttribute : Attribute
	{
		// Token: 0x06003345 RID: 13125 RVA: 0x000E3A18 File Offset: 0x000E1C18
		internal static ExtenderProvidedPropertyAttribute Create(PropertyDescriptor extenderProperty, Type receiverType, IExtenderProvider provider)
		{
			return new ExtenderProvidedPropertyAttribute
			{
				extenderProperty = extenderProperty,
				receiverType = receiverType,
				provider = provider
			};
		}

		// Token: 0x17000C88 RID: 3208
		// (get) Token: 0x06003347 RID: 13127 RVA: 0x000E3A49 File Offset: 0x000E1C49
		public PropertyDescriptor ExtenderProperty
		{
			get
			{
				return this.extenderProperty;
			}
		}

		// Token: 0x17000C89 RID: 3209
		// (get) Token: 0x06003348 RID: 13128 RVA: 0x000E3A51 File Offset: 0x000E1C51
		public IExtenderProvider Provider
		{
			get
			{
				return this.provider;
			}
		}

		// Token: 0x17000C8A RID: 3210
		// (get) Token: 0x06003349 RID: 13129 RVA: 0x000E3A59 File Offset: 0x000E1C59
		public Type ReceiverType
		{
			get
			{
				return this.receiverType;
			}
		}

		// Token: 0x0600334A RID: 13130 RVA: 0x000E3A64 File Offset: 0x000E1C64
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			ExtenderProvidedPropertyAttribute extenderProvidedPropertyAttribute = obj as ExtenderProvidedPropertyAttribute;
			return extenderProvidedPropertyAttribute != null && extenderProvidedPropertyAttribute.extenderProperty.Equals(this.extenderProperty) && extenderProvidedPropertyAttribute.provider.Equals(this.provider) && extenderProvidedPropertyAttribute.receiverType.Equals(this.receiverType);
		}

		// Token: 0x0600334B RID: 13131 RVA: 0x000E3ABA File Offset: 0x000E1CBA
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0600334C RID: 13132 RVA: 0x000E3AC2 File Offset: 0x000E1CC2
		public override bool IsDefaultAttribute()
		{
			return this.receiverType == null;
		}

		// Token: 0x040029AE RID: 10670
		private PropertyDescriptor extenderProperty;

		// Token: 0x040029AF RID: 10671
		private IExtenderProvider provider;

		// Token: 0x040029B0 RID: 10672
		private Type receiverType;
	}
}
