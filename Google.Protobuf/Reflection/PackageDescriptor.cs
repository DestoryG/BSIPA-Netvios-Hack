using System;

namespace Google.Protobuf.Reflection
{
	// Token: 0x0200007C RID: 124
	internal sealed class PackageDescriptor : IDescriptor
	{
		// Token: 0x06000812 RID: 2066 RVA: 0x0001CC27 File Offset: 0x0001AE27
		internal PackageDescriptor(string name, string fullName, FileDescriptor file)
		{
			this.file = file;
			this.fullName = fullName;
			this.name = name;
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000813 RID: 2067 RVA: 0x0001CC44 File Offset: 0x0001AE44
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000814 RID: 2068 RVA: 0x0001CC4C File Offset: 0x0001AE4C
		public string FullName
		{
			get
			{
				return this.fullName;
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000815 RID: 2069 RVA: 0x0001CC54 File Offset: 0x0001AE54
		public FileDescriptor File
		{
			get
			{
				return this.file;
			}
		}

		// Token: 0x0400033D RID: 829
		private readonly string name;

		// Token: 0x0400033E RID: 830
		private readonly string fullName;

		// Token: 0x0400033F RID: 831
		private readonly FileDescriptor file;
	}
}
