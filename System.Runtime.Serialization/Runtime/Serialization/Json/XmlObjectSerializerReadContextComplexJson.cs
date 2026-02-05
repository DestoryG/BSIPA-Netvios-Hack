using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using System.Xml;

namespace System.Runtime.Serialization.Json
{
	// Token: 0x02000119 RID: 281
	internal class XmlObjectSerializerReadContextComplexJson : XmlObjectSerializerReadContextComplex
	{
		// Token: 0x0600110E RID: 4366 RVA: 0x00046ECC File Offset: 0x000450CC
		public XmlObjectSerializerReadContextComplexJson(DataContractJsonSerializer serializer, DataContract rootTypeDataContract)
			: base(serializer, serializer.MaxItemsInObjectGraph, new StreamingContext(StreamingContextStates.All), serializer.IgnoreExtensionDataObject)
		{
			this.rootTypeDataContract = rootTypeDataContract;
			this.serializerKnownTypeList = serializer.knownTypeList;
			this.dataContractSurrogate = serializer.DataContractSurrogate;
			this.dateTimeFormat = serializer.DateTimeFormat;
			this.useSimpleDictionaryFormat = serializer.UseSimpleDictionaryFormat;
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x0600110F RID: 4367 RVA: 0x00046F2D File Offset: 0x0004512D
		internal IList<Type> SerializerKnownTypeList
		{
			get
			{
				return this.serializerKnownTypeList;
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06001110 RID: 4368 RVA: 0x00046F35 File Offset: 0x00045135
		public bool UseSimpleDictionaryFormat
		{
			get
			{
				return this.useSimpleDictionaryFormat;
			}
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x00046F3D File Offset: 0x0004513D
		protected override void StartReadExtensionDataValue(XmlReaderDelegator xmlReader)
		{
			this.extensionDataValueType = xmlReader.GetAttribute("type");
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x00046F50 File Offset: 0x00045150
		protected override IDataNode ReadPrimitiveExtensionDataValue(XmlReaderDelegator xmlReader, string dataContractName, string dataContractNamespace)
		{
			string text = this.extensionDataValueType;
			IDataNode dataNode;
			if (text != null && !(text == "string"))
			{
				if (!(text == "boolean"))
				{
					if (!(text == "number"))
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Unexpected attribute value '{0}'.", new object[] { this.extensionDataValueType })));
					}
					dataNode = this.ReadNumericalPrimitiveExtensionDataValue(xmlReader);
				}
				else
				{
					dataNode = new DataNode<bool>(xmlReader.ReadContentAsBoolean());
				}
			}
			else
			{
				dataNode = new DataNode<string>(xmlReader.ReadContentAsString());
			}
			xmlReader.ReadEndElement();
			return dataNode;
		}

		// Token: 0x06001113 RID: 4371 RVA: 0x00046FE4 File Offset: 0x000451E4
		private IDataNode ReadNumericalPrimitiveExtensionDataValue(XmlReaderDelegator xmlReader)
		{
			TypeCode typeCode;
			object obj = JsonObjectDataContract.ParseJsonNumber(xmlReader.ReadContentAsString(), out typeCode);
			switch (typeCode)
			{
			case TypeCode.SByte:
				return new DataNode<sbyte>((sbyte)obj);
			case TypeCode.Byte:
				return new DataNode<byte>((byte)obj);
			case TypeCode.Int16:
				return new DataNode<short>((short)obj);
			case TypeCode.UInt16:
				return new DataNode<ushort>((ushort)obj);
			case TypeCode.Int32:
				return new DataNode<int>((int)obj);
			case TypeCode.UInt32:
				return new DataNode<uint>((uint)obj);
			case TypeCode.Int64:
				return new DataNode<long>((long)obj);
			case TypeCode.UInt64:
				return new DataNode<ulong>((ulong)obj);
			case TypeCode.Single:
				return new DataNode<float>((float)obj);
			case TypeCode.Double:
				return new DataNode<double>((double)obj);
			case TypeCode.Decimal:
				return new DataNode<decimal>((decimal)obj);
			default:
				throw Fx.AssertAndThrow("JsonObjectDataContract.ParseJsonNumber shouldn't return a TypeCode that we're not expecting");
			}
		}

		// Token: 0x06001114 RID: 4372 RVA: 0x000470C6 File Offset: 0x000452C6
		internal static XmlObjectSerializerReadContextComplexJson CreateContext(DataContractJsonSerializer serializer, DataContract rootTypeDataContract)
		{
			return new XmlObjectSerializerReadContextComplexJson(serializer, rootTypeDataContract);
		}

		// Token: 0x06001115 RID: 4373 RVA: 0x000470CF File Offset: 0x000452CF
		internal override int GetArraySize()
		{
			return -1;
		}

		// Token: 0x06001116 RID: 4374 RVA: 0x000470D2 File Offset: 0x000452D2
		protected override object ReadDataContractValue(DataContract dataContract, XmlReaderDelegator reader)
		{
			return DataContractJsonSerializer.ReadJsonValue(dataContract, reader, this);
		}

		// Token: 0x06001117 RID: 4375 RVA: 0x000470DC File Offset: 0x000452DC
		internal override void ReadAttributes(XmlReaderDelegator xmlReader)
		{
			if (this.attributes == null)
			{
				this.attributes = new Attributes();
			}
			this.attributes.Reset();
			if (xmlReader.MoveToAttribute("type") && xmlReader.Value == "null")
			{
				this.attributes.XsiNil = true;
			}
			else if (xmlReader.MoveToAttribute("__type"))
			{
				XmlQualifiedName xmlQualifiedName = JsonReaderDelegator.ParseQualifiedName(xmlReader.Value);
				this.attributes.XsiTypeName = xmlQualifiedName.Name;
				string text = xmlQualifiedName.Namespace;
				if (!string.IsNullOrEmpty(text))
				{
					char c = text[0];
					if (c != '#')
					{
						if (c == '\\')
						{
							if (text.Length >= 2)
							{
								c = text[1];
								if (c == '#' || c == '\\')
								{
									text = text.Substring(1);
								}
							}
						}
					}
					else
					{
						text = "http://schemas.datacontract.org/2004/07/" + text.Substring(1);
					}
				}
				this.attributes.XsiTypeNamespace = text;
			}
			xmlReader.MoveToElement();
		}

		// Token: 0x06001118 RID: 4376 RVA: 0x000471D4 File Offset: 0x000453D4
		public int GetJsonMemberIndex(XmlReaderDelegator xmlReader, XmlDictionaryString[] memberNames, int memberIndex, ExtensionDataObject extensionData)
		{
			int num = memberNames.Length;
			if (num != 0)
			{
				int i = 0;
				int num2 = (memberIndex + 1) % num;
				while (i < num)
				{
					if (xmlReader.IsStartElement(memberNames[num2], XmlDictionaryString.Empty))
					{
						return num2;
					}
					i++;
					num2 = (num2 + 1) % num;
				}
				string text;
				if (XmlObjectSerializerReadContextComplexJson.TryGetJsonLocalName(xmlReader, out text))
				{
					int j = 0;
					int num3 = (memberIndex + 1) % num;
					while (j < num)
					{
						if (memberNames[num3].Value == text)
						{
							return num3;
						}
						j++;
						num3 = (num3 + 1) % num;
					}
				}
			}
			base.HandleMemberNotFound(xmlReader, extensionData, memberIndex);
			return num;
		}

		// Token: 0x06001119 RID: 4377 RVA: 0x0004725A File Offset: 0x0004545A
		internal static bool TryGetJsonLocalName(XmlReaderDelegator xmlReader, out string name)
		{
			if (xmlReader.IsStartElement(JsonGlobals.itemDictionaryString, JsonGlobals.itemDictionaryString) && xmlReader.MoveToAttribute("item"))
			{
				name = xmlReader.Value;
				return true;
			}
			name = null;
			return false;
		}

		// Token: 0x0600111A RID: 4378 RVA: 0x0004728C File Offset: 0x0004548C
		public static string GetJsonMemberName(XmlReaderDelegator xmlReader)
		{
			string localName;
			if (!XmlObjectSerializerReadContextComplexJson.TryGetJsonLocalName(xmlReader, out localName))
			{
				localName = xmlReader.LocalName;
			}
			return localName;
		}

		// Token: 0x0600111B RID: 4379 RVA: 0x000472AC File Offset: 0x000454AC
		public static void ThrowMissingRequiredMembers(object obj, XmlDictionaryString[] memberNames, byte[] expectedElements, byte[] requiredElements)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			for (int i = 0; i < memberNames.Length; i++)
			{
				if (XmlObjectSerializerReadContextComplexJson.IsBitSet(expectedElements, i) && XmlObjectSerializerReadContextComplexJson.IsBitSet(requiredElements, i))
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(memberNames[i]);
					num++;
				}
			}
			if (num == 1)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new SerializationException(SR.GetString("Required member {1} in type '{0}' is not found.", new object[]
				{
					DataContract.GetClrTypeFullName(obj.GetType()),
					stringBuilder.ToString()
				})));
			}
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new SerializationException(SR.GetString("Required members {0} in type '{1}' are not found.", new object[]
			{
				DataContract.GetClrTypeFullName(obj.GetType()),
				stringBuilder.ToString()
			})));
		}

		// Token: 0x0600111C RID: 4380 RVA: 0x0004736A File Offset: 0x0004556A
		public static void ThrowDuplicateMemberException(object obj, XmlDictionaryString[] memberNames, int memberIndex)
		{
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new SerializationException(SR.GetString("Duplicate member '{0}' is found in JSON input.", new object[]
			{
				DataContract.GetClrTypeFullName(obj.GetType()),
				memberNames[memberIndex]
			})));
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x0004739A File Offset: 0x0004559A
		[SecuritySafeCritical]
		private static bool IsBitSet(byte[] bytes, int bitIndex)
		{
			return BitFlagsGenerator.IsBitSet(bytes, bitIndex);
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x000473A3 File Offset: 0x000455A3
		protected override bool IsReadingCollectionExtensionData(XmlReaderDelegator xmlReader)
		{
			return xmlReader.GetAttribute("type") == "array";
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x000473BA File Offset: 0x000455BA
		protected override bool IsReadingClassExtensionData(XmlReaderDelegator xmlReader)
		{
			return xmlReader.GetAttribute("type") == "object";
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x000473D1 File Offset: 0x000455D1
		protected override XmlReaderDelegator CreateReaderDelegatorForReader(XmlReader xmlReader)
		{
			return new JsonReaderDelegator(xmlReader, this.dateTimeFormat);
		}

		// Token: 0x06001121 RID: 4385 RVA: 0x000473DF File Offset: 0x000455DF
		internal override DataContract GetDataContract(RuntimeTypeHandle typeHandle, Type type)
		{
			DataContract dataContract = base.GetDataContract(typeHandle, type);
			DataContractJsonSerializer.CheckIfTypeIsReference(dataContract);
			return dataContract;
		}

		// Token: 0x06001122 RID: 4386 RVA: 0x000473EF File Offset: 0x000455EF
		internal override DataContract GetDataContractSkipValidation(int typeId, RuntimeTypeHandle typeHandle, Type type)
		{
			DataContract dataContractSkipValidation = base.GetDataContractSkipValidation(typeId, typeHandle, type);
			DataContractJsonSerializer.CheckIfTypeIsReference(dataContractSkipValidation);
			return dataContractSkipValidation;
		}

		// Token: 0x06001123 RID: 4387 RVA: 0x00047400 File Offset: 0x00045600
		internal override DataContract GetDataContract(int id, RuntimeTypeHandle typeHandle)
		{
			DataContract dataContract = base.GetDataContract(id, typeHandle);
			DataContractJsonSerializer.CheckIfTypeIsReference(dataContract);
			return dataContract;
		}

		// Token: 0x06001124 RID: 4388 RVA: 0x00047410 File Offset: 0x00045610
		protected override DataContract ResolveDataContractFromRootDataContract(XmlQualifiedName typeQName)
		{
			return XmlObjectSerializerWriteContextComplexJson.ResolveJsonDataContractFromRootDataContract(this, typeQName, this.rootTypeDataContract);
		}

		// Token: 0x0400085E RID: 2142
		private string extensionDataValueType;

		// Token: 0x0400085F RID: 2143
		private DateTimeFormat dateTimeFormat;

		// Token: 0x04000860 RID: 2144
		private bool useSimpleDictionaryFormat;
	}
}
