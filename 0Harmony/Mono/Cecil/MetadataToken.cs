using System;

namespace Mono.Cecil
{
	// Token: 0x0200018C RID: 396
	internal struct MetadataToken : IEquatable<MetadataToken>
	{
		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000C6C RID: 3180 RVA: 0x00029CB0 File Offset: 0x00027EB0
		public uint RID
		{
			get
			{
				return this.token & 16777215U;
			}
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000C6D RID: 3181 RVA: 0x00029CBE File Offset: 0x00027EBE
		public TokenType TokenType
		{
			get
			{
				return (TokenType)(this.token & 4278190080U);
			}
		}

		// Token: 0x06000C6E RID: 3182 RVA: 0x00029CCC File Offset: 0x00027ECC
		public MetadataToken(uint token)
		{
			this.token = token;
		}

		// Token: 0x06000C6F RID: 3183 RVA: 0x00029CD5 File Offset: 0x00027ED5
		public MetadataToken(TokenType type)
		{
			this = new MetadataToken(type, 0);
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x00029CDF File Offset: 0x00027EDF
		public MetadataToken(TokenType type, uint rid)
		{
			this.token = (uint)(type | (TokenType)rid);
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x00029CDF File Offset: 0x00027EDF
		public MetadataToken(TokenType type, int rid)
		{
			this.token = (uint)(type | (TokenType)rid);
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x00029CEA File Offset: 0x00027EEA
		public int ToInt32()
		{
			return (int)this.token;
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x00029CEA File Offset: 0x00027EEA
		public uint ToUInt32()
		{
			return this.token;
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x00029CEA File Offset: 0x00027EEA
		public override int GetHashCode()
		{
			return (int)this.token;
		}

		// Token: 0x06000C75 RID: 3189 RVA: 0x00029CF2 File Offset: 0x00027EF2
		public bool Equals(MetadataToken other)
		{
			return other.token == this.token;
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x00029D02 File Offset: 0x00027F02
		public override bool Equals(object obj)
		{
			return obj is MetadataToken && ((MetadataToken)obj).token == this.token;
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x00029D21 File Offset: 0x00027F21
		public static bool operator ==(MetadataToken one, MetadataToken other)
		{
			return one.token == other.token;
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x00029D31 File Offset: 0x00027F31
		public static bool operator !=(MetadataToken one, MetadataToken other)
		{
			return one.token != other.token;
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x00029D44 File Offset: 0x00027F44
		public override string ToString()
		{
			return string.Format("[{0}:0x{1}]", this.TokenType, this.RID.ToString("x4"));
		}

		// Token: 0x04000572 RID: 1394
		private readonly uint token;

		// Token: 0x04000573 RID: 1395
		public static readonly MetadataToken Zero = new MetadataToken(0U);
	}
}
