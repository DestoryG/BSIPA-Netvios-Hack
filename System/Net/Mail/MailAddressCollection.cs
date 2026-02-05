using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x0200026B RID: 619
	public class MailAddressCollection : Collection<MailAddress>
	{
		// Token: 0x06001743 RID: 5955 RVA: 0x00076B74 File Offset: 0x00074D74
		public void Add(string addresses)
		{
			if (addresses == null)
			{
				throw new ArgumentNullException("addresses");
			}
			if (addresses == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "addresses" }), "addresses");
			}
			this.ParseValue(addresses);
		}

		// Token: 0x06001744 RID: 5956 RVA: 0x00076BC6 File Offset: 0x00074DC6
		protected override void SetItem(int index, MailAddress item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			base.SetItem(index, item);
		}

		// Token: 0x06001745 RID: 5957 RVA: 0x00076BDE File Offset: 0x00074DDE
		protected override void InsertItem(int index, MailAddress item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			base.InsertItem(index, item);
		}

		// Token: 0x06001746 RID: 5958 RVA: 0x00076BF8 File Offset: 0x00074DF8
		internal void ParseValue(string addresses)
		{
			IList<MailAddress> list = MailAddressParser.ParseMultipleAddresses(addresses);
			for (int i = 0; i < list.Count; i++)
			{
				base.Add(list[i]);
			}
		}

		// Token: 0x06001747 RID: 5959 RVA: 0x00076C2C File Offset: 0x00074E2C
		public override string ToString()
		{
			bool flag = true;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (MailAddress mailAddress in this)
			{
				if (!flag)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(mailAddress.ToString());
				flag = false;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001748 RID: 5960 RVA: 0x00076C9C File Offset: 0x00074E9C
		internal string Encode(int charsConsumed, bool allowUnicode)
		{
			string text = string.Empty;
			foreach (MailAddress mailAddress in this)
			{
				if (string.IsNullOrEmpty(text))
				{
					text = mailAddress.Encode(charsConsumed, allowUnicode);
				}
				else
				{
					text = text + ", " + mailAddress.Encode(1, allowUnicode);
				}
			}
			return text;
		}
	}
}
