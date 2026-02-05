using System;
using System.Runtime.InteropServices;

namespace System.Net.Mail
{
	// Token: 0x02000261 RID: 609
	[StructLayout(LayoutKind.Sequential)]
	internal class _METADATA_HANDLE_INFO
	{
		// Token: 0x060016F9 RID: 5881 RVA: 0x00076262 File Offset: 0x00074462
		private _METADATA_HANDLE_INFO()
		{
			this.dwMDPermissions = 0;
			this.dwMDSystemChangeNumber = 0;
		}

		// Token: 0x0400178D RID: 6029
		internal int dwMDPermissions;

		// Token: 0x0400178E RID: 6030
		internal int dwMDSystemChangeNumber;
	}
}
