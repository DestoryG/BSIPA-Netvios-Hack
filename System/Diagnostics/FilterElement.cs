using System;

namespace System.Diagnostics
{
	// Token: 0x0200049B RID: 1179
	internal class FilterElement : TypedElement
	{
		// Token: 0x06002BBC RID: 11196 RVA: 0x000C5E8C File Offset: 0x000C408C
		public FilterElement()
			: base(typeof(TraceFilter))
		{
		}

		// Token: 0x06002BBD RID: 11197 RVA: 0x000C5EA0 File Offset: 0x000C40A0
		public TraceFilter GetRuntimeObject()
		{
			TraceFilter traceFilter = (TraceFilter)base.BaseGetRuntimeObject();
			traceFilter.initializeData = base.InitData;
			return traceFilter;
		}

		// Token: 0x06002BBE RID: 11198 RVA: 0x000C5EC6 File Offset: 0x000C40C6
		internal TraceFilter RefreshRuntimeObject(TraceFilter filter)
		{
			if (Type.GetType(this.TypeName) != filter.GetType() || base.InitData != filter.initializeData)
			{
				this._runtimeObject = null;
				return this.GetRuntimeObject();
			}
			return filter;
		}
	}
}
