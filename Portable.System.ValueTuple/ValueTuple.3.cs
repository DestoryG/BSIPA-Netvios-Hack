using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000005 RID: 5
	[Serializable]
	[StructLayout(LayoutKind.Auto)]
	public struct ValueTuple<T1, T2> : IEquatable<global::System.ValueTuple<T1, T2>>, IStructuralEquatable, IStructuralComparable, IComparable, IComparable<global::System.ValueTuple<T1, T2>>, IValueTupleInternal, global::System.Runtime.CompilerServices.ITuple
	{
		// Token: 0x06000030 RID: 48 RVA: 0x000023B1 File Offset: 0x000005B1
		public ValueTuple(T1 item1, T2 item2)
		{
			this.Item1 = item1;
			this.Item2 = item2;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x000023C1 File Offset: 0x000005C1
		public override bool Equals(object obj)
		{
			return obj is global::System.ValueTuple<T1, T2> && this.Equals((global::System.ValueTuple<T1, T2>)obj);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000023D9 File Offset: 0x000005D9
		public bool Equals(global::System.ValueTuple<T1, T2> other)
		{
			return EqualityComparer<T1>.Default.Equals(this.Item1, other.Item1) && EqualityComparer<T2>.Default.Equals(this.Item2, other.Item2);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x0000240B File Offset: 0x0000060B
		int IComparable.CompareTo(object other)
		{
			if (other == null)
			{
				return 1;
			}
			if (!(other is global::System.ValueTuple<T1, T2>))
			{
				throw new ArgumentException();
			}
			return this.CompareTo((global::System.ValueTuple<T1, T2>)other);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x0000242C File Offset: 0x0000062C
		public int CompareTo(global::System.ValueTuple<T1, T2> other)
		{
			int num = Comparer<T1>.Default.Compare(this.Item1, other.Item1);
			if (num != 0)
			{
				return num;
			}
			return Comparer<T2>.Default.Compare(this.Item2, other.Item2);
		}

		// Token: 0x06000035 RID: 53 RVA: 0x0000246C File Offset: 0x0000066C
		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (!(other is global::System.ValueTuple<T1, T2>))
			{
				return false;
			}
			global::System.ValueTuple<T1, T2> valueTuple = (global::System.ValueTuple<T1, T2>)other;
			return comparer.Equals(this.Item1, valueTuple.Item1) && comparer.Equals(this.Item2, valueTuple.Item2);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000024C8 File Offset: 0x000006C8
		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
			{
				return 1;
			}
			if (!(other is global::System.ValueTuple<T1, T2>))
			{
				throw new ArgumentException();
			}
			global::System.ValueTuple<T1, T2> valueTuple = (global::System.ValueTuple<T1, T2>)other;
			int num = comparer.Compare(this.Item1, valueTuple.Item1);
			if (num == 0)
			{
				return comparer.Compare(this.Item2, valueTuple.Item2);
			}
			return num;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x0000252D File Offset: 0x0000072D
		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return this.GetHashCodeCore(comparer);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002538 File Offset: 0x00000738
		public override int GetHashCode()
		{
			T1 item = this.Item1;
			ref T1 ptr = ref item;
			T1 t = default(T1);
			int num;
			if (t == null)
			{
				t = item;
				ptr = ref t;
				if (t == null)
				{
					num = 0;
					goto IL_0038;
				}
			}
			num = ptr.GetHashCode();
			IL_0038:
			T2 item2 = this.Item2;
			ref T2 ptr2 = ref item2;
			T2 t2 = default(T2);
			int num2;
			if (t2 == null)
			{
				t2 = item2;
				ptr2 = ref t2;
				if (t2 == null)
				{
					num2 = 0;
					goto IL_0070;
				}
			}
			num2 = ptr2.GetHashCode();
			IL_0070:
			return global::System.ValueTuple.CombineHashCodes(num, num2);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000025BA File Offset: 0x000007BA
		private int GetHashCodeCore(IEqualityComparer comparer)
		{
			return global::System.ValueTuple.CombineHashCodes(comparer.GetHashCode(this.Item1), comparer.GetHashCode(this.Item2));
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000025E3 File Offset: 0x000007E3
		int IValueTupleInternal.GetHashCode(IEqualityComparer comparer)
		{
			return this.GetHashCodeCore(comparer);
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000025EC File Offset: 0x000007EC
		public override string ToString()
		{
			return string.Format("({0}, {1})", this.Item1, this.Item2);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x0000260E File Offset: 0x0000080E
		string IValueTupleInternal.ToStringEnd()
		{
			return string.Format("{0}, {1})", this.Item1, this.Item2);
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00002630 File Offset: 0x00000830
		int global::System.Runtime.CompilerServices.ITuple.Length
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000006 RID: 6
		object global::System.Runtime.CompilerServices.ITuple.this[int index]
		{
			get
			{
				if (index == 0)
				{
					return this.Item1;
				}
				if (index != 1)
				{
					throw new IndexOutOfRangeException();
				}
				return this.Item2;
			}
		}

		// Token: 0x04000003 RID: 3
		public readonly T1 Item1;

		// Token: 0x04000004 RID: 4
		public readonly T2 Item2;
	}
}
