using System;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x0200000E RID: 14
	public struct CapturedScope
	{
		// Token: 0x0600004B RID: 75 RVA: 0x00003512 File Offset: 0x00001712
		public CapturedScope(int scope, string captured_name)
		{
			this.Scope = scope;
			this.CapturedName = captured_name;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003522 File Offset: 0x00001722
		internal CapturedScope(MyBinaryReader reader)
		{
			this.Scope = reader.ReadLeb128();
			this.CapturedName = reader.ReadString();
		}

		// Token: 0x0600004D RID: 77 RVA: 0x0000353C File Offset: 0x0000173C
		internal void Write(MyBinaryWriter bw)
		{
			bw.WriteLeb128(this.Scope);
			bw.Write(this.CapturedName);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003556 File Offset: 0x00001756
		public override string ToString()
		{
			return string.Format("[CapturedScope {0}:{1}]", this.Scope, this.CapturedName);
		}

		// Token: 0x04000042 RID: 66
		public readonly int Scope;

		// Token: 0x04000043 RID: 67
		public readonly string CapturedName;
	}
}
