using System;

namespace Mono.Cecil
{
	// Token: 0x020000FC RID: 252
	internal sealed class AssemblyResolveEventArgs : EventArgs
	{
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000676 RID: 1654 RVA: 0x0001E0BE File Offset: 0x0001C2BE
		public AssemblyNameReference AssemblyReference
		{
			get
			{
				return this.reference;
			}
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0001E0C6 File Offset: 0x0001C2C6
		public AssemblyResolveEventArgs(AssemblyNameReference reference)
		{
			this.reference = reference;
		}

		// Token: 0x04000287 RID: 647
		private readonly AssemblyNameReference reference;
	}
}
