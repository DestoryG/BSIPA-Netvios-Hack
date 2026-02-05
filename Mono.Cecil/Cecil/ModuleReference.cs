using System;

namespace Mono.Cecil
{
	// Token: 0x0200009C RID: 156
	public class ModuleReference : IMetadataScope, IMetadataTokenProvider
	{
		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x060006CF RID: 1743 RVA: 0x000166D9 File Offset: 0x000148D9
		// (set) Token: 0x060006D0 RID: 1744 RVA: 0x000166E1 File Offset: 0x000148E1
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x060006D1 RID: 1745 RVA: 0x00002BE8 File Offset: 0x00000DE8
		public virtual MetadataScopeType MetadataScopeType
		{
			get
			{
				return MetadataScopeType.ModuleReference;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x060006D2 RID: 1746 RVA: 0x000166EA File Offset: 0x000148EA
		// (set) Token: 0x060006D3 RID: 1747 RVA: 0x000166F2 File Offset: 0x000148F2
		public MetadataToken MetadataToken
		{
			get
			{
				return this.token;
			}
			set
			{
				this.token = value;
			}
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x000166FB File Offset: 0x000148FB
		internal ModuleReference()
		{
			this.token = new MetadataToken(TokenType.ModuleRef);
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x00016713 File Offset: 0x00014913
		public ModuleReference(string name)
			: this()
		{
			this.name = name;
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x000166D9 File Offset: 0x000148D9
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x040001CD RID: 461
		private string name;

		// Token: 0x040001CE RID: 462
		internal MetadataToken token;
	}
}
