using System;
using System.Collections.Generic;
using System.Security;
using System.Xml;

namespace System.Runtime.Serialization.Json
{
	// Token: 0x02000103 RID: 259
	internal class JsonDataContract
	{
		// Token: 0x06000FE3 RID: 4067 RVA: 0x0004147A File Offset: 0x0003F67A
		[SecuritySafeCritical]
		protected JsonDataContract(DataContract traditionalDataContract)
		{
			this.helper = new JsonDataContract.JsonDataContractCriticalHelper(traditionalDataContract);
		}

		// Token: 0x06000FE4 RID: 4068 RVA: 0x0004148E File Offset: 0x0003F68E
		[SecuritySafeCritical]
		protected JsonDataContract(JsonDataContract.JsonDataContractCriticalHelper helper)
		{
			this.helper = helper;
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000FE5 RID: 4069 RVA: 0x0004149D File Offset: 0x0003F69D
		internal virtual string TypeName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000FE6 RID: 4070 RVA: 0x000414A0 File Offset: 0x0003F6A0
		protected JsonDataContract.JsonDataContractCriticalHelper Helper
		{
			[SecurityCritical]
			get
			{
				return this.helper;
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000FE7 RID: 4071 RVA: 0x000414A8 File Offset: 0x0003F6A8
		protected DataContract TraditionalDataContract
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.TraditionalDataContract;
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000FE8 RID: 4072 RVA: 0x000414B5 File Offset: 0x0003F6B5
		private Dictionary<XmlQualifiedName, DataContract> KnownDataContracts
		{
			[SecuritySafeCritical]
			get
			{
				return this.helper.KnownDataContracts;
			}
		}

		// Token: 0x06000FE9 RID: 4073 RVA: 0x000414C2 File Offset: 0x0003F6C2
		[SecuritySafeCritical]
		public static JsonDataContract GetJsonDataContract(DataContract traditionalDataContract)
		{
			return JsonDataContract.JsonDataContractCriticalHelper.GetJsonDataContract(traditionalDataContract);
		}

		// Token: 0x06000FEA RID: 4074 RVA: 0x000414CA File Offset: 0x0003F6CA
		public object ReadJsonValue(XmlReaderDelegator jsonReader, XmlObjectSerializerReadContextComplexJson context)
		{
			this.PushKnownDataContracts(context);
			object obj = this.ReadJsonValueCore(jsonReader, context);
			this.PopKnownDataContracts(context);
			return obj;
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x000414E2 File Offset: 0x0003F6E2
		public virtual object ReadJsonValueCore(XmlReaderDelegator jsonReader, XmlObjectSerializerReadContextComplexJson context)
		{
			return this.TraditionalDataContract.ReadXmlValue(jsonReader, context);
		}

		// Token: 0x06000FEC RID: 4076 RVA: 0x000414F1 File Offset: 0x0003F6F1
		public void WriteJsonValue(XmlWriterDelegator jsonWriter, object obj, XmlObjectSerializerWriteContextComplexJson context, RuntimeTypeHandle declaredTypeHandle)
		{
			this.PushKnownDataContracts(context);
			this.WriteJsonValueCore(jsonWriter, obj, context, declaredTypeHandle);
			this.PopKnownDataContracts(context);
		}

		// Token: 0x06000FED RID: 4077 RVA: 0x0004150C File Offset: 0x0003F70C
		public virtual void WriteJsonValueCore(XmlWriterDelegator jsonWriter, object obj, XmlObjectSerializerWriteContextComplexJson context, RuntimeTypeHandle declaredTypeHandle)
		{
			this.TraditionalDataContract.WriteXmlValue(jsonWriter, obj, context);
		}

		// Token: 0x06000FEE RID: 4078 RVA: 0x0004151C File Offset: 0x0003F71C
		protected static object HandleReadValue(object obj, XmlObjectSerializerReadContext context)
		{
			context.AddNewObject(obj);
			return obj;
		}

		// Token: 0x06000FEF RID: 4079 RVA: 0x00041526 File Offset: 0x0003F726
		protected static bool TryReadNullAtTopLevel(XmlReaderDelegator reader)
		{
			if (!reader.MoveToAttribute("type") || !(reader.Value == "null"))
			{
				reader.MoveToElement();
				return false;
			}
			reader.Skip();
			reader.MoveToElement();
			return true;
		}

		// Token: 0x06000FF0 RID: 4080 RVA: 0x00041560 File Offset: 0x0003F760
		protected void PopKnownDataContracts(XmlObjectSerializerContext context)
		{
			if (this.KnownDataContracts != null)
			{
				context.scopedKnownTypes.Pop();
			}
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x00041575 File Offset: 0x0003F775
		protected void PushKnownDataContracts(XmlObjectSerializerContext context)
		{
			if (this.KnownDataContracts != null)
			{
				context.scopedKnownTypes.Push(this.KnownDataContracts);
			}
		}

		// Token: 0x040007DD RID: 2013
		[SecurityCritical]
		private JsonDataContract.JsonDataContractCriticalHelper helper;

		// Token: 0x02000189 RID: 393
		[SecurityCritical(SecurityCriticalScope.Everything)]
		internal class JsonDataContractCriticalHelper
		{
			// Token: 0x0600151E RID: 5406 RVA: 0x00054D10 File Offset: 0x00052F10
			internal JsonDataContractCriticalHelper(DataContract traditionalDataContract)
			{
				this.traditionalDataContract = traditionalDataContract;
				this.AddCollectionItemContractsToKnownDataContracts();
				this.typeName = (string.IsNullOrEmpty(traditionalDataContract.Namespace.Value) ? traditionalDataContract.Name.Value : (traditionalDataContract.Name.Value + ":" + XmlObjectSerializerWriteContextComplexJson.TruncateDefaultDataContractNamespace(traditionalDataContract.Namespace.Value)));
			}

			// Token: 0x1700046A RID: 1130
			// (get) Token: 0x0600151F RID: 5407 RVA: 0x00054D7A File Offset: 0x00052F7A
			internal Dictionary<XmlQualifiedName, DataContract> KnownDataContracts
			{
				get
				{
					return this.knownDataContracts;
				}
			}

			// Token: 0x1700046B RID: 1131
			// (get) Token: 0x06001520 RID: 5408 RVA: 0x00054D82 File Offset: 0x00052F82
			internal DataContract TraditionalDataContract
			{
				get
				{
					return this.traditionalDataContract;
				}
			}

			// Token: 0x1700046C RID: 1132
			// (get) Token: 0x06001521 RID: 5409 RVA: 0x00054D8A File Offset: 0x00052F8A
			internal virtual string TypeName
			{
				get
				{
					return this.typeName;
				}
			}

			// Token: 0x06001522 RID: 5410 RVA: 0x00054D94 File Offset: 0x00052F94
			public static JsonDataContract GetJsonDataContract(DataContract traditionalDataContract)
			{
				int id = JsonDataContract.JsonDataContractCriticalHelper.GetId(traditionalDataContract.UnderlyingType.TypeHandle);
				JsonDataContract jsonDataContract = JsonDataContract.JsonDataContractCriticalHelper.dataContractCache[id];
				if (jsonDataContract == null)
				{
					jsonDataContract = JsonDataContract.JsonDataContractCriticalHelper.CreateJsonDataContract(id, traditionalDataContract);
					JsonDataContract.JsonDataContractCriticalHelper.dataContractCache[id] = jsonDataContract;
				}
				return jsonDataContract;
			}

			// Token: 0x06001523 RID: 5411 RVA: 0x00054DD0 File Offset: 0x00052FD0
			internal static int GetId(RuntimeTypeHandle typeHandle)
			{
				object obj = JsonDataContract.JsonDataContractCriticalHelper.cacheLock;
				int value;
				lock (obj)
				{
					JsonDataContract.JsonDataContractCriticalHelper.typeHandleRef.Value = typeHandle;
					IntRef intRef;
					if (!JsonDataContract.JsonDataContractCriticalHelper.typeToIDCache.TryGetValue(JsonDataContract.JsonDataContractCriticalHelper.typeHandleRef, out intRef))
					{
						int num = JsonDataContract.JsonDataContractCriticalHelper.dataContractID++;
						if (num >= JsonDataContract.JsonDataContractCriticalHelper.dataContractCache.Length)
						{
							int num2 = ((num < 1073741823) ? (num * 2) : int.MaxValue);
							if (num2 <= num)
							{
								throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new SerializationException(SR.GetString("An internal error has occurred. DataContract cache overflow.")));
							}
							Array.Resize<JsonDataContract>(ref JsonDataContract.JsonDataContractCriticalHelper.dataContractCache, num2);
						}
						intRef = new IntRef(num);
						try
						{
							JsonDataContract.JsonDataContractCriticalHelper.typeToIDCache.Add(new TypeHandleRef(typeHandle), intRef);
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
					value = intRef.Value;
				}
				return value;
			}

			// Token: 0x06001524 RID: 5412 RVA: 0x00054EC8 File Offset: 0x000530C8
			private static JsonDataContract CreateJsonDataContract(int id, DataContract traditionalDataContract)
			{
				object obj = JsonDataContract.JsonDataContractCriticalHelper.createDataContractLock;
				JsonDataContract jsonDataContract2;
				lock (obj)
				{
					JsonDataContract jsonDataContract = JsonDataContract.JsonDataContractCriticalHelper.dataContractCache[id];
					if (jsonDataContract == null)
					{
						Type type = traditionalDataContract.GetType();
						if (type == typeof(ObjectDataContract))
						{
							jsonDataContract = new JsonObjectDataContract(traditionalDataContract);
						}
						else if (type == typeof(StringDataContract))
						{
							jsonDataContract = new JsonStringDataContract((StringDataContract)traditionalDataContract);
						}
						else if (type == typeof(UriDataContract))
						{
							jsonDataContract = new JsonUriDataContract((UriDataContract)traditionalDataContract);
						}
						else if (type == typeof(QNameDataContract))
						{
							jsonDataContract = new JsonQNameDataContract((QNameDataContract)traditionalDataContract);
						}
						else if (type == typeof(ByteArrayDataContract))
						{
							jsonDataContract = new JsonByteArrayDataContract((ByteArrayDataContract)traditionalDataContract);
						}
						else if (traditionalDataContract.IsPrimitive || traditionalDataContract.UnderlyingType == Globals.TypeOfXmlQualifiedName)
						{
							jsonDataContract = new JsonDataContract(traditionalDataContract);
						}
						else if (type == typeof(ClassDataContract))
						{
							jsonDataContract = new JsonClassDataContract((ClassDataContract)traditionalDataContract);
						}
						else if (type == typeof(EnumDataContract))
						{
							jsonDataContract = new JsonEnumDataContract((EnumDataContract)traditionalDataContract);
						}
						else if (type == typeof(GenericParameterDataContract) || type == typeof(SpecialTypeDataContract))
						{
							jsonDataContract = new JsonDataContract(traditionalDataContract);
						}
						else if (type == typeof(CollectionDataContract))
						{
							jsonDataContract = new JsonCollectionDataContract((CollectionDataContract)traditionalDataContract);
						}
						else
						{
							if (!(type == typeof(XmlDataContract)))
							{
								throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument("traditionalDataContract", SR.GetString("Type '{0}' is not suppotred by DataContractJsonSerializer.", new object[] { traditionalDataContract.UnderlyingType }));
							}
							jsonDataContract = new JsonXmlDataContract((XmlDataContract)traditionalDataContract);
						}
					}
					jsonDataContract2 = jsonDataContract;
				}
				return jsonDataContract2;
			}

			// Token: 0x06001525 RID: 5413 RVA: 0x000550D0 File Offset: 0x000532D0
			private void AddCollectionItemContractsToKnownDataContracts()
			{
				if (this.traditionalDataContract.KnownDataContracts != null)
				{
					foreach (KeyValuePair<XmlQualifiedName, DataContract> keyValuePair in this.traditionalDataContract.KnownDataContracts)
					{
						if (keyValuePair != null)
						{
							DataContract itemContract;
							for (CollectionDataContract collectionDataContract = keyValuePair.Value as CollectionDataContract; collectionDataContract != null; collectionDataContract = itemContract as CollectionDataContract)
							{
								itemContract = collectionDataContract.ItemContract;
								if (this.knownDataContracts == null)
								{
									this.knownDataContracts = new Dictionary<XmlQualifiedName, DataContract>();
								}
								if (!this.knownDataContracts.ContainsKey(itemContract.StableName))
								{
									this.knownDataContracts.Add(itemContract.StableName, itemContract);
								}
								if (collectionDataContract.ItemType.IsGenericType && collectionDataContract.ItemType.GetGenericTypeDefinition() == typeof(KeyValue<, >))
								{
									DataContract dataContract = DataContract.GetDataContract(Globals.TypeOfKeyValuePair.MakeGenericType(collectionDataContract.ItemType.GetGenericArguments()));
									if (!this.knownDataContracts.ContainsKey(dataContract.StableName))
									{
										this.knownDataContracts.Add(dataContract.StableName, dataContract);
									}
								}
								if (!(itemContract is CollectionDataContract))
								{
									break;
								}
							}
						}
					}
				}
			}

			// Token: 0x04000A4A RID: 2634
			private static object cacheLock = new object();

			// Token: 0x04000A4B RID: 2635
			private static object createDataContractLock = new object();

			// Token: 0x04000A4C RID: 2636
			private static JsonDataContract[] dataContractCache = new JsonDataContract[32];

			// Token: 0x04000A4D RID: 2637
			private static int dataContractID = 0;

			// Token: 0x04000A4E RID: 2638
			private static TypeHandleRef typeHandleRef = new TypeHandleRef();

			// Token: 0x04000A4F RID: 2639
			private static Dictionary<TypeHandleRef, IntRef> typeToIDCache = new Dictionary<TypeHandleRef, IntRef>(new TypeHandleRefEqualityComparer());

			// Token: 0x04000A50 RID: 2640
			private Dictionary<XmlQualifiedName, DataContract> knownDataContracts;

			// Token: 0x04000A51 RID: 2641
			private DataContract traditionalDataContract;

			// Token: 0x04000A52 RID: 2642
			private string typeName;
		}
	}
}
