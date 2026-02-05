using System;
using System.Diagnostics;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200052A RID: 1322
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	internal static class CompModSwitches
	{
		// Token: 0x17000C44 RID: 3140
		// (get) Token: 0x06003200 RID: 12800 RVA: 0x000E0545 File Offset: 0x000DE745
		public static BooleanSwitch CommonDesignerServices
		{
			get
			{
				if (CompModSwitches.commonDesignerServices == null)
				{
					CompModSwitches.commonDesignerServices = new BooleanSwitch("CommonDesignerServices", "Assert if any common designer service is not found.");
				}
				return CompModSwitches.commonDesignerServices;
			}
		}

		// Token: 0x17000C45 RID: 3141
		// (get) Token: 0x06003201 RID: 12801 RVA: 0x000E056D File Offset: 0x000DE76D
		public static TraceSwitch EventLog
		{
			get
			{
				if (CompModSwitches.eventLog == null)
				{
					CompModSwitches.eventLog = new TraceSwitch("EventLog", "Enable tracing for the EventLog component.");
				}
				return CompModSwitches.eventLog;
			}
		}

		// Token: 0x04002950 RID: 10576
		private static volatile BooleanSwitch commonDesignerServices;

		// Token: 0x04002951 RID: 10577
		private static volatile TraceSwitch eventLog;
	}
}
