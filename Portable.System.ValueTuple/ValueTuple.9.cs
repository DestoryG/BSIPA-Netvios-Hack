using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x0200000B RID: 11
	[Serializable]
	[StructLayout(LayoutKind.Auto)]
	public struct ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> : IEquatable<global::System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>>, IStructuralEquatable, IStructuralComparable, IComparable, IComparable<global::System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>>, IValueTupleInternal, global::System.Runtime.CompilerServices.ITuple where TRest : struct
	{
		// Token: 0x0600008A RID: 138 RVA: 0x0000439C File Offset: 0x0000259C
		public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, TRest rest)
		{
			if (!(rest is IValueTupleInternal))
			{
				throw new ArgumentException();
			}
			this.Item1 = item1;
			this.Item2 = item2;
			this.Item3 = item3;
			this.Item4 = item4;
			this.Item5 = item5;
			this.Item6 = item6;
			this.Item7 = item7;
			this.Rest = rest;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000043FA File Offset: 0x000025FA
		public override bool Equals(object obj)
		{
			return obj is global::System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> && this.Equals((global::System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>)obj);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00004414 File Offset: 0x00002614
		public bool Equals(global::System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> other)
		{
			return EqualityComparer<T1>.Default.Equals(this.Item1, other.Item1) && EqualityComparer<T2>.Default.Equals(this.Item2, other.Item2) && EqualityComparer<T3>.Default.Equals(this.Item3, other.Item3) && EqualityComparer<T4>.Default.Equals(this.Item4, other.Item4) && EqualityComparer<T5>.Default.Equals(this.Item5, other.Item5) && EqualityComparer<T6>.Default.Equals(this.Item6, other.Item6) && EqualityComparer<T7>.Default.Equals(this.Item7, other.Item7) && EqualityComparer<TRest>.Default.Equals(this.Rest, other.Rest);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000044E7 File Offset: 0x000026E7
		int IComparable.CompareTo(object other)
		{
			if (other == null)
			{
				return 1;
			}
			if (!(other is global::System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>))
			{
				throw new ArgumentException();
			}
			return this.CompareTo((global::System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>)other);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00004508 File Offset: 0x00002708
		public int CompareTo(global::System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> other)
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
			num = Comparer<T6>.Default.Compare(this.Item6, other.Item6);
			if (num != 0)
			{
				return num;
			}
			num = Comparer<T7>.Default.Compare(this.Item7, other.Item7);
			if (num != 0)
			{
				return num;
			}
			return Comparer<TRest>.Default.Compare(this.Rest, other.Rest);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x000045F0 File Offset: 0x000027F0
		bool IStructuralEquatable.Equals(object other, IEqualityComparer comparer)
		{
			if (!(other is global::System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>))
			{
				return false;
			}
			global::System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> valueTuple = (global::System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>)other;
			return comparer.Equals(this.Item1, valueTuple.Item1) && comparer.Equals(this.Item2, valueTuple.Item2) && comparer.Equals(this.Item3, valueTuple.Item3) && comparer.Equals(this.Item4, valueTuple.Item4) && comparer.Equals(this.Item5, valueTuple.Item5) && comparer.Equals(this.Item6, valueTuple.Item6) && comparer.Equals(this.Item7, valueTuple.Item7) && comparer.Equals(this.Rest, valueTuple.Rest);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00004708 File Offset: 0x00002908
		int IStructuralComparable.CompareTo(object other, IComparer comparer)
		{
			if (other == null)
			{
				return 1;
			}
			if (!(other is global::System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>))
			{
				throw new ArgumentException();
			}
			global::System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> valueTuple = (global::System.ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>)other;
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
			num = comparer.Compare(this.Item6, valueTuple.Item6);
			if (num != 0)
			{
				return num;
			}
			num = comparer.Compare(this.Item7, valueTuple.Item7);
			if (num != 0)
			{
				return num;
			}
			return comparer.Compare(this.Rest, valueTuple.Rest);
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00004839 File Offset: 0x00002A39
		int IStructuralEquatable.GetHashCode(IEqualityComparer comparer)
		{
			return this.GetHashCodeCore(comparer);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00004844 File Offset: 0x00002A44
		public override int GetHashCode()
		{
			IValueTupleInternal valueTupleInternal = this.Rest as IValueTupleInternal;
			if (valueTupleInternal == null)
			{
				T1 t = this.Item1;
				ref T1 ptr = ref t;
				T1 t2 = default(T1);
				int num;
				if (t2 == null)
				{
					t2 = t;
					ptr = ref t2;
					if (t2 == null)
					{
						num = 0;
						goto IL_0052;
					}
				}
				num = ptr.GetHashCode();
				IL_0052:
				T2 t3 = this.Item2;
				ref T2 ptr2 = ref t3;
				T2 t4 = default(T2);
				int num2;
				if (t4 == null)
				{
					t4 = t3;
					ptr2 = ref t4;
					if (t4 == null)
					{
						num2 = 0;
						goto IL_008E;
					}
				}
				num2 = ptr2.GetHashCode();
				IL_008E:
				T3 t5 = this.Item3;
				ref T3 ptr3 = ref t5;
				T3 t6 = default(T3);
				int num3;
				if (t6 == null)
				{
					t6 = t5;
					ptr3 = ref t6;
					if (t6 == null)
					{
						num3 = 0;
						goto IL_00CA;
					}
				}
				num3 = ptr3.GetHashCode();
				IL_00CA:
				T4 t7 = this.Item4;
				ref T4 ptr4 = ref t7;
				T4 t8 = default(T4);
				int num4;
				if (t8 == null)
				{
					t8 = t7;
					ptr4 = ref t8;
					if (t8 == null)
					{
						num4 = 0;
						goto IL_0106;
					}
				}
				num4 = ptr4.GetHashCode();
				IL_0106:
				T5 t9 = this.Item5;
				ref T5 ptr5 = ref t9;
				T5 t10 = default(T5);
				int num5;
				if (t10 == null)
				{
					t10 = t9;
					ptr5 = ref t10;
					if (t10 == null)
					{
						num5 = 0;
						goto IL_0142;
					}
				}
				num5 = ptr5.GetHashCode();
				IL_0142:
				T6 t11 = this.Item6;
				ref T6 ptr6 = ref t11;
				T6 t12 = default(T6);
				int num6;
				if (t12 == null)
				{
					t12 = t11;
					ptr6 = ref t12;
					if (t12 == null)
					{
						num6 = 0;
						goto IL_017E;
					}
				}
				num6 = ptr6.GetHashCode();
				IL_017E:
				T7 t13 = this.Item7;
				ref T7 ptr7 = ref t13;
				T7 t14 = default(T7);
				int num7;
				if (t14 == null)
				{
					t14 = t13;
					ptr7 = ref t14;
					if (t14 == null)
					{
						num7 = 0;
						goto IL_01BA;
					}
				}
				num7 = ptr7.GetHashCode();
				IL_01BA:
				return global::System.ValueTuple.CombineHashCodes(num, num2, num3, num4, num5, num6, num7);
			}
			int length = valueTupleInternal.Length;
			if (length >= 8)
			{
				return valueTupleInternal.GetHashCode();
			}
			switch (8 - length)
			{
			case 1:
			{
				T7 t13 = this.Item7;
				ref T7 ptr8 = ref t13;
				T7 t14 = default(T7);
				int num8;
				if (t14 == null)
				{
					t14 = t13;
					ptr8 = ref t14;
					if (t14 == null)
					{
						num8 = 0;
						goto IL_023F;
					}
				}
				num8 = ptr8.GetHashCode();
				IL_023F:
				return global::System.ValueTuple.CombineHashCodes(num8, valueTupleInternal.GetHashCode());
			}
			case 2:
			{
				T6 t11 = this.Item6;
				ref T6 ptr9 = ref t11;
				T6 t12 = default(T6);
				int num9;
				if (t12 == null)
				{
					t12 = t11;
					ptr9 = ref t12;
					if (t12 == null)
					{
						num9 = 0;
						goto IL_0287;
					}
				}
				num9 = ptr9.GetHashCode();
				IL_0287:
				T7 t13 = this.Item7;
				ref T7 ptr10 = ref t13;
				T7 t14 = default(T7);
				int num10;
				if (t14 == null)
				{
					t14 = t13;
					ptr10 = ref t14;
					if (t14 == null)
					{
						num10 = 0;
						goto IL_02C3;
					}
				}
				num10 = ptr10.GetHashCode();
				IL_02C3:
				return global::System.ValueTuple.CombineHashCodes(num9, num10, valueTupleInternal.GetHashCode());
			}
			case 3:
			{
				T5 t9 = this.Item5;
				ref T5 ptr11 = ref t9;
				T5 t10 = default(T5);
				int num11;
				if (t10 == null)
				{
					t10 = t9;
					ptr11 = ref t10;
					if (t10 == null)
					{
						num11 = 0;
						goto IL_030B;
					}
				}
				num11 = ptr11.GetHashCode();
				IL_030B:
				T6 t11 = this.Item6;
				ref T6 ptr12 = ref t11;
				T6 t12 = default(T6);
				int num12;
				if (t12 == null)
				{
					t12 = t11;
					ptr12 = ref t12;
					if (t12 == null)
					{
						num12 = 0;
						goto IL_0347;
					}
				}
				num12 = ptr12.GetHashCode();
				IL_0347:
				T7 t13 = this.Item7;
				ref T7 ptr13 = ref t13;
				T7 t14 = default(T7);
				int num13;
				if (t14 == null)
				{
					t14 = t13;
					ptr13 = ref t14;
					if (t14 == null)
					{
						num13 = 0;
						goto IL_0383;
					}
				}
				num13 = ptr13.GetHashCode();
				IL_0383:
				return global::System.ValueTuple.CombineHashCodes(num11, num12, num13, valueTupleInternal.GetHashCode());
			}
			case 4:
			{
				T4 t7 = this.Item4;
				ref T4 ptr14 = ref t7;
				T4 t8 = default(T4);
				int num14;
				if (t8 == null)
				{
					t8 = t7;
					ptr14 = ref t8;
					if (t8 == null)
					{
						num14 = 0;
						goto IL_03CB;
					}
				}
				num14 = ptr14.GetHashCode();
				IL_03CB:
				T5 t9 = this.Item5;
				ref T5 ptr15 = ref t9;
				T5 t10 = default(T5);
				int num15;
				if (t10 == null)
				{
					t10 = t9;
					ptr15 = ref t10;
					if (t10 == null)
					{
						num15 = 0;
						goto IL_0407;
					}
				}
				num15 = ptr15.GetHashCode();
				IL_0407:
				T6 t11 = this.Item6;
				ref T6 ptr16 = ref t11;
				T6 t12 = default(T6);
				int num16;
				if (t12 == null)
				{
					t12 = t11;
					ptr16 = ref t12;
					if (t12 == null)
					{
						num16 = 0;
						goto IL_0443;
					}
				}
				num16 = ptr16.GetHashCode();
				IL_0443:
				T7 t13 = this.Item7;
				ref T7 ptr17 = ref t13;
				T7 t14 = default(T7);
				int num17;
				if (t14 == null)
				{
					t14 = t13;
					ptr17 = ref t14;
					if (t14 == null)
					{
						num17 = 0;
						goto IL_047F;
					}
				}
				num17 = ptr17.GetHashCode();
				IL_047F:
				return global::System.ValueTuple.CombineHashCodes(num14, num15, num16, num17, valueTupleInternal.GetHashCode());
			}
			case 5:
			{
				T3 t5 = this.Item3;
				ref T3 ptr18 = ref t5;
				T3 t6 = default(T3);
				int num18;
				if (t6 == null)
				{
					t6 = t5;
					ptr18 = ref t6;
					if (t6 == null)
					{
						num18 = 0;
						goto IL_04C7;
					}
				}
				num18 = ptr18.GetHashCode();
				IL_04C7:
				T4 t7 = this.Item4;
				ref T4 ptr19 = ref t7;
				T4 t8 = default(T4);
				int num19;
				if (t8 == null)
				{
					t8 = t7;
					ptr19 = ref t8;
					if (t8 == null)
					{
						num19 = 0;
						goto IL_0503;
					}
				}
				num19 = ptr19.GetHashCode();
				IL_0503:
				T5 t9 = this.Item5;
				ref T5 ptr20 = ref t9;
				T5 t10 = default(T5);
				int num20;
				if (t10 == null)
				{
					t10 = t9;
					ptr20 = ref t10;
					if (t10 == null)
					{
						num20 = 0;
						goto IL_053F;
					}
				}
				num20 = ptr20.GetHashCode();
				IL_053F:
				T6 t11 = this.Item6;
				ref T6 ptr21 = ref t11;
				T6 t12 = default(T6);
				int num21;
				if (t12 == null)
				{
					t12 = t11;
					ptr21 = ref t12;
					if (t12 == null)
					{
						num21 = 0;
						goto IL_057B;
					}
				}
				num21 = ptr21.GetHashCode();
				IL_057B:
				T7 t13 = this.Item7;
				ref T7 ptr22 = ref t13;
				T7 t14 = default(T7);
				int num22;
				if (t14 == null)
				{
					t14 = t13;
					ptr22 = ref t14;
					if (t14 == null)
					{
						num22 = 0;
						goto IL_05B7;
					}
				}
				num22 = ptr22.GetHashCode();
				IL_05B7:
				return global::System.ValueTuple.CombineHashCodes(num18, num19, num20, num21, num22, valueTupleInternal.GetHashCode());
			}
			case 6:
			{
				T2 t3 = this.Item2;
				ref T2 ptr23 = ref t3;
				T2 t4 = default(T2);
				int num23;
				if (t4 == null)
				{
					t4 = t3;
					ptr23 = ref t4;
					if (t4 == null)
					{
						num23 = 0;
						goto IL_05FF;
					}
				}
				num23 = ptr23.GetHashCode();
				IL_05FF:
				T3 t5 = this.Item3;
				ref T3 ptr24 = ref t5;
				T3 t6 = default(T3);
				int num24;
				if (t6 == null)
				{
					t6 = t5;
					ptr24 = ref t6;
					if (t6 == null)
					{
						num24 = 0;
						goto IL_063B;
					}
				}
				num24 = ptr24.GetHashCode();
				IL_063B:
				T4 t7 = this.Item4;
				ref T4 ptr25 = ref t7;
				T4 t8 = default(T4);
				int num25;
				if (t8 == null)
				{
					t8 = t7;
					ptr25 = ref t8;
					if (t8 == null)
					{
						num25 = 0;
						goto IL_0677;
					}
				}
				num25 = ptr25.GetHashCode();
				IL_0677:
				T5 t9 = this.Item5;
				ref T5 ptr26 = ref t9;
				T5 t10 = default(T5);
				int num26;
				if (t10 == null)
				{
					t10 = t9;
					ptr26 = ref t10;
					if (t10 == null)
					{
						num26 = 0;
						goto IL_06B3;
					}
				}
				num26 = ptr26.GetHashCode();
				IL_06B3:
				T6 t11 = this.Item6;
				ref T6 ptr27 = ref t11;
				T6 t12 = default(T6);
				int num27;
				if (t12 == null)
				{
					t12 = t11;
					ptr27 = ref t12;
					if (t12 == null)
					{
						num27 = 0;
						goto IL_06EF;
					}
				}
				num27 = ptr27.GetHashCode();
				IL_06EF:
				T7 t13 = this.Item7;
				ref T7 ptr28 = ref t13;
				T7 t14 = default(T7);
				int num28;
				if (t14 == null)
				{
					t14 = t13;
					ptr28 = ref t14;
					if (t14 == null)
					{
						num28 = 0;
						goto IL_072B;
					}
				}
				num28 = ptr28.GetHashCode();
				IL_072B:
				return global::System.ValueTuple.CombineHashCodes(num23, num24, num25, num26, num27, num28, valueTupleInternal.GetHashCode());
			}
			case 7:
			case 8:
			{
				T1 t = this.Item1;
				ref T1 ptr29 = ref t;
				T1 t2 = default(T1);
				int num29;
				if (t2 == null)
				{
					t2 = t;
					ptr29 = ref t2;
					if (t2 == null)
					{
						num29 = 0;
						goto IL_0772;
					}
				}
				num29 = ptr29.GetHashCode();
				IL_0772:
				T2 t3 = this.Item2;
				ref T2 ptr30 = ref t3;
				T2 t4 = default(T2);
				int num30;
				if (t4 == null)
				{
					t4 = t3;
					ptr30 = ref t4;
					if (t4 == null)
					{
						num30 = 0;
						goto IL_07AE;
					}
				}
				num30 = ptr30.GetHashCode();
				IL_07AE:
				T3 t5 = this.Item3;
				ref T3 ptr31 = ref t5;
				T3 t6 = default(T3);
				int num31;
				if (t6 == null)
				{
					t6 = t5;
					ptr31 = ref t6;
					if (t6 == null)
					{
						num31 = 0;
						goto IL_07EA;
					}
				}
				num31 = ptr31.GetHashCode();
				IL_07EA:
				T4 t7 = this.Item4;
				ref T4 ptr32 = ref t7;
				T4 t8 = default(T4);
				int num32;
				if (t8 == null)
				{
					t8 = t7;
					ptr32 = ref t8;
					if (t8 == null)
					{
						num32 = 0;
						goto IL_0826;
					}
				}
				num32 = ptr32.GetHashCode();
				IL_0826:
				T5 t9 = this.Item5;
				ref T5 ptr33 = ref t9;
				T5 t10 = default(T5);
				int num33;
				if (t10 == null)
				{
					t10 = t9;
					ptr33 = ref t10;
					if (t10 == null)
					{
						num33 = 0;
						goto IL_0862;
					}
				}
				num33 = ptr33.GetHashCode();
				IL_0862:
				T6 t11 = this.Item6;
				ref T6 ptr34 = ref t11;
				T6 t12 = default(T6);
				int num34;
				if (t12 == null)
				{
					t12 = t11;
					ptr34 = ref t12;
					if (t12 == null)
					{
						num34 = 0;
						goto IL_089E;
					}
				}
				num34 = ptr34.GetHashCode();
				IL_089E:
				T7 t13 = this.Item7;
				ref T7 ptr35 = ref t13;
				T7 t14 = default(T7);
				int num35;
				if (t14 == null)
				{
					t14 = t13;
					ptr35 = ref t14;
					if (t14 == null)
					{
						num35 = 0;
						goto IL_08DA;
					}
				}
				num35 = ptr35.GetHashCode();
				IL_08DA:
				return global::System.ValueTuple.CombineHashCodes(num29, num30, num31, num32, num33, num34, num35, valueTupleInternal.GetHashCode());
			}
			default:
				throw new InvalidOperationException("Missed all cases for computing ValueTuple hash code");
			}
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00005144 File Offset: 0x00003344
		private int GetHashCodeCore(IEqualityComparer comparer)
		{
			IValueTupleInternal valueTupleInternal = this.Rest as IValueTupleInternal;
			if (valueTupleInternal == null)
			{
				return global::System.ValueTuple.CombineHashCodes(comparer.GetHashCode(this.Item1), comparer.GetHashCode(this.Item2), comparer.GetHashCode(this.Item3), comparer.GetHashCode(this.Item4), comparer.GetHashCode(this.Item5), comparer.GetHashCode(this.Item6), comparer.GetHashCode(this.Item7));
			}
			int length = valueTupleInternal.Length;
			if (length >= 8)
			{
				return valueTupleInternal.GetHashCode(comparer);
			}
			switch (8 - length)
			{
			case 1:
				return global::System.ValueTuple.CombineHashCodes(comparer.GetHashCode(this.Item7), valueTupleInternal.GetHashCode(comparer));
			case 2:
				return global::System.ValueTuple.CombineHashCodes(comparer.GetHashCode(this.Item6), comparer.GetHashCode(this.Item7), valueTupleInternal.GetHashCode(comparer));
			case 3:
				return global::System.ValueTuple.CombineHashCodes(comparer.GetHashCode(this.Item5), comparer.GetHashCode(this.Item6), comparer.GetHashCode(this.Item7), valueTupleInternal.GetHashCode(comparer));
			case 4:
				return global::System.ValueTuple.CombineHashCodes(comparer.GetHashCode(this.Item4), comparer.GetHashCode(this.Item5), comparer.GetHashCode(this.Item6), comparer.GetHashCode(this.Item7), valueTupleInternal.GetHashCode(comparer));
			case 5:
				return global::System.ValueTuple.CombineHashCodes(comparer.GetHashCode(this.Item3), comparer.GetHashCode(this.Item4), comparer.GetHashCode(this.Item5), comparer.GetHashCode(this.Item6), comparer.GetHashCode(this.Item7), valueTupleInternal.GetHashCode(comparer));
			case 6:
				return global::System.ValueTuple.CombineHashCodes(comparer.GetHashCode(this.Item2), comparer.GetHashCode(this.Item3), comparer.GetHashCode(this.Item4), comparer.GetHashCode(this.Item5), comparer.GetHashCode(this.Item6), comparer.GetHashCode(this.Item7), valueTupleInternal.GetHashCode(comparer));
			case 7:
			case 8:
				return global::System.ValueTuple.CombineHashCodes(comparer.GetHashCode(this.Item1), comparer.GetHashCode(this.Item2), comparer.GetHashCode(this.Item3), comparer.GetHashCode(this.Item4), comparer.GetHashCode(this.Item5), comparer.GetHashCode(this.Item6), comparer.GetHashCode(this.Item7), valueTupleInternal.GetHashCode(comparer));
			default:
				throw new InvalidOperationException("Missed all cases for computing ValueTuple hash code");
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00005467 File Offset: 0x00003667
		int IValueTupleInternal.GetHashCode(IEqualityComparer comparer)
		{
			return this.GetHashCodeCore(comparer);
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00005470 File Offset: 0x00003670
		public override string ToString()
		{
			IValueTupleInternal valueTupleInternal = this.Rest as IValueTupleInternal;
			if (valueTupleInternal == null)
			{
				return string.Format("({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})", new object[] { this.Item1, this.Item2, this.Item3, this.Item4, this.Item5, this.Item6, this.Item7, this.Rest });
			}
			return string.Format("({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", new object[]
			{
				this.Item1,
				this.Item2,
				this.Item3,
				this.Item4,
				this.Item5,
				this.Item6,
				this.Item7,
				valueTupleInternal.ToStringEnd()
			});
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00005590 File Offset: 0x00003790
		string IValueTupleInternal.ToStringEnd()
		{
			IValueTupleInternal valueTupleInternal = this.Rest as IValueTupleInternal;
			if (valueTupleInternal == null)
			{
				return string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})", new object[] { this.Item1, this.Item2, this.Item3, this.Item4, this.Item5, this.Item6, this.Item7, this.Rest });
			}
			return string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", new object[]
			{
				this.Item1,
				this.Item2,
				this.Item3,
				this.Item4,
				this.Item5,
				this.Item6,
				this.Item7,
				valueTupleInternal.ToStringEnd()
			});
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000097 RID: 151 RVA: 0x000056B0 File Offset: 0x000038B0
		int global::System.Runtime.CompilerServices.ITuple.Length
		{
			get
			{
				IValueTupleInternal valueTupleInternal = this.Rest as IValueTupleInternal;
				if (valueTupleInternal != null)
				{
					return 7 + valueTupleInternal.Length;
				}
				return 8;
			}
		}

		// Token: 0x17000012 RID: 18
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
				case 6:
					return this.Item7;
				default:
				{
					IValueTupleInternal valueTupleInternal = this.Rest as IValueTupleInternal;
					if (valueTupleInternal != null)
					{
						return valueTupleInternal[index - 7];
					}
					if (index == 7)
					{
						return this.Rest;
					}
					throw new IndexOutOfRangeException();
				}
				}
			}
		}

		// Token: 0x0400001E RID: 30
		public readonly T1 Item1;

		// Token: 0x0400001F RID: 31
		public readonly T2 Item2;

		// Token: 0x04000020 RID: 32
		public readonly T3 Item3;

		// Token: 0x04000021 RID: 33
		public readonly T4 Item4;

		// Token: 0x04000022 RID: 34
		public readonly T5 Item5;

		// Token: 0x04000023 RID: 35
		public readonly T6 Item6;

		// Token: 0x04000024 RID: 36
		public readonly T7 Item7;

		// Token: 0x04000025 RID: 37
		public readonly TRest Rest;
	}
}
