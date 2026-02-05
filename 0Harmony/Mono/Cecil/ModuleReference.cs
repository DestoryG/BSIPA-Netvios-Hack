using System;

namespace Mono.Cecil
{
	// Token: 0x02000156 RID: 342
	internal class ModuleReference : IMetadataScope, IMetadataTokenProvider
	{
		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000A86 RID: 2694 RVA: 0x000252AC File Offset: 0x000234AC
		// (set) Token: 0x06000A87 RID: 2695 RVA: 0x000252B4 File Offset: 0x000234B4
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

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000A88 RID: 2696 RVA: 0x00010F39 File Offset: 0x0000F139
		public virtual MetadataScopeType MetadataScopeType
		{
			get
			{
				return MetadataScopeType.ModuleReference;
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000A89 RID: 2697 RVA: 0x000252BD File Offset: 0x000234BD
		// (set) Token: 0x06000A8A RID: 2698 RVA: 0x000252C5 File Offset: 0x000234C5
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

		// Token: 0x06000A8B RID: 2699 RVA: 0x000252CE File Offset: 0x000234CE
		internal ModuleReference()
		{
			this.token = new MetadataToken(TokenType.ModuleRef);
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x000252E6 File Offset: 0x000234E6
		public ModuleReference(string name)
			: this()
		{
			this.name = name;
		}

		// Token: 0x06000A8D RID: 2701 RVA: 0x000252AC File Offset: 0x000234AC
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x04000405 RID: 1029
		private string name;

		// Token: 0x04000406 RID: 1030
		internal MetadataToken token;
	}
}
