using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MonoMod.RuntimeDetour.Platforms
{
	// Token: 0x02000350 RID: 848
	internal class DetourNativeLibcPlatform : IDetourNativePlatform
	{
		// Token: 0x060013C6 RID: 5062 RVA: 0x00047E0C File Offset: 0x0004600C
		public DetourNativeLibcPlatform(IDetourNativePlatform inner)
		{
			this.Inner = inner;
			PropertyInfo property = typeof(Environment).GetProperty("SystemPageSize");
			if (property == null)
			{
				throw new NotSupportedException("Unsupported runtime");
			}
			this._Pagesize = (long)((int)property.GetValue(null, new object[0]));
		}

		// Token: 0x060013C7 RID: 5063 RVA: 0x00047E68 File Offset: 0x00046068
		private void SetMemPerms(IntPtr start, ulong len, DetourNativeLibcPlatform.MmapProts prot)
		{
			long pagesize = this._Pagesize;
			long num = (long)start & ~(pagesize - 1L);
			long num2 = ((long)start + (long)len + pagesize - 1L) & ~(pagesize - 1L);
			if (DetourNativeLibcPlatform.mprotect((IntPtr)num, (IntPtr)(num2 - num), prot) != 0)
			{
				throw new Win32Exception();
			}
		}

		// Token: 0x060013C8 RID: 5064 RVA: 0x00047EB9 File Offset: 0x000460B9
		public void MakeWritable(IntPtr src, uint size)
		{
			this.SetMemPerms(src, (ulong)size, DetourNativeLibcPlatform.MmapProts.PROT_READ | DetourNativeLibcPlatform.MmapProts.PROT_WRITE | DetourNativeLibcPlatform.MmapProts.PROT_EXEC);
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x00047EB9 File Offset: 0x000460B9
		public void MakeExecutable(IntPtr src, uint size)
		{
			this.SetMemPerms(src, (ulong)size, DetourNativeLibcPlatform.MmapProts.PROT_READ | DetourNativeLibcPlatform.MmapProts.PROT_WRITE | DetourNativeLibcPlatform.MmapProts.PROT_EXEC);
		}

		// Token: 0x060013CA RID: 5066 RVA: 0x00047EC5 File Offset: 0x000460C5
		public void FlushICache(IntPtr src, uint size)
		{
			this.Inner.FlushICache(src, size);
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x00047ED4 File Offset: 0x000460D4
		public NativeDetourData Create(IntPtr from, IntPtr to, byte? type)
		{
			return this.Inner.Create(from, to, type);
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x00047EE4 File Offset: 0x000460E4
		public void Free(NativeDetourData detour)
		{
			this.Inner.Free(detour);
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x00047EF2 File Offset: 0x000460F2
		public void Apply(NativeDetourData detour)
		{
			this.Inner.Apply(detour);
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x00047F00 File Offset: 0x00046100
		public void Copy(IntPtr src, IntPtr dst, byte type)
		{
			this.Inner.Copy(src, dst, type);
		}

		// Token: 0x060013CF RID: 5071 RVA: 0x00047F10 File Offset: 0x00046110
		public IntPtr MemAlloc(uint size)
		{
			return this.Inner.MemAlloc(size);
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x00047F1E File Offset: 0x0004611E
		public void MemFree(IntPtr ptr)
		{
			this.Inner.MemFree(ptr);
		}

		// Token: 0x060013D1 RID: 5073
		[DllImport("libc", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		private static extern int mprotect(IntPtr start, IntPtr len, DetourNativeLibcPlatform.MmapProts prot);

		// Token: 0x04000FF7 RID: 4087
		private readonly IDetourNativePlatform Inner;

		// Token: 0x04000FF8 RID: 4088
		private readonly long _Pagesize;

		// Token: 0x02000351 RID: 849
		[Flags]
		private enum MmapProts
		{
			// Token: 0x04000FFA RID: 4090
			PROT_READ = 1,
			// Token: 0x04000FFB RID: 4091
			PROT_WRITE = 2,
			// Token: 0x04000FFC RID: 4092
			PROT_EXEC = 4,
			// Token: 0x04000FFD RID: 4093
			PROT_NONE = 0,
			// Token: 0x04000FFE RID: 4094
			PROT_GROWSDOWN = 16777216,
			// Token: 0x04000FFF RID: 4095
			PROT_GROWSUP = 33554432
		}
	}
}
