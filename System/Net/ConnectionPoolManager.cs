using System;
using System.Collections;
using System.Globalization;
using System.Threading;

namespace System.Net
{
	// Token: 0x020000CF RID: 207
	internal class ConnectionPoolManager
	{
		// Token: 0x060006E4 RID: 1764 RVA: 0x000260A0 File Offset: 0x000242A0
		private ConnectionPoolManager()
		{
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060006E5 RID: 1765 RVA: 0x000260A8 File Offset: 0x000242A8
		private static object InternalSyncObject
		{
			get
			{
				if (ConnectionPoolManager.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref ConnectionPoolManager.s_InternalSyncObject, obj, null);
				}
				return ConnectionPoolManager.s_InternalSyncObject;
			}
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x000260D4 File Offset: 0x000242D4
		private static string GenerateKey(string hostName, int port, string groupName)
		{
			return string.Concat(new string[]
			{
				hostName,
				"\r",
				port.ToString(NumberFormatInfo.InvariantInfo),
				"\r",
				groupName
			});
		}

		// Token: 0x060006E7 RID: 1767 RVA: 0x00026108 File Offset: 0x00024308
		internal static ConnectionPool GetConnectionPool(ServicePoint servicePoint, string groupName, CreateConnectionDelegate createConnectionCallback)
		{
			string text = ConnectionPoolManager.GenerateKey(servicePoint.Host, servicePoint.Port, groupName);
			object internalSyncObject = ConnectionPoolManager.InternalSyncObject;
			ConnectionPool connectionPool2;
			lock (internalSyncObject)
			{
				ConnectionPool connectionPool = (ConnectionPool)ConnectionPoolManager.m_ConnectionPools[text];
				if (connectionPool == null)
				{
					connectionPool = new ConnectionPool(servicePoint, servicePoint.ConnectionLimit, 0, servicePoint.MaxIdleTime, createConnectionCallback);
					ConnectionPoolManager.m_ConnectionPools[text] = connectionPool;
				}
				connectionPool2 = connectionPool;
			}
			return connectionPool2;
		}

		// Token: 0x060006E8 RID: 1768 RVA: 0x00026190 File Offset: 0x00024390
		internal static bool RemoveConnectionPool(ServicePoint servicePoint, string groupName)
		{
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, "ConnectionPoolManager::RemoveConnectionPool, groupName=" + groupName);
			}
			string text = ConnectionPoolManager.GenerateKey(servicePoint.Host, servicePoint.Port, groupName);
			object internalSyncObject = ConnectionPoolManager.InternalSyncObject;
			lock (internalSyncObject)
			{
				ConnectionPool connectionPool = (ConnectionPool)ConnectionPoolManager.m_ConnectionPools[text];
				if (connectionPool != null)
				{
					ConnectionPoolManager.m_ConnectionPools[text] = null;
					ConnectionPoolManager.m_ConnectionPools.Remove(text);
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.Web, "ConnectionPoolManager::RemoveConnectionPool, removed connection pool: " + text);
					}
					return true;
				}
			}
			if (Logging.On)
			{
				Logging.PrintInfo(Logging.Web, "ConnectionPoolManager::RemoveConnectionPool, no connection pool found: " + text);
			}
			return false;
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x00026264 File Offset: 0x00024464
		internal static void CleanupConnectionPool(ServicePoint servicePoint, string groupName)
		{
			string text = ConnectionPoolManager.GenerateKey(servicePoint.Host, servicePoint.Port, groupName);
			object internalSyncObject = ConnectionPoolManager.InternalSyncObject;
			lock (internalSyncObject)
			{
				ConnectionPool connectionPool = (ConnectionPool)ConnectionPoolManager.m_ConnectionPools[text];
				if (connectionPool != null)
				{
					connectionPool.ForceCleanup();
				}
			}
		}

		// Token: 0x04000CB4 RID: 3252
		private static Hashtable m_ConnectionPools = new Hashtable();

		// Token: 0x04000CB5 RID: 3253
		private static object s_InternalSyncObject;
	}
}
