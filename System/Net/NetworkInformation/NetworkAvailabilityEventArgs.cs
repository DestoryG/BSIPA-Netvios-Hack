using System;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002DB RID: 731
	public class NetworkAvailabilityEventArgs : EventArgs
	{
		// Token: 0x060019D5 RID: 6613 RVA: 0x0007E183 File Offset: 0x0007C383
		internal NetworkAvailabilityEventArgs(bool isAvailable)
		{
			this.isAvailable = isAvailable;
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x060019D6 RID: 6614 RVA: 0x0007E192 File Offset: 0x0007C392
		public bool IsAvailable
		{
			get
			{
				return this.isAvailable;
			}
		}

		// Token: 0x04001A44 RID: 6724
		private bool isAvailable;
	}
}
