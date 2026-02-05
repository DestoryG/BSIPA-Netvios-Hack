using System;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000015 RID: 21
	public struct NamespaceEntry
	{
		// Token: 0x0600008D RID: 141 RVA: 0x00004D78 File Offset: 0x00002F78
		public NamespaceEntry(string name, int index, string[] using_clauses, int parent)
		{
			this.Name = name;
			this.Index = index;
			this.Parent = parent;
			this.UsingClauses = ((using_clauses != null) ? using_clauses : new string[0]);
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00004DA4 File Offset: 0x00002FA4
		internal NamespaceEntry(MonoSymbolFile file, MyBinaryReader reader)
		{
			this.Name = reader.ReadString();
			this.Index = reader.ReadLeb128();
			this.Parent = reader.ReadLeb128();
			int num = reader.ReadLeb128();
			this.UsingClauses = new string[num];
			for (int i = 0; i < num; i++)
			{
				this.UsingClauses[i] = reader.ReadString();
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00004E04 File Offset: 0x00003004
		internal void Write(MonoSymbolFile file, MyBinaryWriter bw)
		{
			bw.Write(this.Name);
			bw.WriteLeb128(this.Index);
			bw.WriteLeb128(this.Parent);
			bw.WriteLeb128(this.UsingClauses.Length);
			foreach (string text in this.UsingClauses)
			{
				bw.Write(text);
			}
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00004E63 File Offset: 0x00003063
		public override string ToString()
		{
			return string.Format("[Namespace {0}:{1}:{2}]", this.Name, this.Index, this.Parent);
		}

		// Token: 0x0400007D RID: 125
		public readonly string Name;

		// Token: 0x0400007E RID: 126
		public readonly int Index;

		// Token: 0x0400007F RID: 127
		public readonly int Parent;

		// Token: 0x04000080 RID: 128
		public readonly string[] UsingClauses;
	}
}
