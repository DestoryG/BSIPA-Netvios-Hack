using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security;
using System.Threading;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Runtime.Serialization
{
	// Token: 0x020000DC RID: 220
	internal sealed class XmlDataContract : DataContract
	{
		// Token: 0x06000C30 RID: 3120 RVA: 0x000346C0 File Offset: 0x000328C0
		[SecuritySafeCritical]
		internal XmlDataContract()
			: base(new XmlDataContract.XmlDataContractCriticalHelper())
		{
			this.helper = base.Helper as XmlDataContract.XmlDataContractCriticalHelper;
		}

		// Token: 0x06000C31 RID: 3121 RVA: 0x000346DE File Offset: 0x000328DE
		[SecuritySafeCritical]
		internal XmlDataContract(Type type)
			: base(new XmlDataContract.XmlDataContractCriticalHelper(type))
		{
			this.helper = base.Helper as XmlDataContract.XmlDataContractCriticalHelper;
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000C32 RID: 3122 RVA: 0x000346FD File Offset: 0x000328FD
		// (set) Token: 0x06000C33 RID: 3123 RVA: 0x0003470A File Offset: 0x0003290A
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

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000C34 RID: 3124 RVA: 0x00034718 File Offset: 0x00032918
		// (set) Token: 0x06000C35 RID: 3125 RVA: 0x00034725 File Offset: 0x00032925
		internal XmlSchemaType XsdType
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.XsdType;
			}
			[SecurityCritical]
			set
			{
				this.helper.XsdType = value;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000C36 RID: 3126 RVA: 0x00034733 File Offset: 0x00032933
		internal bool IsAnonymous
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.IsAnonymous;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000C37 RID: 3127 RVA: 0x00034740 File Offset: 0x00032940
		// (set) Token: 0x06000C38 RID: 3128 RVA: 0x0003474D File Offset: 0x0003294D
		internal override bool HasRoot
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.HasRoot;
			}
			[SecurityCritical]
			set
			{
				this.helper.HasRoot = value;
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000C39 RID: 3129 RVA: 0x0003475B File Offset: 0x0003295B
		// (set) Token: 0x06000C3A RID: 3130 RVA: 0x00034768 File Offset: 0x00032968
		internal override XmlDictionaryString TopLevelElementName
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

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000C3B RID: 3131 RVA: 0x00034776 File Offset: 0x00032976
		// (set) Token: 0x06000C3C RID: 3132 RVA: 0x00034783 File Offset: 0x00032983
		internal override XmlDictionaryString TopLevelElementNamespace
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

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000C3D RID: 3133 RVA: 0x00034791 File Offset: 0x00032991
		// (set) Token: 0x06000C3E RID: 3134 RVA: 0x0003479E File Offset: 0x0003299E
		internal bool IsTopLevelElementNullable
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.IsTopLevelElementNullable;
			}
			[SecurityCritical]
			set
			{
				this.helper.IsTopLevelElementNullable = value;
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000C3F RID: 3135 RVA: 0x000347AC File Offset: 0x000329AC
		// (set) Token: 0x06000C40 RID: 3136 RVA: 0x000347B9 File Offset: 0x000329B9
		internal bool IsTypeDefinedOnImport
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.IsTypeDefinedOnImport;
			}
			[SecurityCritical]
			set
			{
				this.helper.IsTypeDefinedOnImport = value;
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000C41 RID: 3137 RVA: 0x000347C8 File Offset: 0x000329C8
		internal CreateXmlSerializableDelegate CreateXmlSerializableDelegate
		{
			[SecuritySafeCritical]
			get
			{
				if (this.helper.CreateXmlSerializableDelegate == null)
				{
					lock (this)
					{
						if (this.helper.CreateXmlSerializableDelegate == null)
						{
							CreateXmlSerializableDelegate createXmlSerializableDelegate = this.GenerateCreateXmlSerializableDelegate();
							Thread.MemoryBarrier();
							this.helper.CreateXmlSerializableDelegate = createXmlSerializableDelegate;
						}
					}
				}
				return this.helper.CreateXmlSerializableDelegate;
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000C42 RID: 3138 RVA: 0x0003483C File Offset: 0x00032A3C
		internal override bool CanContainReferences
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000C43 RID: 3139 RVA: 0x0003483F File Offset: 0x00032A3F
		internal override bool IsBuiltInDataContract
		{
			get
			{
				return base.UnderlyingType == Globals.TypeOfXmlElement || base.UnderlyingType == Globals.TypeOfXmlNodeArray;
			}
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x00034868 File Offset: 0x00032A68
		private ConstructorInfo GetConstructor()
		{
			Type underlyingType = base.UnderlyingType;
			if (underlyingType.IsValueType)
			{
				return null;
			}
			ConstructorInfo constructor = underlyingType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Globals.EmptyTypeArray, null);
			if (constructor == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("IXmlSerializable Type '{0}' must have default constructor.", new object[] { DataContract.GetClrTypeFullName(underlyingType) })));
			}
			return constructor;
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x000348C4 File Offset: 0x00032AC4
		[SecurityCritical]
		internal void SetTopLevelElementName(XmlQualifiedName elementName)
		{
			if (elementName != null)
			{
				XmlDictionary xmlDictionary = new XmlDictionary();
				this.TopLevelElementName = xmlDictionary.Add(elementName.Name);
				this.TopLevelElementNamespace = xmlDictionary.Add(elementName.Namespace);
			}
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x00034904 File Offset: 0x00032B04
		internal override bool Equals(object other, Dictionary<DataContractPairKey, object> checkedContracts)
		{
			if (base.IsEqualOrChecked(other, checkedContracts))
			{
				return true;
			}
			XmlDataContract xmlDataContract = other as XmlDataContract;
			if (xmlDataContract == null)
			{
				return false;
			}
			if (this.HasRoot != xmlDataContract.HasRoot)
			{
				return false;
			}
			if (this.IsAnonymous)
			{
				return xmlDataContract.IsAnonymous;
			}
			return base.StableName.Name == xmlDataContract.StableName.Name && base.StableName.Namespace == xmlDataContract.StableName.Namespace;
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x00034982 File Offset: 0x00032B82
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x0003498A File Offset: 0x00032B8A
		public override void WriteXmlValue(XmlWriterDelegator xmlWriter, object obj, XmlObjectSerializerWriteContext context)
		{
			if (context == null)
			{
				XmlObjectSerializerWriteContext.WriteRootIXmlSerializable(xmlWriter, obj);
				return;
			}
			context.WriteIXmlSerializable(xmlWriter, obj);
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x000349A0 File Offset: 0x00032BA0
		public override object ReadXmlValue(XmlReaderDelegator xmlReader, XmlObjectSerializerReadContext context)
		{
			object obj;
			if (context == null)
			{
				obj = XmlObjectSerializerReadContext.ReadRootIXmlSerializable(xmlReader, this, true);
			}
			else
			{
				obj = context.ReadIXmlSerializable(xmlReader, this, true);
				context.AddNewObject(obj);
			}
			xmlReader.ReadEndElement();
			return obj;
		}

		// Token: 0x06000C4A RID: 3146 RVA: 0x000349D3 File Offset: 0x00032BD3
		internal CreateXmlSerializableDelegate GenerateCreateXmlSerializableDelegate()
		{
			return () => new XmlDataContractInterpreter(this).CreateXmlSerializable();
		}

		// Token: 0x040004FB RID: 1275
		[SecurityCritical]
		private XmlDataContract.XmlDataContractCriticalHelper helper;

		// Token: 0x0200017B RID: 379
		[SecurityCritical(SecurityCriticalScope.Everything)]
		private class XmlDataContractCriticalHelper : DataContract.DataContractCriticalHelper
		{
			// Token: 0x060014D3 RID: 5331 RVA: 0x00054603 File Offset: 0x00052803
			internal XmlDataContractCriticalHelper()
			{
			}

			// Token: 0x060014D4 RID: 5332 RVA: 0x0005460C File Offset: 0x0005280C
			internal XmlDataContractCriticalHelper(Type type)
				: base(type)
			{
				if (type.IsDefined(Globals.TypeOfDataContractAttribute, false))
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Type '{0}' cannot be IXmlSerializable and have DataContractAttribute attribute.", new object[] { DataContract.GetClrTypeFullName(type) })));
				}
				if (type.IsDefined(Globals.TypeOfCollectionDataContractAttribute, false))
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Type '{0}' cannot be IXmlSerializable and have CollectionDataContractAttribute attribute.", new object[] { DataContract.GetClrTypeFullName(type) })));
				}
				XmlQualifiedName xmlQualifiedName;
				XmlSchemaType xmlSchemaType;
				bool flag;
				SchemaExporter.GetXmlTypeInfo(type, out xmlQualifiedName, out xmlSchemaType, out flag);
				base.StableName = xmlQualifiedName;
				this.XsdType = xmlSchemaType;
				this.HasRoot = flag;
				XmlDictionary xmlDictionary = new XmlDictionary();
				base.Name = xmlDictionary.Add(base.StableName.Name);
				base.Namespace = xmlDictionary.Add(base.StableName.Namespace);
				object[] array = ((base.UnderlyingType == null) ? null : base.UnderlyingType.GetCustomAttributes(Globals.TypeOfXmlRootAttribute, false));
				if (array == null || array.Length == 0)
				{
					if (flag)
					{
						this.topLevelElementName = base.Name;
						this.topLevelElementNamespace = ((base.StableName.Namespace == "http://www.w3.org/2001/XMLSchema") ? DictionaryGlobals.EmptyString : base.Namespace);
						this.isTopLevelElementNullable = true;
						return;
					}
					return;
				}
				else
				{
					if (flag)
					{
						XmlRootAttribute xmlRootAttribute = (XmlRootAttribute)array[0];
						this.isTopLevelElementNullable = xmlRootAttribute.IsNullable;
						string elementName = xmlRootAttribute.ElementName;
						this.topLevelElementName = ((elementName == null || elementName.Length == 0) ? base.Name : xmlDictionary.Add(DataContract.EncodeLocalName(elementName)));
						string @namespace = xmlRootAttribute.Namespace;
						this.topLevelElementNamespace = ((@namespace == null || @namespace.Length == 0) ? DictionaryGlobals.EmptyString : xmlDictionary.Add(@namespace));
						return;
					}
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Type '{0}' cannot specify an XmlRootAttribute attribute because its IsAny setting is 'true'. This type must write all its contents including the root element. Verify that the IXmlSerializable implementation is correct.", new object[] { DataContract.GetClrTypeFullName(base.UnderlyingType) })));
				}
			}

			// Token: 0x17000456 RID: 1110
			// (get) Token: 0x060014D5 RID: 5333 RVA: 0x000547EC File Offset: 0x000529EC
			// (set) Token: 0x060014D6 RID: 5334 RVA: 0x00054864 File Offset: 0x00052A64
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

			// Token: 0x17000457 RID: 1111
			// (get) Token: 0x060014D7 RID: 5335 RVA: 0x0005486D File Offset: 0x00052A6D
			// (set) Token: 0x060014D8 RID: 5336 RVA: 0x00054875 File Offset: 0x00052A75
			internal XmlSchemaType XsdType
			{
				get
				{
					return this.xsdType;
				}
				set
				{
					this.xsdType = value;
				}
			}

			// Token: 0x17000458 RID: 1112
			// (get) Token: 0x060014D9 RID: 5337 RVA: 0x0005487E File Offset: 0x00052A7E
			internal bool IsAnonymous
			{
				get
				{
					return this.xsdType != null;
				}
			}

			// Token: 0x17000459 RID: 1113
			// (get) Token: 0x060014DA RID: 5338 RVA: 0x00054889 File Offset: 0x00052A89
			// (set) Token: 0x060014DB RID: 5339 RVA: 0x00054891 File Offset: 0x00052A91
			internal override bool HasRoot
			{
				get
				{
					return this.hasRoot;
				}
				set
				{
					this.hasRoot = value;
				}
			}

			// Token: 0x1700045A RID: 1114
			// (get) Token: 0x060014DC RID: 5340 RVA: 0x0005489A File Offset: 0x00052A9A
			// (set) Token: 0x060014DD RID: 5341 RVA: 0x000548A2 File Offset: 0x00052AA2
			internal override XmlDictionaryString TopLevelElementName
			{
				get
				{
					return this.topLevelElementName;
				}
				set
				{
					this.topLevelElementName = value;
				}
			}

			// Token: 0x1700045B RID: 1115
			// (get) Token: 0x060014DE RID: 5342 RVA: 0x000548AB File Offset: 0x00052AAB
			// (set) Token: 0x060014DF RID: 5343 RVA: 0x000548B3 File Offset: 0x00052AB3
			internal override XmlDictionaryString TopLevelElementNamespace
			{
				get
				{
					return this.topLevelElementNamespace;
				}
				set
				{
					this.topLevelElementNamespace = value;
				}
			}

			// Token: 0x1700045C RID: 1116
			// (get) Token: 0x060014E0 RID: 5344 RVA: 0x000548BC File Offset: 0x00052ABC
			// (set) Token: 0x060014E1 RID: 5345 RVA: 0x000548C4 File Offset: 0x00052AC4
			internal bool IsTopLevelElementNullable
			{
				get
				{
					return this.isTopLevelElementNullable;
				}
				set
				{
					this.isTopLevelElementNullable = value;
				}
			}

			// Token: 0x1700045D RID: 1117
			// (get) Token: 0x060014E2 RID: 5346 RVA: 0x000548CD File Offset: 0x00052ACD
			// (set) Token: 0x060014E3 RID: 5347 RVA: 0x000548D5 File Offset: 0x00052AD5
			internal bool IsTypeDefinedOnImport
			{
				get
				{
					return this.isTypeDefinedOnImport;
				}
				set
				{
					this.isTypeDefinedOnImport = value;
				}
			}

			// Token: 0x1700045E RID: 1118
			// (get) Token: 0x060014E4 RID: 5348 RVA: 0x000548DE File Offset: 0x00052ADE
			// (set) Token: 0x060014E5 RID: 5349 RVA: 0x000548E6 File Offset: 0x00052AE6
			internal CreateXmlSerializableDelegate CreateXmlSerializableDelegate
			{
				get
				{
					return this.createXmlSerializable;
				}
				set
				{
					this.createXmlSerializable = value;
				}
			}

			// Token: 0x04000A2F RID: 2607
			private Dictionary<XmlQualifiedName, DataContract> knownDataContracts;

			// Token: 0x04000A30 RID: 2608
			private bool isKnownTypeAttributeChecked;

			// Token: 0x04000A31 RID: 2609
			private XmlDictionaryString topLevelElementName;

			// Token: 0x04000A32 RID: 2610
			private XmlDictionaryString topLevelElementNamespace;

			// Token: 0x04000A33 RID: 2611
			private bool isTopLevelElementNullable;

			// Token: 0x04000A34 RID: 2612
			private bool isTypeDefinedOnImport;

			// Token: 0x04000A35 RID: 2613
			private XmlSchemaType xsdType;

			// Token: 0x04000A36 RID: 2614
			private bool hasRoot;

			// Token: 0x04000A37 RID: 2615
			private CreateXmlSerializableDelegate createXmlSerializable;
		}
	}
}
