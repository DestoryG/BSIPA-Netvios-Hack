using System;

namespace Mono.Cecil
{
	// Token: 0x02000134 RID: 308
	internal sealed class FixedArrayMarshalInfo : MarshalInfo
	{
		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000872 RID: 2162 RVA: 0x00021A36 File Offset: 0x0001FC36
		// (set) Token: 0x06000873 RID: 2163 RVA: 0x00021A3E File Offset: 0x0001FC3E
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

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000874 RID: 2164 RVA: 0x00021A47 File Offset: 0x0001FC47
		// (set) Token: 0x06000875 RID: 2165 RVA: 0x00021A4F File Offset: 0x0001FC4F
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

		// Token: 0x06000876 RID: 2166 RVA: 0x00021A58 File Offset: 0x0001FC58
		public FixedArrayMarshalInfo()
			: base(NativeType.FixedArray)
		{
			this.element_type = NativeType.None;
		}

		// Token: 0x04000310 RID: 784
		internal NativeType element_type;

		// Token: 0x04000311 RID: 785
		internal int size;
	}
}
