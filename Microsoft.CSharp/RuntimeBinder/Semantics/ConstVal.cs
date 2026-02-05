using System;
using System.Globalization;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x02000034 RID: 52
	internal struct ConstVal
	{
		// Token: 0x06000235 RID: 565 RVA: 0x0001160C File Offset: 0x0000F80C
		private ConstVal(object value)
		{
			this.ObjectVal = value;
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000236 RID: 566 RVA: 0x00011615 File Offset: 0x0000F815
		public object ObjectVal { get; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000237 RID: 567 RVA: 0x0001161D File Offset: 0x0000F81D
		public bool BooleanVal
		{
			get
			{
				return ConstVal.SpecialUnbox<bool>(this.ObjectVal);
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000238 RID: 568 RVA: 0x0001162A File Offset: 0x0000F82A
		public sbyte SByteVal
		{
			get
			{
				return ConstVal.SpecialUnbox<sbyte>(this.ObjectVal);
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000239 RID: 569 RVA: 0x00011637 File Offset: 0x0000F837
		public byte ByteVal
		{
			get
			{
				return ConstVal.SpecialUnbox<byte>(this.ObjectVal);
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600023A RID: 570 RVA: 0x00011644 File Offset: 0x0000F844
		public short Int16Val
		{
			get
			{
				return ConstVal.SpecialUnbox<short>(this.ObjectVal);
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600023B RID: 571 RVA: 0x00011651 File Offset: 0x0000F851
		public ushort UInt16Val
		{
			get
			{
				return ConstVal.SpecialUnbox<ushort>(this.ObjectVal);
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600023C RID: 572 RVA: 0x0001165E File Offset: 0x0000F85E
		public int Int32Val
		{
			get
			{
				return ConstVal.SpecialUnbox<int>(this.ObjectVal);
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600023D RID: 573 RVA: 0x0001166B File Offset: 0x0000F86B
		public uint UInt32Val
		{
			get
			{
				return ConstVal.SpecialUnbox<uint>(this.ObjectVal);
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600023E RID: 574 RVA: 0x00011678 File Offset: 0x0000F878
		public long Int64Val
		{
			get
			{
				return ConstVal.SpecialUnbox<long>(this.ObjectVal);
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600023F RID: 575 RVA: 0x00011685 File Offset: 0x0000F885
		public ulong UInt64Val
		{
			get
			{
				return ConstVal.SpecialUnbox<ulong>(this.ObjectVal);
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000240 RID: 576 RVA: 0x00011692 File Offset: 0x0000F892
		public float SingleVal
		{
			get
			{
				return ConstVal.SpecialUnbox<float>(this.ObjectVal);
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000241 RID: 577 RVA: 0x0001169F File Offset: 0x0000F89F
		public double DoubleVal
		{
			get
			{
				return ConstVal.SpecialUnbox<double>(this.ObjectVal);
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000242 RID: 578 RVA: 0x000116AC File Offset: 0x0000F8AC
		public decimal DecimalVal
		{
			get
			{
				return ConstVal.SpecialUnbox<decimal>(this.ObjectVal);
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000243 RID: 579 RVA: 0x000116B9 File Offset: 0x0000F8B9
		public char CharVal
		{
			get
			{
				return ConstVal.SpecialUnbox<char>(this.ObjectVal);
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000244 RID: 580 RVA: 0x000116C6 File Offset: 0x0000F8C6
		public string StringVal
		{
			get
			{
				return ConstVal.SpecialUnbox<string>(this.ObjectVal);
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000245 RID: 581 RVA: 0x000116D3 File Offset: 0x0000F8D3
		public bool IsNullRef
		{
			get
			{
				return this.ObjectVal == null;
			}
		}

		// Token: 0x06000246 RID: 582 RVA: 0x000116DE File Offset: 0x0000F8DE
		public bool IsZero(ConstValKind kind)
		{
			if (kind == ConstValKind.String)
			{
				return false;
			}
			if (kind == ConstValKind.Decimal)
			{
				return this.DecimalVal == 0m;
			}
			return ConstVal.IsDefault(this.ObjectVal);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x00011708 File Offset: 0x0000F908
		private static T SpecialUnbox<T>(object o)
		{
			if (ConstVal.IsDefault(o))
			{
				return default(T);
			}
			return (T)((object)Convert.ChangeType(o, typeof(T), CultureInfo.InvariantCulture));
		}

		// Token: 0x06000248 RID: 584 RVA: 0x00011744 File Offset: 0x0000F944
		private static bool IsDefault(object o)
		{
			if (o == null)
			{
				return true;
			}
			switch (Type.GetTypeCode(o.GetType()))
			{
			case TypeCode.Boolean:
				return false.Equals(o);
			case TypeCode.Char:
				return '\0'.Equals(o);
			case TypeCode.SByte:
				return 0.Equals(o);
			case TypeCode.Byte:
				return 0.Equals(o);
			case TypeCode.Int16:
				return 0.Equals(o);
			case TypeCode.UInt16:
				return 0.Equals(o);
			case TypeCode.Int32:
				return 0.Equals(o);
			case TypeCode.UInt32:
				return 0U.Equals(o);
			case TypeCode.Int64:
				return 0L.Equals(o);
			case TypeCode.UInt64:
				return 0UL.Equals(o);
			case TypeCode.Single:
				return 0f.Equals(o);
			case TypeCode.Double:
				return 0.0.Equals(o);
			case TypeCode.Decimal:
				return 0m.Equals(o);
			default:
				return false;
			}
		}

		// Token: 0x06000249 RID: 585 RVA: 0x00011850 File Offset: 0x0000FA50
		public static ConstVal GetDefaultValue(ConstValKind kind)
		{
			switch (kind)
			{
			case ConstValKind.Int:
				return new ConstVal(ConstVal.s_zeroInt32);
			case ConstValKind.Double:
				return new ConstVal(0.0);
			case ConstValKind.Long:
				return new ConstVal(0L);
			case ConstValKind.Decimal:
				return new ConstVal(0m);
			case ConstValKind.Float:
				return new ConstVal(0f);
			case ConstValKind.Boolean:
				return new ConstVal(ConstVal.s_false);
			}
			return default(ConstVal);
		}

		// Token: 0x0600024A RID: 586 RVA: 0x000118E5 File Offset: 0x0000FAE5
		public static ConstVal Get(bool value)
		{
			return new ConstVal(value ? ConstVal.s_true : ConstVal.s_false);
		}

		// Token: 0x0600024B RID: 587 RVA: 0x000118FB File Offset: 0x0000FAFB
		public static ConstVal Get(int value)
		{
			return new ConstVal((value == 0) ? ConstVal.s_zeroInt32 : value);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x00011912 File Offset: 0x0000FB12
		public static ConstVal Get(uint value)
		{
			return new ConstVal(value);
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0001191F File Offset: 0x0000FB1F
		public static ConstVal Get(decimal value)
		{
			return new ConstVal(value);
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0001192C File Offset: 0x0000FB2C
		public static ConstVal Get(string value)
		{
			return new ConstVal(value);
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00011934 File Offset: 0x0000FB34
		public static ConstVal Get(float value)
		{
			return new ConstVal(value);
		}

		// Token: 0x06000250 RID: 592 RVA: 0x00011941 File Offset: 0x0000FB41
		public static ConstVal Get(double value)
		{
			return new ConstVal(value);
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0001194E File Offset: 0x0000FB4E
		public static ConstVal Get(long value)
		{
			return new ConstVal(value);
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0001195B File Offset: 0x0000FB5B
		public static ConstVal Get(ulong value)
		{
			return new ConstVal(value);
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00011968 File Offset: 0x0000FB68
		public static ConstVal Get(object p)
		{
			return new ConstVal(p);
		}

		// Token: 0x040002A4 RID: 676
		private static readonly object s_false = false;

		// Token: 0x040002A5 RID: 677
		private static readonly object s_true = true;

		// Token: 0x040002A6 RID: 678
		private static readonly object s_zeroInt32 = 0;
	}
}
