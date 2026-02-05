using System;
using System.ComponentModel;

namespace System.Net
{
	// Token: 0x02000172 RID: 370
	public class UploadStringCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06000DE9 RID: 3561 RVA: 0x000498D8 File Offset: 0x00047AD8
		internal UploadStringCompletedEventArgs(string result, Exception exception, bool cancelled, object userToken)
			: base(exception, cancelled, userToken)
		{
			this.m_Result = result;
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000DEA RID: 3562 RVA: 0x000498EB File Offset: 0x00047AEB
		public string Result
		{
			get
			{
				base.RaiseExceptionIfNecessary();
				return this.m_Result;
			}
		}

		// Token: 0x04001216 RID: 4630
		private string m_Result;
	}
}
