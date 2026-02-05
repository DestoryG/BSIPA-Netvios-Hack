using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001D9 RID: 473
	internal sealed class ImageDebugHeader
	{
		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000EB4 RID: 3764 RVA: 0x00032C3F File Offset: 0x00030E3F
		public bool HasEntries
		{
			get
			{
				return !this.entries.IsNullOrEmpty<ImageDebugHeaderEntry>();
			}
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000EB5 RID: 3765 RVA: 0x00032C4F File Offset: 0x00030E4F
		public ImageDebugHeaderEntry[] Entries
		{
			get
			{
				return this.entries;
			}
		}

		// Token: 0x06000EB6 RID: 3766 RVA: 0x00032C57 File Offset: 0x00030E57
		public ImageDebugHeader(ImageDebugHeaderEntry[] entries)
		{
			this.entries = entries ?? Empty<ImageDebugHeaderEntry>.Array;
		}

		// Token: 0x06000EB7 RID: 3767 RVA: 0x00032C6F File Offset: 0x00030E6F
		public ImageDebugHeader()
			: this(Empty<ImageDebugHeaderEntry>.Array)
		{
		}

		// Token: 0x06000EB8 RID: 3768 RVA: 0x00032C7C File Offset: 0x00030E7C
		public ImageDebugHeader(ImageDebugHeaderEntry entry)
			: this(new ImageDebugHeaderEntry[] { entry })
		{
		}

		// Token: 0x040008FA RID: 2298
		private readonly ImageDebugHeaderEntry[] entries;
	}
}
