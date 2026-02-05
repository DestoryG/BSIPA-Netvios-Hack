using System;
using System.CodeDom;
using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Runtime.Serialization
{
	// Token: 0x02000092 RID: 146
	public interface IDataContractSurrogate
	{
		// Token: 0x06000A5F RID: 2655
		Type GetDataContractType(Type type);

		// Token: 0x06000A60 RID: 2656
		object GetObjectToSerialize(object obj, Type targetType);

		// Token: 0x06000A61 RID: 2657
		object GetDeserializedObject(object obj, Type targetType);

		// Token: 0x06000A62 RID: 2658
		object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType);

		// Token: 0x06000A63 RID: 2659
		object GetCustomDataToExport(Type clrType, Type dataContractType);

		// Token: 0x06000A64 RID: 2660
		void GetKnownCustomDataTypes(Collection<Type> customDataTypes);

		// Token: 0x06000A65 RID: 2661
		Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData);

		// Token: 0x06000A66 RID: 2662
		CodeTypeDeclaration ProcessImportedType(CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit);
	}
}
