using System;
using System.CodeDom;
using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Runtime.Serialization
{
	// Token: 0x02000093 RID: 147
	internal static class DataContractSurrogateCaller
	{
		// Token: 0x06000A67 RID: 2663 RVA: 0x0002CA74 File Offset: 0x0002AC74
		internal static Type GetDataContractType(IDataContractSurrogate surrogate, Type type)
		{
			if (DataContract.GetBuiltInDataContract(type) != null)
			{
				return type;
			}
			Type dataContractType = surrogate.GetDataContractType(type);
			if (dataContractType == null)
			{
				return type;
			}
			return dataContractType;
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x0002CA9F File Offset: 0x0002AC9F
		internal static object GetObjectToSerialize(IDataContractSurrogate surrogate, object obj, Type objType, Type membertype)
		{
			if (obj == null)
			{
				return null;
			}
			if (DataContract.GetBuiltInDataContract(objType) != null)
			{
				return obj;
			}
			return surrogate.GetObjectToSerialize(obj, membertype);
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x0002CAB8 File Offset: 0x0002ACB8
		internal static object GetDeserializedObject(IDataContractSurrogate surrogate, object obj, Type objType, Type memberType)
		{
			if (obj == null)
			{
				return null;
			}
			if (DataContract.GetBuiltInDataContract(objType) != null)
			{
				return obj;
			}
			return surrogate.GetDeserializedObject(obj, memberType);
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x0002CAD1 File Offset: 0x0002ACD1
		internal static object GetCustomDataToExport(IDataContractSurrogate surrogate, MemberInfo memberInfo, Type dataContractType)
		{
			return surrogate.GetCustomDataToExport(memberInfo, dataContractType);
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x0002CADB File Offset: 0x0002ACDB
		internal static object GetCustomDataToExport(IDataContractSurrogate surrogate, Type clrType, Type dataContractType)
		{
			if (DataContract.GetBuiltInDataContract(clrType) != null)
			{
				return null;
			}
			return surrogate.GetCustomDataToExport(clrType, dataContractType);
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x0002CAEF File Offset: 0x0002ACEF
		internal static void GetKnownCustomDataTypes(IDataContractSurrogate surrogate, Collection<Type> customDataTypes)
		{
			surrogate.GetKnownCustomDataTypes(customDataTypes);
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x0002CAF8 File Offset: 0x0002ACF8
		internal static Type GetReferencedTypeOnImport(IDataContractSurrogate surrogate, string typeName, string typeNamespace, object customData)
		{
			if (DataContract.GetBuiltInDataContract(typeName, typeNamespace) != null)
			{
				return null;
			}
			return surrogate.GetReferencedTypeOnImport(typeName, typeNamespace, customData);
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x0002CB0E File Offset: 0x0002AD0E
		internal static CodeTypeDeclaration ProcessImportedType(IDataContractSurrogate surrogate, CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit)
		{
			return surrogate.ProcessImportedType(typeDeclaration, compileUnit);
		}
	}
}
