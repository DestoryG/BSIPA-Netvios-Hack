using System;

namespace System.Net.Sockets
{
	// Token: 0x02000388 RID: 904
	[Flags]
	public enum TransmitFileOptions
	{
		// Token: 0x04001F40 RID: 8000
		UseDefaultWorkerThread = 0,
		// Token: 0x04001F41 RID: 8001
		Disconnect = 1,
		// Token: 0x04001F42 RID: 8002
		ReuseSocket = 2,
		// Token: 0x04001F43 RID: 8003
		WriteBehind = 4,
		// Token: 0x04001F44 RID: 8004
		UseSystemThread = 16,
		// Token: 0x04001F45 RID: 8005
		UseKernelApc = 32
	}
}
