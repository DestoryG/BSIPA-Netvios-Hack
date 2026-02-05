using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000163 RID: 355
	internal abstract class PropertyReference : MemberReference
	{
		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000B0F RID: 2831 RVA: 0x00025E51 File Offset: 0x00024051
		// (set) Token: 0x06000B10 RID: 2832 RVA: 0x00025E59 File Offset: 0x00024059
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

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000B11 RID: 2833
		public abstract Collection<ParameterDefinition> Parameters { get; }

		// Token: 0x06000B12 RID: 2834 RVA: 0x00025E62 File Offset: 0x00024062
		internal PropertyReference(string name, TypeReference propertyType)
			: base(name)
		{
			Mixin.CheckType(propertyType, Mixin.Argument.propertyType);
			this.property_type = propertyType;
		}

		// Token: 0x06000B13 RID: 2835 RVA: 0x00025E7A File Offset: 0x0002407A
		protected override IMemberDefinition ResolveDefinition()
		{
			return this.Resolve();
		}

		// Token: 0x06000B14 RID: 2836
		public new abstract PropertyDefinition Resolve();

		// Token: 0x04000467 RID: 1127
		private TypeReference property_type;
	}
}
