using System;

namespace System.Net.Sockets
{
	// Token: 0x0200037D RID: 893
	[global::__DynamicallyInvokable]
	public enum SocketError
	{
		// Token: 0x04001EB6 RID: 7862
		[global::__DynamicallyInvokable]
		Success,
		// Token: 0x04001EB7 RID: 7863
		[global::__DynamicallyInvokable]
		SocketError = -1,
		// Token: 0x04001EB8 RID: 7864
		[global::__DynamicallyInvokable]
		Interrupted = 10004,
		// Token: 0x04001EB9 RID: 7865
		[global::__DynamicallyInvokable]
		AccessDenied = 10013,
		// Token: 0x04001EBA RID: 7866
		[global::__DynamicallyInvokable]
		Fault,
		// Token: 0x04001EBB RID: 7867
		[global::__DynamicallyInvokable]
		InvalidArgument = 10022,
		// Token: 0x04001EBC RID: 7868
		[global::__DynamicallyInvokable]
		TooManyOpenSockets = 10024,
		// Token: 0x04001EBD RID: 7869
		[global::__DynamicallyInvokable]
		WouldBlock = 10035,
		// Token: 0x04001EBE RID: 7870
		[global::__DynamicallyInvokable]
		InProgress,
		// Token: 0x04001EBF RID: 7871
		[global::__DynamicallyInvokable]
		AlreadyInProgress,
		// Token: 0x04001EC0 RID: 7872
		[global::__DynamicallyInvokable]
		NotSocket,
		// Token: 0x04001EC1 RID: 7873
		[global::__DynamicallyInvokable]
		DestinationAddressRequired,
		// Token: 0x04001EC2 RID: 7874
		[global::__DynamicallyInvokable]
		MessageSize,
		// Token: 0x04001EC3 RID: 7875
		[global::__DynamicallyInvokable]
		ProtocolType,
		// Token: 0x04001EC4 RID: 7876
		[global::__DynamicallyInvokable]
		ProtocolOption,
		// Token: 0x04001EC5 RID: 7877
		[global::__DynamicallyInvokable]
		ProtocolNotSupported,
		// Token: 0x04001EC6 RID: 7878
		[global::__DynamicallyInvokable]
		SocketNotSupported,
		// Token: 0x04001EC7 RID: 7879
		[global::__DynamicallyInvokable]
		OperationNotSupported,
		// Token: 0x04001EC8 RID: 7880
		[global::__DynamicallyInvokable]
		ProtocolFamilyNotSupported,
		// Token: 0x04001EC9 RID: 7881
		[global::__DynamicallyInvokable]
		AddressFamilyNotSupported,
		// Token: 0x04001ECA RID: 7882
		[global::__DynamicallyInvokable]
		AddressAlreadyInUse,
		// Token: 0x04001ECB RID: 7883
		[global::__DynamicallyInvokable]
		AddressNotAvailable,
		// Token: 0x04001ECC RID: 7884
		[global::__DynamicallyInvokable]
		NetworkDown,
		// Token: 0x04001ECD RID: 7885
		[global::__DynamicallyInvokable]
		NetworkUnreachable,
		// Token: 0x04001ECE RID: 7886
		[global::__DynamicallyInvokable]
		NetworkReset,
		// Token: 0x04001ECF RID: 7887
		[global::__DynamicallyInvokable]
		ConnectionAborted,
		// Token: 0x04001ED0 RID: 7888
		[global::__DynamicallyInvokable]
		ConnectionReset,
		// Token: 0x04001ED1 RID: 7889
		[global::__DynamicallyInvokable]
		NoBufferSpaceAvailable,
		// Token: 0x04001ED2 RID: 7890
		[global::__DynamicallyInvokable]
		IsConnected,
		// Token: 0x04001ED3 RID: 7891
		[global::__DynamicallyInvokable]
		NotConnected,
		// Token: 0x04001ED4 RID: 7892
		[global::__DynamicallyInvokable]
		Shutdown,
		// Token: 0x04001ED5 RID: 7893
		[global::__DynamicallyInvokable]
		TimedOut = 10060,
		// Token: 0x04001ED6 RID: 7894
		[global::__DynamicallyInvokable]
		ConnectionRefused,
		// Token: 0x04001ED7 RID: 7895
		[global::__DynamicallyInvokable]
		HostDown = 10064,
		// Token: 0x04001ED8 RID: 7896
		[global::__DynamicallyInvokable]
		HostUnreachable,
		// Token: 0x04001ED9 RID: 7897
		[global::__DynamicallyInvokable]
		ProcessLimit = 10067,
		// Token: 0x04001EDA RID: 7898
		[global::__DynamicallyInvokable]
		SystemNotReady = 10091,
		// Token: 0x04001EDB RID: 7899
		[global::__DynamicallyInvokable]
		VersionNotSupported,
		// Token: 0x04001EDC RID: 7900
		[global::__DynamicallyInvokable]
		NotInitialized,
		// Token: 0x04001EDD RID: 7901
		[global::__DynamicallyInvokable]
		Disconnecting = 10101,
		// Token: 0x04001EDE RID: 7902
		[global::__DynamicallyInvokable]
		TypeNotFound = 10109,
		// Token: 0x04001EDF RID: 7903
		[global::__DynamicallyInvokable]
		HostNotFound = 11001,
		// Token: 0x04001EE0 RID: 7904
		[global::__DynamicallyInvokable]
		TryAgain,
		// Token: 0x04001EE1 RID: 7905
		[global::__DynamicallyInvokable]
		NoRecovery,
		// Token: 0x04001EE2 RID: 7906
		[global::__DynamicallyInvokable]
		NoData,
		// Token: 0x04001EE3 RID: 7907
		[global::__DynamicallyInvokable]
		IOPending = 997,
		// Token: 0x04001EE4 RID: 7908
		[global::__DynamicallyInvokable]
		OperationAborted = 995
	}
}
