using System;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000211 RID: 529
	internal struct ScopeVariable
	{
		// Token: 0x06000FAC RID: 4012 RVA: 0x000352AB File Offset: 0x000334AB
		public ScopeVariable(int scope, int index)
		{
			this.Scope = scope;
			this.Index = index;
		}

		// Token: 0x06000FAD RID: 4013 RVA: 0x000352BB File Offset: 0x000334BB
		internal ScopeVariable(MyBinaryReader reader)
		{
			this.Scope = reader.ReadLeb128();
			this.Index = reader.ReadLeb128();
		}

		// Token: 0x06000FAE RID: 4014 RVA: 0x000352D5 File Offset: 0x000334D5
		internal void Write(MyBinaryWriter bw)
		{
			bw.WriteLeb128(this.Scope);
			bw.WriteLeb128(this.Index);
		}

		// Token: 0x06000FAF RID: 4015 RVA: 0x000352EF File Offset: 0x000334EF
		public override string ToString()
		{
			return string.Format("[ScopeVariable {0}:{1}]", this.Scope, this.Index);
		}

		// Token: 0x040009A6 RID: 2470
		public readonly int Scope;

		// Token: 0x040009A7 RID: 2471
		public readonly int Index;
	}
}
