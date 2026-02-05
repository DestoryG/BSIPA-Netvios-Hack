using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Net.Sockets;
using System.Threading;

namespace System.Net.NetworkInformation
{
	// Token: 0x020002DC RID: 732
	[global::__DynamicallyInvokable]
	public class NetworkChange
	{
		// Token: 0x060019D7 RID: 6615 RVA: 0x0007E19A File Offset: 0x0007C39A
		[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public NetworkChange()
		{
		}

		// Token: 0x060019D8 RID: 6616 RVA: 0x0007E1A2 File Offset: 0x0007C3A2
		[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void RegisterNetworkChange(NetworkChange nc)
		{
		}

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x060019D9 RID: 6617 RVA: 0x0007E1A4 File Offset: 0x0007C3A4
		// (remove) Token: 0x060019DA RID: 6618 RVA: 0x0007E1AC File Offset: 0x0007C3AC
		public static event NetworkAvailabilityChangedEventHandler NetworkAvailabilityChanged
		{
			add
			{
				NetworkChange.AvailabilityChangeListener.Start(value);
			}
			remove
			{
				NetworkChange.AvailabilityChangeListener.Stop(value);
			}
		}

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x060019DB RID: 6619 RVA: 0x0007E1B4 File Offset: 0x0007C3B4
		// (remove) Token: 0x060019DC RID: 6620 RVA: 0x0007E1BC File Offset: 0x0007C3BC
		[global::__DynamicallyInvokable]
		public static event NetworkAddressChangedEventHandler NetworkAddressChanged
		{
			[global::__DynamicallyInvokable]
			add
			{
				NetworkChange.AddressChangeListener.Start(value);
			}
			[global::__DynamicallyInvokable]
			remove
			{
				NetworkChange.AddressChangeListener.Stop(value);
			}
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x060019DD RID: 6621 RVA: 0x0007E1C4 File Offset: 0x0007C3C4
		internal static bool CanListenForNetworkChanges
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04001A45 RID: 6725
		private static readonly object s_globalLock = new object();

		// Token: 0x04001A46 RID: 6726
		private static readonly object s_protectCallbackLock = new object();

		// Token: 0x020007A4 RID: 1956
		internal static class AvailabilityChangeListener
		{
			// Token: 0x06004309 RID: 17161 RVA: 0x0011920D File Offset: 0x0011740D
			private static void RunHandlerCallback(object state)
			{
				((NetworkAvailabilityChangedEventHandler)state)(null, new NetworkAvailabilityEventArgs(NetworkChange.AvailabilityChangeListener.isAvailable));
			}

			// Token: 0x0600430A RID: 17162 RVA: 0x00119228 File Offset: 0x00117428
			private static void ChangedAddress(object sender, EventArgs eventArgs)
			{
				DictionaryEntry[] array = null;
				object s_globalLock = NetworkChange.s_globalLock;
				lock (s_globalLock)
				{
					bool flag2 = SystemNetworkInterface.InternalGetIsNetworkAvailable();
					if (flag2 != NetworkChange.AvailabilityChangeListener.isAvailable)
					{
						NetworkChange.AvailabilityChangeListener.isAvailable = flag2;
						array = new DictionaryEntry[NetworkChange.AvailabilityChangeListener.s_availabilityCallerArray.Count];
						NetworkChange.AvailabilityChangeListener.s_availabilityCallerArray.CopyTo(array, 0);
					}
				}
				if (array != null)
				{
					object s_protectCallbackLock = NetworkChange.s_protectCallbackLock;
					lock (s_protectCallbackLock)
					{
						foreach (DictionaryEntry dictionaryEntry in array)
						{
							NetworkAvailabilityChangedEventHandler networkAvailabilityChangedEventHandler = (NetworkAvailabilityChangedEventHandler)dictionaryEntry.Key;
							ExecutionContext executionContext = (ExecutionContext)dictionaryEntry.Value;
							if (executionContext == null)
							{
								networkAvailabilityChangedEventHandler(null, new NetworkAvailabilityEventArgs(NetworkChange.AvailabilityChangeListener.isAvailable));
							}
							else
							{
								ExecutionContext.Run(executionContext.CreateCopy(), NetworkChange.AvailabilityChangeListener.s_RunHandlerCallback, networkAvailabilityChangedEventHandler);
							}
						}
					}
				}
			}

			// Token: 0x0600430B RID: 17163 RVA: 0x00119334 File Offset: 0x00117534
			internal static void Start(NetworkAvailabilityChangedEventHandler caller)
			{
				object s_globalLock = NetworkChange.s_globalLock;
				lock (s_globalLock)
				{
					if (NetworkChange.AvailabilityChangeListener.s_availabilityCallerArray.Count == 0)
					{
						NetworkChange.AvailabilityChangeListener.isAvailable = NetworkInterface.GetIsNetworkAvailable();
						NetworkChange.AddressChangeListener.UnsafeStart(NetworkChange.AvailabilityChangeListener.addressChange);
					}
					if (caller != null && !NetworkChange.AvailabilityChangeListener.s_availabilityCallerArray.Contains(caller))
					{
						NetworkChange.AvailabilityChangeListener.s_availabilityCallerArray.Add(caller, ExecutionContext.Capture());
					}
				}
			}

			// Token: 0x0600430C RID: 17164 RVA: 0x001193B0 File Offset: 0x001175B0
			internal static void Stop(NetworkAvailabilityChangedEventHandler caller)
			{
				object s_globalLock = NetworkChange.s_globalLock;
				lock (s_globalLock)
				{
					NetworkChange.AvailabilityChangeListener.s_availabilityCallerArray.Remove(caller);
					if (NetworkChange.AvailabilityChangeListener.s_availabilityCallerArray.Count == 0)
					{
						NetworkChange.AddressChangeListener.Stop(NetworkChange.AvailabilityChangeListener.addressChange);
					}
				}
			}

			// Token: 0x040033CB RID: 13259
			private static ListDictionary s_availabilityCallerArray = new ListDictionary();

			// Token: 0x040033CC RID: 13260
			private static NetworkAddressChangedEventHandler addressChange = new NetworkAddressChangedEventHandler(NetworkChange.AvailabilityChangeListener.ChangedAddress);

			// Token: 0x040033CD RID: 13261
			private static volatile bool isAvailable = false;

			// Token: 0x040033CE RID: 13262
			private static ContextCallback s_RunHandlerCallback = new ContextCallback(NetworkChange.AvailabilityChangeListener.RunHandlerCallback);
		}

		// Token: 0x020007A5 RID: 1957
		internal static class AddressChangeListener
		{
			// Token: 0x0600430E RID: 17166 RVA: 0x00119444 File Offset: 0x00117644
			private static void AddressChangedCallback(object stateObject, bool signaled)
			{
				DictionaryEntry[] array = null;
				object s_globalLock = NetworkChange.s_globalLock;
				lock (s_globalLock)
				{
					NetworkChange.AddressChangeListener.s_isPending = false;
					if (!NetworkChange.AddressChangeListener.s_isListening)
					{
						return;
					}
					NetworkChange.AddressChangeListener.s_isListening = false;
					if (NetworkChange.AddressChangeListener.s_callerArray.Count > 0)
					{
						array = new DictionaryEntry[NetworkChange.AddressChangeListener.s_callerArray.Count];
						NetworkChange.AddressChangeListener.s_callerArray.CopyTo(array, 0);
					}
					try
					{
						NetworkChange.AddressChangeListener.StartHelper(null, false, (StartIPOptions)stateObject);
					}
					catch (NetworkInformationException ex)
					{
						if (Logging.On)
						{
							Logging.Exception(Logging.Web, "AddressChangeListener", "AddressChangedCallback", ex);
						}
					}
				}
				if (array != null)
				{
					object s_protectCallbackLock = NetworkChange.s_protectCallbackLock;
					lock (s_protectCallbackLock)
					{
						foreach (DictionaryEntry dictionaryEntry in array)
						{
							NetworkAddressChangedEventHandler networkAddressChangedEventHandler = (NetworkAddressChangedEventHandler)dictionaryEntry.Key;
							ExecutionContext executionContext = (ExecutionContext)dictionaryEntry.Value;
							if (executionContext == null)
							{
								networkAddressChangedEventHandler(null, EventArgs.Empty);
							}
							else
							{
								ExecutionContext.Run(executionContext.CreateCopy(), NetworkChange.AddressChangeListener.s_runHandlerCallback, networkAddressChangedEventHandler);
							}
						}
					}
				}
			}

			// Token: 0x0600430F RID: 17167 RVA: 0x00119590 File Offset: 0x00117790
			private static void RunHandlerCallback(object state)
			{
				((NetworkAddressChangedEventHandler)state)(null, EventArgs.Empty);
			}

			// Token: 0x06004310 RID: 17168 RVA: 0x001195A3 File Offset: 0x001177A3
			internal static void Start(NetworkAddressChangedEventHandler caller)
			{
				NetworkChange.AddressChangeListener.StartHelper(caller, true, StartIPOptions.Both);
			}

			// Token: 0x06004311 RID: 17169 RVA: 0x001195AD File Offset: 0x001177AD
			internal static void UnsafeStart(NetworkAddressChangedEventHandler caller)
			{
				NetworkChange.AddressChangeListener.StartHelper(caller, false, StartIPOptions.Both);
			}

			// Token: 0x06004312 RID: 17170 RVA: 0x001195B8 File Offset: 0x001177B8
			private static void StartHelper(NetworkAddressChangedEventHandler caller, bool captureContext, StartIPOptions startIPOptions)
			{
				object s_globalLock = NetworkChange.s_globalLock;
				lock (s_globalLock)
				{
					if (NetworkChange.AddressChangeListener.s_ipv4Socket == null)
					{
						Socket.InitializeSockets();
						if (Socket.OSSupportsIPv4)
						{
							int num = -1;
							NetworkChange.AddressChangeListener.s_ipv4Socket = SafeCloseSocketAndEvent.CreateWSASocketWithEvent(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP, true, false);
							UnsafeNclNativeMethods.OSSOCK.ioctlsocket(NetworkChange.AddressChangeListener.s_ipv4Socket, -2147195266, ref num);
							NetworkChange.AddressChangeListener.s_ipv4WaitHandle = NetworkChange.AddressChangeListener.s_ipv4Socket.GetEventHandle();
						}
						if (Socket.OSSupportsIPv6)
						{
							int num = -1;
							NetworkChange.AddressChangeListener.s_ipv6Socket = SafeCloseSocketAndEvent.CreateWSASocketWithEvent(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.IP, true, false);
							UnsafeNclNativeMethods.OSSOCK.ioctlsocket(NetworkChange.AddressChangeListener.s_ipv6Socket, -2147195266, ref num);
							NetworkChange.AddressChangeListener.s_ipv6WaitHandle = NetworkChange.AddressChangeListener.s_ipv6Socket.GetEventHandle();
						}
					}
					if (caller != null && !NetworkChange.AddressChangeListener.s_callerArray.Contains(caller))
					{
						NetworkChange.AddressChangeListener.s_callerArray.Add(caller, captureContext ? ExecutionContext.Capture() : null);
					}
					if (!NetworkChange.AddressChangeListener.s_isListening && NetworkChange.AddressChangeListener.s_callerArray.Count != 0)
					{
						if (!NetworkChange.AddressChangeListener.s_isPending)
						{
							if (Socket.OSSupportsIPv4 && (startIPOptions & StartIPOptions.StartIPv4) != StartIPOptions.None)
							{
								NetworkChange.AddressChangeListener.s_registeredWait = ThreadPool.UnsafeRegisterWaitForSingleObject(NetworkChange.AddressChangeListener.s_ipv4WaitHandle, new WaitOrTimerCallback(NetworkChange.AddressChangeListener.AddressChangedCallback), StartIPOptions.StartIPv4, -1, true);
								int num2;
								SocketError socketError = UnsafeNclNativeMethods.OSSOCK.WSAIoctl_Blocking(NetworkChange.AddressChangeListener.s_ipv4Socket.DangerousGetHandle(), 671088663, null, 0, null, 0, out num2, SafeNativeOverlapped.Zero, IntPtr.Zero);
								if (socketError != SocketError.Success)
								{
									NetworkInformationException ex = new NetworkInformationException();
									if ((long)ex.ErrorCode != 10035L)
									{
										throw ex;
									}
								}
								socketError = UnsafeNclNativeMethods.OSSOCK.WSAEventSelect(NetworkChange.AddressChangeListener.s_ipv4Socket, NetworkChange.AddressChangeListener.s_ipv4Socket.GetEventHandle().SafeWaitHandle, AsyncEventBits.FdAddressListChange);
								if (socketError != SocketError.Success)
								{
									throw new NetworkInformationException();
								}
							}
							if (Socket.OSSupportsIPv6 && (startIPOptions & StartIPOptions.StartIPv6) != StartIPOptions.None)
							{
								NetworkChange.AddressChangeListener.s_registeredWait = ThreadPool.UnsafeRegisterWaitForSingleObject(NetworkChange.AddressChangeListener.s_ipv6WaitHandle, new WaitOrTimerCallback(NetworkChange.AddressChangeListener.AddressChangedCallback), StartIPOptions.StartIPv6, -1, true);
								int num2;
								SocketError socketError = UnsafeNclNativeMethods.OSSOCK.WSAIoctl_Blocking(NetworkChange.AddressChangeListener.s_ipv6Socket.DangerousGetHandle(), 671088663, null, 0, null, 0, out num2, SafeNativeOverlapped.Zero, IntPtr.Zero);
								if (socketError != SocketError.Success)
								{
									NetworkInformationException ex2 = new NetworkInformationException();
									if ((long)ex2.ErrorCode != 10035L)
									{
										throw ex2;
									}
								}
								socketError = UnsafeNclNativeMethods.OSSOCK.WSAEventSelect(NetworkChange.AddressChangeListener.s_ipv6Socket, NetworkChange.AddressChangeListener.s_ipv6Socket.GetEventHandle().SafeWaitHandle, AsyncEventBits.FdAddressListChange);
								if (socketError != SocketError.Success)
								{
									throw new NetworkInformationException();
								}
							}
						}
						NetworkChange.AddressChangeListener.s_isListening = true;
						NetworkChange.AddressChangeListener.s_isPending = true;
					}
				}
			}

			// Token: 0x06004313 RID: 17171 RVA: 0x00119818 File Offset: 0x00117A18
			internal static void Stop(object caller)
			{
				object s_globalLock = NetworkChange.s_globalLock;
				lock (s_globalLock)
				{
					NetworkChange.AddressChangeListener.s_callerArray.Remove(caller);
					if (NetworkChange.AddressChangeListener.s_callerArray.Count == 0 && NetworkChange.AddressChangeListener.s_isListening)
					{
						NetworkChange.AddressChangeListener.s_isListening = false;
					}
				}
			}

			// Token: 0x040033CF RID: 13263
			private static ListDictionary s_callerArray = new ListDictionary();

			// Token: 0x040033D0 RID: 13264
			private static ContextCallback s_runHandlerCallback = new ContextCallback(NetworkChange.AddressChangeListener.RunHandlerCallback);

			// Token: 0x040033D1 RID: 13265
			private static RegisteredWaitHandle s_registeredWait;

			// Token: 0x040033D2 RID: 13266
			private static bool s_isListening = false;

			// Token: 0x040033D3 RID: 13267
			private static bool s_isPending = false;

			// Token: 0x040033D4 RID: 13268
			private static SafeCloseSocketAndEvent s_ipv4Socket = null;

			// Token: 0x040033D5 RID: 13269
			private static SafeCloseSocketAndEvent s_ipv6Socket = null;

			// Token: 0x040033D6 RID: 13270
			private static WaitHandle s_ipv4WaitHandle = null;

			// Token: 0x040033D7 RID: 13271
			private static WaitHandle s_ipv6WaitHandle = null;
		}
	}
}
