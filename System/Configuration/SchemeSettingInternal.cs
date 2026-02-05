using System;

namespace System.Configuration
{
	// Token: 0x02000071 RID: 113
	internal sealed class SchemeSettingInternal
	{
		// Token: 0x06000496 RID: 1174 RVA: 0x0001F4EB File Offset: 0x0001D6EB
		public SchemeSettingInternal(string name, GenericUriParserOptions options)
		{
			this.name = name.ToLowerInvariant();
			this.options = options;
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000497 RID: 1175 RVA: 0x0001F506 File Offset: 0x0001D706
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000498 RID: 1176 RVA: 0x0001F50E File Offset: 0x0001D70E
		public GenericUriParserOptions Options
		{
			get
			{
				return this.options;
			}
		}

		// Token: 0x04000BE7 RID: 3047
		private string name;

		// Token: 0x04000BE8 RID: 3048
		private GenericUriParserOptions options;
	}
}
