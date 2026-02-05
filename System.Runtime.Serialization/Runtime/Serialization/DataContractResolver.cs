using System;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x02000076 RID: 118
	public abstract class DataContractResolver
	{
		// Token: 0x060008A2 RID: 2210
		public abstract bool TryResolveType(Type type, Type declaredType, DataContractResolver knownTypeResolver, out XmlDictionaryString typeName, out XmlDictionaryString typeNamespace);

		// Token: 0x060008A3 RID: 2211
		public abstract Type ResolveName(string typeName, string typeNamespace, Type declaredType, DataContractResolver knownTypeResolver);
	}
}
