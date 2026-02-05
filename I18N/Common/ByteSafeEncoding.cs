using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace I18N.Common
{
	// Token: 0x02000004 RID: 4
	[Serializable]
	public abstract class ByteSafeEncoding : MonoSafeEncoding
	{
		// Token: 0x06000018 RID: 24 RVA: 0x000025AC File Offset: 0x000007AC
		protected ByteSafeEncoding(int codePage, char[] toChars, string encodingName, string bodyName, string headerName, string webName, bool isBrowserDisplay, bool isBrowserSave, bool isMailNewsDisplay, bool isMailNewsSave, int windowsCodePage)
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

		// Token: 0x06000019 RID: 25 RVA: 0x00002624 File Offset: 0x00000824
		public override bool IsAlwaysNormalized(NormalizationForm form)
		{
			if (form != NormalizationForm.FormC)
			{
				return false;
			}
			if (ByteSafeEncoding.isNormalized == null)
			{
				ByteSafeEncoding.isNormalized = new byte[8192];
			}
			if (ByteSafeEncoding.isNormalizedComputed == null)
			{
				ByteSafeEncoding.isNormalizedComputed = new byte[8192];
			}
			if (ByteSafeEncoding.normalization_bytes == null)
			{
				ByteSafeEncoding.normalization_bytes = new byte[256];
				byte[] array = ByteSafeEncoding.normalization_bytes;
				lock (array)
				{
					for (int i = 0; i < 256; i++)
					{
						ByteSafeEncoding.normalization_bytes[i] = (byte)i;
					}
				}
			}
			byte b = (byte)(1 << this.CodePage % 8);
			if ((ByteSafeEncoding.isNormalizedComputed[this.CodePage / 8] & b) == 0)
			{
				Encoding encoding = this.Clone() as Encoding;
				encoding.DecoderFallback = new DecoderReplacementFallback("");
				string @string = encoding.GetString(ByteSafeEncoding.normalization_bytes);
				if (@string != @string.Normalize(form))
				{
					byte[] array2 = ByteSafeEncoding.isNormalized;
					int num = this.CodePage / 8;
					array2[num] |= b;
				}
				byte[] array3 = ByteSafeEncoding.isNormalizedComputed;
				int num2 = this.CodePage / 8;
				array3[num2] |= b;
			}
			return (ByteSafeEncoding.isNormalized[this.CodePage / 8] & b) == 0;
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00002758 File Offset: 0x00000958
		public override bool IsSingleByte
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600001B RID: 27 RVA: 0x0000275B File Offset: 0x0000095B
		public override int GetByteCount(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			return s.Length;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002771 File Offset: 0x00000971
		public override int GetByteCount(char[] chars, int index, int count)
		{
			return count - index;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002776 File Offset: 0x00000976
		public unsafe override int GetByteCount(char* chars, int count)
		{
			return count;
		}

		// Token: 0x0600001E RID: 30
		protected abstract void ToBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex);

		// Token: 0x0600001F RID: 31 RVA: 0x00002779 File Offset: 0x00000979
		protected virtual void ToBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			if (s.Length == 0 || bytes.Length == byteIndex)
			{
				return;
			}
			this.ToBytes(s.ToCharArray(), charIndex, charCount, bytes, byteIndex);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000027A0 File Offset: 0x000009A0
		public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars");
			}
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (charIndex < 0 || charIndex > chars.Length)
			{
				throw new ArgumentOutOfRangeException("charIndex", Strings.GetString("ArgRange_Array"));
			}
			if (charCount < 0 || charCount > chars.Length - charIndex)
			{
				throw new ArgumentOutOfRangeException("charCount", Strings.GetString("ArgRange_Array"));
			}
			if (byteIndex < 0 || byteIndex > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("byteIndex", Strings.GetString("ArgRange_Array"));
			}
			if (bytes.Length - byteIndex < charCount)
			{
				throw new ArgumentException(Strings.GetString("Arg_InsufficientSpace"));
			}
			this.ToBytes(chars, charIndex, charCount, bytes, byteIndex);
			return charCount;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002854 File Offset: 0x00000A54
		public override int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (charIndex < 0 || charIndex > s.Length)
			{
				throw new ArgumentOutOfRangeException("charIndex", Strings.GetString("ArgRange_StringIndex"));
			}
			if (charCount < 0 || charCount > s.Length - charIndex)
			{
				throw new ArgumentOutOfRangeException("charCount", Strings.GetString("ArgRange_StringRange"));
			}
			if (byteIndex < 0 || byteIndex > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("byteIndex", Strings.GetString("ArgRange_Array"));
			}
			if (bytes.Length - byteIndex < charCount)
			{
				throw new ArgumentException(Strings.GetString("Arg_InsufficientSpace"));
			}
			this.ToBytes(s, charIndex, charCount, bytes, byteIndex);
			return charCount;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002910 File Offset: 0x00000B10
		public override byte[] GetBytes(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			char[] array = s.ToCharArray();
			return this.GetBytes(array, 0, array.Length);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002940 File Offset: 0x00000B40
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

		// Token: 0x06000024 RID: 36 RVA: 0x0000299C File Offset: 0x00000B9C
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

		// Token: 0x06000025 RID: 37 RVA: 0x00002A6A File Offset: 0x00000C6A
		public override int GetMaxByteCount(int charCount)
		{
			if (charCount < 0)
			{
				throw new ArgumentOutOfRangeException("charCount", Strings.GetString("ArgRange_NonNegative"));
			}
			return charCount;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002A86 File Offset: 0x00000C86
		public override int GetMaxCharCount(int byteCount)
		{
			if (byteCount < 0)
			{
				throw new ArgumentOutOfRangeException("byteCount", Strings.GetString("ArgRange_NonNegative"));
			}
			return byteCount;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002AA4 File Offset: 0x00000CA4
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

		// Token: 0x06000028 RID: 40 RVA: 0x00002B8E File Offset: 0x00000D8E
		public override string GetString(byte[] bytes)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			return this.GetString(bytes, 0, bytes.Length);
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000029 RID: 41 RVA: 0x00002BA9 File Offset: 0x00000DA9
		public override string BodyName
		{
			get
			{
				return this.bodyName;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002BB1 File Offset: 0x00000DB1
		public override string EncodingName
		{
			get
			{
				return this.encodingName;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002BB9 File Offset: 0x00000DB9
		public override string HeaderName
		{
			get
			{
				return this.headerName;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00002BC1 File Offset: 0x00000DC1
		public override bool IsBrowserDisplay
		{
			get
			{
				return this.isBrowserDisplay;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002BC9 File Offset: 0x00000DC9
		public override bool IsBrowserSave
		{
			get
			{
				return this.isBrowserSave;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002BD1 File Offset: 0x00000DD1
		public override bool IsMailNewsDisplay
		{
			get
			{
				return this.isMailNewsDisplay;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002BD9 File Offset: 0x00000DD9
		public override bool IsMailNewsSave
		{
			get
			{
				return this.isMailNewsSave;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00002BE1 File Offset: 0x00000DE1
		public override string WebName
		{
			get
			{
				return this.webName;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000031 RID: 49 RVA: 0x00002BE9 File Offset: 0x00000DE9
		public override int WindowsCodePage
		{
			get
			{
				return this.windowsCodePage;
			}
		}

		// Token: 0x04000037 RID: 55
		protected char[] toChars;

		// Token: 0x04000038 RID: 56
		protected string encodingName;

		// Token: 0x04000039 RID: 57
		protected string bodyName;

		// Token: 0x0400003A RID: 58
		protected string headerName;

		// Token: 0x0400003B RID: 59
		protected string webName;

		// Token: 0x0400003C RID: 60
		protected bool isBrowserDisplay;

		// Token: 0x0400003D RID: 61
		protected bool isBrowserSave;

		// Token: 0x0400003E RID: 62
		protected bool isMailNewsDisplay;

		// Token: 0x0400003F RID: 63
		protected bool isMailNewsSave;

		// Token: 0x04000040 RID: 64
		protected int windowsCodePage;

		// Token: 0x04000041 RID: 65
		private static byte[] isNormalized;

		// Token: 0x04000042 RID: 66
		private static byte[] isNormalizedComputed;

		// Token: 0x04000043 RID: 67
		private static byte[] normalization_bytes;
	}
}
