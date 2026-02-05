using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000078 RID: 120
	public class DiagnosticsTraceWriter : ITraceWriter
	{
		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000661 RID: 1633 RVA: 0x0001B9E0 File Offset: 0x00019BE0
		// (set) Token: 0x06000662 RID: 1634 RVA: 0x0001B9E8 File Offset: 0x00019BE8
		public TraceLevel LevelFilter { get; set; }

		// Token: 0x06000663 RID: 1635 RVA: 0x0001B9F1 File Offset: 0x00019BF1
		private TraceEventType GetTraceEventType(TraceLevel level)
		{
			switch (level)
			{
			case TraceLevel.Error:
				return TraceEventType.Error;
			case TraceLevel.Warning:
				return TraceEventType.Warning;
			case TraceLevel.Info:
				return TraceEventType.Information;
			case TraceLevel.Verbose:
				return TraceEventType.Verbose;
			default:
				throw new ArgumentOutOfRangeException("level");
			}
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x0001BA20 File Offset: 0x00019C20
		[NullableContext(1)]
		public void Trace(TraceLevel level, string message, [Nullable(2)] Exception ex)
		{
			if (level == TraceLevel.Off)
			{
				return;
			}
			TraceEventCache traceEventCache = new TraceEventCache();
			TraceEventType traceEventType = this.GetTraceEventType(level);
			foreach (object obj in global::System.Diagnostics.Trace.Listeners)
			{
				TraceListener traceListener = (TraceListener)obj;
				if (!traceListener.IsThreadSafe)
				{
					TraceListener traceListener2 = traceListener;
					lock (traceListener2)
					{
						traceListener.TraceEvent(traceEventCache, "Newtonsoft.Json", traceEventType, 0, message);
						goto IL_006E;
					}
					goto IL_005F;
				}
				goto IL_005F;
				IL_006E:
				if (global::System.Diagnostics.Trace.AutoFlush)
				{
					traceListener.Flush();
					continue;
				}
				continue;
				IL_005F:
				traceListener.TraceEvent(traceEventCache, "Newtonsoft.Json", traceEventType, 0, message);
				goto IL_006E;
			}
		}
	}
}
