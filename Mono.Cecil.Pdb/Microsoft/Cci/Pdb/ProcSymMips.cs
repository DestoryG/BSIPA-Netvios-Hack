using System;

namespace Microsoft.Cci.Pdb
{
	// Token: 0x020000AE RID: 174
	internal struct ProcSymMips
	{
		// Token: 0x040003CC RID: 972
		internal uint parent;

		// Token: 0x040003CD RID: 973
		internal uint end;

		// Token: 0x040003CE RID: 974
		internal uint next;

		// Token: 0x040003CF RID: 975
		internal uint len;

		// Token: 0x040003D0 RID: 976
		internal uint dbgStart;

		// Token: 0x040003D1 RID: 977
		internal uint dbgEnd;

		// Token: 0x040003D2 RID: 978
		internal uint regSave;

		// Token: 0x040003D3 RID: 979
		internal uint fpSave;

		// Token: 0x040003D4 RID: 980
		internal uint intOff;

		// Token: 0x040003D5 RID: 981
		internal uint fpOff;

		// Token: 0x040003D6 RID: 982
		internal uint typind;

		// Token: 0x040003D7 RID: 983
		internal uint off;

		// Token: 0x040003D8 RID: 984
		internal ushort seg;

		// Token: 0x040003D9 RID: 985
		internal byte retReg;

		// Token: 0x040003DA RID: 986
		internal byte frameReg;

		// Token: 0x040003DB RID: 987
		internal string name;
	}
}
