using System;
using System.IO;
using System.Threading;

namespace System.Net
{
	// Token: 0x02000226 RID: 550
	internal class ClosableStream : DelegatedStream
	{
		// Token: 0x0600143E RID: 5182 RVA: 0x0006B67B File Offset: 0x0006987B
		internal ClosableStream(Stream stream, EventHandler onClose)
			: base(stream)
		{
			this.onClose = onClose;
		}

		// Token: 0x0600143F RID: 5183 RVA: 0x0006B68B File Offset: 0x0006988B
		public override void Close()
		{
			if (Interlocked.Increment(ref this.closed) == 1 && this.onClose != null)
			{
				this.onClose(this, new EventArgs());
			}
		}

		// Token: 0x0400161E RID: 5662
		private EventHandler onClose;

		// Token: 0x0400161F RID: 5663
		private int closed;
	}
}
