using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x02000128 RID: 296
	public sealed class EmbeddedSourceDebugInformation : CustomDebugInformation
	{
		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000B2E RID: 2862 RVA: 0x000241A7 File Offset: 0x000223A7
		// (set) Token: 0x06000B2F RID: 2863 RVA: 0x000241AF File Offset: 0x000223AF
		public byte[] Content
		{
			get
			{
				return this.content;
			}
			set
			{
				this.content = value;
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000B30 RID: 2864 RVA: 0x000241B8 File Offset: 0x000223B8
		// (set) Token: 0x06000B31 RID: 2865 RVA: 0x000241C0 File Offset: 0x000223C0
		public bool Compress
		{
			get
			{
				return this.compress;
			}
			set
			{
				this.compress = value;
			}
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000B32 RID: 2866 RVA: 0x000241C9 File Offset: 0x000223C9
		public override CustomDebugInformationKind Kind
		{
			get
			{
				return CustomDebugInformationKind.EmbeddedSource;
			}
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x000241CC File Offset: 0x000223CC
		public EmbeddedSourceDebugInformation(byte[] content, bool compress)
			: base(EmbeddedSourceDebugInformation.KindIdentifier)
		{
			this.content = content;
			this.compress = compress;
		}

		// Token: 0x040006D7 RID: 1751
		internal byte[] content;

		// Token: 0x040006D8 RID: 1752
		internal bool compress;

		// Token: 0x040006D9 RID: 1753
		public static Guid KindIdentifier = new Guid("{0E8A571B-6926-466E-B4AD-8AB04611F5FE}");
	}
}
