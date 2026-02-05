using System;
using Mono.Cecil.Metadata;

namespace Mono.Cecil
{
	// Token: 0x020000A9 RID: 169
	public sealed class ByReferenceType : TypeSpecification
	{
		// Token: 0x1700021A RID: 538
		// (get) Token: 0x0600075B RID: 1883 RVA: 0x000172A1 File Offset: 0x000154A1
		public override string Name
		{
			get
			{
				return base.Name + "&";
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x0600075C RID: 1884 RVA: 0x000172B3 File Offset: 0x000154B3
		public override string FullName
		{
			get
			{
				return base.FullName + "&";
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x0600075D RID: 1885 RVA: 0x000026DB File Offset: 0x000008DB
		// (set) Token: 0x0600075E RID: 1886 RVA: 0x00002C55 File Offset: 0x00000E55
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

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x0600075F RID: 1887 RVA: 0x00002BE8 File Offset: 0x00000DE8
		public override bool IsByReference
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x000172C5 File Offset: 0x000154C5
		public ByReferenceType(TypeReference type)
			: base(type)
		{
			Mixin.CheckType(type);
			this.etype = Mono.Cecil.Metadata.ElementType.ByRef;
		}
	}
}
