using System;
using System.Collections;

namespace System.Text.RegularExpressions
{
	// Token: 0x0200069A RID: 1690
	internal class GroupEnumerator : IEnumerator
	{
		// Token: 0x06003ED3 RID: 16083 RVA: 0x00105794 File Offset: 0x00103994
		internal GroupEnumerator(GroupCollection rgc)
		{
			this._curindex = -1;
			this._rgc = rgc;
		}

		// Token: 0x06003ED4 RID: 16084 RVA: 0x001057AC File Offset: 0x001039AC
		public bool MoveNext()
		{
			int count = this._rgc.Count;
			if (this._curindex >= count)
			{
				return false;
			}
			this._curindex++;
			return this._curindex < count;
		}

		// Token: 0x17000EC8 RID: 3784
		// (get) Token: 0x06003ED5 RID: 16085 RVA: 0x001057E7 File Offset: 0x001039E7
		public object Current
		{
			get
			{
				return this.Capture;
			}
		}

		// Token: 0x17000EC9 RID: 3785
		// (get) Token: 0x06003ED6 RID: 16086 RVA: 0x001057EF File Offset: 0x001039EF
		public Capture Capture
		{
			get
			{
				if (this._curindex < 0 || this._curindex >= this._rgc.Count)
				{
					throw new InvalidOperationException(SR.GetString("EnumNotStarted"));
				}
				return this._rgc[this._curindex];
			}
		}

		// Token: 0x06003ED7 RID: 16087 RVA: 0x0010582E File Offset: 0x00103A2E
		public void Reset()
		{
			this._curindex = -1;
		}

		// Token: 0x04002DD7 RID: 11735
		internal GroupCollection _rgc;

		// Token: 0x04002DD8 RID: 11736
		internal int _curindex;
	}
}
