using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;

namespace System
{
	// Token: 0x02000061 RID: 97
	internal sealed class Gen2GcCallback : CriticalFinalizerObject
	{
		// Token: 0x06000430 RID: 1072 RVA: 0x0001E3D0 File Offset: 0x0001C5D0
		[SecuritySafeCritical]
		public Gen2GcCallback()
		{
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x0001E3D8 File Offset: 0x0001C5D8
		public static void Register(Func<object, bool> callback, object targetObj)
		{
			Gen2GcCallback gen2GcCallback = new Gen2GcCallback();
			gen2GcCallback.Setup(callback, targetObj);
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x0001E3F3 File Offset: 0x0001C5F3
		[SecuritySafeCritical]
		private void Setup(Func<object, bool> callback, object targetObj)
		{
			this.m_callback = callback;
			this.m_weakTargetObj = GCHandle.Alloc(targetObj, GCHandleType.Weak);
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x0001E40C File Offset: 0x0001C60C
		[SecuritySafeCritical]
		protected override void Finalize()
		{
			try
			{
				if (this.m_weakTargetObj.IsAllocated)
				{
					object target = this.m_weakTargetObj.Target;
					if (target == null)
					{
						this.m_weakTargetObj.Free();
					}
					else
					{
						try
						{
							if (!this.m_callback(target))
							{
								return;
							}
						}
						catch
						{
						}
						if (!Environment.HasShutdownStarted && !AppDomain.CurrentDomain.IsFinalizingForUnload())
						{
							GC.ReRegisterForFinalize(this);
						}
					}
				}
			}
			finally
			{
				base.Finalize();
			}
		}

		// Token: 0x0400052C RID: 1324
		private Func<object, bool> m_callback;

		// Token: 0x0400052D RID: 1325
		private GCHandle m_weakTargetObj;
	}
}
