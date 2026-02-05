using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000008 RID: 8
	[Serializable]
	[StructLayout(LayoutKind.Auto)]
	public struct ValueTuple<T1, T2, T3, T4, T5> : IEquatable<global::System.ValueTuple<T1, T2, T3, T4, T5>>, IStructuralEquatable, IStructuralComparable, IComparable, IComparable<global::System.ValueTuple<T1, T2, T3, T4, T5>>, IValueTupleInternal, global::System.Runtime.CompilerServices.ITuple
	{
		// Token: 0x0600005D RID: 93 RVA: 0x00002EF6 File Offset: 0x000010F6
		public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
		{
			this.Item1 = item1;
			this.Item2 = item2;
			this.Item3 = item3;
			this.Item4 = item4;
			this.Item5 = item5;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002F1D File Offset: 0x0000111D
		public override bool Equals(object obj)
		{
			return obj is global::System.ValueTuple<T1, T2, T3, T4, T5> && this.Equals((global::System.ValueTuple<T1, T2, T3, T4, T5>)obj);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00002F38 File Offset: 0x00001138
		public bool Equals(global::System.ValueTuple<T1, T2, T3, T4, T5> other)
		{
			return EqualityComparer<T1>.Default.Equals(this.Item1, other.Item1) && EqualityComparer<T2>.Default.Equals(this.Item2, other.Item2) && EqualityComparer<T3>.Default.Equals(this.Item3, other.Item3) && EqualityComparer<T4>.Default.Equals(this.Item4, other.Item4) && EqualityComparer<T5>.Default.Equals(this.Item5, other.Item5);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00002FBD File Offset: 0x000011BD
		int IComparable.CompareTo(object other)
		{
			if (other == null)
			{
				return 1;
			}
			if (!(other is global::System.ValueTuple<T1, T2, T3, T4, T5>))
			{
				throw new ArgumentException();
			}
			return this.CompareTo((global::System.ValueTuple<T1, T2, T3, T4, T5>)other);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x00002FE0 File Offset: 0x000011E0
		public int CompareTo(global::System.ValueTuple<T1, T2, T3, T4, T5> other)
		{
			int num = Comparer<T1>.Default.Compare(this.Item1, other.Item1);
			if (num != 0)
			{
				return num;
			}
			num = Comparer<T2>.Default.Compare(this.Item2, other.Item2);
			if (num != 0)
			{
				return num;
			}
			num = Comparer<T3>.Default.Compare(this.Item3, other.Item3);
			if (num != 0)
			{
				return num;
			}
			num = Comparer<T4>.Default.Compare(this.Item4, other.Item4);
			if (num != 0)
			{
				return num;
			}
			return Comparer<T5>.Default.Compare(this.Item5, other.Item5);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003074 File Offset: 0x00001274
		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (!(other is global::System.ValueTuple<T1, T2, T3, T4, T5>))
			{
				return false;
			}
			global::System.ValueTuple<T1, T2, T3, T4, T5> valueTuple = (global::System.ValueTuple<T1, T2, T3, T4, T5>)other;
			return comparer.Equals(this.Item1, valueTuple.Item1) && comparer.Equals(this.Item2, valueTuple.Item2) && comparer.Equals(this.Item3, valueTuple.Item3) && comparer.Equals(this.Item4, valueTuple.Item4) && comparer.Equals(this.Item5, valueTuple.Item5);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003128 File Offset: 0x00001328
		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
			{
				return 1;
			}
			if (!(other is global::System.ValueTuple<T1, T2, T3, T4, T5>))
			{
				throw new ArgumentException();
			}
			global::System.ValueTuple<T1, T2, T3, T4, T5> valueTuple = (global::System.ValueTuple<T1, T2, T3, T4, T5>)other;
			int num = comparer.Compare(this.Item1, valueTuple.Item1);
			if (num != 0)
			{
				return num;
			}
			num = comparer.Compare(this.Item2, valueTuple.Item2);
			if (num != 0)
			{
				return num;
			}
			num = comparer.Compare(this.Item3, valueTuple.Item3);
			if (num != 0)
			{
				return num;
			}
			num = comparer.Compare(this.Item4, valueTuple.Item4);
			if (num != 0)
			{
				return num;
			}
			return comparer.Compare(this.Item5, valueTuple.Item5);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000031F3 File Offset: 0x000013F3
		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return this.GetHashCodeCore(comparer);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x000031FC File Offset: 0x000013FC
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
			T3 item3 = this.Item3;
			ref T3 ptr3 = ref item3;
			T3 t3 = default(T3);
			int num3;
			if (t3 == null)
			{
				t3 = item3;
				ptr3 = ref t3;
				if (t3 == null)
				{
					num3 = 0;
					goto IL_00AC;
				}
			}
			num3 = ptr3.GetHashCode();
			IL_00AC:
			T4 item4 = this.Item4;
			ref T4 ptr4 = ref item4;
			T4 t4 = default(T4);
			int num4;
			if (t4 == null)
			{
				t4 = item4;
				ptr4 = ref t4;
				if (t4 == null)
				{
					num4 = 0;
					goto IL_00E8;
				}
			}
			num4 = ptr4.GetHashCode();
			IL_00E8:
			T5 item5 = this.Item5;
			ref T5 ptr5 = ref item5;
			T5 t5 = default(T5);
			int num5;
			if (t5 == null)
			{
				t5 = item5;
				ptr5 = ref t5;
				if (t5 == null)
				{
					num5 = 0;
					goto IL_0124;
				}
			}
			num5 = ptr5.GetHashCode();
			IL_0124:
			return global::System.ValueTuple.CombineHashCodes(num, num2, num3, num4, num5);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003334 File Offset: 0x00001534
		private int GetHashCodeCore(IEqualityComparer comparer)
		{
			return global::System.ValueTuple.CombineHashCodes(comparer.GetHashCode(this.Item1), comparer.GetHashCode(this.Item2), comparer.GetHashCode(this.Item3), comparer.GetHashCode(this.Item4), comparer.GetHashCode(this.Item5));
		}

		// Token: 0x06000067 RID: 103 RVA: 0x0000339B File Offset: 0x0000159B
		int IValueTupleInternal.GetHashCode(IEqualityComparer comparer)
		{
			return this.GetHashCodeCore(comparer);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x000033A4 File Offset: 0x000015A4
		public override string ToString()
		{
			return string.Format("({0}, {1}, {2}, {3}, {4})", new object[] { this.Item1, this.Item2, this.Item3, this.Item4, this.Item5 });
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003408 File Offset: 0x00001608
		string IValueTupleInternal.ToStringEnd()
		{
			return string.Format("{0}, {1}, {2}, {3}, {4})", new object[] { this.Item1, this.Item2, this.Item3, this.Item4, this.Item5 });
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600006A RID: 106 RVA: 0x0000346B File Offset: 0x0000166B
		int global::System.Runtime.CompilerServices.ITuple.Length
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x1700000C RID: 12
		object global::System.Runtime.CompilerServices.ITuple.this[int index]
		{
			get
			{
				switch (index)
				{
				case 0:
					return this.Item1;
				case 1:
					return this.Item2;
				case 2:
					return this.Item3;
				case 3:
					return this.Item4;
				case 4:
					return this.Item5;
				default:
					throw new IndexOutOfRangeException();
				}
			}
		}

		// Token: 0x0400000C RID: 12
		public readonly T1 Item1;

		// Token: 0x0400000D RID: 13
		public readonly T2 Item2;

		// Token: 0x0400000E RID: 14
		public readonly T3 Item3;

		// Token: 0x0400000F RID: 15
		public readonly T4 Item4;

		// Token: 0x04000010 RID: 16
		public readonly T5 Item5;
	}
}
