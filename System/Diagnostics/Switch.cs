using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Xml.Serialization;

namespace System.Diagnostics
{
	// Token: 0x020004A6 RID: 1190
	public abstract class Switch
	{
		// Token: 0x06002C06 RID: 11270 RVA: 0x000C6C3D File Offset: 0x000C4E3D
		protected Switch(string displayName, string description)
			: this(displayName, description, "0")
		{
		}

		// Token: 0x06002C07 RID: 11271 RVA: 0x000C6C4C File Offset: 0x000C4E4C
		protected Switch(string displayName, string description, string defaultSwitchValue)
		{
			if (displayName == null)
			{
				displayName = string.Empty;
			}
			this.displayName = displayName;
			this.description = description;
			List<WeakReference> list = Switch.switches;
			lock (list)
			{
				Switch._pruneCachedSwitches();
				Switch.switches.Add(new WeakReference(this));
			}
			this.defaultValue = defaultSwitchValue;
		}

		// Token: 0x06002C08 RID: 11272 RVA: 0x000C6CCC File Offset: 0x000C4ECC
		private static void _pruneCachedSwitches()
		{
			List<WeakReference> list = Switch.switches;
			lock (list)
			{
				if (Switch.s_LastCollectionCount != GC.CollectionCount(2))
				{
					List<WeakReference> list2 = new List<WeakReference>(Switch.switches.Count);
					for (int i = 0; i < Switch.switches.Count; i++)
					{
						Switch @switch = (Switch)Switch.switches[i].Target;
						if (@switch != null)
						{
							list2.Add(Switch.switches[i]);
						}
					}
					if (list2.Count < Switch.switches.Count)
					{
						Switch.switches.Clear();
						Switch.switches.AddRange(list2);
						Switch.switches.TrimExcess();
					}
					Switch.s_LastCollectionCount = GC.CollectionCount(2);
				}
			}
		}

		// Token: 0x17000AA8 RID: 2728
		// (get) Token: 0x06002C09 RID: 11273 RVA: 0x000C6DA4 File Offset: 0x000C4FA4
		[XmlIgnore]
		public StringDictionary Attributes
		{
			get
			{
				this.Initialize();
				if (this.attributes == null)
				{
					this.attributes = new StringDictionary();
				}
				return this.attributes;
			}
		}

		// Token: 0x17000AA9 RID: 2729
		// (get) Token: 0x06002C0A RID: 11274 RVA: 0x000C6DC5 File Offset: 0x000C4FC5
		public string DisplayName
		{
			get
			{
				return this.displayName;
			}
		}

		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x06002C0B RID: 11275 RVA: 0x000C6DCD File Offset: 0x000C4FCD
		public string Description
		{
			get
			{
				if (this.description != null)
				{
					return this.description;
				}
				return string.Empty;
			}
		}

		// Token: 0x17000AAB RID: 2731
		// (get) Token: 0x06002C0C RID: 11276 RVA: 0x000C6DE3 File Offset: 0x000C4FE3
		// (set) Token: 0x06002C0D RID: 11277 RVA: 0x000C6E04 File Offset: 0x000C5004
		protected int SwitchSetting
		{
			get
			{
				if (!this.initialized && this.InitializeWithStatus())
				{
					this.OnSwitchSettingChanged();
				}
				return this.switchSetting;
			}
			set
			{
				bool flag = false;
				object critSec = TraceInternal.critSec;
				lock (critSec)
				{
					this.initialized = true;
					if (this.switchSetting != value)
					{
						this.switchSetting = value;
						flag = true;
					}
				}
				if (flag)
				{
					this.OnSwitchSettingChanged();
				}
			}
		}

		// Token: 0x17000AAC RID: 2732
		// (get) Token: 0x06002C0E RID: 11278 RVA: 0x000C6E64 File Offset: 0x000C5064
		// (set) Token: 0x06002C0F RID: 11279 RVA: 0x000C6E74 File Offset: 0x000C5074
		protected string Value
		{
			get
			{
				this.Initialize();
				return this.switchValueString;
			}
			set
			{
				this.Initialize();
				this.switchValueString = value;
				try
				{
					this.OnValueChanged();
				}
				catch (ArgumentException ex)
				{
					throw new ConfigurationErrorsException(SR.GetString("BadConfigSwitchValue", new object[] { this.DisplayName }), ex);
				}
				catch (FormatException ex2)
				{
					throw new ConfigurationErrorsException(SR.GetString("BadConfigSwitchValue", new object[] { this.DisplayName }), ex2);
				}
				catch (OverflowException ex3)
				{
					throw new ConfigurationErrorsException(SR.GetString("BadConfigSwitchValue", new object[] { this.DisplayName }), ex3);
				}
			}
		}

		// Token: 0x06002C10 RID: 11280 RVA: 0x000C6F24 File Offset: 0x000C5124
		private void Initialize()
		{
			this.InitializeWithStatus();
		}

		// Token: 0x06002C11 RID: 11281 RVA: 0x000C6F30 File Offset: 0x000C5130
		private bool InitializeWithStatus()
		{
			if (!this.initialized)
			{
				object critSec = TraceInternal.critSec;
				lock (critSec)
				{
					if (this.initialized || this.initializing)
					{
						return false;
					}
					this.initializing = true;
					if (this.switchSettings == null && !this.InitializeConfigSettings())
					{
						this.initialized = true;
						this.initializing = false;
						return false;
					}
					if (this.switchSettings != null)
					{
						SwitchElement switchElement = this.switchSettings[this.displayName];
						if (switchElement != null)
						{
							string value = switchElement.Value;
							if (value != null)
							{
								this.Value = value;
							}
							else
							{
								this.Value = this.defaultValue;
							}
							try
							{
								TraceUtils.VerifyAttributes(switchElement.Attributes, this.GetSupportedAttributes(), this);
							}
							catch (ConfigurationException)
							{
								this.initialized = false;
								this.initializing = false;
								throw;
							}
							this.attributes = new StringDictionary();
							this.attributes.ReplaceHashtable(switchElement.Attributes);
						}
						else
						{
							this.switchValueString = this.defaultValue;
							this.OnValueChanged();
						}
					}
					else
					{
						this.switchValueString = this.defaultValue;
						this.OnValueChanged();
					}
					this.initialized = true;
					this.initializing = false;
				}
				return true;
			}
			return true;
		}

		// Token: 0x06002C12 RID: 11282 RVA: 0x000C70A8 File Offset: 0x000C52A8
		private bool InitializeConfigSettings()
		{
			if (this.switchSettings != null)
			{
				return true;
			}
			if (!DiagnosticsConfiguration.CanInitialize())
			{
				return false;
			}
			this.switchSettings = DiagnosticsConfiguration.SwitchSettings;
			return true;
		}

		// Token: 0x06002C13 RID: 11283 RVA: 0x000C70C9 File Offset: 0x000C52C9
		protected internal virtual string[] GetSupportedAttributes()
		{
			return null;
		}

		// Token: 0x06002C14 RID: 11284 RVA: 0x000C70CC File Offset: 0x000C52CC
		protected virtual void OnSwitchSettingChanged()
		{
		}

		// Token: 0x06002C15 RID: 11285 RVA: 0x000C70CE File Offset: 0x000C52CE
		protected virtual void OnValueChanged()
		{
			this.SwitchSetting = int.Parse(this.Value, CultureInfo.InvariantCulture);
		}

		// Token: 0x06002C16 RID: 11286 RVA: 0x000C70E8 File Offset: 0x000C52E8
		internal static void RefreshAll()
		{
			List<WeakReference> list = Switch.switches;
			lock (list)
			{
				Switch._pruneCachedSwitches();
				for (int i = 0; i < Switch.switches.Count; i++)
				{
					Switch @switch = (Switch)Switch.switches[i].Target;
					if (@switch != null)
					{
						@switch.Refresh();
					}
				}
			}
		}

		// Token: 0x06002C17 RID: 11287 RVA: 0x000C715C File Offset: 0x000C535C
		internal void Refresh()
		{
			object critSec = TraceInternal.critSec;
			lock (critSec)
			{
				this.initialized = false;
				this.switchSettings = null;
				this.Initialize();
			}
		}

		// Token: 0x040026A5 RID: 9893
		private SwitchElementsCollection switchSettings;

		// Token: 0x040026A6 RID: 9894
		private readonly string description;

		// Token: 0x040026A7 RID: 9895
		private readonly string displayName;

		// Token: 0x040026A8 RID: 9896
		private int switchSetting;

		// Token: 0x040026A9 RID: 9897
		private volatile bool initialized;

		// Token: 0x040026AA RID: 9898
		private bool initializing;

		// Token: 0x040026AB RID: 9899
		private volatile string switchValueString = string.Empty;

		// Token: 0x040026AC RID: 9900
		private StringDictionary attributes;

		// Token: 0x040026AD RID: 9901
		private string defaultValue;

		// Token: 0x040026AE RID: 9902
		private static List<WeakReference> switches = new List<WeakReference>();

		// Token: 0x040026AF RID: 9903
		private static int s_LastCollectionCount;
	}
}
