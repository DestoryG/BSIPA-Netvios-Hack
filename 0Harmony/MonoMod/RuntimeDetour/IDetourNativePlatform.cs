using System;

namespace MonoMod.RuntimeDetour
{
	// Token: 0x0200034A RID: 842
	internal interface IDetourNativePlatform
	{
		// Token: 0x060013A7 RID: 5031
		NativeDetourData Create(IntPtr from, IntPtr to, byte? type = null);

		// Token: 0x060013A8 RID: 5032
		void Free(NativeDetourData detour);

		// Token: 0x060013A9 RID: 5033
		void Apply(NativeDetourData detour);

		// Token: 0x060013AA RID: 5034
		void Copy(IntPtr src, IntPtr dst, byte type);

		// Token: 0x060013AB RID: 5035
		void MakeWritable(IntPtr src, uint size);

		// Token: 0x060013AC RID: 5036
		void MakeExecutable(IntPtr src, uint size);

		// Token: 0x060013AD RID: 5037
		void FlushICache(IntPtr src, uint size);

		// Token: 0x060013AE RID: 5038
		IntPtr MemAlloc(uint size);

		// Token: 0x060013AF RID: 5039
		void MemFree(IntPtr ptr);
	}
}
