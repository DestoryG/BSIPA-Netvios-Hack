using System;

namespace Mono.Cecil
{
	// Token: 0x0200011F RID: 287
	internal interface IGenericContext
	{
		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060007F7 RID: 2039
		bool IsDefinition { get; }

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060007F8 RID: 2040
		IGenericParameterProvider Type { get; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060007F9 RID: 2041
		IGenericParameterProvider Method { get; }
	}
}
