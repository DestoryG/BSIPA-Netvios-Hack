using System;
using System.Runtime.Serialization;

namespace System.Configuration
{
	// Token: 0x020000AF RID: 175
	[Serializable]
	public class SettingsPropertyWrongTypeException : Exception
	{
		// Token: 0x06000609 RID: 1545 RVA: 0x00023CD2 File Offset: 0x00021ED2
		public SettingsPropertyWrongTypeException(string message)
			: base(message)
		{
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00023CDB File Offset: 0x00021EDB
		public SettingsPropertyWrongTypeException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00023CE5 File Offset: 0x00021EE5
		protected SettingsPropertyWrongTypeException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x00023CEF File Offset: 0x00021EEF
		public SettingsPropertyWrongTypeException()
		{
		}
	}
}
