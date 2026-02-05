using System;

namespace System.Diagnostics
{
	// Token: 0x020004C5 RID: 1221
	public class DataReceivedEventArgs : EventArgs
	{
		// Token: 0x06002D96 RID: 11670 RVA: 0x000CCEFD File Offset: 0x000CB0FD
		internal DataReceivedEventArgs(string data)
		{
			this._data = data;
		}

		// Token: 0x17000B02 RID: 2818
		// (get) Token: 0x06002D97 RID: 11671 RVA: 0x000CCF0C File Offset: 0x000CB10C
		public string Data
		{
			get
			{
				return this._data;
			}
		}

		// Token: 0x04002727 RID: 10023
		internal string _data;
	}
}
