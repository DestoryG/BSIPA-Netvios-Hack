using System;

namespace System.ComponentModel
{
	// Token: 0x02000516 RID: 1302
	[AttributeUsage(AttributeTargets.Property)]
	public class AttributeProviderAttribute : Attribute
	{
		// Token: 0x06003148 RID: 12616 RVA: 0x000DF162 File Offset: 0x000DD362
		public AttributeProviderAttribute(string typeName)
		{
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			this._typeName = typeName;
		}

		// Token: 0x06003149 RID: 12617 RVA: 0x000DF17F File Offset: 0x000DD37F
		public AttributeProviderAttribute(string typeName, string propertyName)
		{
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			if (propertyName == null)
			{
				throw new ArgumentNullException("propertyName");
			}
			this._typeName = typeName;
			this._propertyName = propertyName;
		}

		// Token: 0x0600314A RID: 12618 RVA: 0x000DF1B1 File Offset: 0x000DD3B1
		public AttributeProviderAttribute(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this._typeName = type.AssemblyQualifiedName;
		}

		// Token: 0x17000C0E RID: 3086
		// (get) Token: 0x0600314B RID: 12619 RVA: 0x000DF1D9 File Offset: 0x000DD3D9
		public string TypeName
		{
			get
			{
				return this._typeName;
			}
		}

		// Token: 0x17000C0F RID: 3087
		// (get) Token: 0x0600314C RID: 12620 RVA: 0x000DF1E1 File Offset: 0x000DD3E1
		public string PropertyName
		{
			get
			{
				return this._propertyName;
			}
		}

		// Token: 0x0400290B RID: 10507
		private string _typeName;

		// Token: 0x0400290C RID: 10508
		private string _propertyName;
	}
}
