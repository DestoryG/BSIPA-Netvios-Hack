using System;
using System.Security.Cryptography.X509Certificates;

namespace System.Security.Cryptography
{
	// Token: 0x02000450 RID: 1104
	internal sealed class BigInt
	{
		// Token: 0x060028D8 RID: 10456 RVA: 0x000BACB3 File Offset: 0x000B8EB3
		internal BigInt()
		{
			this.m_elements = new byte[128];
		}

		// Token: 0x060028D9 RID: 10457 RVA: 0x000BACCB File Offset: 0x000B8ECB
		internal BigInt(byte b)
		{
			this.m_elements = new byte[128];
			this.SetDigit(0, b);
		}

		// Token: 0x17000A0A RID: 2570
		// (get) Token: 0x060028DA RID: 10458 RVA: 0x000BACEB File Offset: 0x000B8EEB
		// (set) Token: 0x060028DB RID: 10459 RVA: 0x000BACF3 File Offset: 0x000B8EF3
		internal int Size
		{
			get
			{
				return this.m_size;
			}
			set
			{
				if (value > 128)
				{
					this.m_size = 128;
				}
				if (value < 0)
				{
					this.m_size = 0;
				}
				this.m_size = value;
			}
		}

		// Token: 0x060028DC RID: 10460 RVA: 0x000BAD1A File Offset: 0x000B8F1A
		internal byte GetDigit(int index)
		{
			if (index < 0 || index >= this.m_size)
			{
				return 0;
			}
			return this.m_elements[index];
		}

		// Token: 0x060028DD RID: 10461 RVA: 0x000BAD34 File Offset: 0x000B8F34
		internal void SetDigit(int index, byte digit)
		{
			if (index >= 0 && index < 128)
			{
				this.m_elements[index] = digit;
				if (index >= this.m_size && digit != 0)
				{
					this.m_size = index + 1;
				}
				if (index == this.m_size - 1 && digit == 0)
				{
					this.m_size--;
				}
			}
		}

		// Token: 0x060028DE RID: 10462 RVA: 0x000BAD87 File Offset: 0x000B8F87
		internal void SetDigit(int index, byte digit, ref int size)
		{
			if (index >= 0 && index < 128)
			{
				this.m_elements[index] = digit;
				if (index >= size && digit != 0)
				{
					size = index + 1;
				}
				if (index == size - 1 && digit == 0)
				{
					size--;
				}
			}
		}

		// Token: 0x060028DF RID: 10463 RVA: 0x000BADBC File Offset: 0x000B8FBC
		public static bool operator <(BigInt value1, BigInt value2)
		{
			if (value1 == null)
			{
				return true;
			}
			if (value2 == null)
			{
				return false;
			}
			int size = value1.Size;
			int size2 = value2.Size;
			if (size != size2)
			{
				return size < size2;
			}
			while (size-- > 0)
			{
				if (value1.m_elements[size] != value2.m_elements[size])
				{
					return value1.m_elements[size] < value2.m_elements[size];
				}
			}
			return false;
		}

		// Token: 0x060028E0 RID: 10464 RVA: 0x000BAE24 File Offset: 0x000B9024
		public static bool operator >(BigInt value1, BigInt value2)
		{
			if (value1 == null)
			{
				return false;
			}
			if (value2 == null)
			{
				return true;
			}
			int size = value1.Size;
			int size2 = value2.Size;
			if (size != size2)
			{
				return size > size2;
			}
			while (size-- > 0)
			{
				if (value1.m_elements[size] != value2.m_elements[size])
				{
					return value1.m_elements[size] > value2.m_elements[size];
				}
			}
			return false;
		}

		// Token: 0x060028E1 RID: 10465 RVA: 0x000BAE8C File Offset: 0x000B908C
		public static bool operator ==(BigInt value1, BigInt value2)
		{
			if (value1 == null)
			{
				return value2 == null;
			}
			if (value2 == null)
			{
				return value1 == null;
			}
			int size = value1.Size;
			int size2 = value2.Size;
			if (size != size2)
			{
				return false;
			}
			for (int i = 0; i < size; i++)
			{
				if (value1.m_elements[i] != value2.m_elements[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060028E2 RID: 10466 RVA: 0x000BAEDE File Offset: 0x000B90DE
		public static bool operator !=(BigInt value1, BigInt value2)
		{
			return !(value1 == value2);
		}

		// Token: 0x060028E3 RID: 10467 RVA: 0x000BAEEA File Offset: 0x000B90EA
		public override bool Equals(object obj)
		{
			return obj is BigInt && this == (BigInt)obj;
		}

		// Token: 0x060028E4 RID: 10468 RVA: 0x000BAF04 File Offset: 0x000B9104
		public override int GetHashCode()
		{
			int num = 0;
			for (int i = 0; i < this.m_size; i++)
			{
				num += (int)this.GetDigit(i);
			}
			return num;
		}

		// Token: 0x060028E5 RID: 10469 RVA: 0x000BAF30 File Offset: 0x000B9130
		internal static void Add(BigInt a, byte b, ref BigInt c)
		{
			byte b2 = b;
			int size = a.Size;
			int num = 0;
			for (int i = 0; i < size; i++)
			{
				int num2 = (int)(a.GetDigit(i) + b2);
				c.SetDigit(i, (byte)(num2 & 255), ref num);
				b2 = (byte)((num2 >> 8) & 255);
			}
			if (b2 != 0)
			{
				c.SetDigit(a.Size, b2, ref num);
			}
			c.Size = num;
		}

		// Token: 0x060028E6 RID: 10470 RVA: 0x000BAFA0 File Offset: 0x000B91A0
		internal static void Negate(ref BigInt a)
		{
			int num = 0;
			for (int i = 0; i < 128; i++)
			{
				a.SetDigit(i, ~a.GetDigit(i) & byte.MaxValue, ref num);
			}
			for (int j = 0; j < 128; j++)
			{
				a.SetDigit(j, a.GetDigit(j) + 1, ref num);
				if ((a.GetDigit(j) & 255) != 0)
				{
					break;
				}
				a.SetDigit(j, a.GetDigit(j) & byte.MaxValue, ref num);
			}
			a.Size = num;
		}

		// Token: 0x060028E7 RID: 10471 RVA: 0x000BB030 File Offset: 0x000B9230
		internal static void Subtract(BigInt a, BigInt b, ref BigInt c)
		{
			byte b2 = 0;
			if (a < b)
			{
				BigInt.Subtract(b, a, ref c);
				BigInt.Negate(ref c);
				return;
			}
			int size = a.Size;
			int num = 0;
			for (int i = 0; i < size; i++)
			{
				int num2 = (int)(a.GetDigit(i) - b.GetDigit(i) - b2);
				b2 = 0;
				if (num2 < 0)
				{
					num2 += 256;
					b2 = 1;
				}
				c.SetDigit(i, (byte)(num2 & 255), ref num);
			}
			c.Size = num;
		}

		// Token: 0x060028E8 RID: 10472 RVA: 0x000BB0B0 File Offset: 0x000B92B0
		private void Multiply(int b)
		{
			if (b == 0)
			{
				this.Clear();
				return;
			}
			int num = 0;
			int size = this.Size;
			int num2 = 0;
			for (int i = 0; i < size; i++)
			{
				int num3 = b * (int)this.GetDigit(i) + num;
				num = num3 / 256;
				this.SetDigit(i, (byte)(num3 % 256), ref num2);
			}
			if (num != 0)
			{
				byte[] bytes = BitConverter.GetBytes(num);
				for (int j = 0; j < bytes.Length; j++)
				{
					this.SetDigit(size + j, bytes[j], ref num2);
				}
			}
			this.Size = num2;
		}

		// Token: 0x060028E9 RID: 10473 RVA: 0x000BB144 File Offset: 0x000B9344
		private static void Multiply(BigInt a, int b, ref BigInt c)
		{
			if (b == 0)
			{
				c.Clear();
				return;
			}
			int num = 0;
			int size = a.Size;
			int num2 = 0;
			for (int i = 0; i < size; i++)
			{
				int num3 = b * (int)a.GetDigit(i) + num;
				num = num3 / 256;
				c.SetDigit(i, (byte)(num3 % 256), ref num2);
			}
			if (num != 0)
			{
				byte[] bytes = BitConverter.GetBytes(num);
				for (int j = 0; j < bytes.Length; j++)
				{
					c.SetDigit(size + j, bytes[j], ref num2);
				}
			}
			c.Size = num2;
		}

		// Token: 0x060028EA RID: 10474 RVA: 0x000BB1DC File Offset: 0x000B93DC
		private void Divide(int b)
		{
			int num = 0;
			int size = this.Size;
			int num2 = 0;
			while (size-- > 0)
			{
				int num3 = 256 * num + (int)this.GetDigit(size);
				num = num3 % b;
				this.SetDigit(size, (byte)(num3 / b), ref num2);
			}
			this.Size = num2;
		}

		// Token: 0x060028EB RID: 10475 RVA: 0x000BB228 File Offset: 0x000B9428
		internal static void Divide(BigInt numerator, BigInt denominator, ref BigInt quotient, ref BigInt remainder)
		{
			if (numerator < denominator)
			{
				quotient.Clear();
				remainder.CopyFrom(numerator);
				return;
			}
			if (numerator == denominator)
			{
				quotient.Clear();
				quotient.SetDigit(0, 1);
				remainder.Clear();
				return;
			}
			BigInt bigInt = new BigInt();
			bigInt.CopyFrom(numerator);
			BigInt bigInt2 = new BigInt();
			bigInt2.CopyFrom(denominator);
			uint num = 0U;
			while (bigInt2.Size < bigInt.Size)
			{
				bigInt2.Multiply(256);
				num += 1U;
			}
			if (bigInt2 > bigInt)
			{
				bigInt2.Divide(256);
				num -= 1U;
			}
			BigInt bigInt3 = new BigInt();
			quotient.Clear();
			int num2 = 0;
			while ((long)num2 <= (long)((ulong)num))
			{
				int num3 = ((bigInt.Size == bigInt2.Size) ? ((int)bigInt.GetDigit(bigInt.Size - 1)) : (256 * (int)bigInt.GetDigit(bigInt.Size - 1) + (int)bigInt.GetDigit(bigInt.Size - 2)));
				int digit = (int)bigInt2.GetDigit(bigInt2.Size - 1);
				int num4 = num3 / digit;
				if (num4 >= 256)
				{
					num4 = 255;
				}
				BigInt.Multiply(bigInt2, num4, ref bigInt3);
				while (bigInt3 > bigInt)
				{
					num4--;
					BigInt.Multiply(bigInt2, num4, ref bigInt3);
				}
				quotient.Multiply(256);
				BigInt.Add(quotient, (byte)num4, ref quotient);
				BigInt.Subtract(bigInt, bigInt3, ref bigInt);
				bigInt2.Divide(256);
				num2++;
			}
			remainder.CopyFrom(bigInt);
		}

		// Token: 0x060028EC RID: 10476 RVA: 0x000BB3B2 File Offset: 0x000B95B2
		internal void CopyFrom(BigInt a)
		{
			Array.Copy(a.m_elements, this.m_elements, 128);
			this.m_size = a.m_size;
		}

		// Token: 0x060028ED RID: 10477 RVA: 0x000BB3D8 File Offset: 0x000B95D8
		internal bool IsZero()
		{
			for (int i = 0; i < this.m_size; i++)
			{
				if (this.m_elements[i] != 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060028EE RID: 10478 RVA: 0x000BB404 File Offset: 0x000B9604
		internal byte[] ToByteArray()
		{
			byte[] array = new byte[this.Size];
			Array.Copy(this.m_elements, array, this.Size);
			return array;
		}

		// Token: 0x060028EF RID: 10479 RVA: 0x000BB430 File Offset: 0x000B9630
		internal void Clear()
		{
			this.m_size = 0;
		}

		// Token: 0x060028F0 RID: 10480 RVA: 0x000BB43C File Offset: 0x000B963C
		internal void FromHexadecimal(string hexNum)
		{
			byte[] array = global::System.Security.Cryptography.X509Certificates.X509Utils.DecodeHexString(hexNum);
			Array.Reverse(array);
			int hexArraySize = global::System.Security.Cryptography.X509Certificates.X509Utils.GetHexArraySize(array);
			Array.Copy(array, this.m_elements, hexArraySize);
			this.Size = hexArraySize;
		}

		// Token: 0x060028F1 RID: 10481 RVA: 0x000BB474 File Offset: 0x000B9674
		internal void FromDecimal(string decNum)
		{
			BigInt bigInt = new BigInt();
			BigInt bigInt2 = new BigInt();
			int length = decNum.Length;
			for (int i = 0; i < length; i++)
			{
				if (decNum[i] <= '9' && decNum[i] >= '0')
				{
					BigInt.Multiply(bigInt, 10, ref bigInt2);
					BigInt.Add(bigInt2, (byte)(decNum[i] - '0'), ref bigInt);
				}
			}
			this.CopyFrom(bigInt);
		}

		// Token: 0x060028F2 RID: 10482 RVA: 0x000BB4DC File Offset: 0x000B96DC
		internal string ToDecimal()
		{
			if (this.IsZero())
			{
				return "0";
			}
			BigInt bigInt = new BigInt(10);
			BigInt bigInt2 = new BigInt();
			BigInt bigInt3 = new BigInt();
			BigInt bigInt4 = new BigInt();
			bigInt2.CopyFrom(this);
			char[] array = new char[(int)Math.Ceiling((double)(this.m_size * 2) * 1.21)];
			int num = 0;
			do
			{
				BigInt.Divide(bigInt2, bigInt, ref bigInt3, ref bigInt4);
				array[num++] = BigInt.decValues[(int)(bigInt4.IsZero() ? 0 : bigInt4.m_elements[0])];
				bigInt2.CopyFrom(bigInt3);
			}
			while (!bigInt3.IsZero());
			Array.Reverse(array, 0, num);
			return new string(array, 0, num);
		}

		// Token: 0x04002279 RID: 8825
		private byte[] m_elements;

		// Token: 0x0400227A RID: 8826
		private const int m_maxbytes = 128;

		// Token: 0x0400227B RID: 8827
		private const int m_base = 256;

		// Token: 0x0400227C RID: 8828
		private int m_size;

		// Token: 0x0400227D RID: 8829
		private static readonly char[] decValues = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
	}
}
