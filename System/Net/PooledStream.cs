using System;
using System.IO;
using System.Net.Sockets;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net
{
	// Token: 0x020001DC RID: 476
	internal class PooledStream : Stream
	{
		// Token: 0x06001279 RID: 4729 RVA: 0x0006298F File Offset: 0x00060B8F
		internal PooledStream(object owner)
		{
			this.m_Owner = new WeakReference(owner);
			this.m_PooledCount = -1;
			this.m_Initalizing = true;
			this.m_NetworkStream = new NetworkStream();
			this.m_CreateTime = DateTime.UtcNow;
		}

		// Token: 0x0600127A RID: 4730 RVA: 0x000629C7 File Offset: 0x00060BC7
		internal PooledStream(ConnectionPool connectionPool, TimeSpan lifetime, bool checkLifetime)
		{
			this.m_ConnectionPool = connectionPool;
			this.m_Lifetime = lifetime;
			this.m_CheckLifetime = checkLifetime;
			this.m_Initalizing = true;
			this.m_NetworkStream = new NetworkStream();
			this.m_CreateTime = DateTime.UtcNow;
		}

		// Token: 0x170003FD RID: 1021
		// (get) Token: 0x0600127B RID: 4731 RVA: 0x00062A01 File Offset: 0x00060C01
		internal bool JustConnected
		{
			get
			{
				if (this.m_JustConnected)
				{
					this.m_JustConnected = false;
					return true;
				}
				return false;
			}
		}

		// Token: 0x170003FE RID: 1022
		// (get) Token: 0x0600127C RID: 4732 RVA: 0x00062A15 File Offset: 0x00060C15
		internal IPAddress ServerAddress
		{
			get
			{
				return this.m_ServerAddress;
			}
		}

		// Token: 0x170003FF RID: 1023
		// (get) Token: 0x0600127D RID: 4733 RVA: 0x00062A1D File Offset: 0x00060C1D
		internal bool IsInitalizing
		{
			get
			{
				return this.m_Initalizing;
			}
		}

		// Token: 0x17000400 RID: 1024
		// (get) Token: 0x0600127E RID: 4734 RVA: 0x00062A28 File Offset: 0x00060C28
		// (set) Token: 0x0600127F RID: 4735 RVA: 0x00062A71 File Offset: 0x00060C71
		internal bool CanBePooled
		{
			get
			{
				if (this.m_Initalizing)
				{
					return true;
				}
				if (!this.m_NetworkStream.Connected)
				{
					return false;
				}
				WeakReference owner = this.m_Owner;
				return !this.m_ConnectionIsDoomed && (owner == null || !owner.IsAlive);
			}
			set
			{
				this.m_ConnectionIsDoomed |= !value;
			}
		}

		// Token: 0x17000401 RID: 1025
		// (get) Token: 0x06001280 RID: 4736 RVA: 0x00062A84 File Offset: 0x00060C84
		internal bool IsEmancipated
		{
			get
			{
				WeakReference owner = this.m_Owner;
				return 0 >= this.m_PooledCount && (owner == null || !owner.IsAlive);
			}
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x06001281 RID: 4737 RVA: 0x00062AB8 File Offset: 0x00060CB8
		// (set) Token: 0x06001282 RID: 4738 RVA: 0x00062AE0 File Offset: 0x00060CE0
		internal object Owner
		{
			get
			{
				WeakReference owner = this.m_Owner;
				if (owner != null && owner.IsAlive)
				{
					return owner.Target;
				}
				return null;
			}
			set
			{
				lock (this)
				{
					if (this.m_Owner != null)
					{
						this.m_Owner.Target = value;
					}
				}
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06001283 RID: 4739 RVA: 0x00062B2C File Offset: 0x00060D2C
		internal ConnectionPool Pool
		{
			get
			{
				return this.m_ConnectionPool;
			}
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06001284 RID: 4740 RVA: 0x00062B34 File Offset: 0x00060D34
		internal virtual ServicePoint ServicePoint
		{
			get
			{
				return this.Pool.ServicePoint;
			}
		}

		// Token: 0x06001285 RID: 4741 RVA: 0x00062B41 File Offset: 0x00060D41
		internal bool Activate(object owningObject, GeneralAsyncDelegate asyncCallback)
		{
			return this.Activate(owningObject, asyncCallback != null, asyncCallback);
		}

		// Token: 0x06001286 RID: 4742 RVA: 0x00062B50 File Offset: 0x00060D50
		protected bool Activate(object owningObject, bool async, GeneralAsyncDelegate asyncCallback)
		{
			bool flag;
			try
			{
				if (this.m_Initalizing)
				{
					IPAddress ipaddress = null;
					this.m_AsyncCallback = asyncCallback;
					Socket connection = this.ServicePoint.GetConnection(this, owningObject, async, out ipaddress, ref this.m_AbortSocket, ref this.m_AbortSocket6);
					if (connection != null)
					{
						if (Logging.On)
						{
							Logging.PrintInfo(Logging.Web, this, SR.GetString("net_log_socket_connected", new object[] { connection.LocalEndPoint, connection.RemoteEndPoint }));
						}
						this.m_NetworkStream.InitNetworkStream(connection, FileAccess.ReadWrite);
						this.m_ServerAddress = ipaddress;
						this.m_Initalizing = false;
						this.m_JustConnected = true;
						this.m_AbortSocket = null;
						this.m_AbortSocket6 = null;
						flag = true;
					}
					else
					{
						flag = false;
					}
				}
				else
				{
					if (async && asyncCallback != null)
					{
						asyncCallback(owningObject, this);
					}
					flag = true;
				}
			}
			catch
			{
				this.m_Initalizing = false;
				throw;
			}
			return flag;
		}

		// Token: 0x06001287 RID: 4743 RVA: 0x00062C2C File Offset: 0x00060E2C
		internal void Deactivate()
		{
			this.m_AsyncCallback = null;
			if (!this.m_ConnectionIsDoomed && this.m_CheckLifetime)
			{
				this.CheckLifetime();
			}
		}

		// Token: 0x06001288 RID: 4744 RVA: 0x00062C4C File Offset: 0x00060E4C
		internal virtual void ConnectionCallback(object owningObject, Exception e, Socket socket, IPAddress address)
		{
			object obj = null;
			if (e != null)
			{
				this.m_Initalizing = false;
				obj = e;
			}
			else
			{
				try
				{
					if (Logging.On)
					{
						Logging.PrintInfo(Logging.Web, this, SR.GetString("net_log_socket_connected", new object[] { socket.LocalEndPoint, socket.RemoteEndPoint }));
					}
					this.m_NetworkStream.InitNetworkStream(socket, FileAccess.ReadWrite);
					obj = this;
				}
				catch (Exception ex)
				{
					if (NclUtilities.IsFatal(ex))
					{
						throw;
					}
					obj = ex;
				}
				this.m_ServerAddress = address;
				this.m_Initalizing = false;
				this.m_JustConnected = true;
			}
			if (this.m_AsyncCallback != null)
			{
				this.m_AsyncCallback(owningObject, obj);
			}
			this.m_AbortSocket = null;
			this.m_AbortSocket6 = null;
		}

		// Token: 0x06001289 RID: 4745 RVA: 0x00062D08 File Offset: 0x00060F08
		protected void CheckLifetime()
		{
			bool flag = !this.m_ConnectionIsDoomed;
			if (flag)
			{
				TimeSpan timeSpan = DateTime.UtcNow.Subtract(this.m_CreateTime);
				this.m_ConnectionIsDoomed = 0 < TimeSpan.Compare(this.m_Lifetime, timeSpan);
			}
		}

		// Token: 0x0600128A RID: 4746 RVA: 0x00062D4C File Offset: 0x00060F4C
		internal void UpdateLifetime()
		{
			int connectionLeaseTimeout = this.ServicePoint.ConnectionLeaseTimeout;
			TimeSpan maxValue;
			if (connectionLeaseTimeout == -1)
			{
				maxValue = TimeSpan.MaxValue;
				this.m_CheckLifetime = false;
			}
			else
			{
				maxValue = new TimeSpan(0, 0, 0, 0, connectionLeaseTimeout);
				this.m_CheckLifetime = true;
			}
			if (maxValue != this.m_Lifetime)
			{
				this.m_Lifetime = maxValue;
			}
		}

		// Token: 0x0600128B RID: 4747 RVA: 0x00062DA0 File Offset: 0x00060FA0
		internal void PrePush(object expectedOwner)
		{
			lock (this)
			{
				if (expectedOwner == null)
				{
					if (this.m_Owner != null && this.m_Owner.Target != null)
					{
						throw new InternalException();
					}
				}
				else if (this.m_Owner == null || this.m_Owner.Target != expectedOwner)
				{
					throw new InternalException();
				}
				this.m_PooledCount++;
				if (1 != this.m_PooledCount)
				{
					throw new InternalException();
				}
				if (this.m_Owner != null)
				{
					this.m_Owner.Target = null;
				}
			}
		}

		// Token: 0x0600128C RID: 4748 RVA: 0x00062E40 File Offset: 0x00061040
		internal void PostPop(object newOwner)
		{
			lock (this)
			{
				if (this.m_Owner == null)
				{
					this.m_Owner = new WeakReference(newOwner);
				}
				else
				{
					if (this.m_Owner.Target != null)
					{
						throw new InternalException();
					}
					this.m_Owner.Target = newOwner;
				}
				this.m_PooledCount--;
				if (this.Pool != null)
				{
					if (this.m_PooledCount != 0)
					{
						throw new InternalException();
					}
				}
				else if (-1 != this.m_PooledCount)
				{
					throw new InternalException();
				}
			}
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x0600128D RID: 4749 RVA: 0x00062EE0 File Offset: 0x000610E0
		protected bool UsingSecureStream
		{
			get
			{
				return this.m_NetworkStream is TlsStream;
			}
		}

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x0600128E RID: 4750 RVA: 0x00062EF0 File Offset: 0x000610F0
		// (set) Token: 0x0600128F RID: 4751 RVA: 0x00062EF8 File Offset: 0x000610F8
		internal NetworkStream NetworkStream
		{
			get
			{
				return this.m_NetworkStream;
			}
			set
			{
				this.m_Initalizing = false;
				this.m_NetworkStream = value;
			}
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06001290 RID: 4752 RVA: 0x00062F08 File Offset: 0x00061108
		protected Socket Socket
		{
			get
			{
				return this.m_NetworkStream.InternalSocket;
			}
		}

		// Token: 0x17000408 RID: 1032
		// (get) Token: 0x06001291 RID: 4753 RVA: 0x00062F15 File Offset: 0x00061115
		public override bool CanRead
		{
			get
			{
				return this.m_NetworkStream.CanRead;
			}
		}

		// Token: 0x17000409 RID: 1033
		// (get) Token: 0x06001292 RID: 4754 RVA: 0x00062F22 File Offset: 0x00061122
		public override bool CanSeek
		{
			get
			{
				return this.m_NetworkStream.CanSeek;
			}
		}

		// Token: 0x1700040A RID: 1034
		// (get) Token: 0x06001293 RID: 4755 RVA: 0x00062F2F File Offset: 0x0006112F
		public override bool CanWrite
		{
			get
			{
				return this.m_NetworkStream.CanWrite;
			}
		}

		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x06001294 RID: 4756 RVA: 0x00062F3C File Offset: 0x0006113C
		public override bool CanTimeout
		{
			get
			{
				return this.m_NetworkStream.CanTimeout;
			}
		}

		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x06001295 RID: 4757 RVA: 0x00062F49 File Offset: 0x00061149
		// (set) Token: 0x06001296 RID: 4758 RVA: 0x00062F56 File Offset: 0x00061156
		public override int ReadTimeout
		{
			get
			{
				return this.m_NetworkStream.ReadTimeout;
			}
			set
			{
				this.m_NetworkStream.ReadTimeout = value;
			}
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06001297 RID: 4759 RVA: 0x00062F64 File Offset: 0x00061164
		// (set) Token: 0x06001298 RID: 4760 RVA: 0x00062F71 File Offset: 0x00061171
		public override int WriteTimeout
		{
			get
			{
				return this.m_NetworkStream.WriteTimeout;
			}
			set
			{
				this.m_NetworkStream.WriteTimeout = value;
			}
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06001299 RID: 4761 RVA: 0x00062F7F File Offset: 0x0006117F
		public override long Length
		{
			get
			{
				return this.m_NetworkStream.Length;
			}
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x0600129A RID: 4762 RVA: 0x00062F8C File Offset: 0x0006118C
		// (set) Token: 0x0600129B RID: 4763 RVA: 0x00062F99 File Offset: 0x00061199
		public override long Position
		{
			get
			{
				return this.m_NetworkStream.Position;
			}
			set
			{
				this.m_NetworkStream.Position = value;
			}
		}

		// Token: 0x0600129C RID: 4764 RVA: 0x00062FA7 File Offset: 0x000611A7
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.m_NetworkStream.Seek(offset, origin);
		}

		// Token: 0x0600129D RID: 4765 RVA: 0x00062FB8 File Offset: 0x000611B8
		public override int Read(byte[] buffer, int offset, int size)
		{
			int num2;
			try
			{
				if (ServicePointManager.UseSafeSynchronousClose)
				{
					int num = Interlocked.Increment(ref this.m_SynchronousIOClosingState);
					if ((num & 1610612736) != 0)
					{
						throw new ObjectDisposedException(base.GetType().FullName);
					}
				}
				num2 = this.m_NetworkStream.Read(buffer, offset, size);
			}
			finally
			{
				if (ServicePointManager.UseSafeSynchronousClose && 536870912 == Interlocked.Decrement(ref this.m_SynchronousIOClosingState))
				{
					try
					{
						this.TryCloseNetworkStream(false, 0);
					}
					catch
					{
					}
				}
			}
			return num2;
		}

		// Token: 0x0600129E RID: 4766 RVA: 0x00063048 File Offset: 0x00061248
		public override void Write(byte[] buffer, int offset, int size)
		{
			try
			{
				if (ServicePointManager.UseSafeSynchronousClose)
				{
					int num = Interlocked.Increment(ref this.m_SynchronousIOClosingState);
					if ((num & 1610612736) != 0)
					{
						throw new ObjectDisposedException(base.GetType().FullName);
					}
				}
				this.m_NetworkStream.Write(buffer, offset, size);
			}
			finally
			{
				if (ServicePointManager.UseSafeSynchronousClose && 536870912 == Interlocked.Decrement(ref this.m_SynchronousIOClosingState))
				{
					try
					{
						this.TryCloseNetworkStream(false, 0);
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x0600129F RID: 4767 RVA: 0x000630D8 File Offset: 0x000612D8
		internal void MultipleWrite(BufferOffsetSize[] buffers)
		{
			this.m_NetworkStream.MultipleWrite(buffers);
		}

		// Token: 0x060012A0 RID: 4768 RVA: 0x000630E8 File Offset: 0x000612E8
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					this.m_Owner = null;
					this.m_ConnectionIsDoomed = true;
					this.CloseSocket();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x060012A1 RID: 4769 RVA: 0x00063128 File Offset: 0x00061328
		private int InterlockedOr(ref int location1, int bitMask)
		{
			int num;
			do
			{
				num = Volatile.Read(ref location1);
			}
			while (num != Interlocked.CompareExchange(ref location1, num | bitMask, num));
			return num;
		}

		// Token: 0x060012A2 RID: 4770 RVA: 0x0006314C File Offset: 0x0006134C
		private bool TryCloseNetworkStream(bool closeWithTimeout, int timeout)
		{
			if (!ServicePointManager.UseSafeSynchronousClose)
			{
				return false;
			}
			if (536870912 == Interlocked.CompareExchange(ref this.m_SynchronousIOClosingState, 1073741824, 536870912))
			{
				if (closeWithTimeout)
				{
					this.m_NetworkStream.Close(timeout);
				}
				else
				{
					this.m_NetworkStream.Close();
				}
				return true;
			}
			return false;
		}

		// Token: 0x060012A3 RID: 4771 RVA: 0x000631A0 File Offset: 0x000613A0
		private bool CancelPendingIoAndCloseIfSafe(bool closeWithTimeout, int timeout)
		{
			if (this.TryCloseNetworkStream(closeWithTimeout, timeout))
			{
				return true;
			}
			try
			{
				Socket internalSocket = this.m_NetworkStream.InternalSocket;
				UnsafeNclNativeMethods.CancelIoEx(internalSocket.SafeHandle, IntPtr.Zero);
			}
			catch
			{
			}
			return this.TryCloseNetworkStream(closeWithTimeout, timeout);
		}

		// Token: 0x060012A4 RID: 4772 RVA: 0x000631F4 File Offset: 0x000613F4
		private void CloseConnectingSockets(bool useTimeout, int timeout)
		{
			Socket abortSocket = this.m_AbortSocket;
			Socket abortSocket2 = this.m_AbortSocket6;
			if (abortSocket != null)
			{
				if (ServicePointManager.UseSafeSynchronousClose)
				{
					try
					{
						UnsafeNclNativeMethods.CancelIoEx(abortSocket.SafeHandle, IntPtr.Zero);
					}
					catch
					{
					}
				}
				if (useTimeout)
				{
					abortSocket.Close(timeout);
				}
				else
				{
					abortSocket.Close();
				}
				this.m_AbortSocket = null;
			}
			if (abortSocket2 != null)
			{
				if (ServicePointManager.UseSafeSynchronousClose)
				{
					try
					{
						UnsafeNclNativeMethods.CancelIoEx(abortSocket2.SafeHandle, IntPtr.Zero);
					}
					catch
					{
					}
				}
				if (useTimeout)
				{
					abortSocket2.Close(timeout);
				}
				else
				{
					abortSocket2.Close();
				}
				this.m_AbortSocket6 = null;
			}
		}

		// Token: 0x060012A5 RID: 4773 RVA: 0x000632A0 File Offset: 0x000614A0
		internal void CloseSocket()
		{
			if (!ServicePointManager.UseSafeSynchronousClose)
			{
				this.m_NetworkStream.Close();
			}
			else
			{
				this.InterlockedOr(ref this.m_SynchronousIOClosingState, 536870912);
				this.CancelPendingIoAndCloseIfSafe(false, 0);
			}
			this.CloseConnectingSockets(false, 0);
		}

		// Token: 0x060012A6 RID: 4774 RVA: 0x000632D9 File Offset: 0x000614D9
		public void Close(int timeout)
		{
			if (!ServicePointManager.UseSafeSynchronousClose)
			{
				this.m_NetworkStream.Close(timeout);
			}
			else
			{
				this.InterlockedOr(ref this.m_SynchronousIOClosingState, 536870912);
				this.CancelPendingIoAndCloseIfSafe(true, timeout);
			}
			this.CloseConnectingSockets(true, timeout);
		}

		// Token: 0x060012A7 RID: 4775 RVA: 0x00063313 File Offset: 0x00061513
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			return this.m_NetworkStream.BeginRead(buffer, offset, size, callback, state);
		}

		// Token: 0x060012A8 RID: 4776 RVA: 0x00063327 File Offset: 0x00061527
		internal virtual IAsyncResult UnsafeBeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			return this.m_NetworkStream.UnsafeBeginRead(buffer, offset, size, callback, state);
		}

		// Token: 0x060012A9 RID: 4777 RVA: 0x0006333B File Offset: 0x0006153B
		public override int EndRead(IAsyncResult asyncResult)
		{
			return this.m_NetworkStream.EndRead(asyncResult);
		}

		// Token: 0x060012AA RID: 4778 RVA: 0x00063349 File Offset: 0x00061549
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			return this.m_NetworkStream.BeginWrite(buffer, offset, size, callback, state);
		}

		// Token: 0x060012AB RID: 4779 RVA: 0x0006335D File Offset: 0x0006155D
		internal virtual IAsyncResult UnsafeBeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			return this.m_NetworkStream.UnsafeBeginWrite(buffer, offset, size, callback, state);
		}

		// Token: 0x060012AC RID: 4780 RVA: 0x00063371 File Offset: 0x00061571
		public override void EndWrite(IAsyncResult asyncResult)
		{
			this.m_NetworkStream.EndWrite(asyncResult);
		}

		// Token: 0x060012AD RID: 4781 RVA: 0x0006337F File Offset: 0x0006157F
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		internal IAsyncResult BeginMultipleWrite(BufferOffsetSize[] buffers, AsyncCallback callback, object state)
		{
			return this.m_NetworkStream.BeginMultipleWrite(buffers, callback, state);
		}

		// Token: 0x060012AE RID: 4782 RVA: 0x0006338F File Offset: 0x0006158F
		internal void EndMultipleWrite(IAsyncResult asyncResult)
		{
			this.m_NetworkStream.EndMultipleWrite(asyncResult);
		}

		// Token: 0x060012AF RID: 4783 RVA: 0x0006339D File Offset: 0x0006159D
		public override void Flush()
		{
			this.m_NetworkStream.Flush();
		}

		// Token: 0x060012B0 RID: 4784 RVA: 0x000633AA File Offset: 0x000615AA
		public override Task FlushAsync(CancellationToken cancellationToken)
		{
			return this.m_NetworkStream.FlushAsync(cancellationToken);
		}

		// Token: 0x060012B1 RID: 4785 RVA: 0x000633B8 File Offset: 0x000615B8
		public override void SetLength(long value)
		{
			this.m_NetworkStream.SetLength(value);
		}

		// Token: 0x060012B2 RID: 4786 RVA: 0x000633C6 File Offset: 0x000615C6
		internal void SetSocketTimeoutOption(SocketShutdown mode, int timeout, bool silent)
		{
			this.m_NetworkStream.SetSocketTimeoutOption(mode, timeout, silent);
		}

		// Token: 0x060012B3 RID: 4787 RVA: 0x000633D6 File Offset: 0x000615D6
		internal bool Poll(int microSeconds, SelectMode mode)
		{
			return this.m_NetworkStream.Poll(microSeconds, mode);
		}

		// Token: 0x060012B4 RID: 4788 RVA: 0x000633E5 File Offset: 0x000615E5
		internal bool PollRead()
		{
			return this.m_NetworkStream.PollRead();
		}

		// Token: 0x040014FB RID: 5371
		private const int ClosingFlag = 536870912;

		// Token: 0x040014FC RID: 5372
		private const int ClosedFlag = 1073741824;

		// Token: 0x040014FD RID: 5373
		private bool m_CheckLifetime;

		// Token: 0x040014FE RID: 5374
		private TimeSpan m_Lifetime;

		// Token: 0x040014FF RID: 5375
		private DateTime m_CreateTime;

		// Token: 0x04001500 RID: 5376
		private bool m_ConnectionIsDoomed;

		// Token: 0x04001501 RID: 5377
		private ConnectionPool m_ConnectionPool;

		// Token: 0x04001502 RID: 5378
		private WeakReference m_Owner;

		// Token: 0x04001503 RID: 5379
		private int m_PooledCount;

		// Token: 0x04001504 RID: 5380
		private bool m_Initalizing;

		// Token: 0x04001505 RID: 5381
		private IPAddress m_ServerAddress;

		// Token: 0x04001506 RID: 5382
		private NetworkStream m_NetworkStream;

		// Token: 0x04001507 RID: 5383
		private Socket m_AbortSocket;

		// Token: 0x04001508 RID: 5384
		private Socket m_AbortSocket6;

		// Token: 0x04001509 RID: 5385
		private bool m_JustConnected;

		// Token: 0x0400150A RID: 5386
		private int m_SynchronousIOClosingState;

		// Token: 0x0400150B RID: 5387
		private GeneralAsyncDelegate m_AsyncCallback;
	}
}
