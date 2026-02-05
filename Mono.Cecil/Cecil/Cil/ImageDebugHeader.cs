using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x02000115 RID: 277
	public sealed class ImageDebugHeader
	{
		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000ACD RID: 2765 RVA: 0x00023A33 File Offset: 0x00021C33
		public bool HasEntries
		{
			get
			{
				return !this.entries.IsNullOrEmpty<ImageDebugHeaderEntry>();
			}
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000ACE RID: 2766 RVA: 0x00023A43 File Offset: 0x00021C43
		public ImageDebugHeaderEntry[] Entries
		{
			get
			{
				return this.entries;
			}
		}

		// Token: 0x06000ACF RID: 2767 RVA: 0x00023A4B File Offset: 0x00021C4B
		public ImageDebugHeader(ImageDebugHeaderEntry[] entries)
		{
			this.entries = entries ?? Empty<ImageDebugHeaderEntry>.Array;
		}

		// Token: 0x06000AD0 RID: 2768 RVA: 0x00023A63 File Offset: 0x00021C63
		public ImageDebugHeader()
			: this(Empty<ImageDebugHeaderEntry>.Array)
		{
		}

		// Token: 0x06000AD1 RID: 2769 RVA: 0x00023A70 File Offset: 0x00021C70
		public ImageDebugHeader(ImageDebugHeaderEntry entry)
			: this(new ImageDebugHeaderEntry[] { entry })
		{
		}

		// Token: 0x0400069B RID: 1691
		private readonly ImageDebugHeaderEntry[] entries;
	}
}
