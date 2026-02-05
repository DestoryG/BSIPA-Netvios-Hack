using System;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x0200009A RID: 154
	internal sealed class KnownTypeDataContractResolver : DataContractResolver
	{
		// Token: 0x06000AAF RID: 2735 RVA: 0x0002D439 File Offset: 0x0002B639
		internal KnownTypeDataContractResolver(XmlObjectSerializerContext context)
		{
			this.context = context;
		}

		// Token: 0x06000AB0 RID: 2736 RVA: 0x0002D448 File Offset: 0x0002B648
		public override bool TryResolveType(Type type, Type declaredType, DataContractResolver knownTypeResolver, out XmlDictionaryString typeName, out XmlDictionaryString typeNamespace)
		{
			if (type == null)
			{
				typeName = null;
				typeNamespace = null;
				return false;
			}
			if (declaredType != null && declaredType.IsInterface && CollectionDataContract.IsCollectionInterface(declaredType))
			{
				typeName = null;
				typeNamespace = null;
				return true;
			}
			DataContract dataContract = DataContract.GetDataContract(type);
			if (this.context.IsKnownType(dataContract, dataContract.KnownDataContracts, declaredType))
			{
				typeName = dataContract.Name;
				typeNamespace = dataContract.Namespace;
				return true;
			}
			typeName = null;
			typeNamespace = null;
			return false;
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x0002D4C4 File Offset: 0x0002B6C4
		public override Type ResolveName(string typeName, string typeNamespace, Type declaredType, DataContractResolver knownTypeResolver)
		{
			if (typeName == null || typeNamespace == null)
			{
				return null;
			}
			return this.context.ResolveNameFromKnownTypes(new XmlQualifiedName(typeName, typeNamespace));
		}

		// Token: 0x040004BC RID: 1212
		private XmlObjectSerializerContext context;
	}
}
