using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace System.Xml
{
	// Token: 0x02000050 RID: 80
	internal class BufferedWrite
	{
		// Token: 0x060005A4 RID: 1444 RVA: 0x0001B563 File Offset: 0x00019763
		internal BufferedWrite()
			: this(256)
		{
		}

		// Token: 0x060005A5 RID: 1445 RVA: 0x0001B570 File Offset: 0x00019770
		internal BufferedWrite(int initialSize)
		{
			this.buffer = new byte[initialSize];
		}

		// Token: 0x060005A6 RID: 1446 RVA: 0x0001B584 File Offset: 0x00019784
		private void EnsureBuffer(int count)
		{
			int num = this.buffer.Length;
			if (count > num - this.offset)
			{
				int num2 = num;
				while (num2 != 2147483647)
				{
					num2 = ((num2 < 1073741823) ? (num2 * 2) : int.MaxValue);
					if (count <= num2 - this.offset)
					{
						byte[] array = new byte[num2];
						Buffer.BlockCopy(this.buffer, 0, array, 0, this.offset);
						this.buffer = array;
						return;
					}
				}
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new XmlException(global::System.Runtime.Serialization.SR.GetString("Write buffer overflow.")));
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060005A7 RID: 1447 RVA: 0x0001B604 File Offset: 0x00019804
		internal int Length
		{
			get
			{
				return this.offset;
			}
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x0001B60C File Offset: 0x0001980C
		internal byte[] GetBuffer()
		{
			return this.buffer;
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x0001B614 File Offset: 0x00019814
		internal void Reset()
		{
			this.offset = 0;
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x0001B61D File Offset: 0x0001981D
		internal void Write(byte[] value)
		{
			this.Write(value, 0, value.Length);
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x0001B62A File Offset: 0x0001982A
		internal void Write(byte[] value, int index, int count)
		{
			this.EnsureBuffer(count);
			Buffer.BlockCopy(value, index, this.buffer, this.offset, count);
			this.offset += count;
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x0001B655 File Offset: 0x00019855
		internal void Write(string value)
		{
			this.Write(value, 0, value.Length);
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x0001B668 File Offset: 0x00019868
		internal void Write(string value, int index, int count)
		{
			this.EnsureBuffer(count);
			for (int i = 0; i < count; i++)
			{
				char c = value[index + i];
				if (c > 'ÿ')
				{
					string text = "MIME header has an invalid character ('{0}', {1} in hexadecimal value).";
					object[] array = new object[2];
					array[0] = c;
					int num = 1;
					int num2 = (int)c;
					array[num] = num2.ToString("X", CultureInfo.InvariantCulture);
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new FormatException(global::System.Runtime.Serialization.SR.GetString(text, array)));
				}
				this.buffer[this.offset + i] = (byte)c;
			}
			this.offset += count;
		}

		// Token: 0x04000280 RID: 640
		private byte[] buffer;

		// Token: 0x04000281 RID: 641
		private int offset;
	}
}
