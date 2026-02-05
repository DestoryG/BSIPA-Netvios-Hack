using System;
using System.Reflection.Emit;
using System.Security.Permissions;

namespace System.Text.RegularExpressions
{
	// Token: 0x020006AC RID: 1708
	internal sealed class CompiledRegexRunnerFactory : RegexRunnerFactory
	{
		// Token: 0x06003FD4 RID: 16340 RVA: 0x0010C7C6 File Offset: 0x0010A9C6
		internal CompiledRegexRunnerFactory(DynamicMethod go, DynamicMethod firstChar, DynamicMethod trackCount)
		{
			this.goMethod = go;
			this.findFirstCharMethod = firstChar;
			this.initTrackCountMethod = trackCount;
		}

		// Token: 0x06003FD5 RID: 16341 RVA: 0x0010C7E4 File Offset: 0x0010A9E4
		protected internal override RegexRunner CreateInstance()
		{
			CompiledRegexRunner compiledRegexRunner = new CompiledRegexRunner();
			new ReflectionPermission(PermissionState.Unrestricted).Assert();
			compiledRegexRunner.SetDelegates((NoParamDelegate)this.goMethod.CreateDelegate(typeof(NoParamDelegate)), (FindFirstCharDelegate)this.findFirstCharMethod.CreateDelegate(typeof(FindFirstCharDelegate)), (NoParamDelegate)this.initTrackCountMethod.CreateDelegate(typeof(NoParamDelegate)));
			return compiledRegexRunner;
		}

		// Token: 0x04002E82 RID: 11906
		private DynamicMethod goMethod;

		// Token: 0x04002E83 RID: 11907
		private DynamicMethod findFirstCharMethod;

		// Token: 0x04002E84 RID: 11908
		private DynamicMethod initTrackCountMethod;
	}
}
