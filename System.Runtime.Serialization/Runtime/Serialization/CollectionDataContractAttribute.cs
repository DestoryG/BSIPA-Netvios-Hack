using System;

namespace System.Runtime.Serialization
{
	// Token: 0x0200006A RID: 106
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
	public sealed class CollectionDataContractAttribute : Attribute
	{
		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060007E6 RID: 2022 RVA: 0x0002603B File Offset: 0x0002423B
		// (set) Token: 0x060007E7 RID: 2023 RVA: 0x00026043 File Offset: 0x00024243
		public string Namespace
		{
			get
			{
				return this.ns;
			}
			set
			{
				this.ns = value;
				this.isNamespaceSetExplicitly = true;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060007E8 RID: 2024 RVA: 0x00026053 File Offset: 0x00024253
		public bool IsNamespaceSetExplicitly
		{
			get
			{
				return this.isNamespaceSetExplicitly;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060007E9 RID: 2025 RVA: 0x0002605B File Offset: 0x0002425B
		// (set) Token: 0x060007EA RID: 2026 RVA: 0x00026063 File Offset: 0x00024263
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
				this.isNameSetExplicitly = true;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060007EB RID: 2027 RVA: 0x00026073 File Offset: 0x00024273
		public bool IsNameSetExplicitly
		{
			get
			{
				return this.isNameSetExplicitly;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060007EC RID: 2028 RVA: 0x0002607B File Offset: 0x0002427B
		// (set) Token: 0x060007ED RID: 2029 RVA: 0x00026083 File Offset: 0x00024283
		public string ItemName
		{
			get
			{
				return this.itemName;
			}
			set
			{
				this.itemName = value;
				this.isItemNameSetExplicitly = true;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060007EE RID: 2030 RVA: 0x00026093 File Offset: 0x00024293
		public bool IsItemNameSetExplicitly
		{
			get
			{
				return this.isItemNameSetExplicitly;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060007EF RID: 2031 RVA: 0x0002609B File Offset: 0x0002429B
		// (set) Token: 0x060007F0 RID: 2032 RVA: 0x000260A3 File Offset: 0x000242A3
		public string KeyName
		{
			get
			{
				return this.keyName;
			}
			set
			{
				this.keyName = value;
				this.isKeyNameSetExplicitly = true;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060007F1 RID: 2033 RVA: 0x000260B3 File Offset: 0x000242B3
		// (set) Token: 0x060007F2 RID: 2034 RVA: 0x000260BB File Offset: 0x000242BB
		public bool IsReference
		{
			get
			{
				return this.isReference;
			}
			set
			{
				this.isReference = value;
				this.isReferenceSetExplicitly = true;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060007F3 RID: 2035 RVA: 0x000260CB File Offset: 0x000242CB
		public bool IsReferenceSetExplicitly
		{
			get
			{
				return this.isReferenceSetExplicitly;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060007F4 RID: 2036 RVA: 0x000260D3 File Offset: 0x000242D3
		public bool IsKeyNameSetExplicitly
		{
			get
			{
				return this.isKeyNameSetExplicitly;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060007F5 RID: 2037 RVA: 0x000260DB File Offset: 0x000242DB
		// (set) Token: 0x060007F6 RID: 2038 RVA: 0x000260E3 File Offset: 0x000242E3
		public string ValueName
		{
			get
			{
				return this.valueName;
			}
			set
			{
				this.valueName = value;
				this.isValueNameSetExplicitly = true;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060007F7 RID: 2039 RVA: 0x000260F3 File Offset: 0x000242F3
		public bool IsValueNameSetExplicitly
		{
			get
			{
				return this.isValueNameSetExplicitly;
			}
		}

		// Token: 0x040002FE RID: 766
		private string name;

		// Token: 0x040002FF RID: 767
		private string ns;

		// Token: 0x04000300 RID: 768
		private string itemName;

		// Token: 0x04000301 RID: 769
		private string keyName;

		// Token: 0x04000302 RID: 770
		private string valueName;

		// Token: 0x04000303 RID: 771
		private bool isReference;

		// Token: 0x04000304 RID: 772
		private bool isNameSetExplicitly;

		// Token: 0x04000305 RID: 773
		private bool isNamespaceSetExplicitly;

		// Token: 0x04000306 RID: 774
		private bool isReferenceSetExplicitly;

		// Token: 0x04000307 RID: 775
		private bool isItemNameSetExplicitly;

		// Token: 0x04000308 RID: 776
		private bool isKeyNameSetExplicitly;

		// Token: 0x04000309 RID: 777
		private bool isValueNameSetExplicitly;
	}
}
