using System;

namespace Microsoft.CSharp.RuntimeBinder.Semantics
{
	// Token: 0x020000BB RID: 187
	internal sealed class EventWithType : SymWithType
	{
		// Token: 0x0600065A RID: 1626 RVA: 0x0001E07F File Offset: 0x0001C27F
		public EventWithType(EventSymbol @event, AggregateType ats)
		{
			base.Set(@event, ats);
		}
	}
}
