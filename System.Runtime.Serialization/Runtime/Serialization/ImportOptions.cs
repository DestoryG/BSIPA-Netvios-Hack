using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace System.Runtime.Serialization
{
	// Token: 0x02000096 RID: 150
	public class ImportOptions
	{
		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000A72 RID: 2674 RVA: 0x0002CB20 File Offset: 0x0002AD20
		// (set) Token: 0x06000A73 RID: 2675 RVA: 0x0002CB28 File Offset: 0x0002AD28
		public bool GenerateSerializable
		{
			get
			{
				return this.generateSerializable;
			}
			set
			{
				this.generateSerializable = value;
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000A74 RID: 2676 RVA: 0x0002CB31 File Offset: 0x0002AD31
		// (set) Token: 0x06000A75 RID: 2677 RVA: 0x0002CB39 File Offset: 0x0002AD39
		public bool GenerateInternal
		{
			get
			{
				return this.generateInternal;
			}
			set
			{
				this.generateInternal = value;
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000A76 RID: 2678 RVA: 0x0002CB42 File Offset: 0x0002AD42
		// (set) Token: 0x06000A77 RID: 2679 RVA: 0x0002CB4A File Offset: 0x0002AD4A
		public bool EnableDataBinding
		{
			get
			{
				return this.enableDataBinding;
			}
			set
			{
				this.enableDataBinding = value;
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000A78 RID: 2680 RVA: 0x0002CB53 File Offset: 0x0002AD53
		// (set) Token: 0x06000A79 RID: 2681 RVA: 0x0002CB5B File Offset: 0x0002AD5B
		public CodeDomProvider CodeProvider
		{
			get
			{
				return this.codeProvider;
			}
			set
			{
				this.codeProvider = value;
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000A7A RID: 2682 RVA: 0x0002CB64 File Offset: 0x0002AD64
		public ICollection<Type> ReferencedTypes
		{
			get
			{
				if (this.referencedTypes == null)
				{
					this.referencedTypes = new List<Type>();
				}
				return this.referencedTypes;
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000A7B RID: 2683 RVA: 0x0002CB7F File Offset: 0x0002AD7F
		public ICollection<Type> ReferencedCollectionTypes
		{
			get
			{
				if (this.referencedCollectionTypes == null)
				{
					this.referencedCollectionTypes = new List<Type>();
				}
				return this.referencedCollectionTypes;
			}
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000A7C RID: 2684 RVA: 0x0002CB9A File Offset: 0x0002AD9A
		public IDictionary<string, string> Namespaces
		{
			get
			{
				if (this.namespaces == null)
				{
					this.namespaces = new Dictionary<string, string>();
				}
				return this.namespaces;
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000A7D RID: 2685 RVA: 0x0002CBB5 File Offset: 0x0002ADB5
		// (set) Token: 0x06000A7E RID: 2686 RVA: 0x0002CBBD File Offset: 0x0002ADBD
		public bool ImportXmlType
		{
			get
			{
				return this.importXmlType;
			}
			set
			{
				this.importXmlType = value;
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000A7F RID: 2687 RVA: 0x0002CBC6 File Offset: 0x0002ADC6
		// (set) Token: 0x06000A80 RID: 2688 RVA: 0x0002CBCE File Offset: 0x0002ADCE
		public IDataContractSurrogate DataContractSurrogate
		{
			get
			{
				return this.dataContractSurrogate;
			}
			set
			{
				this.dataContractSurrogate = value;
			}
		}

		// Token: 0x0400048D RID: 1165
		private bool generateSerializable;

		// Token: 0x0400048E RID: 1166
		private bool generateInternal;

		// Token: 0x0400048F RID: 1167
		private bool enableDataBinding;

		// Token: 0x04000490 RID: 1168
		private CodeDomProvider codeProvider;

		// Token: 0x04000491 RID: 1169
		private ICollection<Type> referencedTypes;

		// Token: 0x04000492 RID: 1170
		private ICollection<Type> referencedCollectionTypes;

		// Token: 0x04000493 RID: 1171
		private IDictionary<string, string> namespaces;

		// Token: 0x04000494 RID: 1172
		private bool importXmlType;

		// Token: 0x04000495 RID: 1173
		private IDataContractSurrogate dataContractSurrogate;
	}
}
