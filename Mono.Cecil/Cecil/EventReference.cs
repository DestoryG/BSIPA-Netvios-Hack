using System;

namespace Mono.Cecil
{
	// Token: 0x02000058 RID: 88
	public abstract class EventReference : MemberReference
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000375 RID: 885 RVA: 0x00010C70 File Offset: 0x0000EE70
		// (set) Token: 0x06000376 RID: 886 RVA: 0x00010C78 File Offset: 0x0000EE78
		public TypeReference EventType
		{
			get
			{
				return this.event_type;
			}
			set
			{
				this.event_type = value;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000377 RID: 887 RVA: 0x00010C81 File Offset: 0x0000EE81
		public override string FullName
		{
			get
			{
				return this.event_type.FullName + " " + base.MemberFullName();
			}
		}

		// Token: 0x06000378 RID: 888 RVA: 0x00010C9E File Offset: 0x0000EE9E
		protected EventReference(string name, TypeReference eventType)
			: base(name)
		{
			Mixin.CheckType(eventType, Mixin.Argument.eventType);
			this.event_type = eventType;
		}

		// Token: 0x06000379 RID: 889 RVA: 0x00010CB6 File Offset: 0x0000EEB6
		protected override IMemberDefinition ResolveDefinition()
		{
			return this.Resolve();
		}

		// Token: 0x0600037A RID: 890
		public new abstract EventDefinition Resolve();

		// Token: 0x040000A1 RID: 161
		private TypeReference event_type;
	}
}
