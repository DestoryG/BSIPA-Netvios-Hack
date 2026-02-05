using System;
using System.IO;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020000DA RID: 218
	internal class PdbException : IOException
	{
		// Token: 0x0600018B RID: 395 RVA: 0x00004F79 File Offset: 0x00003179
		internal PdbException(string format, params object[] args)
			: base(string.Format(format, args))
		{
		}
	}
}
