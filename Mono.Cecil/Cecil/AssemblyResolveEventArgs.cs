using System;

namespace Mono.Cecil
{
	// Token: 0x0200004C RID: 76
	public sealed class AssemblyResolveEventArgs : EventArgs
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000302 RID: 770 RVA: 0x0000FB22 File Offset: 0x0000DD22
		public AssemblyNameReference AssemblyReference
		{
			get
			{
				return this.reference;
			}
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000FB2A File Offset: 0x0000DD2A
		public AssemblyResolveEventArgs(AssemblyNameReference reference)
		{
			this.reference = reference;
		}

		// Token: 0x0400007F RID: 127
		private readonly AssemblyNameReference reference;
	}
}
