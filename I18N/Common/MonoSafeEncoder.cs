using System;
using System.Text;

namespace I18N.Common
{
	// Token: 0x0200000C RID: 12
	public abstract class MonoSafeEncoder : Encoder
	{
		// Token: 0x06000066 RID: 102 RVA: 0x0000474D File Offset: 0x0000294D
		public MonoSafeEncoder(MonoSafeEncoding encoding)
		{
			this.encoding = encoding;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x0000475C File Offset: 0x0000295C
		public void HandleFallback(char[] chars, ref int charIndex, ref int charCount, byte[] bytes, ref int byteIndex, ref int byteCount, object state)
		{
			EncoderFallbackBuffer fallbackBuffer = base.FallbackBuffer;
			this.encoding.HandleFallback(ref fallbackBuffer, chars, ref charIndex, ref charCount, bytes, ref byteIndex, ref byteCount, state);
		}

		// Token: 0x04000052 RID: 82
		private MonoSafeEncoding encoding;
	}
}
