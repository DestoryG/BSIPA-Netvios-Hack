using System;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x0200011B RID: 283
	internal static class ExceptionHelper
	{
		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000B2C RID: 2860 RVA: 0x0003D91C File Offset: 0x0003BB1C
		internal static NotImplementedException MethodNotImplementedException
		{
			get
			{
				return new NotImplementedException(SR.GetString("net_MethodNotImplementedException"));
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000B2D RID: 2861 RVA: 0x0003D92D File Offset: 0x0003BB2D
		internal static NotImplementedException PropertyNotImplementedException
		{
			get
			{
				return new NotImplementedException(SR.GetString("net_PropertyNotImplementedException"));
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000B2E RID: 2862 RVA: 0x0003D93E File Offset: 0x0003BB3E
		internal static NotSupportedException MethodNotSupportedException
		{
			get
			{
				return new NotSupportedException(SR.GetString("net_MethodNotSupportedException"));
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000B2F RID: 2863 RVA: 0x0003D94F File Offset: 0x0003BB4F
		internal static NotSupportedException PropertyNotSupportedException
		{
			get
			{
				return new NotSupportedException(SR.GetString("net_PropertyNotSupportedException"));
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000B30 RID: 2864 RVA: 0x0003D960 File Offset: 0x0003BB60
		internal static WebException IsolatedException
		{
			get
			{
				return new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.KeepAliveFailure), WebExceptionStatus.KeepAliveFailure, WebExceptionInternalStatus.Isolated, null);
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000B31 RID: 2865 RVA: 0x0003D977 File Offset: 0x0003BB77
		internal static WebException RequestAbortedException
		{
			get
			{
				return new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.RequestCanceled), WebExceptionStatus.RequestCanceled);
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000B32 RID: 2866 RVA: 0x0003D98A File Offset: 0x0003BB8A
		internal static WebException CacheEntryNotFoundException
		{
			get
			{
				return new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.CacheEntryNotFound), WebExceptionStatus.CacheEntryNotFound);
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000B33 RID: 2867 RVA: 0x0003D99F File Offset: 0x0003BB9F
		internal static WebException RequestProhibitedByCachePolicyException
		{
			get
			{
				return new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.RequestProhibitedByCachePolicy), WebExceptionStatus.RequestProhibitedByCachePolicy);
			}
		}

		// Token: 0x04000F79 RID: 3961
		internal static readonly KeyContainerPermission KeyContainerPermissionOpen = new KeyContainerPermission(KeyContainerPermissionFlags.Open);

		// Token: 0x04000F7A RID: 3962
		internal static readonly WebPermission WebPermissionUnrestricted = new WebPermission(NetworkAccess.Connect);

		// Token: 0x04000F7B RID: 3963
		internal static readonly SecurityPermission UnmanagedPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);

		// Token: 0x04000F7C RID: 3964
		internal static readonly SocketPermission UnrestrictedSocketPermission = new SocketPermission(PermissionState.Unrestricted);

		// Token: 0x04000F7D RID: 3965
		internal static readonly SecurityPermission InfrastructurePermission = new SecurityPermission(SecurityPermissionFlag.Infrastructure);

		// Token: 0x04000F7E RID: 3966
		internal static readonly SecurityPermission ControlPolicyPermission = new SecurityPermission(SecurityPermissionFlag.ControlPolicy);

		// Token: 0x04000F7F RID: 3967
		internal static readonly SecurityPermission ControlPrincipalPermission = new SecurityPermission(SecurityPermissionFlag.ControlPrincipal);
	}
}
