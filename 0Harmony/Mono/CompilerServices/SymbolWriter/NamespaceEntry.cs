using System;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000218 RID: 536
	internal struct NamespaceEntry
	{
		// Token: 0x06000FEA RID: 4074 RVA: 0x00036AAC File Offset: 0x00034CAC
		public NamespaceEntry(string name, int index, string[] using_clauses, int parent)
		{
			this.Name = name;
			this.Index = index;
			this.Parent = parent;
			this.UsingClauses = ((using_clauses != null) ? using_clauses : new string[0]);
		}

		// Token: 0x06000FEB RID: 4075 RVA: 0x00036AD8 File Offset: 0x00034CD8
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

		// Token: 0x06000FEC RID: 4076 RVA: 0x00036B38 File Offset: 0x00034D38
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

		// Token: 0x06000FED RID: 4077 RVA: 0x00036B97 File Offset: 0x00034D97
		public override string ToString()
		{
			return string.Format("[Namespace {0}:{1}:{2}]", this.Name, this.Index, this.Parent);
		}

		// Token: 0x040009E3 RID: 2531
		public readonly string Name;

		// Token: 0x040009E4 RID: 2532
		public readonly int Index;

		// Token: 0x040009E5 RID: 2533
		public readonly int Parent;

		// Token: 0x040009E6 RID: 2534
		public readonly string[] UsingClauses;
	}
}
