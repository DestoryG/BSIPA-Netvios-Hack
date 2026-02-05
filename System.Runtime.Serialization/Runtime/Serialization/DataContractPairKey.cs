using System;

namespace System.Runtime.Serialization
{
	// Token: 0x02000071 RID: 113
	internal class DataContractPairKey
	{
		// Token: 0x0600088C RID: 2188 RVA: 0x00027FB4 File Offset: 0x000261B4
		public DataContractPairKey(object object1, object object2)
		{
			this.object1 = object1;
			this.object2 = object2;
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x00027FCC File Offset: 0x000261CC
		public override bool Equals(object other)
		{
			DataContractPairKey dataContractPairKey = other as DataContractPairKey;
			return dataContractPairKey != null && ((dataContractPairKey.object1 == this.object1 && dataContractPairKey.object2 == this.object2) || (dataContractPairKey.object1 == this.object2 && dataContractPairKey.object2 == this.object1));
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x00028021 File Offset: 0x00026221
		public override int GetHashCode()
		{
			return this.object1.GetHashCode() ^ this.object2.GetHashCode();
		}

		// Token: 0x0400031F RID: 799
		private object object1;

		// Token: 0x04000320 RID: 800
		private object object2;
	}
}
