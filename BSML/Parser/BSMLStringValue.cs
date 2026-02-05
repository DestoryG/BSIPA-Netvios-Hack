using System;

namespace BeatSaberMarkupLanguage.Parser
{
	// Token: 0x02000074 RID: 116
	public class BSMLStringValue : BSMLValue
	{
		// Token: 0x060001F2 RID: 498 RVA: 0x0000C2CC File Offset: 0x0000A4CC
		public BSMLStringValue(string value)
		{
			this.value = value;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000C2DB File Offset: 0x0000A4DB
		public override object GetValue()
		{
			return this.value;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000C2E3 File Offset: 0x0000A4E3
		public override void SetValue(object value)
		{
			this.value = value.ToString();
		}

		// Token: 0x04000058 RID: 88
		private string value;
	}
}
