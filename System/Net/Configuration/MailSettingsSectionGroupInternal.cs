using System;

namespace System.Net.Configuration
{
	// Token: 0x02000338 RID: 824
	internal sealed class MailSettingsSectionGroupInternal
	{
		// Token: 0x06001D70 RID: 7536 RVA: 0x0008BC86 File Offset: 0x00089E86
		internal MailSettingsSectionGroupInternal()
		{
			this.smtp = SmtpSectionInternal.GetSection();
		}

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x06001D71 RID: 7537 RVA: 0x0008BC99 File Offset: 0x00089E99
		internal SmtpSectionInternal Smtp
		{
			get
			{
				return this.smtp;
			}
		}

		// Token: 0x06001D72 RID: 7538 RVA: 0x0008BCA1 File Offset: 0x00089EA1
		internal static MailSettingsSectionGroupInternal GetSection()
		{
			return new MailSettingsSectionGroupInternal();
		}

		// Token: 0x04001C43 RID: 7235
		private SmtpSectionInternal smtp;
	}
}
