using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;

namespace Microsoft.Win32
{
	// Token: 0x0200001E RID: 30
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class SystemEvents
	{
		// Token: 0x060001CA RID: 458 RVA: 0x0000D562 File Offset: 0x0000B762
		private SystemEvents()
		{
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060001CB RID: 459 RVA: 0x0000D56C File Offset: 0x0000B76C
		private static bool UserInteractive
		{
			get
			{
				if (Environment.OSVersion.Platform == PlatformID.Win32NT)
				{
					IntPtr intPtr = IntPtr.Zero;
					intPtr = UnsafeNativeMethods.GetProcessWindowStation();
					if (intPtr != IntPtr.Zero && SystemEvents.processWinStation != intPtr)
					{
						SystemEvents.isUserInteractive = true;
						int num = 0;
						NativeMethods.USEROBJECTFLAGS userobjectflags = new NativeMethods.USEROBJECTFLAGS();
						if (UnsafeNativeMethods.GetUserObjectInformation(new HandleRef(null, intPtr), 1, userobjectflags, Marshal.SizeOf(userobjectflags), ref num) && (userobjectflags.dwFlags & 1) == 0)
						{
							SystemEvents.isUserInteractive = false;
						}
						SystemEvents.processWinStation = intPtr;
					}
				}
				else
				{
					SystemEvents.isUserInteractive = true;
				}
				return SystemEvents.isUserInteractive;
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060001CC RID: 460 RVA: 0x0000D601 File Offset: 0x0000B801
		// (remove) Token: 0x060001CD RID: 461 RVA: 0x0000D60E File Offset: 0x0000B80E
		public static event EventHandler DisplaySettingsChanging
		{
			add
			{
				SystemEvents.AddEventHandler(SystemEvents.OnDisplaySettingsChangingEvent, value);
			}
			remove
			{
				SystemEvents.RemoveEventHandler(SystemEvents.OnDisplaySettingsChangingEvent, value);
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060001CE RID: 462 RVA: 0x0000D61B File Offset: 0x0000B81B
		// (remove) Token: 0x060001CF RID: 463 RVA: 0x0000D628 File Offset: 0x0000B828
		public static event EventHandler DisplaySettingsChanged
		{
			add
			{
				SystemEvents.AddEventHandler(SystemEvents.OnDisplaySettingsChangedEvent, value);
			}
			remove
			{
				SystemEvents.RemoveEventHandler(SystemEvents.OnDisplaySettingsChangedEvent, value);
			}
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060001D0 RID: 464 RVA: 0x0000D635 File Offset: 0x0000B835
		// (remove) Token: 0x060001D1 RID: 465 RVA: 0x0000D642 File Offset: 0x0000B842
		public static event EventHandler EventsThreadShutdown
		{
			add
			{
				SystemEvents.AddEventHandler(SystemEvents.OnEventsThreadShutdownEvent, value);
			}
			remove
			{
				SystemEvents.RemoveEventHandler(SystemEvents.OnEventsThreadShutdownEvent, value);
			}
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060001D2 RID: 466 RVA: 0x0000D64F File Offset: 0x0000B84F
		// (remove) Token: 0x060001D3 RID: 467 RVA: 0x0000D65C File Offset: 0x0000B85C
		public static event EventHandler InstalledFontsChanged
		{
			add
			{
				SystemEvents.AddEventHandler(SystemEvents.OnInstalledFontsChangedEvent, value);
			}
			remove
			{
				SystemEvents.RemoveEventHandler(SystemEvents.OnInstalledFontsChangedEvent, value);
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060001D4 RID: 468 RVA: 0x0000D669 File Offset: 0x0000B869
		// (remove) Token: 0x060001D5 RID: 469 RVA: 0x0000D67D File Offset: 0x0000B87D
		[Obsolete("This event has been deprecated. http://go.microsoft.com/fwlink/?linkid=14202")]
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static event EventHandler LowMemory
		{
			add
			{
				SystemEvents.EnsureSystemEvents(true, true);
				SystemEvents.AddEventHandler(SystemEvents.OnLowMemoryEvent, value);
			}
			remove
			{
				SystemEvents.RemoveEventHandler(SystemEvents.OnLowMemoryEvent, value);
			}
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060001D6 RID: 470 RVA: 0x0000D68A File Offset: 0x0000B88A
		// (remove) Token: 0x060001D7 RID: 471 RVA: 0x0000D697 File Offset: 0x0000B897
		public static event EventHandler PaletteChanged
		{
			add
			{
				SystemEvents.AddEventHandler(SystemEvents.OnPaletteChangedEvent, value);
			}
			remove
			{
				SystemEvents.RemoveEventHandler(SystemEvents.OnPaletteChangedEvent, value);
			}
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060001D8 RID: 472 RVA: 0x0000D6A4 File Offset: 0x0000B8A4
		// (remove) Token: 0x060001D9 RID: 473 RVA: 0x0000D6B8 File Offset: 0x0000B8B8
		public static event PowerModeChangedEventHandler PowerModeChanged
		{
			add
			{
				SystemEvents.EnsureSystemEvents(true, true);
				SystemEvents.AddEventHandler(SystemEvents.OnPowerModeChangedEvent, value);
			}
			remove
			{
				SystemEvents.RemoveEventHandler(SystemEvents.OnPowerModeChangedEvent, value);
			}
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060001DA RID: 474 RVA: 0x0000D6C5 File Offset: 0x0000B8C5
		// (remove) Token: 0x060001DB RID: 475 RVA: 0x0000D6D9 File Offset: 0x0000B8D9
		public static event SessionEndedEventHandler SessionEnded
		{
			add
			{
				SystemEvents.EnsureSystemEvents(true, false);
				SystemEvents.AddEventHandler(SystemEvents.OnSessionEndedEvent, value);
			}
			remove
			{
				SystemEvents.RemoveEventHandler(SystemEvents.OnSessionEndedEvent, value);
			}
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060001DC RID: 476 RVA: 0x0000D6E6 File Offset: 0x0000B8E6
		// (remove) Token: 0x060001DD RID: 477 RVA: 0x0000D6FA File Offset: 0x0000B8FA
		public static event SessionEndingEventHandler SessionEnding
		{
			add
			{
				SystemEvents.EnsureSystemEvents(true, false);
				SystemEvents.AddEventHandler(SystemEvents.OnSessionEndingEvent, value);
			}
			remove
			{
				SystemEvents.RemoveEventHandler(SystemEvents.OnSessionEndingEvent, value);
			}
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x060001DE RID: 478 RVA: 0x0000D707 File Offset: 0x0000B907
		// (remove) Token: 0x060001DF RID: 479 RVA: 0x0000D720 File Offset: 0x0000B920
		public static event SessionSwitchEventHandler SessionSwitch
		{
			add
			{
				SystemEvents.EnsureSystemEvents(true, true);
				SystemEvents.EnsureRegisteredSessionNotification();
				SystemEvents.AddEventHandler(SystemEvents.OnSessionSwitchEvent, value);
			}
			remove
			{
				SystemEvents.RemoveEventHandler(SystemEvents.OnSessionSwitchEvent, value);
			}
		}

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x060001E0 RID: 480 RVA: 0x0000D72D File Offset: 0x0000B92D
		// (remove) Token: 0x060001E1 RID: 481 RVA: 0x0000D741 File Offset: 0x0000B941
		public static event EventHandler TimeChanged
		{
			add
			{
				SystemEvents.EnsureSystemEvents(true, false);
				SystemEvents.AddEventHandler(SystemEvents.OnTimeChangedEvent, value);
			}
			remove
			{
				SystemEvents.RemoveEventHandler(SystemEvents.OnTimeChangedEvent, value);
			}
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x060001E2 RID: 482 RVA: 0x0000D74E File Offset: 0x0000B94E
		// (remove) Token: 0x060001E3 RID: 483 RVA: 0x0000D762 File Offset: 0x0000B962
		public static event TimerElapsedEventHandler TimerElapsed
		{
			add
			{
				SystemEvents.EnsureSystemEvents(true, false);
				SystemEvents.AddEventHandler(SystemEvents.OnTimerElapsedEvent, value);
			}
			remove
			{
				SystemEvents.RemoveEventHandler(SystemEvents.OnTimerElapsedEvent, value);
			}
		}

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060001E4 RID: 484 RVA: 0x0000D76F File Offset: 0x0000B96F
		// (remove) Token: 0x060001E5 RID: 485 RVA: 0x0000D77C File Offset: 0x0000B97C
		public static event UserPreferenceChangedEventHandler UserPreferenceChanged
		{
			add
			{
				SystemEvents.AddEventHandler(SystemEvents.OnUserPreferenceChangedEvent, value);
			}
			remove
			{
				SystemEvents.RemoveEventHandler(SystemEvents.OnUserPreferenceChangedEvent, value);
			}
		}

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x060001E6 RID: 486 RVA: 0x0000D789 File Offset: 0x0000B989
		// (remove) Token: 0x060001E7 RID: 487 RVA: 0x0000D796 File Offset: 0x0000B996
		public static event UserPreferenceChangingEventHandler UserPreferenceChanging
		{
			add
			{
				SystemEvents.AddEventHandler(SystemEvents.OnUserPreferenceChangingEvent, value);
			}
			remove
			{
				SystemEvents.RemoveEventHandler(SystemEvents.OnUserPreferenceChangingEvent, value);
			}
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000D7A4 File Offset: 0x0000B9A4
		private static void AddEventHandler(object key, Delegate value)
		{
			object obj = SystemEvents.eventLockObject;
			lock (obj)
			{
				if (SystemEvents._handlers == null)
				{
					SystemEvents._handlers = new Dictionary<object, List<SystemEvents.SystemEventInvokeInfo>>();
					SystemEvents.EnsureSystemEvents(false, false);
				}
				List<SystemEvents.SystemEventInvokeInfo> list;
				if (!SystemEvents._handlers.TryGetValue(key, out list))
				{
					list = new List<SystemEvents.SystemEventInvokeInfo>();
					SystemEvents._handlers[key] = list;
				}
				else
				{
					list = SystemEvents._handlers[key];
				}
				list.Add(new SystemEvents.SystemEventInvokeInfo(value));
			}
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000D830 File Offset: 0x0000BA30
		private int ConsoleHandlerProc(int signalType)
		{
			if (signalType != 5)
			{
				if (signalType == 6)
				{
					this.OnSessionEnded((IntPtr)1, (IntPtr)0);
				}
			}
			else
			{
				this.OnSessionEnded((IntPtr)1, (IntPtr)int.MinValue);
			}
			return 0;
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060001EA RID: 490 RVA: 0x0000D868 File Offset: 0x0000BA68
		private NativeMethods.WNDCLASS WndClass
		{
			get
			{
				if (SystemEvents.staticwndclass == null)
				{
					IntPtr moduleHandle = UnsafeNativeMethods.GetModuleHandle(null);
					SystemEvents.className = string.Format(CultureInfo.InvariantCulture, ".NET-BroadcastEventWindow.{0}.{1}.{2}", new object[]
					{
						"4.0.0.0",
						Convert.ToString(AppDomain.CurrentDomain.GetHashCode(), 16),
						SystemEvents.domainQualifier
					});
					NativeMethods.WNDCLASS wndclass = new NativeMethods.WNDCLASS();
					wndclass.hbrBackground = (IntPtr)6;
					wndclass.style = 0;
					this.windowProc = new NativeMethods.WndProc(this.WindowProc);
					wndclass.lpszClassName = SystemEvents.className;
					wndclass.lpfnWndProc = this.windowProc;
					wndclass.hInstance = moduleHandle;
					SystemEvents.staticwndclass = wndclass;
				}
				return SystemEvents.staticwndclass;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060001EB RID: 491 RVA: 0x0000D92C File Offset: 0x0000BB2C
		private IntPtr DefWndProc
		{
			get
			{
				if (SystemEvents.defWindowProc == IntPtr.Zero)
				{
					string text = ((Marshal.SystemDefaultCharSize == 1) ? "DefWindowProcA" : "DefWindowProcW");
					SystemEvents.defWindowProc = UnsafeNativeMethods.GetProcAddress(new HandleRef(this, UnsafeNativeMethods.GetModuleHandle("user32.dll")), text);
				}
				return SystemEvents.defWindowProc;
			}
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000D985 File Offset: 0x0000BB85
		private void BumpQualifier()
		{
			SystemEvents.staticwndclass = null;
			SystemEvents.domainQualifier++;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000D9A0 File Offset: 0x0000BBA0
		private IntPtr CreateBroadcastWindow()
		{
			NativeMethods.WNDCLASS_I wndclass_I = new NativeMethods.WNDCLASS_I();
			IntPtr moduleHandle = UnsafeNativeMethods.GetModuleHandle(null);
			if (!UnsafeNativeMethods.GetClassInfo(new HandleRef(this, moduleHandle), this.WndClass.lpszClassName, wndclass_I))
			{
				if (UnsafeNativeMethods.RegisterClass(this.WndClass) == 0)
				{
					this.windowProc = null;
					return IntPtr.Zero;
				}
			}
			else if (wndclass_I.lpfnWndProc == this.DefWndProc)
			{
				short num = 0;
				if (UnsafeNativeMethods.UnregisterClass(this.WndClass.lpszClassName, new HandleRef(null, UnsafeNativeMethods.GetModuleHandle(null))) != 0)
				{
					num = UnsafeNativeMethods.RegisterClass(this.WndClass);
				}
				if (num == 0)
				{
					do
					{
						this.BumpQualifier();
					}
					while (UnsafeNativeMethods.RegisterClass(this.WndClass) == 0 && Marshal.GetLastWin32Error() == 1410);
				}
			}
			return UnsafeNativeMethods.CreateWindowEx(0, this.WndClass.lpszClassName, this.WndClass.lpszClassName, int.MinValue, 0, 0, 0, 0, NativeMethods.NullHandleRef, NativeMethods.NullHandleRef, new HandleRef(this, moduleHandle), null);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000DA8C File Offset: 0x0000BC8C
		public static IntPtr CreateTimer(int interval)
		{
			if (interval <= 0)
			{
				throw new ArgumentException(SR.GetString("InvalidLowBoundArgument", new object[]
				{
					"interval",
					interval.ToString(Thread.CurrentThread.CurrentCulture),
					"0"
				}));
			}
			SystemEvents.EnsureSystemEvents(true, true);
			IntPtr intPtr = UnsafeNativeMethods.SendMessage(new HandleRef(SystemEvents.systemEvents, SystemEvents.systemEvents.windowHandle), 1025, (IntPtr)interval, IntPtr.Zero);
			if (intPtr == IntPtr.Zero)
			{
				throw new ExternalException(SR.GetString("ErrorCreateTimer"));
			}
			return intPtr;
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000DB2C File Offset: 0x0000BD2C
		private void Dispose()
		{
			if (this.windowHandle != IntPtr.Zero)
			{
				if (SystemEvents.registeredSessionNotification)
				{
					UnsafeNativeMethods.WTSUnRegisterSessionNotification(new HandleRef(SystemEvents.systemEvents, SystemEvents.systemEvents.windowHandle));
				}
				IntPtr intPtr = this.windowHandle;
				this.windowHandle = IntPtr.Zero;
				HandleRef handleRef = new HandleRef(this, intPtr);
				if (UnsafeNativeMethods.IsWindow(handleRef) && this.DefWndProc != IntPtr.Zero)
				{
					UnsafeNativeMethods.SetWindowLong(handleRef, -4, new HandleRef(this, this.DefWndProc));
					UnsafeNativeMethods.SetClassLong(handleRef, -24, this.DefWndProc);
				}
				if (UnsafeNativeMethods.IsWindow(handleRef) && !UnsafeNativeMethods.DestroyWindow(handleRef))
				{
					UnsafeNativeMethods.PostMessage(handleRef, 16, IntPtr.Zero, IntPtr.Zero);
				}
				else
				{
					IntPtr moduleHandle = UnsafeNativeMethods.GetModuleHandle(null);
					UnsafeNativeMethods.UnregisterClass(SystemEvents.className, new HandleRef(this, moduleHandle));
				}
			}
			if (this.consoleHandler != null)
			{
				UnsafeNativeMethods.SetConsoleCtrlHandler(this.consoleHandler, 0);
				this.consoleHandler = null;
			}
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000DC34 File Offset: 0x0000BE34
		private static void EnsureSystemEvents(bool requireHandle, bool throwOnRefusal)
		{
			if (SystemEvents.systemEvents == null)
			{
				object obj = SystemEvents.procLockObject;
				lock (obj)
				{
					if (SystemEvents.systemEvents == null)
					{
						if (Thread.GetDomain().GetData(".appDomain") != null)
						{
							if (throwOnRefusal)
							{
								throw new InvalidOperationException(SR.GetString("ErrorSystemEventsNotSupported"));
							}
						}
						else
						{
							if (!SystemEvents.UserInteractive || Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
							{
								SystemEvents.systemEvents = new SystemEvents();
								SystemEvents.systemEvents.Initialize();
							}
							else
							{
								SystemEvents.eventWindowReady = new ManualResetEvent(false);
								SystemEvents.systemEvents = new SystemEvents();
								SystemEvents.windowThread = new Thread(new ThreadStart(SystemEvents.systemEvents.WindowThreadProc));
								SystemEvents.windowThread.IsBackground = true;
								SystemEvents.windowThread.Name = ".NET SystemEvents";
								SystemEvents.windowThread.Start();
								SystemEvents.eventWindowReady.WaitOne();
							}
							if (requireHandle && SystemEvents.systemEvents.windowHandle == IntPtr.Zero)
							{
								throw new ExternalException(SR.GetString("ErrorCreateSystemEvents"));
							}
							SystemEvents.startupRecreates = false;
						}
					}
				}
			}
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000DD8C File Offset: 0x0000BF8C
		private static void EnsureRegisteredSessionNotification()
		{
			if (!SystemEvents.registeredSessionNotification)
			{
				IntPtr intPtr = SafeNativeMethods.LoadLibrary("wtsapi32.dll");
				if (intPtr != IntPtr.Zero)
				{
					UnsafeNativeMethods.WTSRegisterSessionNotification(new HandleRef(SystemEvents.systemEvents, SystemEvents.systemEvents.windowHandle), 0);
					SystemEvents.registeredSessionNotification = true;
					SafeNativeMethods.FreeLibrary(new HandleRef(null, intPtr));
				}
			}
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000DDF0 File Offset: 0x0000BFF0
		private UserPreferenceCategory GetUserPreferenceCategory(int msg, IntPtr wParam, IntPtr lParam)
		{
			UserPreferenceCategory userPreferenceCategory = UserPreferenceCategory.General;
			if (msg == 26)
			{
				if (lParam != IntPtr.Zero && Marshal.PtrToStringAuto(lParam).Equals("Policy"))
				{
					userPreferenceCategory = UserPreferenceCategory.Policy;
				}
				else if (lParam != IntPtr.Zero && Marshal.PtrToStringAuto(lParam).Equals("intl"))
				{
					userPreferenceCategory = UserPreferenceCategory.Locale;
				}
				else
				{
					int num = (int)wParam;
					if (num <= 113)
					{
						if (num <= 107)
						{
							switch (num)
							{
							case 4:
							case 29:
							case 30:
							case 32:
							case 33:
							case 93:
							case 96:
								break;
							case 5:
							case 7:
							case 8:
							case 9:
							case 10:
							case 12:
							case 14:
							case 16:
							case 18:
							case 22:
							case 25:
							case 27:
							case 31:
							case 35:
							case 36:
							case 38:
							case 39:
							case 40:
							case 41:
							case 43:
							case 45:
							case 48:
							case 49:
							case 50:
							case 52:
							case 54:
							case 56:
							case 58:
							case 60:
							case 62:
							case 64:
							case 66:
							case 68:
							case 70:
							case 72:
							case 74:
							case 78:
							case 79:
							case 80:
							case 83:
							case 84:
							case 89:
							case 90:
							case 92:
							case 94:
							case 95:
								return userPreferenceCategory;
							case 6:
							case 37:
							case 42:
							case 44:
							case 73:
							case 76:
							case 77:
								goto IL_0304;
							case 11:
							case 23:
							case 69:
							case 91:
								return UserPreferenceCategory.Keyboard;
							case 13:
							case 24:
							case 26:
							case 34:
							case 46:
							case 88:
								return UserPreferenceCategory.Icon;
							case 15:
							case 17:
							case 97:
								return UserPreferenceCategory.Screensaver;
							case 19:
							case 20:
							case 21:
							case 47:
							case 75:
							case 87:
								return UserPreferenceCategory.Desktop;
							case 28:
								goto IL_02F6;
							case 51:
							case 53:
							case 55:
							case 57:
							case 59:
							case 61:
							case 63:
							case 65:
							case 67:
							case 71:
								return UserPreferenceCategory.Accessibility;
							case 81:
							case 82:
							case 85:
							case 86:
								return UserPreferenceCategory.Power;
							default:
								switch (num)
								{
								case 101:
								case 103:
								case 105:
									break;
								case 102:
								case 104:
								case 106:
									return userPreferenceCategory;
								case 107:
									goto IL_02F6;
								default:
									return userPreferenceCategory;
								}
								break;
							}
						}
						else
						{
							if (num == 111)
							{
								goto IL_0304;
							}
							if (num != 113)
							{
								return userPreferenceCategory;
							}
						}
					}
					else if (num <= 4123)
					{
						switch (num)
						{
						case 4097:
						case 4101:
						case 4103:
						case 4105:
						case 4107:
						case 4109:
							goto IL_0304;
						case 4098:
						case 4100:
						case 4102:
						case 4104:
						case 4106:
						case 4108:
						case 4110:
							return userPreferenceCategory;
						case 4099:
							goto IL_02F6;
						case 4111:
							break;
						default:
							switch (num)
							{
							case 4115:
							case 4117:
								goto IL_02F6;
							case 4116:
							case 4118:
							case 4120:
							case 4122:
								return userPreferenceCategory;
							case 4119:
							case 4121:
							case 4123:
								break;
							default:
								return userPreferenceCategory;
							}
							break;
						}
					}
					else
					{
						if (num == 4159)
						{
							goto IL_0304;
						}
						switch (num)
						{
						case 8193:
						case 8195:
						case 8197:
						case 8199:
							goto IL_0304;
						case 8194:
						case 8196:
						case 8198:
							return userPreferenceCategory;
						default:
							return userPreferenceCategory;
						}
					}
					return UserPreferenceCategory.Mouse;
					IL_02F6:
					return UserPreferenceCategory.Menu;
					IL_0304:
					userPreferenceCategory = UserPreferenceCategory.Window;
				}
			}
			else if (msg == 21)
			{
				userPreferenceCategory = UserPreferenceCategory.Color;
			}
			return userPreferenceCategory;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000E110 File Offset: 0x0000C310
		private void Initialize()
		{
			this.consoleHandler = new NativeMethods.ConHndlr(this.ConsoleHandlerProc);
			if (!UnsafeNativeMethods.SetConsoleCtrlHandler(this.consoleHandler, 1))
			{
				this.consoleHandler = null;
			}
			this.windowHandle = this.CreateBroadcastWindow();
			AppDomain.CurrentDomain.ProcessExit += SystemEvents.Shutdown;
			AppDomain.CurrentDomain.DomainUnload += SystemEvents.Shutdown;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000E180 File Offset: 0x0000C380
		private void InvokeMarshaledCallbacks()
		{
			Delegate @delegate = null;
			Queue queue = SystemEvents.threadCallbackList;
			lock (queue)
			{
				if (SystemEvents.threadCallbackList.Count > 0)
				{
					@delegate = (Delegate)SystemEvents.threadCallbackList.Dequeue();
				}
				goto IL_00B5;
			}
			IL_0041:
			try
			{
				EventHandler eventHandler = @delegate as EventHandler;
				if (eventHandler != null)
				{
					eventHandler(null, EventArgs.Empty);
				}
				else
				{
					@delegate.DynamicInvoke(new object[0]);
				}
			}
			catch (Exception ex)
			{
			}
			Queue queue2 = SystemEvents.threadCallbackList;
			lock (queue2)
			{
				if (SystemEvents.threadCallbackList.Count > 0)
				{
					@delegate = (Delegate)SystemEvents.threadCallbackList.Dequeue();
				}
				else
				{
					@delegate = null;
				}
			}
			IL_00B5:
			if (@delegate == null)
			{
				return;
			}
			goto IL_0041;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000E270 File Offset: 0x0000C470
		public static void InvokeOnEventsThread(Delegate method)
		{
			SystemEvents.EnsureSystemEvents(true, true);
			if (SystemEvents.threadCallbackList == null)
			{
				object obj = SystemEvents.eventLockObject;
				lock (obj)
				{
					if (SystemEvents.threadCallbackList == null)
					{
						SystemEvents.threadCallbackMessage = SafeNativeMethods.RegisterWindowMessage("SystemEventsThreadCallbackMessage");
						SystemEvents.threadCallbackList = new Queue();
					}
				}
			}
			Queue queue = SystemEvents.threadCallbackList;
			lock (queue)
			{
				SystemEvents.threadCallbackList.Enqueue(method);
			}
			UnsafeNativeMethods.PostMessage(new HandleRef(SystemEvents.systemEvents, SystemEvents.systemEvents.windowHandle), SystemEvents.threadCallbackMessage, IntPtr.Zero, IntPtr.Zero);
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000E348 File Offset: 0x0000C548
		public static void KillTimer(IntPtr timerId)
		{
			SystemEvents.EnsureSystemEvents(true, true);
			if (SystemEvents.systemEvents.windowHandle != IntPtr.Zero && (int)UnsafeNativeMethods.SendMessage(new HandleRef(SystemEvents.systemEvents, SystemEvents.systemEvents.windowHandle), 1026, timerId, IntPtr.Zero) == 0)
			{
				throw new ExternalException(SR.GetString("ErrorKillTimer"));
			}
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000E3BC File Offset: 0x0000C5BC
		private IntPtr OnCreateTimer(IntPtr wParam)
		{
			IntPtr intPtr = (IntPtr)SystemEvents.randomTimerId.Next();
			IntPtr intPtr2 = UnsafeNativeMethods.SetTimer(new HandleRef(this, this.windowHandle), new HandleRef(this, intPtr), (int)wParam, NativeMethods.NullHandleRef);
			if (!(intPtr2 == IntPtr.Zero))
			{
				return intPtr;
			}
			return IntPtr.Zero;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000E413 File Offset: 0x0000C613
		private void OnDisplaySettingsChanging()
		{
			SystemEvents.RaiseEvent(SystemEvents.OnDisplaySettingsChangingEvent, new object[]
			{
				this,
				EventArgs.Empty
			});
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000E431 File Offset: 0x0000C631
		private void OnDisplaySettingsChanged()
		{
			SystemEvents.RaiseEvent(SystemEvents.OnDisplaySettingsChangedEvent, new object[]
			{
				this,
				EventArgs.Empty
			});
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000E44F File Offset: 0x0000C64F
		private void OnGenericEvent(object eventKey)
		{
			SystemEvents.RaiseEvent(eventKey, new object[]
			{
				this,
				EventArgs.Empty
			});
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000E469 File Offset: 0x0000C669
		private void OnShutdown(object eventKey)
		{
			SystemEvents.RaiseEvent(false, eventKey, new object[]
			{
				this,
				EventArgs.Empty
			});
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000E484 File Offset: 0x0000C684
		private bool OnKillTimer(IntPtr wParam)
		{
			return UnsafeNativeMethods.KillTimer(new HandleRef(this, this.windowHandle), new HandleRef(this, wParam));
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000E4B0 File Offset: 0x0000C6B0
		private void OnPowerModeChanged(IntPtr wParam)
		{
			PowerModes powerModes;
			switch ((int)wParam)
			{
			case 4:
			case 5:
				powerModes = PowerModes.Suspend;
				break;
			case 6:
			case 7:
			case 8:
				powerModes = PowerModes.Resume;
				break;
			case 9:
			case 10:
			case 11:
				powerModes = PowerModes.StatusChange;
				break;
			default:
				return;
			}
			SystemEvents.RaiseEvent(SystemEvents.OnPowerModeChangedEvent, new object[]
			{
				this,
				new PowerModeChangedEventArgs(powerModes)
			});
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000E514 File Offset: 0x0000C714
		private void OnSessionEnded(IntPtr wParam, IntPtr lParam)
		{
			if (wParam != (IntPtr)0)
			{
				SessionEndReasons sessionEndReasons = SessionEndReasons.SystemShutdown;
				if (((int)(long)lParam & -2147483648) != 0)
				{
					sessionEndReasons = SessionEndReasons.Logoff;
				}
				SessionEndedEventArgs sessionEndedEventArgs = new SessionEndedEventArgs(sessionEndReasons);
				SystemEvents.RaiseEvent(SystemEvents.OnSessionEndedEvent, new object[] { this, sessionEndedEventArgs });
			}
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000E564 File Offset: 0x0000C764
		private int OnSessionEnding(IntPtr lParam)
		{
			SessionEndReasons sessionEndReasons = SessionEndReasons.SystemShutdown;
			if (((long)lParam & -2147483648L) != 0L)
			{
				sessionEndReasons = SessionEndReasons.Logoff;
			}
			SessionEndingEventArgs sessionEndingEventArgs = new SessionEndingEventArgs(sessionEndReasons);
			SystemEvents.RaiseEvent(SystemEvents.OnSessionEndingEvent, new object[] { this, sessionEndingEventArgs });
			return sessionEndingEventArgs.Cancel ? 0 : 1;
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000E5B4 File Offset: 0x0000C7B4
		private void OnSessionSwitch(int wParam)
		{
			SessionSwitchEventArgs sessionSwitchEventArgs = new SessionSwitchEventArgs((SessionSwitchReason)wParam);
			SystemEvents.RaiseEvent(SystemEvents.OnSessionSwitchEvent, new object[] { this, sessionSwitchEventArgs });
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000E5E0 File Offset: 0x0000C7E0
		private void OnThemeChanged()
		{
			SystemEvents.RaiseEvent(SystemEvents.OnUserPreferenceChangingEvent, new object[]
			{
				this,
				new UserPreferenceChangingEventArgs(UserPreferenceCategory.VisualStyle)
			});
			UserPreferenceCategory userPreferenceCategory = UserPreferenceCategory.Window;
			SystemEvents.RaiseEvent(SystemEvents.OnUserPreferenceChangedEvent, new object[]
			{
				this,
				new UserPreferenceChangedEventArgs(userPreferenceCategory)
			});
			userPreferenceCategory = UserPreferenceCategory.VisualStyle;
			SystemEvents.RaiseEvent(SystemEvents.OnUserPreferenceChangedEvent, new object[]
			{
				this,
				new UserPreferenceChangedEventArgs(userPreferenceCategory)
			});
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000E64C File Offset: 0x0000C84C
		private void OnUserPreferenceChanged(int msg, IntPtr wParam, IntPtr lParam)
		{
			UserPreferenceCategory userPreferenceCategory = this.GetUserPreferenceCategory(msg, wParam, lParam);
			SystemEvents.RaiseEvent(SystemEvents.OnUserPreferenceChangedEvent, new object[]
			{
				this,
				new UserPreferenceChangedEventArgs(userPreferenceCategory)
			});
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000E680 File Offset: 0x0000C880
		private void OnUserPreferenceChanging(int msg, IntPtr wParam, IntPtr lParam)
		{
			UserPreferenceCategory userPreferenceCategory = this.GetUserPreferenceCategory(msg, wParam, lParam);
			SystemEvents.RaiseEvent(SystemEvents.OnUserPreferenceChangingEvent, new object[]
			{
				this,
				new UserPreferenceChangingEventArgs(userPreferenceCategory)
			});
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000E6B4 File Offset: 0x0000C8B4
		private void OnTimerElapsed(IntPtr wParam)
		{
			SystemEvents.RaiseEvent(SystemEvents.OnTimerElapsedEvent, new object[]
			{
				this,
				new TimerElapsedEventArgs(wParam)
			});
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000205 RID: 517 RVA: 0x0000E6D4 File Offset: 0x0000C8D4
		internal static bool UseEverettThreadAffinity
		{
			get
			{
				if (!SystemEvents.checkedThreadAffinity)
				{
					object obj = SystemEvents.eventLockObject;
					lock (obj)
					{
						if (!SystemEvents.checkedThreadAffinity)
						{
							SystemEvents.checkedThreadAffinity = true;
							string text = "Software\\{0}\\{1}\\{2}";
							try
							{
								new RegistryPermission(PermissionState.Unrestricted).Assert();
								RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(string.Format(CultureInfo.CurrentCulture, text, new object[]
								{
									SystemEvents.CompanyNameInternal,
									SystemEvents.ProductNameInternal,
									SystemEvents.ProductVersionInternal
								}));
								if (registryKey != null)
								{
									object value = registryKey.GetValue("EnableSystemEventsThreadAffinityCompatibility");
									if (value != null && (int)value != 0)
									{
										SystemEvents.useEverettThreadAffinity = true;
									}
								}
							}
							catch (SecurityException)
							{
							}
							catch (InvalidCastException)
							{
							}
						}
					}
				}
				return SystemEvents.useEverettThreadAffinity;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000206 RID: 518 RVA: 0x0000E7BC File Offset: 0x0000C9BC
		private static string CompanyNameInternal
		{
			get
			{
				string text = null;
				Assembly entryAssembly = Assembly.GetEntryAssembly();
				if (entryAssembly != null)
				{
					object[] customAttributes = entryAssembly.GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
					if (customAttributes != null && customAttributes.Length != 0)
					{
						text = ((AssemblyCompanyAttribute)customAttributes[0]).Company;
					}
				}
				if (text == null || text.Length == 0)
				{
					text = SystemEvents.GetAppFileVersionInfo().CompanyName;
					if (text != null)
					{
						text = text.Trim();
					}
				}
				if (text == null || text.Length == 0)
				{
					Type appMainType = SystemEvents.GetAppMainType();
					if (appMainType != null)
					{
						string @namespace = appMainType.Namespace;
						if (!string.IsNullOrEmpty(@namespace))
						{
							int num = @namespace.IndexOf(".", StringComparison.Ordinal);
							if (num != -1)
							{
								text = @namespace.Substring(0, num);
							}
							else
							{
								text = @namespace;
							}
						}
						else
						{
							text = SystemEvents.ProductNameInternal;
						}
					}
				}
				return text;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000207 RID: 519 RVA: 0x0000E87C File Offset: 0x0000CA7C
		private static string ProductNameInternal
		{
			get
			{
				string text = null;
				Assembly entryAssembly = Assembly.GetEntryAssembly();
				if (entryAssembly != null)
				{
					object[] customAttributes = entryAssembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
					if (customAttributes != null && customAttributes.Length != 0)
					{
						text = ((AssemblyProductAttribute)customAttributes[0]).Product;
					}
				}
				if (text == null || text.Length == 0)
				{
					text = SystemEvents.GetAppFileVersionInfo().ProductName;
					if (text != null)
					{
						text = text.Trim();
					}
				}
				if (text == null || text.Length == 0)
				{
					Type appMainType = SystemEvents.GetAppMainType();
					if (appMainType != null)
					{
						string @namespace = appMainType.Namespace;
						if (!string.IsNullOrEmpty(@namespace))
						{
							int num = @namespace.LastIndexOf(".", StringComparison.Ordinal);
							if (num != -1 && num < @namespace.Length - 1)
							{
								text = @namespace.Substring(num + 1);
							}
							else
							{
								text = @namespace;
							}
						}
						else
						{
							text = appMainType.Name;
						}
					}
				}
				return text;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000208 RID: 520 RVA: 0x0000E948 File Offset: 0x0000CB48
		private static string ProductVersionInternal
		{
			get
			{
				string text = null;
				Assembly entryAssembly = Assembly.GetEntryAssembly();
				if (entryAssembly != null)
				{
					object[] customAttributes = entryAssembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false);
					if (customAttributes != null && customAttributes.Length != 0)
					{
						text = ((AssemblyInformationalVersionAttribute)customAttributes[0]).InformationalVersion;
					}
				}
				if (text == null || text.Length == 0)
				{
					text = SystemEvents.GetAppFileVersionInfo().ProductVersion;
					if (text != null)
					{
						text = text.Trim();
					}
				}
				if (text == null || text.Length == 0)
				{
					text = "1.0.0.0";
				}
				return text;
			}
		}

		// Token: 0x06000209 RID: 521 RVA: 0x0000E9C0 File Offset: 0x0000CBC0
		private static FileVersionInfo GetAppFileVersionInfo()
		{
			if (SystemEvents.appFileVersion == null)
			{
				Type appMainType = SystemEvents.GetAppMainType();
				if (appMainType != null)
				{
					new FileIOPermission(PermissionState.None)
					{
						AllFiles = (FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery)
					}.Assert();
					try
					{
						SystemEvents.appFileVersion = FileVersionInfo.GetVersionInfo(appMainType.Module.FullyQualifiedName);
						goto IL_005D;
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
				SystemEvents.appFileVersion = FileVersionInfo.GetVersionInfo(SystemEvents.ExecutablePath);
			}
			IL_005D:
			return (FileVersionInfo)SystemEvents.appFileVersion;
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000EA48 File Offset: 0x0000CC48
		private static Type GetAppMainType()
		{
			if (SystemEvents.mainType == null)
			{
				Assembly entryAssembly = Assembly.GetEntryAssembly();
				if (entryAssembly != null)
				{
					SystemEvents.mainType = entryAssembly.EntryPoint.ReflectedType;
				}
			}
			return SystemEvents.mainType;
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600020B RID: 523 RVA: 0x0000EA8C File Offset: 0x0000CC8C
		private static string ExecutablePath
		{
			get
			{
				if (SystemEvents.executablePath == null)
				{
					Assembly entryAssembly = Assembly.GetEntryAssembly();
					if (entryAssembly == null)
					{
						StringBuilder stringBuilder = new StringBuilder(260);
						UnsafeNativeMethods.GetModuleFileName(NativeMethods.NullHandleRef, stringBuilder, stringBuilder.Capacity);
						SystemEvents.executablePath = IntSecurity.UnsafeGetFullPath(stringBuilder.ToString());
					}
					else
					{
						string escapedCodeBase = entryAssembly.EscapedCodeBase;
						Uri uri = new Uri(escapedCodeBase);
						if (uri.Scheme == "file")
						{
							SystemEvents.executablePath = NativeMethods.GetLocalPath(escapedCodeBase);
						}
						else
						{
							SystemEvents.executablePath = uri.ToString();
						}
					}
				}
				Uri uri2 = new Uri(SystemEvents.executablePath);
				if (uri2.Scheme == "file")
				{
					new FileIOPermission(FileIOPermissionAccess.PathDiscovery, SystemEvents.executablePath).Demand();
				}
				return SystemEvents.executablePath;
			}
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000EB59 File Offset: 0x0000CD59
		private static void RaiseEvent(object key, params object[] args)
		{
			SystemEvents.RaiseEvent(true, key, args);
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000EB64 File Offset: 0x0000CD64
		private static void RaiseEvent(bool checkFinalization, object key, params object[] args)
		{
			if (checkFinalization && AppDomain.CurrentDomain.IsFinalizingForUnload())
			{
				return;
			}
			SystemEvents.SystemEventInvokeInfo[] array = null;
			object obj = SystemEvents.eventLockObject;
			lock (obj)
			{
				if (SystemEvents._handlers != null && SystemEvents._handlers.ContainsKey(key))
				{
					List<SystemEvents.SystemEventInvokeInfo> list = SystemEvents._handlers[key];
					if (list != null)
					{
						array = list.ToArray();
					}
				}
			}
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					try
					{
						SystemEvents.SystemEventInvokeInfo systemEventInvokeInfo = array[i];
						systemEventInvokeInfo.Invoke(checkFinalization, args);
						array[i] = null;
					}
					catch (Exception)
					{
					}
				}
				object obj2 = SystemEvents.eventLockObject;
				lock (obj2)
				{
					List<SystemEvents.SystemEventInvokeInfo> list2 = null;
					foreach (SystemEvents.SystemEventInvokeInfo systemEventInvokeInfo2 in array)
					{
						if (systemEventInvokeInfo2 != null)
						{
							if (list2 == null && !SystemEvents._handlers.TryGetValue(key, out list2))
							{
								break;
							}
							list2.Remove(systemEventInvokeInfo2);
						}
					}
				}
			}
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000EC84 File Offset: 0x0000CE84
		private static void RemoveEventHandler(object key, Delegate value)
		{
			object obj = SystemEvents.eventLockObject;
			lock (obj)
			{
				if (SystemEvents._handlers != null && SystemEvents._handlers.ContainsKey(key))
				{
					List<SystemEvents.SystemEventInvokeInfo> list = SystemEvents._handlers[key];
					list.Remove(new SystemEvents.SystemEventInvokeInfo(value));
				}
			}
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000ECEC File Offset: 0x0000CEEC
		private static void Startup()
		{
			if (SystemEvents.startupRecreates)
			{
				SystemEvents.EnsureSystemEvents(false, false);
			}
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000ED00 File Offset: 0x0000CF00
		private static void Shutdown()
		{
			if (SystemEvents.systemEvents != null && SystemEvents.systemEvents.windowHandle != IntPtr.Zero)
			{
				object obj = SystemEvents.procLockObject;
				lock (obj)
				{
					if (SystemEvents.systemEvents != null)
					{
						SystemEvents.startupRecreates = true;
						if (SystemEvents.windowThread != null)
						{
							SystemEvents.eventThreadTerminated = new ManualResetEvent(false);
							UnsafeNativeMethods.PostMessage(new HandleRef(SystemEvents.systemEvents, SystemEvents.systemEvents.windowHandle), 18, IntPtr.Zero, IntPtr.Zero);
							SystemEvents.eventThreadTerminated.WaitOne();
							SystemEvents.windowThread.Join();
						}
						else
						{
							SystemEvents.systemEvents.Dispose();
							SystemEvents.systemEvents = null;
						}
					}
				}
			}
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000EDE4 File Offset: 0x0000CFE4
		[PrePrepareMethod]
		private static void Shutdown(object sender, EventArgs e)
		{
			SystemEvents.Shutdown();
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000EDEC File Offset: 0x0000CFEC
		private IntPtr WindowProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
		{
			if (msg > 689)
			{
				if (msg <= 8213)
				{
					if (msg <= 794)
					{
						if (msg != 785 && msg != 794)
						{
							goto IL_02B3;
						}
						goto IL_0196;
					}
					else
					{
						if (msg == 1025)
						{
							return this.OnCreateTimer(wParam);
						}
						if (msg == 1026)
						{
							return (IntPtr)(this.OnKillTimer(wParam) ? 1 : 0);
						}
						if (msg != 8213)
						{
							goto IL_02B3;
						}
					}
				}
				else if (msg <= 8318)
				{
					switch (msg)
					{
					case 8218:
						try
						{
							this.OnUserPreferenceChanging(msg - 8192, wParam, lParam);
							this.OnUserPreferenceChanged(msg - 8192, wParam, lParam);
							goto IL_02CC;
						}
						finally
						{
							try
							{
								if (lParam != IntPtr.Zero)
								{
									Marshal.FreeHGlobal(lParam);
								}
							}
							catch (Exception ex)
							{
							}
						}
						break;
					case 8219:
					case 8220:
						goto IL_02B3;
					case 8221:
						this.OnGenericEvent(SystemEvents.OnInstalledFontsChangedEvent);
						goto IL_02CC;
					case 8222:
						this.OnGenericEvent(SystemEvents.OnTimeChangedEvent);
						goto IL_02CC;
					default:
						if (msg == 8257)
						{
							this.OnGenericEvent(SystemEvents.OnLowMemoryEvent);
							goto IL_02CC;
						}
						if (msg != 8318)
						{
							goto IL_02B3;
						}
						this.OnDisplaySettingsChanging();
						this.OnDisplaySettingsChanged();
						goto IL_02CC;
					}
				}
				else
				{
					if (msg == 8467)
					{
						this.OnTimerElapsed(wParam);
						goto IL_02CC;
					}
					if (msg == 8977)
					{
						this.OnGenericEvent(SystemEvents.OnPaletteChangedEvent);
						goto IL_02CC;
					}
					if (msg != 8986)
					{
						goto IL_02B3;
					}
					this.OnThemeChanged();
					goto IL_02CC;
				}
				this.OnUserPreferenceChanging(msg - 8192, wParam, lParam);
				this.OnUserPreferenceChanged(msg - 8192, wParam, lParam);
				goto IL_02CC;
			}
			if (msg <= 30)
			{
				if (msg <= 21)
				{
					if (msg == 17)
					{
						return (IntPtr)this.OnSessionEnding(lParam);
					}
					if (msg != 21)
					{
						goto IL_02B3;
					}
				}
				else
				{
					if (msg == 22)
					{
						this.OnSessionEnded(wParam, lParam);
						goto IL_02CC;
					}
					if (msg == 26)
					{
						IntPtr intPtr = lParam;
						if (lParam != IntPtr.Zero)
						{
							string text = Marshal.PtrToStringAuto(lParam);
							if (text != null)
							{
								intPtr = Marshal.StringToHGlobalAuto(text);
							}
						}
						UnsafeNativeMethods.PostMessage(new HandleRef(this, this.windowHandle), 8192 + msg, wParam, intPtr);
						goto IL_02CC;
					}
					if (msg - 29 > 1)
					{
						goto IL_02B3;
					}
				}
			}
			else if (msg <= 126)
			{
				if (msg != 65 && msg != 126)
				{
					goto IL_02B3;
				}
			}
			else if (msg != 275)
			{
				if (msg == 536)
				{
					this.OnPowerModeChanged(wParam);
					goto IL_02CC;
				}
				if (msg != 689)
				{
					goto IL_02B3;
				}
				this.OnSessionSwitch((int)wParam);
				goto IL_02CC;
			}
			IL_0196:
			UnsafeNativeMethods.PostMessage(new HandleRef(this, this.windowHandle), 8192 + msg, wParam, lParam);
			goto IL_02CC;
			IL_02B3:
			if (msg == SystemEvents.threadCallbackMessage && msg != 0)
			{
				this.InvokeMarshaledCallbacks();
				return IntPtr.Zero;
			}
			IL_02CC:
			return UnsafeNativeMethods.DefWindowProc(hWnd, msg, wParam, lParam);
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000F0EC File Offset: 0x0000D2EC
		private void WindowThreadProc()
		{
			try
			{
				this.Initialize();
				SystemEvents.eventWindowReady.Set();
				if (this.windowHandle != IntPtr.Zero)
				{
					NativeMethods.MSG msg = default(NativeMethods.MSG);
					bool flag = true;
					while (flag)
					{
						int num = UnsafeNativeMethods.MsgWaitForMultipleObjectsEx(0, IntPtr.Zero, 100, 255, 4);
						if (num == 258)
						{
							Thread.Sleep(1);
						}
						else
						{
							while (UnsafeNativeMethods.PeekMessage(ref msg, NativeMethods.NullHandleRef, 0, 0, 1))
							{
								if (msg.message == 18)
								{
									flag = false;
									break;
								}
								UnsafeNativeMethods.TranslateMessage(ref msg);
								UnsafeNativeMethods.DispatchMessage(ref msg);
							}
						}
					}
				}
				this.OnShutdown(SystemEvents.OnEventsThreadShutdownEvent);
			}
			catch (Exception ex)
			{
				SystemEvents.eventWindowReady.Set();
				if (!(ex is ThreadInterruptedException))
				{
					ThreadAbortException ex2 = ex as ThreadAbortException;
				}
			}
			this.Dispose();
			if (SystemEvents.eventThreadTerminated != null)
			{
				SystemEvents.eventThreadTerminated.Set();
			}
		}

		// Token: 0x04000311 RID: 785
		private static readonly object eventLockObject = new object();

		// Token: 0x04000312 RID: 786
		private static readonly object procLockObject = new object();

		// Token: 0x04000313 RID: 787
		private static volatile SystemEvents systemEvents;

		// Token: 0x04000314 RID: 788
		private static volatile Thread windowThread;

		// Token: 0x04000315 RID: 789
		private static volatile ManualResetEvent eventWindowReady;

		// Token: 0x04000316 RID: 790
		private static Random randomTimerId = new Random();

		// Token: 0x04000317 RID: 791
		private static volatile bool startupRecreates;

		// Token: 0x04000318 RID: 792
		private static volatile bool registeredSessionNotification = false;

		// Token: 0x04000319 RID: 793
		private static volatile int domainQualifier;

		// Token: 0x0400031A RID: 794
		private static volatile NativeMethods.WNDCLASS staticwndclass;

		// Token: 0x0400031B RID: 795
		private static volatile IntPtr defWindowProc;

		// Token: 0x0400031C RID: 796
		private static volatile string className = null;

		// Token: 0x0400031D RID: 797
		private static volatile Queue threadCallbackList;

		// Token: 0x0400031E RID: 798
		private static volatile int threadCallbackMessage = 0;

		// Token: 0x0400031F RID: 799
		private static volatile ManualResetEvent eventThreadTerminated;

		// Token: 0x04000320 RID: 800
		private static volatile bool checkedThreadAffinity = false;

		// Token: 0x04000321 RID: 801
		private static volatile bool useEverettThreadAffinity = false;

		// Token: 0x04000322 RID: 802
		private const string everettThreadAffinityValue = "EnableSystemEventsThreadAffinityCompatibility";

		// Token: 0x04000323 RID: 803
		private volatile IntPtr windowHandle;

		// Token: 0x04000324 RID: 804
		private NativeMethods.WndProc windowProc;

		// Token: 0x04000325 RID: 805
		private NativeMethods.ConHndlr consoleHandler;

		// Token: 0x04000326 RID: 806
		private static readonly object OnUserPreferenceChangingEvent = new object();

		// Token: 0x04000327 RID: 807
		private static readonly object OnUserPreferenceChangedEvent = new object();

		// Token: 0x04000328 RID: 808
		private static readonly object OnSessionEndingEvent = new object();

		// Token: 0x04000329 RID: 809
		private static readonly object OnSessionEndedEvent = new object();

		// Token: 0x0400032A RID: 810
		private static readonly object OnPowerModeChangedEvent = new object();

		// Token: 0x0400032B RID: 811
		private static readonly object OnLowMemoryEvent = new object();

		// Token: 0x0400032C RID: 812
		private static readonly object OnDisplaySettingsChangingEvent = new object();

		// Token: 0x0400032D RID: 813
		private static readonly object OnDisplaySettingsChangedEvent = new object();

		// Token: 0x0400032E RID: 814
		private static readonly object OnInstalledFontsChangedEvent = new object();

		// Token: 0x0400032F RID: 815
		private static readonly object OnTimeChangedEvent = new object();

		// Token: 0x04000330 RID: 816
		private static readonly object OnTimerElapsedEvent = new object();

		// Token: 0x04000331 RID: 817
		private static readonly object OnPaletteChangedEvent = new object();

		// Token: 0x04000332 RID: 818
		private static readonly object OnEventsThreadShutdownEvent = new object();

		// Token: 0x04000333 RID: 819
		private static readonly object OnSessionSwitchEvent = new object();

		// Token: 0x04000334 RID: 820
		private static Dictionary<object, List<SystemEvents.SystemEventInvokeInfo>> _handlers;

		// Token: 0x04000335 RID: 821
		private static volatile IntPtr processWinStation = IntPtr.Zero;

		// Token: 0x04000336 RID: 822
		private static volatile bool isUserInteractive = false;

		// Token: 0x04000337 RID: 823
		private static volatile object appFileVersion;

		// Token: 0x04000338 RID: 824
		private static volatile Type mainType;

		// Token: 0x04000339 RID: 825
		private static volatile string executablePath = null;

		// Token: 0x020006D2 RID: 1746
		private class SystemEventInvokeInfo
		{
			// Token: 0x06004011 RID: 16401 RVA: 0x0010CF76 File Offset: 0x0010B176
			public SystemEventInvokeInfo(Delegate d)
			{
				this._delegate = d;
				this._syncContext = AsyncOperationManager.SynchronizationContext;
			}

			// Token: 0x06004012 RID: 16402 RVA: 0x0010CF90 File Offset: 0x0010B190
			public void Invoke(bool checkFinalization, params object[] args)
			{
				try
				{
					if (this._syncContext == null || SystemEvents.UseEverettThreadAffinity)
					{
						this.InvokeCallback(args);
					}
					else
					{
						this._syncContext.Send(new SendOrPostCallback(this.InvokeCallback), args);
					}
				}
				catch (InvalidAsynchronousStateException)
				{
					if (!checkFinalization || !AppDomain.CurrentDomain.IsFinalizingForUnload())
					{
						this.InvokeCallback(args);
					}
				}
			}

			// Token: 0x06004013 RID: 16403 RVA: 0x0010CFF8 File Offset: 0x0010B1F8
			private void InvokeCallback(object arg)
			{
				this._delegate.DynamicInvoke((object[])arg);
			}

			// Token: 0x06004014 RID: 16404 RVA: 0x0010D00C File Offset: 0x0010B20C
			public override bool Equals(object other)
			{
				SystemEvents.SystemEventInvokeInfo systemEventInvokeInfo = other as SystemEvents.SystemEventInvokeInfo;
				return systemEventInvokeInfo != null && systemEventInvokeInfo._delegate.Equals(this._delegate);
			}

			// Token: 0x06004015 RID: 16405 RVA: 0x0010D036 File Offset: 0x0010B236
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			// Token: 0x04002FBD RID: 12221
			private SynchronizationContext _syncContext;

			// Token: 0x04002FBE RID: 12222
			private Delegate _delegate;
		}
	}
}
