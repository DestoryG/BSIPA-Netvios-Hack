using System;

namespace Mono.Cecil
{
	// Token: 0x0200012E RID: 302
	internal sealed class LinkedResource : Resource
	{
		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000854 RID: 2132 RVA: 0x00021911 File Offset: 0x0001FB11
		public byte[] Hash
		{
			get
			{
				return this.hash;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000855 RID: 2133 RVA: 0x00021919 File Offset: 0x0001FB19
		// (set) Token: 0x06000856 RID: 2134 RVA: 0x00021921 File Offset: 0x0001FB21
		public string File
		{
			get
			{
				return this.file;
			}
			set
			{
				this.file = value;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000857 RID: 2135 RVA: 0x00010910 File Offset: 0x0000EB10
		public override ResourceType ResourceType
		{
			get
			{
				return ResourceType.Linked;
			}
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x0001347E File Offset: 0x0001167E
		public LinkedResource(string name, ManifestResourceAttributes flags)
			: base(name, flags)
		{
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x0002192A File Offset: 0x0001FB2A
		public LinkedResource(string name, ManifestResourceAttributes flags, string file)
			: base(name, flags)
		{
			this.file = file;
		}

		// Token: 0x04000300 RID: 768
		internal byte[] hash;

		// Token: 0x04000301 RID: 769
		private string file;
	}
}
