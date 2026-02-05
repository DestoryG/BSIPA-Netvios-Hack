using System;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000117 RID: 279
	internal sealed class GenericParameterConstraint : ICustomAttributeProvider, IMetadataTokenProvider
	{
		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060007DC RID: 2012 RVA: 0x00020645 File Offset: 0x0001E845
		// (set) Token: 0x060007DD RID: 2013 RVA: 0x0002064D File Offset: 0x0001E84D
		public TypeReference ConstraintType
		{
			get
			{
				return this.constraint_type;
			}
			set
			{
				this.constraint_type = value;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060007DE RID: 2014 RVA: 0x00020656 File Offset: 0x0001E856
		public bool HasCustomAttributes
		{
			get
			{
				if (this.custom_attributes != null)
				{
					return this.custom_attributes.Count > 0;
				}
				return this.generic_parameter != null && this.GetHasCustomAttributes(this.generic_parameter.Module);
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060007DF RID: 2015 RVA: 0x0002068C File Offset: 0x0001E88C
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				if (this.generic_parameter == null)
				{
					if (this.custom_attributes == null)
					{
						Interlocked.CompareExchange<Collection<CustomAttribute>>(ref this.custom_attributes, new Collection<CustomAttribute>(), null);
					}
					return this.custom_attributes;
				}
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.generic_parameter.Module);
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060007E0 RID: 2016 RVA: 0x000206E3 File Offset: 0x0001E8E3
		// (set) Token: 0x060007E1 RID: 2017 RVA: 0x000206EB File Offset: 0x0001E8EB
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

		// Token: 0x060007E2 RID: 2018 RVA: 0x000206F4 File Offset: 0x0001E8F4
		internal GenericParameterConstraint(TypeReference constraintType, MetadataToken token)
		{
			this.constraint_type = constraintType;
			this.token = token;
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x0002070A File Offset: 0x0001E90A
		public GenericParameterConstraint(TypeReference constraintType)
		{
			Mixin.CheckType(constraintType, Mixin.Argument.constraintType);
			this.constraint_type = constraintType;
			this.token = new MetadataToken(TokenType.GenericParamConstraint);
		}

		// Token: 0x040002E4 RID: 740
		internal GenericParameter generic_parameter;

		// Token: 0x040002E5 RID: 741
		internal MetadataToken token;

		// Token: 0x040002E6 RID: 742
		private TypeReference constraint_type;

		// Token: 0x040002E7 RID: 743
		private Collection<CustomAttribute> custom_attributes;
	}
}
