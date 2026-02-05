using System;
using System.Collections.Generic;

namespace Google.Protobuf.Reflection
{
	// Token: 0x02000065 RID: 101
	public abstract class DescriptorBase : IDescriptor
	{
		// Token: 0x0600072E RID: 1838 RVA: 0x0001A208 File Offset: 0x00018408
		internal DescriptorBase(FileDescriptor file, string fullName, int index)
		{
			this.File = file;
			this.FullName = fullName;
			this.Index = index;
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x0600072F RID: 1839 RVA: 0x0001A225 File Offset: 0x00018425
		public int Index { get; }

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000730 RID: 1840
		public abstract string Name { get; }

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000731 RID: 1841 RVA: 0x0001A22D File Offset: 0x0001842D
		public string FullName { get; }

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000732 RID: 1842 RVA: 0x0001A235 File Offset: 0x00018435
		public FileDescriptor File { get; }

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000733 RID: 1843 RVA: 0x0001A23D File Offset: 0x0001843D
		public DescriptorDeclaration Declaration
		{
			get
			{
				return this.File.GetDeclaration(this);
			}
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x0001A24B File Offset: 0x0001844B
		internal virtual IReadOnlyList<DescriptorBase> GetNestedDescriptorListForField(int fieldNumber)
		{
			return null;
		}
	}
}
