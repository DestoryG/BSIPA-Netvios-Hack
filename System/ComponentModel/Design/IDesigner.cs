using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	// Token: 0x020005E6 RID: 1510
	[ComVisible(true)]
	public interface IDesigner : IDisposable
	{
		// Token: 0x17000D6B RID: 3435
		// (get) Token: 0x060037E7 RID: 14311
		IComponent Component { get; }

		// Token: 0x17000D6C RID: 3436
		// (get) Token: 0x060037E8 RID: 14312
		DesignerVerbCollection Verbs { get; }

		// Token: 0x060037E9 RID: 14313
		void DoDefaultAction();

		// Token: 0x060037EA RID: 14314
		void Initialize(IComponent component);
	}
}
