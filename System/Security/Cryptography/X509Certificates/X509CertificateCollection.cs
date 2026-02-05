using System;
using System.Collections;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000483 RID: 1155
	[Serializable]
	public class X509CertificateCollection : CollectionBase
	{
		// Token: 0x06002AC7 RID: 10951 RVA: 0x000C2F43 File Offset: 0x000C1143
		public X509CertificateCollection()
		{
		}

		// Token: 0x06002AC8 RID: 10952 RVA: 0x000C2F4B File Offset: 0x000C114B
		public X509CertificateCollection(X509CertificateCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x06002AC9 RID: 10953 RVA: 0x000C2F5A File Offset: 0x000C115A
		public X509CertificateCollection(X509Certificate[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x17000A5D RID: 2653
		public X509Certificate this[int index]
		{
			get
			{
				return (X509Certificate)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06002ACC RID: 10956 RVA: 0x000C2F8B File Offset: 0x000C118B
		public int Add(X509Certificate value)
		{
			return base.List.Add(value);
		}

		// Token: 0x06002ACD RID: 10957 RVA: 0x000C2F9C File Offset: 0x000C119C
		public void AddRange(X509Certificate[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			for (int i = 0; i < value.Length; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x06002ACE RID: 10958 RVA: 0x000C2FD0 File Offset: 0x000C11D0
		public void AddRange(X509CertificateCollection value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			for (int i = 0; i < value.Count; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x06002ACF RID: 10959 RVA: 0x000C300C File Offset: 0x000C120C
		public bool Contains(X509Certificate value)
		{
			foreach (object obj in base.List)
			{
				X509Certificate x509Certificate = (X509Certificate)obj;
				if (x509Certificate.Equals(value))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002AD0 RID: 10960 RVA: 0x000C3070 File Offset: 0x000C1270
		public void CopyTo(X509Certificate[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06002AD1 RID: 10961 RVA: 0x000C307F File Offset: 0x000C127F
		public int IndexOf(X509Certificate value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x06002AD2 RID: 10962 RVA: 0x000C308D File Offset: 0x000C128D
		public void Insert(int index, X509Certificate value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x06002AD3 RID: 10963 RVA: 0x000C309C File Offset: 0x000C129C
		public new X509CertificateCollection.X509CertificateEnumerator GetEnumerator()
		{
			return new X509CertificateCollection.X509CertificateEnumerator(this);
		}

		// Token: 0x06002AD4 RID: 10964 RVA: 0x000C30A4 File Offset: 0x000C12A4
		public void Remove(X509Certificate value)
		{
			base.List.Remove(value);
		}

		// Token: 0x06002AD5 RID: 10965 RVA: 0x000C30B4 File Offset: 0x000C12B4
		public override int GetHashCode()
		{
			int num = 0;
			foreach (X509Certificate x509Certificate in this)
			{
				num += x509Certificate.GetHashCode();
			}
			return num;
		}

		// Token: 0x0200087B RID: 2171
		public class X509CertificateEnumerator : IEnumerator
		{
			// Token: 0x0600456A RID: 17770 RVA: 0x001218ED File Offset: 0x0011FAED
			public X509CertificateEnumerator(X509CertificateCollection mappings)
			{
				this.temp = mappings;
				this.baseEnumerator = this.temp.GetEnumerator();
			}

			// Token: 0x17000FB2 RID: 4018
			// (get) Token: 0x0600456B RID: 17771 RVA: 0x0012190D File Offset: 0x0011FB0D
			public X509Certificate Current
			{
				get
				{
					return (X509Certificate)this.baseEnumerator.Current;
				}
			}

			// Token: 0x17000FB3 RID: 4019
			// (get) Token: 0x0600456C RID: 17772 RVA: 0x0012191F File Offset: 0x0011FB1F
			object IEnumerator.Current
			{
				get
				{
					return this.baseEnumerator.Current;
				}
			}

			// Token: 0x0600456D RID: 17773 RVA: 0x0012192C File Offset: 0x0011FB2C
			public bool MoveNext()
			{
				return this.baseEnumerator.MoveNext();
			}

			// Token: 0x0600456E RID: 17774 RVA: 0x00121939 File Offset: 0x0011FB39
			bool IEnumerator.MoveNext()
			{
				return this.baseEnumerator.MoveNext();
			}

			// Token: 0x0600456F RID: 17775 RVA: 0x00121946 File Offset: 0x0011FB46
			public void Reset()
			{
				this.baseEnumerator.Reset();
			}

			// Token: 0x06004570 RID: 17776 RVA: 0x00121953 File Offset: 0x0011FB53
			void IEnumerator.Reset()
			{
				this.baseEnumerator.Reset();
			}

			// Token: 0x0400371F RID: 14111
			private IEnumerator baseEnumerator;

			// Token: 0x04003720 RID: 14112
			private IEnumerable temp;
		}
	}
}
