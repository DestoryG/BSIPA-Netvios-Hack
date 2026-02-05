using System;

namespace System
{
	// Token: 0x02000002 RID: 2
	internal static class NotImplemented
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		internal static Exception ByDesign
		{
			get
			{
				return new NotImplementedException();
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002057 File Offset: 0x00000257
		internal static Exception ByDesignWithMessage(string message)
		{
			return new NotImplementedException(message);
		}
	}
}
