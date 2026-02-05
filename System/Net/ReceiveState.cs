using System;

namespace System.Net
{
	// Token: 0x0200019A RID: 410
	internal class ReceiveState
	{
		// Token: 0x06000FF4 RID: 4084 RVA: 0x000537E2 File Offset: 0x000519E2
		internal ReceiveState(CommandStream connection)
		{
			this.Connection = connection;
			this.Resp = new ResponseDescription();
			this.Buffer = new byte[1024];
			this.ValidThrough = 0;
		}

		// Token: 0x04001306 RID: 4870
		private const int bufferSize = 1024;

		// Token: 0x04001307 RID: 4871
		internal ResponseDescription Resp;

		// Token: 0x04001308 RID: 4872
		internal int ValidThrough;

		// Token: 0x04001309 RID: 4873
		internal byte[] Buffer;

		// Token: 0x0400130A RID: 4874
		internal CommandStream Connection;
	}
}
