using System;
using System.Collections;
using System.Collections.Specialized;

namespace System.Configuration
{
	// Token: 0x02000097 RID: 151
	internal class ReadOnlyNameValueCollection : NameValueCollection
	{
		// Token: 0x06000592 RID: 1426 RVA: 0x000229D2 File Offset: 0x00020BD2
		internal ReadOnlyNameValueCollection(IEqualityComparer equalityComparer)
			: base(equalityComparer)
		{
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x000229DB File Offset: 0x00020BDB
		internal ReadOnlyNameValueCollection(ReadOnlyNameValueCollection value)
			: base(value)
		{
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x000229E4 File Offset: 0x00020BE4
		internal void SetReadOnly()
		{
			base.IsReadOnly = true;
		}
	}
}
