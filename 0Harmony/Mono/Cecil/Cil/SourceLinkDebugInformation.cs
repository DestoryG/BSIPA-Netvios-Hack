using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001ED RID: 493
	internal sealed class SourceLinkDebugInformation : CustomDebugInformation
	{
		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000F1C RID: 3868 RVA: 0x000333A8 File Offset: 0x000315A8
		// (set) Token: 0x06000F1D RID: 3869 RVA: 0x000333B0 File Offset: 0x000315B0
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

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000F1E RID: 3870 RVA: 0x000333B9 File Offset: 0x000315B9
		public override CustomDebugInformationKind Kind
		{
			get
			{
				return CustomDebugInformationKind.SourceLink;
			}
		}

		// Token: 0x06000F1F RID: 3871 RVA: 0x000333BC File Offset: 0x000315BC
		public SourceLinkDebugInformation(string content)
			: base(SourceLinkDebugInformation.KindIdentifier)
		{
			this.content = content;
		}

		// Token: 0x04000939 RID: 2361
		internal string content;

		// Token: 0x0400093A RID: 2362
		public static Guid KindIdentifier = new Guid("{CC110556-A091-4D38-9FEC-25AB9A351A6A}");
	}
}
