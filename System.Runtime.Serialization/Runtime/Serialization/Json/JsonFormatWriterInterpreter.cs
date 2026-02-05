using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace System.Runtime.Serialization.Json
{
	// Token: 0x0200011C RID: 284
	internal class JsonFormatWriterInterpreter
	{
		// Token: 0x06001163 RID: 4451 RVA: 0x00048D21 File Offset: 0x00046F21
		public JsonFormatWriterInterpreter(ClassDataContract classContract)
		{
			this.classContract = classContract;
		}

		// Token: 0x06001164 RID: 4452 RVA: 0x00048D37 File Offset: 0x00046F37
		public JsonFormatWriterInterpreter(CollectionDataContract collectionContract)
		{
			this.collectionContract = collectionContract;
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06001165 RID: 4453 RVA: 0x00048D4D File Offset: 0x00046F4D
		private ClassDataContract classDataContract
		{
			get
			{
				return (ClassDataContract)this.dataContract;
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06001166 RID: 4454 RVA: 0x00048D5A File Offset: 0x00046F5A
		private CollectionDataContract collectionDataContract
		{
			get
			{
				return (CollectionDataContract)this.dataContract;
			}
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x00048D68 File Offset: 0x00046F68
		public void WriteToJson(XmlWriterDelegator xmlWriter, object obj, XmlObjectSerializerWriteContextComplexJson context, ClassDataContract dataContract, XmlDictionaryString[] memberNames)
		{
			this.writer = xmlWriter;
			this.obj = obj;
			this.context = context;
			this.dataContract = dataContract;
			this.memberNames = memberNames;
			this.InitArgs(this.classContract.UnderlyingType);
			if (this.classContract.IsReadOnlyContract)
			{
				DataContract.ThrowInvalidDataContractException(this.classContract.SerializationExceptionMessage, null);
			}
			this.WriteClass(this.classContract);
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x00048DD8 File Offset: 0x00046FD8
		public void WriteCollectionToJson(XmlWriterDelegator xmlWriter, object obj, XmlObjectSerializerWriteContextComplexJson context, CollectionDataContract dataContract)
		{
			this.writer = xmlWriter;
			this.obj = obj;
			this.context = context;
			this.dataContract = dataContract;
			this.InitArgs(this.collectionContract.UnderlyingType);
			if (this.collectionContract.IsReadOnlyContract)
			{
				DataContract.ThrowInvalidDataContractException(this.collectionContract.SerializationExceptionMessage, null);
			}
			this.WriteCollection(this.collectionContract);
		}

		// Token: 0x06001169 RID: 4457 RVA: 0x00048E40 File Offset: 0x00047040
		private void InitArgs(Type objType)
		{
			if (objType == Globals.TypeOfDateTimeOffsetAdapter)
			{
				this.objLocal = DateTimeOffsetAdapter.GetDateTimeOffsetAdapter((DateTimeOffset)this.obj);
				return;
			}
			this.objLocal = CodeInterpreter.ConvertValue(this.obj, typeof(object), objType);
		}

		// Token: 0x0600116A RID: 4458 RVA: 0x00048E94 File Offset: 0x00047094
		private void InvokeOnSerializing(ClassDataContract classContract, object objSerialized, XmlObjectSerializerWriteContext context)
		{
			if (classContract.BaseContract != null)
			{
				this.InvokeOnSerializing(classContract.BaseContract, objSerialized, context);
			}
			if (classContract.OnSerializing != null)
			{
				classContract.OnSerializing.Invoke(objSerialized, new object[] { context.GetStreamingContext() });
			}
		}

		// Token: 0x0600116B RID: 4459 RVA: 0x00048EE8 File Offset: 0x000470E8
		private void InvokeOnSerialized(ClassDataContract classContract, object objSerialized, XmlObjectSerializerWriteContext context)
		{
			if (classContract.BaseContract != null)
			{
				this.InvokeOnSerialized(classContract.BaseContract, objSerialized, context);
			}
			if (classContract.OnSerialized != null)
			{
				classContract.OnSerialized.Invoke(objSerialized, new object[] { context.GetStreamingContext() });
			}
		}

		// Token: 0x0600116C RID: 4460 RVA: 0x00048F3C File Offset: 0x0004713C
		private void WriteClass(ClassDataContract classContract)
		{
			this.InvokeOnSerializing(classContract, this.objLocal, this.context);
			if (classContract.IsISerializable)
			{
				this.context.WriteJsonISerializable(this.writer, (ISerializable)this.objLocal);
			}
			else if (classContract.HasExtensionData)
			{
				ExtensionDataObject extensionData = ((IExtensibleDataObject)this.objLocal).ExtensionData;
				this.context.WriteExtensionData(this.writer, extensionData, -1);
				this.WriteMembers(classContract, extensionData, classContract);
			}
			else
			{
				this.WriteMembers(classContract, null, classContract);
			}
			this.InvokeOnSerialized(classContract, this.objLocal, this.context);
		}

		// Token: 0x0600116D RID: 4461 RVA: 0x00048FD8 File Offset: 0x000471D8
		private void WriteCollection(CollectionDataContract collectionContract)
		{
			XmlDictionaryString collectionItemName = this.context.CollectionItemName;
			if (collectionContract.Kind == CollectionKind.Array)
			{
				Type itemType = collectionContract.ItemType;
				if (this.objLocal.GetType().GetElementType() != itemType)
				{
					throw new InvalidCastException(string.Format("Cannot cast array of {0} to array of {1}", this.objLocal.GetType().GetElementType(), itemType));
				}
				this.context.IncrementArrayCount(this.writer, (Array)this.objLocal);
				if (!this.TryWritePrimitiveArray(collectionContract.UnderlyingType, itemType, () => this.objLocal, collectionItemName))
				{
					this.WriteArrayAttribute();
					Array array = (Array)this.objLocal;
					int[] array2 = new int[1];
					for (int i = 0; i < array.Length; i++)
					{
						if (!this.TryWritePrimitive(itemType, null, null, new int?(i), collectionItemName, 0))
						{
							this.WriteStartElement(collectionItemName, 0);
							array2[0] = i;
							object value = array.GetValue(array2);
							this.WriteValue(itemType, value);
							this.WriteEndElement();
						}
					}
					return;
				}
			}
			else
			{
				if (!collectionContract.UnderlyingType.IsAssignableFrom(this.objLocal.GetType()))
				{
					throw new InvalidCastException(string.Format("Cannot cast {0} to {1}", this.objLocal.GetType(), collectionContract.UnderlyingType));
				}
				MethodInfo methodInfo = null;
				switch (collectionContract.Kind)
				{
				case CollectionKind.GenericDictionary:
					methodInfo = XmlFormatGeneratorStatics.IncrementCollectionCountGenericMethod.MakeGenericMethod(new Type[] { Globals.TypeOfKeyValuePair.MakeGenericType(collectionContract.ItemType.GetGenericArguments()) });
					break;
				case CollectionKind.Dictionary:
				case CollectionKind.List:
				case CollectionKind.Collection:
					methodInfo = XmlFormatGeneratorStatics.IncrementCollectionCountMethod;
					break;
				case CollectionKind.GenericList:
				case CollectionKind.GenericCollection:
					methodInfo = XmlFormatGeneratorStatics.IncrementCollectionCountGenericMethod.MakeGenericMethod(new Type[] { collectionContract.ItemType });
					break;
				}
				if (methodInfo != null)
				{
					methodInfo.Invoke(this.context, new object[] { this.writer, this.objLocal });
				}
				bool flag = false;
				bool flag2 = false;
				Type[] array3 = null;
				Type type;
				if (collectionContract.Kind == CollectionKind.GenericDictionary)
				{
					flag2 = true;
					array3 = collectionContract.ItemType.GetGenericArguments();
					type = Globals.TypeOfGenericDictionaryEnumerator.MakeGenericType(array3);
				}
				else if (collectionContract.Kind == CollectionKind.Dictionary)
				{
					flag = true;
					array3 = new Type[]
					{
						Globals.TypeOfObject,
						Globals.TypeOfObject
					};
					type = Globals.TypeOfDictionaryEnumerator;
				}
				else
				{
					type = collectionContract.GetEnumeratorMethod.ReturnType;
				}
				MethodInfo methodInfo2 = type.GetMethod("MoveNext", BindingFlags.Instance | BindingFlags.Public, null, Globals.EmptyTypeArray, null);
				MethodInfo methodInfo3 = type.GetMethod("get_Current", BindingFlags.Instance | BindingFlags.Public, null, Globals.EmptyTypeArray, null);
				if (methodInfo2 == null || methodInfo3 == null)
				{
					if (type.IsInterface)
					{
						if (methodInfo2 == null)
						{
							methodInfo2 = JsonFormatGeneratorStatics.MoveNextMethod;
						}
						if (methodInfo3 == null)
						{
							methodInfo3 = JsonFormatGeneratorStatics.GetCurrentMethod;
						}
					}
					else
					{
						Type type2 = Globals.TypeOfIEnumerator;
						CollectionKind kind = collectionContract.Kind;
						if (kind == CollectionKind.GenericDictionary || kind == CollectionKind.GenericCollection || kind == CollectionKind.GenericEnumerable)
						{
							foreach (Type type3 in type.GetInterfaces())
							{
								if (type3.IsGenericType && type3.GetGenericTypeDefinition() == Globals.TypeOfIEnumeratorGeneric && type3.GetGenericArguments()[0] == collectionContract.ItemType)
								{
									type2 = type3;
									break;
								}
							}
						}
						if (methodInfo2 == null)
						{
							methodInfo2 = CollectionDataContract.GetTargetMethodWithName("MoveNext", type, type2);
						}
						if (methodInfo3 == null)
						{
							methodInfo3 = CollectionDataContract.GetTargetMethodWithName("get_Current", type, type2);
						}
					}
				}
				Type returnType = methodInfo3.ReturnType;
				object currentValue = null;
				IEnumerator enumerator = (IEnumerator)collectionContract.GetEnumeratorMethod.Invoke(this.objLocal, new object[0]);
				if (flag)
				{
					enumerator = (IEnumerator)type.GetConstructor(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { Globals.TypeOfIDictionaryEnumerator }, null).Invoke(new object[] { enumerator });
				}
				else if (flag2)
				{
					Type type4 = Globals.TypeOfIEnumeratorGeneric.MakeGenericType(new Type[] { Globals.TypeOfKeyValuePair.MakeGenericType(array3) });
					type.GetConstructor(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { type4 }, null);
					enumerator = (IEnumerator)Activator.CreateInstance(type, new object[] { enumerator });
				}
				bool flag3 = flag || flag2;
				bool flag4 = flag3 && this.context.UseSimpleDictionaryFormat;
				PropertyInfo propertyInfo = null;
				PropertyInfo propertyInfo2 = null;
				if (flag3)
				{
					Type type5 = Globals.TypeOfKeyValue.MakeGenericType(array3);
					propertyInfo = type5.GetProperty("Key");
					propertyInfo2 = type5.GetProperty("Value");
				}
				if (flag4)
				{
					this.WriteObjectAttribute();
					object[] array4 = new object[0];
					while ((bool)methodInfo2.Invoke(enumerator, array4))
					{
						currentValue = methodInfo3.Invoke(enumerator, array4);
						object member = CodeInterpreter.GetMember(propertyInfo, currentValue);
						object member2 = CodeInterpreter.GetMember(propertyInfo2, currentValue);
						this.WriteStartElement(member, 0);
						this.WriteValue(propertyInfo2.PropertyType, member2);
						this.WriteEndElement();
					}
					return;
				}
				this.WriteArrayAttribute();
				object[] array5 = new object[0];
				Func<object> <>9__1;
				while (enumerator != null && enumerator.MoveNext())
				{
					currentValue = methodInfo3.Invoke(enumerator, array5);
					if (methodInfo == null)
					{
						XmlFormatGeneratorStatics.IncrementItemCountMethod.Invoke(this.context, new object[] { 1 });
					}
					Type type6 = returnType;
					Func<object> func;
					if ((func = <>9__1) == null)
					{
						func = (<>9__1 = () => currentValue);
					}
					if (!this.TryWritePrimitive(type6, func, null, null, collectionItemName, 0))
					{
						this.WriteStartElement(collectionItemName, 0);
						if (flag2 || flag)
						{
							DataContractJsonSerializer.WriteJsonValue(JsonDataContract.GetJsonDataContract(XmlObjectSerializerWriteContextComplexJson.GetRevisedItemContract(this.collectionDataContract.ItemContract)), this.writer, currentValue, this.context, currentValue.GetType().TypeHandle);
						}
						else
						{
							this.WriteValue(returnType, currentValue);
						}
						this.WriteEndElement();
					}
				}
			}
		}

		// Token: 0x0600116E RID: 4462 RVA: 0x000495D8 File Offset: 0x000477D8
		private int WriteMembers(ClassDataContract classContract, ExtensionDataObject extensionData, ClassDataContract derivedMostClassContract)
		{
			int num = ((classContract.BaseContract == null) ? 0 : this.WriteMembers(classContract.BaseContract, extensionData, derivedMostClassContract));
			this.context.IncrementItemCount(classContract.Members.Count);
			int i = 0;
			while (i < classContract.Members.Count)
			{
				DataMember dataMember = classContract.Members[i];
				Type memberType = dataMember.MemberType;
				object memberValue = null;
				if (dataMember.IsGetOnlyCollection)
				{
					this.context.StoreIsGetOnlyCollection();
				}
				bool flag = true;
				bool flag2 = false;
				if (!dataMember.EmitDefaultValue)
				{
					flag2 = true;
					memberValue = this.LoadMemberValue(dataMember);
					flag = !this.IsDefaultValue(memberType, memberValue);
				}
				if (flag)
				{
					bool flag3 = DataContractJsonSerializer.CheckIfXmlNameRequiresMapping(classContract.MemberNames[i]);
					if (flag3 || !this.TryWritePrimitive(memberType, flag2 ? (() => memberValue) : null, dataMember.MemberInfo, null, null, i + this.childElementIndex))
					{
						if (flag3)
						{
							XmlObjectSerializerWriteContextComplexJson.WriteJsonNameWithMapping(this.writer, this.memberNames, i + this.childElementIndex);
						}
						else
						{
							this.WriteStartElement(null, i + this.childElementIndex);
						}
						if (memberValue == null)
						{
							memberValue = this.LoadMemberValue(dataMember);
						}
						this.WriteValue(memberType, memberValue);
						this.WriteEndElement();
					}
					if (classContract.HasExtensionData)
					{
						this.context.WriteExtensionData(this.writer, extensionData, num);
					}
				}
				else if (!dataMember.EmitDefaultValue && dataMember.IsRequired)
				{
					XmlObjectSerializerWriteContext.ThrowRequiredMemberMustBeEmitted(dataMember.Name, classContract.UnderlyingType);
				}
				i++;
				num++;
			}
			this.typeIndex++;
			this.childElementIndex += classContract.Members.Count;
			return num;
		}

		// Token: 0x0600116F RID: 4463 RVA: 0x000497A8 File Offset: 0x000479A8
		internal bool IsDefaultValue(Type type, object value)
		{
			object defaultValue = this.GetDefaultValue(type);
			if (defaultValue != null)
			{
				return defaultValue.Equals(value);
			}
			return value == null;
		}

		// Token: 0x06001170 RID: 4464 RVA: 0x000497CC File Offset: 0x000479CC
		internal object GetDefaultValue(Type type)
		{
			if (type.IsValueType)
			{
				switch (Type.GetTypeCode(type))
				{
				case TypeCode.Boolean:
					return false;
				case TypeCode.Char:
				case TypeCode.SByte:
				case TypeCode.Byte:
				case TypeCode.Int16:
				case TypeCode.UInt16:
				case TypeCode.Int32:
				case TypeCode.UInt32:
					return 0;
				case TypeCode.Int64:
				case TypeCode.UInt64:
					return 0L;
				case TypeCode.Single:
					return 0f;
				case TypeCode.Double:
					return 0.0;
				case TypeCode.Decimal:
					return 0m;
				case TypeCode.DateTime:
					return default(DateTime);
				}
			}
			return null;
		}

		// Token: 0x06001171 RID: 4465 RVA: 0x00049878 File Offset: 0x00047A78
		private void WriteStartElement(object nameLocal, int nameIndex)
		{
			object obj = nameLocal ?? this.memberNames[nameIndex];
			if (nameLocal != null && nameLocal is string)
			{
				this.writer.WriteStartElement((string)obj, null);
				return;
			}
			this.writer.WriteStartElement((XmlDictionaryString)obj, null);
		}

		// Token: 0x06001172 RID: 4466 RVA: 0x000498C3 File Offset: 0x00047AC3
		private void WriteEndElement()
		{
			this.writer.WriteEndElement();
		}

		// Token: 0x06001173 RID: 4467 RVA: 0x000498D0 File Offset: 0x00047AD0
		private void WriteArrayAttribute()
		{
			this.writer.WriteAttributeString(null, "type", string.Empty, "array");
		}

		// Token: 0x06001174 RID: 4468 RVA: 0x000498ED File Offset: 0x00047AED
		private void WriteObjectAttribute()
		{
			this.writer.WriteAttributeString(null, "type", null, "object");
		}

		// Token: 0x06001175 RID: 4469 RVA: 0x00049908 File Offset: 0x00047B08
		private void WriteValue(Type memberType, object memberValue)
		{
			if (memberType.IsPointer)
			{
				Pointer pointer = (Pointer)JsonFormatGeneratorStatics.BoxPointer.Invoke(null, new object[] { memberValue, memberType });
			}
			bool flag = memberType.IsGenericType && memberType.GetGenericTypeDefinition() == Globals.TypeOfNullable;
			if (memberType.IsValueType && !flag)
			{
				PrimitiveDataContract primitiveDataContract = PrimitiveDataContract.GetPrimitiveDataContract(memberType);
				if (primitiveDataContract != null)
				{
					primitiveDataContract.XmlFormatContentWriterMethod.Invoke(this.writer, new object[] { memberValue });
					return;
				}
				this.InternalSerialize(XmlFormatGeneratorStatics.InternalSerializeMethod, () => memberValue, memberType, false);
				return;
			}
			else
			{
				bool flag2;
				if (flag)
				{
					memberValue = this.UnwrapNullableObject(() => memberValue, ref memberType, out flag2);
				}
				else
				{
					flag2 = memberValue == null;
				}
				if (flag2)
				{
					XmlFormatGeneratorStatics.WriteNullMethod.Invoke(this.context, new object[]
					{
						this.writer,
						memberType,
						DataContract.IsTypeSerializable(memberType)
					});
					return;
				}
				PrimitiveDataContract primitiveDataContract2 = PrimitiveDataContract.GetPrimitiveDataContract(memberType);
				if (primitiveDataContract2 != null && primitiveDataContract2.UnderlyingType != Globals.TypeOfObject)
				{
					if (flag)
					{
						primitiveDataContract2.XmlFormatContentWriterMethod.Invoke(this.writer, new object[] { memberValue });
						return;
					}
					primitiveDataContract2.XmlFormatContentWriterMethod.Invoke(this.context, new object[] { this.writer, memberValue });
					return;
				}
				else
				{
					bool flag3 = false;
					if (memberType == Globals.TypeOfObject || memberType == Globals.TypeOfValueType || ((IList)Globals.TypeOfNullable.GetInterfaces()).Contains(memberType))
					{
						object obj = CodeInterpreter.ConvertValue(memberValue, memberType.GetType(), Globals.TypeOfObject);
						memberValue = obj;
						flag3 = memberValue == null;
					}
					if (flag3)
					{
						XmlFormatGeneratorStatics.WriteNullMethod.Invoke(this.context, new object[]
						{
							this.writer,
							memberType,
							DataContract.IsTypeSerializable(memberType)
						});
						return;
					}
					this.InternalSerialize(flag ? XmlFormatGeneratorStatics.InternalSerializeMethod : XmlFormatGeneratorStatics.InternalSerializeReferenceMethod, () => memberValue, memberType, false);
					return;
				}
			}
		}

		// Token: 0x06001176 RID: 4470 RVA: 0x00049B44 File Offset: 0x00047D44
		private void InternalSerialize(MethodInfo methodInfo, Func<object> memberValue, Type memberType, bool writeXsiType)
		{
			object obj = memberValue();
			bool flag = Type.GetTypeHandle(obj).Equals(CodeInterpreter.ConvertValue(obj, memberType, Globals.TypeOfObject));
			try
			{
				methodInfo.Invoke(this.context, new object[]
				{
					this.writer,
					(memberValue != null) ? obj : null,
					flag,
					writeXsiType,
					DataContract.GetId(memberType.TypeHandle),
					memberType.TypeHandle
				});
			}
			catch (TargetInvocationException ex)
			{
				if (ex.InnerException != null)
				{
					throw ex.InnerException;
				}
				throw;
			}
		}

		// Token: 0x06001177 RID: 4471 RVA: 0x00049BF4 File Offset: 0x00047DF4
		private object UnwrapNullableObject(Func<object> memberValue, ref Type memberType, out bool isNull)
		{
			object obj = memberValue();
			isNull = false;
			while (memberType.IsGenericType && memberType.GetGenericTypeDefinition() == Globals.TypeOfNullable)
			{
				Type type = memberType.GetGenericArguments()[0];
				if ((bool)XmlFormatGeneratorStatics.GetHasValueMethod.MakeGenericMethod(new Type[] { type }).Invoke(null, new object[] { obj }))
				{
					obj = XmlFormatGeneratorStatics.GetNullableValueMethod.MakeGenericMethod(new Type[] { type }).Invoke(null, new object[] { obj });
				}
				else
				{
					isNull = true;
					obj = XmlFormatGeneratorStatics.GetDefaultValueMethod.MakeGenericMethod(new Type[] { memberType }).Invoke(null, new object[0]);
				}
				memberType = type;
			}
			return obj;
		}

		// Token: 0x06001178 RID: 4472 RVA: 0x00049CB4 File Offset: 0x00047EB4
		private bool TryWritePrimitive(Type type, Func<object> value, MemberInfo memberInfo, int? arrayItemIndex, XmlDictionaryString name, int nameIndex)
		{
			PrimitiveDataContract primitiveDataContract = PrimitiveDataContract.GetPrimitiveDataContract(type);
			if (primitiveDataContract == null || primitiveDataContract.UnderlyingType == Globals.TypeOfObject)
			{
				return false;
			}
			List<object> list = new List<object>();
			object obj;
			if (type.IsValueType)
			{
				obj = this.writer;
			}
			else
			{
				obj = this.context;
				list.Add(this.writer);
			}
			if (value != null)
			{
				list.Add(value());
			}
			else if (memberInfo != null)
			{
				list.Add(CodeInterpreter.GetMember(memberInfo, this.objLocal));
			}
			else
			{
				list.Add(((Array)this.objLocal).GetValue(new int[] { arrayItemIndex.Value }));
			}
			if (name != null)
			{
				list.Add(name);
			}
			else
			{
				list.Add(this.memberNames[nameIndex]);
			}
			list.Add(null);
			primitiveDataContract.XmlFormatWriterMethod.Invoke(obj, list.ToArray());
			return true;
		}

		// Token: 0x06001179 RID: 4473 RVA: 0x00049D98 File Offset: 0x00047F98
		private bool TryWritePrimitiveArray(Type type, Type itemType, Func<object> value, XmlDictionaryString itemName)
		{
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
					text = "WriteJsonInt32Array";
					break;
				case TypeCode.Int64:
					text = "WriteJsonInt64Array";
					break;
				case TypeCode.Single:
					text = "WriteJsonSingleArray";
					break;
				case TypeCode.Double:
					text = "WriteJsonDoubleArray";
					break;
				case TypeCode.Decimal:
					text = "WriteJsonDecimalArray";
					break;
				case TypeCode.DateTime:
					text = "WriteJsonDateTimeArray";
					break;
				}
			}
			else
			{
				text = "WriteJsonBooleanArray";
			}
			if (text != null)
			{
				this.WriteArrayAttribute();
				MethodBase method = typeof(JsonWriterDelegator).GetMethod(text, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[]
				{
					type,
					typeof(XmlDictionaryString),
					typeof(XmlDictionaryString)
				}, null);
				object obj = this.writer;
				object[] array = new object[3];
				array[0] = value();
				array[1] = itemName;
				method.Invoke(obj, array);
				return true;
			}
			return false;
		}

		// Token: 0x0600117A RID: 4474 RVA: 0x00049E81 File Offset: 0x00048081
		private object LoadMemberValue(DataMember member)
		{
			return CodeInterpreter.GetMember(member.MemberInfo, this.objLocal);
		}

		// Token: 0x0400086F RID: 2159
		private ClassDataContract classContract;

		// Token: 0x04000870 RID: 2160
		private CollectionDataContract collectionContract;

		// Token: 0x04000871 RID: 2161
		private XmlWriterDelegator writer;

		// Token: 0x04000872 RID: 2162
		private object obj;

		// Token: 0x04000873 RID: 2163
		private XmlObjectSerializerWriteContextComplexJson context;

		// Token: 0x04000874 RID: 2164
		private DataContract dataContract;

		// Token: 0x04000875 RID: 2165
		private object objLocal;

		// Token: 0x04000876 RID: 2166
		private XmlDictionaryString[] memberNames;

		// Token: 0x04000877 RID: 2167
		private int typeIndex = 1;

		// Token: 0x04000878 RID: 2168
		private int childElementIndex;
	}
}
