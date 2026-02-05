using System;
using System.Configuration;

namespace System.Runtime.Serialization.Configuration
{
	// Token: 0x02000125 RID: 293
	[AttributeUsage(AttributeTargets.Property)]
	internal sealed class DeclaredTypeValidatorAttribute : ConfigurationValidatorAttribute
	{
		// Token: 0x1700037B RID: 891
		// (get) Token: 0x060011CE RID: 4558 RVA: 0x0004ACCD File Offset: 0x00048ECD
		public override ConfigurationValidatorBase ValidatorInstance
		{
			get
			{
				return new DeclaredTypeValidator();
			}
		}
	}
}
