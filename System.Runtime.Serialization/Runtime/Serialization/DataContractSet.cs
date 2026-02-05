using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x02000079 RID: 121
	internal class DataContractSet
	{
		// Token: 0x060008ED RID: 2285 RVA: 0x00028A1F File Offset: 0x00026C1F
		internal DataContractSet(IDataContractSurrogate dataContractSurrogate)
			: this(dataContractSurrogate, null, null)
		{
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x00028A2A File Offset: 0x00026C2A
		internal DataContractSet(IDataContractSurrogate dataContractSurrogate, ICollection<Type> referencedTypes, ICollection<Type> referencedCollectionTypes)
		{
			this.dataContractSurrogate = dataContractSurrogate;
			this.referencedTypes = referencedTypes;
			this.referencedCollectionTypes = referencedCollectionTypes;
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x00028A48 File Offset: 0x00026C48
		internal DataContractSet(DataContractSet dataContractSet)
		{
			if (dataContractSet == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("dataContractSet"));
			}
			this.dataContractSurrogate = dataContractSet.dataContractSurrogate;
			this.referencedTypes = dataContractSet.referencedTypes;
			this.referencedCollectionTypes = dataContractSet.referencedCollectionTypes;
			foreach (KeyValuePair<XmlQualifiedName, DataContract> keyValuePair in dataContractSet)
			{
				this.Add(keyValuePair.Key, keyValuePair.Value);
			}
			if (dataContractSet.processedContracts != null)
			{
				foreach (KeyValuePair<DataContract, object> keyValuePair2 in dataContractSet.processedContracts)
				{
					this.ProcessedContracts.Add(keyValuePair2.Key, keyValuePair2.Value);
				}
			}
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060008F0 RID: 2288 RVA: 0x00028B38 File Offset: 0x00026D38
		private Dictionary<XmlQualifiedName, DataContract> Contracts
		{
			get
			{
				if (this.contracts == null)
				{
					this.contracts = new Dictionary<XmlQualifiedName, DataContract>();
				}
				return this.contracts;
			}
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060008F1 RID: 2289 RVA: 0x00028B53 File Offset: 0x00026D53
		private Dictionary<DataContract, object> ProcessedContracts
		{
			get
			{
				if (this.processedContracts == null)
				{
					this.processedContracts = new Dictionary<DataContract, object>();
				}
				return this.processedContracts;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060008F2 RID: 2290 RVA: 0x00028B6E File Offset: 0x00026D6E
		private Hashtable SurrogateDataTable
		{
			get
			{
				if (this.surrogateDataTable == null)
				{
					this.surrogateDataTable = new Hashtable();
				}
				return this.surrogateDataTable;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060008F3 RID: 2291 RVA: 0x00028B89 File Offset: 0x00026D89
		// (set) Token: 0x060008F4 RID: 2292 RVA: 0x00028B91 File Offset: 0x00026D91
		internal Dictionary<XmlQualifiedName, DataContract> KnownTypesForObject
		{
			get
			{
				return this.knownTypesForObject;
			}
			set
			{
				this.knownTypesForObject = value;
			}
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x00028B9C File Offset: 0x00026D9C
		internal void Add(Type type)
		{
			DataContract dataContract = this.GetDataContract(type);
			DataContractSet.EnsureTypeNotGeneric(dataContract.UnderlyingType);
			this.Add(dataContract);
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x00028BC3 File Offset: 0x00026DC3
		internal static void EnsureTypeNotGeneric(Type type)
		{
			if (type.ContainsGenericParameters)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Generic type '{0}' is not exportable.", new object[] { type })));
			}
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x00028BEC File Offset: 0x00026DEC
		private void Add(DataContract dataContract)
		{
			this.Add(dataContract.StableName, dataContract);
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x00028BFB File Offset: 0x00026DFB
		public void Add(XmlQualifiedName name, DataContract dataContract)
		{
			if (dataContract.IsBuiltInDataContract)
			{
				return;
			}
			this.InternalAdd(name, dataContract);
		}

		// Token: 0x060008F9 RID: 2297 RVA: 0x00028C10 File Offset: 0x00026E10
		internal void InternalAdd(XmlQualifiedName name, DataContract dataContract)
		{
			DataContract dataContract2 = null;
			if (this.Contracts.TryGetValue(name, out dataContract2))
			{
				if (!dataContract2.Equals(dataContract))
				{
					if (dataContract.UnderlyingType == null || dataContract2.UnderlyingType == null)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("Duplicate contract in data contract set was found, for '{0}' in '{1}' namespace.", new object[]
						{
							dataContract.StableName.Name,
							dataContract.StableName.Namespace
						})));
					}
					bool flag = DataContract.GetClrTypeFullName(dataContract.UnderlyingType) == DataContract.GetClrTypeFullName(dataContract2.UnderlyingType);
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("Duplicate type contract in data contract set. Type name '{0}', for data contract '{1}' in '{2}' namespace.", new object[]
					{
						flag ? dataContract.UnderlyingType.AssemblyQualifiedName : DataContract.GetClrTypeFullName(dataContract.UnderlyingType),
						flag ? dataContract2.UnderlyingType.AssemblyQualifiedName : DataContract.GetClrTypeFullName(dataContract2.UnderlyingType),
						dataContract.StableName.Name,
						dataContract.StableName.Namespace
					})));
				}
			}
			else
			{
				this.Contracts.Add(name, dataContract);
				if (dataContract is ClassDataContract)
				{
					this.AddClassDataContract((ClassDataContract)dataContract);
					return;
				}
				if (dataContract is CollectionDataContract)
				{
					this.AddCollectionDataContract((CollectionDataContract)dataContract);
					return;
				}
				if (dataContract is XmlDataContract)
				{
					this.AddXmlDataContract((XmlDataContract)dataContract);
				}
			}
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x00028D6C File Offset: 0x00026F6C
		private void AddClassDataContract(ClassDataContract classDataContract)
		{
			if (classDataContract.BaseContract != null)
			{
				this.Add(classDataContract.BaseContract.StableName, classDataContract.BaseContract);
			}
			if (!classDataContract.IsISerializable && classDataContract.Members != null)
			{
				for (int i = 0; i < classDataContract.Members.Count; i++)
				{
					DataMember dataMember = classDataContract.Members[i];
					DataContract memberTypeDataContract = this.GetMemberTypeDataContract(dataMember);
					if (this.dataContractSurrogate != null && dataMember.MemberInfo != null)
					{
						object customDataToExport = DataContractSurrogateCaller.GetCustomDataToExport(this.dataContractSurrogate, dataMember.MemberInfo, memberTypeDataContract.UnderlyingType);
						if (customDataToExport != null)
						{
							this.SurrogateDataTable.Add(dataMember, customDataToExport);
						}
					}
					this.Add(memberTypeDataContract.StableName, memberTypeDataContract);
				}
			}
			this.AddKnownDataContracts(classDataContract.KnownDataContracts);
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x00028E2C File Offset: 0x0002702C
		private void AddCollectionDataContract(CollectionDataContract collectionDataContract)
		{
			if (collectionDataContract.IsDictionary)
			{
				ClassDataContract classDataContract = collectionDataContract.ItemContract as ClassDataContract;
				this.AddClassDataContract(classDataContract);
			}
			else
			{
				DataContract itemTypeDataContract = this.GetItemTypeDataContract(collectionDataContract);
				if (itemTypeDataContract != null)
				{
					this.Add(itemTypeDataContract.StableName, itemTypeDataContract);
				}
			}
			this.AddKnownDataContracts(collectionDataContract.KnownDataContracts);
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x00028E7A File Offset: 0x0002707A
		private void AddXmlDataContract(XmlDataContract xmlDataContract)
		{
			this.AddKnownDataContracts(xmlDataContract.KnownDataContracts);
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x00028E88 File Offset: 0x00027088
		private void AddKnownDataContracts(Dictionary<XmlQualifiedName, DataContract> knownDataContracts)
		{
			if (knownDataContracts != null)
			{
				foreach (DataContract dataContract in knownDataContracts.Values)
				{
					this.Add(dataContract);
				}
			}
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x00028EE0 File Offset: 0x000270E0
		internal XmlQualifiedName GetStableName(Type clrType)
		{
			if (this.dataContractSurrogate != null)
			{
				return DataContract.GetStableName(DataContractSurrogateCaller.GetDataContractType(this.dataContractSurrogate, clrType));
			}
			return DataContract.GetStableName(clrType);
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x00028F04 File Offset: 0x00027104
		internal DataContract GetDataContract(Type clrType)
		{
			if (this.dataContractSurrogate == null)
			{
				return DataContract.GetDataContract(clrType);
			}
			DataContract dataContract = DataContract.GetBuiltInDataContract(clrType);
			if (dataContract != null)
			{
				return dataContract;
			}
			Type dataContractType = DataContractSurrogateCaller.GetDataContractType(this.dataContractSurrogate, clrType);
			dataContract = DataContract.GetDataContract(dataContractType);
			if (!this.SurrogateDataTable.Contains(dataContract))
			{
				object customDataToExport = DataContractSurrogateCaller.GetCustomDataToExport(this.dataContractSurrogate, clrType, dataContractType);
				if (customDataToExport != null)
				{
					this.SurrogateDataTable.Add(dataContract, customDataToExport);
				}
			}
			return dataContract;
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x00028F70 File Offset: 0x00027170
		internal DataContract GetMemberTypeDataContract(DataMember dataMember)
		{
			if (!(dataMember.MemberInfo != null))
			{
				return dataMember.MemberTypeContract;
			}
			Type memberType = dataMember.MemberType;
			if (!dataMember.IsGetOnlyCollection)
			{
				return this.GetDataContract(memberType);
			}
			if (this.dataContractSurrogate != null && DataContractSurrogateCaller.GetDataContractType(this.dataContractSurrogate, memberType) != memberType)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Surrogates with get-only collections are not supported. Type '{1}' contains '{2}' which is of '{0}' type.", new object[]
				{
					DataContract.GetClrTypeFullName(memberType),
					DataContract.GetClrTypeFullName(dataMember.MemberInfo.DeclaringType),
					dataMember.MemberInfo.Name
				})));
			}
			return DataContract.GetGetOnlyCollectionDataContract(DataContract.GetId(memberType.TypeHandle), memberType.TypeHandle, memberType, SerializationMode.SharedContract);
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x00029025 File Offset: 0x00027225
		internal DataContract GetItemTypeDataContract(CollectionDataContract collectionContract)
		{
			if (collectionContract.ItemType != null)
			{
				return this.GetDataContract(collectionContract.ItemType);
			}
			return collectionContract.ItemContract;
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x00029048 File Offset: 0x00027248
		internal object GetSurrogateData(object key)
		{
			return this.SurrogateDataTable[key];
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x00029056 File Offset: 0x00027256
		internal void SetSurrogateData(object key, object surrogateData)
		{
			this.SurrogateDataTable[key] = surrogateData;
		}

		// Token: 0x17000141 RID: 321
		public DataContract this[XmlQualifiedName key]
		{
			get
			{
				DataContract builtInDataContract = DataContract.GetBuiltInDataContract(key.Name, key.Namespace);
				if (builtInDataContract == null)
				{
					this.Contracts.TryGetValue(key, out builtInDataContract);
				}
				return builtInDataContract;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000905 RID: 2309 RVA: 0x0002909A File Offset: 0x0002729A
		public IDataContractSurrogate DataContractSurrogate
		{
			get
			{
				return this.dataContractSurrogate;
			}
		}

		// Token: 0x06000906 RID: 2310 RVA: 0x000290A2 File Offset: 0x000272A2
		public bool Remove(XmlQualifiedName key)
		{
			return DataContract.GetBuiltInDataContract(key.Name, key.Namespace) == null && this.Contracts.Remove(key);
		}

		// Token: 0x06000907 RID: 2311 RVA: 0x000290C5 File Offset: 0x000272C5
		public IEnumerator<KeyValuePair<XmlQualifiedName, DataContract>> GetEnumerator()
		{
			return this.Contracts.GetEnumerator();
		}

		// Token: 0x06000908 RID: 2312 RVA: 0x000290D7 File Offset: 0x000272D7
		internal bool IsContractProcessed(DataContract dataContract)
		{
			return this.ProcessedContracts.ContainsKey(dataContract);
		}

		// Token: 0x06000909 RID: 2313 RVA: 0x000290E5 File Offset: 0x000272E5
		internal void SetContractProcessed(DataContract dataContract)
		{
			this.ProcessedContracts.Add(dataContract, dataContract);
		}

		// Token: 0x0600090A RID: 2314 RVA: 0x000290F4 File Offset: 0x000272F4
		internal ContractCodeDomInfo GetContractCodeDomInfo(DataContract dataContract)
		{
			object obj;
			if (this.ProcessedContracts.TryGetValue(dataContract, out obj))
			{
				return (ContractCodeDomInfo)obj;
			}
			return null;
		}

		// Token: 0x0600090B RID: 2315 RVA: 0x00029119 File Offset: 0x00027319
		internal void SetContractCodeDomInfo(DataContract dataContract, ContractCodeDomInfo info)
		{
			this.ProcessedContracts.Add(dataContract, info);
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x00029128 File Offset: 0x00027328
		private Dictionary<XmlQualifiedName, object> GetReferencedTypes()
		{
			if (this.referencedTypesDictionary == null)
			{
				this.referencedTypesDictionary = new Dictionary<XmlQualifiedName, object>();
				this.referencedTypesDictionary.Add(DataContract.GetStableName(Globals.TypeOfNullable), Globals.TypeOfNullable);
				if (this.referencedTypes != null)
				{
					foreach (Type type in this.referencedTypes)
					{
						if (type == null)
						{
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("Referenced types cannot contain null.")));
						}
						this.AddReferencedType(this.referencedTypesDictionary, type);
					}
				}
			}
			return this.referencedTypesDictionary;
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x000291D8 File Offset: 0x000273D8
		private Dictionary<XmlQualifiedName, object> GetReferencedCollectionTypes()
		{
			if (this.referencedCollectionTypesDictionary == null)
			{
				this.referencedCollectionTypesDictionary = new Dictionary<XmlQualifiedName, object>();
				if (this.referencedCollectionTypes != null)
				{
					foreach (Type type in this.referencedCollectionTypes)
					{
						if (type == null)
						{
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("Referenced collection types cannot contain null.")));
						}
						this.AddReferencedType(this.referencedCollectionTypesDictionary, type);
					}
				}
				XmlQualifiedName stableName = DataContract.GetStableName(Globals.TypeOfDictionaryGeneric);
				if (!this.referencedCollectionTypesDictionary.ContainsKey(stableName) && this.GetReferencedTypes().ContainsKey(stableName))
				{
					this.AddReferencedType(this.referencedCollectionTypesDictionary, Globals.TypeOfDictionaryGeneric);
				}
			}
			return this.referencedCollectionTypesDictionary;
		}

		// Token: 0x0600090E RID: 2318 RVA: 0x000292A8 File Offset: 0x000274A8
		private void AddReferencedType(Dictionary<XmlQualifiedName, object> referencedTypes, Type type)
		{
			if (DataContractSet.IsTypeReferenceable(type))
			{
				XmlQualifiedName stableName;
				try
				{
					stableName = this.GetStableName(type);
				}
				catch (InvalidDataContractException)
				{
					return;
				}
				catch (InvalidOperationException)
				{
					return;
				}
				object obj;
				if (referencedTypes.TryGetValue(stableName, out obj))
				{
					Type type2 = obj as Type;
					if (type2 != null)
					{
						if (type2 != type)
						{
							referencedTypes.Remove(stableName);
							referencedTypes.Add(stableName, new List<Type> { type2, type });
							return;
						}
					}
					else
					{
						List<Type> list = (List<Type>)obj;
						if (!list.Contains(type))
						{
							list.Add(type);
							return;
						}
					}
				}
				else
				{
					referencedTypes.Add(stableName, type);
				}
			}
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x00029358 File Offset: 0x00027558
		internal bool TryGetReferencedType(XmlQualifiedName stableName, DataContract dataContract, out Type type)
		{
			return this.TryGetReferencedType(stableName, dataContract, false, out type);
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x00029364 File Offset: 0x00027564
		internal bool TryGetReferencedCollectionType(XmlQualifiedName stableName, DataContract dataContract, out Type type)
		{
			return this.TryGetReferencedType(stableName, dataContract, true, out type);
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x00029370 File Offset: 0x00027570
		private bool TryGetReferencedType(XmlQualifiedName stableName, DataContract dataContract, bool useReferencedCollectionTypes, out Type type)
		{
			object obj;
			if (!(useReferencedCollectionTypes ? this.GetReferencedCollectionTypes() : this.GetReferencedTypes()).TryGetValue(stableName, out obj))
			{
				type = null;
				return false;
			}
			type = obj as Type;
			if (type != null)
			{
				return true;
			}
			List<Type> list = (List<Type>)obj;
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			for (int i = 0; i < list.Count; i++)
			{
				Type type2 = list[i];
				if (!flag)
				{
					flag = type2.IsGenericTypeDefinition;
				}
				stringBuilder.AppendFormat("{0}\"{1}\" ", Environment.NewLine, type2.AssemblyQualifiedName);
				if (dataContract != null)
				{
					DataContract dataContract2 = this.GetDataContract(type2);
					stringBuilder.Append(SR.GetString((dataContract2 != null && dataContract2.Equals(dataContract)) ? "Reference type matches." : "Reference type does not match."));
				}
			}
			if (flag)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString(useReferencedCollectionTypes ? "Ambiguous collection types were referenced: {0}" : "Ambiguous types were referenced: {0}", new object[] { stringBuilder.ToString() })));
			}
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString(useReferencedCollectionTypes ? "In '{0}' element in '{1}' namespace, ambiguous collection types were referenced: {2}" : "In '{0}' element in '{1}' namespace, ambiguous types were referenced: {2}", new object[]
			{
				XmlConvert.DecodeName(stableName.Name),
				stableName.Namespace,
				stringBuilder.ToString()
			})));
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x000294B0 File Offset: 0x000276B0
		private static bool IsTypeReferenceable(Type type)
		{
			try
			{
				Type type2;
				return type.IsSerializable || type.IsDefined(Globals.TypeOfDataContractAttribute, false) || (Globals.TypeOfIXmlSerializable.IsAssignableFrom(type) && !type.IsGenericTypeDefinition) || CollectionDataContract.IsCollection(type, out type2) || ClassDataContract.IsNonAttributedTypeValidForSerialization(type);
			}
			catch (Exception ex)
			{
				if (Fx.IsFatal(ex))
				{
					throw;
				}
			}
			return false;
		}

		// Token: 0x04000340 RID: 832
		private Dictionary<XmlQualifiedName, DataContract> contracts;

		// Token: 0x04000341 RID: 833
		private Dictionary<DataContract, object> processedContracts;

		// Token: 0x04000342 RID: 834
		private IDataContractSurrogate dataContractSurrogate;

		// Token: 0x04000343 RID: 835
		private Hashtable surrogateDataTable;

		// Token: 0x04000344 RID: 836
		private Dictionary<XmlQualifiedName, DataContract> knownTypesForObject;

		// Token: 0x04000345 RID: 837
		private ICollection<Type> referencedTypes;

		// Token: 0x04000346 RID: 838
		private ICollection<Type> referencedCollectionTypes;

		// Token: 0x04000347 RID: 839
		private Dictionary<XmlQualifiedName, object> referencedTypesDictionary;

		// Token: 0x04000348 RID: 840
		private Dictionary<XmlQualifiedName, object> referencedCollectionTypesDictionary;
	}
}
