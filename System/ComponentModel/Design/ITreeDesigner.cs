using System;
using System.Collections;

namespace System.ComponentModel.Design
{
	// Token: 0x020005F8 RID: 1528
	public interface ITreeDesigner : IDesigner, IDisposable
	{
		// Token: 0x17000D7A RID: 3450
		// (get) Token: 0x06003852 RID: 14418
		ICollection Children { get; }

		// Token: 0x17000D7B RID: 3451
		// (get) Token: 0x06003853 RID: 14419
		IDesigner Parent { get; }
	}
}
