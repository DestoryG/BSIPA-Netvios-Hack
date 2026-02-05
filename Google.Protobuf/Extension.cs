using System;

namespace Google.Protobuf
{
	// Token: 0x02000006 RID: 6
	public abstract class Extension
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000B2 RID: 178
		internal abstract Type TargetType { get; }

		// Token: 0x060000B3 RID: 179 RVA: 0x00004637 File Offset: 0x00002837
		protected Extension(int fieldNumber)
		{
			this.FieldNumber = fieldNumber;
		}

		// Token: 0x060000B4 RID: 180
		internal abstract IExtensionValue CreateValue();

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00004646 File Offset: 0x00002846
		public int FieldNumber { get; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000B6 RID: 182
		internal abstract bool IsRepeated { get; }
	}
}
