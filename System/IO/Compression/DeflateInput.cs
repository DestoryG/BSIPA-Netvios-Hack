using System;

namespace System.IO.Compression
{
	// Token: 0x0200041E RID: 1054
	internal class DeflateInput
	{
		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x06002767 RID: 10087 RVA: 0x000B58DF File Offset: 0x000B3ADF
		// (set) Token: 0x06002768 RID: 10088 RVA: 0x000B58E7 File Offset: 0x000B3AE7
		internal byte[] Buffer
		{
			get
			{
				return this.buffer;
			}
			set
			{
				this.buffer = value;
			}
		}

		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x06002769 RID: 10089 RVA: 0x000B58F0 File Offset: 0x000B3AF0
		// (set) Token: 0x0600276A RID: 10090 RVA: 0x000B58F8 File Offset: 0x000B3AF8
		internal int Count
		{
			get
			{
				return this.count;
			}
			set
			{
				this.count = value;
			}
		}

		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x0600276B RID: 10091 RVA: 0x000B5901 File Offset: 0x000B3B01
		// (set) Token: 0x0600276C RID: 10092 RVA: 0x000B5909 File Offset: 0x000B3B09
		internal int StartIndex
		{
			get
			{
				return this.startIndex;
			}
			set
			{
				this.startIndex = value;
			}
		}

		// Token: 0x0600276D RID: 10093 RVA: 0x000B5912 File Offset: 0x000B3B12
		internal void ConsumeBytes(int n)
		{
			this.startIndex += n;
			this.count -= n;
		}

		// Token: 0x0600276E RID: 10094 RVA: 0x000B5930 File Offset: 0x000B3B30
		internal DeflateInput.InputState DumpState()
		{
			DeflateInput.InputState inputState;
			inputState.count = this.count;
			inputState.startIndex = this.startIndex;
			return inputState;
		}

		// Token: 0x0600276F RID: 10095 RVA: 0x000B5958 File Offset: 0x000B3B58
		internal void RestoreState(DeflateInput.InputState state)
		{
			this.count = state.count;
			this.startIndex = state.startIndex;
		}

		// Token: 0x04002165 RID: 8549
		private byte[] buffer;

		// Token: 0x04002166 RID: 8550
		private int count;

		// Token: 0x04002167 RID: 8551
		private int startIndex;

		// Token: 0x02000816 RID: 2070
		internal struct InputState
		{
			// Token: 0x0400358B RID: 13707
			internal int count;

			// Token: 0x0400358C RID: 13708
			internal int startIndex;
		}
	}
}
