using System;
using System.Collections;

namespace System.Net
{
	// Token: 0x020000D9 RID: 217
	[Serializable]
	internal class PathList
	{
		// Token: 0x1700015F RID: 351
		// (get) Token: 0x0600076B RID: 1899 RVA: 0x000296AD File Offset: 0x000278AD
		public int Count
		{
			get
			{
				return this.m_list.Count;
			}
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x000296BC File Offset: 0x000278BC
		public int GetCookiesCount()
		{
			int num = 0;
			object syncRoot = this.SyncRoot;
			lock (syncRoot)
			{
				foreach (object obj in this.m_list.Values)
				{
					CookieCollection cookieCollection = (CookieCollection)obj;
					num += cookieCollection.Count;
				}
			}
			return num;
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x0600076D RID: 1901 RVA: 0x0002974C File Offset: 0x0002794C
		public ICollection Values
		{
			get
			{
				return this.m_list.Values;
			}
		}

		// Token: 0x17000161 RID: 353
		public object this[string s]
		{
			get
			{
				return this.m_list[s];
			}
			set
			{
				object syncRoot = this.SyncRoot;
				lock (syncRoot)
				{
					this.m_list[s] = value;
				}
			}
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x000297B0 File Offset: 0x000279B0
		public IEnumerator GetEnumerator()
		{
			return this.m_list.GetEnumerator();
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000771 RID: 1905 RVA: 0x000297BD File Offset: 0x000279BD
		public object SyncRoot
		{
			get
			{
				return this.m_list.SyncRoot;
			}
		}

		// Token: 0x04000D19 RID: 3353
		private SortedList m_list = SortedList.Synchronized(new SortedList(PathList.PathListComparer.StaticInstance));

		// Token: 0x020006F5 RID: 1781
		[Serializable]
		private class PathListComparer : IComparer
		{
			// Token: 0x06004073 RID: 16499 RVA: 0x0010E578 File Offset: 0x0010C778
			int IComparer.Compare(object ol, object or)
			{
				string text = CookieParser.CheckQuoted((string)ol);
				string text2 = CookieParser.CheckQuoted((string)or);
				int length = text.Length;
				int length2 = text2.Length;
				int num = Math.Min(length, length2);
				for (int i = 0; i < num; i++)
				{
					if (text[i] != text2[i])
					{
						return (int)(text[i] - text2[i]);
					}
				}
				return length2 - length;
			}

			// Token: 0x04003095 RID: 12437
			internal static readonly PathList.PathListComparer StaticInstance = new PathList.PathListComparer();
		}
	}
}
