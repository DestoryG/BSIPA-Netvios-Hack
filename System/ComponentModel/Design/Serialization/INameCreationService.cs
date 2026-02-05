using System;

namespace System.ComponentModel.Design.Serialization
{
	// Token: 0x0200060E RID: 1550
	public interface INameCreationService
	{
		// Token: 0x060038C5 RID: 14533
		string CreateName(IContainer container, Type dataType);

		// Token: 0x060038C6 RID: 14534
		bool IsValidName(string name);

		// Token: 0x060038C7 RID: 14535
		void ValidateName(string name);
	}
}
