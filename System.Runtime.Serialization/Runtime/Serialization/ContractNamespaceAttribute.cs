using System;

namespace System.Runtime.Serialization
{
	// Token: 0x0200006C RID: 108
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module, Inherited = false, AllowMultiple = true)]
	public sealed class ContractNamespaceAttribute : Attribute
	{
		// Token: 0x060007FC RID: 2044 RVA: 0x000261AC File Offset: 0x000243AC
		public ContractNamespaceAttribute(string contractNamespace)
		{
			this.contractNamespace = contractNamespace;
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060007FD RID: 2045 RVA: 0x000261BB File Offset: 0x000243BB
		// (set) Token: 0x060007FE RID: 2046 RVA: 0x000261C3 File Offset: 0x000243C3
		public string ClrNamespace
		{
			get
			{
				return this.clrNamespace;
			}
			set
			{
				this.clrNamespace = value;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060007FF RID: 2047 RVA: 0x000261CC File Offset: 0x000243CC
		public string ContractNamespace
		{
			get
			{
				return this.contractNamespace;
			}
		}

		// Token: 0x04000312 RID: 786
		private string clrNamespace;

		// Token: 0x04000313 RID: 787
		private string contractNamespace;
	}
}
