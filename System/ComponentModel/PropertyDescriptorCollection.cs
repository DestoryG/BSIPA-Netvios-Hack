using System;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200059D RID: 1437
	[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
	public class PropertyDescriptorCollection : ICollection, IEnumerable, IList, IDictionary
	{
		// Token: 0x0600354C RID: 13644 RVA: 0x000E7EC3 File Offset: 0x000E60C3
		public PropertyDescriptorCollection(PropertyDescriptor[] properties)
		{
			this.properties = properties;
			if (properties == null)
			{
				this.properties = new PropertyDescriptor[0];
				this.propCount = 0;
			}
			else
			{
				this.propCount = properties.Length;
			}
			this.propsOwned = true;
		}

		// Token: 0x0600354D RID: 13645 RVA: 0x000E7F01 File Offset: 0x000E6101
		public PropertyDescriptorCollection(PropertyDescriptor[] properties, bool readOnly)
			: this(properties)
		{
			this.readOnly = readOnly;
		}

		// Token: 0x0600354E RID: 13646 RVA: 0x000E7F14 File Offset: 0x000E6114
		private PropertyDescriptorCollection(PropertyDescriptor[] properties, int propCount, string[] namedSort, IComparer comparer)
		{
			this.propsOwned = false;
			if (namedSort != null)
			{
				this.namedSort = (string[])namedSort.Clone();
			}
			this.comparer = comparer;
			this.properties = properties;
			this.propCount = propCount;
			this.needSort = true;
		}

		// Token: 0x17000D05 RID: 3333
		// (get) Token: 0x0600354F RID: 13647 RVA: 0x000E7F66 File Offset: 0x000E6166
		public int Count
		{
			get
			{
				return this.propCount;
			}
		}

		// Token: 0x17000D06 RID: 3334
		public virtual PropertyDescriptor this[int index]
		{
			get
			{
				if (index >= this.propCount)
				{
					throw new IndexOutOfRangeException();
				}
				this.EnsurePropsOwned();
				return this.properties[index];
			}
		}

		// Token: 0x17000D07 RID: 3335
		public virtual PropertyDescriptor this[string name]
		{
			get
			{
				return this.Find(name, false);
			}
		}

		// Token: 0x06003552 RID: 13650 RVA: 0x000E7F98 File Offset: 0x000E6198
		public int Add(PropertyDescriptor value)
		{
			if (this.readOnly)
			{
				throw new NotSupportedException();
			}
			this.EnsureSize(this.propCount + 1);
			PropertyDescriptor[] array = this.properties;
			int num = this.propCount;
			this.propCount = num + 1;
			array[num] = value;
			return this.propCount - 1;
		}

		// Token: 0x06003553 RID: 13651 RVA: 0x000E7FE2 File Offset: 0x000E61E2
		public void Clear()
		{
			if (this.readOnly)
			{
				throw new NotSupportedException();
			}
			this.propCount = 0;
			this.cachedFoundProperties = null;
		}

		// Token: 0x06003554 RID: 13652 RVA: 0x000E8000 File Offset: 0x000E6200
		public bool Contains(PropertyDescriptor value)
		{
			return this.IndexOf(value) >= 0;
		}

		// Token: 0x06003555 RID: 13653 RVA: 0x000E800F File Offset: 0x000E620F
		public void CopyTo(Array array, int index)
		{
			this.EnsurePropsOwned();
			Array.Copy(this.properties, 0, array, index, this.Count);
		}

		// Token: 0x06003556 RID: 13654 RVA: 0x000E802C File Offset: 0x000E622C
		private void EnsurePropsOwned()
		{
			if (!this.propsOwned)
			{
				this.propsOwned = true;
				if (this.properties != null)
				{
					PropertyDescriptor[] array = new PropertyDescriptor[this.Count];
					Array.Copy(this.properties, 0, array, 0, this.Count);
					this.properties = array;
				}
			}
			if (this.needSort)
			{
				this.needSort = false;
				this.InternalSort(this.namedSort);
			}
		}

		// Token: 0x06003557 RID: 13655 RVA: 0x000E8094 File Offset: 0x000E6294
		private void EnsureSize(int sizeNeeded)
		{
			if (sizeNeeded <= this.properties.Length)
			{
				return;
			}
			if (this.properties == null || this.properties.Length == 0)
			{
				this.propCount = 0;
				this.properties = new PropertyDescriptor[sizeNeeded];
				return;
			}
			this.EnsurePropsOwned();
			int num = Math.Max(sizeNeeded, this.properties.Length * 2);
			PropertyDescriptor[] array = new PropertyDescriptor[num];
			Array.Copy(this.properties, 0, array, 0, this.propCount);
			this.properties = array;
		}

		// Token: 0x06003558 RID: 13656 RVA: 0x000E810C File Offset: 0x000E630C
		public virtual PropertyDescriptor Find(string name, bool ignoreCase)
		{
			PropertyDescriptor propertyDescriptor2;
			lock (this)
			{
				PropertyDescriptor propertyDescriptor = null;
				if (this.cachedFoundProperties == null || this.cachedIgnoreCase != ignoreCase)
				{
					this.cachedIgnoreCase = ignoreCase;
					this.cachedFoundProperties = new HybridDictionary(ignoreCase);
				}
				object obj = this.cachedFoundProperties[name];
				if (obj != null)
				{
					propertyDescriptor2 = (PropertyDescriptor)obj;
				}
				else
				{
					for (int i = 0; i < this.propCount; i++)
					{
						if (ignoreCase)
						{
							if (string.Equals(this.properties[i].Name, name, StringComparison.OrdinalIgnoreCase))
							{
								this.cachedFoundProperties[name] = this.properties[i];
								propertyDescriptor = this.properties[i];
								break;
							}
						}
						else if (this.properties[i].Name.Equals(name))
						{
							this.cachedFoundProperties[name] = this.properties[i];
							propertyDescriptor = this.properties[i];
							break;
						}
					}
					propertyDescriptor2 = propertyDescriptor;
				}
			}
			return propertyDescriptor2;
		}

		// Token: 0x06003559 RID: 13657 RVA: 0x000E8214 File Offset: 0x000E6414
		public int IndexOf(PropertyDescriptor value)
		{
			return Array.IndexOf<PropertyDescriptor>(this.properties, value, 0, this.propCount);
		}

		// Token: 0x0600355A RID: 13658 RVA: 0x000E822C File Offset: 0x000E642C
		public void Insert(int index, PropertyDescriptor value)
		{
			if (this.readOnly)
			{
				throw new NotSupportedException();
			}
			this.EnsureSize(this.propCount + 1);
			if (index < this.propCount)
			{
				Array.Copy(this.properties, index, this.properties, index + 1, this.propCount - index);
			}
			this.properties[index] = value;
			this.propCount++;
		}

		// Token: 0x0600355B RID: 13659 RVA: 0x000E8294 File Offset: 0x000E6494
		public void Remove(PropertyDescriptor value)
		{
			if (this.readOnly)
			{
				throw new NotSupportedException();
			}
			int num = this.IndexOf(value);
			if (num != -1)
			{
				this.RemoveAt(num);
			}
		}

		// Token: 0x0600355C RID: 13660 RVA: 0x000E82C4 File Offset: 0x000E64C4
		public void RemoveAt(int index)
		{
			if (this.readOnly)
			{
				throw new NotSupportedException();
			}
			if (index < this.propCount - 1)
			{
				Array.Copy(this.properties, index + 1, this.properties, index, this.propCount - index - 1);
			}
			this.properties[this.propCount - 1] = null;
			this.propCount--;
		}

		// Token: 0x0600355D RID: 13661 RVA: 0x000E8327 File Offset: 0x000E6527
		public virtual PropertyDescriptorCollection Sort()
		{
			return new PropertyDescriptorCollection(this.properties, this.propCount, this.namedSort, this.comparer);
		}

		// Token: 0x0600355E RID: 13662 RVA: 0x000E8346 File Offset: 0x000E6546
		public virtual PropertyDescriptorCollection Sort(string[] names)
		{
			return new PropertyDescriptorCollection(this.properties, this.propCount, names, this.comparer);
		}

		// Token: 0x0600355F RID: 13663 RVA: 0x000E8360 File Offset: 0x000E6560
		public virtual PropertyDescriptorCollection Sort(string[] names, IComparer comparer)
		{
			return new PropertyDescriptorCollection(this.properties, this.propCount, names, comparer);
		}

		// Token: 0x06003560 RID: 13664 RVA: 0x000E8375 File Offset: 0x000E6575
		public virtual PropertyDescriptorCollection Sort(IComparer comparer)
		{
			return new PropertyDescriptorCollection(this.properties, this.propCount, this.namedSort, comparer);
		}

		// Token: 0x06003561 RID: 13665 RVA: 0x000E8390 File Offset: 0x000E6590
		protected void InternalSort(string[] names)
		{
			if (this.properties == null || this.properties.Length == 0)
			{
				return;
			}
			this.InternalSort(this.comparer);
			if (names != null && names.Length != 0)
			{
				ArrayList arrayList = new ArrayList(this.properties);
				int num = 0;
				int num2 = this.properties.Length;
				for (int i = 0; i < names.Length; i++)
				{
					for (int j = 0; j < num2; j++)
					{
						PropertyDescriptor propertyDescriptor = (PropertyDescriptor)arrayList[j];
						if (propertyDescriptor != null && propertyDescriptor.Name.Equals(names[i]))
						{
							this.properties[num++] = propertyDescriptor;
							arrayList[j] = null;
							break;
						}
					}
				}
				for (int k = 0; k < num2; k++)
				{
					if (arrayList[k] != null)
					{
						this.properties[num++] = (PropertyDescriptor)arrayList[k];
					}
				}
			}
		}

		// Token: 0x06003562 RID: 13666 RVA: 0x000E846D File Offset: 0x000E666D
		protected void InternalSort(IComparer sorter)
		{
			if (sorter == null)
			{
				TypeDescriptor.SortDescriptorArray(this);
				return;
			}
			Array.Sort(this.properties, sorter);
		}

		// Token: 0x06003563 RID: 13667 RVA: 0x000E8488 File Offset: 0x000E6688
		public virtual IEnumerator GetEnumerator()
		{
			this.EnsurePropsOwned();
			if (this.properties.Length != this.propCount)
			{
				PropertyDescriptor[] array = new PropertyDescriptor[this.propCount];
				Array.Copy(this.properties, 0, array, 0, this.propCount);
				return array.GetEnumerator();
			}
			return this.properties.GetEnumerator();
		}

		// Token: 0x17000D08 RID: 3336
		// (get) Token: 0x06003564 RID: 13668 RVA: 0x000E84DD File Offset: 0x000E66DD
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x17000D09 RID: 3337
		// (get) Token: 0x06003565 RID: 13669 RVA: 0x000E84E5 File Offset: 0x000E66E5
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D0A RID: 3338
		// (get) Token: 0x06003566 RID: 13670 RVA: 0x000E84E8 File Offset: 0x000E66E8
		object ICollection.SyncRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06003567 RID: 13671 RVA: 0x000E84EC File Offset: 0x000E66EC
		void IDictionary.Add(object key, object value)
		{
			PropertyDescriptor propertyDescriptor = value as PropertyDescriptor;
			if (propertyDescriptor == null)
			{
				throw new ArgumentException("value");
			}
			this.Add(propertyDescriptor);
		}

		// Token: 0x06003568 RID: 13672 RVA: 0x000E8516 File Offset: 0x000E6716
		void IDictionary.Clear()
		{
			this.Clear();
		}

		// Token: 0x06003569 RID: 13673 RVA: 0x000E851E File Offset: 0x000E671E
		bool IDictionary.Contains(object key)
		{
			return key is string && this[(string)key] != null;
		}

		// Token: 0x0600356A RID: 13674 RVA: 0x000E8539 File Offset: 0x000E6739
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return new PropertyDescriptorCollection.PropertyDescriptorEnumerator(this);
		}

		// Token: 0x17000D0B RID: 3339
		// (get) Token: 0x0600356B RID: 13675 RVA: 0x000E8541 File Offset: 0x000E6741
		bool IDictionary.IsFixedSize
		{
			get
			{
				return this.readOnly;
			}
		}

		// Token: 0x17000D0C RID: 3340
		// (get) Token: 0x0600356C RID: 13676 RVA: 0x000E8549 File Offset: 0x000E6749
		bool IDictionary.IsReadOnly
		{
			get
			{
				return this.readOnly;
			}
		}

		// Token: 0x17000D0D RID: 3341
		object IDictionary.this[object key]
		{
			get
			{
				if (key is string)
				{
					return this[(string)key];
				}
				return null;
			}
			set
			{
				if (this.readOnly)
				{
					throw new NotSupportedException();
				}
				if (value != null && !(value is PropertyDescriptor))
				{
					throw new ArgumentException("value");
				}
				int num = -1;
				if (key is int)
				{
					num = (int)key;
					if (num < 0 || num >= this.propCount)
					{
						throw new IndexOutOfRangeException();
					}
				}
				else
				{
					if (!(key is string))
					{
						throw new ArgumentException("key");
					}
					for (int i = 0; i < this.propCount; i++)
					{
						if (this.properties[i].Name.Equals((string)key))
						{
							num = i;
							break;
						}
					}
				}
				if (num == -1)
				{
					this.Add((PropertyDescriptor)value);
					return;
				}
				this.EnsurePropsOwned();
				this.properties[num] = (PropertyDescriptor)value;
				if (this.cachedFoundProperties != null && key is string)
				{
					this.cachedFoundProperties[key] = value;
				}
			}
		}

		// Token: 0x17000D0E RID: 3342
		// (get) Token: 0x0600356F RID: 13679 RVA: 0x000E8648 File Offset: 0x000E6848
		ICollection IDictionary.Keys
		{
			get
			{
				string[] array = new string[this.propCount];
				for (int i = 0; i < this.propCount; i++)
				{
					array[i] = this.properties[i].Name;
				}
				return array;
			}
		}

		// Token: 0x17000D0F RID: 3343
		// (get) Token: 0x06003570 RID: 13680 RVA: 0x000E8684 File Offset: 0x000E6884
		ICollection IDictionary.Values
		{
			get
			{
				if (this.properties.Length != this.propCount)
				{
					PropertyDescriptor[] array = new PropertyDescriptor[this.propCount];
					Array.Copy(this.properties, 0, array, 0, this.propCount);
					return array;
				}
				return (ICollection)this.properties.Clone();
			}
		}

		// Token: 0x06003571 RID: 13681 RVA: 0x000E86D4 File Offset: 0x000E68D4
		void IDictionary.Remove(object key)
		{
			if (key is string)
			{
				PropertyDescriptor propertyDescriptor = this[(string)key];
				if (propertyDescriptor != null)
				{
					((IList)this).Remove(propertyDescriptor);
				}
			}
		}

		// Token: 0x06003572 RID: 13682 RVA: 0x000E8700 File Offset: 0x000E6900
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06003573 RID: 13683 RVA: 0x000E8708 File Offset: 0x000E6908
		int IList.Add(object value)
		{
			return this.Add((PropertyDescriptor)value);
		}

		// Token: 0x06003574 RID: 13684 RVA: 0x000E8716 File Offset: 0x000E6916
		void IList.Clear()
		{
			this.Clear();
		}

		// Token: 0x06003575 RID: 13685 RVA: 0x000E871E File Offset: 0x000E691E
		bool IList.Contains(object value)
		{
			return this.Contains((PropertyDescriptor)value);
		}

		// Token: 0x06003576 RID: 13686 RVA: 0x000E872C File Offset: 0x000E692C
		int IList.IndexOf(object value)
		{
			return this.IndexOf((PropertyDescriptor)value);
		}

		// Token: 0x06003577 RID: 13687 RVA: 0x000E873A File Offset: 0x000E693A
		void IList.Insert(int index, object value)
		{
			this.Insert(index, (PropertyDescriptor)value);
		}

		// Token: 0x17000D10 RID: 3344
		// (get) Token: 0x06003578 RID: 13688 RVA: 0x000E8749 File Offset: 0x000E6949
		bool IList.IsReadOnly
		{
			get
			{
				return this.readOnly;
			}
		}

		// Token: 0x17000D11 RID: 3345
		// (get) Token: 0x06003579 RID: 13689 RVA: 0x000E8751 File Offset: 0x000E6951
		bool IList.IsFixedSize
		{
			get
			{
				return this.readOnly;
			}
		}

		// Token: 0x0600357A RID: 13690 RVA: 0x000E8759 File Offset: 0x000E6959
		void IList.Remove(object value)
		{
			this.Remove((PropertyDescriptor)value);
		}

		// Token: 0x0600357B RID: 13691 RVA: 0x000E8767 File Offset: 0x000E6967
		void IList.RemoveAt(int index)
		{
			this.RemoveAt(index);
		}

		// Token: 0x17000D12 RID: 3346
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				if (this.readOnly)
				{
					throw new NotSupportedException();
				}
				if (index >= this.propCount)
				{
					throw new IndexOutOfRangeException();
				}
				if (value != null && !(value is PropertyDescriptor))
				{
					throw new ArgumentException("value");
				}
				this.EnsurePropsOwned();
				this.properties[index] = (PropertyDescriptor)value;
			}
		}

		// Token: 0x04002A3B RID: 10811
		public static readonly PropertyDescriptorCollection Empty = new PropertyDescriptorCollection(null, true);

		// Token: 0x04002A3C RID: 10812
		private IDictionary cachedFoundProperties;

		// Token: 0x04002A3D RID: 10813
		private bool cachedIgnoreCase;

		// Token: 0x04002A3E RID: 10814
		private PropertyDescriptor[] properties;

		// Token: 0x04002A3F RID: 10815
		private int propCount;

		// Token: 0x04002A40 RID: 10816
		private string[] namedSort;

		// Token: 0x04002A41 RID: 10817
		private IComparer comparer;

		// Token: 0x04002A42 RID: 10818
		private bool propsOwned = true;

		// Token: 0x04002A43 RID: 10819
		private bool needSort;

		// Token: 0x04002A44 RID: 10820
		private bool readOnly;

		// Token: 0x0200089B RID: 2203
		private class PropertyDescriptorEnumerator : IDictionaryEnumerator, IEnumerator
		{
			// Token: 0x060045A5 RID: 17829 RVA: 0x001232B5 File Offset: 0x001214B5
			public PropertyDescriptorEnumerator(PropertyDescriptorCollection owner)
			{
				this.owner = owner;
			}

			// Token: 0x17000FC0 RID: 4032
			// (get) Token: 0x060045A6 RID: 17830 RVA: 0x001232CB File Offset: 0x001214CB
			public object Current
			{
				get
				{
					return this.Entry;
				}
			}

			// Token: 0x17000FC1 RID: 4033
			// (get) Token: 0x060045A7 RID: 17831 RVA: 0x001232D8 File Offset: 0x001214D8
			public DictionaryEntry Entry
			{
				get
				{
					PropertyDescriptor propertyDescriptor = this.owner[this.index];
					return new DictionaryEntry(propertyDescriptor.Name, propertyDescriptor);
				}
			}

			// Token: 0x17000FC2 RID: 4034
			// (get) Token: 0x060045A8 RID: 17832 RVA: 0x00123303 File Offset: 0x00121503
			public object Key
			{
				get
				{
					return this.owner[this.index].Name;
				}
			}

			// Token: 0x17000FC3 RID: 4035
			// (get) Token: 0x060045A9 RID: 17833 RVA: 0x0012331B File Offset: 0x0012151B
			public object Value
			{
				get
				{
					return this.owner[this.index].Name;
				}
			}

			// Token: 0x060045AA RID: 17834 RVA: 0x00123333 File Offset: 0x00121533
			public bool MoveNext()
			{
				if (this.index < this.owner.Count - 1)
				{
					this.index++;
					return true;
				}
				return false;
			}

			// Token: 0x060045AB RID: 17835 RVA: 0x0012335B File Offset: 0x0012155B
			public void Reset()
			{
				this.index = -1;
			}

			// Token: 0x040037DB RID: 14299
			private PropertyDescriptorCollection owner;

			// Token: 0x040037DC RID: 14300
			private int index = -1;
		}
	}
}
