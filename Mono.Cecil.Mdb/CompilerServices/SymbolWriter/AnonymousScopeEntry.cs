using System;
using System.Collections.Generic;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000010 RID: 16
	public class AnonymousScopeEntry
	{
		// Token: 0x06000053 RID: 83 RVA: 0x000035D9 File Offset: 0x000017D9
		public AnonymousScopeEntry(int id)
		{
			this.ID = id;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003600 File Offset: 0x00001800
		internal AnonymousScopeEntry(MyBinaryReader reader)
		{
			this.ID = reader.ReadLeb128();
			int num = reader.ReadLeb128();
			for (int i = 0; i < num; i++)
			{
				this.captured_vars.Add(new CapturedVariable(reader));
			}
			int num2 = reader.ReadLeb128();
			for (int j = 0; j < num2; j++)
			{
				this.captured_scopes.Add(new CapturedScope(reader));
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x0000367D File Offset: 0x0000187D
		internal void AddCapturedVariable(string name, string captured_name, CapturedVariable.CapturedKind kind)
		{
			this.captured_vars.Add(new CapturedVariable(name, captured_name, kind));
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00003694 File Offset: 0x00001894
		public CapturedVariable[] CapturedVariables
		{
			get
			{
				CapturedVariable[] array = new CapturedVariable[this.captured_vars.Count];
				this.captured_vars.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x000036C0 File Offset: 0x000018C0
		internal void AddCapturedScope(int scope, string captured_name)
		{
			this.captured_scopes.Add(new CapturedScope(scope, captured_name));
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000058 RID: 88 RVA: 0x000036D4 File Offset: 0x000018D4
		public CapturedScope[] CapturedScopes
		{
			get
			{
				CapturedScope[] array = new CapturedScope[this.captured_scopes.Count];
				this.captured_scopes.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003700 File Offset: 0x00001900
		internal void Write(MyBinaryWriter bw)
		{
			bw.WriteLeb128(this.ID);
			bw.WriteLeb128(this.captured_vars.Count);
			foreach (CapturedVariable capturedVariable in this.captured_vars)
			{
				capturedVariable.Write(bw);
			}
			bw.WriteLeb128(this.captured_scopes.Count);
			foreach (CapturedScope capturedScope in this.captured_scopes)
			{
				capturedScope.Write(bw);
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000037C8 File Offset: 0x000019C8
		public override string ToString()
		{
			return string.Format("[AnonymousScope {0}]", this.ID);
		}

		// Token: 0x04000046 RID: 70
		public readonly int ID;

		// Token: 0x04000047 RID: 71
		private List<CapturedVariable> captured_vars = new List<CapturedVariable>();

		// Token: 0x04000048 RID: 72
		private List<CapturedScope> captured_scopes = new List<CapturedScope>();
	}
}
