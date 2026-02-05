using System;

namespace System.Collections.Specialized
{
	// Token: 0x020003A8 RID: 936
	public class CollectionsUtil
	{
		// Token: 0x060022EF RID: 8943 RVA: 0x000A60AD File Offset: 0x000A42AD
		public static Hashtable CreateCaseInsensitiveHashtable()
		{
			return new Hashtable(StringComparer.CurrentCultureIgnoreCase);
		}

		// Token: 0x060022F0 RID: 8944 RVA: 0x000A60B9 File Offset: 0x000A42B9
		public static Hashtable CreateCaseInsensitiveHashtable(int capacity)
		{
			return new Hashtable(capacity, StringComparer.CurrentCultureIgnoreCase);
		}

		// Token: 0x060022F1 RID: 8945 RVA: 0x000A60C6 File Offset: 0x000A42C6
		public static Hashtable CreateCaseInsensitiveHashtable(IDictionary d)
		{
			return new Hashtable(d, StringComparer.CurrentCultureIgnoreCase);
		}

		// Token: 0x060022F2 RID: 8946 RVA: 0x000A60D3 File Offset: 0x000A42D3
		public static SortedList CreateCaseInsensitiveSortedList()
		{
			return new SortedList(CaseInsensitiveComparer.Default);
		}
	}
}
