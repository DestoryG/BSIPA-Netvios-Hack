using System;
using System.Collections;

namespace System.Text.RegularExpressions
{
	// Token: 0x0200069F RID: 1695
	[Serializable]
	internal class MatchEnumerator : IEnumerator
	{
		// Token: 0x06003F1F RID: 16159 RVA: 0x001077E8 File Offset: 0x001059E8
		internal MatchEnumerator(MatchCollection matchcoll)
		{
			this._matchcoll = matchcoll;
		}

		// Token: 0x06003F20 RID: 16160 RVA: 0x001077F8 File Offset: 0x001059F8
		public bool MoveNext()
		{
			if (this._done)
			{
				return false;
			}
			this._match = this._matchcoll.GetMatch(this._curindex);
			this._curindex++;
			if (this._match == null)
			{
				this._done = true;
				return false;
			}
			return true;
		}

		// Token: 0x17000ED2 RID: 3794
		// (get) Token: 0x06003F21 RID: 16161 RVA: 0x00107846 File Offset: 0x00105A46
		public object Current
		{
			get
			{
				if (this._match == null)
				{
					throw new InvalidOperationException(SR.GetString("EnumNotStarted"));
				}
				return this._match;
			}
		}

		// Token: 0x06003F22 RID: 16162 RVA: 0x00107866 File Offset: 0x00105A66
		public void Reset()
		{
			this._curindex = 0;
			this._done = false;
			this._match = null;
		}

		// Token: 0x04002DFA RID: 11770
		internal MatchCollection _matchcoll;

		// Token: 0x04002DFB RID: 11771
		internal Match _match;

		// Token: 0x04002DFC RID: 11772
		internal int _curindex;

		// Token: 0x04002DFD RID: 11773
		internal bool _done;
	}
}
