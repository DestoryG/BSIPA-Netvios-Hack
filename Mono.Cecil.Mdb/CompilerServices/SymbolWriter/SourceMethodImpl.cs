using System;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x0200001A RID: 26
	internal class SourceMethodImpl : IMethodDef
	{
		// Token: 0x060000D6 RID: 214 RVA: 0x000058D2 File Offset: 0x00003AD2
		public SourceMethodImpl(string name, int token, int namespaceID)
		{
			this.name = name;
			this.token = token;
			this.namespaceID = namespaceID;
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x000058EF File Offset: 0x00003AEF
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x000058F7 File Offset: 0x00003AF7
		public int NamespaceID
		{
			get
			{
				return this.namespaceID;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x000058FF File Offset: 0x00003AFF
		public int Token
		{
			get
			{
				return this.token;
			}
		}

		// Token: 0x04000099 RID: 153
		private string name;

		// Token: 0x0400009A RID: 154
		private int token;

		// Token: 0x0400009B RID: 155
		private int namespaceID;
	}
}
