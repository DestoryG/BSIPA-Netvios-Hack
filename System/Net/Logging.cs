using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

namespace System.Net
{
	// Token: 0x02000151 RID: 337
	[FriendAccessAllowed]
	internal class Logging
	{
		// Token: 0x06000BB0 RID: 2992 RVA: 0x0003F5A5 File Offset: 0x0003D7A5
		private Logging()
		{
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000BB1 RID: 2993 RVA: 0x0003F5B0 File Offset: 0x0003D7B0
		private static object InternalSyncObject
		{
			get
			{
				if (Logging.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref Logging.s_InternalSyncObject, obj, null);
				}
				return Logging.s_InternalSyncObject;
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000BB2 RID: 2994 RVA: 0x0003F5DC File Offset: 0x0003D7DC
		[FriendAccessAllowed]
		internal static bool On
		{
			get
			{
				if (!Logging.s_LoggingInitialized)
				{
					Logging.InitializeLogging();
				}
				return Logging.s_LoggingEnabled;
			}
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x0003F5F3 File Offset: 0x0003D7F3
		internal static bool IsVerbose(TraceSource traceSource)
		{
			return Logging.ValidateSettings(traceSource, TraceEventType.Verbose);
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000BB4 RID: 2996 RVA: 0x0003F5FD File Offset: 0x0003D7FD
		internal static TraceSource Web
		{
			get
			{
				if (!Logging.s_LoggingInitialized)
				{
					Logging.InitializeLogging();
				}
				if (!Logging.s_LoggingEnabled)
				{
					return null;
				}
				return Logging.s_WebTraceSource;
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000BB5 RID: 2997 RVA: 0x0003F61D File Offset: 0x0003D81D
		[FriendAccessAllowed]
		internal static TraceSource Http
		{
			get
			{
				if (!Logging.s_LoggingInitialized)
				{
					Logging.InitializeLogging();
				}
				if (!Logging.s_LoggingEnabled)
				{
					return null;
				}
				return Logging.s_TraceSourceHttpName;
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000BB6 RID: 2998 RVA: 0x0003F63D File Offset: 0x0003D83D
		internal static TraceSource HttpListener
		{
			get
			{
				if (!Logging.s_LoggingInitialized)
				{
					Logging.InitializeLogging();
				}
				if (!Logging.s_LoggingEnabled)
				{
					return null;
				}
				return Logging.s_HttpListenerTraceSource;
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000BB7 RID: 2999 RVA: 0x0003F65D File Offset: 0x0003D85D
		internal static TraceSource Sockets
		{
			get
			{
				if (!Logging.s_LoggingInitialized)
				{
					Logging.InitializeLogging();
				}
				if (!Logging.s_LoggingEnabled)
				{
					return null;
				}
				return Logging.s_SocketsTraceSource;
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000BB8 RID: 3000 RVA: 0x0003F67D File Offset: 0x0003D87D
		internal static TraceSource RequestCache
		{
			get
			{
				if (!Logging.s_LoggingInitialized)
				{
					Logging.InitializeLogging();
				}
				if (!Logging.s_LoggingEnabled)
				{
					return null;
				}
				return Logging.s_CacheTraceSource;
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000BB9 RID: 3001 RVA: 0x0003F69D File Offset: 0x0003D89D
		internal static TraceSource WebSockets
		{
			get
			{
				if (!Logging.s_LoggingInitialized)
				{
					Logging.InitializeLogging();
				}
				if (!Logging.s_LoggingEnabled)
				{
					return null;
				}
				return Logging.s_WebSocketsTraceSource;
			}
		}

		// Token: 0x06000BBA RID: 3002 RVA: 0x0003F6C0 File Offset: 0x0003D8C0
		private static bool GetUseProtocolTextSetting(TraceSource traceSource)
		{
			bool flag = false;
			if (traceSource.Attributes["tracemode"] == "protocolonly")
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x06000BBB RID: 3003 RVA: 0x0003F6F0 File Offset: 0x0003D8F0
		private static int GetMaxDumpSizeSetting(TraceSource traceSource)
		{
			int num = 1024;
			if (traceSource.Attributes.ContainsKey("maxdatasize"))
			{
				try
				{
					num = int.Parse(traceSource.Attributes["maxdatasize"], NumberFormatInfo.InvariantInfo);
				}
				catch (Exception ex)
				{
					if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
					{
						throw;
					}
					traceSource.Attributes["maxdatasize"] = num.ToString(NumberFormatInfo.InvariantInfo);
				}
			}
			return num;
		}

		// Token: 0x06000BBC RID: 3004 RVA: 0x0003F77C File Offset: 0x0003D97C
		private static void InitializeLogging()
		{
			object internalSyncObject = Logging.InternalSyncObject;
			lock (internalSyncObject)
			{
				if (!Logging.s_LoggingInitialized)
				{
					bool flag2 = false;
					Logging.s_WebTraceSource = new Logging.NclTraceSource("System.Net");
					Logging.s_HttpListenerTraceSource = new Logging.NclTraceSource("System.Net.HttpListener");
					Logging.s_SocketsTraceSource = new Logging.NclTraceSource("System.Net.Sockets");
					Logging.s_WebSocketsTraceSource = new Logging.NclTraceSource("System.Net.WebSockets");
					Logging.s_CacheTraceSource = new Logging.NclTraceSource("System.Net.Cache");
					Logging.s_TraceSourceHttpName = new Logging.NclTraceSource("System.Net.Http");
					try
					{
						flag2 = Logging.s_WebTraceSource.Switch.ShouldTrace(TraceEventType.Critical) || Logging.s_HttpListenerTraceSource.Switch.ShouldTrace(TraceEventType.Critical) || Logging.s_SocketsTraceSource.Switch.ShouldTrace(TraceEventType.Critical) || Logging.s_WebSocketsTraceSource.Switch.ShouldTrace(TraceEventType.Critical) || Logging.s_CacheTraceSource.Switch.ShouldTrace(TraceEventType.Critical) || Logging.s_TraceSourceHttpName.Switch.ShouldTrace(TraceEventType.Critical);
					}
					catch (SecurityException)
					{
						Logging.Close();
						flag2 = false;
					}
					if (flag2)
					{
						AppDomain currentDomain = AppDomain.CurrentDomain;
						currentDomain.UnhandledException += Logging.UnhandledExceptionHandler;
						currentDomain.DomainUnload += Logging.AppDomainUnloadEvent;
						currentDomain.ProcessExit += Logging.ProcessExitEvent;
					}
					Logging.s_LoggingEnabled = flag2;
					Logging.s_LoggingInitialized = true;
				}
			}
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x0003F90C File Offset: 0x0003DB0C
		private static void Close()
		{
			if (Logging.s_WebTraceSource != null)
			{
				Logging.s_WebTraceSource.Close();
			}
			if (Logging.s_HttpListenerTraceSource != null)
			{
				Logging.s_HttpListenerTraceSource.Close();
			}
			if (Logging.s_SocketsTraceSource != null)
			{
				Logging.s_SocketsTraceSource.Close();
			}
			if (Logging.s_WebSocketsTraceSource != null)
			{
				Logging.s_WebSocketsTraceSource.Close();
			}
			if (Logging.s_CacheTraceSource != null)
			{
				Logging.s_CacheTraceSource.Close();
			}
			if (Logging.s_TraceSourceHttpName != null)
			{
				Logging.s_TraceSourceHttpName.Close();
			}
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x0003F980 File Offset: 0x0003DB80
		private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
		{
			Exception ex = (Exception)args.ExceptionObject;
			Logging.Exception(Logging.Web, sender, "UnhandledExceptionHandler", ex);
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x0003F9AA File Offset: 0x0003DBAA
		private static void ProcessExitEvent(object sender, EventArgs e)
		{
			Logging.Close();
			Logging.s_AppDomainShutdown = true;
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x0003F9B9 File Offset: 0x0003DBB9
		private static void AppDomainUnloadEvent(object sender, EventArgs e)
		{
			Logging.Close();
			Logging.s_AppDomainShutdown = true;
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x0003F9C8 File Offset: 0x0003DBC8
		private static bool ValidateSettings(TraceSource traceSource, TraceEventType traceLevel)
		{
			if (!Logging.s_LoggingEnabled)
			{
				return false;
			}
			if (!Logging.s_LoggingInitialized)
			{
				Logging.InitializeLogging();
			}
			return traceSource != null && traceSource.Switch.ShouldTrace(traceLevel) && !Logging.s_AppDomainShutdown;
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x0003FA02 File Offset: 0x0003DC02
		private static string GetObjectName(object obj)
		{
			if (obj is Uri || obj is IPAddress || obj is IPEndPoint)
			{
				return obj.ToString();
			}
			return obj.GetType().Name;
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x0003FA30 File Offset: 0x0003DC30
		internal static uint GetThreadId()
		{
			uint num = UnsafeNclNativeMethods.GetCurrentThreadId();
			if (num == 0U)
			{
				num = (uint)Thread.CurrentThread.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x0003FA54 File Offset: 0x0003DC54
		internal static void PrintLine(TraceSource traceSource, TraceEventType eventType, int id, string msg)
		{
			string text = "[" + Logging.GetThreadId().ToString("d4", CultureInfo.InvariantCulture) + "] ";
			traceSource.TraceEvent(eventType, id, text + msg);
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x0003FA98 File Offset: 0x0003DC98
		[FriendAccessAllowed]
		internal static void Associate(TraceSource traceSource, object objA, object objB)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			string text = Logging.GetObjectName(objA) + "#" + ValidationHelper.HashString(objA);
			string text2 = Logging.GetObjectName(objB) + "#" + ValidationHelper.HashString(objB);
			Logging.PrintLine(traceSource, TraceEventType.Information, 0, "Associating " + text + " with " + text2);
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x0003FAF6 File Offset: 0x0003DCF6
		[FriendAccessAllowed]
		internal static void Enter(TraceSource traceSource, object obj, string method, string param)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			Logging.Enter(traceSource, Logging.GetObjectName(obj) + "#" + ValidationHelper.HashString(obj), method, param);
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x0003FB20 File Offset: 0x0003DD20
		[FriendAccessAllowed]
		internal static void Enter(TraceSource traceSource, object obj, string method, object paramObject)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			Logging.Enter(traceSource, Logging.GetObjectName(obj) + "#" + ValidationHelper.HashString(obj), method, paramObject);
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x0003FB4C File Offset: 0x0003DD4C
		internal static void Enter(TraceSource traceSource, string obj, string method, string param)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			Logging.Enter(traceSource, string.Concat(new string[] { obj, "::", method, "(", param, ")" }));
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x0003FB98 File Offset: 0x0003DD98
		internal static void Enter(TraceSource traceSource, string obj, string method, object paramObject)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			string text = "";
			if (paramObject != null)
			{
				text = Logging.GetObjectName(paramObject) + "#" + ValidationHelper.HashString(paramObject);
			}
			Logging.Enter(traceSource, string.Concat(new string[] { obj, "::", method, "(", text, ")" }));
		}

		// Token: 0x06000BCA RID: 3018 RVA: 0x0003FC04 File Offset: 0x0003DE04
		internal static void Enter(TraceSource traceSource, string method, string parameters)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			Logging.Enter(traceSource, method + "(" + parameters + ")");
		}

		// Token: 0x06000BCB RID: 3019 RVA: 0x0003FC27 File Offset: 0x0003DE27
		internal static void Enter(TraceSource traceSource, string msg)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			Logging.PrintLine(traceSource, TraceEventType.Verbose, 0, "Entering " + msg);
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x0003FC48 File Offset: 0x0003DE48
		[FriendAccessAllowed]
		internal static void Exit(TraceSource traceSource, object obj, string method, object retObject)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			string text = "";
			if (retObject != null)
			{
				text = Logging.GetObjectName(retObject) + "#" + ValidationHelper.HashString(retObject);
			}
			Logging.Exit(traceSource, obj, method, text);
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x0003FC88 File Offset: 0x0003DE88
		internal static void Exit(TraceSource traceSource, string obj, string method, object retObject)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			string text = "";
			if (retObject != null)
			{
				text = Logging.GetObjectName(retObject) + "#" + ValidationHelper.HashString(retObject);
			}
			Logging.Exit(traceSource, obj, method, text);
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x0003FCC8 File Offset: 0x0003DEC8
		[FriendAccessAllowed]
		internal static void Exit(TraceSource traceSource, object obj, string method, string retValue)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			Logging.Exit(traceSource, Logging.GetObjectName(obj) + "#" + ValidationHelper.HashString(obj), method, retValue);
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x0003FCF4 File Offset: 0x0003DEF4
		internal static void Exit(TraceSource traceSource, string obj, string method, string retValue)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			if (!ValidationHelper.IsBlankString(retValue))
			{
				retValue = "\t-> " + retValue;
			}
			Logging.Exit(traceSource, string.Concat(new string[] { obj, "::", method, "() ", retValue }));
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x0003FD4D File Offset: 0x0003DF4D
		internal static void Exit(TraceSource traceSource, string method, string parameters)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			Logging.Exit(traceSource, method + "() " + parameters);
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x0003FD6B File Offset: 0x0003DF6B
		internal static void Exit(TraceSource traceSource, string msg)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			Logging.PrintLine(traceSource, TraceEventType.Verbose, 0, "Exiting " + msg);
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x0003FD8C File Offset: 0x0003DF8C
		[FriendAccessAllowed]
		internal static void Exception(TraceSource traceSource, object obj, string method, Exception e)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Error))
			{
				return;
			}
			string text = SR.GetString("net_log_exception", new object[]
			{
				Logging.GetObjectLogHash(obj),
				method,
				e.Message
			});
			if (!ValidationHelper.IsBlankString(e.StackTrace))
			{
				text = text + "\r\n" + e.StackTrace;
			}
			Logging.PrintLine(traceSource, TraceEventType.Error, 0, text);
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x0003FDF2 File Offset: 0x0003DFF2
		internal static void PrintInfo(TraceSource traceSource, string msg)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			Logging.PrintLine(traceSource, TraceEventType.Information, 0, msg);
		}

		// Token: 0x06000BD4 RID: 3028 RVA: 0x0003FE08 File Offset: 0x0003E008
		[FriendAccessAllowed]
		internal static void PrintInfo(TraceSource traceSource, object obj, string msg)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			Logging.PrintLine(traceSource, TraceEventType.Information, 0, string.Concat(new string[]
			{
				Logging.GetObjectName(obj),
				"#",
				ValidationHelper.HashString(obj),
				" - ",
				msg
			}));
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x0003FE58 File Offset: 0x0003E058
		internal static void PrintInfo(TraceSource traceSource, object obj, string method, string param)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Information))
			{
				return;
			}
			Logging.PrintLine(traceSource, TraceEventType.Information, 0, string.Concat(new string[]
			{
				Logging.GetObjectName(obj),
				"#",
				ValidationHelper.HashString(obj),
				"::",
				method,
				"(",
				param,
				")"
			}));
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x0003FEBC File Offset: 0x0003E0BC
		[FriendAccessAllowed]
		internal static void PrintWarning(TraceSource traceSource, string msg)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Warning))
			{
				return;
			}
			Logging.PrintLine(traceSource, TraceEventType.Warning, 0, msg);
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x0003FED4 File Offset: 0x0003E0D4
		internal static void PrintWarning(TraceSource traceSource, object obj, string method, string msg)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Warning))
			{
				return;
			}
			Logging.PrintLine(traceSource, TraceEventType.Warning, 0, string.Concat(new string[]
			{
				Logging.GetObjectName(obj),
				"#",
				ValidationHelper.HashString(obj),
				"::",
				method,
				"() - ",
				msg
			}));
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x0003FF30 File Offset: 0x0003E130
		[FriendAccessAllowed]
		internal static void PrintError(TraceSource traceSource, string msg)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Error))
			{
				return;
			}
			Logging.PrintLine(traceSource, TraceEventType.Error, 0, msg);
		}

		// Token: 0x06000BD9 RID: 3033 RVA: 0x0003FF48 File Offset: 0x0003E148
		[FriendAccessAllowed]
		internal static void PrintError(TraceSource traceSource, object obj, string method, string msg)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Error))
			{
				return;
			}
			Logging.PrintLine(traceSource, TraceEventType.Error, 0, string.Concat(new string[]
			{
				Logging.GetObjectName(obj),
				"#",
				ValidationHelper.HashString(obj),
				"::",
				method,
				"() - ",
				msg
			}));
		}

		// Token: 0x06000BDA RID: 3034 RVA: 0x0003FFA4 File Offset: 0x0003E1A4
		[FriendAccessAllowed]
		internal static string GetObjectLogHash(object obj)
		{
			return Logging.GetObjectName(obj) + "#" + ValidationHelper.HashString(obj);
		}

		// Token: 0x06000BDB RID: 3035 RVA: 0x0003FFBC File Offset: 0x0003E1BC
		internal static void Dump(TraceSource traceSource, object obj, string method, IntPtr bufferPtr, int length)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Verbose) || bufferPtr == IntPtr.Zero || length < 0)
			{
				return;
			}
			byte[] array = new byte[length];
			Marshal.Copy(bufferPtr, array, 0, length);
			Logging.Dump(traceSource, obj, method, array, 0, length);
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x00040004 File Offset: 0x0003E204
		internal static void Dump(TraceSource traceSource, object obj, string method, byte[] buffer, int offset, int length)
		{
			if (!Logging.ValidateSettings(traceSource, TraceEventType.Verbose))
			{
				return;
			}
			if (buffer == null)
			{
				Logging.PrintLine(traceSource, TraceEventType.Verbose, 0, "(null)");
				return;
			}
			if (offset > buffer.Length)
			{
				Logging.PrintLine(traceSource, TraceEventType.Verbose, 0, "(offset out of range)");
				return;
			}
			Logging.PrintLine(traceSource, TraceEventType.Verbose, 0, string.Concat(new string[]
			{
				"Data from ",
				Logging.GetObjectName(obj),
				"#",
				ValidationHelper.HashString(obj),
				"::",
				method
			}));
			int maxDumpSizeSetting = Logging.GetMaxDumpSizeSetting(traceSource);
			if (length > maxDumpSizeSetting)
			{
				Logging.PrintLine(traceSource, TraceEventType.Verbose, 0, string.Concat(new string[]
				{
					"(printing ",
					maxDumpSizeSetting.ToString(NumberFormatInfo.InvariantInfo),
					" out of ",
					length.ToString(NumberFormatInfo.InvariantInfo),
					")"
				}));
				length = maxDumpSizeSetting;
			}
			if (length < 0 || length > buffer.Length - offset)
			{
				length = buffer.Length - offset;
			}
			if (Logging.GetUseProtocolTextSetting(traceSource))
			{
				string text = "<<" + WebHeaderCollection.HeaderEncoding.GetString(buffer, offset, length) + ">>";
				Logging.PrintLine(traceSource, TraceEventType.Verbose, 0, text);
				return;
			}
			do
			{
				int num = Math.Min(length, 16);
				string text2 = string.Format(CultureInfo.CurrentCulture, "{0:X8} : ", new object[] { offset });
				for (int i = 0; i < num; i++)
				{
					text2 = text2 + string.Format(CultureInfo.CurrentCulture, "{0:X2}", new object[] { buffer[offset + i] }) + ((i == 7) ? '-' : ' ').ToString();
				}
				for (int j = num; j < 16; j++)
				{
					text2 += "   ";
				}
				text2 += ": ";
				for (int k = 0; k < num; k++)
				{
					text2 += ((char)((buffer[offset + k] < 32 || buffer[offset + k] > 126) ? 46 : buffer[offset + k])).ToString();
				}
				Logging.PrintLine(traceSource, TraceEventType.Verbose, 0, text2);
				offset += num;
				length -= num;
			}
			while (length > 0);
		}

		// Token: 0x0400110C RID: 4364
		private static volatile bool s_LoggingEnabled = true;

		// Token: 0x0400110D RID: 4365
		private static volatile bool s_LoggingInitialized;

		// Token: 0x0400110E RID: 4366
		private static volatile bool s_AppDomainShutdown;

		// Token: 0x0400110F RID: 4367
		private const int DefaultMaxDumpSize = 1024;

		// Token: 0x04001110 RID: 4368
		private const bool DefaultUseProtocolTextOnly = false;

		// Token: 0x04001111 RID: 4369
		private const string AttributeNameMaxSize = "maxdatasize";

		// Token: 0x04001112 RID: 4370
		private const string AttributeNameTraceMode = "tracemode";

		// Token: 0x04001113 RID: 4371
		private static readonly string[] SupportedAttributes = new string[] { "maxdatasize", "tracemode" };

		// Token: 0x04001114 RID: 4372
		private const string AttributeValueProtocolOnly = "protocolonly";

		// Token: 0x04001115 RID: 4373
		private const string TraceSourceWebName = "System.Net";

		// Token: 0x04001116 RID: 4374
		private const string TraceSourceHttpListenerName = "System.Net.HttpListener";

		// Token: 0x04001117 RID: 4375
		private const string TraceSourceSocketsName = "System.Net.Sockets";

		// Token: 0x04001118 RID: 4376
		private const string TraceSourceWebSocketsName = "System.Net.WebSockets";

		// Token: 0x04001119 RID: 4377
		private const string TraceSourceCacheName = "System.Net.Cache";

		// Token: 0x0400111A RID: 4378
		private const string TraceSourceHttpName = "System.Net.Http";

		// Token: 0x0400111B RID: 4379
		private static TraceSource s_WebTraceSource;

		// Token: 0x0400111C RID: 4380
		private static TraceSource s_HttpListenerTraceSource;

		// Token: 0x0400111D RID: 4381
		private static TraceSource s_SocketsTraceSource;

		// Token: 0x0400111E RID: 4382
		private static TraceSource s_WebSocketsTraceSource;

		// Token: 0x0400111F RID: 4383
		private static TraceSource s_CacheTraceSource;

		// Token: 0x04001120 RID: 4384
		private static TraceSource s_TraceSourceHttpName;

		// Token: 0x04001121 RID: 4385
		private static object s_InternalSyncObject;

		// Token: 0x0200070E RID: 1806
		private class NclTraceSource : TraceSource
		{
			// Token: 0x0600409A RID: 16538 RVA: 0x0010EE03 File Offset: 0x0010D003
			internal NclTraceSource(string name)
				: base(name)
			{
			}

			// Token: 0x0600409B RID: 16539 RVA: 0x0010EE0C File Offset: 0x0010D00C
			protected internal override string[] GetSupportedAttributes()
			{
				return Logging.SupportedAttributes;
			}
		}
	}
}
