using System;

namespace System.ComponentModel.Design
{
	// Token: 0x020005F3 RID: 1523
	public interface IReferenceService
	{
		// Token: 0x06003839 RID: 14393
		IComponent GetComponent(object reference);

		// Token: 0x0600383A RID: 14394
		object GetReference(string name);

		// Token: 0x0600383B RID: 14395
		string GetName(object reference);

		// Token: 0x0600383C RID: 14396
		object[] GetReferences();

		// Token: 0x0600383D RID: 14397
		object[] GetReferences(Type baseType);
	}
}
