using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System
{
	// Token: 0x02000004 RID: 4
	[Serializable]
	public struct ValueTuple<T1> : IEquatable<global::System.ValueTuple<T1>>, IStructuralEquatable, IStructuralComparable, IComparable, IComparable<global::System.ValueTuple<T1>>, IValueTupleInternal, global::System.Runtime.CompilerServices.ITuple
	{
		// Token: 0x06000022 RID: 34 RVA: 0x000021EC File Offset: 0x000003EC
		public ValueTuple(T1 item1)
		{
			this.Item1 = item1;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000021F5 File Offset: 0x000003F5
		public override bool Equals(object obj)
		{
			return obj is global::System.ValueTuple<T1> && this.Equals((global::System.ValueTuple<T1>)obj);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x0000220D File Offset: 0x0000040D
		public bool Equals(global::System.ValueTuple<T1> other)
		{
			return EqualityComparer<T1>.Default.Equals(this.Item1, other.Item1);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002228 File Offset: 0x00000428
		int IComparable.CompareTo(object other)
		{
			if (other == null)
			{
				return 1;
			}
			if (!(other is global::System.ValueTuple<T1>))
			{
				throw new ArgumentException();
			}
			global::System.ValueTuple<T1> valueTuple = (global::System.ValueTuple<T1>)other;
			return Comparer<T1>.Default.Compare(this.Item1, valueTuple.Item1);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002265 File Offset: 0x00000465
		public int CompareTo(global::System.ValueTuple<T1> other)
		{
			return Comparer<T1>.Default.Compare(this.Item1, other.Item1);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002280 File Offset: 0x00000480
		public override int GetHashCode()
		{
			T1 item = this.Item1;
			ref T1 ptr = ref item;
			T1 t = default(T1);
			if (t == null)
			{
				t = item;
				ptr = ref t;
				if (t == null)
				{
					return 0;
				}
			}
			return ptr.GetHashCode();
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000022C4 File Offset: 0x000004C4
		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (!(other is global::System.ValueTuple<T1>))
			{
				return false;
			}
			global::System.ValueTuple<T1> valueTuple = (global::System.ValueTuple<T1>)other;
			return comparer.Equals(this.Item1, valueTuple.Item1);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000022FE File Offset: 0x000004FE
		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return comparer.GetHashCode(this.Item1);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002314 File Offset: 0x00000514
		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
			{
				return 1;
			}
			if (!(other is global::System.ValueTuple<T1>))
			{
				throw new ArgumentException();
			}
			global::System.ValueTuple<T1> valueTuple = (global::System.ValueTuple<T1>)other;
			return comparer.Compare(this.Item1, valueTuple.Item1);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002357 File Offset: 0x00000557
		int IValueTupleInternal.GetHashCode(IEqualityComparer comparer)
		{
			return comparer.GetHashCode(this.Item1);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x0000236A File Offset: 0x0000056A
		public override string ToString()
		{
			return string.Format("({0})", this.Item1);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002381 File Offset: 0x00000581
		string IValueTupleInternal.ToStringEnd()
		{
			return string.Format("{0})", this.Item1);
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002398 File Offset: 0x00000598
		int global::System.Runtime.CompilerServices.ITuple.Length
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000004 RID: 4
		object global::System.Runtime.CompilerServices.ITuple.this[int index]
		{
			get
			{
				if (index != 0)
				{
					throw new IndexOutOfRangeException();
				}
				return this.Item1;
			}
		}

		// Token: 0x04000002 RID: 2
		public readonly T1 Item1;
	}
}
