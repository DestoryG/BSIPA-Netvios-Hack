using System;
using System.Collections;
using System.Reflection;
using System.Security;
using System.Xml;

namespace System.Runtime.Serialization
{
	// Token: 0x020000DD RID: 221
	internal static class XmlFormatGeneratorStatics
	{
		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000C4C RID: 3148 RVA: 0x000349F0 File Offset: 0x00032BF0
		internal static MethodInfo WriteStartElementMethod2
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.writeStartElementMethod2 == null)
				{
					XmlFormatGeneratorStatics.writeStartElementMethod2 = typeof(XmlWriterDelegator).GetMethod("WriteStartElement", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[]
					{
						typeof(XmlDictionaryString),
						typeof(XmlDictionaryString)
					}, null);
				}
				return XmlFormatGeneratorStatics.writeStartElementMethod2;
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000C4D RID: 3149 RVA: 0x00034A4C File Offset: 0x00032C4C
		internal static MethodInfo WriteStartElementMethod3
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.writeStartElementMethod3 == null)
				{
					XmlFormatGeneratorStatics.writeStartElementMethod3 = typeof(XmlWriterDelegator).GetMethod("WriteStartElement", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[]
					{
						typeof(string),
						typeof(XmlDictionaryString),
						typeof(XmlDictionaryString)
					}, null);
				}
				return XmlFormatGeneratorStatics.writeStartElementMethod3;
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000C4E RID: 3150 RVA: 0x00034AB5 File Offset: 0x00032CB5
		internal static MethodInfo WriteEndElementMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.writeEndElementMethod == null)
				{
					XmlFormatGeneratorStatics.writeEndElementMethod = typeof(XmlWriterDelegator).GetMethod("WriteEndElement", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null);
				}
				return XmlFormatGeneratorStatics.writeEndElementMethod;
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000C4F RID: 3151 RVA: 0x00034AEC File Offset: 0x00032CEC
		internal static MethodInfo WriteNamespaceDeclMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.writeNamespaceDeclMethod == null)
				{
					XmlFormatGeneratorStatics.writeNamespaceDeclMethod = typeof(XmlWriterDelegator).GetMethod("WriteNamespaceDecl", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { typeof(XmlDictionaryString) }, null);
				}
				return XmlFormatGeneratorStatics.writeNamespaceDeclMethod;
			}
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000C50 RID: 3152 RVA: 0x00034B3B File Offset: 0x00032D3B
		internal static PropertyInfo ExtensionDataProperty
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.extensionDataProperty == null)
				{
					XmlFormatGeneratorStatics.extensionDataProperty = typeof(IExtensibleDataObject).GetProperty("ExtensionData");
				}
				return XmlFormatGeneratorStatics.extensionDataProperty;
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000C51 RID: 3153 RVA: 0x00034B68 File Offset: 0x00032D68
		internal static MethodInfo BoxPointer
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.boxPointer == null)
				{
					XmlFormatGeneratorStatics.boxPointer = typeof(Pointer).GetMethod("Box");
				}
				return XmlFormatGeneratorStatics.boxPointer;
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000C52 RID: 3154 RVA: 0x00034B95 File Offset: 0x00032D95
		internal static ConstructorInfo DictionaryEnumeratorCtor
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.dictionaryEnumeratorCtor == null)
				{
					XmlFormatGeneratorStatics.dictionaryEnumeratorCtor = Globals.TypeOfDictionaryEnumerator.GetConstructor(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { Globals.TypeOfIDictionaryEnumerator }, null);
				}
				return XmlFormatGeneratorStatics.dictionaryEnumeratorCtor;
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000C53 RID: 3155 RVA: 0x00034BCA File Offset: 0x00032DCA
		internal static MethodInfo MoveNextMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.ienumeratorMoveNextMethod == null)
				{
					XmlFormatGeneratorStatics.ienumeratorMoveNextMethod = typeof(IEnumerator).GetMethod("MoveNext");
				}
				return XmlFormatGeneratorStatics.ienumeratorMoveNextMethod;
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000C54 RID: 3156 RVA: 0x00034BF7 File Offset: 0x00032DF7
		internal static MethodInfo GetCurrentMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.ienumeratorGetCurrentMethod == null)
				{
					XmlFormatGeneratorStatics.ienumeratorGetCurrentMethod = typeof(IEnumerator).GetProperty("Current").GetGetMethod();
				}
				return XmlFormatGeneratorStatics.ienumeratorGetCurrentMethod;
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000C55 RID: 3157 RVA: 0x00034C29 File Offset: 0x00032E29
		internal static MethodInfo GetItemContractMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.getItemContractMethod == null)
				{
					XmlFormatGeneratorStatics.getItemContractMethod = typeof(CollectionDataContract).GetProperty("ItemContract", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).GetGetMethod(true);
				}
				return XmlFormatGeneratorStatics.getItemContractMethod;
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000C56 RID: 3158 RVA: 0x00034C60 File Offset: 0x00032E60
		internal static MethodInfo IsStartElementMethod2
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.isStartElementMethod2 == null)
				{
					XmlFormatGeneratorStatics.isStartElementMethod2 = typeof(XmlReaderDelegator).GetMethod("IsStartElement", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[]
					{
						typeof(XmlDictionaryString),
						typeof(XmlDictionaryString)
					}, null);
				}
				return XmlFormatGeneratorStatics.isStartElementMethod2;
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000C57 RID: 3159 RVA: 0x00034CBC File Offset: 0x00032EBC
		internal static MethodInfo IsStartElementMethod0
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.isStartElementMethod0 == null)
				{
					XmlFormatGeneratorStatics.isStartElementMethod0 = typeof(XmlReaderDelegator).GetMethod("IsStartElement", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null);
				}
				return XmlFormatGeneratorStatics.isStartElementMethod0;
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000C58 RID: 3160 RVA: 0x00034CF4 File Offset: 0x00032EF4
		internal static MethodInfo GetUninitializedObjectMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.getUninitializedObjectMethod == null)
				{
					XmlFormatGeneratorStatics.getUninitializedObjectMethod = typeof(XmlFormatReaderGenerator).GetMethod("UnsafeGetUninitializedObject", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { typeof(int) }, null);
				}
				return XmlFormatGeneratorStatics.getUninitializedObjectMethod;
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000C59 RID: 3161 RVA: 0x00034D43 File Offset: 0x00032F43
		internal static MethodInfo OnDeserializationMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.onDeserializationMethod == null)
				{
					XmlFormatGeneratorStatics.onDeserializationMethod = typeof(IDeserializationCallback).GetMethod("OnDeserialization");
				}
				return XmlFormatGeneratorStatics.onDeserializationMethod;
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000C5A RID: 3162 RVA: 0x00034D70 File Offset: 0x00032F70
		internal static MethodInfo UnboxPointer
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.unboxPointer == null)
				{
					XmlFormatGeneratorStatics.unboxPointer = typeof(Pointer).GetMethod("Unbox");
				}
				return XmlFormatGeneratorStatics.unboxPointer;
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000C5B RID: 3163 RVA: 0x00034D9D File Offset: 0x00032F9D
		internal static PropertyInfo NodeTypeProperty
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.nodeTypeProperty == null)
				{
					XmlFormatGeneratorStatics.nodeTypeProperty = typeof(XmlReaderDelegator).GetProperty("NodeType", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.nodeTypeProperty;
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000C5C RID: 3164 RVA: 0x00034DCC File Offset: 0x00032FCC
		internal static ConstructorInfo SerializationExceptionCtor
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.serializationExceptionCtor == null)
				{
					XmlFormatGeneratorStatics.serializationExceptionCtor = typeof(SerializationException).GetConstructor(new Type[] { typeof(string) });
				}
				return XmlFormatGeneratorStatics.serializationExceptionCtor;
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000C5D RID: 3165 RVA: 0x00034E07 File Offset: 0x00033007
		internal static ConstructorInfo ExtensionDataObjectCtor
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.extensionDataObjectCtor == null)
				{
					XmlFormatGeneratorStatics.extensionDataObjectCtor = typeof(ExtensionDataObject).GetConstructor(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], null);
				}
				return XmlFormatGeneratorStatics.extensionDataObjectCtor;
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000C5E RID: 3166 RVA: 0x00034E39 File Offset: 0x00033039
		internal static ConstructorInfo HashtableCtor
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.hashtableCtor == null)
				{
					XmlFormatGeneratorStatics.hashtableCtor = Globals.TypeOfHashtable.GetConstructor(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, Globals.EmptyTypeArray, null);
				}
				return XmlFormatGeneratorStatics.hashtableCtor;
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000C5F RID: 3167 RVA: 0x00034E65 File Offset: 0x00033065
		internal static MethodInfo GetStreamingContextMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.getStreamingContextMethod == null)
				{
					XmlFormatGeneratorStatics.getStreamingContextMethod = typeof(XmlObjectSerializerContext).GetMethod("GetStreamingContext", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.getStreamingContextMethod;
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000C60 RID: 3168 RVA: 0x00034E94 File Offset: 0x00033094
		internal static MethodInfo GetCollectionMemberMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.getCollectionMemberMethod == null)
				{
					XmlFormatGeneratorStatics.getCollectionMemberMethod = typeof(XmlObjectSerializerReadContext).GetMethod("GetCollectionMember", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.getCollectionMemberMethod;
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000C61 RID: 3169 RVA: 0x00034EC4 File Offset: 0x000330C4
		internal static MethodInfo StoreCollectionMemberInfoMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.storeCollectionMemberInfoMethod == null)
				{
					XmlFormatGeneratorStatics.storeCollectionMemberInfoMethod = typeof(XmlObjectSerializerReadContext).GetMethod("StoreCollectionMemberInfo", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { typeof(object) }, null);
				}
				return XmlFormatGeneratorStatics.storeCollectionMemberInfoMethod;
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000C62 RID: 3170 RVA: 0x00034F13 File Offset: 0x00033113
		internal static MethodInfo StoreIsGetOnlyCollectionMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.storeIsGetOnlyCollectionMethod == null)
				{
					XmlFormatGeneratorStatics.storeIsGetOnlyCollectionMethod = typeof(XmlObjectSerializerWriteContext).GetMethod("StoreIsGetOnlyCollection", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.storeIsGetOnlyCollectionMethod;
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000C63 RID: 3171 RVA: 0x00034F42 File Offset: 0x00033142
		internal static MethodInfo ThrowNullValueReturnedForGetOnlyCollectionExceptionMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.throwNullValueReturnedForGetOnlyCollectionExceptionMethod == null)
				{
					XmlFormatGeneratorStatics.throwNullValueReturnedForGetOnlyCollectionExceptionMethod = typeof(XmlObjectSerializerReadContext).GetMethod("ThrowNullValueReturnedForGetOnlyCollectionException", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.throwNullValueReturnedForGetOnlyCollectionExceptionMethod;
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000C64 RID: 3172 RVA: 0x00034F71 File Offset: 0x00033171
		internal static MethodInfo ThrowArrayExceededSizeExceptionMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.throwArrayExceededSizeExceptionMethod == null)
				{
					XmlFormatGeneratorStatics.throwArrayExceededSizeExceptionMethod = typeof(XmlObjectSerializerReadContext).GetMethod("ThrowArrayExceededSizeException", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.throwArrayExceededSizeExceptionMethod;
			}
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000C65 RID: 3173 RVA: 0x00034FA0 File Offset: 0x000331A0
		internal static MethodInfo IncrementItemCountMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.incrementItemCountMethod == null)
				{
					XmlFormatGeneratorStatics.incrementItemCountMethod = typeof(XmlObjectSerializerContext).GetMethod("IncrementItemCount", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.incrementItemCountMethod;
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000C66 RID: 3174 RVA: 0x00034FCF File Offset: 0x000331CF
		internal static MethodInfo DemandSerializationFormatterPermissionMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.demandSerializationFormatterPermissionMethod == null)
				{
					XmlFormatGeneratorStatics.demandSerializationFormatterPermissionMethod = typeof(XmlObjectSerializerContext).GetMethod("DemandSerializationFormatterPermission", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.demandSerializationFormatterPermissionMethod;
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000C67 RID: 3175 RVA: 0x00034FFE File Offset: 0x000331FE
		internal static MethodInfo DemandMemberAccessPermissionMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.demandMemberAccessPermissionMethod == null)
				{
					XmlFormatGeneratorStatics.demandMemberAccessPermissionMethod = typeof(XmlObjectSerializerContext).GetMethod("DemandMemberAccessPermission", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.demandMemberAccessPermissionMethod;
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000C68 RID: 3176 RVA: 0x00035030 File Offset: 0x00033230
		internal static MethodInfo InternalDeserializeMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.internalDeserializeMethod == null)
				{
					XmlFormatGeneratorStatics.internalDeserializeMethod = typeof(XmlObjectSerializerReadContext).GetMethod("InternalDeserialize", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[]
					{
						typeof(XmlReaderDelegator),
						typeof(int),
						typeof(RuntimeTypeHandle),
						typeof(string),
						typeof(string)
					}, null);
				}
				return XmlFormatGeneratorStatics.internalDeserializeMethod;
			}
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000C69 RID: 3177 RVA: 0x000350B3 File Offset: 0x000332B3
		internal static MethodInfo MoveToNextElementMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.moveToNextElementMethod == null)
				{
					XmlFormatGeneratorStatics.moveToNextElementMethod = typeof(XmlObjectSerializerReadContext).GetMethod("MoveToNextElement", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.moveToNextElementMethod;
			}
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000C6A RID: 3178 RVA: 0x000350E2 File Offset: 0x000332E2
		internal static MethodInfo GetMemberIndexMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.getMemberIndexMethod == null)
				{
					XmlFormatGeneratorStatics.getMemberIndexMethod = typeof(XmlObjectSerializerReadContext).GetMethod("GetMemberIndex", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.getMemberIndexMethod;
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000C6B RID: 3179 RVA: 0x00035111 File Offset: 0x00033311
		internal static MethodInfo GetMemberIndexWithRequiredMembersMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.getMemberIndexWithRequiredMembersMethod == null)
				{
					XmlFormatGeneratorStatics.getMemberIndexWithRequiredMembersMethod = typeof(XmlObjectSerializerReadContext).GetMethod("GetMemberIndexWithRequiredMembers", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.getMemberIndexWithRequiredMembersMethod;
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000C6C RID: 3180 RVA: 0x00035140 File Offset: 0x00033340
		internal static MethodInfo ThrowRequiredMemberMissingExceptionMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.throwRequiredMemberMissingExceptionMethod == null)
				{
					XmlFormatGeneratorStatics.throwRequiredMemberMissingExceptionMethod = typeof(XmlObjectSerializerReadContext).GetMethod("ThrowRequiredMemberMissingException", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.throwRequiredMemberMissingExceptionMethod;
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000C6D RID: 3181 RVA: 0x0003516F File Offset: 0x0003336F
		internal static MethodInfo SkipUnknownElementMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.skipUnknownElementMethod == null)
				{
					XmlFormatGeneratorStatics.skipUnknownElementMethod = typeof(XmlObjectSerializerReadContext).GetMethod("SkipUnknownElement", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.skipUnknownElementMethod;
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000C6E RID: 3182 RVA: 0x000351A0 File Offset: 0x000333A0
		internal static MethodInfo ReadIfNullOrRefMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.readIfNullOrRefMethod == null)
				{
					XmlFormatGeneratorStatics.readIfNullOrRefMethod = typeof(XmlObjectSerializerReadContext).GetMethod("ReadIfNullOrRef", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[]
					{
						typeof(XmlReaderDelegator),
						typeof(Type),
						typeof(bool)
					}, null);
				}
				return XmlFormatGeneratorStatics.readIfNullOrRefMethod;
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000C6F RID: 3183 RVA: 0x00035209 File Offset: 0x00033409
		internal static MethodInfo ReadAttributesMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.readAttributesMethod == null)
				{
					XmlFormatGeneratorStatics.readAttributesMethod = typeof(XmlObjectSerializerReadContext).GetMethod("ReadAttributes", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.readAttributesMethod;
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000C70 RID: 3184 RVA: 0x00035238 File Offset: 0x00033438
		internal static MethodInfo ResetAttributesMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.resetAttributesMethod == null)
				{
					XmlFormatGeneratorStatics.resetAttributesMethod = typeof(XmlObjectSerializerReadContext).GetMethod("ResetAttributes", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.resetAttributesMethod;
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000C71 RID: 3185 RVA: 0x00035267 File Offset: 0x00033467
		internal static MethodInfo GetObjectIdMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.getObjectIdMethod == null)
				{
					XmlFormatGeneratorStatics.getObjectIdMethod = typeof(XmlObjectSerializerReadContext).GetMethod("GetObjectId", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.getObjectIdMethod;
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000C72 RID: 3186 RVA: 0x00035296 File Offset: 0x00033496
		internal static MethodInfo GetArraySizeMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.getArraySizeMethod == null)
				{
					XmlFormatGeneratorStatics.getArraySizeMethod = typeof(XmlObjectSerializerReadContext).GetMethod("GetArraySize", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.getArraySizeMethod;
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000C73 RID: 3187 RVA: 0x000352C5 File Offset: 0x000334C5
		internal static MethodInfo AddNewObjectMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.addNewObjectMethod == null)
				{
					XmlFormatGeneratorStatics.addNewObjectMethod = typeof(XmlObjectSerializerReadContext).GetMethod("AddNewObject", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.addNewObjectMethod;
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000C74 RID: 3188 RVA: 0x000352F4 File Offset: 0x000334F4
		internal static MethodInfo AddNewObjectWithIdMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.addNewObjectWithIdMethod == null)
				{
					XmlFormatGeneratorStatics.addNewObjectWithIdMethod = typeof(XmlObjectSerializerReadContext).GetMethod("AddNewObjectWithId", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.addNewObjectWithIdMethod;
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000C75 RID: 3189 RVA: 0x00035323 File Offset: 0x00033523
		internal static MethodInfo ReplaceDeserializedObjectMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.replaceDeserializedObjectMethod == null)
				{
					XmlFormatGeneratorStatics.replaceDeserializedObjectMethod = typeof(XmlObjectSerializerReadContext).GetMethod("ReplaceDeserializedObject", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.replaceDeserializedObjectMethod;
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000C76 RID: 3190 RVA: 0x00035352 File Offset: 0x00033552
		internal static MethodInfo GetExistingObjectMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.getExistingObjectMethod == null)
				{
					XmlFormatGeneratorStatics.getExistingObjectMethod = typeof(XmlObjectSerializerReadContext).GetMethod("GetExistingObject", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.getExistingObjectMethod;
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000C77 RID: 3191 RVA: 0x00035381 File Offset: 0x00033581
		internal static MethodInfo GetRealObjectMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.getRealObjectMethod == null)
				{
					XmlFormatGeneratorStatics.getRealObjectMethod = typeof(XmlObjectSerializerReadContext).GetMethod("GetRealObject", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.getRealObjectMethod;
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000C78 RID: 3192 RVA: 0x000353B0 File Offset: 0x000335B0
		internal static MethodInfo ReadMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.readMethod == null)
				{
					XmlFormatGeneratorStatics.readMethod = typeof(XmlObjectSerializerReadContext).GetMethod("Read", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.readMethod;
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000C79 RID: 3193 RVA: 0x000353DF File Offset: 0x000335DF
		internal static MethodInfo EnsureArraySizeMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.ensureArraySizeMethod == null)
				{
					XmlFormatGeneratorStatics.ensureArraySizeMethod = typeof(XmlObjectSerializerReadContext).GetMethod("EnsureArraySize", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.ensureArraySizeMethod;
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000C7A RID: 3194 RVA: 0x0003540E File Offset: 0x0003360E
		internal static MethodInfo TrimArraySizeMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.trimArraySizeMethod == null)
				{
					XmlFormatGeneratorStatics.trimArraySizeMethod = typeof(XmlObjectSerializerReadContext).GetMethod("TrimArraySize", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.trimArraySizeMethod;
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000C7B RID: 3195 RVA: 0x0003543D File Offset: 0x0003363D
		internal static MethodInfo CheckEndOfArrayMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.checkEndOfArrayMethod == null)
				{
					XmlFormatGeneratorStatics.checkEndOfArrayMethod = typeof(XmlObjectSerializerReadContext).GetMethod("CheckEndOfArray", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.checkEndOfArrayMethod;
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000C7C RID: 3196 RVA: 0x0003546C File Offset: 0x0003366C
		internal static MethodInfo GetArrayLengthMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.getArrayLengthMethod == null)
				{
					XmlFormatGeneratorStatics.getArrayLengthMethod = Globals.TypeOfArray.GetProperty("Length").GetGetMethod();
				}
				return XmlFormatGeneratorStatics.getArrayLengthMethod;
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000C7D RID: 3197 RVA: 0x00035499 File Offset: 0x00033699
		internal static MethodInfo ReadSerializationInfoMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.readSerializationInfoMethod == null)
				{
					XmlFormatGeneratorStatics.readSerializationInfoMethod = typeof(XmlObjectSerializerReadContext).GetMethod("ReadSerializationInfo", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.readSerializationInfoMethod;
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000C7E RID: 3198 RVA: 0x000354C8 File Offset: 0x000336C8
		internal static MethodInfo CreateUnexpectedStateExceptionMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.createUnexpectedStateExceptionMethod == null)
				{
					XmlFormatGeneratorStatics.createUnexpectedStateExceptionMethod = typeof(XmlObjectSerializerReadContext).GetMethod("CreateUnexpectedStateException", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[]
					{
						typeof(XmlNodeType),
						typeof(XmlReaderDelegator)
					}, null);
				}
				return XmlFormatGeneratorStatics.createUnexpectedStateExceptionMethod;
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000C7F RID: 3199 RVA: 0x00035524 File Offset: 0x00033724
		internal static MethodInfo InternalSerializeReferenceMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.internalSerializeReferenceMethod == null)
				{
					XmlFormatGeneratorStatics.internalSerializeReferenceMethod = typeof(XmlObjectSerializerWriteContext).GetMethod("InternalSerializeReference", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.internalSerializeReferenceMethod;
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000C80 RID: 3200 RVA: 0x00035553 File Offset: 0x00033753
		internal static MethodInfo InternalSerializeMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.internalSerializeMethod == null)
				{
					XmlFormatGeneratorStatics.internalSerializeMethod = typeof(XmlObjectSerializerWriteContext).GetMethod("InternalSerialize", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.internalSerializeMethod;
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000C81 RID: 3201 RVA: 0x00035584 File Offset: 0x00033784
		internal static MethodInfo WriteNullMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.writeNullMethod == null)
				{
					XmlFormatGeneratorStatics.writeNullMethod = typeof(XmlObjectSerializerWriteContext).GetMethod("WriteNull", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[]
					{
						typeof(XmlWriterDelegator),
						typeof(Type),
						typeof(bool)
					}, null);
				}
				return XmlFormatGeneratorStatics.writeNullMethod;
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000C82 RID: 3202 RVA: 0x000355ED File Offset: 0x000337ED
		internal static MethodInfo IncrementArrayCountMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.incrementArrayCountMethod == null)
				{
					XmlFormatGeneratorStatics.incrementArrayCountMethod = typeof(XmlObjectSerializerWriteContext).GetMethod("IncrementArrayCount", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.incrementArrayCountMethod;
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000C83 RID: 3203 RVA: 0x0003561C File Offset: 0x0003381C
		internal static MethodInfo IncrementCollectionCountMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.incrementCollectionCountMethod == null)
				{
					XmlFormatGeneratorStatics.incrementCollectionCountMethod = typeof(XmlObjectSerializerWriteContext).GetMethod("IncrementCollectionCount", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[]
					{
						typeof(XmlWriterDelegator),
						typeof(ICollection)
					}, null);
				}
				return XmlFormatGeneratorStatics.incrementCollectionCountMethod;
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000C84 RID: 3204 RVA: 0x00035678 File Offset: 0x00033878
		internal static MethodInfo IncrementCollectionCountGenericMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.incrementCollectionCountGenericMethod == null)
				{
					XmlFormatGeneratorStatics.incrementCollectionCountGenericMethod = typeof(XmlObjectSerializerWriteContext).GetMethod("IncrementCollectionCountGeneric", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.incrementCollectionCountGenericMethod;
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000C85 RID: 3205 RVA: 0x000356A7 File Offset: 0x000338A7
		internal static MethodInfo GetDefaultValueMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.getDefaultValueMethod == null)
				{
					XmlFormatGeneratorStatics.getDefaultValueMethod = typeof(XmlObjectSerializerWriteContext).GetMethod("GetDefaultValue", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.getDefaultValueMethod;
			}
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000C86 RID: 3206 RVA: 0x000356D6 File Offset: 0x000338D6
		internal static MethodInfo GetNullableValueMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.getNullableValueMethod == null)
				{
					XmlFormatGeneratorStatics.getNullableValueMethod = typeof(XmlObjectSerializerWriteContext).GetMethod("GetNullableValue", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.getNullableValueMethod;
			}
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000C87 RID: 3207 RVA: 0x00035705 File Offset: 0x00033905
		internal static MethodInfo ThrowRequiredMemberMustBeEmittedMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.throwRequiredMemberMustBeEmittedMethod == null)
				{
					XmlFormatGeneratorStatics.throwRequiredMemberMustBeEmittedMethod = typeof(XmlObjectSerializerWriteContext).GetMethod("ThrowRequiredMemberMustBeEmitted", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.throwRequiredMemberMustBeEmittedMethod;
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000C88 RID: 3208 RVA: 0x00035734 File Offset: 0x00033934
		internal static MethodInfo GetHasValueMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.getHasValueMethod == null)
				{
					XmlFormatGeneratorStatics.getHasValueMethod = typeof(XmlObjectSerializerWriteContext).GetMethod("GetHasValue", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.getHasValueMethod;
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000C89 RID: 3209 RVA: 0x00035763 File Offset: 0x00033963
		internal static MethodInfo WriteISerializableMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.writeISerializableMethod == null)
				{
					XmlFormatGeneratorStatics.writeISerializableMethod = typeof(XmlObjectSerializerWriteContext).GetMethod("WriteISerializable", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.writeISerializableMethod;
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000C8A RID: 3210 RVA: 0x00035792 File Offset: 0x00033992
		internal static MethodInfo WriteExtensionDataMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.writeExtensionDataMethod == null)
				{
					XmlFormatGeneratorStatics.writeExtensionDataMethod = typeof(XmlObjectSerializerWriteContext).GetMethod("WriteExtensionData", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.writeExtensionDataMethod;
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000C8B RID: 3211 RVA: 0x000357C1 File Offset: 0x000339C1
		internal static MethodInfo WriteXmlValueMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.writeXmlValueMethod == null)
				{
					XmlFormatGeneratorStatics.writeXmlValueMethod = typeof(DataContract).GetMethod("WriteXmlValue", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.writeXmlValueMethod;
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000C8C RID: 3212 RVA: 0x000357F0 File Offset: 0x000339F0
		internal static MethodInfo ReadXmlValueMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.readXmlValueMethod == null)
				{
					XmlFormatGeneratorStatics.readXmlValueMethod = typeof(DataContract).GetMethod("ReadXmlValue", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.readXmlValueMethod;
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000C8D RID: 3213 RVA: 0x0003581F File Offset: 0x00033A1F
		internal static MethodInfo ThrowTypeNotSerializableMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.throwTypeNotSerializableMethod == null)
				{
					XmlFormatGeneratorStatics.throwTypeNotSerializableMethod = typeof(DataContract).GetMethod("ThrowTypeNotSerializable", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.throwTypeNotSerializableMethod;
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000C8E RID: 3214 RVA: 0x0003584E File Offset: 0x00033A4E
		internal static PropertyInfo NamespaceProperty
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.namespaceProperty == null)
				{
					XmlFormatGeneratorStatics.namespaceProperty = typeof(DataContract).GetProperty("Namespace", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.namespaceProperty;
			}
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000C8F RID: 3215 RVA: 0x0003587D File Offset: 0x00033A7D
		internal static FieldInfo ContractNamespacesField
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.contractNamespacesField == null)
				{
					XmlFormatGeneratorStatics.contractNamespacesField = typeof(ClassDataContract).GetField("ContractNamespaces", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.contractNamespacesField;
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000C90 RID: 3216 RVA: 0x000358AC File Offset: 0x00033AAC
		internal static FieldInfo MemberNamesField
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.memberNamesField == null)
				{
					XmlFormatGeneratorStatics.memberNamesField = typeof(ClassDataContract).GetField("MemberNames", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.memberNamesField;
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000C91 RID: 3217 RVA: 0x000358DB File Offset: 0x00033ADB
		internal static MethodInfo ExtensionDataSetExplicitMethodInfo
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.extensionDataSetExplicitMethodInfo == null)
				{
					XmlFormatGeneratorStatics.extensionDataSetExplicitMethodInfo = typeof(IExtensibleDataObject).GetMethod("set_ExtensionData");
				}
				return XmlFormatGeneratorStatics.extensionDataSetExplicitMethodInfo;
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000C92 RID: 3218 RVA: 0x00035908 File Offset: 0x00033B08
		internal static PropertyInfo ChildElementNamespacesProperty
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.childElementNamespacesProperty == null)
				{
					XmlFormatGeneratorStatics.childElementNamespacesProperty = typeof(ClassDataContract).GetProperty("ChildElementNamespaces", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.childElementNamespacesProperty;
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000C93 RID: 3219 RVA: 0x00035937 File Offset: 0x00033B37
		internal static PropertyInfo CollectionItemNameProperty
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.collectionItemNameProperty == null)
				{
					XmlFormatGeneratorStatics.collectionItemNameProperty = typeof(CollectionDataContract).GetProperty("CollectionItemName", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.collectionItemNameProperty;
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000C94 RID: 3220 RVA: 0x00035966 File Offset: 0x00033B66
		internal static PropertyInfo ChildElementNamespaceProperty
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.childElementNamespaceProperty == null)
				{
					XmlFormatGeneratorStatics.childElementNamespaceProperty = typeof(CollectionDataContract).GetProperty("ChildElementNamespace", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.childElementNamespaceProperty;
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000C95 RID: 3221 RVA: 0x00035995 File Offset: 0x00033B95
		internal static MethodInfo GetDateTimeOffsetMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.getDateTimeOffsetMethod == null)
				{
					XmlFormatGeneratorStatics.getDateTimeOffsetMethod = typeof(DateTimeOffsetAdapter).GetMethod("GetDateTimeOffset", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.getDateTimeOffsetMethod;
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000C96 RID: 3222 RVA: 0x000359C4 File Offset: 0x00033BC4
		internal static MethodInfo GetDateTimeOffsetAdapterMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.getDateTimeOffsetAdapterMethod == null)
				{
					XmlFormatGeneratorStatics.getDateTimeOffsetAdapterMethod = typeof(DateTimeOffsetAdapter).GetMethod("GetDateTimeOffsetAdapter", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.getDateTimeOffsetAdapterMethod;
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000C97 RID: 3223 RVA: 0x000359F3 File Offset: 0x00033BF3
		internal static MethodInfo TraceInstructionMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.traceInstructionMethod == null)
				{
					XmlFormatGeneratorStatics.traceInstructionMethod = typeof(SerializationTrace).GetMethod("TraceInstruction", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.traceInstructionMethod;
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000C98 RID: 3224 RVA: 0x00035A24 File Offset: 0x00033C24
		internal static MethodInfo ThrowInvalidDataContractExceptionMethod
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.throwInvalidDataContractExceptionMethod == null)
				{
					XmlFormatGeneratorStatics.throwInvalidDataContractExceptionMethod = typeof(DataContract).GetMethod("ThrowInvalidDataContractException", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[]
					{
						typeof(string),
						typeof(Type)
					}, null);
				}
				return XmlFormatGeneratorStatics.throwInvalidDataContractExceptionMethod;
			}
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000C99 RID: 3225 RVA: 0x00035A80 File Offset: 0x00033C80
		internal static PropertyInfo SerializeReadOnlyTypesProperty
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.serializeReadOnlyTypesProperty == null)
				{
					XmlFormatGeneratorStatics.serializeReadOnlyTypesProperty = typeof(XmlObjectSerializerWriteContext).GetProperty("SerializeReadOnlyTypes", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.serializeReadOnlyTypesProperty;
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000C9A RID: 3226 RVA: 0x00035AAF File Offset: 0x00033CAF
		internal static PropertyInfo ClassSerializationExceptionMessageProperty
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.classSerializationExceptionMessageProperty == null)
				{
					XmlFormatGeneratorStatics.classSerializationExceptionMessageProperty = typeof(ClassDataContract).GetProperty("SerializationExceptionMessage", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.classSerializationExceptionMessageProperty;
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000C9B RID: 3227 RVA: 0x00035ADE File Offset: 0x00033CDE
		internal static PropertyInfo CollectionSerializationExceptionMessageProperty
		{
			[SecuritySafeCritical]
			get
			{
				if (XmlFormatGeneratorStatics.collectionSerializationExceptionMessageProperty == null)
				{
					XmlFormatGeneratorStatics.collectionSerializationExceptionMessageProperty = typeof(CollectionDataContract).GetProperty("SerializationExceptionMessage", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				}
				return XmlFormatGeneratorStatics.collectionSerializationExceptionMessageProperty;
			}
		}

		// Token: 0x040004FC RID: 1276
		[SecurityCritical]
		private static MethodInfo writeStartElementMethod2;

		// Token: 0x040004FD RID: 1277
		[SecurityCritical]
		private static MethodInfo writeStartElementMethod3;

		// Token: 0x040004FE RID: 1278
		[SecurityCritical]
		private static MethodInfo writeEndElementMethod;

		// Token: 0x040004FF RID: 1279
		[SecurityCritical]
		private static MethodInfo writeNamespaceDeclMethod;

		// Token: 0x04000500 RID: 1280
		[SecurityCritical]
		private static PropertyInfo extensionDataProperty;

		// Token: 0x04000501 RID: 1281
		[SecurityCritical]
		private static MethodInfo boxPointer;

		// Token: 0x04000502 RID: 1282
		[SecurityCritical]
		private static ConstructorInfo dictionaryEnumeratorCtor;

		// Token: 0x04000503 RID: 1283
		[SecurityCritical]
		private static MethodInfo ienumeratorMoveNextMethod;

		// Token: 0x04000504 RID: 1284
		[SecurityCritical]
		private static MethodInfo ienumeratorGetCurrentMethod;

		// Token: 0x04000505 RID: 1285
		[SecurityCritical]
		private static MethodInfo getItemContractMethod;

		// Token: 0x04000506 RID: 1286
		[SecurityCritical]
		private static MethodInfo isStartElementMethod2;

		// Token: 0x04000507 RID: 1287
		[SecurityCritical]
		private static MethodInfo isStartElementMethod0;

		// Token: 0x04000508 RID: 1288
		[SecurityCritical]
		private static MethodInfo getUninitializedObjectMethod;

		// Token: 0x04000509 RID: 1289
		[SecurityCritical]
		private static MethodInfo onDeserializationMethod;

		// Token: 0x0400050A RID: 1290
		[SecurityCritical]
		private static MethodInfo unboxPointer;

		// Token: 0x0400050B RID: 1291
		[SecurityCritical]
		private static PropertyInfo nodeTypeProperty;

		// Token: 0x0400050C RID: 1292
		[SecurityCritical]
		private static ConstructorInfo serializationExceptionCtor;

		// Token: 0x0400050D RID: 1293
		[SecurityCritical]
		private static ConstructorInfo extensionDataObjectCtor;

		// Token: 0x0400050E RID: 1294
		[SecurityCritical]
		private static ConstructorInfo hashtableCtor;

		// Token: 0x0400050F RID: 1295
		[SecurityCritical]
		private static MethodInfo getStreamingContextMethod;

		// Token: 0x04000510 RID: 1296
		[SecurityCritical]
		private static MethodInfo getCollectionMemberMethod;

		// Token: 0x04000511 RID: 1297
		[SecurityCritical]
		private static MethodInfo storeCollectionMemberInfoMethod;

		// Token: 0x04000512 RID: 1298
		[SecurityCritical]
		private static MethodInfo storeIsGetOnlyCollectionMethod;

		// Token: 0x04000513 RID: 1299
		[SecurityCritical]
		private static MethodInfo throwNullValueReturnedForGetOnlyCollectionExceptionMethod;

		// Token: 0x04000514 RID: 1300
		private static MethodInfo throwArrayExceededSizeExceptionMethod;

		// Token: 0x04000515 RID: 1301
		[SecurityCritical]
		private static MethodInfo incrementItemCountMethod;

		// Token: 0x04000516 RID: 1302
		[SecurityCritical]
		private static MethodInfo demandSerializationFormatterPermissionMethod;

		// Token: 0x04000517 RID: 1303
		[SecurityCritical]
		private static MethodInfo demandMemberAccessPermissionMethod;

		// Token: 0x04000518 RID: 1304
		[SecurityCritical]
		private static MethodInfo internalDeserializeMethod;

		// Token: 0x04000519 RID: 1305
		[SecurityCritical]
		private static MethodInfo moveToNextElementMethod;

		// Token: 0x0400051A RID: 1306
		[SecurityCritical]
		private static MethodInfo getMemberIndexMethod;

		// Token: 0x0400051B RID: 1307
		[SecurityCritical]
		private static MethodInfo getMemberIndexWithRequiredMembersMethod;

		// Token: 0x0400051C RID: 1308
		[SecurityCritical]
		private static MethodInfo throwRequiredMemberMissingExceptionMethod;

		// Token: 0x0400051D RID: 1309
		[SecurityCritical]
		private static MethodInfo skipUnknownElementMethod;

		// Token: 0x0400051E RID: 1310
		[SecurityCritical]
		private static MethodInfo readIfNullOrRefMethod;

		// Token: 0x0400051F RID: 1311
		[SecurityCritical]
		private static MethodInfo readAttributesMethod;

		// Token: 0x04000520 RID: 1312
		[SecurityCritical]
		private static MethodInfo resetAttributesMethod;

		// Token: 0x04000521 RID: 1313
		[SecurityCritical]
		private static MethodInfo getObjectIdMethod;

		// Token: 0x04000522 RID: 1314
		[SecurityCritical]
		private static MethodInfo getArraySizeMethod;

		// Token: 0x04000523 RID: 1315
		[SecurityCritical]
		private static MethodInfo addNewObjectMethod;

		// Token: 0x04000524 RID: 1316
		[SecurityCritical]
		private static MethodInfo addNewObjectWithIdMethod;

		// Token: 0x04000525 RID: 1317
		[SecurityCritical]
		private static MethodInfo replaceDeserializedObjectMethod;

		// Token: 0x04000526 RID: 1318
		[SecurityCritical]
		private static MethodInfo getExistingObjectMethod;

		// Token: 0x04000527 RID: 1319
		[SecurityCritical]
		private static MethodInfo getRealObjectMethod;

		// Token: 0x04000528 RID: 1320
		[SecurityCritical]
		private static MethodInfo readMethod;

		// Token: 0x04000529 RID: 1321
		[SecurityCritical]
		private static MethodInfo ensureArraySizeMethod;

		// Token: 0x0400052A RID: 1322
		[SecurityCritical]
		private static MethodInfo trimArraySizeMethod;

		// Token: 0x0400052B RID: 1323
		[SecurityCritical]
		private static MethodInfo checkEndOfArrayMethod;

		// Token: 0x0400052C RID: 1324
		[SecurityCritical]
		private static MethodInfo getArrayLengthMethod;

		// Token: 0x0400052D RID: 1325
		[SecurityCritical]
		private static MethodInfo readSerializationInfoMethod;

		// Token: 0x0400052E RID: 1326
		[SecurityCritical]
		private static MethodInfo createUnexpectedStateExceptionMethod;

		// Token: 0x0400052F RID: 1327
		[SecurityCritical]
		private static MethodInfo internalSerializeReferenceMethod;

		// Token: 0x04000530 RID: 1328
		[SecurityCritical]
		private static MethodInfo internalSerializeMethod;

		// Token: 0x04000531 RID: 1329
		[SecurityCritical]
		private static MethodInfo writeNullMethod;

		// Token: 0x04000532 RID: 1330
		[SecurityCritical]
		private static MethodInfo incrementArrayCountMethod;

		// Token: 0x04000533 RID: 1331
		[SecurityCritical]
		private static MethodInfo incrementCollectionCountMethod;

		// Token: 0x04000534 RID: 1332
		[SecurityCritical]
		private static MethodInfo incrementCollectionCountGenericMethod;

		// Token: 0x04000535 RID: 1333
		[SecurityCritical]
		private static MethodInfo getDefaultValueMethod;

		// Token: 0x04000536 RID: 1334
		[SecurityCritical]
		private static MethodInfo getNullableValueMethod;

		// Token: 0x04000537 RID: 1335
		[SecurityCritical]
		private static MethodInfo throwRequiredMemberMustBeEmittedMethod;

		// Token: 0x04000538 RID: 1336
		[SecurityCritical]
		private static MethodInfo getHasValueMethod;

		// Token: 0x04000539 RID: 1337
		[SecurityCritical]
		private static MethodInfo writeISerializableMethod;

		// Token: 0x0400053A RID: 1338
		[SecurityCritical]
		private static MethodInfo writeExtensionDataMethod;

		// Token: 0x0400053B RID: 1339
		[SecurityCritical]
		private static MethodInfo writeXmlValueMethod;

		// Token: 0x0400053C RID: 1340
		[SecurityCritical]
		private static MethodInfo readXmlValueMethod;

		// Token: 0x0400053D RID: 1341
		[SecurityCritical]
		private static MethodInfo throwTypeNotSerializableMethod;

		// Token: 0x0400053E RID: 1342
		[SecurityCritical]
		private static PropertyInfo namespaceProperty;

		// Token: 0x0400053F RID: 1343
		[SecurityCritical]
		private static FieldInfo contractNamespacesField;

		// Token: 0x04000540 RID: 1344
		[SecurityCritical]
		private static FieldInfo memberNamesField;

		// Token: 0x04000541 RID: 1345
		[SecurityCritical]
		private static MethodInfo extensionDataSetExplicitMethodInfo;

		// Token: 0x04000542 RID: 1346
		[SecurityCritical]
		private static PropertyInfo childElementNamespacesProperty;

		// Token: 0x04000543 RID: 1347
		[SecurityCritical]
		private static PropertyInfo collectionItemNameProperty;

		// Token: 0x04000544 RID: 1348
		[SecurityCritical]
		private static PropertyInfo childElementNamespaceProperty;

		// Token: 0x04000545 RID: 1349
		[SecurityCritical]
		private static MethodInfo getDateTimeOffsetMethod;

		// Token: 0x04000546 RID: 1350
		[SecurityCritical]
		private static MethodInfo getDateTimeOffsetAdapterMethod;

		// Token: 0x04000547 RID: 1351
		[SecurityCritical]
		private static MethodInfo traceInstructionMethod;

		// Token: 0x04000548 RID: 1352
		[SecurityCritical]
		private static MethodInfo throwInvalidDataContractExceptionMethod;

		// Token: 0x04000549 RID: 1353
		[SecurityCritical]
		private static PropertyInfo serializeReadOnlyTypesProperty;

		// Token: 0x0400054A RID: 1354
		[SecurityCritical]
		private static PropertyInfo classSerializationExceptionMessageProperty;

		// Token: 0x0400054B RID: 1355
		[SecurityCritical]
		private static PropertyInfo collectionSerializationExceptionMessageProperty;
	}
}
