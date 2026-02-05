using System;
using System.IO;
using System.Runtime.Serialization;

namespace Mono.Cecil
{
	// Token: 0x0200004D RID: 77
	[Serializable]
	public sealed class AssemblyResolutionException : FileNotFoundException
	{
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000304 RID: 772 RVA: 0x0000FB39 File Offset: 0x0000DD39
		public AssemblyNameReference AssemblyReference
		{
			get
			{
				return this.reference;
			}
		}

		// Token: 0x06000305 RID: 773 RVA: 0x0000FB41 File Offset: 0x0000DD41
		public AssemblyResolutionException(AssemblyNameReference reference)
			: this(reference, null)
		{
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000FB4B File Offset: 0x0000DD4B
		public AssemblyResolutionException(AssemblyNameReference reference, Exception innerException)
			: base(string.Format("Failed to resolve assembly: '{0}'", reference), innerException)
		{
			this.reference = reference;
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000FB66 File Offset: 0x0000DD66
		private AssemblyResolutionException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x04000080 RID: 128
		private readonly AssemblyNameReference reference;
	}
}
