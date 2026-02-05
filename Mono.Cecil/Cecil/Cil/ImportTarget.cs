using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x0200011F RID: 287
	public sealed class ImportTarget
	{
		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000B01 RID: 2817 RVA: 0x00023EF4 File Offset: 0x000220F4
		// (set) Token: 0x06000B02 RID: 2818 RVA: 0x00023EFC File Offset: 0x000220FC
		public string Namespace
		{
			get
			{
				return this.@namespace;
			}
			set
			{
				this.@namespace = value;
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000B03 RID: 2819 RVA: 0x00023F05 File Offset: 0x00022105
		// (set) Token: 0x06000B04 RID: 2820 RVA: 0x00023F0D File Offset: 0x0002210D
		public TypeReference Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000B05 RID: 2821 RVA: 0x00023F16 File Offset: 0x00022116
		// (set) Token: 0x06000B06 RID: 2822 RVA: 0x00023F1E File Offset: 0x0002211E
		public AssemblyNameReference AssemblyReference
		{
			get
			{
				return this.reference;
			}
			set
			{
				this.reference = value;
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000B07 RID: 2823 RVA: 0x00023F27 File Offset: 0x00022127
		// (set) Token: 0x06000B08 RID: 2824 RVA: 0x00023F2F File Offset: 0x0002212F
		public string Alias
		{
			get
			{
				return this.alias;
			}
			set
			{
				this.alias = value;
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000B09 RID: 2825 RVA: 0x00023F38 File Offset: 0x00022138
		// (set) Token: 0x06000B0A RID: 2826 RVA: 0x00023F40 File Offset: 0x00022140
		public ImportTargetKind Kind
		{
			get
			{
				return this.kind;
			}
			set
			{
				this.kind = value;
			}
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x00023F49 File Offset: 0x00022149
		public ImportTarget(ImportTargetKind kind)
		{
			this.kind = kind;
		}

		// Token: 0x040006BD RID: 1725
		internal ImportTargetKind kind;

		// Token: 0x040006BE RID: 1726
		internal string @namespace;

		// Token: 0x040006BF RID: 1727
		internal TypeReference type;

		// Token: 0x040006C0 RID: 1728
		internal AssemblyNameReference reference;

		// Token: 0x040006C1 RID: 1729
		internal string alias;
	}
}
