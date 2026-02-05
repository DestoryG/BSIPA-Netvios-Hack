using System;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x02000259 RID: 601
	internal class BufferBuilder
	{
		// Token: 0x060016EA RID: 5866 RVA: 0x00075F2D File Offset: 0x0007412D
		internal BufferBuilder()
			: this(256)
		{
		}

		// Token: 0x060016EB RID: 5867 RVA: 0x00075F3A File Offset: 0x0007413A
		internal BufferBuilder(int initialSize)
		{
			this.buffer = new byte[initialSize];
		}

		// Token: 0x060016EC RID: 5868 RVA: 0x00075F50 File Offset: 0x00074150
		private void EnsureBuffer(int count)
		{
			if (count > this.buffer.Length - this.offset)
			{
				byte[] array = new byte[(this.buffer.Length * 2 > this.buffer.Length + count) ? (this.buffer.Length * 2) : (this.buffer.Length + count)];
				Buffer.BlockCopy(this.buffer, 0, array, 0, this.offset);
				this.buffer = array;
			}
		}

		// Token: 0x060016ED RID: 5869 RVA: 0x00075FBC File Offset: 0x000741BC
		internal void Append(byte value)
		{
			this.EnsureBuffer(1);
			byte[] array = this.buffer;
			int num = this.offset;
			this.offset = num + 1;
			array[num] = value;
		}

		// Token: 0x060016EE RID: 5870 RVA: 0x00075FE9 File Offset: 0x000741E9
		internal void Append(byte[] value)
		{
			this.Append(value, 0, value.Length);
		}

		// Token: 0x060016EF RID: 5871 RVA: 0x00075FF6 File Offset: 0x000741F6
		internal void Append(byte[] value, int offset, int count)
		{
			this.EnsureBuffer(count);
			Buffer.BlockCopy(value, offset, this.buffer, this.offset, count);
			this.offset += count;
		}

		// Token: 0x060016F0 RID: 5872 RVA: 0x00076021 File Offset: 0x00074221
		internal void Append(string value)
		{
			this.Append(value, false);
		}

		// Token: 0x060016F1 RID: 5873 RVA: 0x0007602B File Offset: 0x0007422B
		internal void Append(string value, bool allowUnicode)
		{
			if (string.IsNullOrEmpty(value))
			{
				return;
			}
			this.Append(value, 0, value.Length, allowUnicode);
		}

		// Token: 0x060016F2 RID: 5874 RVA: 0x00076048 File Offset: 0x00074248
		internal void Append(string value, int offset, int count, bool allowUnicode)
		{
			if (allowUnicode)
			{
				byte[] bytes = Encoding.UTF8.GetBytes(value.ToCharArray(), offset, count);
				this.Append(bytes);
				return;
			}
			this.Append(value, offset, count);
		}

		// Token: 0x060016F3 RID: 5875 RVA: 0x00076080 File Offset: 0x00074280
		internal void Append(string value, int offset, int count)
		{
			this.EnsureBuffer(count);
			for (int i = 0; i < count; i++)
			{
				char c = value[offset + i];
				if (c > 'ÿ')
				{
					throw new FormatException(SR.GetString("MailHeaderFieldInvalidCharacter", new object[] { c }));
				}
				this.buffer[this.offset + i] = (byte)c;
			}
			this.offset += count;
		}

		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x060016F4 RID: 5876 RVA: 0x000760F0 File Offset: 0x000742F0
		internal int Length
		{
			get
			{
				return this.offset;
			}
		}

		// Token: 0x060016F5 RID: 5877 RVA: 0x000760F8 File Offset: 0x000742F8
		internal byte[] GetBuffer()
		{
			return this.buffer;
		}

		// Token: 0x060016F6 RID: 5878 RVA: 0x00076100 File Offset: 0x00074300
		internal void Reset()
		{
			this.offset = 0;
		}

		// Token: 0x04001769 RID: 5993
		private byte[] buffer;

		// Token: 0x0400176A RID: 5994
		private int offset;
	}
}
