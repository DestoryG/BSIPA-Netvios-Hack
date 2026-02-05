using System;

namespace System.Net.Sockets
{
	// Token: 0x0200036B RID: 875
	public enum IOControlCode : long
	{
		// Token: 0x04001DB0 RID: 7600
		AsyncIO = 2147772029L,
		// Token: 0x04001DB1 RID: 7601
		NonBlockingIO,
		// Token: 0x04001DB2 RID: 7602
		DataToRead = 1074030207L,
		// Token: 0x04001DB3 RID: 7603
		OobDataRead = 1074033415L,
		// Token: 0x04001DB4 RID: 7604
		AssociateHandle = 2281701377L,
		// Token: 0x04001DB5 RID: 7605
		EnableCircularQueuing = 671088642L,
		// Token: 0x04001DB6 RID: 7606
		Flush = 671088644L,
		// Token: 0x04001DB7 RID: 7607
		GetBroadcastAddress = 1207959557L,
		// Token: 0x04001DB8 RID: 7608
		GetExtensionFunctionPointer = 3355443206L,
		// Token: 0x04001DB9 RID: 7609
		GetQos,
		// Token: 0x04001DBA RID: 7610
		GetGroupQos,
		// Token: 0x04001DBB RID: 7611
		MultipointLoopback = 2281701385L,
		// Token: 0x04001DBC RID: 7612
		MulticastScope,
		// Token: 0x04001DBD RID: 7613
		SetQos,
		// Token: 0x04001DBE RID: 7614
		SetGroupQos,
		// Token: 0x04001DBF RID: 7615
		TranslateHandle = 3355443213L,
		// Token: 0x04001DC0 RID: 7616
		RoutingInterfaceQuery = 3355443220L,
		// Token: 0x04001DC1 RID: 7617
		RoutingInterfaceChange = 2281701397L,
		// Token: 0x04001DC2 RID: 7618
		AddressListQuery = 1207959574L,
		// Token: 0x04001DC3 RID: 7619
		AddressListChange = 671088663L,
		// Token: 0x04001DC4 RID: 7620
		QueryTargetPnpHandle = 1207959576L,
		// Token: 0x04001DC5 RID: 7621
		NamespaceChange = 2281701401L,
		// Token: 0x04001DC6 RID: 7622
		AddressListSort = 3355443225L,
		// Token: 0x04001DC7 RID: 7623
		ReceiveAll = 2550136833L,
		// Token: 0x04001DC8 RID: 7624
		ReceiveAllMulticast,
		// Token: 0x04001DC9 RID: 7625
		ReceiveAllIgmpMulticast,
		// Token: 0x04001DCA RID: 7626
		KeepAliveValues,
		// Token: 0x04001DCB RID: 7627
		AbsorbRouterAlert,
		// Token: 0x04001DCC RID: 7628
		UnicastInterface,
		// Token: 0x04001DCD RID: 7629
		LimitBroadcasts,
		// Token: 0x04001DCE RID: 7630
		BindToInterface,
		// Token: 0x04001DCF RID: 7631
		MulticastInterface,
		// Token: 0x04001DD0 RID: 7632
		AddMulticastGroupOnInterface,
		// Token: 0x04001DD1 RID: 7633
		DeleteMulticastGroupFromInterface
	}
}
