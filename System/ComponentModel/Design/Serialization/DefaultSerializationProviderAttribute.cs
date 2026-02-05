using System;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x02000605 RID: 1541
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public sealed class DefaultSerializationProviderAttribute : Attribute
	{
		// Token: 0x0600389C RID: 14492 RVA: 0x000F19E9 File Offset: 0x000EFBE9
		public DefaultSerializationProviderAttribute(Type providerType)
		{
			if (providerType == null)
			{
				throw new ArgumentNullException("providerType");
			}
			this._providerTypeName = providerType.AssemblyQualifiedName;
		}

		// Token: 0x0600389D RID: 14493 RVA: 0x000F1A11 File Offset: 0x000EFC11
		public DefaultSerializationProviderAttribute(string providerTypeName)
		{
			if (providerTypeName == null)
			{
				throw new ArgumentNullException("providerTypeName");
			}
			this._providerTypeName = providerTypeName;
		}

		// Token: 0x17000D89 RID: 3465
		// (get) Token: 0x0600389E RID: 14494 RVA: 0x000F1A2E File Offset: 0x000EFC2E
		public string ProviderTypeName
		{
			get
			{
				return this._providerTypeName;
			}
		}

		// Token: 0x04002B69 RID: 11113
		private string _providerTypeName;
	}
}
