using System;
using System.Collections;

namespace System.Text.RegularExpressions
{
	// Token: 0x0200068E RID: 1678
	[Serializable]
	internal class CaptureEnumerator : IEnumerator
	{
		// Token: 0x06003DF7 RID: 15863 RVA: 0x000FDCBF File Offset: 0x000FBEBF
		internal CaptureEnumerator(CaptureCollection rcc)
		{
			this._curindex = -1;
			this._rcc = rcc;
		}

		// Token: 0x06003DF8 RID: 15864 RVA: 0x000FDCD8 File Offset: 0x000FBED8
		public bool MoveNext()
		{
			int count = this._rcc.Count;
			if (this._curindex >= count)
			{
				return false;
			}
			this._curindex++;
			return this._curindex < count;
		}

		// Token: 0x17000EB2 RID: 3762
		// (get) Token: 0x06003DF9 RID: 15865 RVA: 0x000FDD13 File Offset: 0x000FBF13
		public object Current
		{
			get
			{
				return this.Capture;
			}
		}

		// Token: 0x17000EB3 RID: 3763
		// (get) Token: 0x06003DFA RID: 15866 RVA: 0x000FDD1B File Offset: 0x000FBF1B
		public Capture Capture
		{
			get
			{
				if (this._curindex < 0 || this._curindex >= this._rcc.Count)
				{
					throw new InvalidOperationException(SR.GetString("EnumNotStarted"));
				}
				return this._rcc[this._curindex];
			}
		}

		// Token: 0x06003DFB RID: 15867 RVA: 0x000FDD5A File Offset: 0x000FBF5A
		public void Reset()
		{
			this._curindex = -1;
		}

		// Token: 0x04002CF9 RID: 11513
		internal CaptureCollection _rcc;

		// Token: 0x04002CFA RID: 11514
		internal int _curindex;
	}
}
