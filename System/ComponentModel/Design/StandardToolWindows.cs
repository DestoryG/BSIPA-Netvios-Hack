using System;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x02000601 RID: 1537
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class StandardToolWindows
	{
		// Token: 0x04002B5C RID: 11100
		public static readonly Guid ObjectBrowser = new Guid("{970d9861-ee83-11d0-a778-00a0c91110c3}");

		// Token: 0x04002B5D RID: 11101
		public static readonly Guid OutputWindow = new Guid("{34e76e81-ee4a-11d0-ae2e-00a0c90fffc3}");

		// Token: 0x04002B5E RID: 11102
		public static readonly Guid ProjectExplorer = new Guid("{3ae79031-e1bc-11d0-8f78-00a0c9110057}");

		// Token: 0x04002B5F RID: 11103
		public static readonly Guid PropertyBrowser = new Guid("{eefa5220-e298-11d0-8f78-00a0c9110057}");

		// Token: 0x04002B60 RID: 11104
		public static readonly Guid RelatedLinks = new Guid("{66dba47c-61df-11d2-aa79-00c04f990343}");

		// Token: 0x04002B61 RID: 11105
		public static readonly Guid ServerExplorer = new Guid("{74946827-37a0-11d2-a273-00c04f8ef4ff}");

		// Token: 0x04002B62 RID: 11106
		public static readonly Guid TaskList = new Guid("{4a9b7e51-aa16-11d0-a8c5-00a0c921a4d2}");

		// Token: 0x04002B63 RID: 11107
		public static readonly Guid Toolbox = new Guid("{b1e99781-ab81-11d0-b683-00aa00a3ee26}");
	}
}
