using System;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Net.Sockets
{
	// Token: 0x0200039D RID: 925
	internal class TransmitFileOverlappedAsyncResult : BaseOverlappedAsyncResult
	{
		// Token: 0x06002284 RID: 8836 RVA: 0x000A47C4 File Offset: 0x000A29C4
		internal TransmitFileOverlappedAsyncResult(Socket socket, object asyncState, AsyncCallback asyncCallback)
			: base(socket, asyncState, asyncCallback)
		{
		}

		// Token: 0x06002285 RID: 8837 RVA: 0x000A47CF File Offset: 0x000A29CF
		internal TransmitFileOverlappedAsyncResult(Socket socket)
			: base(socket)
		{
		}

		// Token: 0x06002286 RID: 8838 RVA: 0x000A47D8 File Offset: 0x000A29D8
		internal void SetUnmanagedStructures(byte[] preBuffer, byte[] postBuffer, FileStream fileStream, TransmitFileOptions flags, bool sync)
		{
			this.m_fileStream = fileStream;
			this.m_flags = flags;
			this.m_buffers = null;
			int num = 0;
			if (preBuffer != null && preBuffer.Length != 0)
			{
				num++;
			}
			if (postBuffer != null && postBuffer.Length != 0)
			{
				num++;
			}
			if (num != 0)
			{
				num++;
				object[] array = new object[num];
				this.m_buffers = new TransmitFileBuffers();
				array[--num] = this.m_buffers;
				if (preBuffer != null && preBuffer.Length != 0)
				{
					this.m_buffers.preBufferLength = preBuffer.Length;
					array[--num] = preBuffer;
				}
				if (postBuffer != null && postBuffer.Length != 0)
				{
					this.m_buffers.postBufferLength = postBuffer.Length;
					array[num - 1] = postBuffer;
				}
				if (sync)
				{
					base.PinUnmanagedObjects(array);
				}
				else
				{
					base.SetUnmanagedStructures(array);
				}
				if (preBuffer != null && preBuffer.Length != 0)
				{
					this.m_buffers.preBuffer = Marshal.UnsafeAddrOfPinnedArrayElement(preBuffer, 0);
				}
				if (postBuffer != null && postBuffer.Length != 0)
				{
					this.m_buffers.postBuffer = Marshal.UnsafeAddrOfPinnedArrayElement(postBuffer, 0);
					return;
				}
			}
			else if (!sync)
			{
				base.SetUnmanagedStructures(null);
			}
		}

		// Token: 0x06002287 RID: 8839 RVA: 0x000A48CA File Offset: 0x000A2ACA
		internal void SetUnmanagedStructures(byte[] preBuffer, byte[] postBuffer, FileStream fileStream, TransmitFileOptions flags, ref OverlappedCache overlappedCache)
		{
			base.SetupCache(ref overlappedCache);
			this.SetUnmanagedStructures(preBuffer, postBuffer, fileStream, flags, false);
		}

		// Token: 0x06002288 RID: 8840 RVA: 0x000A48E0 File Offset: 0x000A2AE0
		protected override void ForceReleaseUnmanagedStructures()
		{
			if (this.m_fileStream != null)
			{
				this.m_fileStream.Close();
				this.m_fileStream = null;
			}
			base.ForceReleaseUnmanagedStructures();
		}

		// Token: 0x06002289 RID: 8841 RVA: 0x000A4902 File Offset: 0x000A2B02
		internal void SyncReleaseUnmanagedStructures()
		{
			this.ForceReleaseUnmanagedStructures();
		}

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x0600228A RID: 8842 RVA: 0x000A490A File Offset: 0x000A2B0A
		internal TransmitFileBuffers TransmitFileBuffers
		{
			get
			{
				return this.m_buffers;
			}
		}

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x0600228B RID: 8843 RVA: 0x000A4912 File Offset: 0x000A2B12
		internal TransmitFileOptions Flags
		{
			get
			{
				return this.m_flags;
			}
		}

		// Token: 0x04001F7D RID: 8061
		private FileStream m_fileStream;

		// Token: 0x04001F7E RID: 8062
		private TransmitFileOptions m_flags;

		// Token: 0x04001F7F RID: 8063
		private TransmitFileBuffers m_buffers;
	}
}
