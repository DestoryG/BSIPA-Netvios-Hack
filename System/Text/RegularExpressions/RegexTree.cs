using System;
using System.Collections;

namespace System.Text.RegularExpressions
{
	// Token: 0x020006A7 RID: 1703
	internal sealed class RegexTree
	{
		// Token: 0x06003FB7 RID: 16311 RVA: 0x0010BC32 File Offset: 0x00109E32
		internal RegexTree(RegexNode root, Hashtable caps, int[] capnumlist, int captop, Hashtable capnames, string[] capslist, RegexOptions opts)
		{
			this._root = root;
			this._caps = caps;
			this._capnumlist = capnumlist;
			this._capnames = capnames;
			this._capslist = capslist;
			this._captop = captop;
			this._options = opts;
		}

		// Token: 0x04002E6C RID: 11884
		internal RegexNode _root;

		// Token: 0x04002E6D RID: 11885
		internal Hashtable _caps;

		// Token: 0x04002E6E RID: 11886
		internal int[] _capnumlist;

		// Token: 0x04002E6F RID: 11887
		internal Hashtable _capnames;

		// Token: 0x04002E70 RID: 11888
		internal string[] _capslist;

		// Token: 0x04002E71 RID: 11889
		internal RegexOptions _options;

		// Token: 0x04002E72 RID: 11890
		internal int _captop;
	}
}
