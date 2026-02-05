using System;

namespace System.Collections.Specialized
{
	// Token: 0x020003B8 RID: 952
	[Serializable]
	internal class StringDictionaryWithComparer : StringDictionary
	{
		// Token: 0x060023DB RID: 9179 RVA: 0x000A8A1D File Offset: 0x000A6C1D
		public StringDictionaryWithComparer()
			: this(StringComparer.OrdinalIgnoreCase)
		{
		}

		// Token: 0x060023DC RID: 9180 RVA: 0x000A8A2A File Offset: 0x000A6C2A
		public StringDictionaryWithComparer(IEqualityComparer comparer)
		{
			base.ReplaceHashtable(new Hashtable(comparer));
		}

		// Token: 0x17000917 RID: 2327
		public override string this[string key]
		{
			get
			{
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				return (string)this.contents[key];
			}
			set
			{
				if (key == null)
				{
					throw new ArgumentNullException("key");
				}
				this.contents[key] = value;
			}
		}

		// Token: 0x060023DF RID: 9183 RVA: 0x000A8A7C File Offset: 0x000A6C7C
		public override void Add(string key, string value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this.contents.Add(key, value);
		}

		// Token: 0x060023E0 RID: 9184 RVA: 0x000A8A99 File Offset: 0x000A6C99
		public override bool ContainsKey(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			return this.contents.ContainsKey(key);
		}

		// Token: 0x060023E1 RID: 9185 RVA: 0x000A8AB5 File Offset: 0x000A6CB5
		public override void Remove(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			this.contents.Remove(key);
		}
	}
}
