using System;
using System.IO;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x020005AE RID: 1454
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public static class SyntaxCheck
	{
		// Token: 0x06003624 RID: 13860 RVA: 0x000EC3DE File Offset: 0x000EA5DE
		public static bool CheckMachineName(string value)
		{
			if (value == null)
			{
				return false;
			}
			value = value.Trim();
			return !value.Equals(string.Empty) && value.IndexOf('\\') == -1;
		}

		// Token: 0x06003625 RID: 13861 RVA: 0x000EC407 File Offset: 0x000EA607
		public static bool CheckPath(string value)
		{
			if (value == null)
			{
				return false;
			}
			value = value.Trim();
			return !value.Equals(string.Empty) && value.StartsWith("\\\\");
		}

		// Token: 0x06003626 RID: 13862 RVA: 0x000EC430 File Offset: 0x000EA630
		public static bool CheckRootedPath(string value)
		{
			if (value == null)
			{
				return false;
			}
			value = value.Trim();
			return !value.Equals(string.Empty) && Path.IsPathRooted(value);
		}
	}
}
