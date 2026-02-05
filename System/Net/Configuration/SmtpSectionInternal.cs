using System;
using System.Configuration;
using System.Net.Mail;
using System.Threading;

namespace System.Net.Configuration
{
	// Token: 0x02000343 RID: 835
	internal sealed class SmtpSectionInternal
	{
		// Token: 0x06001DF7 RID: 7671 RVA: 0x0008D078 File Offset: 0x0008B278
		internal SmtpSectionInternal(SmtpSection section)
		{
			this.deliveryMethod = section.DeliveryMethod;
			this.deliveryFormat = section.DeliveryFormat;
			this.from = section.From;
			this.network = new SmtpNetworkElementInternal(section.Network);
			this.specifiedPickupDirectory = new SmtpSpecifiedPickupDirectoryElementInternal(section.SpecifiedPickupDirectory);
		}

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x06001DF8 RID: 7672 RVA: 0x0008D0D1 File Offset: 0x0008B2D1
		internal SmtpDeliveryMethod DeliveryMethod
		{
			get
			{
				return this.deliveryMethod;
			}
		}

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x06001DF9 RID: 7673 RVA: 0x0008D0D9 File Offset: 0x0008B2D9
		internal SmtpDeliveryFormat DeliveryFormat
		{
			get
			{
				return this.deliveryFormat;
			}
		}

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x06001DFA RID: 7674 RVA: 0x0008D0E1 File Offset: 0x0008B2E1
		internal SmtpNetworkElementInternal Network
		{
			get
			{
				return this.network;
			}
		}

		// Token: 0x170007B4 RID: 1972
		// (get) Token: 0x06001DFB RID: 7675 RVA: 0x0008D0E9 File Offset: 0x0008B2E9
		internal string From
		{
			get
			{
				return this.from;
			}
		}

		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x06001DFC RID: 7676 RVA: 0x0008D0F1 File Offset: 0x0008B2F1
		internal SmtpSpecifiedPickupDirectoryElementInternal SpecifiedPickupDirectory
		{
			get
			{
				return this.specifiedPickupDirectory;
			}
		}

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x06001DFD RID: 7677 RVA: 0x0008D0F9 File Offset: 0x0008B2F9
		internal static object ClassSyncObject
		{
			get
			{
				if (SmtpSectionInternal.classSyncObject == null)
				{
					Interlocked.CompareExchange(ref SmtpSectionInternal.classSyncObject, new object(), null);
				}
				return SmtpSectionInternal.classSyncObject;
			}
		}

		// Token: 0x06001DFE RID: 7678 RVA: 0x0008D118 File Offset: 0x0008B318
		internal static SmtpSectionInternal GetSection()
		{
			object obj = SmtpSectionInternal.ClassSyncObject;
			SmtpSectionInternal smtpSectionInternal;
			lock (obj)
			{
				SmtpSection smtpSection = PrivilegedConfigurationManager.GetSection(ConfigurationStrings.SmtpSectionPath) as SmtpSection;
				if (smtpSection == null)
				{
					smtpSectionInternal = null;
				}
				else
				{
					smtpSectionInternal = new SmtpSectionInternal(smtpSection);
				}
			}
			return smtpSectionInternal;
		}

		// Token: 0x04001C90 RID: 7312
		private SmtpDeliveryMethod deliveryMethod;

		// Token: 0x04001C91 RID: 7313
		private SmtpDeliveryFormat deliveryFormat;

		// Token: 0x04001C92 RID: 7314
		private string from;

		// Token: 0x04001C93 RID: 7315
		private SmtpNetworkElementInternal network;

		// Token: 0x04001C94 RID: 7316
		private SmtpSpecifiedPickupDirectoryElementInternal specifiedPickupDirectory;

		// Token: 0x04001C95 RID: 7317
		private static object classSyncObject;
	}
}
