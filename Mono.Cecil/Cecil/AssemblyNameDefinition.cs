using System;

namespace Mono.Cecil
{
	// Token: 0x02000013 RID: 19
	public sealed class AssemblyNameDefinition : AssemblyNameReference
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000DF RID: 223 RVA: 0x000050C5 File Offset: 0x000032C5
		public override byte[] Hash
		{
			get
			{
				return Empty<byte>.Array;
			}
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x000050CC File Offset: 0x000032CC
		internal AssemblyNameDefinition()
		{
			this.token = new MetadataToken(TokenType.Assembly, 1);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000050E5 File Offset: 0x000032E5
		public AssemblyNameDefinition(string name, Version version)
			: base(name, version)
		{
			this.token = new MetadataToken(TokenType.Assembly, 1);
		}
	}
}
