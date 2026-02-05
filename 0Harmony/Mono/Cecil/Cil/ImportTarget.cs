using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001E3 RID: 483
	internal sealed class ImportTarget
	{
		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000EE8 RID: 3816 RVA: 0x000330B4 File Offset: 0x000312B4
		// (set) Token: 0x06000EE9 RID: 3817 RVA: 0x000330BC File Offset: 0x000312BC
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

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000EEA RID: 3818 RVA: 0x000330C5 File Offset: 0x000312C5
		// (set) Token: 0x06000EEB RID: 3819 RVA: 0x000330CD File Offset: 0x000312CD
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

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000EEC RID: 3820 RVA: 0x000330D6 File Offset: 0x000312D6
		// (set) Token: 0x06000EED RID: 3821 RVA: 0x000330DE File Offset: 0x000312DE
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

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000EEE RID: 3822 RVA: 0x000330E7 File Offset: 0x000312E7
		// (set) Token: 0x06000EEF RID: 3823 RVA: 0x000330EF File Offset: 0x000312EF
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

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000EF0 RID: 3824 RVA: 0x000330F8 File Offset: 0x000312F8
		// (set) Token: 0x06000EF1 RID: 3825 RVA: 0x00033100 File Offset: 0x00031300
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

		// Token: 0x06000EF2 RID: 3826 RVA: 0x00033109 File Offset: 0x00031309
		public ImportTarget(ImportTargetKind kind)
		{
			this.kind = kind;
		}

		// Token: 0x0400091C RID: 2332
		internal ImportTargetKind kind;

		// Token: 0x0400091D RID: 2333
		internal string @namespace;

		// Token: 0x0400091E RID: 2334
		internal TypeReference type;

		// Token: 0x0400091F RID: 2335
		internal AssemblyNameReference reference;

		// Token: 0x04000920 RID: 2336
		internal string alias;
	}
}
