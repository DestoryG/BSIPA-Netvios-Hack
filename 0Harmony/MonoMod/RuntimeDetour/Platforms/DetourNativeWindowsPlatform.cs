using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace MonoMod.RuntimeDetour.Platforms
{
	// Token: 0x02000358 RID: 856
	internal class DetourNativeWindowsPlatform : IDetourNativePlatform
	{
		// Token: 0x060013EF RID: 5103 RVA: 0x00048139 File Offset: 0x00046339
		public DetourNativeWindowsPlatform(IDetourNativePlatform inner)
		{
			this.Inner = inner;
		}

		// Token: 0x060013F0 RID: 5104 RVA: 0x00048148 File Offset: 0x00046348
		public void MakeWritable(IntPtr src, uint size)
		{
			DetourNativeWindowsPlatform.Protection protection;
			if (!DetourNativeWindowsPlatform.VirtualProtect(src, (IntPtr)((long)((ulong)size)), DetourNativeWindowsPlatform.Protection.PAGE_EXECUTE_READWRITE, out protection))
			{
				throw new Win32Exception();
			}
		}

		// Token: 0x060013F1 RID: 5105 RVA: 0x00048170 File Offset: 0x00046370
		public void MakeExecutable(IntPtr src, uint size)
		{
			DetourNativeWindowsPlatform.Protection protection;
			if (!DetourNativeWindowsPlatform.VirtualProtect(src, (IntPtr)((long)((ulong)size)), DetourNativeWindowsPlatform.Protection.PAGE_EXECUTE_READWRITE, out protection))
			{
				throw new Win32Exception();
			}
		}

		// Token: 0x060013F2 RID: 5106 RVA: 0x00048196 File Offset: 0x00046396
		public void FlushICache(IntPtr src, uint size)
		{
			if (!DetourNativeWindowsPlatform.FlushInstructionCache(DetourNativeWindowsPlatform.GetCurrentProcess(), src, (UIntPtr)size))
			{
				throw new Win32Exception();
			}
		}

		// Token: 0x060013F3 RID: 5107 RVA: 0x000481B1 File Offset: 0x000463B1
		public NativeDetourData Create(IntPtr from, IntPtr to, byte? type)
		{
			return this.Inner.Create(from, to, type);
		}

		// Token: 0x060013F4 RID: 5108 RVA: 0x000481C1 File Offset: 0x000463C1
		public void Free(NativeDetourData detour)
		{
			this.Inner.Free(detour);
		}

		// Token: 0x060013F5 RID: 5109 RVA: 0x000481CF File Offset: 0x000463CF
		public void Apply(NativeDetourData detour)
		{
			this.Inner.Apply(detour);
		}

		// Token: 0x060013F6 RID: 5110 RVA: 0x000481DD File Offset: 0x000463DD
		public void Copy(IntPtr src, IntPtr dst, byte type)
		{
			this.Inner.Copy(src, dst, type);
		}

		// Token: 0x060013F7 RID: 5111 RVA: 0x000481ED File Offset: 0x000463ED
		public IntPtr MemAlloc(uint size)
		{
			return this.Inner.MemAlloc(size);
		}

		// Token: 0x060013F8 RID: 5112 RVA: 0x000481FB File Offset: 0x000463FB
		public void MemFree(IntPtr ptr)
		{
			this.Inner.MemFree(ptr);
		}

		// Token: 0x060013F9 RID: 5113
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool VirtualProtect(IntPtr lpAddress, IntPtr dwSize, DetourNativeWindowsPlatform.Protection flNewProtect, out DetourNativeWindowsPlatform.Protection lpflOldProtect);

		// Token: 0x060013FA RID: 5114
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr GetCurrentProcess();

		// Token: 0x060013FB RID: 5115
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool FlushInstructionCache(IntPtr hProcess, IntPtr lpBaseAddress, UIntPtr dwSize);

		// Token: 0x04001171 RID: 4465
		private readonly IDetourNativePlatform Inner;

		// Token: 0x02000359 RID: 857
		[Flags]
		private enum Protection : uint
		{
			// Token: 0x04001173 RID: 4467
			PAGE_NOACCESS = 1U,
			// Token: 0x04001174 RID: 4468
			PAGE_READONLY = 2U,
			// Token: 0x04001175 RID: 4469
			PAGE_READWRITE = 4U,
			// Token: 0x04001176 RID: 4470
			PAGE_WRITECOPY = 8U,
			// Token: 0x04001177 RID: 4471
			PAGE_EXECUTE = 16U,
			// Token: 0x04001178 RID: 4472
			PAGE_EXECUTE_READ = 32U,
			// Token: 0x04001179 RID: 4473
			PAGE_EXECUTE_READWRITE = 64U,
			// Token: 0x0400117A RID: 4474
			PAGE_EXECUTE_WRITECOPY = 128U,
			// Token: 0x0400117B RID: 4475
			PAGE_GUARD = 256U,
			// Token: 0x0400117C RID: 4476
			PAGE_NOCACHE = 512U,
			// Token: 0x0400117D RID: 4477
			PAGE_WRITECOMBINE = 1024U
		}
	}
}
