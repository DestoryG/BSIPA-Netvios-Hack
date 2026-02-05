using System;

namespace Mono
{
	// Token: 0x02000006 RID: 6
	internal class ArgumentNullOrEmptyException : ArgumentException
	{
		// Token: 0x06000006 RID: 6 RVA: 0x000020A6 File Offset: 0x000002A6
		public ArgumentNullOrEmptyException(string paramName)
			: base("Argument null or empty", paramName)
		{
		}
	}
}
