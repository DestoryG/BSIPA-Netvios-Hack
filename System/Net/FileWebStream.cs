using System;
using System.IO;

namespace System.Net
{
	// Token: 0x020000E8 RID: 232
	internal sealed class FileWebStream : FileStream, ICloseEx
	{
		// Token: 0x060007F4 RID: 2036 RVA: 0x0002BFE5 File Offset: 0x0002A1E5
		public FileWebStream(FileWebRequest request, string path, FileMode mode, FileAccess access, FileShare sharing)
			: base(path, mode, access, sharing)
		{
			this.m_request = request;
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x0002BFFA File Offset: 0x0002A1FA
		public FileWebStream(FileWebRequest request, string path, FileMode mode, FileAccess access, FileShare sharing, int length, bool async)
			: base(path, mode, access, sharing, length, async)
		{
			this.m_request = request;
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x0002C014 File Offset: 0x0002A214
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing && this.m_request != null)
				{
					this.m_request.UnblockReader();
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x0002C054 File Offset: 0x0002A254
		void ICloseEx.CloseEx(CloseExState closeState)
		{
			if ((closeState & CloseExState.Abort) != CloseExState.Normal)
			{
				this.SafeFileHandle.Close();
				return;
			}
			this.Close();
		}

		// Token: 0x060007F8 RID: 2040 RVA: 0x0002C070 File Offset: 0x0002A270
		public override int Read(byte[] buffer, int offset, int size)
		{
			this.CheckError();
			int num;
			try
			{
				num = base.Read(buffer, offset, size);
			}
			catch
			{
				this.CheckError();
				throw;
			}
			return num;
		}

		// Token: 0x060007F9 RID: 2041 RVA: 0x0002C0AC File Offset: 0x0002A2AC
		public override void Write(byte[] buffer, int offset, int size)
		{
			this.CheckError();
			try
			{
				base.Write(buffer, offset, size);
			}
			catch
			{
				this.CheckError();
				throw;
			}
		}

		// Token: 0x060007FA RID: 2042 RVA: 0x0002C0E4 File Offset: 0x0002A2E4
		public override IAsyncResult BeginRead(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			this.CheckError();
			IAsyncResult asyncResult;
			try
			{
				asyncResult = base.BeginRead(buffer, offset, size, callback, state);
			}
			catch
			{
				this.CheckError();
				throw;
			}
			return asyncResult;
		}

		// Token: 0x060007FB RID: 2043 RVA: 0x0002C124 File Offset: 0x0002A324
		public override int EndRead(IAsyncResult ar)
		{
			int num;
			try
			{
				num = base.EndRead(ar);
			}
			catch
			{
				this.CheckError();
				throw;
			}
			return num;
		}

		// Token: 0x060007FC RID: 2044 RVA: 0x0002C158 File Offset: 0x0002A358
		public override IAsyncResult BeginWrite(byte[] buffer, int offset, int size, AsyncCallback callback, object state)
		{
			this.CheckError();
			IAsyncResult asyncResult;
			try
			{
				asyncResult = base.BeginWrite(buffer, offset, size, callback, state);
			}
			catch
			{
				this.CheckError();
				throw;
			}
			return asyncResult;
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x0002C198 File Offset: 0x0002A398
		public override void EndWrite(IAsyncResult ar)
		{
			try
			{
				base.EndWrite(ar);
			}
			catch
			{
				this.CheckError();
				throw;
			}
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x0002C1C8 File Offset: 0x0002A3C8
		private void CheckError()
		{
			if (this.m_request.Aborted)
			{
				throw new WebException(NetRes.GetWebStatusString("net_requestaborted", WebExceptionStatus.RequestCanceled), WebExceptionStatus.RequestCanceled);
			}
		}

		// Token: 0x04000D52 RID: 3410
		private FileWebRequest m_request;
	}
}
