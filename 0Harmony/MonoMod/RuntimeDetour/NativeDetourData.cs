using System;

namespace MonoMod.RuntimeDetour
{
	// Token: 0x0200034C RID: 844
	internal struct NativeDetourData
	{
		// Token: 0x04000FE8 RID: 4072
		public IntPtr Method;

		// Token: 0x04000FE9 RID: 4073
		public IntPtr Target;

		// Token: 0x04000FEA RID: 4074
		public byte Type;

		// Token: 0x04000FEB RID: 4075
		public uint Size;

		// Token: 0x04000FEC RID: 4076
		public IntPtr Extra;
	}
}
