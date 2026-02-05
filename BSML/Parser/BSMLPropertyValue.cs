using System;
using System.Reflection;

namespace BeatSaberMarkupLanguage.Parser
{
	// Token: 0x02000073 RID: 115
	public class BSMLPropertyValue : BSMLValue
	{
		// Token: 0x060001EF RID: 495 RVA: 0x0000C274 File Offset: 0x0000A474
		public BSMLPropertyValue(object host, PropertyInfo propertyInfo)
		{
			this.host = host;
			this.propertyInfo = propertyInfo;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000C28A File Offset: 0x0000A48A
		public override object GetValue()
		{
			return this.propertyInfo.GetGetMethod(true).Invoke(this.host, Array.Empty<object>());
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000C2A8 File Offset: 0x0000A4A8
		public override void SetValue(object value)
		{
			this.propertyInfo.GetSetMethod(true).Invoke(this.host, new object[] { value });
		}

		// Token: 0x04000056 RID: 86
		private object host;

		// Token: 0x04000057 RID: 87
		internal PropertyInfo propertyInfo;
	}
}
