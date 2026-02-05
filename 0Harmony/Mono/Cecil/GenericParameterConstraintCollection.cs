using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000118 RID: 280
	internal class GenericParameterConstraintCollection : Collection<GenericParameterConstraint>
	{
		// Token: 0x060007E4 RID: 2020 RVA: 0x00020731 File Offset: 0x0001E931
		internal GenericParameterConstraintCollection(GenericParameter genericParameter)
		{
			this.generic_parameter = genericParameter;
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x00020740 File Offset: 0x0001E940
		internal GenericParameterConstraintCollection(GenericParameter genericParameter, int length)
			: base(length)
		{
			this.generic_parameter = genericParameter;
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x00020750 File Offset: 0x0001E950
		protected override void OnAdd(GenericParameterConstraint item, int index)
		{
			item.generic_parameter = this.generic_parameter;
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x00020750 File Offset: 0x0001E950
		protected override void OnInsert(GenericParameterConstraint item, int index)
		{
			item.generic_parameter = this.generic_parameter;
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x00020750 File Offset: 0x0001E950
		protected override void OnSet(GenericParameterConstraint item, int index)
		{
			item.generic_parameter = this.generic_parameter;
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x0002075E File Offset: 0x0001E95E
		protected override void OnRemove(GenericParameterConstraint item, int index)
		{
			item.generic_parameter = null;
		}

		// Token: 0x040002E8 RID: 744
		private readonly GenericParameter generic_parameter;
	}
}
