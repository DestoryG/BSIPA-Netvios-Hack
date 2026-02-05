using System;

namespace Mono.Cecil
{
	// Token: 0x02000184 RID: 388
	internal sealed class MemberReferenceProjection
	{
		// Token: 0x06000C40 RID: 3136 RVA: 0x00028445 File Offset: 0x00026645
		public MemberReferenceProjection(MemberReference member, MemberReferenceTreatment treatment)
		{
			this.Name = member.Name;
			this.Treatment = treatment;
		}

		// Token: 0x04000554 RID: 1364
		public readonly string Name;

		// Token: 0x04000555 RID: 1365
		public readonly MemberReferenceTreatment Treatment;
	}
}
