using System;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x020004B8 RID: 1208
	[SwitchLevel(typeof(TraceLevel))]
	public class TraceSwitch : Switch
	{
		// Token: 0x06002D1C RID: 11548 RVA: 0x000CB11D File Offset: 0x000C931D
		public TraceSwitch(string displayName, string description)
			: base(displayName, description)
		{
		}

		// Token: 0x06002D1D RID: 11549 RVA: 0x000CB127 File Offset: 0x000C9327
		public TraceSwitch(string displayName, string description, string defaultSwitchValue)
			: base(displayName, description, defaultSwitchValue)
		{
		}

		// Token: 0x17000AEB RID: 2795
		// (get) Token: 0x06002D1E RID: 11550 RVA: 0x000CB132 File Offset: 0x000C9332
		// (set) Token: 0x06002D1F RID: 11551 RVA: 0x000CB13A File Offset: 0x000C933A
		public TraceLevel Level
		{
			get
			{
				return (TraceLevel)base.SwitchSetting;
			}
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			set
			{
				if (value < TraceLevel.Off || value > TraceLevel.Verbose)
				{
					throw new ArgumentException(SR.GetString("TraceSwitchInvalidLevel"));
				}
				base.SwitchSetting = (int)value;
			}
		}

		// Token: 0x17000AEC RID: 2796
		// (get) Token: 0x06002D20 RID: 11552 RVA: 0x000CB15B File Offset: 0x000C935B
		public bool TraceError
		{
			get
			{
				return this.Level >= TraceLevel.Error;
			}
		}

		// Token: 0x17000AED RID: 2797
		// (get) Token: 0x06002D21 RID: 11553 RVA: 0x000CB169 File Offset: 0x000C9369
		public bool TraceWarning
		{
			get
			{
				return this.Level >= TraceLevel.Warning;
			}
		}

		// Token: 0x17000AEE RID: 2798
		// (get) Token: 0x06002D22 RID: 11554 RVA: 0x000CB177 File Offset: 0x000C9377
		public bool TraceInfo
		{
			get
			{
				return this.Level >= TraceLevel.Info;
			}
		}

		// Token: 0x17000AEF RID: 2799
		// (get) Token: 0x06002D23 RID: 11555 RVA: 0x000CB185 File Offset: 0x000C9385
		public bool TraceVerbose
		{
			get
			{
				return this.Level == TraceLevel.Verbose;
			}
		}

		// Token: 0x06002D24 RID: 11556 RVA: 0x000CB190 File Offset: 0x000C9390
		protected override void OnSwitchSettingChanged()
		{
			int switchSetting = base.SwitchSetting;
			if (switchSetting < 0)
			{
				Trace.WriteLine(SR.GetString("TraceSwitchLevelTooLow", new object[] { base.DisplayName }));
				base.SwitchSetting = 0;
				return;
			}
			if (switchSetting > 4)
			{
				Trace.WriteLine(SR.GetString("TraceSwitchLevelTooHigh", new object[] { base.DisplayName }));
				base.SwitchSetting = 4;
			}
		}

		// Token: 0x06002D25 RID: 11557 RVA: 0x000CB1F7 File Offset: 0x000C93F7
		protected override void OnValueChanged()
		{
			base.SwitchSetting = (int)Enum.Parse(typeof(TraceLevel), base.Value, true);
		}
	}
}
