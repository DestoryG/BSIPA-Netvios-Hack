using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020000A8 RID: 168
	public abstract class PropertyReference : MemberReference
	{
		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000755 RID: 1877 RVA: 0x00017270 File Offset: 0x00015470
		// (set) Token: 0x06000756 RID: 1878 RVA: 0x00017278 File Offset: 0x00015478
		public TypeReference PropertyType
		{
			get
			{
				return this.property_type;
			}
			set
			{
				this.property_type = value;
			}
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000757 RID: 1879
		public abstract Collection<ParameterDefinition> Parameters { get; }

		// Token: 0x06000758 RID: 1880 RVA: 0x00017281 File Offset: 0x00015481
		internal PropertyReference(string name, TypeReference propertyType)
			: base(name)
		{
			Mixin.CheckType(propertyType, Mixin.Argument.propertyType);
			this.property_type = propertyType;
		}

		// Token: 0x06000759 RID: 1881 RVA: 0x00017299 File Offset: 0x00015499
		protected override IMemberDefinition ResolveDefinition()
		{
			return this.Resolve();
		}

		// Token: 0x0600075A RID: 1882
		public new abstract PropertyDefinition Resolve();

		// Token: 0x0400022D RID: 557
		private TypeReference property_type;
	}
}
