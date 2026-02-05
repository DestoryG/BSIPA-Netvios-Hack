using System;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x0200000D RID: 13
	public struct CapturedVariable
	{
		// Token: 0x06000047 RID: 71 RVA: 0x0000348C File Offset: 0x0000168C
		public CapturedVariable(string name, string captured_name, CapturedVariable.CapturedKind kind)
		{
			this.Name = name;
			this.CapturedName = captured_name;
			this.Kind = kind;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000034A3 File Offset: 0x000016A3
		internal CapturedVariable(MyBinaryReader reader)
		{
			this.Name = reader.ReadString();
			this.CapturedName = reader.ReadString();
			this.Kind = (CapturedVariable.CapturedKind)reader.ReadByte();
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000034C9 File Offset: 0x000016C9
		internal void Write(MyBinaryWriter bw)
		{
			bw.Write(this.Name);
			bw.Write(this.CapturedName);
			bw.Write((byte)this.Kind);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000034EF File Offset: 0x000016EF
		public override string ToString()
		{
			return string.Format("[CapturedVariable {0}:{1}:{2}]", this.Name, this.CapturedName, this.Kind);
		}

		// Token: 0x0400003F RID: 63
		public readonly string Name;

		// Token: 0x04000040 RID: 64
		public readonly string CapturedName;

		// Token: 0x04000041 RID: 65
		public readonly CapturedVariable.CapturedKind Kind;

		// Token: 0x02000024 RID: 36
		public enum CapturedKind : byte
		{
			// Token: 0x040000AF RID: 175
			Local,
			// Token: 0x040000B0 RID: 176
			Parameter,
			// Token: 0x040000B1 RID: 177
			This
		}
	}
}
