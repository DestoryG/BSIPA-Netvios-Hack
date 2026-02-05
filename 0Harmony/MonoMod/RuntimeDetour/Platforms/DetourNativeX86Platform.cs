using System;
using System.Runtime.InteropServices;

namespace MonoMod.RuntimeDetour.Platforms
{
	// Token: 0x0200035A RID: 858
	internal class DetourNativeX86Platform : IDetourNativePlatform
	{
		// Token: 0x060013FC RID: 5116 RVA: 0x00048209 File Offset: 0x00046409
		private static bool Is32Bit(long to)
		{
			return (to & 2147483647L) == to;
		}

		// Token: 0x060013FD RID: 5117 RVA: 0x00048218 File Offset: 0x00046418
		private static DetourNativeX86Platform.DetourType GetDetourType(IntPtr from, IntPtr to)
		{
			long num = (long)to - ((long)from + 5L);
			if (DetourNativeX86Platform.Is32Bit(num) || DetourNativeX86Platform.Is32Bit(-num))
			{
				return DetourNativeX86Platform.DetourType.Rel32;
			}
			if (DetourNativeX86Platform.Is32Bit((long)to))
			{
				return DetourNativeX86Platform.DetourType.Abs32;
			}
			return DetourNativeX86Platform.DetourType.Abs64;
		}

		// Token: 0x060013FE RID: 5118 RVA: 0x0004825C File Offset: 0x0004645C
		public NativeDetourData Create(IntPtr from, IntPtr to, byte? type)
		{
			NativeDetourData nativeDetourData = new NativeDetourData
			{
				Method = from,
				Target = to
			};
			nativeDetourData.Size = DetourNativeX86Platform.DetourSizes[(int)(nativeDetourData.Type = type ?? ((byte)DetourNativeX86Platform.GetDetourType(from, to)))];
			return nativeDetourData;
		}

		// Token: 0x060013FF RID: 5119 RVA: 0x00010C51 File Offset: 0x0000EE51
		public void Free(NativeDetourData detour)
		{
		}

		// Token: 0x06001400 RID: 5120 RVA: 0x000482B8 File Offset: 0x000464B8
		public void Apply(NativeDetourData detour)
		{
			int num = 0;
			switch (detour.Type)
			{
			case 0:
				detour.Method.Write(ref num, 233);
				detour.Method.Write(ref num, (uint)((int)((long)detour.Target - ((long)detour.Method + (long)num + 4L))));
				return;
			case 1:
				detour.Method.Write(ref num, 104);
				detour.Method.Write(ref num, (uint)(int)detour.Target);
				detour.Method.Write(ref num, 195);
				return;
			case 2:
				detour.Method.Write(ref num, byte.MaxValue);
				detour.Method.Write(ref num, 37);
				detour.Method.Write(ref num, 0U);
				detour.Method.Write(ref num, (ulong)(long)detour.Target);
				return;
			default:
				throw new NotSupportedException(string.Format("Unknown detour type {0}", detour.Type));
			}
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x000483C0 File Offset: 0x000465C0
		public unsafe void Copy(IntPtr src, IntPtr dst, byte type)
		{
			switch (type)
			{
			case 0:
				*(UIntPtr)(long)dst = (int)(*(UIntPtr)(long)src);
				*(UIntPtr)((long)dst + 4L) = *(UIntPtr)((long)src + 4L);
				return;
			case 1:
				*(UIntPtr)(long)dst = (int)(*(UIntPtr)(long)src);
				*(UIntPtr)((long)dst + 4L) = (short)(*(UIntPtr)((long)src + 4L));
				return;
			case 2:
				*(UIntPtr)(long)dst = *(UIntPtr)(long)src;
				*(UIntPtr)((long)dst + 8L) = (int)(*(UIntPtr)((long)src + 8L));
				*(UIntPtr)((long)dst + 12L) = (short)(*(UIntPtr)((long)src + 12L));
				return;
			default:
				throw new NotSupportedException(string.Format("Unknown detour type {0}", type));
			}
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x00010C51 File Offset: 0x0000EE51
		public void MakeWritable(IntPtr src, uint size)
		{
		}

		// Token: 0x06001403 RID: 5123 RVA: 0x00010C51 File Offset: 0x0000EE51
		public void MakeExecutable(IntPtr src, uint size)
		{
		}

		// Token: 0x06001404 RID: 5124 RVA: 0x00010C51 File Offset: 0x0000EE51
		public void FlushICache(IntPtr src, uint size)
		{
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x00047DA5 File Offset: 0x00045FA5
		public IntPtr MemAlloc(uint size)
		{
			return Marshal.AllocHGlobal((int)size);
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x00047DAD File Offset: 0x00045FAD
		public void MemFree(IntPtr ptr)
		{
			Marshal.FreeHGlobal(ptr);
		}

		// Token: 0x0400117E RID: 4478
		private static readonly uint[] DetourSizes = new uint[] { 5U, 6U, 14U };

		// Token: 0x0200035B RID: 859
		public enum DetourType : byte
		{
			// Token: 0x04001180 RID: 4480
			Rel32,
			// Token: 0x04001181 RID: 4481
			Abs32,
			// Token: 0x04001182 RID: 4482
			Abs64
		}
	}
}
