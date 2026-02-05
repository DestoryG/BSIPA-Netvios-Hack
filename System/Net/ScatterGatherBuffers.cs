using System;

namespace System.Net
{
	// Token: 0x02000208 RID: 520
	internal class ScatterGatherBuffers
	{
		// Token: 0x0600136E RID: 4974 RVA: 0x000661C7 File Offset: 0x000643C7
		internal ScatterGatherBuffers()
		{
		}

		// Token: 0x0600136F RID: 4975 RVA: 0x000661DA File Offset: 0x000643DA
		internal ScatterGatherBuffers(long totalSize)
		{
			if (totalSize > 0L)
			{
				this.currentChunk = this.AllocateMemoryChunk((totalSize > 2147483647L) ? int.MaxValue : ((int)totalSize));
			}
		}

		// Token: 0x06001370 RID: 4976 RVA: 0x00066210 File Offset: 0x00064410
		internal BufferOffsetSize[] GetBuffers()
		{
			if (this.Empty)
			{
				return null;
			}
			BufferOffsetSize[] array = new BufferOffsetSize[this.chunkCount];
			int num = 0;
			for (ScatterGatherBuffers.MemoryChunk next = this.headChunk; next != null; next = next.Next)
			{
				array[num] = new BufferOffsetSize(next.Buffer, 0, next.FreeOffset, false);
				num++;
			}
			return array;
		}

		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x06001371 RID: 4977 RVA: 0x00066263 File Offset: 0x00064463
		private bool Empty
		{
			get
			{
				return this.headChunk == null || this.chunkCount == 0;
			}
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06001372 RID: 4978 RVA: 0x00066278 File Offset: 0x00064478
		internal int Length
		{
			get
			{
				return this.totalLength;
			}
		}

		// Token: 0x06001373 RID: 4979 RVA: 0x00066280 File Offset: 0x00064480
		internal void Write(byte[] buffer, int offset, int count)
		{
			while (count > 0)
			{
				int num = (this.Empty ? 0 : (this.currentChunk.Buffer.Length - this.currentChunk.FreeOffset));
				if (num == 0)
				{
					ScatterGatherBuffers.MemoryChunk memoryChunk = this.AllocateMemoryChunk(count);
					if (this.currentChunk != null)
					{
						this.currentChunk.Next = memoryChunk;
					}
					this.currentChunk = memoryChunk;
				}
				int num2 = ((count < num) ? count : num);
				Buffer.BlockCopy(buffer, offset, this.currentChunk.Buffer, this.currentChunk.FreeOffset, num2);
				offset += num2;
				count -= num2;
				this.totalLength += num2;
				this.currentChunk.FreeOffset += num2;
			}
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x00066338 File Offset: 0x00064538
		private ScatterGatherBuffers.MemoryChunk AllocateMemoryChunk(int newSize)
		{
			if (newSize > this.nextChunkLength)
			{
				this.nextChunkLength = newSize;
			}
			ScatterGatherBuffers.MemoryChunk memoryChunk = new ScatterGatherBuffers.MemoryChunk(this.nextChunkLength);
			if (this.Empty)
			{
				this.headChunk = memoryChunk;
			}
			this.nextChunkLength *= 2;
			this.chunkCount++;
			return memoryChunk;
		}

		// Token: 0x04001557 RID: 5463
		private ScatterGatherBuffers.MemoryChunk headChunk;

		// Token: 0x04001558 RID: 5464
		private ScatterGatherBuffers.MemoryChunk currentChunk;

		// Token: 0x04001559 RID: 5465
		private int nextChunkLength = 1024;

		// Token: 0x0400155A RID: 5466
		private int totalLength;

		// Token: 0x0400155B RID: 5467
		private int chunkCount;

		// Token: 0x02000758 RID: 1880
		private class MemoryChunk
		{
			// Token: 0x06004207 RID: 16903 RVA: 0x0011240F File Offset: 0x0011060F
			internal MemoryChunk(int bufferSize)
			{
				this.Buffer = new byte[bufferSize];
			}

			// Token: 0x04003214 RID: 12820
			internal byte[] Buffer;

			// Token: 0x04003215 RID: 12821
			internal int FreeOffset;

			// Token: 0x04003216 RID: 12822
			internal ScatterGatherBuffers.MemoryChunk Next;
		}
	}
}
