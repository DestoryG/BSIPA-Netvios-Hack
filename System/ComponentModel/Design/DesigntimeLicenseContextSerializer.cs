using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x020005DC RID: 1500
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class DesigntimeLicenseContextSerializer
	{
		// Token: 0x060037B9 RID: 14265 RVA: 0x000F0C76 File Offset: 0x000EEE76
		private DesigntimeLicenseContextSerializer()
		{
		}

		// Token: 0x060037BA RID: 14266 RVA: 0x000F0C80 File Offset: 0x000EEE80
		public static void Serialize(Stream o, string cryptoKey, DesigntimeLicenseContext context)
		{
			IFormatter formatter = new BinaryFormatter();
			formatter.Serialize(o, new object[] { cryptoKey, context.savedLicenseKeys });
		}

		// Token: 0x060037BB RID: 14267 RVA: 0x000F0CB0 File Offset: 0x000EEEB0
		internal static void Deserialize(Stream o, string cryptoKey, RuntimeLicenseContext context)
		{
			IFormatter formatter = new BinaryFormatter();
			object obj = formatter.Deserialize(o);
			if (obj is object[])
			{
				object[] array = (object[])obj;
				if (array[0] is string && (string)array[0] == cryptoKey)
				{
					context.savedLicenseKeys = (Hashtable)array[1];
				}
			}
		}
	}
}
