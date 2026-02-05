using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200057C RID: 1404
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public abstract class License : IDisposable
	{
		// Token: 0x17000CB4 RID: 3252
		// (get) Token: 0x060033EE RID: 13294
		public abstract string LicenseKey { get; }

		// Token: 0x060033EF RID: 13295
		public abstract void Dispose();
	}
}
