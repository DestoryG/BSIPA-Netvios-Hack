using System;

namespace Mono.Cecil
{
	// Token: 0x02000012 RID: 18
	public sealed class AssemblyLinkedResource : Resource
	{
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00005096 File Offset: 0x00003296
		// (set) Token: 0x060000DB RID: 219 RVA: 0x0000509E File Offset: 0x0000329E
		public AssemblyNameReference Assembly
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

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000DC RID: 220 RVA: 0x000050A7 File Offset: 0x000032A7
		public override ResourceType ResourceType
		{
			get
			{
				return ResourceType.AssemblyLinked;
			}
		}

		// Token: 0x060000DD RID: 221 RVA: 0x000050AA File Offset: 0x000032AA
		public AssemblyLinkedResource(string name, ManifestResourceAttributes flags)
			: base(name, flags)
		{
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000050B4 File Offset: 0x000032B4
		public AssemblyLinkedResource(string name, ManifestResourceAttributes flags, AssemblyNameReference reference)
			: base(name, flags)
		{
			this.reference = reference;
		}

		// Token: 0x0400002B RID: 43
		private AssemblyNameReference reference;
	}
}
