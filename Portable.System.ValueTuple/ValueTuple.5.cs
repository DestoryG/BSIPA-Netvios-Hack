using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000007 RID: 7
	[Serializable]
	[StructLayout(LayoutKind.Auto)]
	public struct ValueTuple<T1, T2, T3, T4> : IEquatable<global::System.ValueTuple<T1, T2, T3, T4>>, IStructuralEquatable, IStructuralComparable, IComparable, IComparable<global::System.ValueTuple<T1, T2, T3, T4>>, IValueTupleInternal, global::System.Runtime.CompilerServices.ITuple
	{
		// Token: 0x0600004E RID: 78 RVA: 0x00002A05 File Offset: 0x00000C05
		public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4)
		{
			this.Item1 = item1;
			this.Item2 = item2;
			this.Item3 = item3;
			this.Item4 = item4;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002A24 File Offset: 0x00000C24
		public override bool Equals(object obj)
		{
			return obj is global::System.ValueTuple<T1, T2, T3, T4> && this.Equals((global::System.ValueTuple<T1, T2, T3, T4>)obj);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002A3C File Offset: 0x00000C3C
		public bool Equals(global::System.ValueTuple<T1, T2, T3, T4> other)
		{
			return EqualityComparer<T1>.Default.Equals(this.Item1, other.Item1) && EqualityComparer<T2>.Default.Equals(this.Item2, other.Item2) && EqualityComparer<T3>.Default.Equals(this.Item3, other.Item3) && EqualityComparer<T4>.Default.Equals(this.Item4, other.Item4);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00002AA9 File Offset: 0x00000CA9
		int IComparable.CompareTo(object other)
		{
			if (other == null)
			{
				return 1;
			}
			if (!(other is global::System.ValueTuple<T1, T2, T3, T4>))
			{
				throw new ArgumentException();
			}
			return this.CompareTo((global::System.ValueTuple<T1, T2, T3, T4>)other);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00002ACC File Offset: 0x00000CCC
		public int CompareTo(global::System.ValueTuple<T1, T2, T3, T4> other)
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
			return Comparer<T4>.Default.Compare(this.Item4, other.Item4);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00002B44 File Offset: 0x00000D44
		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (!(other is global::System.ValueTuple<T1, T2, T3, T4>))
			{
				return false;
			}
			global::System.ValueTuple<T1, T2, T3, T4> valueTuple = (global::System.ValueTuple<T1, T2, T3, T4>)other;
			return comparer.Equals(this.Item1, valueTuple.Item1) && comparer.Equals(this.Item2, valueTuple.Item2) && comparer.Equals(this.Item3, valueTuple.Item3) && comparer.Equals(this.Item4, valueTuple.Item4);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00002BDC File Offset: 0x00000DDC
		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
			{
				return 1;
			}
			if (!(other is global::System.ValueTuple<T1, T2, T3, T4>))
			{
				throw new ArgumentException();
			}
			global::System.ValueTuple<T1, T2, T3, T4> valueTuple = (global::System.ValueTuple<T1, T2, T3, T4>)other;
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
			return comparer.Compare(this.Item4, valueTuple.Item4);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00002C85 File Offset: 0x00000E85
		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return this.GetHashCodeCore(comparer);
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00002C90 File Offset: 0x00000E90
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
			return global::System.ValueTuple.CombineHashCodes(num, num2, num3, num4);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00002D8C File Offset: 0x00000F8C
		private int GetHashCodeCore(IEqualityComparer comparer)
		{
			return global::System.ValueTuple.CombineHashCodes(comparer.GetHashCode(this.Item1), comparer.GetHashCode(this.Item2), comparer.GetHashCode(this.Item3), comparer.GetHashCode(this.Item4));
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00002DE2 File Offset: 0x00000FE2
		int IValueTupleInternal.GetHashCode(IEqualityComparer comparer)
		{
			return this.GetHashCodeCore(comparer);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00002DEC File Offset: 0x00000FEC
		public override string ToString()
		{
			return string.Format("({0}, {1}, {2}, {3})", new object[] { this.Item1, this.Item2, this.Item3, this.Item4 });
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002E44 File Offset: 0x00001044
		string IValueTupleInternal.ToStringEnd()
		{
			return string.Format("{0}, {1}, {2}, {3})", new object[] { this.Item1, this.Item2, this.Item3, this.Item4 });
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600005B RID: 91 RVA: 0x00002E99 File Offset: 0x00001099
		int global::System.Runtime.CompilerServices.ITuple.Length
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x1700000A RID: 10
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
				default:
					throw new IndexOutOfRangeException();
				}
			}
		}

		// Token: 0x04000008 RID: 8
		public readonly T1 Item1;

		// Token: 0x04000009 RID: 9
		public readonly T2 Item2;

		// Token: 0x0400000A RID: 10
		public readonly T3 Item3;

		// Token: 0x0400000B RID: 11
		public readonly T4 Item4;
	}
}
