using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Net.Mail;
using System.Text;

namespace System.Net.Mime
{
	// Token: 0x02000245 RID: 581
	internal class HeaderCollection : NameValueCollection
	{
		// Token: 0x06001608 RID: 5640 RVA: 0x00071C2B File Offset: 0x0006FE2B
		internal HeaderCollection()
			: base(StringComparer.OrdinalIgnoreCase)
		{
		}

		// Token: 0x06001609 RID: 5641 RVA: 0x00071C38 File Offset: 0x0006FE38
		public override void Remove(string name)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, "Remove", name);
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "name" }), "name");
			}
			MailHeaderID id = MailHeaderInfo.GetID(name);
			if (id == MailHeaderID.ContentType && this.part != null)
			{
				this.part.ContentType = null;
			}
			else if (id == MailHeaderID.ContentDisposition && this.part is MimePart)
			{
				((MimePart)this.part).ContentDisposition = null;
			}
			base.Remove(name);
		}

		// Token: 0x0600160A RID: 5642 RVA: 0x00071CE8 File Offset: 0x0006FEE8
		public override string Get(string name)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, "Get", name);
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "name" }), "name");
			}
			MailHeaderID id = MailHeaderInfo.GetID(name);
			if (id == MailHeaderID.ContentType && this.part != null)
			{
				this.part.ContentType.PersistIfNeeded(this, false);
			}
			else if (id == MailHeaderID.ContentDisposition && this.part is MimePart)
			{
				((MimePart)this.part).ContentDisposition.PersistIfNeeded(this, false);
			}
			return base.Get(name);
		}

		// Token: 0x0600160B RID: 5643 RVA: 0x00071DA4 File Offset: 0x0006FFA4
		public override string[] GetValues(string name)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, "Get", name);
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "name" }), "name");
			}
			MailHeaderID id = MailHeaderInfo.GetID(name);
			if (id == MailHeaderID.ContentType && this.part != null)
			{
				this.part.ContentType.PersistIfNeeded(this, false);
			}
			else if (id == MailHeaderID.ContentDisposition && this.part is MimePart)
			{
				((MimePart)this.part).ContentDisposition.PersistIfNeeded(this, false);
			}
			return base.GetValues(name);
		}

		// Token: 0x0600160C RID: 5644 RVA: 0x00071E5D File Offset: 0x0007005D
		internal void InternalRemove(string name)
		{
			base.Remove(name);
		}

		// Token: 0x0600160D RID: 5645 RVA: 0x00071E66 File Offset: 0x00070066
		internal void InternalSet(string name, string value)
		{
			base.Set(name, value);
		}

		// Token: 0x0600160E RID: 5646 RVA: 0x00071E70 File Offset: 0x00070070
		internal void InternalAdd(string name, string value)
		{
			if (MailHeaderInfo.IsSingleton(name))
			{
				base.Set(name, value);
				return;
			}
			base.Add(name, value);
		}

		// Token: 0x0600160F RID: 5647 RVA: 0x00071E8C File Offset: 0x0007008C
		public override void Set(string name, string value)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, "Set", name.ToString() + "=" + value.ToString());
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (name == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "name" }), "name");
			}
			if (value == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "value" }), "value");
			}
			if (!MimeBasePart.IsAscii(name, false))
			{
				throw new FormatException(SR.GetString("InvalidHeaderName"));
			}
			name = MailHeaderInfo.NormalizeCase(name);
			MailHeaderID id = MailHeaderInfo.GetID(name);
			value = value.Normalize(NormalizationForm.FormC);
			if (id == MailHeaderID.ContentType && this.part != null)
			{
				this.part.ContentType.Set(value.ToLower(CultureInfo.InvariantCulture), this);
				return;
			}
			if (id == MailHeaderID.ContentDisposition && this.part is MimePart)
			{
				((MimePart)this.part).ContentDisposition.Set(value.ToLower(CultureInfo.InvariantCulture), this);
				return;
			}
			base.Set(name, value);
		}

		// Token: 0x06001610 RID: 5648 RVA: 0x00071FD8 File Offset: 0x000701D8
		public override void Add(string name, string value)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, this, "Add", name.ToString() + "=" + value.ToString());
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (name == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "name" }), "name");
			}
			if (value == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "value" }), "value");
			}
			MailBnfHelper.ValidateHeaderName(name);
			name = MailHeaderInfo.NormalizeCase(name);
			MailHeaderID id = MailHeaderInfo.GetID(name);
			value = value.Normalize(NormalizationForm.FormC);
			if (id == MailHeaderID.ContentType && this.part != null)
			{
				this.part.ContentType.Set(value.ToLower(CultureInfo.InvariantCulture), this);
				return;
			}
			if (id == MailHeaderID.ContentDisposition && this.part is MimePart)
			{
				((MimePart)this.part).ContentDisposition.Set(value.ToLower(CultureInfo.InvariantCulture), this);
				return;
			}
			this.InternalAdd(name, value);
		}

		// Token: 0x04001704 RID: 5892
		private MimeBasePart part;
	}
}
