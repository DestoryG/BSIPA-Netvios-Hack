using System;
using System.ComponentModel;

namespace System.Net
{
	// Token: 0x0200018E RID: 398
	[EditorBrowsable(EditorBrowsableState.Never)]
	public class WriteStreamClosedEventArgs : EventArgs
	{
		// Token: 0x06000F53 RID: 3923 RVA: 0x0004F6E7 File Offset: 0x0004D8E7
		[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public WriteStreamClosedEventArgs()
		{
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06000F54 RID: 3924 RVA: 0x0004F6EF File Offset: 0x0004D8EF
		[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public Exception Error
		{
			get
			{
				return null;
			}
		}
	}
}
