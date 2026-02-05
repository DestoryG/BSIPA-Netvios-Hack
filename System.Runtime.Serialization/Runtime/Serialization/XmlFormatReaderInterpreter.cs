using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x020000F9 RID: 249
	internal class XmlFormatReaderInterpreter
	{
		// Token: 0x06000F43 RID: 3907 RVA: 0x0003E309 File Offset: 0x0003C509
		public XmlFormatReaderInterpreter(ClassDataContract classContract)
		{
			this.classContract = classContract;
		}

		// Token: 0x06000F44 RID: 3908 RVA: 0x0003E318 File Offset: 0x0003C518
		public XmlFormatReaderInterpreter(CollectionDataContract collectionContract, bool isGetOnly)
		{
			this.collectionContract = collectionContract;
			this.is_get_only_collection = isGetOnly;
		}

		// Token: 0x06000F45 RID: 3909 RVA: 0x0003E330 File Offset: 0x0003C530
		public object ReadFromXml(XmlReaderDelegator xmlReader, XmlObjectSerializerReadContext context, XmlDictionaryString[] memberNames, XmlDictionaryString[] memberNamespaces)
		{
			this.xmlReader = xmlReader;
			this.context = context;
			this.memberNames = memberNames;
			this.memberNamespaces = memberNamespaces;
			this.CreateObject(this.classContract);
			context.AddNewObject(this.objectLocal);
			this.InvokeOnDeserializing(this.classContract);
			string text = null;
			if (this.HasFactoryMethod(this.classContract))
			{
				text = context.GetObjectId();
			}
			if (this.classContract.IsISerializable)
			{
				this.ReadISerializable(this.classContract);
			}
			else
			{
				this.ReadClass(this.classContract);
			}
			bool flag = this.InvokeFactoryMethod(this.classContract, text);
			if (Globals.TypeOfIDeserializationCallback.IsAssignableFrom(this.classContract.UnderlyingType))
			{
				((IDeserializationCallback)this.objectLocal).OnDeserialization(null);
			}
			this.InvokeOnDeserialized(this.classContract);
			if ((text == null || !flag) && this.classContract.UnderlyingType == Globals.TypeOfDateTimeOffsetAdapter)
			{
				this.objectLocal = DateTimeOffsetAdapter.GetDateTimeOffset((DateTimeOffsetAdapter)this.objectLocal);
			}
			return this.objectLocal;
		}

		// Token: 0x06000F46 RID: 3910 RVA: 0x0003E43C File Offset: 0x0003C63C
		public object ReadCollectionFromXml(XmlReaderDelegator xmlReader, XmlObjectSerializerReadContext context, XmlDictionaryString itemName, XmlDictionaryString itemNamespace, CollectionDataContract collectionContract)
		{
			this.xmlReader = xmlReader;
			this.context = context;
			this.itemName = itemName;
			this.itemNamespace = itemNamespace;
			this.collectionContract = collectionContract;
			this.ReadCollection(collectionContract);
			return this.objectLocal;
		}

		// Token: 0x06000F47 RID: 3911 RVA: 0x0003E471 File Offset: 0x0003C671
		public void ReadGetOnlyCollectionFromXml(XmlReaderDelegator xmlReader, XmlObjectSerializerReadContext context, XmlDictionaryString itemName, XmlDictionaryString itemNamespace, CollectionDataContract collectionContract)
		{
			this.xmlReader = xmlReader;
			this.context = context;
			this.itemName = itemName;
			this.itemNamespace = itemNamespace;
			this.collectionContract = collectionContract;
			this.ReadGetOnlyCollection(collectionContract);
		}

		// Token: 0x06000F48 RID: 3912 RVA: 0x0003E4A0 File Offset: 0x0003C6A0
		private void CreateObject(ClassDataContract classContract)
		{
			Type type = (this.objectType = classContract.UnderlyingType);
			if (type.IsValueType && !classContract.IsNonAttributedType)
			{
				type = Globals.TypeOfValueType;
			}
			if (classContract.UnderlyingType == Globals.TypeOfDBNull)
			{
				this.objectLocal = DBNull.Value;
				return;
			}
			if (!classContract.IsNonAttributedType)
			{
				this.objectLocal = CodeInterpreter.ConvertValue(XmlFormatReaderGenerator.UnsafeGetUninitializedObject(DataContract.GetIdForInitialization(classContract)), Globals.TypeOfObject, type);
				return;
			}
			if (type.IsValueType)
			{
				this.objectLocal = FormatterServices.GetUninitializedObject(type);
				return;
			}
			this.objectLocal = classContract.GetNonAttributedTypeConstructor().Invoke(new object[0]);
		}

		// Token: 0x06000F49 RID: 3913 RVA: 0x0003E544 File Offset: 0x0003C744
		private void InvokeOnDeserializing(ClassDataContract classContract)
		{
			if (classContract.BaseContract != null)
			{
				this.InvokeOnDeserializing(classContract.BaseContract);
			}
			if (classContract.OnDeserializing != null)
			{
				classContract.OnDeserializing.Invoke(this.objectLocal, new object[] { this.context.GetStreamingContext() });
			}
		}

		// Token: 0x06000F4A RID: 3914 RVA: 0x0003E5A0 File Offset: 0x0003C7A0
		private void InvokeOnDeserialized(ClassDataContract classContract)
		{
			if (classContract.BaseContract != null)
			{
				this.InvokeOnDeserialized(classContract.BaseContract);
			}
			if (classContract.OnDeserialized != null)
			{
				classContract.OnDeserialized.Invoke(this.objectLocal, new object[] { this.context.GetStreamingContext() });
			}
		}

		// Token: 0x06000F4B RID: 3915 RVA: 0x0003E5FA File Offset: 0x0003C7FA
		private bool HasFactoryMethod(ClassDataContract classContract)
		{
			return Globals.TypeOfIObjectReference.IsAssignableFrom(classContract.UnderlyingType);
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x0003E60C File Offset: 0x0003C80C
		private bool InvokeFactoryMethod(ClassDataContract classContract, string objectId)
		{
			if (this.HasFactoryMethod(classContract))
			{
				this.objectLocal = CodeInterpreter.ConvertValue(this.context.GetRealObject((IObjectReference)this.objectLocal, objectId), Globals.TypeOfObject, classContract.UnderlyingType);
				return true;
			}
			return false;
		}

		// Token: 0x06000F4D RID: 3917 RVA: 0x0003E648 File Offset: 0x0003C848
		private void ReadISerializable(ClassDataContract classContract)
		{
			MethodBase iserializableConstructor = classContract.GetISerializableConstructor();
			SerializationInfo serializationInfo = this.context.ReadSerializationInfo(this.xmlReader, classContract.UnderlyingType);
			iserializableConstructor.Invoke(this.objectLocal, new object[]
			{
				serializationInfo,
				this.context.GetStreamingContext()
			});
		}

		// Token: 0x06000F4E RID: 3918 RVA: 0x0003E69C File Offset: 0x0003C89C
		private void ReadClass(ClassDataContract classContract)
		{
			if (classContract.HasExtensionData)
			{
				ExtensionDataObject extensionDataObject = new ExtensionDataObject();
				this.ReadMembers(classContract, extensionDataObject);
				for (ClassDataContract classDataContract = classContract; classDataContract != null; classDataContract = classDataContract.BaseContract)
				{
					MethodInfo extensionDataSetMethod = classDataContract.ExtensionDataSetMethod;
					if (extensionDataSetMethod != null)
					{
						extensionDataSetMethod.Invoke(this.objectLocal, new object[] { extensionDataObject });
					}
				}
				return;
			}
			this.ReadMembers(classContract, null);
		}

		// Token: 0x06000F4F RID: 3919 RVA: 0x0003E700 File Offset: 0x0003C900
		private void ReadMembers(ClassDataContract classContract, ExtensionDataObject extensionData)
		{
			int num = classContract.MemberNames.Length;
			this.context.IncrementItemCount(num);
			int num2 = -1;
			int num3;
			bool[] requiredMembers = this.GetRequiredMembers(classContract, out num3);
			bool flag = num3 < num;
			int num4 = (flag ? num3 : num);
			while (XmlObjectSerializerReadContext.MoveToNextElement(this.xmlReader))
			{
				int num5;
				if (flag)
				{
					num5 = this.context.GetMemberIndexWithRequiredMembers(this.xmlReader, this.memberNames, this.memberNamespaces, num2, num4, extensionData);
				}
				else
				{
					num5 = this.context.GetMemberIndex(this.xmlReader, this.memberNames, this.memberNamespaces, num2, extensionData);
				}
				if (num > 0)
				{
					this.ReadMembers(num5, classContract, requiredMembers, ref num2, ref num4);
				}
			}
			if (flag && num4 < num)
			{
				XmlObjectSerializerReadContext.ThrowRequiredMemberMissingException(this.xmlReader, num2, num4, this.memberNames);
			}
		}

		// Token: 0x06000F50 RID: 3920 RVA: 0x0003E7C8 File Offset: 0x0003C9C8
		private int ReadMembers(int index, ClassDataContract classContract, bool[] requiredMembers, ref int memberIndex, ref int requiredIndex)
		{
			int num = ((classContract.BaseContract == null) ? 0 : this.ReadMembers(index, classContract.BaseContract, requiredMembers, ref memberIndex, ref requiredIndex));
			if (num <= index && index < num + classContract.Members.Count)
			{
				DataMember dataMember = classContract.Members[index - num];
				Type memberType = dataMember.MemberType;
				if (dataMember.IsRequired)
				{
					int num2 = index + 1;
					while (num2 < requiredMembers.Length && !requiredMembers[num2])
					{
						num2++;
					}
					requiredIndex = num2;
				}
				if (dataMember.IsGetOnlyCollection)
				{
					object member = CodeInterpreter.GetMember(dataMember.MemberInfo, this.objectLocal);
					this.context.StoreCollectionMemberInfo(member);
					this.ReadValue(memberType, dataMember.Name, classContract.StableName.Namespace);
				}
				else
				{
					object obj = this.ReadValue(memberType, dataMember.Name, classContract.StableName.Namespace);
					CodeInterpreter.SetMember(dataMember.MemberInfo, this.objectLocal, obj);
				}
				memberIndex = index;
			}
			return num + classContract.Members.Count;
		}

		// Token: 0x06000F51 RID: 3921 RVA: 0x0003E8C8 File Offset: 0x0003CAC8
		private bool[] GetRequiredMembers(ClassDataContract contract, out int firstRequiredMember)
		{
			int num = contract.MemberNames.Length;
			bool[] array = new bool[num];
			this.GetRequiredMembers(contract, array);
			firstRequiredMember = 0;
			while (firstRequiredMember < num && !array[firstRequiredMember])
			{
				firstRequiredMember++;
			}
			return array;
		}

		// Token: 0x06000F52 RID: 3922 RVA: 0x0003E908 File Offset: 0x0003CB08
		private int GetRequiredMembers(ClassDataContract contract, bool[] requiredMembers)
		{
			int num = ((contract.BaseContract == null) ? 0 : this.GetRequiredMembers(contract.BaseContract, requiredMembers));
			List<DataMember> members = contract.Members;
			int i = 0;
			while (i < members.Count)
			{
				requiredMembers[num] = members[i].IsRequired;
				i++;
				num++;
			}
			return num;
		}

		// Token: 0x06000F53 RID: 3923 RVA: 0x0003E95C File Offset: 0x0003CB5C
		private object ReadValue(Type type, string name, string ns)
		{
			Type type2 = type;
			bool flag = false;
			int num = 0;
			while (type.IsGenericType && type.GetGenericTypeDefinition() == Globals.TypeOfNullable)
			{
				num++;
				type = type.GetGenericArguments()[0];
			}
			PrimitiveDataContract primitiveDataContract = PrimitiveDataContract.GetPrimitiveDataContract(type);
			object obj;
			if ((primitiveDataContract != null && primitiveDataContract.UnderlyingType != Globals.TypeOfObject) || num != 0 || type.IsValueType)
			{
				this.context.ReadAttributes(this.xmlReader);
				string text = this.context.ReadIfNullOrRef(this.xmlReader, type, DataContract.IsTypeSerializable(type));
				if (text == null)
				{
					if (num != 0)
					{
						obj = Activator.CreateInstance(type2);
					}
					else
					{
						if (type.IsValueType)
						{
							throw new SerializationException(SR.GetString("ValueType '{0}' cannot be null.", new object[] { DataContract.GetClrTypeFullName(type) }));
						}
						obj = null;
					}
				}
				else if (text == string.Empty)
				{
					text = this.context.GetObjectId();
					if (type.IsValueType && !string.IsNullOrEmpty(text))
					{
						throw new SerializationException(SR.GetString("ValueType '{0}' cannot have id.", new object[] { DataContract.GetClrTypeFullName(type) }));
					}
					if (num != 0)
					{
						flag = true;
					}
					if (primitiveDataContract != null && primitiveDataContract.UnderlyingType != Globals.TypeOfObject)
					{
						obj = primitiveDataContract.XmlFormatReaderMethod.Invoke(this.xmlReader, new object[0]);
						if (!type.IsValueType)
						{
							this.context.AddNewObject(obj);
						}
					}
					else
					{
						obj = this.InternalDeserialize(type, name, ns);
					}
				}
				else
				{
					if (type.IsValueType)
					{
						throw new SerializationException(SR.GetString("ValueType '{0}' cannot have ref to another object.", new object[] { DataContract.GetClrTypeFullName(type) }));
					}
					obj = CodeInterpreter.ConvertValue(this.context.GetExistingObject(text, type, name, ns), Globals.TypeOfObject, type);
				}
				if (flag && text != null)
				{
					obj = this.WrapNullableObject(type, obj, type2, num);
				}
			}
			else
			{
				obj = this.InternalDeserialize(type, name, ns);
			}
			return obj;
		}

		// Token: 0x06000F54 RID: 3924 RVA: 0x0003EB3C File Offset: 0x0003CD3C
		private object InternalDeserialize(Type type, string name, string ns)
		{
			Type type2 = (type.IsPointer ? Globals.TypeOfReflectionPointer : type);
			object obj = this.context.InternalDeserialize(this.xmlReader, DataContract.GetId(type2.TypeHandle), type2.TypeHandle, name, ns);
			if (type.IsPointer)
			{
				return XmlFormatGeneratorStatics.UnboxPointer.Invoke(null, new object[] { obj });
			}
			return CodeInterpreter.ConvertValue(obj, Globals.TypeOfObject, type);
		}

		// Token: 0x06000F55 RID: 3925 RVA: 0x0003EBAC File Offset: 0x0003CDAC
		private object WrapNullableObject(Type innerType, object innerValue, Type outerType, int nullables)
		{
			object obj = innerValue;
			for (int i = 1; i < nullables; i++)
			{
				Type type = Globals.TypeOfNullable.MakeGenericType(new Type[] { innerType });
				obj = Activator.CreateInstance(type, new object[] { obj });
				innerType = type;
			}
			return Activator.CreateInstance(outerType, new object[] { obj });
		}

		// Token: 0x06000F56 RID: 3926 RVA: 0x0003EC04 File Offset: 0x0003CE04
		private void ReadCollection(CollectionDataContract collectionContract)
		{
			Type type = collectionContract.UnderlyingType;
			Type itemType = collectionContract.ItemType;
			bool flag = collectionContract.Kind == CollectionKind.Array;
			ConstructorInfo constructorInfo = collectionContract.Constructor;
			if (type.IsInterface)
			{
				switch (collectionContract.Kind)
				{
				case CollectionKind.GenericDictionary:
					type = Globals.TypeOfDictionaryGeneric.MakeGenericType(itemType.GetGenericArguments());
					constructorInfo = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, Globals.EmptyTypeArray, null);
					break;
				case CollectionKind.Dictionary:
					type = Globals.TypeOfHashtable;
					constructorInfo = XmlFormatGeneratorStatics.HashtableCtor;
					break;
				case CollectionKind.GenericList:
				case CollectionKind.GenericCollection:
				case CollectionKind.List:
				case CollectionKind.GenericEnumerable:
				case CollectionKind.Collection:
				case CollectionKind.Enumerable:
					type = itemType.MakeArrayType();
					flag = true;
					break;
				}
			}
			string text = collectionContract.ItemName;
			string @namespace = collectionContract.StableName.Namespace;
			if (!flag)
			{
				if (type.IsValueType)
				{
					this.objectLocal = FormatterServices.GetUninitializedObject(type);
				}
				else
				{
					this.objectLocal = constructorInfo.Invoke(new object[0]);
					this.context.AddNewObject(this.objectLocal);
				}
			}
			int arraySize = this.context.GetArraySize();
			string objectId = this.context.GetObjectId();
			bool flag2 = false;
			bool flag3 = false;
			if (flag && this.TryReadPrimitiveArray(type, itemType, arraySize, out flag3))
			{
				flag2 = true;
			}
			if (!flag3)
			{
				if (arraySize == -1)
				{
					object obj = null;
					if (flag)
					{
						obj = Array.CreateInstance(itemType, 32);
					}
					int i;
					for (i = 0; i < 2147483647; i++)
					{
						if (this.IsStartElement(this.itemName, this.itemNamespace))
						{
							this.context.IncrementItemCount(1);
							object obj2 = this.ReadCollectionItem(collectionContract, itemType, text, @namespace);
							if (flag)
							{
								obj = XmlFormatGeneratorStatics.EnsureArraySizeMethod.MakeGenericMethod(new Type[] { itemType }).Invoke(null, new object[] { obj, i });
								((Array)obj).SetValue(obj2, i);
							}
							else
							{
								this.StoreCollectionValue(this.objectLocal, itemType, obj2, collectionContract);
							}
						}
						else
						{
							if (this.IsEndElement())
							{
								break;
							}
							this.HandleUnexpectedItemInCollection(ref i);
						}
					}
					if (flag)
					{
						MethodInfo methodInfo = XmlFormatGeneratorStatics.TrimArraySizeMethod.MakeGenericMethod(new Type[] { itemType });
						this.objectLocal = methodInfo.Invoke(null, new object[] { obj, i });
						this.context.AddNewObjectWithId(objectId, this.objectLocal);
					}
				}
				else
				{
					this.context.IncrementItemCount(arraySize);
					if (flag)
					{
						this.objectLocal = Array.CreateInstance(itemType, arraySize);
						this.context.AddNewObject(this.objectLocal);
					}
					for (int j = 0; j < arraySize; j++)
					{
						if (this.IsStartElement(this.itemName, this.itemNamespace))
						{
							object obj3 = this.ReadCollectionItem(collectionContract, itemType, text, @namespace);
							if (flag)
							{
								((Array)this.objectLocal).SetValue(obj3, j);
							}
							else
							{
								this.StoreCollectionValue(this.objectLocal, itemType, obj3, collectionContract);
							}
						}
						else
						{
							this.HandleUnexpectedItemInCollection(ref j);
						}
					}
					this.context.CheckEndOfArray(this.xmlReader, arraySize, this.itemName, this.itemNamespace);
				}
			}
			if (flag2)
			{
				this.context.AddNewObjectWithId(objectId, this.objectLocal);
			}
		}

		// Token: 0x06000F57 RID: 3927 RVA: 0x0003EF1C File Offset: 0x0003D11C
		private void ReadGetOnlyCollection(CollectionDataContract collectionContract)
		{
			Type underlyingType = collectionContract.UnderlyingType;
			Type itemType = collectionContract.ItemType;
			bool flag = collectionContract.Kind == CollectionKind.Array;
			string text = collectionContract.ItemName;
			string @namespace = collectionContract.StableName.Namespace;
			this.objectLocal = this.context.GetCollectionMember();
			if (this.IsStartElement(this.itemName, this.itemNamespace))
			{
				if (this.objectLocal == null)
				{
					XmlObjectSerializerReadContext.ThrowNullValueReturnedForGetOnlyCollectionException(underlyingType);
					return;
				}
				int num = 0;
				if (flag)
				{
					num = ((Array)this.objectLocal).Length;
				}
				this.context.AddNewObject(this.objectLocal);
				int i = 0;
				while (i < 2147483647)
				{
					if (this.IsStartElement(this.itemName, this.itemNamespace))
					{
						this.context.IncrementItemCount(1);
						object obj = this.ReadCollectionItem(collectionContract, itemType, text, @namespace);
						if (flag)
						{
							if (num == i)
							{
								XmlObjectSerializerReadContext.ThrowArrayExceededSizeException(num, underlyingType);
							}
							else
							{
								((Array)this.objectLocal).SetValue(obj, i);
							}
						}
						else
						{
							this.StoreCollectionValue(this.objectLocal, itemType, obj, collectionContract);
						}
					}
					else
					{
						if (this.IsEndElement())
						{
							break;
						}
						this.HandleUnexpectedItemInCollection(ref i);
					}
				}
				this.context.CheckEndOfArray(this.xmlReader, num, this.itemName, this.itemNamespace);
			}
		}

		// Token: 0x06000F58 RID: 3928 RVA: 0x0003F060 File Offset: 0x0003D260
		private bool TryReadPrimitiveArray(Type type, Type itemType, int size, out bool readResult)
		{
			readResult = false;
			if (PrimitiveDataContract.GetPrimitiveDataContract(itemType) == null)
			{
				return false;
			}
			string text = null;
			TypeCode typeCode = Type.GetTypeCode(itemType);
			if (typeCode != TypeCode.Boolean)
			{
				switch (typeCode)
				{
				case TypeCode.Int32:
					text = "TryReadInt32Array";
					break;
				case TypeCode.Int64:
					text = "TryReadInt64Array";
					break;
				case TypeCode.Single:
					text = "TryReadSingleArray";
					break;
				case TypeCode.Double:
					text = "TryReadDoubleArray";
					break;
				case TypeCode.Decimal:
					text = "TryReadDecimalArray";
					break;
				case TypeCode.DateTime:
					text = "TryReadDateTimeArray";
					break;
				}
			}
			else
			{
				text = "TryReadBooleanArray";
			}
			if (text != null)
			{
				MethodInfo method = typeof(XmlReaderDelegator).GetMethod(text, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				object[] array = new object[] { this.context, this.itemName, this.itemNamespace, size, this.objectLocal };
				readResult = (bool)method.Invoke(this.xmlReader, array);
				this.objectLocal = array.Last<object>();
				return true;
			}
			return false;
		}

		// Token: 0x06000F59 RID: 3929 RVA: 0x0003F158 File Offset: 0x0003D358
		private object ReadCollectionItem(CollectionDataContract collectionContract, Type itemType, string itemName, string itemNs)
		{
			if (collectionContract.Kind == CollectionKind.Dictionary || collectionContract.Kind == CollectionKind.GenericDictionary)
			{
				this.context.ResetAttributes();
				return CodeInterpreter.ConvertValue(collectionContract.ItemContract.ReadXmlValue(this.xmlReader, this.context), Globals.TypeOfObject, itemType);
			}
			return this.ReadValue(itemType, itemName, itemNs);
		}

		// Token: 0x06000F5A RID: 3930 RVA: 0x0003F1B0 File Offset: 0x0003D3B0
		private void StoreCollectionValue(object collection, Type valueType, object value, CollectionDataContract collectionContract)
		{
			if (collectionContract.Kind == CollectionKind.GenericDictionary || collectionContract.Kind == CollectionKind.Dictionary)
			{
				ClassDataContract classDataContract = DataContract.GetDataContract(valueType) as ClassDataContract;
				DataMember dataMember = classDataContract.Members[0];
				DataMember dataMember2 = classDataContract.Members[1];
				object member = CodeInterpreter.GetMember(dataMember.MemberInfo, value);
				object member2 = CodeInterpreter.GetMember(dataMember2.MemberInfo, value);
				collectionContract.AddMethod.Invoke(collection, new object[] { member, member2 });
				return;
			}
			collectionContract.AddMethod.Invoke(collection, new object[] { value });
		}

		// Token: 0x06000F5B RID: 3931 RVA: 0x0003F244 File Offset: 0x0003D444
		private void HandleUnexpectedItemInCollection(ref int iterator)
		{
			if (this.IsStartElement())
			{
				this.context.SkipUnknownElement(this.xmlReader);
				iterator--;
				return;
			}
			throw XmlObjectSerializerReadContext.CreateUnexpectedStateException(XmlNodeType.Element, this.xmlReader);
		}

		// Token: 0x06000F5C RID: 3932 RVA: 0x0003F272 File Offset: 0x0003D472
		private bool IsStartElement(XmlDictionaryString name, XmlDictionaryString ns)
		{
			return this.xmlReader.IsStartElement(name, ns);
		}

		// Token: 0x06000F5D RID: 3933 RVA: 0x0003F281 File Offset: 0x0003D481
		private bool IsStartElement()
		{
			return this.xmlReader.IsStartElement();
		}

		// Token: 0x06000F5E RID: 3934 RVA: 0x0003F28E File Offset: 0x0003D48E
		private bool IsEndElement()
		{
			return this.xmlReader.NodeType == XmlNodeType.EndElement;
		}

		// Token: 0x040007AC RID: 1964
		private bool is_get_only_collection;

		// Token: 0x040007AD RID: 1965
		private ClassDataContract classContract;

		// Token: 0x040007AE RID: 1966
		private CollectionDataContract collectionContract;

		// Token: 0x040007AF RID: 1967
		private object objectLocal;

		// Token: 0x040007B0 RID: 1968
		private Type objectType;

		// Token: 0x040007B1 RID: 1969
		private XmlReaderDelegator xmlReader;

		// Token: 0x040007B2 RID: 1970
		private XmlObjectSerializerReadContext context;

		// Token: 0x040007B3 RID: 1971
		private XmlDictionaryString[] memberNames;

		// Token: 0x040007B4 RID: 1972
		private XmlDictionaryString[] memberNamespaces;

		// Token: 0x040007B5 RID: 1973
		private XmlDictionaryString itemName;

		// Token: 0x040007B6 RID: 1974
		private XmlDictionaryString itemNamespace;
	}
}
