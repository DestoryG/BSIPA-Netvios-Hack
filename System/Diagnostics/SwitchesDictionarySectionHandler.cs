using System;
using System.Configuration;

namespace System.Diagnostics
{
	// Token: 0x020004C7 RID: 1223
	internal class SwitchesDictionarySectionHandler : DictionarySectionHandler
	{
		// Token: 0x17000B03 RID: 2819
		// (get) Token: 0x06002D9F RID: 11679 RVA: 0x000CD580 File Offset: 0x000CB780
		protected override string KeyAttributeName
		{
			get
			{
				return "name";
			}
		}

		// Token: 0x17000B04 RID: 2820
		// (get) Token: 0x06002DA0 RID: 11680 RVA: 0x000CD587 File Offset: 0x000CB787
		internal override bool ValueRequired
		{
			get
			{
				return true;
			}
		}
	}
}
