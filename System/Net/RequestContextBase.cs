using System;

namespace System.Net
{
	// Token: 0x020000F4 RID: 244
	internal abstract class RequestContextBase : IDisposable
	{
		// Token: 0x0600087F RID: 2175 RVA: 0x0002F58E File Offset: 0x0002D78E
		protected unsafe void BaseConstruction(UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST* requestBlob)
		{
			if (requestBlob == null)
			{
				GC.SuppressFinalize(this);
				return;
			}
			this.m_MemoryBlob = requestBlob;
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x0002F5A3 File Offset: 0x0002D7A3
		internal void ReleasePins()
		{
			this.m_OriginalBlobAddress = this.m_MemoryBlob;
			this.UnsetBlob();
			this.OnReleasePins();
		}

		// Token: 0x06000881 RID: 2177
		protected abstract void OnReleasePins();

		// Token: 0x06000882 RID: 2178 RVA: 0x0002F5BD File Offset: 0x0002D7BD
		public void Close()
		{
			this.Dispose();
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x0002F5C5 File Offset: 0x0002D7C5
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x0002F5CE File Offset: 0x0002D7CE
		protected virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x0002F5D0 File Offset: 0x0002D7D0
		~RequestContextBase()
		{
			this.Dispose(false);
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000886 RID: 2182 RVA: 0x0002F600 File Offset: 0x0002D800
		internal unsafe UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST* RequestBlob
		{
			get
			{
				return this.m_MemoryBlob;
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000887 RID: 2183 RVA: 0x0002F608 File Offset: 0x0002D808
		internal byte[] RequestBuffer
		{
			get
			{
				return this.m_BackingBuffer;
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x06000888 RID: 2184 RVA: 0x0002F610 File Offset: 0x0002D810
		internal uint Size
		{
			get
			{
				return (uint)this.m_BackingBuffer.Length;
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x06000889 RID: 2185 RVA: 0x0002F61C File Offset: 0x0002D81C
		internal unsafe IntPtr OriginalBlobAddress
		{
			get
			{
				UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST* memoryBlob = this.m_MemoryBlob;
				return (IntPtr)((void*)((memoryBlob == null) ? this.m_OriginalBlobAddress : memoryBlob));
			}
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x0002F643 File Offset: 0x0002D843
		protected unsafe void SetBlob(UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST* requestBlob)
		{
			if (requestBlob == null)
			{
				this.UnsetBlob();
				return;
			}
			if (this.m_MemoryBlob == null)
			{
				GC.ReRegisterForFinalize(this);
			}
			this.m_MemoryBlob = requestBlob;
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x0002F668 File Offset: 0x0002D868
		protected void UnsetBlob()
		{
			if (this.m_MemoryBlob != null)
			{
				GC.SuppressFinalize(this);
			}
			this.m_MemoryBlob = null;
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x0002F682 File Offset: 0x0002D882
		protected void SetBuffer(int size)
		{
			this.m_BackingBuffer = ((size == 0) ? null : new byte[size]);
		}

		// Token: 0x04000DD9 RID: 3545
		private unsafe UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST* m_MemoryBlob;

		// Token: 0x04000DDA RID: 3546
		private unsafe UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST* m_OriginalBlobAddress;

		// Token: 0x04000DDB RID: 3547
		private byte[] m_BackingBuffer;
	}
}
