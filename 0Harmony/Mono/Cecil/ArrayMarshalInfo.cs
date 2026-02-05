using System;

namespace Mono.Cecil
{
	// Token: 0x02000131 RID: 305
	internal sealed class ArrayMarshalInfo : MarshalInfo
	{
		// Token: 0x17000122 RID: 290
		// (get) Token: 0x0600085D RID: 2141 RVA: 0x0002195B File Offset: 0x0001FB5B
		// (set) Token: 0x0600085E RID: 2142 RVA: 0x00021963 File Offset: 0x0001FB63
		public NativeType ElementType
		{
			get
			{
				return this.element_type;
			}
			set
			{
				this.element_type = value;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x0600085F RID: 2143 RVA: 0x0002196C File Offset: 0x0001FB6C
		// (set) Token: 0x06000860 RID: 2144 RVA: 0x00021974 File Offset: 0x0001FB74
		public int SizeParameterIndex
		{
			get
			{
				return this.size_parameter_index;
			}
			set
			{
				this.size_parameter_index = value;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000861 RID: 2145 RVA: 0x0002197D File Offset: 0x0001FB7D
		// (set) Token: 0x06000862 RID: 2146 RVA: 0x00021985 File Offset: 0x0001FB85
		public int Size
		{
			get
			{
				return this.size;
			}
			set
			{
				this.size = value;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000863 RID: 2147 RVA: 0x0002198E File Offset: 0x0001FB8E
		// (set) Token: 0x06000864 RID: 2148 RVA: 0x00021996 File Offset: 0x0001FB96
		public int SizeParameterMultiplier
		{
			get
			{
				return this.size_parameter_multiplier;
			}
			set
			{
				this.size_parameter_multiplier = value;
			}
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x0002199F File Offset: 0x0001FB9F
		public ArrayMarshalInfo()
			: base(NativeType.Array)
		{
			this.element_type = NativeType.None;
			this.size_parameter_index = -1;
			this.size = -1;
			this.size_parameter_multiplier = -1;
		}

		// Token: 0x04000307 RID: 775
		internal NativeType element_type;

		// Token: 0x04000308 RID: 776
		internal int size_parameter_index;

		// Token: 0x04000309 RID: 777
		internal int size;

		// Token: 0x0400030A RID: 778
		internal int size_parameter_multiplier;
	}
}
