using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
	// Token: 0x02000225 RID: 549
	internal class BufferedReadStream : DelegatedStream
	{
		// Token: 0x06001431 RID: 5169 RVA: 0x0006B1E6 File Offset: 0x000693E6
		internal BufferedReadStream(Stream stream)
			: this(stream, false)
		{
		}

		// Token: 0x06001432 RID: 5170 RVA: 0x0006B1F0 File Offset: 0x000693F0
		internal BufferedReadStream(Stream stream, bool readMore)
			: base(stream)
		{
			this.readMore = readMore;
		}

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06001433 RID: 5171 RVA: 0x0006B200 File Offset: 0x00069400
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x06001434 RID: 5172 RVA: 0x0006B203 File Offset: 0x00069403
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001435 RID: 5173 RVA: 0x0006B208 File Offset: 0x00069408
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
		{
			BufferedReadStream.ReadAsyncResult readAsyncResult = new BufferedReadStream.ReadAsyncResult(this, callback, state);
			readAsyncResult.Read(buffer, offset, count);
			return readAsyncResult;
		}

		// Token: 0x06001436 RID: 5174 RVA: 0x0006B22C File Offset: 0x0006942C
		public override int EndRead(IAsyncResult asyncResult)
		{
			return BufferedReadStream.ReadAsyncResult.End(asyncResult);
		}

		// Token: 0x06001437 RID: 5175 RVA: 0x0006B244 File Offset: 0x00069444
		public override int Read(byte[] buffer, int offset, int count)
		{
			int num = 0;
			if (this.storedOffset < this.storedLength)
			{
				num = Math.Min(count, this.storedLength - this.storedOffset);
				Buffer.BlockCopy(this.storedBuffer, this.storedOffset, buffer, offset, num);
				this.storedOffset += num;
				if (num == count || !this.readMore)
				{
					return num;
				}
				offset += num;
				count -= num;
			}
			return num + base.Read(buffer, offset, count);
		}

		// Token: 0x06001438 RID: 5176 RVA: 0x0006B2BC File Offset: 0x000694BC
		public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			if (this.storedOffset >= this.storedLength)
			{
				return base.ReadAsync(buffer, offset, count, cancellationToken);
			}
			int num = Math.Min(count, this.storedLength - this.storedOffset);
			Buffer.BlockCopy(this.storedBuffer, this.storedOffset, buffer, offset, num);
			this.storedOffset += num;
			if (num == count || !this.readMore)
			{
				return Task.FromResult<int>(num);
			}
			offset += num;
			count -= num;
			return this.ReadMoreAsync(num, buffer, offset, count, cancellationToken);
		}

		// Token: 0x06001439 RID: 5177 RVA: 0x0006B344 File Offset: 0x00069544
		private async Task<int> ReadMoreAsync(int bytesAlreadyRead, byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			int num = await base.ReadAsync(buffer, offset, count, cancellationToken).ConfigureAwait(false);
			int num2 = num;
			return bytesAlreadyRead + num2;
		}

		// Token: 0x0600143A RID: 5178 RVA: 0x0006B3B4 File Offset: 0x000695B4
		public override int ReadByte()
		{
			if (this.storedOffset < this.storedLength)
			{
				byte[] array = this.storedBuffer;
				int num = this.storedOffset;
				this.storedOffset = num + 1;
				return array[num];
			}
			return base.ReadByte();
		}

		// Token: 0x0600143B RID: 5179 RVA: 0x0006B3F0 File Offset: 0x000695F0
		internal void Push(byte[] buffer, int offset, int count)
		{
			if (count == 0)
			{
				return;
			}
			if (this.storedOffset == this.storedLength)
			{
				if (this.storedBuffer == null || this.storedBuffer.Length < count)
				{
					this.storedBuffer = new byte[count];
				}
				this.storedOffset = 0;
				this.storedLength = count;
			}
			else if (count <= this.storedOffset)
			{
				this.storedOffset -= count;
			}
			else if (count <= this.storedBuffer.Length - this.storedLength + this.storedOffset)
			{
				Buffer.BlockCopy(this.storedBuffer, this.storedOffset, this.storedBuffer, count, this.storedLength - this.storedOffset);
				this.storedLength += count - this.storedOffset;
				this.storedOffset = 0;
			}
			else
			{
				byte[] array = new byte[count + this.storedLength - this.storedOffset];
				Buffer.BlockCopy(this.storedBuffer, this.storedOffset, array, count, this.storedLength - this.storedOffset);
				this.storedLength += count - this.storedOffset;
				this.storedOffset = 0;
				this.storedBuffer = array;
			}
			Buffer.BlockCopy(buffer, offset, this.storedBuffer, this.storedOffset, count);
		}

		// Token: 0x0600143C RID: 5180 RVA: 0x0006B528 File Offset: 0x00069728
		internal void Append(byte[] buffer, int offset, int count)
		{
			if (count == 0)
			{
				return;
			}
			int num;
			if (this.storedOffset == this.storedLength)
			{
				if (this.storedBuffer == null || this.storedBuffer.Length < count)
				{
					this.storedBuffer = new byte[count];
				}
				this.storedOffset = 0;
				this.storedLength = count;
				num = 0;
			}
			else if (count <= this.storedBuffer.Length - this.storedLength)
			{
				num = this.storedLength;
				this.storedLength += count;
			}
			else if (count <= this.storedBuffer.Length - this.storedLength + this.storedOffset)
			{
				Buffer.BlockCopy(this.storedBuffer, this.storedOffset, this.storedBuffer, 0, this.storedLength - this.storedOffset);
				num = this.storedLength - this.storedOffset;
				this.storedOffset = 0;
				this.storedLength = count + num;
			}
			else
			{
				byte[] array = new byte[count + this.storedLength - this.storedOffset];
				Buffer.BlockCopy(this.storedBuffer, this.storedOffset, array, 0, this.storedLength - this.storedOffset);
				num = this.storedLength - this.storedOffset;
				this.storedOffset = 0;
				this.storedLength = count + num;
				this.storedBuffer = array;
			}
			Buffer.BlockCopy(buffer, offset, this.storedBuffer, num, count);
		}

		// Token: 0x0400161A RID: 5658
		private byte[] storedBuffer;

		// Token: 0x0400161B RID: 5659
		private int storedLength;

		// Token: 0x0400161C RID: 5660
		private int storedOffset;

		// Token: 0x0400161D RID: 5661
		private bool readMore;

		// Token: 0x02000768 RID: 1896
		private class ReadAsyncResult : LazyAsyncResult
		{
			// Token: 0x06004247 RID: 16967 RVA: 0x00112E38 File Offset: 0x00111038
			internal ReadAsyncResult(BufferedReadStream parent, AsyncCallback callback, object state)
				: base(null, state, callback)
			{
				this.parent = parent;
			}

			// Token: 0x06004248 RID: 16968 RVA: 0x00112E4C File Offset: 0x0011104C
			internal void Read(byte[] buffer, int offset, int count)
			{
				if (this.parent.storedOffset < this.parent.storedLength)
				{
					this.read = Math.Min(count, this.parent.storedLength - this.parent.storedOffset);
					Buffer.BlockCopy(this.parent.storedBuffer, this.parent.storedOffset, buffer, offset, this.read);
					this.parent.storedOffset += this.read;
					if (this.read == count || !this.parent.readMore)
					{
						base.InvokeCallback();
						return;
					}
					count -= this.read;
					offset += this.read;
				}
				IAsyncResult asyncResult = this.parent.BaseStream.BeginRead(buffer, offset, count, BufferedReadStream.ReadAsyncResult.onRead, this);
				if (asyncResult.CompletedSynchronously)
				{
					this.read += this.parent.BaseStream.EndRead(asyncResult);
					base.InvokeCallback();
				}
			}

			// Token: 0x06004249 RID: 16969 RVA: 0x00112F4C File Offset: 0x0011114C
			internal static int End(IAsyncResult result)
			{
				BufferedReadStream.ReadAsyncResult readAsyncResult = (BufferedReadStream.ReadAsyncResult)result;
				readAsyncResult.InternalWaitForCompletion();
				return readAsyncResult.read;
			}

			// Token: 0x0600424A RID: 16970 RVA: 0x00112F70 File Offset: 0x00111170
			private static void OnRead(IAsyncResult result)
			{
				if (!result.CompletedSynchronously)
				{
					BufferedReadStream.ReadAsyncResult readAsyncResult = (BufferedReadStream.ReadAsyncResult)result.AsyncState;
					try
					{
						readAsyncResult.read += readAsyncResult.parent.BaseStream.EndRead(result);
						readAsyncResult.InvokeCallback();
					}
					catch (Exception ex)
					{
						if (readAsyncResult.IsCompleted)
						{
							throw;
						}
						readAsyncResult.InvokeCallback(ex);
					}
				}
			}

			// Token: 0x04003247 RID: 12871
			private BufferedReadStream parent;

			// Token: 0x04003248 RID: 12872
			private int read;

			// Token: 0x04003249 RID: 12873
			private static AsyncCallback onRead = new AsyncCallback(BufferedReadStream.ReadAsyncResult.OnRead);
		}
	}
}
