using System;
using System.Text;

namespace System.Collections.Specialized
{
	// Token: 0x020003A7 RID: 935
	public struct BitVector32
	{
		// Token: 0x060022DD RID: 8925 RVA: 0x000A5E28 File Offset: 0x000A4028
		public BitVector32(int data)
		{
			this.data = (uint)data;
		}

		// Token: 0x060022DE RID: 8926 RVA: 0x000A5E31 File Offset: 0x000A4031
		public BitVector32(BitVector32 value)
		{
			this.data = value.data;
		}

		// Token: 0x170008D6 RID: 2262
		public bool this[int bit]
		{
			get
			{
				return ((ulong)this.data & (ulong)((long)bit)) == (ulong)bit;
			}
			set
			{
				if (value)
				{
					this.data |= (uint)bit;
					return;
				}
				this.data &= (uint)(~(uint)bit);
			}
		}

		// Token: 0x170008D7 RID: 2263
		public int this[BitVector32.Section section]
		{
			get
			{
				return (int)((this.data & (uint)((uint)section.Mask << (int)section.Offset)) >> (int)section.Offset);
			}
			set
			{
				value <<= (int)section.Offset;
				int num = (65535 & (int)section.Mask) << (int)section.Offset;
				this.data = (this.data & (uint)(~(uint)num)) | (uint)(value & num);
			}
		}

		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x060022E3 RID: 8931 RVA: 0x000A5EDF File Offset: 0x000A40DF
		public int Data
		{
			get
			{
				return (int)this.data;
			}
		}

		// Token: 0x060022E4 RID: 8932 RVA: 0x000A5EE8 File Offset: 0x000A40E8
		private static short CountBitsSet(short mask)
		{
			short num = 0;
			while ((mask & 1) != 0)
			{
				num += 1;
				mask = (short)(mask >> 1);
			}
			return num;
		}

		// Token: 0x060022E5 RID: 8933 RVA: 0x000A5F0A File Offset: 0x000A410A
		public static int CreateMask()
		{
			return BitVector32.CreateMask(0);
		}

		// Token: 0x060022E6 RID: 8934 RVA: 0x000A5F12 File Offset: 0x000A4112
		public static int CreateMask(int previous)
		{
			if (previous == 0)
			{
				return 1;
			}
			if (previous == -2147483648)
			{
				throw new InvalidOperationException(SR.GetString("BitVectorFull"));
			}
			return previous << 1;
		}

		// Token: 0x060022E7 RID: 8935 RVA: 0x000A5F34 File Offset: 0x000A4134
		private static short CreateMaskFromHighValue(short highValue)
		{
			short num = 16;
			while (((int)highValue & 32768) == 0)
			{
				num -= 1;
				highValue = (short)(highValue << 1);
			}
			ushort num2 = 0;
			while (num > 0)
			{
				num -= 1;
				num2 = (ushort)(num2 << 1);
				num2 |= 1;
			}
			return (short)num2;
		}

		// Token: 0x060022E8 RID: 8936 RVA: 0x000A5F73 File Offset: 0x000A4173
		public static BitVector32.Section CreateSection(short maxValue)
		{
			return BitVector32.CreateSectionHelper(maxValue, 0, 0);
		}

		// Token: 0x060022E9 RID: 8937 RVA: 0x000A5F7D File Offset: 0x000A417D
		public static BitVector32.Section CreateSection(short maxValue, BitVector32.Section previous)
		{
			return BitVector32.CreateSectionHelper(maxValue, previous.Mask, previous.Offset);
		}

		// Token: 0x060022EA RID: 8938 RVA: 0x000A5F94 File Offset: 0x000A4194
		private static BitVector32.Section CreateSectionHelper(short maxValue, short priorMask, short priorOffset)
		{
			if (maxValue < 1)
			{
				throw new ArgumentException(SR.GetString("Argument_InvalidValue", new object[] { "maxValue", 0 }), "maxValue");
			}
			short num = priorOffset + BitVector32.CountBitsSet(priorMask);
			if (num >= 32)
			{
				throw new InvalidOperationException(SR.GetString("BitVectorFull"));
			}
			return new BitVector32.Section(BitVector32.CreateMaskFromHighValue(maxValue), num);
		}

		// Token: 0x060022EB RID: 8939 RVA: 0x000A5FFC File Offset: 0x000A41FC
		public override bool Equals(object o)
		{
			return o is BitVector32 && this.data == ((BitVector32)o).data;
		}

		// Token: 0x060022EC RID: 8940 RVA: 0x000A601B File Offset: 0x000A421B
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x060022ED RID: 8941 RVA: 0x000A6030 File Offset: 0x000A4230
		public static string ToString(BitVector32 value)
		{
			StringBuilder stringBuilder = new StringBuilder(45);
			stringBuilder.Append("BitVector32{");
			int num = (int)value.data;
			for (int i = 0; i < 32; i++)
			{
				if (((long)num & (long)((ulong)(-2147483648))) != 0L)
				{
					stringBuilder.Append("1");
				}
				else
				{
					stringBuilder.Append("0");
				}
				num <<= 1;
			}
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}

		// Token: 0x060022EE RID: 8942 RVA: 0x000A60A0 File Offset: 0x000A42A0
		public override string ToString()
		{
			return BitVector32.ToString(this);
		}

		// Token: 0x04001FAC RID: 8108
		private uint data;

		// Token: 0x020007E7 RID: 2023
		public struct Section
		{
			// Token: 0x060043D7 RID: 17367 RVA: 0x0011D841 File Offset: 0x0011BA41
			internal Section(short mask, short offset)
			{
				this.mask = mask;
				this.offset = offset;
			}

			// Token: 0x17000F56 RID: 3926
			// (get) Token: 0x060043D8 RID: 17368 RVA: 0x0011D851 File Offset: 0x0011BA51
			public short Mask
			{
				get
				{
					return this.mask;
				}
			}

			// Token: 0x17000F57 RID: 3927
			// (get) Token: 0x060043D9 RID: 17369 RVA: 0x0011D859 File Offset: 0x0011BA59
			public short Offset
			{
				get
				{
					return this.offset;
				}
			}

			// Token: 0x060043DA RID: 17370 RVA: 0x0011D861 File Offset: 0x0011BA61
			public override bool Equals(object o)
			{
				return o is BitVector32.Section && this.Equals((BitVector32.Section)o);
			}

			// Token: 0x060043DB RID: 17371 RVA: 0x0011D879 File Offset: 0x0011BA79
			public bool Equals(BitVector32.Section obj)
			{
				return obj.mask == this.mask && obj.offset == this.offset;
			}

			// Token: 0x060043DC RID: 17372 RVA: 0x0011D899 File Offset: 0x0011BA99
			public static bool operator ==(BitVector32.Section a, BitVector32.Section b)
			{
				return a.Equals(b);
			}

			// Token: 0x060043DD RID: 17373 RVA: 0x0011D8A3 File Offset: 0x0011BAA3
			public static bool operator !=(BitVector32.Section a, BitVector32.Section b)
			{
				return !(a == b);
			}

			// Token: 0x060043DE RID: 17374 RVA: 0x0011D8AF File Offset: 0x0011BAAF
			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			// Token: 0x060043DF RID: 17375 RVA: 0x0011D8C4 File Offset: 0x0011BAC4
			public static string ToString(BitVector32.Section value)
			{
				return string.Concat(new string[]
				{
					"Section{0x",
					Convert.ToString(value.Mask, 16),
					", 0x",
					Convert.ToString(value.Offset, 16),
					"}"
				});
			}

			// Token: 0x060043E0 RID: 17376 RVA: 0x0011D916 File Offset: 0x0011BB16
			public override string ToString()
			{
				return BitVector32.Section.ToString(this);
			}

			// Token: 0x040034EB RID: 13547
			private readonly short mask;

			// Token: 0x040034EC RID: 13548
			private readonly short offset;
		}
	}
}
