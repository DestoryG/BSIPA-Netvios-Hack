using System;

namespace System.Collections.Specialized
{
	// Token: 0x020003AC RID: 940
	public interface IOrderedDictionary : IDictionary, ICollection, IEnumerable
	{
		// Token: 0x170008E2 RID: 2274
		object this[int index] { get; set; }

		// Token: 0x06002311 RID: 8977
		IDictionaryEnumerator GetEnumerator();

		// Token: 0x06002312 RID: 8978
		void Insert(int index, object key, object value);

		// Token: 0x06002313 RID: 8979
		void RemoveAt(int index);
	}
}
