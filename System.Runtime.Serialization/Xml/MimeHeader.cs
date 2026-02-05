using System;
using System.Runtime.Serialization;

namespace System.Xml
{
	// Token: 0x02000041 RID: 65
	internal class MimeHeader
	{
		// Token: 0x06000515 RID: 1301 RVA: 0x00018874 File Offset: 0x00016A74
		public MimeHeader(string name, string value)
		{
			if (name == null)
			{
				throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgumentNull("name");
			}
			this.name = name;
			this.value = value;
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000516 RID: 1302 RVA: 0x00018898 File Offset: 0x00016A98
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x000188A0 File Offset: 0x00016AA0
		public string Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x0400021C RID: 540
		private string name;

		// Token: 0x0400021D RID: 541
		private string value;
	}
}
