using System;
using System.Collections;
using System.Configuration;
using System.Threading;

namespace System.Net.Configuration
{
	// Token: 0x0200032E RID: 814
	internal sealed class ConnectionManagementSectionInternal
	{
		// Token: 0x06001D28 RID: 7464 RVA: 0x0008ACE4 File Offset: 0x00088EE4
		internal ConnectionManagementSectionInternal(ConnectionManagementSection section)
		{
			if (section.ConnectionManagement.Count > 0)
			{
				this.connectionManagement = new Hashtable(section.ConnectionManagement.Count);
				foreach (object obj in section.ConnectionManagement)
				{
					ConnectionManagementElement connectionManagementElement = (ConnectionManagementElement)obj;
					this.connectionManagement[connectionManagementElement.Address] = connectionManagementElement.MaxConnection;
				}
			}
		}

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x06001D29 RID: 7465 RVA: 0x0008AD7C File Offset: 0x00088F7C
		internal Hashtable ConnectionManagement
		{
			get
			{
				Hashtable hashtable = this.connectionManagement;
				if (hashtable == null)
				{
					hashtable = new Hashtable();
				}
				return hashtable;
			}
		}

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x06001D2A RID: 7466 RVA: 0x0008AD9C File Offset: 0x00088F9C
		internal static object ClassSyncObject
		{
			get
			{
				if (ConnectionManagementSectionInternal.classSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref ConnectionManagementSectionInternal.classSyncObject, obj, null);
				}
				return ConnectionManagementSectionInternal.classSyncObject;
			}
		}

		// Token: 0x06001D2B RID: 7467 RVA: 0x0008ADC8 File Offset: 0x00088FC8
		internal static ConnectionManagementSectionInternal GetSection()
		{
			object obj = ConnectionManagementSectionInternal.ClassSyncObject;
			ConnectionManagementSectionInternal connectionManagementSectionInternal;
			lock (obj)
			{
				ConnectionManagementSection connectionManagementSection = PrivilegedConfigurationManager.GetSection(ConfigurationStrings.ConnectionManagementSectionPath) as ConnectionManagementSection;
				if (connectionManagementSection == null)
				{
					connectionManagementSectionInternal = null;
				}
				else
				{
					connectionManagementSectionInternal = new ConnectionManagementSectionInternal(connectionManagementSection);
				}
			}
			return connectionManagementSectionInternal;
		}

		// Token: 0x04001C1E RID: 7198
		private Hashtable connectionManagement;

		// Token: 0x04001C1F RID: 7199
		private static object classSyncObject;
	}
}
