using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000009 RID: 9
	[Serializable]
	[StructLayout(LayoutKind.Auto)]
	public struct ValueTuple<T1, T2, T3, T4, T5, T6> : IEquatable<global::System.ValueTuple<T1, T2, T3, T4, T5, T6>>, IStructuralEquatable, IStructuralComparable, IComparable, IComparable<global::System.ValueTuple<T1, T2, T3, T4, T5, T6>>, IValueTupleInternal, global::System.Runtime.CompilerServices.ITuple
	{
		// Token: 0x0600006C RID: 108 RVA: 0x000034DA File Offset: 0x000016DA
		public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
		{
			this.Item1 = item1;
			this.Item2 = item2;
			this.Item3 = item3;
			this.Item4 = item4;
			this.Item5 = item5;
			this.Item6 = item6;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00003509 File Offset: 0x00001709
		public override bool Equals(object obj)
		{
			return obj is global::System.ValueTuple<T1, T2, T3, T4, T5, T6> && this.Equals((global::System.ValueTuple<T1, T2, T3, T4, T5, T6>)obj);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00003524 File Offset: 0x00001724
		public bool Equals(global::System.ValueTuple<T1, T2, T3, T4, T5, T6> other)
		{
			return EqualityComparer<T1>.Default.Equals(this.Item1, other.Item1) && EqualityComparer<T2>.Default.Equals(this.Item2, other.Item2) && EqualityComparer<T3>.Default.Equals(this.Item3, other.Item3) && EqualityComparer<T4>.Default.Equals(this.Item4, other.Item4) && EqualityComparer<T5>.Default.Equals(this.Item5, other.Item5) && EqualityComparer<T6>.Default.Equals(this.Item6, other.Item6);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000035C1 File Offset: 0x000017C1
		int IComparable.CompareTo(object other)
		{
			if (other == null)
			{
				return 1;
			}
			if (!(other is global::System.ValueTuple<T1, T2, T3, T4, T5, T6>))
			{
				throw new ArgumentException();
			}
			return this.CompareTo((global::System.ValueTuple<T1, T2, T3, T4, T5, T6>)other);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x000035E4 File Offset: 0x000017E4
		public int CompareTo(global::System.ValueTuple<T1, T2, T3, T4, T5, T6> other)
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
			num = Comparer<T5>.Default.Compare(this.Item5, other.Item5);
			if (num != 0)
			{
				return num;
			}
			return Comparer<T6>.Default.Compare(this.Item6, other.Item6);
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00003694 File Offset: 0x00001894
		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (!(other is global::System.ValueTuple<T1, T2, T3, T4, T5, T6>))
			{
				return false;
			}
			global::System.ValueTuple<T1, T2, T3, T4, T5, T6> valueTuple = (global::System.ValueTuple<T1, T2, T3, T4, T5, T6>)other;
			return comparer.Equals(this.Item1, valueTuple.Item1) && comparer.Equals(this.Item2, valueTuple.Item2) && comparer.Equals(this.Item3, valueTuple.Item3) && comparer.Equals(this.Item4, valueTuple.Item4) && comparer.Equals(this.Item5, valueTuple.Item5) && comparer.Equals(this.Item6, valueTuple.Item6);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x0000376C File Offset: 0x0000196C
		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
			{
				return 1;
			}
			if (!(other is global::System.ValueTuple<T1, T2, T3, T4, T5, T6>))
			{
				throw new ArgumentException();
			}
			global::System.ValueTuple<T1, T2, T3, T4, T5, T6> valueTuple = (global::System.ValueTuple<T1, T2, T3, T4, T5, T6>)other;
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
			num = comparer.Compare(this.Item5, valueTuple.Item5);
			if (num != 0)
			{
				return num;
			}
			return comparer.Compare(this.Item6, valueTuple.Item6);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00003859 File Offset: 0x00001A59
		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return this.GetHashCodeCore(comparer);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00003864 File Offset: 0x00001A64
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
			T6 item6 = this.Item6;
			ref T6 ptr6 = ref item6;
			T6 t6 = default(T6);
			int num6;
			if (t6 == null)
			{
				t6 = item6;
				ptr6 = ref t6;
				if (t6 == null)
				{
					num6 = 0;
					goto IL_0160;
				}
			}
			num6 = ptr6.GetHashCode();
			IL_0160:
			return global::System.ValueTuple.CombineHashCodes(num, num2, num3, num4, num5, num6);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x000039D8 File Offset: 0x00001BD8
		private int GetHashCodeCore(IEqualityComparer comparer)
		{
			return global::System.ValueTuple.CombineHashCodes(comparer.GetHashCode(this.Item1), comparer.GetHashCode(this.Item2), comparer.GetHashCode(this.Item3), comparer.GetHashCode(this.Item4), comparer.GetHashCode(this.Item5), comparer.GetHashCode(this.Item6));
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00003A50 File Offset: 0x00001C50
		int IValueTupleInternal.GetHashCode(IEqualityComparer comparer)
		{
			return this.GetHashCodeCore(comparer);
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003A5C File Offset: 0x00001C5C
		public override string ToString()
		{
			return string.Format("({0}, {1}, {2}, {3}, {4}, {5})", new object[] { this.Item1, this.Item2, this.Item3, this.Item4, this.Item5, this.Item6 });
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003AD0 File Offset: 0x00001CD0
		string IValueTupleInternal.ToStringEnd()
		{
			return string.Format("{0}, {1}, {2}, {3}, {4}, {5})", new object[] { this.Item1, this.Item2, this.Item3, this.Item4, this.Item5, this.Item6 });
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00003B41 File Offset: 0x00001D41
		int global::System.Runtime.CompilerServices.ITuple.Length
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x1700000E RID: 14
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
				case 5:
					return this.Item6;
				default:
					throw new IndexOutOfRangeException();
				}
			}
		}

		// Token: 0x04000011 RID: 17
		public readonly T1 Item1;

		// Token: 0x04000012 RID: 18
		public readonly T2 Item2;

		// Token: 0x04000013 RID: 19
		public readonly T3 Item3;

		// Token: 0x04000014 RID: 20
		public readonly T4 Item4;

		// Token: 0x04000015 RID: 21
		public readonly T5 Item5;

		// Token: 0x04000016 RID: 22
		public readonly T6 Item6;
	}
}
