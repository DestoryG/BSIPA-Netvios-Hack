using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Google.Protobuf.Reflection
{
	// Token: 0x02000066 RID: 102
	public sealed class DescriptorDeclaration
	{
		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000735 RID: 1845 RVA: 0x0001A24E File Offset: 0x0001844E
		public IDescriptor Descriptor { get; }

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000736 RID: 1846 RVA: 0x0001A256 File Offset: 0x00018456
		public int StartLine { get; }

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000737 RID: 1847 RVA: 0x0001A25E File Offset: 0x0001845E
		public int StartColumn { get; }

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000738 RID: 1848 RVA: 0x0001A266 File Offset: 0x00018466
		public int EndLine { get; }

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000739 RID: 1849 RVA: 0x0001A26E File Offset: 0x0001846E
		public int EndColumn { get; }

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x0600073A RID: 1850 RVA: 0x0001A276 File Offset: 0x00018476
		public string LeadingComments { get; }

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x0600073B RID: 1851 RVA: 0x0001A27E File Offset: 0x0001847E
		public string TrailingComments { get; }

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x0600073C RID: 1852 RVA: 0x0001A286 File Offset: 0x00018486
		public IReadOnlyList<string> LeadingDetachedComments { get; }

		// Token: 0x0600073D RID: 1853 RVA: 0x0001A290 File Offset: 0x00018490
		private DescriptorDeclaration(IDescriptor descriptor, SourceCodeInfo.Types.Location location)
		{
			this.Descriptor = descriptor;
			bool flag = location.Span.Count == 4;
			this.StartLine = location.Span[0] + 1;
			this.StartColumn = location.Span[1] + 1;
			this.EndLine = (flag ? (location.Span[2] + 1) : this.StartLine);
			this.EndColumn = location.Span[flag ? 3 : 2] + 1;
			this.LeadingComments = location.LeadingComments;
			this.TrailingComments = location.TrailingComments;
			this.LeadingDetachedComments = new ReadOnlyCollection<string>(location.LeadingDetachedComments.ToList<string>());
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x0001A348 File Offset: 0x00018548
		internal static DescriptorDeclaration FromProto(IDescriptor descriptor, SourceCodeInfo.Types.Location location)
		{
			return new DescriptorDeclaration(descriptor, location);
		}
	}
}
