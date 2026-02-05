using System;
using System.Configuration;

namespace System.Runtime.Serialization.Configuration
{
	// Token: 0x02000124 RID: 292
	internal class DeclaredTypeValidator : ConfigurationValidatorBase
	{
		// Token: 0x060011CB RID: 4555 RVA: 0x0004AC5B File Offset: 0x00048E5B
		public override bool CanValidate(Type type)
		{
			return typeof(string) == type;
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x0004AC70 File Offset: 0x00048E70
		public override void Validate(object value)
		{
			string text = (string)value;
			if (text.StartsWith(Globals.TypeOfObject.FullName, StringComparison.Ordinal))
			{
				Type type = Type.GetType(text, false);
				if (type != null && Globals.TypeOfObject.Equals(type))
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperArgument(SR.GetString("Known type configuration specifies System.Object."));
				}
			}
		}
	}
}
