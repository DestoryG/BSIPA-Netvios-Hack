using System;
using System.Collections;

namespace System.Configuration
{
	// Token: 0x020000AE RID: 174
	public class SettingsPropertyValueCollection : IEnumerable, ICloneable, ICollection
	{
		// Token: 0x060005FC RID: 1532 RVA: 0x00023A1C File Offset: 0x00021C1C
		public SettingsPropertyValueCollection()
		{
			this._Indices = new Hashtable(10, StringComparer.CurrentCultureIgnoreCase);
			this._Values = new ArrayList();
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00023A44 File Offset: 0x00021C44
		public void Add(SettingsPropertyValue property)
		{
			if (this._ReadOnly)
			{
				throw new NotSupportedException();
			}
			int num = this._Values.Add(property);
			try
			{
				this._Indices.Add(property.Name, num);
			}
			catch (Exception)
			{
				this._Values.RemoveAt(num);
				throw;
			}
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00023AA4 File Offset: 0x00021CA4
		public void Remove(string name)
		{
			if (this._ReadOnly)
			{
				throw new NotSupportedException();
			}
			object obj = this._Indices[name];
			if (obj == null || !(obj is int))
			{
				return;
			}
			int num = (int)obj;
			if (num >= this._Values.Count)
			{
				return;
			}
			this._Values.RemoveAt(num);
			this._Indices.Remove(name);
			ArrayList arrayList = new ArrayList();
			foreach (object obj2 in this._Indices)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
				if ((int)dictionaryEntry.Value > num)
				{
					arrayList.Add(dictionaryEntry.Key);
				}
			}
			foreach (object obj3 in arrayList)
			{
				string text = (string)obj3;
				this._Indices[text] = (int)this._Indices[text] - 1;
			}
		}

		// Token: 0x170000F8 RID: 248
		public SettingsPropertyValue this[string name]
		{
			get
			{
				object obj = this._Indices[name];
				if (obj == null || !(obj is int))
				{
					return null;
				}
				int num = (int)obj;
				if (num >= this._Values.Count)
				{
					return null;
				}
				return (SettingsPropertyValue)this._Values[num];
			}
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x00023C2B File Offset: 0x00021E2B
		public IEnumerator GetEnumerator()
		{
			return this._Values.GetEnumerator();
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x00023C38 File Offset: 0x00021E38
		public object Clone()
		{
			return new SettingsPropertyValueCollection(this._Indices, this._Values);
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00023C4B File Offset: 0x00021E4B
		public void SetReadOnly()
		{
			if (this._ReadOnly)
			{
				return;
			}
			this._ReadOnly = true;
			this._Values = ArrayList.ReadOnly(this._Values);
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x00023C6E File Offset: 0x00021E6E
		public void Clear()
		{
			this._Values.Clear();
			this._Indices.Clear();
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000604 RID: 1540 RVA: 0x00023C86 File Offset: 0x00021E86
		public int Count
		{
			get
			{
				return this._Values.Count;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000605 RID: 1541 RVA: 0x00023C93 File Offset: 0x00021E93
		public bool IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000606 RID: 1542 RVA: 0x00023C96 File Offset: 0x00021E96
		public object SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x00023C99 File Offset: 0x00021E99
		public void CopyTo(Array array, int index)
		{
			this._Values.CopyTo(array, index);
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x00023CA8 File Offset: 0x00021EA8
		private SettingsPropertyValueCollection(Hashtable indices, ArrayList values)
		{
			this._Indices = (Hashtable)indices.Clone();
			this._Values = (ArrayList)values.Clone();
		}

		// Token: 0x04000C5B RID: 3163
		private Hashtable _Indices;

		// Token: 0x04000C5C RID: 3164
		private ArrayList _Values;

		// Token: 0x04000C5D RID: 3165
		private bool _ReadOnly;
	}
}
