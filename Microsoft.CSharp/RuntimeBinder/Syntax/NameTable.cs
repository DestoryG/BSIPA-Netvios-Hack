using System;

namespace Microsoft.CSharp.RuntimeBinder.Syntax
{
	// Token: 0x02000025 RID: 37
	internal sealed class NameTable
	{
		// Token: 0x06000160 RID: 352 RVA: 0x00009970 File Offset: 0x00007B70
		internal NameTable()
		{
			this._mask = 31;
			this._entries = new NameTable.Entry[this._mask + 1];
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00009994 File Offset: 0x00007B94
		public Name Add(string key)
		{
			int num = NameTable.ComputeHashCode(key);
			for (NameTable.Entry entry = this._entries[num & this._mask]; entry != null; entry = entry.Next)
			{
				if (entry.HashCode == num && entry.Name.Text.Equals(key))
				{
					return entry.Name;
				}
			}
			return this.AddEntry(new Name(key), num);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x000099F4 File Offset: 0x00007BF4
		public Name Add(string key, int length)
		{
			int num = NameTable.ComputeHashCode(key, length);
			for (NameTable.Entry entry = this._entries[num & this._mask]; entry != null; entry = entry.Next)
			{
				if (entry.HashCode == num && NameTable.Equals(entry.Name.Text, key, length))
				{
					return entry.Name;
				}
			}
			return this.AddEntry(new Name(key.Substring(0, length)), num);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00009A60 File Offset: 0x00007C60
		internal void Add(Name name)
		{
			int num = NameTable.ComputeHashCode(name.Text);
			this.AddEntry(name, num);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00009A84 File Offset: 0x00007C84
		public Name Lookup(string key)
		{
			int num = NameTable.ComputeHashCode(key);
			for (NameTable.Entry entry = this._entries[num & this._mask]; entry != null; entry = entry.Next)
			{
				if (entry.HashCode == num && entry.Name.Text.Equals(key))
				{
					return entry.Name;
				}
			}
			return null;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00009AD8 File Offset: 0x00007CD8
		public Name Lookup(string key, int length)
		{
			int num = NameTable.ComputeHashCode(key, length);
			for (NameTable.Entry entry = this._entries[num & this._mask]; entry != null; entry = entry.Next)
			{
				if (entry.HashCode == num && NameTable.Equals(entry.Name.Text, key, length))
				{
					return entry.Name;
				}
			}
			return null;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00009B30 File Offset: 0x00007D30
		private static int ComputeHashCode(string key)
		{
			int num = key.Length;
			for (int i = 0; i < key.Length; i++)
			{
				num += (num << 7) ^ (int)key[i];
			}
			num -= num >> 17;
			num -= num >> 11;
			return num - (num >> 5);
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00009B78 File Offset: 0x00007D78
		private static int ComputeHashCode(string key, int length)
		{
			int num = length;
			for (int i = 0; i < length; i++)
			{
				num += (num << 7) ^ (int)key[i];
			}
			num -= num >> 17;
			num -= num >> 11;
			return num - (num >> 5);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00009BB8 File Offset: 0x00007DB8
		private static bool Equals(string candidate, string key, int length)
		{
			if (candidate.Length != length)
			{
				return false;
			}
			for (int i = 0; i < candidate.Length; i++)
			{
				if (candidate[i] != key[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00009BF4 File Offset: 0x00007DF4
		private Name AddEntry(Name name, int hashCode)
		{
			int num = hashCode & this._mask;
			NameTable.Entry entry = new NameTable.Entry(name, hashCode, this._entries[num]);
			this._entries[num] = entry;
			int count = this._count;
			this._count = count + 1;
			if (count == this._mask)
			{
				this.Grow();
			}
			return entry.Name;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x00009C48 File Offset: 0x00007E48
		private void Grow()
		{
			int num = this._mask * 2 + 1;
			NameTable.Entry[] entries = this._entries;
			NameTable.Entry[] array = new NameTable.Entry[num + 1];
			foreach (NameTable.Entry entry in entries)
			{
				while (entry != null)
				{
					int num2 = entry.HashCode & num;
					NameTable.Entry next = entry.Next;
					entry.Next = array[num2];
					array[num2] = entry;
					entry = next;
				}
			}
			this._entries = array;
			this._mask = num;
		}

		// Token: 0x04000126 RID: 294
		private NameTable.Entry[] _entries;

		// Token: 0x04000127 RID: 295
		private int _count;

		// Token: 0x04000128 RID: 296
		private int _mask;

		// Token: 0x020000DB RID: 219
		private sealed class Entry
		{
			// Token: 0x060006BE RID: 1726 RVA: 0x0001FADB File Offset: 0x0001DCDB
			public Entry(Name name, int hashCode, NameTable.Entry next)
			{
				this.Name = name;
				this.HashCode = hashCode;
				this.Next = next;
			}

			// Token: 0x04000696 RID: 1686
			public readonly Name Name;

			// Token: 0x04000697 RID: 1687
			public readonly int HashCode;

			// Token: 0x04000698 RID: 1688
			public NameTable.Entry Next;
		}
	}
}
