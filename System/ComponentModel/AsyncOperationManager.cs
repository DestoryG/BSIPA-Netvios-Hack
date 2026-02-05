using System;
using System.Security.Permissions;
using System.Threading;

namespace System.ComponentModel
{
	// Token: 0x02000514 RID: 1300
	[global::__DynamicallyInvokable]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public static class AsyncOperationManager
	{
		// Token: 0x06003132 RID: 12594 RVA: 0x000DEC3E File Offset: 0x000DCE3E
		[global::__DynamicallyInvokable]
		public static AsyncOperation CreateOperation(object userSuppliedState)
		{
			return AsyncOperation.CreateOperation(userSuppliedState, AsyncOperationManager.SynchronizationContext);
		}

		// Token: 0x17000C06 RID: 3078
		// (get) Token: 0x06003133 RID: 12595 RVA: 0x000DEC4B File Offset: 0x000DCE4B
		// (set) Token: 0x06003134 RID: 12596 RVA: 0x000DEC63 File Offset: 0x000DCE63
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		[global::__DynamicallyInvokable]
		public static SynchronizationContext SynchronizationContext
		{
			[global::__DynamicallyInvokable]
			get
			{
				if (SynchronizationContext.Current == null)
				{
					SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
				}
				return SynchronizationContext.Current;
			}
			[global::__DynamicallyInvokable]
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set
			{
				SynchronizationContext.SetSynchronizationContext(value);
			}
		}
	}
}
