using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000A5 RID: 165
	public sealed class PointerType : TypeSpecification
	{
		// Token: 0x17000202 RID: 514
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x00016DD5 File Offset: 0x00014FD5
		public override string Name
		{
			get
			{
				return base.Name + "*";
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000730 RID: 1840 RVA: 0x00016DE7 File Offset: 0x00014FE7
		public override string FullName
		{
			get
			{
				return base.FullName + "*";
			}
		}

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000731 RID: 1841 RVA: 0x000026DB File Offset: 0x000008DB
		// (set) Token: 0x06000732 RID: 1842 RVA: 0x00002C55 File Offset: 0x00000E55
		public override bool IsValueType
		{
			get
			{
				return false;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000733 RID: 1843 RVA: 0x00002BE8 File Offset: 0x00000DE8
		public override bool IsPointer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x00016DF9 File Offset: 0x00014FF9
		public PointerType(TypeReference type)
			: base(type)
		{
			Mixin.CheckType(type);
			this.etype = Mono.Cecil.Metadata.ElementType.Ptr;
		}
	}
}
