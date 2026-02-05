using System;
using System.Reflection;

namespace BeatSaberMarkupLanguage.Parser
{
	// Token: 0x02000070 RID: 112
	public class BSMLAction
	{
		// Token: 0x060001E3 RID: 483 RVA: 0x0000C009 File Offset: 0x0000A209
		public BSMLAction(object host, MethodInfo methodInfo)
		{
			this.host = host;
			this.methodInfo = methodInfo;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000C01F File Offset: 0x0000A21F
		public object Invoke(params object[] parameters)
		{
			return this.methodInfo.Invoke(this.host, parameters);
		}

		// Token: 0x0400004D RID: 77
		private object host;

		// Token: 0x0400004E RID: 78
		private MethodInfo methodInfo;
	}
}
