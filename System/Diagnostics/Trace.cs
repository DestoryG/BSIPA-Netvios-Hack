using System;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x020004AD RID: 1197
	public sealed class Trace
	{
		// Token: 0x06002C50 RID: 11344 RVA: 0x000C7A38 File Offset: 0x000C5C38
		private Trace()
		{
		}

		// Token: 0x17000ABF RID: 2751
		// (get) Token: 0x06002C51 RID: 11345 RVA: 0x000C7A40 File Offset: 0x000C5C40
		public static TraceListenerCollection Listeners
		{
			[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
			get
			{
				new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
				return TraceInternal.Listeners;
			}
		}

		// Token: 0x17000AC0 RID: 2752
		// (get) Token: 0x06002C52 RID: 11346 RVA: 0x000C7A52 File Offset: 0x000C5C52
		// (set) Token: 0x06002C53 RID: 11347 RVA: 0x000C7A59 File Offset: 0x000C5C59
		public static bool AutoFlush
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				return TraceInternal.AutoFlush;
			}
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			set
			{
				TraceInternal.AutoFlush = value;
			}
		}

		// Token: 0x17000AC1 RID: 2753
		// (get) Token: 0x06002C54 RID: 11348 RVA: 0x000C7A61 File Offset: 0x000C5C61
		// (set) Token: 0x06002C55 RID: 11349 RVA: 0x000C7A68 File Offset: 0x000C5C68
		public static bool UseGlobalLock
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				return TraceInternal.UseGlobalLock;
			}
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			set
			{
				TraceInternal.UseGlobalLock = value;
			}
		}

		// Token: 0x17000AC2 RID: 2754
		// (get) Token: 0x06002C56 RID: 11350 RVA: 0x000C7A70 File Offset: 0x000C5C70
		public static CorrelationManager CorrelationManager
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				if (Trace.correlationManager == null)
				{
					Trace.correlationManager = new CorrelationManager();
				}
				return Trace.correlationManager;
			}
		}

		// Token: 0x17000AC3 RID: 2755
		// (get) Token: 0x06002C57 RID: 11351 RVA: 0x000C7A8E File Offset: 0x000C5C8E
		// (set) Token: 0x06002C58 RID: 11352 RVA: 0x000C7A95 File Offset: 0x000C5C95
		public static int IndentLevel
		{
			get
			{
				return TraceInternal.IndentLevel;
			}
			set
			{
				TraceInternal.IndentLevel = value;
			}
		}

		// Token: 0x17000AC4 RID: 2756
		// (get) Token: 0x06002C59 RID: 11353 RVA: 0x000C7A9D File Offset: 0x000C5C9D
		// (set) Token: 0x06002C5A RID: 11354 RVA: 0x000C7AA4 File Offset: 0x000C5CA4
		public static int IndentSize
		{
			get
			{
				return TraceInternal.IndentSize;
			}
			set
			{
				TraceInternal.IndentSize = value;
			}
		}

		// Token: 0x06002C5B RID: 11355 RVA: 0x000C7AAC File Offset: 0x000C5CAC
		[Conditional("TRACE")]
		public static void Flush()
		{
			TraceInternal.Flush();
		}

		// Token: 0x06002C5C RID: 11356 RVA: 0x000C7AB3 File Offset: 0x000C5CB3
		[Conditional("TRACE")]
		public static void Close()
		{
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			TraceInternal.Close();
		}

		// Token: 0x06002C5D RID: 11357 RVA: 0x000C7AC5 File Offset: 0x000C5CC5
		[Conditional("TRACE")]
		public static void Assert(bool condition)
		{
			TraceInternal.Assert(condition);
		}

		// Token: 0x06002C5E RID: 11358 RVA: 0x000C7ACD File Offset: 0x000C5CCD
		[Conditional("TRACE")]
		public static void Assert(bool condition, string message)
		{
			TraceInternal.Assert(condition, message);
		}

		// Token: 0x06002C5F RID: 11359 RVA: 0x000C7AD6 File Offset: 0x000C5CD6
		[Conditional("TRACE")]
		public static void Assert(bool condition, string message, string detailMessage)
		{
			TraceInternal.Assert(condition, message, detailMessage);
		}

		// Token: 0x06002C60 RID: 11360 RVA: 0x000C7AE0 File Offset: 0x000C5CE0
		[Conditional("TRACE")]
		public static void Fail(string message)
		{
			TraceInternal.Fail(message);
		}

		// Token: 0x06002C61 RID: 11361 RVA: 0x000C7AE8 File Offset: 0x000C5CE8
		[Conditional("TRACE")]
		public static void Fail(string message, string detailMessage)
		{
			TraceInternal.Fail(message, detailMessage);
		}

		// Token: 0x06002C62 RID: 11362 RVA: 0x000C7AF1 File Offset: 0x000C5CF1
		public static void Refresh()
		{
			DiagnosticsConfiguration.Refresh();
			Switch.RefreshAll();
			TraceSource.RefreshAll();
			TraceInternal.Refresh();
		}

		// Token: 0x06002C63 RID: 11363 RVA: 0x000C7B07 File Offset: 0x000C5D07
		[Conditional("TRACE")]
		public static void TraceInformation(string message)
		{
			TraceInternal.TraceEvent(TraceEventType.Information, 0, message, null);
		}

		// Token: 0x06002C64 RID: 11364 RVA: 0x000C7B12 File Offset: 0x000C5D12
		[Conditional("TRACE")]
		public static void TraceInformation(string format, params object[] args)
		{
			TraceInternal.TraceEvent(TraceEventType.Information, 0, format, args);
		}

		// Token: 0x06002C65 RID: 11365 RVA: 0x000C7B1D File Offset: 0x000C5D1D
		[Conditional("TRACE")]
		public static void TraceWarning(string message)
		{
			TraceInternal.TraceEvent(TraceEventType.Warning, 0, message, null);
		}

		// Token: 0x06002C66 RID: 11366 RVA: 0x000C7B28 File Offset: 0x000C5D28
		[Conditional("TRACE")]
		public static void TraceWarning(string format, params object[] args)
		{
			TraceInternal.TraceEvent(TraceEventType.Warning, 0, format, args);
		}

		// Token: 0x06002C67 RID: 11367 RVA: 0x000C7B33 File Offset: 0x000C5D33
		[Conditional("TRACE")]
		public static void TraceError(string message)
		{
			TraceInternal.TraceEvent(TraceEventType.Error, 0, message, null);
		}

		// Token: 0x06002C68 RID: 11368 RVA: 0x000C7B3E File Offset: 0x000C5D3E
		[Conditional("TRACE")]
		public static void TraceError(string format, params object[] args)
		{
			TraceInternal.TraceEvent(TraceEventType.Error, 0, format, args);
		}

		// Token: 0x06002C69 RID: 11369 RVA: 0x000C7B49 File Offset: 0x000C5D49
		[Conditional("TRACE")]
		public static void Write(string message)
		{
			TraceInternal.Write(message);
		}

		// Token: 0x06002C6A RID: 11370 RVA: 0x000C7B51 File Offset: 0x000C5D51
		[Conditional("TRACE")]
		public static void Write(object value)
		{
			TraceInternal.Write(value);
		}

		// Token: 0x06002C6B RID: 11371 RVA: 0x000C7B59 File Offset: 0x000C5D59
		[Conditional("TRACE")]
		public static void Write(string message, string category)
		{
			TraceInternal.Write(message, category);
		}

		// Token: 0x06002C6C RID: 11372 RVA: 0x000C7B62 File Offset: 0x000C5D62
		[Conditional("TRACE")]
		public static void Write(object value, string category)
		{
			TraceInternal.Write(value, category);
		}

		// Token: 0x06002C6D RID: 11373 RVA: 0x000C7B6B File Offset: 0x000C5D6B
		[Conditional("TRACE")]
		public static void WriteLine(string message)
		{
			TraceInternal.WriteLine(message);
		}

		// Token: 0x06002C6E RID: 11374 RVA: 0x000C7B73 File Offset: 0x000C5D73
		[Conditional("TRACE")]
		public static void WriteLine(object value)
		{
			TraceInternal.WriteLine(value);
		}

		// Token: 0x06002C6F RID: 11375 RVA: 0x000C7B7B File Offset: 0x000C5D7B
		[Conditional("TRACE")]
		public static void WriteLine(string message, string category)
		{
			TraceInternal.WriteLine(message, category);
		}

		// Token: 0x06002C70 RID: 11376 RVA: 0x000C7B84 File Offset: 0x000C5D84
		[Conditional("TRACE")]
		public static void WriteLine(object value, string category)
		{
			TraceInternal.WriteLine(value, category);
		}

		// Token: 0x06002C71 RID: 11377 RVA: 0x000C7B8D File Offset: 0x000C5D8D
		[Conditional("TRACE")]
		public static void WriteIf(bool condition, string message)
		{
			TraceInternal.WriteIf(condition, message);
		}

		// Token: 0x06002C72 RID: 11378 RVA: 0x000C7B96 File Offset: 0x000C5D96
		[Conditional("TRACE")]
		public static void WriteIf(bool condition, object value)
		{
			TraceInternal.WriteIf(condition, value);
		}

		// Token: 0x06002C73 RID: 11379 RVA: 0x000C7B9F File Offset: 0x000C5D9F
		[Conditional("TRACE")]
		public static void WriteIf(bool condition, string message, string category)
		{
			TraceInternal.WriteIf(condition, message, category);
		}

		// Token: 0x06002C74 RID: 11380 RVA: 0x000C7BA9 File Offset: 0x000C5DA9
		[Conditional("TRACE")]
		public static void WriteIf(bool condition, object value, string category)
		{
			TraceInternal.WriteIf(condition, value, category);
		}

		// Token: 0x06002C75 RID: 11381 RVA: 0x000C7BB3 File Offset: 0x000C5DB3
		[Conditional("TRACE")]
		public static void WriteLineIf(bool condition, string message)
		{
			TraceInternal.WriteLineIf(condition, message);
		}

		// Token: 0x06002C76 RID: 11382 RVA: 0x000C7BBC File Offset: 0x000C5DBC
		[Conditional("TRACE")]
		public static void WriteLineIf(bool condition, object value)
		{
			TraceInternal.WriteLineIf(condition, value);
		}

		// Token: 0x06002C77 RID: 11383 RVA: 0x000C7BC5 File Offset: 0x000C5DC5
		[Conditional("TRACE")]
		public static void WriteLineIf(bool condition, string message, string category)
		{
			TraceInternal.WriteLineIf(condition, message, category);
		}

		// Token: 0x06002C78 RID: 11384 RVA: 0x000C7BCF File Offset: 0x000C5DCF
		[Conditional("TRACE")]
		public static void WriteLineIf(bool condition, object value, string category)
		{
			TraceInternal.WriteLineIf(condition, value, category);
		}

		// Token: 0x06002C79 RID: 11385 RVA: 0x000C7BD9 File Offset: 0x000C5DD9
		[Conditional("TRACE")]
		public static void Indent()
		{
			TraceInternal.Indent();
		}

		// Token: 0x06002C7A RID: 11386 RVA: 0x000C7BE0 File Offset: 0x000C5DE0
		[Conditional("TRACE")]
		public static void Unindent()
		{
			TraceInternal.Unindent();
		}

		// Token: 0x040026C1 RID: 9921
		private static volatile CorrelationManager correlationManager;
	}
}
