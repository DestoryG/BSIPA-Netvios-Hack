using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200009F RID: 159
	public sealed class ParameterDefinition : ParameterReference, ICustomAttributeProvider, IMetadataTokenProvider, IConstantProvider, IMarshalInfoProvider
	{
		// Token: 0x170001DA RID: 474
		// (get) Token: 0x060006D7 RID: 1751 RVA: 0x00016722 File Offset: 0x00014922
		// (set) Token: 0x060006D8 RID: 1752 RVA: 0x0001672A File Offset: 0x0001492A
		public ParameterAttributes Attributes
		{
			get
			{
				return (ParameterAttributes)this.attributes;
			}
			set
			{
				this.attributes = (ushort)value;
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x060006D9 RID: 1753 RVA: 0x00016733 File Offset: 0x00014933
		public IMethodSignature Method
		{
			get
			{
				return this.method;
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x060006DA RID: 1754 RVA: 0x0001673B File Offset: 0x0001493B
		public int Sequence
		{
			get
			{
				if (this.method == null)
				{
					return -1;
				}
				if (!this.method.HasImplicitThis())
				{
					return this.index;
				}
				return this.index + 1;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x060006DB RID: 1755 RVA: 0x00016763 File Offset: 0x00014963
		// (set) Token: 0x060006DC RID: 1756 RVA: 0x0001678C File Offset: 0x0001498C
		public bool HasConstant
		{
			get
			{
				this.ResolveConstant(ref this.constant, this.parameter_type.Module);
				return this.constant != Mixin.NoValue;
			}
			set
			{
				if (!value)
				{
					this.constant = Mixin.NoValue;
				}
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x060006DD RID: 1757 RVA: 0x0001679C File Offset: 0x0001499C
		// (set) Token: 0x060006DE RID: 1758 RVA: 0x000167AE File Offset: 0x000149AE
		public object Constant
		{
			get
			{
				if (!this.HasConstant)
				{
					return null;
				}
				return this.constant;
			}
			set
			{
				this.constant = value;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060006DF RID: 1759 RVA: 0x000167B7 File Offset: 0x000149B7
		public bool HasCustomAttributes
		{
			get
			{
				if (this.custom_attributes != null)
				{
					return this.custom_attributes.Count > 0;
				}
				return this.GetHasCustomAttributes(this.parameter_type.Module);
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x060006E0 RID: 1760 RVA: 0x000167E1 File Offset: 0x000149E1
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.parameter_type.Module);
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x060006E1 RID: 1761 RVA: 0x00016804 File Offset: 0x00014A04
		public bool HasMarshalInfo
		{
			get
			{
				return this.marshal_info != null || this.GetHasMarshalInfo(this.parameter_type.Module);
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x060006E2 RID: 1762 RVA: 0x00016821 File Offset: 0x00014A21
		// (set) Token: 0x060006E3 RID: 1763 RVA: 0x00016844 File Offset: 0x00014A44
		public MarshalInfo MarshalInfo
		{
			get
			{
				return this.marshal_info ?? this.GetMarshalInfo(ref this.marshal_info, this.parameter_type.Module);
			}
			set
			{
				this.marshal_info = value;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x0001684D File Offset: 0x00014A4D
		// (set) Token: 0x060006E5 RID: 1765 RVA: 0x0001685B File Offset: 0x00014A5B
		public bool IsIn
		{
			get
			{
				return this.attributes.GetAttributes(1);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(1, value);
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x060006E6 RID: 1766 RVA: 0x00016870 File Offset: 0x00014A70
		// (set) Token: 0x060006E7 RID: 1767 RVA: 0x0001687E File Offset: 0x00014A7E
		public bool IsOut
		{
			get
			{
				return this.attributes.GetAttributes(2);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(2, value);
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x060006E8 RID: 1768 RVA: 0x00016893 File Offset: 0x00014A93
		// (set) Token: 0x060006E9 RID: 1769 RVA: 0x000168A1 File Offset: 0x00014AA1
		public bool IsLcid
		{
			get
			{
				return this.attributes.GetAttributes(4);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(4, value);
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x060006EA RID: 1770 RVA: 0x000168B6 File Offset: 0x00014AB6
		// (set) Token: 0x060006EB RID: 1771 RVA: 0x000168C4 File Offset: 0x00014AC4
		public bool IsReturnValue
		{
			get
			{
				return this.attributes.GetAttributes(8);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(8, value);
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x000168D9 File Offset: 0x00014AD9
		// (set) Token: 0x060006ED RID: 1773 RVA: 0x000168E8 File Offset: 0x00014AE8
		public bool IsOptional
		{
			get
			{
				return this.attributes.GetAttributes(16);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(16, value);
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x060006EE RID: 1774 RVA: 0x000168FE File Offset: 0x00014AFE
		// (set) Token: 0x060006EF RID: 1775 RVA: 0x00016910 File Offset: 0x00014B10
		public bool HasDefault
		{
			get
			{
				return this.attributes.GetAttributes(4096);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(4096, value);
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x060006F0 RID: 1776 RVA: 0x00016929 File Offset: 0x00014B29
		// (set) Token: 0x060006F1 RID: 1777 RVA: 0x0001693B File Offset: 0x00014B3B
		public bool HasFieldMarshal
		{
			get
			{
				return this.attributes.GetAttributes(8192);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(8192, value);
			}
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x00016954 File Offset: 0x00014B54
		internal ParameterDefinition(TypeReference parameterType, IMethodSignature method)
			: this(string.Empty, ParameterAttributes.None, parameterType)
		{
			this.method = method;
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x0001696A File Offset: 0x00014B6A
		public ParameterDefinition(TypeReference parameterType)
			: this(string.Empty, ParameterAttributes.None, parameterType)
		{
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x00016979 File Offset: 0x00014B79
		public ParameterDefinition(string name, ParameterAttributes attributes, TypeReference parameterType)
			: base(name, parameterType)
		{
			this.attributes = (ushort)attributes;
			this.token = new MetadataToken(TokenType.Param);
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x00002740 File Offset: 0x00000940
		public override ParameterDefinition Resolve()
		{
			return this;
		}

		// Token: 0x040001FF RID: 511
		private ushort attributes;

		// Token: 0x04000200 RID: 512
		internal IMethodSignature method;

		// Token: 0x04000201 RID: 513
		private object constant = Mixin.NotResolved;

		// Token: 0x04000202 RID: 514
		private Collection<CustomAttribute> custom_attributes;

		// Token: 0x04000203 RID: 515
		private MarshalInfo marshal_info;
	}
}
