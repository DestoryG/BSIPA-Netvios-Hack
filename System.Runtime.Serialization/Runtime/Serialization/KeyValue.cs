using System;

namespace System.Runtime.Serialization
{
	// Token: 0x02000067 RID: 103
	[DataContract(Namespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays")]
	internal struct KeyValue<K, V>
	{
		// Token: 0x0600079B RID: 1947 RVA: 0x00024E44 File Offset: 0x00023044
		internal KeyValue(K key, V value)
		{
			this.key = key;
			this.value = value;
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600079C RID: 1948 RVA: 0x00024E54 File Offset: 0x00023054
		// (set) Token: 0x0600079D RID: 1949 RVA: 0x00024E5C File Offset: 0x0002305C
		[DataMember(IsRequired = true)]
		public K Key
		{
			get
			{
				return this.key;
			}
			set
			{
				this.key = value;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600079E RID: 1950 RVA: 0x00024E65 File Offset: 0x00023065
		// (set) Token: 0x0600079F RID: 1951 RVA: 0x00024E6D File Offset: 0x0002306D
		[DataMember(IsRequired = true)]
		public V Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}

		// Token: 0x040002ED RID: 749
		private K key;

		// Token: 0x040002EE RID: 750
		private V value;
	}
}
