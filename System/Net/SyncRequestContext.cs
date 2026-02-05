using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x020000F6 RID: 246
	internal class SyncRequestContext : RequestContextBase
	{
		// Token: 0x06000894 RID: 2196 RVA: 0x0002F7DE File Offset: 0x0002D9DE
		internal SyncRequestContext(int size)
		{
			base.BaseConstruction(this.Allocate(size));
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x0002F7F4 File Offset: 0x0002D9F4
		private unsafe UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST* Allocate(int size)
		{
			if (this.m_PinnedHandle.IsAllocated)
			{
				if (base.RequestBuffer.Length == size)
				{
					return base.RequestBlob;
				}
				this.m_PinnedHandle.Free();
			}
			base.SetBuffer(size);
			if (base.RequestBuffer == null)
			{
				return null;
			}
			this.m_PinnedHandle = GCHandle.Alloc(base.RequestBuffer, GCHandleType.Pinned);
			return (UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST*)(void*)Marshal.UnsafeAddrOfPinnedArrayElement(base.RequestBuffer, 0);
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x0002F860 File Offset: 0x0002DA60
		internal void Reset(int size)
		{
			base.SetBlob(this.Allocate(size));
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x0002F86F File Offset: 0x0002DA6F
		protected override void OnReleasePins()
		{
			if (this.m_PinnedHandle.IsAllocated)
			{
				this.m_PinnedHandle.Free();
			}
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x0002F889 File Offset: 0x0002DA89
		protected override void Dispose(bool disposing)
		{
			if (this.m_PinnedHandle.IsAllocated && (!NclUtilities.HasShutdownStarted || disposing))
			{
				this.m_PinnedHandle.Free();
			}
			base.Dispose(disposing);
		}

		// Token: 0x04000DDE RID: 3550
		private GCHandle m_PinnedHandle;
	}
}
