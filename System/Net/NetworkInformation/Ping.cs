using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002EA RID: 746
	public class Ping : Component
	{
		// Token: 0x14000023 RID: 35
		// (add) Token: 0x06001A1B RID: 6683 RVA: 0x0007EA84 File Offset: 0x0007CC84
		// (remove) Token: 0x06001A1C RID: 6684 RVA: 0x0007EABC File Offset: 0x0007CCBC
		public event PingCompletedEventHandler PingCompleted;

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06001A1D RID: 6685 RVA: 0x0007EAF1 File Offset: 0x0007CCF1
		// (set) Token: 0x06001A1E RID: 6686 RVA: 0x0007EB0C File Offset: 0x0007CD0C
		private bool InAsyncCall
		{
			get
			{
				return this.asyncFinished != null && !this.asyncFinished.WaitOne(0);
			}
			set
			{
				if (this.asyncFinished == null)
				{
					this.asyncFinished = new ManualResetEvent(!value);
					return;
				}
				if (value)
				{
					this.asyncFinished.Reset();
					return;
				}
				this.asyncFinished.Set();
			}
		}

		// Token: 0x06001A1F RID: 6687 RVA: 0x0007EB44 File Offset: 0x0007CD44
		private void CheckStart(bool async)
		{
			if (this.disposeRequested)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			int num = Interlocked.CompareExchange(ref this.status, 1, 0);
			if (num == 1)
			{
				throw new InvalidOperationException(SR.GetString("net_inasync"));
			}
			if (num == 2)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			if (async)
			{
				this.InAsyncCall = true;
			}
		}

		// Token: 0x06001A20 RID: 6688 RVA: 0x0007EBAB File Offset: 0x0007CDAB
		private void Finish(bool async)
		{
			this.status = 0;
			if (async)
			{
				this.InAsyncCall = false;
			}
			if (this.disposeRequested)
			{
				this.InternalDispose();
			}
		}

		// Token: 0x06001A21 RID: 6689 RVA: 0x0007EBCC File Offset: 0x0007CDCC
		protected void OnPingCompleted(PingCompletedEventArgs e)
		{
			if (this.PingCompleted != null)
			{
				this.PingCompleted(this, e);
			}
		}

		// Token: 0x06001A22 RID: 6690 RVA: 0x0007EBE3 File Offset: 0x0007CDE3
		private void PingCompletedWaitCallback(object operationState)
		{
			this.OnPingCompleted((PingCompletedEventArgs)operationState);
		}

		// Token: 0x06001A23 RID: 6691 RVA: 0x0007EBF1 File Offset: 0x0007CDF1
		public Ping()
		{
			this.onPingCompletedDelegate = new SendOrPostCallback(this.PingCompletedWaitCallback);
		}

		// Token: 0x06001A24 RID: 6692 RVA: 0x0007EC18 File Offset: 0x0007CE18
		private void InternalDispose()
		{
			this.disposeRequested = true;
			if (Interlocked.CompareExchange(ref this.status, 2, 0) != 0)
			{
				return;
			}
			if (this.handlePingV4 != null)
			{
				this.handlePingV4.Close();
				this.handlePingV4 = null;
			}
			if (this.handlePingV6 != null)
			{
				this.handlePingV6.Close();
				this.handlePingV6 = null;
			}
			this.UnregisterWaitHandle();
			if (this.pingEvent != null)
			{
				this.pingEvent.Close();
				this.pingEvent = null;
			}
			if (this.replyBuffer != null)
			{
				this.replyBuffer.Close();
				this.replyBuffer = null;
			}
			if (this.asyncFinished != null)
			{
				this.asyncFinished.Close();
				this.asyncFinished = null;
			}
		}

		// Token: 0x06001A25 RID: 6693 RVA: 0x0007ECC4 File Offset: 0x0007CEC4
		private void UnregisterWaitHandle()
		{
			object obj = this.lockObject;
			lock (obj)
			{
				if (this.registeredWait != null)
				{
					this.registeredWait.Unregister(null);
					this.registeredWait = null;
				}
			}
		}

		// Token: 0x06001A26 RID: 6694 RVA: 0x0007ED1C File Offset: 0x0007CF1C
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.InternalDispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06001A27 RID: 6695 RVA: 0x0007ED30 File Offset: 0x0007CF30
		public void SendAsyncCancel()
		{
			object obj = this.lockObject;
			lock (obj)
			{
				if (!this.InAsyncCall)
				{
					return;
				}
				this.cancelled = true;
			}
			this.asyncFinished.WaitOne();
		}

		// Token: 0x06001A28 RID: 6696 RVA: 0x0007ED88 File Offset: 0x0007CF88
		private static void PingCallback(object state, bool signaled)
		{
			Ping ping = (Ping)state;
			PingCompletedEventArgs pingCompletedEventArgs = null;
			AsyncOperation asyncOperation = null;
			SendOrPostCallback sendOrPostCallback = null;
			try
			{
				object obj = ping.lockObject;
				lock (obj)
				{
					bool flag2 = ping.cancelled;
					asyncOperation = ping.asyncOp;
					sendOrPostCallback = ping.onPingCompletedDelegate;
					if (!flag2)
					{
						SafeLocalFree safeLocalFree = ping.replyBuffer;
						PingReply pingReply;
						if (ping.ipv6)
						{
							Icmp6EchoReply icmp6EchoReply = (Icmp6EchoReply)Marshal.PtrToStructure(safeLocalFree.DangerousGetHandle(), typeof(Icmp6EchoReply));
							pingReply = new PingReply(icmp6EchoReply, safeLocalFree.DangerousGetHandle(), ping.sendSize);
						}
						else
						{
							IcmpEchoReply icmpEchoReply = (IcmpEchoReply)Marshal.PtrToStructure(safeLocalFree.DangerousGetHandle(), typeof(IcmpEchoReply));
							pingReply = new PingReply(icmpEchoReply);
						}
						pingCompletedEventArgs = new PingCompletedEventArgs(pingReply, null, false, asyncOperation.UserSuppliedState);
					}
					else
					{
						pingCompletedEventArgs = new PingCompletedEventArgs(null, null, true, asyncOperation.UserSuppliedState);
					}
				}
			}
			catch (Exception ex)
			{
				PingException ex2 = new PingException(SR.GetString("net_ping"), ex);
				pingCompletedEventArgs = new PingCompletedEventArgs(null, ex2, false, asyncOperation.UserSuppliedState);
			}
			finally
			{
				ping.FreeUnmanagedStructures();
				ping.UnregisterWaitHandle();
				ping.Finish(true);
			}
			asyncOperation.PostOperationCompleted(sendOrPostCallback, pingCompletedEventArgs);
		}

		// Token: 0x06001A29 RID: 6697 RVA: 0x0007EEDC File Offset: 0x0007D0DC
		public PingReply Send(string hostNameOrAddress)
		{
			return this.Send(hostNameOrAddress, 5000, this.DefaultSendBuffer, null);
		}

		// Token: 0x06001A2A RID: 6698 RVA: 0x0007EEF1 File Offset: 0x0007D0F1
		public PingReply Send(string hostNameOrAddress, int timeout)
		{
			return this.Send(hostNameOrAddress, timeout, this.DefaultSendBuffer, null);
		}

		// Token: 0x06001A2B RID: 6699 RVA: 0x0007EF02 File Offset: 0x0007D102
		public PingReply Send(IPAddress address)
		{
			return this.Send(address, 5000, this.DefaultSendBuffer, null);
		}

		// Token: 0x06001A2C RID: 6700 RVA: 0x0007EF17 File Offset: 0x0007D117
		public PingReply Send(IPAddress address, int timeout)
		{
			return this.Send(address, timeout, this.DefaultSendBuffer, null);
		}

		// Token: 0x06001A2D RID: 6701 RVA: 0x0007EF28 File Offset: 0x0007D128
		public PingReply Send(string hostNameOrAddress, int timeout, byte[] buffer)
		{
			return this.Send(hostNameOrAddress, timeout, buffer, null);
		}

		// Token: 0x06001A2E RID: 6702 RVA: 0x0007EF34 File Offset: 0x0007D134
		public PingReply Send(IPAddress address, int timeout, byte[] buffer)
		{
			return this.Send(address, timeout, buffer, null);
		}

		// Token: 0x06001A2F RID: 6703 RVA: 0x0007EF40 File Offset: 0x0007D140
		public PingReply Send(string hostNameOrAddress, int timeout, byte[] buffer, PingOptions options)
		{
			if (ValidationHelper.IsBlankString(hostNameOrAddress))
			{
				throw new ArgumentNullException("hostNameOrAddress");
			}
			IPAddress ipaddress;
			if (!IPAddress.TryParse(hostNameOrAddress, out ipaddress))
			{
				try
				{
					ipaddress = Dns.GetHostAddresses(hostNameOrAddress)[0];
				}
				catch (ArgumentException)
				{
					throw;
				}
				catch (Exception ex)
				{
					throw new PingException(SR.GetString("net_ping"), ex);
				}
			}
			return this.Send(ipaddress, timeout, buffer, options);
		}

		// Token: 0x06001A30 RID: 6704 RVA: 0x0007EFB4 File Offset: 0x0007D1B4
		public PingReply Send(IPAddress address, int timeout, byte[] buffer, PingOptions options)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (buffer.Length > 65500)
			{
				throw new ArgumentException(SR.GetString("net_invalidPingBufferSize"), "buffer");
			}
			if (timeout < 0)
			{
				throw new ArgumentOutOfRangeException("timeout");
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			this.TestIsIpSupported(address);
			if (address.Equals(IPAddress.Any) || address.Equals(IPAddress.IPv6Any))
			{
				throw new ArgumentException(SR.GetString("net_invalid_ip_addr"), "address");
			}
			IPAddress ipaddress;
			if (address.AddressFamily == AddressFamily.InterNetwork)
			{
				ipaddress = new IPAddress(address.GetAddressBytes());
			}
			else
			{
				ipaddress = new IPAddress(address.GetAddressBytes(), address.ScopeId);
			}
			new NetworkInformationPermission(NetworkInformationAccess.Ping).Demand();
			this.CheckStart(false);
			PingReply pingReply;
			try
			{
				pingReply = this.InternalSend(ipaddress, buffer, timeout, options, false);
			}
			catch (Exception ex)
			{
				throw new PingException(SR.GetString("net_ping"), ex);
			}
			finally
			{
				this.Finish(false);
			}
			return pingReply;
		}

		// Token: 0x06001A31 RID: 6705 RVA: 0x0007F0C4 File Offset: 0x0007D2C4
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void SendAsync(string hostNameOrAddress, object userToken)
		{
			this.SendAsync(hostNameOrAddress, 5000, this.DefaultSendBuffer, userToken);
		}

		// Token: 0x06001A32 RID: 6706 RVA: 0x0007F0D9 File Offset: 0x0007D2D9
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void SendAsync(string hostNameOrAddress, int timeout, object userToken)
		{
			this.SendAsync(hostNameOrAddress, timeout, this.DefaultSendBuffer, userToken);
		}

		// Token: 0x06001A33 RID: 6707 RVA: 0x0007F0EA File Offset: 0x0007D2EA
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void SendAsync(IPAddress address, object userToken)
		{
			this.SendAsync(address, 5000, this.DefaultSendBuffer, userToken);
		}

		// Token: 0x06001A34 RID: 6708 RVA: 0x0007F0FF File Offset: 0x0007D2FF
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void SendAsync(IPAddress address, int timeout, object userToken)
		{
			this.SendAsync(address, timeout, this.DefaultSendBuffer, userToken);
		}

		// Token: 0x06001A35 RID: 6709 RVA: 0x0007F110 File Offset: 0x0007D310
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void SendAsync(string hostNameOrAddress, int timeout, byte[] buffer, object userToken)
		{
			this.SendAsync(hostNameOrAddress, timeout, buffer, null, userToken);
		}

		// Token: 0x06001A36 RID: 6710 RVA: 0x0007F11E File Offset: 0x0007D31E
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void SendAsync(IPAddress address, int timeout, byte[] buffer, object userToken)
		{
			this.SendAsync(address, timeout, buffer, null, userToken);
		}

		// Token: 0x06001A37 RID: 6711 RVA: 0x0007F12C File Offset: 0x0007D32C
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void SendAsync(string hostNameOrAddress, int timeout, byte[] buffer, PingOptions options, object userToken)
		{
			if (ValidationHelper.IsBlankString(hostNameOrAddress))
			{
				throw new ArgumentNullException("hostNameOrAddress");
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (buffer.Length > 65500)
			{
				throw new ArgumentException(SR.GetString("net_invalidPingBufferSize"), "buffer");
			}
			if (timeout < 0)
			{
				throw new ArgumentOutOfRangeException("timeout");
			}
			IPAddress ipaddress;
			if (IPAddress.TryParse(hostNameOrAddress, out ipaddress))
			{
				this.SendAsync(ipaddress, timeout, buffer, options, userToken);
				return;
			}
			this.CheckStart(true);
			try
			{
				this.cancelled = false;
				this.asyncOp = AsyncOperationManager.CreateOperation(userToken);
				Ping.AsyncStateObject asyncStateObject = new Ping.AsyncStateObject(hostNameOrAddress, buffer, timeout, options, userToken);
				ThreadPool.QueueUserWorkItem(new WaitCallback(this.ContinueAsyncSend), asyncStateObject);
			}
			catch (Exception ex)
			{
				this.Finish(true);
				throw new PingException(SR.GetString("net_ping"), ex);
			}
		}

		// Token: 0x06001A38 RID: 6712 RVA: 0x0007F208 File Offset: 0x0007D408
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public void SendAsync(IPAddress address, int timeout, byte[] buffer, PingOptions options, object userToken)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (buffer.Length > 65500)
			{
				throw new ArgumentException(SR.GetString("net_invalidPingBufferSize"), "buffer");
			}
			if (timeout < 0)
			{
				throw new ArgumentOutOfRangeException("timeout");
			}
			if (address == null)
			{
				throw new ArgumentNullException("address");
			}
			this.TestIsIpSupported(address);
			if (address.Equals(IPAddress.Any) || address.Equals(IPAddress.IPv6Any))
			{
				throw new ArgumentException(SR.GetString("net_invalid_ip_addr"), "address");
			}
			IPAddress ipaddress;
			if (address.AddressFamily == AddressFamily.InterNetwork)
			{
				ipaddress = new IPAddress(address.GetAddressBytes());
			}
			else
			{
				ipaddress = new IPAddress(address.GetAddressBytes(), address.ScopeId);
			}
			new NetworkInformationPermission(NetworkInformationAccess.Ping).Demand();
			this.CheckStart(true);
			try
			{
				this.cancelled = false;
				this.asyncOp = AsyncOperationManager.CreateOperation(userToken);
				this.InternalSend(ipaddress, buffer, timeout, options, true);
			}
			catch (Exception ex)
			{
				this.Finish(true);
				throw new PingException(SR.GetString("net_ping"), ex);
			}
		}

		// Token: 0x06001A39 RID: 6713 RVA: 0x0007F31C File Offset: 0x0007D51C
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<PingReply> SendPingAsync(IPAddress address)
		{
			return this.SendPingAsyncCore(delegate(TaskCompletionSource<PingReply> tcs)
			{
				this.SendAsync(address, tcs);
			});
		}

		// Token: 0x06001A3A RID: 6714 RVA: 0x0007F350 File Offset: 0x0007D550
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<PingReply> SendPingAsync(string hostNameOrAddress)
		{
			return this.SendPingAsyncCore(delegate(TaskCompletionSource<PingReply> tcs)
			{
				this.SendAsync(hostNameOrAddress, tcs);
			});
		}

		// Token: 0x06001A3B RID: 6715 RVA: 0x0007F384 File Offset: 0x0007D584
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<PingReply> SendPingAsync(IPAddress address, int timeout)
		{
			return this.SendPingAsyncCore(delegate(TaskCompletionSource<PingReply> tcs)
			{
				this.SendAsync(address, timeout, tcs);
			});
		}

		// Token: 0x06001A3C RID: 6716 RVA: 0x0007F3C0 File Offset: 0x0007D5C0
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<PingReply> SendPingAsync(string hostNameOrAddress, int timeout)
		{
			return this.SendPingAsyncCore(delegate(TaskCompletionSource<PingReply> tcs)
			{
				this.SendAsync(hostNameOrAddress, timeout, tcs);
			});
		}

		// Token: 0x06001A3D RID: 6717 RVA: 0x0007F3FC File Offset: 0x0007D5FC
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<PingReply> SendPingAsync(IPAddress address, int timeout, byte[] buffer)
		{
			return this.SendPingAsyncCore(delegate(TaskCompletionSource<PingReply> tcs)
			{
				this.SendAsync(address, timeout, buffer, tcs);
			});
		}

		// Token: 0x06001A3E RID: 6718 RVA: 0x0007F440 File Offset: 0x0007D640
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<PingReply> SendPingAsync(string hostNameOrAddress, int timeout, byte[] buffer)
		{
			return this.SendPingAsyncCore(delegate(TaskCompletionSource<PingReply> tcs)
			{
				this.SendAsync(hostNameOrAddress, timeout, buffer, tcs);
			});
		}

		// Token: 0x06001A3F RID: 6719 RVA: 0x0007F484 File Offset: 0x0007D684
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<PingReply> SendPingAsync(IPAddress address, int timeout, byte[] buffer, PingOptions options)
		{
			return this.SendPingAsyncCore(delegate(TaskCompletionSource<PingReply> tcs)
			{
				this.SendAsync(address, timeout, buffer, options, tcs);
			});
		}

		// Token: 0x06001A40 RID: 6720 RVA: 0x0007F4D0 File Offset: 0x0007D6D0
		[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
		public Task<PingReply> SendPingAsync(string hostNameOrAddress, int timeout, byte[] buffer, PingOptions options)
		{
			return this.SendPingAsyncCore(delegate(TaskCompletionSource<PingReply> tcs)
			{
				this.SendAsync(hostNameOrAddress, timeout, buffer, options, tcs);
			});
		}

		// Token: 0x06001A41 RID: 6721 RVA: 0x0007F51C File Offset: 0x0007D71C
		private Task<PingReply> SendPingAsyncCore(Action<TaskCompletionSource<PingReply>> sendAsync)
		{
			TaskCompletionSource<PingReply> tcs = new TaskCompletionSource<PingReply>();
			PingCompletedEventHandler handler = null;
			handler = delegate(object sender, PingCompletedEventArgs e)
			{
				this.HandleCompletion(tcs, e, handler);
			};
			this.PingCompleted += handler;
			try
			{
				sendAsync(tcs);
			}
			catch
			{
				this.PingCompleted -= handler;
				throw;
			}
			return tcs.Task;
		}

		// Token: 0x06001A42 RID: 6722 RVA: 0x0007F5A0 File Offset: 0x0007D7A0
		private void HandleCompletion(TaskCompletionSource<PingReply> tcs, PingCompletedEventArgs e, PingCompletedEventHandler handler)
		{
			if (e.UserState == tcs)
			{
				try
				{
					this.PingCompleted -= handler;
				}
				finally
				{
					if (e.Error != null)
					{
						tcs.TrySetException(e.Error);
					}
					else if (e.Cancelled)
					{
						tcs.TrySetCanceled();
					}
					else
					{
						tcs.TrySetResult(e.Reply);
					}
				}
			}
		}

		// Token: 0x06001A43 RID: 6723 RVA: 0x0007F608 File Offset: 0x0007D808
		private void ContinueAsyncSend(object state)
		{
			Ping.AsyncStateObject asyncStateObject = (Ping.AsyncStateObject)state;
			try
			{
				IPAddress ipaddress = Dns.GetHostAddresses(asyncStateObject.hostName)[0];
				new NetworkInformationPermission(NetworkInformationAccess.Ping).Demand();
				this.InternalSend(ipaddress, asyncStateObject.buffer, asyncStateObject.timeout, asyncStateObject.options, true);
			}
			catch (Exception ex)
			{
				PingException ex2 = new PingException(SR.GetString("net_ping"), ex);
				PingCompletedEventArgs pingCompletedEventArgs = new PingCompletedEventArgs(null, ex2, false, this.asyncOp.UserSuppliedState);
				this.Finish(true);
				this.asyncOp.PostOperationCompleted(this.onPingCompletedDelegate, pingCompletedEventArgs);
			}
		}

		// Token: 0x06001A44 RID: 6724 RVA: 0x0007F6A8 File Offset: 0x0007D8A8
		private PingReply InternalSend(IPAddress address, byte[] buffer, int timeout, PingOptions options, bool async)
		{
			this.ipv6 = address.AddressFamily == AddressFamily.InterNetworkV6;
			this.sendSize = buffer.Length;
			if (!this.ipv6 && this.handlePingV4 == null)
			{
				this.handlePingV4 = UnsafeNetInfoNativeMethods.IcmpCreateFile();
				if (this.handlePingV4.IsInvalid)
				{
					this.handlePingV4 = null;
					throw new Win32Exception();
				}
			}
			else if (this.ipv6 && this.handlePingV6 == null)
			{
				this.handlePingV6 = UnsafeNetInfoNativeMethods.Icmp6CreateFile();
				if (this.handlePingV6.IsInvalid)
				{
					this.handlePingV6 = null;
					throw new Win32Exception();
				}
			}
			IPOptions ipoptions = new IPOptions(options);
			if (this.replyBuffer == null)
			{
				this.replyBuffer = SafeLocalFree.LocalAlloc(65791);
			}
			int num;
			try
			{
				if (async)
				{
					if (this.pingEvent == null)
					{
						this.pingEvent = new ManualResetEvent(false);
					}
					else
					{
						this.pingEvent.Reset();
					}
					this.registeredWait = ThreadPool.RegisterWaitForSingleObject(this.pingEvent, new WaitOrTimerCallback(Ping.PingCallback), this, -1, true);
				}
				this.SetUnmanagedStructures(buffer);
				if (!this.ipv6)
				{
					if (async)
					{
						num = (int)UnsafeNetInfoNativeMethods.IcmpSendEcho2(this.handlePingV4, this.pingEvent.SafeWaitHandle, IntPtr.Zero, IntPtr.Zero, (uint)address.m_Address, this.requestBuffer, (ushort)buffer.Length, ref ipoptions, this.replyBuffer, 65791U, (uint)timeout);
					}
					else
					{
						num = (int)UnsafeNetInfoNativeMethods.IcmpSendEcho2(this.handlePingV4, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, (uint)address.m_Address, this.requestBuffer, (ushort)buffer.Length, ref ipoptions, this.replyBuffer, 65791U, (uint)timeout);
					}
				}
				else
				{
					IPEndPoint ipendPoint = new IPEndPoint(address, 0);
					SocketAddress socketAddress = ipendPoint.Serialize();
					byte[] array = new byte[28];
					if (async)
					{
						num = (int)UnsafeNetInfoNativeMethods.Icmp6SendEcho2(this.handlePingV6, this.pingEvent.SafeWaitHandle, IntPtr.Zero, IntPtr.Zero, array, socketAddress.m_Buffer, this.requestBuffer, (ushort)buffer.Length, ref ipoptions, this.replyBuffer, 65791U, (uint)timeout);
					}
					else
					{
						num = (int)UnsafeNetInfoNativeMethods.Icmp6SendEcho2(this.handlePingV6, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, array, socketAddress.m_Buffer, this.requestBuffer, (ushort)buffer.Length, ref ipoptions, this.replyBuffer, 65791U, (uint)timeout);
					}
				}
			}
			catch
			{
				this.UnregisterWaitHandle();
				throw;
			}
			if (num == 0)
			{
				num = Marshal.GetLastWin32Error();
				if (async && (long)num == 997L)
				{
					return null;
				}
				this.FreeUnmanagedStructures();
				this.UnregisterWaitHandle();
				if (async || num < 11002 || num > 11045)
				{
					throw new Win32Exception(num);
				}
				return new PingReply((IPStatus)num);
			}
			else
			{
				if (async)
				{
					return null;
				}
				this.FreeUnmanagedStructures();
				PingReply pingReply;
				if (this.ipv6)
				{
					Icmp6EchoReply icmp6EchoReply = (Icmp6EchoReply)Marshal.PtrToStructure(this.replyBuffer.DangerousGetHandle(), typeof(Icmp6EchoReply));
					pingReply = new PingReply(icmp6EchoReply, this.replyBuffer.DangerousGetHandle(), this.sendSize);
				}
				else
				{
					IcmpEchoReply icmpEchoReply = (IcmpEchoReply)Marshal.PtrToStructure(this.replyBuffer.DangerousGetHandle(), typeof(IcmpEchoReply));
					pingReply = new PingReply(icmpEchoReply);
				}
				GC.KeepAlive(this.replyBuffer);
				return pingReply;
			}
		}

		// Token: 0x06001A45 RID: 6725 RVA: 0x0007F9D0 File Offset: 0x0007DBD0
		private void TestIsIpSupported(IPAddress ip)
		{
			if (ip.AddressFamily == AddressFamily.InterNetwork && !Socket.OSSupportsIPv4)
			{
				throw new NotSupportedException(SR.GetString("net_ipv4_not_installed"));
			}
			if (ip.AddressFamily == AddressFamily.InterNetworkV6 && !Socket.OSSupportsIPv6)
			{
				throw new NotSupportedException(SR.GetString("net_ipv6_not_installed"));
			}
		}

		// Token: 0x06001A46 RID: 6726 RVA: 0x0007FA20 File Offset: 0x0007DC20
		private unsafe void SetUnmanagedStructures(byte[] buffer)
		{
			this.requestBuffer = SafeLocalFree.LocalAlloc(buffer.Length);
			byte* ptr = (byte*)(void*)this.requestBuffer.DangerousGetHandle();
			for (int i = 0; i < buffer.Length; i++)
			{
				ptr[i] = buffer[i];
			}
		}

		// Token: 0x06001A47 RID: 6727 RVA: 0x0007FA61 File Offset: 0x0007DC61
		private void FreeUnmanagedStructures()
		{
			if (this.requestBuffer != null)
			{
				this.requestBuffer.Close();
				this.requestBuffer = null;
			}
		}

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06001A48 RID: 6728 RVA: 0x0007FA80 File Offset: 0x0007DC80
		private byte[] DefaultSendBuffer
		{
			get
			{
				if (this.defaultSendBuffer == null)
				{
					this.defaultSendBuffer = new byte[32];
					for (int i = 0; i < 32; i++)
					{
						this.defaultSendBuffer[i] = (byte)(97 + i % 23);
					}
				}
				return this.defaultSendBuffer;
			}
		}

		// Token: 0x04001A65 RID: 6757
		private const int MaxUdpPacket = 65791;

		// Token: 0x04001A66 RID: 6758
		private const int MaxBufferSize = 65500;

		// Token: 0x04001A67 RID: 6759
		private const int DefaultTimeout = 5000;

		// Token: 0x04001A68 RID: 6760
		private const int DefaultSendBufferSize = 32;

		// Token: 0x04001A69 RID: 6761
		private byte[] defaultSendBuffer;

		// Token: 0x04001A6A RID: 6762
		private bool ipv6;

		// Token: 0x04001A6B RID: 6763
		private bool cancelled;

		// Token: 0x04001A6C RID: 6764
		private bool disposeRequested;

		// Token: 0x04001A6D RID: 6765
		private object lockObject = new object();

		// Token: 0x04001A6E RID: 6766
		internal ManualResetEvent pingEvent;

		// Token: 0x04001A6F RID: 6767
		private RegisteredWaitHandle registeredWait;

		// Token: 0x04001A70 RID: 6768
		private SafeLocalFree requestBuffer;

		// Token: 0x04001A71 RID: 6769
		private SafeLocalFree replyBuffer;

		// Token: 0x04001A72 RID: 6770
		private int sendSize;

		// Token: 0x04001A73 RID: 6771
		private SafeCloseIcmpHandle handlePingV4;

		// Token: 0x04001A74 RID: 6772
		private SafeCloseIcmpHandle handlePingV6;

		// Token: 0x04001A75 RID: 6773
		private AsyncOperation asyncOp;

		// Token: 0x04001A76 RID: 6774
		private SendOrPostCallback onPingCompletedDelegate;

		// Token: 0x04001A78 RID: 6776
		private ManualResetEvent asyncFinished;

		// Token: 0x04001A79 RID: 6777
		private const int Free = 0;

		// Token: 0x04001A7A RID: 6778
		private const int InProgress = 1;

		// Token: 0x04001A7B RID: 6779
		private new const int Disposed = 2;

		// Token: 0x04001A7C RID: 6780
		private int status;

		// Token: 0x020007A6 RID: 1958
		internal class AsyncStateObject
		{
			// Token: 0x06004315 RID: 17173 RVA: 0x001198C4 File Offset: 0x00117AC4
			internal AsyncStateObject(string hostName, byte[] buffer, int timeout, PingOptions options, object userToken)
			{
				this.hostName = hostName;
				this.buffer = buffer;
				this.timeout = timeout;
				this.options = options;
				this.userToken = userToken;
			}

			// Token: 0x040033D8 RID: 13272
			internal byte[] buffer;

			// Token: 0x040033D9 RID: 13273
			internal string hostName;

			// Token: 0x040033DA RID: 13274
			internal int timeout;

			// Token: 0x040033DB RID: 13275
			internal PingOptions options;

			// Token: 0x040033DC RID: 13276
			internal object userToken;
		}
	}
}
