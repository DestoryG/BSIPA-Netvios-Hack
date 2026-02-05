using System;

namespace System.Net
{
	// Token: 0x0200011F RID: 287
	internal enum ContextAttribute
	{
		// Token: 0x04000FB5 RID: 4021
		Sizes,
		// Token: 0x04000FB6 RID: 4022
		Names,
		// Token: 0x04000FB7 RID: 4023
		Lifespan,
		// Token: 0x04000FB8 RID: 4024
		DceInfo,
		// Token: 0x04000FB9 RID: 4025
		StreamSizes,
		// Token: 0x04000FBA RID: 4026
		Authority = 6,
		// Token: 0x04000FBB RID: 4027
		PackageInfo = 10,
		// Token: 0x04000FBC RID: 4028
		NegotiationInfo = 12,
		// Token: 0x04000FBD RID: 4029
		UniqueBindings = 25,
		// Token: 0x04000FBE RID: 4030
		EndpointBindings,
		// Token: 0x04000FBF RID: 4031
		ClientSpecifiedSpn,
		// Token: 0x04000FC0 RID: 4032
		RemoteCertificate = 83,
		// Token: 0x04000FC1 RID: 4033
		LocalCertificate,
		// Token: 0x04000FC2 RID: 4034
		RootStore,
		// Token: 0x04000FC3 RID: 4035
		IssuerListInfoEx = 89,
		// Token: 0x04000FC4 RID: 4036
		ConnectionInfo,
		// Token: 0x04000FC5 RID: 4037
		UiInfo = 104
	}
}
