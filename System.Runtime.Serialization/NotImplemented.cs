using System;

namespace System
{
	// Token: 0x02000004 RID: 4
	internal static class NotImplemented
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000B RID: 11 RVA: 0x000020B3 File Offset: 0x000002B3
		internal static Exception ByDesign
		{
			get
			{
				return new NotImplementedException();
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000020BA File Offset: 0x000002BA
		internal static Exception ByDesignWithMessage(string message)
		{
			return new NotImplementedException(message);
		}
	}
}
