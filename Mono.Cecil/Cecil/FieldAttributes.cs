using System;

namespace Mono.Cecil
{
	// Token: 0x0200005A RID: 90
	[Flags]
	public enum FieldAttributes : ushort
	{
		// Token: 0x040000AB RID: 171
		FieldAccessMask = 7,
		// Token: 0x040000AC RID: 172
		CompilerControlled = 0,
		// Token: 0x040000AD RID: 173
		Private = 1,
		// Token: 0x040000AE RID: 174
		FamANDAssem = 2,
		// Token: 0x040000AF RID: 175
		Assembly = 3,
		// Token: 0x040000B0 RID: 176
		Family = 4,
		// Token: 0x040000B1 RID: 177
		FamORAssem = 5,
		// Token: 0x040000B2 RID: 178
		Public = 6,
		// Token: 0x040000B3 RID: 179
		Static = 16,
		// Token: 0x040000B4 RID: 180
		InitOnly = 32,
		// Token: 0x040000B5 RID: 181
		Literal = 64,
		// Token: 0x040000B6 RID: 182
		NotSerialized = 128,
		// Token: 0x040000B7 RID: 183
		SpecialName = 512,
		// Token: 0x040000B8 RID: 184
		PInvokeImpl = 8192,
		// Token: 0x040000B9 RID: 185
		RTSpecialName = 1024,
		// Token: 0x040000BA RID: 186
		HasFieldMarshal = 4096,
		// Token: 0x040000BB RID: 187
		HasDefault = 32768,
		// Token: 0x040000BC RID: 188
		HasFieldRVA = 256
	}
}
