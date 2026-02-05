using System;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x0200011B RID: 283
	public abstract class DebugInformation : ICustomDebugInformationProvider, IMetadataTokenProvider
	{
		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000AEC RID: 2796 RVA: 0x00023D71 File Offset: 0x00021F71
		// (set) Token: 0x06000AED RID: 2797 RVA: 0x00023D79 File Offset: 0x00021F79
		public MetadataToken MetadataToken
		{
			get
			{
				return this.token;
			}
			set
			{
				this.token = value;
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000AEE RID: 2798 RVA: 0x00023D82 File Offset: 0x00021F82
		public bool HasCustomDebugInformations
		{
			get
			{
				return !this.custom_infos.IsNullOrEmpty<CustomDebugInformation>();
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000AEF RID: 2799 RVA: 0x00023D94 File Offset: 0x00021F94
		public Collection<CustomDebugInformation> CustomDebugInformations
		{
			get
			{
				Collection<CustomDebugInformation> collection;
				if ((collection = this.custom_infos) == null)
				{
					collection = (this.custom_infos = new Collection<CustomDebugInformation>());
				}
				return collection;
			}
		}

		// Token: 0x06000AF0 RID: 2800 RVA: 0x00004F37 File Offset: 0x00003137
		internal DebugInformation()
		{
		}

		// Token: 0x040006AB RID: 1707
		internal MetadataToken token;

		// Token: 0x040006AC RID: 1708
		internal Collection<CustomDebugInformation> custom_infos;
	}
}
