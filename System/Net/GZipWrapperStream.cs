using System;
using System.IO;
using System.IO.Compression;

namespace System.Net
{
	// Token: 0x0200010C RID: 268
	internal class GZipWrapperStream : GZipStream, ICloseEx, IRequestLifetimeTracker
	{
		// Token: 0x06000AE7 RID: 2791 RVA: 0x0003C65A File Offset: 0x0003A85A
		public GZipWrapperStream(Stream stream, CompressionMode mode)
			: base(stream, mode, false)
		{
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x0003C668 File Offset: 0x0003A868
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

		// Token: 0x06000AE9 RID: 2793 RVA: 0x0003C694 File Offset: 0x0003A894
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

		// Token: 0x06000AEA RID: 2794 RVA: 0x0003C73C File Offset: 0x0003A93C
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

		// Token: 0x06000AEB RID: 2795 RVA: 0x0003C7B0 File Offset: 0x0003A9B0
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

		// Token: 0x06000AEC RID: 2796 RVA: 0x0003C854 File Offset: 0x0003AA54
		void IRequestLifetimeTracker.TrackRequestLifetime(long requestStartTimestamp)
		{
			IRequestLifetimeTracker requestLifetimeTracker = base.BaseStream as IRequestLifetimeTracker;
			requestLifetimeTracker.TrackRequestLifetime(requestStartTimestamp);
		}
	}
}
