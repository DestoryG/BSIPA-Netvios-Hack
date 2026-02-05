using System;
using System.IO;

namespace Google.Protobuf
{
	// Token: 0x0200001E RID: 30
	internal sealed class LimitedInputStream : Stream
	{
		// Token: 0x060001B1 RID: 433 RVA: 0x0000946A File Offset: 0x0000766A
		internal LimitedInputStream(Stream proxied, int size)
		{
			this.proxied = proxied;
			this.bytesLeft = size;
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x00009480 File Offset: 0x00007680
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x00009483 File Offset: 0x00007683
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x00009486 File Offset: 0x00007686
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00009489 File Offset: 0x00007689
		public override void Flush()
		{
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x0000948B File Offset: 0x0000768B
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x00009492 File Offset: 0x00007692
		// (set) Token: 0x060001B8 RID: 440 RVA: 0x00009499 File Offset: 0x00007699
		public override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x000094A0 File Offset: 0x000076A0
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this.bytesLeft > 0)
			{
				int num = this.proxied.Read(buffer, offset, Math.Min(this.bytesLeft, count));
				this.bytesLeft -= num;
				return num;
			}
			return 0;
		}

		// Token: 0x060001BA RID: 442 RVA: 0x000094E1 File Offset: 0x000076E1
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060001BB RID: 443 RVA: 0x000094E8 File Offset: 0x000076E8
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060001BC RID: 444 RVA: 0x000094EF File Offset: 0x000076EF
		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0400005C RID: 92
		private readonly Stream proxied;

		// Token: 0x0400005D RID: 93
		private int bytesLeft;
	}
}
