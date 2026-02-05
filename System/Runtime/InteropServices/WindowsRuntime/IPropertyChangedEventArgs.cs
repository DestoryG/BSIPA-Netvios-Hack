using System;

namespace System.Runtime.InteropServices.WindowsRuntime
{
	// Token: 0x020003E9 RID: 1001
	[Guid("4f33a9a0-5cf4-47a4-b16f-d7faaf17457e")]
	[ComImport]
	internal interface IPropertyChangedEventArgs
	{
		// Token: 0x17000979 RID: 2425
		// (get) Token: 0x0600261A RID: 9754
		string PropertyName { get; }
	}
}
