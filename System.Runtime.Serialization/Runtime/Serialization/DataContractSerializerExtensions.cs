using System;
using System.CodeDom;
using System.Collections.ObjectModel;
using System.Reflection;

namespace System.Runtime.Serialization
{
	// Token: 0x020000F4 RID: 244
	public static class DataContractSerializerExtensions
	{
		// Token: 0x06000F32 RID: 3890 RVA: 0x0003E18C File Offset: 0x0003C38C
		public static ISerializationSurrogateProvider GetSerializationSurrogateProvider(this DataContractSerializer serializer)
		{
			DataContractSerializerExtensions.SurrogateProviderAdapter surrogateProviderAdapter = serializer.DataContractSurrogate as DataContractSerializerExtensions.SurrogateProviderAdapter;
			if (surrogateProviderAdapter != null)
			{
				return surrogateProviderAdapter.Provider;
			}
			return null;
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x0003E1B0 File Offset: 0x0003C3B0
		public static void SetSerializationSurrogateProvider(this DataContractSerializer serializer, ISerializationSurrogateProvider provider)
		{
			IDataContractSurrogate dataContractSurrogate = new DataContractSerializerExtensions.SurrogateProviderAdapter(provider);
			typeof(DataContractSerializer).GetField("dataContractSurrogate", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(serializer, dataContractSurrogate);
		}

		// Token: 0x02000181 RID: 385
		private class SurrogateProviderAdapter : IDataContractSurrogate
		{
			// Token: 0x060014F5 RID: 5365 RVA: 0x00054A6A File Offset: 0x00052C6A
			public SurrogateProviderAdapter(ISerializationSurrogateProvider provider)
			{
				this._provider = provider;
			}

			// Token: 0x17000461 RID: 1121
			// (get) Token: 0x060014F6 RID: 5366 RVA: 0x00054A79 File Offset: 0x00052C79
			public ISerializationSurrogateProvider Provider
			{
				get
				{
					return this._provider;
				}
			}

			// Token: 0x060014F7 RID: 5367 RVA: 0x00054A81 File Offset: 0x00052C81
			public object GetCustomDataToExport(Type clrType, Type dataContractType)
			{
				throw NotImplemented.ByDesign;
			}

			// Token: 0x060014F8 RID: 5368 RVA: 0x00054A88 File Offset: 0x00052C88
			public object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType)
			{
				throw NotImplemented.ByDesign;
			}

			// Token: 0x060014F9 RID: 5369 RVA: 0x00054A8F File Offset: 0x00052C8F
			public Type GetDataContractType(Type type)
			{
				return this._provider.GetSurrogateType(type);
			}

			// Token: 0x060014FA RID: 5370 RVA: 0x00054A9D File Offset: 0x00052C9D
			public object GetDeserializedObject(object obj, Type targetType)
			{
				return this._provider.GetDeserializedObject(obj, targetType);
			}

			// Token: 0x060014FB RID: 5371 RVA: 0x00054AAC File Offset: 0x00052CAC
			public void GetKnownCustomDataTypes(Collection<Type> customDataTypes)
			{
				throw NotImplemented.ByDesign;
			}

			// Token: 0x060014FC RID: 5372 RVA: 0x00054AB3 File Offset: 0x00052CB3
			public object GetObjectToSerialize(object obj, Type targetType)
			{
				return this._provider.GetObjectToSerialize(obj, targetType);
			}

			// Token: 0x060014FD RID: 5373 RVA: 0x00054AC2 File Offset: 0x00052CC2
			public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
			{
				throw NotImplemented.ByDesign;
			}

			// Token: 0x060014FE RID: 5374 RVA: 0x00054AC9 File Offset: 0x00052CC9
			public CodeTypeDeclaration ProcessImportedType(CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit)
			{
				throw NotImplemented.ByDesign;
			}

			// Token: 0x04000A3D RID: 2621
			private ISerializationSurrogateProvider _provider;
		}
	}
}
