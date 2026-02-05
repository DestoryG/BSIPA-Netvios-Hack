using System;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x0200021D RID: 541
	internal class SourceMethodImpl : IMethodDef
	{
		// Token: 0x06001033 RID: 4147 RVA: 0x00037602 File Offset: 0x00035802
		public SourceMethodImpl(string name, int token, int namespaceID)
		{
			this.name = name;
			this.token = token;
			this.namespaceID = namespaceID;
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06001034 RID: 4148 RVA: 0x0003761F File Offset: 0x0003581F
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06001035 RID: 4149 RVA: 0x00037627 File Offset: 0x00035827
		public int NamespaceID
		{
			get
			{
				return this.namespaceID;
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06001036 RID: 4150 RVA: 0x0003762F File Offset: 0x0003582F
		public int Token
		{
			get
			{
				return this.token;
			}
		}

		// Token: 0x040009FF RID: 2559
		private string name;

		// Token: 0x04000A00 RID: 2560
		private int token;

		// Token: 0x04000A01 RID: 2561
		private int namespaceID;
	}
}
