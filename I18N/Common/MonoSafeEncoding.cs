using System;
using System.Text;

namespace I18N.Common
{
	// Token: 0x0200000B RID: 11
	[Serializable]
	public abstract class MonoSafeEncoding : Encoding
	{
		// Token: 0x06000061 RID: 97 RVA: 0x0000463C File Offset: 0x0000283C
		public MonoSafeEncoding(int codePage)
			: this(codePage, 0)
		{
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00004646 File Offset: 0x00002846
		public MonoSafeEncoding(int codePage, int windowsCodePage)
			: base(codePage)
		{
			this.win_code_page = windowsCodePage;
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00004656 File Offset: 0x00002856
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

		// Token: 0x06000064 RID: 100 RVA: 0x0000466D File Offset: 0x0000286D
		protected virtual int GetBytesInternal(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex, bool flush, object state)
		{
			throw new NotImplementedException("Statefull encoding is not implemented (yet?) by this encoding class.");
		}

		// Token: 0x06000065 RID: 101 RVA: 0x0000467C File Offset: 0x0000287C
		public void HandleFallback(ref EncoderFallbackBuffer buffer, char[] chars, ref int charIndex, ref int charCount, byte[] bytes, ref int byteIndex, ref int byteCount, object state)
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
			int num2 = ((state == null) ? this.GetBytes(array, 0, array.Length, bytes, byteIndex) : this.GetBytesInternal(array, 0, array.Length, bytes, byteIndex, true, state));
			byteIndex += num2;
			byteCount -= num2;
		}

		// Token: 0x04000051 RID: 81
		private readonly int win_code_page;
	}
}
