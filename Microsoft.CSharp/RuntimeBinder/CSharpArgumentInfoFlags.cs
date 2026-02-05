using System;
using System.ComponentModel;

namespace Microsoft.CSharp.RuntimeBinder
{
	// Token: 0x02000009 RID: 9
	[Flags]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public enum CSharpArgumentInfoFlags
	{
		// Token: 0x04000094 RID: 148
		None = 0,
		// Token: 0x04000095 RID: 149
		UseCompileTimeType = 1,
		// Token: 0x04000096 RID: 150
		Constant = 2,
		// Token: 0x04000097 RID: 151
		NamedArgument = 4,
		// Token: 0x04000098 RID: 152
		IsRef = 8,
		// Token: 0x04000099 RID: 153
		IsOut = 16,
		// Token: 0x0400009A RID: 154
		IsStaticType = 32
	}
}
