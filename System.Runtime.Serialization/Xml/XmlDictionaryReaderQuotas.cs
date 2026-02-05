using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace System.Xml
{
	// Token: 0x02000038 RID: 56
	public sealed class XmlDictionaryReaderQuotas
	{
		// Token: 0x0600043B RID: 1083 RVA: 0x00015A51 File Offset: 0x00013C51
		public XmlDictionaryReaderQuotas()
		{
			XmlDictionaryReaderQuotas.defaultQuota.CopyTo(this);
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00015A64 File Offset: 0x00013C64
		private XmlDictionaryReaderQuotas(int maxDepth, int maxStringContentLength, int maxArrayLength, int maxBytesPerRead, int maxNameTableCharCount, XmlDictionaryReaderQuotaTypes modifiedQuotas)
		{
			this.maxDepth = maxDepth;
			this.maxStringContentLength = maxStringContentLength;
			this.maxArrayLength = maxArrayLength;
			this.maxBytesPerRead = maxBytesPerRead;
			this.maxNameTableCharCount = maxNameTableCharCount;
			this.modifiedQuotas = modifiedQuotas;
			this.MakeReadOnly();
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600043D RID: 1085 RVA: 0x00015A9F File Offset: 0x00013C9F
		public static XmlDictionaryReaderQuotas Max
		{
			get
			{
				return XmlDictionaryReaderQuotas.maxQuota;
			}
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x00015AA6 File Offset: 0x00013CA6
		public void CopyTo(XmlDictionaryReaderQuotas quotas)
		{
			if (quotas == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("quotas"));
			}
			if (quotas.readOnly)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("Cannot copy XmlDictionaryReaderQuotas. Target is readonly.")));
			}
			this.InternalCopyTo(quotas);
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00015AE0 File Offset: 0x00013CE0
		internal void InternalCopyTo(XmlDictionaryReaderQuotas quotas)
		{
			quotas.maxStringContentLength = this.maxStringContentLength;
			quotas.maxArrayLength = this.maxArrayLength;
			quotas.maxDepth = this.maxDepth;
			quotas.maxNameTableCharCount = this.maxNameTableCharCount;
			quotas.maxBytesPerRead = this.maxBytesPerRead;
			quotas.modifiedQuotas = this.modifiedQuotas;
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000440 RID: 1088 RVA: 0x00015B35 File Offset: 0x00013D35
		// (set) Token: 0x06000441 RID: 1089 RVA: 0x00015B40 File Offset: 0x00013D40
		[DefaultValue(8192)]
		public int MaxStringContentLength
		{
			get
			{
				return this.maxStringContentLength;
			}
			set
			{
				if (this.readOnly)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("The '{0}' quota is readonly.", new object[] { "MaxStringContentLength" })));
				}
				if (value <= 0)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(global::System.Runtime.Serialization.SR.GetString("Quota must be a positive value."), "value"));
				}
				this.maxStringContentLength = value;
				this.modifiedQuotas |= XmlDictionaryReaderQuotaTypes.MaxStringContentLength;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000442 RID: 1090 RVA: 0x00015BAB File Offset: 0x00013DAB
		// (set) Token: 0x06000443 RID: 1091 RVA: 0x00015BB4 File Offset: 0x00013DB4
		[DefaultValue(16384)]
		public int MaxArrayLength
		{
			get
			{
				return this.maxArrayLength;
			}
			set
			{
				if (this.readOnly)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("The '{0}' quota is readonly.", new object[] { "MaxArrayLength" })));
				}
				if (value <= 0)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(global::System.Runtime.Serialization.SR.GetString("Quota must be a positive value."), "value"));
				}
				this.maxArrayLength = value;
				this.modifiedQuotas |= XmlDictionaryReaderQuotaTypes.MaxArrayLength;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000444 RID: 1092 RVA: 0x00015C1F File Offset: 0x00013E1F
		// (set) Token: 0x06000445 RID: 1093 RVA: 0x00015C28 File Offset: 0x00013E28
		[DefaultValue(4096)]
		public int MaxBytesPerRead
		{
			get
			{
				return this.maxBytesPerRead;
			}
			set
			{
				if (this.readOnly)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("The '{0}' quota is readonly.", new object[] { "MaxBytesPerRead" })));
				}
				if (value <= 0)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(global::System.Runtime.Serialization.SR.GetString("Quota must be a positive value."), "value"));
				}
				this.maxBytesPerRead = value;
				this.modifiedQuotas |= XmlDictionaryReaderQuotaTypes.MaxBytesPerRead;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000446 RID: 1094 RVA: 0x00015C93 File Offset: 0x00013E93
		// (set) Token: 0x06000447 RID: 1095 RVA: 0x00015C9C File Offset: 0x00013E9C
		[DefaultValue(32)]
		public int MaxDepth
		{
			get
			{
				return this.maxDepth;
			}
			set
			{
				if (this.readOnly)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("The '{0}' quota is readonly.", new object[] { "MaxDepth" })));
				}
				if (value <= 0)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(global::System.Runtime.Serialization.SR.GetString("Quota must be a positive value."), "value"));
				}
				this.maxDepth = value;
				this.modifiedQuotas |= XmlDictionaryReaderQuotaTypes.MaxDepth;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x00015D07 File Offset: 0x00013F07
		// (set) Token: 0x06000449 RID: 1097 RVA: 0x00015D10 File Offset: 0x00013F10
		[DefaultValue(16384)]
		public int MaxNameTableCharCount
		{
			get
			{
				return this.maxNameTableCharCount;
			}
			set
			{
				if (this.readOnly)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidOperationException(global::System.Runtime.Serialization.SR.GetString("The '{0}' quota is readonly.", new object[] { "MaxNameTableCharCount" })));
				}
				if (value <= 0)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(global::System.Runtime.Serialization.SR.GetString("Quota must be a positive value."), "value"));
				}
				this.maxNameTableCharCount = value;
				this.modifiedQuotas |= XmlDictionaryReaderQuotaTypes.MaxNameTableCharCount;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x00015D7C File Offset: 0x00013F7C
		public XmlDictionaryReaderQuotaTypes ModifiedQuotas
		{
			get
			{
				return this.modifiedQuotas;
			}
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x00015D84 File Offset: 0x00013F84
		internal void MakeReadOnly()
		{
			this.readOnly = true;
		}

		// Token: 0x040001EB RID: 491
		private bool readOnly;

		// Token: 0x040001EC RID: 492
		private int maxStringContentLength;

		// Token: 0x040001ED RID: 493
		private int maxArrayLength;

		// Token: 0x040001EE RID: 494
		private int maxDepth;

		// Token: 0x040001EF RID: 495
		private int maxNameTableCharCount;

		// Token: 0x040001F0 RID: 496
		private int maxBytesPerRead;

		// Token: 0x040001F1 RID: 497
		private XmlDictionaryReaderQuotaTypes modifiedQuotas;

		// Token: 0x040001F2 RID: 498
		private const int DefaultMaxDepth = 32;

		// Token: 0x040001F3 RID: 499
		private const int DefaultMaxStringContentLength = 8192;

		// Token: 0x040001F4 RID: 500
		private const int DefaultMaxArrayLength = 16384;

		// Token: 0x040001F5 RID: 501
		private const int DefaultMaxBytesPerRead = 4096;

		// Token: 0x040001F6 RID: 502
		private const int DefaultMaxNameTableCharCount = 16384;

		// Token: 0x040001F7 RID: 503
		private static XmlDictionaryReaderQuotas defaultQuota = new XmlDictionaryReaderQuotas(32, 8192, 16384, 4096, 16384, (XmlDictionaryReaderQuotaTypes)0);

		// Token: 0x040001F8 RID: 504
		private static XmlDictionaryReaderQuotas maxQuota = new XmlDictionaryReaderQuotas(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue, XmlDictionaryReaderQuotaTypes.MaxDepth | XmlDictionaryReaderQuotaTypes.MaxStringContentLength | XmlDictionaryReaderQuotaTypes.MaxArrayLength | XmlDictionaryReaderQuotaTypes.MaxBytesPerRead | XmlDictionaryReaderQuotaTypes.MaxNameTableCharCount);
	}
}
