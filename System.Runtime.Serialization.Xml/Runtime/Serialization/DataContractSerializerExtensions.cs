using System;
using System.CodeDom;
using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Runtime.Serialization
{
	// Token: 0x02000003 RID: 3
	public static class DataContractSerializerExtensions
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002060 File Offset: 0x00000260
		public static ISerializationSurrogateProvider GetSerializationSurrogateProvider(this DataContractSerializer serializer)
		{
			DataContractSerializerExtensions.SurrogateProviderAdapter surrogateProviderAdapter = serializer.DataContractSurrogate as DataContractSerializerExtensions.SurrogateProviderAdapter;
			if (surrogateProviderAdapter != null)
			{
				return surrogateProviderAdapter.Provider;
			}
			return null;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002084 File Offset: 0x00000284
		public static void SetSerializationSurrogateProvider(this DataContractSerializer serializer, ISerializationSurrogateProvider provider)
		{
			IDataContractSurrogate dataContractSurrogate = new DataContractSerializerExtensions.SurrogateProviderAdapter(provider);
			typeof(DataContractSerializer).GetField("dataContractSurrogate", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(serializer, dataContractSurrogate);
		}

		// Token: 0x02000004 RID: 4
		private class SurrogateProviderAdapter : IDataContractSurrogate
		{
			// Token: 0x06000005 RID: 5 RVA: 0x000020B5 File Offset: 0x000002B5
			public SurrogateProviderAdapter(ISerializationSurrogateProvider provider)
			{
				this._provider = provider;
			}

			// Token: 0x17000002 RID: 2
			// (get) Token: 0x06000006 RID: 6 RVA: 0x000020C4 File Offset: 0x000002C4
			public ISerializationSurrogateProvider Provider
			{
				get
				{
					return this._provider;
				}
			}

			// Token: 0x06000007 RID: 7 RVA: 0x000020CC File Offset: 0x000002CC
			public object GetCustomDataToExport(Type clrType, Type dataContractType)
			{
				throw NotImplemented.ByDesign;
			}

			// Token: 0x06000008 RID: 8 RVA: 0x000020D3 File Offset: 0x000002D3
			public object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType)
			{
				throw NotImplemented.ByDesign;
			}

			// Token: 0x06000009 RID: 9 RVA: 0x000020DA File Offset: 0x000002DA
			public Type GetDataContractType(Type type)
			{
				return this._provider.GetSurrogateType(type);
			}

			// Token: 0x0600000A RID: 10 RVA: 0x000020E8 File Offset: 0x000002E8
			public object GetDeserializedObject(object obj, Type targetType)
			{
				return this._provider.GetDeserializedObject(obj, targetType);
			}

			// Token: 0x0600000B RID: 11 RVA: 0x000020F7 File Offset: 0x000002F7
			public void GetKnownCustomDataTypes(Collection<Type> customDataTypes)
			{
				throw NotImplemented.ByDesign;
			}

			// Token: 0x0600000C RID: 12 RVA: 0x000020FE File Offset: 0x000002FE
			public object GetObjectToSerialize(object obj, Type targetType)
			{
				return this._provider.GetObjectToSerialize(obj, targetType);
			}

			// Token: 0x0600000D RID: 13 RVA: 0x0000210D File Offset: 0x0000030D
			public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
			{
				throw NotImplemented.ByDesign;
			}

			// Token: 0x0600000E RID: 14 RVA: 0x00002114 File Offset: 0x00000314
			public CodeTypeDeclaration ProcessImportedType(CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit)
			{
				throw NotImplemented.ByDesign;
			}

			// Token: 0x04000001 RID: 1
			private ISerializationSurrogateProvider _provider;
		}
	}
}
