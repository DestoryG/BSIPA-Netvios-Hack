using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Configuration;
using System.Runtime.Serialization.Diagnostics.Application;
using System.Security;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace System.Runtime.Serialization
{
	// Token: 0x0200006D RID: 109
	internal abstract class DataContract
	{
		// Token: 0x06000800 RID: 2048 RVA: 0x000261D4 File Offset: 0x000243D4
		[SecuritySafeCritical]
		protected DataContract(DataContract.DataContractCriticalHelper helper)
		{
			this.helper = helper;
			this.name = helper.Name;
			this.ns = helper.Namespace;
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x000261FB File Offset: 0x000243FB
		internal static DataContract GetDataContract(Type type)
		{
			return DataContract.GetDataContract(type.TypeHandle, type, SerializationMode.SharedContract);
		}

		// Token: 0x06000802 RID: 2050 RVA: 0x0002620A File Offset: 0x0002440A
		internal static DataContract GetDataContract(RuntimeTypeHandle typeHandle, Type type, SerializationMode mode)
		{
			return DataContract.GetDataContract(DataContract.GetId(typeHandle), typeHandle, mode);
		}

		// Token: 0x06000803 RID: 2051 RVA: 0x00026219 File Offset: 0x00024419
		internal static DataContract GetDataContract(int id, RuntimeTypeHandle typeHandle, SerializationMode mode)
		{
			return DataContract.GetDataContractSkipValidation(id, typeHandle, null).GetValidContract(mode);
		}

		// Token: 0x06000804 RID: 2052 RVA: 0x00026229 File Offset: 0x00024429
		[SecuritySafeCritical]
		internal static DataContract GetDataContractSkipValidation(int id, RuntimeTypeHandle typeHandle, Type type)
		{
			return DataContract.DataContractCriticalHelper.GetDataContractSkipValidation(id, typeHandle, type);
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x00026234 File Offset: 0x00024434
		internal static DataContract GetGetOnlyCollectionDataContract(int id, RuntimeTypeHandle typeHandle, Type type, SerializationMode mode)
		{
			DataContract dataContract = DataContract.GetGetOnlyCollectionDataContractSkipValidation(id, typeHandle, type);
			dataContract = dataContract.GetValidContract(mode);
			if (dataContract is ClassDataContract)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new SerializationException(SR.GetString("For '{0}' type, class data contract was returned for get-only collection.", new object[] { DataContract.GetClrTypeFullName(dataContract.UnderlyingType) })));
			}
			return dataContract;
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00026284 File Offset: 0x00024484
		[SecuritySafeCritical]
		internal static DataContract GetGetOnlyCollectionDataContractSkipValidation(int id, RuntimeTypeHandle typeHandle, Type type)
		{
			return DataContract.DataContractCriticalHelper.GetGetOnlyCollectionDataContractSkipValidation(id, typeHandle, type);
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x0002628E File Offset: 0x0002448E
		[SecuritySafeCritical]
		internal static DataContract GetDataContractForInitialization(int id)
		{
			return DataContract.DataContractCriticalHelper.GetDataContractForInitialization(id);
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x00026296 File Offset: 0x00024496
		[SecuritySafeCritical]
		internal static int GetIdForInitialization(ClassDataContract classContract)
		{
			return DataContract.DataContractCriticalHelper.GetIdForInitialization(classContract);
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x0002629E File Offset: 0x0002449E
		[SecuritySafeCritical]
		internal static int GetId(RuntimeTypeHandle typeHandle)
		{
			return DataContract.DataContractCriticalHelper.GetId(typeHandle);
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x000262A6 File Offset: 0x000244A6
		[SecuritySafeCritical]
		public static DataContract GetBuiltInDataContract(Type type)
		{
			return DataContract.DataContractCriticalHelper.GetBuiltInDataContract(type);
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x000262AE File Offset: 0x000244AE
		[SecuritySafeCritical]
		public static DataContract GetBuiltInDataContract(string name, string ns)
		{
			return DataContract.DataContractCriticalHelper.GetBuiltInDataContract(name, ns);
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x000262B7 File Offset: 0x000244B7
		[SecuritySafeCritical]
		public static DataContract GetBuiltInDataContract(string typeName)
		{
			return DataContract.DataContractCriticalHelper.GetBuiltInDataContract(typeName);
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x000262BF File Offset: 0x000244BF
		[SecuritySafeCritical]
		internal static string GetNamespace(string key)
		{
			return DataContract.DataContractCriticalHelper.GetNamespace(key);
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x000262C7 File Offset: 0x000244C7
		[SecuritySafeCritical]
		internal static XmlDictionaryString GetClrTypeString(string key)
		{
			return DataContract.DataContractCriticalHelper.GetClrTypeString(key);
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x000262CF File Offset: 0x000244CF
		[SecuritySafeCritical]
		internal static void ThrowInvalidDataContractException(string message, Type type)
		{
			DataContract.DataContractCriticalHelper.ThrowInvalidDataContractException(message, type);
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000810 RID: 2064 RVA: 0x000262D8 File Offset: 0x000244D8
		protected DataContract.DataContractCriticalHelper Helper
		{
			[SecurityCritical]
			get
			{
				return this.helper;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000811 RID: 2065 RVA: 0x000262E0 File Offset: 0x000244E0
		internal Type UnderlyingType
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.UnderlyingType;
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000812 RID: 2066 RVA: 0x000262ED File Offset: 0x000244ED
		internal Type OriginalUnderlyingType
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.OriginalUnderlyingType;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000813 RID: 2067 RVA: 0x000262FA File Offset: 0x000244FA
		internal virtual bool IsBuiltInDataContract
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.IsBuiltInDataContract;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000814 RID: 2068 RVA: 0x00026307 File Offset: 0x00024507
		internal Type TypeForInitialization
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.TypeForInitialization;
			}
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x00026314 File Offset: 0x00024514
		public virtual void WriteXmlValue(XmlWriterDelegator xmlWriter, object obj, XmlObjectSerializerWriteContext context)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("An internal error has occurred. Unexpected contract type '{0}' for type '{1}' encountered.", new object[]
			{
				DataContract.GetClrTypeFullName(base.GetType()),
				DataContract.GetClrTypeFullName(this.UnderlyingType)
			})));
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x0002634C File Offset: 0x0002454C
		public virtual object ReadXmlValue(XmlReaderDelegator xmlReader, XmlObjectSerializerReadContext context)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("An internal error has occurred. Unexpected contract type '{0}' for type '{1}' encountered.", new object[]
			{
				DataContract.GetClrTypeFullName(base.GetType()),
				DataContract.GetClrTypeFullName(this.UnderlyingType)
			})));
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000817 RID: 2071 RVA: 0x00026384 File Offset: 0x00024584
		// (set) Token: 0x06000818 RID: 2072 RVA: 0x00026391 File Offset: 0x00024591
		internal bool IsValueType
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.IsValueType;
			}
			[SecurityCritical]
			set
			{
				this.helper.IsValueType = value;
			}
		}

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000819 RID: 2073 RVA: 0x0002639F File Offset: 0x0002459F
		// (set) Token: 0x0600081A RID: 2074 RVA: 0x000263AC File Offset: 0x000245AC
		internal bool IsReference
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.IsReference;
			}
			[SecurityCritical]
			set
			{
				this.helper.IsReference = value;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x0600081B RID: 2075 RVA: 0x000263BA File Offset: 0x000245BA
		// (set) Token: 0x0600081C RID: 2076 RVA: 0x000263C7 File Offset: 0x000245C7
		internal XmlQualifiedName StableName
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.StableName;
			}
			[SecurityCritical]
			set
			{
				this.helper.StableName = value;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x0600081D RID: 2077 RVA: 0x000263D5 File Offset: 0x000245D5
		// (set) Token: 0x0600081E RID: 2078 RVA: 0x000263E2 File Offset: 0x000245E2
		internal GenericInfo GenericInfo
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.GenericInfo;
			}
			[SecurityCritical]
			set
			{
				this.helper.GenericInfo = value;
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x0600081F RID: 2079 RVA: 0x000263F0 File Offset: 0x000245F0
		// (set) Token: 0x06000820 RID: 2080 RVA: 0x000263FD File Offset: 0x000245FD
		internal virtual Dictionary<XmlQualifiedName, DataContract> KnownDataContracts
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

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000821 RID: 2081 RVA: 0x0002640B File Offset: 0x0002460B
		// (set) Token: 0x06000822 RID: 2082 RVA: 0x00026418 File Offset: 0x00024618
		internal virtual bool IsISerializable
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.IsISerializable;
			}
			[SecurityCritical]
			set
			{
				this.helper.IsISerializable = value;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000823 RID: 2083 RVA: 0x00026426 File Offset: 0x00024626
		internal XmlDictionaryString Name
		{
			[SecuritySafeCritical]
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000824 RID: 2084 RVA: 0x0002642E File Offset: 0x0002462E
		public virtual XmlDictionaryString Namespace
		{
			[SecuritySafeCritical]
			get
			{
				return this.ns;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000825 RID: 2085 RVA: 0x00026436 File Offset: 0x00024636
		// (set) Token: 0x06000826 RID: 2086 RVA: 0x00026439 File Offset: 0x00024639
		internal virtual bool HasRoot
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000827 RID: 2087 RVA: 0x0002643B File Offset: 0x0002463B
		// (set) Token: 0x06000828 RID: 2088 RVA: 0x00026448 File Offset: 0x00024648
		internal virtual XmlDictionaryString TopLevelElementName
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.TopLevelElementName;
			}
			[SecurityCritical]
			set
			{
				this.helper.TopLevelElementName = value;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000829 RID: 2089 RVA: 0x00026456 File Offset: 0x00024656
		// (set) Token: 0x0600082A RID: 2090 RVA: 0x00026463 File Offset: 0x00024663
		internal virtual XmlDictionaryString TopLevelElementNamespace
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.TopLevelElementNamespace;
			}
			[SecurityCritical]
			set
			{
				this.helper.TopLevelElementNamespace = value;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600082B RID: 2091 RVA: 0x00026471 File Offset: 0x00024671
		internal virtual bool CanContainReferences
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x0600082C RID: 2092 RVA: 0x00026474 File Offset: 0x00024674
		internal virtual bool IsPrimitive
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x00026477 File Offset: 0x00024677
		internal virtual void WriteRootElement(XmlWriterDelegator writer, XmlDictionaryString name, XmlDictionaryString ns)
		{
			if (ns == DictionaryGlobals.SerializationNamespace && !this.IsPrimitive)
			{
				writer.WriteStartElement("z", name, ns);
				return;
			}
			writer.WriteStartElement(name, ns);
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x0002649F File Offset: 0x0002469F
		internal virtual DataContract BindGenericParameters(DataContract[] paramContracts, Dictionary<DataContract, DataContract> boundContracts)
		{
			return this;
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x000264A2 File Offset: 0x000246A2
		internal virtual DataContract GetValidContract(SerializationMode mode)
		{
			return this;
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x000264A5 File Offset: 0x000246A5
		internal virtual DataContract GetValidContract()
		{
			return this;
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x000264A8 File Offset: 0x000246A8
		internal virtual bool IsValidContract(SerializationMode mode)
		{
			return true;
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000832 RID: 2098 RVA: 0x000264AB File Offset: 0x000246AB
		internal MethodInfo ParseMethod
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.ParseMethod;
			}
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x000264B8 File Offset: 0x000246B8
		internal static bool IsTypeSerializable(Type type)
		{
			return DataContract.IsTypeSerializable(type, new Dictionary<Type, object>());
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x000264C8 File Offset: 0x000246C8
		private static bool IsTypeSerializable(Type type, Dictionary<Type, object> previousCollectionTypes)
		{
			if (type.IsSerializable || type.IsDefined(Globals.TypeOfDataContractAttribute, false) || type.IsInterface || type.IsPointer || Globals.TypeOfIXmlSerializable.IsAssignableFrom(type))
			{
				return true;
			}
			Type type2;
			if (CollectionDataContract.IsCollection(type, out type2))
			{
				DataContract.ValidatePreviousCollectionTypes(type, type2, previousCollectionTypes);
				if (DataContract.IsTypeSerializable(type2, previousCollectionTypes))
				{
					return true;
				}
			}
			return DataContract.GetBuiltInDataContract(type) != null || ClassDataContract.IsNonAttributedTypeValidForSerialization(type);
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x00026538 File Offset: 0x00024738
		private static void ValidatePreviousCollectionTypes(Type collectionType, Type itemType, Dictionary<Type, object> previousCollectionTypes)
		{
			previousCollectionTypes.Add(collectionType, collectionType);
			while (itemType.IsArray)
			{
				itemType = itemType.GetElementType();
			}
			List<Type> list = new List<Type>();
			Queue<Type> queue = new Queue<Type>();
			queue.Enqueue(itemType);
			list.Add(itemType);
			while (queue.Count > 0)
			{
				itemType = queue.Dequeue();
				if (previousCollectionTypes.ContainsKey(itemType))
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Type '{0}' involves recursive collection.", new object[] { DataContract.GetClrTypeFullName(itemType) })));
				}
				if (itemType.IsGenericType)
				{
					foreach (Type type in itemType.GetGenericArguments())
					{
						if (!list.Contains(type))
						{
							queue.Enqueue(type);
							list.Add(type);
						}
					}
				}
			}
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x000265F8 File Offset: 0x000247F8
		internal static Type UnwrapRedundantNullableType(Type type)
		{
			Type type2 = type;
			while (type.IsGenericType && type.GetGenericTypeDefinition() == Globals.TypeOfNullable)
			{
				type2 = type;
				type = type.GetGenericArguments()[0];
			}
			return type2;
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x00026630 File Offset: 0x00024830
		internal static Type UnwrapNullableType(Type type)
		{
			while (type.IsGenericType && type.GetGenericTypeDefinition() == Globals.TypeOfNullable)
			{
				type = type.GetGenericArguments()[0];
			}
			return type;
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x00026659 File Offset: 0x00024859
		private static bool IsAlpha(char ch)
		{
			return (ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z');
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x00026676 File Offset: 0x00024876
		private static bool IsDigit(char ch)
		{
			return ch >= '0' && ch <= '9';
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x00026688 File Offset: 0x00024888
		private static bool IsAsciiLocalName(string localName)
		{
			if (localName.Length == 0)
			{
				return false;
			}
			if (!DataContract.IsAlpha(localName[0]))
			{
				return false;
			}
			for (int i = 1; i < localName.Length; i++)
			{
				char c = localName[i];
				if (!DataContract.IsAlpha(c) && !DataContract.IsDigit(c))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x000266DB File Offset: 0x000248DB
		internal static string EncodeLocalName(string localName)
		{
			if (DataContract.IsAsciiLocalName(localName))
			{
				return localName;
			}
			if (DataContract.IsValidNCName(localName))
			{
				return localName;
			}
			return XmlConvert.EncodeLocalName(localName);
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x000266F8 File Offset: 0x000248F8
		internal static bool IsValidNCName(string name)
		{
			bool flag;
			try
			{
				XmlConvert.VerifyNCName(name);
				flag = true;
			}
			catch (XmlException)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x00026728 File Offset: 0x00024928
		internal static XmlQualifiedName GetStableName(Type type)
		{
			bool flag;
			return DataContract.GetStableName(type, out flag);
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x0002673D File Offset: 0x0002493D
		internal static XmlQualifiedName GetStableName(Type type, out bool hasDataContract)
		{
			return DataContract.GetStableName(type, new Dictionary<Type, object>(), out hasDataContract);
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x0002674C File Offset: 0x0002494C
		private static XmlQualifiedName GetStableName(Type type, Dictionary<Type, object> previousCollectionTypes, out bool hasDataContract)
		{
			type = DataContract.UnwrapRedundantNullableType(type);
			XmlQualifiedName xmlQualifiedName;
			DataContractAttribute dataContractAttribute;
			if (DataContract.TryGetBuiltInXmlAndArrayTypeStableName(type, previousCollectionTypes, out xmlQualifiedName))
			{
				hasDataContract = false;
			}
			else if (DataContract.TryGetDCAttribute(type, out dataContractAttribute))
			{
				xmlQualifiedName = DataContract.GetDCTypeStableName(type, dataContractAttribute);
				hasDataContract = true;
			}
			else
			{
				xmlQualifiedName = DataContract.GetNonDCTypeStableName(type, previousCollectionTypes);
				hasDataContract = false;
			}
			return xmlQualifiedName;
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x00026794 File Offset: 0x00024994
		private static XmlQualifiedName GetDCTypeStableName(Type type, DataContractAttribute dataContractAttribute)
		{
			string text;
			if (dataContractAttribute.IsNameSetExplicitly)
			{
				text = dataContractAttribute.Name;
				if (text == null || text.Length == 0)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Type '{0}' cannot have DataContractAttribute attribute Name set to null or empty string.", new object[] { DataContract.GetClrTypeFullName(type) })));
				}
				if (type.IsGenericType && !type.IsGenericTypeDefinition)
				{
					text = DataContract.ExpandGenericParameters(text, type);
				}
				text = DataContract.EncodeLocalName(text);
			}
			else
			{
				text = DataContract.GetDefaultStableLocalName(type);
			}
			string text2;
			if (dataContractAttribute.IsNamespaceSetExplicitly)
			{
				text2 = dataContractAttribute.Namespace;
				if (text2 == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Type '{0}' cannot have DataContractAttribute attribute Namespace set to null.", new object[] { DataContract.GetClrTypeFullName(type) })));
				}
				DataContract.CheckExplicitDataContractNamespaceUri(text2, type);
			}
			else
			{
				text2 = DataContract.GetDefaultDataContractNamespace(type);
			}
			return DataContract.CreateQualifiedName(text, text2);
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x00026858 File Offset: 0x00024A58
		private static XmlQualifiedName GetNonDCTypeStableName(Type type, Dictionary<Type, object> previousCollectionTypes)
		{
			Type type2;
			if (CollectionDataContract.IsCollection(type, out type2))
			{
				DataContract.ValidatePreviousCollectionTypes(type, type2, previousCollectionTypes);
				CollectionDataContractAttribute collectionDataContractAttribute;
				return DataContract.GetCollectionStableName(type, type2, previousCollectionTypes, out collectionDataContractAttribute);
			}
			string defaultStableLocalName = DataContract.GetDefaultStableLocalName(type);
			string text;
			if (ClassDataContract.IsNonAttributedTypeValidForSerialization(type))
			{
				text = DataContract.GetDefaultDataContractNamespace(type);
			}
			else
			{
				text = DataContract.GetDefaultStableNamespace(type);
			}
			return DataContract.CreateQualifiedName(defaultStableLocalName, text);
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x000268A8 File Offset: 0x00024AA8
		private static bool TryGetBuiltInXmlAndArrayTypeStableName(Type type, Dictionary<Type, object> previousCollectionTypes, out XmlQualifiedName stableName)
		{
			stableName = null;
			DataContract builtInDataContract = DataContract.GetBuiltInDataContract(type);
			if (builtInDataContract != null)
			{
				stableName = builtInDataContract.StableName;
			}
			else if (Globals.TypeOfIXmlSerializable.IsAssignableFrom(type))
			{
				XmlQualifiedName xmlQualifiedName;
				XmlSchemaType xmlSchemaType;
				bool flag;
				SchemaExporter.GetXmlTypeInfo(type, out xmlQualifiedName, out xmlSchemaType, out flag);
				stableName = xmlQualifiedName;
			}
			else if (type.IsArray)
			{
				Type elementType = type.GetElementType();
				DataContract.ValidatePreviousCollectionTypes(type, elementType, previousCollectionTypes);
				CollectionDataContractAttribute collectionDataContractAttribute;
				stableName = DataContract.GetCollectionStableName(type, elementType, previousCollectionTypes, out collectionDataContractAttribute);
			}
			return stableName != null;
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x00026918 File Offset: 0x00024B18
		[SecuritySafeCritical]
		internal static bool TryGetDCAttribute(Type type, out DataContractAttribute dataContractAttribute)
		{
			dataContractAttribute = null;
			object[] customAttributes = type.GetCustomAttributes(Globals.TypeOfDataContractAttribute, false);
			if (customAttributes != null && customAttributes.Length != 0)
			{
				dataContractAttribute = (DataContractAttribute)customAttributes[0];
			}
			return dataContractAttribute != null;
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x0002694B File Offset: 0x00024B4B
		internal static XmlQualifiedName GetCollectionStableName(Type type, Type itemType, out CollectionDataContractAttribute collectionContractAttribute)
		{
			return DataContract.GetCollectionStableName(type, itemType, new Dictionary<Type, object>(), out collectionContractAttribute);
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0002695C File Offset: 0x00024B5C
		private static XmlQualifiedName GetCollectionStableName(Type type, Type itemType, Dictionary<Type, object> previousCollectionTypes, out CollectionDataContractAttribute collectionContractAttribute)
		{
			object[] customAttributes = type.GetCustomAttributes(Globals.TypeOfCollectionDataContractAttribute, false);
			string text;
			string text2;
			if (customAttributes != null && customAttributes.Length != 0)
			{
				collectionContractAttribute = (CollectionDataContractAttribute)customAttributes[0];
				if (collectionContractAttribute.IsNameSetExplicitly)
				{
					text = collectionContractAttribute.Name;
					if (text == null || text.Length == 0)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Type '{0}' cannot have CollectionDataContractAttribute attribute Name set to null or empty string.", new object[] { DataContract.GetClrTypeFullName(type) })));
					}
					if (type.IsGenericType && !type.IsGenericTypeDefinition)
					{
						text = DataContract.ExpandGenericParameters(text, type);
					}
					text = DataContract.EncodeLocalName(text);
				}
				else
				{
					text = DataContract.GetDefaultStableLocalName(type);
				}
				if (collectionContractAttribute.IsNamespaceSetExplicitly)
				{
					text2 = collectionContractAttribute.Namespace;
					if (text2 == null)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Type '{0}' cannot have CollectionDataContractAttribute attribute Namespace set to null.", new object[] { DataContract.GetClrTypeFullName(type) })));
					}
					DataContract.CheckExplicitDataContractNamespaceUri(text2, type);
				}
				else
				{
					text2 = DataContract.GetDefaultDataContractNamespace(type);
				}
			}
			else
			{
				collectionContractAttribute = null;
				string text3 = "ArrayOf" + DataContract.GetArrayPrefix(ref itemType);
				bool flag;
				XmlQualifiedName stableName = DataContract.GetStableName(itemType, previousCollectionTypes, out flag);
				text = text3 + stableName.Name;
				text2 = DataContract.GetCollectionNamespace(stableName.Namespace);
			}
			return DataContract.CreateQualifiedName(text, text2);
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x00026A80 File Offset: 0x00024C80
		private static string GetArrayPrefix(ref Type itemType)
		{
			string text = string.Empty;
			while (itemType.IsArray && DataContract.GetBuiltInDataContract(itemType) == null)
			{
				text += "ArrayOf";
				itemType = itemType.GetElementType();
			}
			return text;
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x00026AC0 File Offset: 0x00024CC0
		internal XmlQualifiedName GetArrayTypeName(bool isNullable)
		{
			XmlQualifiedName xmlQualifiedName;
			if (this.IsValueType && isNullable)
			{
				GenericInfo genericInfo = new GenericInfo(DataContract.GetStableName(Globals.TypeOfNullable), Globals.TypeOfNullable.FullName);
				genericInfo.Add(new GenericInfo(this.StableName, null));
				genericInfo.AddToLevel(0, 1);
				xmlQualifiedName = genericInfo.GetExpandedStableName();
			}
			else
			{
				xmlQualifiedName = this.StableName;
			}
			string collectionNamespace = DataContract.GetCollectionNamespace(xmlQualifiedName.Namespace);
			return new XmlQualifiedName("ArrayOf" + xmlQualifiedName.Name, collectionNamespace);
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x00026B3B File Offset: 0x00024D3B
		internal static string GetCollectionNamespace(string elementNs)
		{
			if (!DataContract.IsBuiltInNamespace(elementNs))
			{
				return elementNs;
			}
			return "http://schemas.microsoft.com/2003/10/Serialization/Arrays";
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x00026B4C File Offset: 0x00024D4C
		internal static XmlQualifiedName GetDefaultStableName(Type type)
		{
			return DataContract.CreateQualifiedName(DataContract.GetDefaultStableLocalName(type), DataContract.GetDefaultStableNamespace(type));
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x00026B60 File Offset: 0x00024D60
		private static string GetDefaultStableLocalName(Type type)
		{
			if (type.IsGenericParameter)
			{
				return "{" + type.GenericParameterPosition + "}";
			}
			string text = null;
			if (type.IsArray)
			{
				text = DataContract.GetArrayPrefix(ref type);
			}
			string text2;
			if (type.DeclaringType == null)
			{
				text2 = type.Name;
			}
			else
			{
				int num = ((type.Namespace == null) ? 0 : type.Namespace.Length);
				if (num > 0)
				{
					num++;
				}
				text2 = DataContract.GetClrTypeFullName(type).Substring(num).Replace('+', '.');
			}
			if (text != null)
			{
				text2 = text + text2;
			}
			if (type.IsGenericType)
			{
				StringBuilder stringBuilder = new StringBuilder();
				StringBuilder stringBuilder2 = new StringBuilder();
				bool flag = true;
				int num2 = text2.IndexOf('[');
				if (num2 >= 0)
				{
					text2 = text2.Substring(0, num2);
				}
				IList<int> dataContractNameForGenericName = DataContract.GetDataContractNameForGenericName(text2, stringBuilder);
				bool isGenericTypeDefinition = type.IsGenericTypeDefinition;
				Type[] genericArguments = type.GetGenericArguments();
				for (int i = 0; i < genericArguments.Length; i++)
				{
					Type type2 = genericArguments[i];
					if (isGenericTypeDefinition)
					{
						stringBuilder.Append("{").Append(i).Append("}");
					}
					else
					{
						XmlQualifiedName stableName = DataContract.GetStableName(type2);
						stringBuilder.Append(stableName.Name);
						stringBuilder2.Append(" ").Append(stableName.Namespace);
						if (flag)
						{
							flag = DataContract.IsBuiltInNamespace(stableName.Namespace);
						}
					}
				}
				if (isGenericTypeDefinition)
				{
					stringBuilder.Append("{#}");
				}
				else if (dataContractNameForGenericName.Count > 1 || !flag)
				{
					foreach (int num3 in dataContractNameForGenericName)
					{
						stringBuilder2.Insert(0, num3).Insert(0, " ");
					}
					stringBuilder.Append(DataContract.GetNamespacesDigest(stringBuilder2.ToString()));
				}
				text2 = stringBuilder.ToString();
			}
			return DataContract.EncodeLocalName(text2);
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x00026D58 File Offset: 0x00024F58
		private static string GetDefaultDataContractNamespace(Type type)
		{
			string text = type.Namespace;
			if (text == null)
			{
				text = string.Empty;
			}
			string text2 = DataContract.GetGlobalDataContractNamespace(text, type.Module);
			if (text2 == null)
			{
				text2 = DataContract.GetGlobalDataContractNamespace(text, type.Assembly);
			}
			if (text2 == null)
			{
				text2 = DataContract.GetDefaultStableNamespace(type);
			}
			else
			{
				DataContract.CheckExplicitDataContractNamespaceUri(text2, type);
			}
			return text2;
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x00026DA8 File Offset: 0x00024FA8
		internal static IList<int> GetDataContractNameForGenericName(string typeName, StringBuilder localName)
		{
			List<int> list = new List<int>();
			int num = 0;
			int num2;
			for (;;)
			{
				num2 = typeName.IndexOf('`', num);
				if (num2 < 0)
				{
					break;
				}
				if (localName != null)
				{
					localName.Append(typeName.Substring(num, num2 - num));
				}
				while ((num = typeName.IndexOf('.', num + 1, num2 - num - 1)) >= 0)
				{
					list.Add(0);
				}
				num = typeName.IndexOf('.', num2);
				if (num < 0)
				{
					goto Block_5;
				}
				list.Add(int.Parse(typeName.Substring(num2 + 1, num - num2 - 1), CultureInfo.InvariantCulture));
			}
			if (localName != null)
			{
				localName.Append(typeName.Substring(num));
			}
			list.Add(0);
			goto IL_00AE;
			Block_5:
			list.Add(int.Parse(typeName.Substring(num2 + 1), CultureInfo.InvariantCulture));
			IL_00AE:
			if (localName != null)
			{
				localName.Append("Of");
			}
			return list;
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x00026E73 File Offset: 0x00025073
		internal static bool IsBuiltInNamespace(string ns)
		{
			return ns == "http://www.w3.org/2001/XMLSchema" || ns == "http://schemas.microsoft.com/2003/10/Serialization/";
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x00026E8F File Offset: 0x0002508F
		internal static string GetDefaultStableNamespace(Type type)
		{
			if (type.IsGenericParameter)
			{
				return "{ns}";
			}
			return DataContract.GetDefaultStableNamespace(type.Namespace);
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x00026EAA File Offset: 0x000250AA
		internal static XmlQualifiedName CreateQualifiedName(string localName, string ns)
		{
			return new XmlQualifiedName(localName, DataContract.GetNamespace(ns));
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x00026EB8 File Offset: 0x000250B8
		internal static string GetDefaultStableNamespace(string clrNs)
		{
			if (clrNs == null)
			{
				clrNs = string.Empty;
			}
			return new Uri(Globals.DataContractXsdBaseNamespaceUri, clrNs).AbsoluteUri;
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x00026ED4 File Offset: 0x000250D4
		private static void CheckExplicitDataContractNamespaceUri(string dataContractNs, Type type)
		{
			if (dataContractNs.Length > 0)
			{
				string text = dataContractNs.Trim();
				if (text.Length == 0 || text.IndexOf("##", StringComparison.Ordinal) != -1)
				{
					DataContract.ThrowInvalidDataContractException(SR.GetString("DataContract namespace '{0}' is not a valid URI.", new object[] { dataContractNs }), type);
				}
				dataContractNs = text;
			}
			Uri uri;
			if (Uri.TryCreate(dataContractNs, UriKind.RelativeOrAbsolute, out uri))
			{
				if (uri.ToString() == "http://schemas.microsoft.com/2003/10/Serialization/")
				{
					DataContract.ThrowInvalidDataContractException(SR.GetString("DataContract namespace '{0}' cannot be specified since it is reserved.", new object[] { "http://schemas.microsoft.com/2003/10/Serialization/" }), type);
					return;
				}
			}
			else
			{
				DataContract.ThrowInvalidDataContractException(SR.GetString("DataContract namespace '{0}' is not a valid URI.", new object[] { dataContractNs }), type);
			}
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x00026F7B File Offset: 0x0002517B
		internal static string GetClrTypeFullName(Type type)
		{
			if (type.IsGenericTypeDefinition || !type.ContainsGenericParameters)
			{
				return type.FullName;
			}
			return string.Format(CultureInfo.InvariantCulture, "{0}.{1}", type.Namespace, type.Name);
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x00026FB0 File Offset: 0x000251B0
		internal static string GetClrAssemblyName(Type type, out bool hasTypeForwardedFrom)
		{
			hasTypeForwardedFrom = false;
			object[] customAttributes = type.GetCustomAttributes(typeof(TypeForwardedFromAttribute), false);
			if (customAttributes != null && customAttributes.Length != 0)
			{
				TypeForwardedFromAttribute typeForwardedFromAttribute = (TypeForwardedFromAttribute)customAttributes[0];
				hasTypeForwardedFrom = true;
				return typeForwardedFromAttribute.AssemblyFullName;
			}
			return type.Assembly.FullName;
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x00026FF5 File Offset: 0x000251F5
		internal static string GetClrTypeFullNameUsingTypeForwardedFromAttribute(Type type)
		{
			if (type.IsArray)
			{
				return DataContract.GetClrTypeFullNameForArray(type);
			}
			return DataContract.GetClrTypeFullNameForNonArrayTypes(type);
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x0002700C File Offset: 0x0002520C
		private static string GetClrTypeFullNameForArray(Type type)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}", DataContract.GetClrTypeFullNameUsingTypeForwardedFromAttribute(type.GetElementType()), "[", "]");
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x00027034 File Offset: 0x00025234
		private static string GetClrTypeFullNameForNonArrayTypes(Type type)
		{
			if (!type.IsGenericType)
			{
				return DataContract.GetClrTypeFullName(type);
			}
			Type[] genericArguments = type.GetGenericArguments();
			StringBuilder stringBuilder = new StringBuilder(type.GetGenericTypeDefinition().FullName).Append("[");
			foreach (Type type2 in genericArguments)
			{
				stringBuilder.Append("[").Append(DataContract.GetClrTypeFullNameUsingTypeForwardedFromAttribute(type2)).Append(",");
				bool flag;
				stringBuilder.Append(" ").Append(DataContract.GetClrAssemblyName(type2, out flag));
				stringBuilder.Append("]").Append(",");
			}
			return stringBuilder.Remove(stringBuilder.Length - 1, 1).Append("]").ToString();
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x000270F4 File Offset: 0x000252F4
		internal static void GetClrNameAndNamespace(string fullTypeName, out string localName, out string ns)
		{
			int num = fullTypeName.LastIndexOf('.');
			if (num < 0)
			{
				ns = string.Empty;
				localName = fullTypeName.Replace('+', '.');
			}
			else
			{
				ns = fullTypeName.Substring(0, num);
				localName = fullTypeName.Substring(num + 1).Replace('+', '.');
			}
			int num2 = localName.IndexOf('[');
			if (num2 >= 0)
			{
				localName = localName.Substring(0, num2);
			}
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x0002715A File Offset: 0x0002535A
		internal static void GetDefaultStableName(string fullTypeName, out string localName, out string ns)
		{
			DataContract.GetDefaultStableName(new CodeTypeReference(fullTypeName), out localName, out ns);
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x0002716C File Offset: 0x0002536C
		private static void GetDefaultStableName(CodeTypeReference typeReference, out string localName, out string ns)
		{
			string baseType = typeReference.BaseType;
			DataContract builtInDataContract = DataContract.GetBuiltInDataContract(baseType);
			if (builtInDataContract != null)
			{
				localName = builtInDataContract.StableName.Name;
				ns = builtInDataContract.StableName.Namespace;
				return;
			}
			DataContract.GetClrNameAndNamespace(baseType, out localName, out ns);
			if (typeReference.TypeArguments.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				StringBuilder stringBuilder2 = new StringBuilder();
				bool flag = true;
				IList<int> dataContractNameForGenericName = DataContract.GetDataContractNameForGenericName(localName, stringBuilder);
				foreach (object obj in typeReference.TypeArguments)
				{
					string text;
					string text2;
					DataContract.GetDefaultStableName((CodeTypeReference)obj, out text, out text2);
					stringBuilder.Append(text);
					stringBuilder2.Append(" ").Append(text2);
					if (flag)
					{
						flag = DataContract.IsBuiltInNamespace(text2);
					}
				}
				if (dataContractNameForGenericName.Count > 1 || !flag)
				{
					foreach (int num in dataContractNameForGenericName)
					{
						stringBuilder2.Insert(0, num).Insert(0, " ");
					}
					stringBuilder.Append(DataContract.GetNamespacesDigest(stringBuilder2.ToString()));
				}
				localName = stringBuilder.ToString();
			}
			localName = DataContract.EncodeLocalName(localName);
			ns = DataContract.GetDefaultStableNamespace(ns);
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x000272DC File Offset: 0x000254DC
		internal static string GetDataContractNamespaceFromUri(string uriString)
		{
			if (!uriString.StartsWith("http://schemas.datacontract.org/2004/07/", StringComparison.Ordinal))
			{
				return uriString;
			}
			return uriString.Substring("http://schemas.datacontract.org/2004/07/".Length);
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x00027300 File Offset: 0x00025500
		private static string GetGlobalDataContractNamespace(string clrNs, ICustomAttributeProvider customAttribuetProvider)
		{
			object[] customAttributes = customAttribuetProvider.GetCustomAttributes(typeof(ContractNamespaceAttribute), false);
			string text = null;
			foreach (ContractNamespaceAttribute contractNamespaceAttribute in customAttributes)
			{
				string text2 = contractNamespaceAttribute.ClrNamespace;
				if (text2 == null)
				{
					text2 = string.Empty;
				}
				if (text2 == clrNs)
				{
					if (contractNamespaceAttribute.ContractNamespace == null)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("CLR namespace '{0}' cannot have ContractNamespace set to null.", new object[] { clrNs })));
					}
					if (text != null)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("ContractNamespaceAttribute attribute maps CLR namespace '{2}' to multiple data contract namespaces '{0}' and '{1}'. You can map a CLR namespace to only one data contract namespace.", new object[] { text, contractNamespaceAttribute.ContractNamespace, clrNs })));
					}
					text = contractNamespaceAttribute.ContractNamespace;
				}
			}
			return text;
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x000273BC File Offset: 0x000255BC
		private static string GetNamespacesDigest(string namespaces)
		{
			byte[] array = HashHelper.ComputeHash(Encoding.UTF8.GetBytes(namespaces));
			char[] array2 = new char[24];
			int num = Convert.ToBase64CharArray(array, 0, 6, array2, 0);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < num; i++)
			{
				char c = array2[i];
				if (c != '+')
				{
					if (c != '/')
					{
						if (c != '=')
						{
							stringBuilder.Append(c);
						}
					}
					else
					{
						stringBuilder.Append("_S");
					}
				}
				else
				{
					stringBuilder.Append("_P");
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600085D RID: 2141 RVA: 0x00027444 File Offset: 0x00025644
		private static string ExpandGenericParameters(string format, Type type)
		{
			GenericNameProvider genericNameProvider = new GenericNameProvider(type);
			return DataContract.ExpandGenericParameters(format, genericNameProvider);
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x00027460 File Offset: 0x00025660
		internal static string ExpandGenericParameters(string format, IGenericNameProvider genericNameProvider)
		{
			string text = null;
			StringBuilder stringBuilder = new StringBuilder();
			IList<int> nestedParameterCounts = genericNameProvider.GetNestedParameterCounts();
			for (int i = 0; i < format.Length; i++)
			{
				char c = format[i];
				if (c == '{')
				{
					i++;
					int num = i;
					while (i < format.Length && format[i] != '}')
					{
						i++;
					}
					if (i == format.Length)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("The data contract name '{0}' for type '{1}' has a curly brace '{{' that is not matched with a closing curly brace. Curly braces have special meaning in data contract names - they are used to customize the naming of data contracts for generic types.", new object[]
						{
							format,
							genericNameProvider.GetGenericTypeName()
						})));
					}
					if (format[num] == '#' && i == num + 1)
					{
						if (nestedParameterCounts.Count > 1 || !genericNameProvider.ParametersFromBuiltInNamespaces)
						{
							if (text == null)
							{
								StringBuilder stringBuilder2 = new StringBuilder(genericNameProvider.GetNamespaces());
								foreach (int num2 in nestedParameterCounts)
								{
									stringBuilder2.Insert(0, num2).Insert(0, " ");
								}
								text = DataContract.GetNamespacesDigest(stringBuilder2.ToString());
							}
							stringBuilder.Append(text);
						}
					}
					else
					{
						int num3;
						if (!int.TryParse(format.Substring(num, i - num), out num3) || num3 < 0 || num3 >= genericNameProvider.GetParameterCount())
						{
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("In the data contract name for type '{1}', there are curly braces with '{0}' inside, which is an invalid value. Curly braces have special meaning in data contract names - they are used to customize the naming of data contracts for generic types. Based on the number of generic parameters this type has, the contents of the curly braces must either be a number between 0 and '{2}' to insert the name of the generic parameter at that index or the '#' symbol to insert a digest of the generic parameter namespaces.", new object[]
							{
								format.Substring(num, i - num),
								genericNameProvider.GetGenericTypeName(),
								genericNameProvider.GetParameterCount() - 1
							})));
						}
						stringBuilder.Append(genericNameProvider.GetParameterName(num3));
					}
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x00027624 File Offset: 0x00025824
		internal static bool IsTypeNullable(Type type)
		{
			return !type.IsValueType || (type.IsGenericType && type.GetGenericTypeDefinition() == Globals.TypeOfNullable);
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x0002764A File Offset: 0x0002584A
		public static void ThrowTypeNotSerializable(Type type)
		{
			DataContract.ThrowInvalidDataContractException(SR.GetString("Type '{0}' cannot be serialized. Consider marking it with the DataContractAttribute attribute, and marking all of its members you want serialized with the DataMemberAttribute attribute. Alternatively, you can ensure that the type is public and has a parameterless constructor - all public members of the type will then be serialized, and no attributes will be required.", new object[] { type }), type);
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000861 RID: 2145 RVA: 0x00027666 File Offset: 0x00025866
		private static DataContractSerializerSection ConfigSection
		{
			[SecurityCritical]
			get
			{
				if (DataContract.configSection == null)
				{
					DataContract.configSection = DataContractSerializerSection.UnsafeGetSection();
				}
				return DataContract.configSection;
			}
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x00027680 File Offset: 0x00025880
		internal static Dictionary<XmlQualifiedName, DataContract> ImportKnownTypeAttributes(Type type)
		{
			Dictionary<XmlQualifiedName, DataContract> dictionary = null;
			Dictionary<Type, Type> dictionary2 = new Dictionary<Type, Type>();
			DataContract.ImportKnownTypeAttributes(type, dictionary2, ref dictionary);
			return dictionary;
		}

		// Token: 0x06000863 RID: 2147 RVA: 0x000276A0 File Offset: 0x000258A0
		private static void ImportKnownTypeAttributes(Type type, Dictionary<Type, Type> typesChecked, ref Dictionary<XmlQualifiedName, DataContract> knownDataContracts)
		{
			if (TD.ImportKnownTypesStartIsEnabled())
			{
				TD.ImportKnownTypesStart();
			}
			while (type != null && DataContract.IsTypeSerializable(type))
			{
				if (typesChecked.ContainsKey(type))
				{
					return;
				}
				typesChecked.Add(type, type);
				object[] customAttributes = type.GetCustomAttributes(Globals.TypeOfKnownTypeAttribute, false);
				if (customAttributes != null)
				{
					bool flag = false;
					bool flag2 = false;
					foreach (KnownTypeAttribute knownTypeAttribute in customAttributes)
					{
						if (knownTypeAttribute.Type != null)
						{
							if (flag)
							{
								DataContract.ThrowInvalidDataContractException(SR.GetString("Type '{0}': If a KnownTypeAttribute attribute specifies a method it must be the only KnownTypeAttribute attribute on that type.", new object[] { DataContract.GetClrTypeFullName(type) }), type);
							}
							DataContract.CheckAndAdd(knownTypeAttribute.Type, typesChecked, ref knownDataContracts);
							flag2 = true;
						}
						else
						{
							if (flag || flag2)
							{
								DataContract.ThrowInvalidDataContractException(SR.GetString("Type '{0}': If a KnownTypeAttribute attribute specifies a method it must be the only KnownTypeAttribute attribute on that type.", new object[] { DataContract.GetClrTypeFullName(type) }), type);
							}
							string methodName = knownTypeAttribute.MethodName;
							if (methodName == null)
							{
								DataContract.ThrowInvalidDataContractException(SR.GetString("KnownTypeAttribute attribute on type '{0}' contains no data.", new object[] { DataContract.GetClrTypeFullName(type) }), type);
							}
							if (methodName.Length == 0)
							{
								DataContract.ThrowInvalidDataContractException(SR.GetString("Method name specified by KnownTypeAttribute attribute on type '{0}' cannot be the empty string.", new object[] { DataContract.GetClrTypeFullName(type) }), type);
							}
							MethodInfo method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, Globals.EmptyTypeArray, null);
							if (method == null)
							{
								DataContract.ThrowInvalidDataContractException(SR.GetString("KnownTypeAttribute attribute on type '{1}' specifies a method named '{0}' to provide known types. Static method '{0}()' was not found on this type. Ensure that the method exists and is marked as static.", new object[]
								{
									methodName,
									DataContract.GetClrTypeFullName(type)
								}), type);
							}
							if (!Globals.TypeOfTypeEnumerable.IsAssignableFrom(method.ReturnType))
							{
								DataContract.ThrowInvalidDataContractException(SR.GetString("KnownTypeAttribute attribute on type '{0}' specifies a method named '{1}' to provide known types. The return type of this method is invalid because it is not assignable to IEnumerable<Type>. Ensure that the method exists and has a valid signature.", new object[]
								{
									DataContract.GetClrTypeFullName(type),
									methodName
								}), type);
							}
							object obj = method.Invoke(null, Globals.EmptyObjectArray);
							if (obj == null)
							{
								DataContract.ThrowInvalidDataContractException(SR.GetString("Method specified by KnownTypeAttribute attribute on type '{0}' returned null.", new object[] { DataContract.GetClrTypeFullName(type) }), type);
							}
							foreach (Type type2 in ((IEnumerable<Type>)obj))
							{
								if (type2 == null)
								{
									DataContract.ThrowInvalidDataContractException(SR.GetString("Method specified by KnownTypeAttribute attribute on type '{0}' does not expose valid types.", new object[] { DataContract.GetClrTypeFullName(type) }), type);
								}
								DataContract.CheckAndAdd(type2, typesChecked, ref knownDataContracts);
							}
							flag = true;
						}
					}
				}
				DataContract.LoadKnownTypesFromConfig(type, typesChecked, ref knownDataContracts);
				type = type.BaseType;
			}
			if (TD.ImportKnownTypesStopIsEnabled())
			{
				TD.ImportKnownTypesStop();
			}
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x00027918 File Offset: 0x00025B18
		[SecuritySafeCritical]
		private static void LoadKnownTypesFromConfig(Type type, Dictionary<Type, Type> typesChecked, ref Dictionary<XmlQualifiedName, DataContract> knownDataContracts)
		{
			if (DataContract.ConfigSection != null)
			{
				DeclaredTypeElementCollection declaredTypes = DataContract.ConfigSection.DeclaredTypes;
				Type type2 = type;
				Type[] array = null;
				DataContract.CheckRootTypeInConfigIsGeneric(type, ref type2, ref array);
				DeclaredTypeElement declaredTypeElement = declaredTypes[type2.AssemblyQualifiedName];
				if (declaredTypeElement != null && DataContract.IsElemTypeNullOrNotEqualToRootType(declaredTypeElement.Type, type2))
				{
					declaredTypeElement = null;
				}
				if (declaredTypeElement == null)
				{
					for (int i = 0; i < declaredTypes.Count; i++)
					{
						if (DataContract.IsCollectionElementTypeEqualToRootType(declaredTypes[i].Type, type2))
						{
							declaredTypeElement = declaredTypes[i];
							break;
						}
					}
				}
				if (declaredTypeElement != null)
				{
					for (int j = 0; j < declaredTypeElement.KnownTypes.Count; j++)
					{
						Type type3 = declaredTypeElement.KnownTypes[j].GetType(declaredTypeElement.Type, array);
						if (type3 != null)
						{
							DataContract.CheckAndAdd(type3, typesChecked, ref knownDataContracts);
						}
					}
				}
			}
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x000279EC File Offset: 0x00025BEC
		private static void CheckRootTypeInConfigIsGeneric(Type type, ref Type rootType, ref Type[] genArgs)
		{
			if (rootType.IsGenericType)
			{
				if (!rootType.ContainsGenericParameters)
				{
					genArgs = rootType.GetGenericArguments();
					rootType = rootType.GetGenericTypeDefinition();
					return;
				}
				DataContract.ThrowInvalidDataContractException(SR.GetString("Error while getting known types for Type '{0}'. The type must not be an open or partial generic class.", new object[] { type }), type);
			}
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x00027A38 File Offset: 0x00025C38
		private static bool IsElemTypeNullOrNotEqualToRootType(string elemTypeName, Type rootType)
		{
			Type type = Type.GetType(elemTypeName, false);
			return type == null || !rootType.Equals(type);
		}

		// Token: 0x06000867 RID: 2151 RVA: 0x00027A64 File Offset: 0x00025C64
		private static bool IsCollectionElementTypeEqualToRootType(string collectionElementTypeName, Type rootType)
		{
			if (collectionElementTypeName.StartsWith(DataContract.GetClrTypeFullName(rootType), StringComparison.Ordinal))
			{
				Type type = Type.GetType(collectionElementTypeName, false);
				if (type != null)
				{
					if (type.IsGenericType && !DataContract.IsOpenGenericType(type))
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(SR.GetString("Declared type '{0}' in config cannot be a closed or partial generic type.", new object[] { collectionElementTypeName })));
					}
					if (rootType.Equals(type))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x00027ACC File Offset: 0x00025CCC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal static void CheckAndAdd(Type type, Dictionary<Type, Type> typesChecked, ref Dictionary<XmlQualifiedName, DataContract> nameToDataContractTable)
		{
			type = DataContract.UnwrapNullableType(type);
			DataContract dataContract = DataContract.GetDataContract(type);
			DataContract dataContract2;
			if (nameToDataContractTable == null)
			{
				nameToDataContractTable = new Dictionary<XmlQualifiedName, DataContract>();
			}
			else if (nameToDataContractTable.TryGetValue(dataContract.StableName, out dataContract2))
			{
				if (dataContract2.UnderlyingType != DataContract.DataContractCriticalHelper.GetDataContractAdapterType(type))
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(SR.GetString("Type '{0}' cannot be added to list of known types since another type '{1}' with the same data contract name '{2}:{3}' is already present.", new object[]
					{
						type,
						dataContract2.UnderlyingType,
						dataContract.StableName.Namespace,
						dataContract.StableName.Name
					})));
				}
				return;
			}
			nameToDataContractTable.Add(dataContract.StableName, dataContract);
			DataContract.ImportKnownTypeAttributes(type, typesChecked, ref nameToDataContractTable);
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x00027B74 File Offset: 0x00025D74
		private static bool IsOpenGenericType(Type t)
		{
			Type[] genericArguments = t.GetGenericArguments();
			for (int i = 0; i < genericArguments.Length; i++)
			{
				if (!genericArguments[i].IsGenericParameter)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x00027BA3 File Offset: 0x00025DA3
		public sealed override bool Equals(object other)
		{
			return this == other || this.Equals(other, new Dictionary<DataContractPairKey, object>());
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x00027BB8 File Offset: 0x00025DB8
		internal virtual bool Equals(object other, Dictionary<DataContractPairKey, object> checkedContracts)
		{
			DataContract dataContract = other as DataContract;
			return dataContract != null && (this.StableName.Name == dataContract.StableName.Name && this.StableName.Namespace == dataContract.StableName.Namespace) && this.IsReference == dataContract.IsReference;
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x00027C1C File Offset: 0x00025E1C
		internal bool IsEqualOrChecked(object other, Dictionary<DataContractPairKey, object> checkedContracts)
		{
			if (this == other)
			{
				return true;
			}
			if (checkedContracts != null)
			{
				DataContractPairKey dataContractPairKey = new DataContractPairKey(this, other);
				if (checkedContracts.ContainsKey(dataContractPairKey))
				{
					return true;
				}
				checkedContracts.Add(dataContractPairKey, null);
			}
			return false;
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x00027C4E File Offset: 0x00025E4E
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0600086E RID: 2158 RVA: 0x00027C56 File Offset: 0x00025E56
		internal void ThrowInvalidDataContractException(string message)
		{
			DataContract.ThrowInvalidDataContractException(message, this.UnderlyingType);
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x00027C64 File Offset: 0x00025E64
		internal static bool IsTypeVisible(Type t)
		{
			return true;
		}

		// Token: 0x04000314 RID: 788
		[SecurityCritical]
		private XmlDictionaryString name;

		// Token: 0x04000315 RID: 789
		[SecurityCritical]
		private XmlDictionaryString ns;

		// Token: 0x04000316 RID: 790
		[SecurityCritical]
		private DataContract.DataContractCriticalHelper helper;

		// Token: 0x04000317 RID: 791
		[SecurityCritical]
		private static DataContractSerializerSection configSection;

		// Token: 0x02000171 RID: 369
		[SecurityCritical(SecurityCriticalScope.Everything)]
		protected class DataContractCriticalHelper
		{
			// Token: 0x06001453 RID: 5203 RVA: 0x00052468 File Offset: 0x00050668
			internal static DataContract GetDataContractSkipValidation(int id, RuntimeTypeHandle typeHandle, Type type)
			{
				DataContract dataContract = DataContract.DataContractCriticalHelper.dataContractCache[id];
				if (dataContract == null)
				{
					return DataContract.DataContractCriticalHelper.CreateDataContract(id, typeHandle, type);
				}
				return dataContract.GetValidContract();
			}

			// Token: 0x06001454 RID: 5204 RVA: 0x00052494 File Offset: 0x00050694
			internal static DataContract GetGetOnlyCollectionDataContractSkipValidation(int id, RuntimeTypeHandle typeHandle, Type type)
			{
				DataContract dataContract = DataContract.DataContractCriticalHelper.dataContractCache[id];
				if (dataContract == null)
				{
					dataContract = DataContract.DataContractCriticalHelper.CreateGetOnlyCollectionDataContract(id, typeHandle, type);
					DataContract.DataContractCriticalHelper.AssignDataContractToId(dataContract, id);
				}
				return dataContract;
			}

			// Token: 0x06001455 RID: 5205 RVA: 0x000524BD File Offset: 0x000506BD
			internal static DataContract GetDataContractForInitialization(int id)
			{
				DataContract dataContract = DataContract.DataContractCriticalHelper.dataContractCache[id];
				if (dataContract == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new SerializationException(SR.GetString("An internal error has occurred. DataContract cache overflow.")));
				}
				return dataContract;
			}

			// Token: 0x06001456 RID: 5206 RVA: 0x000524E0 File Offset: 0x000506E0
			internal static int GetIdForInitialization(ClassDataContract classContract)
			{
				int id = DataContract.GetId(classContract.TypeForInitialization.TypeHandle);
				if (id < DataContract.DataContractCriticalHelper.dataContractCache.Length && DataContract.DataContractCriticalHelper.ContractMatches(classContract, DataContract.DataContractCriticalHelper.dataContractCache[id]))
				{
					return id;
				}
				int num = DataContract.DataContractCriticalHelper.dataContractID;
				for (int i = 0; i < num; i++)
				{
					if (DataContract.DataContractCriticalHelper.ContractMatches(classContract, DataContract.DataContractCriticalHelper.dataContractCache[i]))
					{
						return i;
					}
				}
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new SerializationException(SR.GetString("An internal error has occurred. DataContract cache overflow.")));
			}

			// Token: 0x06001457 RID: 5207 RVA: 0x00052550 File Offset: 0x00050750
			private static bool ContractMatches(DataContract contract, DataContract cachedContract)
			{
				return cachedContract != null && cachedContract.UnderlyingType == contract.UnderlyingType;
			}

			// Token: 0x06001458 RID: 5208 RVA: 0x00052568 File Offset: 0x00050768
			internal static int GetId(RuntimeTypeHandle typeHandle)
			{
				object obj = DataContract.DataContractCriticalHelper.cacheLock;
				int value;
				lock (obj)
				{
					typeHandle = DataContract.DataContractCriticalHelper.GetDataContractAdapterTypeHandle(typeHandle);
					DataContract.DataContractCriticalHelper.typeHandleRef.Value = typeHandle;
					IntRef nextId;
					if (!DataContract.DataContractCriticalHelper.typeToIDCache.TryGetValue(DataContract.DataContractCriticalHelper.typeHandleRef, out nextId))
					{
						nextId = DataContract.DataContractCriticalHelper.GetNextId();
						try
						{
							DataContract.DataContractCriticalHelper.typeToIDCache.Add(new TypeHandleRef(typeHandle), nextId);
						}
						catch (Exception ex)
						{
							if (Fx.IsFatal(ex))
							{
								throw;
							}
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperFatal(ex.Message, ex);
						}
					}
					value = nextId.Value;
				}
				return value;
			}

			// Token: 0x06001459 RID: 5209 RVA: 0x00052610 File Offset: 0x00050810
			private static IntRef GetNextId()
			{
				int num = DataContract.DataContractCriticalHelper.dataContractID++;
				if (num >= DataContract.DataContractCriticalHelper.dataContractCache.Length)
				{
					int num2 = ((num < 1073741823) ? (num * 2) : int.MaxValue);
					if (num2 <= num)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new SerializationException(SR.GetString("An internal error has occurred. DataContract cache overflow.")));
					}
					Array.Resize<DataContract>(ref DataContract.DataContractCriticalHelper.dataContractCache, num2);
				}
				return new IntRef(num);
			}

			// Token: 0x0600145A RID: 5210 RVA: 0x00052674 File Offset: 0x00050874
			private static DataContract CreateDataContract(int id, RuntimeTypeHandle typeHandle, Type type)
			{
				DataContract dataContract = DataContract.DataContractCriticalHelper.dataContractCache[id];
				if (dataContract == null)
				{
					object obj = DataContract.DataContractCriticalHelper.createDataContractLock;
					lock (obj)
					{
						dataContract = DataContract.DataContractCriticalHelper.dataContractCache[id];
						if (dataContract == null)
						{
							if (type == null)
							{
								type = Type.GetTypeFromHandle(typeHandle);
							}
							type = DataContract.UnwrapNullableType(type);
							type = DataContract.DataContractCriticalHelper.GetDataContractAdapterType(type);
							dataContract = DataContract.DataContractCriticalHelper.GetBuiltInDataContract(type);
							if (dataContract == null)
							{
								if (type.IsArray)
								{
									dataContract = new CollectionDataContract(type);
								}
								else if (type.IsEnum)
								{
									dataContract = new EnumDataContract(type);
								}
								else if (type.IsGenericParameter)
								{
									dataContract = new GenericParameterDataContract(type);
								}
								else if (Globals.TypeOfIXmlSerializable.IsAssignableFrom(type))
								{
									dataContract = new XmlDataContract(type);
								}
								else
								{
									if (type.IsPointer)
									{
										type = Globals.TypeOfReflectionPointer;
									}
									if (!CollectionDataContract.TryCreate(type, out dataContract))
									{
										if (type.IsSerializable || type.IsDefined(Globals.TypeOfDataContractAttribute, false) || ClassDataContract.IsNonAttributedTypeValidForSerialization(type))
										{
											dataContract = new ClassDataContract(type);
										}
										else
										{
											DataContract.DataContractCriticalHelper.ThrowInvalidDataContractException(SR.GetString("Type '{0}' cannot be serialized. Consider marking it with the DataContractAttribute attribute, and marking all of its members you want serialized with the DataMemberAttribute attribute. Alternatively, you can ensure that the type is public and has a parameterless constructor - all public members of the type will then be serialized, and no attributes will be required.", new object[] { type }), type);
										}
									}
								}
							}
							DataContract.DataContractCriticalHelper.AssignDataContractToId(dataContract, id);
						}
					}
				}
				return dataContract;
			}

			// Token: 0x0600145B RID: 5211 RVA: 0x000527A8 File Offset: 0x000509A8
			[MethodImpl(MethodImplOptions.NoInlining)]
			private static void AssignDataContractToId(DataContract dataContract, int id)
			{
				object obj = DataContract.DataContractCriticalHelper.cacheLock;
				lock (obj)
				{
					DataContract.DataContractCriticalHelper.dataContractCache[id] = dataContract;
				}
			}

			// Token: 0x0600145C RID: 5212 RVA: 0x000527EC File Offset: 0x000509EC
			private static DataContract CreateGetOnlyCollectionDataContract(int id, RuntimeTypeHandle typeHandle, Type type)
			{
				DataContract dataContract = null;
				object obj = DataContract.DataContractCriticalHelper.createDataContractLock;
				lock (obj)
				{
					dataContract = DataContract.DataContractCriticalHelper.dataContractCache[id];
					if (dataContract == null)
					{
						if (type == null)
						{
							type = Type.GetTypeFromHandle(typeHandle);
						}
						type = DataContract.UnwrapNullableType(type);
						type = DataContract.DataContractCriticalHelper.GetDataContractAdapterType(type);
						if (!CollectionDataContract.TryCreateGetOnlyCollectionDataContract(type, out dataContract))
						{
							DataContract.DataContractCriticalHelper.ThrowInvalidDataContractException(SR.GetString("Type '{0}' cannot be serialized. Consider marking it with the DataContractAttribute attribute, and marking all of its members you want serialized with the DataMemberAttribute attribute. Alternatively, you can ensure that the type is public and has a parameterless constructor - all public members of the type will then be serialized, and no attributes will be required.", new object[] { type }), type);
						}
					}
				}
				return dataContract;
			}

			// Token: 0x0600145D RID: 5213 RVA: 0x00052878 File Offset: 0x00050A78
			internal static Type GetDataContractAdapterType(Type type)
			{
				if (type == Globals.TypeOfDateTimeOffset)
				{
					return Globals.TypeOfDateTimeOffsetAdapter;
				}
				return type;
			}

			// Token: 0x0600145E RID: 5214 RVA: 0x0005288E File Offset: 0x00050A8E
			internal static Type GetDataContractOriginalType(Type type)
			{
				if (type == Globals.TypeOfDateTimeOffsetAdapter)
				{
					return Globals.TypeOfDateTimeOffset;
				}
				return type;
			}

			// Token: 0x0600145F RID: 5215 RVA: 0x000528A4 File Offset: 0x00050AA4
			private static RuntimeTypeHandle GetDataContractAdapterTypeHandle(RuntimeTypeHandle typeHandle)
			{
				if (Globals.TypeOfDateTimeOffset.TypeHandle.Equals(typeHandle))
				{
					return Globals.TypeOfDateTimeOffsetAdapter.TypeHandle;
				}
				return typeHandle;
			}

			// Token: 0x06001460 RID: 5216 RVA: 0x000528D4 File Offset: 0x00050AD4
			public static DataContract GetBuiltInDataContract(Type type)
			{
				if (type.IsInterface && !CollectionDataContract.IsCollectionInterface(type))
				{
					type = Globals.TypeOfObject;
				}
				object obj = DataContract.DataContractCriticalHelper.initBuiltInContractsLock;
				DataContract dataContract2;
				lock (obj)
				{
					if (DataContract.DataContractCriticalHelper.typeToBuiltInContract == null)
					{
						DataContract.DataContractCriticalHelper.typeToBuiltInContract = new Dictionary<Type, DataContract>();
					}
					DataContract dataContract = null;
					if (!DataContract.DataContractCriticalHelper.typeToBuiltInContract.TryGetValue(type, out dataContract))
					{
						DataContract.DataContractCriticalHelper.TryCreateBuiltInDataContract(type, out dataContract);
						DataContract.DataContractCriticalHelper.typeToBuiltInContract.Add(type, dataContract);
					}
					dataContract2 = dataContract;
				}
				return dataContract2;
			}

			// Token: 0x06001461 RID: 5217 RVA: 0x00052960 File Offset: 0x00050B60
			public static DataContract GetBuiltInDataContract(string name, string ns)
			{
				object obj = DataContract.DataContractCriticalHelper.initBuiltInContractsLock;
				DataContract dataContract2;
				lock (obj)
				{
					if (DataContract.DataContractCriticalHelper.nameToBuiltInContract == null)
					{
						DataContract.DataContractCriticalHelper.nameToBuiltInContract = new Dictionary<XmlQualifiedName, DataContract>();
					}
					DataContract dataContract = null;
					XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(name, ns);
					if (!DataContract.DataContractCriticalHelper.nameToBuiltInContract.TryGetValue(xmlQualifiedName, out dataContract) && DataContract.DataContractCriticalHelper.TryCreateBuiltInDataContract(name, ns, out dataContract))
					{
						DataContract.DataContractCriticalHelper.nameToBuiltInContract.Add(xmlQualifiedName, dataContract);
					}
					dataContract2 = dataContract;
				}
				return dataContract2;
			}

			// Token: 0x06001462 RID: 5218 RVA: 0x000529E0 File Offset: 0x00050BE0
			public static DataContract GetBuiltInDataContract(string typeName)
			{
				if (!typeName.StartsWith("System.", StringComparison.Ordinal))
				{
					return null;
				}
				object obj = DataContract.DataContractCriticalHelper.initBuiltInContractsLock;
				DataContract dataContract2;
				lock (obj)
				{
					if (DataContract.DataContractCriticalHelper.typeNameToBuiltInContract == null)
					{
						DataContract.DataContractCriticalHelper.typeNameToBuiltInContract = new Dictionary<string, DataContract>();
					}
					DataContract dataContract = null;
					if (!DataContract.DataContractCriticalHelper.typeNameToBuiltInContract.TryGetValue(typeName, out dataContract))
					{
						Type type = null;
						string text = typeName.Substring(7);
						if (text == "Char")
						{
							type = typeof(char);
						}
						else if (text == "Boolean")
						{
							type = typeof(bool);
						}
						else if (text == "SByte")
						{
							type = typeof(sbyte);
						}
						else if (text == "Byte")
						{
							type = typeof(byte);
						}
						else if (text == "Int16")
						{
							type = typeof(short);
						}
						else if (text == "UInt16")
						{
							type = typeof(ushort);
						}
						else if (text == "Int32")
						{
							type = typeof(int);
						}
						else if (text == "UInt32")
						{
							type = typeof(uint);
						}
						else if (text == "Int64")
						{
							type = typeof(long);
						}
						else if (text == "UInt64")
						{
							type = typeof(ulong);
						}
						else if (text == "Single")
						{
							type = typeof(float);
						}
						else if (text == "Double")
						{
							type = typeof(double);
						}
						else if (text == "Decimal")
						{
							type = typeof(decimal);
						}
						else if (text == "DateTime")
						{
							type = typeof(DateTime);
						}
						else if (text == "String")
						{
							type = typeof(string);
						}
						else if (text == "Byte[]")
						{
							type = typeof(byte[]);
						}
						else if (text == "Object")
						{
							type = typeof(object);
						}
						else if (text == "TimeSpan")
						{
							type = typeof(TimeSpan);
						}
						else if (text == "Guid")
						{
							type = typeof(Guid);
						}
						else if (text == "Uri")
						{
							type = typeof(Uri);
						}
						else if (text == "Xml.XmlQualifiedName")
						{
							type = typeof(XmlQualifiedName);
						}
						else if (text == "Enum")
						{
							type = typeof(Enum);
						}
						else if (text == "ValueType")
						{
							type = typeof(ValueType);
						}
						else if (text == "Array")
						{
							type = typeof(Array);
						}
						else if (text == "Xml.XmlElement")
						{
							type = typeof(XmlElement);
						}
						else if (text == "Xml.XmlNode[]")
						{
							type = typeof(XmlNode[]);
						}
						if (type != null)
						{
							DataContract.DataContractCriticalHelper.TryCreateBuiltInDataContract(type, out dataContract);
						}
						DataContract.DataContractCriticalHelper.typeNameToBuiltInContract.Add(typeName, dataContract);
					}
					dataContract2 = dataContract;
				}
				return dataContract2;
			}

			// Token: 0x06001463 RID: 5219 RVA: 0x00052D84 File Offset: 0x00050F84
			public static bool TryCreateBuiltInDataContract(Type type, out DataContract dataContract)
			{
				if (type.IsEnum)
				{
					dataContract = null;
					return false;
				}
				dataContract = null;
				switch (Type.GetTypeCode(type))
				{
				case TypeCode.Boolean:
					dataContract = new BooleanDataContract();
					goto IL_024C;
				case TypeCode.Char:
					dataContract = new CharDataContract();
					goto IL_024C;
				case TypeCode.SByte:
					dataContract = new SignedByteDataContract();
					goto IL_024C;
				case TypeCode.Byte:
					dataContract = new UnsignedByteDataContract();
					goto IL_024C;
				case TypeCode.Int16:
					dataContract = new ShortDataContract();
					goto IL_024C;
				case TypeCode.UInt16:
					dataContract = new UnsignedShortDataContract();
					goto IL_024C;
				case TypeCode.Int32:
					dataContract = new IntDataContract();
					goto IL_024C;
				case TypeCode.UInt32:
					dataContract = new UnsignedIntDataContract();
					goto IL_024C;
				case TypeCode.Int64:
					dataContract = new LongDataContract();
					goto IL_024C;
				case TypeCode.UInt64:
					dataContract = new UnsignedLongDataContract();
					goto IL_024C;
				case TypeCode.Single:
					dataContract = new FloatDataContract();
					goto IL_024C;
				case TypeCode.Double:
					dataContract = new DoubleDataContract();
					goto IL_024C;
				case TypeCode.Decimal:
					dataContract = new DecimalDataContract();
					goto IL_024C;
				case TypeCode.DateTime:
					dataContract = new DateTimeDataContract();
					goto IL_024C;
				case TypeCode.String:
					dataContract = new StringDataContract();
					goto IL_024C;
				}
				if (type == typeof(byte[]))
				{
					dataContract = new ByteArrayDataContract();
				}
				else if (type == typeof(object))
				{
					dataContract = new ObjectDataContract();
				}
				else if (type == typeof(Uri))
				{
					dataContract = new UriDataContract();
				}
				else if (type == typeof(XmlQualifiedName))
				{
					dataContract = new QNameDataContract();
				}
				else if (type == typeof(TimeSpan))
				{
					dataContract = new TimeSpanDataContract();
				}
				else if (type == typeof(Guid))
				{
					dataContract = new GuidDataContract();
				}
				else if (type == typeof(Enum) || type == typeof(ValueType))
				{
					dataContract = new SpecialTypeDataContract(type, DictionaryGlobals.ObjectLocalName, DictionaryGlobals.SchemaNamespace);
				}
				else if (type == typeof(Array))
				{
					dataContract = new CollectionDataContract(type);
				}
				else if (type == typeof(XmlElement) || type == typeof(XmlNode[]))
				{
					dataContract = new XmlDataContract(type);
				}
				IL_024C:
				return dataContract != null;
			}

			// Token: 0x06001464 RID: 5220 RVA: 0x00052FE4 File Offset: 0x000511E4
			public static bool TryCreateBuiltInDataContract(string name, string ns, out DataContract dataContract)
			{
				dataContract = null;
				if (ns == DictionaryGlobals.SchemaNamespace.Value)
				{
					if (DictionaryGlobals.BooleanLocalName.Value == name)
					{
						dataContract = new BooleanDataContract();
					}
					else if (DictionaryGlobals.SignedByteLocalName.Value == name)
					{
						dataContract = new SignedByteDataContract();
					}
					else if (DictionaryGlobals.UnsignedByteLocalName.Value == name)
					{
						dataContract = new UnsignedByteDataContract();
					}
					else if (DictionaryGlobals.ShortLocalName.Value == name)
					{
						dataContract = new ShortDataContract();
					}
					else if (DictionaryGlobals.UnsignedShortLocalName.Value == name)
					{
						dataContract = new UnsignedShortDataContract();
					}
					else if (DictionaryGlobals.IntLocalName.Value == name)
					{
						dataContract = new IntDataContract();
					}
					else if (DictionaryGlobals.UnsignedIntLocalName.Value == name)
					{
						dataContract = new UnsignedIntDataContract();
					}
					else if (DictionaryGlobals.LongLocalName.Value == name)
					{
						dataContract = new LongDataContract();
					}
					else if (DictionaryGlobals.integerLocalName.Value == name)
					{
						dataContract = new IntegerDataContract();
					}
					else if (DictionaryGlobals.positiveIntegerLocalName.Value == name)
					{
						dataContract = new PositiveIntegerDataContract();
					}
					else if (DictionaryGlobals.negativeIntegerLocalName.Value == name)
					{
						dataContract = new NegativeIntegerDataContract();
					}
					else if (DictionaryGlobals.nonPositiveIntegerLocalName.Value == name)
					{
						dataContract = new NonPositiveIntegerDataContract();
					}
					else if (DictionaryGlobals.nonNegativeIntegerLocalName.Value == name)
					{
						dataContract = new NonNegativeIntegerDataContract();
					}
					else if (DictionaryGlobals.UnsignedLongLocalName.Value == name)
					{
						dataContract = new UnsignedLongDataContract();
					}
					else if (DictionaryGlobals.FloatLocalName.Value == name)
					{
						dataContract = new FloatDataContract();
					}
					else if (DictionaryGlobals.DoubleLocalName.Value == name)
					{
						dataContract = new DoubleDataContract();
					}
					else if (DictionaryGlobals.DecimalLocalName.Value == name)
					{
						dataContract = new DecimalDataContract();
					}
					else if (DictionaryGlobals.DateTimeLocalName.Value == name)
					{
						dataContract = new DateTimeDataContract();
					}
					else if (DictionaryGlobals.StringLocalName.Value == name)
					{
						dataContract = new StringDataContract();
					}
					else if (DictionaryGlobals.timeLocalName.Value == name)
					{
						dataContract = new TimeDataContract();
					}
					else if (DictionaryGlobals.dateLocalName.Value == name)
					{
						dataContract = new DateDataContract();
					}
					else if (DictionaryGlobals.hexBinaryLocalName.Value == name)
					{
						dataContract = new HexBinaryDataContract();
					}
					else if (DictionaryGlobals.gYearMonthLocalName.Value == name)
					{
						dataContract = new GYearMonthDataContract();
					}
					else if (DictionaryGlobals.gYearLocalName.Value == name)
					{
						dataContract = new GYearDataContract();
					}
					else if (DictionaryGlobals.gMonthDayLocalName.Value == name)
					{
						dataContract = new GMonthDayDataContract();
					}
					else if (DictionaryGlobals.gDayLocalName.Value == name)
					{
						dataContract = new GDayDataContract();
					}
					else if (DictionaryGlobals.gMonthLocalName.Value == name)
					{
						dataContract = new GMonthDataContract();
					}
					else if (DictionaryGlobals.normalizedStringLocalName.Value == name)
					{
						dataContract = new NormalizedStringDataContract();
					}
					else if (DictionaryGlobals.tokenLocalName.Value == name)
					{
						dataContract = new TokenDataContract();
					}
					else if (DictionaryGlobals.languageLocalName.Value == name)
					{
						dataContract = new LanguageDataContract();
					}
					else if (DictionaryGlobals.NameLocalName.Value == name)
					{
						dataContract = new NameDataContract();
					}
					else if (DictionaryGlobals.NCNameLocalName.Value == name)
					{
						dataContract = new NCNameDataContract();
					}
					else if (DictionaryGlobals.XSDIDLocalName.Value == name)
					{
						dataContract = new IDDataContract();
					}
					else if (DictionaryGlobals.IDREFLocalName.Value == name)
					{
						dataContract = new IDREFDataContract();
					}
					else if (DictionaryGlobals.IDREFSLocalName.Value == name)
					{
						dataContract = new IDREFSDataContract();
					}
					else if (DictionaryGlobals.ENTITYLocalName.Value == name)
					{
						dataContract = new ENTITYDataContract();
					}
					else if (DictionaryGlobals.ENTITIESLocalName.Value == name)
					{
						dataContract = new ENTITIESDataContract();
					}
					else if (DictionaryGlobals.NMTOKENLocalName.Value == name)
					{
						dataContract = new NMTOKENDataContract();
					}
					else if (DictionaryGlobals.NMTOKENSLocalName.Value == name)
					{
						dataContract = new NMTOKENDataContract();
					}
					else if (DictionaryGlobals.ByteArrayLocalName.Value == name)
					{
						dataContract = new ByteArrayDataContract();
					}
					else if (DictionaryGlobals.ObjectLocalName.Value == name)
					{
						dataContract = new ObjectDataContract();
					}
					else if (DictionaryGlobals.TimeSpanLocalName.Value == name)
					{
						dataContract = new XsDurationDataContract();
					}
					else if (DictionaryGlobals.UriLocalName.Value == name)
					{
						dataContract = new UriDataContract();
					}
					else if (DictionaryGlobals.QNameLocalName.Value == name)
					{
						dataContract = new QNameDataContract();
					}
				}
				else if (ns == DictionaryGlobals.SerializationNamespace.Value)
				{
					if (DictionaryGlobals.TimeSpanLocalName.Value == name)
					{
						dataContract = new TimeSpanDataContract();
					}
					else if (DictionaryGlobals.GuidLocalName.Value == name)
					{
						dataContract = new GuidDataContract();
					}
					else if (DictionaryGlobals.CharLocalName.Value == name)
					{
						dataContract = new CharDataContract();
					}
					else if ("ArrayOfanyType" == name)
					{
						dataContract = new CollectionDataContract(typeof(Array));
					}
				}
				else if (ns == DictionaryGlobals.AsmxTypesNamespace.Value)
				{
					if (DictionaryGlobals.CharLocalName.Value == name)
					{
						dataContract = new AsmxCharDataContract();
					}
					else if (DictionaryGlobals.GuidLocalName.Value == name)
					{
						dataContract = new AsmxGuidDataContract();
					}
				}
				else if (ns == "http://schemas.datacontract.org/2004/07/System.Xml")
				{
					if (name == "XmlElement")
					{
						dataContract = new XmlDataContract(typeof(XmlElement));
					}
					else if (name == "ArrayOfXmlNode")
					{
						dataContract = new XmlDataContract(typeof(XmlNode[]));
					}
				}
				return dataContract != null;
			}

			// Token: 0x06001465 RID: 5221 RVA: 0x00053664 File Offset: 0x00051864
			internal static string GetNamespace(string key)
			{
				object obj = DataContract.DataContractCriticalHelper.namespacesLock;
				string text2;
				lock (obj)
				{
					if (DataContract.DataContractCriticalHelper.namespaces == null)
					{
						DataContract.DataContractCriticalHelper.namespaces = new Dictionary<string, string>();
					}
					string text;
					if (DataContract.DataContractCriticalHelper.namespaces.TryGetValue(key, out text))
					{
						text2 = text;
					}
					else
					{
						try
						{
							DataContract.DataContractCriticalHelper.namespaces.Add(key, key);
						}
						catch (Exception ex)
						{
							if (Fx.IsFatal(ex))
							{
								throw;
							}
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperFatal(ex.Message, ex);
						}
						text2 = key;
					}
				}
				return text2;
			}

			// Token: 0x06001466 RID: 5222 RVA: 0x000536FC File Offset: 0x000518FC
			internal static XmlDictionaryString GetClrTypeString(string key)
			{
				object obj = DataContract.DataContractCriticalHelper.clrTypeStringsLock;
				XmlDictionaryString xmlDictionaryString2;
				lock (obj)
				{
					if (DataContract.DataContractCriticalHelper.clrTypeStrings == null)
					{
						DataContract.DataContractCriticalHelper.clrTypeStringsDictionary = new XmlDictionary();
						DataContract.DataContractCriticalHelper.clrTypeStrings = new Dictionary<string, XmlDictionaryString>();
						try
						{
							DataContract.DataContractCriticalHelper.clrTypeStrings.Add(Globals.TypeOfInt.Assembly.FullName, DataContract.DataContractCriticalHelper.clrTypeStringsDictionary.Add("0"));
						}
						catch (Exception ex)
						{
							if (Fx.IsFatal(ex))
							{
								throw;
							}
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperFatal(ex.Message, ex);
						}
					}
					XmlDictionaryString xmlDictionaryString;
					if (DataContract.DataContractCriticalHelper.clrTypeStrings.TryGetValue(key, out xmlDictionaryString))
					{
						xmlDictionaryString2 = xmlDictionaryString;
					}
					else
					{
						xmlDictionaryString = DataContract.DataContractCriticalHelper.clrTypeStringsDictionary.Add(key);
						try
						{
							DataContract.DataContractCriticalHelper.clrTypeStrings.Add(key, xmlDictionaryString);
						}
						catch (Exception ex2)
						{
							if (Fx.IsFatal(ex2))
							{
								throw;
							}
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperFatal(ex2.Message, ex2);
						}
						xmlDictionaryString2 = xmlDictionaryString;
					}
				}
				return xmlDictionaryString2;
			}

			// Token: 0x06001467 RID: 5223 RVA: 0x000537FC File Offset: 0x000519FC
			internal static void ThrowInvalidDataContractException(string message, Type type)
			{
				if (type != null)
				{
					object obj = DataContract.DataContractCriticalHelper.cacheLock;
					lock (obj)
					{
						DataContract.DataContractCriticalHelper.typeHandleRef.Value = DataContract.DataContractCriticalHelper.GetDataContractAdapterTypeHandle(type.TypeHandle);
						try
						{
							DataContract.DataContractCriticalHelper.typeToIDCache.Remove(DataContract.DataContractCriticalHelper.typeHandleRef);
						}
						catch (Exception ex)
						{
							if (Fx.IsFatal(ex))
							{
								throw;
							}
							throw DiagnosticUtility.ExceptionUtility.ThrowHelperFatal(ex.Message, ex);
						}
					}
				}
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(message));
			}

			// Token: 0x06001468 RID: 5224 RVA: 0x00053894 File Offset: 0x00051A94
			internal DataContractCriticalHelper()
			{
			}

			// Token: 0x06001469 RID: 5225 RVA: 0x0005389C File Offset: 0x00051A9C
			internal DataContractCriticalHelper(Type type)
			{
				this.underlyingType = type;
				this.SetTypeForInitialization(type);
				this.isValueType = type.IsValueType;
			}

			// Token: 0x1700042A RID: 1066
			// (get) Token: 0x0600146A RID: 5226 RVA: 0x000538BE File Offset: 0x00051ABE
			internal Type UnderlyingType
			{
				get
				{
					return this.underlyingType;
				}
			}

			// Token: 0x1700042B RID: 1067
			// (get) Token: 0x0600146B RID: 5227 RVA: 0x000538C6 File Offset: 0x00051AC6
			internal Type OriginalUnderlyingType
			{
				get
				{
					if (this.originalUnderlyingType == null)
					{
						this.originalUnderlyingType = DataContract.DataContractCriticalHelper.GetDataContractOriginalType(this.underlyingType);
					}
					return this.originalUnderlyingType;
				}
			}

			// Token: 0x1700042C RID: 1068
			// (get) Token: 0x0600146C RID: 5228 RVA: 0x000538ED File Offset: 0x00051AED
			internal virtual bool IsBuiltInDataContract
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700042D RID: 1069
			// (get) Token: 0x0600146D RID: 5229 RVA: 0x000538F0 File Offset: 0x00051AF0
			internal Type TypeForInitialization
			{
				get
				{
					return this.typeForInitialization;
				}
			}

			// Token: 0x0600146E RID: 5230 RVA: 0x000538F8 File Offset: 0x00051AF8
			[SecuritySafeCritical]
			private void SetTypeForInitialization(Type classType)
			{
				if (classType.IsSerializable || classType.IsDefined(Globals.TypeOfDataContractAttribute, false))
				{
					this.typeForInitialization = classType;
				}
			}

			// Token: 0x1700042E RID: 1070
			// (get) Token: 0x0600146F RID: 5231 RVA: 0x00053917 File Offset: 0x00051B17
			// (set) Token: 0x06001470 RID: 5232 RVA: 0x0005391F File Offset: 0x00051B1F
			internal bool IsReference
			{
				get
				{
					return this.isReference;
				}
				set
				{
					this.isReference = value;
				}
			}

			// Token: 0x1700042F RID: 1071
			// (get) Token: 0x06001471 RID: 5233 RVA: 0x00053928 File Offset: 0x00051B28
			// (set) Token: 0x06001472 RID: 5234 RVA: 0x00053930 File Offset: 0x00051B30
			internal bool IsValueType
			{
				get
				{
					return this.isValueType;
				}
				set
				{
					this.isValueType = value;
				}
			}

			// Token: 0x17000430 RID: 1072
			// (get) Token: 0x06001473 RID: 5235 RVA: 0x00053939 File Offset: 0x00051B39
			// (set) Token: 0x06001474 RID: 5236 RVA: 0x00053941 File Offset: 0x00051B41
			internal XmlQualifiedName StableName
			{
				get
				{
					return this.stableName;
				}
				set
				{
					this.stableName = value;
				}
			}

			// Token: 0x17000431 RID: 1073
			// (get) Token: 0x06001475 RID: 5237 RVA: 0x0005394A File Offset: 0x00051B4A
			// (set) Token: 0x06001476 RID: 5238 RVA: 0x00053952 File Offset: 0x00051B52
			internal GenericInfo GenericInfo
			{
				get
				{
					return this.genericInfo;
				}
				set
				{
					this.genericInfo = value;
				}
			}

			// Token: 0x17000432 RID: 1074
			// (get) Token: 0x06001477 RID: 5239 RVA: 0x0005395B File Offset: 0x00051B5B
			// (set) Token: 0x06001478 RID: 5240 RVA: 0x0005395E File Offset: 0x00051B5E
			internal virtual Dictionary<XmlQualifiedName, DataContract> KnownDataContracts
			{
				get
				{
					return null;
				}
				set
				{
				}
			}

			// Token: 0x17000433 RID: 1075
			// (get) Token: 0x06001479 RID: 5241 RVA: 0x00053960 File Offset: 0x00051B60
			// (set) Token: 0x0600147A RID: 5242 RVA: 0x00053963 File Offset: 0x00051B63
			internal virtual bool IsISerializable
			{
				get
				{
					return false;
				}
				set
				{
					this.ThrowInvalidDataContractException(SR.GetString("To set IsISerializable, class data cotnract is required."));
				}
			}

			// Token: 0x17000434 RID: 1076
			// (get) Token: 0x0600147B RID: 5243 RVA: 0x00053975 File Offset: 0x00051B75
			// (set) Token: 0x0600147C RID: 5244 RVA: 0x0005397D File Offset: 0x00051B7D
			internal XmlDictionaryString Name
			{
				get
				{
					return this.name;
				}
				set
				{
					this.name = value;
				}
			}

			// Token: 0x17000435 RID: 1077
			// (get) Token: 0x0600147D RID: 5245 RVA: 0x00053986 File Offset: 0x00051B86
			// (set) Token: 0x0600147E RID: 5246 RVA: 0x0005398E File Offset: 0x00051B8E
			public XmlDictionaryString Namespace
			{
				get
				{
					return this.ns;
				}
				set
				{
					this.ns = value;
				}
			}

			// Token: 0x17000436 RID: 1078
			// (get) Token: 0x0600147F RID: 5247 RVA: 0x00053997 File Offset: 0x00051B97
			// (set) Token: 0x06001480 RID: 5248 RVA: 0x0005399A File Offset: 0x00051B9A
			internal virtual bool HasRoot
			{
				get
				{
					return true;
				}
				set
				{
				}
			}

			// Token: 0x17000437 RID: 1079
			// (get) Token: 0x06001481 RID: 5249 RVA: 0x0005399C File Offset: 0x00051B9C
			// (set) Token: 0x06001482 RID: 5250 RVA: 0x000539A4 File Offset: 0x00051BA4
			internal virtual XmlDictionaryString TopLevelElementName
			{
				get
				{
					return this.name;
				}
				set
				{
					this.name = value;
				}
			}

			// Token: 0x17000438 RID: 1080
			// (get) Token: 0x06001483 RID: 5251 RVA: 0x000539AD File Offset: 0x00051BAD
			// (set) Token: 0x06001484 RID: 5252 RVA: 0x000539B5 File Offset: 0x00051BB5
			internal virtual XmlDictionaryString TopLevelElementNamespace
			{
				get
				{
					return this.ns;
				}
				set
				{
					this.ns = value;
				}
			}

			// Token: 0x17000439 RID: 1081
			// (get) Token: 0x06001485 RID: 5253 RVA: 0x000539BE File Offset: 0x00051BBE
			internal virtual bool CanContainReferences
			{
				get
				{
					return true;
				}
			}

			// Token: 0x1700043A RID: 1082
			// (get) Token: 0x06001486 RID: 5254 RVA: 0x000539C1 File Offset: 0x00051BC1
			internal virtual bool IsPrimitive
			{
				get
				{
					return false;
				}
			}

			// Token: 0x1700043B RID: 1083
			// (get) Token: 0x06001487 RID: 5255 RVA: 0x000539C4 File Offset: 0x00051BC4
			internal MethodInfo ParseMethod
			{
				get
				{
					if (!this.parseMethodSet)
					{
						MethodInfo method = this.UnderlyingType.GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, null, new Type[] { Globals.TypeOfString }, null);
						if (method != null && method.ReturnType == this.UnderlyingType)
						{
							this.parseMethod = method;
						}
						this.parseMethodSet = true;
					}
					return this.parseMethod;
				}
			}

			// Token: 0x06001488 RID: 5256 RVA: 0x00053A2C File Offset: 0x00051C2C
			internal virtual void WriteRootElement(XmlWriterDelegator writer, XmlDictionaryString name, XmlDictionaryString ns)
			{
				if (ns == DictionaryGlobals.SerializationNamespace && !this.IsPrimitive)
				{
					writer.WriteStartElement("z", name, ns);
					return;
				}
				writer.WriteStartElement(name, ns);
			}

			// Token: 0x06001489 RID: 5257 RVA: 0x00053A54 File Offset: 0x00051C54
			internal void SetDataContractName(XmlQualifiedName stableName)
			{
				XmlDictionary xmlDictionary = new XmlDictionary(2);
				this.Name = xmlDictionary.Add(stableName.Name);
				this.Namespace = xmlDictionary.Add(stableName.Namespace);
				this.StableName = stableName;
			}

			// Token: 0x0600148A RID: 5258 RVA: 0x00053A93 File Offset: 0x00051C93
			internal void SetDataContractName(XmlDictionaryString name, XmlDictionaryString ns)
			{
				this.Name = name;
				this.Namespace = ns;
				this.StableName = DataContract.CreateQualifiedName(name.Value, ns.Value);
			}

			// Token: 0x0600148B RID: 5259 RVA: 0x00053ABA File Offset: 0x00051CBA
			internal void ThrowInvalidDataContractException(string message)
			{
				DataContract.DataContractCriticalHelper.ThrowInvalidDataContractException(message, this.UnderlyingType);
			}

			// Token: 0x040009EB RID: 2539
			private static Dictionary<TypeHandleRef, IntRef> typeToIDCache = new Dictionary<TypeHandleRef, IntRef>(new TypeHandleRefEqualityComparer());

			// Token: 0x040009EC RID: 2540
			private static DataContract[] dataContractCache = new DataContract[32];

			// Token: 0x040009ED RID: 2541
			private static int dataContractID = 0;

			// Token: 0x040009EE RID: 2542
			private static Dictionary<Type, DataContract> typeToBuiltInContract;

			// Token: 0x040009EF RID: 2543
			private static Dictionary<XmlQualifiedName, DataContract> nameToBuiltInContract;

			// Token: 0x040009F0 RID: 2544
			private static Dictionary<string, DataContract> typeNameToBuiltInContract;

			// Token: 0x040009F1 RID: 2545
			private static Dictionary<string, string> namespaces;

			// Token: 0x040009F2 RID: 2546
			private static Dictionary<string, XmlDictionaryString> clrTypeStrings;

			// Token: 0x040009F3 RID: 2547
			private static XmlDictionary clrTypeStringsDictionary;

			// Token: 0x040009F4 RID: 2548
			private static TypeHandleRef typeHandleRef = new TypeHandleRef();

			// Token: 0x040009F5 RID: 2549
			private static object cacheLock = new object();

			// Token: 0x040009F6 RID: 2550
			private static object createDataContractLock = new object();

			// Token: 0x040009F7 RID: 2551
			private static object initBuiltInContractsLock = new object();

			// Token: 0x040009F8 RID: 2552
			private static object namespacesLock = new object();

			// Token: 0x040009F9 RID: 2553
			private static object clrTypeStringsLock = new object();

			// Token: 0x040009FA RID: 2554
			private readonly Type underlyingType;

			// Token: 0x040009FB RID: 2555
			private Type originalUnderlyingType;

			// Token: 0x040009FC RID: 2556
			private bool isReference;

			// Token: 0x040009FD RID: 2557
			private bool isValueType;

			// Token: 0x040009FE RID: 2558
			private XmlQualifiedName stableName;

			// Token: 0x040009FF RID: 2559
			private GenericInfo genericInfo;

			// Token: 0x04000A00 RID: 2560
			private XmlDictionaryString name;

			// Token: 0x04000A01 RID: 2561
			private XmlDictionaryString ns;

			// Token: 0x04000A02 RID: 2562
			private Type typeForInitialization;

			// Token: 0x04000A03 RID: 2563
			private MethodInfo parseMethod;

			// Token: 0x04000A04 RID: 2564
			private bool parseMethodSet;
		}
	}
}
