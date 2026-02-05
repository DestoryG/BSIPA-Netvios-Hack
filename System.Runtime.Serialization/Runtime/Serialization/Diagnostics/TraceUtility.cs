using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Diagnostics;

namespace System.Runtime.Serialization.Diagnostics
{
	// Token: 0x0200011E RID: 286
	internal static class TraceUtility
	{
		// Token: 0x0600117C RID: 4476 RVA: 0x00049E9C File Offset: 0x0004809C
		internal static void Trace(TraceEventType severity, int traceCode, string traceDescription)
		{
			TraceUtility.Trace(severity, traceCode, traceDescription, null);
		}

		// Token: 0x0600117D RID: 4477 RVA: 0x00049EA7 File Offset: 0x000480A7
		internal static void Trace(TraceEventType severity, int traceCode, string traceDescription, TraceRecord record)
		{
			TraceUtility.Trace(severity, traceCode, traceDescription, record, null);
		}

		// Token: 0x0600117E RID: 4478 RVA: 0x00049EB4 File Offset: 0x000480B4
		internal static void Trace(TraceEventType severity, int traceCode, string traceDescription, TraceRecord record, Exception exception)
		{
			string text = "";
			object[] array = new object[7];
			array[0] = severity;
			array[1] = traceCode;
			array[2] = text;
			array[3] = traceDescription;
			array[4] = record;
			array[5] = exception;
			DiagnosticUtility.DiagnosticTrace.TraceEvent(array);
		}

		// Token: 0x0400088C RID: 2188
		private static Dictionary<int, string> traceCodes = new Dictionary<int, string>(18)
		{
			{ 196609, "WriteObjectBegin" },
			{ 196610, "WriteObjectEnd" },
			{ 196611, "WriteObjectContentBegin" },
			{ 196612, "WriteObjectContentEnd" },
			{ 196613, "ReadObjectBegin" },
			{ 196614, "ReadObjectEnd" },
			{ 196615, "ElementIgnored" },
			{ 196616, "XsdExportBegin" },
			{ 196617, "XsdExportEnd" },
			{ 196618, "XsdImportBegin" },
			{ 196619, "XsdImportEnd" },
			{ 196620, "XsdExportError" },
			{ 196621, "XsdImportError" },
			{ 196622, "XsdExportAnnotationFailed" },
			{ 196623, "XsdImportAnnotationFailed" },
			{ 196624, "XsdExportDupItems" },
			{ 196625, "FactoryTypeNotFound" },
			{ 196626, "ObjectWithLargeDepth" }
		};
	}
}
