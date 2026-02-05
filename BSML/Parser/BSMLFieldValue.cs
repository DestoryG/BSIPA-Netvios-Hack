using System;
using System.Reflection;

namespace BeatSaberMarkupLanguage.Parser
{
	// Token: 0x02000071 RID: 113
	public class BSMLFieldValue : BSMLValue
	{
		// Token: 0x060001E5 RID: 485 RVA: 0x0000C033 File Offset: 0x0000A233
		public BSMLFieldValue(object host, FieldInfo fieldInfo)
		{
			this.host = host;
			this.fieldInfo = fieldInfo;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000C049 File Offset: 0x0000A249
		public override object GetValue()
		{
			return this.fieldInfo.GetValue(this.host);
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000C05C File Offset: 0x0000A25C
		public override void SetValue(object value)
		{
			this.fieldInfo.SetValue(this.host, value);
		}

		// Token: 0x0400004F RID: 79
		private object host;

		// Token: 0x04000050 RID: 80
		private FieldInfo fieldInfo;
	}
}
