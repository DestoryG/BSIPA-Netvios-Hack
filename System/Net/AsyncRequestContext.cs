using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Net
{
	// Token: 0x020000F5 RID: 245
	internal class AsyncRequestContext : RequestContextBase
	{
		// Token: 0x0600088E RID: 2190 RVA: 0x0002F69E File Offset: 0x0002D89E
		internal AsyncRequestContext(ListenerAsyncResult result)
		{
			this.m_Result = result;
			base.BaseConstruction(this.Allocate(0U));
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x0002F6BC File Offset: 0x0002D8BC
		private unsafe UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST* Allocate(uint size)
		{
			uint num = ((size != 0U) ? size : ((base.RequestBuffer == null) ? 4096U : base.Size));
			if (this.m_NativeOverlapped != null && (ulong)num != (ulong)((long)base.RequestBuffer.Length))
			{
				NativeOverlapped* nativeOverlapped = this.m_NativeOverlapped;
				this.m_NativeOverlapped = null;
				Overlapped.Free(nativeOverlapped);
			}
			if (this.m_NativeOverlapped == null)
			{
				base.SetBuffer(checked((int)num));
				this.m_NativeOverlapped = new Overlapped
				{
					AsyncResult = this.m_Result
				}.Pack(ListenerAsyncResult.IOCallback, base.RequestBuffer);
				return (UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST*)(void*)Marshal.UnsafeAddrOfPinnedArrayElement(base.RequestBuffer, 0);
			}
			return base.RequestBlob;
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x0002F764 File Offset: 0x0002D964
		internal unsafe void Reset(ulong requestId, uint size)
		{
			base.SetBlob(this.Allocate(size));
			base.RequestBlob->RequestId = requestId;
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x0002F780 File Offset: 0x0002D980
		protected unsafe override void OnReleasePins()
		{
			if (this.m_NativeOverlapped != null)
			{
				NativeOverlapped* nativeOverlapped = this.m_NativeOverlapped;
				this.m_NativeOverlapped = null;
				Overlapped.Free(nativeOverlapped);
			}
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x0002F7AC File Offset: 0x0002D9AC
		protected override void Dispose(bool disposing)
		{
			if (this.m_NativeOverlapped != null && (!NclUtilities.HasShutdownStarted || disposing))
			{
				Overlapped.Free(this.m_NativeOverlapped);
			}
			base.Dispose(disposing);
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000893 RID: 2195 RVA: 0x0002F7D6 File Offset: 0x0002D9D6
		internal unsafe NativeOverlapped* NativeOverlapped
		{
			get
			{
				return this.m_NativeOverlapped;
			}
		}

		// Token: 0x04000DDC RID: 3548
		private unsafe NativeOverlapped* m_NativeOverlapped;

		// Token: 0x04000DDD RID: 3549
		private ListenerAsyncResult m_Result;
	}
}
