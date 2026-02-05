using System;
using System.Collections.Generic;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000212 RID: 530
	internal class AnonymousScopeEntry
	{
		// Token: 0x06000FB0 RID: 4016 RVA: 0x00035311 File Offset: 0x00033511
		public AnonymousScopeEntry(int id)
		{
			this.ID = id;
		}

		// Token: 0x06000FB1 RID: 4017 RVA: 0x00035338 File Offset: 0x00033538
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

		// Token: 0x06000FB2 RID: 4018 RVA: 0x000353B5 File Offset: 0x000335B5
		internal void AddCapturedVariable(string name, string captured_name, CapturedVariable.CapturedKind kind)
		{
			this.captured_vars.Add(new CapturedVariable(name, captured_name, kind));
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06000FB3 RID: 4019 RVA: 0x000353CC File Offset: 0x000335CC
		public CapturedVariable[] CapturedVariables
		{
			get
			{
				CapturedVariable[] array = new CapturedVariable[this.captured_vars.Count];
				this.captured_vars.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x06000FB4 RID: 4020 RVA: 0x000353F8 File Offset: 0x000335F8
		internal void AddCapturedScope(int scope, string captured_name)
		{
			this.captured_scopes.Add(new CapturedScope(scope, captured_name));
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06000FB5 RID: 4021 RVA: 0x0003540C File Offset: 0x0003360C
		public CapturedScope[] CapturedScopes
		{
			get
			{
				CapturedScope[] array = new CapturedScope[this.captured_scopes.Count];
				this.captured_scopes.CopyTo(array, 0);
				return array;
			}
		}

		// Token: 0x06000FB6 RID: 4022 RVA: 0x00035438 File Offset: 0x00033638
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

		// Token: 0x06000FB7 RID: 4023 RVA: 0x00035500 File Offset: 0x00033700
		public override string ToString()
		{
			return string.Format("[AnonymousScope {0}]", this.ID);
		}

		// Token: 0x040009A8 RID: 2472
		public readonly int ID;

		// Token: 0x040009A9 RID: 2473
		private List<CapturedVariable> captured_vars = new List<CapturedVariable>();

		// Token: 0x040009AA RID: 2474
		private List<CapturedScope> captured_scopes = new List<CapturedScope>();
	}
}
