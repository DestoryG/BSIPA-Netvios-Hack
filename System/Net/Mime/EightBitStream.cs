using System;
using System.IO;

namespace System.Net.Mime
{
	// Token: 0x02000243 RID: 579
	internal class EightBitStream : DelegatedStream, IEncodableStream
	{
		// Token: 0x170004A3 RID: 1187
		// (get) Token: 0x060015F7 RID: 5623 RVA: 0x00071940 File Offset: 0x0006FB40
		private WriteStateInfoBase WriteState
		{
			get
			{
				if (this.writeState == null)
				{
					this.writeState = new WriteStateInfoBase();
				}
				return this.writeState;
			}
		}

		// Token: 0x060015F8 RID: 5624 RVA: 0x0007195B File Offset: 0x0006FB5B
		internal EightBitStream(Stream stream)
			: base(stream)
		{
		}

		// Token: 0x060015F9 RID: 5625 RVA: 0x00071964 File Offset: 0x0006FB64
		internal EightBitStream(Stream stream, bool shouldEncodeLeadingDots)
			: this(stream)
		{
			this.shouldEncodeLeadingDots = shouldEncodeLeadingDots;
		}

		// Token: 0x060015FA RID: 5626 RVA: 0x00071974 File Offset: 0x0006FB74
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset >= buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset + count > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			IAsyncResult asyncResult;
			if (this.shouldEncodeLeadingDots)
			{
				this.EncodeLines(buffer, offset, count);
				asyncResult = base.BeginWrite(this.WriteState.Buffer, 0, this.WriteState.Length, callback, state);
			}
			else
			{
				asyncResult = base.BeginWrite(buffer, offset, count, callback, state);
			}
			return asyncResult;
		}

		// Token: 0x060015FB RID: 5627 RVA: 0x000719FB File Offset: 0x0006FBFB
		public override void EndWrite(IAsyncResult asyncResult)
		{
			base.EndWrite(asyncResult);
			this.WriteState.BufferFlushed();
		}

		// Token: 0x060015FC RID: 5628 RVA: 0x00071A10 File Offset: 0x0006FC10
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset >= buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset + count > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (this.shouldEncodeLeadingDots)
			{
				this.EncodeLines(buffer, offset, count);
				base.Write(this.WriteState.Buffer, 0, this.WriteState.Length);
				this.WriteState.BufferFlushed();
				return;
			}
			base.Write(buffer, offset, count);
		}

		// Token: 0x060015FD RID: 5629 RVA: 0x00071A98 File Offset: 0x0006FC98
		private void EncodeLines(byte[] buffer, int offset, int count)
		{
			int num = offset;
			while (num < offset + count && num < buffer.Length)
			{
				if (buffer[num] == 13 && num + 1 < offset + count && buffer[num + 1] == 10)
				{
					this.WriteState.AppendCRLF(false);
					num++;
				}
				else if (this.WriteState.CurrentLineLength == 0 && buffer[num] == 46)
				{
					this.WriteState.Append(46);
					this.WriteState.Append(buffer[num]);
				}
				else
				{
					this.WriteState.Append(buffer[num]);
				}
				num++;
			}
		}

		// Token: 0x060015FE RID: 5630 RVA: 0x00071B22 File Offset: 0x0006FD22
		public int DecodeBytes(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060015FF RID: 5631 RVA: 0x00071B29 File Offset: 0x0006FD29
		public int EncodeBytes(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001600 RID: 5632 RVA: 0x00071B30 File Offset: 0x0006FD30
		public Stream GetStream()
		{
			return this;
		}

		// Token: 0x06001601 RID: 5633 RVA: 0x00071B33 File Offset: 0x0006FD33
		public string GetEncodedString()
		{
			throw new NotImplementedException();
		}

		// Token: 0x04001700 RID: 5888
		private WriteStateInfoBase writeState;

		// Token: 0x04001701 RID: 5889
		private bool shouldEncodeLeadingDots;
	}
}
