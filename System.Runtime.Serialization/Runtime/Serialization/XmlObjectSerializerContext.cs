using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization.Diagnostics.Application;
using System.Security;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x020000E6 RID: 230
	internal class XmlObjectSerializerContext
	{
		// Token: 0x06000CEB RID: 3307 RVA: 0x00036674 File Offset: 0x00034874
		internal XmlObjectSerializerContext(XmlObjectSerializer serializer, int maxItemsInObjectGraph, StreamingContext streamingContext, bool ignoreExtensionDataObject, DataContractResolver dataContractResolver)
		{
			this.serializer = serializer;
			this.itemCount = 1;
			this.maxItemsInObjectGraph = maxItemsInObjectGraph;
			this.streamingContext = streamingContext;
			this.ignoreExtensionDataObject = ignoreExtensionDataObject;
			this.dataContractResolver = dataContractResolver;
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x000366A8 File Offset: 0x000348A8
		internal XmlObjectSerializerContext(XmlObjectSerializer serializer, int maxItemsInObjectGraph, StreamingContext streamingContext, bool ignoreExtensionDataObject)
			: this(serializer, maxItemsInObjectGraph, streamingContext, ignoreExtensionDataObject, null)
		{
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x000366B6 File Offset: 0x000348B6
		internal XmlObjectSerializerContext(DataContractSerializer serializer, DataContract rootTypeDataContract, DataContractResolver dataContractResolver)
			: this(serializer, serializer.MaxItemsInObjectGraph, new StreamingContext(StreamingContextStates.All), serializer.IgnoreExtensionDataObject, dataContractResolver)
		{
			this.rootTypeDataContract = rootTypeDataContract;
			this.serializerKnownTypeList = serializer.knownTypeList;
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x000366E9 File Offset: 0x000348E9
		internal XmlObjectSerializerContext(NetDataContractSerializer serializer)
			: this(serializer, serializer.MaxItemsInObjectGraph, serializer.Context, serializer.IgnoreExtensionDataObject)
		{
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000CEF RID: 3311 RVA: 0x00036704 File Offset: 0x00034904
		internal virtual SerializationMode Mode
		{
			get
			{
				return SerializationMode.SharedContract;
			}
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000CF0 RID: 3312 RVA: 0x00036707 File Offset: 0x00034907
		// (set) Token: 0x06000CF1 RID: 3313 RVA: 0x0003670A File Offset: 0x0003490A
		internal virtual bool IsGetOnlyCollection
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x0003670C File Offset: 0x0003490C
		[SecuritySafeCritical]
		public void DemandSerializationFormatterPermission()
		{
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x0003670E File Offset: 0x0003490E
		[SecuritySafeCritical]
		public void DemandMemberAccessPermission()
		{
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x00036710 File Offset: 0x00034910
		public StreamingContext GetStreamingContext()
		{
			return this.streamingContext;
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000CF5 RID: 3317 RVA: 0x00036718 File Offset: 0x00034918
		internal static MethodInfo IncrementItemCountMethod
		{
			get
			{
				if (XmlObjectSerializerContext.incrementItemCountMethod == null)
				{
					XmlObjectSerializerContext.incrementItemCountMethod = typeof(XmlObjectSerializerContext).GetMethod("IncrementItemCount", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlObjectSerializerContext.incrementItemCountMethod;
			}
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x00036748 File Offset: 0x00034948
		public void IncrementItemCount(int count)
		{
			if (count > this.maxItemsInObjectGraph - this.itemCount)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Maximum number of items that can be serialized or deserialized in an object graph is '{0}'.", new object[] { this.maxItemsInObjectGraph })));
			}
			this.itemCount += count;
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000CF7 RID: 3319 RVA: 0x0003679C File Offset: 0x0003499C
		internal int RemainingItemCount
		{
			get
			{
				return this.maxItemsInObjectGraph - this.itemCount;
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000CF8 RID: 3320 RVA: 0x000367AB File Offset: 0x000349AB
		internal bool IgnoreExtensionDataObject
		{
			get
			{
				return this.ignoreExtensionDataObject;
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000CF9 RID: 3321 RVA: 0x000367B3 File Offset: 0x000349B3
		protected DataContractResolver DataContractResolver
		{
			get
			{
				return this.dataContractResolver;
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000CFA RID: 3322 RVA: 0x000367BB File Offset: 0x000349BB
		protected KnownTypeDataContractResolver KnownTypeResolver
		{
			get
			{
				if (this.knownTypeResolver == null)
				{
					this.knownTypeResolver = new KnownTypeDataContractResolver(this);
				}
				return this.knownTypeResolver;
			}
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x000367D7 File Offset: 0x000349D7
		internal DataContract GetDataContract(Type type)
		{
			return this.GetDataContract(type.TypeHandle, type);
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x000367E6 File Offset: 0x000349E6
		internal virtual DataContract GetDataContract(RuntimeTypeHandle typeHandle, Type type)
		{
			if (this.IsGetOnlyCollection)
			{
				return DataContract.GetGetOnlyCollectionDataContract(DataContract.GetId(typeHandle), typeHandle, type, this.Mode);
			}
			return DataContract.GetDataContract(typeHandle, type, this.Mode);
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x00036811 File Offset: 0x00034A11
		internal virtual DataContract GetDataContractSkipValidation(int typeId, RuntimeTypeHandle typeHandle, Type type)
		{
			if (this.IsGetOnlyCollection)
			{
				return DataContract.GetGetOnlyCollectionDataContractSkipValidation(typeId, typeHandle, type);
			}
			return DataContract.GetDataContractSkipValidation(typeId, typeHandle, type);
		}

		// Token: 0x06000CFE RID: 3326 RVA: 0x0003682C File Offset: 0x00034A2C
		internal virtual DataContract GetDataContract(int id, RuntimeTypeHandle typeHandle)
		{
			if (this.IsGetOnlyCollection)
			{
				return DataContract.GetGetOnlyCollectionDataContract(id, typeHandle, null, this.Mode);
			}
			return DataContract.GetDataContract(id, typeHandle, this.Mode);
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x00036852 File Offset: 0x00034A52
		internal virtual void CheckIfTypeSerializable(Type memberType, bool isMemberTypeSerializable)
		{
			if (!isMemberTypeSerializable)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Type '{0}' cannot be serialized. Consider marking it with the DataContractAttribute attribute, and marking all of its members you want serialized with the DataMemberAttribute attribute. Alternatively, you can ensure that the type is public and has a parameterless constructor - all public members of the type will then be serialized, and no attributes will be required.", new object[] { memberType })));
			}
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x00036876 File Offset: 0x00034A76
		internal virtual Type GetSurrogatedType(Type type)
		{
			return type;
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000D01 RID: 3329 RVA: 0x00036879 File Offset: 0x00034A79
		private Dictionary<XmlQualifiedName, DataContract> SerializerKnownDataContracts
		{
			get
			{
				if (!this.isSerializerKnownDataContractsSetExplicit)
				{
					this.serializerKnownDataContracts = this.serializer.KnownDataContracts;
					this.isSerializerKnownDataContractsSetExplicit = true;
				}
				return this.serializerKnownDataContracts;
			}
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x000368A4 File Offset: 0x00034AA4
		private DataContract GetDataContractFromSerializerKnownTypes(XmlQualifiedName qname)
		{
			Dictionary<XmlQualifiedName, DataContract> dictionary = this.SerializerKnownDataContracts;
			if (dictionary == null)
			{
				return null;
			}
			DataContract dataContract;
			if (!dictionary.TryGetValue(qname, out dataContract))
			{
				return null;
			}
			return dataContract;
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x000368CC File Offset: 0x00034ACC
		internal static Dictionary<XmlQualifiedName, DataContract> GetDataContractsForKnownTypes(IList<Type> knownTypeList)
		{
			if (knownTypeList == null)
			{
				return null;
			}
			Dictionary<XmlQualifiedName, DataContract> dictionary = new Dictionary<XmlQualifiedName, DataContract>();
			Dictionary<Type, Type> dictionary2 = new Dictionary<Type, Type>();
			for (int i = 0; i < knownTypeList.Count; i++)
			{
				Type type = knownTypeList[i];
				if (type == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("One of the known types provided to the serializer via '{0}' argument was invalid because it was null. All known types specified must be non-null values.", new object[] { "knownTypes" })));
				}
				DataContract.CheckAndAdd(type, dictionary2, ref dictionary);
			}
			return dictionary;
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x0003693C File Offset: 0x00034B3C
		internal bool IsKnownType(DataContract dataContract, Dictionary<XmlQualifiedName, DataContract> knownDataContracts, Type declaredType)
		{
			bool flag = false;
			if (knownDataContracts != null)
			{
				this.scopedKnownTypes.Push(knownDataContracts);
				flag = true;
			}
			bool flag2 = this.IsKnownType(dataContract, declaredType);
			if (flag)
			{
				this.scopedKnownTypes.Pop();
			}
			return flag2;
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x00036974 File Offset: 0x00034B74
		internal bool IsKnownType(DataContract dataContract, Type declaredType)
		{
			DataContract dataContract2 = this.ResolveDataContractFromKnownTypes(dataContract.StableName.Name, dataContract.StableName.Namespace, null, declaredType);
			return dataContract2 != null && dataContract2.UnderlyingType == dataContract.UnderlyingType;
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x000369B8 File Offset: 0x00034BB8
		private DataContract ResolveDataContractFromKnownTypes(XmlQualifiedName typeName)
		{
			DataContract dataContract = PrimitiveDataContract.GetPrimitiveDataContract(typeName.Name, typeName.Namespace);
			if (dataContract == null)
			{
				dataContract = this.scopedKnownTypes.GetDataContract(typeName);
				if (dataContract == null)
				{
					dataContract = this.GetDataContractFromSerializerKnownTypes(typeName);
				}
			}
			return dataContract;
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x000369F4 File Offset: 0x00034BF4
		private DataContract ResolveDataContractFromDataContractResolver(XmlQualifiedName typeName, Type declaredType)
		{
			if (TD.DCResolverResolveIsEnabled())
			{
				TD.DCResolverResolve(typeName.Name + ":" + typeName.Namespace);
			}
			Type type = this.DataContractResolver.ResolveName(typeName.Name, typeName.Namespace, declaredType, this.KnownTypeResolver);
			if (type == null)
			{
				return null;
			}
			return this.GetDataContract(type);
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x00036A54 File Offset: 0x00034C54
		internal Type ResolveNameFromKnownTypes(XmlQualifiedName typeName)
		{
			DataContract dataContract = this.ResolveDataContractFromKnownTypes(typeName);
			if (dataContract == null)
			{
				return null;
			}
			return dataContract.OriginalUnderlyingType;
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x00036A74 File Offset: 0x00034C74
		protected DataContract ResolveDataContractFromKnownTypes(string typeName, string typeNs, DataContract memberTypeContract, Type declaredType)
		{
			XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(typeName, typeNs);
			DataContract dataContract;
			if (this.DataContractResolver == null)
			{
				dataContract = this.ResolveDataContractFromKnownTypes(xmlQualifiedName);
			}
			else
			{
				dataContract = this.ResolveDataContractFromDataContractResolver(xmlQualifiedName, declaredType);
			}
			if (dataContract == null)
			{
				if (memberTypeContract != null && !memberTypeContract.UnderlyingType.IsInterface && memberTypeContract.StableName == xmlQualifiedName)
				{
					dataContract = memberTypeContract;
				}
				if (dataContract == null && this.rootTypeDataContract != null)
				{
					dataContract = this.ResolveDataContractFromRootDataContract(xmlQualifiedName);
				}
			}
			return dataContract;
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x00036ADC File Offset: 0x00034CDC
		protected virtual DataContract ResolveDataContractFromRootDataContract(XmlQualifiedName typeQName)
		{
			if (this.rootTypeDataContract.StableName == typeQName)
			{
				return this.rootTypeDataContract;
			}
			DataContract dataContract;
			for (CollectionDataContract collectionDataContract = this.rootTypeDataContract as CollectionDataContract; collectionDataContract != null; collectionDataContract = dataContract as CollectionDataContract)
			{
				dataContract = this.GetDataContract(this.GetSurrogatedType(collectionDataContract.ItemType));
				if (dataContract.StableName == typeQName)
				{
					return dataContract;
				}
			}
			return null;
		}

		// Token: 0x0400054F RID: 1359
		protected XmlObjectSerializer serializer;

		// Token: 0x04000550 RID: 1360
		protected DataContract rootTypeDataContract;

		// Token: 0x04000551 RID: 1361
		internal ScopedKnownTypes scopedKnownTypes;

		// Token: 0x04000552 RID: 1362
		protected Dictionary<XmlQualifiedName, DataContract> serializerKnownDataContracts;

		// Token: 0x04000553 RID: 1363
		private bool isSerializerKnownDataContractsSetExplicit;

		// Token: 0x04000554 RID: 1364
		protected IList<Type> serializerKnownTypeList;

		// Token: 0x04000555 RID: 1365
		[SecurityCritical]
		private bool demandedSerializationFormatterPermission;

		// Token: 0x04000556 RID: 1366
		[SecurityCritical]
		private bool demandedMemberAccessPermission;

		// Token: 0x04000557 RID: 1367
		private int itemCount;

		// Token: 0x04000558 RID: 1368
		private int maxItemsInObjectGraph;

		// Token: 0x04000559 RID: 1369
		private StreamingContext streamingContext;

		// Token: 0x0400055A RID: 1370
		private bool ignoreExtensionDataObject;

		// Token: 0x0400055B RID: 1371
		private DataContractResolver dataContractResolver;

		// Token: 0x0400055C RID: 1372
		private KnownTypeDataContractResolver knownTypeResolver;

		// Token: 0x0400055D RID: 1373
		private static MethodInfo incrementItemCountMethod;
	}
}
