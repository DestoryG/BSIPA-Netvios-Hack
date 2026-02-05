using System;
using System.Runtime.InteropServices;
using System.Text;

namespace I18N.Common
{
	// Token: 0x0200000A RID: 10
	public class MonoEncodingDefaultEncoder : ReferenceSourceDefaultEncoder
	{
		// Token: 0x0600005D RID: 93 RVA: 0x000044C8 File Offset: 0x000026C8
		public MonoEncodingDefaultEncoder(Encoding encoding)
			: base(encoding)
		{
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000044D4 File Offset: 0x000026D4
		[CLSCompliant(false)]
		[ComVisible(false)]
		public unsafe override void Convert(char* chars, int charCount, byte* bytes, int byteCount, bool flush, out int charsUsed, out int bytesUsed, out bool completed)
		{
			this.CheckArguments(chars, charCount, bytes, byteCount);
			charsUsed = charCount;
			for (;;)
			{
				bytesUsed = this.GetByteCount(chars, charsUsed, flush);
				if (bytesUsed <= byteCount)
				{
					break;
				}
				flush = false;
				charsUsed >>= 1;
			}
			completed = charsUsed == charCount;
			bytesUsed = this.GetBytes(chars, charsUsed, bytes, byteCount, flush);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00004530 File Offset: 0x00002730
		[ComVisible(false)]
		public override void Convert(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex, int byteCount, bool flush, out int charsUsed, out int bytesUsed, out bool completed)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars");
			}
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (charIndex < 0)
			{
				throw new ArgumentOutOfRangeException("charIndex");
			}
			if (charCount < 0 || chars.Length < charIndex + charCount)
			{
				throw new ArgumentOutOfRangeException("charCount");
			}
			if (byteIndex < 0)
			{
				throw new ArgumentOutOfRangeException("byteIndex");
			}
			if (byteCount < 0 || bytes.Length < byteIndex + byteCount)
			{
				throw new ArgumentOutOfRangeException("byteCount");
			}
			charsUsed = charCount;
			for (;;)
			{
				bytesUsed = this.GetByteCount(chars, charIndex, charsUsed, flush);
				if (bytesUsed <= byteCount)
				{
					break;
				}
				flush = false;
				charsUsed >>= 1;
			}
			completed = charsUsed == charCount;
			bytesUsed = this.GetBytes(chars, charIndex, charsUsed, bytes, byteIndex, flush);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000045F0 File Offset: 0x000027F0
		private unsafe void CheckArguments(char* chars, int charCount, byte* bytes, int byteCount)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars");
			}
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (charCount < 0)
			{
				throw new ArgumentOutOfRangeException("charCount");
			}
			if (byteCount < 0)
			{
				throw new ArgumentOutOfRangeException("byteCount");
			}
		}
	}
}
