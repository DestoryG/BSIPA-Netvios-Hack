using System;
using System.Runtime.Serialization;
using System.Security;
using System.Text;

namespace I18N.Common
{
	// Token: 0x02000005 RID: 5
	[Serializable]
	public class ReferenceSourceDefaultEncoder : Encoder, IObjectReference
	{
		// Token: 0x06000032 RID: 50 RVA: 0x00002BF1 File Offset: 0x00000DF1
		public ReferenceSourceDefaultEncoder(Encoding encoding)
		{
			this.m_encoding = encoding;
			this.m_hasInitializedEncoding = true;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002C08 File Offset: 0x00000E08
		internal ReferenceSourceDefaultEncoder(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.m_encoding = (Encoding)info.GetValue("encoding", typeof(Encoding));
			try
			{
				this.charLeftOver = (char)info.GetValue("charLeftOver", typeof(char));
			}
			catch (SerializationException)
			{
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002C80 File Offset: 0x00000E80
		[SecurityCritical]
		public object GetRealObject(StreamingContext context)
		{
			if (this.m_hasInitializedEncoding)
			{
				return this;
			}
			return this.m_encoding.GetEncoder();
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002C97 File Offset: 0x00000E97
		public override int GetByteCount(char[] chars, int index, int count, bool flush)
		{
			return this.m_encoding.GetByteCount(chars, index, count);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002CA7 File Offset: 0x00000EA7
		[SecurityCritical]
		public unsafe override int GetByteCount(char* chars, int count, bool flush)
		{
			return this.m_encoding.GetByteCount(chars, count);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002CB6 File Offset: 0x00000EB6
		public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex, bool flush)
		{
			return this.m_encoding.GetBytes(chars, charIndex, charCount, bytes, byteIndex);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002CCA File Offset: 0x00000ECA
		[SecurityCritical]
		public unsafe override int GetBytes(char* chars, int charCount, byte* bytes, int byteCount, bool flush)
		{
			return this.m_encoding.GetBytes(chars, charCount, bytes, byteCount);
		}

		// Token: 0x04000044 RID: 68
		private Encoding m_encoding;

		// Token: 0x04000045 RID: 69
		[NonSerialized]
		private bool m_hasInitializedEncoding;

		// Token: 0x04000046 RID: 70
		[NonSerialized]
		internal char charLeftOver;
	}
}
