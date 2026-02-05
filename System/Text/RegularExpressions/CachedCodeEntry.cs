using System;
using System.Collections;

namespace System.Text.RegularExpressions
{
	// Token: 0x02000688 RID: 1672
	internal sealed class CachedCodeEntry
	{
		// Token: 0x06003DD9 RID: 15833 RVA: 0x000FD450 File Offset: 0x000FB650
		internal CachedCodeEntry(string key, Hashtable capnames, string[] capslist, RegexCode code, Hashtable caps, int capsize, ExclusiveReference runner, SharedReference repl)
		{
			this._key = key;
			this._capnames = capnames;
			this._capslist = capslist;
			this._code = code;
			this._caps = caps;
			this._capsize = capsize;
			this._runnerref = runner;
			this._replref = repl;
		}

		// Token: 0x06003DDA RID: 15834 RVA: 0x000FD4A0 File Offset: 0x000FB6A0
		internal void AddCompiled(RegexRunnerFactory factory)
		{
			this._factory = factory;
			this._code = null;
		}

		// Token: 0x04002CDB RID: 11483
		internal string _key;

		// Token: 0x04002CDC RID: 11484
		internal RegexCode _code;

		// Token: 0x04002CDD RID: 11485
		internal Hashtable _caps;

		// Token: 0x04002CDE RID: 11486
		internal Hashtable _capnames;

		// Token: 0x04002CDF RID: 11487
		internal string[] _capslist;

		// Token: 0x04002CE0 RID: 11488
		internal int _capsize;

		// Token: 0x04002CE1 RID: 11489
		internal RegexRunnerFactory _factory;

		// Token: 0x04002CE2 RID: 11490
		internal ExclusiveReference _runnerref;

		// Token: 0x04002CE3 RID: 11491
		internal SharedReference _replref;
	}
}
