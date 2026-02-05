using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;

namespace System.Net
{
	// Token: 0x020001A4 RID: 420
	internal class ConnectionGroup
	{
		// Token: 0x0600103E RID: 4158 RVA: 0x00056AD8 File Offset: 0x00054CD8
		internal ConnectionGroup(ServicePoint servicePoint, string connName)
		{
			this.m_ServicePoint = servicePoint;
			this.m_ConnectionLimit = servicePoint.ConnectionLimit;
			this.m_ConnectionList = new ArrayList(3);
			this.m_Name = ConnectionGroup.MakeQueryStr(connName);
			this.m_AbortDelegate = new HttpAbortDelegate(this.Abort);
		}

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x0600103F RID: 4159 RVA: 0x00056B2F File Offset: 0x00054D2F
		internal string Name
		{
			get
			{
				return this.m_Name;
			}
		}

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x06001040 RID: 4160 RVA: 0x00056B37 File Offset: 0x00054D37
		internal ServicePoint ServicePoint
		{
			get
			{
				return this.m_ServicePoint;
			}
		}

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x06001041 RID: 4161 RVA: 0x00056B3F File Offset: 0x00054D3F
		internal int CurrentConnections
		{
			get
			{
				return this.m_ConnectionList.Count;
			}
		}

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06001042 RID: 4162 RVA: 0x00056B4C File Offset: 0x00054D4C
		// (set) Token: 0x06001043 RID: 4163 RVA: 0x00056B54 File Offset: 0x00054D54
		internal int ConnectionLimit
		{
			get
			{
				return this.m_ConnectionLimit;
			}
			set
			{
				this.m_ConnectionLimit = value;
				this.PruneExcesiveConnections();
			}
		}

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06001044 RID: 4164 RVA: 0x00056B64 File Offset: 0x00054D64
		private ManualResetEvent AsyncWaitHandle
		{
			get
			{
				if (this.m_Event == null)
				{
					Interlocked.CompareExchange(ref this.m_Event, new ManualResetEvent(false), null);
				}
				return (ManualResetEvent)this.m_Event;
			}
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06001045 RID: 4165 RVA: 0x00056B9C File Offset: 0x00054D9C
		// (set) Token: 0x06001046 RID: 4166 RVA: 0x00056BF8 File Offset: 0x00054DF8
		private Queue AuthenticationRequestQueue
		{
			get
			{
				if (this.m_AuthenticationRequestQueue == null)
				{
					ArrayList connectionList = this.m_ConnectionList;
					lock (connectionList)
					{
						if (this.m_AuthenticationRequestQueue == null)
						{
							this.m_AuthenticationRequestQueue = new Queue();
						}
					}
				}
				return this.m_AuthenticationRequestQueue;
			}
			set
			{
				this.m_AuthenticationRequestQueue = value;
			}
		}

		// Token: 0x06001047 RID: 4167 RVA: 0x00056C01 File Offset: 0x00054E01
		internal static string MakeQueryStr(string connName)
		{
			if (connName != null)
			{
				return connName;
			}
			return "";
		}

		// Token: 0x06001048 RID: 4168 RVA: 0x00056C10 File Offset: 0x00054E10
		internal void Associate(Connection connection)
		{
			ArrayList connectionList = this.m_ConnectionList;
			lock (connectionList)
			{
				this.m_ConnectionList.Add(connection);
			}
		}

		// Token: 0x06001049 RID: 4169 RVA: 0x00056C58 File Offset: 0x00054E58
		internal void Disassociate(Connection connection)
		{
			ArrayList connectionList = this.m_ConnectionList;
			lock (connectionList)
			{
				this.m_ConnectionList.Remove(connection);
			}
		}

		// Token: 0x0600104A RID: 4170 RVA: 0x00056CA0 File Offset: 0x00054EA0
		internal void ConnectionGoneIdle()
		{
			if (this.m_AuthenticationGroup)
			{
				ArrayList connectionList = this.m_ConnectionList;
				lock (connectionList)
				{
					this.AsyncWaitHandle.Set();
				}
			}
		}

		// Token: 0x0600104B RID: 4171 RVA: 0x00056CF0 File Offset: 0x00054EF0
		internal void IncrementConnection()
		{
			ArrayList connectionList = this.m_ConnectionList;
			lock (connectionList)
			{
				this.m_ActiveConnections++;
				if (this.m_ActiveConnections == 1)
				{
					this.CancelIdleTimer();
				}
			}
		}

		// Token: 0x0600104C RID: 4172 RVA: 0x00056D48 File Offset: 0x00054F48
		internal void DecrementConnection()
		{
			ArrayList connectionList = this.m_ConnectionList;
			lock (connectionList)
			{
				this.m_ActiveConnections--;
				if (this.m_ActiveConnections == 0)
				{
					this.m_ExpiringTimer = this.ServicePoint.CreateConnectionGroupTimer(this);
				}
				else if (this.m_ActiveConnections < 0)
				{
					this.m_ActiveConnections = 0;
				}
			}
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x00056DBC File Offset: 0x00054FBC
		internal void CancelIdleTimer()
		{
			ArrayList connectionList = this.m_ConnectionList;
			lock (connectionList)
			{
				TimerThread.Timer expiringTimer = this.m_ExpiringTimer;
				this.m_ExpiringTimer = null;
				if (expiringTimer != null)
				{
					expiringTimer.Cancel();
				}
			}
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x00056E10 File Offset: 0x00055010
		private bool Abort(HttpWebRequest request, WebException webException)
		{
			ArrayList connectionList = this.m_ConnectionList;
			lock (connectionList)
			{
				this.AsyncWaitHandle.Set();
			}
			return true;
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x00056E58 File Offset: 0x00055058
		private void PruneAbortedRequests()
		{
			ArrayList connectionList = this.m_ConnectionList;
			lock (connectionList)
			{
				Queue queue = new Queue();
				foreach (object obj in this.AuthenticationRequestQueue)
				{
					HttpWebRequest httpWebRequest = (HttpWebRequest)obj;
					if (!httpWebRequest.Aborted)
					{
						queue.Enqueue(httpWebRequest);
					}
				}
				this.AuthenticationRequestQueue = queue;
			}
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x00056EF8 File Offset: 0x000550F8
		private void PruneExcesiveConnections()
		{
			ArrayList arrayList = new ArrayList();
			ArrayList connectionList = this.m_ConnectionList;
			lock (connectionList)
			{
				int connectionLimit = this.ConnectionLimit;
				if (this.CurrentConnections > connectionLimit)
				{
					int num = this.CurrentConnections - connectionLimit;
					for (int i = 0; i < num; i++)
					{
						arrayList.Add(this.m_ConnectionList[i]);
					}
					this.m_ConnectionList.RemoveRange(0, num);
				}
			}
			foreach (object obj in arrayList)
			{
				Connection connection = (Connection)obj;
				connection.CloseOnIdle();
			}
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x00056FD0 File Offset: 0x000551D0
		internal void DisableKeepAliveOnConnections()
		{
			ArrayList arrayList = new ArrayList();
			ArrayList connectionList = this.m_ConnectionList;
			lock (connectionList)
			{
				foreach (object obj in this.m_ConnectionList)
				{
					Connection connection = (Connection)obj;
					arrayList.Add(connection);
				}
				this.m_ConnectionList.Clear();
			}
			foreach (object obj2 in arrayList)
			{
				Connection connection2 = (Connection)obj2;
				connection2.CloseOnIdle();
			}
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x000570B4 File Offset: 0x000552B4
		private Connection FindMatchingConnection(HttpWebRequest request, string connName, out Connection leastbusyConnection)
		{
			bool flag = false;
			leastbusyConnection = null;
			ArrayList connectionList = this.m_ConnectionList;
			lock (connectionList)
			{
				int num = int.MaxValue;
				foreach (object obj in this.m_ConnectionList)
				{
					Connection connection = (Connection)obj;
					if (connection.LockedRequest == request)
					{
						leastbusyConnection = connection;
						return connection;
					}
					if (!connection.NonKeepAliveRequestPipelined && connection.BusyCount < num && connection.LockedRequest == null)
					{
						leastbusyConnection = connection;
						num = connection.BusyCount;
						if (num == 0)
						{
							flag = true;
						}
					}
				}
				if (!flag && this.CurrentConnections < this.ConnectionLimit)
				{
					leastbusyConnection = new Connection(this);
				}
			}
			return null;
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x000571A8 File Offset: 0x000553A8
		private Connection FindConnectionAuthenticationGroup(HttpWebRequest request, string connName)
		{
			Connection connection = null;
			ArrayList connectionList = this.m_ConnectionList;
			lock (connectionList)
			{
				Connection connection2 = this.FindMatchingConnection(request, connName, out connection);
				if (connection2 != null)
				{
					connection2.MarkAsReserved();
					return connection2;
				}
				if (this.AuthenticationRequestQueue.Count == 0)
				{
					if (connection != null)
					{
						if (request.LockConnection)
						{
							this.m_NtlmNegGroup = true;
							this.m_IISVersion = connection.IISVersion;
						}
						if (request.LockConnection || (this.m_NtlmNegGroup && !request.Pipelined && request.UnsafeOrProxyAuthenticatedConnectionSharing && this.m_IISVersion >= 6))
						{
							connection.LockedRequest = request;
						}
						connection.MarkAsReserved();
						return connection;
					}
				}
				else if (connection != null)
				{
					this.AsyncWaitHandle.Set();
				}
				this.AuthenticationRequestQueue.Enqueue(request);
			}
			Connection connection3;
			for (;;)
			{
				request.AbortDelegate = this.m_AbortDelegate;
				if (!request.Aborted)
				{
					this.AsyncWaitHandle.WaitOne();
				}
				ArrayList connectionList2 = this.m_ConnectionList;
				lock (connectionList2)
				{
					if (!request.Aborted)
					{
						this.FindMatchingConnection(request, connName, out connection);
						if (this.AuthenticationRequestQueue.Peek() == request)
						{
							this.AuthenticationRequestQueue.Dequeue();
							if (connection != null)
							{
								if (request.LockConnection)
								{
									this.m_NtlmNegGroup = true;
									this.m_IISVersion = connection.IISVersion;
								}
								if (request.LockConnection || (this.m_NtlmNegGroup && !request.Pipelined && request.UnsafeOrProxyAuthenticatedConnectionSharing && this.m_IISVersion >= 6))
								{
									connection.LockedRequest = request;
								}
								connection.MarkAsReserved();
								connection3 = connection;
								break;
							}
							this.AuthenticationRequestQueue.Enqueue(request);
						}
						if (connection == null)
						{
							this.AsyncWaitHandle.Reset();
						}
						continue;
					}
					this.PruneAbortedRequests();
					connection3 = null;
				}
				break;
			}
			return connection3;
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x00057388 File Offset: 0x00055588
		internal Connection FindConnection(HttpWebRequest request, string connName, out bool forcedsubmit)
		{
			Connection connection = null;
			Connection connection2 = null;
			bool flag = false;
			ArrayList arrayList = new ArrayList();
			forcedsubmit = false;
			if (this.m_AuthenticationGroup || request.LockConnection)
			{
				this.m_AuthenticationGroup = true;
				return this.FindConnectionAuthenticationGroup(request, connName);
			}
			ArrayList connectionList = this.m_ConnectionList;
			lock (connectionList)
			{
				int num = int.MaxValue;
				bool flag3 = false;
				foreach (object obj in this.m_ConnectionList)
				{
					Connection connection3 = (Connection)obj;
					bool flag4 = false;
					if (!connection3.IsInitalizing && !connection3.NetworkStream.Connected)
					{
						arrayList.Add(connection3);
					}
					else if (flag3)
					{
						flag4 = !connection3.NonKeepAliveRequestPipelined && num > connection3.BusyCount;
					}
					else
					{
						flag4 = !connection3.NonKeepAliveRequestPipelined || num > connection3.BusyCount;
					}
					if (flag4)
					{
						connection = connection3;
						num = connection3.BusyCount;
						if (!flag3)
						{
							flag3 = !connection3.NonKeepAliveRequestPipelined;
						}
						if (flag3 && num == 0)
						{
							flag = true;
							break;
						}
					}
				}
				foreach (object obj2 in arrayList)
				{
					Connection connection4 = (Connection)obj2;
					connection4.RemoveFromConnectionList();
				}
				if (!flag && this.CurrentConnections < this.ConnectionLimit)
				{
					connection2 = new Connection(this);
					forcedsubmit = false;
				}
				else
				{
					connection2 = connection;
					forcedsubmit = !flag3;
				}
				connection2.MarkAsReserved();
			}
			return connection2;
		}

		// Token: 0x06001055 RID: 4181 RVA: 0x00057574 File Offset: 0x00055774
		[Conditional("DEBUG")]
		internal void DebugMembers(int requestHash)
		{
			foreach (object obj in this.m_ConnectionList)
			{
				Connection connection = (Connection)obj;
			}
		}

		// Token: 0x04001368 RID: 4968
		private const int DefaultConnectionListSize = 3;

		// Token: 0x04001369 RID: 4969
		private ServicePoint m_ServicePoint;

		// Token: 0x0400136A RID: 4970
		private string m_Name;

		// Token: 0x0400136B RID: 4971
		private int m_ConnectionLimit;

		// Token: 0x0400136C RID: 4972
		private ArrayList m_ConnectionList;

		// Token: 0x0400136D RID: 4973
		private object m_Event;

		// Token: 0x0400136E RID: 4974
		private Queue m_AuthenticationRequestQueue;

		// Token: 0x0400136F RID: 4975
		internal bool m_AuthenticationGroup;

		// Token: 0x04001370 RID: 4976
		private HttpAbortDelegate m_AbortDelegate;

		// Token: 0x04001371 RID: 4977
		private bool m_NtlmNegGroup;

		// Token: 0x04001372 RID: 4978
		private int m_IISVersion = -1;

		// Token: 0x04001373 RID: 4979
		private int m_ActiveConnections;

		// Token: 0x04001374 RID: 4980
		private TimerThread.Timer m_ExpiringTimer;
	}
}
