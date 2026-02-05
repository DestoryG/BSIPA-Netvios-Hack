using System;

namespace Mono.Cecil
{
	// Token: 0x020000C3 RID: 195
	internal sealed class MemberReferenceProjection
	{
		// Token: 0x06000867 RID: 2151 RVA: 0x000194CE File Offset: 0x000176CE
		public MemberReferenceProjection(MemberReference member, MemberReferenceTreatment treatment)
		{
			this.Name = member.Name;
			this.Treatment = treatment;
		}

		// Token: 0x040002FD RID: 765
		public readonly string Name;

		// Token: 0x040002FE RID: 766
		public readonly MemberReferenceTreatment Treatment;
	}
}
