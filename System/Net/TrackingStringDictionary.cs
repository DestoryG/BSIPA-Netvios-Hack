using System;
using System.Collections.Specialized;

namespace System.Net
{
	// Token: 0x02000228 RID: 552
	internal class TrackingStringDictionary : StringDictionary
	{
		// Token: 0x06001456 RID: 5206 RVA: 0x0006B9C4 File Offset: 0x00069BC4
		internal TrackingStringDictionary()
			: this(false)
		{
		}

		// Token: 0x06001457 RID: 5207 RVA: 0x0006B9CD File Offset: 0x00069BCD
		internal TrackingStringDictionary(bool isReadOnly)
		{
			this.isReadOnly = isReadOnly;
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x06001458 RID: 5208 RVA: 0x0006B9DC File Offset: 0x00069BDC
		// (set) Token: 0x06001459 RID: 5209 RVA: 0x0006B9E4 File Offset: 0x00069BE4
		internal bool IsChanged
		{
			get
			{
				return this.isChanged;
			}
			set
			{
				this.isChanged = value;
			}
		}

		// Token: 0x0600145A RID: 5210 RVA: 0x0006B9ED File Offset: 0x00069BED
		public override void Add(string key, string value)
		{
			if (this.isReadOnly)
			{
				throw new InvalidOperationException(SR.GetString("MailCollectionIsReadOnly"));
			}
			base.Add(key, value);
			this.isChanged = true;
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x0006BA16 File Offset: 0x00069C16
		public override void Clear()
		{
			if (this.isReadOnly)
			{
				throw new InvalidOperationException(SR.GetString("MailCollectionIsReadOnly"));
			}
			base.Clear();
			this.isChanged = true;
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x0006BA3D File Offset: 0x00069C3D
		public override void Remove(string key)
		{
			if (this.isReadOnly)
			{
				throw new InvalidOperationException(SR.GetString("MailCollectionIsReadOnly"));
			}
			base.Remove(key);
			this.isChanged = true;
		}

		// Token: 0x17000441 RID: 1089
		public override string this[string key]
		{
			get
			{
				return base[key];
			}
			set
			{
				if (this.isReadOnly)
				{
					throw new InvalidOperationException(SR.GetString("MailCollectionIsReadOnly"));
				}
				base[key] = value;
				this.isChanged = true;
			}
		}

		// Token: 0x04001622 RID: 5666
		private bool isChanged;

		// Token: 0x04001623 RID: 5667
		private bool isReadOnly;
	}
}
