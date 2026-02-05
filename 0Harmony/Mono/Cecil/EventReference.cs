using System;

namespace Mono.Cecil
{
	// Token: 0x0200010A RID: 266
	internal abstract class EventReference : MemberReference
	{
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060006EF RID: 1775 RVA: 0x0001F289 File Offset: 0x0001D489
		// (set) Token: 0x060006F0 RID: 1776 RVA: 0x0001F291 File Offset: 0x0001D491
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

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060006F1 RID: 1777 RVA: 0x0001F29A File Offset: 0x0001D49A
		public override string FullName
		{
			get
			{
				return this.event_type.FullName + " " + base.MemberFullName();
			}
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x0001F2B7 File Offset: 0x0001D4B7
		protected EventReference(string name, TypeReference eventType)
			: base(name)
		{
			Mixin.CheckType(eventType, Mixin.Argument.eventType);
			this.event_type = eventType;
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x0001F2CF File Offset: 0x0001D4CF
		protected override IMemberDefinition ResolveDefinition()
		{
			return this.Resolve();
		}

		// Token: 0x060006F4 RID: 1780
		public new abstract EventDefinition Resolve();

		// Token: 0x040002AD RID: 685
		private TypeReference event_type;
	}
}
