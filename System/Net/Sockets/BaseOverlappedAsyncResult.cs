using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Net.Sockets
{
	// Token: 0x0200038C RID: 908
	internal class BaseOverlappedAsyncResult : ContextAwareResult
	{
		// Token: 0x06002214 RID: 8724 RVA: 0x000A301E File Offset: 0x000A121E
		internal BaseOverlappedAsyncResult(Socket socket, object asyncState, AsyncCallback asyncCallback)
			: base(socket, asyncState, asyncCallback)
		{
			this.m_UseOverlappedIO = Socket.UseOverlappedIO || socket.UseOnlyOverlappedIO;
			if (this.m_UseOverlappedIO)
			{
				this.m_CleanupCount = 1;
				return;
			}
			this.m_CleanupCount = 2;
		}

		// Token: 0x06002215 RID: 8725 RVA: 0x000A3058 File Offset: 0x000A1258
		internal BaseOverlappedAsyncResult(Socket socket)
			: base(socket, null, null)
		{
			this.m_CleanupCount = 1;
			this.m_DisableOverlapped = true;
		}

		// Token: 0x06002216 RID: 8726 RVA: 0x000A3071 File Offset: 0x000A1271
		internal virtual object PostCompletion(int numBytes)
		{
			return numBytes;
		}

		// Token: 0x06002217 RID: 8727 RVA: 0x000A307C File Offset: 0x000A127C
		internal void SetUnmanagedStructures(object objectsToPin)
		{
			if (!this.m_DisableOverlapped)
			{
				object[] array = null;
				bool flag = false;
				bool flag2 = false;
				if (this.m_Cache != null)
				{
					if (objectsToPin == null && this.m_Cache.PinnedObjects == null)
					{
						flag2 = true;
					}
					else if (this.m_Cache.PinnedObjects != null)
					{
						if (this.m_Cache.PinnedObjectsArray == null)
						{
							if (objectsToPin == this.m_Cache.PinnedObjects)
							{
								flag2 = true;
							}
						}
						else if (objectsToPin != null)
						{
							flag = true;
							array = objectsToPin as object[];
							if (array != null && array.Length == 0)
							{
								array = null;
							}
							if (array != null && array.Length == this.m_Cache.PinnedObjectsArray.Length)
							{
								flag2 = true;
								for (int i = 0; i < array.Length; i++)
								{
									if (array[i] != this.m_Cache.PinnedObjectsArray[i])
									{
										flag2 = false;
										break;
									}
								}
							}
						}
					}
				}
				if (!flag2 && this.m_Cache != null)
				{
					this.m_Cache.Free();
					this.m_Cache = null;
				}
				Socket socket = (Socket)base.AsyncObject;
				if (this.m_UseOverlappedIO)
				{
					this.m_UnmanagedBlob = SafeOverlappedFree.Alloc(socket.SafeHandle);
					this.PinUnmanagedObjects(objectsToPin);
					this.m_OverlappedEvent = new AutoResetEvent(false);
					Marshal.WriteIntPtr(this.m_UnmanagedBlob.DangerousGetHandle(), Win32.OverlappedhEventOffset, this.m_OverlappedEvent.SafeWaitHandle.DangerousGetHandle());
					return;
				}
				socket.BindToCompletionPort();
				if (this.m_Cache == null)
				{
					if (array != null)
					{
						this.m_Cache = new OverlappedCache(new Overlapped(), array, BaseOverlappedAsyncResult.s_IOCallback);
					}
					else
					{
						this.m_Cache = new OverlappedCache(new Overlapped(), objectsToPin, BaseOverlappedAsyncResult.s_IOCallback, flag);
					}
				}
				this.m_Cache.Overlapped.AsyncResult = this;
			}
		}

		// Token: 0x06002218 RID: 8728 RVA: 0x000A320A File Offset: 0x000A140A
		protected void SetupCache(ref OverlappedCache overlappedCache)
		{
			if (!this.m_UseOverlappedIO && !this.m_DisableOverlapped)
			{
				this.m_Cache = ((overlappedCache == null) ? null : Interlocked.Exchange<OverlappedCache>(ref overlappedCache, null));
				this.m_CleanupCount++;
			}
		}

		// Token: 0x06002219 RID: 8729 RVA: 0x000A3240 File Offset: 0x000A1440
		protected void PinUnmanagedObjects(object objectsToPin)
		{
			if (this.m_Cache != null)
			{
				this.m_Cache.Free();
				this.m_Cache = null;
			}
			if (objectsToPin != null)
			{
				if (objectsToPin.GetType() == typeof(object[]))
				{
					object[] array = (object[])objectsToPin;
					this.m_GCHandles = new GCHandle[array.Length];
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i] != null)
						{
							this.m_GCHandles[i] = GCHandle.Alloc(array[i], GCHandleType.Pinned);
						}
					}
					return;
				}
				this.m_GCHandles = new GCHandle[1];
				this.m_GCHandles[0] = GCHandle.Alloc(objectsToPin, GCHandleType.Pinned);
			}
		}

		// Token: 0x0600221A RID: 8730 RVA: 0x000A32E0 File Offset: 0x000A14E0
		internal void ExtractCache(ref OverlappedCache overlappedCache)
		{
			if (!this.m_UseOverlappedIO && !this.m_DisableOverlapped)
			{
				OverlappedCache overlappedCache2 = ((this.m_Cache == null) ? null : Interlocked.Exchange<OverlappedCache>(ref this.m_Cache, null));
				if (overlappedCache2 != null)
				{
					if (overlappedCache == null)
					{
						overlappedCache = overlappedCache2;
					}
					else
					{
						OverlappedCache overlappedCache3 = Interlocked.Exchange<OverlappedCache>(ref overlappedCache, overlappedCache2);
						if (overlappedCache3 != null)
						{
							overlappedCache3.Free();
						}
					}
				}
				this.ReleaseUnmanagedStructures();
			}
		}

		// Token: 0x0600221B RID: 8731 RVA: 0x000A3338 File Offset: 0x000A1538
		private unsafe static void CompletionPortCallback(uint errorCode, uint numBytes, NativeOverlapped* nativeOverlapped)
		{
			Overlapped overlapped = Overlapped.Unpack(nativeOverlapped);
			BaseOverlappedAsyncResult baseOverlappedAsyncResult = (BaseOverlappedAsyncResult)overlapped.AsyncResult;
			overlapped.AsyncResult = null;
			SocketError socketError = (SocketError)errorCode;
			if (socketError != SocketError.Success && socketError != SocketError.OperationAborted)
			{
				Socket socket = baseOverlappedAsyncResult.AsyncObject as Socket;
				if (socket == null)
				{
					socketError = SocketError.NotSocket;
				}
				else if (socket.CleanedUp)
				{
					socketError = SocketError.OperationAborted;
				}
				else
				{
					try
					{
						SocketFlags socketFlags;
						if (!UnsafeNclNativeMethods.OSSOCK.WSAGetOverlappedResult(socket.SafeHandle, baseOverlappedAsyncResult.m_Cache.NativeOverlapped, out numBytes, false, out socketFlags))
						{
							socketError = (SocketError)Marshal.GetLastWin32Error();
						}
					}
					catch (ObjectDisposedException)
					{
						socketError = SocketError.OperationAborted;
					}
				}
			}
			baseOverlappedAsyncResult.ErrorCode = (int)socketError;
			object obj = baseOverlappedAsyncResult.PostCompletion((int)numBytes);
			baseOverlappedAsyncResult.ReleaseUnmanagedStructures();
			baseOverlappedAsyncResult.InvokeCallback(obj);
		}

		// Token: 0x0600221C RID: 8732 RVA: 0x000A33F8 File Offset: 0x000A15F8
		private void OverlappedCallback(object stateObject, bool Signaled)
		{
			BaseOverlappedAsyncResult baseOverlappedAsyncResult = (BaseOverlappedAsyncResult)stateObject;
			uint num = (uint)Marshal.ReadInt32(IntPtrHelper.Add(baseOverlappedAsyncResult.m_UnmanagedBlob.DangerousGetHandle(), 0));
			uint num2 = (uint)((num != 0U) ? (-1) : Marshal.ReadInt32(IntPtrHelper.Add(baseOverlappedAsyncResult.m_UnmanagedBlob.DangerousGetHandle(), Win32.OverlappedInternalHighOffset)));
			baseOverlappedAsyncResult.ErrorCode = (int)num;
			object obj = baseOverlappedAsyncResult.PostCompletion((int)num2);
			baseOverlappedAsyncResult.ReleaseUnmanagedStructures();
			baseOverlappedAsyncResult.InvokeCallback(obj);
		}

		// Token: 0x0600221D RID: 8733 RVA: 0x000A3460 File Offset: 0x000A1660
		internal SocketError CheckAsyncCallOverlappedResult(SocketError errorCode)
		{
			if (this.m_UseOverlappedIO)
			{
				if (errorCode == SocketError.Success || errorCode == SocketError.IOPending)
				{
					ThreadPool.UnsafeRegisterWaitForSingleObject(this.m_OverlappedEvent, new WaitOrTimerCallback(this.OverlappedCallback), this, -1, true);
					return SocketError.Success;
				}
				base.ErrorCode = (int)errorCode;
				base.Result = -1;
				this.ReleaseUnmanagedStructures();
			}
			else
			{
				this.ReleaseUnmanagedStructures();
				if (errorCode == SocketError.Success || errorCode == SocketError.IOPending)
				{
					return SocketError.Success;
				}
				base.ErrorCode = (int)errorCode;
				base.Result = -1;
				if (this.m_Cache != null)
				{
					this.m_Cache.Overlapped.AsyncResult = null;
				}
				this.ReleaseUnmanagedStructures();
			}
			return errorCode;
		}

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x0600221E RID: 8734 RVA: 0x000A3500 File Offset: 0x000A1700
		internal SafeHandle OverlappedHandle
		{
			get
			{
				if (this.m_UseOverlappedIO)
				{
					if (this.m_UnmanagedBlob != null && !this.m_UnmanagedBlob.IsInvalid)
					{
						return this.m_UnmanagedBlob;
					}
					return SafeOverlappedFree.Zero;
				}
				else
				{
					if (this.m_Cache != null)
					{
						return this.m_Cache.NativeOverlapped;
					}
					return SafeNativeOverlapped.Zero;
				}
			}
		}

		// Token: 0x0600221F RID: 8735 RVA: 0x000A3550 File Offset: 0x000A1750
		private void ReleaseUnmanagedStructures()
		{
			if (Interlocked.Decrement(ref this.m_CleanupCount) == 0)
			{
				this.ForceReleaseUnmanagedStructures();
			}
		}

		// Token: 0x06002220 RID: 8736 RVA: 0x000A3565 File Offset: 0x000A1765
		protected override void Cleanup()
		{
			base.Cleanup();
			if (this.m_CleanupCount > 0 && Interlocked.Exchange(ref this.m_CleanupCount, 0) > 0)
			{
				this.ForceReleaseUnmanagedStructures();
			}
		}

		// Token: 0x06002221 RID: 8737 RVA: 0x000A358C File Offset: 0x000A178C
		protected virtual void ForceReleaseUnmanagedStructures()
		{
			this.ReleaseGCHandles();
			GC.SuppressFinalize(this);
			if (this.m_UnmanagedBlob != null && !this.m_UnmanagedBlob.IsInvalid)
			{
				this.m_UnmanagedBlob.Close(true);
				this.m_UnmanagedBlob = null;
			}
			OverlappedCache.InterlockedFree(ref this.m_Cache);
			if (this.m_OverlappedEvent != null)
			{
				this.m_OverlappedEvent.Close();
				this.m_OverlappedEvent = null;
			}
		}

		// Token: 0x06002222 RID: 8738 RVA: 0x000A35F4 File Offset: 0x000A17F4
		~BaseOverlappedAsyncResult()
		{
			this.ReleaseGCHandles();
		}

		// Token: 0x06002223 RID: 8739 RVA: 0x000A3620 File Offset: 0x000A1820
		private void ReleaseGCHandles()
		{
			GCHandle[] gchandles = this.m_GCHandles;
			if (gchandles != null)
			{
				for (int i = 0; i < gchandles.Length; i++)
				{
					if (gchandles[i].IsAllocated)
					{
						gchandles[i].Free();
					}
				}
			}
		}

		// Token: 0x04001F54 RID: 8020
		private SafeOverlappedFree m_UnmanagedBlob;

		// Token: 0x04001F55 RID: 8021
		private AutoResetEvent m_OverlappedEvent;

		// Token: 0x04001F56 RID: 8022
		private int m_CleanupCount;

		// Token: 0x04001F57 RID: 8023
		private bool m_DisableOverlapped;

		// Token: 0x04001F58 RID: 8024
		private bool m_UseOverlappedIO;

		// Token: 0x04001F59 RID: 8025
		private GCHandle[] m_GCHandles;

		// Token: 0x04001F5A RID: 8026
		private OverlappedCache m_Cache;

		// Token: 0x04001F5B RID: 8027
		private static readonly IOCompletionCallback s_IOCallback = new IOCompletionCallback(BaseOverlappedAsyncResult.CompletionPortCallback);
	}
}
