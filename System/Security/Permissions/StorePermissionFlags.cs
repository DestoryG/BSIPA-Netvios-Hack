using System;

namespace System.Security.Permissions
{
	// Token: 0x02000486 RID: 1158
	[Flags]
	[Serializable]
	public enum StorePermissionFlags
	{
		// Token: 0x04002654 RID: 9812
		NoFlags = 0,
		// Token: 0x04002655 RID: 9813
		CreateStore = 1,
		// Token: 0x04002656 RID: 9814
		DeleteStore = 2,
		// Token: 0x04002657 RID: 9815
		EnumerateStores = 4,
		// Token: 0x04002658 RID: 9816
		OpenStore = 16,
		// Token: 0x04002659 RID: 9817
		AddToStore = 32,
		// Token: 0x0400265A RID: 9818
		RemoveFromStore = 64,
		// Token: 0x0400265B RID: 9819
		EnumerateCertificates = 128,
		// Token: 0x0400265C RID: 9820
		AllFlags = 247
	}
}
