using System;
using System.Collections;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Security;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x02000098 RID: 152
	internal static class JsonFormatGeneratorStatics
	{
		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000A86 RID: 2694 RVA: 0x0002CC04 File Offset: 0x0002AE04
		public static MethodInfo BoxPointer
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.boxPointer == null)
				{
					JsonFormatGeneratorStatics.boxPointer = typeof(Pointer).GetMethod("Box");
				}
				return JsonFormatGeneratorStatics.boxPointer;
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000A87 RID: 2695 RVA: 0x0002CC31 File Offset: 0x0002AE31
		public static PropertyInfo CollectionItemNameProperty
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.collectionItemNameProperty == null)
				{
					JsonFormatGeneratorStatics.collectionItemNameProperty = typeof(XmlObjectSerializerWriteContextComplexJson).GetProperty("CollectionItemName", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return JsonFormatGeneratorStatics.collectionItemNameProperty;
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000A88 RID: 2696 RVA: 0x0002CC60 File Offset: 0x0002AE60
		public static ConstructorInfo ExtensionDataObjectCtor
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.extensionDataObjectCtor == null)
				{
					JsonFormatGeneratorStatics.extensionDataObjectCtor = typeof(ExtensionDataObject).GetConstructor(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null);
				}
				return JsonFormatGeneratorStatics.extensionDataObjectCtor;
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000A89 RID: 2697 RVA: 0x0002CC92 File Offset: 0x0002AE92
		public static PropertyInfo ExtensionDataProperty
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.extensionDataProperty == null)
				{
					JsonFormatGeneratorStatics.extensionDataProperty = typeof(IExtensibleDataObject).GetProperty("ExtensionData");
				}
				return JsonFormatGeneratorStatics.extensionDataProperty;
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000A8A RID: 2698 RVA: 0x0002CCBF File Offset: 0x0002AEBF
		public static MethodInfo GetCurrentMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.ienumeratorGetCurrentMethod == null)
				{
					JsonFormatGeneratorStatics.ienumeratorGetCurrentMethod = typeof(IEnumerator).GetProperty("Current").GetGetMethod();
				}
				return JsonFormatGeneratorStatics.ienumeratorGetCurrentMethod;
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000A8B RID: 2699 RVA: 0x0002CCF1 File Offset: 0x0002AEF1
		public static MethodInfo GetItemContractMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.getItemContractMethod == null)
				{
					JsonFormatGeneratorStatics.getItemContractMethod = typeof(CollectionDataContract).GetProperty("ItemContract", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).GetGetMethod(true);
				}
				return JsonFormatGeneratorStatics.getItemContractMethod;
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000A8C RID: 2700 RVA: 0x0002CD26 File Offset: 0x0002AF26
		public static MethodInfo GetJsonDataContractMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.getJsonDataContractMethod == null)
				{
					JsonFormatGeneratorStatics.getJsonDataContractMethod = typeof(JsonDataContract).GetMethod("GetJsonDataContract", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return JsonFormatGeneratorStatics.getJsonDataContractMethod;
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000A8D RID: 2701 RVA: 0x0002CD55 File Offset: 0x0002AF55
		public static MethodInfo GetJsonMemberIndexMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.getJsonMemberIndexMethod == null)
				{
					JsonFormatGeneratorStatics.getJsonMemberIndexMethod = typeof(XmlObjectSerializerReadContextComplexJson).GetMethod("GetJsonMemberIndex", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return JsonFormatGeneratorStatics.getJsonMemberIndexMethod;
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000A8E RID: 2702 RVA: 0x0002CD84 File Offset: 0x0002AF84
		public static MethodInfo GetRevisedItemContractMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.getRevisedItemContractMethod == null)
				{
					JsonFormatGeneratorStatics.getRevisedItemContractMethod = typeof(XmlObjectSerializerWriteContextComplexJson).GetMethod("GetRevisedItemContract", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return JsonFormatGeneratorStatics.getRevisedItemContractMethod;
			}
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000A8F RID: 2703 RVA: 0x0002CDB4 File Offset: 0x0002AFB4
		public static MethodInfo GetUninitializedObjectMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.getUninitializedObjectMethod == null)
				{
					JsonFormatGeneratorStatics.getUninitializedObjectMethod = typeof(XmlFormatReaderGenerator).GetMethod("UnsafeGetUninitializedObject", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { typeof(int) }, null);
				}
				return JsonFormatGeneratorStatics.getUninitializedObjectMethod;
			}
		}

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000A90 RID: 2704 RVA: 0x0002CE03 File Offset: 0x0002B003
		public static MethodInfo IsStartElementMethod0
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.isStartElementMethod0 == null)
				{
					JsonFormatGeneratorStatics.isStartElementMethod0 = typeof(XmlReaderDelegator).GetMethod("IsStartElement", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null);
				}
				return JsonFormatGeneratorStatics.isStartElementMethod0;
			}
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000A91 RID: 2705 RVA: 0x0002CE3C File Offset: 0x0002B03C
		public static MethodInfo IsStartElementMethod2
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.isStartElementMethod2 == null)
				{
					JsonFormatGeneratorStatics.isStartElementMethod2 = typeof(XmlReaderDelegator).GetMethod("IsStartElement", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[]
					{
						typeof(XmlDictionaryString),
						typeof(XmlDictionaryString)
					}, null);
				}
				return JsonFormatGeneratorStatics.isStartElementMethod2;
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000A92 RID: 2706 RVA: 0x0002CE98 File Offset: 0x0002B098
		public static PropertyInfo LocalNameProperty
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.localNameProperty == null)
				{
					JsonFormatGeneratorStatics.localNameProperty = typeof(XmlReaderDelegator).GetProperty("LocalName", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return JsonFormatGeneratorStatics.localNameProperty;
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000A93 RID: 2707 RVA: 0x0002CEC7 File Offset: 0x0002B0C7
		public static PropertyInfo NamespaceProperty
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.namespaceProperty == null)
				{
					JsonFormatGeneratorStatics.namespaceProperty = typeof(XmlReaderDelegator).GetProperty("NamespaceProperty", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return JsonFormatGeneratorStatics.namespaceProperty;
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000A94 RID: 2708 RVA: 0x0002CEF6 File Offset: 0x0002B0F6
		public static MethodInfo MoveNextMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.ienumeratorMoveNextMethod == null)
				{
					JsonFormatGeneratorStatics.ienumeratorMoveNextMethod = typeof(IEnumerator).GetMethod("MoveNext");
				}
				return JsonFormatGeneratorStatics.ienumeratorMoveNextMethod;
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000A95 RID: 2709 RVA: 0x0002CF23 File Offset: 0x0002B123
		public static MethodInfo MoveToContentMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.moveToContentMethod == null)
				{
					JsonFormatGeneratorStatics.moveToContentMethod = typeof(XmlReaderDelegator).GetMethod("MoveToContent", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return JsonFormatGeneratorStatics.moveToContentMethod;
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000A96 RID: 2710 RVA: 0x0002CF52 File Offset: 0x0002B152
		public static PropertyInfo NodeTypeProperty
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.nodeTypeProperty == null)
				{
					JsonFormatGeneratorStatics.nodeTypeProperty = typeof(XmlReaderDelegator).GetProperty("NodeType", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return JsonFormatGeneratorStatics.nodeTypeProperty;
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000A97 RID: 2711 RVA: 0x0002CF81 File Offset: 0x0002B181
		public static MethodInfo OnDeserializationMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.onDeserializationMethod == null)
				{
					JsonFormatGeneratorStatics.onDeserializationMethod = typeof(IDeserializationCallback).GetMethod("OnDeserialization");
				}
				return JsonFormatGeneratorStatics.onDeserializationMethod;
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000A98 RID: 2712 RVA: 0x0002CFAE File Offset: 0x0002B1AE
		public static MethodInfo ReadJsonValueMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.readJsonValueMethod == null)
				{
					JsonFormatGeneratorStatics.readJsonValueMethod = typeof(DataContractJsonSerializer).GetMethod("ReadJsonValue", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return JsonFormatGeneratorStatics.readJsonValueMethod;
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000A99 RID: 2713 RVA: 0x0002CFDD File Offset: 0x0002B1DD
		public static ConstructorInfo SerializationExceptionCtor
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.serializationExceptionCtor == null)
				{
					JsonFormatGeneratorStatics.serializationExceptionCtor = typeof(SerializationException).GetConstructor(new Type[] { typeof(string) });
				}
				return JsonFormatGeneratorStatics.serializationExceptionCtor;
			}
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000A9A RID: 2714 RVA: 0x0002D018 File Offset: 0x0002B218
		public static Type[] SerInfoCtorArgs
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.serInfoCtorArgs == null)
				{
					JsonFormatGeneratorStatics.serInfoCtorArgs = new Type[]
					{
						typeof(SerializationInfo),
						typeof(StreamingContext)
					};
				}
				return JsonFormatGeneratorStatics.serInfoCtorArgs;
			}
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000A9B RID: 2715 RVA: 0x0002D04B File Offset: 0x0002B24B
		public static MethodInfo ThrowDuplicateMemberExceptionMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.throwDuplicateMemberExceptionMethod == null)
				{
					JsonFormatGeneratorStatics.throwDuplicateMemberExceptionMethod = typeof(XmlObjectSerializerReadContextComplexJson).GetMethod("ThrowDuplicateMemberException", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return JsonFormatGeneratorStatics.throwDuplicateMemberExceptionMethod;
			}
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000A9C RID: 2716 RVA: 0x0002D07A File Offset: 0x0002B27A
		public static MethodInfo ThrowMissingRequiredMembersMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.throwMissingRequiredMembersMethod == null)
				{
					JsonFormatGeneratorStatics.throwMissingRequiredMembersMethod = typeof(XmlObjectSerializerReadContextComplexJson).GetMethod("ThrowMissingRequiredMembers", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return JsonFormatGeneratorStatics.throwMissingRequiredMembersMethod;
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000A9D RID: 2717 RVA: 0x0002D0A9 File Offset: 0x0002B2A9
		public static PropertyInfo TypeHandleProperty
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.typeHandleProperty == null)
				{
					JsonFormatGeneratorStatics.typeHandleProperty = typeof(Type).GetProperty("TypeHandle");
				}
				return JsonFormatGeneratorStatics.typeHandleProperty;
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000A9E RID: 2718 RVA: 0x0002D0D6 File Offset: 0x0002B2D6
		public static MethodInfo UnboxPointer
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.unboxPointer == null)
				{
					JsonFormatGeneratorStatics.unboxPointer = typeof(Pointer).GetMethod("Unbox");
				}
				return JsonFormatGeneratorStatics.unboxPointer;
			}
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000A9F RID: 2719 RVA: 0x0002D103 File Offset: 0x0002B303
		public static PropertyInfo UseSimpleDictionaryFormatReadProperty
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.useSimpleDictionaryFormatReadProperty == null)
				{
					JsonFormatGeneratorStatics.useSimpleDictionaryFormatReadProperty = typeof(XmlObjectSerializerReadContextComplexJson).GetProperty("UseSimpleDictionaryFormat", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return JsonFormatGeneratorStatics.useSimpleDictionaryFormatReadProperty;
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000AA0 RID: 2720 RVA: 0x0002D132 File Offset: 0x0002B332
		public static PropertyInfo UseSimpleDictionaryFormatWriteProperty
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.useSimpleDictionaryFormatWriteProperty == null)
				{
					JsonFormatGeneratorStatics.useSimpleDictionaryFormatWriteProperty = typeof(XmlObjectSerializerWriteContextComplexJson).GetProperty("UseSimpleDictionaryFormat", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return JsonFormatGeneratorStatics.useSimpleDictionaryFormatWriteProperty;
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000AA1 RID: 2721 RVA: 0x0002D164 File Offset: 0x0002B364
		public static MethodInfo WriteAttributeStringMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.writeAttributeStringMethod == null)
				{
					JsonFormatGeneratorStatics.writeAttributeStringMethod = typeof(XmlWriterDelegator).GetMethod("WriteAttributeString", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[]
					{
						typeof(string),
						typeof(string),
						typeof(string),
						typeof(string)
					}, null);
				}
				return JsonFormatGeneratorStatics.writeAttributeStringMethod;
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000AA2 RID: 2722 RVA: 0x0002D1DA File Offset: 0x0002B3DA
		public static MethodInfo WriteEndElementMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.writeEndElementMethod == null)
				{
					JsonFormatGeneratorStatics.writeEndElementMethod = typeof(XmlWriterDelegator).GetMethod("WriteEndElement", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null);
				}
				return JsonFormatGeneratorStatics.writeEndElementMethod;
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000AA3 RID: 2723 RVA: 0x0002D211 File Offset: 0x0002B411
		public static MethodInfo WriteJsonISerializableMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.writeJsonISerializableMethod == null)
				{
					JsonFormatGeneratorStatics.writeJsonISerializableMethod = typeof(XmlObjectSerializerWriteContextComplexJson).GetMethod("WriteJsonISerializable", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return JsonFormatGeneratorStatics.writeJsonISerializableMethod;
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000AA4 RID: 2724 RVA: 0x0002D240 File Offset: 0x0002B440
		public static MethodInfo WriteJsonNameWithMappingMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.writeJsonNameWithMappingMethod == null)
				{
					JsonFormatGeneratorStatics.writeJsonNameWithMappingMethod = typeof(XmlObjectSerializerWriteContextComplexJson).GetMethod("WriteJsonNameWithMapping", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return JsonFormatGeneratorStatics.writeJsonNameWithMappingMethod;
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000AA5 RID: 2725 RVA: 0x0002D26F File Offset: 0x0002B46F
		public static MethodInfo WriteJsonValueMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.writeJsonValueMethod == null)
				{
					JsonFormatGeneratorStatics.writeJsonValueMethod = typeof(DataContractJsonSerializer).GetMethod("WriteJsonValue", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return JsonFormatGeneratorStatics.writeJsonValueMethod;
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000AA6 RID: 2726 RVA: 0x0002D2A0 File Offset: 0x0002B4A0
		public static MethodInfo WriteStartElementMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.writeStartElementMethod == null)
				{
					JsonFormatGeneratorStatics.writeStartElementMethod = typeof(XmlWriterDelegator).GetMethod("WriteStartElement", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[]
					{
						typeof(XmlDictionaryString),
						typeof(XmlDictionaryString)
					}, null);
				}
				return JsonFormatGeneratorStatics.writeStartElementMethod;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000AA7 RID: 2727 RVA: 0x0002D2FC File Offset: 0x0002B4FC
		public static MethodInfo WriteStartElementStringMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.writeStartElementStringMethod == null)
				{
					JsonFormatGeneratorStatics.writeStartElementStringMethod = typeof(XmlWriterDelegator).GetMethod("WriteStartElement", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[]
					{
						typeof(string),
						typeof(string)
					}, null);
				}
				return JsonFormatGeneratorStatics.writeStartElementStringMethod;
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000AA8 RID: 2728 RVA: 0x0002D358 File Offset: 0x0002B558
		public static MethodInfo ParseEnumMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.parseEnumMethod == null)
				{
					JsonFormatGeneratorStatics.parseEnumMethod = typeof(Enum).GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, null, new Type[]
					{
						typeof(Type),
						typeof(string)
					}, null);
				}
				return JsonFormatGeneratorStatics.parseEnumMethod;
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000AA9 RID: 2729 RVA: 0x0002D3B4 File Offset: 0x0002B5B4
		public static MethodInfo GetJsonMemberNameMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (JsonFormatGeneratorStatics.getJsonMemberNameMethod == null)
				{
					JsonFormatGeneratorStatics.getJsonMemberNameMethod = typeof(XmlObjectSerializerReadContextComplexJson).GetMethod("GetJsonMemberName", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { typeof(XmlReaderDelegator) }, null);
				}
				return JsonFormatGeneratorStatics.getJsonMemberNameMethod;
			}
		}

		// Token: 0x04000496 RID: 1174
		[SecurityCritical]
		private static MethodInfo boxPointer;

		// Token: 0x04000497 RID: 1175
		[SecurityCritical]
		private static PropertyInfo collectionItemNameProperty;

		// Token: 0x04000498 RID: 1176
		[SecurityCritical]
		private static ConstructorInfo extensionDataObjectCtor;

		// Token: 0x04000499 RID: 1177
		[SecurityCritical]
		private static PropertyInfo extensionDataProperty;

		// Token: 0x0400049A RID: 1178
		[SecurityCritical]
		private static MethodInfo getItemContractMethod;

		// Token: 0x0400049B RID: 1179
		[SecurityCritical]
		private static MethodInfo getJsonDataContractMethod;

		// Token: 0x0400049C RID: 1180
		[SecurityCritical]
		private static MethodInfo getJsonMemberIndexMethod;

		// Token: 0x0400049D RID: 1181
		[SecurityCritical]
		private static MethodInfo getRevisedItemContractMethod;

		// Token: 0x0400049E RID: 1182
		[SecurityCritical]
		private static MethodInfo getUninitializedObjectMethod;

		// Token: 0x0400049F RID: 1183
		[SecurityCritical]
		private static MethodInfo ienumeratorGetCurrentMethod;

		// Token: 0x040004A0 RID: 1184
		[SecurityCritical]
		private static MethodInfo ienumeratorMoveNextMethod;

		// Token: 0x040004A1 RID: 1185
		[SecurityCritical]
		private static MethodInfo isStartElementMethod0;

		// Token: 0x040004A2 RID: 1186
		[SecurityCritical]
		private static MethodInfo isStartElementMethod2;

		// Token: 0x040004A3 RID: 1187
		[SecurityCritical]
		private static PropertyInfo localNameProperty;

		// Token: 0x040004A4 RID: 1188
		[SecurityCritical]
		private static PropertyInfo namespaceProperty;

		// Token: 0x040004A5 RID: 1189
		[SecurityCritical]
		private static MethodInfo moveToContentMethod;

		// Token: 0x040004A6 RID: 1190
		[SecurityCritical]
		private static PropertyInfo nodeTypeProperty;

		// Token: 0x040004A7 RID: 1191
		[SecurityCritical]
		private static MethodInfo onDeserializationMethod;

		// Token: 0x040004A8 RID: 1192
		[SecurityCritical]
		private static MethodInfo readJsonValueMethod;

		// Token: 0x040004A9 RID: 1193
		[SecurityCritical]
		private static ConstructorInfo serializationExceptionCtor;

		// Token: 0x040004AA RID: 1194
		[SecurityCritical]
		private static Type[] serInfoCtorArgs;

		// Token: 0x040004AB RID: 1195
		[SecurityCritical]
		private static MethodInfo throwDuplicateMemberExceptionMethod;

		// Token: 0x040004AC RID: 1196
		[SecurityCritical]
		private static MethodInfo throwMissingRequiredMembersMethod;

		// Token: 0x040004AD RID: 1197
		[SecurityCritical]
		private static PropertyInfo typeHandleProperty;

		// Token: 0x040004AE RID: 1198
		[SecurityCritical]
		private static MethodInfo unboxPointer;

		// Token: 0x040004AF RID: 1199
		[SecurityCritical]
		private static PropertyInfo useSimpleDictionaryFormatReadProperty;

		// Token: 0x040004B0 RID: 1200
		[SecurityCritical]
		private static PropertyInfo useSimpleDictionaryFormatWriteProperty;

		// Token: 0x040004B1 RID: 1201
		[SecurityCritical]
		private static MethodInfo writeAttributeStringMethod;

		// Token: 0x040004B2 RID: 1202
		[SecurityCritical]
		private static MethodInfo writeEndElementMethod;

		// Token: 0x040004B3 RID: 1203
		[SecurityCritical]
		private static MethodInfo writeJsonISerializableMethod;

		// Token: 0x040004B4 RID: 1204
		[SecurityCritical]
		private static MethodInfo writeJsonNameWithMappingMethod;

		// Token: 0x040004B5 RID: 1205
		[SecurityCritical]
		private static MethodInfo writeJsonValueMethod;

		// Token: 0x040004B6 RID: 1206
		[SecurityCritical]
		private static MethodInfo writeStartElementMethod;

		// Token: 0x040004B7 RID: 1207
		[SecurityCritical]
		private static MethodInfo writeStartElementStringMethod;

		// Token: 0x040004B8 RID: 1208
		[SecurityCritical]
		private static MethodInfo parseEnumMethod;

		// Token: 0x040004B9 RID: 1209
		[SecurityCritical]
		private static MethodInfo getJsonMemberNameMethod;
	}
}
