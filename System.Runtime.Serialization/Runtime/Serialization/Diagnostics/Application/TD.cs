using System;
using System.Globalization;
using System.Resources;
using System.Runtime.Diagnostics;
using System.Security;

namespace System.Runtime.Serialization.Diagnostics.Application
{
	// Token: 0x0200011F RID: 287
	internal class TD
	{
		// Token: 0x06001180 RID: 4480 RVA: 0x0004A031 File Offset: 0x00048231
		private TD()
		{
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06001181 RID: 4481 RVA: 0x0004A039 File Offset: 0x00048239
		private static ResourceManager ResourceManager
		{
			get
			{
				if (TD.resourceManager == null)
				{
					TD.resourceManager = new ResourceManager("System.Runtime.Serialization.Diagnostics.Application.TD", typeof(TD).Assembly);
				}
				return TD.resourceManager;
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06001182 RID: 4482 RVA: 0x0004A065 File Offset: 0x00048265
		// (set) Token: 0x06001183 RID: 4483 RVA: 0x0004A06C File Offset: 0x0004826C
		internal static CultureInfo Culture
		{
			get
			{
				return TD.resourceCulture;
			}
			set
			{
				TD.resourceCulture = value;
			}
		}

		// Token: 0x06001184 RID: 4484 RVA: 0x0004A074 File Offset: 0x00048274
		internal static bool ReaderQuotaExceededIsEnabled()
		{
			return FxTrace.ShouldTraceError && TD.IsEtwEventEnabled(0);
		}

		// Token: 0x06001185 RID: 4485 RVA: 0x0004A088 File Offset: 0x00048288
		internal static void ReaderQuotaExceeded(string param0)
		{
			TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
			if (TD.IsEtwEventEnabled(0))
			{
				TD.WriteEtwEvent(0, null, param0, serializedPayload.AppDomainFriendlyName);
			}
		}

		// Token: 0x06001186 RID: 4486 RVA: 0x0004A0BB File Offset: 0x000482BB
		internal static bool DCSerializeWithSurrogateStartIsEnabled()
		{
			return FxTrace.ShouldTraceVerbose && TD.IsEtwEventEnabled(1);
		}

		// Token: 0x06001187 RID: 4487 RVA: 0x0004A0CC File Offset: 0x000482CC
		internal static void DCSerializeWithSurrogateStart(string SurrogateType)
		{
			TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
			if (TD.IsEtwEventEnabled(1))
			{
				TD.WriteEtwEvent(1, null, SurrogateType, serializedPayload.AppDomainFriendlyName);
			}
		}

		// Token: 0x06001188 RID: 4488 RVA: 0x0004A0FF File Offset: 0x000482FF
		internal static bool DCSerializeWithSurrogateStopIsEnabled()
		{
			return FxTrace.ShouldTraceVerbose && TD.IsEtwEventEnabled(2);
		}

		// Token: 0x06001189 RID: 4489 RVA: 0x0004A110 File Offset: 0x00048310
		internal static void DCSerializeWithSurrogateStop()
		{
			TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
			if (TD.IsEtwEventEnabled(2))
			{
				TD.WriteEtwEvent(2, null, serializedPayload.AppDomainFriendlyName);
			}
		}

		// Token: 0x0600118A RID: 4490 RVA: 0x0004A142 File Offset: 0x00048342
		internal static bool DCDeserializeWithSurrogateStartIsEnabled()
		{
			return FxTrace.ShouldTraceVerbose && TD.IsEtwEventEnabled(3);
		}

		// Token: 0x0600118B RID: 4491 RVA: 0x0004A154 File Offset: 0x00048354
		internal static void DCDeserializeWithSurrogateStart(string SurrogateType)
		{
			TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
			if (TD.IsEtwEventEnabled(3))
			{
				TD.WriteEtwEvent(3, null, SurrogateType, serializedPayload.AppDomainFriendlyName);
			}
		}

		// Token: 0x0600118C RID: 4492 RVA: 0x0004A187 File Offset: 0x00048387
		internal static bool DCDeserializeWithSurrogateStopIsEnabled()
		{
			return FxTrace.ShouldTraceVerbose && TD.IsEtwEventEnabled(4);
		}

		// Token: 0x0600118D RID: 4493 RVA: 0x0004A198 File Offset: 0x00048398
		internal static void DCDeserializeWithSurrogateStop()
		{
			TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
			if (TD.IsEtwEventEnabled(4))
			{
				TD.WriteEtwEvent(4, null, serializedPayload.AppDomainFriendlyName);
			}
		}

		// Token: 0x0600118E RID: 4494 RVA: 0x0004A1CA File Offset: 0x000483CA
		internal static bool ImportKnownTypesStartIsEnabled()
		{
			return FxTrace.ShouldTraceVerbose && TD.IsEtwEventEnabled(5);
		}

		// Token: 0x0600118F RID: 4495 RVA: 0x0004A1DC File Offset: 0x000483DC
		internal static void ImportKnownTypesStart()
		{
			TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
			if (TD.IsEtwEventEnabled(5))
			{
				TD.WriteEtwEvent(5, null, serializedPayload.AppDomainFriendlyName);
			}
		}

		// Token: 0x06001190 RID: 4496 RVA: 0x0004A20E File Offset: 0x0004840E
		internal static bool ImportKnownTypesStopIsEnabled()
		{
			return FxTrace.ShouldTraceVerbose && TD.IsEtwEventEnabled(6);
		}

		// Token: 0x06001191 RID: 4497 RVA: 0x0004A220 File Offset: 0x00048420
		internal static void ImportKnownTypesStop()
		{
			TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
			if (TD.IsEtwEventEnabled(6))
			{
				TD.WriteEtwEvent(6, null, serializedPayload.AppDomainFriendlyName);
			}
		}

		// Token: 0x06001192 RID: 4498 RVA: 0x0004A252 File Offset: 0x00048452
		internal static bool DCResolverResolveIsEnabled()
		{
			return FxTrace.ShouldTraceVerbose && TD.IsEtwEventEnabled(7);
		}

		// Token: 0x06001193 RID: 4499 RVA: 0x0004A264 File Offset: 0x00048464
		internal static void DCResolverResolve(string TypeName)
		{
			TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
			if (TD.IsEtwEventEnabled(7))
			{
				TD.WriteEtwEvent(7, null, TypeName, serializedPayload.AppDomainFriendlyName);
			}
		}

		// Token: 0x06001194 RID: 4500 RVA: 0x0004A297 File Offset: 0x00048497
		internal static bool DCGenWriterStartIsEnabled()
		{
			return FxTrace.ShouldTraceVerbose && TD.IsEtwEventEnabled(8);
		}

		// Token: 0x06001195 RID: 4501 RVA: 0x0004A2A8 File Offset: 0x000484A8
		internal static void DCGenWriterStart(string Kind, string TypeName)
		{
			TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
			if (TD.IsEtwEventEnabled(8))
			{
				TD.WriteEtwEvent(8, null, Kind, TypeName, serializedPayload.AppDomainFriendlyName);
			}
		}

		// Token: 0x06001196 RID: 4502 RVA: 0x0004A2DC File Offset: 0x000484DC
		internal static bool DCGenWriterStopIsEnabled()
		{
			return FxTrace.ShouldTraceVerbose && TD.IsEtwEventEnabled(9);
		}

		// Token: 0x06001197 RID: 4503 RVA: 0x0004A2F0 File Offset: 0x000484F0
		internal static void DCGenWriterStop()
		{
			TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
			if (TD.IsEtwEventEnabled(9))
			{
				TD.WriteEtwEvent(9, null, serializedPayload.AppDomainFriendlyName);
			}
		}

		// Token: 0x06001198 RID: 4504 RVA: 0x0004A324 File Offset: 0x00048524
		internal static bool DCGenReaderStartIsEnabled()
		{
			return FxTrace.ShouldTraceVerbose && TD.IsEtwEventEnabled(10);
		}

		// Token: 0x06001199 RID: 4505 RVA: 0x0004A338 File Offset: 0x00048538
		internal static void DCGenReaderStart(string Kind, string TypeName)
		{
			TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
			if (TD.IsEtwEventEnabled(10))
			{
				TD.WriteEtwEvent(10, null, Kind, TypeName, serializedPayload.AppDomainFriendlyName);
			}
		}

		// Token: 0x0600119A RID: 4506 RVA: 0x0004A36E File Offset: 0x0004856E
		internal static bool DCGenReaderStopIsEnabled()
		{
			return FxTrace.ShouldTraceVerbose && TD.IsEtwEventEnabled(11);
		}

		// Token: 0x0600119B RID: 4507 RVA: 0x0004A380 File Offset: 0x00048580
		internal static void DCGenReaderStop()
		{
			TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
			if (TD.IsEtwEventEnabled(11))
			{
				TD.WriteEtwEvent(11, null, serializedPayload.AppDomainFriendlyName);
			}
		}

		// Token: 0x0600119C RID: 4508 RVA: 0x0004A3B4 File Offset: 0x000485B4
		internal static bool DCJsonGenReaderStartIsEnabled()
		{
			return FxTrace.ShouldTraceVerbose && TD.IsEtwEventEnabled(12);
		}

		// Token: 0x0600119D RID: 4509 RVA: 0x0004A3C8 File Offset: 0x000485C8
		internal static void DCJsonGenReaderStart(string Kind, string TypeName)
		{
			TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
			if (TD.IsEtwEventEnabled(12))
			{
				TD.WriteEtwEvent(12, null, Kind, TypeName, serializedPayload.AppDomainFriendlyName);
			}
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x0004A3FE File Offset: 0x000485FE
		internal static bool DCJsonGenReaderStopIsEnabled()
		{
			return FxTrace.ShouldTraceVerbose && TD.IsEtwEventEnabled(13);
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x0004A410 File Offset: 0x00048610
		internal static void DCJsonGenReaderStop()
		{
			TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
			if (TD.IsEtwEventEnabled(13))
			{
				TD.WriteEtwEvent(13, null, serializedPayload.AppDomainFriendlyName);
			}
		}

		// Token: 0x060011A0 RID: 4512 RVA: 0x0004A444 File Offset: 0x00048644
		internal static bool DCJsonGenWriterStartIsEnabled()
		{
			return FxTrace.ShouldTraceVerbose && TD.IsEtwEventEnabled(14);
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x0004A458 File Offset: 0x00048658
		internal static void DCJsonGenWriterStart(string Kind, string TypeName)
		{
			TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
			if (TD.IsEtwEventEnabled(14))
			{
				TD.WriteEtwEvent(14, null, Kind, TypeName, serializedPayload.AppDomainFriendlyName);
			}
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x0004A48E File Offset: 0x0004868E
		internal static bool DCJsonGenWriterStopIsEnabled()
		{
			return FxTrace.ShouldTraceVerbose && TD.IsEtwEventEnabled(15);
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x0004A4A0 File Offset: 0x000486A0
		internal static void DCJsonGenWriterStop()
		{
			TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
			if (TD.IsEtwEventEnabled(15))
			{
				TD.WriteEtwEvent(15, null, serializedPayload.AppDomainFriendlyName);
			}
		}

		// Token: 0x060011A4 RID: 4516 RVA: 0x0004A4D4 File Offset: 0x000486D4
		internal static bool GenXmlSerializableStartIsEnabled()
		{
			return FxTrace.ShouldTraceVerbose && TD.IsEtwEventEnabled(16);
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x0004A4E8 File Offset: 0x000486E8
		internal static void GenXmlSerializableStart(string DCType)
		{
			TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
			if (TD.IsEtwEventEnabled(16))
			{
				TD.WriteEtwEvent(16, null, DCType, serializedPayload.AppDomainFriendlyName);
			}
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x0004A51D File Offset: 0x0004871D
		internal static bool GenXmlSerializableStopIsEnabled()
		{
			return FxTrace.ShouldTraceVerbose && TD.IsEtwEventEnabled(17);
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x0004A530 File Offset: 0x00048730
		internal static void GenXmlSerializableStop()
		{
			TracePayload serializedPayload = FxTrace.Trace.GetSerializedPayload(null, null, null);
			if (TD.IsEtwEventEnabled(17))
			{
				TD.WriteEtwEvent(17, null, serializedPayload.AppDomainFriendlyName);
			}
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x0004A564 File Offset: 0x00048764
		[SecuritySafeCritical]
		private static void CreateEventDescriptors()
		{
			EventDescriptor[] array = new EventDescriptor[]
			{
				new EventDescriptor(1420, 0, 18, 2, 0, 2560, 2305843009217888256L),
				new EventDescriptor(5001, 0, 19, 5, 1, 2592, 1152921504606846978L),
				new EventDescriptor(5002, 0, 19, 5, 2, 2592, 1152921504606846978L),
				new EventDescriptor(5003, 0, 19, 5, 1, 2591, 1152921504606846978L),
				new EventDescriptor(5004, 0, 19, 5, 2, 2591, 1152921504606846978L),
				new EventDescriptor(5005, 0, 19, 5, 1, 2547, 1152921504606846978L),
				new EventDescriptor(5006, 0, 19, 5, 2, 2547, 1152921504606846978L),
				new EventDescriptor(5007, 0, 19, 5, 1, 2528, 1152921504606846978L),
				new EventDescriptor(5008, 0, 19, 5, 1, 2544, 1152921504606846978L),
				new EventDescriptor(5009, 0, 19, 5, 2, 2544, 1152921504606846978L),
				new EventDescriptor(5010, 0, 19, 5, 1, 2543, 1152921504606846978L),
				new EventDescriptor(5011, 0, 19, 5, 2, 2543, 1152921504606846978L),
				new EventDescriptor(5012, 0, 19, 5, 1, 2543, 1152921504606846978L),
				new EventDescriptor(5013, 0, 19, 5, 2, 2543, 1152921504606846978L),
				new EventDescriptor(5014, 0, 19, 5, 1, 2544, 1152921504606846978L),
				new EventDescriptor(5015, 0, 19, 5, 2, 2544, 1152921504606846978L),
				new EventDescriptor(5016, 0, 19, 5, 1, 2545, 1152921504606846978L),
				new EventDescriptor(5017, 0, 19, 5, 2, 2545, 1152921504606846978L)
			};
			ushort[] array2 = new ushort[0];
			FxTrace.UpdateEventDefinitions(array, array2);
			TD.eventDescriptors = array;
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x0004A81C File Offset: 0x00048A1C
		private static void EnsureEventDescriptors()
		{
			if (TD.eventDescriptorsCreated)
			{
				return;
			}
			lock (TD.syncLock)
			{
				if (!TD.eventDescriptorsCreated)
				{
					TD.CreateEventDescriptors();
					TD.eventDescriptorsCreated = true;
				}
			}
		}

		// Token: 0x060011AA RID: 4522 RVA: 0x0004A874 File Offset: 0x00048A74
		private static bool IsEtwEventEnabled(int eventIndex)
		{
			if (FxTrace.Trace.IsEtwProviderEnabled)
			{
				TD.EnsureEventDescriptors();
				return FxTrace.IsEventEnabled(eventIndex);
			}
			return false;
		}

		// Token: 0x060011AB RID: 4523 RVA: 0x0004A88F File Offset: 0x00048A8F
		[SecuritySafeCritical]
		private static bool WriteEtwEvent(int eventIndex, EventTraceActivity eventParam0, string eventParam1, string eventParam2)
		{
			TD.EnsureEventDescriptors();
			return FxTrace.Trace.EtwProvider.WriteEvent(ref TD.eventDescriptors[eventIndex], eventParam0, eventParam1, eventParam2);
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x0004A8B3 File Offset: 0x00048AB3
		[SecuritySafeCritical]
		private static bool WriteEtwEvent(int eventIndex, EventTraceActivity eventParam0, string eventParam1)
		{
			TD.EnsureEventDescriptors();
			return FxTrace.Trace.EtwProvider.WriteEvent(ref TD.eventDescriptors[eventIndex], eventParam0, eventParam1);
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x0004A8D6 File Offset: 0x00048AD6
		[SecuritySafeCritical]
		private static bool WriteEtwEvent(int eventIndex, EventTraceActivity eventParam0, string eventParam1, string eventParam2, string eventParam3)
		{
			TD.EnsureEventDescriptors();
			return FxTrace.Trace.EtwProvider.WriteEvent(ref TD.eventDescriptors[eventIndex], eventParam0, eventParam1, eventParam2, eventParam3);
		}

		// Token: 0x0400088D RID: 2189
		private static ResourceManager resourceManager;

		// Token: 0x0400088E RID: 2190
		private static CultureInfo resourceCulture;

		// Token: 0x0400088F RID: 2191
		[SecurityCritical]
		private static EventDescriptor[] eventDescriptors;

		// Token: 0x04000890 RID: 2192
		private static object syncLock = new object();

		// Token: 0x04000891 RID: 2193
		private static volatile bool eventDescriptorsCreated;
	}
}
