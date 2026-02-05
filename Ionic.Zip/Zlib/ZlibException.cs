using System;
using System.Runtime.InteropServices;

namespace Ionic.Zlib
{
	// Token: 0x02000065 RID: 101
	[Guid("ebc25cf6-9120-4283-b972-0e5520d0000E")]
	public class ZlibException : Exception
	{
		// Token: 0x0600047E RID: 1150 RVA: 0x00020179 File Offset: 0x0001E379
		public ZlibException()
		{
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x00020181 File Offset: 0x0001E381
		public ZlibException(string s)
			: base(s)
		{
		}
	}
}
