using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001E7 RID: 487
	internal abstract class CustomDebugInformation : DebugInformation
	{
		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000EFA RID: 3834 RVA: 0x00033173 File Offset: 0x00031373
		public Guid Identifier
		{
			get
			{
				return this.identifier;
			}
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000EFB RID: 3835
		public abstract CustomDebugInformationKind Kind { get; }

		// Token: 0x06000EFC RID: 3836 RVA: 0x0003317B File Offset: 0x0003137B
		internal CustomDebugInformation(Guid identifier)
		{
			this.identifier = identifier;
			this.token = new MetadataToken(TokenType.CustomDebugInformation);
		}

		// Token: 0x0400092B RID: 2347
		private Guid identifier;
	}
}
