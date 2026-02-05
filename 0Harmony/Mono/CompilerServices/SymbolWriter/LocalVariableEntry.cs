using System;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x0200020D RID: 525
	internal struct LocalVariableEntry
	{
		// Token: 0x06000FA0 RID: 4000 RVA: 0x00035137 File Offset: 0x00033337
		public LocalVariableEntry(int index, string name, int block)
		{
			this.Index = index;
			this.Name = name;
			this.BlockIndex = block;
		}

		// Token: 0x06000FA1 RID: 4001 RVA: 0x0003514E File Offset: 0x0003334E
		internal LocalVariableEntry(MonoSymbolFile file, MyBinaryReader reader)
		{
			this.Index = reader.ReadLeb128();
			this.Name = reader.ReadString();
			this.BlockIndex = reader.ReadLeb128();
		}

		// Token: 0x06000FA2 RID: 4002 RVA: 0x00035174 File Offset: 0x00033374
		internal void Write(MonoSymbolFile file, MyBinaryWriter bw)
		{
			bw.WriteLeb128(this.Index);
			bw.Write(this.Name);
			bw.WriteLeb128(this.BlockIndex);
		}

		// Token: 0x06000FA3 RID: 4003 RVA: 0x0003519A File Offset: 0x0003339A
		public override string ToString()
		{
			return string.Format("[LocalVariable {0}:{1}:{2}]", this.Name, this.Index, this.BlockIndex - 1);
		}

		// Token: 0x0400099A RID: 2458
		public readonly int Index;

		// Token: 0x0400099B RID: 2459
		public readonly string Name;

		// Token: 0x0400099C RID: 2460
		public readonly int BlockIndex;
	}
}
