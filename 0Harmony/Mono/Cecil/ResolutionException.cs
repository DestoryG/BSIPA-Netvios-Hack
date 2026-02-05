using System;
using System.Runtime.Serialization;

namespace Mono.Cecil
{
	// Token: 0x0200013A RID: 314
	[Serializable]
	internal sealed class ResolutionException : Exception
	{
		// Token: 0x17000138 RID: 312
		// (get) Token: 0x0600089C RID: 2204 RVA: 0x00021C71 File Offset: 0x0001FE71
		public MemberReference Member
		{
			get
			{
				return this.member;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x0600089D RID: 2205 RVA: 0x00021C7C File Offset: 0x0001FE7C
		public IMetadataScope Scope
		{
			get
			{
				TypeReference typeReference = this.member as TypeReference;
				if (typeReference != null)
				{
					return typeReference.Scope;
				}
				TypeReference declaringType = this.member.DeclaringType;
				if (declaringType != null)
				{
					return declaringType.Scope;
				}
				throw new NotSupportedException();
			}
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x00021CBA File Offset: 0x0001FEBA
		public ResolutionException(MemberReference member)
			: base("Failed to resolve " + member.FullName)
		{
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			this.member = member;
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x00021CE7 File Offset: 0x0001FEE7
		public ResolutionException(MemberReference member, Exception innerException)
			: base("Failed to resolve " + member.FullName, innerException)
		{
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			this.member = member;
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x00021D15 File Offset: 0x0001FF15
		private ResolutionException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x04000318 RID: 792
		private readonly MemberReference member;
	}
}
