using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Google.Protobuf.Collections
{
	// Token: 0x02000088 RID: 136
	public sealed class RepeatedField<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IList, ICollection, IDeepCloneable<RepeatedField<T>>, IEquatable<RepeatedField<T>>, IReadOnlyList<T>, IReadOnlyCollection<T>
	{
		// Token: 0x0600088B RID: 2187 RVA: 0x0001DF28 File Offset: 0x0001C128
		public RepeatedField<T> Clone()
		{
			RepeatedField<T> repeatedField = new RepeatedField<T>();
			if (this.array != RepeatedField<T>.EmptyArray)
			{
				repeatedField.array = (T[])this.array.Clone();
				IDeepCloneable<T>[] array = repeatedField.array as IDeepCloneable<T>[];
				if (array != null)
				{
					for (int i = 0; i < this.count; i++)
					{
						repeatedField.array[i] = array[i].Clone();
					}
				}
			}
			repeatedField.count = this.count;
			return repeatedField;
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x0001DFA0 File Offset: 0x0001C1A0
		public void AddEntriesFrom(CodedInputStream input, FieldCodec<T> codec)
		{
			uint lastTag = input.LastTag;
			Func<CodedInputStream, T> valueReader = codec.ValueReader;
			if (FieldCodec<T>.IsPackedRepeatedField(lastTag))
			{
				int num = input.ReadLength();
				if (num > 0)
				{
					int num2 = input.PushLimit(num);
					while (!input.ReachedLimit)
					{
						this.Add(valueReader(input));
					}
					input.PopLimit(num2);
					return;
				}
			}
			else
			{
				do
				{
					this.Add(valueReader(input));
				}
				while (input.MaybeConsumeTag(lastTag));
			}
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x0001E00C File Offset: 0x0001C20C
		public int CalculateSize(FieldCodec<T> codec)
		{
			if (this.count == 0)
			{
				return 0;
			}
			uint tag = codec.Tag;
			if (codec.PackedRepeatedField)
			{
				int num = this.CalculatePackedDataSize(codec);
				return CodedOutputStream.ComputeRawVarint32Size(tag) + CodedOutputStream.ComputeLengthSize(num) + num;
			}
			Func<T, int> valueSizeCalculator = codec.ValueSizeCalculator;
			int num2 = this.count * CodedOutputStream.ComputeRawVarint32Size(tag);
			if (codec.EndTag != 0U)
			{
				num2 += this.count * CodedOutputStream.ComputeRawVarint32Size(codec.EndTag);
			}
			for (int i = 0; i < this.count; i++)
			{
				num2 += valueSizeCalculator(this.array[i]);
			}
			return num2;
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x0001E0A8 File Offset: 0x0001C2A8
		private int CalculatePackedDataSize(FieldCodec<T> codec)
		{
			int fixedSize = codec.FixedSize;
			if (fixedSize == 0)
			{
				Func<T, int> valueSizeCalculator = codec.ValueSizeCalculator;
				int num = 0;
				for (int i = 0; i < this.count; i++)
				{
					num += valueSizeCalculator(this.array[i]);
				}
				return num;
			}
			return fixedSize * this.Count;
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x0001E0F8 File Offset: 0x0001C2F8
		public void WriteTo(CodedOutputStream output, FieldCodec<T> codec)
		{
			if (this.count == 0)
			{
				return;
			}
			Action<CodedOutputStream, T> valueWriter = codec.ValueWriter;
			uint tag = codec.Tag;
			if (codec.PackedRepeatedField)
			{
				uint num = (uint)this.CalculatePackedDataSize(codec);
				output.WriteTag(tag);
				output.WriteRawVarint32(num);
				for (int i = 0; i < this.count; i++)
				{
					valueWriter(output, this.array[i]);
				}
				return;
			}
			for (int j = 0; j < this.count; j++)
			{
				output.WriteTag(tag);
				valueWriter(output, this.array[j]);
				if (codec.EndTag != 0U)
				{
					output.WriteTag(codec.EndTag);
				}
			}
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000890 RID: 2192 RVA: 0x0001E1A3 File Offset: 0x0001C3A3
		// (set) Token: 0x06000891 RID: 2193 RVA: 0x0001E1B0 File Offset: 0x0001C3B0
		public int Capacity
		{
			get
			{
				return this.array.Length;
			}
			set
			{
				if (value < this.count)
				{
					throw new ArgumentOutOfRangeException("Capacity", value, string.Format("Cannot set Capacity to a value smaller than the current item count, {0}", this.count));
				}
				if (value >= 0 && value != this.array.Length)
				{
					this.SetSize(value);
				}
			}
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x0001E204 File Offset: 0x0001C404
		private void EnsureSize(int size)
		{
			if (this.array.Length < size)
			{
				size = Math.Max(size, 8);
				int num = Math.Max(this.array.Length * 2, size);
				this.SetSize(num);
			}
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x0001E240 File Offset: 0x0001C440
		private void SetSize(int size)
		{
			if (size != this.array.Length)
			{
				T[] array = new T[size];
				Array.Copy(this.array, 0, array, 0, this.count);
				this.array = array;
			}
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x0001E27C File Offset: 0x0001C47C
		public void Add(T item)
		{
			ProtoPreconditions.CheckNotNullUnconstrained<T>(item, "item");
			this.EnsureSize(this.count + 1);
			T[] array = this.array;
			int num = this.count;
			this.count = num + 1;
			array[num] = item;
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x0001E2C0 File Offset: 0x0001C4C0
		public void Clear()
		{
			this.array = RepeatedField<T>.EmptyArray;
			this.count = 0;
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x0001E2D4 File Offset: 0x0001C4D4
		public bool Contains(T item)
		{
			return this.IndexOf(item) != -1;
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x0001E2E3 File Offset: 0x0001C4E3
		public void CopyTo(T[] array, int arrayIndex)
		{
			Array.Copy(this.array, 0, array, arrayIndex, this.count);
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x0001E2FC File Offset: 0x0001C4FC
		public bool Remove(T item)
		{
			int num = this.IndexOf(item);
			if (num == -1)
			{
				return false;
			}
			Array.Copy(this.array, num + 1, this.array, num, this.count - num - 1);
			this.count--;
			this.array[this.count] = default(T);
			return true;
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000899 RID: 2201 RVA: 0x0001E35F File Offset: 0x0001C55F
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x0600089A RID: 2202 RVA: 0x0001E367 File Offset: 0x0001C567
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x0001E36C File Offset: 0x0001C56C
		public void AddRange(IEnumerable<T> values)
		{
			ProtoPreconditions.CheckNotNull<IEnumerable<T>>(values, "values");
			RepeatedField<T> repeatedField = values as RepeatedField<T>;
			if (repeatedField != null)
			{
				this.EnsureSize(this.count + repeatedField.count);
				Array.Copy(repeatedField.array, 0, this.array, this.count, repeatedField.count);
				this.count += repeatedField.count;
				return;
			}
			ICollection collection = values as ICollection;
			if (collection != null)
			{
				int num = collection.Count;
				if (default(T) == null)
				{
					using (IEnumerator enumerator = collection.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current == null)
							{
								throw new ArgumentException("Sequence contained null element", "values");
							}
						}
					}
				}
				this.EnsureSize(this.count + num);
				collection.CopyTo(this.array, this.count);
				this.count += num;
				return;
			}
			foreach (T t in values)
			{
				this.Add(t);
			}
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x0001E4B8 File Offset: 0x0001C6B8
		public void Add(IEnumerable<T> values)
		{
			this.AddRange(values);
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x0001E4C1 File Offset: 0x0001C6C1
		public IEnumerator<T> GetEnumerator()
		{
			int num;
			for (int i = 0; i < this.count; i = num + 1)
			{
				yield return this.array[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x0001E4D0 File Offset: 0x0001C6D0
		public override bool Equals(object obj)
		{
			return this.Equals(obj as RepeatedField<T>);
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x0001E4DE File Offset: 0x0001C6DE
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x0001E4E8 File Offset: 0x0001C6E8
		public override int GetHashCode()
		{
			int num = 0;
			for (int i = 0; i < this.count; i++)
			{
				num = num * 31 + this.array[i].GetHashCode();
			}
			return num;
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x0001E528 File Offset: 0x0001C728
		public bool Equals(RepeatedField<T> other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (other.Count != this.Count)
			{
				return false;
			}
			EqualityComparer<T> equalityComparer = RepeatedField<T>.EqualityComparer;
			for (int i = 0; i < this.count; i++)
			{
				if (!equalityComparer.Equals(this.array[i], other.array[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x0001E58C File Offset: 0x0001C78C
		public int IndexOf(T item)
		{
			ProtoPreconditions.CheckNotNullUnconstrained<T>(item, "item");
			EqualityComparer<T> equalityComparer = RepeatedField<T>.EqualityComparer;
			for (int i = 0; i < this.count; i++)
			{
				if (equalityComparer.Equals(this.array[i], item))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x0001E5D4 File Offset: 0x0001C7D4
		public void Insert(int index, T item)
		{
			ProtoPreconditions.CheckNotNullUnconstrained<T>(item, "item");
			if (index < 0 || index > this.count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			this.EnsureSize(this.count + 1);
			Array.Copy(this.array, index, this.array, index + 1, this.count - index);
			this.array[index] = item;
			this.count++;
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x0001E64C File Offset: 0x0001C84C
		public void RemoveAt(int index)
		{
			if (index < 0 || index >= this.count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			Array.Copy(this.array, index + 1, this.array, index, this.count - index - 1);
			this.count--;
			this.array[this.count] = default(T);
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x0001E6B8 File Offset: 0x0001C8B8
		public override string ToString()
		{
			StringWriter stringWriter = new StringWriter();
			JsonFormatter.Default.WriteList(stringWriter, this);
			return stringWriter.ToString();
		}

		// Token: 0x1700025B RID: 603
		public T this[int index]
		{
			get
			{
				if (index < 0 || index >= this.count)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				return this.array[index];
			}
			set
			{
				if (index < 0 || index >= this.count)
				{
					throw new ArgumentOutOfRangeException("index");
				}
				ProtoPreconditions.CheckNotNullUnconstrained<T>(value, "value");
				this.array[index] = value;
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x060008A8 RID: 2216 RVA: 0x0001E736 File Offset: 0x0001C936
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x0001E739 File Offset: 0x0001C939
		void ICollection.CopyTo(Array array, int index)
		{
			Array.Copy(this.array, 0, array, index, this.count);
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x060008AA RID: 2218 RVA: 0x0001E74F File Offset: 0x0001C94F
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x060008AB RID: 2219 RVA: 0x0001E752 File Offset: 0x0001C952
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700025F RID: 607
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				this[index] = (T)((object)value);
			}
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x0001E772 File Offset: 0x0001C972
		int IList.Add(object value)
		{
			this.Add((T)((object)value));
			return this.count - 1;
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x0001E788 File Offset: 0x0001C988
		bool IList.Contains(object value)
		{
			return value is T && this.Contains((T)((object)value));
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x0001E7A0 File Offset: 0x0001C9A0
		int IList.IndexOf(object value)
		{
			if (!(value is T))
			{
				return -1;
			}
			return this.IndexOf((T)((object)value));
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x0001E7B8 File Offset: 0x0001C9B8
		void IList.Insert(int index, object value)
		{
			this.Insert(index, (T)((object)value));
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x0001E7C7 File Offset: 0x0001C9C7
		void IList.Remove(object value)
		{
			if (!(value is T))
			{
				return;
			}
			this.Remove((T)((object)value));
		}

		// Token: 0x04000352 RID: 850
		private static readonly EqualityComparer<T> EqualityComparer = ProtobufEqualityComparers.GetEqualityComparer<T>();

		// Token: 0x04000353 RID: 851
		private static readonly T[] EmptyArray = new T[0];

		// Token: 0x04000354 RID: 852
		private const int MinArraySize = 8;

		// Token: 0x04000355 RID: 853
		private T[] array = RepeatedField<T>.EmptyArray;

		// Token: 0x04000356 RID: 854
		private int count;
	}
}
