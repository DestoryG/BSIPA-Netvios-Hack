using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000579 RID: 1401
	public interface ISynchronizeInvoke
	{
		// Token: 0x17000CB0 RID: 3248
		// (get) Token: 0x060033E3 RID: 13283
		bool InvokeRequired { get; }

		// Token: 0x060033E4 RID: 13284
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
		IAsyncResult BeginInvoke(Delegate method, object[] args);

		// Token: 0x060033E5 RID: 13285
		object EndInvoke(IAsyncResult result);

		// Token: 0x060033E6 RID: 13286
		object Invoke(Delegate method, object[] args);
	}
}
