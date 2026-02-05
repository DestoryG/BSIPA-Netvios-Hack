using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x020001F9 RID: 505
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct SSPIHandle
	{
		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06001322 RID: 4898 RVA: 0x0006479D File Offset: 0x0006299D
		public bool IsZero
		{
			get
			{
				return this.HandleHi == IntPtr.Zero && this.HandleLo == IntPtr.Zero;
			}
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x000647C3 File Offset: 0x000629C3
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		internal void SetToInvalid()
		{
			this.HandleHi = IntPtr.Zero;
			this.HandleLo = IntPtr.Zero;
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x000647DB File Offset: 0x000629DB
		public override string ToString()
		{
			return this.HandleHi.ToString("x") + ":" + this.HandleLo.ToString("x");
		}

		// Token: 0x04001546 RID: 5446
		private IntPtr HandleHi;

		// Token: 0x04001547 RID: 5447
		private IntPtr HandleLo;
	}
}
