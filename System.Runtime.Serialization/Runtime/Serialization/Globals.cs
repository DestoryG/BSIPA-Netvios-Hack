using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Security;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace System.Runtime.Serialization
{
	// Token: 0x02000090 RID: 144
	internal static class Globals
	{
		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000A06 RID: 2566 RVA: 0x0002BE20 File Offset: 0x0002A020
		internal static XmlQualifiedName IdQualifiedName
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.idQualifiedName == null)
				{
					Globals.idQualifiedName = new XmlQualifiedName("Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				}
				return Globals.idQualifiedName;
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06000A07 RID: 2567 RVA: 0x0002BE48 File Offset: 0x0002A048
		internal static XmlQualifiedName RefQualifiedName
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.refQualifiedName == null)
				{
					Globals.refQualifiedName = new XmlQualifiedName("Ref", "http://schemas.microsoft.com/2003/10/Serialization/");
				}
				return Globals.refQualifiedName;
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06000A08 RID: 2568 RVA: 0x0002BE70 File Offset: 0x0002A070
		internal static Type TypeOfObject
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfObject == null)
				{
					Globals.typeOfObject = typeof(object);
				}
				return Globals.typeOfObject;
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06000A09 RID: 2569 RVA: 0x0002BE93 File Offset: 0x0002A093
		internal static Type TypeOfValueType
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfValueType == null)
				{
					Globals.typeOfValueType = typeof(ValueType);
				}
				return Globals.typeOfValueType;
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000A0A RID: 2570 RVA: 0x0002BEB6 File Offset: 0x0002A0B6
		internal static Type TypeOfArray
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfArray == null)
				{
					Globals.typeOfArray = typeof(Array);
				}
				return Globals.typeOfArray;
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000A0B RID: 2571 RVA: 0x0002BED9 File Offset: 0x0002A0D9
		internal static Type TypeOfString
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfString == null)
				{
					Globals.typeOfString = typeof(string);
				}
				return Globals.typeOfString;
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000A0C RID: 2572 RVA: 0x0002BEFC File Offset: 0x0002A0FC
		internal static Type TypeOfInt
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfInt == null)
				{
					Globals.typeOfInt = typeof(int);
				}
				return Globals.typeOfInt;
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000A0D RID: 2573 RVA: 0x0002BF1F File Offset: 0x0002A11F
		internal static Type TypeOfULong
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfULong == null)
				{
					Globals.typeOfULong = typeof(ulong);
				}
				return Globals.typeOfULong;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000A0E RID: 2574 RVA: 0x0002BF42 File Offset: 0x0002A142
		internal static Type TypeOfVoid
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfVoid == null)
				{
					Globals.typeOfVoid = typeof(void);
				}
				return Globals.typeOfVoid;
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000A0F RID: 2575 RVA: 0x0002BF65 File Offset: 0x0002A165
		internal static Type TypeOfByteArray
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfByteArray == null)
				{
					Globals.typeOfByteArray = typeof(byte[]);
				}
				return Globals.typeOfByteArray;
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000A10 RID: 2576 RVA: 0x0002BF88 File Offset: 0x0002A188
		internal static Type TypeOfTimeSpan
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfTimeSpan == null)
				{
					Globals.typeOfTimeSpan = typeof(TimeSpan);
				}
				return Globals.typeOfTimeSpan;
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000A11 RID: 2577 RVA: 0x0002BFAB File Offset: 0x0002A1AB
		internal static Type TypeOfGuid
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfGuid == null)
				{
					Globals.typeOfGuid = typeof(Guid);
				}
				return Globals.typeOfGuid;
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000A12 RID: 2578 RVA: 0x0002BFCE File Offset: 0x0002A1CE
		internal static Type TypeOfDateTimeOffset
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfDateTimeOffset == null)
				{
					Globals.typeOfDateTimeOffset = typeof(DateTimeOffset);
				}
				return Globals.typeOfDateTimeOffset;
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000A13 RID: 2579 RVA: 0x0002BFF1 File Offset: 0x0002A1F1
		internal static Type TypeOfDateTimeOffsetAdapter
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfDateTimeOffsetAdapter == null)
				{
					Globals.typeOfDateTimeOffsetAdapter = typeof(DateTimeOffsetAdapter);
				}
				return Globals.typeOfDateTimeOffsetAdapter;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000A14 RID: 2580 RVA: 0x0002C014 File Offset: 0x0002A214
		internal static Type TypeOfUri
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfUri == null)
				{
					Globals.typeOfUri = typeof(Uri);
				}
				return Globals.typeOfUri;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000A15 RID: 2581 RVA: 0x0002C037 File Offset: 0x0002A237
		internal static Type TypeOfTypeEnumerable
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfTypeEnumerable == null)
				{
					Globals.typeOfTypeEnumerable = typeof(IEnumerable<Type>);
				}
				return Globals.typeOfTypeEnumerable;
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000A16 RID: 2582 RVA: 0x0002C05A File Offset: 0x0002A25A
		internal static Type TypeOfStreamingContext
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfStreamingContext == null)
				{
					Globals.typeOfStreamingContext = typeof(StreamingContext);
				}
				return Globals.typeOfStreamingContext;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000A17 RID: 2583 RVA: 0x0002C07D File Offset: 0x0002A27D
		internal static Type TypeOfISerializable
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfISerializable == null)
				{
					Globals.typeOfISerializable = typeof(ISerializable);
				}
				return Globals.typeOfISerializable;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000A18 RID: 2584 RVA: 0x0002C0A0 File Offset: 0x0002A2A0
		internal static Type TypeOfIDeserializationCallback
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfIDeserializationCallback == null)
				{
					Globals.typeOfIDeserializationCallback = typeof(IDeserializationCallback);
				}
				return Globals.typeOfIDeserializationCallback;
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000A19 RID: 2585 RVA: 0x0002C0C3 File Offset: 0x0002A2C3
		internal static Type TypeOfIObjectReference
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfIObjectReference == null)
				{
					Globals.typeOfIObjectReference = typeof(IObjectReference);
				}
				return Globals.typeOfIObjectReference;
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000A1A RID: 2586 RVA: 0x0002C0E6 File Offset: 0x0002A2E6
		internal static Type TypeOfXmlFormatClassWriterDelegate
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfXmlFormatClassWriterDelegate == null)
				{
					Globals.typeOfXmlFormatClassWriterDelegate = typeof(XmlFormatClassWriterDelegate);
				}
				return Globals.typeOfXmlFormatClassWriterDelegate;
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000A1B RID: 2587 RVA: 0x0002C109 File Offset: 0x0002A309
		internal static Type TypeOfXmlFormatCollectionWriterDelegate
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfXmlFormatCollectionWriterDelegate == null)
				{
					Globals.typeOfXmlFormatCollectionWriterDelegate = typeof(XmlFormatCollectionWriterDelegate);
				}
				return Globals.typeOfXmlFormatCollectionWriterDelegate;
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000A1C RID: 2588 RVA: 0x0002C12C File Offset: 0x0002A32C
		internal static Type TypeOfXmlFormatClassReaderDelegate
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfXmlFormatClassReaderDelegate == null)
				{
					Globals.typeOfXmlFormatClassReaderDelegate = typeof(XmlFormatClassReaderDelegate);
				}
				return Globals.typeOfXmlFormatClassReaderDelegate;
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000A1D RID: 2589 RVA: 0x0002C14F File Offset: 0x0002A34F
		internal static Type TypeOfXmlFormatCollectionReaderDelegate
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfXmlFormatCollectionReaderDelegate == null)
				{
					Globals.typeOfXmlFormatCollectionReaderDelegate = typeof(XmlFormatCollectionReaderDelegate);
				}
				return Globals.typeOfXmlFormatCollectionReaderDelegate;
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000A1E RID: 2590 RVA: 0x0002C172 File Offset: 0x0002A372
		internal static Type TypeOfXmlFormatGetOnlyCollectionReaderDelegate
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfXmlFormatGetOnlyCollectionReaderDelegate == null)
				{
					Globals.typeOfXmlFormatGetOnlyCollectionReaderDelegate = typeof(XmlFormatGetOnlyCollectionReaderDelegate);
				}
				return Globals.typeOfXmlFormatGetOnlyCollectionReaderDelegate;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000A1F RID: 2591 RVA: 0x0002C195 File Offset: 0x0002A395
		internal static Type TypeOfKnownTypeAttribute
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfKnownTypeAttribute == null)
				{
					Globals.typeOfKnownTypeAttribute = typeof(KnownTypeAttribute);
				}
				return Globals.typeOfKnownTypeAttribute;
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000A20 RID: 2592 RVA: 0x0002C1B8 File Offset: 0x0002A3B8
		internal static Type TypeOfDataContractAttribute
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfDataContractAttribute == null)
				{
					Globals.typeOfDataContractAttribute = typeof(DataContractAttribute);
				}
				return Globals.typeOfDataContractAttribute;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000A21 RID: 2593 RVA: 0x0002C1DB File Offset: 0x0002A3DB
		internal static Type TypeOfContractNamespaceAttribute
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfContractNamespaceAttribute == null)
				{
					Globals.typeOfContractNamespaceAttribute = typeof(ContractNamespaceAttribute);
				}
				return Globals.typeOfContractNamespaceAttribute;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000A22 RID: 2594 RVA: 0x0002C1FE File Offset: 0x0002A3FE
		internal static Type TypeOfDataMemberAttribute
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfDataMemberAttribute == null)
				{
					Globals.typeOfDataMemberAttribute = typeof(DataMemberAttribute);
				}
				return Globals.typeOfDataMemberAttribute;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000A23 RID: 2595 RVA: 0x0002C221 File Offset: 0x0002A421
		internal static Type TypeOfEnumMemberAttribute
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfEnumMemberAttribute == null)
				{
					Globals.typeOfEnumMemberAttribute = typeof(EnumMemberAttribute);
				}
				return Globals.typeOfEnumMemberAttribute;
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000A24 RID: 2596 RVA: 0x0002C244 File Offset: 0x0002A444
		internal static Type TypeOfCollectionDataContractAttribute
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfCollectionDataContractAttribute == null)
				{
					Globals.typeOfCollectionDataContractAttribute = typeof(CollectionDataContractAttribute);
				}
				return Globals.typeOfCollectionDataContractAttribute;
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000A25 RID: 2597 RVA: 0x0002C267 File Offset: 0x0002A467
		internal static Type TypeOfOptionalFieldAttribute
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfOptionalFieldAttribute == null)
				{
					Globals.typeOfOptionalFieldAttribute = typeof(OptionalFieldAttribute);
				}
				return Globals.typeOfOptionalFieldAttribute;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000A26 RID: 2598 RVA: 0x0002C28A File Offset: 0x0002A48A
		internal static Type TypeOfObjectArray
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfObjectArray == null)
				{
					Globals.typeOfObjectArray = typeof(object[]);
				}
				return Globals.typeOfObjectArray;
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000A27 RID: 2599 RVA: 0x0002C2AD File Offset: 0x0002A4AD
		internal static Type TypeOfOnSerializingAttribute
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfOnSerializingAttribute == null)
				{
					Globals.typeOfOnSerializingAttribute = typeof(OnSerializingAttribute);
				}
				return Globals.typeOfOnSerializingAttribute;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000A28 RID: 2600 RVA: 0x0002C2D0 File Offset: 0x0002A4D0
		internal static Type TypeOfOnSerializedAttribute
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfOnSerializedAttribute == null)
				{
					Globals.typeOfOnSerializedAttribute = typeof(OnSerializedAttribute);
				}
				return Globals.typeOfOnSerializedAttribute;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000A29 RID: 2601 RVA: 0x0002C2F3 File Offset: 0x0002A4F3
		internal static Type TypeOfOnDeserializingAttribute
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfOnDeserializingAttribute == null)
				{
					Globals.typeOfOnDeserializingAttribute = typeof(OnDeserializingAttribute);
				}
				return Globals.typeOfOnDeserializingAttribute;
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000A2A RID: 2602 RVA: 0x0002C316 File Offset: 0x0002A516
		internal static Type TypeOfOnDeserializedAttribute
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfOnDeserializedAttribute == null)
				{
					Globals.typeOfOnDeserializedAttribute = typeof(OnDeserializedAttribute);
				}
				return Globals.typeOfOnDeserializedAttribute;
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000A2B RID: 2603 RVA: 0x0002C339 File Offset: 0x0002A539
		internal static Type TypeOfFlagsAttribute
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfFlagsAttribute == null)
				{
					Globals.typeOfFlagsAttribute = typeof(FlagsAttribute);
				}
				return Globals.typeOfFlagsAttribute;
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000A2C RID: 2604 RVA: 0x0002C35C File Offset: 0x0002A55C
		internal static Type TypeOfSerializableAttribute
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfSerializableAttribute == null)
				{
					Globals.typeOfSerializableAttribute = typeof(SerializableAttribute);
				}
				return Globals.typeOfSerializableAttribute;
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000A2D RID: 2605 RVA: 0x0002C37F File Offset: 0x0002A57F
		internal static Type TypeOfNonSerializedAttribute
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfNonSerializedAttribute == null)
				{
					Globals.typeOfNonSerializedAttribute = typeof(NonSerializedAttribute);
				}
				return Globals.typeOfNonSerializedAttribute;
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000A2E RID: 2606 RVA: 0x0002C3A2 File Offset: 0x0002A5A2
		internal static Type TypeOfSerializationInfo
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfSerializationInfo == null)
				{
					Globals.typeOfSerializationInfo = typeof(SerializationInfo);
				}
				return Globals.typeOfSerializationInfo;
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000A2F RID: 2607 RVA: 0x0002C3C5 File Offset: 0x0002A5C5
		internal static Type TypeOfSerializationInfoEnumerator
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfSerializationInfoEnumerator == null)
				{
					Globals.typeOfSerializationInfoEnumerator = typeof(SerializationInfoEnumerator);
				}
				return Globals.typeOfSerializationInfoEnumerator;
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000A30 RID: 2608 RVA: 0x0002C3E8 File Offset: 0x0002A5E8
		internal static Type TypeOfSerializationEntry
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfSerializationEntry == null)
				{
					Globals.typeOfSerializationEntry = typeof(SerializationEntry);
				}
				return Globals.typeOfSerializationEntry;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000A31 RID: 2609 RVA: 0x0002C40B File Offset: 0x0002A60B
		internal static Type TypeOfIXmlSerializable
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfIXmlSerializable == null)
				{
					Globals.typeOfIXmlSerializable = typeof(IXmlSerializable);
				}
				return Globals.typeOfIXmlSerializable;
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000A32 RID: 2610 RVA: 0x0002C42E File Offset: 0x0002A62E
		internal static Type TypeOfXmlSchemaProviderAttribute
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfXmlSchemaProviderAttribute == null)
				{
					Globals.typeOfXmlSchemaProviderAttribute = typeof(XmlSchemaProviderAttribute);
				}
				return Globals.typeOfXmlSchemaProviderAttribute;
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000A33 RID: 2611 RVA: 0x0002C451 File Offset: 0x0002A651
		internal static Type TypeOfXmlRootAttribute
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfXmlRootAttribute == null)
				{
					Globals.typeOfXmlRootAttribute = typeof(XmlRootAttribute);
				}
				return Globals.typeOfXmlRootAttribute;
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000A34 RID: 2612 RVA: 0x0002C474 File Offset: 0x0002A674
		internal static Type TypeOfXmlQualifiedName
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfXmlQualifiedName == null)
				{
					Globals.typeOfXmlQualifiedName = typeof(XmlQualifiedName);
				}
				return Globals.typeOfXmlQualifiedName;
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000A35 RID: 2613 RVA: 0x0002C497 File Offset: 0x0002A697
		internal static Type TypeOfXmlSchemaType
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfXmlSchemaType == null)
				{
					Globals.typeOfXmlSchemaType = typeof(XmlSchemaType);
				}
				return Globals.typeOfXmlSchemaType;
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000A36 RID: 2614 RVA: 0x0002C4BA File Offset: 0x0002A6BA
		internal static Type TypeOfXmlSerializableServices
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfXmlSerializableServices == null)
				{
					Globals.typeOfXmlSerializableServices = typeof(XmlSerializableServices);
				}
				return Globals.typeOfXmlSerializableServices;
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000A37 RID: 2615 RVA: 0x0002C4DD File Offset: 0x0002A6DD
		internal static Type TypeOfXmlNodeArray
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfXmlNodeArray == null)
				{
					Globals.typeOfXmlNodeArray = typeof(XmlNode[]);
				}
				return Globals.typeOfXmlNodeArray;
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000A38 RID: 2616 RVA: 0x0002C500 File Offset: 0x0002A700
		internal static Type TypeOfXmlSchemaSet
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfXmlSchemaSet == null)
				{
					Globals.typeOfXmlSchemaSet = typeof(XmlSchemaSet);
				}
				return Globals.typeOfXmlSchemaSet;
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000A39 RID: 2617 RVA: 0x0002C523 File Offset: 0x0002A723
		internal static object[] EmptyObjectArray
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.emptyObjectArray == null)
				{
					Globals.emptyObjectArray = new object[0];
				}
				return Globals.emptyObjectArray;
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000A3A RID: 2618 RVA: 0x0002C53C File Offset: 0x0002A73C
		internal static Type[] EmptyTypeArray
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.emptyTypeArray == null)
				{
					Globals.emptyTypeArray = new Type[0];
				}
				return Globals.emptyTypeArray;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000A3B RID: 2619 RVA: 0x0002C555 File Offset: 0x0002A755
		internal static Type TypeOfIPropertyChange
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfIPropertyChange == null)
				{
					Globals.typeOfIPropertyChange = typeof(INotifyPropertyChanged);
				}
				return Globals.typeOfIPropertyChange;
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000A3C RID: 2620 RVA: 0x0002C578 File Offset: 0x0002A778
		internal static Type TypeOfIExtensibleDataObject
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfIExtensibleDataObject == null)
				{
					Globals.typeOfIExtensibleDataObject = typeof(IExtensibleDataObject);
				}
				return Globals.typeOfIExtensibleDataObject;
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000A3D RID: 2621 RVA: 0x0002C59B File Offset: 0x0002A79B
		internal static Type TypeOfExtensionDataObject
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfExtensionDataObject == null)
				{
					Globals.typeOfExtensionDataObject = typeof(ExtensionDataObject);
				}
				return Globals.typeOfExtensionDataObject;
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000A3E RID: 2622 RVA: 0x0002C5BE File Offset: 0x0002A7BE
		internal static Type TypeOfISerializableDataNode
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfISerializableDataNode == null)
				{
					Globals.typeOfISerializableDataNode = typeof(ISerializableDataNode);
				}
				return Globals.typeOfISerializableDataNode;
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000A3F RID: 2623 RVA: 0x0002C5E1 File Offset: 0x0002A7E1
		internal static Type TypeOfClassDataNode
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfClassDataNode == null)
				{
					Globals.typeOfClassDataNode = typeof(ClassDataNode);
				}
				return Globals.typeOfClassDataNode;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000A40 RID: 2624 RVA: 0x0002C604 File Offset: 0x0002A804
		internal static Type TypeOfCollectionDataNode
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfCollectionDataNode == null)
				{
					Globals.typeOfCollectionDataNode = typeof(CollectionDataNode);
				}
				return Globals.typeOfCollectionDataNode;
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000A41 RID: 2625 RVA: 0x0002C627 File Offset: 0x0002A827
		internal static Type TypeOfXmlDataNode
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfXmlDataNode == null)
				{
					Globals.typeOfXmlDataNode = typeof(XmlDataNode);
				}
				return Globals.typeOfXmlDataNode;
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000A42 RID: 2626 RVA: 0x0002C64A File Offset: 0x0002A84A
		internal static Type TypeOfNullable
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfNullable == null)
				{
					Globals.typeOfNullable = typeof(Nullable<>);
				}
				return Globals.typeOfNullable;
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000A43 RID: 2627 RVA: 0x0002C66D File Offset: 0x0002A86D
		internal static Type TypeOfReflectionPointer
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfReflectionPointer == null)
				{
					Globals.typeOfReflectionPointer = typeof(Pointer);
				}
				return Globals.typeOfReflectionPointer;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000A44 RID: 2628 RVA: 0x0002C690 File Offset: 0x0002A890
		internal static Type TypeOfIDictionaryGeneric
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfIDictionaryGeneric == null)
				{
					Globals.typeOfIDictionaryGeneric = typeof(IDictionary<, >);
				}
				return Globals.typeOfIDictionaryGeneric;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000A45 RID: 2629 RVA: 0x0002C6B3 File Offset: 0x0002A8B3
		internal static Type TypeOfIDictionary
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfIDictionary == null)
				{
					Globals.typeOfIDictionary = typeof(IDictionary);
				}
				return Globals.typeOfIDictionary;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000A46 RID: 2630 RVA: 0x0002C6D6 File Offset: 0x0002A8D6
		internal static Type TypeOfIListGeneric
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfIListGeneric == null)
				{
					Globals.typeOfIListGeneric = typeof(IList<>);
				}
				return Globals.typeOfIListGeneric;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000A47 RID: 2631 RVA: 0x0002C6F9 File Offset: 0x0002A8F9
		internal static Type TypeOfIList
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfIList == null)
				{
					Globals.typeOfIList = typeof(IList);
				}
				return Globals.typeOfIList;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000A48 RID: 2632 RVA: 0x0002C71C File Offset: 0x0002A91C
		internal static Type TypeOfICollectionGeneric
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfICollectionGeneric == null)
				{
					Globals.typeOfICollectionGeneric = typeof(ICollection<>);
				}
				return Globals.typeOfICollectionGeneric;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000A49 RID: 2633 RVA: 0x0002C73F File Offset: 0x0002A93F
		internal static Type TypeOfICollection
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfICollection == null)
				{
					Globals.typeOfICollection = typeof(ICollection);
				}
				return Globals.typeOfICollection;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000A4A RID: 2634 RVA: 0x0002C762 File Offset: 0x0002A962
		internal static Type TypeOfIEnumerableGeneric
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfIEnumerableGeneric == null)
				{
					Globals.typeOfIEnumerableGeneric = typeof(IEnumerable<>);
				}
				return Globals.typeOfIEnumerableGeneric;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000A4B RID: 2635 RVA: 0x0002C785 File Offset: 0x0002A985
		internal static Type TypeOfIEnumerable
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfIEnumerable == null)
				{
					Globals.typeOfIEnumerable = typeof(IEnumerable);
				}
				return Globals.typeOfIEnumerable;
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000A4C RID: 2636 RVA: 0x0002C7A8 File Offset: 0x0002A9A8
		internal static Type TypeOfIEnumeratorGeneric
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfIEnumeratorGeneric == null)
				{
					Globals.typeOfIEnumeratorGeneric = typeof(IEnumerator<>);
				}
				return Globals.typeOfIEnumeratorGeneric;
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000A4D RID: 2637 RVA: 0x0002C7CB File Offset: 0x0002A9CB
		internal static Type TypeOfIEnumerator
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfIEnumerator == null)
				{
					Globals.typeOfIEnumerator = typeof(IEnumerator);
				}
				return Globals.typeOfIEnumerator;
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000A4E RID: 2638 RVA: 0x0002C7EE File Offset: 0x0002A9EE
		internal static Type TypeOfKeyValuePair
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfKeyValuePair == null)
				{
					Globals.typeOfKeyValuePair = typeof(KeyValuePair<, >);
				}
				return Globals.typeOfKeyValuePair;
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000A4F RID: 2639 RVA: 0x0002C811 File Offset: 0x0002AA11
		internal static Type TypeOfKeyValue
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfKeyValue == null)
				{
					Globals.typeOfKeyValue = typeof(KeyValue<, >);
				}
				return Globals.typeOfKeyValue;
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000A50 RID: 2640 RVA: 0x0002C834 File Offset: 0x0002AA34
		internal static Type TypeOfIDictionaryEnumerator
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfIDictionaryEnumerator == null)
				{
					Globals.typeOfIDictionaryEnumerator = typeof(IDictionaryEnumerator);
				}
				return Globals.typeOfIDictionaryEnumerator;
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000A51 RID: 2641 RVA: 0x0002C857 File Offset: 0x0002AA57
		internal static Type TypeOfDictionaryEnumerator
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfDictionaryEnumerator == null)
				{
					Globals.typeOfDictionaryEnumerator = typeof(CollectionDataContract.DictionaryEnumerator);
				}
				return Globals.typeOfDictionaryEnumerator;
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000A52 RID: 2642 RVA: 0x0002C87A File Offset: 0x0002AA7A
		internal static Type TypeOfGenericDictionaryEnumerator
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfGenericDictionaryEnumerator == null)
				{
					Globals.typeOfGenericDictionaryEnumerator = typeof(CollectionDataContract.GenericDictionaryEnumerator<, >);
				}
				return Globals.typeOfGenericDictionaryEnumerator;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000A53 RID: 2643 RVA: 0x0002C89D File Offset: 0x0002AA9D
		internal static Type TypeOfDictionaryGeneric
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfDictionaryGeneric == null)
				{
					Globals.typeOfDictionaryGeneric = typeof(Dictionary<, >);
				}
				return Globals.typeOfDictionaryGeneric;
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000A54 RID: 2644 RVA: 0x0002C8C0 File Offset: 0x0002AAC0
		internal static Type TypeOfHashtable
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfHashtable == null)
				{
					Globals.typeOfHashtable = typeof(Hashtable);
				}
				return Globals.typeOfHashtable;
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000A55 RID: 2645 RVA: 0x0002C8E3 File Offset: 0x0002AAE3
		internal static Type TypeOfListGeneric
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfListGeneric == null)
				{
					Globals.typeOfListGeneric = typeof(List<>);
				}
				return Globals.typeOfListGeneric;
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000A56 RID: 2646 RVA: 0x0002C906 File Offset: 0x0002AB06
		internal static Type TypeOfXmlElement
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfXmlElement == null)
				{
					Globals.typeOfXmlElement = typeof(XmlElement);
				}
				return Globals.typeOfXmlElement;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000A57 RID: 2647 RVA: 0x0002C929 File Offset: 0x0002AB29
		internal static Type TypeOfDBNull
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.typeOfDBNull == null)
				{
					Globals.typeOfDBNull = typeof(DBNull);
				}
				return Globals.typeOfDBNull;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000A58 RID: 2648 RVA: 0x0002C94C File Offset: 0x0002AB4C
		internal static Uri DataContractXsdBaseNamespaceUri
		{
			[SecuritySafeCritical]
			get
			{
				if (Globals.dataContractXsdBaseNamespaceUri == null)
				{
					Globals.dataContractXsdBaseNamespaceUri = new Uri("http://schemas.datacontract.org/2004/07/");
				}
				return Globals.dataContractXsdBaseNamespaceUri;
			}
		}

		// Token: 0x040003D3 RID: 979
		internal const BindingFlags ScanAllMembers = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

		// Token: 0x040003D4 RID: 980
		[SecurityCritical]
		private static XmlQualifiedName idQualifiedName;

		// Token: 0x040003D5 RID: 981
		[SecurityCritical]
		private static XmlQualifiedName refQualifiedName;

		// Token: 0x040003D6 RID: 982
		[SecurityCritical]
		private static Type typeOfObject;

		// Token: 0x040003D7 RID: 983
		[SecurityCritical]
		private static Type typeOfValueType;

		// Token: 0x040003D8 RID: 984
		[SecurityCritical]
		private static Type typeOfArray;

		// Token: 0x040003D9 RID: 985
		[SecurityCritical]
		private static Type typeOfString;

		// Token: 0x040003DA RID: 986
		[SecurityCritical]
		private static Type typeOfInt;

		// Token: 0x040003DB RID: 987
		[SecurityCritical]
		private static Type typeOfULong;

		// Token: 0x040003DC RID: 988
		[SecurityCritical]
		private static Type typeOfVoid;

		// Token: 0x040003DD RID: 989
		[SecurityCritical]
		private static Type typeOfByteArray;

		// Token: 0x040003DE RID: 990
		[SecurityCritical]
		private static Type typeOfTimeSpan;

		// Token: 0x040003DF RID: 991
		[SecurityCritical]
		private static Type typeOfGuid;

		// Token: 0x040003E0 RID: 992
		[SecurityCritical]
		private static Type typeOfDateTimeOffset;

		// Token: 0x040003E1 RID: 993
		[SecurityCritical]
		private static Type typeOfDateTimeOffsetAdapter;

		// Token: 0x040003E2 RID: 994
		[SecurityCritical]
		private static Type typeOfUri;

		// Token: 0x040003E3 RID: 995
		[SecurityCritical]
		private static Type typeOfTypeEnumerable;

		// Token: 0x040003E4 RID: 996
		[SecurityCritical]
		private static Type typeOfStreamingContext;

		// Token: 0x040003E5 RID: 997
		[SecurityCritical]
		private static Type typeOfISerializable;

		// Token: 0x040003E6 RID: 998
		[SecurityCritical]
		private static Type typeOfIDeserializationCallback;

		// Token: 0x040003E7 RID: 999
		[SecurityCritical]
		private static Type typeOfIObjectReference;

		// Token: 0x040003E8 RID: 1000
		[SecurityCritical]
		private static Type typeOfXmlFormatClassWriterDelegate;

		// Token: 0x040003E9 RID: 1001
		[SecurityCritical]
		private static Type typeOfXmlFormatCollectionWriterDelegate;

		// Token: 0x040003EA RID: 1002
		[SecurityCritical]
		private static Type typeOfXmlFormatClassReaderDelegate;

		// Token: 0x040003EB RID: 1003
		[SecurityCritical]
		private static Type typeOfXmlFormatCollectionReaderDelegate;

		// Token: 0x040003EC RID: 1004
		[SecurityCritical]
		private static Type typeOfXmlFormatGetOnlyCollectionReaderDelegate;

		// Token: 0x040003ED RID: 1005
		[SecurityCritical]
		private static Type typeOfKnownTypeAttribute;

		// Token: 0x040003EE RID: 1006
		[SecurityCritical]
		private static Type typeOfDataContractAttribute;

		// Token: 0x040003EF RID: 1007
		[SecurityCritical]
		private static Type typeOfContractNamespaceAttribute;

		// Token: 0x040003F0 RID: 1008
		[SecurityCritical]
		private static Type typeOfDataMemberAttribute;

		// Token: 0x040003F1 RID: 1009
		[SecurityCritical]
		private static Type typeOfEnumMemberAttribute;

		// Token: 0x040003F2 RID: 1010
		[SecurityCritical]
		private static Type typeOfCollectionDataContractAttribute;

		// Token: 0x040003F3 RID: 1011
		[SecurityCritical]
		private static Type typeOfOptionalFieldAttribute;

		// Token: 0x040003F4 RID: 1012
		[SecurityCritical]
		private static Type typeOfObjectArray;

		// Token: 0x040003F5 RID: 1013
		[SecurityCritical]
		private static Type typeOfOnSerializingAttribute;

		// Token: 0x040003F6 RID: 1014
		[SecurityCritical]
		private static Type typeOfOnSerializedAttribute;

		// Token: 0x040003F7 RID: 1015
		[SecurityCritical]
		private static Type typeOfOnDeserializingAttribute;

		// Token: 0x040003F8 RID: 1016
		[SecurityCritical]
		private static Type typeOfOnDeserializedAttribute;

		// Token: 0x040003F9 RID: 1017
		[SecurityCritical]
		private static Type typeOfFlagsAttribute;

		// Token: 0x040003FA RID: 1018
		[SecurityCritical]
		private static Type typeOfSerializableAttribute;

		// Token: 0x040003FB RID: 1019
		[SecurityCritical]
		private static Type typeOfNonSerializedAttribute;

		// Token: 0x040003FC RID: 1020
		[SecurityCritical]
		private static Type typeOfSerializationInfo;

		// Token: 0x040003FD RID: 1021
		[SecurityCritical]
		private static Type typeOfSerializationInfoEnumerator;

		// Token: 0x040003FE RID: 1022
		[SecurityCritical]
		private static Type typeOfSerializationEntry;

		// Token: 0x040003FF RID: 1023
		[SecurityCritical]
		private static Type typeOfIXmlSerializable;

		// Token: 0x04000400 RID: 1024
		[SecurityCritical]
		private static Type typeOfXmlSchemaProviderAttribute;

		// Token: 0x04000401 RID: 1025
		[SecurityCritical]
		private static Type typeOfXmlRootAttribute;

		// Token: 0x04000402 RID: 1026
		[SecurityCritical]
		private static Type typeOfXmlQualifiedName;

		// Token: 0x04000403 RID: 1027
		[SecurityCritical]
		private static Type typeOfXmlSchemaType;

		// Token: 0x04000404 RID: 1028
		[SecurityCritical]
		private static Type typeOfXmlSerializableServices;

		// Token: 0x04000405 RID: 1029
		[SecurityCritical]
		private static Type typeOfXmlNodeArray;

		// Token: 0x04000406 RID: 1030
		[SecurityCritical]
		private static Type typeOfXmlSchemaSet;

		// Token: 0x04000407 RID: 1031
		[SecurityCritical]
		private static object[] emptyObjectArray;

		// Token: 0x04000408 RID: 1032
		[SecurityCritical]
		private static Type[] emptyTypeArray;

		// Token: 0x04000409 RID: 1033
		[SecurityCritical]
		private static Type typeOfIPropertyChange;

		// Token: 0x0400040A RID: 1034
		[SecurityCritical]
		private static Type typeOfIExtensibleDataObject;

		// Token: 0x0400040B RID: 1035
		[SecurityCritical]
		private static Type typeOfExtensionDataObject;

		// Token: 0x0400040C RID: 1036
		[SecurityCritical]
		private static Type typeOfISerializableDataNode;

		// Token: 0x0400040D RID: 1037
		[SecurityCritical]
		private static Type typeOfClassDataNode;

		// Token: 0x0400040E RID: 1038
		[SecurityCritical]
		private static Type typeOfCollectionDataNode;

		// Token: 0x0400040F RID: 1039
		[SecurityCritical]
		private static Type typeOfXmlDataNode;

		// Token: 0x04000410 RID: 1040
		[SecurityCritical]
		private static Type typeOfNullable;

		// Token: 0x04000411 RID: 1041
		[SecurityCritical]
		private static Type typeOfReflectionPointer;

		// Token: 0x04000412 RID: 1042
		[SecurityCritical]
		private static Type typeOfIDictionaryGeneric;

		// Token: 0x04000413 RID: 1043
		[SecurityCritical]
		private static Type typeOfIDictionary;

		// Token: 0x04000414 RID: 1044
		[SecurityCritical]
		private static Type typeOfIListGeneric;

		// Token: 0x04000415 RID: 1045
		[SecurityCritical]
		private static Type typeOfIList;

		// Token: 0x04000416 RID: 1046
		[SecurityCritical]
		private static Type typeOfICollectionGeneric;

		// Token: 0x04000417 RID: 1047
		[SecurityCritical]
		private static Type typeOfICollection;

		// Token: 0x04000418 RID: 1048
		[SecurityCritical]
		private static Type typeOfIEnumerableGeneric;

		// Token: 0x04000419 RID: 1049
		[SecurityCritical]
		private static Type typeOfIEnumerable;

		// Token: 0x0400041A RID: 1050
		[SecurityCritical]
		private static Type typeOfIEnumeratorGeneric;

		// Token: 0x0400041B RID: 1051
		[SecurityCritical]
		private static Type typeOfIEnumerator;

		// Token: 0x0400041C RID: 1052
		[SecurityCritical]
		private static Type typeOfKeyValuePair;

		// Token: 0x0400041D RID: 1053
		[SecurityCritical]
		private static Type typeOfKeyValue;

		// Token: 0x0400041E RID: 1054
		[SecurityCritical]
		private static Type typeOfIDictionaryEnumerator;

		// Token: 0x0400041F RID: 1055
		[SecurityCritical]
		private static Type typeOfDictionaryEnumerator;

		// Token: 0x04000420 RID: 1056
		[SecurityCritical]
		private static Type typeOfGenericDictionaryEnumerator;

		// Token: 0x04000421 RID: 1057
		[SecurityCritical]
		private static Type typeOfDictionaryGeneric;

		// Token: 0x04000422 RID: 1058
		[SecurityCritical]
		private static Type typeOfHashtable;

		// Token: 0x04000423 RID: 1059
		[SecurityCritical]
		private static Type typeOfListGeneric;

		// Token: 0x04000424 RID: 1060
		[SecurityCritical]
		private static Type typeOfXmlElement;

		// Token: 0x04000425 RID: 1061
		[SecurityCritical]
		private static Type typeOfDBNull;

		// Token: 0x04000426 RID: 1062
		[SecurityCritical]
		private static Uri dataContractXsdBaseNamespaceUri;

		// Token: 0x04000427 RID: 1063
		public const bool DefaultIsRequired = false;

		// Token: 0x04000428 RID: 1064
		public const bool DefaultEmitDefaultValue = true;

		// Token: 0x04000429 RID: 1065
		public const int DefaultOrder = 0;

		// Token: 0x0400042A RID: 1066
		public const bool DefaultIsReference = false;

		// Token: 0x0400042B RID: 1067
		public static readonly string NewObjectId = string.Empty;

		// Token: 0x0400042C RID: 1068
		public const string SimpleSRSInternalsVisiblePattern = "^[\\s]*System\\.Runtime\\.Serialization[\\s]*$";

		// Token: 0x0400042D RID: 1069
		public const string FullSRSInternalsVisiblePattern = "^[\\s]*System\\.Runtime\\.Serialization[\\s]*,[\\s]*PublicKey[\\s]*=[\\s]*(?i:00000000000000000400000000000000)[\\s]*$";

		// Token: 0x0400042E RID: 1070
		public const string NullObjectId = null;

		// Token: 0x0400042F RID: 1071
		public const string Space = " ";

		// Token: 0x04000430 RID: 1072
		public const string OpenBracket = "[";

		// Token: 0x04000431 RID: 1073
		public const string CloseBracket = "]";

		// Token: 0x04000432 RID: 1074
		public const string Comma = ",";

		// Token: 0x04000433 RID: 1075
		public const string XsiPrefix = "i";

		// Token: 0x04000434 RID: 1076
		public const string XsdPrefix = "x";

		// Token: 0x04000435 RID: 1077
		public const string SerPrefix = "z";

		// Token: 0x04000436 RID: 1078
		public const string SerPrefixForSchema = "ser";

		// Token: 0x04000437 RID: 1079
		public const string ElementPrefix = "q";

		// Token: 0x04000438 RID: 1080
		public const string DataContractXsdBaseNamespace = "http://schemas.datacontract.org/2004/07/";

		// Token: 0x04000439 RID: 1081
		public const string DataContractXmlNamespace = "http://schemas.datacontract.org/2004/07/System.Xml";

		// Token: 0x0400043A RID: 1082
		public const string SchemaInstanceNamespace = "http://www.w3.org/2001/XMLSchema-instance";

		// Token: 0x0400043B RID: 1083
		public const string SchemaNamespace = "http://www.w3.org/2001/XMLSchema";

		// Token: 0x0400043C RID: 1084
		public const string XsiNilLocalName = "nil";

		// Token: 0x0400043D RID: 1085
		public const string XsiTypeLocalName = "type";

		// Token: 0x0400043E RID: 1086
		public const string TnsPrefix = "tns";

		// Token: 0x0400043F RID: 1087
		public const string OccursUnbounded = "unbounded";

		// Token: 0x04000440 RID: 1088
		public const string AnyTypeLocalName = "anyType";

		// Token: 0x04000441 RID: 1089
		public const string StringLocalName = "string";

		// Token: 0x04000442 RID: 1090
		public const string IntLocalName = "int";

		// Token: 0x04000443 RID: 1091
		public const string True = "true";

		// Token: 0x04000444 RID: 1092
		public const string False = "false";

		// Token: 0x04000445 RID: 1093
		public const string ArrayPrefix = "ArrayOf";

		// Token: 0x04000446 RID: 1094
		public const string XmlnsNamespace = "http://www.w3.org/2000/xmlns/";

		// Token: 0x04000447 RID: 1095
		public const string XmlnsPrefix = "xmlns";

		// Token: 0x04000448 RID: 1096
		public const string SchemaLocalName = "schema";

		// Token: 0x04000449 RID: 1097
		public const string CollectionsNamespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays";

		// Token: 0x0400044A RID: 1098
		public const string DefaultClrNamespace = "GeneratedNamespace";

		// Token: 0x0400044B RID: 1099
		public const string DefaultTypeName = "GeneratedType";

		// Token: 0x0400044C RID: 1100
		public const string DefaultGeneratedMember = "GeneratedMember";

		// Token: 0x0400044D RID: 1101
		public const string DefaultFieldSuffix = "Field";

		// Token: 0x0400044E RID: 1102
		public const string DefaultPropertySuffix = "Property";

		// Token: 0x0400044F RID: 1103
		public const string DefaultMemberSuffix = "Member";

		// Token: 0x04000450 RID: 1104
		public const string NameProperty = "Name";

		// Token: 0x04000451 RID: 1105
		public const string NamespaceProperty = "Namespace";

		// Token: 0x04000452 RID: 1106
		public const string OrderProperty = "Order";

		// Token: 0x04000453 RID: 1107
		public const string IsReferenceProperty = "IsReference";

		// Token: 0x04000454 RID: 1108
		public const string IsRequiredProperty = "IsRequired";

		// Token: 0x04000455 RID: 1109
		public const string EmitDefaultValueProperty = "EmitDefaultValue";

		// Token: 0x04000456 RID: 1110
		public const string ClrNamespaceProperty = "ClrNamespace";

		// Token: 0x04000457 RID: 1111
		public const string ItemNameProperty = "ItemName";

		// Token: 0x04000458 RID: 1112
		public const string KeyNameProperty = "KeyName";

		// Token: 0x04000459 RID: 1113
		public const string ValueNameProperty = "ValueName";

		// Token: 0x0400045A RID: 1114
		public const string SerializationInfoPropertyName = "SerializationInfo";

		// Token: 0x0400045B RID: 1115
		public const string SerializationInfoFieldName = "info";

		// Token: 0x0400045C RID: 1116
		public const string NodeArrayPropertyName = "Nodes";

		// Token: 0x0400045D RID: 1117
		public const string NodeArrayFieldName = "nodesField";

		// Token: 0x0400045E RID: 1118
		public const string ExportSchemaMethod = "ExportSchema";

		// Token: 0x0400045F RID: 1119
		public const string IsAnyProperty = "IsAny";

		// Token: 0x04000460 RID: 1120
		public const string ContextFieldName = "context";

		// Token: 0x04000461 RID: 1121
		public const string GetObjectDataMethodName = "GetObjectData";

		// Token: 0x04000462 RID: 1122
		public const string GetEnumeratorMethodName = "GetEnumerator";

		// Token: 0x04000463 RID: 1123
		public const string MoveNextMethodName = "MoveNext";

		// Token: 0x04000464 RID: 1124
		public const string AddValueMethodName = "AddValue";

		// Token: 0x04000465 RID: 1125
		public const string CurrentPropertyName = "Current";

		// Token: 0x04000466 RID: 1126
		public const string ValueProperty = "Value";

		// Token: 0x04000467 RID: 1127
		public const string EnumeratorFieldName = "enumerator";

		// Token: 0x04000468 RID: 1128
		public const string SerializationEntryFieldName = "entry";

		// Token: 0x04000469 RID: 1129
		public const string ExtensionDataSetMethod = "set_ExtensionData";

		// Token: 0x0400046A RID: 1130
		public const string ExtensionDataSetExplicitMethod = "System.Runtime.Serialization.IExtensibleDataObject.set_ExtensionData";

		// Token: 0x0400046B RID: 1131
		public const string ExtensionDataObjectPropertyName = "ExtensionData";

		// Token: 0x0400046C RID: 1132
		public const string ExtensionDataObjectFieldName = "extensionDataField";

		// Token: 0x0400046D RID: 1133
		public const string AddMethodName = "Add";

		// Token: 0x0400046E RID: 1134
		public const string ParseMethodName = "Parse";

		// Token: 0x0400046F RID: 1135
		public const string GetCurrentMethodName = "get_Current";

		// Token: 0x04000470 RID: 1136
		public const string SerializationNamespace = "http://schemas.microsoft.com/2003/10/Serialization/";

		// Token: 0x04000471 RID: 1137
		public const string ClrTypeLocalName = "Type";

		// Token: 0x04000472 RID: 1138
		public const string ClrAssemblyLocalName = "Assembly";

		// Token: 0x04000473 RID: 1139
		public const string IsValueTypeLocalName = "IsValueType";

		// Token: 0x04000474 RID: 1140
		public const string EnumerationValueLocalName = "EnumerationValue";

		// Token: 0x04000475 RID: 1141
		public const string SurrogateDataLocalName = "Surrogate";

		// Token: 0x04000476 RID: 1142
		public const string GenericTypeLocalName = "GenericType";

		// Token: 0x04000477 RID: 1143
		public const string GenericParameterLocalName = "GenericParameter";

		// Token: 0x04000478 RID: 1144
		public const string GenericNameAttribute = "Name";

		// Token: 0x04000479 RID: 1145
		public const string GenericNamespaceAttribute = "Namespace";

		// Token: 0x0400047A RID: 1146
		public const string GenericParameterNestedLevelAttribute = "NestedLevel";

		// Token: 0x0400047B RID: 1147
		public const string IsDictionaryLocalName = "IsDictionary";

		// Token: 0x0400047C RID: 1148
		public const string ActualTypeLocalName = "ActualType";

		// Token: 0x0400047D RID: 1149
		public const string ActualTypeNameAttribute = "Name";

		// Token: 0x0400047E RID: 1150
		public const string ActualTypeNamespaceAttribute = "Namespace";

		// Token: 0x0400047F RID: 1151
		public const string DefaultValueLocalName = "DefaultValue";

		// Token: 0x04000480 RID: 1152
		public const string EmitDefaultValueAttribute = "EmitDefaultValue";

		// Token: 0x04000481 RID: 1153
		public const string ISerializableFactoryTypeLocalName = "FactoryType";

		// Token: 0x04000482 RID: 1154
		public const string IdLocalName = "Id";

		// Token: 0x04000483 RID: 1155
		public const string RefLocalName = "Ref";

		// Token: 0x04000484 RID: 1156
		public const string ArraySizeLocalName = "Size";

		// Token: 0x04000485 RID: 1157
		public const string KeyLocalName = "Key";

		// Token: 0x04000486 RID: 1158
		public const string ValueLocalName = "Value";

		// Token: 0x04000487 RID: 1159
		public const string MscorlibAssemblyName = "0";

		// Token: 0x04000488 RID: 1160
		public const string MscorlibAssemblySimpleName = "mscorlib";

		// Token: 0x04000489 RID: 1161
		public const string MscorlibFileName = "mscorlib.dll";

		// Token: 0x0400048A RID: 1162
		public const string SerializationSchema = "<?xml version='1.0' encoding='utf-8'?>\n<xs:schema elementFormDefault='qualified' attributeFormDefault='qualified' xmlns:tns='http://schemas.microsoft.com/2003/10/Serialization/' targetNamespace='http://schemas.microsoft.com/2003/10/Serialization/' xmlns:xs='http://www.w3.org/2001/XMLSchema'>\n  <xs:element name='anyType' nillable='true' type='xs:anyType' />\n  <xs:element name='anyURI' nillable='true' type='xs:anyURI' />\n  <xs:element name='base64Binary' nillable='true' type='xs:base64Binary' />\n  <xs:element name='boolean' nillable='true' type='xs:boolean' />\n  <xs:element name='byte' nillable='true' type='xs:byte' />\n  <xs:element name='dateTime' nillable='true' type='xs:dateTime' />\n  <xs:element name='decimal' nillable='true' type='xs:decimal' />\n  <xs:element name='double' nillable='true' type='xs:double' />\n  <xs:element name='float' nillable='true' type='xs:float' />\n  <xs:element name='int' nillable='true' type='xs:int' />\n  <xs:element name='long' nillable='true' type='xs:long' />\n  <xs:element name='QName' nillable='true' type='xs:QName' />\n  <xs:element name='short' nillable='true' type='xs:short' />\n  <xs:element name='string' nillable='true' type='xs:string' />\n  <xs:element name='unsignedByte' nillable='true' type='xs:unsignedByte' />\n  <xs:element name='unsignedInt' nillable='true' type='xs:unsignedInt' />\n  <xs:element name='unsignedLong' nillable='true' type='xs:unsignedLong' />\n  <xs:element name='unsignedShort' nillable='true' type='xs:unsignedShort' />\n  <xs:element name='char' nillable='true' type='tns:char' />\n  <xs:simpleType name='char'>\n    <xs:restriction base='xs:int'/>\n  </xs:simpleType>  \n  <xs:element name='duration' nillable='true' type='tns:duration' />\n  <xs:simpleType name='duration'>\n    <xs:restriction base='xs:duration'>\n      <xs:pattern value='\\-?P(\\d*D)?(T(\\d*H)?(\\d*M)?(\\d*(\\.\\d*)?S)?)?' />\n      <xs:minInclusive value='-P10675199DT2H48M5.4775808S' />\n      <xs:maxInclusive value='P10675199DT2H48M5.4775807S' />\n    </xs:restriction>\n  </xs:simpleType>\n  <xs:element name='guid' nillable='true' type='tns:guid' />\n  <xs:simpleType name='guid'>\n    <xs:restriction base='xs:string'>\n      <xs:pattern value='[\\da-fA-F]{8}-[\\da-fA-F]{4}-[\\da-fA-F]{4}-[\\da-fA-F]{4}-[\\da-fA-F]{12}' />\n    </xs:restriction>\n  </xs:simpleType>\n  <xs:attribute name='FactoryType' type='xs:QName' />\n  <xs:attribute name='Id' type='xs:ID' />\n  <xs:attribute name='Ref' type='xs:IDREF' />\n</xs:schema>\n";
	}
}
