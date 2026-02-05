using System;
using System.IO;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000305 RID: 773
	internal class PdbDebugException : IOException
	{
		// Token: 0x060011FA RID: 4602 RVA: 0x0003AF1D File Offset: 0x0003911D
		internal PdbDebugException(string format, params object[] args)
			: base(string.Format(format, args))
		{
		}
	}
}
