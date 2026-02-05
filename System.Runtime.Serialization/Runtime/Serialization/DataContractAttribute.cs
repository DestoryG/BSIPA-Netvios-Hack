using System;

namespace System.Runtime.Serialization
{
	// Token: 0x02000075 RID: 117
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum, Inherited = false, AllowMultiple = false)]
	public sealed class DataContractAttribute : Attribute
	{
		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000899 RID: 2201 RVA: 0x000280CA File Offset: 0x000262CA
		// (set) Token: 0x0600089A RID: 2202 RVA: 0x000280D2 File Offset: 0x000262D2
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

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x0600089B RID: 2203 RVA: 0x000280E2 File Offset: 0x000262E2
		public bool IsReferenceSetExplicitly
		{
			get
			{
				return this.isReferenceSetExplicitly;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x0600089C RID: 2204 RVA: 0x000280EA File Offset: 0x000262EA
		// (set) Token: 0x0600089D RID: 2205 RVA: 0x000280F2 File Offset: 0x000262F2
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

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x0600089E RID: 2206 RVA: 0x00028102 File Offset: 0x00026302
		public bool IsNamespaceSetExplicitly
		{
			get
			{
				return this.isNamespaceSetExplicitly;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x0600089F RID: 2207 RVA: 0x0002810A File Offset: 0x0002630A
		// (set) Token: 0x060008A0 RID: 2208 RVA: 0x00028112 File Offset: 0x00026312
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

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060008A1 RID: 2209 RVA: 0x00028122 File Offset: 0x00026322
		public bool IsNameSetExplicitly
		{
			get
			{
				return this.isNameSetExplicitly;
			}
		}

		// Token: 0x04000323 RID: 803
		private string name;

		// Token: 0x04000324 RID: 804
		private string ns;

		// Token: 0x04000325 RID: 805
		private bool isNameSetExplicitly;

		// Token: 0x04000326 RID: 806
		private bool isNamespaceSetExplicitly;

		// Token: 0x04000327 RID: 807
		private bool isReference;

		// Token: 0x04000328 RID: 808
		private bool isReferenceSetExplicitly;
	}
}
