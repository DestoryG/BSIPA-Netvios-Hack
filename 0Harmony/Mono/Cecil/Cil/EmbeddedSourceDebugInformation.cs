using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001EC RID: 492
	internal sealed class EmbeddedSourceDebugInformation : CustomDebugInformation
	{
		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000F15 RID: 3861 RVA: 0x00033357 File Offset: 0x00031557
		// (set) Token: 0x06000F16 RID: 3862 RVA: 0x0003335F File Offset: 0x0003155F
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

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000F17 RID: 3863 RVA: 0x00033368 File Offset: 0x00031568
		// (set) Token: 0x06000F18 RID: 3864 RVA: 0x00033370 File Offset: 0x00031570
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

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000F19 RID: 3865 RVA: 0x00033379 File Offset: 0x00031579
		public override CustomDebugInformationKind Kind
		{
			get
			{
				return CustomDebugInformationKind.EmbeddedSource;
			}
		}

		// Token: 0x06000F1A RID: 3866 RVA: 0x0003337C File Offset: 0x0003157C
		public EmbeddedSourceDebugInformation(byte[] content, bool compress)
			: base(EmbeddedSourceDebugInformation.KindIdentifier)
		{
			this.content = content;
			this.compress = compress;
		}

		// Token: 0x04000936 RID: 2358
		internal byte[] content;

		// Token: 0x04000937 RID: 2359
		internal bool compress;

		// Token: 0x04000938 RID: 2360
		public static Guid KindIdentifier = new Guid("{0E8A571B-6926-466E-B4AD-8AB04611F5FE}");
	}
}
