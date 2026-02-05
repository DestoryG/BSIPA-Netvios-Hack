using System;
using System.ComponentModel;
using System.IO;

namespace System.Net
{
	// Token: 0x0200016A RID: 362
	public class OpenReadCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06000DD1 RID: 3537 RVA: 0x00049854 File Offset: 0x00047A54
		internal OpenReadCompletedEventArgs(Stream result, Exception exception, bool cancelled, object userToken)
			: base(exception, cancelled, userToken)
		{
			this.m_Result = result;
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000DD2 RID: 3538 RVA: 0x00049867 File Offset: 0x00047A67
		public Stream Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this.m_Result;
			}
		}

		// Token: 0x04001212 RID: 4626
		private Stream m_Result;
	}
}
