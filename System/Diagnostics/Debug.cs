using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x02000496 RID: 1174
	[global::__DynamicallyInvokable]
	public static class Debug
	{
		// Token: 0x17000A7D RID: 2685
		// (get) Token: 0x06002B66 RID: 11110 RVA: 0x000C4F2A File Offset: 0x000C312A
		public static TraceListenerCollection Listeners
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
			get
			{
				return TraceInternal.Listeners;
			}
		}

		// Token: 0x17000A7E RID: 2686
		// (get) Token: 0x06002B67 RID: 11111 RVA: 0x000C4F31 File Offset: 0x000C3131
		// (set) Token: 0x06002B68 RID: 11112 RVA: 0x000C4F38 File Offset: 0x000C3138
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

		// Token: 0x17000A7F RID: 2687
		// (get) Token: 0x06002B69 RID: 11113 RVA: 0x000C4F40 File Offset: 0x000C3140
		// (set) Token: 0x06002B6A RID: 11114 RVA: 0x000C4F47 File Offset: 0x000C3147
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

		// Token: 0x17000A80 RID: 2688
		// (get) Token: 0x06002B6B RID: 11115 RVA: 0x000C4F4F File Offset: 0x000C314F
		// (set) Token: 0x06002B6C RID: 11116 RVA: 0x000C4F56 File Offset: 0x000C3156
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

		// Token: 0x06002B6D RID: 11117 RVA: 0x000C4F5E File Offset: 0x000C315E
		[Conditional("DEBUG")]
		public static void Flush()
		{
			TraceInternal.Flush();
		}

		// Token: 0x06002B6E RID: 11118 RVA: 0x000C4F65 File Offset: 0x000C3165
		[Conditional("DEBUG")]
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static void Close()
		{
			TraceInternal.Close();
		}

		// Token: 0x06002B6F RID: 11119 RVA: 0x000C4F6C File Offset: 0x000C316C
		[Conditional("DEBUG")]
		[global::__DynamicallyInvokable]
		public static void Assert(bool condition)
		{
			TraceInternal.Assert(condition);
		}

		// Token: 0x06002B70 RID: 11120 RVA: 0x000C4F74 File Offset: 0x000C3174
		[Conditional("DEBUG")]
		[global::__DynamicallyInvokable]
		public static void Assert(bool condition, string message)
		{
			TraceInternal.Assert(condition, message);
		}

		// Token: 0x06002B71 RID: 11121 RVA: 0x000C4F7D File Offset: 0x000C317D
		[Conditional("DEBUG")]
		[global::__DynamicallyInvokable]
		public static void Assert(bool condition, string message, string detailMessage)
		{
			TraceInternal.Assert(condition, message, detailMessage);
		}

		// Token: 0x06002B72 RID: 11122 RVA: 0x000C4F87 File Offset: 0x000C3187
		[Conditional("DEBUG")]
		[global::__DynamicallyInvokable]
		public static void Assert(bool condition, string message, string detailMessageFormat, params object[] args)
		{
			TraceInternal.Assert(condition, message, string.Format(CultureInfo.InvariantCulture, detailMessageFormat, args));
		}

		// Token: 0x06002B73 RID: 11123 RVA: 0x000C4F9C File Offset: 0x000C319C
		[Conditional("DEBUG")]
		[global::__DynamicallyInvokable]
		public static void Fail(string message)
		{
			TraceInternal.Fail(message);
		}

		// Token: 0x06002B74 RID: 11124 RVA: 0x000C4FA4 File Offset: 0x000C31A4
		[Conditional("DEBUG")]
		[global::__DynamicallyInvokable]
		public static void Fail(string message, string detailMessage)
		{
			TraceInternal.Fail(message, detailMessage);
		}

		// Token: 0x06002B75 RID: 11125 RVA: 0x000C4FAD File Offset: 0x000C31AD
		[Conditional("DEBUG")]
		public static void Print(string message)
		{
			TraceInternal.WriteLine(message);
		}

		// Token: 0x06002B76 RID: 11126 RVA: 0x000C4FB5 File Offset: 0x000C31B5
		[Conditional("DEBUG")]
		public static void Print(string format, params object[] args)
		{
			TraceInternal.WriteLine(string.Format(CultureInfo.InvariantCulture, format, args));
		}

		// Token: 0x06002B77 RID: 11127 RVA: 0x000C4FC8 File Offset: 0x000C31C8
		[Conditional("DEBUG")]
		[global::__DynamicallyInvokable]
		public static void Write(string message)
		{
			TraceInternal.Write(message);
		}

		// Token: 0x06002B78 RID: 11128 RVA: 0x000C4FD0 File Offset: 0x000C31D0
		[Conditional("DEBUG")]
		[global::__DynamicallyInvokable]
		public static void Write(object value)
		{
			TraceInternal.Write(value);
		}

		// Token: 0x06002B79 RID: 11129 RVA: 0x000C4FD8 File Offset: 0x000C31D8
		[Conditional("DEBUG")]
		[global::__DynamicallyInvokable]
		public static void Write(string message, string category)
		{
			TraceInternal.Write(message, category);
		}

		// Token: 0x06002B7A RID: 11130 RVA: 0x000C4FE1 File Offset: 0x000C31E1
		[Conditional("DEBUG")]
		[global::__DynamicallyInvokable]
		public static void Write(object value, string category)
		{
			TraceInternal.Write(value, category);
		}

		// Token: 0x06002B7B RID: 11131 RVA: 0x000C4FEA File Offset: 0x000C31EA
		[Conditional("DEBUG")]
		[global::__DynamicallyInvokable]
		public static void WriteLine(string message)
		{
			TraceInternal.WriteLine(message);
		}

		// Token: 0x06002B7C RID: 11132 RVA: 0x000C4FF2 File Offset: 0x000C31F2
		[Conditional("DEBUG")]
		[global::__DynamicallyInvokable]
		public static void WriteLine(object value)
		{
			TraceInternal.WriteLine(value);
		}

		// Token: 0x06002B7D RID: 11133 RVA: 0x000C4FFA File Offset: 0x000C31FA
		[Conditional("DEBUG")]
		[global::__DynamicallyInvokable]
		public static void WriteLine(string message, string category)
		{
			TraceInternal.WriteLine(message, category);
		}

		// Token: 0x06002B7E RID: 11134 RVA: 0x000C5003 File Offset: 0x000C3203
		[Conditional("DEBUG")]
		[global::__DynamicallyInvokable]
		public static void WriteLine(object value, string category)
		{
			TraceInternal.WriteLine(value, category);
		}

		// Token: 0x06002B7F RID: 11135 RVA: 0x000C500C File Offset: 0x000C320C
		[Conditional("DEBUG")]
		[global::__DynamicallyInvokable]
		public static void WriteLine(string format, params object[] args)
		{
			TraceInternal.WriteLine(string.Format(CultureInfo.InvariantCulture, format, args));
		}

		// Token: 0x06002B80 RID: 11136 RVA: 0x000C501F File Offset: 0x000C321F
		[Conditional("DEBUG")]
		[global::__DynamicallyInvokable]
		public static void WriteIf(bool condition, string message)
		{
			TraceInternal.WriteIf(condition, message);
		}

		// Token: 0x06002B81 RID: 11137 RVA: 0x000C5028 File Offset: 0x000C3228
		[Conditional("DEBUG")]
		[global::__DynamicallyInvokable]
		public static void WriteIf(bool condition, object value)
		{
			TraceInternal.WriteIf(condition, value);
		}

		// Token: 0x06002B82 RID: 11138 RVA: 0x000C5031 File Offset: 0x000C3231
		[Conditional("DEBUG")]
		[global::__DynamicallyInvokable]
		public static void WriteIf(bool condition, string message, string category)
		{
			TraceInternal.WriteIf(condition, message, category);
		}

		// Token: 0x06002B83 RID: 11139 RVA: 0x000C503B File Offset: 0x000C323B
		[Conditional("DEBUG")]
		[global::__DynamicallyInvokable]
		public static void WriteIf(bool condition, object value, string category)
		{
			TraceInternal.WriteIf(condition, value, category);
		}

		// Token: 0x06002B84 RID: 11140 RVA: 0x000C5045 File Offset: 0x000C3245
		[Conditional("DEBUG")]
		[global::__DynamicallyInvokable]
		public static void WriteLineIf(bool condition, string message)
		{
			TraceInternal.WriteLineIf(condition, message);
		}

		// Token: 0x06002B85 RID: 11141 RVA: 0x000C504E File Offset: 0x000C324E
		[Conditional("DEBUG")]
		[global::__DynamicallyInvokable]
		public static void WriteLineIf(bool condition, object value)
		{
			TraceInternal.WriteLineIf(condition, value);
		}

		// Token: 0x06002B86 RID: 11142 RVA: 0x000C5057 File Offset: 0x000C3257
		[Conditional("DEBUG")]
		[global::__DynamicallyInvokable]
		public static void WriteLineIf(bool condition, string message, string category)
		{
			TraceInternal.WriteLineIf(condition, message, category);
		}

		// Token: 0x06002B87 RID: 11143 RVA: 0x000C5061 File Offset: 0x000C3261
		[Conditional("DEBUG")]
		[global::__DynamicallyInvokable]
		public static void WriteLineIf(bool condition, object value, string category)
		{
			TraceInternal.WriteLineIf(condition, value, category);
		}

		// Token: 0x06002B88 RID: 11144 RVA: 0x000C506B File Offset: 0x000C326B
		[Conditional("DEBUG")]
		public static void Indent()
		{
			TraceInternal.Indent();
		}

		// Token: 0x06002B89 RID: 11145 RVA: 0x000C5072 File Offset: 0x000C3272
		[Conditional("DEBUG")]
		public static void Unindent()
		{
			TraceInternal.Unindent();
		}
	}
}
