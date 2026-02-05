using System;

namespace Microsoft.CSharp.RuntimeBinder
{
	// Token: 0x0200001C RID: 28
	internal interface ICSharpInvokeOrInvokeMemberBinder : ICSharpBinder
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000E6 RID: 230
		bool StaticCall { get; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000E7 RID: 231
		bool ResultDiscarded { get; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000E8 RID: 232
		CSharpCallFlags Flags { get; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000E9 RID: 233
		Type[] TypeArguments { get; }
	}
}
