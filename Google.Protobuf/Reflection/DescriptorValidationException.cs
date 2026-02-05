using System;

namespace Google.Protobuf.Reflection
{
	// Token: 0x02000069 RID: 105
	public sealed class DescriptorValidationException : Exception
	{
		// Token: 0x170001DB RID: 475
		// (get) Token: 0x0600074C RID: 1868 RVA: 0x0001A966 File Offset: 0x00018B66
		public string ProblemSymbolName
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x0600074D RID: 1869 RVA: 0x0001A96E File Offset: 0x00018B6E
		public string Description
		{
			get
			{
				return this.description;
			}
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x0001A976 File Offset: 0x00018B76
		internal DescriptorValidationException(IDescriptor problemDescriptor, string description)
			: base(problemDescriptor.FullName + ": " + description)
		{
			this.name = problemDescriptor.FullName;
			this.description = description;
		}

		// Token: 0x0600074F RID: 1871 RVA: 0x0001A9A2 File Offset: 0x00018BA2
		internal DescriptorValidationException(IDescriptor problemDescriptor, string description, Exception cause)
			: base(problemDescriptor.FullName + ": " + description, cause)
		{
			this.name = problemDescriptor.FullName;
			this.description = description;
		}

		// Token: 0x040002D9 RID: 729
		private readonly string name;

		// Token: 0x040002DA RID: 730
		private readonly string description;
	}
}
