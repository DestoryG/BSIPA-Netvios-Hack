using System;
using System.Text;
using Mono.Cecil;

namespace MonoMod.Utils
{
	// Token: 0x0200033D RID: 829
	internal class RelinkFailedException : Exception
	{
		// Token: 0x06001316 RID: 4886 RVA: 0x00045BA0 File Offset: 0x00043DA0
		public RelinkFailedException(IMetadataTokenProvider mtp, IMetadataTokenProvider context = null)
			: this(RelinkFailedException._Format("MonoMod failed relinking", mtp, context), mtp, context)
		{
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x00045BB6 File Offset: 0x00043DB6
		public RelinkFailedException(string message, IMetadataTokenProvider mtp, IMetadataTokenProvider context = null)
			: base(message)
		{
			this.MTP = mtp;
			this.Context = context;
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x00045BCD File Offset: 0x00043DCD
		public RelinkFailedException(string message, Exception innerException, IMetadataTokenProvider mtp, IMetadataTokenProvider context = null)
			: base(message ?? RelinkFailedException._Format("MonoMod failed relinking", mtp, context), innerException)
		{
			this.MTP = mtp;
			this.Context = context;
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x00045BF8 File Offset: 0x00043DF8
		protected static string _Format(string message, IMetadataTokenProvider mtp, IMetadataTokenProvider context)
		{
			if (mtp == null && context == null)
			{
				return message;
			}
			StringBuilder stringBuilder = new StringBuilder(message);
			stringBuilder.Append(" ");
			if (mtp != null)
			{
				stringBuilder.Append(mtp.ToString());
			}
			if (context != null)
			{
				stringBuilder.Append(" ");
			}
			if (context != null)
			{
				stringBuilder.Append("(context: ").Append(context.ToString()).Append(")");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04000FB2 RID: 4018
		public const string DefaultMessage = "MonoMod failed relinking";

		// Token: 0x04000FB3 RID: 4019
		public IMetadataTokenProvider MTP;

		// Token: 0x04000FB4 RID: 4020
		public IMetadataTokenProvider Context;
	}
}
