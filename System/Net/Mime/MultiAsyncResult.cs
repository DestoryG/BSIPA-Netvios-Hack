using System;
using System.Threading;

namespace System.Net.Mime
{
	// Token: 0x0200024E RID: 590
	internal class MultiAsyncResult : LazyAsyncResult
	{
		// Token: 0x06001660 RID: 5728 RVA: 0x00073E4C File Offset: 0x0007204C
		internal MultiAsyncResult(object context, AsyncCallback callback, object state)
			: base(context, state, callback)
		{
			this.context = context;
		}

		// Token: 0x170004AE RID: 1198
		// (get) Token: 0x06001661 RID: 5729 RVA: 0x00073E5E File Offset: 0x0007205E
		internal object Context
		{
			get
			{
				return this.context;
			}
		}

		// Token: 0x06001662 RID: 5730 RVA: 0x00073E66 File Offset: 0x00072066
		internal void Enter()
		{
			this.Increment();
		}

		// Token: 0x06001663 RID: 5731 RVA: 0x00073E6E File Offset: 0x0007206E
		internal void Leave()
		{
			this.Decrement();
		}

		// Token: 0x06001664 RID: 5732 RVA: 0x00073E76 File Offset: 0x00072076
		internal void Leave(object result)
		{
			base.Result = result;
			this.Decrement();
		}

		// Token: 0x06001665 RID: 5733 RVA: 0x00073E85 File Offset: 0x00072085
		private void Decrement()
		{
			if (Interlocked.Decrement(ref this.outstanding) == -1)
			{
				base.InvokeCallback(base.Result);
			}
		}

		// Token: 0x06001666 RID: 5734 RVA: 0x00073EA1 File Offset: 0x000720A1
		private void Increment()
		{
			Interlocked.Increment(ref this.outstanding);
		}

		// Token: 0x06001667 RID: 5735 RVA: 0x00073EAF File Offset: 0x000720AF
		internal void CompleteSequence()
		{
			this.Decrement();
		}

		// Token: 0x06001668 RID: 5736 RVA: 0x00073EB8 File Offset: 0x000720B8
		internal static object End(IAsyncResult result)
		{
			MultiAsyncResult multiAsyncResult = (MultiAsyncResult)result;
			multiAsyncResult.InternalWaitForCompletion();
			return multiAsyncResult.Result;
		}

		// Token: 0x04001734 RID: 5940
		private int outstanding;

		// Token: 0x04001735 RID: 5941
		private object context;
	}
}
