using System;
using System.Collections.Generic;

namespace System.Runtime.Serialization
{
	// Token: 0x0200006E RID: 110
	internal interface IGenericNameProvider
	{
		// Token: 0x06000870 RID: 2160
		int GetParameterCount();

		// Token: 0x06000871 RID: 2161
		IList<int> GetNestedParameterCounts();

		// Token: 0x06000872 RID: 2162
		string GetParameterName(int paramIndex);

		// Token: 0x06000873 RID: 2163
		string GetNamespaces();

		// Token: 0x06000874 RID: 2164
		string GetGenericTypeName();

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000875 RID: 2165
		bool ParametersFromBuiltInNamespaces { get; }
	}
}
