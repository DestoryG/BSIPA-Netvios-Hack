using System;
using System.Runtime.Serialization;

namespace System.Configuration
{
	// Token: 0x020000AC RID: 172
	[Serializable]
	public class SettingsPropertyNotFoundException : Exception
	{
		// Token: 0x060005E7 RID: 1511 RVA: 0x0002337C File Offset: 0x0002157C
		public SettingsPropertyNotFoundException(string message)
			: base(message)
		{
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00023385 File Offset: 0x00021585
		public SettingsPropertyNotFoundException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x0002338F File Offset: 0x0002158F
		protected SettingsPropertyNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x00023399 File Offset: 0x00021599
		public SettingsPropertyNotFoundException()
		{
		}
	}
}
