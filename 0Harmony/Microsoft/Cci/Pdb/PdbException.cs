using System;
using System.IO;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x02000306 RID: 774
	internal class PdbException : IOException
	{
		// Token: 0x060011FB RID: 4603 RVA: 0x0003AF1D File Offset: 0x0003911D
		internal PdbException(string format, params object[] args)
			: base(string.Format(format, args))
		{
		}
	}
}
