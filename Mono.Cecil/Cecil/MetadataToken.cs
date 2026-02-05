using System;

namespace Mono.Cecil
{
	// Token: 0x020000CA RID: 202
	public struct MetadataToken : IEquatable<MetadataToken>
	{
		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000892 RID: 2194 RVA: 0x0001AD01 File Offset: 0x00018F01
		public uint RID
		{
			get
			{
				return this.token & 16777215U;
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000893 RID: 2195 RVA: 0x0001AD0F File Offset: 0x00018F0F
		public TokenType TokenType
		{
			get
			{
				return (TokenType)(this.token & 4278190080U);
			}
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x0001AD1D File Offset: 0x00018F1D
		public MetadataToken(uint token)
		{
			this.token = token;
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x0001AD26 File Offset: 0x00018F26
		public MetadataToken(TokenType type)
		{
			this = new MetadataToken(type, 0);
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x0001AD30 File Offset: 0x00018F30
		public MetadataToken(TokenType type, uint rid)
		{
			this.token = (uint)(type | (TokenType)rid);
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x0001AD30 File Offset: 0x00018F30
		public MetadataToken(TokenType type, int rid)
		{
			this.token = (uint)(type | (TokenType)rid);
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x0001AD3B File Offset: 0x00018F3B
		public int ToInt32()
		{
			return (int)this.token;
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x0001AD3B File Offset: 0x00018F3B
		public uint ToUInt32()
		{
			return this.token;
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x0001AD3B File Offset: 0x00018F3B
		public override int GetHashCode()
		{
			return (int)this.token;
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x0001AD43 File Offset: 0x00018F43
		public bool Equals(MetadataToken other)
		{
			return other.token == this.token;
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x0001AD53 File Offset: 0x00018F53
		public override bool Equals(object obj)
		{
			return obj is MetadataToken && ((MetadataToken)obj).token == this.token;
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x0001AD72 File Offset: 0x00018F72
		public static bool operator ==(MetadataToken one, MetadataToken other)
		{
			return one.token == other.token;
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x0001AD82 File Offset: 0x00018F82
		public static bool operator !=(MetadataToken one, MetadataToken other)
		{
			return one.token != other.token;
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x0001AD98 File Offset: 0x00018F98
		public override string ToString()
		{
			return string.Format("[{0}:0x{1}]", this.TokenType, this.RID.ToString("x4"));
		}

		// Token: 0x04000315 RID: 789
		private readonly uint token;

		// Token: 0x04000316 RID: 790
		public static readonly MetadataToken Zero = new MetadataToken(0U);
	}
}
