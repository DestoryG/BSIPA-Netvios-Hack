using System;

namespace System.IO.Ports
{
	// Token: 0x0200040F RID: 1039
	public class SerialPinChangedEventArgs : EventArgs
	{
		// Token: 0x060026BD RID: 9917 RVA: 0x000B2024 File Offset: 0x000B0224
		internal SerialPinChangedEventArgs(SerialPinChange eventCode)
		{
			this.pinChanged = eventCode;
		}

		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x060026BE RID: 9918 RVA: 0x000B2033 File Offset: 0x000B0233
		public SerialPinChange EventType
		{
			get
			{
				return this.pinChanged;
			}
		}

		// Token: 0x04002100 RID: 8448
		private SerialPinChange pinChanged;
	}
}
