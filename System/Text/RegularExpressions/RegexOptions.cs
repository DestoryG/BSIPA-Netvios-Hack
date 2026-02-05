using System;

namespace System.Text.RegularExpressions
{
	// Token: 0x020006A2 RID: 1698
	[Flags]
	[global::__DynamicallyInvokable]
	public enum RegexOptions
	{
		// Token: 0x04002E2C RID: 11820
		[global::__DynamicallyInvokable]
		None = 0,
		// Token: 0x04002E2D RID: 11821
		[global::__DynamicallyInvokable]
		IgnoreCase = 1,
		// Token: 0x04002E2E RID: 11822
		[global::__DynamicallyInvokable]
		Multiline = 2,
		// Token: 0x04002E2F RID: 11823
		[global::__DynamicallyInvokable]
		ExplicitCapture = 4,
		// Token: 0x04002E30 RID: 11824
		[global::__DynamicallyInvokable]
		Compiled = 8,
		// Token: 0x04002E31 RID: 11825
		[global::__DynamicallyInvokable]
		Singleline = 16,
		// Token: 0x04002E32 RID: 11826
		[global::__DynamicallyInvokable]
		IgnorePatternWhitespace = 32,
		// Token: 0x04002E33 RID: 11827
		[global::__DynamicallyInvokable]
		RightToLeft = 64,
		// Token: 0x04002E34 RID: 11828
		[global::__DynamicallyInvokable]
		ECMAScript = 256,
		// Token: 0x04002E35 RID: 11829
		[global::__DynamicallyInvokable]
		CultureInvariant = 512
	}
}
