using System;

namespace Mono.Cecil
{
	// Token: 0x02000077 RID: 119
	public sealed class LinkedResource : Resource
	{
		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060004C2 RID: 1218 RVA: 0x000130E5 File Offset: 0x000112E5
		public byte[] Hash
		{
			get
			{
				return this.hash;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x000130ED File Offset: 0x000112ED
		// (set) Token: 0x060004C4 RID: 1220 RVA: 0x000130F5 File Offset: 0x000112F5
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

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060004C5 RID: 1221 RVA: 0x000026DB File Offset: 0x000008DB
		public override ResourceType ResourceType
		{
			get
			{
				return ResourceType.Linked;
			}
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x000050AA File Offset: 0x000032AA
		public LinkedResource(string name, ManifestResourceAttributes flags)
			: base(name, flags)
		{
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x000130FE File Offset: 0x000112FE
		public LinkedResource(string name, ManifestResourceAttributes flags, string file)
			: base(name, flags)
		{
			this.file = file;
		}

		// Token: 0x040000E6 RID: 230
		internal byte[] hash;

		// Token: 0x040000E7 RID: 231
		private string file;
	}
}
