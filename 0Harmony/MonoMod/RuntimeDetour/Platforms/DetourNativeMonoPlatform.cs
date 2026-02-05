using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace MonoMod.RuntimeDetour.Platforms
{
	// Token: 0x02000352 RID: 850
	internal class DetourNativeMonoPlatform : IDetourNativePlatform
	{
		// Token: 0x060013D2 RID: 5074 RVA: 0x00047F2C File Offset: 0x0004612C
		public DetourNativeMonoPlatform(IDetourNativePlatform inner, string libmono)
		{
			this.Inner = inner;
			this._Pagesize = (long)DetourNativeMonoPlatform.mono_pagesize();
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x00047F48 File Offset: 0x00046148
		private void SetMemPerms(IntPtr start, ulong len, DetourNativeMonoPlatform.MmapProts prot)
		{
			long pagesize = this._Pagesize;
			long num = (long)start & ~(pagesize - 1L);
			long num2 = ((long)start + (long)len + pagesize - 1L) & ~(pagesize - 1L);
			if (DetourNativeMonoPlatform.mono_mprotect((IntPtr)num, (IntPtr)(num2 - num), (int)prot) != 0 && Marshal.GetLastWin32Error() != 0)
			{
				throw new Win32Exception();
			}
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x00047FA0 File Offset: 0x000461A0
		public void MakeWritable(IntPtr src, uint size)
		{
			this.SetMemPerms(src, (ulong)size, DetourNativeMonoPlatform.MmapProts.PROT_READ | DetourNativeMonoPlatform.MmapProts.PROT_WRITE | DetourNativeMonoPlatform.MmapProts.PROT_EXEC);
		}

		// Token: 0x060013D5 RID: 5077 RVA: 0x00047FA0 File Offset: 0x000461A0
		public void MakeExecutable(IntPtr src, uint size)
		{
			this.SetMemPerms(src, (ulong)size, DetourNativeMonoPlatform.MmapProts.PROT_READ | DetourNativeMonoPlatform.MmapProts.PROT_WRITE | DetourNativeMonoPlatform.MmapProts.PROT_EXEC);
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x00047FAC File Offset: 0x000461AC
		public void FlushICache(IntPtr src, uint size)
		{
			this.Inner.FlushICache(src, size);
		}

		// Token: 0x060013D7 RID: 5079 RVA: 0x00047FBB File Offset: 0x000461BB
		public NativeDetourData Create(IntPtr from, IntPtr to, byte? type)
		{
			return this.Inner.Create(from, to, type);
		}

		// Token: 0x060013D8 RID: 5080 RVA: 0x00047FCB File Offset: 0x000461CB
		public void Free(NativeDetourData detour)
		{
			this.Inner.Free(detour);
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x00047FD9 File Offset: 0x000461D9
		public void Apply(NativeDetourData detour)
		{
			this.Inner.Apply(detour);
		}

		// Token: 0x060013DA RID: 5082 RVA: 0x00047FE7 File Offset: 0x000461E7
		public void Copy(IntPtr src, IntPtr dst, byte type)
		{
			this.Inner.Copy(src, dst, type);
		}

		// Token: 0x060013DB RID: 5083 RVA: 0x00047FF7 File Offset: 0x000461F7
		public IntPtr MemAlloc(uint size)
		{
			return this.Inner.MemAlloc(size);
		}

		// Token: 0x060013DC RID: 5084 RVA: 0x00048005 File Offset: 0x00046205
		public void MemFree(IntPtr ptr)
		{
			this.Inner.MemFree(ptr);
		}

		// Token: 0x060013DD RID: 5085
		[DllImport("libmonosgen-2.0", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int mono_pagesize();

		// Token: 0x060013DE RID: 5086
		[DllImport("libmonosgen-2.0", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int mono_mprotect(IntPtr addr, IntPtr length, int flags);

		// Token: 0x04001000 RID: 4096
		private readonly IDetourNativePlatform Inner;

		// Token: 0x04001001 RID: 4097
		private readonly long _Pagesize;

		// Token: 0x02000353 RID: 851
		[Flags]
		private enum MmapProts
		{
			// Token: 0x04001003 RID: 4099
			PROT_READ = 1,
			// Token: 0x04001004 RID: 4100
			PROT_WRITE = 2,
			// Token: 0x04001005 RID: 4101
			PROT_EXEC = 4,
			// Token: 0x04001006 RID: 4102
			PROT_NONE = 0,
			// Token: 0x04001007 RID: 4103
			PROT_GROWSDOWN = 16777216,
			// Token: 0x04001008 RID: 4104
			PROT_GROWSUP = 33554432
		}
	}
}
