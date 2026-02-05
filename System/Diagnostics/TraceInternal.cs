using System;
using System.Collections;
using System.IO;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x020004B1 RID: 1201
	internal static class TraceInternal
	{
		// Token: 0x17000ACC RID: 2764
		// (get) Token: 0x06002C8C RID: 11404 RVA: 0x000C7D98 File Offset: 0x000C5F98
		public static TraceListenerCollection Listeners
		{
			get
			{
				TraceInternal.InitializeSettings();
				if (TraceInternal.listeners == null)
				{
					object obj = TraceInternal.critSec;
					lock (obj)
					{
						if (TraceInternal.listeners == null)
						{
							SystemDiagnosticsSection systemDiagnosticsSection = DiagnosticsConfiguration.SystemDiagnosticsSection;
							if (systemDiagnosticsSection != null)
							{
								TraceInternal.listeners = systemDiagnosticsSection.Trace.Listeners.GetRuntimeObject();
							}
							else
							{
								TraceInternal.listeners = new TraceListenerCollection();
								TraceListener traceListener = new DefaultTraceListener();
								traceListener.IndentLevel = TraceInternal.indentLevel;
								traceListener.IndentSize = TraceInternal.indentSize;
								TraceInternal.listeners.Add(traceListener);
							}
						}
					}
				}
				return TraceInternal.listeners;
			}
		}

		// Token: 0x17000ACD RID: 2765
		// (get) Token: 0x06002C8D RID: 11405 RVA: 0x000C7E4C File Offset: 0x000C604C
		internal static string AppName
		{
			get
			{
				if (TraceInternal.appName == null)
				{
					new EnvironmentPermission(EnvironmentPermissionAccess.Read, "Path").Assert();
					TraceInternal.appName = Path.GetFileName(Environment.GetCommandLineArgs()[0]);
				}
				return TraceInternal.appName;
			}
		}

		// Token: 0x17000ACE RID: 2766
		// (get) Token: 0x06002C8E RID: 11406 RVA: 0x000C7E81 File Offset: 0x000C6081
		// (set) Token: 0x06002C8F RID: 11407 RVA: 0x000C7E8F File Offset: 0x000C608F
		public static bool AutoFlush
		{
			get
			{
				TraceInternal.InitializeSettings();
				return TraceInternal.autoFlush;
			}
			set
			{
				TraceInternal.InitializeSettings();
				TraceInternal.autoFlush = value;
			}
		}

		// Token: 0x17000ACF RID: 2767
		// (get) Token: 0x06002C90 RID: 11408 RVA: 0x000C7E9E File Offset: 0x000C609E
		// (set) Token: 0x06002C91 RID: 11409 RVA: 0x000C7EAC File Offset: 0x000C60AC
		public static bool UseGlobalLock
		{
			get
			{
				TraceInternal.InitializeSettings();
				return TraceInternal.useGlobalLock;
			}
			set
			{
				TraceInternal.InitializeSettings();
				TraceInternal.useGlobalLock = value;
			}
		}

		// Token: 0x17000AD0 RID: 2768
		// (get) Token: 0x06002C92 RID: 11410 RVA: 0x000C7EBB File Offset: 0x000C60BB
		// (set) Token: 0x06002C93 RID: 11411 RVA: 0x000C7EC4 File Offset: 0x000C60C4
		public static int IndentLevel
		{
			get
			{
				return TraceInternal.indentLevel;
			}
			set
			{
				object obj = TraceInternal.critSec;
				lock (obj)
				{
					if (value < 0)
					{
						value = 0;
					}
					TraceInternal.indentLevel = value;
					if (TraceInternal.listeners != null)
					{
						foreach (object obj2 in TraceInternal.Listeners)
						{
							TraceListener traceListener = (TraceListener)obj2;
							traceListener.IndentLevel = TraceInternal.indentLevel;
						}
					}
				}
			}
		}

		// Token: 0x17000AD1 RID: 2769
		// (get) Token: 0x06002C94 RID: 11412 RVA: 0x000C7F64 File Offset: 0x000C6164
		// (set) Token: 0x06002C95 RID: 11413 RVA: 0x000C7F72 File Offset: 0x000C6172
		public static int IndentSize
		{
			get
			{
				TraceInternal.InitializeSettings();
				return TraceInternal.indentSize;
			}
			set
			{
				TraceInternal.InitializeSettings();
				TraceInternal.SetIndentSize(value);
			}
		}

		// Token: 0x06002C96 RID: 11414 RVA: 0x000C7F80 File Offset: 0x000C6180
		private static void SetIndentSize(int value)
		{
			object obj = TraceInternal.critSec;
			lock (obj)
			{
				if (value < 0)
				{
					value = 0;
				}
				TraceInternal.indentSize = value;
				if (TraceInternal.listeners != null)
				{
					foreach (object obj2 in TraceInternal.Listeners)
					{
						TraceListener traceListener = (TraceListener)obj2;
						traceListener.IndentSize = TraceInternal.indentSize;
					}
				}
			}
		}

		// Token: 0x06002C97 RID: 11415 RVA: 0x000C8024 File Offset: 0x000C6224
		public static void Indent()
		{
			object obj = TraceInternal.critSec;
			lock (obj)
			{
				TraceInternal.InitializeSettings();
				if (TraceInternal.indentLevel < 2147483647)
				{
					TraceInternal.indentLevel++;
				}
				foreach (object obj2 in TraceInternal.Listeners)
				{
					TraceListener traceListener = (TraceListener)obj2;
					traceListener.IndentLevel = TraceInternal.indentLevel;
				}
			}
		}

		// Token: 0x06002C98 RID: 11416 RVA: 0x000C80C8 File Offset: 0x000C62C8
		public static void Unindent()
		{
			object obj = TraceInternal.critSec;
			lock (obj)
			{
				TraceInternal.InitializeSettings();
				if (TraceInternal.indentLevel > 0)
				{
					TraceInternal.indentLevel--;
				}
				foreach (object obj2 in TraceInternal.Listeners)
				{
					TraceListener traceListener = (TraceListener)obj2;
					traceListener.IndentLevel = TraceInternal.indentLevel;
				}
			}
		}

		// Token: 0x06002C99 RID: 11417 RVA: 0x000C8168 File Offset: 0x000C6368
		public static void Flush()
		{
			if (TraceInternal.listeners != null)
			{
				if (TraceInternal.UseGlobalLock)
				{
					object obj = TraceInternal.critSec;
					lock (obj)
					{
						using (IEnumerator enumerator = TraceInternal.Listeners.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								object obj2 = enumerator.Current;
								TraceListener traceListener = (TraceListener)obj2;
								traceListener.Flush();
							}
							return;
						}
					}
				}
				foreach (object obj3 in TraceInternal.Listeners)
				{
					TraceListener traceListener2 = (TraceListener)obj3;
					if (!traceListener2.IsThreadSafe)
					{
						TraceListener traceListener3 = traceListener2;
						lock (traceListener3)
						{
							traceListener2.Flush();
							continue;
						}
					}
					traceListener2.Flush();
				}
			}
		}

		// Token: 0x06002C9A RID: 11418 RVA: 0x000C8288 File Offset: 0x000C6488
		public static void Close()
		{
			if (TraceInternal.listeners != null)
			{
				object obj = TraceInternal.critSec;
				lock (obj)
				{
					foreach (object obj2 in TraceInternal.Listeners)
					{
						TraceListener traceListener = (TraceListener)obj2;
						traceListener.Close();
					}
				}
			}
		}

		// Token: 0x06002C9B RID: 11419 RVA: 0x000C8314 File Offset: 0x000C6514
		public static void Assert(bool condition)
		{
			if (condition)
			{
				return;
			}
			TraceInternal.Fail(string.Empty);
		}

		// Token: 0x06002C9C RID: 11420 RVA: 0x000C8324 File Offset: 0x000C6524
		public static void Assert(bool condition, string message)
		{
			if (condition)
			{
				return;
			}
			TraceInternal.Fail(message);
		}

		// Token: 0x06002C9D RID: 11421 RVA: 0x000C8330 File Offset: 0x000C6530
		public static void Assert(bool condition, string message, string detailMessage)
		{
			if (condition)
			{
				return;
			}
			TraceInternal.Fail(message, detailMessage);
		}

		// Token: 0x06002C9E RID: 11422 RVA: 0x000C8340 File Offset: 0x000C6540
		public static void Fail(string message)
		{
			if (TraceInternal.UseGlobalLock)
			{
				object obj = TraceInternal.critSec;
				lock (obj)
				{
					using (IEnumerator enumerator = TraceInternal.Listeners.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj2 = enumerator.Current;
							TraceListener traceListener = (TraceListener)obj2;
							traceListener.Fail(message);
							if (TraceInternal.AutoFlush)
							{
								traceListener.Flush();
							}
						}
						return;
					}
				}
			}
			foreach (object obj3 in TraceInternal.Listeners)
			{
				TraceListener traceListener2 = (TraceListener)obj3;
				if (!traceListener2.IsThreadSafe)
				{
					TraceListener traceListener3 = traceListener2;
					lock (traceListener3)
					{
						traceListener2.Fail(message);
						if (TraceInternal.AutoFlush)
						{
							traceListener2.Flush();
						}
						continue;
					}
				}
				traceListener2.Fail(message);
				if (TraceInternal.AutoFlush)
				{
					traceListener2.Flush();
				}
			}
		}

		// Token: 0x06002C9F RID: 11423 RVA: 0x000C8480 File Offset: 0x000C6680
		public static void Fail(string message, string detailMessage)
		{
			if (TraceInternal.UseGlobalLock)
			{
				object obj = TraceInternal.critSec;
				lock (obj)
				{
					using (IEnumerator enumerator = TraceInternal.Listeners.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj2 = enumerator.Current;
							TraceListener traceListener = (TraceListener)obj2;
							traceListener.Fail(message, detailMessage);
							if (TraceInternal.AutoFlush)
							{
								traceListener.Flush();
							}
						}
						return;
					}
				}
			}
			foreach (object obj3 in TraceInternal.Listeners)
			{
				TraceListener traceListener2 = (TraceListener)obj3;
				if (!traceListener2.IsThreadSafe)
				{
					TraceListener traceListener3 = traceListener2;
					lock (traceListener3)
					{
						traceListener2.Fail(message, detailMessage);
						if (TraceInternal.AutoFlush)
						{
							traceListener2.Flush();
						}
						continue;
					}
				}
				traceListener2.Fail(message, detailMessage);
				if (TraceInternal.AutoFlush)
				{
					traceListener2.Flush();
				}
			}
		}

		// Token: 0x06002CA0 RID: 11424 RVA: 0x000C85C4 File Offset: 0x000C67C4
		private static void InitializeSettings()
		{
			if (!TraceInternal.settingsInitialized || (TraceInternal.defaultInitialized && DiagnosticsConfiguration.IsInitialized()))
			{
				object obj = TraceInternal.critSec;
				lock (obj)
				{
					if (!TraceInternal.settingsInitialized || (TraceInternal.defaultInitialized && DiagnosticsConfiguration.IsInitialized()))
					{
						TraceInternal.defaultInitialized = DiagnosticsConfiguration.IsInitializing();
						TraceInternal.SetIndentSize(DiagnosticsConfiguration.IndentSize);
						TraceInternal.autoFlush = DiagnosticsConfiguration.AutoFlush;
						TraceInternal.useGlobalLock = DiagnosticsConfiguration.UseGlobalLock;
						TraceInternal.settingsInitialized = true;
					}
				}
			}
		}

		// Token: 0x06002CA1 RID: 11425 RVA: 0x000C8668 File Offset: 0x000C6868
		internal static void Refresh()
		{
			object obj = TraceInternal.critSec;
			lock (obj)
			{
				TraceInternal.settingsInitialized = false;
				TraceInternal.listeners = null;
			}
			TraceInternal.InitializeSettings();
		}

		// Token: 0x06002CA2 RID: 11426 RVA: 0x000C86B8 File Offset: 0x000C68B8
		public static void TraceEvent(TraceEventType eventType, int id, string format, params object[] args)
		{
			TraceEventCache traceEventCache = new TraceEventCache();
			if (TraceInternal.UseGlobalLock)
			{
				object obj = TraceInternal.critSec;
				lock (obj)
				{
					if (args == null)
					{
						using (IEnumerator enumerator = TraceInternal.Listeners.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								object obj2 = enumerator.Current;
								TraceListener traceListener = (TraceListener)obj2;
								traceListener.TraceEvent(traceEventCache, TraceInternal.AppName, eventType, id, format);
								if (TraceInternal.AutoFlush)
								{
									traceListener.Flush();
								}
							}
							return;
						}
					}
					using (IEnumerator enumerator2 = TraceInternal.Listeners.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							object obj3 = enumerator2.Current;
							TraceListener traceListener2 = (TraceListener)obj3;
							traceListener2.TraceEvent(traceEventCache, TraceInternal.AppName, eventType, id, format, args);
							if (TraceInternal.AutoFlush)
							{
								traceListener2.Flush();
							}
						}
						return;
					}
				}
			}
			if (args == null)
			{
				using (IEnumerator enumerator3 = TraceInternal.Listeners.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						object obj4 = enumerator3.Current;
						TraceListener traceListener3 = (TraceListener)obj4;
						if (!traceListener3.IsThreadSafe)
						{
							TraceListener traceListener4 = traceListener3;
							lock (traceListener4)
							{
								traceListener3.TraceEvent(traceEventCache, TraceInternal.AppName, eventType, id, format);
								if (TraceInternal.AutoFlush)
								{
									traceListener3.Flush();
								}
								continue;
							}
						}
						traceListener3.TraceEvent(traceEventCache, TraceInternal.AppName, eventType, id, format);
						if (TraceInternal.AutoFlush)
						{
							traceListener3.Flush();
						}
					}
					return;
				}
			}
			foreach (object obj5 in TraceInternal.Listeners)
			{
				TraceListener traceListener5 = (TraceListener)obj5;
				if (!traceListener5.IsThreadSafe)
				{
					TraceListener traceListener6 = traceListener5;
					lock (traceListener6)
					{
						traceListener5.TraceEvent(traceEventCache, TraceInternal.AppName, eventType, id, format, args);
						if (TraceInternal.AutoFlush)
						{
							traceListener5.Flush();
						}
						continue;
					}
				}
				traceListener5.TraceEvent(traceEventCache, TraceInternal.AppName, eventType, id, format, args);
				if (TraceInternal.AutoFlush)
				{
					traceListener5.Flush();
				}
			}
		}

		// Token: 0x06002CA3 RID: 11427 RVA: 0x000C894C File Offset: 0x000C6B4C
		public static void Write(string message)
		{
			if (TraceInternal.UseGlobalLock)
			{
				object obj = TraceInternal.critSec;
				lock (obj)
				{
					using (IEnumerator enumerator = TraceInternal.Listeners.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj2 = enumerator.Current;
							TraceListener traceListener = (TraceListener)obj2;
							traceListener.Write(message);
							if (TraceInternal.AutoFlush)
							{
								traceListener.Flush();
							}
						}
						return;
					}
				}
			}
			foreach (object obj3 in TraceInternal.Listeners)
			{
				TraceListener traceListener2 = (TraceListener)obj3;
				if (!traceListener2.IsThreadSafe)
				{
					TraceListener traceListener3 = traceListener2;
					lock (traceListener3)
					{
						traceListener2.Write(message);
						if (TraceInternal.AutoFlush)
						{
							traceListener2.Flush();
						}
						continue;
					}
				}
				traceListener2.Write(message);
				if (TraceInternal.AutoFlush)
				{
					traceListener2.Flush();
				}
			}
		}

		// Token: 0x06002CA4 RID: 11428 RVA: 0x000C8A8C File Offset: 0x000C6C8C
		public static void Write(object value)
		{
			if (TraceInternal.UseGlobalLock)
			{
				object obj = TraceInternal.critSec;
				lock (obj)
				{
					using (IEnumerator enumerator = TraceInternal.Listeners.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj2 = enumerator.Current;
							TraceListener traceListener = (TraceListener)obj2;
							traceListener.Write(value);
							if (TraceInternal.AutoFlush)
							{
								traceListener.Flush();
							}
						}
						return;
					}
				}
			}
			foreach (object obj3 in TraceInternal.Listeners)
			{
				TraceListener traceListener2 = (TraceListener)obj3;
				if (!traceListener2.IsThreadSafe)
				{
					TraceListener traceListener3 = traceListener2;
					lock (traceListener3)
					{
						traceListener2.Write(value);
						if (TraceInternal.AutoFlush)
						{
							traceListener2.Flush();
						}
						continue;
					}
				}
				traceListener2.Write(value);
				if (TraceInternal.AutoFlush)
				{
					traceListener2.Flush();
				}
			}
		}

		// Token: 0x06002CA5 RID: 11429 RVA: 0x000C8BCC File Offset: 0x000C6DCC
		public static void Write(string message, string category)
		{
			if (TraceInternal.UseGlobalLock)
			{
				object obj = TraceInternal.critSec;
				lock (obj)
				{
					using (IEnumerator enumerator = TraceInternal.Listeners.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj2 = enumerator.Current;
							TraceListener traceListener = (TraceListener)obj2;
							traceListener.Write(message, category);
							if (TraceInternal.AutoFlush)
							{
								traceListener.Flush();
							}
						}
						return;
					}
				}
			}
			foreach (object obj3 in TraceInternal.Listeners)
			{
				TraceListener traceListener2 = (TraceListener)obj3;
				if (!traceListener2.IsThreadSafe)
				{
					TraceListener traceListener3 = traceListener2;
					lock (traceListener3)
					{
						traceListener2.Write(message, category);
						if (TraceInternal.AutoFlush)
						{
							traceListener2.Flush();
						}
						continue;
					}
				}
				traceListener2.Write(message, category);
				if (TraceInternal.AutoFlush)
				{
					traceListener2.Flush();
				}
			}
		}

		// Token: 0x06002CA6 RID: 11430 RVA: 0x000C8D10 File Offset: 0x000C6F10
		public static void Write(object value, string category)
		{
			if (TraceInternal.UseGlobalLock)
			{
				object obj = TraceInternal.critSec;
				lock (obj)
				{
					using (IEnumerator enumerator = TraceInternal.Listeners.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj2 = enumerator.Current;
							TraceListener traceListener = (TraceListener)obj2;
							traceListener.Write(value, category);
							if (TraceInternal.AutoFlush)
							{
								traceListener.Flush();
							}
						}
						return;
					}
				}
			}
			foreach (object obj3 in TraceInternal.Listeners)
			{
				TraceListener traceListener2 = (TraceListener)obj3;
				if (!traceListener2.IsThreadSafe)
				{
					TraceListener traceListener3 = traceListener2;
					lock (traceListener3)
					{
						traceListener2.Write(value, category);
						if (TraceInternal.AutoFlush)
						{
							traceListener2.Flush();
						}
						continue;
					}
				}
				traceListener2.Write(value, category);
				if (TraceInternal.AutoFlush)
				{
					traceListener2.Flush();
				}
			}
		}

		// Token: 0x06002CA7 RID: 11431 RVA: 0x000C8E54 File Offset: 0x000C7054
		public static void WriteLine(string message)
		{
			if (TraceInternal.UseGlobalLock)
			{
				object obj = TraceInternal.critSec;
				lock (obj)
				{
					using (IEnumerator enumerator = TraceInternal.Listeners.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj2 = enumerator.Current;
							TraceListener traceListener = (TraceListener)obj2;
							traceListener.WriteLine(message);
							if (TraceInternal.AutoFlush)
							{
								traceListener.Flush();
							}
						}
						return;
					}
				}
			}
			foreach (object obj3 in TraceInternal.Listeners)
			{
				TraceListener traceListener2 = (TraceListener)obj3;
				if (!traceListener2.IsThreadSafe)
				{
					TraceListener traceListener3 = traceListener2;
					lock (traceListener3)
					{
						traceListener2.WriteLine(message);
						if (TraceInternal.AutoFlush)
						{
							traceListener2.Flush();
						}
						continue;
					}
				}
				traceListener2.WriteLine(message);
				if (TraceInternal.AutoFlush)
				{
					traceListener2.Flush();
				}
			}
		}

		// Token: 0x06002CA8 RID: 11432 RVA: 0x000C8F94 File Offset: 0x000C7194
		public static void WriteLine(object value)
		{
			if (TraceInternal.UseGlobalLock)
			{
				object obj = TraceInternal.critSec;
				lock (obj)
				{
					using (IEnumerator enumerator = TraceInternal.Listeners.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj2 = enumerator.Current;
							TraceListener traceListener = (TraceListener)obj2;
							traceListener.WriteLine(value);
							if (TraceInternal.AutoFlush)
							{
								traceListener.Flush();
							}
						}
						return;
					}
				}
			}
			foreach (object obj3 in TraceInternal.Listeners)
			{
				TraceListener traceListener2 = (TraceListener)obj3;
				if (!traceListener2.IsThreadSafe)
				{
					TraceListener traceListener3 = traceListener2;
					lock (traceListener3)
					{
						traceListener2.WriteLine(value);
						if (TraceInternal.AutoFlush)
						{
							traceListener2.Flush();
						}
						continue;
					}
				}
				traceListener2.WriteLine(value);
				if (TraceInternal.AutoFlush)
				{
					traceListener2.Flush();
				}
			}
		}

		// Token: 0x06002CA9 RID: 11433 RVA: 0x000C90D4 File Offset: 0x000C72D4
		public static void WriteLine(string message, string category)
		{
			if (TraceInternal.UseGlobalLock)
			{
				object obj = TraceInternal.critSec;
				lock (obj)
				{
					using (IEnumerator enumerator = TraceInternal.Listeners.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj2 = enumerator.Current;
							TraceListener traceListener = (TraceListener)obj2;
							traceListener.WriteLine(message, category);
							if (TraceInternal.AutoFlush)
							{
								traceListener.Flush();
							}
						}
						return;
					}
				}
			}
			foreach (object obj3 in TraceInternal.Listeners)
			{
				TraceListener traceListener2 = (TraceListener)obj3;
				if (!traceListener2.IsThreadSafe)
				{
					TraceListener traceListener3 = traceListener2;
					lock (traceListener3)
					{
						traceListener2.WriteLine(message, category);
						if (TraceInternal.AutoFlush)
						{
							traceListener2.Flush();
						}
						continue;
					}
				}
				traceListener2.WriteLine(message, category);
				if (TraceInternal.AutoFlush)
				{
					traceListener2.Flush();
				}
			}
		}

		// Token: 0x06002CAA RID: 11434 RVA: 0x000C9218 File Offset: 0x000C7418
		public static void WriteLine(object value, string category)
		{
			if (TraceInternal.UseGlobalLock)
			{
				object obj = TraceInternal.critSec;
				lock (obj)
				{
					using (IEnumerator enumerator = TraceInternal.Listeners.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj2 = enumerator.Current;
							TraceListener traceListener = (TraceListener)obj2;
							traceListener.WriteLine(value, category);
							if (TraceInternal.AutoFlush)
							{
								traceListener.Flush();
							}
						}
						return;
					}
				}
			}
			foreach (object obj3 in TraceInternal.Listeners)
			{
				TraceListener traceListener2 = (TraceListener)obj3;
				if (!traceListener2.IsThreadSafe)
				{
					TraceListener traceListener3 = traceListener2;
					lock (traceListener3)
					{
						traceListener2.WriteLine(value, category);
						if (TraceInternal.AutoFlush)
						{
							traceListener2.Flush();
						}
						continue;
					}
				}
				traceListener2.WriteLine(value, category);
				if (TraceInternal.AutoFlush)
				{
					traceListener2.Flush();
				}
			}
		}

		// Token: 0x06002CAB RID: 11435 RVA: 0x000C935C File Offset: 0x000C755C
		public static void WriteIf(bool condition, string message)
		{
			if (condition)
			{
				TraceInternal.Write(message);
			}
		}

		// Token: 0x06002CAC RID: 11436 RVA: 0x000C9367 File Offset: 0x000C7567
		public static void WriteIf(bool condition, object value)
		{
			if (condition)
			{
				TraceInternal.Write(value);
			}
		}

		// Token: 0x06002CAD RID: 11437 RVA: 0x000C9372 File Offset: 0x000C7572
		public static void WriteIf(bool condition, string message, string category)
		{
			if (condition)
			{
				TraceInternal.Write(message, category);
			}
		}

		// Token: 0x06002CAE RID: 11438 RVA: 0x000C937E File Offset: 0x000C757E
		public static void WriteIf(bool condition, object value, string category)
		{
			if (condition)
			{
				TraceInternal.Write(value, category);
			}
		}

		// Token: 0x06002CAF RID: 11439 RVA: 0x000C938A File Offset: 0x000C758A
		public static void WriteLineIf(bool condition, string message)
		{
			if (condition)
			{
				TraceInternal.WriteLine(message);
			}
		}

		// Token: 0x06002CB0 RID: 11440 RVA: 0x000C9395 File Offset: 0x000C7595
		public static void WriteLineIf(bool condition, object value)
		{
			if (condition)
			{
				TraceInternal.WriteLine(value);
			}
		}

		// Token: 0x06002CB1 RID: 11441 RVA: 0x000C93A0 File Offset: 0x000C75A0
		public static void WriteLineIf(bool condition, string message, string category)
		{
			if (condition)
			{
				TraceInternal.WriteLine(message, category);
			}
		}

		// Token: 0x06002CB2 RID: 11442 RVA: 0x000C93AC File Offset: 0x000C75AC
		public static void WriteLineIf(bool condition, object value, string category)
		{
			if (condition)
			{
				TraceInternal.WriteLine(value, category);
			}
		}

		// Token: 0x040026D3 RID: 9939
		private static volatile string appName = null;

		// Token: 0x040026D4 RID: 9940
		private static volatile TraceListenerCollection listeners;

		// Token: 0x040026D5 RID: 9941
		private static volatile bool autoFlush;

		// Token: 0x040026D6 RID: 9942
		private static volatile bool useGlobalLock;

		// Token: 0x040026D7 RID: 9943
		[ThreadStatic]
		private static int indentLevel;

		// Token: 0x040026D8 RID: 9944
		private static volatile int indentSize;

		// Token: 0x040026D9 RID: 9945
		private static volatile bool settingsInitialized;

		// Token: 0x040026DA RID: 9946
		private static volatile bool defaultInitialized;

		// Token: 0x040026DB RID: 9947
		internal static readonly object critSec = new object();
	}
}
