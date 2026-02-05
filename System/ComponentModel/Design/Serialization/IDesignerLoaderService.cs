using System;
using System.Collections;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x0200060A RID: 1546
	public interface IDesignerLoaderService
	{
		// Token: 0x060038B0 RID: 14512
		void AddLoadDependency();

		// Token: 0x060038B1 RID: 14513
		void DependentLoadComplete(bool successful, ICollection errorCollection);

		// Token: 0x060038B2 RID: 14514
		bool Reload();
	}
}
