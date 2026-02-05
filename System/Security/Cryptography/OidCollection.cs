using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;

namespace System.Security.Cryptography
{
	// Token: 0x0200045F RID: 1119
	public sealed class OidCollection : ICollection, IEnumerable
	{
		// Token: 0x0600298B RID: 10635 RVA: 0x000BC7AD File Offset: 0x000BA9AD
		public OidCollection()
		{
			this.m_list = new ArrayList();
		}

		// Token: 0x0600298C RID: 10636 RVA: 0x000BC7C0 File Offset: 0x000BA9C0
		public int Add(Oid oid)
		{
			return this.m_list.Add(oid);
		}

		// Token: 0x17000A12 RID: 2578
		public Oid this[int index]
		{
			get
			{
				return this.m_list[index] as Oid;
			}
		}

		// Token: 0x17000A13 RID: 2579
		public Oid this[string oid]
		{
			get
			{
				string text = global::System.Security.Cryptography.X509Certificates.X509Utils.FindOidInfoWithFallback(2U, oid, OidGroup.All);
				if (text == null)
				{
					text = oid;
				}
				foreach (object obj in this.m_list)
				{
					Oid oid2 = (Oid)obj;
					if (oid2.Value == text)
					{
						return oid2;
					}
				}
				return null;
			}
		}

		// Token: 0x17000A14 RID: 2580
		// (get) Token: 0x0600298F RID: 10639 RVA: 0x000BC85C File Offset: 0x000BAA5C
		public int Count
		{
			get
			{
				return this.m_list.Count;
			}
		}

		// Token: 0x06002990 RID: 10640 RVA: 0x000BC869 File Offset: 0x000BAA69
		public OidEnumerator GetEnumerator()
		{
			return new OidEnumerator(this);
		}

		// Token: 0x06002991 RID: 10641 RVA: 0x000BC871 File Offset: 0x000BAA71
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new OidEnumerator(this);
		}

		// Token: 0x06002992 RID: 10642 RVA: 0x000BC87C File Offset: 0x000BAA7C
		void ICollection.CopyTo(Array array, int index)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException(global::System.SR.GetString("Arg_RankMultiDimNotSupported"));
			}
			if (index < 0 || index >= array.Length)
			{
				throw new ArgumentOutOfRangeException("index", global::System.SR.GetString("ArgumentOutOfRange_Index"));
			}
			if (index + this.Count > array.Length)
			{
				throw new ArgumentException(global::System.SR.GetString("Argument_InvalidOffLen"));
			}
			for (int i = 0; i < this.Count; i++)
			{
				array.SetValue(this[i], index);
				index++;
			}
		}

		// Token: 0x06002993 RID: 10643 RVA: 0x000BC916 File Offset: 0x000BAB16
		public void CopyTo(Oid[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		// Token: 0x17000A15 RID: 2581
		// (get) Token: 0x06002994 RID: 10644 RVA: 0x000BC920 File Offset: 0x000BAB20
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000A16 RID: 2582
		// (get) Token: 0x06002995 RID: 10645 RVA: 0x000BC923 File Offset: 0x000BAB23
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x04002592 RID: 9618
		private ArrayList m_list;
	}
}
