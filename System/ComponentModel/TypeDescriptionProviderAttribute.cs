using System;

namespace System.ComponentModel
{
	// Token: 0x020005B5 RID: 1461
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public sealed class TypeDescriptionProviderAttribute : Attribute
	{
		// Token: 0x06003674 RID: 13940 RVA: 0x000ECD4B File Offset: 0x000EAF4B
		public TypeDescriptionProviderAttribute(string typeName)
		{
			if (typeName == null)
			{
				throw new ArgumentNullException("typeName");
			}
			this._typeName = typeName;
		}

		// Token: 0x06003675 RID: 13941 RVA: 0x000ECD68 File Offset: 0x000EAF68
		public TypeDescriptionProviderAttribute(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this._typeName = type.AssemblyQualifiedName;
		}

		// Token: 0x17000D38 RID: 3384
		// (get) Token: 0x06003676 RID: 13942 RVA: 0x000ECD90 File Offset: 0x000EAF90
		public string TypeName
		{
			get
			{
				return this._typeName;
			}
		}

		// Token: 0x04002A9F RID: 10911
		private string _typeName;
	}
}
