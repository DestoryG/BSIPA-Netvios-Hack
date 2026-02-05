using System;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x02000494 RID: 1172
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
	public class ConsoleTraceListener : TextWriterTraceListener
	{
		// Token: 0x06002B5B RID: 11099 RVA: 0x000C4E2C File Offset: 0x000C302C
		public ConsoleTraceListener()
			: base(Console.Out)
		{
		}

		// Token: 0x06002B5C RID: 11100 RVA: 0x000C4E39 File Offset: 0x000C3039
		public ConsoleTraceListener(bool useErrorStream)
			: base(useErrorStream ? Console.Error : Console.Out)
		{
		}

		// Token: 0x06002B5D RID: 11101 RVA: 0x000C4E50 File Offset: 0x000C3050
		public override void Close()
		{
		}
	}
}
