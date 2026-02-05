using System;

namespace System.Net.Sockets
{
	// Token: 0x0200037A RID: 890
	public class SendPacketsElement
	{
		// Token: 0x06002127 RID: 8487 RVA: 0x0009EEF9 File Offset: 0x0009D0F9
		private SendPacketsElement()
		{
		}

		// Token: 0x06002128 RID: 8488 RVA: 0x0009EF01 File Offset: 0x0009D101
		public SendPacketsElement(string filepath)
			: this(filepath, 0, 0, false)
		{
		}

		// Token: 0x06002129 RID: 8489 RVA: 0x0009EF0D File Offset: 0x0009D10D
		public SendPacketsElement(string filepath, int offset, int count)
			: this(filepath, offset, count, false)
		{
		}

		// Token: 0x0600212A RID: 8490 RVA: 0x0009EF1C File Offset: 0x0009D11C
		public SendPacketsElement(string filepath, int offset, int count, bool endOfPacket)
		{
			if (filepath == null)
			{
				throw new ArgumentNullException("filepath");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			this.Initialize(filepath, null, offset, count, UnsafeNclNativeMethods.OSSOCK.TransmitPacketsElementFlags.File, endOfPacket);
		}

		// Token: 0x0600212B RID: 8491 RVA: 0x0009EF68 File Offset: 0x0009D168
		public SendPacketsElement(byte[] buffer)
			: this(buffer, 0, (buffer != null) ? buffer.Length : 0, false)
		{
		}

		// Token: 0x0600212C RID: 8492 RVA: 0x0009EF7C File Offset: 0x0009D17C
		public SendPacketsElement(byte[] buffer, int offset, int count)
			: this(buffer, offset, count, false)
		{
		}

		// Token: 0x0600212D RID: 8493 RVA: 0x0009EF88 File Offset: 0x0009D188
		public SendPacketsElement(byte[] buffer, int offset, int count, bool endOfPacket)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (count < 0 || count > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			this.Initialize(null, buffer, offset, count, UnsafeNclNativeMethods.OSSOCK.TransmitPacketsElementFlags.Memory, endOfPacket);
		}

		// Token: 0x0600212E RID: 8494 RVA: 0x0009EFE2 File Offset: 0x0009D1E2
		private void Initialize(string filePath, byte[] buffer, int offset, int count, UnsafeNclNativeMethods.OSSOCK.TransmitPacketsElementFlags flags, bool endOfPacket)
		{
			this.m_FilePath = filePath;
			this.m_Buffer = buffer;
			this.m_Offset = offset;
			this.m_Count = count;
			this.m_Flags = flags;
			if (endOfPacket)
			{
				this.m_Flags |= UnsafeNclNativeMethods.OSSOCK.TransmitPacketsElementFlags.EndOfPacket;
			}
		}

		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x0600212F RID: 8495 RVA: 0x0009F01B File Offset: 0x0009D21B
		public string FilePath
		{
			get
			{
				return this.m_FilePath;
			}
		}

		// Token: 0x1700088A RID: 2186
		// (get) Token: 0x06002130 RID: 8496 RVA: 0x0009F023 File Offset: 0x0009D223
		public byte[] Buffer
		{
			get
			{
				return this.m_Buffer;
			}
		}

		// Token: 0x1700088B RID: 2187
		// (get) Token: 0x06002131 RID: 8497 RVA: 0x0009F02B File Offset: 0x0009D22B
		public int Count
		{
			get
			{
				return this.m_Count;
			}
		}

		// Token: 0x1700088C RID: 2188
		// (get) Token: 0x06002132 RID: 8498 RVA: 0x0009F033 File Offset: 0x0009D233
		public int Offset
		{
			get
			{
				return this.m_Offset;
			}
		}

		// Token: 0x1700088D RID: 2189
		// (get) Token: 0x06002133 RID: 8499 RVA: 0x0009F03B File Offset: 0x0009D23B
		public bool EndOfPacket
		{
			get
			{
				return (this.m_Flags & UnsafeNclNativeMethods.OSSOCK.TransmitPacketsElementFlags.EndOfPacket) > UnsafeNclNativeMethods.OSSOCK.TransmitPacketsElementFlags.None;
			}
		}

		// Token: 0x04001E65 RID: 7781
		internal string m_FilePath;

		// Token: 0x04001E66 RID: 7782
		internal byte[] m_Buffer;

		// Token: 0x04001E67 RID: 7783
		internal int m_Offset;

		// Token: 0x04001E68 RID: 7784
		internal int m_Count;

		// Token: 0x04001E69 RID: 7785
		internal UnsafeNclNativeMethods.OSSOCK.TransmitPacketsElementFlags m_Flags;
	}
}
