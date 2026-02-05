using System;

namespace Mono.Cecil
{
	// Token: 0x0200015B RID: 347
	internal abstract class ParameterReference : IMetadataTokenProvider
	{
		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000AB3 RID: 2739 RVA: 0x0002562E File Offset: 0x0002382E
		// (set) Token: 0x06000AB4 RID: 2740 RVA: 0x00025636 File Offset: 0x00023836
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

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000AB5 RID: 2741 RVA: 0x0002563F File Offset: 0x0002383F
		public int Index
		{
			get
			{
				return this.index;
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000AB6 RID: 2742 RVA: 0x00025647 File Offset: 0x00023847
		// (set) Token: 0x06000AB7 RID: 2743 RVA: 0x0002564F File Offset: 0x0002384F
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

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000AB8 RID: 2744 RVA: 0x00025658 File Offset: 0x00023858
		// (set) Token: 0x06000AB9 RID: 2745 RVA: 0x00025660 File Offset: 0x00023860
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

		// Token: 0x06000ABA RID: 2746 RVA: 0x00025669 File Offset: 0x00023869
		internal ParameterReference(string name, TypeReference parameterType)
		{
			if (parameterType == null)
			{
				throw new ArgumentNullException("parameterType");
			}
			this.name = name ?? string.Empty;
			this.parameter_type = parameterType;
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x0002562E File Offset: 0x0002382E
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x06000ABC RID: 2748
		public abstract ParameterDefinition Resolve();

		// Token: 0x0400043D RID: 1085
		private string name;

		// Token: 0x0400043E RID: 1086
		internal int index = -1;

		// Token: 0x0400043F RID: 1087
		protected TypeReference parameter_type;

		// Token: 0x04000440 RID: 1088
		internal MetadataToken token;
	}
}
