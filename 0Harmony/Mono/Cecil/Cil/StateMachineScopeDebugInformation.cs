using System;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001EB RID: 491
	internal sealed class StateMachineScopeDebugInformation : CustomDebugInformation
	{
		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000F11 RID: 3857 RVA: 0x00033314 File Offset: 0x00031514
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

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000F12 RID: 3858 RVA: 0x00010F39 File Offset: 0x0000F139
		public override CustomDebugInformationKind Kind
		{
			get
			{
				return CustomDebugInformationKind.StateMachineScope;
			}
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x00033339 File Offset: 0x00031539
		public StateMachineScopeDebugInformation()
			: base(StateMachineScopeDebugInformation.KindIdentifier)
		{
		}

		// Token: 0x04000934 RID: 2356
		internal Collection<StateMachineScope> scopes;

		// Token: 0x04000935 RID: 2357
		public static Guid KindIdentifier = new Guid("{6DA9A61E-F8C7-4874-BE62-68BC5630DF71}");
	}
}
