using System;
using System.IO;
using System.IO.Compression;

namespace System.Net
{
	// Token: 0x0200010D RID: 269
	internal class DeflateWrapperStream : DeflateStream, ICloseEx, IRequestLifetimeTracker
	{
		// Token: 0x06000AED RID: 2797 RVA: 0x0003C874 File Offset: 0x0003AA74
		public DeflateWrapperStream(Stream stream, CompressionMode mode)
			: base(stream, mode, false)
		{
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x0003C880 File Offset: 0x0003AA80
		void ICloseEx.CloseEx(CloseExState closeState)
		{
			ICloseEx closeEx = base.BaseStream as ICloseEx;
			if (closeEx != null)
			{
				closeEx.CloseEx(closeState);
				return;
			}
			this.Close();
		}

		// Token: 0x06000AEF RID: 2799 RVA: 0x0003C8AC File Offset: 0x0003AAAC
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || size > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			IAsyncResult asyncResult;
			try
			{
				asyncResult = base.BeginRead(buffer, offset, size, callback, state);
			}
			catch (Exception ex)
			{
				try
				{
					if (NclUtilities.IsFatal(ex))
					{
						throw;
					}
					if (ex is InvalidDataException || ex is InvalidOperationException || ex is IndexOutOfRangeException)
					{
						this.Close();
					}
				}
				catch
				{
				}
				throw ex;
			}
			return asyncResult;
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x0003C954 File Offset: 0x0003AB54
		public override int EndRead(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			int num;
			try
			{
				num = base.EndRead(asyncResult);
			}
			catch (Exception ex)
			{
				try
				{
					if (NclUtilities.IsFatal(ex))
					{
						throw;
					}
					if (ex is InvalidDataException || ex is InvalidOperationException || ex is IndexOutOfRangeException)
					{
						this.Close();
					}
				}
				catch
				{
				}
				throw ex;
			}
			return num;
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x0003C9C8 File Offset: 0x0003ABC8
		public override int Read(byte[] buffer, int offset, int size)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0 || offset > buffer.Length)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (size < 0 || size > buffer.Length - offset)
			{
				throw new ArgumentOutOfRangeException("size");
			}
			int num;
			try
			{
				num = base.Read(buffer, offset, size);
			}
			catch (Exception ex)
			{
				try
				{
					if (NclUtilities.IsFatal(ex))
					{
						throw;
					}
					if (ex is InvalidDataException || ex is InvalidOperationException || ex is IndexOutOfRangeException)
					{
						this.Close();
					}
				}
				catch
				{
				}
				throw ex;
			}
			return num;
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x0003CA6C File Offset: 0x0003AC6C
		void IRequestLifetimeTracker.TrackRequestLifetime(long requestStartTimestamp)
		{
			IRequestLifetimeTracker requestLifetimeTracker = base.BaseStream as IRequestLifetimeTracker;
			requestLifetimeTracker.TrackRequestLifetime(requestStartTimestamp);
		}
	}
}
