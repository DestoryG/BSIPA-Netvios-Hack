using System;
using System.Runtime.Serialization;

namespace System.Configuration
{
	// Token: 0x020000AB RID: 171
	[Serializable]
	public class SettingsPropertyIsReadOnlyException : Exception
	{
		// Token: 0x060005E3 RID: 1507 RVA: 0x00023357 File Offset: 0x00021557
		public SettingsPropertyIsReadOnlyException(string message)
			: base(message)
		{
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x00023360 File Offset: 0x00021560
		public SettingsPropertyIsReadOnlyException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0002336A File Offset: 0x0002156A
		protected SettingsPropertyIsReadOnlyException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00023374 File Offset: 0x00021574
		public SettingsPropertyIsReadOnlyException()
		{
		}
	}
}
