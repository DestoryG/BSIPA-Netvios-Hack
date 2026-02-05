using System;

namespace System.Net.Mime
{
	// Token: 0x02000253 RID: 595
	internal class WriteStateInfoBase
	{
		// Token: 0x06001693 RID: 5779 RVA: 0x00075124 File Offset: 0x00073324
		internal WriteStateInfoBase()
		{
			this.buffer = new byte[1024];
			this._header = new byte[0];
			this._footer = new byte[0];
			this._maxLineLength = EncodedStreamFactory.DefaultMaxLineLength;
			this._currentLineLength = 0;
			this._currentBufferUsed = 0;
		}

		// Token: 0x06001694 RID: 5780 RVA: 0x00075178 File Offset: 0x00073378
		internal WriteStateInfoBase(int bufferSize, byte[] header, byte[] footer, int maxLineLength)
			: this(bufferSize, header, footer, maxLineLength, 0)
		{
		}

		// Token: 0x06001695 RID: 5781 RVA: 0x00075186 File Offset: 0x00073386
		internal WriteStateInfoBase(int bufferSize, byte[] header, byte[] footer, int maxLineLength, int mimeHeaderLength)
		{
			this.buffer = new byte[bufferSize];
			this._header = header;
			this._footer = footer;
			this._maxLineLength = maxLineLength;
			this._currentLineLength = mimeHeaderLength;
			this._currentBufferUsed = 0;
		}

		// Token: 0x170004B4 RID: 1204
		// (get) Token: 0x06001696 RID: 5782 RVA: 0x000751BF File Offset: 0x000733BF
		internal int FooterLength
		{
			get
			{
				return this._footer.Length;
			}
		}

		// Token: 0x170004B5 RID: 1205
		// (get) Token: 0x06001697 RID: 5783 RVA: 0x000751C9 File Offset: 0x000733C9
		internal byte[] Footer
		{
			get
			{
				return this._footer;
			}
		}

		// Token: 0x170004B6 RID: 1206
		// (get) Token: 0x06001698 RID: 5784 RVA: 0x000751D1 File Offset: 0x000733D1
		internal byte[] Header
		{
			get
			{
				return this._header;
			}
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06001699 RID: 5785 RVA: 0x000751D9 File Offset: 0x000733D9
		internal byte[] Buffer
		{
			get
			{
				return this.buffer;
			}
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x0600169A RID: 5786 RVA: 0x000751E1 File Offset: 0x000733E1
		internal int Length
		{
			get
			{
				return this._currentBufferUsed;
			}
		}

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x0600169B RID: 5787 RVA: 0x000751E9 File Offset: 0x000733E9
		internal int CurrentLineLength
		{
			get
			{
				return this._currentLineLength;
			}
		}

		// Token: 0x0600169C RID: 5788 RVA: 0x000751F4 File Offset: 0x000733F4
		private void EnsureSpaceInBuffer(int moreBytes)
		{
			int num = this.Buffer.Length;
			while (this._currentBufferUsed + moreBytes >= num)
			{
				num *= 2;
			}
			if (num > this.Buffer.Length)
			{
				byte[] array = new byte[num];
				this.buffer.CopyTo(array, 0);
				this.buffer = array;
			}
		}

		// Token: 0x0600169D RID: 5789 RVA: 0x00075244 File Offset: 0x00073444
		internal void Append(byte aByte)
		{
			this.EnsureSpaceInBuffer(1);
			byte[] array = this.Buffer;
			int currentBufferUsed = this._currentBufferUsed;
			this._currentBufferUsed = currentBufferUsed + 1;
			array[currentBufferUsed] = aByte;
			this._currentLineLength++;
		}

		// Token: 0x0600169E RID: 5790 RVA: 0x0007527F File Offset: 0x0007347F
		internal void Append(params byte[] bytes)
		{
			this.EnsureSpaceInBuffer(bytes.Length);
			bytes.CopyTo(this.buffer, this.Length);
			this._currentLineLength += bytes.Length;
			this._currentBufferUsed += bytes.Length;
		}

		// Token: 0x0600169F RID: 5791 RVA: 0x000752BC File Offset: 0x000734BC
		internal void AppendCRLF(bool includeSpace)
		{
			this.AppendFooter();
			this.Append(new byte[] { 13, 10 });
			this._currentLineLength = 0;
			if (includeSpace)
			{
				this.Append(32);
			}
			this.AppendHeader();
		}

		// Token: 0x060016A0 RID: 5792 RVA: 0x000752F2 File Offset: 0x000734F2
		internal void AppendHeader()
		{
			if (this.Header != null && this.Header.Length != 0)
			{
				this.Append(this.Header);
			}
		}

		// Token: 0x060016A1 RID: 5793 RVA: 0x00075311 File Offset: 0x00073511
		internal void AppendFooter()
		{
			if (this.Footer != null && this.Footer.Length != 0)
			{
				this.Append(this.Footer);
			}
		}

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x060016A2 RID: 5794 RVA: 0x00075330 File Offset: 0x00073530
		internal int MaxLineLength
		{
			get
			{
				return this._maxLineLength;
			}
		}

		// Token: 0x060016A3 RID: 5795 RVA: 0x00075338 File Offset: 0x00073538
		internal void Reset()
		{
			this._currentBufferUsed = 0;
			this._currentLineLength = 0;
		}

		// Token: 0x060016A4 RID: 5796 RVA: 0x00075348 File Offset: 0x00073548
		internal void BufferFlushed()
		{
			this._currentBufferUsed = 0;
		}

		// Token: 0x0400175B RID: 5979
		protected byte[] _header;

		// Token: 0x0400175C RID: 5980
		protected byte[] _footer;

		// Token: 0x0400175D RID: 5981
		protected int _maxLineLength;

		// Token: 0x0400175E RID: 5982
		protected byte[] buffer;

		// Token: 0x0400175F RID: 5983
		protected int _currentLineLength;

		// Token: 0x04001760 RID: 5984
		protected int _currentBufferUsed;

		// Token: 0x04001761 RID: 5985
		protected const int defaultBufferSize = 1024;
	}
}
