using System;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x020004A5 RID: 1189
	public class SourceSwitch : Switch
	{
		// Token: 0x06002C00 RID: 11264 RVA: 0x000C6BDF File Offset: 0x000C4DDF
		public SourceSwitch(string name)
			: base(name, string.Empty)
		{
		}

		// Token: 0x06002C01 RID: 11265 RVA: 0x000C6BED File Offset: 0x000C4DED
		public SourceSwitch(string displayName, string defaultSwitchValue)
			: base(displayName, string.Empty, defaultSwitchValue)
		{
		}

		// Token: 0x17000AA7 RID: 2727
		// (get) Token: 0x06002C02 RID: 11266 RVA: 0x000C6BFC File Offset: 0x000C4DFC
		// (set) Token: 0x06002C03 RID: 11267 RVA: 0x000C6C04 File Offset: 0x000C4E04
		public SourceLevels Level
		{
			get
			{
				return (SourceLevels)base.SwitchSetting;
			}
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			set
			{
				base.SwitchSetting = (int)value;
			}
		}

		// Token: 0x06002C04 RID: 11268 RVA: 0x000C6C0D File Offset: 0x000C4E0D
		public bool ShouldTrace(TraceEventType eventType)
		{
			return (base.SwitchSetting & (int)eventType) != 0;
		}

		// Token: 0x06002C05 RID: 11269 RVA: 0x000C6C1A File Offset: 0x000C4E1A
		protected override void OnValueChanged()
		{
			base.SwitchSetting = (int)Enum.Parse(typeof(SourceLevels), base.Value, true);
		}
	}
}
