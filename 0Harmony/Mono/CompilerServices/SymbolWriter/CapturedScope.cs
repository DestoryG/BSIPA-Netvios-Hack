using System;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x02000210 RID: 528
	internal struct CapturedScope
	{
		// Token: 0x06000FA8 RID: 4008 RVA: 0x0003524A File Offset: 0x0003344A
		public CapturedScope(int scope, string captured_name)
		{
			this.Scope = scope;
			this.CapturedName = captured_name;
		}

		// Token: 0x06000FA9 RID: 4009 RVA: 0x0003525A File Offset: 0x0003345A
		internal CapturedScope(MyBinaryReader reader)
		{
			this.Scope = reader.ReadLeb128();
			this.CapturedName = reader.ReadString();
		}

		// Token: 0x06000FAA RID: 4010 RVA: 0x00035274 File Offset: 0x00033474
		internal void Write(MyBinaryWriter bw)
		{
			bw.WriteLeb128(this.Scope);
			bw.Write(this.CapturedName);
		}

		// Token: 0x06000FAB RID: 4011 RVA: 0x0003528E File Offset: 0x0003348E
		public override string ToString()
		{
			return string.Format("[CapturedScope {0}:{1}]", this.Scope, this.CapturedName);
		}

		// Token: 0x040009A4 RID: 2468
		public readonly int Scope;

		// Token: 0x040009A5 RID: 2469
		public readonly string CapturedName;
	}
}
