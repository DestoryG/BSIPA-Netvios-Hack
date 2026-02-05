using System;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x0200020B RID: 523
	internal class CodeBlockEntry
	{
		// Token: 0x06000F9B RID: 3995 RVA: 0x00035001 File Offset: 0x00033201
		public CodeBlockEntry(int index, int parent, CodeBlockEntry.Type type, int start_offset)
		{
			this.Index = index;
			this.Parent = parent;
			this.BlockType = type;
			this.StartOffset = start_offset;
		}

		// Token: 0x06000F9C RID: 3996 RVA: 0x00035028 File Offset: 0x00033228
		internal CodeBlockEntry(int index, MyBinaryReader reader)
		{
			this.Index = index;
			int num = reader.ReadLeb128();
			this.BlockType = (CodeBlockEntry.Type)(num & 63);
			this.Parent = reader.ReadLeb128();
			this.StartOffset = reader.ReadLeb128();
			this.EndOffset = reader.ReadLeb128();
			if ((num & 64) != 0)
			{
				int num2 = (int)reader.ReadInt16();
				reader.BaseStream.Position += (long)num2;
			}
		}

		// Token: 0x06000F9D RID: 3997 RVA: 0x00035098 File Offset: 0x00033298
		public void Close(int end_offset)
		{
			this.EndOffset = end_offset;
		}

		// Token: 0x06000F9E RID: 3998 RVA: 0x000350A1 File Offset: 0x000332A1
		internal void Write(MyBinaryWriter bw)
		{
			bw.WriteLeb128((int)this.BlockType);
			bw.WriteLeb128(this.Parent);
			bw.WriteLeb128(this.StartOffset);
			bw.WriteLeb128(this.EndOffset);
		}

		// Token: 0x06000F9F RID: 3999 RVA: 0x000350D4 File Offset: 0x000332D4
		public override string ToString()
		{
			return string.Format("[CodeBlock {0}:{1}:{2}:{3}:{4}]", new object[] { this.Index, this.Parent, this.BlockType, this.StartOffset, this.EndOffset });
		}

		// Token: 0x04000990 RID: 2448
		public int Index;

		// Token: 0x04000991 RID: 2449
		public int Parent;

		// Token: 0x04000992 RID: 2450
		public CodeBlockEntry.Type BlockType;

		// Token: 0x04000993 RID: 2451
		public int StartOffset;

		// Token: 0x04000994 RID: 2452
		public int EndOffset;

		// Token: 0x0200020C RID: 524
		public enum Type
		{
			// Token: 0x04000996 RID: 2454
			Lexical = 1,
			// Token: 0x04000997 RID: 2455
			CompilerGenerated,
			// Token: 0x04000998 RID: 2456
			IteratorBody,
			// Token: 0x04000999 RID: 2457
			IteratorDispatcher
		}
	}
}
