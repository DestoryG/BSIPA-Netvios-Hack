using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security;
using System.Threading;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x02000069 RID: 105
	internal sealed class CollectionDataContract : DataContract
	{
		// Token: 0x060007A0 RID: 1952 RVA: 0x00024E76 File Offset: 0x00023076
		[SecuritySafeCritical]
		internal CollectionDataContract(CollectionKind kind)
			: base(new CollectionDataContract.CollectionDataContractCriticalHelper(kind))
		{
			this.InitCollectionDataContract(this);
		}

		// Token: 0x060007A1 RID: 1953 RVA: 0x00024E8B File Offset: 0x0002308B
		[SecuritySafeCritical]
		internal CollectionDataContract(Type type)
			: base(new CollectionDataContract.CollectionDataContractCriticalHelper(type))
		{
			this.InitCollectionDataContract(this);
		}

		// Token: 0x060007A2 RID: 1954 RVA: 0x00024EA0 File Offset: 0x000230A0
		[SecuritySafeCritical]
		internal CollectionDataContract(Type type, DataContract itemContract)
			: base(new CollectionDataContract.CollectionDataContractCriticalHelper(type, itemContract))
		{
			this.InitCollectionDataContract(this);
		}

		// Token: 0x060007A3 RID: 1955 RVA: 0x00024EB6 File Offset: 0x000230B6
		[SecuritySafeCritical]
		private CollectionDataContract(Type type, CollectionKind kind, Type itemType, MethodInfo getEnumeratorMethod, string serializationExceptionMessage, string deserializationExceptionMessage)
			: base(new CollectionDataContract.CollectionDataContractCriticalHelper(type, kind, itemType, getEnumeratorMethod, serializationExceptionMessage, deserializationExceptionMessage))
		{
			this.InitCollectionDataContract(this.GetSharedTypeContract(type));
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x00024ED9 File Offset: 0x000230D9
		[SecuritySafeCritical]
		private CollectionDataContract(Type type, CollectionKind kind, Type itemType, MethodInfo getEnumeratorMethod, MethodInfo addMethod, ConstructorInfo constructor)
			: base(new CollectionDataContract.CollectionDataContractCriticalHelper(type, kind, itemType, getEnumeratorMethod, addMethod, constructor))
		{
			this.InitCollectionDataContract(this.GetSharedTypeContract(type));
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x00024EFC File Offset: 0x000230FC
		[SecuritySafeCritical]
		private CollectionDataContract(Type type, CollectionKind kind, Type itemType, MethodInfo getEnumeratorMethod, MethodInfo addMethod, ConstructorInfo constructor, bool isConstructorCheckRequired)
			: base(new CollectionDataContract.CollectionDataContractCriticalHelper(type, kind, itemType, getEnumeratorMethod, addMethod, constructor, isConstructorCheckRequired))
		{
			this.InitCollectionDataContract(this.GetSharedTypeContract(type));
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x00024F21 File Offset: 0x00023121
		[SecuritySafeCritical]
		private CollectionDataContract(Type type, string invalidCollectionInSharedContractMessage)
			: base(new CollectionDataContract.CollectionDataContractCriticalHelper(type, invalidCollectionInSharedContractMessage))
		{
			this.InitCollectionDataContract(this.GetSharedTypeContract(type));
		}

		// Token: 0x060007A7 RID: 1959 RVA: 0x00024F40 File Offset: 0x00023140
		[SecurityCritical]
		private void InitCollectionDataContract(DataContract sharedTypeContract)
		{
			this.helper = base.Helper as CollectionDataContract.CollectionDataContractCriticalHelper;
			this.collectionItemName = this.helper.CollectionItemName;
			if (this.helper.Kind == CollectionKind.Dictionary || this.helper.Kind == CollectionKind.GenericDictionary)
			{
				this.itemContract = this.helper.ItemContract;
			}
			this.helper.SharedTypeContract = sharedTypeContract;
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x00024FA8 File Offset: 0x000231A8
		private void InitSharedTypeContract()
		{
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060007A9 RID: 1961 RVA: 0x00024FAA File Offset: 0x000231AA
		private static Type[] KnownInterfaces
		{
			[SecuritySafeCritical]
			get
			{
				return CollectionDataContract.CollectionDataContractCriticalHelper.KnownInterfaces;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060007AA RID: 1962 RVA: 0x00024FB1 File Offset: 0x000231B1
		internal CollectionKind Kind
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.Kind;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060007AB RID: 1963 RVA: 0x00024FBE File Offset: 0x000231BE
		internal Type ItemType
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.ItemType;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060007AC RID: 1964 RVA: 0x00024FCB File Offset: 0x000231CB
		// (set) Token: 0x060007AD RID: 1965 RVA: 0x00024FE2 File Offset: 0x000231E2
		public DataContract ItemContract
		{
			[SecuritySafeCritical]
			get
			{
				return this.itemContract ?? this.helper.ItemContract;
			}
			[SecurityCritical]
			set
			{
				this.itemContract = value;
				this.helper.ItemContract = value;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060007AE RID: 1966 RVA: 0x00024FF7 File Offset: 0x000231F7
		internal DataContract SharedTypeContract
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.SharedTypeContract;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060007AF RID: 1967 RVA: 0x00025004 File Offset: 0x00023204
		// (set) Token: 0x060007B0 RID: 1968 RVA: 0x00025011 File Offset: 0x00023211
		internal string ItemName
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.ItemName;
			}
			[SecurityCritical]
			set
			{
				this.helper.ItemName = value;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060007B1 RID: 1969 RVA: 0x0002501F File Offset: 0x0002321F
		public XmlDictionaryString CollectionItemName
		{
			[SecuritySafeCritical]
			get
			{
				return this.collectionItemName;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060007B2 RID: 1970 RVA: 0x00025027 File Offset: 0x00023227
		// (set) Token: 0x060007B3 RID: 1971 RVA: 0x00025034 File Offset: 0x00023234
		internal string KeyName
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.KeyName;
			}
			[SecurityCritical]
			set
			{
				this.helper.KeyName = value;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060007B4 RID: 1972 RVA: 0x00025042 File Offset: 0x00023242
		// (set) Token: 0x060007B5 RID: 1973 RVA: 0x0002504F File Offset: 0x0002324F
		internal string ValueName
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.ValueName;
			}
			[SecurityCritical]
			set
			{
				this.helper.ValueName = value;
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x060007B6 RID: 1974 RVA: 0x0002505D File Offset: 0x0002325D
		internal bool IsDictionary
		{
			get
			{
				return this.KeyName != null;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060007B7 RID: 1975 RVA: 0x00025068 File Offset: 0x00023268
		public XmlDictionaryString ChildElementNamespace
		{
			[SecuritySafeCritical]
			get
			{
				if (this.childElementNamespace == null)
				{
					lock (this)
					{
						if (this.childElementNamespace == null)
						{
							if (this.helper.ChildElementNamespace == null && !this.IsDictionary)
							{
								XmlDictionaryString childNamespaceToDeclare = ClassDataContract.GetChildNamespaceToDeclare(this, this.ItemType, new XmlDictionary());
								Thread.MemoryBarrier();
								this.helper.ChildElementNamespace = childNamespaceToDeclare;
							}
							this.childElementNamespace = this.helper.ChildElementNamespace;
						}
					}
				}
				return this.childElementNamespace;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060007B8 RID: 1976 RVA: 0x000250FC File Offset: 0x000232FC
		// (set) Token: 0x060007B9 RID: 1977 RVA: 0x00025109 File Offset: 0x00023309
		internal bool IsItemTypeNullable
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.IsItemTypeNullable;
			}
			[SecurityCritical]
			set
			{
				this.helper.IsItemTypeNullable = value;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060007BA RID: 1978 RVA: 0x00025117 File Offset: 0x00023317
		// (set) Token: 0x060007BB RID: 1979 RVA: 0x00025124 File Offset: 0x00023324
		internal bool IsConstructorCheckRequired
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.IsConstructorCheckRequired;
			}
			[SecurityCritical]
			set
			{
				this.helper.IsConstructorCheckRequired = value;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060007BC RID: 1980 RVA: 0x00025132 File Offset: 0x00023332
		internal MethodInfo GetEnumeratorMethod
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.GetEnumeratorMethod;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060007BD RID: 1981 RVA: 0x0002513F File Offset: 0x0002333F
		internal MethodInfo AddMethod
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.AddMethod;
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060007BE RID: 1982 RVA: 0x0002514C File Offset: 0x0002334C
		internal ConstructorInfo Constructor
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.Constructor;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060007BF RID: 1983 RVA: 0x00025159 File Offset: 0x00023359
		// (set) Token: 0x060007C0 RID: 1984 RVA: 0x00025166 File Offset: 0x00023366
		internal override Dictionary<XmlQualifiedName, DataContract> KnownDataContracts
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.KnownDataContracts;
			}
			[SecurityCritical]
			set
			{
				this.helper.KnownDataContracts = value;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060007C1 RID: 1985 RVA: 0x00025174 File Offset: 0x00023374
		internal string InvalidCollectionInSharedContractMessage
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.InvalidCollectionInSharedContractMessage;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060007C2 RID: 1986 RVA: 0x00025181 File Offset: 0x00023381
		internal string SerializationExceptionMessage
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.SerializationExceptionMessage;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060007C3 RID: 1987 RVA: 0x0002518E File Offset: 0x0002338E
		internal string DeserializationExceptionMessage
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.DeserializationExceptionMessage;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060007C4 RID: 1988 RVA: 0x0002519B File Offset: 0x0002339B
		internal bool IsReadOnlyContract
		{
			get
			{
				return this.DeserializationExceptionMessage != null;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060007C5 RID: 1989 RVA: 0x000251A6 File Offset: 0x000233A6
		private bool ItemNameSetExplicit
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.ItemNameSetExplicit;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060007C6 RID: 1990 RVA: 0x000251B4 File Offset: 0x000233B4
		internal XmlFormatCollectionWriterDelegate XmlFormatWriterDelegate
		{
			[SecuritySafeCritical]
			get
			{
				if (this.helper.XmlFormatWriterDelegate == null)
				{
					lock (this)
					{
						if (this.helper.XmlFormatWriterDelegate == null)
						{
							XmlFormatCollectionWriterDelegate xmlFormatCollectionWriterDelegate = new XmlFormatWriterGenerator().GenerateCollectionWriter(this);
							Thread.MemoryBarrier();
							this.helper.XmlFormatWriterDelegate = xmlFormatCollectionWriterDelegate;
						}
					}
				}
				return this.helper.XmlFormatWriterDelegate;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060007C7 RID: 1991 RVA: 0x0002522C File Offset: 0x0002342C
		internal XmlFormatCollectionReaderDelegate XmlFormatReaderDelegate
		{
			[SecuritySafeCritical]
			get
			{
				if (this.helper.XmlFormatReaderDelegate == null)
				{
					lock (this)
					{
						if (this.helper.XmlFormatReaderDelegate == null)
						{
							if (this.IsReadOnlyContract)
							{
								DataContract.ThrowInvalidDataContractException(this.helper.DeserializationExceptionMessage, null);
							}
							XmlFormatCollectionReaderDelegate xmlFormatCollectionReaderDelegate = new XmlFormatReaderGenerator().GenerateCollectionReader(this);
							Thread.MemoryBarrier();
							this.helper.XmlFormatReaderDelegate = xmlFormatCollectionReaderDelegate;
						}
					}
				}
				return this.helper.XmlFormatReaderDelegate;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060007C8 RID: 1992 RVA: 0x000252BC File Offset: 0x000234BC
		internal XmlFormatGetOnlyCollectionReaderDelegate XmlFormatGetOnlyCollectionReaderDelegate
		{
			[SecuritySafeCritical]
			get
			{
				if (this.helper.XmlFormatGetOnlyCollectionReaderDelegate == null)
				{
					lock (this)
					{
						if (this.helper.XmlFormatGetOnlyCollectionReaderDelegate == null)
						{
							if (base.UnderlyingType.IsInterface && (this.Kind == CollectionKind.Enumerable || this.Kind == CollectionKind.Collection || this.Kind == CollectionKind.GenericEnumerable))
							{
								throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("On type '{0}', get-only collection must have an Add method.", new object[] { DataContract.GetClrTypeFullName(base.UnderlyingType) })));
							}
							if (this.IsReadOnlyContract)
							{
								DataContract.ThrowInvalidDataContractException(this.helper.DeserializationExceptionMessage, null);
							}
							XmlFormatGetOnlyCollectionReaderDelegate xmlFormatGetOnlyCollectionReaderDelegate = new XmlFormatReaderGenerator().GenerateGetOnlyCollectionReader(this);
							Thread.MemoryBarrier();
							this.helper.XmlFormatGetOnlyCollectionReaderDelegate = xmlFormatGetOnlyCollectionReaderDelegate;
						}
					}
				}
				return this.helper.XmlFormatGetOnlyCollectionReaderDelegate;
			}
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x000253A4 File Offset: 0x000235A4
		private DataContract GetSharedTypeContract(Type type)
		{
			if (type.IsDefined(Globals.TypeOfCollectionDataContractAttribute, false))
			{
				return this;
			}
			if (type.IsSerializable || type.IsDefined(Globals.TypeOfDataContractAttribute, false))
			{
				return new ClassDataContract(type);
			}
			return null;
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x000253D4 File Offset: 0x000235D4
		internal static bool IsCollectionInterface(Type type)
		{
			if (type.IsGenericType)
			{
				type = type.GetGenericTypeDefinition();
			}
			return ((ICollection<Type>)CollectionDataContract.KnownInterfaces).Contains(type);
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x000253F4 File Offset: 0x000235F4
		internal static bool IsCollection(Type type)
		{
			Type type2;
			return CollectionDataContract.IsCollection(type, out type2);
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x00025409 File Offset: 0x00023609
		internal static bool IsCollection(Type type, out Type itemType)
		{
			return CollectionDataContract.IsCollectionHelper(type, out itemType, true, false);
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x00025414 File Offset: 0x00023614
		internal static bool IsCollection(Type type, bool constructorRequired, bool skipIfReadOnlyContract)
		{
			Type type2;
			return CollectionDataContract.IsCollectionHelper(type, out type2, constructorRequired, skipIfReadOnlyContract);
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x0002542C File Offset: 0x0002362C
		private static bool IsCollectionHelper(Type type, out Type itemType, bool constructorRequired, bool skipIfReadOnlyContract = false)
		{
			if (type.IsArray && DataContract.GetBuiltInDataContract(type) == null)
			{
				itemType = type.GetElementType();
				return true;
			}
			DataContract dataContract;
			return CollectionDataContract.IsCollectionOrTryCreate(type, false, out dataContract, out itemType, constructorRequired, skipIfReadOnlyContract);
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x00025460 File Offset: 0x00023660
		internal static bool TryCreate(Type type, out DataContract dataContract)
		{
			Type type2;
			return CollectionDataContract.IsCollectionOrTryCreate(type, true, out dataContract, out type2, true, false);
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x0002547C File Offset: 0x0002367C
		internal static bool TryCreateGetOnlyCollectionDataContract(Type type, out DataContract dataContract)
		{
			if (type.IsArray)
			{
				dataContract = new CollectionDataContract(type);
				return true;
			}
			Type type2;
			return CollectionDataContract.IsCollectionOrTryCreate(type, true, out dataContract, out type2, false, false);
		}

		// Token: 0x060007D1 RID: 2001 RVA: 0x000254A8 File Offset: 0x000236A8
		internal static MethodInfo GetTargetMethodWithName(string name, Type type, Type interfaceType)
		{
			InterfaceMapping interfaceMap = type.GetInterfaceMap(interfaceType);
			for (int i = 0; i < interfaceMap.TargetMethods.Length; i++)
			{
				if (interfaceMap.InterfaceMethods[i].Name == name)
				{
					return interfaceMap.InterfaceMethods[i];
				}
			}
			return null;
		}

		// Token: 0x060007D2 RID: 2002 RVA: 0x000254EF File Offset: 0x000236EF
		private static bool IsArraySegment(Type t)
		{
			return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ArraySegment<>);
		}

		// Token: 0x060007D3 RID: 2003 RVA: 0x00025510 File Offset: 0x00023710
		private static bool IsCollectionOrTryCreate(Type type, bool tryCreate, out DataContract dataContract, out Type itemType, bool constructorRequired, bool skipIfReadOnlyContract = false)
		{
			dataContract = null;
			itemType = Globals.TypeOfObject;
			if (DataContract.GetBuiltInDataContract(type) != null)
			{
				return CollectionDataContract.HandleIfInvalidCollection(type, tryCreate, false, false, "{0} is a built-in type and cannot be a collection.", null, ref dataContract);
			}
			bool flag = CollectionDataContract.IsCollectionDataContract(type);
			bool flag2 = false;
			string text = null;
			string text2 = null;
			Type baseType = type.BaseType;
			bool flag3 = baseType != null && baseType != Globals.TypeOfObject && baseType != Globals.TypeOfValueType && baseType != Globals.TypeOfUri && CollectionDataContract.IsCollection(baseType) && !type.IsSerializable;
			if (type.IsDefined(Globals.TypeOfDataContractAttribute, false))
			{
				return CollectionDataContract.HandleIfInvalidCollection(type, tryCreate, flag, flag3, "{0} has DataContractAttribute attribute.", null, ref dataContract);
			}
			if (Globals.TypeOfIXmlSerializable.IsAssignableFrom(type) || CollectionDataContract.IsArraySegment(type))
			{
				return false;
			}
			if (!Globals.TypeOfIEnumerable.IsAssignableFrom(type))
			{
				return CollectionDataContract.HandleIfInvalidCollection(type, tryCreate, flag, flag3, "{0} does not implement IEnumerable interface.", null, ref dataContract);
			}
			if (type.IsInterface)
			{
				Type type2 = (type.IsGenericType ? type.GetGenericTypeDefinition() : type);
				Type[] knownInterfaces = CollectionDataContract.KnownInterfaces;
				for (int i = 0; i < knownInterfaces.Length; i++)
				{
					if (knownInterfaces[i] == type2)
					{
						MethodInfo methodInfo = null;
						MethodInfo methodInfo2;
						if (type.IsGenericType)
						{
							Type[] genericArguments = type.GetGenericArguments();
							if (type2 == Globals.TypeOfIDictionaryGeneric)
							{
								itemType = Globals.TypeOfKeyValue.MakeGenericType(genericArguments);
								methodInfo = type.GetMethod("Add");
								methodInfo2 = Globals.TypeOfIEnumerableGeneric.MakeGenericType(new Type[] { Globals.TypeOfKeyValuePair.MakeGenericType(genericArguments) }).GetMethod("GetEnumerator");
							}
							else
							{
								itemType = genericArguments[0];
								if (type2 == Globals.TypeOfICollectionGeneric || type2 == Globals.TypeOfIListGeneric)
								{
									methodInfo = Globals.TypeOfICollectionGeneric.MakeGenericType(new Type[] { itemType }).GetMethod("Add");
								}
								methodInfo2 = Globals.TypeOfIEnumerableGeneric.MakeGenericType(new Type[] { itemType }).GetMethod("GetEnumerator");
							}
						}
						else
						{
							if (type2 == Globals.TypeOfIDictionary)
							{
								itemType = typeof(KeyValue<object, object>);
								methodInfo = type.GetMethod("Add");
							}
							else
							{
								itemType = Globals.TypeOfObject;
								if (type2 == Globals.TypeOfIList)
								{
									methodInfo = Globals.TypeOfIList.GetMethod("Add");
								}
							}
							methodInfo2 = Globals.TypeOfIEnumerable.GetMethod("GetEnumerator");
						}
						if (tryCreate)
						{
							dataContract = new CollectionDataContract(type, (CollectionKind)(i + 1), itemType, methodInfo2, methodInfo, null);
						}
						return true;
					}
				}
			}
			ConstructorInfo constructorInfo = null;
			if (!type.IsValueType)
			{
				constructorInfo = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Globals.EmptyTypeArray, null);
				if (constructorInfo == null && constructorRequired)
				{
					if (type.IsSerializable)
					{
						return CollectionDataContract.HandleIfInvalidCollection(type, tryCreate, flag, flag3, "{0} does not have a default constructor.", null, ref dataContract);
					}
					flag2 = true;
					CollectionDataContract.GetReadOnlyCollectionExceptionMessages(type, flag, "{0} does not have a default constructor.", null, out text, out text2);
				}
			}
			Type type3 = null;
			CollectionKind collectionKind = CollectionKind.None;
			bool flag4 = false;
			foreach (Type type4 in type.GetInterfaces())
			{
				Type type5 = (type4.IsGenericType ? type4.GetGenericTypeDefinition() : type4);
				Type[] knownInterfaces2 = CollectionDataContract.KnownInterfaces;
				int k = 0;
				while (k < knownInterfaces2.Length)
				{
					if (knownInterfaces2[k] == type5)
					{
						CollectionKind collectionKind2 = (CollectionKind)(k + 1);
						if (collectionKind == CollectionKind.None || collectionKind2 < collectionKind)
						{
							collectionKind = collectionKind2;
							type3 = type4;
							flag4 = false;
							break;
						}
						if ((collectionKind & collectionKind2) == collectionKind2)
						{
							flag4 = true;
							break;
						}
						break;
					}
					else
					{
						k++;
					}
				}
			}
			if (collectionKind == CollectionKind.None)
			{
				return CollectionDataContract.HandleIfInvalidCollection(type, tryCreate, flag, flag3, "{0} does not implement IEnumerable interface.", null, ref dataContract);
			}
			if (collectionKind == CollectionKind.Enumerable || collectionKind == CollectionKind.Collection || collectionKind == CollectionKind.GenericEnumerable)
			{
				if (flag4)
				{
					type3 = Globals.TypeOfIEnumerable;
				}
				itemType = (type3.IsGenericType ? type3.GetGenericArguments()[0] : Globals.TypeOfObject);
				MethodInfo methodInfo;
				MethodInfo methodInfo2;
				CollectionDataContract.GetCollectionMethods(type, type3, new Type[] { itemType }, false, out methodInfo2, out methodInfo);
				if (methodInfo == null)
				{
					if (type.IsSerializable || skipIfReadOnlyContract)
					{
						return CollectionDataContract.HandleIfInvalidCollection(type, tryCreate, flag, flag3 && !skipIfReadOnlyContract, "{0} does not have a valid Add method with parameter of type '{1}'.", DataContract.GetClrTypeFullName(itemType), ref dataContract);
					}
					flag2 = true;
					CollectionDataContract.GetReadOnlyCollectionExceptionMessages(type, flag, "{0} does not have a valid Add method with parameter of type '{1}'.", DataContract.GetClrTypeFullName(itemType), out text, out text2);
				}
				if (tryCreate)
				{
					dataContract = (flag2 ? new CollectionDataContract(type, collectionKind, itemType, methodInfo2, text, text2) : new CollectionDataContract(type, collectionKind, itemType, methodInfo2, methodInfo, constructorInfo, !constructorRequired));
				}
			}
			else
			{
				if (flag4)
				{
					return CollectionDataContract.HandleIfInvalidCollection(type, tryCreate, flag, flag3, "{0} has multiple definitions of interface '{1}'.", CollectionDataContract.KnownInterfaces[(int)(collectionKind - CollectionKind.GenericDictionary)].Name, ref dataContract);
				}
				Type[] array = null;
				switch (collectionKind)
				{
				case CollectionKind.GenericDictionary:
					array = type3.GetGenericArguments();
					itemType = ((type3.IsGenericTypeDefinition || (array[0].IsGenericParameter && array[1].IsGenericParameter)) ? Globals.TypeOfKeyValue : Globals.TypeOfKeyValue.MakeGenericType(array));
					break;
				case CollectionKind.Dictionary:
					array = new Type[]
					{
						Globals.TypeOfObject,
						Globals.TypeOfObject
					};
					itemType = Globals.TypeOfKeyValue.MakeGenericType(array);
					break;
				case CollectionKind.GenericList:
				case CollectionKind.GenericCollection:
					array = type3.GetGenericArguments();
					itemType = array[0];
					break;
				case CollectionKind.List:
					itemType = Globals.TypeOfObject;
					array = new Type[] { itemType };
					break;
				}
				if (tryCreate)
				{
					MethodInfo methodInfo;
					MethodInfo methodInfo2;
					CollectionDataContract.GetCollectionMethods(type, type3, array, true, out methodInfo2, out methodInfo);
					dataContract = (flag2 ? new CollectionDataContract(type, collectionKind, itemType, methodInfo2, text, text2) : new CollectionDataContract(type, collectionKind, itemType, methodInfo2, methodInfo, constructorInfo, !constructorRequired));
				}
			}
			return !flag2 || !skipIfReadOnlyContract;
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x00025A83 File Offset: 0x00023C83
		internal static bool IsCollectionDataContract(Type type)
		{
			return type.IsDefined(Globals.TypeOfCollectionDataContractAttribute, false);
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x00025A94 File Offset: 0x00023C94
		private static bool HandleIfInvalidCollection(Type type, bool tryCreate, bool hasCollectionDataContract, bool createContractWithException, string message, string param, ref DataContract dataContract)
		{
			if (hasCollectionDataContract)
			{
				if (tryCreate)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(CollectionDataContract.GetInvalidCollectionMessage(message, SR.GetString("Type '{0}' with CollectionDataContractAttribute attribute is an invalid collection type since it", new object[] { DataContract.GetClrTypeFullName(type) }), param)));
				}
				return true;
			}
			else
			{
				if (createContractWithException)
				{
					if (tryCreate)
					{
						dataContract = new CollectionDataContract(type, CollectionDataContract.GetInvalidCollectionMessage(message, SR.GetString("Type '{0}' is an invalid collection type since it", new object[] { DataContract.GetClrTypeFullName(type) }), param));
					}
					return true;
				}
				return false;
			}
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x00025B0C File Offset: 0x00023D0C
		private static void GetReadOnlyCollectionExceptionMessages(Type type, bool hasCollectionDataContract, string message, string param, out string serializationExceptionMessage, out string deserializationExceptionMessage)
		{
			serializationExceptionMessage = CollectionDataContract.GetInvalidCollectionMessage(message, SR.GetString(hasCollectionDataContract ? "Type '{0}' with CollectionDataContractAttribute attribute is an invalid collection type since it" : "Type '{0}' is an invalid collection type since it", new object[] { DataContract.GetClrTypeFullName(type) }), param);
			deserializationExceptionMessage = CollectionDataContract.GetInvalidCollectionMessage(message, SR.GetString("Error on deserializing read-only collection: {0}", new object[] { DataContract.GetClrTypeFullName(type) }), param);
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x00025B69 File Offset: 0x00023D69
		private static string GetInvalidCollectionMessage(string message, string nestedMessage, string param)
		{
			if (param != null)
			{
				return SR.GetString(message, new object[] { nestedMessage, param });
			}
			return SR.GetString(message, new object[] { nestedMessage });
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x00025B94 File Offset: 0x00023D94
		private static void FindCollectionMethodsOnInterface(Type type, Type interfaceType, ref MethodInfo addMethod, ref MethodInfo getEnumeratorMethod)
		{
			InterfaceMapping interfaceMap = type.GetInterfaceMap(interfaceType);
			for (int i = 0; i < interfaceMap.TargetMethods.Length; i++)
			{
				if (interfaceMap.InterfaceMethods[i].Name == "Add")
				{
					addMethod = interfaceMap.InterfaceMethods[i];
				}
				else if (interfaceMap.InterfaceMethods[i].Name == "GetEnumerator")
				{
					getEnumeratorMethod = interfaceMap.InterfaceMethods[i];
				}
			}
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x00025C04 File Offset: 0x00023E04
		private static void GetCollectionMethods(Type type, Type interfaceType, Type[] addMethodTypeArray, bool addMethodOnInterface, out MethodInfo getEnumeratorMethod, out MethodInfo addMethod)
		{
			MethodInfo methodInfo;
			getEnumeratorMethod = (methodInfo = null);
			addMethod = methodInfo;
			if (addMethodOnInterface)
			{
				addMethod = type.GetMethod("Add", BindingFlags.Instance | BindingFlags.Public, null, addMethodTypeArray, null);
				if (addMethod == null || addMethod.GetParameters()[0].ParameterType != addMethodTypeArray[0])
				{
					CollectionDataContract.FindCollectionMethodsOnInterface(type, interfaceType, ref addMethod, ref getEnumeratorMethod);
					if (addMethod == null)
					{
						foreach (Type type2 in interfaceType.GetInterfaces())
						{
							if (CollectionDataContract.IsKnownInterface(type2))
							{
								CollectionDataContract.FindCollectionMethodsOnInterface(type, type2, ref addMethod, ref getEnumeratorMethod);
								if (addMethod == null)
								{
									break;
								}
							}
						}
					}
				}
			}
			else
			{
				addMethod = type.GetMethod("Add", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, addMethodTypeArray, null);
			}
			if (getEnumeratorMethod == null)
			{
				getEnumeratorMethod = type.GetMethod("GetEnumerator", BindingFlags.Instance | BindingFlags.Public, null, Globals.EmptyTypeArray, null);
				if (getEnumeratorMethod == null || !Globals.TypeOfIEnumerator.IsAssignableFrom(getEnumeratorMethod.ReturnType))
				{
					Type type3 = interfaceType.GetInterface("System.Collections.Generic.IEnumerable*");
					if (type3 == null)
					{
						type3 = Globals.TypeOfIEnumerable;
					}
					getEnumeratorMethod = CollectionDataContract.GetTargetMethodWithName("GetEnumerator", type, type3);
				}
			}
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x00025D2C File Offset: 0x00023F2C
		private static bool IsKnownInterface(Type type)
		{
			Type type2 = (type.IsGenericType ? type.GetGenericTypeDefinition() : type);
			foreach (Type type3 in CollectionDataContract.KnownInterfaces)
			{
				if (type2 == type3)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x00025D70 File Offset: 0x00023F70
		[SecuritySafeCritical]
		internal override DataContract BindGenericParameters(DataContract[] paramContracts, Dictionary<DataContract, DataContract> boundContracts)
		{
			DataContract dataContract;
			if (boundContracts.TryGetValue(this, out dataContract))
			{
				return dataContract;
			}
			CollectionDataContract collectionDataContract = new CollectionDataContract(this.Kind);
			boundContracts.Add(this, collectionDataContract);
			collectionDataContract.ItemContract = this.ItemContract.BindGenericParameters(paramContracts, boundContracts);
			collectionDataContract.IsItemTypeNullable = !collectionDataContract.ItemContract.IsValueType;
			collectionDataContract.ItemName = (this.ItemNameSetExplicit ? this.ItemName : collectionDataContract.ItemContract.StableName.Name);
			collectionDataContract.KeyName = this.KeyName;
			collectionDataContract.ValueName = this.ValueName;
			collectionDataContract.StableName = DataContract.CreateQualifiedName(DataContract.ExpandGenericParameters(XmlConvert.DecodeName(base.StableName.Name), new GenericNameProvider(DataContract.GetClrTypeFullName(base.UnderlyingType), paramContracts)), CollectionDataContract.IsCollectionDataContract(base.UnderlyingType) ? base.StableName.Namespace : DataContract.GetCollectionNamespace(collectionDataContract.ItemContract.StableName.Namespace));
			return collectionDataContract;
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x00025E66 File Offset: 0x00024066
		internal override DataContract GetValidContract(SerializationMode mode)
		{
			if (mode == SerializationMode.SharedType)
			{
				if (this.SharedTypeContract == null)
				{
					DataContract.ThrowTypeNotSerializable(base.UnderlyingType);
				}
				return this.SharedTypeContract;
			}
			this.ThrowIfInvalid();
			return this;
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x00025E8D File Offset: 0x0002408D
		private void ThrowIfInvalid()
		{
			if (this.InvalidCollectionInSharedContractMessage != null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(this.InvalidCollectionInSharedContractMessage));
			}
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x00025EA8 File Offset: 0x000240A8
		internal override DataContract GetValidContract()
		{
			if (this.IsConstructorCheckRequired)
			{
				this.CheckConstructor();
			}
			return this;
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x00025EB9 File Offset: 0x000240B9
		[SecuritySafeCritical]
		private void CheckConstructor()
		{
			if (this.Constructor == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("{0} does not have a default constructor.", new object[] { DataContract.GetClrTypeFullName(base.UnderlyingType) })));
			}
			this.IsConstructorCheckRequired = false;
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x00025EF9 File Offset: 0x000240F9
		internal override bool IsValidContract(SerializationMode mode)
		{
			if (mode == SerializationMode.SharedType)
			{
				return this.SharedTypeContract != null;
			}
			return this.InvalidCollectionInSharedContractMessage == null;
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x00025F14 File Offset: 0x00024114
		internal override bool Equals(object other, Dictionary<DataContractPairKey, object> checkedContracts)
		{
			if (base.IsEqualOrChecked(other, checkedContracts))
			{
				return true;
			}
			if (base.Equals(other, checkedContracts))
			{
				CollectionDataContract collectionDataContract = other as CollectionDataContract;
				if (collectionDataContract != null)
				{
					bool flag = this.ItemContract != null && !this.ItemContract.IsValueType;
					bool flag2 = collectionDataContract.ItemContract != null && !collectionDataContract.ItemContract.IsValueType;
					return this.ItemName == collectionDataContract.ItemName && (this.IsItemTypeNullable || flag) == (collectionDataContract.IsItemTypeNullable || flag2) && this.ItemContract.Equals(collectionDataContract.ItemContract, checkedContracts);
				}
			}
			return false;
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x00025FB0 File Offset: 0x000241B0
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x00025FB8 File Offset: 0x000241B8
		public override void WriteXmlValue(XmlWriterDelegator xmlWriter, object obj, XmlObjectSerializerWriteContext context)
		{
			context.IsGetOnlyCollection = false;
			this.XmlFormatWriterDelegate(xmlWriter, obj, context, this);
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x00025FD0 File Offset: 0x000241D0
		public override object ReadXmlValue(XmlReaderDelegator xmlReader, XmlObjectSerializerReadContext context)
		{
			xmlReader.Read();
			object obj = null;
			if (context.IsGetOnlyCollection)
			{
				context.IsGetOnlyCollection = false;
				this.XmlFormatGetOnlyCollectionReaderDelegate(xmlReader, context, this.CollectionItemName, this.Namespace, this);
			}
			else
			{
				obj = this.XmlFormatReaderDelegate(xmlReader, context, this.CollectionItemName, this.Namespace, this);
			}
			xmlReader.ReadEndElement();
			return obj;
		}

		// Token: 0x040002FA RID: 762
		[SecurityCritical]
		private XmlDictionaryString collectionItemName;

		// Token: 0x040002FB RID: 763
		[SecurityCritical]
		private XmlDictionaryString childElementNamespace;

		// Token: 0x040002FC RID: 764
		[SecurityCritical]
		private DataContract itemContract;

		// Token: 0x040002FD RID: 765
		[SecurityCritical]
		private CollectionDataContract.CollectionDataContractCriticalHelper helper;

		// Token: 0x0200016E RID: 366
		[SecurityCritical(SecurityCriticalScope.Everything)]
		private class CollectionDataContractCriticalHelper : DataContract.DataContractCriticalHelper
		{
			// Token: 0x1700040E RID: 1038
			// (get) Token: 0x0600141A RID: 5146 RVA: 0x00051C0C File Offset: 0x0004FE0C
			internal static Type[] KnownInterfaces
			{
				get
				{
					if (CollectionDataContract.CollectionDataContractCriticalHelper._knownInterfaces == null)
					{
						CollectionDataContract.CollectionDataContractCriticalHelper._knownInterfaces = new Type[]
						{
							Globals.TypeOfIDictionaryGeneric,
							Globals.TypeOfIDictionary,
							Globals.TypeOfIListGeneric,
							Globals.TypeOfICollectionGeneric,
							Globals.TypeOfIList,
							Globals.TypeOfIEnumerableGeneric,
							Globals.TypeOfICollection,
							Globals.TypeOfIEnumerable
						};
					}
					return CollectionDataContract.CollectionDataContractCriticalHelper._knownInterfaces;
				}
			}

			// Token: 0x0600141B RID: 5147 RVA: 0x00051C70 File Offset: 0x0004FE70
			private void Init(CollectionKind kind, Type itemType, CollectionDataContractAttribute collectionContractAttribute)
			{
				this.kind = kind;
				if (itemType != null)
				{
					this.itemType = itemType;
					this.isItemTypeNullable = DataContract.IsTypeNullable(itemType);
					bool flag = kind == CollectionKind.Dictionary || kind == CollectionKind.GenericDictionary;
					string text = null;
					string text2 = null;
					string text3 = null;
					if (collectionContractAttribute != null)
					{
						if (collectionContractAttribute.IsItemNameSetExplicitly)
						{
							if (collectionContractAttribute.ItemName == null || collectionContractAttribute.ItemName.Length == 0)
							{
								throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Type '{0}' cannot have CollectionDataContractAttribute attribute ItemName set to null or empty string.", new object[] { DataContract.GetClrTypeFullName(base.UnderlyingType) })));
							}
							text = DataContract.EncodeLocalName(collectionContractAttribute.ItemName);
							this.itemNameSetExplicit = true;
						}
						if (collectionContractAttribute.IsKeyNameSetExplicitly)
						{
							if (collectionContractAttribute.KeyName == null || collectionContractAttribute.KeyName.Length == 0)
							{
								throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Type '{0}' cannot have CollectionDataContractAttribute attribute KeyName set to null or empty string.", new object[] { DataContract.GetClrTypeFullName(base.UnderlyingType) })));
							}
							if (!flag)
							{
								throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("The collection data contract type '{0}' specifies '{1}' for the KeyName property. This is not allowed since the type is not IDictionary. Remove the setting for the KeyName property.", new object[]
								{
									DataContract.GetClrTypeFullName(base.UnderlyingType),
									collectionContractAttribute.KeyName
								})));
							}
							text2 = DataContract.EncodeLocalName(collectionContractAttribute.KeyName);
						}
						if (collectionContractAttribute.IsValueNameSetExplicitly)
						{
							if (collectionContractAttribute.ValueName == null || collectionContractAttribute.ValueName.Length == 0)
							{
								throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Type '{0}' cannot have CollectionDataContractAttribute attribute ValueName set to null or empty string.", new object[] { DataContract.GetClrTypeFullName(base.UnderlyingType) })));
							}
							if (!flag)
							{
								throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("The collection data contract type '{0}' specifies '{1}' for the ValueName property. This is not allowed since the type is not IDictionary. Remove the setting for the ValueName property.", new object[]
								{
									DataContract.GetClrTypeFullName(base.UnderlyingType),
									collectionContractAttribute.ValueName
								})));
							}
							text3 = DataContract.EncodeLocalName(collectionContractAttribute.ValueName);
						}
					}
					XmlDictionary xmlDictionary = (flag ? new XmlDictionary(5) : new XmlDictionary(3));
					base.Name = xmlDictionary.Add(base.StableName.Name);
					base.Namespace = xmlDictionary.Add(base.StableName.Namespace);
					this.itemName = text ?? DataContract.GetStableName(DataContract.UnwrapNullableType(itemType)).Name;
					this.collectionItemName = xmlDictionary.Add(this.itemName);
					if (flag)
					{
						this.keyName = text2 ?? "Key";
						this.valueName = text3 ?? "Value";
					}
				}
				if (collectionContractAttribute != null)
				{
					base.IsReference = collectionContractAttribute.IsReference;
				}
			}

			// Token: 0x0600141C RID: 5148 RVA: 0x00051EC5 File Offset: 0x000500C5
			internal CollectionDataContractCriticalHelper(CollectionKind kind)
			{
				this.Init(kind, null, null);
			}

			// Token: 0x0600141D RID: 5149 RVA: 0x00051ED8 File Offset: 0x000500D8
			internal CollectionDataContractCriticalHelper(Type type)
				: base(type)
			{
				if (type == Globals.TypeOfArray)
				{
					type = Globals.TypeOfObjectArray;
				}
				if (type.GetArrayRank() > 1)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("Multi-dimensional arrays are not supported.")));
				}
				base.StableName = DataContract.GetStableName(type);
				this.Init(CollectionKind.Array, type.GetElementType(), null);
			}

			// Token: 0x0600141E RID: 5150 RVA: 0x00051F3C File Offset: 0x0005013C
			internal CollectionDataContractCriticalHelper(Type type, DataContract itemContract)
				: base(type)
			{
				if (type.GetArrayRank() > 1)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new NotSupportedException(SR.GetString("Multi-dimensional arrays are not supported.")));
				}
				base.StableName = DataContract.CreateQualifiedName("ArrayOf" + itemContract.StableName.Name, itemContract.StableName.Namespace);
				this.itemContract = itemContract;
				this.Init(CollectionKind.Array, type.GetElementType(), null);
			}

			// Token: 0x0600141F RID: 5151 RVA: 0x00051FB0 File Offset: 0x000501B0
			internal CollectionDataContractCriticalHelper(Type type, CollectionKind kind, Type itemType, MethodInfo getEnumeratorMethod, string serializationExceptionMessage, string deserializationExceptionMessage)
				: base(type)
			{
				if (getEnumeratorMethod == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Collection type '{0}' does not have a valid GetEnumerator method.", new object[] { DataContract.GetClrTypeFullName(type) })));
				}
				if (itemType == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Collection type '{0}' must have a non-null item type.", new object[] { DataContract.GetClrTypeFullName(type) })));
				}
				CollectionDataContractAttribute collectionDataContractAttribute;
				base.StableName = DataContract.GetCollectionStableName(type, itemType, out collectionDataContractAttribute);
				this.Init(kind, itemType, collectionDataContractAttribute);
				this.getEnumeratorMethod = getEnumeratorMethod;
				this.serializationExceptionMessage = serializationExceptionMessage;
				this.deserializationExceptionMessage = deserializationExceptionMessage;
			}

			// Token: 0x06001420 RID: 5152 RVA: 0x00052050 File Offset: 0x00050250
			internal CollectionDataContractCriticalHelper(Type type, CollectionKind kind, Type itemType, MethodInfo getEnumeratorMethod, MethodInfo addMethod, ConstructorInfo constructor)
				: this(type, kind, itemType, getEnumeratorMethod, null, null)
			{
				if (addMethod == null && !type.IsInterface)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Collection type '{0}' does not have a valid Add method.", new object[] { DataContract.GetClrTypeFullName(type) })));
				}
				this.addMethod = addMethod;
				this.constructor = constructor;
			}

			// Token: 0x06001421 RID: 5153 RVA: 0x000520B0 File Offset: 0x000502B0
			internal CollectionDataContractCriticalHelper(Type type, CollectionKind kind, Type itemType, MethodInfo getEnumeratorMethod, MethodInfo addMethod, ConstructorInfo constructor, bool isConstructorCheckRequired)
				: this(type, kind, itemType, getEnumeratorMethod, addMethod, constructor)
			{
				this.isConstructorCheckRequired = isConstructorCheckRequired;
			}

			// Token: 0x06001422 RID: 5154 RVA: 0x000520C9 File Offset: 0x000502C9
			internal CollectionDataContractCriticalHelper(Type type, string invalidCollectionInSharedContractMessage)
				: base(type)
			{
				this.Init(CollectionKind.Collection, null, null);
				this.invalidCollectionInSharedContractMessage = invalidCollectionInSharedContractMessage;
			}

			// Token: 0x1700040F RID: 1039
			// (get) Token: 0x06001423 RID: 5155 RVA: 0x000520E2 File Offset: 0x000502E2
			internal CollectionKind Kind
			{
				get
				{
					return this.kind;
				}
			}

			// Token: 0x17000410 RID: 1040
			// (get) Token: 0x06001424 RID: 5156 RVA: 0x000520EA File Offset: 0x000502EA
			internal Type ItemType
			{
				get
				{
					return this.itemType;
				}
			}

			// Token: 0x17000411 RID: 1041
			// (get) Token: 0x06001425 RID: 5157 RVA: 0x000520F4 File Offset: 0x000502F4
			// (set) Token: 0x06001426 RID: 5158 RVA: 0x000521C1 File Offset: 0x000503C1
			internal DataContract ItemContract
			{
				get
				{
					if (this.itemContract == null && base.UnderlyingType != null)
					{
						if (this.IsDictionary)
						{
							if (string.CompareOrdinal(this.KeyName, this.ValueName) == 0)
							{
								DataContract.ThrowInvalidDataContractException(SR.GetString("The collection data contract type '{0}' specifies the same value '{1}' for both the KeyName and the ValueName properties. This is not allowed. Consider changing either the KeyName or the ValueName property.", new object[]
								{
									DataContract.GetClrTypeFullName(base.UnderlyingType),
									this.KeyName
								}), base.UnderlyingType);
							}
							this.itemContract = ClassDataContract.CreateClassDataContractForKeyValue(this.ItemType, base.Namespace, new string[] { this.KeyName, this.ValueName });
							DataContract.GetDataContract(this.ItemType);
						}
						else
						{
							this.itemContract = DataContract.GetDataContract(this.ItemType);
						}
					}
					return this.itemContract;
				}
				set
				{
					this.itemContract = value;
				}
			}

			// Token: 0x17000412 RID: 1042
			// (get) Token: 0x06001427 RID: 5159 RVA: 0x000521CA File Offset: 0x000503CA
			// (set) Token: 0x06001428 RID: 5160 RVA: 0x000521D2 File Offset: 0x000503D2
			internal DataContract SharedTypeContract
			{
				get
				{
					return this.sharedTypeContract;
				}
				set
				{
					this.sharedTypeContract = value;
				}
			}

			// Token: 0x17000413 RID: 1043
			// (get) Token: 0x06001429 RID: 5161 RVA: 0x000521DB File Offset: 0x000503DB
			// (set) Token: 0x0600142A RID: 5162 RVA: 0x000521E3 File Offset: 0x000503E3
			internal string ItemName
			{
				get
				{
					return this.itemName;
				}
				set
				{
					this.itemName = value;
				}
			}

			// Token: 0x17000414 RID: 1044
			// (get) Token: 0x0600142B RID: 5163 RVA: 0x000521EC File Offset: 0x000503EC
			// (set) Token: 0x0600142C RID: 5164 RVA: 0x000521F4 File Offset: 0x000503F4
			internal bool IsConstructorCheckRequired
			{
				get
				{
					return this.isConstructorCheckRequired;
				}
				set
				{
					this.isConstructorCheckRequired = value;
				}
			}

			// Token: 0x17000415 RID: 1045
			// (get) Token: 0x0600142D RID: 5165 RVA: 0x000521FD File Offset: 0x000503FD
			public XmlDictionaryString CollectionItemName
			{
				get
				{
					return this.collectionItemName;
				}
			}

			// Token: 0x17000416 RID: 1046
			// (get) Token: 0x0600142E RID: 5166 RVA: 0x00052205 File Offset: 0x00050405
			// (set) Token: 0x0600142F RID: 5167 RVA: 0x0005220D File Offset: 0x0005040D
			internal string KeyName
			{
				get
				{
					return this.keyName;
				}
				set
				{
					this.keyName = value;
				}
			}

			// Token: 0x17000417 RID: 1047
			// (get) Token: 0x06001430 RID: 5168 RVA: 0x00052216 File Offset: 0x00050416
			// (set) Token: 0x06001431 RID: 5169 RVA: 0x0005221E File Offset: 0x0005041E
			internal string ValueName
			{
				get
				{
					return this.valueName;
				}
				set
				{
					this.valueName = value;
				}
			}

			// Token: 0x17000418 RID: 1048
			// (get) Token: 0x06001432 RID: 5170 RVA: 0x00052227 File Offset: 0x00050427
			internal bool IsDictionary
			{
				get
				{
					return this.KeyName != null;
				}
			}

			// Token: 0x17000419 RID: 1049
			// (get) Token: 0x06001433 RID: 5171 RVA: 0x00052232 File Offset: 0x00050432
			public string SerializationExceptionMessage
			{
				get
				{
					return this.serializationExceptionMessage;
				}
			}

			// Token: 0x1700041A RID: 1050
			// (get) Token: 0x06001434 RID: 5172 RVA: 0x0005223A File Offset: 0x0005043A
			public string DeserializationExceptionMessage
			{
				get
				{
					return this.deserializationExceptionMessage;
				}
			}

			// Token: 0x1700041B RID: 1051
			// (get) Token: 0x06001435 RID: 5173 RVA: 0x00052242 File Offset: 0x00050442
			// (set) Token: 0x06001436 RID: 5174 RVA: 0x0005224A File Offset: 0x0005044A
			public XmlDictionaryString ChildElementNamespace
			{
				get
				{
					return this.childElementNamespace;
				}
				set
				{
					this.childElementNamespace = value;
				}
			}

			// Token: 0x1700041C RID: 1052
			// (get) Token: 0x06001437 RID: 5175 RVA: 0x00052253 File Offset: 0x00050453
			// (set) Token: 0x06001438 RID: 5176 RVA: 0x0005225B File Offset: 0x0005045B
			internal bool IsItemTypeNullable
			{
				get
				{
					return this.isItemTypeNullable;
				}
				set
				{
					this.isItemTypeNullable = value;
				}
			}

			// Token: 0x1700041D RID: 1053
			// (get) Token: 0x06001439 RID: 5177 RVA: 0x00052264 File Offset: 0x00050464
			internal MethodInfo GetEnumeratorMethod
			{
				get
				{
					return this.getEnumeratorMethod;
				}
			}

			// Token: 0x1700041E RID: 1054
			// (get) Token: 0x0600143A RID: 5178 RVA: 0x0005226C File Offset: 0x0005046C
			internal MethodInfo AddMethod
			{
				get
				{
					return this.addMethod;
				}
			}

			// Token: 0x1700041F RID: 1055
			// (get) Token: 0x0600143B RID: 5179 RVA: 0x00052274 File Offset: 0x00050474
			internal ConstructorInfo Constructor
			{
				get
				{
					return this.constructor;
				}
			}

			// Token: 0x17000420 RID: 1056
			// (get) Token: 0x0600143C RID: 5180 RVA: 0x0005227C File Offset: 0x0005047C
			// (set) Token: 0x0600143D RID: 5181 RVA: 0x000522F4 File Offset: 0x000504F4
			internal override Dictionary<XmlQualifiedName, DataContract> KnownDataContracts
			{
				get
				{
					if (!this.isKnownTypeAttributeChecked && base.UnderlyingType != null)
					{
						lock (this)
						{
							if (!this.isKnownTypeAttributeChecked)
							{
								this.knownDataContracts = DataContract.ImportKnownTypeAttributes(base.UnderlyingType);
								Thread.MemoryBarrier();
								this.isKnownTypeAttributeChecked = true;
							}
						}
					}
					return this.knownDataContracts;
				}
				set
				{
					this.knownDataContracts = value;
				}
			}

			// Token: 0x17000421 RID: 1057
			// (get) Token: 0x0600143E RID: 5182 RVA: 0x000522FD File Offset: 0x000504FD
			internal string InvalidCollectionInSharedContractMessage
			{
				get
				{
					return this.invalidCollectionInSharedContractMessage;
				}
			}

			// Token: 0x17000422 RID: 1058
			// (get) Token: 0x0600143F RID: 5183 RVA: 0x00052305 File Offset: 0x00050505
			internal bool ItemNameSetExplicit
			{
				get
				{
					return this.itemNameSetExplicit;
				}
			}

			// Token: 0x17000423 RID: 1059
			// (get) Token: 0x06001440 RID: 5184 RVA: 0x0005230D File Offset: 0x0005050D
			// (set) Token: 0x06001441 RID: 5185 RVA: 0x00052315 File Offset: 0x00050515
			internal XmlFormatCollectionWriterDelegate XmlFormatWriterDelegate
			{
				get
				{
					return this.xmlFormatWriterDelegate;
				}
				set
				{
					this.xmlFormatWriterDelegate = value;
				}
			}

			// Token: 0x17000424 RID: 1060
			// (get) Token: 0x06001442 RID: 5186 RVA: 0x0005231E File Offset: 0x0005051E
			// (set) Token: 0x06001443 RID: 5187 RVA: 0x00052326 File Offset: 0x00050526
			internal XmlFormatCollectionReaderDelegate XmlFormatReaderDelegate
			{
				get
				{
					return this.xmlFormatReaderDelegate;
				}
				set
				{
					this.xmlFormatReaderDelegate = value;
				}
			}

			// Token: 0x17000425 RID: 1061
			// (get) Token: 0x06001444 RID: 5188 RVA: 0x0005232F File Offset: 0x0005052F
			// (set) Token: 0x06001445 RID: 5189 RVA: 0x00052337 File Offset: 0x00050537
			internal XmlFormatGetOnlyCollectionReaderDelegate XmlFormatGetOnlyCollectionReaderDelegate
			{
				get
				{
					return this.xmlFormatGetOnlyCollectionReaderDelegate;
				}
				set
				{
					this.xmlFormatGetOnlyCollectionReaderDelegate = value;
				}
			}

			// Token: 0x040009D1 RID: 2513
			private static Type[] _knownInterfaces;

			// Token: 0x040009D2 RID: 2514
			private Type itemType;

			// Token: 0x040009D3 RID: 2515
			private bool isItemTypeNullable;

			// Token: 0x040009D4 RID: 2516
			private CollectionKind kind;

			// Token: 0x040009D5 RID: 2517
			private readonly MethodInfo getEnumeratorMethod;

			// Token: 0x040009D6 RID: 2518
			private readonly MethodInfo addMethod;

			// Token: 0x040009D7 RID: 2519
			private readonly ConstructorInfo constructor;

			// Token: 0x040009D8 RID: 2520
			private readonly string serializationExceptionMessage;

			// Token: 0x040009D9 RID: 2521
			private readonly string deserializationExceptionMessage;

			// Token: 0x040009DA RID: 2522
			private DataContract itemContract;

			// Token: 0x040009DB RID: 2523
			private DataContract sharedTypeContract;

			// Token: 0x040009DC RID: 2524
			private Dictionary<XmlQualifiedName, DataContract> knownDataContracts;

			// Token: 0x040009DD RID: 2525
			private bool isKnownTypeAttributeChecked;

			// Token: 0x040009DE RID: 2526
			private string itemName;

			// Token: 0x040009DF RID: 2527
			private bool itemNameSetExplicit;

			// Token: 0x040009E0 RID: 2528
			private XmlDictionaryString collectionItemName;

			// Token: 0x040009E1 RID: 2529
			private string keyName;

			// Token: 0x040009E2 RID: 2530
			private string valueName;

			// Token: 0x040009E3 RID: 2531
			private XmlDictionaryString childElementNamespace;

			// Token: 0x040009E4 RID: 2532
			private string invalidCollectionInSharedContractMessage;

			// Token: 0x040009E5 RID: 2533
			private XmlFormatCollectionReaderDelegate xmlFormatReaderDelegate;

			// Token: 0x040009E6 RID: 2534
			private XmlFormatGetOnlyCollectionReaderDelegate xmlFormatGetOnlyCollectionReaderDelegate;

			// Token: 0x040009E7 RID: 2535
			private XmlFormatCollectionWriterDelegate xmlFormatWriterDelegate;

			// Token: 0x040009E8 RID: 2536
			private bool isConstructorCheckRequired;
		}

		// Token: 0x0200016F RID: 367
		public class DictionaryEnumerator : IEnumerator<KeyValue<object, object>>, IDisposable, IEnumerator
		{
			// Token: 0x06001446 RID: 5190 RVA: 0x00052340 File Offset: 0x00050540
			public DictionaryEnumerator(IDictionaryEnumerator enumerator)
			{
				this.enumerator = enumerator;
			}

			// Token: 0x06001447 RID: 5191 RVA: 0x0005234F File Offset: 0x0005054F
			public void Dispose()
			{
			}

			// Token: 0x06001448 RID: 5192 RVA: 0x00052351 File Offset: 0x00050551
			public bool MoveNext()
			{
				return this.enumerator.MoveNext();
			}

			// Token: 0x17000426 RID: 1062
			// (get) Token: 0x06001449 RID: 5193 RVA: 0x0005235E File Offset: 0x0005055E
			public KeyValue<object, object> Current
			{
				get
				{
					return new KeyValue<object, object>(this.enumerator.Key, this.enumerator.Value);
				}
			}

			// Token: 0x17000427 RID: 1063
			// (get) Token: 0x0600144A RID: 5194 RVA: 0x0005237B File Offset: 0x0005057B
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x0600144B RID: 5195 RVA: 0x00052388 File Offset: 0x00050588
			public void Reset()
			{
				this.enumerator.Reset();
			}

			// Token: 0x040009E9 RID: 2537
			private IDictionaryEnumerator enumerator;
		}

		// Token: 0x02000170 RID: 368
		public class GenericDictionaryEnumerator<K, V> : IEnumerator<KeyValue<K, V>>, IDisposable, IEnumerator
		{
			// Token: 0x0600144C RID: 5196 RVA: 0x00052395 File Offset: 0x00050595
			public GenericDictionaryEnumerator(IEnumerator<KeyValuePair<K, V>> enumerator)
			{
				this.enumerator = enumerator;
			}

			// Token: 0x0600144D RID: 5197 RVA: 0x000523A4 File Offset: 0x000505A4
			public void Dispose()
			{
			}

			// Token: 0x0600144E RID: 5198 RVA: 0x000523A6 File Offset: 0x000505A6
			public bool MoveNext()
			{
				return this.enumerator.MoveNext();
			}

			// Token: 0x17000428 RID: 1064
			// (get) Token: 0x0600144F RID: 5199 RVA: 0x000523B4 File Offset: 0x000505B4
			public KeyValue<K, V> Current
			{
				get
				{
					KeyValuePair<K, V> keyValuePair = this.enumerator.Current;
					return new KeyValue<K, V>(keyValuePair.Key, keyValuePair.Value);
				}
			}

			// Token: 0x17000429 RID: 1065
			// (get) Token: 0x06001450 RID: 5200 RVA: 0x000523E0 File Offset: 0x000505E0
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x06001451 RID: 5201 RVA: 0x000523ED File Offset: 0x000505ED
			public void Reset()
			{
				this.enumerator.Reset();
			}

			// Token: 0x040009EA RID: 2538
			private IEnumerator<KeyValuePair<K, V>> enumerator;
		}
	}
}
