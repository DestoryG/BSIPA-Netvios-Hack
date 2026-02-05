using System;

namespace Mono.Cecil
{
	// Token: 0x020000BF RID: 191
	internal sealed class AssemblyLinkedResource : Resource
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000443 RID: 1091 RVA: 0x0001346A File Offset: 0x0001166A
		// (set) Token: 0x06000444 RID: 1092 RVA: 0x00013472 File Offset: 0x00011672
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

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000445 RID: 1093 RVA: 0x0001347B File Offset: 0x0001167B
		public override ResourceType ResourceType
		{
			get
			{
				return ResourceType.AssemblyLinked;
			}
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0001347E File Offset: 0x0001167E
		public AssemblyLinkedResource(string name, ManifestResourceAttributes flags)
			: base(name, flags)
		{
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00013488 File Offset: 0x00011688
		public AssemblyLinkedResource(string name, ManifestResourceAttributes flags, AssemblyNameReference reference)
			: base(name, flags)
		{
			this.reference = reference;
		}

		// Token: 0x0400022E RID: 558
		private AssemblyNameReference reference;
	}
}
