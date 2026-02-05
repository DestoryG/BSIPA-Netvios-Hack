using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security;

namespace System.Xml
{
	// Token: 0x02000022 RID: 34
	public class UniqueId
	{
		// Token: 0x060000CD RID: 205 RVA: 0x0000409E File Offset: 0x0000229E
		public UniqueId()
			: this(Guid.NewGuid())
		{
		}

		// Token: 0x060000CE RID: 206 RVA: 0x000040AB File Offset: 0x000022AB
		public UniqueId(Guid guid)
			: this(guid.ToByteArray())
		{
		}

		// Token: 0x060000CF RID: 207 RVA: 0x000040BA File Offset: 0x000022BA
		public UniqueId(byte[] guid)
			: this(guid, 0)
		{
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x000040C4 File Offset: 0x000022C4
		[SecuritySafeCritical]
		public unsafe UniqueId(byte[] guid, int offset)
		{
			if (guid == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("guid"));
			}
			if (offset < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (offset > guid.Length)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", global::System.Runtime.Serialization.SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", new object[] { guid.Length })));
			}
			if (16 > guid.Length - offset)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentException(global::System.Runtime.Serialization.SR.GetString("Array too small.  Length of available data must be at least {0}.", new object[] { 16 }), "guid"));
			}
			fixed (byte* ptr = &guid[offset])
			{
				byte* ptr2 = ptr;
				this.idLow = this.UnsafeGetInt64(ptr2);
				this.idHigh = this.UnsafeGetInt64(ptr2 + 8);
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00004198 File Offset: 0x00002398
		[SecuritySafeCritical]
		public unsafe UniqueId(string value)
		{
			if (value == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("value");
			}
			if (value.Length == 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(global::System.Runtime.Serialization.SR.GetString("UniqueId cannot be zero length.")));
			}
			fixed (string text = value)
			{
				char* ptr = text;
				if (ptr != null)
				{
					ptr += RuntimeHelpers.OffsetToStringData / 2;
				}
				this.UnsafeParse(ptr, value.Length);
			}
			this.s = value;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000041FC File Offset: 0x000023FC
		[SecuritySafeCritical]
		public unsafe UniqueId(char[] chars, int offset, int count)
		{
			if (chars == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("chars"));
			}
			if (offset < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (offset > chars.Length)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", global::System.Runtime.Serialization.SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", new object[] { chars.Length })));
			}
			if (count < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (count > chars.Length - offset)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("count", global::System.Runtime.Serialization.SR.GetString("The specified size exceeds the remaining buffer space ({0} bytes).", new object[] { chars.Length - offset })));
			}
			if (count == 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(global::System.Runtime.Serialization.SR.GetString("UniqueId cannot be zero length.")));
			}
			fixed (char* ptr = &chars[offset])
			{
				char* ptr2 = ptr;
				this.UnsafeParse(ptr2, count);
			}
			if (!this.IsGuid)
			{
				this.s = new string(chars, offset, count);
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00004308 File Offset: 0x00002508
		public int CharArrayLength
		{
			[SecuritySafeCritical]
			get
			{
				if (this.s != null)
				{
					return this.s.Length;
				}
				return 45;
			}
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00004320 File Offset: 0x00002520
		[SecurityCritical]
		private unsafe int UnsafeDecode(short* char2val, char ch1, char ch2)
		{
			if ((ch1 | ch2) >= '\u0080')
			{
				return 256;
			}
			return (int)(char2val[(IntPtr)ch1] | char2val[(IntPtr)('\u0080' + ch2)]);
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00004347 File Offset: 0x00002547
		[SecurityCritical]
		private unsafe void UnsafeEncode(char* val2char, byte b, char* pch)
		{
			*pch = val2char[b >> 4];
			pch[1] = val2char[b & 15];
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00004362 File Offset: 0x00002562
		public bool IsGuid
		{
			get
			{
				return (this.idLow | this.idHigh) != 0L;
			}
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00004378 File Offset: 0x00002578
		[SecurityCritical]
		private unsafe void UnsafeParse(char* chars, int charCount)
		{
			if (charCount != 45 || *chars != 'u' || chars[1] != 'r' || chars[2] != 'n' || chars[3] != ':' || chars[4] != 'u' || chars[5] != 'u' || chars[6] != 'i' || chars[7] != 'd' || chars[8] != ':' || chars[17] != '-' || chars[22] != '-' || chars[27] != '-' || chars[32] != '-')
			{
				return;
			}
			byte* ptr = stackalloc byte[(UIntPtr)16];
			int num = 0;
			short[] array;
			short* ptr2;
			if ((array = UniqueId.char2val) == null || array.Length == 0)
			{
				ptr2 = null;
			}
			else
			{
				ptr2 = &array[0];
			}
			short* ptr3 = ptr2;
			int num2 = this.UnsafeDecode(ptr3, chars[15], chars[16]);
			*ptr = (byte)num2;
			int num3 = num | num2;
			num2 = this.UnsafeDecode(ptr3, chars[13], chars[14]);
			ptr[1] = (byte)num2;
			int num4 = num3 | num2;
			num2 = this.UnsafeDecode(ptr3, chars[11], chars[12]);
			ptr[2] = (byte)num2;
			int num5 = num4 | num2;
			num2 = this.UnsafeDecode(ptr3, chars[9], chars[10]);
			ptr[3] = (byte)num2;
			int num6 = num5 | num2;
			num2 = this.UnsafeDecode(ptr3, chars[20], chars[21]);
			ptr[4] = (byte)num2;
			int num7 = num6 | num2;
			num2 = this.UnsafeDecode(ptr3, chars[18], chars[19]);
			ptr[5] = (byte)num2;
			int num8 = num7 | num2;
			num2 = this.UnsafeDecode(ptr3, chars[25], chars[26]);
			ptr[6] = (byte)num2;
			int num9 = num8 | num2;
			num2 = this.UnsafeDecode(ptr3, chars[23], chars[24]);
			ptr[7] = (byte)num2;
			int num10 = num9 | num2;
			num2 = this.UnsafeDecode(ptr3, chars[28], chars[29]);
			ptr[8] = (byte)num2;
			int num11 = num10 | num2;
			num2 = this.UnsafeDecode(ptr3, chars[30], chars[31]);
			ptr[9] = (byte)num2;
			int num12 = num11 | num2;
			num2 = this.UnsafeDecode(ptr3, chars[33], chars[34]);
			ptr[10] = (byte)num2;
			int num13 = num12 | num2;
			num2 = this.UnsafeDecode(ptr3, chars[35], chars[36]);
			ptr[11] = (byte)num2;
			int num14 = num13 | num2;
			num2 = this.UnsafeDecode(ptr3, chars[37], chars[38]);
			ptr[12] = (byte)num2;
			int num15 = num14 | num2;
			num2 = this.UnsafeDecode(ptr3, chars[39], chars[40]);
			ptr[13] = (byte)num2;
			int num16 = num15 | num2;
			num2 = this.UnsafeDecode(ptr3, chars[41], chars[42]);
			ptr[14] = (byte)num2;
			int num17 = num16 | num2;
			num2 = this.UnsafeDecode(ptr3, chars[43], chars[44]);
			ptr[15] = (byte)num2;
			if ((num17 | num2) >= 256)
			{
				return;
			}
			this.idLow = this.UnsafeGetInt64(ptr);
			this.idHigh = this.UnsafeGetInt64(ptr + 8);
			array = null;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00004680 File Offset: 0x00002880
		[SecuritySafeCritical]
		public unsafe int ToCharArray(char[] chars, int offset)
		{
			int charArrayLength = this.CharArrayLength;
			if (chars == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("chars"));
			}
			if (offset < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (offset > chars.Length)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", global::System.Runtime.Serialization.SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", new object[] { chars.Length })));
			}
			if (charArrayLength > chars.Length - offset)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("chars", global::System.Runtime.Serialization.SR.GetString("Array too small.  Must be able to hold at least {0}.", new object[] { charArrayLength })));
			}
			if (this.s != null)
			{
				this.s.CopyTo(0, chars, offset, charArrayLength);
			}
			else
			{
				byte* ptr = stackalloc byte[(UIntPtr)16];
				this.UnsafeSetInt64(this.idLow, ptr);
				this.UnsafeSetInt64(this.idHigh, ptr + 8);
				fixed (char* ptr2 = &chars[offset])
				{
					char* ptr3 = ptr2;
					*ptr3 = 'u';
					ptr3[1] = 'r';
					ptr3[2] = 'n';
					ptr3[3] = ':';
					ptr3[4] = 'u';
					ptr3[5] = 'u';
					ptr3[6] = 'i';
					ptr3[7] = 'd';
					ptr3[8] = ':';
					ptr3[17] = '-';
					ptr3[22] = '-';
					ptr3[27] = '-';
					ptr3[32] = '-';
					fixed (string text = "0123456789abcdef")
					{
						char* ptr4 = text;
						if (ptr4 != null)
						{
							ptr4 += RuntimeHelpers.OffsetToStringData / 2;
						}
						char* ptr5 = ptr4;
						this.UnsafeEncode(ptr5, *ptr, ptr3 + 15);
						this.UnsafeEncode(ptr5, ptr[1], ptr3 + 13);
						this.UnsafeEncode(ptr5, ptr[2], ptr3 + 11);
						this.UnsafeEncode(ptr5, ptr[3], ptr3 + 9);
						this.UnsafeEncode(ptr5, ptr[4], ptr3 + 20);
						this.UnsafeEncode(ptr5, ptr[5], ptr3 + 18);
						this.UnsafeEncode(ptr5, ptr[6], ptr3 + 25);
						this.UnsafeEncode(ptr5, ptr[7], ptr3 + 23);
						this.UnsafeEncode(ptr5, ptr[8], ptr3 + 28);
						this.UnsafeEncode(ptr5, ptr[9], ptr3 + 30);
						this.UnsafeEncode(ptr5, ptr[10], ptr3 + 33);
						this.UnsafeEncode(ptr5, ptr[11], ptr3 + 35);
						this.UnsafeEncode(ptr5, ptr[12], ptr3 + 37);
						this.UnsafeEncode(ptr5, ptr[13], ptr3 + 39);
						this.UnsafeEncode(ptr5, ptr[14], ptr3 + 41);
						this.UnsafeEncode(ptr5, ptr[15], ptr3 + 43);
					}
				}
			}
			return charArrayLength;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x0000494C File Offset: 0x00002B4C
		public bool TryGetGuid(out Guid guid)
		{
			byte[] array = new byte[16];
			if (!this.TryGetGuid(array, 0))
			{
				guid = Guid.Empty;
				return false;
			}
			guid = new Guid(array);
			return true;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00004988 File Offset: 0x00002B88
		[SecuritySafeCritical]
		public unsafe bool TryGetGuid(byte[] buffer, int offset)
		{
			if (!this.IsGuid)
			{
				return false;
			}
			if (buffer == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentNullException("buffer"));
			}
			if (offset < 0)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", global::System.Runtime.Serialization.SR.GetString("The value of this argument must be non-negative.")));
			}
			if (offset > buffer.Length)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("offset", global::System.Runtime.Serialization.SR.GetString("The specified offset exceeds the buffer size ({0} bytes).", new object[] { buffer.Length })));
			}
			if (16 > buffer.Length - offset)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new ArgumentOutOfRangeException("buffer", global::System.Runtime.Serialization.SR.GetString("Array too small.  Must be able to hold at least {0}.", new object[] { 16 })));
			}
			fixed (byte* ptr = &buffer[offset])
			{
				byte* ptr2 = ptr;
				this.UnsafeSetInt64(this.idLow, ptr2);
				this.UnsafeSetInt64(this.idHigh, ptr2 + 8);
			}
			return true;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00004A60 File Offset: 0x00002C60
		[SecuritySafeCritical]
		public override string ToString()
		{
			if (this.s == null)
			{
				int charArrayLength = this.CharArrayLength;
				char[] array = new char[charArrayLength];
				this.ToCharArray(array, 0);
				this.s = new string(array, 0, charArrayLength);
			}
			return this.s;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00004AA0 File Offset: 0x00002CA0
		public static bool operator ==(UniqueId id1, UniqueId id2)
		{
			if (id1 == null && id2 == null)
			{
				return true;
			}
			if (id1 == null || id2 == null)
			{
				return false;
			}
			if (id1.IsGuid && id2.IsGuid)
			{
				return id1.idLow == id2.idLow && id1.idHigh == id2.idHigh;
			}
			return id1.ToString() == id2.ToString();
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00004AFD File Offset: 0x00002CFD
		public static bool operator !=(UniqueId id1, UniqueId id2)
		{
			return !(id1 == id2);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00004B09 File Offset: 0x00002D09
		public override bool Equals(object obj)
		{
			return this == obj as UniqueId;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00004B18 File Offset: 0x00002D18
		public override int GetHashCode()
		{
			if (this.IsGuid)
			{
				long num = this.idLow ^ this.idHigh;
				return (int)(num >> 32) ^ (int)num;
			}
			return this.ToString().GetHashCode();
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00004B50 File Offset: 0x00002D50
		[SecurityCritical]
		private unsafe long UnsafeGetInt64(byte* pb)
		{
			int num = this.UnsafeGetInt32(pb);
			return ((long)this.UnsafeGetInt32(pb + 4) << 32) | (long)((ulong)num);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00004B76 File Offset: 0x00002D76
		[SecurityCritical]
		private unsafe int UnsafeGetInt32(byte* pb)
		{
			return ((((((int)pb[3] << 8) | (int)pb[2]) << 8) | (int)pb[1]) << 8) | (int)(*pb);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00004B8F File Offset: 0x00002D8F
		[SecurityCritical]
		private unsafe void UnsafeSetInt64(long value, byte* pb)
		{
			this.UnsafeSetInt32((int)value, pb);
			this.UnsafeSetInt32((int)(value >> 32), pb + 4);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00004BA9 File Offset: 0x00002DA9
		[SecurityCritical]
		private unsafe void UnsafeSetInt32(int value, byte* pb)
		{
			*pb = (byte)value;
			value >>= 8;
			pb[1] = (byte)value;
			value >>= 8;
			pb[2] = (byte)value;
			value >>= 8;
			pb[3] = (byte)value;
		}

		// Token: 0x0400005C RID: 92
		private long idLow;

		// Token: 0x0400005D RID: 93
		private long idHigh;

		// Token: 0x0400005E RID: 94
		[SecurityCritical]
		private string s;

		// Token: 0x0400005F RID: 95
		private const int guidLength = 16;

		// Token: 0x04000060 RID: 96
		private const int uuidLength = 45;

		// Token: 0x04000061 RID: 97
		private static short[] char2val = new short[]
		{
			256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
			256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
			256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
			256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
			256, 256, 256, 256, 256, 256, 256, 256, 0, 16,
			32, 48, 64, 80, 96, 112, 128, 144, 256, 256,
			256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
			256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
			256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
			256, 256, 256, 256, 256, 256, 256, 160, 176, 192,
			208, 224, 240, 256, 256, 256, 256, 256, 256, 256,
			256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
			256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
			256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
			256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
			256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
			256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
			256, 256, 256, 256, 256, 256, 0, 1, 2, 3,
			4, 5, 6, 7, 8, 9, 256, 256, 256, 256,
			256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
			256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
			256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
			256, 256, 256, 256, 256, 10, 11, 12, 13, 14,
			15, 256, 256, 256, 256, 256, 256, 256, 256, 256,
			256, 256, 256, 256, 256, 256, 256, 256, 256, 256,
			256, 256, 256, 256, 256, 256
		};

		// Token: 0x04000062 RID: 98
		private const string val2char = "0123456789abcdef";
	}
}
