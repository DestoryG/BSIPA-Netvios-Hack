using System;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001DF RID: 479
	internal abstract class DebugInformation : ICustomDebugInformationProvider, IMetadataTokenProvider
	{
		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000ED3 RID: 3795 RVA: 0x00032F36 File Offset: 0x00031136
		// (set) Token: 0x06000ED4 RID: 3796 RVA: 0x00032F3E File Offset: 0x0003113E
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

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000ED5 RID: 3797 RVA: 0x00032F47 File Offset: 0x00031147
		public bool HasCustomDebugInformations
		{
			get
			{
				return !this.custom_infos.IsNullOrEmpty<CustomDebugInformation>();
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000ED6 RID: 3798 RVA: 0x00032F57 File Offset: 0x00031157
		public Collection<CustomDebugInformation> CustomDebugInformations
		{
			get
			{
				if (this.custom_infos == null)
				{
					Interlocked.CompareExchange<Collection<CustomDebugInformation>>(ref this.custom_infos, new Collection<CustomDebugInformation>(), null);
				}
				return this.custom_infos;
			}
		}

		// Token: 0x06000ED7 RID: 3799 RVA: 0x00002AB9 File Offset: 0x00000CB9
		internal DebugInformation()
		{
		}

		// Token: 0x0400090A RID: 2314
		internal MetadataToken token;

		// Token: 0x0400090B RID: 2315
		internal Collection<CustomDebugInformation> custom_infos;
	}
}
