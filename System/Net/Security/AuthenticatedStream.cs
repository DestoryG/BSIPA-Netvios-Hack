using System;
using System.IO;

namespace System.Net.Security
{
	// Token: 0x02000354 RID: 852
	public abstract class AuthenticatedStream : Stream
	{
		// Token: 0x06001E89 RID: 7817 RVA: 0x0008FBA0 File Offset: 0x0008DDA0
		protected AuthenticatedStream(Stream innerStream, bool leaveInnerStreamOpen)
		{
			if (innerStream == null || innerStream == Stream.Null)
			{
				throw new ArgumentNullException("innerStream");
			}
			if (!innerStream.CanRead || !innerStream.CanWrite)
			{
				throw new ArgumentException(SR.GetString("net_io_must_be_rw_stream"), "innerStream");
			}
			this._InnerStream = innerStream;
			this._LeaveStreamOpen = leaveInnerStreamOpen;
		}

		// Token: 0x170007EE RID: 2030
		// (get) Token: 0x06001E8A RID: 7818 RVA: 0x0008FBFC File Offset: 0x0008DDFC
		public bool LeaveInnerStreamOpen
		{
			get
			{
				return this._LeaveStreamOpen;
			}
		}

		// Token: 0x170007EF RID: 2031
		// (get) Token: 0x06001E8B RID: 7819 RVA: 0x0008FC04 File Offset: 0x0008DE04
		protected Stream InnerStream
		{
			get
			{
				return this._InnerStream;
			}
		}

		// Token: 0x06001E8C RID: 7820 RVA: 0x0008FC0C File Offset: 0x0008DE0C
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (disposing)
				{
					if (this._LeaveStreamOpen)
					{
						this._InnerStream.Flush();
					}
					else
					{
						this._InnerStream.Close();
					}
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x170007F0 RID: 2032
		// (get) Token: 0x06001E8D RID: 7821
		public abstract bool IsAuthenticated { get; }

		// Token: 0x170007F1 RID: 2033
		// (get) Token: 0x06001E8E RID: 7822
		public abstract bool IsMutuallyAuthenticated { get; }

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x06001E8F RID: 7823
		public abstract bool IsEncrypted { get; }

		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x06001E90 RID: 7824
		public abstract bool IsSigned { get; }

		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x06001E91 RID: 7825
		public abstract bool IsServer { get; }

		// Token: 0x04001CE3 RID: 7395
		private Stream _InnerStream;

		// Token: 0x04001CE4 RID: 7396
		private bool _LeaveStreamOpen;
	}
}
