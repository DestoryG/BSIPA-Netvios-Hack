using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IPA.Utilities;

namespace IPA.Config.Data
{
	/// <summary>
	/// A list of <see cref="T:IPA.Config.Data.Value" />s for serialization by an <see cref="T:IPA.Config.IConfigProvider" />.
	/// Use <see cref="M:IPA.Config.Data.Value.List" /> or <see cref="M:IPA.Config.Data.Value.From(System.Collections.Generic.IEnumerable{IPA.Config.Data.Value})" /> to create.
	/// </summary>
	// Token: 0x02000094 RID: 148
	public sealed class List : Value, IList<Value>, ICollection<Value>, IEnumerable<Value>, IEnumerable
	{
		// Token: 0x060003A2 RID: 930 RVA: 0x0001324C File Offset: 0x0001144C
		internal List()
		{
		}

		/// <summary>
		/// Gets the value at the given index in this <see cref="T:IPA.Config.Data.List" />.
		/// </summary>
		/// <param name="index">the index to retrieve the <see cref="T:IPA.Config.Data.Value" /> at</param>
		/// <returns>the <see cref="T:IPA.Config.Data.Value" /> at <paramref name="index" /></returns>
		/// <seealso cref="P:System.Collections.Generic.IList`1.Item(System.Int32)" />
		// Token: 0x170000A1 RID: 161
		public Value this[int index]
		{
			get
			{
				return this.values[index];
			}
			set
			{
				this.values[index] = value;
			}
		}

		/// <summary>
		/// Gets the number of elements in the <see cref="T:IPA.Config.Data.List" />.
		/// </summary>
		/// <seealso cref="P:System.Collections.Generic.ICollection`1.Count" />
		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x0001327C File Offset: 0x0001147C
		public int Count
		{
			get
			{
				return this.values.Count;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060003A6 RID: 934 RVA: 0x00013289 File Offset: 0x00011489
		bool ICollection<Value>.IsReadOnly
		{
			get
			{
				return ((ICollection<Value>)this.values).IsReadOnly;
			}
		}

		/// <summary>
		/// Adds a <see cref="T:IPA.Config.Data.Value" /> to the end of this <see cref="T:IPA.Config.Data.List" />.
		/// </summary>
		/// <param name="item">the <see cref="T:IPA.Config.Data.Value" /> to add</param>
		/// <seealso cref="M:System.Collections.Generic.ICollection`1.Add(`0)" />
		// Token: 0x060003A7 RID: 935 RVA: 0x00013296 File Offset: 0x00011496
		public void Add(Value item)
		{
			this.values.Add(item);
		}

		/// <summary>
		/// Adds a range of <see cref="T:IPA.Config.Data.Value" />s to the end of this <see cref="T:IPA.Config.Data.List" />.
		/// </summary>
		/// <param name="vals">the range of <see cref="T:IPA.Config.Data.Value" />s to add</param>
		// Token: 0x060003A8 RID: 936 RVA: 0x000132A4 File Offset: 0x000114A4
		public void AddRange(IEnumerable<Value> vals)
		{
			foreach (Value val in vals)
			{
				this.Add(val);
			}
		}

		/// <summary>
		/// Clears the <see cref="T:IPA.Config.Data.List" />.
		/// </summary>
		/// <seealso cref="M:System.Collections.Generic.ICollection`1.Clear" />
		// Token: 0x060003A9 RID: 937 RVA: 0x000132EC File Offset: 0x000114EC
		public void Clear()
		{
			this.values.Clear();
		}

		/// <summary>
		/// Checks if the <see cref="T:IPA.Config.Data.List" /> contains a certian item.
		/// </summary>
		/// <param name="item">the <see cref="T:IPA.Config.Data.Value" /> to check for</param>
		/// <returns><see langword="true" /> if the item was founc, otherwise <see langword="false" /></returns>
		/// <seealso cref="M:System.Collections.Generic.ICollection`1.Contains(`0)" />
		// Token: 0x060003AA RID: 938 RVA: 0x000132F9 File Offset: 0x000114F9
		public bool Contains(Value item)
		{
			return this.values.Contains(item);
		}

		/// <summary>
		/// Copies the <see cref="T:IPA.Config.Data.Value" />s in the <see cref="T:IPA.Config.Data.List" /> to the <see cref="T:System.Array" /> in <paramref name="array" />.
		/// </summary>
		/// <param name="array">the <see cref="T:System.Array" /> to copy to</param>
		/// <param name="arrayIndex">the starting index to copy to</param>
		/// <seealso cref="M:System.Collections.Generic.ICollection`1.CopyTo(`0[],System.Int32)" />
		// Token: 0x060003AB RID: 939 RVA: 0x00013307 File Offset: 0x00011507
		public void CopyTo(Value[] array, int arrayIndex)
		{
			this.values.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Gets an enumerator to enumerate the <see cref="T:IPA.Config.Data.List" />.
		/// </summary>
		/// <returns>an <see cref="T:System.Collections.Generic.IEnumerator`1" /> for this <see cref="T:IPA.Config.Data.List" /></returns>
		/// <seealso cref="M:System.Collections.Generic.IEnumerable`1.GetEnumerator" />
		// Token: 0x060003AC RID: 940 RVA: 0x00013316 File Offset: 0x00011516
		public IEnumerator<Value> GetEnumerator()
		{
			return ((IEnumerable<Value>)this.values).GetEnumerator();
		}

		/// <summary>
		/// Gets the index that a given <see cref="T:IPA.Config.Data.Value" /> is in the <see cref="T:IPA.Config.Data.List" />.
		/// </summary>
		/// <param name="item">the <see cref="T:IPA.Config.Data.Value" /> to search for</param>
		/// <returns>the index that the <paramref name="item" /> was at, or -1.</returns>
		/// <seealso cref="M:System.Collections.Generic.IList`1.IndexOf(`0)" />
		// Token: 0x060003AD RID: 941 RVA: 0x00013323 File Offset: 0x00011523
		public int IndexOf(Value item)
		{
			return this.values.IndexOf(item);
		}

		/// <summary>
		/// Inserts a <see cref="T:IPA.Config.Data.Value" /> at an index.
		/// </summary>
		/// <param name="index">the index to insert at</param>
		/// <param name="item">the <see cref="T:IPA.Config.Data.Value" /> to insert</param>
		/// <seealso cref="M:System.Collections.Generic.IList`1.Insert(System.Int32,`0)" />
		// Token: 0x060003AE RID: 942 RVA: 0x00013331 File Offset: 0x00011531
		public void Insert(int index, Value item)
		{
			this.values.Insert(index, item);
		}

		/// <summary>
		/// Removes a <see cref="T:IPA.Config.Data.Value" /> from the <see cref="T:IPA.Config.Data.List" />.
		/// </summary>
		/// <param name="item">the <see cref="T:IPA.Config.Data.Value" /> to remove</param>
		/// <returns><see langword="true" /> if the item was removed, <see langword="false" /> otherwise</returns>
		/// <seealso cref="M:System.Collections.Generic.ICollection`1.Remove(`0)" />
		// Token: 0x060003AF RID: 943 RVA: 0x00013340 File Offset: 0x00011540
		public bool Remove(Value item)
		{
			return this.values.Remove(item);
		}

		/// <summary>
		/// Removes a <see cref="T:IPA.Config.Data.Value" /> at an index.
		/// </summary>
		/// <param name="index">the index to remove a <see cref="T:IPA.Config.Data.Value" /> at</param>
		/// <seealso cref="M:System.Collections.Generic.IList`1.RemoveAt(System.Int32)" />
		// Token: 0x060003B0 RID: 944 RVA: 0x0001334E File Offset: 0x0001154E
		public void RemoveAt(int index)
		{
			this.values.RemoveAt(index);
		}

		/// <summary>
		/// Converts this <see cref="T:IPA.Config.Data.Value" /> into a human-readable format.
		/// </summary>
		/// <returns>a comma-seperated list of the result of <see cref="M:IPA.Config.Data.Value.ToString" /> wrapped in square brackets</returns>
		// Token: 0x060003B1 RID: 945 RVA: 0x0001335C File Offset: 0x0001155C
		public override string ToString()
		{
			return "[" + string.Join(",", this.Select((Value v) => ((v != null) ? v.ToString() : null) ?? "null").StrJP()) + "]";
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x000133AC File Offset: 0x000115AC
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Value>)this.values).GetEnumerator();
		}

		// Token: 0x0400012D RID: 301
		private readonly List<Value> values = new List<Value>();
	}
}
