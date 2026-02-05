using System;
using System.Configuration;

namespace System.Net.Configuration
{
	// Token: 0x02000337 RID: 823
	public sealed class MailSettingsSectionGroup : ConfigurationSectionGroup
	{
		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x06001D6F RID: 7535 RVA: 0x0008BC6F File Offset: 0x00089E6F
		public SmtpSection Smtp
		{
			get
			{
				return (SmtpSection)base.Sections["smtp"];
			}
		}
	}
}
