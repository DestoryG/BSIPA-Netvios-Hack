using System;

namespace Mono.Cecil
{
	// Token: 0x020000C0 RID: 192
	internal sealed class AssemblyNameDefinition : AssemblyNameReference
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x00013499 File Offset: 0x00011699
		public override byte[] Hash
		{
			get
			{
				return Empty<byte>.Array;
			}
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x000134A0 File Offset: 0x000116A0
		internal AssemblyNameDefinition()
		{
			this.token = new MetadataToken(TokenType.Assembly, 1);
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x000134B9 File Offset: 0x000116B9
		public AssemblyNameDefinition(string name, Version version)
			: base(name, version)
		{
			this.token = new MetadataToken(TokenType.Assembly, 1);
		}
	}
}
