using System;

namespace System.IO.Ports
{
	// Token: 0x0200040C RID: 1036
	public class SerialErrorReceivedEventArgs : EventArgs
	{
		// Token: 0x060026B7 RID: 9911 RVA: 0x000B200D File Offset: 0x000B020D
		internal SerialErrorReceivedEventArgs(SerialError eventCode)
		{
			this.errorType = eventCode;
		}

		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x060026B8 RID: 9912 RVA: 0x000B201C File Offset: 0x000B021C
		public SerialError EventType
		{
			get
			{
				return this.errorType;
			}
		}

		// Token: 0x040020F9 RID: 8441
		private SerialError errorType;
	}
}
