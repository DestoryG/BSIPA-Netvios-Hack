using System;
using System.Runtime.Serialization;

namespace Mono.Cecil
{
	// Token: 0x02000083 RID: 131
	[Serializable]
	public sealed class ResolutionException : Exception
	{
		// Token: 0x1700011E RID: 286
		// (get) Token: 0x0600050A RID: 1290 RVA: 0x00013445 File Offset: 0x00011645
		public MemberReference Member
		{
			get
			{
				return this.member;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600050B RID: 1291 RVA: 0x00013450 File Offset: 0x00011650
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

		// Token: 0x0600050C RID: 1292 RVA: 0x0001348E File Offset: 0x0001168E
		public ResolutionException(MemberReference member)
			: base("Failed to resolve " + member.FullName)
		{
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			this.member = member;
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x000134BB File Offset: 0x000116BB
		public ResolutionException(MemberReference member, Exception innerException)
			: base("Failed to resolve " + member.FullName, innerException)
		{
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			this.member = member;
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x000134E9 File Offset: 0x000116E9
		private ResolutionException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x040000FE RID: 254
		private readonly MemberReference member;
	}
}
