using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000573 RID: 1395
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[Serializable]
	public class InvalidEnumArgumentException : ArgumentException
	{
		// Token: 0x060033D2 RID: 13266 RVA: 0x000E3EBE File Offset: 0x000E20BE
		public InvalidEnumArgumentException()
			: this(null)
		{
		}

		// Token: 0x060033D3 RID: 13267 RVA: 0x000E3EC7 File Offset: 0x000E20C7
		public InvalidEnumArgumentException(string message)
			: base(message)
		{
		}

		// Token: 0x060033D4 RID: 13268 RVA: 0x000E3ED0 File Offset: 0x000E20D0
		public InvalidEnumArgumentException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x060033D5 RID: 13269 RVA: 0x000E3EDA File Offset: 0x000E20DA
		public InvalidEnumArgumentException(string argumentName, int invalidValue, Type enumClass)
			: base(SR.GetString("InvalidEnumArgument", new object[]
			{
				argumentName,
				invalidValue.ToString(CultureInfo.CurrentCulture),
				enumClass.Name
			}), argumentName)
		{
		}

		// Token: 0x060033D6 RID: 13270 RVA: 0x000E3F0F File Offset: 0x000E210F
		protected InvalidEnumArgumentException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
