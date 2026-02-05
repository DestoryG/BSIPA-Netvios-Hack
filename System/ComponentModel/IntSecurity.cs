using System;
using System.IO;
using System.Security;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000571 RID: 1393
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	internal static class IntSecurity
	{
		// Token: 0x060033CC RID: 13260 RVA: 0x000E3E3C File Offset: 0x000E203C
		public static string UnsafeGetFullPath(string fileName)
		{
			string text = fileName;
			new FileIOPermission(PermissionState.None)
			{
				AllFiles = FileIOPermissionAccess.PathDiscovery
			}.Assert();
			try
			{
				text = Path.GetFullPath(fileName);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return text;
		}

		// Token: 0x040029B8 RID: 10680
		public static readonly CodeAccessPermission UnmanagedCode = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);

		// Token: 0x040029B9 RID: 10681
		public static readonly CodeAccessPermission FullReflection = new ReflectionPermission(PermissionState.Unrestricted);
	}
}
