using System;

namespace Mono.Cecil
{
	// Token: 0x020000A1 RID: 161
	public abstract class ParameterReference : IMetadataTokenProvider
	{
		// Token: 0x170001EA RID: 490
		// (get) Token: 0x060006FC RID: 1788 RVA: 0x00016A5E File Offset: 0x00014C5E
		// (set) Token: 0x060006FD RID: 1789 RVA: 0x00016A66 File Offset: 0x00014C66
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x060006FE RID: 1790 RVA: 0x00016A6F File Offset: 0x00014C6F
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x060006FF RID: 1791 RVA: 0x00016A77 File Offset: 0x00014C77
		// (set) Token: 0x06000700 RID: 1792 RVA: 0x00016A7F File Offset: 0x00014C7F
		public TypeReference ParameterType
		{
			get
			{
				return this.parameter_type;
			}
			set
			{
				this.parameter_type = value;
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x00016A88 File Offset: 0x00014C88
		// (set) Token: 0x06000702 RID: 1794 RVA: 0x00016A90 File Offset: 0x00014C90
		public MetadataToken MetadataToken
		{
			get
			{
				return this.token;
			}
			set
			{
				this.token = value;
			}
		}

		// Token: 0x06000703 RID: 1795 RVA: 0x00016A99 File Offset: 0x00014C99
		internal ParameterReference(string name, TypeReference parameterType)
		{
			if (parameterType == null)
			{
				throw new ArgumentNullException("parameterType");
			}
			this.name = name ?? string.Empty;
			this.parameter_type = parameterType;
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x00016A5E File Offset: 0x00014C5E
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x06000705 RID: 1797
		public abstract ParameterDefinition Resolve();

		// Token: 0x04000205 RID: 517
		private string name;

		// Token: 0x04000206 RID: 518
		internal int index = -1;

		// Token: 0x04000207 RID: 519
		protected TypeReference parameter_type;

		// Token: 0x04000208 RID: 520
		internal MetadataToken token;
	}
}
