using System;

namespace System.Text.RegularExpressions
{
	// Token: 0x020006A9 RID: 1705
	internal sealed class CompiledRegexRunner : RegexRunner
	{
		// Token: 0x06003FC7 RID: 16327 RVA: 0x0010C77D File Offset: 0x0010A97D
		internal CompiledRegexRunner()
		{
		}

		// Token: 0x06003FC8 RID: 16328 RVA: 0x0010C785 File Offset: 0x0010A985
		internal void SetDelegates(NoParamDelegate go, FindFirstCharDelegate firstChar, NoParamDelegate trackCount)
		{
			this.goMethod = go;
			this.findFirstCharMethod = firstChar;
			this.initTrackCountMethod = trackCount;
		}

		// Token: 0x06003FC9 RID: 16329 RVA: 0x0010C79C File Offset: 0x0010A99C
		protected override void Go()
		{
			this.goMethod(this);
		}

		// Token: 0x06003FCA RID: 16330 RVA: 0x0010C7AA File Offset: 0x0010A9AA
		protected override bool FindFirstChar()
		{
			return this.findFirstCharMethod(this);
		}

		// Token: 0x06003FCB RID: 16331 RVA: 0x0010C7B8 File Offset: 0x0010A9B8
		protected override void InitTrackCount()
		{
			this.initTrackCountMethod(this);
		}

		// Token: 0x04002E7F RID: 11903
		private NoParamDelegate goMethod;

		// Token: 0x04002E80 RID: 11904
		private FindFirstCharDelegate findFirstCharMethod;

		// Token: 0x04002E81 RID: 11905
		private NoParamDelegate initTrackCountMethod;
	}
}
