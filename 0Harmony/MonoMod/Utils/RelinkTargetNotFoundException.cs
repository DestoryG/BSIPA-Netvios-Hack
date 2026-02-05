using System;
using Mono.Cecil;

namespace MonoMod.Utils
{
	// Token: 0x0200033E RID: 830
	internal class RelinkTargetNotFoundException : RelinkFailedException
	{
		// Token: 0x0600131A RID: 4890 RVA: 0x00045C69 File Offset: 0x00043E69
		public RelinkTargetNotFoundException(IMetadataTokenProvider mtp, IMetadataTokenProvider context = null)
			: base(RelinkFailedException._Format("MonoMod relinker failed finding", mtp, context), mtp, context)
		{
		}

		// Token: 0x0600131B RID: 4891 RVA: 0x00045C7F File Offset: 0x00043E7F
		public RelinkTargetNotFoundException(string message, IMetadataTokenProvider mtp, IMetadataTokenProvider context = null)
			: base(message ?? "MonoMod relinker failed finding", mtp, context)
		{
		}

		// Token: 0x0600131C RID: 4892 RVA: 0x00045C93 File Offset: 0x00043E93
		public RelinkTargetNotFoundException(string message, Exception innerException, IMetadataTokenProvider mtp, IMetadataTokenProvider context = null)
			: base(message ?? "MonoMod relinker failed finding", innerException, mtp, context)
		{
		}

		// Token: 0x04000FB5 RID: 4021
		public new const string DefaultMessage = "MonoMod relinker failed finding";
	}
}
