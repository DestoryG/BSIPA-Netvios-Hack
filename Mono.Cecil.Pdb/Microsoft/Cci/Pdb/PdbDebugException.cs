using System;
using System.IO;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020000D9 RID: 217
	internal class PdbDebugException : IOException
	{
		// Token: 0x0600018A RID: 394 RVA: 0x00004F79 File Offset: 0x00003179
		internal PdbDebugException(string format, params object[] args)
			: base(string.Format(format, args))
		{
		}
	}
}
