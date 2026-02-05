using System;
using UnityEngine;

namespace DynamicOpenVR.Logging
{
	// Token: 0x020000D5 RID: 213
	internal class UnityDebugLogHandler : ILogHandler
	{
		// Token: 0x060001DC RID: 476 RVA: 0x0000600A File Offset: 0x0000420A
		public void Trace(object message)
		{
			global::UnityEngine.Debug.Log(message);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x00006012 File Offset: 0x00004212
		public void Debug(object message)
		{
			global::UnityEngine.Debug.Log(message);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000601A File Offset: 0x0000421A
		public void Info(object message)
		{
			global::UnityEngine.Debug.Log(message);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00006022 File Offset: 0x00004222
		public void Notice(object message)
		{
			global::UnityEngine.Debug.Log(message);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000602A File Offset: 0x0000422A
		public void Warn(object message)
		{
			global::UnityEngine.Debug.LogWarning(message);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00006032 File Offset: 0x00004232
		public void Error(object message)
		{
			global::UnityEngine.Debug.LogError(message);
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000603A File Offset: 0x0000423A
		public void Critical(object message)
		{
			global::UnityEngine.Debug.LogError(message);
		}
	}
}
