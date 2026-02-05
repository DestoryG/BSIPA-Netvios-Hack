using System;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;

namespace System.Net
{
	// Token: 0x020001A9 RID: 425
	internal class ContextAwareResult : LazyAsyncResult
	{
		// Token: 0x060010B8 RID: 4280 RVA: 0x00059B46 File Offset: 0x00057D46
		internal ContextAwareResult(object myObject, object myState, AsyncCallback myCallBack)
			: this(false, false, myObject, myState, myCallBack)
		{
		}

		// Token: 0x060010B9 RID: 4281 RVA: 0x00059B53 File Offset: 0x00057D53
		internal ContextAwareResult(bool captureIdentity, bool forceCaptureContext, object myObject, object myState, AsyncCallback myCallBack)
			: this(captureIdentity, forceCaptureContext, false, myObject, myState, myCallBack)
		{
		}

		// Token: 0x060010BA RID: 4282 RVA: 0x00059B63 File Offset: 0x00057D63
		internal ContextAwareResult(bool captureIdentity, bool forceCaptureContext, bool threadSafeContextCopy, object myObject, object myState, AsyncCallback myCallBack)
			: base(myObject, myState, myCallBack)
		{
			if (forceCaptureContext)
			{
				this._Flags = ContextAwareResult.StateFlags.CaptureContext;
			}
			if (captureIdentity)
			{
				this._Flags |= ContextAwareResult.StateFlags.CaptureIdentity;
			}
			if (threadSafeContextCopy)
			{
				this._Flags |= ContextAwareResult.StateFlags.ThreadSafeContextCopy;
			}
		}

		// Token: 0x060010BB RID: 4283 RVA: 0x00059B9D File Offset: 0x00057D9D
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.ControlPrincipal)]
		private void SafeCaptureIdenity()
		{
			this._Wi = WindowsIdentity.GetCurrent();
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x060010BC RID: 4284 RVA: 0x00059BAC File Offset: 0x00057DAC
		internal ExecutionContext ContextCopy
		{
			get
			{
				if (base.InternalPeekCompleted)
				{
					throw new InvalidOperationException(SR.GetString("net_completed_result"));
				}
				ExecutionContext executionContext = this._Context;
				if (executionContext != null)
				{
					return executionContext.CreateCopy();
				}
				if ((this._Flags & ContextAwareResult.StateFlags.PostBlockFinished) == ContextAwareResult.StateFlags.None)
				{
					object @lock = this._Lock;
					lock (@lock)
					{
					}
				}
				if (base.InternalPeekCompleted)
				{
					throw new InvalidOperationException(SR.GetString("net_completed_result"));
				}
				executionContext = this._Context;
				if (executionContext != null)
				{
					return executionContext.CreateCopy();
				}
				return null;
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x060010BD RID: 4285 RVA: 0x00059C48 File Offset: 0x00057E48
		internal WindowsIdentity Identity
		{
			get
			{
				if (base.InternalPeekCompleted)
				{
					throw new InvalidOperationException(SR.GetString("net_completed_result"));
				}
				if (this._Wi != null)
				{
					return this._Wi;
				}
				if ((this._Flags & ContextAwareResult.StateFlags.PostBlockFinished) == ContextAwareResult.StateFlags.None)
				{
					object @lock = this._Lock;
					lock (@lock)
					{
					}
				}
				if (base.InternalPeekCompleted)
				{
					throw new InvalidOperationException(SR.GetString("net_completed_result"));
				}
				return this._Wi;
			}
		}

		// Token: 0x060010BE RID: 4286 RVA: 0x00059CD4 File Offset: 0x00057ED4
		internal object StartPostingAsyncOp()
		{
			return this.StartPostingAsyncOp(true);
		}

		// Token: 0x060010BF RID: 4287 RVA: 0x00059CDD File Offset: 0x00057EDD
		internal object StartPostingAsyncOp(bool lockCapture)
		{
			this._Lock = (lockCapture ? new object() : null);
			this._Flags |= ContextAwareResult.StateFlags.PostBlockStarted;
			return this._Lock;
		}

		// Token: 0x060010C0 RID: 4288 RVA: 0x00059D04 File Offset: 0x00057F04
		internal bool FinishPostingAsyncOp()
		{
			if ((this._Flags & (ContextAwareResult.StateFlags.PostBlockStarted | ContextAwareResult.StateFlags.PostBlockFinished)) != ContextAwareResult.StateFlags.PostBlockStarted)
			{
				return false;
			}
			this._Flags |= ContextAwareResult.StateFlags.PostBlockFinished;
			ExecutionContext executionContext = null;
			return this.CaptureOrComplete(ref executionContext, false);
		}

		// Token: 0x060010C1 RID: 4289 RVA: 0x00059D3C File Offset: 0x00057F3C
		internal bool FinishPostingAsyncOp(ref CallbackClosure closure)
		{
			if ((this._Flags & (ContextAwareResult.StateFlags.PostBlockStarted | ContextAwareResult.StateFlags.PostBlockFinished)) != ContextAwareResult.StateFlags.PostBlockStarted)
			{
				return false;
			}
			this._Flags |= ContextAwareResult.StateFlags.PostBlockFinished;
			CallbackClosure callbackClosure = closure;
			ExecutionContext executionContext;
			if (callbackClosure == null)
			{
				executionContext = null;
			}
			else if (!callbackClosure.IsCompatible(base.AsyncCallback))
			{
				closure = null;
				executionContext = null;
			}
			else
			{
				base.AsyncCallback = callbackClosure.AsyncCallback;
				executionContext = callbackClosure.Context;
			}
			bool flag = this.CaptureOrComplete(ref executionContext, true);
			if (closure == null && base.AsyncCallback != null && executionContext != null)
			{
				closure = new CallbackClosure(executionContext, base.AsyncCallback);
			}
			return flag;
		}

		// Token: 0x060010C2 RID: 4290 RVA: 0x00059DC0 File Offset: 0x00057FC0
		protected override void Cleanup()
		{
			base.Cleanup();
			if (this._Wi != null)
			{
				this._Wi.Dispose();
				this._Wi = null;
			}
		}

		// Token: 0x060010C3 RID: 4291 RVA: 0x00059DE4 File Offset: 0x00057FE4
		private bool CaptureOrComplete(ref ExecutionContext cachedContext, bool returnContext)
		{
			bool flag = base.AsyncCallback != null || (this._Flags & ContextAwareResult.StateFlags.CaptureContext) > ContextAwareResult.StateFlags.None;
			if ((this._Flags & ContextAwareResult.StateFlags.CaptureIdentity) != ContextAwareResult.StateFlags.None && !base.InternalPeekCompleted && (!flag || SecurityContext.IsWindowsIdentityFlowSuppressed()))
			{
				this.SafeCaptureIdenity();
			}
			if (flag && !base.InternalPeekCompleted)
			{
				if (cachedContext == null)
				{
					cachedContext = ExecutionContext.Capture();
				}
				if (cachedContext != null)
				{
					if (!returnContext)
					{
						this._Context = cachedContext;
						cachedContext = null;
					}
					else
					{
						this._Context = cachedContext.CreateCopy();
					}
				}
			}
			else
			{
				cachedContext = null;
			}
			if (base.CompletedSynchronously)
			{
				base.Complete(IntPtr.Zero);
				return true;
			}
			return false;
		}

		// Token: 0x060010C4 RID: 4292 RVA: 0x00059E80 File Offset: 0x00058080
		protected override void Complete(IntPtr userToken)
		{
			if ((this._Flags & ContextAwareResult.StateFlags.PostBlockStarted) == ContextAwareResult.StateFlags.None)
			{
				base.Complete(userToken);
				return;
			}
			if (base.CompletedSynchronously)
			{
				return;
			}
			ExecutionContext context = this._Context;
			if (userToken != IntPtr.Zero || context == null)
			{
				base.Complete(userToken);
				return;
			}
			ExecutionContext.Run(((this._Flags & ContextAwareResult.StateFlags.ThreadSafeContextCopy) != ContextAwareResult.StateFlags.None) ? context.CreateCopy() : context, new ContextCallback(this.CompleteCallback), null);
		}

		// Token: 0x060010C5 RID: 4293 RVA: 0x00059EEE File Offset: 0x000580EE
		private void CompleteCallback(object state)
		{
			base.Complete(IntPtr.Zero);
		}

		// Token: 0x040013A2 RID: 5026
		private volatile ExecutionContext _Context;

		// Token: 0x040013A3 RID: 5027
		private object _Lock;

		// Token: 0x040013A4 RID: 5028
		private ContextAwareResult.StateFlags _Flags;

		// Token: 0x040013A5 RID: 5029
		private WindowsIdentity _Wi;

		// Token: 0x0200074D RID: 1869
		[Flags]
		private enum StateFlags
		{
			// Token: 0x040031EB RID: 12779
			None = 0,
			// Token: 0x040031EC RID: 12780
			CaptureIdentity = 1,
			// Token: 0x040031ED RID: 12781
			CaptureContext = 2,
			// Token: 0x040031EE RID: 12782
			ThreadSafeContextCopy = 4,
			// Token: 0x040031EF RID: 12783
			PostBlockStarted = 8,
			// Token: 0x040031F0 RID: 12784
			PostBlockFinished = 16
		}
	}
}
