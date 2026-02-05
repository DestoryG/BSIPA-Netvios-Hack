using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace I18N.Common
{
	// Token: 0x02000008 RID: 8
	[Serializable]
	public abstract class MonoEncoding : Encoding
	{
		// Token: 0x06000048 RID: 72 RVA: 0x00003EE1 File Offset: 0x000020E1
		public MonoEncoding(int codePage)
			: this(codePage, 0)
		{
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003EEB File Offset: 0x000020EB
		public MonoEncoding(int codePage, int windowsCodePage)
			: base(codePage)
		{
			this.win_code_page = windowsCodePage;
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00003EFB File Offset: 0x000020FB
		public override int WindowsCodePage
		{
			get
			{
				if (this.win_code_page == 0)
				{
					return base.WindowsCodePage;
				}
				return this.win_code_page;
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003F12 File Offset: 0x00002112
		protected unsafe virtual int GetBytesInternal(char* chars, int charCount, byte* bytes, int byteCount, bool flush, object state)
		{
			throw new NotImplementedException("Statefull encoding is not implemented (yet?) by this encoding class.");
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003F20 File Offset: 0x00002120
		public unsafe void HandleFallback(ref EncoderFallbackBuffer buffer, char* chars, ref int charIndex, ref int charCount, byte* bytes, ref int byteIndex, ref int byteCount, object state)
		{
			if (buffer == null)
			{
				buffer = base.EncoderFallback.CreateFallbackBuffer();
			}
			if (charCount > 1 && char.IsSurrogate(chars[charIndex]) && char.IsSurrogate(chars[charIndex + 1]))
			{
				buffer.Fallback(chars[charIndex], chars[charIndex + 1], charIndex);
				charIndex++;
				charCount--;
			}
			else
			{
				buffer.Fallback(chars[charIndex], charIndex);
			}
			char[] array = new char[buffer.Remaining];
			int num = 0;
			while (buffer.Remaining > 0)
			{
				array[num++] = buffer.GetNextChar();
			}
			char[] array2;
			char* ptr;
			if ((array2 = array) == null || array2.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array2[0];
			}
			byte* ptr2 = ((bytes == null) ? null : (bytes + byteIndex));
			int num2 = ((state == null) ? this.GetBytes(ptr, array.Length, ptr2, byteCount) : this.GetBytesInternal(ptr, array.Length, ptr2, byteCount, true, state));
			byteIndex += num2;
			byteCount -= num2;
			array2 = null;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00004034 File Offset: 0x00002234
		public unsafe void HandleFallback(ref EncoderFallbackBuffer buffer, char* chars, ref int charIndex, ref int charCount, byte* bytes, ref int byteIndex, ref int byteCount)
		{
			this.HandleFallback(ref buffer, chars, ref charIndex, ref charCount, bytes, ref byteIndex, ref byteCount, null);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00004054 File Offset: 0x00002254
		public unsafe override int GetByteCount(char[] chars, int index, int count)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars");
			}
			if (index < 0 || index > chars.Length)
			{
				throw new ArgumentOutOfRangeException("index", Strings.GetString("ArgRange_Array"));
			}
			if (count < 0 || count > chars.Length - index)
			{
				throw new ArgumentOutOfRangeException("count", Strings.GetString("ArgRange_Array"));
			}
			if (count == 0)
			{
				return 0;
			}
			char* ptr;
			if (chars == null || chars.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &chars[0];
			}
			return this.GetByteCountImpl(ptr + index, count);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000040DC File Offset: 0x000022DC
		public unsafe override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
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
				throw new ArgumentException(Strings.GetString("Arg_InsufficientSpace"), "bytes");
			}
			if (charCount == 0)
			{
				return 0;
			}
			char* ptr;
			if (chars == null || chars.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &chars[0];
			}
			byte* ptr2;
			if (bytes == null || bytes.Length == 0)
			{
				ptr2 = null;
			}
			else
			{
				ptr2 = &bytes[0];
			}
			return this.GetBytesImpl(ptr + charIndex, charCount, ptr2 + byteIndex, bytes.Length - byteIndex);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000041D8 File Offset: 0x000023D8
		public unsafe override int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex)
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
				throw new ArgumentException(Strings.GetString("Arg_InsufficientSpace"), "bytes");
			}
			if (charCount == 0 || bytes.Length == byteIndex)
			{
				return 0;
			}
			char* ptr = s;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			byte* ptr2;
			if (bytes == null || bytes.Length == 0)
			{
				ptr2 = null;
			}
			else
			{
				ptr2 = &bytes[0];
			}
			return this.GetBytesImpl(ptr + charIndex, charCount, ptr2 + byteIndex, bytes.Length - byteIndex);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000042D7 File Offset: 0x000024D7
		public unsafe override int GetByteCount(char* chars, int count)
		{
			return this.GetByteCountImpl(chars, count);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000042E1 File Offset: 0x000024E1
		public unsafe override int GetBytes(char* chars, int charCount, byte* bytes, int byteCount)
		{
			return this.GetBytesImpl(chars, charCount, bytes, byteCount);
		}

		// Token: 0x06000053 RID: 83
		public unsafe abstract int GetByteCountImpl(char* chars, int charCount);

		// Token: 0x06000054 RID: 84
		public unsafe abstract int GetBytesImpl(char* chars, int charCount, byte* bytes, int byteCount);

		// Token: 0x06000055 RID: 85 RVA: 0x000042EE File Offset: 0x000024EE
		public override Encoder GetEncoder()
		{
			return new MonoEncodingDefaultEncoder(this);
		}

		// Token: 0x0400004F RID: 79
		private readonly int win_code_page;
	}
}
