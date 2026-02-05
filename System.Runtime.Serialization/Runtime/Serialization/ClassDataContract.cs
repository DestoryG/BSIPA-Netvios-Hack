using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security;
using System.Threading;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x02000065 RID: 101
	internal sealed class ClassDataContract : DataContract
	{
		// Token: 0x06000712 RID: 1810 RVA: 0x00020A91 File Offset: 0x0001EC91
		[SecuritySafeCritical]
		internal ClassDataContract()
			: base(new ClassDataContract.ClassDataContractCriticalHelper())
		{
			this.InitClassDataContract();
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x00020AA4 File Offset: 0x0001ECA4
		[SecuritySafeCritical]
		internal ClassDataContract(Type type)
			: base(new ClassDataContract.ClassDataContractCriticalHelper(type))
		{
			this.InitClassDataContract();
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x00020AB8 File Offset: 0x0001ECB8
		[SecuritySafeCritical]
		private ClassDataContract(Type type, XmlDictionaryString ns, string[] memberNames)
			: base(new ClassDataContract.ClassDataContractCriticalHelper(type, ns, memberNames))
		{
			this.InitClassDataContract();
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x00020AD0 File Offset: 0x0001ECD0
		[SecurityCritical]
		private void InitClassDataContract()
		{
			this.helper = base.Helper as ClassDataContract.ClassDataContractCriticalHelper;
			this.ContractNamespaces = this.helper.ContractNamespaces;
			this.MemberNames = this.helper.MemberNames;
			this.MemberNamespaces = this.helper.MemberNamespaces;
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000716 RID: 1814 RVA: 0x00020B21 File Offset: 0x0001ED21
		// (set) Token: 0x06000717 RID: 1815 RVA: 0x00020B2E File Offset: 0x0001ED2E
		internal ClassDataContract BaseContract
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.BaseContract;
			}
			[SecurityCritical]
			set
			{
				this.helper.BaseContract = value;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000718 RID: 1816 RVA: 0x00020B3C File Offset: 0x0001ED3C
		// (set) Token: 0x06000719 RID: 1817 RVA: 0x00020B49 File Offset: 0x0001ED49
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

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x0600071A RID: 1818 RVA: 0x00020B58 File Offset: 0x0001ED58
		public XmlDictionaryString[] ChildElementNamespaces
		{
			[SecuritySafeCritical]
			get
			{
				if (this.childElementNamespaces == null)
				{
					lock (this)
					{
						if (this.childElementNamespaces == null)
						{
							if (this.helper.ChildElementNamespaces == null)
							{
								XmlDictionaryString[] array = this.CreateChildElementNamespaces();
								Thread.MemoryBarrier();
								this.helper.ChildElementNamespaces = array;
							}
							this.childElementNamespaces = this.helper.ChildElementNamespaces;
						}
					}
				}
				return this.childElementNamespaces;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x0600071B RID: 1819 RVA: 0x00020BDC File Offset: 0x0001EDDC
		internal MethodInfo OnSerializing
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.OnSerializing;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600071C RID: 1820 RVA: 0x00020BE9 File Offset: 0x0001EDE9
		internal MethodInfo OnSerialized
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.OnSerialized;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600071D RID: 1821 RVA: 0x00020BF6 File Offset: 0x0001EDF6
		internal MethodInfo OnDeserializing
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.OnDeserializing;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x0600071E RID: 1822 RVA: 0x00020C03 File Offset: 0x0001EE03
		internal MethodInfo OnDeserialized
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.OnDeserialized;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600071F RID: 1823 RVA: 0x00020C10 File Offset: 0x0001EE10
		internal MethodInfo ExtensionDataSetMethod
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.ExtensionDataSetMethod;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000720 RID: 1824 RVA: 0x00020C1D File Offset: 0x0001EE1D
		// (set) Token: 0x06000721 RID: 1825 RVA: 0x00020C2A File Offset: 0x0001EE2A
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

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000722 RID: 1826 RVA: 0x00020C38 File Offset: 0x0001EE38
		// (set) Token: 0x06000723 RID: 1827 RVA: 0x00020C45 File Offset: 0x0001EE45
		internal override bool IsISerializable
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

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000724 RID: 1828 RVA: 0x00020C53 File Offset: 0x0001EE53
		internal bool IsNonAttributedType
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.IsNonAttributedType;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000725 RID: 1829 RVA: 0x00020C60 File Offset: 0x0001EE60
		internal bool HasDataContract
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.HasDataContract;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000726 RID: 1830 RVA: 0x00020C6D File Offset: 0x0001EE6D
		internal bool HasExtensionData
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.HasExtensionData;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000727 RID: 1831 RVA: 0x00020C7A File Offset: 0x0001EE7A
		internal string SerializationExceptionMessage
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.SerializationExceptionMessage;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000728 RID: 1832 RVA: 0x00020C87 File Offset: 0x0001EE87
		internal string DeserializationExceptionMessage
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.DeserializationExceptionMessage;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000729 RID: 1833 RVA: 0x00020C94 File Offset: 0x0001EE94
		internal bool IsReadOnlyContract
		{
			get
			{
				return this.DeserializationExceptionMessage != null;
			}
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x00020C9F File Offset: 0x0001EE9F
		[SecuritySafeCritical]
		internal ConstructorInfo GetISerializableConstructor()
		{
			return this.helper.GetISerializableConstructor();
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x00020CAC File Offset: 0x0001EEAC
		[SecuritySafeCritical]
		internal ConstructorInfo GetNonAttributedTypeConstructor()
		{
			return this.helper.GetNonAttributedTypeConstructor();
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600072C RID: 1836 RVA: 0x00020CBC File Offset: 0x0001EEBC
		internal XmlFormatClassWriterDelegate XmlFormatWriterDelegate
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
							XmlFormatClassWriterDelegate xmlFormatClassWriterDelegate = new XmlFormatWriterGenerator().GenerateClassWriter(this);
							Thread.MemoryBarrier();
							this.helper.XmlFormatWriterDelegate = xmlFormatClassWriterDelegate;
						}
					}
				}
				return this.helper.XmlFormatWriterDelegate;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x0600072D RID: 1837 RVA: 0x00020D34 File Offset: 0x0001EF34
		internal XmlFormatClassReaderDelegate XmlFormatReaderDelegate
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
							XmlFormatClassReaderDelegate xmlFormatClassReaderDelegate = new XmlFormatReaderGenerator().GenerateClassReader(this);
							Thread.MemoryBarrier();
							this.helper.XmlFormatReaderDelegate = xmlFormatClassReaderDelegate;
						}
					}
				}
				return this.helper.XmlFormatReaderDelegate;
			}
		}

		// Token: 0x0600072E RID: 1838 RVA: 0x00020DC4 File Offset: 0x0001EFC4
		internal static ClassDataContract CreateClassDataContractForKeyValue(Type type, XmlDictionaryString ns, string[] memberNames)
		{
			return new ClassDataContract(type, ns, memberNames);
		}

		// Token: 0x0600072F RID: 1839 RVA: 0x00020DD0 File Offset: 0x0001EFD0
		internal static void CheckAndAddMember(List<DataMember> members, DataMember memberContract, Dictionary<string, DataMember> memberNamesTable)
		{
			DataMember dataMember;
			if (memberNamesTable.TryGetValue(memberContract.Name, out dataMember))
			{
				Type declaringType = memberContract.MemberInfo.DeclaringType;
				DataContract.ThrowInvalidDataContractException(SR.GetString(declaringType.IsEnum ? "Type '{2}' contains two members '{0}' 'and '{1}' with the same name '{3}'. Multiple members with the same name in one type are not supported. Consider changing one of the member names using EnumMemberAttribute attribute." : "Type '{2}' contains two members '{0}' 'and '{1}' with the same data member name '{3}'. Multiple members with the same name in one type are not supported. Consider changing one of the member names using DataMemberAttribute attribute.", new object[]
				{
					dataMember.MemberInfo.Name,
					memberContract.MemberInfo.Name,
					DataContract.GetClrTypeFullName(declaringType),
					memberContract.Name
				}), declaringType);
			}
			memberNamesTable.Add(memberContract.Name, memberContract);
			members.Add(memberContract);
		}

		// Token: 0x06000730 RID: 1840 RVA: 0x00020E60 File Offset: 0x0001F060
		internal static XmlDictionaryString GetChildNamespaceToDeclare(DataContract dataContract, Type childType, XmlDictionary dictionary)
		{
			childType = DataContract.UnwrapNullableType(childType);
			if (!childType.IsEnum && !Globals.TypeOfIXmlSerializable.IsAssignableFrom(childType) && DataContract.GetBuiltInDataContract(childType) == null && childType != Globals.TypeOfDBNull)
			{
				string @namespace = DataContract.GetStableName(childType).Namespace;
				if (@namespace.Length > 0 && @namespace != dataContract.Namespace.Value)
				{
					return dictionary.Add(@namespace);
				}
			}
			return null;
		}

		// Token: 0x06000731 RID: 1841 RVA: 0x00020ED0 File Offset: 0x0001F0D0
		internal static bool IsNonAttributedTypeValidForSerialization(Type type)
		{
			if (type.IsArray)
			{
				return false;
			}
			if (type.IsEnum)
			{
				return false;
			}
			if (type.IsGenericParameter)
			{
				return false;
			}
			if (Globals.TypeOfIXmlSerializable.IsAssignableFrom(type))
			{
				return false;
			}
			if (type.IsPointer)
			{
				return false;
			}
			if (type.IsDefined(Globals.TypeOfCollectionDataContractAttribute, false))
			{
				return false;
			}
			Type[] interfaces = type.GetInterfaces();
			for (int i = 0; i < interfaces.Length; i++)
			{
				if (CollectionDataContract.IsCollectionInterface(interfaces[i]))
				{
					return false;
				}
			}
			if (type.IsSerializable)
			{
				return false;
			}
			if (Globals.TypeOfISerializable.IsAssignableFrom(type))
			{
				return false;
			}
			if (type.IsDefined(Globals.TypeOfDataContractAttribute, false))
			{
				return false;
			}
			if (type == Globals.TypeOfExtensionDataObject)
			{
				return false;
			}
			if (type.IsValueType)
			{
				return type.IsVisible;
			}
			return type.IsVisible && type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Globals.EmptyTypeArray, null) != null;
		}

		// Token: 0x06000732 RID: 1842 RVA: 0x00020FAC File Offset: 0x0001F1AC
		private XmlDictionaryString[] CreateChildElementNamespaces()
		{
			if (this.Members == null)
			{
				return null;
			}
			XmlDictionaryString[] array = null;
			if (this.BaseContract != null)
			{
				array = this.BaseContract.ChildElementNamespaces;
			}
			int num = ((array != null) ? array.Length : 0);
			XmlDictionaryString[] array2 = new XmlDictionaryString[this.Members.Count + num];
			if (num > 0)
			{
				Array.Copy(array, 0, array2, 0, array.Length);
			}
			XmlDictionary xmlDictionary = new XmlDictionary();
			for (int i = 0; i < this.Members.Count; i++)
			{
				array2[i + num] = ClassDataContract.GetChildNamespaceToDeclare(this, this.Members[i].MemberType, xmlDictionary);
			}
			return array2;
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x00021046 File Offset: 0x0001F246
		[SecuritySafeCritical]
		private void EnsureMethodsImported()
		{
			this.helper.EnsureMethodsImported();
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x00021053 File Offset: 0x0001F253
		public override void WriteXmlValue(XmlWriterDelegator xmlWriter, object obj, XmlObjectSerializerWriteContext context)
		{
			this.XmlFormatWriterDelegate(xmlWriter, obj, context, this);
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x00021064 File Offset: 0x0001F264
		public override object ReadXmlValue(XmlReaderDelegator xmlReader, XmlObjectSerializerReadContext context)
		{
			xmlReader.Read();
			object obj = this.XmlFormatReaderDelegate(xmlReader, context, this.MemberNames, this.MemberNamespaces);
			xmlReader.ReadEndElement();
			return obj;
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x0002108C File Offset: 0x0001F28C
		[SecuritySafeCritical]
		internal override DataContract BindGenericParameters(DataContract[] paramContracts, Dictionary<DataContract, DataContract> boundContracts)
		{
			Type underlyingType = base.UnderlyingType;
			if (!underlyingType.IsGenericType || !underlyingType.ContainsGenericParameters)
			{
				return this;
			}
			DataContract dataContract2;
			lock (this)
			{
				DataContract dataContract;
				if (boundContracts.TryGetValue(this, out dataContract))
				{
					dataContract2 = dataContract;
				}
				else
				{
					ClassDataContract classDataContract = new ClassDataContract();
					boundContracts.Add(this, classDataContract);
					XmlQualifiedName xmlQualifiedName;
					object[] array;
					if (underlyingType.IsGenericTypeDefinition)
					{
						xmlQualifiedName = base.StableName;
						array = paramContracts;
					}
					else
					{
						xmlQualifiedName = DataContract.GetStableName(underlyingType.GetGenericTypeDefinition());
						Type[] genericArguments = underlyingType.GetGenericArguments();
						array = new object[genericArguments.Length];
						for (int i = 0; i < genericArguments.Length; i++)
						{
							Type type = genericArguments[i];
							if (type.IsGenericParameter)
							{
								array[i] = paramContracts[type.GenericParameterPosition];
							}
							else
							{
								array[i] = type;
							}
						}
					}
					classDataContract.StableName = DataContract.CreateQualifiedName(DataContract.ExpandGenericParameters(XmlConvert.DecodeName(xmlQualifiedName.Name), new GenericNameProvider(DataContract.GetClrTypeFullName(base.UnderlyingType), array)), xmlQualifiedName.Namespace);
					if (this.BaseContract != null)
					{
						classDataContract.BaseContract = (ClassDataContract)this.BaseContract.BindGenericParameters(paramContracts, boundContracts);
					}
					classDataContract.IsISerializable = this.IsISerializable;
					classDataContract.IsValueType = base.IsValueType;
					classDataContract.IsReference = base.IsReference;
					if (this.Members != null)
					{
						classDataContract.Members = new List<DataMember>(this.Members.Count);
						foreach (DataMember dataMember in this.Members)
						{
							classDataContract.Members.Add(dataMember.BindGenericParameters(paramContracts, boundContracts));
						}
					}
					dataContract2 = classDataContract;
				}
			}
			return dataContract2;
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x00021280 File Offset: 0x0001F480
		internal override bool Equals(object other, Dictionary<DataContractPairKey, object> checkedContracts)
		{
			if (base.IsEqualOrChecked(other, checkedContracts))
			{
				return true;
			}
			if (base.Equals(other, checkedContracts))
			{
				ClassDataContract classDataContract = other as ClassDataContract;
				if (classDataContract != null)
				{
					if (this.IsISerializable)
					{
						if (!classDataContract.IsISerializable)
						{
							return false;
						}
					}
					else
					{
						if (classDataContract.IsISerializable)
						{
							return false;
						}
						if (this.Members == null)
						{
							if (classDataContract.Members != null && !this.IsEveryDataMemberOptional(classDataContract.Members))
							{
								return false;
							}
						}
						else if (classDataContract.Members == null)
						{
							if (!this.IsEveryDataMemberOptional(this.Members))
							{
								return false;
							}
						}
						else
						{
							Dictionary<string, DataMember> dictionary = new Dictionary<string, DataMember>(this.Members.Count);
							List<DataMember> list = new List<DataMember>();
							for (int i = 0; i < this.Members.Count; i++)
							{
								dictionary.Add(this.Members[i].Name, this.Members[i]);
							}
							for (int j = 0; j < classDataContract.Members.Count; j++)
							{
								DataMember dataMember;
								if (dictionary.TryGetValue(classDataContract.Members[j].Name, out dataMember))
								{
									if (!dataMember.Equals(classDataContract.Members[j], checkedContracts))
									{
										return false;
									}
									dictionary.Remove(dataMember.Name);
								}
								else
								{
									list.Add(classDataContract.Members[j]);
								}
							}
							if (!this.IsEveryDataMemberOptional(dictionary.Values))
							{
								return false;
							}
							if (!this.IsEveryDataMemberOptional(list))
							{
								return false;
							}
						}
					}
					if (this.BaseContract == null)
					{
						return classDataContract.BaseContract == null;
					}
					return classDataContract.BaseContract != null && this.BaseContract.Equals(classDataContract.BaseContract, checkedContracts);
				}
			}
			return false;
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x00021420 File Offset: 0x0001F620
		private bool IsEveryDataMemberOptional(IEnumerable<DataMember> dataMembers)
		{
			using (IEnumerator<DataMember> enumerator = dataMembers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsRequired)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x00021470 File Offset: 0x0001F670
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x040002DE RID: 734
		public XmlDictionaryString[] ContractNamespaces;

		// Token: 0x040002DF RID: 735
		public XmlDictionaryString[] MemberNames;

		// Token: 0x040002E0 RID: 736
		public XmlDictionaryString[] MemberNamespaces;

		// Token: 0x040002E1 RID: 737
		[SecurityCritical]
		private XmlDictionaryString[] childElementNamespaces;

		// Token: 0x040002E2 RID: 738
		[SecurityCritical]
		private ClassDataContract.ClassDataContractCriticalHelper helper;

		// Token: 0x0200016C RID: 364
		[SecurityCritical(SecurityCriticalScope.Everything)]
		private class ClassDataContractCriticalHelper : DataContract.DataContractCriticalHelper
		{
			// Token: 0x060013EF RID: 5103 RVA: 0x000506BA File Offset: 0x0004E8BA
			internal ClassDataContractCriticalHelper()
			{
			}

			// Token: 0x060013F0 RID: 5104 RVA: 0x000506C4 File Offset: 0x0004E8C4
			internal ClassDataContractCriticalHelper(Type type)
				: base(type)
			{
				XmlQualifiedName stableNameAndSetHasDataContract = this.GetStableNameAndSetHasDataContract(type);
				if (type == Globals.TypeOfDBNull)
				{
					base.StableName = stableNameAndSetHasDataContract;
					this.members = new List<DataMember>();
					XmlDictionary xmlDictionary = new XmlDictionary(2);
					base.Name = xmlDictionary.Add(base.StableName.Name);
					base.Namespace = xmlDictionary.Add(base.StableName.Namespace);
					this.ContractNamespaces = (this.MemberNames = (this.MemberNamespaces = new XmlDictionaryString[0]));
					this.EnsureMethodsImported();
					return;
				}
				Type type2 = type.BaseType;
				this.isISerializable = Globals.TypeOfISerializable.IsAssignableFrom(type);
				this.SetIsNonAttributedType(type);
				if (this.isISerializable)
				{
					if (this.HasDataContract)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("ISerializable type '{0}' cannot have DataContract.", new object[] { DataContract.GetClrTypeFullName(type) })));
					}
					if (type2 != null && (!type2.IsSerializable || !Globals.TypeOfISerializable.IsAssignableFrom(type2)))
					{
						type2 = null;
					}
				}
				base.IsValueType = type.IsValueType;
				if (type2 != null && type2 != Globals.TypeOfObject && type2 != Globals.TypeOfValueType && type2 != Globals.TypeOfUri)
				{
					DataContract dataContract = DataContract.GetDataContract(type2);
					if (dataContract is CollectionDataContract)
					{
						this.BaseContract = ((CollectionDataContract)dataContract).SharedTypeContract as ClassDataContract;
					}
					else
					{
						this.BaseContract = dataContract as ClassDataContract;
					}
					if (this.BaseContract != null && this.BaseContract.IsNonAttributedType && !this.isNonAttributedType)
					{
						throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Type '{0}' cannot inherit from a type that is not marked with DataContractAttribute or SerializableAttribute.  Consider marking the base type '{1}' with DataContractAttribute or SerializableAttribute, or removing them from the derived type.", new object[]
						{
							DataContract.GetClrTypeFullName(type),
							DataContract.GetClrTypeFullName(type2)
						})));
					}
				}
				else
				{
					this.BaseContract = null;
				}
				this.hasExtensionData = Globals.TypeOfIExtensibleDataObject.IsAssignableFrom(type);
				if (this.hasExtensionData && !this.HasDataContract && !this.IsNonAttributedType)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("On '{0}' type, only DataContract types can have extension data.", new object[] { DataContract.GetClrTypeFullName(type) })));
				}
				if (this.isISerializable)
				{
					base.SetDataContractName(stableNameAndSetHasDataContract);
				}
				else
				{
					base.StableName = stableNameAndSetHasDataContract;
					this.ImportDataMembers();
					XmlDictionary xmlDictionary2 = new XmlDictionary(2 + this.Members.Count);
					base.Name = xmlDictionary2.Add(base.StableName.Name);
					base.Namespace = xmlDictionary2.Add(base.StableName.Namespace);
					int num = 0;
					int num2 = 0;
					if (this.BaseContract == null)
					{
						this.MemberNames = new XmlDictionaryString[this.Members.Count];
						this.MemberNamespaces = new XmlDictionaryString[this.Members.Count];
						this.ContractNamespaces = new XmlDictionaryString[1];
					}
					else
					{
						if (this.BaseContract.IsReadOnlyContract)
						{
							this.serializationExceptionMessage = this.BaseContract.SerializationExceptionMessage;
						}
						num = this.BaseContract.MemberNames.Length;
						this.MemberNames = new XmlDictionaryString[this.Members.Count + num];
						Array.Copy(this.BaseContract.MemberNames, this.MemberNames, num);
						this.MemberNamespaces = new XmlDictionaryString[this.Members.Count + num];
						Array.Copy(this.BaseContract.MemberNamespaces, this.MemberNamespaces, num);
						num2 = this.BaseContract.ContractNamespaces.Length;
						this.ContractNamespaces = new XmlDictionaryString[1 + num2];
						Array.Copy(this.BaseContract.ContractNamespaces, this.ContractNamespaces, num2);
					}
					this.ContractNamespaces[num2] = base.Namespace;
					for (int i = 0; i < this.Members.Count; i++)
					{
						this.MemberNames[i + num] = xmlDictionary2.Add(this.Members[i].Name);
						this.MemberNamespaces[i + num] = base.Namespace;
					}
				}
				this.EnsureMethodsImported();
			}

			// Token: 0x060013F1 RID: 5105 RVA: 0x00050AC4 File Offset: 0x0004ECC4
			internal ClassDataContractCriticalHelper(Type type, XmlDictionaryString ns, string[] memberNames)
				: base(type)
			{
				base.StableName = new XmlQualifiedName(this.GetStableNameAndSetHasDataContract(type).Name, ns.Value);
				this.ImportDataMembers();
				XmlDictionary xmlDictionary = new XmlDictionary(1 + this.Members.Count);
				base.Name = xmlDictionary.Add(base.StableName.Name);
				base.Namespace = ns;
				this.ContractNamespaces = new XmlDictionaryString[] { base.Namespace };
				this.MemberNames = new XmlDictionaryString[this.Members.Count];
				this.MemberNamespaces = new XmlDictionaryString[this.Members.Count];
				for (int i = 0; i < this.Members.Count; i++)
				{
					this.Members[i].Name = memberNames[i];
					this.MemberNames[i] = xmlDictionary.Add(this.Members[i].Name);
					this.MemberNamespaces[i] = base.Namespace;
				}
				this.EnsureMethodsImported();
			}

			// Token: 0x060013F2 RID: 5106 RVA: 0x00050BCC File Offset: 0x0004EDCC
			private void EnsureIsReferenceImported(Type type)
			{
				bool flag = false;
				DataContractAttribute dataContractAttribute;
				bool flag2 = DataContract.TryGetDCAttribute(type, out dataContractAttribute);
				if (this.BaseContract != null)
				{
					if (flag2 && dataContractAttribute.IsReferenceSetExplicitly)
					{
						bool isReference = this.BaseContract.IsReference;
						if ((isReference && !dataContractAttribute.IsReference) || (!isReference && dataContractAttribute.IsReference))
						{
							DataContract.ThrowInvalidDataContractException(SR.GetString("The IsReference setting for type '{0}' is '{1}', but the same setting for its parent class '{2}' is '{3}'. Derived types must have the same value for IsReference as the base type. Change the setting on type '{0}' to '{3}', or on type '{2}' to '{1}', or do not set IsReference explicitly.", new object[]
							{
								DataContract.GetClrTypeFullName(type),
								dataContractAttribute.IsReference,
								DataContract.GetClrTypeFullName(this.BaseContract.UnderlyingType),
								this.BaseContract.IsReference
							}), type);
						}
						else
						{
							flag = dataContractAttribute.IsReference;
						}
					}
					else
					{
						flag = this.BaseContract.IsReference;
					}
				}
				else if (flag2 && dataContractAttribute.IsReference)
				{
					flag = dataContractAttribute.IsReference;
				}
				if (flag && type.IsValueType)
				{
					DataContract.ThrowInvalidDataContractException(SR.GetString("Value type '{0}' cannot have the IsReference setting of '{1}'. Either change the setting to '{2}', or remove it completely.", new object[]
					{
						DataContract.GetClrTypeFullName(type),
						true,
						false
					}), type);
					return;
				}
				base.IsReference = flag;
			}

			// Token: 0x060013F3 RID: 5107 RVA: 0x00050CE4 File Offset: 0x0004EEE4
			private void ImportDataMembers()
			{
				Type underlyingType = base.UnderlyingType;
				this.EnsureIsReferenceImported(underlyingType);
				List<DataMember> list = new List<DataMember>();
				Dictionary<string, DataMember> dictionary = new Dictionary<string, DataMember>();
				MemberInfo[] array;
				if (this.isNonAttributedType)
				{
					array = underlyingType.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
				}
				else
				{
					array = underlyingType.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				}
				foreach (MemberInfo memberInfo in array)
				{
					if (this.HasDataContract)
					{
						object[] customAttributes = memberInfo.GetCustomAttributes(typeof(DataMemberAttribute), false);
						if (customAttributes != null && customAttributes.Length != 0)
						{
							if (customAttributes.Length > 1)
							{
								base.ThrowInvalidDataContractException(SR.GetString("Member '{0}.{1}' has more than one DataMemberAttribute attribute.", new object[]
								{
									DataContract.GetClrTypeFullName(memberInfo.DeclaringType),
									memberInfo.Name
								}));
							}
							DataMember dataMember = new DataMember(memberInfo);
							if (memberInfo.MemberType == MemberTypes.Property)
							{
								PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
								MethodInfo getMethod = propertyInfo.GetGetMethod(true);
								if (getMethod != null && ClassDataContract.ClassDataContractCriticalHelper.IsMethodOverriding(getMethod))
								{
									goto IL_053D;
								}
								MethodInfo setMethod = propertyInfo.GetSetMethod(true);
								if (setMethod != null && ClassDataContract.ClassDataContractCriticalHelper.IsMethodOverriding(setMethod))
								{
									goto IL_053D;
								}
								if (getMethod == null)
								{
									base.ThrowInvalidDataContractException(SR.GetString("No get method for property '{1}' in type '{0}'.", new object[] { propertyInfo.DeclaringType, propertyInfo.Name }));
								}
								if (setMethod == null && !this.SetIfGetOnlyCollection(dataMember, false))
								{
									this.serializationExceptionMessage = SR.GetString("No set method for property '{1}' in type '{0}'.", new object[] { propertyInfo.DeclaringType, propertyInfo.Name });
								}
								if (getMethod.GetParameters().Length != 0)
								{
									base.ThrowInvalidDataContractException(SR.GetString("Property '{1}' in type '{0}' cannot be serialized because serialization of indexed properties is not supported.", new object[] { propertyInfo.DeclaringType, propertyInfo.Name }));
								}
							}
							else if (memberInfo.MemberType != MemberTypes.Field)
							{
								base.ThrowInvalidDataContractException(SR.GetString("Member '{0}.{1}' cannot be serialized since it is neither a field nor a property, and therefore cannot be marked with the DataMemberAttribute attribute. Remove the DataMemberAttribute attribute from the '{1}' member.", new object[]
								{
									DataContract.GetClrTypeFullName(underlyingType),
									memberInfo.Name
								}));
							}
							DataMemberAttribute dataMemberAttribute = (DataMemberAttribute)customAttributes[0];
							if (dataMemberAttribute.IsNameSetExplicitly)
							{
								if (dataMemberAttribute.Name == null || dataMemberAttribute.Name.Length == 0)
								{
									base.ThrowInvalidDataContractException(SR.GetString("Member '{0}' in type '{1}' cannot have DataMemberAttribute attribute Name set to null or empty string.", new object[]
									{
										memberInfo.Name,
										DataContract.GetClrTypeFullName(underlyingType)
									}));
								}
								dataMember.Name = dataMemberAttribute.Name;
							}
							else
							{
								dataMember.Name = memberInfo.Name;
							}
							dataMember.Name = DataContract.EncodeLocalName(dataMember.Name);
							dataMember.IsNullable = DataContract.IsTypeNullable(dataMember.MemberType);
							dataMember.IsRequired = dataMemberAttribute.IsRequired;
							if (dataMemberAttribute.IsRequired && base.IsReference)
							{
								DataContract.DataContractCriticalHelper.ThrowInvalidDataContractException(SR.GetString("'{0}.{1}' has the IsRequired setting of '{2}. However, '{0}' has the IsReference setting of '{2}', because either it is set explicitly, or it is derived from a base class. Set IsRequired on '{0}.{1}' to false, or disable IsReference on '{0}'.", new object[]
								{
									DataContract.GetClrTypeFullName(memberInfo.DeclaringType),
									memberInfo.Name,
									true
								}), underlyingType);
							}
							dataMember.EmitDefaultValue = dataMemberAttribute.EmitDefaultValue;
							dataMember.Order = dataMemberAttribute.Order;
							ClassDataContract.CheckAndAddMember(list, dataMember, dictionary);
						}
					}
					else if (this.isNonAttributedType)
					{
						FieldInfo fieldInfo = memberInfo as FieldInfo;
						PropertyInfo propertyInfo2 = memberInfo as PropertyInfo;
						if ((!(fieldInfo == null) || !(propertyInfo2 == null)) && (!(fieldInfo != null) || !fieldInfo.IsInitOnly))
						{
							object[] customAttributes2 = memberInfo.GetCustomAttributes(typeof(IgnoreDataMemberAttribute), false);
							if (customAttributes2 != null && customAttributes2.Length != 0)
							{
								if (customAttributes2.Length <= 1)
								{
									goto IL_053D;
								}
								base.ThrowInvalidDataContractException(SR.GetString("Member '{0}.{1}' has more than one IgnoreDataMemberAttribute attribute.", new object[]
								{
									DataContract.GetClrTypeFullName(memberInfo.DeclaringType),
									memberInfo.Name
								}));
							}
							DataMember dataMember2 = new DataMember(memberInfo);
							if (propertyInfo2 != null)
							{
								MethodInfo getMethod2 = propertyInfo2.GetGetMethod();
								if (getMethod2 == null || ClassDataContract.ClassDataContractCriticalHelper.IsMethodOverriding(getMethod2) || getMethod2.GetParameters().Length != 0)
								{
									goto IL_053D;
								}
								MethodInfo setMethod2 = propertyInfo2.GetSetMethod(true);
								if (setMethod2 == null)
								{
									if (!this.SetIfGetOnlyCollection(dataMember2, true))
									{
										goto IL_053D;
									}
								}
								else if (!setMethod2.IsPublic || ClassDataContract.ClassDataContractCriticalHelper.IsMethodOverriding(setMethod2))
								{
									goto IL_053D;
								}
								if (this.hasExtensionData && dataMember2.MemberType == Globals.TypeOfExtensionDataObject && memberInfo.Name == "ExtensionData")
								{
									goto IL_053D;
								}
							}
							dataMember2.Name = DataContract.EncodeLocalName(memberInfo.Name);
							dataMember2.IsNullable = DataContract.IsTypeNullable(dataMember2.MemberType);
							ClassDataContract.CheckAndAddMember(list, dataMember2, dictionary);
						}
					}
					else
					{
						FieldInfo fieldInfo2 = memberInfo as FieldInfo;
						if (fieldInfo2 != null && !fieldInfo2.IsNotSerialized)
						{
							DataMember dataMember3 = new DataMember(memberInfo);
							dataMember3.Name = DataContract.EncodeLocalName(memberInfo.Name);
							object[] customAttributes3 = fieldInfo2.GetCustomAttributes(Globals.TypeOfOptionalFieldAttribute, false);
							if (customAttributes3 == null || customAttributes3.Length == 0)
							{
								if (base.IsReference)
								{
									DataContract.DataContractCriticalHelper.ThrowInvalidDataContractException(SR.GetString("For type '{0}', non-optional field member '{1}' is on the Serializable type that has IsReference as {2}.", new object[]
									{
										DataContract.GetClrTypeFullName(memberInfo.DeclaringType),
										memberInfo.Name,
										true
									}), underlyingType);
								}
								dataMember3.IsRequired = true;
							}
							dataMember3.IsNullable = DataContract.IsTypeNullable(dataMember3.MemberType);
							ClassDataContract.CheckAndAddMember(list, dataMember3, dictionary);
						}
					}
					IL_053D:;
				}
				if (list.Count > 1)
				{
					list.Sort(ClassDataContract.DataMemberComparer.Singleton);
				}
				this.SetIfMembersHaveConflict(list);
				Thread.MemoryBarrier();
				this.members = list;
			}

			// Token: 0x060013F4 RID: 5108 RVA: 0x00051265 File Offset: 0x0004F465
			private bool SetIfGetOnlyCollection(DataMember memberContract, bool skipIfReadOnlyContract)
			{
				if (CollectionDataContract.IsCollection(memberContract.MemberType, false, skipIfReadOnlyContract) && !memberContract.MemberType.IsValueType)
				{
					memberContract.IsGetOnlyCollection = true;
					return true;
				}
				return false;
			}

			// Token: 0x060013F5 RID: 5109 RVA: 0x00051290 File Offset: 0x0004F490
			private void SetIfMembersHaveConflict(List<DataMember> members)
			{
				if (this.BaseContract == null)
				{
					return;
				}
				int num = 0;
				List<ClassDataContract.ClassDataContractCriticalHelper.Member> list = new List<ClassDataContract.ClassDataContractCriticalHelper.Member>();
				foreach (DataMember dataMember in members)
				{
					list.Add(new ClassDataContract.ClassDataContractCriticalHelper.Member(dataMember, base.StableName.Namespace, num));
				}
				for (ClassDataContract classDataContract = this.BaseContract; classDataContract != null; classDataContract = classDataContract.BaseContract)
				{
					num++;
					foreach (DataMember dataMember2 in classDataContract.Members)
					{
						list.Add(new ClassDataContract.ClassDataContractCriticalHelper.Member(dataMember2, classDataContract.StableName.Namespace, num));
					}
				}
				IComparer<ClassDataContract.ClassDataContractCriticalHelper.Member> singleton = ClassDataContract.ClassDataContractCriticalHelper.DataMemberConflictComparer.Singleton;
				list.Sort(singleton);
				for (int i = 0; i < list.Count - 1; i++)
				{
					int num2 = i;
					int num3 = i;
					bool flag = false;
					while (num3 < list.Count - 1 && string.CompareOrdinal(list[num3].member.Name, list[num3 + 1].member.Name) == 0 && string.CompareOrdinal(list[num3].ns, list[num3 + 1].ns) == 0)
					{
						list[num3].member.ConflictingMember = list[num3 + 1].member;
						if (!flag)
						{
							flag = list[num3 + 1].member.HasConflictingNameAndType || list[num3].member.MemberType != list[num3 + 1].member.MemberType;
						}
						num3++;
					}
					if (flag)
					{
						for (int j = num2; j <= num3; j++)
						{
							list[j].member.HasConflictingNameAndType = true;
						}
					}
					i = num3 + 1;
				}
			}

			// Token: 0x060013F6 RID: 5110 RVA: 0x000514A8 File Offset: 0x0004F6A8
			[SecuritySafeCritical]
			private XmlQualifiedName GetStableNameAndSetHasDataContract(Type type)
			{
				return DataContract.GetStableName(type, out this.hasDataContract);
			}

			// Token: 0x060013F7 RID: 5111 RVA: 0x000514B6 File Offset: 0x0004F6B6
			private void SetIsNonAttributedType(Type type)
			{
				this.isNonAttributedType = !type.IsSerializable && !this.hasDataContract && ClassDataContract.IsNonAttributedTypeValidForSerialization(type);
			}

			// Token: 0x060013F8 RID: 5112 RVA: 0x000514D7 File Offset: 0x0004F6D7
			private static bool IsMethodOverriding(MethodInfo method)
			{
				return method.IsVirtual && (method.Attributes & MethodAttributes.VtableLayoutMask) == MethodAttributes.PrivateScope;
			}

			// Token: 0x060013F9 RID: 5113 RVA: 0x000514F4 File Offset: 0x0004F6F4
			internal void EnsureMethodsImported()
			{
				if (!this.isMethodChecked && base.UnderlyingType != null)
				{
					lock (this)
					{
						if (!this.isMethodChecked)
						{
							foreach (MethodInfo methodInfo in base.UnderlyingType.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
							{
								Type type = null;
								ParameterInfo[] parameters = methodInfo.GetParameters();
								if (this.HasExtensionData && this.IsValidExtensionDataSetMethod(methodInfo, parameters))
								{
									if (methodInfo.Name == "System.Runtime.Serialization.IExtensibleDataObject.set_ExtensionData" || !methodInfo.IsPublic)
									{
										this.extensionDataSetMethod = XmlFormatGeneratorStatics.ExtensionDataSetExplicitMethodInfo;
									}
									else
									{
										this.extensionDataSetMethod = methodInfo;
									}
								}
								if (ClassDataContract.ClassDataContractCriticalHelper.IsValidCallback(methodInfo, parameters, Globals.TypeOfOnSerializingAttribute, this.onSerializing, ref type))
								{
									this.onSerializing = methodInfo;
								}
								if (ClassDataContract.ClassDataContractCriticalHelper.IsValidCallback(methodInfo, parameters, Globals.TypeOfOnSerializedAttribute, this.onSerialized, ref type))
								{
									this.onSerialized = methodInfo;
								}
								if (ClassDataContract.ClassDataContractCriticalHelper.IsValidCallback(methodInfo, parameters, Globals.TypeOfOnDeserializingAttribute, this.onDeserializing, ref type))
								{
									this.onDeserializing = methodInfo;
								}
								if (ClassDataContract.ClassDataContractCriticalHelper.IsValidCallback(methodInfo, parameters, Globals.TypeOfOnDeserializedAttribute, this.onDeserialized, ref type))
								{
									this.onDeserialized = methodInfo;
								}
							}
							Thread.MemoryBarrier();
							this.isMethodChecked = true;
						}
					}
				}
			}

			// Token: 0x060013FA RID: 5114 RVA: 0x00051660 File Offset: 0x0004F860
			private bool IsValidExtensionDataSetMethod(MethodInfo method, ParameterInfo[] parameters)
			{
				if (method.Name == "System.Runtime.Serialization.IExtensibleDataObject.set_ExtensionData" || method.Name == "set_ExtensionData")
				{
					if (this.extensionDataSetMethod != null)
					{
						base.ThrowInvalidDataContractException(SR.GetString("Duplicate extension data set method was found, for method '{0}', existing method is '{1}', on data contract type '{2}'.", new object[]
						{
							method,
							this.extensionDataSetMethod,
							DataContract.GetClrTypeFullName(method.DeclaringType)
						}));
					}
					if (method.ReturnType != Globals.TypeOfVoid)
					{
						DataContract.ThrowInvalidDataContractException(SR.GetString("For type '{0}' method '{1}', extension data set method must return void.", new object[]
						{
							DataContract.GetClrTypeFullName(method.DeclaringType),
							method
						}), method.DeclaringType);
					}
					if (parameters == null || parameters.Length != 1 || parameters[0].ParameterType != Globals.TypeOfExtensionDataObject)
					{
						DataContract.ThrowInvalidDataContractException(SR.GetString("For type '{0}' method '{1}', extension data set method has invalid type of parameter '{2}'.", new object[]
						{
							DataContract.GetClrTypeFullName(method.DeclaringType),
							method,
							Globals.TypeOfExtensionDataObject
						}), method.DeclaringType);
					}
					return true;
				}
				return false;
			}

			// Token: 0x060013FB RID: 5115 RVA: 0x00051768 File Offset: 0x0004F968
			private static bool IsValidCallback(MethodInfo method, ParameterInfo[] parameters, Type attributeType, MethodInfo currentCallback, ref Type prevAttributeType)
			{
				if (method.IsDefined(attributeType, false))
				{
					if (currentCallback != null)
					{
						DataContract.ThrowInvalidDataContractException(SR.GetString("Invalid attribute. Both '{0}' and '{1}' in type '{2}' have '{3}'.", new object[]
						{
							method,
							currentCallback,
							DataContract.GetClrTypeFullName(method.DeclaringType),
							attributeType
						}), method.DeclaringType);
					}
					else if (prevAttributeType != null)
					{
						DataContract.ThrowInvalidDataContractException(SR.GetString("Invalid Callback. Method '{3}' in type '{2}' has both '{0}' and '{1}'.", new object[]
						{
							prevAttributeType,
							attributeType,
							DataContract.GetClrTypeFullName(method.DeclaringType),
							method
						}), method.DeclaringType);
					}
					else if (method.IsVirtual)
					{
						DataContract.ThrowInvalidDataContractException(SR.GetString("Virtual Method '{0}' of type '{1}' cannot be marked with '{2}' attribute.", new object[]
						{
							method,
							DataContract.GetClrTypeFullName(method.DeclaringType),
							attributeType
						}), method.DeclaringType);
					}
					else
					{
						if (method.ReturnType != Globals.TypeOfVoid)
						{
							DataContract.ThrowInvalidDataContractException(SR.GetString("Serialization Callback '{1}' in type '{0}' must return void.", new object[]
							{
								DataContract.GetClrTypeFullName(method.DeclaringType),
								method
							}), method.DeclaringType);
						}
						if (parameters == null || parameters.Length != 1 || parameters[0].ParameterType != Globals.TypeOfStreamingContext)
						{
							DataContract.ThrowInvalidDataContractException(SR.GetString("Serialization Callback '{1}' in type '{0}' must have a single parameter of type '{2}'.", new object[]
							{
								DataContract.GetClrTypeFullName(method.DeclaringType),
								method,
								Globals.TypeOfStreamingContext
							}), method.DeclaringType);
						}
						prevAttributeType = attributeType;
					}
					return true;
				}
				return false;
			}

			// Token: 0x170003FC RID: 1020
			// (get) Token: 0x060013FC RID: 5116 RVA: 0x000518E2 File Offset: 0x0004FAE2
			// (set) Token: 0x060013FD RID: 5117 RVA: 0x000518EC File Offset: 0x0004FAEC
			internal ClassDataContract BaseContract
			{
				get
				{
					return this.baseContract;
				}
				set
				{
					this.baseContract = value;
					if (this.baseContract != null && base.IsValueType)
					{
						base.ThrowInvalidDataContractException(SR.GetString("Data contract '{0}' from namespace '{1}' is a value type and cannot have base contract '{2}' from namespace '{3}'.", new object[]
						{
							base.StableName.Name,
							base.StableName.Namespace,
							this.baseContract.StableName.Name,
							this.baseContract.StableName.Namespace
						}));
					}
				}
			}

			// Token: 0x170003FD RID: 1021
			// (get) Token: 0x060013FE RID: 5118 RVA: 0x00051968 File Offset: 0x0004FB68
			// (set) Token: 0x060013FF RID: 5119 RVA: 0x00051970 File Offset: 0x0004FB70
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

			// Token: 0x170003FE RID: 1022
			// (get) Token: 0x06001400 RID: 5120 RVA: 0x00051979 File Offset: 0x0004FB79
			internal MethodInfo OnSerializing
			{
				get
				{
					this.EnsureMethodsImported();
					return this.onSerializing;
				}
			}

			// Token: 0x170003FF RID: 1023
			// (get) Token: 0x06001401 RID: 5121 RVA: 0x00051987 File Offset: 0x0004FB87
			internal MethodInfo OnSerialized
			{
				get
				{
					this.EnsureMethodsImported();
					return this.onSerialized;
				}
			}

			// Token: 0x17000400 RID: 1024
			// (get) Token: 0x06001402 RID: 5122 RVA: 0x00051995 File Offset: 0x0004FB95
			internal MethodInfo OnDeserializing
			{
				get
				{
					this.EnsureMethodsImported();
					return this.onDeserializing;
				}
			}

			// Token: 0x17000401 RID: 1025
			// (get) Token: 0x06001403 RID: 5123 RVA: 0x000519A3 File Offset: 0x0004FBA3
			internal MethodInfo OnDeserialized
			{
				get
				{
					this.EnsureMethodsImported();
					return this.onDeserialized;
				}
			}

			// Token: 0x17000402 RID: 1026
			// (get) Token: 0x06001404 RID: 5124 RVA: 0x000519B1 File Offset: 0x0004FBB1
			internal MethodInfo ExtensionDataSetMethod
			{
				get
				{
					this.EnsureMethodsImported();
					return this.extensionDataSetMethod;
				}
			}

			// Token: 0x17000403 RID: 1027
			// (get) Token: 0x06001405 RID: 5125 RVA: 0x000519C0 File Offset: 0x0004FBC0
			// (set) Token: 0x06001406 RID: 5126 RVA: 0x00051A38 File Offset: 0x0004FC38
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

			// Token: 0x17000404 RID: 1028
			// (get) Token: 0x06001407 RID: 5127 RVA: 0x00051A41 File Offset: 0x0004FC41
			internal string SerializationExceptionMessage
			{
				get
				{
					return this.serializationExceptionMessage;
				}
			}

			// Token: 0x17000405 RID: 1029
			// (get) Token: 0x06001408 RID: 5128 RVA: 0x00051A49 File Offset: 0x0004FC49
			internal string DeserializationExceptionMessage
			{
				get
				{
					if (this.serializationExceptionMessage == null)
					{
						return null;
					}
					return SR.GetString("Error on deserializing read-only members in the class: {0}", new object[] { this.serializationExceptionMessage });
				}
			}

			// Token: 0x17000406 RID: 1030
			// (get) Token: 0x06001409 RID: 5129 RVA: 0x00051A6E File Offset: 0x0004FC6E
			// (set) Token: 0x0600140A RID: 5130 RVA: 0x00051A76 File Offset: 0x0004FC76
			internal override bool IsISerializable
			{
				get
				{
					return this.isISerializable;
				}
				set
				{
					this.isISerializable = value;
				}
			}

			// Token: 0x17000407 RID: 1031
			// (get) Token: 0x0600140B RID: 5131 RVA: 0x00051A7F File Offset: 0x0004FC7F
			internal bool HasDataContract
			{
				get
				{
					return this.hasDataContract;
				}
			}

			// Token: 0x17000408 RID: 1032
			// (get) Token: 0x0600140C RID: 5132 RVA: 0x00051A87 File Offset: 0x0004FC87
			internal bool HasExtensionData
			{
				get
				{
					return this.hasExtensionData;
				}
			}

			// Token: 0x17000409 RID: 1033
			// (get) Token: 0x0600140D RID: 5133 RVA: 0x00051A8F File Offset: 0x0004FC8F
			internal bool IsNonAttributedType
			{
				get
				{
					return this.isNonAttributedType;
				}
			}

			// Token: 0x0600140E RID: 5134 RVA: 0x00051A98 File Offset: 0x0004FC98
			internal ConstructorInfo GetISerializableConstructor()
			{
				if (!this.IsISerializable)
				{
					return null;
				}
				ConstructorInfo constructor = base.UnderlyingType.GetConstructor(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, ClassDataContract.ClassDataContractCriticalHelper.SerInfoCtorArgs, null);
				if (constructor == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(XmlObjectSerializer.CreateSerializationException(SR.GetString("Constructor that takes SerializationInfo and StreamingContext is not found for '{0}'.", new object[] { DataContract.GetClrTypeFullName(base.UnderlyingType) })));
				}
				return constructor;
			}

			// Token: 0x0600140F RID: 5135 RVA: 0x00051AF8 File Offset: 0x0004FCF8
			internal ConstructorInfo GetNonAttributedTypeConstructor()
			{
				if (!this.IsNonAttributedType)
				{
					return null;
				}
				Type underlyingType = base.UnderlyingType;
				if (underlyingType.IsValueType)
				{
					return null;
				}
				ConstructorInfo constructor = underlyingType.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Globals.EmptyTypeArray, null);
				if (constructor == null)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("The Type '{0}' must have a parameterless constructor.", new object[] { DataContract.GetClrTypeFullName(underlyingType) })));
				}
				return constructor;
			}

			// Token: 0x1700040A RID: 1034
			// (get) Token: 0x06001410 RID: 5136 RVA: 0x00051B5E File Offset: 0x0004FD5E
			// (set) Token: 0x06001411 RID: 5137 RVA: 0x00051B66 File Offset: 0x0004FD66
			internal XmlFormatClassWriterDelegate XmlFormatWriterDelegate
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

			// Token: 0x1700040B RID: 1035
			// (get) Token: 0x06001412 RID: 5138 RVA: 0x00051B6F File Offset: 0x0004FD6F
			// (set) Token: 0x06001413 RID: 5139 RVA: 0x00051B77 File Offset: 0x0004FD77
			internal XmlFormatClassReaderDelegate XmlFormatReaderDelegate
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

			// Token: 0x1700040C RID: 1036
			// (get) Token: 0x06001414 RID: 5140 RVA: 0x00051B80 File Offset: 0x0004FD80
			// (set) Token: 0x06001415 RID: 5141 RVA: 0x00051B88 File Offset: 0x0004FD88
			public XmlDictionaryString[] ChildElementNamespaces
			{
				get
				{
					return this.childElementNamespaces;
				}
				set
				{
					this.childElementNamespaces = value;
				}
			}

			// Token: 0x1700040D RID: 1037
			// (get) Token: 0x06001416 RID: 5142 RVA: 0x00051B91 File Offset: 0x0004FD91
			private static Type[] SerInfoCtorArgs
			{
				get
				{
					if (ClassDataContract.ClassDataContractCriticalHelper.serInfoCtorArgs == null)
					{
						ClassDataContract.ClassDataContractCriticalHelper.serInfoCtorArgs = new Type[]
						{
							typeof(SerializationInfo),
							typeof(StreamingContext)
						};
					}
					return ClassDataContract.ClassDataContractCriticalHelper.serInfoCtorArgs;
				}
			}

			// Token: 0x040009BA RID: 2490
			private ClassDataContract baseContract;

			// Token: 0x040009BB RID: 2491
			private List<DataMember> members;

			// Token: 0x040009BC RID: 2492
			private MethodInfo onSerializing;

			// Token: 0x040009BD RID: 2493
			private MethodInfo onSerialized;

			// Token: 0x040009BE RID: 2494
			private MethodInfo onDeserializing;

			// Token: 0x040009BF RID: 2495
			private MethodInfo onDeserialized;

			// Token: 0x040009C0 RID: 2496
			private MethodInfo extensionDataSetMethod;

			// Token: 0x040009C1 RID: 2497
			private Dictionary<XmlQualifiedName, DataContract> knownDataContracts;

			// Token: 0x040009C2 RID: 2498
			private string serializationExceptionMessage;

			// Token: 0x040009C3 RID: 2499
			private bool isISerializable;

			// Token: 0x040009C4 RID: 2500
			private bool isKnownTypeAttributeChecked;

			// Token: 0x040009C5 RID: 2501
			private bool isMethodChecked;

			// Token: 0x040009C6 RID: 2502
			private bool hasExtensionData;

			// Token: 0x040009C7 RID: 2503
			private bool isNonAttributedType;

			// Token: 0x040009C8 RID: 2504
			private bool hasDataContract;

			// Token: 0x040009C9 RID: 2505
			private XmlDictionaryString[] childElementNamespaces;

			// Token: 0x040009CA RID: 2506
			private XmlFormatClassReaderDelegate xmlFormatReaderDelegate;

			// Token: 0x040009CB RID: 2507
			private XmlFormatClassWriterDelegate xmlFormatWriterDelegate;

			// Token: 0x040009CC RID: 2508
			public XmlDictionaryString[] ContractNamespaces;

			// Token: 0x040009CD RID: 2509
			public XmlDictionaryString[] MemberNames;

			// Token: 0x040009CE RID: 2510
			public XmlDictionaryString[] MemberNamespaces;

			// Token: 0x040009CF RID: 2511
			private static Type[] serInfoCtorArgs;

			// Token: 0x020001AC RID: 428
			internal struct Member
			{
				// Token: 0x06001557 RID: 5463 RVA: 0x000554E9 File Offset: 0x000536E9
				internal Member(DataMember member, string ns, int baseTypeIndex)
				{
					this.member = member;
					this.ns = ns;
					this.baseTypeIndex = baseTypeIndex;
				}

				// Token: 0x04000A94 RID: 2708
				internal DataMember member;

				// Token: 0x04000A95 RID: 2709
				internal string ns;

				// Token: 0x04000A96 RID: 2710
				internal int baseTypeIndex;
			}

			// Token: 0x020001AD RID: 429
			internal class DataMemberConflictComparer : IComparer<ClassDataContract.ClassDataContractCriticalHelper.Member>
			{
				// Token: 0x06001558 RID: 5464 RVA: 0x00055500 File Offset: 0x00053700
				public int Compare(ClassDataContract.ClassDataContractCriticalHelper.Member x, ClassDataContract.ClassDataContractCriticalHelper.Member y)
				{
					int num = string.CompareOrdinal(x.ns, y.ns);
					if (num != 0)
					{
						return num;
					}
					int num2 = string.CompareOrdinal(x.member.Name, y.member.Name);
					if (num2 != 0)
					{
						return num2;
					}
					return x.baseTypeIndex - y.baseTypeIndex;
				}

				// Token: 0x04000A97 RID: 2711
				internal static ClassDataContract.ClassDataContractCriticalHelper.DataMemberConflictComparer Singleton = new ClassDataContract.ClassDataContractCriticalHelper.DataMemberConflictComparer();
			}
		}

		// Token: 0x0200016D RID: 365
		internal class DataMemberComparer : IComparer<DataMember>
		{
			// Token: 0x06001417 RID: 5143 RVA: 0x00051BC4 File Offset: 0x0004FDC4
			public int Compare(DataMember x, DataMember y)
			{
				int num = x.Order - y.Order;
				if (num != 0)
				{
					return num;
				}
				return string.CompareOrdinal(x.Name, y.Name);
			}

			// Token: 0x040009D0 RID: 2512
			internal static ClassDataContract.DataMemberComparer Singleton = new ClassDataContract.DataMemberComparer();
		}
	}
}
