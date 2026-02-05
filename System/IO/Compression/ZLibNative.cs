using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace System.IO.Compression
{
	// Token: 0x02000424 RID: 1060
	internal static class ZLibNative
	{
		// Token: 0x060027A0 RID: 10144 RVA: 0x000B6648 File Offset: 0x000B4848
		[SecurityCritical]
		public static ZLibNative.ErrorCode CreateZLibStreamForDeflate(out ZLibNative.ZLibStreamHandle zLibStreamHandle)
		{
			return ZLibNative.CreateZLibStreamForDeflate(out zLibStreamHandle, ZLibNative.CompressionLevel.DefaultCompression, -15, 8, ZLibNative.CompressionStrategy.DefaultStrategy);
		}

		// Token: 0x060027A1 RID: 10145 RVA: 0x000B6655 File Offset: 0x000B4855
		[SecurityCritical]
		public static ZLibNative.ErrorCode CreateZLibStreamForDeflate(out ZLibNative.ZLibStreamHandle zLibStreamHandle, ZLibNative.CompressionLevel level, int windowBits, int memLevel, ZLibNative.CompressionStrategy strategy)
		{
			zLibStreamHandle = new ZLibNative.ZLibStreamHandle();
			return zLibStreamHandle.DeflateInit2_(level, windowBits, memLevel, strategy);
		}

		// Token: 0x060027A2 RID: 10146 RVA: 0x000B666A File Offset: 0x000B486A
		[SecurityCritical]
		public static ZLibNative.ErrorCode CreateZLibStreamForInflate(out ZLibNative.ZLibStreamHandle zLibStreamHandle)
		{
			return ZLibNative.CreateZLibStreamForInflate(out zLibStreamHandle, -15);
		}

		// Token: 0x060027A3 RID: 10147 RVA: 0x000B6674 File Offset: 0x000B4874
		[SecurityCritical]
		public static ZLibNative.ErrorCode CreateZLibStreamForInflate(out ZLibNative.ZLibStreamHandle zLibStreamHandle, int windowBits)
		{
			zLibStreamHandle = new ZLibNative.ZLibStreamHandle();
			return zLibStreamHandle.InflateInit2_(windowBits);
		}

		// Token: 0x060027A4 RID: 10148 RVA: 0x000B6685 File Offset: 0x000B4885
		[SecurityCritical]
		public static int ZLibCompileFlags()
		{
			return ZLibNative.ZLibStreamHandle.ZLibCompileFlags();
		}

		// Token: 0x0400217D RID: 8573
		public const string ZLibNativeDllName = "clrcompression.dll";

		// Token: 0x0400217E RID: 8574
		private const string Kernel32DllName = "kernel32.dll";

		// Token: 0x0400217F RID: 8575
		public const string ZLibVersion = "1.3.1";

		// Token: 0x04002180 RID: 8576
		internal static readonly IntPtr ZNullPtr = (IntPtr)0;

		// Token: 0x04002181 RID: 8577
		public const int Deflate_DefaultWindowBits = -15;

		// Token: 0x04002182 RID: 8578
		public const int Deflate_DefaultMemLevel = 8;

		// Token: 0x02000818 RID: 2072
		public enum FlushCode
		{
			// Token: 0x04003596 RID: 13718
			NoFlush,
			// Token: 0x04003597 RID: 13719
			PartialFlush,
			// Token: 0x04003598 RID: 13720
			SyncFlush,
			// Token: 0x04003599 RID: 13721
			FullFlush,
			// Token: 0x0400359A RID: 13722
			Finish,
			// Token: 0x0400359B RID: 13723
			Block
		}

		// Token: 0x02000819 RID: 2073
		public enum ErrorCode
		{
			// Token: 0x0400359D RID: 13725
			Ok,
			// Token: 0x0400359E RID: 13726
			StreamEnd,
			// Token: 0x0400359F RID: 13727
			NeedDictionary,
			// Token: 0x040035A0 RID: 13728
			ErrorNo = -1,
			// Token: 0x040035A1 RID: 13729
			StreamError = -2,
			// Token: 0x040035A2 RID: 13730
			DataError = -3,
			// Token: 0x040035A3 RID: 13731
			MemError = -4,
			// Token: 0x040035A4 RID: 13732
			BufError = -5,
			// Token: 0x040035A5 RID: 13733
			VersionError = -6
		}

		// Token: 0x0200081A RID: 2074
		public enum CompressionLevel
		{
			// Token: 0x040035A7 RID: 13735
			NoCompression,
			// Token: 0x040035A8 RID: 13736
			BestSpeed,
			// Token: 0x040035A9 RID: 13737
			BestCompression = 9,
			// Token: 0x040035AA RID: 13738
			DefaultCompression = -1
		}

		// Token: 0x0200081B RID: 2075
		public enum CompressionStrategy
		{
			// Token: 0x040035AC RID: 13740
			Filtered = 1,
			// Token: 0x040035AD RID: 13741
			HuffmanOnly,
			// Token: 0x040035AE RID: 13742
			Rle,
			// Token: 0x040035AF RID: 13743
			Fixed,
			// Token: 0x040035B0 RID: 13744
			DefaultStrategy = 0
		}

		// Token: 0x0200081C RID: 2076
		public enum CompressionMethod
		{
			// Token: 0x040035B2 RID: 13746
			Deflated = 8
		}

		// Token: 0x0200081D RID: 2077
		internal struct ZStream
		{
			// Token: 0x040035B3 RID: 13747
			internal IntPtr nextIn;

			// Token: 0x040035B4 RID: 13748
			internal uint availIn;

			// Token: 0x040035B5 RID: 13749
			internal uint totalIn;

			// Token: 0x040035B6 RID: 13750
			internal IntPtr nextOut;

			// Token: 0x040035B7 RID: 13751
			internal uint availOut;

			// Token: 0x040035B8 RID: 13752
			internal uint totalOut;

			// Token: 0x040035B9 RID: 13753
			internal IntPtr msg;

			// Token: 0x040035BA RID: 13754
			internal IntPtr state;

			// Token: 0x040035BB RID: 13755
			internal IntPtr zalloc;

			// Token: 0x040035BC RID: 13756
			internal IntPtr zfree;

			// Token: 0x040035BD RID: 13757
			internal IntPtr opaque;

			// Token: 0x040035BE RID: 13758
			internal int dataType;

			// Token: 0x040035BF RID: 13759
			internal uint adler;

			// Token: 0x040035C0 RID: 13760
			internal uint reserved;
		}

		// Token: 0x0200081E RID: 2078
		// (Invoke) Token: 0x06004504 RID: 17668
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		private unsafe delegate ZLibNative.ErrorCode DeflateInit2_Delegate(ZLibNative.ZStream* stream, ZLibNative.CompressionLevel level, ZLibNative.CompressionMethod method, int windowBits, int memLevel, ZLibNative.CompressionStrategy strategy, [MarshalAs(UnmanagedType.LPStr)] string version, int streamSize);

		// Token: 0x0200081F RID: 2079
		// (Invoke) Token: 0x06004508 RID: 17672
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		private unsafe delegate ZLibNative.ErrorCode DeflateDelegate(ZLibNative.ZStream* stream, ZLibNative.FlushCode flush);

		// Token: 0x02000820 RID: 2080
		// (Invoke) Token: 0x0600450C RID: 17676
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		private unsafe delegate ZLibNative.ErrorCode DeflateEndDelegate(ZLibNative.ZStream* stream);

		// Token: 0x02000821 RID: 2081
		// (Invoke) Token: 0x06004510 RID: 17680
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		private unsafe delegate ZLibNative.ErrorCode InflateInit2_Delegate(ZLibNative.ZStream* stream, int windowBits, [MarshalAs(UnmanagedType.LPStr)] string version, int streamSize);

		// Token: 0x02000822 RID: 2082
		// (Invoke) Token: 0x06004514 RID: 17684
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		private unsafe delegate ZLibNative.ErrorCode InflateDelegate(ZLibNative.ZStream* stream, ZLibNative.FlushCode flush);

		// Token: 0x02000823 RID: 2083
		// (Invoke) Token: 0x06004518 RID: 17688
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		private unsafe delegate ZLibNative.ErrorCode InflateEndDelegate(ZLibNative.ZStream* stream);

		// Token: 0x02000824 RID: 2084
		// (Invoke) Token: 0x0600451C RID: 17692
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		[SuppressUnmanagedCodeSecurity]
		[SecurityCritical]
		private delegate int ZlibCompileFlagsDelegate();

		// Token: 0x02000825 RID: 2085
		private class NativeMethods
		{
			// Token: 0x0600451F RID: 17695
			[SuppressUnmanagedCodeSecurity]
			[SecurityCritical]
			[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Ansi)]
			internal static extern IntPtr GetProcAddress(ZLibNative.SafeLibraryHandle moduleHandle, string procName);

			// Token: 0x06004520 RID: 17696
			[SuppressUnmanagedCodeSecurity]
			[SecurityCritical]
			[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
			internal static extern ZLibNative.SafeLibraryHandle LoadLibrary(string libPath);

			// Token: 0x06004521 RID: 17697
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			[SuppressUnmanagedCodeSecurity]
			[SecurityCritical]
			[DllImport("kernel32.dll", ExactSpelling = true)]
			internal static extern bool FreeLibrary(IntPtr moduleHandle);
		}

		// Token: 0x02000826 RID: 2086
		[SecurityCritical]
		private class SafeLibraryHandle : SafeHandleZeroOrMinusOneIsInvalid
		{
			// Token: 0x06004523 RID: 17699 RVA: 0x00120D80 File Offset: 0x0011EF80
			[SecurityCritical]
			internal SafeLibraryHandle()
				: base(true)
			{
			}

			// Token: 0x06004524 RID: 17700 RVA: 0x00120D8C File Offset: 0x0011EF8C
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
			[SecurityCritical]
			protected override bool ReleaseHandle()
			{
				bool flag = ZLibNative.NativeMethods.FreeLibrary(this.handle);
				this.handle = IntPtr.Zero;
				return flag;
			}
		}

		// Token: 0x02000827 RID: 2087
		[SecurityCritical]
		public sealed class ZLibStreamHandle : SafeHandleMinusOneIsInvalid
		{
			// Token: 0x06004525 RID: 17701 RVA: 0x00120DB1 File Offset: 0x0011EFB1
			public unsafe ZLibStreamHandle()
				: base(true)
			{
				this.zStreamPtr = (ZLibNative.ZStream*)(void*)ZLibNative.ZLibStreamHandle.AllocWithZeroOut(sizeof(ZLibNative.ZStream));
				this.initializationState = ZLibNative.ZLibStreamHandle.State.NotInitialized;
				this.handle = IntPtr.Zero;
			}

			// Token: 0x17000FA9 RID: 4009
			// (get) Token: 0x06004526 RID: 17702 RVA: 0x00120DE4 File Offset: 0x0011EFE4
			public ZLibNative.ZLibStreamHandle.State InitializationState
			{
				[SecurityCritical]
				get
				{
					return this.initializationState;
				}
			}

			// Token: 0x06004527 RID: 17703 RVA: 0x00120DF0 File Offset: 0x0011EFF0
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
			[SecurityCritical]
			protected unsafe override bool ReleaseHandle()
			{
				bool flag;
				try
				{
					if (ZLibNative.ZLibStreamHandle.zlibLibraryHandle == null || ZLibNative.ZLibStreamHandle.zlibLibraryHandle.IsInvalid)
					{
						flag = false;
					}
					else
					{
						switch (this.InitializationState)
						{
						case ZLibNative.ZLibStreamHandle.State.NotInitialized:
							flag = true;
							break;
						case ZLibNative.ZLibStreamHandle.State.InitializedForDeflate:
							flag = this.DeflateEnd() == ZLibNative.ErrorCode.Ok;
							break;
						case ZLibNative.ZLibStreamHandle.State.InitializedForInflate:
							flag = this.InflateEnd() == ZLibNative.ErrorCode.Ok;
							break;
						case ZLibNative.ZLibStreamHandle.State.Disposed:
							flag = true;
							break;
						default:
							flag = false;
							break;
						}
					}
				}
				finally
				{
					if (this.zStreamPtr != null)
					{
						Marshal.FreeHGlobal((IntPtr)((void*)this.zStreamPtr));
						this.zStreamPtr = null;
					}
				}
				return flag;
			}

			// Token: 0x17000FAA RID: 4010
			// (get) Token: 0x06004528 RID: 17704 RVA: 0x00120E8C File Offset: 0x0011F08C
			// (set) Token: 0x06004529 RID: 17705 RVA: 0x00120E99 File Offset: 0x0011F099
			public unsafe IntPtr NextIn
			{
				[SecurityCritical]
				get
				{
					return this.zStreamPtr->nextIn;
				}
				[SecurityCritical]
				set
				{
					if (this.zStreamPtr != null)
					{
						this.zStreamPtr->nextIn = value;
					}
				}
			}

			// Token: 0x17000FAB RID: 4011
			// (get) Token: 0x0600452A RID: 17706 RVA: 0x00120EB1 File Offset: 0x0011F0B1
			// (set) Token: 0x0600452B RID: 17707 RVA: 0x00120EBE File Offset: 0x0011F0BE
			public unsafe uint AvailIn
			{
				[SecurityCritical]
				get
				{
					return this.zStreamPtr->availIn;
				}
				[SecurityCritical]
				set
				{
					if (this.zStreamPtr != null)
					{
						this.zStreamPtr->availIn = value;
					}
				}
			}

			// Token: 0x17000FAC RID: 4012
			// (get) Token: 0x0600452C RID: 17708 RVA: 0x00120ED6 File Offset: 0x0011F0D6
			public unsafe uint TotalIn
			{
				[SecurityCritical]
				get
				{
					return this.zStreamPtr->totalIn;
				}
			}

			// Token: 0x17000FAD RID: 4013
			// (get) Token: 0x0600452D RID: 17709 RVA: 0x00120EE3 File Offset: 0x0011F0E3
			// (set) Token: 0x0600452E RID: 17710 RVA: 0x00120EF0 File Offset: 0x0011F0F0
			public unsafe IntPtr NextOut
			{
				[SecurityCritical]
				get
				{
					return this.zStreamPtr->nextOut;
				}
				[SecurityCritical]
				set
				{
					if (this.zStreamPtr != null)
					{
						this.zStreamPtr->nextOut = value;
					}
				}
			}

			// Token: 0x17000FAE RID: 4014
			// (get) Token: 0x0600452F RID: 17711 RVA: 0x00120F08 File Offset: 0x0011F108
			// (set) Token: 0x06004530 RID: 17712 RVA: 0x00120F15 File Offset: 0x0011F115
			public unsafe uint AvailOut
			{
				[SecurityCritical]
				get
				{
					return this.zStreamPtr->availOut;
				}
				[SecurityCritical]
				set
				{
					if (this.zStreamPtr != null)
					{
						this.zStreamPtr->availOut = value;
					}
				}
			}

			// Token: 0x17000FAF RID: 4015
			// (get) Token: 0x06004531 RID: 17713 RVA: 0x00120F2D File Offset: 0x0011F12D
			public unsafe uint TotalOut
			{
				[SecurityCritical]
				get
				{
					return this.zStreamPtr->totalOut;
				}
			}

			// Token: 0x17000FB0 RID: 4016
			// (get) Token: 0x06004532 RID: 17714 RVA: 0x00120F3A File Offset: 0x0011F13A
			public unsafe int DataType
			{
				[SecurityCritical]
				get
				{
					return this.zStreamPtr->dataType;
				}
			}

			// Token: 0x17000FB1 RID: 4017
			// (get) Token: 0x06004533 RID: 17715 RVA: 0x00120F47 File Offset: 0x0011F147
			public unsafe uint Adler
			{
				[SecurityCritical]
				get
				{
					return this.zStreamPtr->adler;
				}
			}

			// Token: 0x06004534 RID: 17716 RVA: 0x00120F54 File Offset: 0x0011F154
			[SecurityCritical]
			private void EnsureNotDisposed()
			{
				if (this.InitializationState == ZLibNative.ZLibStreamHandle.State.Disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
			}

			// Token: 0x06004535 RID: 17717 RVA: 0x00120F70 File Offset: 0x0011F170
			[SecurityCritical]
			private void EnsureState(ZLibNative.ZLibStreamHandle.State requiredState)
			{
				if (this.InitializationState != requiredState)
				{
					throw new InvalidOperationException("InitializationState != " + requiredState.ToString());
				}
			}

			// Token: 0x06004536 RID: 17718 RVA: 0x00120F98 File Offset: 0x0011F198
			[SecurityCritical]
			public unsafe ZLibNative.ErrorCode DeflateInit2_(ZLibNative.CompressionLevel level, int windowBits, int memLevel, ZLibNative.CompressionStrategy strategy)
			{
				this.EnsureNotDisposed();
				this.EnsureState(ZLibNative.ZLibStreamHandle.State.NotInitialized);
				bool flag = false;
				RuntimeHelpers.PrepareConstrainedRegions();
				ZLibNative.ErrorCode errorCode;
				try
				{
				}
				finally
				{
					errorCode = ZLibNative.ZLibStreamHandle.NativeZLibDLLStub.deflateInit2_Delegate(this.zStreamPtr, level, ZLibNative.CompressionMethod.Deflated, windowBits, memLevel, strategy, "1.3.1", sizeof(ZLibNative.ZStream));
					this.initializationState = ZLibNative.ZLibStreamHandle.State.InitializedForDeflate;
					ZLibNative.ZLibStreamHandle.zlibLibraryHandle.DangerousAddRef(ref flag);
				}
				return errorCode;
			}

			// Token: 0x06004537 RID: 17719 RVA: 0x00121004 File Offset: 0x0011F204
			[SecurityCritical]
			public ZLibNative.ErrorCode Deflate(ZLibNative.FlushCode flush)
			{
				this.EnsureNotDisposed();
				this.EnsureState(ZLibNative.ZLibStreamHandle.State.InitializedForDeflate);
				return ZLibNative.ZLibStreamHandle.NativeZLibDLLStub.deflateDelegate(this.zStreamPtr, flush);
			}

			// Token: 0x06004538 RID: 17720 RVA: 0x00121024 File Offset: 0x0011F224
			[SecurityCritical]
			public ZLibNative.ErrorCode DeflateEnd()
			{
				this.EnsureNotDisposed();
				this.EnsureState(ZLibNative.ZLibStreamHandle.State.InitializedForDeflate);
				RuntimeHelpers.PrepareConstrainedRegions();
				ZLibNative.ErrorCode errorCode;
				try
				{
				}
				finally
				{
					errorCode = ZLibNative.ZLibStreamHandle.NativeZLibDLLStub.deflateEndDelegate(this.zStreamPtr);
					this.initializationState = ZLibNative.ZLibStreamHandle.State.Disposed;
					ZLibNative.ZLibStreamHandle.zlibLibraryHandle.DangerousRelease();
				}
				return errorCode;
			}

			// Token: 0x06004539 RID: 17721 RVA: 0x0012107C File Offset: 0x0011F27C
			[SecurityCritical]
			public unsafe ZLibNative.ErrorCode InflateInit2_(int windowBits)
			{
				this.EnsureNotDisposed();
				this.EnsureState(ZLibNative.ZLibStreamHandle.State.NotInitialized);
				bool flag = false;
				RuntimeHelpers.PrepareConstrainedRegions();
				ZLibNative.ErrorCode errorCode;
				try
				{
				}
				finally
				{
					errorCode = ZLibNative.ZLibStreamHandle.NativeZLibDLLStub.inflateInit2_Delegate(this.zStreamPtr, windowBits, "1.3.1", sizeof(ZLibNative.ZStream));
					this.initializationState = ZLibNative.ZLibStreamHandle.State.InitializedForInflate;
					ZLibNative.ZLibStreamHandle.zlibLibraryHandle.DangerousAddRef(ref flag);
				}
				return errorCode;
			}

			// Token: 0x0600453A RID: 17722 RVA: 0x001210E4 File Offset: 0x0011F2E4
			[SecurityCritical]
			public ZLibNative.ErrorCode Inflate(ZLibNative.FlushCode flush)
			{
				this.EnsureNotDisposed();
				this.EnsureState(ZLibNative.ZLibStreamHandle.State.InitializedForInflate);
				return ZLibNative.ZLibStreamHandle.NativeZLibDLLStub.inflateDelegate(this.zStreamPtr, flush);
			}

			// Token: 0x0600453B RID: 17723 RVA: 0x00121104 File Offset: 0x0011F304
			[SecurityCritical]
			public ZLibNative.ErrorCode InflateEnd()
			{
				this.EnsureNotDisposed();
				this.EnsureState(ZLibNative.ZLibStreamHandle.State.InitializedForInflate);
				RuntimeHelpers.PrepareConstrainedRegions();
				ZLibNative.ErrorCode errorCode;
				try
				{
				}
				finally
				{
					errorCode = ZLibNative.ZLibStreamHandle.NativeZLibDLLStub.inflateEndDelegate(this.zStreamPtr);
					this.initializationState = ZLibNative.ZLibStreamHandle.State.Disposed;
					ZLibNative.ZLibStreamHandle.zlibLibraryHandle.DangerousRelease();
				}
				return errorCode;
			}

			// Token: 0x0600453C RID: 17724 RVA: 0x0012115C File Offset: 0x0011F35C
			[SecurityCritical]
			public unsafe string GetErrorMessage()
			{
				if (ZLibNative.ZNullPtr.Equals(this.zStreamPtr->msg))
				{
					return string.Empty;
				}
				return new string((sbyte*)(void*)this.zStreamPtr->msg);
			}

			// Token: 0x0600453D RID: 17725 RVA: 0x001211A5 File Offset: 0x0011F3A5
			[SecurityCritical]
			internal static int ZLibCompileFlags()
			{
				return ZLibNative.ZLibStreamHandle.NativeZLibDLLStub.zlibCompileFlagsDelegate();
			}

			// Token: 0x0600453E RID: 17726 RVA: 0x001211B4 File Offset: 0x0011F3B4
			[SecurityCritical]
			private unsafe static IntPtr AllocWithZeroOut(int byteCount)
			{
				IntPtr intPtr = Marshal.AllocHGlobal(byteCount);
				byte* ptr = (byte*)(void*)intPtr;
				int num = byteCount / 4;
				int* ptr2 = (int*)ptr;
				for (int i = 0; i < num; i++)
				{
					ptr2[i] = 0;
				}
				num *= 4;
				ptr += num;
				int num2 = byteCount - num;
				for (int j = 0; j < num2; j++)
				{
					ptr[j] = 0;
				}
				return intPtr;
			}

			// Token: 0x040035C1 RID: 13761
			[SecurityCritical]
			private static ZLibNative.SafeLibraryHandle zlibLibraryHandle;

			// Token: 0x040035C2 RID: 13762
			[SecurityCritical]
			private unsafe ZLibNative.ZStream* zStreamPtr;

			// Token: 0x040035C3 RID: 13763
			[SecurityCritical]
			private volatile ZLibNative.ZLibStreamHandle.State initializationState;

			// Token: 0x02000930 RID: 2352
			[SecurityCritical]
			private static class NativeZLibDLLStub
			{
				// Token: 0x0600469F RID: 18079 RVA: 0x00126A44 File Offset: 0x00124C44
				[SecuritySafeCritical]
				private static void LoadZLibDLL()
				{
					new FileIOPermission(PermissionState.Unrestricted).Assert();
					string runtimeDirectory = RuntimeEnvironment.GetRuntimeDirectory();
					string text = Path.Combine(runtimeDirectory, "clrcompression.dll");
					if (!File.Exists(text))
					{
						throw new DllNotFoundException("clrcompression.dll");
					}
					ZLibNative.SafeLibraryHandle safeLibraryHandle = ZLibNative.NativeMethods.LoadLibrary(text);
					if (safeLibraryHandle.IsInvalid)
					{
						int hrforLastWin32Error = Marshal.GetHRForLastWin32Error();
						Marshal.ThrowExceptionForHR(hrforLastWin32Error, new IntPtr(-1));
						throw new InvalidOperationException();
					}
					ZLibNative.ZLibStreamHandle.zlibLibraryHandle = safeLibraryHandle;
				}

				// Token: 0x060046A0 RID: 18080 RVA: 0x00126AB0 File Offset: 0x00124CB0
				[SecurityCritical]
				private static DT CreateDelegate<DT>(string entryPointName)
				{
					IntPtr procAddress = ZLibNative.NativeMethods.GetProcAddress(ZLibNative.ZLibStreamHandle.zlibLibraryHandle, entryPointName);
					if (IntPtr.Zero == procAddress)
					{
						throw new EntryPointNotFoundException("clrcompression.dll!" + entryPointName);
					}
					return (DT)((object)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(DT)));
				}

				// Token: 0x060046A1 RID: 18081 RVA: 0x00126AFC File Offset: 0x00124CFC
				[SecuritySafeCritical]
				private static void InitDelegates()
				{
					ZLibNative.ZLibStreamHandle.NativeZLibDLLStub.deflateInit2_Delegate = ZLibNative.ZLibStreamHandle.NativeZLibDLLStub.CreateDelegate<ZLibNative.DeflateInit2_Delegate>("deflateInit2_");
					ZLibNative.ZLibStreamHandle.NativeZLibDLLStub.deflateDelegate = ZLibNative.ZLibStreamHandle.NativeZLibDLLStub.CreateDelegate<ZLibNative.DeflateDelegate>("deflate");
					ZLibNative.ZLibStreamHandle.NativeZLibDLLStub.deflateEndDelegate = ZLibNative.ZLibStreamHandle.NativeZLibDLLStub.CreateDelegate<ZLibNative.DeflateEndDelegate>("deflateEnd");
					ZLibNative.ZLibStreamHandle.NativeZLibDLLStub.inflateInit2_Delegate = ZLibNative.ZLibStreamHandle.NativeZLibDLLStub.CreateDelegate<ZLibNative.InflateInit2_Delegate>("inflateInit2_");
					ZLibNative.ZLibStreamHandle.NativeZLibDLLStub.inflateDelegate = ZLibNative.ZLibStreamHandle.NativeZLibDLLStub.CreateDelegate<ZLibNative.InflateDelegate>("inflate");
					ZLibNative.ZLibStreamHandle.NativeZLibDLLStub.inflateEndDelegate = ZLibNative.ZLibStreamHandle.NativeZLibDLLStub.CreateDelegate<ZLibNative.InflateEndDelegate>("inflateEnd");
					ZLibNative.ZLibStreamHandle.NativeZLibDLLStub.zlibCompileFlagsDelegate = ZLibNative.ZLibStreamHandle.NativeZLibDLLStub.CreateDelegate<ZLibNative.ZlibCompileFlagsDelegate>("zlibCompileFlags");
					RuntimeHelpers.PrepareDelegate(ZLibNative.ZLibStreamHandle.NativeZLibDLLStub.deflateInit2_Delegate);
					RuntimeHelpers.PrepareDelegate(ZLibNative.ZLibStreamHandle.NativeZLibDLLStub.deflateDelegate);
					RuntimeHelpers.PrepareDelegate(ZLibNative.ZLibStreamHandle.NativeZLibDLLStub.deflateEndDelegate);
					RuntimeHelpers.PrepareDelegate(ZLibNative.ZLibStreamHandle.NativeZLibDLLStub.inflateInit2_Delegate);
					RuntimeHelpers.PrepareDelegate(ZLibNative.ZLibStreamHandle.NativeZLibDLLStub.inflateDelegate);
					RuntimeHelpers.PrepareDelegate(ZLibNative.ZLibStreamHandle.NativeZLibDLLStub.inflateEndDelegate);
					RuntimeHelpers.PrepareDelegate(ZLibNative.ZLibStreamHandle.NativeZLibDLLStub.zlibCompileFlagsDelegate);
				}

				// Token: 0x060046A2 RID: 18082 RVA: 0x00126BB8 File Offset: 0x00124DB8
				[SecuritySafeCritical]
				static NativeZLibDLLStub()
				{
					ZLibNative.ZLibStreamHandle.NativeZLibDLLStub.LoadZLibDLL();
					ZLibNative.ZLibStreamHandle.NativeZLibDLLStub.InitDelegates();
				}

				// Token: 0x04003DC7 RID: 15815
				[SecurityCritical]
				internal static ZLibNative.DeflateInit2_Delegate deflateInit2_Delegate;

				// Token: 0x04003DC8 RID: 15816
				[SecurityCritical]
				internal static ZLibNative.DeflateDelegate deflateDelegate;

				// Token: 0x04003DC9 RID: 15817
				[SecurityCritical]
				internal static ZLibNative.DeflateEndDelegate deflateEndDelegate;

				// Token: 0x04003DCA RID: 15818
				[SecurityCritical]
				internal static ZLibNative.InflateInit2_Delegate inflateInit2_Delegate;

				// Token: 0x04003DCB RID: 15819
				[SecurityCritical]
				internal static ZLibNative.InflateDelegate inflateDelegate;

				// Token: 0x04003DCC RID: 15820
				[SecurityCritical]
				internal static ZLibNative.InflateEndDelegate inflateEndDelegate;

				// Token: 0x04003DCD RID: 15821
				[SecurityCritical]
				internal static ZLibNative.ZlibCompileFlagsDelegate zlibCompileFlagsDelegate;
			}

			// Token: 0x02000931 RID: 2353
			public enum State
			{
				// Token: 0x04003DCF RID: 15823
				NotInitialized,
				// Token: 0x04003DD0 RID: 15824
				InitializedForDeflate,
				// Token: 0x04003DD1 RID: 15825
				InitializedForInflate,
				// Token: 0x04003DD2 RID: 15826
				Disposed
			}
		}
	}
}
