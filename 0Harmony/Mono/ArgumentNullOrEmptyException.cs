using System;

namespace Mono
{
	// Token: 0x020000B0 RID: 176
	internal class ArgumentNullOrEmptyException : ArgumentException
	{
		// Token: 0x0600035E RID: 862 RVA: 0x0001020C File Offset: 0x0000E40C
		public ArgumentNullOrEmptyException(string paramName)
			: base("Argument null or empty", paramName)
		{
		}
	}
}
