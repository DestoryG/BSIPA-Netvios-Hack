using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security;
using System.Threading;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x02000080 RID: 128
	internal sealed class EnumDataContract : DataContract
	{
		// Token: 0x06000947 RID: 2375 RVA: 0x00029DAC File Offset: 0x00027FAC
		[SecuritySafeCritical]
		internal EnumDataContract()
			: base(new EnumDataContract.EnumDataContractCriticalHelper())
		{
			this.helper = base.Helper as EnumDataContract.EnumDataContractCriticalHelper;
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x00029DCA File Offset: 0x00027FCA
		[SecuritySafeCritical]
		internal EnumDataContract(Type type)
			: base(new EnumDataContract.EnumDataContractCriticalHelper(type))
		{
			this.helper = base.Helper as EnumDataContract.EnumDataContractCriticalHelper;
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x00029DE9 File Offset: 0x00027FE9
		[SecuritySafeCritical]
		internal static XmlQualifiedName GetBaseContractName(Type type)
		{
			return EnumDataContract.EnumDataContractCriticalHelper.GetBaseContractName(type);
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x00029DF1 File Offset: 0x00027FF1
		[SecuritySafeCritical]
		internal static Type GetBaseType(XmlQualifiedName baseContractName)
		{
			return EnumDataContract.EnumDataContractCriticalHelper.GetBaseType(baseContractName);
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x0600094B RID: 2379 RVA: 0x00029DF9 File Offset: 0x00027FF9
		// (set) Token: 0x0600094C RID: 2380 RVA: 0x00029E06 File Offset: 0x00028006
		internal XmlQualifiedName BaseContractName
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.BaseContractName;
			}
			[SecurityCritical]
			set
			{
				this.helper.BaseContractName = value;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x0600094D RID: 2381 RVA: 0x00029E14 File Offset: 0x00028014
		// (set) Token: 0x0600094E RID: 2382 RVA: 0x00029E21 File Offset: 0x00028021
		internal List<DataMember> Members
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.Members;
			}
			[SecurityCritical]
			set
			{
				this.helper.Members = value;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x0600094F RID: 2383 RVA: 0x00029E2F File Offset: 0x0002802F
		// (set) Token: 0x06000950 RID: 2384 RVA: 0x00029E3C File Offset: 0x0002803C
		internal List<long> Values
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.Values;
			}
			[SecurityCritical]
			set
			{
				this.helper.Values = value;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000951 RID: 2385 RVA: 0x00029E4A File Offset: 0x0002804A
		// (set) Token: 0x06000952 RID: 2386 RVA: 0x00029E57 File Offset: 0x00028057
		internal bool IsFlags
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.IsFlags;
			}
			[SecurityCritical]
			set
			{
				this.helper.IsFlags = value;
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000953 RID: 2387 RVA: 0x00029E65 File Offset: 0x00028065
		internal bool IsULong
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.IsULong;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000954 RID: 2388 RVA: 0x00029E72 File Offset: 0x00028072
		private XmlDictionaryString[] ChildElementNames
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.ChildElementNames;
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000955 RID: 2389 RVA: 0x00029E7F File Offset: 0x0002807F
		internal override bool CanContainReferences
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x00029E84 File Offset: 0x00028084
		internal void WriteEnumValue(XmlWriterDelegator writer, object value)
		{
			long num = (long)(this.IsULong ? ((IConvertible)value).ToUInt64(null) : ((ulong)((IConvertible)value).ToInt64(null)));
			for (int i = 0; i < this.Values.Count; i++)
			{
				if (num == this.Values[i])
				{
					writer.WriteString(this.ChildElementNames[i].Value);
					return;
				}
			}
			if (!this.IsFlags)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Enum value '{0}' is invalid for type '{1}' and cannot be serialized. Ensure that the necessary enum values are present and are marked with EnumMemberAttribute attribute if the type has DataContractAttribute attribute.", new object[]
				{
					value,
					DataContract.GetClrTypeFullName(base.UnderlyingType)
				})));
			}
			int num2 = -1;
			bool flag = true;
			for (int j = 0; j < this.Values.Count; j++)
			{
				long num3 = this.Values[j];
				if (num3 == 0L)
				{
					num2 = j;
				}
				else
				{
					if (num == 0L)
					{
						break;
					}
					if ((num3 & num) == num3)
					{
						if (flag)
						{
							flag = false;
						}
						else
						{
							writer.WriteString(DictionaryGlobals.Space.Value);
						}
						writer.WriteString(this.ChildElementNames[j].Value);
						num &= ~num3;
					}
				}
			}
			if (num != 0L)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Enum value '{0}' is invalid for type '{1}' and cannot be serialized. Ensure that the necessary enum values are present and are marked with EnumMemberAttribute attribute if the type has DataContractAttribute attribute.", new object[]
				{
					value,
					DataContract.GetClrTypeFullName(base.UnderlyingType)
				})));
			}
			if (flag && num2 >= 0)
			{
				writer.WriteString(this.ChildElementNames[num2].Value);
				return;
			}
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x00029FE4 File Offset: 0x000281E4
		internal object ReadEnumValue(XmlReaderDelegator reader)
		{
			string text = reader.ReadElementContentAsString();
			long num = 0L;
			int i = 0;
			if (this.IsFlags)
			{
				while (i < text.Length && text[i] == ' ')
				{
					i++;
				}
				int num2 = i;
				int num3;
				while (i < text.Length)
				{
					if (text[i] == ' ')
					{
						num3 = i - num2;
						if (num3 > 0)
						{
							num |= this.ReadEnumValue(text, num2, num3);
						}
						i++;
						while (i < text.Length && text[i] == ' ')
						{
							i++;
						}
						num2 = i;
						if (i == text.Length)
						{
							break;
						}
					}
					i++;
				}
				num3 = i - num2;
				if (num3 > 0)
				{
					num |= this.ReadEnumValue(text, num2, num3);
				}
			}
			else
			{
				if (text.Length == 0)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Invalid enum value '{0}' cannot be deserialized into type '{1}'. Ensure that the necessary enum values are present and are marked with EnumMemberAttribute attribute if the type has DataContractAttribute attribute.", new object[]
					{
						text,
						DataContract.GetClrTypeFullName(base.UnderlyingType)
					})));
				}
				num = this.ReadEnumValue(text, 0, text.Length);
			}
			if (this.IsULong)
			{
				return Enum.ToObject(base.UnderlyingType, (ulong)num);
			}
			return Enum.ToObject(base.UnderlyingType, num);
		}

		// Token: 0x06000958 RID: 2392 RVA: 0x0002A100 File Offset: 0x00028300
		private long ReadEnumValue(string value, int index, int count)
		{
			for (int i = 0; i < this.Members.Count; i++)
			{
				string name = this.Members[i].Name;
				if (name.Length == count && string.CompareOrdinal(value, index, name, 0, count) == 0)
				{
					return this.Values[i];
				}
			}
			throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Invalid enum value '{0}' cannot be deserialized into type '{1}'. Ensure that the necessary enum values are present and are marked with EnumMemberAttribute attribute if the type has DataContractAttribute attribute.", new object[]
			{
				value.Substring(index, count),
				DataContract.GetClrTypeFullName(base.UnderlyingType)
			})));
		}

		// Token: 0x06000959 RID: 2393 RVA: 0x0002A18A File Offset: 0x0002838A
		internal string GetStringFromEnumValue(long value)
		{
			if (this.IsULong)
			{
				return XmlConvert.ToString((ulong)value);
			}
			return XmlConvert.ToString(value);
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x0002A1A1 File Offset: 0x000283A1
		internal long GetEnumValueFromString(string value)
		{
			if (this.IsULong)
			{
				return (long)XmlConverter.ToUInt64(value);
			}
			return XmlConverter.ToInt64(value);
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x0002A1B8 File Offset: 0x000283B8
		internal override bool Equals(object other, Dictionary<DataContractPairKey, object> checkedContracts)
		{
			if (base.IsEqualOrChecked(other, checkedContracts))
			{
				return true;
			}
			if (base.Equals(other, null))
			{
				EnumDataContract enumDataContract = other as EnumDataContract;
				if (enumDataContract != null)
				{
					if (this.Members.Count != enumDataContract.Members.Count || this.Values.Count != enumDataContract.Values.Count)
					{
						return false;
					}
					string[] array = new string[this.Members.Count];
					string[] array2 = new string[this.Members.Count];
					for (int i = 0; i < this.Members.Count; i++)
					{
						array[i] = this.Members[i].Name;
						array2[i] = enumDataContract.Members[i].Name;
					}
					Array.Sort<string>(array);
					Array.Sort<string>(array2);
					for (int j = 0; j < this.Members.Count; j++)
					{
						if (array[j] != array2[j])
						{
							return false;
						}
					}
					return this.IsFlags == enumDataContract.IsFlags;
				}
			}
			return false;
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x0002A2C4 File Offset: 0x000284C4
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0600095D RID: 2397 RVA: 0x0002A2CC File Offset: 0x000284CC
		public override void WriteXmlValue(XmlWriterDelegator xmlWriter, object obj, XmlObjectSerializerWriteContext context)
		{
			this.WriteEnumValue(xmlWriter, obj);
		}

		// Token: 0x0600095E RID: 2398 RVA: 0x0002A2D8 File Offset: 0x000284D8
		public override object ReadXmlValue(XmlReaderDelegator xmlReader, XmlObjectSerializerReadContext context)
		{
			object obj = this.ReadEnumValue(xmlReader);
			if (context != null)
			{
				context.AddNewObject(obj);
			}
			return obj;
		}

		// Token: 0x04000395 RID: 917
		[SecurityCritical]
		private EnumDataContract.EnumDataContractCriticalHelper helper;

		// Token: 0x02000173 RID: 371
		[SecurityCritical(SecurityCriticalScope.Everything)]
		private class EnumDataContractCriticalHelper : DataContract.DataContractCriticalHelper
		{
			// Token: 0x060014A4 RID: 5284 RVA: 0x00053C78 File Offset: 0x00051E78
			static EnumDataContractCriticalHelper()
			{
				EnumDataContract.EnumDataContractCriticalHelper.Add(typeof(sbyte), "byte");
				EnumDataContract.EnumDataContractCriticalHelper.Add(typeof(byte), "unsignedByte");
				EnumDataContract.EnumDataContractCriticalHelper.Add(typeof(short), "short");
				EnumDataContract.EnumDataContractCriticalHelper.Add(typeof(ushort), "unsignedShort");
				EnumDataContract.EnumDataContractCriticalHelper.Add(typeof(int), "int");
				EnumDataContract.EnumDataContractCriticalHelper.Add(typeof(uint), "unsignedInt");
				EnumDataContract.EnumDataContractCriticalHelper.Add(typeof(long), "long");
				EnumDataContract.EnumDataContractCriticalHelper.Add(typeof(ulong), "unsignedLong");
			}

			// Token: 0x060014A5 RID: 5285 RVA: 0x00053D3C File Offset: 0x00051F3C
			internal static void Add(Type type, string localName)
			{
				XmlQualifiedName xmlQualifiedName = DataContract.CreateQualifiedName(localName, "http://www.w3.org/2001/XMLSchema");
				EnumDataContract.EnumDataContractCriticalHelper.typeToName.Add(type, xmlQualifiedName);
				EnumDataContract.EnumDataContractCriticalHelper.nameToType.Add(xmlQualifiedName, type);
			}

			// Token: 0x060014A6 RID: 5286 RVA: 0x00053D70 File Offset: 0x00051F70
			internal static XmlQualifiedName GetBaseContractName(Type type)
			{
				XmlQualifiedName xmlQualifiedName = null;
				EnumDataContract.EnumDataContractCriticalHelper.typeToName.TryGetValue(type, out xmlQualifiedName);
				return xmlQualifiedName;
			}

			// Token: 0x060014A7 RID: 5287 RVA: 0x00053D90 File Offset: 0x00051F90
			internal static Type GetBaseType(XmlQualifiedName baseContractName)
			{
				Type type = null;
				EnumDataContract.EnumDataContractCriticalHelper.nameToType.TryGetValue(baseContractName, out type);
				return type;
			}

			// Token: 0x060014A8 RID: 5288 RVA: 0x00053DAE File Offset: 0x00051FAE
			internal EnumDataContractCriticalHelper()
			{
				base.IsValueType = true;
			}

			// Token: 0x060014A9 RID: 5289 RVA: 0x00053DC0 File Offset: 0x00051FC0
			internal EnumDataContractCriticalHelper(Type type)
				: base(type)
			{
				base.StableName = DataContract.GetStableName(type, out this.hasDataContract);
				Type underlyingType = Enum.GetUnderlyingType(type);
				this.baseContractName = EnumDataContract.EnumDataContractCriticalHelper.GetBaseContractName(underlyingType);
				this.ImportBaseType(underlyingType);
				this.IsFlags = type.IsDefined(Globals.TypeOfFlagsAttribute, false);
				this.ImportDataMembers();
				XmlDictionary xmlDictionary = new XmlDictionary(2 + this.Members.Count);
				base.Name = xmlDictionary.Add(base.StableName.Name);
				base.Namespace = xmlDictionary.Add(base.StableName.Namespace);
				this.childElementNames = new XmlDictionaryString[this.Members.Count];
				for (int i = 0; i < this.Members.Count; i++)
				{
					this.childElementNames[i] = xmlDictionary.Add(this.Members[i].Name);
				}
				DataContractAttribute dataContractAttribute;
				if (DataContract.TryGetDCAttribute(type, out dataContractAttribute) && dataContractAttribute.IsReference)
				{
					DataContract.ThrowInvalidDataContractException(SR.GetString("Enum type '{0}' cannot have the IsReference setting of '{1}'. Either change the setting to '{2}', or remove it completely.", new object[]
					{
						DataContract.GetClrTypeFullName(type),
						dataContractAttribute.IsReference,
						false
					}), type);
				}
			}

			// Token: 0x17000447 RID: 1095
			// (get) Token: 0x060014AA RID: 5290 RVA: 0x00053EEC File Offset: 0x000520EC
			// (set) Token: 0x060014AB RID: 5291 RVA: 0x00053EF4 File Offset: 0x000520F4
			internal XmlQualifiedName BaseContractName
			{
				get
				{
					return this.baseContractName;
				}
				set
				{
					this.baseContractName = value;
					Type baseType = EnumDataContract.EnumDataContractCriticalHelper.GetBaseType(this.baseContractName);
					if (baseType == null)
					{
						base.ThrowInvalidDataContractException(SR.GetString("Invalid enum base type is specified for type '{0}' in '{1}' namespace, element name is '{2}' in '{3}' namespace.", new object[]
						{
							value.Name,
							value.Namespace,
							base.StableName.Name,
							base.StableName.Namespace
						}));
					}
					this.ImportBaseType(baseType);
				}
			}

			// Token: 0x17000448 RID: 1096
			// (get) Token: 0x060014AC RID: 5292 RVA: 0x00053F68 File Offset: 0x00052168
			// (set) Token: 0x060014AD RID: 5293 RVA: 0x00053F70 File Offset: 0x00052170
			internal List<DataMember> Members
			{
				get
				{
					return this.members;
				}
				set
				{
					this.members = value;
				}
			}

			// Token: 0x17000449 RID: 1097
			// (get) Token: 0x060014AE RID: 5294 RVA: 0x00053F79 File Offset: 0x00052179
			// (set) Token: 0x060014AF RID: 5295 RVA: 0x00053F81 File Offset: 0x00052181
			internal List<long> Values
			{
				get
				{
					return this.values;
				}
				set
				{
					this.values = value;
				}
			}

			// Token: 0x1700044A RID: 1098
			// (get) Token: 0x060014B0 RID: 5296 RVA: 0x00053F8A File Offset: 0x0005218A
			// (set) Token: 0x060014B1 RID: 5297 RVA: 0x00053F92 File Offset: 0x00052192
			internal bool IsFlags
			{
				get
				{
					return this.isFlags;
				}
				set
				{
					this.isFlags = value;
				}
			}

			// Token: 0x1700044B RID: 1099
			// (get) Token: 0x060014B2 RID: 5298 RVA: 0x00053F9B File Offset: 0x0005219B
			internal bool IsULong
			{
				get
				{
					return this.isULong;
				}
			}

			// Token: 0x1700044C RID: 1100
			// (get) Token: 0x060014B3 RID: 5299 RVA: 0x00053FA3 File Offset: 0x000521A3
			internal XmlDictionaryString[] ChildElementNames
			{
				get
				{
					return this.childElementNames;
				}
			}

			// Token: 0x060014B4 RID: 5300 RVA: 0x00053FAB File Offset: 0x000521AB
			private void ImportBaseType(Type baseType)
			{
				this.isULong = baseType == Globals.TypeOfULong;
			}

			// Token: 0x060014B5 RID: 5301 RVA: 0x00053FC0 File Offset: 0x000521C0
			private void ImportDataMembers()
			{
				Type underlyingType = base.UnderlyingType;
				FieldInfo[] fields = underlyingType.GetFields(BindingFlags.Static | BindingFlags.Public);
				Dictionary<string, DataMember> dictionary = new Dictionary<string, DataMember>();
				List<DataMember> list = new List<DataMember>(fields.Length);
				List<long> list2 = new List<long>(fields.Length);
				foreach (FieldInfo fieldInfo in fields)
				{
					bool flag = false;
					if (this.hasDataContract)
					{
						object[] customAttributes = fieldInfo.GetCustomAttributes(Globals.TypeOfEnumMemberAttribute, false);
						if (customAttributes != null && customAttributes.Length != 0)
						{
							if (customAttributes.Length > 1)
							{
								base.ThrowInvalidDataContractException(SR.GetString("Member '{0}.{1}' has more than one EnumMemberAttribute attribute.", new object[]
								{
									DataContract.GetClrTypeFullName(fieldInfo.DeclaringType),
									fieldInfo.Name
								}));
							}
							EnumMemberAttribute enumMemberAttribute = (EnumMemberAttribute)customAttributes[0];
							DataMember dataMember = new DataMember(fieldInfo);
							if (enumMemberAttribute.IsValueSetExplicitly)
							{
								if (enumMemberAttribute.Value == null || enumMemberAttribute.Value.Length == 0)
								{
									base.ThrowInvalidDataContractException(SR.GetString("'{0}' in type '{1}' cannot have EnumMemberAttribute attribute Value set to null or empty string.", new object[]
									{
										fieldInfo.Name,
										DataContract.GetClrTypeFullName(underlyingType)
									}));
								}
								dataMember.Name = enumMemberAttribute.Value;
							}
							else
							{
								dataMember.Name = fieldInfo.Name;
							}
							ClassDataContract.CheckAndAddMember(list, dataMember, dictionary);
							flag = true;
						}
						object[] customAttributes2 = fieldInfo.GetCustomAttributes(Globals.TypeOfDataMemberAttribute, false);
						if (customAttributes2 != null && customAttributes2.Length != 0)
						{
							base.ThrowInvalidDataContractException(SR.GetString("Member '{0}.{1}' has DataMemberAttribute attribute. Use EnumMemberAttribute attribute instead.", new object[]
							{
								DataContract.GetClrTypeFullName(fieldInfo.DeclaringType),
								fieldInfo.Name
							}));
						}
					}
					else if (!fieldInfo.IsNotSerialized)
					{
						ClassDataContract.CheckAndAddMember(list, new DataMember(fieldInfo)
						{
							Name = fieldInfo.Name
						}, dictionary);
						flag = true;
					}
					if (flag)
					{
						object value = fieldInfo.GetValue(null);
						if (this.isULong)
						{
							list2.Add((long)((IConvertible)value).ToUInt64(null));
						}
						else
						{
							list2.Add(((IConvertible)value).ToInt64(null));
						}
					}
				}
				Thread.MemoryBarrier();
				this.members = list;
				this.values = list2;
			}

			// Token: 0x04000A0F RID: 2575
			private static Dictionary<Type, XmlQualifiedName> typeToName = new Dictionary<Type, XmlQualifiedName>();

			// Token: 0x04000A10 RID: 2576
			private static Dictionary<XmlQualifiedName, Type> nameToType = new Dictionary<XmlQualifiedName, Type>();

			// Token: 0x04000A11 RID: 2577
			private XmlQualifiedName baseContractName;

			// Token: 0x04000A12 RID: 2578
			private List<DataMember> members;

			// Token: 0x04000A13 RID: 2579
			private List<long> values;

			// Token: 0x04000A14 RID: 2580
			private bool isULong;

			// Token: 0x04000A15 RID: 2581
			private bool isFlags;

			// Token: 0x04000A16 RID: 2582
			private bool hasDataContract;

			// Token: 0x04000A17 RID: 2583
			private XmlDictionaryString[] childElementNames;
		}
	}
}
