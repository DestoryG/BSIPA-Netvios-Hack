using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001E8 RID: 488
	internal sealed class BinaryCustomDebugInformation : CustomDebugInformation
	{
		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000EFD RID: 3837 RVA: 0x0003319A File Offset: 0x0003139A
		// (set) Token: 0x06000EFE RID: 3838 RVA: 0x000331A2 File Offset: 0x000313A2
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

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000EFF RID: 3839 RVA: 0x00010910 File Offset: 0x0000EB10
		public override CustomDebugInformationKind Kind
		{
			get
			{
				return CustomDebugInformationKind.Binary;
			}
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x000331AB File Offset: 0x000313AB
		public BinaryCustomDebugInformation(Guid identifier, byte[] data)
			: base(identifier)
		{
			this.data = data;
		}

		// Token: 0x0400092C RID: 2348
		private byte[] data;
	}
}
