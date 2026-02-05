using System;

namespace System.ComponentModel
{
	// Token: 0x0200059E RID: 1438
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public sealed class ProvidePropertyAttribute : Attribute
	{
		// Token: 0x0600357F RID: 13695 RVA: 0x000E87DE File Offset: 0x000E69DE
		public ProvidePropertyAttribute(string propertyName, Type receiverType)
		{
			this.propertyName = propertyName;
			this.receiverTypeName = receiverType.AssemblyQualifiedName;
		}

		// Token: 0x06003580 RID: 13696 RVA: 0x000E87F9 File Offset: 0x000E69F9
		public ProvidePropertyAttribute(string propertyName, string receiverTypeName)
		{
			this.propertyName = propertyName;
			this.receiverTypeName = receiverTypeName;
		}

		// Token: 0x17000D13 RID: 3347
		// (get) Token: 0x06003581 RID: 13697 RVA: 0x000E880F File Offset: 0x000E6A0F
		public string PropertyName
		{
			get
			{
				return this.propertyName;
			}
		}

		// Token: 0x17000D14 RID: 3348
		// (get) Token: 0x06003582 RID: 13698 RVA: 0x000E8817 File Offset: 0x000E6A17
		public string ReceiverTypeName
		{
			get
			{
				return this.receiverTypeName;
			}
		}

		// Token: 0x17000D15 RID: 3349
		// (get) Token: 0x06003583 RID: 13699 RVA: 0x000E881F File Offset: 0x000E6A1F
		public override object TypeId
		{
			get
			{
				return base.GetType().FullName + this.propertyName;
			}
		}

		// Token: 0x06003584 RID: 13700 RVA: 0x000E8838 File Offset: 0x000E6A38
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			ProvidePropertyAttribute providePropertyAttribute = obj as ProvidePropertyAttribute;
			return providePropertyAttribute != null && providePropertyAttribute.propertyName == this.propertyName && providePropertyAttribute.receiverTypeName == this.receiverTypeName;
		}

		// Token: 0x06003585 RID: 13701 RVA: 0x000E887B File Offset: 0x000E6A7B
		public override int GetHashCode()
		{
			return this.propertyName.GetHashCode() ^ this.receiverTypeName.GetHashCode();
		}

		// Token: 0x04002A45 RID: 10821
		private readonly string propertyName;

		// Token: 0x04002A46 RID: 10822
		private readonly string receiverTypeName;
	}
}
