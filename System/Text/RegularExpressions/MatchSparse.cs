using System;
using System.Collections;

namespace System.Text.RegularExpressions
{
	// Token: 0x0200069D RID: 1693
	internal class MatchSparse : Match
	{
		// Token: 0x06003F13 RID: 16147 RVA: 0x001075B2 File Offset: 0x001057B2
		internal MatchSparse(Regex regex, Hashtable caps, int capcount, string text, int begpos, int len, int startpos)
			: base(regex, capcount, text, begpos, len, startpos)
		{
			this._caps = caps;
		}

		// Token: 0x17000ECC RID: 3788
		// (get) Token: 0x06003F14 RID: 16148 RVA: 0x001075CB File Offset: 0x001057CB
		public override GroupCollection Groups
		{
			get
			{
				if (this._groupcoll == null)
				{
					this._groupcoll = new GroupCollection(this, this._caps);
				}
				return this._groupcoll;
			}
		}

		// Token: 0x04002DF0 RID: 11760
		internal new Hashtable _caps;
	}
}
