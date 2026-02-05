using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace System.Net.Sockets
{
	// Token: 0x0200039C RID: 924
	internal class OverlappedAsyncResult : BaseOverlappedAsyncResult
	{
		// Token: 0x06002276 RID: 8822 RVA: 0x000A4491 File Offset: 0x000A2691
		internal OverlappedAsyncResult(Socket socket, object asyncState, AsyncCallback asyncCallback)
			: base(socket, asyncState, asyncCallback)
		{
		}

		// Token: 0x06002277 RID: 8823 RVA: 0x000A449C File Offset: 0x000A269C
		internal IntPtr GetSocketAddressPtr()
		{
			return Marshal.UnsafeAddrOfPinnedArrayElement(this.m_SocketAddress.m_Buffer, 0);
		}

		// Token: 0x06002278 RID: 8824 RVA: 0x000A44AF File Offset: 0x000A26AF
		internal IntPtr GetSocketAddressSizePtr()
		{
			return Marshal.UnsafeAddrOfPinnedArrayElement(this.m_SocketAddress.m_Buffer, this.m_SocketAddress.GetAddressSizeOffset());
		}

		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x06002279 RID: 8825 RVA: 0x000A44CC File Offset: 0x000A26CC
		internal SocketAddress SocketAddress
		{
			get
			{
				return this.m_SocketAddress;
			}
		}

		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x0600227A RID: 8826 RVA: 0x000A44D4 File Offset: 0x000A26D4
		// (set) Token: 0x0600227B RID: 8827 RVA: 0x000A44DC File Offset: 0x000A26DC
		internal SocketAddress SocketAddressOriginal
		{
			get
			{
				return this.m_SocketAddressOriginal;
			}
			set
			{
				this.m_SocketAddressOriginal = value;
			}
		}

		// Token: 0x0600227C RID: 8828 RVA: 0x000A44E8 File Offset: 0x000A26E8
		internal void SetUnmanagedStructures(byte[] buffer, int offset, int size, SocketAddress socketAddress, bool pinSocketAddress)
		{
			this.m_SocketAddress = socketAddress;
			if (pinSocketAddress && this.m_SocketAddress != null)
			{
				object[] array = new object[2];
				array[0] = buffer;
				this.m_SocketAddress.CopyAddressSizeIntoBuffer();
				array[1] = this.m_SocketAddress.m_Buffer;
				base.SetUnmanagedStructures(array);
			}
			else
			{
				base.SetUnmanagedStructures(buffer);
			}
			this.m_SingleBuffer.Length = size;
			this.m_SingleBuffer.Pointer = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, offset);
		}

		// Token: 0x0600227D RID: 8829 RVA: 0x000A455D File Offset: 0x000A275D
		internal void SetUnmanagedStructures(byte[] buffer, int offset, int size, SocketAddress socketAddress, bool pinSocketAddress, ref OverlappedCache overlappedCache)
		{
			base.SetupCache(ref overlappedCache);
			this.SetUnmanagedStructures(buffer, offset, size, socketAddress, pinSocketAddress);
		}

		// Token: 0x0600227E RID: 8830 RVA: 0x000A4574 File Offset: 0x000A2774
		internal void SetUnmanagedStructures(BufferOffsetSize[] buffers)
		{
			this.m_WSABuffers = new WSABuffer[buffers.Length];
			object[] array = new object[buffers.Length];
			for (int i = 0; i < buffers.Length; i++)
			{
				array[i] = buffers[i].Buffer;
			}
			base.SetUnmanagedStructures(array);
			for (int j = 0; j < buffers.Length; j++)
			{
				this.m_WSABuffers[j].Length = buffers[j].Size;
				this.m_WSABuffers[j].Pointer = Marshal.UnsafeAddrOfPinnedArrayElement(buffers[j].Buffer, buffers[j].Offset);
			}
		}

		// Token: 0x0600227F RID: 8831 RVA: 0x000A4605 File Offset: 0x000A2805
		internal void SetUnmanagedStructures(BufferOffsetSize[] buffers, ref OverlappedCache overlappedCache)
		{
			base.SetupCache(ref overlappedCache);
			this.SetUnmanagedStructures(buffers);
		}

		// Token: 0x06002280 RID: 8832 RVA: 0x000A4618 File Offset: 0x000A2818
		internal void SetUnmanagedStructures(IList<ArraySegment<byte>> buffers)
		{
			int count = buffers.Count;
			ArraySegment<byte>[] array = new ArraySegment<byte>[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = buffers[i];
				ValidationHelper.ValidateSegment(array[i]);
			}
			this.m_WSABuffers = new WSABuffer[count];
			object[] array2 = new object[count];
			for (int j = 0; j < count; j++)
			{
				array2[j] = array[j].Array;
			}
			base.SetUnmanagedStructures(array2);
			for (int k = 0; k < count; k++)
			{
				this.m_WSABuffers[k].Length = array[k].Count;
				this.m_WSABuffers[k].Pointer = Marshal.UnsafeAddrOfPinnedArrayElement(array[k].Array, array[k].Offset);
			}
		}

		// Token: 0x06002281 RID: 8833 RVA: 0x000A46F4 File Offset: 0x000A28F4
		internal void SetUnmanagedStructures(IList<ArraySegment<byte>> buffers, ref OverlappedCache overlappedCache)
		{
			base.SetupCache(ref overlappedCache);
			this.SetUnmanagedStructures(buffers);
		}

		// Token: 0x06002282 RID: 8834 RVA: 0x000A4704 File Offset: 0x000A2904
		internal override object PostCompletion(int numBytes)
		{
			if (base.ErrorCode == 0 && Logging.On)
			{
				this.LogBuffer(numBytes);
			}
			return numBytes;
		}

		// Token: 0x06002283 RID: 8835 RVA: 0x000A4724 File Offset: 0x000A2924
		private void LogBuffer(int size)
		{
			if (size > -1)
			{
				if (this.m_WSABuffers != null)
				{
					foreach (WSABuffer wsabuffer in this.m_WSABuffers)
					{
						Logging.Dump(Logging.Sockets, base.AsyncObject, "PostCompletion", wsabuffer.Pointer, Math.Min(wsabuffer.Length, size));
						if ((size -= wsabuffer.Length) <= 0)
						{
							return;
						}
					}
					return;
				}
				Logging.Dump(Logging.Sockets, base.AsyncObject, "PostCompletion", this.m_SingleBuffer.Pointer, Math.Min(this.m_SingleBuffer.Length, size));
			}
		}

		// Token: 0x04001F79 RID: 8057
		private SocketAddress m_SocketAddress;

		// Token: 0x04001F7A RID: 8058
		private SocketAddress m_SocketAddressOriginal;

		// Token: 0x04001F7B RID: 8059
		internal WSABuffer m_SingleBuffer;

		// Token: 0x04001F7C RID: 8060
		internal WSABuffer[] m_WSABuffers;
	}
}
