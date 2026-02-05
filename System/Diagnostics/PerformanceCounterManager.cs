using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x020004E8 RID: 1256
	[ComVisible(true)]
	[Guid("82840BE1-D273-11D2-B94A-00600893B17A")]
	[Obsolete("This class has been deprecated.  Use the PerformanceCounters through the System.Diagnostics.PerformanceCounter class instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class PerformanceCounterManager : ICollectData
	{
		// Token: 0x06002F74 RID: 12148 RVA: 0x000D6B52 File Offset: 0x000D4D52
		[Obsolete("This class has been deprecated.  Use the PerformanceCounters through the System.Diagnostics.PerformanceCounter class instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public PerformanceCounterManager()
		{
		}

		// Token: 0x06002F75 RID: 12149 RVA: 0x000D6B5A File Offset: 0x000D4D5A
		[Obsolete("This class has been deprecated.  Use the PerformanceCounters through the System.Diagnostics.PerformanceCounter class instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		void ICollectData.CollectData(int callIdx, IntPtr valueNamePtr, IntPtr dataPtr, int totalBytes, out IntPtr res)
		{
			res = (IntPtr)(-1);
		}

		// Token: 0x06002F76 RID: 12150 RVA: 0x000D6B65 File Offset: 0x000D4D65
		[Obsolete("This class has been deprecated.  Use the PerformanceCounters through the System.Diagnostics.PerformanceCounter class instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		void ICollectData.CloseData()
		{
		}
	}
}
