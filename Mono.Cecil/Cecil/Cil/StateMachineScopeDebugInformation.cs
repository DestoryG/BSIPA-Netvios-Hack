using System;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x02000127 RID: 295
	public sealed class StateMachineScopeDebugInformation : CustomDebugInformation
	{
		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000B2A RID: 2858 RVA: 0x00024164 File Offset: 0x00022364
		public Collection<StateMachineScope> Scopes
		{
			get
			{
				Collection<StateMachineScope> collection;
				if ((collection = this.scopes) == null)
				{
					collection = (this.scopes = new Collection<StateMachineScope>());
				}
				return collection;
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000B2B RID: 2859 RVA: 0x00002BE8 File Offset: 0x00000DE8
		public override CustomDebugInformationKind Kind
		{
			get
			{
				return CustomDebugInformationKind.StateMachineScope;
			}
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x00024189 File Offset: 0x00022389
		public StateMachineScopeDebugInformation()
			: base(StateMachineScopeDebugInformation.KindIdentifier)
		{
		}

		// Token: 0x040006D5 RID: 1749
		internal Collection<StateMachineScope> scopes;

		// Token: 0x040006D6 RID: 1750
		public static Guid KindIdentifier = new Guid("{6DA9A61E-F8C7-4874-BE62-68BC5630DF71}");
	}
}
