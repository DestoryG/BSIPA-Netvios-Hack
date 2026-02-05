using System;
using System.IO;
using System.Runtime.Serialization;

namespace Mono.Cecil
{
	// Token: 0x020000FD RID: 253
	[Serializable]
	internal sealed class AssemblyResolutionException : FileNotFoundException
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000678 RID: 1656 RVA: 0x0001E0D5 File Offset: 0x0001C2D5
		public AssemblyNameReference AssemblyReference
		{
			get
			{
				return this.reference;
			}
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x0001E0DD File Offset: 0x0001C2DD
		public AssemblyResolutionException(AssemblyNameReference reference)
			: this(reference, null)
		{
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x0001E0E7 File Offset: 0x0001C2E7
		public AssemblyResolutionException(AssemblyNameReference reference, Exception innerException)
			: base(string.Format("Failed to resolve assembly: '{0}'", reference), innerException)
		{
			this.reference = reference;
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x0001E102 File Offset: 0x0001C302
		private AssemblyResolutionException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x04000288 RID: 648
		private readonly AssemblyNameReference reference;
	}
}
