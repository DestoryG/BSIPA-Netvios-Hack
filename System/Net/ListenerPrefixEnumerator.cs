using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Net
{
	// Token: 0x020000FA RID: 250
	internal class ListenerPrefixEnumerator : IEnumerator<string>, IDisposable, IEnumerator
	{
		// Token: 0x060008FE RID: 2302 RVA: 0x00032B1A File Offset: 0x00030D1A
		internal ListenerPrefixEnumerator(IEnumerator enumerator)
		{
			this.enumerator = enumerator;
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x060008FF RID: 2303 RVA: 0x00032B29 File Offset: 0x00030D29
		public string Current
		{
			get
			{
				return (string)this.enumerator.Current;
			}
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x00032B3B File Offset: 0x00030D3B
		public bool MoveNext()
		{
			return this.enumerator.MoveNext();
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x00032B48 File Offset: 0x00030D48
		public void Dispose()
		{
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x00032B4A File Offset: 0x00030D4A
		void IEnumerator.Reset()
		{
			this.enumerator.Reset();
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000903 RID: 2307 RVA: 0x00032B57 File Offset: 0x00030D57
		object IEnumerator.Current
		{
			get
			{
				return this.enumerator.Current;
			}
		}

		// Token: 0x04000E0A RID: 3594
		private IEnumerator enumerator;
	}
}
