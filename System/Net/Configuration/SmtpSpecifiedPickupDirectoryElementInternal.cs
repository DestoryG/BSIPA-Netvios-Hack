using System;

namespace System.Net.Configuration
{
	// Token: 0x02000347 RID: 839
	internal sealed class SmtpSpecifiedPickupDirectoryElementInternal
	{
		// Token: 0x06001E1D RID: 7709 RVA: 0x0008D5FB File Offset: 0x0008B7FB
		internal SmtpSpecifiedPickupDirectoryElementInternal(SmtpSpecifiedPickupDirectoryElement element)
		{
			this.pickupDirectoryLocation = element.PickupDirectoryLocation;
		}

		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x06001E1E RID: 7710 RVA: 0x0008D60F File Offset: 0x0008B80F
		internal string PickupDirectoryLocation
		{
			get
			{
				return this.pickupDirectoryLocation;
			}
		}

		// Token: 0x04001CA7 RID: 7335
		private string pickupDirectoryLocation;
	}
}
