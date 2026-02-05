using System;
using System.Collections;

namespace System.IO
{
	// Token: 0x02000002 RID: 2
	public class FifoFloatStream
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		private float[] AllocBlock()
		{
			if (this.m_UsedBlocks.Count <= 0)
			{
				return new float[16384];
			}
			return (float[])this.m_UsedBlocks.Pop();
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000207B File Offset: 0x0000027B
		private void FreeBlock(float[] block)
		{
			if (this.m_UsedBlocks.Count < 192)
			{
				this.m_UsedBlocks.Push(block);
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000209C File Offset: 0x0000029C
		private float[] GetWBlock()
		{
			float[] array;
			if (this.m_WPos < 16384 && this.m_Blocks.Count > 0)
			{
				array = (float[])this.m_Blocks[this.m_Blocks.Count - 1];
			}
			else
			{
				array = this.AllocBlock();
				this.m_Blocks.Add(array);
				this.m_WPos = 0;
			}
			return array;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000004 RID: 4 RVA: 0x00002104 File Offset: 0x00000304
		public long Length
		{
			get
			{
				long num;
				lock (this)
				{
					num = (long)this.m_Size;
				}
				return num;
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002144 File Offset: 0x00000344
		public void Close()
		{
			this.Flush();
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000214C File Offset: 0x0000034C
		public void Flush()
		{
			lock (this)
			{
				foreach (object obj in this.m_Blocks)
				{
					float[] array = (float[])obj;
					this.FreeBlock(array);
				}
				this.m_Blocks.Clear();
				this.m_RPos = 0;
				this.m_WPos = 0;
				this.m_Size = 0;
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000021EC File Offset: 0x000003EC
		public int Read(float[] buf, int ofs, int count)
		{
			int num2;
			lock (this)
			{
				int num = this.Peek(buf, ofs, count);
				this.Advance(num);
				num2 = num;
			}
			return num2;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002238 File Offset: 0x00000438
		public void Write(float[] buf, int ofs, int count)
		{
			lock (this)
			{
				int num;
				for (int i = count; i > 0; i -= num)
				{
					num = Math.Min(16384 - this.m_WPos, i);
					Array.Copy(buf, ofs + count - i, this.GetWBlock(), this.m_WPos, num);
					this.m_WPos += num;
				}
				this.m_Size += count;
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000022C0 File Offset: 0x000004C0
		public int Advance(int count)
		{
			int num3;
			lock (this)
			{
				int num = count;
				while (num > 0 && this.m_Size > 0)
				{
					if (this.m_RPos == 16384)
					{
						this.m_RPos = 0;
						this.FreeBlock((float[])this.m_Blocks[0]);
						this.m_Blocks.RemoveAt(0);
					}
					int num2 = ((this.m_Blocks.Count == 1) ? Math.Min(this.m_WPos - this.m_RPos, num) : Math.Min(16384 - this.m_RPos, num));
					this.m_RPos += num2;
					num -= num2;
					this.m_Size -= num2;
				}
				num3 = count - num;
			}
			return num3;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000023A0 File Offset: 0x000005A0
		public int Peek(float[] buf, int ofs, int count)
		{
			int num6;
			lock (this)
			{
				int num = count;
				int num2 = this.m_RPos;
				int num3 = this.m_Size;
				int num4 = 0;
				while (num > 0 && num3 > 0)
				{
					if (num2 == 16384)
					{
						num2 = 0;
						num4++;
					}
					int num5 = Math.Min(((num4 < this.m_Blocks.Count - 1) ? 16384 : this.m_WPos) - num2, num);
					Array.Copy((float[])this.m_Blocks[num4], num2, buf, ofs + count - num, num5);
					num -= num5;
					num2 += num5;
					num3 -= num5;
				}
				num6 = count - num;
			}
			return num6;
		}

		// Token: 0x04000001 RID: 1
		private const int BlockSize = 16384;

		// Token: 0x04000002 RID: 2
		private const int MaxBlocksInCache = 192;

		// Token: 0x04000003 RID: 3
		private int m_Size;

		// Token: 0x04000004 RID: 4
		private int m_RPos;

		// Token: 0x04000005 RID: 5
		private int m_WPos;

		// Token: 0x04000006 RID: 6
		private Stack m_UsedBlocks = new Stack();

		// Token: 0x04000007 RID: 7
		private ArrayList m_Blocks = new ArrayList();
	}
}
