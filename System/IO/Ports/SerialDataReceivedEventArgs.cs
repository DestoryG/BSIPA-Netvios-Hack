using System;

namespace System.IO.Ports
{
	// Token: 0x02000413 RID: 1043
	public class SerialDataReceivedEventArgs : EventArgs
	{
		// Token: 0x06002716 RID: 10006 RVA: 0x000B3B44 File Offset: 0x000B1D44
		internal SerialDataReceivedEventArgs(SerialData eventCode)
		{
			this.receiveType = eventCode;
		}

		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x06002717 RID: 10007 RVA: 0x000B3B53 File Offset: 0x000B1D53
		public SerialData EventType
		{
			get
			{
				return this.receiveType;
			}
		}

		// Token: 0x04002135 RID: 8501
		internal SerialData receiveType;
	}
}
