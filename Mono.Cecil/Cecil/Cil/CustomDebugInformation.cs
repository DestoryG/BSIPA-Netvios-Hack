using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x02000123 RID: 291
	public abstract class CustomDebugInformation : DebugInformation
	{
		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000B13 RID: 2835 RVA: 0x00023FB6 File Offset: 0x000221B6
		public Guid Identifier
		{
			get
			{
				return this.identifier;
			}
		}

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000B14 RID: 2836
		public abstract CustomDebugInformationKind Kind { get; }

		// Token: 0x06000B15 RID: 2837 RVA: 0x00023FBE File Offset: 0x000221BE
		internal CustomDebugInformation(Guid identifier)
		{
			this.identifier = identifier;
			this.token = new MetadataToken(TokenType.CustomDebugInformation);
		}

		// Token: 0x040006CC RID: 1740
		private Guid identifier;
	}
}
