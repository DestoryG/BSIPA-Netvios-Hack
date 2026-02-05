using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x02000124 RID: 292
	public sealed class BinaryCustomDebugInformation : CustomDebugInformation
	{
		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000B16 RID: 2838 RVA: 0x00023FDD File Offset: 0x000221DD
		// (set) Token: 0x06000B17 RID: 2839 RVA: 0x00023FE5 File Offset: 0x000221E5
		public byte[] Data
		{
			get
			{
				return this.data;
			}
			set
			{
				this.data = value;
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000B18 RID: 2840 RVA: 0x000026DB File Offset: 0x000008DB
		public override CustomDebugInformationKind Kind
		{
			get
			{
				return CustomDebugInformationKind.Binary;
			}
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x00023FEE File Offset: 0x000221EE
		public BinaryCustomDebugInformation(Guid identifier, byte[] data)
			: base(identifier)
		{
			this.data = data;
		}

		// Token: 0x040006CD RID: 1741
		private byte[] data;
	}
}
