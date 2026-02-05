using System;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x0200000F RID: 15
	public struct ScopeVariable
	{
		// Token: 0x0600004F RID: 79 RVA: 0x00003573 File Offset: 0x00001773
		public ScopeVariable(int scope, int index)
		{
			this.Scope = scope;
			this.Index = index;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003583 File Offset: 0x00001783
		internal ScopeVariable(MyBinaryReader reader)
		{
			this.Scope = reader.ReadLeb128();
			this.Index = reader.ReadLeb128();
		}

		// Token: 0x06000051 RID: 81 RVA: 0x0000359D File Offset: 0x0000179D
		internal void Write(MyBinaryWriter bw)
		{
			bw.WriteLeb128(this.Scope);
			bw.WriteLeb128(this.Index);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x000035B7 File Offset: 0x000017B7
		public override string ToString()
		{
			return string.Format("[ScopeVariable {0}:{1}]", this.Scope, this.Index);
		}

		// Token: 0x04000044 RID: 68
		public readonly int Scope;

		// Token: 0x04000045 RID: 69
		public readonly int Index;
	}
}
