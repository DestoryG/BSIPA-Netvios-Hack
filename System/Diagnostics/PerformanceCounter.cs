using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security.Permissions;
using System.Threading;

namespace System.Diagnostics
{
	// Token: 0x020004DE RID: 1246
	[InstallerType("System.Diagnostics.PerformanceCounterInstaller,System.Configuration.Install, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("PerformanceCounterDesc")]
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true, SharedState = true)]
	public sealed class PerformanceCounter : Component, ISupportInitialize
	{
		// Token: 0x17000B6F RID: 2927
		// (get) Token: 0x06002EEF RID: 12015 RVA: 0x000D2AB0 File Offset: 0x000D0CB0
		private object InstanceLockObject
		{
			get
			{
				if (this.m_InstanceLockObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref this.m_InstanceLockObject, obj, null);
				}
				return this.m_InstanceLockObject;
			}
		}

		// Token: 0x06002EF0 RID: 12016 RVA: 0x000D2AE0 File Offset: 0x000D0CE0
		public PerformanceCounter()
		{
			this.machineName = ".";
			this.categoryName = string.Empty;
			this.counterName = string.Empty;
			this.instanceName = string.Empty;
			this.isReadOnly = true;
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002EF1 RID: 12017 RVA: 0x000D2B40 File Offset: 0x000D0D40
		public PerformanceCounter(string categoryName, string counterName, string instanceName, string machineName)
		{
			this.MachineName = machineName;
			this.CategoryName = categoryName;
			this.CounterName = counterName;
			this.InstanceName = instanceName;
			this.isReadOnly = true;
			this.Initialize();
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002EF2 RID: 12018 RVA: 0x000D2B98 File Offset: 0x000D0D98
		internal PerformanceCounter(string categoryName, string counterName, string instanceName, string machineName, bool skipInit)
		{
			this.MachineName = machineName;
			this.CategoryName = categoryName;
			this.CounterName = counterName;
			this.InstanceName = instanceName;
			this.isReadOnly = true;
			this.initialized = true;
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002EF3 RID: 12019 RVA: 0x000D2BEE File Offset: 0x000D0DEE
		public PerformanceCounter(string categoryName, string counterName, string instanceName)
			: this(categoryName, counterName, instanceName, true)
		{
		}

		// Token: 0x06002EF4 RID: 12020 RVA: 0x000D2BFC File Offset: 0x000D0DFC
		public PerformanceCounter(string categoryName, string counterName, string instanceName, bool readOnly)
		{
			if (!readOnly)
			{
				PerformanceCounter.VerifyWriteableCounterAllowed();
			}
			this.MachineName = ".";
			this.CategoryName = categoryName;
			this.CounterName = counterName;
			this.InstanceName = instanceName;
			this.isReadOnly = readOnly;
			this.Initialize();
			GC.SuppressFinalize(this);
		}

		// Token: 0x06002EF5 RID: 12021 RVA: 0x000D2C5E File Offset: 0x000D0E5E
		public PerformanceCounter(string categoryName, string counterName)
			: this(categoryName, counterName, true)
		{
		}

		// Token: 0x06002EF6 RID: 12022 RVA: 0x000D2C69 File Offset: 0x000D0E69
		public PerformanceCounter(string categoryName, string counterName, bool readOnly)
			: this(categoryName, counterName, "", readOnly)
		{
		}

		// Token: 0x17000B70 RID: 2928
		// (get) Token: 0x06002EF7 RID: 12023 RVA: 0x000D2C79 File Offset: 0x000D0E79
		// (set) Token: 0x06002EF8 RID: 12024 RVA: 0x000D2C81 File Offset: 0x000D0E81
		[ReadOnly(true)]
		[DefaultValue("")]
		[TypeConverter("System.Diagnostics.Design.CategoryValueConverter, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[SRDescription("PCCategoryName")]
		[SettingsBindable(true)]
		public string CategoryName
		{
			get
			{
				return this.categoryName;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (this.categoryName == null || string.Compare(this.categoryName, value, StringComparison.OrdinalIgnoreCase) != 0)
				{
					this.categoryName = value;
					this.Close();
				}
			}
		}

		// Token: 0x17000B71 RID: 2929
		// (get) Token: 0x06002EF9 RID: 12025 RVA: 0x000D2CB8 File Offset: 0x000D0EB8
		[ReadOnly(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("PC_CounterHelp")]
		public string CounterHelp
		{
			get
			{
				string text = this.categoryName;
				string text2 = this.machineName;
				PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Browse, text2, text);
				performanceCounterPermission.Demand();
				this.Initialize();
				if (this.helpMsg == null)
				{
					this.helpMsg = PerformanceCounterLib.GetCounterHelp(text2, text, this.counterName);
				}
				return this.helpMsg;
			}
		}

		// Token: 0x17000B72 RID: 2930
		// (get) Token: 0x06002EFA RID: 12026 RVA: 0x000D2D09 File Offset: 0x000D0F09
		// (set) Token: 0x06002EFB RID: 12027 RVA: 0x000D2D11 File Offset: 0x000D0F11
		[ReadOnly(true)]
		[DefaultValue("")]
		[TypeConverter("System.Diagnostics.Design.CounterNameConverter, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[SRDescription("PCCounterName")]
		[SettingsBindable(true)]
		public string CounterName
		{
			get
			{
				return this.counterName;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (this.counterName == null || string.Compare(this.counterName, value, StringComparison.OrdinalIgnoreCase) != 0)
				{
					this.counterName = value;
					this.Close();
				}
			}
		}

		// Token: 0x17000B73 RID: 2931
		// (get) Token: 0x06002EFC RID: 12028 RVA: 0x000D2D48 File Offset: 0x000D0F48
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("PC_CounterType")]
		public PerformanceCounterType CounterType
		{
			get
			{
				if (this.counterType == -1)
				{
					string text = this.categoryName;
					string text2 = this.machineName;
					PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Browse, text2, text);
					performanceCounterPermission.Demand();
					this.Initialize();
					CategorySample categorySample = PerformanceCounterLib.GetCategorySample(text2, text);
					CounterDefinitionSample counterDefinitionSample = categorySample.GetCounterDefinitionSample(this.counterName);
					this.counterType = counterDefinitionSample.CounterType;
				}
				return (PerformanceCounterType)this.counterType;
			}
		}

		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x06002EFD RID: 12029 RVA: 0x000D2DAA File Offset: 0x000D0FAA
		// (set) Token: 0x06002EFE RID: 12030 RVA: 0x000D2DB2 File Offset: 0x000D0FB2
		[DefaultValue(PerformanceCounterInstanceLifetime.Global)]
		[SRDescription("PCInstanceLifetime")]
		public PerformanceCounterInstanceLifetime InstanceLifetime
		{
			get
			{
				return this.instanceLifetime;
			}
			set
			{
				if (value > PerformanceCounterInstanceLifetime.Process || value < PerformanceCounterInstanceLifetime.Global)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (this.initialized)
				{
					throw new InvalidOperationException(SR.GetString("CantSetLifetimeAfterInitialized"));
				}
				this.instanceLifetime = value;
			}
		}

		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x06002EFF RID: 12031 RVA: 0x000D2DE6 File Offset: 0x000D0FE6
		// (set) Token: 0x06002F00 RID: 12032 RVA: 0x000D2DEE File Offset: 0x000D0FEE
		[ReadOnly(true)]
		[DefaultValue("")]
		[TypeConverter("System.Diagnostics.Design.InstanceNameConverter, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[SRDescription("PCInstanceName")]
		[SettingsBindable(true)]
		public string InstanceName
		{
			get
			{
				return this.instanceName;
			}
			set
			{
				if (value == null && this.instanceName == null)
				{
					return;
				}
				if ((value == null && this.instanceName != null) || (value != null && this.instanceName == null) || string.Compare(this.instanceName, value, StringComparison.OrdinalIgnoreCase) != 0)
				{
					this.instanceName = value;
					this.Close();
				}
			}
		}

		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x06002F01 RID: 12033 RVA: 0x000D2E2E File Offset: 0x000D102E
		// (set) Token: 0x06002F02 RID: 12034 RVA: 0x000D2E36 File Offset: 0x000D1036
		[Browsable(false)]
		[DefaultValue(true)]
		[MonitoringDescription("PC_ReadOnly")]
		public bool ReadOnly
		{
			get
			{
				return this.isReadOnly;
			}
			set
			{
				if (value != this.isReadOnly)
				{
					if (!value)
					{
						PerformanceCounter.VerifyWriteableCounterAllowed();
					}
					this.isReadOnly = value;
					this.Close();
				}
			}
		}

		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x06002F03 RID: 12035 RVA: 0x000D2E56 File Offset: 0x000D1056
		// (set) Token: 0x06002F04 RID: 12036 RVA: 0x000D2E60 File Offset: 0x000D1060
		[Browsable(false)]
		[DefaultValue(".")]
		[SRDescription("PCMachineName")]
		[SettingsBindable(true)]
		public string MachineName
		{
			get
			{
				return this.machineName;
			}
			set
			{
				if (!SyntaxCheck.CheckMachineName(value))
				{
					throw new ArgumentException(SR.GetString("InvalidParameter", new object[] { "machineName", value }));
				}
				if (this.machineName != value)
				{
					this.machineName = value;
					this.Close();
				}
			}
		}

		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x06002F05 RID: 12037 RVA: 0x000D2EB4 File Offset: 0x000D10B4
		// (set) Token: 0x06002F06 RID: 12038 RVA: 0x000D2EE9 File Offset: 0x000D10E9
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("PC_RawValue")]
		public long RawValue
		{
			get
			{
				if (this.ReadOnly)
				{
					return this.NextSample().RawValue;
				}
				this.Initialize();
				return this.sharedCounter.Value;
			}
			set
			{
				if (this.ReadOnly)
				{
					this.ThrowReadOnly();
				}
				this.Initialize();
				this.sharedCounter.Value = value;
			}
		}

		// Token: 0x06002F07 RID: 12039 RVA: 0x000D2F0B File Offset: 0x000D110B
		public void BeginInit()
		{
			this.Close();
		}

		// Token: 0x06002F08 RID: 12040 RVA: 0x000D2F13 File Offset: 0x000D1113
		public void Close()
		{
			this.helpMsg = null;
			this.oldSample = CounterSample.Empty;
			this.sharedCounter = null;
			this.initialized = false;
			this.counterType = -1;
		}

		// Token: 0x06002F09 RID: 12041 RVA: 0x000D2F3C File Offset: 0x000D113C
		public static void CloseSharedResources()
		{
			PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Browse, ".", "*");
			performanceCounterPermission.Demand();
			PerformanceCounterLib.CloseAllLibraries();
		}

		// Token: 0x06002F0A RID: 12042 RVA: 0x000D2F65 File Offset: 0x000D1165
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.Close();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06002F0B RID: 12043 RVA: 0x000D2F77 File Offset: 0x000D1177
		public long Decrement()
		{
			if (this.ReadOnly)
			{
				this.ThrowReadOnly();
			}
			this.Initialize();
			return this.sharedCounter.Decrement();
		}

		// Token: 0x06002F0C RID: 12044 RVA: 0x000D2F98 File Offset: 0x000D1198
		public void EndInit()
		{
			this.Initialize();
		}

		// Token: 0x06002F0D RID: 12045 RVA: 0x000D2FA0 File Offset: 0x000D11A0
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public long IncrementBy(long value)
		{
			if (this.isReadOnly)
			{
				this.ThrowReadOnly();
			}
			this.Initialize();
			return this.sharedCounter.IncrementBy(value);
		}

		// Token: 0x06002F0E RID: 12046 RVA: 0x000D2FC2 File Offset: 0x000D11C2
		public long Increment()
		{
			if (this.isReadOnly)
			{
				this.ThrowReadOnly();
			}
			this.Initialize();
			return this.sharedCounter.Increment();
		}

		// Token: 0x06002F0F RID: 12047 RVA: 0x000D2FE3 File Offset: 0x000D11E3
		private void ThrowReadOnly()
		{
			throw new InvalidOperationException(SR.GetString("ReadOnlyCounter"));
		}

		// Token: 0x06002F10 RID: 12048 RVA: 0x000D2FF4 File Offset: 0x000D11F4
		private static void VerifyWriteableCounterAllowed()
		{
			if (EnvironmentHelpers.IsAppContainerProcess)
			{
				throw new NotSupportedException(SR.GetString("PCNotSupportedUnderAppContainer"));
			}
		}

		// Token: 0x06002F11 RID: 12049 RVA: 0x000D300D File Offset: 0x000D120D
		private void Initialize()
		{
			if (!this.initialized && !base.DesignMode)
			{
				this.InitializeImpl();
			}
		}

		// Token: 0x06002F12 RID: 12050 RVA: 0x000D3028 File Offset: 0x000D1228
		private void InitializeImpl()
		{
			bool flag = false;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				Monitor.Enter(this.InstanceLockObject, ref flag);
				if (!this.initialized)
				{
					string text = this.categoryName;
					string text2 = this.machineName;
					if (text == string.Empty)
					{
						throw new InvalidOperationException(SR.GetString("CategoryNameMissing"));
					}
					if (this.counterName == string.Empty)
					{
						throw new InvalidOperationException(SR.GetString("CounterNameMissing"));
					}
					if (this.ReadOnly)
					{
						PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Browse, text2, text);
						performanceCounterPermission.Demand();
						if (!PerformanceCounterLib.CounterExists(text2, text, this.counterName))
						{
							throw new InvalidOperationException(SR.GetString("CounterExists", new object[] { text, this.counterName }));
						}
						PerformanceCounterCategoryType categoryType = PerformanceCounterLib.GetCategoryType(text2, text);
						if (categoryType == PerformanceCounterCategoryType.MultiInstance)
						{
							if (string.IsNullOrEmpty(this.instanceName))
							{
								throw new InvalidOperationException(SR.GetString("MultiInstanceOnly", new object[] { text }));
							}
						}
						else if (categoryType == PerformanceCounterCategoryType.SingleInstance && !string.IsNullOrEmpty(this.instanceName))
						{
							throw new InvalidOperationException(SR.GetString("SingleInstanceOnly", new object[] { text }));
						}
						if (this.instanceLifetime != PerformanceCounterInstanceLifetime.Global)
						{
							throw new InvalidOperationException(SR.GetString("InstanceLifetimeProcessonReadOnly"));
						}
						this.initialized = true;
					}
					else
					{
						PerformanceCounterPermission performanceCounterPermission2 = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Write, text2, text);
						performanceCounterPermission2.Demand();
						if (text2 != "." && string.Compare(text2, PerformanceCounterLib.ComputerName, StringComparison.OrdinalIgnoreCase) != 0)
						{
							throw new InvalidOperationException(SR.GetString("RemoteWriting"));
						}
						SharedUtils.CheckNtEnvironment();
						if (!PerformanceCounterLib.IsCustomCategory(text2, text))
						{
							throw new InvalidOperationException(SR.GetString("NotCustomCounter"));
						}
						PerformanceCounterCategoryType categoryType2 = PerformanceCounterLib.GetCategoryType(text2, text);
						if (categoryType2 == PerformanceCounterCategoryType.MultiInstance)
						{
							if (string.IsNullOrEmpty(this.instanceName))
							{
								throw new InvalidOperationException(SR.GetString("MultiInstanceOnly", new object[] { text }));
							}
						}
						else if (categoryType2 == PerformanceCounterCategoryType.SingleInstance && !string.IsNullOrEmpty(this.instanceName))
						{
							throw new InvalidOperationException(SR.GetString("SingleInstanceOnly", new object[] { text }));
						}
						if (string.IsNullOrEmpty(this.instanceName) && this.InstanceLifetime == PerformanceCounterInstanceLifetime.Process)
						{
							throw new InvalidOperationException(SR.GetString("InstanceLifetimeProcessforSingleInstance"));
						}
						this.sharedCounter = new SharedPerformanceCounter(text.ToLower(CultureInfo.InvariantCulture), this.counterName.ToLower(CultureInfo.InvariantCulture), this.instanceName.ToLower(CultureInfo.InvariantCulture), this.instanceLifetime);
						this.initialized = true;
					}
				}
			}
			finally
			{
				if (flag)
				{
					Monitor.Exit(this.InstanceLockObject);
				}
			}
		}

		// Token: 0x06002F13 RID: 12051 RVA: 0x000D32C4 File Offset: 0x000D14C4
		public CounterSample NextSample()
		{
			string text = this.categoryName;
			string text2 = this.machineName;
			PerformanceCounterPermission performanceCounterPermission = new PerformanceCounterPermission(PerformanceCounterPermissionAccess.Browse, text2, text);
			performanceCounterPermission.Demand();
			this.Initialize();
			CategorySample categorySample = PerformanceCounterLib.GetCategorySample(text2, text);
			CounterDefinitionSample counterDefinitionSample = categorySample.GetCounterDefinitionSample(this.counterName);
			this.counterType = counterDefinitionSample.CounterType;
			if (!categorySample.IsMultiInstance)
			{
				if (this.instanceName != null && this.instanceName.Length != 0)
				{
					throw new InvalidOperationException(SR.GetString("InstanceNameProhibited", new object[] { this.instanceName }));
				}
				return counterDefinitionSample.GetSingleValue();
			}
			else
			{
				if (this.instanceName == null || this.instanceName.Length == 0)
				{
					throw new InvalidOperationException(SR.GetString("InstanceNameRequired"));
				}
				return counterDefinitionSample.GetInstanceValue(this.instanceName);
			}
		}

		// Token: 0x06002F14 RID: 12052 RVA: 0x000D3390 File Offset: 0x000D1590
		public float NextValue()
		{
			CounterSample counterSample = this.NextSample();
			float num = CounterSample.Calculate(this.oldSample, counterSample);
			this.oldSample = counterSample;
			return num;
		}

		// Token: 0x06002F15 RID: 12053 RVA: 0x000D33C0 File Offset: 0x000D15C0
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public void RemoveInstance()
		{
			if (this.isReadOnly)
			{
				throw new InvalidOperationException(SR.GetString("ReadOnlyRemoveInstance"));
			}
			this.Initialize();
			this.sharedCounter.RemoveInstance(this.instanceName.ToLower(CultureInfo.InvariantCulture), this.instanceLifetime);
		}

		// Token: 0x0400279D RID: 10141
		private string machineName;

		// Token: 0x0400279E RID: 10142
		private string categoryName;

		// Token: 0x0400279F RID: 10143
		private string counterName;

		// Token: 0x040027A0 RID: 10144
		private string instanceName;

		// Token: 0x040027A1 RID: 10145
		private PerformanceCounterInstanceLifetime instanceLifetime;

		// Token: 0x040027A2 RID: 10146
		private bool isReadOnly;

		// Token: 0x040027A3 RID: 10147
		private bool initialized;

		// Token: 0x040027A4 RID: 10148
		private string helpMsg;

		// Token: 0x040027A5 RID: 10149
		private int counterType = -1;

		// Token: 0x040027A6 RID: 10150
		private CounterSample oldSample = CounterSample.Empty;

		// Token: 0x040027A7 RID: 10151
		private SharedPerformanceCounter sharedCounter;

		// Token: 0x040027A8 RID: 10152
		[Obsolete("This field has been deprecated and is not used.  Use machine.config or an application configuration file to set the size of the PerformanceCounter file mapping.")]
		public static int DefaultFileMappingSize = 524288;

		// Token: 0x040027A9 RID: 10153
		private object m_InstanceLockObject;
	}
}
