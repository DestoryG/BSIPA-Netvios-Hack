using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000006 RID: 6
	[Serializable]
	[StructLayout(LayoutKind.Auto)]
	public struct ValueTuple<T1, T2, T3> : IEquatable<global::System.ValueTuple<T1, T2, T3>>, IStructuralEquatable, IStructuralComparable, IComparable, IComparable<global::System.ValueTuple<T1, T2, T3>>, IValueTupleInternal, global::System.Runtime.CompilerServices.ITuple
	{
		// Token: 0x0600003F RID: 63 RVA: 0x0000265B File Offset: 0x0000085B
		public ValueTuple(T1 item1, T2 item2, T3 item3)
		{
			this.Item1 = item1;
			this.Item2 = item2;
			this.Item3 = item3;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002672 File Offset: 0x00000872
		public override bool Equals(object obj)
		{
			return obj is global::System.ValueTuple<T1, T2, T3> && this.Equals((global::System.ValueTuple<T1, T2, T3>)obj);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x0000268C File Offset: 0x0000088C
		public bool Equals(global::System.ValueTuple<T1, T2, T3> other)
		{
			return EqualityComparer<T1>.Default.Equals(this.Item1, other.Item1) && EqualityComparer<T2>.Default.Equals(this.Item2, other.Item2) && EqualityComparer<T3>.Default.Equals(this.Item3, other.Item3);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000026E1 File Offset: 0x000008E1
		int IComparable.CompareTo(object other)
		{
			if (other == null)
			{
				return 1;
			}
			if (!(other is global::System.ValueTuple<T1, T2, T3>))
			{
				throw new ArgumentException();
			}
			return this.CompareTo((global::System.ValueTuple<T1, T2, T3>)other);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00002704 File Offset: 0x00000904
		public int CompareTo(global::System.ValueTuple<T1, T2, T3> other)
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
			return Comparer<T3>.Default.Compare(this.Item3, other.Item3);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002760 File Offset: 0x00000960
		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (!(other is global::System.ValueTuple<T1, T2, T3>))
			{
				return false;
			}
			global::System.ValueTuple<T1, T2, T3> valueTuple = (global::System.ValueTuple<T1, T2, T3>)other;
			return comparer.Equals(this.Item1, valueTuple.Item1) && comparer.Equals(this.Item2, valueTuple.Item2) && comparer.Equals(this.Item3, valueTuple.Item3);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000027D8 File Offset: 0x000009D8
		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
			{
				return 1;
			}
			if (!(other is global::System.ValueTuple<T1, T2, T3>))
			{
				throw new ArgumentException();
			}
			global::System.ValueTuple<T1, T2, T3> valueTuple = (global::System.ValueTuple<T1, T2, T3>)other;
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
			return comparer.Compare(this.Item3, valueTuple.Item3);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x0000285F File Offset: 0x00000A5F
		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return this.GetHashCodeCore(comparer);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002868 File Offset: 0x00000A68
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
			return global::System.ValueTuple.CombineHashCodes(num, num2, num3);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002926 File Offset: 0x00000B26
		private int GetHashCodeCore(IEqualityComparer comparer)
		{
			return global::System.ValueTuple.CombineHashCodes(comparer.GetHashCode(this.Item1), comparer.GetHashCode(this.Item2), comparer.GetHashCode(this.Item3));
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002960 File Offset: 0x00000B60
		int IValueTupleInternal.GetHashCode(IEqualityComparer comparer)
		{
			return this.GetHashCodeCore(comparer);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002969 File Offset: 0x00000B69
		public override string ToString()
		{
			return string.Format("({0}, {1}, {2})", this.Item1, this.Item2, this.Item3);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002996 File Offset: 0x00000B96
		string IValueTupleInternal.ToStringEnd()
		{
			return string.Format("{0}, {1}, {2})", this.Item1, this.Item2, this.Item3);
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600004C RID: 76 RVA: 0x000029C3 File Offset: 0x00000BC3
		int global::System.Runtime.CompilerServices.ITuple.Length
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17000008 RID: 8
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
				default:
					throw new IndexOutOfRangeException();
				}
			}
		}

		// Token: 0x04000005 RID: 5
		public readonly T1 Item1;

		// Token: 0x04000006 RID: 6
		public readonly T2 Item2;

		// Token: 0x04000007 RID: 7
		public readonly T3 Item3;
	}
}
