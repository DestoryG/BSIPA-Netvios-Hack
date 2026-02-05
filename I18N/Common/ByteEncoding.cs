using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace I18N.Common
{
	// Token: 0x02000003 RID: 3
	[Serializable]
	public abstract class ByteEncoding : MonoEncoding
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		protected ByteEncoding(int codePage, char[] toChars, string encodingName, string bodyName, string headerName, string webName, bool isBrowserDisplay, bool isBrowserSave, bool isMailNewsDisplay, bool isMailNewsSave, int windowsCodePage)
			: base(codePage)
		{
			if (toChars.Length != 256)
			{
				throw new ArgumentException("toChars");
			}
			this.toChars = toChars;
			this.encodingName = encodingName;
			this.bodyName = bodyName;
			this.headerName = headerName;
			this.webName = webName;
			this.isBrowserDisplay = isBrowserDisplay;
			this.isBrowserSave = isBrowserSave;
			this.isMailNewsDisplay = isMailNewsDisplay;
			this.isMailNewsSave = isMailNewsSave;
			this.windowsCodePage = windowsCodePage;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020C8 File Offset: 0x000002C8
		public override bool IsAlwaysNormalized(NormalizationForm form)
		{
			if (form != NormalizationForm.FormC)
			{
				return false;
			}
			if (ByteEncoding.isNormalized == null)
			{
				ByteEncoding.isNormalized = new byte[8192];
			}
			if (ByteEncoding.isNormalizedComputed == null)
			{
				ByteEncoding.isNormalizedComputed = new byte[8192];
			}
			if (ByteEncoding.normalization_bytes == null)
			{
				ByteEncoding.normalization_bytes = new byte[256];
				byte[] array = ByteEncoding.normalization_bytes;
				lock (array)
				{
					for (int i = 0; i < 256; i++)
					{
						ByteEncoding.normalization_bytes[i] = (byte)i;
					}
				}
			}
			byte b = (byte)(1 << this.CodePage % 8);
			if ((ByteEncoding.isNormalizedComputed[this.CodePage / 8] & b) == 0)
			{
				Encoding encoding = this.Clone() as Encoding;
				encoding.DecoderFallback = new DecoderReplacementFallback("");
				string @string = encoding.GetString(ByteEncoding.normalization_bytes);
				if (@string != @string.Normalize(form))
				{
					byte[] array2 = ByteEncoding.isNormalized;
					int num = this.CodePage / 8;
					array2[num] |= b;
				}
				byte[] array3 = ByteEncoding.isNormalizedComputed;
				int num2 = this.CodePage / 8;
				array3[num2] |= b;
			}
			return (ByteEncoding.isNormalized[this.CodePage / 8] & b) == 0;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x000021FC File Offset: 0x000003FC
		public override bool IsSingleByte
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000021FF File Offset: 0x000003FF
		public override int GetByteCount(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			return s.Length;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002215 File Offset: 0x00000415
		public unsafe override int GetByteCountImpl(char* chars, int count)
		{
			return count;
		}

		// Token: 0x06000006 RID: 6
		protected unsafe abstract void ToBytes(char* chars, int charCount, byte* bytes, int byteCount);

		// Token: 0x06000007 RID: 7 RVA: 0x00002218 File Offset: 0x00000418
		protected unsafe virtual void ToBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			if (charCount == 0 || bytes.Length == byteIndex)
			{
				return;
			}
			if (charIndex < 0 || charIndex > chars.Length)
			{
				throw new ArgumentOutOfRangeException("charIndex", Strings.GetString("ArgRange_Array"));
			}
			if (byteIndex < 0 || byteIndex > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("byteIndex", Strings.GetString("ArgRange_Array"));
			}
			if (charCount < 0 || charIndex + charCount > chars.Length || byteIndex + charCount > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("charCount", Strings.GetString("ArgRange_Array"));
			}
			fixed (char[] array = chars)
			{
				char* ptr;
				if (chars == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				fixed (byte[] array2 = bytes)
				{
					byte* ptr2;
					if (bytes == null || array2.Length == 0)
					{
						ptr2 = null;
					}
					else
					{
						ptr2 = &array2[0];
					}
					this.ToBytes(ptr + charIndex, charCount, ptr2 + byteIndex, bytes.Length - byteIndex);
				}
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000022EA File Offset: 0x000004EA
		public unsafe override int GetBytesImpl(char* chars, int charCount, byte* bytes, int byteCount)
		{
			this.ToBytes(chars, charCount, bytes, byteCount);
			return charCount;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000022F8 File Offset: 0x000004F8
		public override int GetCharCount(byte[] bytes, int index, int count)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (index < 0 || index > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("index", Strings.GetString("ArgRange_Array"));
			}
			if (count < 0 || count > bytes.Length - index)
			{
				throw new ArgumentOutOfRangeException("count", Strings.GetString("ArgRange_Array"));
			}
			return count;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002354 File Offset: 0x00000554
		public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (chars == null)
			{
				throw new ArgumentNullException("chars");
			}
			if (byteIndex < 0 || byteIndex > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("byteIndex", Strings.GetString("ArgRange_Array"));
			}
			if (byteCount < 0 || byteCount > bytes.Length - byteIndex)
			{
				throw new ArgumentOutOfRangeException("byteCount", Strings.GetString("ArgRange_Array"));
			}
			if (charIndex < 0 || charIndex > chars.Length)
			{
				throw new ArgumentOutOfRangeException("charIndex", Strings.GetString("ArgRange_Array"));
			}
			if (chars.Length - charIndex < byteCount)
			{
				throw new ArgumentException(Strings.GetString("Arg_InsufficientSpace"));
			}
			int num = byteCount;
			char[] array = this.toChars;
			while (num-- > 0)
			{
				chars[charIndex++] = array[(int)bytes[byteIndex++]];
			}
			return byteCount;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002422 File Offset: 0x00000622
		public override int GetMaxByteCount(int charCount)
		{
			if (charCount < 0)
			{
				throw new ArgumentOutOfRangeException("charCount", Strings.GetString("ArgRange_NonNegative"));
			}
			return charCount;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000243E File Offset: 0x0000063E
		public override int GetMaxCharCount(int byteCount)
		{
			if (byteCount < 0)
			{
				throw new ArgumentOutOfRangeException("byteCount", Strings.GetString("ArgRange_NonNegative"));
			}
			return byteCount;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000245C File Offset: 0x0000065C
		public unsafe override string GetString(byte[] bytes, int index, int count)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (index < 0 || index > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("index", Strings.GetString("ArgRange_Array"));
			}
			if (count < 0 || count > bytes.Length - index)
			{
				throw new ArgumentOutOfRangeException("count", Strings.GetString("ArgRange_Array"));
			}
			if (count == 0)
			{
				return string.Empty;
			}
			string text = new string('\0', count);
			fixed (byte[] array = bytes)
			{
				byte* ptr;
				if (bytes == null || array.Length == 0)
				{
					ptr = null;
				}
				else
				{
					ptr = &array[0];
				}
				fixed (string text2 = text)
				{
					char* ptr2 = text2;
					if (ptr2 != null)
					{
						ptr2 += RuntimeHelpers.OffsetToStringData / 2;
					}
					char[] array2;
					char* ptr3;
					if ((array2 = this.toChars) == null || array2.Length == 0)
					{
						ptr3 = null;
					}
					else
					{
						ptr3 = &array2[0];
					}
					byte* ptr4 = ptr + index;
					char* ptr5 = ptr2;
					while (count-- != 0)
					{
						*(ptr5++) = ptr3[(IntPtr)(*(ptr4++)) * 2];
					}
					array2 = null;
				}
			}
			return text;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002546 File Offset: 0x00000746
		public override string GetString(byte[] bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			return this.GetString(bytes, 0, bytes.Length);
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000F RID: 15 RVA: 0x00002561 File Offset: 0x00000761
		public override string BodyName
		{
			get
			{
				return this.bodyName;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002569 File Offset: 0x00000769
		public override string EncodingName
		{
			get
			{
				return this.encodingName;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000011 RID: 17 RVA: 0x00002571 File Offset: 0x00000771
		public override string HeaderName
		{
			get
			{
				return this.headerName;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000012 RID: 18 RVA: 0x00002579 File Offset: 0x00000779
		public override bool IsBrowserDisplay
		{
			get
			{
				return this.isBrowserDisplay;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000013 RID: 19 RVA: 0x00002581 File Offset: 0x00000781
		public override bool IsBrowserSave
		{
			get
			{
				return this.isBrowserSave;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00002589 File Offset: 0x00000789
		public override bool IsMailNewsDisplay
		{
			get
			{
				return this.isMailNewsDisplay;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000015 RID: 21 RVA: 0x00002591 File Offset: 0x00000791
		public override bool IsMailNewsSave
		{
			get
			{
				return this.isMailNewsSave;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002599 File Offset: 0x00000799
		public override string WebName
		{
			get
			{
				return this.webName;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000017 RID: 23 RVA: 0x000025A1 File Offset: 0x000007A1
		public override int WindowsCodePage
		{
			get
			{
				return this.windowsCodePage;
			}
		}

		// Token: 0x0400002A RID: 42
		protected char[] toChars;

		// Token: 0x0400002B RID: 43
		protected string encodingName;

		// Token: 0x0400002C RID: 44
		protected string bodyName;

		// Token: 0x0400002D RID: 45
		protected string headerName;

		// Token: 0x0400002E RID: 46
		protected string webName;

		// Token: 0x0400002F RID: 47
		protected bool isBrowserDisplay;

		// Token: 0x04000030 RID: 48
		protected bool isBrowserSave;

		// Token: 0x04000031 RID: 49
		protected bool isMailNewsDisplay;

		// Token: 0x04000032 RID: 50
		protected bool isMailNewsSave;

		// Token: 0x04000033 RID: 51
		protected int windowsCodePage;

		// Token: 0x04000034 RID: 52
		private static byte[] isNormalized;

		// Token: 0x04000035 RID: 53
		private static byte[] isNormalizedComputed;

		// Token: 0x04000036 RID: 54
		private static byte[] normalization_bytes;
	}
}
