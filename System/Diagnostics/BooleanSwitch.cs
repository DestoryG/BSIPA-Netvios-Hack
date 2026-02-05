using System;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x02000493 RID: 1171
	[SwitchLevel(typeof(bool))]
	public class BooleanSwitch : Switch
	{
		// Token: 0x06002B56 RID: 11094 RVA: 0x000C4DCB File Offset: 0x000C2FCB
		public BooleanSwitch(string displayName, string description)
			: base(displayName, description)
		{
		}

		// Token: 0x06002B57 RID: 11095 RVA: 0x000C4DD5 File Offset: 0x000C2FD5
		public BooleanSwitch(string displayName, string description, string defaultSwitchValue)
			: base(displayName, description, defaultSwitchValue)
		{
		}

		// Token: 0x17000A7A RID: 2682
		// (get) Token: 0x06002B58 RID: 11096 RVA: 0x000C4DE0 File Offset: 0x000C2FE0
		// (set) Token: 0x06002B59 RID: 11097 RVA: 0x000C4DED File Offset: 0x000C2FED
		public bool Enabled
		{
			get
			{
				return base.SwitchSetting != 0;
			}
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			set
			{
				base.SwitchSetting = (value ? 1 : 0);
			}
		}

		// Token: 0x06002B5A RID: 11098 RVA: 0x000C4DFC File Offset: 0x000C2FFC
		protected override void OnValueChanged()
		{
			bool flag;
			if (bool.TryParse(base.Value, out flag))
			{
				base.SwitchSetting = (flag ? 1 : 0);
				return;
			}
			base.OnValueChanged();
		}
	}
}
