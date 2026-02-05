using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x02000129 RID: 297
	public sealed class SourceLinkDebugInformation : CustomDebugInformation
	{
		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000B35 RID: 2869 RVA: 0x000241F8 File Offset: 0x000223F8
		// (set) Token: 0x06000B36 RID: 2870 RVA: 0x00024200 File Offset: 0x00022400
		public string Content
		{
			get
			{
				return this.content;
			}
			set
			{
				this.content = value;
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000B37 RID: 2871 RVA: 0x00024209 File Offset: 0x00022409
		public override CustomDebugInformationKind Kind
		{
			get
			{
				return CustomDebugInformationKind.SourceLink;
			}
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x0002420C File Offset: 0x0002240C
		public SourceLinkDebugInformation(string content)
			: base(SourceLinkDebugInformation.KindIdentifier)
		{
			this.content = content;
		}

		// Token: 0x040006DA RID: 1754
		internal string content;

		// Token: 0x040006DB RID: 1755
		public static Guid KindIdentifier = new Guid("{CC110556-A091-4D38-9FEC-25AB9A351A6A}");
	}
}
