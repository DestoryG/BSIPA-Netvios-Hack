using System;
using System.Collections.Generic;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x020000D4 RID: 212
	internal struct ScopedKnownTypes
	{
		// Token: 0x06000C11 RID: 3089 RVA: 0x000342AC File Offset: 0x000324AC
		internal void Push(Dictionary<XmlQualifiedName, DataContract> dataContractDictionary)
		{
			if (this.dataContractDictionaries == null)
			{
				this.dataContractDictionaries = new Dictionary<XmlQualifiedName, DataContract>[4];
			}
			else if (this.count == this.dataContractDictionaries.Length)
			{
				Array.Resize<Dictionary<XmlQualifiedName, DataContract>>(ref this.dataContractDictionaries, this.dataContractDictionaries.Length * 2);
			}
			Dictionary<XmlQualifiedName, DataContract>[] array = this.dataContractDictionaries;
			int num = this.count;
			this.count = num + 1;
			array[num] = dataContractDictionary;
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x0003430D File Offset: 0x0003250D
		internal void Pop()
		{
			this.count--;
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x00034320 File Offset: 0x00032520
		internal DataContract GetDataContract(XmlQualifiedName qname)
		{
			for (int i = this.count - 1; i >= 0; i--)
			{
				DataContract dataContract;
				if (this.dataContractDictionaries[i].TryGetValue(qname, out dataContract))
				{
					return dataContract;
				}
			}
			return null;
		}

		// Token: 0x040004EE RID: 1262
		internal Dictionary<XmlQualifiedName, DataContract>[] dataContractDictionaries;

		// Token: 0x040004EF RID: 1263
		private int count;
	}
}
