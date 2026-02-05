using System;

namespace Mono.CompilerServices.SymbolWriter
{
	// Token: 0x0200020E RID: 526
	internal struct CapturedVariable
	{
		// Token: 0x06000FA4 RID: 4004 RVA: 0x000351C4 File Offset: 0x000333C4
		public CapturedVariable(string name, string captured_name, CapturedVariable.CapturedKind kind)
		{
			this.Name = name;
			this.CapturedName = captured_name;
			this.Kind = kind;
		}

		// Token: 0x06000FA5 RID: 4005 RVA: 0x000351DB File Offset: 0x000333DB
		internal CapturedVariable(MyBinaryReader reader)
		{
			this.Name = reader.ReadString();
			this.CapturedName = reader.ReadString();
			this.Kind = (CapturedVariable.CapturedKind)reader.ReadByte();
		}

		// Token: 0x06000FA6 RID: 4006 RVA: 0x00035201 File Offset: 0x00033401
		internal void Write(MyBinaryWriter bw)
		{
			bw.Write(this.Name);
			bw.Write(this.CapturedName);
			bw.Write((byte)this.Kind);
		}

		// Token: 0x06000FA7 RID: 4007 RVA: 0x00035227 File Offset: 0x00033427
		public override string ToString()
		{
			return string.Format("[CapturedVariable {0}:{1}:{2}]", this.Name, this.CapturedName, this.Kind);
		}

		// Token: 0x0400099D RID: 2461
		public readonly string Name;

		// Token: 0x0400099E RID: 2462
		public readonly string CapturedName;

		// Token: 0x0400099F RID: 2463
		public readonly CapturedVariable.CapturedKind Kind;

		// Token: 0x0200020F RID: 527
		public enum CapturedKind : byte
		{
			// Token: 0x040009A1 RID: 2465
			Local,
			// Token: 0x040009A2 RID: 2466
			Parameter,
			// Token: 0x040009A3 RID: 2467
			This
		}
	}
}
