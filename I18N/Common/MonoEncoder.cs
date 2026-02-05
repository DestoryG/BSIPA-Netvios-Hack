using System;
using System.Text;

namespace I18N.Common
{
	// Token: 0x02000009 RID: 9
	public abstract class MonoEncoder : Encoder
	{
		// Token: 0x06000056 RID: 86 RVA: 0x000042F6 File Offset: 0x000024F6
		public MonoEncoder(MonoEncoding encoding)
		{
			this.encoding = encoding;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00004308 File Offset: 0x00002508
		public unsafe override int GetByteCount(char[] chars, int index, int count, bool refresh)
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
			return this.GetByteCountImpl(ptr + index, count, refresh);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00004390 File Offset: 0x00002590
		public unsafe override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex, bool flush)
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
			return this.GetBytesImpl(ptr + charIndex, charCount, ptr2 + byteIndex, bytes.Length - byteIndex, flush);
		}

		// Token: 0x06000059 RID: 89
		public unsafe abstract int GetByteCountImpl(char* chars, int charCount, bool refresh);

		// Token: 0x0600005A RID: 90
		public unsafe abstract int GetBytesImpl(char* chars, int charCount, byte* bytes, int byteCount, bool refresh);

		// Token: 0x0600005B RID: 91 RVA: 0x0000448B File Offset: 0x0000268B
		public unsafe override int GetBytes(char* chars, int charCount, byte* bytes, int byteCount, bool flush)
		{
			return this.GetBytesImpl(chars, charCount, bytes, byteCount, flush);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x0000449C File Offset: 0x0000269C
		public unsafe void HandleFallback(char* chars, ref int charIndex, ref int charCount, byte* bytes, ref int byteIndex, ref int byteCount, object state)
		{
			EncoderFallbackBuffer fallbackBuffer = base.FallbackBuffer;
			this.encoding.HandleFallback(ref fallbackBuffer, chars, ref charIndex, ref charCount, bytes, ref byteIndex, ref byteCount, state);
		}

		// Token: 0x04000050 RID: 80
		private MonoEncoding encoding;
	}
}
