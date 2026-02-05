using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.Diagnostics
{
	// Token: 0x020004CB RID: 1227
	[DefaultEvent("EntryWritten")]
	[InstallerType("System.Diagnostics.EventLogInstaller, System.Configuration.Install, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[MonitoringDescription("EventLogDesc")]
	public class EventLog : Component, ISupportInitialize
	{
		// Token: 0x17000B09 RID: 2825
		// (get) Token: 0x06002DB1 RID: 11697 RVA: 0x000CD66C File Offset: 0x000CB86C
		private static bool SkipRegPatch
		{
			get
			{
				if (!EventLog.s_CheckedOsVersion)
				{
					OperatingSystem osversion = Environment.OSVersion;
					EventLog.s_SkipRegPatch = osversion.Platform == PlatformID.Win32NT && osversion.Version.Major > 5;
					EventLog.s_CheckedOsVersion = true;
				}
				return EventLog.s_SkipRegPatch;
			}
		}

		// Token: 0x06002DB2 RID: 11698 RVA: 0x000CD6B8 File Offset: 0x000CB8B8
		internal static PermissionSet _UnsafeGetAssertPermSet()
		{
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			RegistryPermission registryPermission = new RegistryPermission(PermissionState.Unrestricted);
			permissionSet.AddPermission(registryPermission);
			EnvironmentPermission environmentPermission = new EnvironmentPermission(PermissionState.Unrestricted);
			permissionSet.AddPermission(environmentPermission);
			SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
			permissionSet.AddPermission(securityPermission);
			return permissionSet;
		}

		// Token: 0x06002DB3 RID: 11699 RVA: 0x000CD6FA File Offset: 0x000CB8FA
		public EventLog()
			: this("", ".", "")
		{
		}

		// Token: 0x06002DB4 RID: 11700 RVA: 0x000CD711 File Offset: 0x000CB911
		public EventLog(string logName)
			: this(logName, ".", "")
		{
		}

		// Token: 0x06002DB5 RID: 11701 RVA: 0x000CD724 File Offset: 0x000CB924
		public EventLog(string logName, string machineName)
			: this(logName, machineName, "")
		{
		}

		// Token: 0x06002DB6 RID: 11702 RVA: 0x000CD733 File Offset: 0x000CB933
		public EventLog(string logName, string machineName, string source)
		{
			this.m_underlyingEventLog = new EventLogInternal(logName, machineName, source, this);
		}

		// Token: 0x17000B0A RID: 2826
		// (get) Token: 0x06002DB7 RID: 11703 RVA: 0x000CD74A File Offset: 0x000CB94A
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("LogEntries")]
		public EventLogEntryCollection Entries
		{
			get
			{
				return this.m_underlyingEventLog.Entries;
			}
		}

		// Token: 0x17000B0B RID: 2827
		// (get) Token: 0x06002DB8 RID: 11704 RVA: 0x000CD757 File Offset: 0x000CB957
		[Browsable(false)]
		public string LogDisplayName
		{
			get
			{
				return this.m_underlyingEventLog.LogDisplayName;
			}
		}

		// Token: 0x17000B0C RID: 2828
		// (get) Token: 0x06002DB9 RID: 11705 RVA: 0x000CD764 File Offset: 0x000CB964
		// (set) Token: 0x06002DBA RID: 11706 RVA: 0x000CD774 File Offset: 0x000CB974
		[TypeConverter("System.Diagnostics.Design.LogConverter, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[ReadOnly(true)]
		[MonitoringDescription("LogLog")]
		[DefaultValue("")]
		[SettingsBindable(true)]
		public string Log
		{
			get
			{
				return this.m_underlyingEventLog.Log;
			}
			set
			{
				EventLogInternal eventLogInternal = new EventLogInternal(value, this.m_underlyingEventLog.MachineName, this.m_underlyingEventLog.Source, this);
				EventLogInternal underlyingEventLog = this.m_underlyingEventLog;
				new EventLogPermission(EventLogPermissionAccess.Write, underlyingEventLog.machineName).Assert();
				if (underlyingEventLog.EnableRaisingEvents)
				{
					eventLogInternal.onEntryWrittenHandler = underlyingEventLog.onEntryWrittenHandler;
					eventLogInternal.EnableRaisingEvents = true;
				}
				this.m_underlyingEventLog = eventLogInternal;
				underlyingEventLog.Close();
			}
		}

		// Token: 0x17000B0D RID: 2829
		// (get) Token: 0x06002DBB RID: 11707 RVA: 0x000CD7E0 File Offset: 0x000CB9E0
		// (set) Token: 0x06002DBC RID: 11708 RVA: 0x000CD7F0 File Offset: 0x000CB9F0
		[ReadOnly(true)]
		[MonitoringDescription("LogMachineName")]
		[DefaultValue(".")]
		[SettingsBindable(true)]
		public string MachineName
		{
			get
			{
				return this.m_underlyingEventLog.MachineName;
			}
			set
			{
				EventLogInternal eventLogInternal = new EventLogInternal(this.m_underlyingEventLog.logName, value, this.m_underlyingEventLog.sourceName, this);
				EventLogInternal underlyingEventLog = this.m_underlyingEventLog;
				new EventLogPermission(EventLogPermissionAccess.Write, underlyingEventLog.machineName).Assert();
				if (underlyingEventLog.EnableRaisingEvents)
				{
					eventLogInternal.onEntryWrittenHandler = underlyingEventLog.onEntryWrittenHandler;
					eventLogInternal.EnableRaisingEvents = true;
				}
				this.m_underlyingEventLog = eventLogInternal;
				underlyingEventLog.Close();
			}
		}

		// Token: 0x17000B0E RID: 2830
		// (get) Token: 0x06002DBD RID: 11709 RVA: 0x000CD85C File Offset: 0x000CBA5C
		// (set) Token: 0x06002DBE RID: 11710 RVA: 0x000CD869 File Offset: 0x000CBA69
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[ComVisible(false)]
		public long MaximumKilobytes
		{
			get
			{
				return this.m_underlyingEventLog.MaximumKilobytes;
			}
			set
			{
				this.m_underlyingEventLog.MaximumKilobytes = value;
			}
		}

		// Token: 0x17000B0F RID: 2831
		// (get) Token: 0x06002DBF RID: 11711 RVA: 0x000CD877 File Offset: 0x000CBA77
		[Browsable(false)]
		[ComVisible(false)]
		public OverflowAction OverflowAction
		{
			get
			{
				return this.m_underlyingEventLog.OverflowAction;
			}
		}

		// Token: 0x17000B10 RID: 2832
		// (get) Token: 0x06002DC0 RID: 11712 RVA: 0x000CD884 File Offset: 0x000CBA84
		[Browsable(false)]
		[ComVisible(false)]
		public int MinimumRetentionDays
		{
			get
			{
				return this.m_underlyingEventLog.MinimumRetentionDays;
			}
		}

		// Token: 0x17000B11 RID: 2833
		// (get) Token: 0x06002DC1 RID: 11713 RVA: 0x000CD891 File Offset: 0x000CBA91
		internal bool ComponentDesignMode
		{
			get
			{
				return base.DesignMode;
			}
		}

		// Token: 0x06002DC2 RID: 11714 RVA: 0x000CD899 File Offset: 0x000CBA99
		internal object ComponentGetService(Type service)
		{
			return this.GetService(service);
		}

		// Token: 0x17000B12 RID: 2834
		// (get) Token: 0x06002DC3 RID: 11715 RVA: 0x000CD8A2 File Offset: 0x000CBAA2
		// (set) Token: 0x06002DC4 RID: 11716 RVA: 0x000CD8AF File Offset: 0x000CBAAF
		[Browsable(false)]
		[MonitoringDescription("LogMonitoring")]
		[DefaultValue(false)]
		public bool EnableRaisingEvents
		{
			get
			{
				return this.m_underlyingEventLog.EnableRaisingEvents;
			}
			set
			{
				this.m_underlyingEventLog.EnableRaisingEvents = value;
			}
		}

		// Token: 0x17000B13 RID: 2835
		// (get) Token: 0x06002DC5 RID: 11717 RVA: 0x000CD8BD File Offset: 0x000CBABD
		// (set) Token: 0x06002DC6 RID: 11718 RVA: 0x000CD8CA File Offset: 0x000CBACA
		[Browsable(false)]
		[DefaultValue(null)]
		[MonitoringDescription("LogSynchronizingObject")]
		public ISynchronizeInvoke SynchronizingObject
		{
			[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
			get
			{
				return this.m_underlyingEventLog.SynchronizingObject;
			}
			set
			{
				this.m_underlyingEventLog.SynchronizingObject = value;
			}
		}

		// Token: 0x17000B14 RID: 2836
		// (get) Token: 0x06002DC7 RID: 11719 RVA: 0x000CD8D8 File Offset: 0x000CBAD8
		// (set) Token: 0x06002DC8 RID: 11720 RVA: 0x000CD8E8 File Offset: 0x000CBAE8
		[ReadOnly(true)]
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[MonitoringDescription("LogSource")]
		[DefaultValue("")]
		[SettingsBindable(true)]
		public string Source
		{
			get
			{
				return this.m_underlyingEventLog.Source;
			}
			set
			{
				EventLogInternal eventLogInternal = new EventLogInternal(this.m_underlyingEventLog.Log, this.m_underlyingEventLog.MachineName, EventLog.CheckAndNormalizeSourceName(value), this);
				EventLogInternal underlyingEventLog = this.m_underlyingEventLog;
				new EventLogPermission(EventLogPermissionAccess.Write, underlyingEventLog.machineName).Assert();
				if (underlyingEventLog.EnableRaisingEvents)
				{
					eventLogInternal.onEntryWrittenHandler = underlyingEventLog.onEntryWrittenHandler;
					eventLogInternal.EnableRaisingEvents = true;
				}
				this.m_underlyingEventLog = eventLogInternal;
				underlyingEventLog.Close();
			}
		}

		// Token: 0x14000040 RID: 64
		// (add) Token: 0x06002DC9 RID: 11721 RVA: 0x000CD959 File Offset: 0x000CBB59
		// (remove) Token: 0x06002DCA RID: 11722 RVA: 0x000CD967 File Offset: 0x000CBB67
		[MonitoringDescription("LogEntryWritten")]
		public event EntryWrittenEventHandler EntryWritten
		{
			add
			{
				this.m_underlyingEventLog.EntryWritten += value;
			}
			remove
			{
				this.m_underlyingEventLog.EntryWritten -= value;
			}
		}

		// Token: 0x06002DCB RID: 11723 RVA: 0x000CD975 File Offset: 0x000CBB75
		public void BeginInit()
		{
			this.m_underlyingEventLog.BeginInit();
		}

		// Token: 0x06002DCC RID: 11724 RVA: 0x000CD982 File Offset: 0x000CBB82
		public void Clear()
		{
			this.m_underlyingEventLog.Clear();
		}

		// Token: 0x06002DCD RID: 11725 RVA: 0x000CD98F File Offset: 0x000CBB8F
		public void Close()
		{
			this.m_underlyingEventLog.Close();
		}

		// Token: 0x06002DCE RID: 11726 RVA: 0x000CD99C File Offset: 0x000CBB9C
		public static void CreateEventSource(string source, string logName)
		{
			EventLog.CreateEventSource(new EventSourceCreationData(source, logName, "."));
		}

		// Token: 0x06002DCF RID: 11727 RVA: 0x000CD9AF File Offset: 0x000CBBAF
		[Obsolete("This method has been deprecated.  Please use System.Diagnostics.EventLog.CreateEventSource(EventSourceCreationData sourceData) instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public static void CreateEventSource(string source, string logName, string machineName)
		{
			EventLog.CreateEventSource(new EventSourceCreationData(source, logName, machineName));
		}

		// Token: 0x06002DD0 RID: 11728 RVA: 0x000CD9C0 File Offset: 0x000CBBC0
		public static void CreateEventSource(EventSourceCreationData sourceData)
		{
			if (sourceData == null)
			{
				throw new ArgumentNullException("sourceData");
			}
			string text = sourceData.LogName;
			string source = sourceData.Source;
			string machineName = sourceData.MachineName;
			if (!SyntaxCheck.CheckMachineName(machineName))
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "machineName", machineName }));
			}
			if (text == null || text.Length == 0)
			{
				text = "Application";
			}
			if (!EventLog.ValidLogName(text, false))
			{
				throw new ArgumentException(SR.GetString("BadLogName"));
			}
			if (source == null || source.Length == 0)
			{
				throw new ArgumentException(SR.GetString("MissingParameter", new object[] { "source" }));
			}
			if (source.Length + "SYSTEM\\CurrentControlSet\\Services\\EventLog".Length > 254)
			{
				throw new ArgumentException(SR.GetString("ParameterTooLong", new object[]
				{
					"source",
					254 - "SYSTEM\\CurrentControlSet\\Services\\EventLog".Length
				}));
			}
			EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Administer, machineName);
			eventLogPermission.Demand();
			Mutex mutex = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				SharedUtils.EnterMutex("netfxeventlog.1.0", ref mutex);
				if (EventLog.SourceExists(source, machineName, true))
				{
					if (".".Equals(machineName))
					{
						throw new ArgumentException(SR.GetString("LocalSourceAlreadyExists", new object[] { source }));
					}
					throw new ArgumentException(SR.GetString("SourceAlreadyExists", new object[] { source, machineName }));
				}
				else
				{
					PermissionSet permissionSet = EventLog._UnsafeGetAssertPermSet();
					permissionSet.Assert();
					RegistryKey registryKey = null;
					RegistryKey registryKey2 = null;
					RegistryKey registryKey3 = null;
					RegistryKey registryKey4 = null;
					RegistryKey registryKey5 = null;
					try
					{
						if (machineName == ".")
						{
							registryKey = Registry.LocalMachine;
						}
						else
						{
							registryKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, machineName);
						}
						registryKey2 = registryKey.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\EventLog", true);
						if (registryKey2 == null)
						{
							if (!".".Equals(machineName))
							{
								throw new InvalidOperationException(SR.GetString("RegKeyMissing", new object[] { "SYSTEM\\CurrentControlSet\\Services\\EventLog", text, source, machineName }));
							}
							throw new InvalidOperationException(SR.GetString("LocalRegKeyMissing", new object[] { "SYSTEM\\CurrentControlSet\\Services\\EventLog", text, source }));
						}
						else
						{
							registryKey3 = registryKey2.OpenSubKey(text, true);
							if (registryKey3 == null && text.Length >= 8)
							{
								string text2 = text.Substring(0, 8);
								if (string.Compare(text2, "AppEvent", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(text2, "SecEvent", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(text2, "SysEvent", StringComparison.OrdinalIgnoreCase) == 0)
								{
									throw new ArgumentException(SR.GetString("InvalidCustomerLogName", new object[] { text }));
								}
								string text3 = EventLog.FindSame8FirstCharsLog(registryKey2, text);
								if (text3 != null)
								{
									throw new ArgumentException(SR.GetString("DuplicateLogName", new object[] { text, text3 }));
								}
							}
							bool flag = registryKey3 == null;
							if (flag)
							{
								if (EventLog.SourceExists(text, machineName, true))
								{
									if (".".Equals(machineName))
									{
										throw new ArgumentException(SR.GetString("LocalLogAlreadyExistsAsSource", new object[] { text }));
									}
									throw new ArgumentException(SR.GetString("LogAlreadyExistsAsSource", new object[] { text, machineName }));
								}
								else
								{
									registryKey3 = registryKey2.CreateSubKey(text);
									if (!EventLog.SkipRegPatch)
									{
										registryKey3.SetValue("Sources", new string[] { text, source }, RegistryValueKind.MultiString);
									}
									EventLog.SetSpecialLogRegValues(registryKey3, text);
									registryKey4 = registryKey3.CreateSubKey(text);
									EventLog.SetSpecialSourceRegValues(registryKey4, sourceData);
								}
							}
							if (text != source)
							{
								if (!flag)
								{
									EventLog.SetSpecialLogRegValues(registryKey3, text);
									if (!EventLog.SkipRegPatch)
									{
										string[] array = registryKey3.GetValue("Sources") as string[];
										if (array == null)
										{
											registryKey3.SetValue("Sources", new string[] { text, source }, RegistryValueKind.MultiString);
										}
										else if (Array.IndexOf<string>(array, source) == -1)
										{
											string[] array2 = new string[array.Length + 1];
											Array.Copy(array, array2, array.Length);
											array2[array.Length] = source;
											registryKey3.SetValue("Sources", array2, RegistryValueKind.MultiString);
										}
									}
								}
								registryKey5 = registryKey3.CreateSubKey(source);
								EventLog.SetSpecialSourceRegValues(registryKey5, sourceData);
							}
						}
					}
					finally
					{
						if (registryKey != null)
						{
							registryKey.Close();
						}
						if (registryKey2 != null)
						{
							registryKey2.Close();
						}
						if (registryKey3 != null)
						{
							registryKey3.Flush();
							registryKey3.Close();
						}
						if (registryKey4 != null)
						{
							registryKey4.Flush();
							registryKey4.Close();
						}
						if (registryKey5 != null)
						{
							registryKey5.Flush();
							registryKey5.Close();
						}
						CodeAccessPermission.RevertAssert();
					}
				}
			}
			finally
			{
				if (mutex != null)
				{
					mutex.ReleaseMutex();
					mutex.Close();
				}
			}
		}

		// Token: 0x06002DD1 RID: 11729 RVA: 0x000CDE58 File Offset: 0x000CC058
		public static void Delete(string logName)
		{
			EventLog.Delete(logName, ".");
		}

		// Token: 0x06002DD2 RID: 11730 RVA: 0x000CDE68 File Offset: 0x000CC068
		public static void Delete(string logName, string machineName)
		{
			if (!SyntaxCheck.CheckMachineName(machineName))
			{
				throw new ArgumentException(SR.GetString("InvalidParameterFormat", new object[] { "machineName" }));
			}
			if (logName == null || logName.Length == 0)
			{
				throw new ArgumentException(SR.GetString("NoLogName"));
			}
			if (!EventLog.ValidLogName(logName, false))
			{
				throw new InvalidOperationException(SR.GetString("BadLogName"));
			}
			EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Administer, machineName);
			eventLogPermission.Demand();
			SharedUtils.CheckEnvironment();
			PermissionSet permissionSet = EventLog._UnsafeGetAssertPermSet();
			permissionSet.Assert();
			RegistryKey registryKey = null;
			Mutex mutex = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				SharedUtils.EnterMutex("netfxeventlog.1.0", ref mutex);
				try
				{
					registryKey = EventLog.GetEventLogRegKey(machineName, true);
					if (registryKey == null)
					{
						throw new InvalidOperationException(SR.GetString("RegKeyNoAccess", new object[] { "HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\EventLog", machineName }));
					}
					using (RegistryKey registryKey2 = registryKey.OpenSubKey(logName))
					{
						if (registryKey2 == null)
						{
							throw new InvalidOperationException(SR.GetString("MissingLog", new object[] { logName, machineName }));
						}
						EventLog eventLog = new EventLog(logName, machineName);
						try
						{
							eventLog.Clear();
						}
						finally
						{
							eventLog.Close();
						}
						string text = null;
						try
						{
							text = (string)registryKey2.GetValue("File");
						}
						catch
						{
						}
						if (text != null)
						{
							try
							{
								File.Delete(text);
							}
							catch
							{
							}
						}
					}
					registryKey.DeleteSubKeyTree(logName);
				}
				finally
				{
					if (registryKey != null)
					{
						registryKey.Close();
					}
					CodeAccessPermission.RevertAssert();
				}
			}
			finally
			{
				if (mutex != null)
				{
					mutex.ReleaseMutex();
				}
			}
		}

		// Token: 0x06002DD3 RID: 11731 RVA: 0x000CE020 File Offset: 0x000CC220
		public static void DeleteEventSource(string source)
		{
			EventLog.DeleteEventSource(source, ".");
		}

		// Token: 0x06002DD4 RID: 11732 RVA: 0x000CE030 File Offset: 0x000CC230
		public static void DeleteEventSource(string source, string machineName)
		{
			if (!SyntaxCheck.CheckMachineName(machineName))
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "machineName", machineName }));
			}
			EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Administer, machineName);
			eventLogPermission.Demand();
			SharedUtils.CheckEnvironment();
			PermissionSet permissionSet = EventLog._UnsafeGetAssertPermSet();
			permissionSet.Assert();
			Mutex mutex = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				SharedUtils.EnterMutex("netfxeventlog.1.0", ref mutex);
				RegistryKey registryKey = null;
				RegistryKey registryKey2;
				registryKey = (registryKey2 = EventLog.FindSourceRegistration(source, machineName, true));
				try
				{
					if (registryKey == null)
					{
						if (machineName == null)
						{
							throw new ArgumentException(SR.GetString("LocalSourceNotRegistered", new object[] { source }));
						}
						throw new ArgumentException(SR.GetString("SourceNotRegistered", new object[] { source, machineName, "HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Services\\EventLog" }));
					}
					else
					{
						string name = registryKey.Name;
						int num = name.LastIndexOf('\\');
						if (string.Compare(name, num + 1, source, 0, name.Length - num, StringComparison.Ordinal) == 0)
						{
							throw new InvalidOperationException(SR.GetString("CannotDeleteEqualSource", new object[] { source }));
						}
					}
				}
				finally
				{
					if (registryKey2 != null)
					{
						((IDisposable)registryKey2).Dispose();
					}
				}
				try
				{
					registryKey = EventLog.FindSourceRegistration(source, machineName, false);
					registryKey.DeleteSubKeyTree(source);
					if (!EventLog.SkipRegPatch)
					{
						string[] array = (string[])registryKey.GetValue("Sources");
						ArrayList arrayList = new ArrayList(array.Length - 1);
						for (int i = 0; i < array.Length; i++)
						{
							if (array[i] != source)
							{
								arrayList.Add(array[i]);
							}
						}
						string[] array2 = new string[arrayList.Count];
						arrayList.CopyTo(array2);
						registryKey.SetValue("Sources", array2, RegistryValueKind.MultiString);
					}
				}
				finally
				{
					if (registryKey != null)
					{
						registryKey.Flush();
						registryKey.Close();
					}
					CodeAccessPermission.RevertAssert();
				}
			}
			finally
			{
				if (mutex != null)
				{
					mutex.ReleaseMutex();
				}
			}
		}

		// Token: 0x06002DD5 RID: 11733 RVA: 0x000CE23C File Offset: 0x000CC43C
		protected override void Dispose(bool disposing)
		{
			if (this.m_underlyingEventLog != null)
			{
				this.m_underlyingEventLog.Dispose(disposing);
			}
			base.Dispose(disposing);
		}

		// Token: 0x06002DD6 RID: 11734 RVA: 0x000CE259 File Offset: 0x000CC459
		public void EndInit()
		{
			this.m_underlyingEventLog.EndInit();
		}

		// Token: 0x06002DD7 RID: 11735 RVA: 0x000CE266 File Offset: 0x000CC466
		public static bool Exists(string logName)
		{
			return EventLog.Exists(logName, ".");
		}

		// Token: 0x06002DD8 RID: 11736 RVA: 0x000CE274 File Offset: 0x000CC474
		public static bool Exists(string logName, string machineName)
		{
			if (!SyntaxCheck.CheckMachineName(machineName))
			{
				throw new ArgumentException(SR.GetString("InvalidParameterFormat", new object[] { "machineName" }));
			}
			EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Administer, machineName);
			eventLogPermission.Demand();
			if (logName == null || logName.Length == 0)
			{
				return false;
			}
			SharedUtils.CheckEnvironment();
			PermissionSet permissionSet = EventLog._UnsafeGetAssertPermSet();
			permissionSet.Assert();
			RegistryKey registryKey = null;
			RegistryKey registryKey2 = null;
			bool flag;
			try
			{
				registryKey = EventLog.GetEventLogRegKey(machineName, false);
				if (registryKey == null)
				{
					flag = false;
				}
				else
				{
					registryKey2 = registryKey.OpenSubKey(logName, false);
					flag = registryKey2 != null;
				}
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
				if (registryKey2 != null)
				{
					registryKey2.Close();
				}
				CodeAccessPermission.RevertAssert();
			}
			return flag;
		}

		// Token: 0x06002DD9 RID: 11737 RVA: 0x000CE324 File Offset: 0x000CC524
		private static string FindSame8FirstCharsLog(RegistryKey keyParent, string logName)
		{
			string text = logName.Substring(0, 8);
			foreach (string text2 in keyParent.GetSubKeyNames())
			{
				if (text2.Length >= 8 && string.Compare(text2.Substring(0, 8), text, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return text2;
				}
			}
			return null;
		}

		// Token: 0x06002DDA RID: 11738 RVA: 0x000CE370 File Offset: 0x000CC570
		private static RegistryKey FindSourceRegistration(string source, string machineName, bool readOnly)
		{
			return EventLog.FindSourceRegistration(source, machineName, readOnly, false);
		}

		// Token: 0x06002DDB RID: 11739 RVA: 0x000CE37C File Offset: 0x000CC57C
		private static RegistryKey FindSourceRegistration(string source, string machineName, bool readOnly, bool wantToCreate)
		{
			if (source != null && source.Length != 0)
			{
				SharedUtils.CheckEnvironment();
				PermissionSet permissionSet = EventLog._UnsafeGetAssertPermSet();
				permissionSet.Assert();
				RegistryKey registryKey = null;
				try
				{
					registryKey = EventLog.GetEventLogRegKey(machineName, !readOnly);
					if (registryKey == null)
					{
						return null;
					}
					StringBuilder stringBuilder = null;
					string[] subKeyNames = registryKey.GetSubKeyNames();
					for (int i = 0; i < subKeyNames.Length; i++)
					{
						RegistryKey registryKey2 = null;
						try
						{
							RegistryKey registryKey3 = registryKey.OpenSubKey(subKeyNames[i], !readOnly);
							if (registryKey3 != null)
							{
								registryKey2 = registryKey3.OpenSubKey(source, !readOnly);
								if (registryKey2 != null)
								{
									return registryKey3;
								}
								registryKey3.Close();
							}
						}
						catch (UnauthorizedAccessException)
						{
							if (stringBuilder == null)
							{
								stringBuilder = new StringBuilder(subKeyNames[i]);
							}
							else
							{
								stringBuilder.Append(", ");
								stringBuilder.Append(subKeyNames[i]);
							}
						}
						catch (SecurityException)
						{
							if (stringBuilder == null)
							{
								stringBuilder = new StringBuilder(subKeyNames[i]);
							}
							else
							{
								stringBuilder.Append(", ");
								stringBuilder.Append(subKeyNames[i]);
							}
						}
						finally
						{
							if (registryKey2 != null)
							{
								registryKey2.Close();
							}
						}
					}
					if (stringBuilder != null)
					{
						throw new SecurityException(SR.GetString(wantToCreate ? "SomeLogsInaccessibleToCreate" : "SomeLogsInaccessible", new object[] { stringBuilder.ToString() }));
					}
				}
				finally
				{
					if (registryKey != null)
					{
						registryKey.Close();
					}
					CodeAccessPermission.RevertAssert();
				}
			}
			return null;
		}

		// Token: 0x06002DDC RID: 11740 RVA: 0x000CE4F4 File Offset: 0x000CC6F4
		public static EventLog[] GetEventLogs()
		{
			return EventLog.GetEventLogs(".");
		}

		// Token: 0x06002DDD RID: 11741 RVA: 0x000CE500 File Offset: 0x000CC700
		public static EventLog[] GetEventLogs(string machineName)
		{
			if (!SyntaxCheck.CheckMachineName(machineName))
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "machineName", machineName }));
			}
			EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Administer, machineName);
			eventLogPermission.Demand();
			SharedUtils.CheckEnvironment();
			string[] array = new string[0];
			PermissionSet permissionSet = EventLog._UnsafeGetAssertPermSet();
			permissionSet.Assert();
			RegistryKey registryKey = null;
			try
			{
				registryKey = EventLog.GetEventLogRegKey(machineName, false);
				if (registryKey == null)
				{
					throw new InvalidOperationException(SR.GetString("RegKeyMissingShort", new object[] { "SYSTEM\\CurrentControlSet\\Services\\EventLog", machineName }));
				}
				array = registryKey.GetSubKeyNames();
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
				CodeAccessPermission.RevertAssert();
			}
			if (EventLog.s_dontFilterRegKeys || machineName != ".")
			{
				EventLog[] array2 = new EventLog[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					EventLog eventLog = new EventLog(array[i], machineName);
					array2[i] = eventLog;
				}
				return array2;
			}
			List<EventLog> list = new List<EventLog>(array.Length);
			for (int j = 0; j < array.Length; j++)
			{
				EventLog eventLog2 = new EventLog(array[j], machineName);
				SafeEventLogReadHandle safeEventLogReadHandle = SafeEventLogReadHandle.OpenEventLog(machineName, array[j]);
				if (!safeEventLogReadHandle.IsInvalid)
				{
					safeEventLogReadHandle.Close();
					list.Add(eventLog2);
				}
				else if (Marshal.GetLastWin32Error() != 87)
				{
					list.Add(eventLog2);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06002DDE RID: 11742 RVA: 0x000CE664 File Offset: 0x000CC864
		private static bool IsWindowsRS5OrUp()
		{
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
			Microsoft.Win32.NativeMethods.RTL_OSVERSIONINFOEX rtl_OSVERSIONINFOEX = default(Microsoft.Win32.NativeMethods.RTL_OSVERSIONINFOEX);
			rtl_OSVERSIONINFOEX.dwOSVersionInfoSize = (uint)Marshal.SizeOf(rtl_OSVERSIONINFOEX);
			return Microsoft.Win32.NativeMethods.RtlGetVersion(out rtl_OSVERSIONINFOEX) == 0 && rtl_OSVERSIONINFOEX.dwPlatformId == 2U && (rtl_OSVERSIONINFOEX.dwMajorVersion > 10U || (rtl_OSVERSIONINFOEX.dwMajorVersion == 10U && (rtl_OSVERSIONINFOEX.dwMinorVersion > 0U || (rtl_OSVERSIONINFOEX.dwMinorVersion == 0U && rtl_OSVERSIONINFOEX.dwBuildNumber >= 17763U))));
		}

		// Token: 0x06002DDF RID: 11743 RVA: 0x000CE6EC File Offset: 0x000CC8EC
		internal static RegistryKey GetEventLogRegKey(string machine, bool writable)
		{
			RegistryKey registryKey = null;
			try
			{
				if (machine.Equals("."))
				{
					registryKey = Registry.LocalMachine;
				}
				else
				{
					registryKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, machine);
				}
				if (registryKey != null)
				{
					return registryKey.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\EventLog", writable);
				}
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
			}
			return null;
		}

		// Token: 0x06002DE0 RID: 11744 RVA: 0x000CE750 File Offset: 0x000CC950
		internal static string GetDllPath(string machineName)
		{
			return Path.Combine(SharedUtils.GetLatestBuildDllDirectory(machineName), "EventLogMessages.dll");
		}

		// Token: 0x06002DE1 RID: 11745 RVA: 0x000CE762 File Offset: 0x000CC962
		public static bool SourceExists(string source)
		{
			return EventLog.SourceExists(source, ".");
		}

		// Token: 0x06002DE2 RID: 11746 RVA: 0x000CE76F File Offset: 0x000CC96F
		public static bool SourceExists(string source, string machineName)
		{
			return EventLog.SourceExists(source, machineName, false);
		}

		// Token: 0x06002DE3 RID: 11747 RVA: 0x000CE77C File Offset: 0x000CC97C
		internal static bool SourceExists(string source, string machineName, bool wantToCreate)
		{
			if (!SyntaxCheck.CheckMachineName(machineName))
			{
				throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "machineName", machineName }));
			}
			EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Write, machineName);
			eventLogPermission.Demand();
			bool flag;
			using (RegistryKey registryKey = EventLog.FindSourceRegistration(source, machineName, true, wantToCreate))
			{
				flag = registryKey != null;
			}
			return flag;
		}

		// Token: 0x06002DE4 RID: 11748 RVA: 0x000CE7F0 File Offset: 0x000CC9F0
		public static string LogNameFromSourceName(string source, string machineName)
		{
			EventLogPermission eventLogPermission = new EventLogPermission(EventLogPermissionAccess.Administer, machineName);
			eventLogPermission.Demand();
			return EventLog._InternalLogNameFromSourceName(source, machineName);
		}

		// Token: 0x06002DE5 RID: 11749 RVA: 0x000CE814 File Offset: 0x000CCA14
		internal static string _InternalLogNameFromSourceName(string source, string machineName)
		{
			string text;
			using (RegistryKey registryKey = EventLog.FindSourceRegistration(source, machineName, true))
			{
				if (registryKey == null)
				{
					text = "";
				}
				else
				{
					string name = registryKey.Name;
					int num = name.LastIndexOf('\\');
					text = name.Substring(num + 1);
				}
			}
			return text;
		}

		// Token: 0x06002DE6 RID: 11750 RVA: 0x000CE86C File Offset: 0x000CCA6C
		[ComVisible(false)]
		public void ModifyOverflowPolicy(OverflowAction action, int retentionDays)
		{
			this.m_underlyingEventLog.ModifyOverflowPolicy(action, retentionDays);
		}

		// Token: 0x06002DE7 RID: 11751 RVA: 0x000CE87B File Offset: 0x000CCA7B
		[ComVisible(false)]
		public void RegisterDisplayName(string resourceFile, long resourceId)
		{
			this.m_underlyingEventLog.RegisterDisplayName(resourceFile, resourceId);
		}

		// Token: 0x06002DE8 RID: 11752 RVA: 0x000CE88C File Offset: 0x000CCA8C
		private static void SetSpecialLogRegValues(RegistryKey logKey, string logName)
		{
			if (logKey.GetValue("MaxSize") == null)
			{
				logKey.SetValue("MaxSize", 524288, RegistryValueKind.DWord);
			}
			if (logKey.GetValue("AutoBackupLogFiles") == null)
			{
				logKey.SetValue("AutoBackupLogFiles", 0, RegistryValueKind.DWord);
			}
			if (!EventLog.SkipRegPatch)
			{
				if (logKey.GetValue("Retention") == null)
				{
					logKey.SetValue("Retention", 604800, RegistryValueKind.DWord);
				}
				if (logKey.GetValue("File") == null)
				{
					string text;
					if (logName.Length > 8)
					{
						text = "%SystemRoot%\\System32\\config\\" + logName.Substring(0, 8) + ".evt";
					}
					else
					{
						text = "%SystemRoot%\\System32\\config\\" + logName + ".evt";
					}
					logKey.SetValue("File", text, RegistryValueKind.ExpandString);
				}
			}
		}

		// Token: 0x06002DE9 RID: 11753 RVA: 0x000CE954 File Offset: 0x000CCB54
		private static void SetSpecialSourceRegValues(RegistryKey sourceLogKey, EventSourceCreationData sourceData)
		{
			if (string.IsNullOrEmpty(sourceData.MessageResourceFile))
			{
				sourceLogKey.SetValue("EventMessageFile", EventLog.GetDllPath(sourceData.MachineName), RegistryValueKind.ExpandString);
			}
			else
			{
				sourceLogKey.SetValue("EventMessageFile", EventLog.FixupPath(sourceData.MessageResourceFile), RegistryValueKind.ExpandString);
			}
			if (!string.IsNullOrEmpty(sourceData.ParameterResourceFile))
			{
				sourceLogKey.SetValue("ParameterMessageFile", EventLog.FixupPath(sourceData.ParameterResourceFile), RegistryValueKind.ExpandString);
			}
			if (!string.IsNullOrEmpty(sourceData.CategoryResourceFile))
			{
				sourceLogKey.SetValue("CategoryMessageFile", EventLog.FixupPath(sourceData.CategoryResourceFile), RegistryValueKind.ExpandString);
				sourceLogKey.SetValue("CategoryCount", sourceData.CategoryCount, RegistryValueKind.DWord);
			}
		}

		// Token: 0x06002DEA RID: 11754 RVA: 0x000CE9FD File Offset: 0x000CCBFD
		private static string FixupPath(string path)
		{
			if (path[0] == '%')
			{
				return path;
			}
			return Path.GetFullPath(path);
		}

		// Token: 0x06002DEB RID: 11755 RVA: 0x000CEA14 File Offset: 0x000CCC14
		internal static string TryFormatMessage(Microsoft.Win32.SafeHandles.SafeLibraryHandle hModule, uint messageNum, string[] insertionStrings)
		{
			if (insertionStrings.Length == 0)
			{
				return EventLog.UnsafeTryFormatMessage(hModule, messageNum, insertionStrings);
			}
			string text = EventLog.UnsafeTryFormatMessage(hModule, messageNum, new string[0]);
			if (text == null)
			{
				return null;
			}
			int num = 0;
			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] == '%' && text.Length > i + 1)
				{
					StringBuilder stringBuilder = new StringBuilder();
					while (i + 1 < text.Length && char.IsDigit(text[i + 1]))
					{
						stringBuilder.Append(text[i + 1]);
						i++;
					}
					i++;
					if (stringBuilder.Length > 0)
					{
						int num2 = -1;
						if (int.TryParse(stringBuilder.ToString(), NumberStyles.None, CultureInfo.InvariantCulture, out num2))
						{
							num = Math.Max(num, num2);
						}
					}
				}
			}
			if (num > insertionStrings.Length)
			{
				string[] array = new string[num];
				Array.Copy(insertionStrings, array, insertionStrings.Length);
				for (int j = insertionStrings.Length; j < array.Length; j++)
				{
					array[j] = "%" + (j + 1).ToString();
				}
				insertionStrings = array;
			}
			return EventLog.UnsafeTryFormatMessage(hModule, messageNum, insertionStrings);
		}

		// Token: 0x06002DEC RID: 11756 RVA: 0x000CEB28 File Offset: 0x000CCD28
		internal static string UnsafeTryFormatMessage(Microsoft.Win32.SafeHandles.SafeLibraryHandle hModule, uint messageNum, string[] insertionStrings)
		{
			string text = null;
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder(1024);
			int num2 = 10240;
			IntPtr[] array = new IntPtr[insertionStrings.Length];
			GCHandle[] array2 = new GCHandle[insertionStrings.Length];
			GCHandle gchandle = GCHandle.Alloc(array, GCHandleType.Pinned);
			if (insertionStrings.Length == 0)
			{
				num2 |= 512;
			}
			try
			{
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i] = GCHandle.Alloc(insertionStrings[i], GCHandleType.Pinned);
					array[i] = array2[i].AddrOfPinnedObject();
				}
				int num3 = 122;
				while (num == 0 && num3 == 122)
				{
					num = SafeNativeMethods.FormatMessage(num2, hModule, messageNum, 0, stringBuilder, stringBuilder.Capacity, array);
					if (num == 0)
					{
						num3 = Marshal.GetLastWin32Error();
						if (num3 == 122)
						{
							stringBuilder.Capacity *= 2;
						}
					}
				}
			}
			catch
			{
				num = 0;
			}
			finally
			{
				for (int j = 0; j < array2.Length; j++)
				{
					if (array2[j].IsAllocated)
					{
						array2[j].Free();
					}
				}
				gchandle.Free();
			}
			if (num > 0)
			{
				text = stringBuilder.ToString();
				if (text.Length > 1 && text[text.Length - 1] == '\n')
				{
					text = text.Substring(0, text.Length - 2);
				}
			}
			return text;
		}

		// Token: 0x06002DED RID: 11757 RVA: 0x000CEC84 File Offset: 0x000CCE84
		private static bool CharIsPrintable(char c)
		{
			UnicodeCategory unicodeCategory = char.GetUnicodeCategory(c);
			return unicodeCategory != UnicodeCategory.Control || unicodeCategory == UnicodeCategory.Format || unicodeCategory == UnicodeCategory.LineSeparator || unicodeCategory == UnicodeCategory.ParagraphSeparator || unicodeCategory == UnicodeCategory.OtherNotAssigned;
		}

		// Token: 0x06002DEE RID: 11758 RVA: 0x000CECB4 File Offset: 0x000CCEB4
		internal static bool ValidLogName(string logName, bool ignoreEmpty)
		{
			if (logName.Length == 0 && !ignoreEmpty)
			{
				return false;
			}
			foreach (char c in logName)
			{
				if (!EventLog.CharIsPrintable(c) || c == '\\' || c == '*' || c == '?')
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002DEF RID: 11759 RVA: 0x000CED03 File Offset: 0x000CCF03
		public void WriteEntry(string message)
		{
			this.WriteEntry(message, EventLogEntryType.Information, 0, 0, null);
		}

		// Token: 0x06002DF0 RID: 11760 RVA: 0x000CED10 File Offset: 0x000CCF10
		public static void WriteEntry(string source, string message)
		{
			EventLog.WriteEntry(source, message, EventLogEntryType.Information, 0, 0, null);
		}

		// Token: 0x06002DF1 RID: 11761 RVA: 0x000CED1D File Offset: 0x000CCF1D
		public void WriteEntry(string message, EventLogEntryType type)
		{
			this.WriteEntry(message, type, 0, 0, null);
		}

		// Token: 0x06002DF2 RID: 11762 RVA: 0x000CED2A File Offset: 0x000CCF2A
		public static void WriteEntry(string source, string message, EventLogEntryType type)
		{
			EventLog.WriteEntry(source, message, type, 0, 0, null);
		}

		// Token: 0x06002DF3 RID: 11763 RVA: 0x000CED37 File Offset: 0x000CCF37
		public void WriteEntry(string message, EventLogEntryType type, int eventID)
		{
			this.WriteEntry(message, type, eventID, 0, null);
		}

		// Token: 0x06002DF4 RID: 11764 RVA: 0x000CED44 File Offset: 0x000CCF44
		public static void WriteEntry(string source, string message, EventLogEntryType type, int eventID)
		{
			EventLog.WriteEntry(source, message, type, eventID, 0, null);
		}

		// Token: 0x06002DF5 RID: 11765 RVA: 0x000CED51 File Offset: 0x000CCF51
		public void WriteEntry(string message, EventLogEntryType type, int eventID, short category)
		{
			this.WriteEntry(message, type, eventID, category, null);
		}

		// Token: 0x06002DF6 RID: 11766 RVA: 0x000CED5F File Offset: 0x000CCF5F
		public static void WriteEntry(string source, string message, EventLogEntryType type, int eventID, short category)
		{
			EventLog.WriteEntry(source, message, type, eventID, category, null);
		}

		// Token: 0x06002DF7 RID: 11767 RVA: 0x000CED70 File Offset: 0x000CCF70
		public static void WriteEntry(string source, string message, EventLogEntryType type, int eventID, short category, byte[] rawData)
		{
			using (EventLogInternal eventLogInternal = new EventLogInternal("", ".", EventLog.CheckAndNormalizeSourceName(source)))
			{
				eventLogInternal.WriteEntry(message, type, eventID, category, rawData);
			}
		}

		// Token: 0x06002DF8 RID: 11768 RVA: 0x000CEDBC File Offset: 0x000CCFBC
		public void WriteEntry(string message, EventLogEntryType type, int eventID, short category, byte[] rawData)
		{
			this.m_underlyingEventLog.WriteEntry(message, type, eventID, category, rawData);
		}

		// Token: 0x06002DF9 RID: 11769 RVA: 0x000CEDD0 File Offset: 0x000CCFD0
		[ComVisible(false)]
		public void WriteEvent(EventInstance instance, params object[] values)
		{
			this.WriteEvent(instance, null, values);
		}

		// Token: 0x06002DFA RID: 11770 RVA: 0x000CEDDB File Offset: 0x000CCFDB
		[ComVisible(false)]
		public void WriteEvent(EventInstance instance, byte[] data, params object[] values)
		{
			this.m_underlyingEventLog.WriteEvent(instance, data, values);
		}

		// Token: 0x06002DFB RID: 11771 RVA: 0x000CEDEC File Offset: 0x000CCFEC
		public static void WriteEvent(string source, EventInstance instance, params object[] values)
		{
			using (EventLogInternal eventLogInternal = new EventLogInternal("", ".", EventLog.CheckAndNormalizeSourceName(source)))
			{
				eventLogInternal.WriteEvent(instance, null, values);
			}
		}

		// Token: 0x06002DFC RID: 11772 RVA: 0x000CEE34 File Offset: 0x000CD034
		public static void WriteEvent(string source, EventInstance instance, byte[] data, params object[] values)
		{
			using (EventLogInternal eventLogInternal = new EventLogInternal("", ".", EventLog.CheckAndNormalizeSourceName(source)))
			{
				eventLogInternal.WriteEvent(instance, data, values);
			}
		}

		// Token: 0x06002DFD RID: 11773 RVA: 0x000CEE7C File Offset: 0x000CD07C
		private static string CheckAndNormalizeSourceName(string source)
		{
			if (source == null)
			{
				source = string.Empty;
			}
			if (source.Length + "SYSTEM\\CurrentControlSet\\Services\\EventLog".Length > 254)
			{
				throw new ArgumentException(SR.GetString("ParameterTooLong", new object[]
				{
					"source",
					254 - "SYSTEM\\CurrentControlSet\\Services\\EventLog".Length
				}));
			}
			return source;
		}

		// Token: 0x0400272C RID: 10028
		private const string EventLogKey = "SYSTEM\\CurrentControlSet\\Services\\EventLog";

		// Token: 0x0400272D RID: 10029
		internal const string DllName = "EventLogMessages.dll";

		// Token: 0x0400272E RID: 10030
		private const string eventLogMutexName = "netfxeventlog.1.0";

		// Token: 0x0400272F RID: 10031
		private const int DefaultMaxSize = 524288;

		// Token: 0x04002730 RID: 10032
		private const int DefaultRetention = 604800;

		// Token: 0x04002731 RID: 10033
		private const int SecondsPerDay = 86400;

		// Token: 0x04002732 RID: 10034
		private EventLogInternal m_underlyingEventLog;

		// Token: 0x04002733 RID: 10035
		private static volatile bool s_CheckedOsVersion;

		// Token: 0x04002734 RID: 10036
		private static volatile bool s_SkipRegPatch;

		// Token: 0x04002735 RID: 10037
		private static readonly bool s_dontFilterRegKeys = !EventLog.IsWindowsRS5OrUp() || LocalAppContextSwitches.DisableEventLogRegistryKeysFiltering;
	}
}
