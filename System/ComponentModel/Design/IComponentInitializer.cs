using System;
using System.Collections;

namespace System.ComponentModel.Design
{
	// Token: 0x020005E5 RID: 1509
	public interface IComponentInitializer
	{
		// Token: 0x060037E5 RID: 14309
		void InitializeExistingComponent(IDictionary defaultValues);

		// Token: 0x060037E6 RID: 14310
		void InitializeNewComponent(IDictionary defaultValues);
	}
}
