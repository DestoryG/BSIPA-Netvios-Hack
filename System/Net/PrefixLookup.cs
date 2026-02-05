using System;
using System.Collections.Generic;

namespace System.Net
{
	// Token: 0x020001DD RID: 477
	internal class PrefixLookup
	{
		// Token: 0x060012B5 RID: 4789 RVA: 0x000633F2 File Offset: 0x000615F2
		public PrefixLookup()
			: this(100)
		{
		}

		// Token: 0x060012B6 RID: 4790 RVA: 0x000633FC File Offset: 0x000615FC
		public PrefixLookup(int capacity)
		{
			this.capacity = capacity;
		}

		// Token: 0x060012B7 RID: 4791 RVA: 0x00063418 File Offset: 0x00061618
		public void Add(string prefix, object value)
		{
			if (this.capacity == 0 || prefix == null || prefix.Length == 0 || value == null)
			{
				return;
			}
			LinkedList<PrefixLookup.PrefixValuePair> linkedList = this.lruList;
			lock (linkedList)
			{
				if (this.lruList.First != null && this.lruList.First.Value.prefix.Equals(prefix))
				{
					this.lruList.First.Value.value = value;
				}
				else
				{
					this.lruList.AddFirst(new PrefixLookup.PrefixValuePair(prefix, value));
					while (this.lruList.Count > this.capacity)
					{
						this.lruList.RemoveLast();
					}
				}
			}
		}

		// Token: 0x060012B8 RID: 4792 RVA: 0x000634E4 File Offset: 0x000616E4
		public object Lookup(string lookupKey)
		{
			if (lookupKey == null || lookupKey.Length == 0 || this.lruList.Count == 0)
			{
				return null;
			}
			LinkedListNode<PrefixLookup.PrefixValuePair> linkedListNode = null;
			LinkedList<PrefixLookup.PrefixValuePair> linkedList = this.lruList;
			lock (linkedList)
			{
				int num = 0;
				for (LinkedListNode<PrefixLookup.PrefixValuePair> linkedListNode2 = this.lruList.First; linkedListNode2 != null; linkedListNode2 = linkedListNode2.Next)
				{
					string prefix = linkedListNode2.Value.prefix;
					if (prefix.Length > num && lookupKey.StartsWith(prefix))
					{
						num = prefix.Length;
						linkedListNode = linkedListNode2;
						if (num == lookupKey.Length)
						{
							break;
						}
					}
				}
				if (linkedListNode != null && linkedListNode != this.lruList.First)
				{
					this.lruList.Remove(linkedListNode);
					this.lruList.AddFirst(linkedListNode);
				}
			}
			if (linkedListNode == null)
			{
				return null;
			}
			return linkedListNode.Value.value;
		}

		// Token: 0x0400150C RID: 5388
		private const int defaultCapacity = 100;

		// Token: 0x0400150D RID: 5389
		private volatile int capacity;

		// Token: 0x0400150E RID: 5390
		private readonly LinkedList<PrefixLookup.PrefixValuePair> lruList = new LinkedList<PrefixLookup.PrefixValuePair>();

		// Token: 0x02000754 RID: 1876
		private class PrefixValuePair
		{
			// Token: 0x060041F8 RID: 16888 RVA: 0x001120B7 File Offset: 0x001102B7
			public PrefixValuePair(string pre, object val)
			{
				this.prefix = pre;
				this.value = val;
			}

			// Token: 0x04003207 RID: 12807
			public string prefix;

			// Token: 0x04003208 RID: 12808
			public object value;
		}
	}
}
