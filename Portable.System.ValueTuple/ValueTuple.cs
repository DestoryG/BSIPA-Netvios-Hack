using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace System
{
	// Token: 0x02000003 RID: 3
	[Serializable]
	public struct ValueTuple : IEquatable<global::System.ValueTuple>, IStructuralEquatable, IStructuralComparable, IComparable, IComparable<global::System.ValueTuple>, IValueTupleInternal, global::System.Runtime.CompilerServices.ITuple
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002050 File Offset: 0x00000250
		public override bool Equals(object obj)
		{
			return obj is global::System.ValueTuple;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x0000205B File Offset: 0x0000025B
		public bool Equals(global::System.ValueTuple other)
		{
			return true;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000205E File Offset: 0x0000025E
		int IComparable.CompareTo(object other)
		{
			if (other == null)
			{
				return 1;
			}
			if (other is global::System.ValueTuple)
			{
				return 0;
			}
			throw new ArgumentException();
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002074 File Offset: 0x00000274
		public int CompareTo(global::System.ValueTuple other)
		{
			return 0;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002077 File Offset: 0x00000277
		public override int GetHashCode()
		{
			return 0;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x0000207A File Offset: 0x0000027A
		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			return other is global::System.ValueTuple;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002085 File Offset: 0x00000285
		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return 0;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002088 File Offset: 0x00000288
		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
			{
				return 1;
			}
			if (other is global::System.ValueTuple)
			{
				return 0;
			}
			throw new ArgumentException();
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000209E File Offset: 0x0000029E
		int IValueTupleInternal.GetHashCode(IEqualityComparer comparer)
		{
			return 0;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000020A1 File Offset: 0x000002A1
		public override string ToString()
		{
			return "()";
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000020A8 File Offset: 0x000002A8
		string IValueTupleInternal.ToStringEnd()
		{
			return ")";
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000020AF File Offset: 0x000002AF
		int global::System.Runtime.CompilerServices.ITuple.Length
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000002 RID: 2
		object global::System.Runtime.CompilerServices.ITuple.this[int index]
		{
			get
			{
				throw new IndexOutOfRangeException();
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000020BC File Offset: 0x000002BC
		public static global::System.ValueTuple Create()
		{
			return default(global::System.ValueTuple);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000020D2 File Offset: 0x000002D2
		public static global::System.ValueTuple<T1> Create<T1>(T1 item1)
		{
			return new global::System.ValueTuple<T1>(item1);
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000020DA File Offset: 0x000002DA
		public static global::System.ValueTuple<T1, T2> Create<T1, T2>(T1 item1, T2 item2)
		{
			return new global::System.ValueTuple<T1, T2>(item1, item2);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000020E3 File Offset: 0x000002E3
		public static global::System.ValueTuple<T1, T2, T3> Create<T1, T2, T3>(T1 item1, T2 item2, T3 item3)
		{
			return new global::System.ValueTuple<T1, T2, T3>(item1, item2, item3);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000020ED File Offset: 0x000002ED
		public static global::System.ValueTuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 item1, T2 item2, T3 item3, T4 item4)
		{
			return new global::System.ValueTuple<T1, T2, T3, T4>(item1, item2, item3, item4);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000020F8 File Offset: 0x000002F8
		public static global::System.ValueTuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5)
		{
			return new global::System.ValueTuple<T1, T2, T3, T4, T5>(item1, item2, item3, item4, item5);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002105 File Offset: 0x00000305
		public static global::System.ValueTuple<T1, T2, T3, T4, T5, T6> Create<T1, T2, T3, T4, T5, T6>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6)
		{
			return new global::System.ValueTuple<T1, T2, T3, T4, T5, T6>(item1, item2, item3, item4, item5, item6);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002114 File Offset: 0x00000314
		public static global::System.ValueTuple<T1, T2, T3, T4, T5, T6, T7> Create<T1, T2, T3, T4, T5, T6, T7>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
		{
			return new global::System.ValueTuple<T1, T2, T3, T4, T5, T6, T7>(item1, item2, item3, item4, item5, item6, item7);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002125 File Offset: 0x00000325
		public static global::System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, global::System.ValueTuple<T8>> Create<T1, T2, T3, T4, T5, T6, T7, T8>(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8)
		{
			return new global::System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, global::System.ValueTuple<T8>>(item1, item2, item3, item4, item5, item6, item7, global::System.ValueTuple.Create<T8>(item8));
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0000213D File Offset: 0x0000033D
		internal static int CombineHashCodes(int h1, int h2)
		{
			return global::System.ValueTuple.Combine(global::System.ValueTuple.Combine(global::System.ValueTuple._randomSeed, h1), h2);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002150 File Offset: 0x00000350
		internal static int CombineHashCodes(int h1, int h2, int h3)
		{
			return global::System.ValueTuple.Combine(global::System.ValueTuple.CombineHashCodes(h1, h2), h3);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000215F File Offset: 0x0000035F
		internal static int CombineHashCodes(int h1, int h2, int h3, int h4)
		{
			return global::System.ValueTuple.Combine(global::System.ValueTuple.CombineHashCodes(h1, h2, h3), h4);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x0000216F File Offset: 0x0000036F
		internal static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5)
		{
			return global::System.ValueTuple.Combine(global::System.ValueTuple.CombineHashCodes(h1, h2, h3, h4), h5);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002181 File Offset: 0x00000381
		internal static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6)
		{
			return global::System.ValueTuple.Combine(global::System.ValueTuple.CombineHashCodes(h1, h2, h3, h4, h5), h6);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002195 File Offset: 0x00000395
		internal static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6, int h7)
		{
			return global::System.ValueTuple.Combine(global::System.ValueTuple.CombineHashCodes(h1, h2, h3, h4, h5, h6), h7);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000021AB File Offset: 0x000003AB
		internal static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6, int h7, int h8)
		{
			return global::System.ValueTuple.Combine(global::System.ValueTuple.CombineHashCodes(h1, h2, h3, h4, h5, h6, h7), h8);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000021C3 File Offset: 0x000003C3
		private static int Combine(int h1, int h2)
		{
			return (((h1 << 5) | (int)((uint)h1 >> 27)) + h1) ^ h2;
		}

		// Token: 0x04000001 RID: 1
		private static readonly int _randomSeed = new Random().Next(int.MinValue, int.MaxValue);
	}
}
