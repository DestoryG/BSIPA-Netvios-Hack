using System;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x0200000C RID: 12
	public struct LocalVariableEntry
	{
		// Token: 0x06000043 RID: 67 RVA: 0x000033FF File Offset: 0x000015FF
		public LocalVariableEntry(int index, string name, int block)
		{
			this.Index = index;
			this.Name = name;
			this.BlockIndex = block;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003416 File Offset: 0x00001616
		internal LocalVariableEntry(MonoSymbolFile file, MyBinaryReader reader)
		{
			this.Index = reader.ReadLeb128();
			this.Name = reader.ReadString();
			this.BlockIndex = reader.ReadLeb128();
		}

		// Token: 0x06000045 RID: 69 RVA: 0x0000343C File Offset: 0x0000163C
		internal void Write(MonoSymbolFile file, MyBinaryWriter bw)
		{
			bw.WriteLeb128(this.Index);
			bw.Write(this.Name);
			bw.WriteLeb128(this.BlockIndex);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003462 File Offset: 0x00001662
		public override string ToString()
		{
			return string.Format("[LocalVariable {0}:{1}:{2}]", this.Name, this.Index, this.BlockIndex - 1);
		}

		// Token: 0x0400003C RID: 60
		public readonly int Index;

		// Token: 0x0400003D RID: 61
		public readonly string Name;

		// Token: 0x0400003E RID: 62
		public readonly int BlockIndex;
	}
}
