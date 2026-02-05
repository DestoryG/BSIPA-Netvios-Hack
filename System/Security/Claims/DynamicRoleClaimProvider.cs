using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace System.Security.Claims
{
	// Token: 0x02000439 RID: 1081
	public static class DynamicRoleClaimProvider
	{
		// Token: 0x06002875 RID: 10357 RVA: 0x000B9DC8 File Offset: 0x000B7FC8
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Use ClaimsAuthenticationManager to add claims to a ClaimsIdentity", true)]
		public static void AddDynamicRoleClaims(ClaimsIdentity claimsIdentity, IEnumerable<Claim> claims)
		{
			claimsIdentity.ExternalClaims.Add(claims);
		}
	}
}
