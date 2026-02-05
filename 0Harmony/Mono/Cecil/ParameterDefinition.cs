using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000159 RID: 345
	internal sealed class ParameterDefinition : ParameterReference, ICustomAttributeProvider, IMetadataTokenProvider, IConstantProvider, IMarshalInfoProvider
	{
		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000A8E RID: 2702 RVA: 0x000252F5 File Offset: 0x000234F5
		// (set) Token: 0x06000A8F RID: 2703 RVA: 0x000252FD File Offset: 0x000234FD
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

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000A90 RID: 2704 RVA: 0x00025306 File Offset: 0x00023506
		public IMethodSignature Method
		{
			get
			{
				return this.method;
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000A91 RID: 2705 RVA: 0x0002530E File Offset: 0x0002350E
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

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000A92 RID: 2706 RVA: 0x00025336 File Offset: 0x00023536
		// (set) Token: 0x06000A93 RID: 2707 RVA: 0x0002535F File Offset: 0x0002355F
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

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000A94 RID: 2708 RVA: 0x0002536F File Offset: 0x0002356F
		// (set) Token: 0x06000A95 RID: 2709 RVA: 0x00025381 File Offset: 0x00023581
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

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000A96 RID: 2710 RVA: 0x0002538A File Offset: 0x0002358A
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

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000A97 RID: 2711 RVA: 0x000253B4 File Offset: 0x000235B4
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.parameter_type.Module);
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000A98 RID: 2712 RVA: 0x000253D7 File Offset: 0x000235D7
		public bool HasMarshalInfo
		{
			get
			{
				return this.marshal_info != null || this.GetHasMarshalInfo(this.parameter_type.Module);
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000A99 RID: 2713 RVA: 0x000253F4 File Offset: 0x000235F4
		// (set) Token: 0x06000A9A RID: 2714 RVA: 0x00025417 File Offset: 0x00023617
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

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000A9B RID: 2715 RVA: 0x00025420 File Offset: 0x00023620
		// (set) Token: 0x06000A9C RID: 2716 RVA: 0x0002542E File Offset: 0x0002362E
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

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000A9D RID: 2717 RVA: 0x00025443 File Offset: 0x00023643
		// (set) Token: 0x06000A9E RID: 2718 RVA: 0x00025451 File Offset: 0x00023651
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

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000A9F RID: 2719 RVA: 0x00025466 File Offset: 0x00023666
		// (set) Token: 0x06000AA0 RID: 2720 RVA: 0x00025474 File Offset: 0x00023674
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

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000AA1 RID: 2721 RVA: 0x00025489 File Offset: 0x00023689
		// (set) Token: 0x06000AA2 RID: 2722 RVA: 0x00025497 File Offset: 0x00023697
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

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000AA3 RID: 2723 RVA: 0x000254AC File Offset: 0x000236AC
		// (set) Token: 0x06000AA4 RID: 2724 RVA: 0x000254BB File Offset: 0x000236BB
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

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000AA5 RID: 2725 RVA: 0x000254D1 File Offset: 0x000236D1
		// (set) Token: 0x06000AA6 RID: 2726 RVA: 0x000254E3 File Offset: 0x000236E3
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

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06000AA7 RID: 2727 RVA: 0x000254FC File Offset: 0x000236FC
		// (set) Token: 0x06000AA8 RID: 2728 RVA: 0x0002550E File Offset: 0x0002370E
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

		// Token: 0x06000AA9 RID: 2729 RVA: 0x00025527 File Offset: 0x00023727
		internal ParameterDefinition(TypeReference parameterType, IMethodSignature method)
			: this(string.Empty, ParameterAttributes.None, parameterType)
		{
			this.method = method;
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x0002553D File Offset: 0x0002373D
		public ParameterDefinition(TypeReference parameterType)
			: this(string.Empty, ParameterAttributes.None, parameterType)
		{
		}

		// Token: 0x06000AAB RID: 2731 RVA: 0x0002554C File Offset: 0x0002374C
		public ParameterDefinition(string name, ParameterAttributes attributes, TypeReference parameterType)
			: base(name, parameterType)
		{
			this.attributes = (ushort)attributes;
			this.token = new MetadataToken(TokenType.Param);
		}

		// Token: 0x06000AAC RID: 2732 RVA: 0x00010978 File Offset: 0x0000EB78
		public override ParameterDefinition Resolve()
		{
			return this;
		}

		// Token: 0x04000437 RID: 1079
		private ushort attributes;

		// Token: 0x04000438 RID: 1080
		internal IMethodSignature method;

		// Token: 0x04000439 RID: 1081
		private object constant = Mixin.NotResolved;

		// Token: 0x0400043A RID: 1082
		private Collection<CustomAttribute> custom_attributes;

		// Token: 0x0400043B RID: 1083
		private MarshalInfo marshal_info;
	}
}
