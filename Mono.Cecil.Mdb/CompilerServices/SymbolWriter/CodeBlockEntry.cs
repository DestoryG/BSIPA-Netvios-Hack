using System;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x0200000B RID: 11
	public class CodeBlockEntry
	{
		// Token: 0x0600003E RID: 62 RVA: 0x000032C9 File Offset: 0x000014C9
		public CodeBlockEntry(int index, int parent, CodeBlockEntry.Type type, int start_offset)
		{
			this.Index = index;
			this.Parent = parent;
			this.BlockType = type;
			this.StartOffset = start_offset;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000032F0 File Offset: 0x000014F0
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

		// Token: 0x06000040 RID: 64 RVA: 0x00003360 File Offset: 0x00001560
		public void Close(int end_offset)
		{
			this.EndOffset = end_offset;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003369 File Offset: 0x00001569
		internal void Write(MyBinaryWriter bw)
		{
			bw.WriteLeb128((int)this.BlockType);
			bw.WriteLeb128(this.Parent);
			bw.WriteLeb128(this.StartOffset);
			bw.WriteLeb128(this.EndOffset);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x0000339C File Offset: 0x0000159C
		public override string ToString()
		{
			return string.Format("[CodeBlock {0}:{1}:{2}:{3}:{4}]", new object[] { this.Index, this.Parent, this.BlockType, this.StartOffset, this.EndOffset });
		}

		// Token: 0x04000037 RID: 55
		public int Index;

		// Token: 0x04000038 RID: 56
		public int Parent;

		// Token: 0x04000039 RID: 57
		public CodeBlockEntry.Type BlockType;

		// Token: 0x0400003A RID: 58
		public int StartOffset;

		// Token: 0x0400003B RID: 59
		public int EndOffset;

		// Token: 0x02000023 RID: 35
		public enum Type
		{
			// Token: 0x040000AA RID: 170
			Lexical = 1,
			// Token: 0x040000AB RID: 171
			CompilerGenerated,
			// Token: 0x040000AC RID: 172
			IteratorBody,
			// Token: 0x040000AD RID: 173
			IteratorDispatcher
		}
	}
}
