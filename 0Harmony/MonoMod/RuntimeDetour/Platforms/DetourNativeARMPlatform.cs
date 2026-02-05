using System;
using System.Runtime.InteropServices;

namespace MonoMod.RuntimeDetour.Platforms
{
	// Token: 0x0200034D RID: 845
	internal class DetourNativeARMPlatform : IDetourNativePlatform
	{
		// Token: 0x060013B6 RID: 5046 RVA: 0x00047830 File Offset: 0x00045A30
		private static DetourNativeARMPlatform.DetourType GetDetourType(IntPtr from, IntPtr to)
		{
			if (IntPtr.Size >= 8)
			{
				return DetourNativeARMPlatform.DetourType.AArch64;
			}
			bool flag = ((long)from & 1L) == 1L;
			bool flag2 = ((long)to & 1L) == 1L;
			if (flag)
			{
				if (flag2)
				{
					return DetourNativeARMPlatform.DetourType.Thumb;
				}
				return DetourNativeARMPlatform.DetourType.ThumbBX;
			}
			else
			{
				if (flag2)
				{
					return DetourNativeARMPlatform.DetourType.AArch32BX;
				}
				return DetourNativeARMPlatform.DetourType.AArch32;
			}
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x00047874 File Offset: 0x00045A74
		public NativeDetourData Create(IntPtr from, IntPtr to, byte? type)
		{
			NativeDetourData nativeDetourData = new NativeDetourData
			{
				Method = (IntPtr)((long)from & -2L),
				Target = (IntPtr)((long)to & -2L)
			};
			nativeDetourData.Size = DetourNativeARMPlatform.DetourSizes[(int)(nativeDetourData.Type = type ?? ((byte)DetourNativeARMPlatform.GetDetourType(from, to)))];
			return nativeDetourData;
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x00010C51 File Offset: 0x0000EE51
		public void Free(NativeDetourData detour)
		{
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x000478EC File Offset: 0x00045AEC
		public void Apply(NativeDetourData detour)
		{
			int num = 0;
			switch (detour.Type)
			{
			case 0:
				detour.Method.Write(ref num, 223);
				detour.Method.Write(ref num, 248);
				detour.Method.Write(ref num, 0);
				detour.Method.Write(ref num, 240);
				detour.Method.Write(ref num, (uint)((int)detour.Target | 1));
				return;
			case 1:
				detour.Method.Write(ref num, 223);
				detour.Method.Write(ref num, 248);
				detour.Method.Write(ref num, 4);
				detour.Method.Write(ref num, 160);
				detour.Method.Write(ref num, 80);
				detour.Method.Write(ref num, 71);
				detour.Method.Write(ref num, 0);
				detour.Method.Write(ref num, 191);
				detour.Method.Write(ref num, (uint)((int)detour.Target | 0));
				return;
			case 2:
				detour.Method.Write(ref num, 4);
				detour.Method.Write(ref num, 240);
				detour.Method.Write(ref num, 31);
				detour.Method.Write(ref num, 229);
				detour.Method.Write(ref num, (uint)((int)detour.Target | 0));
				return;
			case 3:
				detour.Method.Write(ref num, 0);
				detour.Method.Write(ref num, 128);
				detour.Method.Write(ref num, 159);
				detour.Method.Write(ref num, 229);
				detour.Method.Write(ref num, 24);
				detour.Method.Write(ref num, byte.MaxValue);
				detour.Method.Write(ref num, 47);
				detour.Method.Write(ref num, 225);
				detour.Method.Write(ref num, (uint)((int)detour.Target | 1));
				return;
			case 4:
				detour.Method.Write(ref num, 79);
				detour.Method.Write(ref num, 0);
				detour.Method.Write(ref num, 0);
				detour.Method.Write(ref num, 88);
				detour.Method.Write(ref num, 224);
				detour.Method.Write(ref num, 1);
				detour.Method.Write(ref num, 31);
				detour.Method.Write(ref num, 214);
				detour.Method.Write(ref num, (ulong)(long)detour.Target);
				return;
			default:
				throw new NotSupportedException(string.Format("Unknown detour type {0}", detour.Type));
			}
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x00047BC8 File Offset: 0x00045DC8
		public unsafe void Copy(IntPtr src, IntPtr dst, byte type)
		{
			switch (type)
			{
			case 0:
				*(UIntPtr)(long)dst = (int)(*(UIntPtr)(long)src);
				*(UIntPtr)((long)dst + 4L) = (int)(*(UIntPtr)((long)src + 4L));
				return;
			case 1:
				*(UIntPtr)(long)dst = (int)(*(UIntPtr)(long)src);
				*(UIntPtr)((long)dst + 4L) = (short)(*(UIntPtr)((long)src + 4L));
				*(UIntPtr)((long)dst + 6L) = (short)(*(UIntPtr)((long)src + 6L));
				*(UIntPtr)((long)dst + 8L) = (int)(*(UIntPtr)((long)src + 8L));
				return;
			case 2:
				*(UIntPtr)(long)dst = (int)(*(UIntPtr)(long)src);
				*(UIntPtr)((long)dst + 4L) = (int)(*(UIntPtr)((long)src + 4L));
				return;
			case 3:
				*(UIntPtr)(long)dst = (int)(*(UIntPtr)(long)src);
				*(UIntPtr)((long)dst + 4L) = (int)(*(UIntPtr)((long)src + 4L));
				*(UIntPtr)((long)dst + 8L) = (int)(*(UIntPtr)((long)src + 8L));
				return;
			case 4:
				*(UIntPtr)(long)dst = (int)(*(UIntPtr)(long)src);
				*(UIntPtr)((long)dst + 4L) = (int)(*(UIntPtr)((long)src + 4L));
				*(UIntPtr)((long)dst + 8L) = *(UIntPtr)((long)src + 8L);
				return;
			default:
				throw new NotSupportedException(string.Format("Unknown detour type {0}", type));
			}
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x00010C51 File Offset: 0x0000EE51
		public void MakeWritable(IntPtr src, uint size)
		{
		}

		// Token: 0x060013BC RID: 5052 RVA: 0x00010C51 File Offset: 0x0000EE51
		public void MakeExecutable(IntPtr src, uint size)
		{
		}

		// Token: 0x060013BD RID: 5053 RVA: 0x00047D28 File Offset: 0x00045F28
		public unsafe void FlushICache(IntPtr src, uint size)
		{
			if (!this.ShouldFlushICache)
			{
				return;
			}
			byte[] array = ((IntPtr.Size >= 8) ? this._FlushCache64 : this._FlushCache32);
			byte[] array2;
			byte* ptr;
			if ((array2 = array) == null || array2.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array2[0];
			}
			DetourHelper.Native.MakeExecutable((IntPtr)((void*)ptr), (uint)array.Length);
			(Marshal.GetDelegateForFunctionPointer((IntPtr)((void*)ptr), typeof(DetourNativeARMPlatform.d_flushicache)) as DetourNativeARMPlatform.d_flushicache)(src, (ulong)size);
			array2 = null;
		}

		// Token: 0x060013BE RID: 5054 RVA: 0x00047DA5 File Offset: 0x00045FA5
		public IntPtr MemAlloc(uint size)
		{
			return Marshal.AllocHGlobal((int)size);
		}

		// Token: 0x060013BF RID: 5055 RVA: 0x00047DAD File Offset: 0x00045FAD
		public void MemFree(IntPtr ptr)
		{
			Marshal.FreeHGlobal(ptr);
		}

		// Token: 0x04000FED RID: 4077
		private static readonly uint[] DetourSizes = new uint[] { 8U, 12U, 8U, 12U, 16U };

		// Token: 0x04000FEE RID: 4078
		public bool ShouldFlushICache = true;

		// Token: 0x04000FEF RID: 4079
		private readonly byte[] _FlushCache32 = new byte[]
		{
			128, 64, 45, 233, 0, 48, 160, 225, 1, 192,
			128, 224, 20, 224, 159, 229, 3, 0, 160, 225,
			12, 16, 160, 225, 14, 112, 160, 225, 0, 32,
			160, 227, 0, 0, 0, 239, 128, 128, 189, 232,
			2, 0, 15, 0
		};

		// Token: 0x04000FF0 RID: 4080
		private readonly byte[] _FlushCache64 = new byte[]
		{
			1, 0, 1, 139, 0, 244, 126, 146, 63, 0,
			0, 235, 201, 0, 0, 84, 226, 3, 0, 170,
			34, 126, 11, 213, 66, 16, 0, 145, 63, 0,
			2, 235, 168, byte.MaxValue, byte.MaxValue, 84, 159, 59, 3, 213,
			63, 0, 0, 235, 169, 0, 0, 84, 32, 117,
			11, 213, 0, 16, 0, 145, 63, 0, 0, 235,
			168, byte.MaxValue, byte.MaxValue, 84, 159, 59, 3, 213, 223, 63,
			3, 213, 192, 3, 95, 214
		};

		// Token: 0x0200034E RID: 846
		public enum DetourType : byte
		{
			// Token: 0x04000FF2 RID: 4082
			Thumb,
			// Token: 0x04000FF3 RID: 4083
			ThumbBX,
			// Token: 0x04000FF4 RID: 4084
			AArch32,
			// Token: 0x04000FF5 RID: 4085
			AArch32BX,
			// Token: 0x04000FF6 RID: 4086
			AArch64
		}

		// Token: 0x0200034F RID: 847
		// (Invoke) Token: 0x060013C3 RID: 5059
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		private delegate int d_flushicache(IntPtr code, ulong size);
	}
}
