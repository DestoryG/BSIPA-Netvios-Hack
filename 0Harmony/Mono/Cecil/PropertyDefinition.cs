using System;
using System.Text;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000161 RID: 353
	internal sealed class PropertyDefinition : PropertyReference, IMemberDefinition, ICustomAttributeProvider, IMetadataTokenProvider, IConstantProvider
	{
		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000AEC RID: 2796 RVA: 0x000259E0 File Offset: 0x00023BE0
		// (set) Token: 0x06000AED RID: 2797 RVA: 0x000259E8 File Offset: 0x00023BE8
		public PropertyAttributes Attributes
		{
			get
			{
				return (PropertyAttributes)this.attributes;
			}
			set
			{
				this.attributes = (ushort)value;
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000AEE RID: 2798 RVA: 0x000259F4 File Offset: 0x00023BF4
		// (set) Token: 0x06000AEF RID: 2799 RVA: 0x00025A43 File Offset: 0x00023C43
		public bool HasThis
		{
			get
			{
				if (this.has_this != null)
				{
					return this.has_this.Value;
				}
				if (this.GetMethod != null)
				{
					return this.get_method.HasThis;
				}
				return this.SetMethod != null && this.set_method.HasThis;
			}
			set
			{
				this.has_this = new bool?(value);
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000AF0 RID: 2800 RVA: 0x00025A51 File Offset: 0x00023C51
		public bool HasCustomAttributes
		{
			get
			{
				if (this.custom_attributes != null)
				{
					return this.custom_attributes.Count > 0;
				}
				return this.GetHasCustomAttributes(this.Module);
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000AF1 RID: 2801 RVA: 0x00025A76 File Offset: 0x00023C76
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.Module);
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000AF2 RID: 2802 RVA: 0x00025A94 File Offset: 0x00023C94
		// (set) Token: 0x06000AF3 RID: 2803 RVA: 0x00025AB1 File Offset: 0x00023CB1
		public MethodDefinition GetMethod
		{
			get
			{
				if (this.get_method != null)
				{
					return this.get_method;
				}
				this.InitializeMethods();
				return this.get_method;
			}
			set
			{
				this.get_method = value;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000AF4 RID: 2804 RVA: 0x00025ABA File Offset: 0x00023CBA
		// (set) Token: 0x06000AF5 RID: 2805 RVA: 0x00025AD7 File Offset: 0x00023CD7
		public MethodDefinition SetMethod
		{
			get
			{
				if (this.set_method != null)
				{
					return this.set_method;
				}
				this.InitializeMethods();
				return this.set_method;
			}
			set
			{
				this.set_method = value;
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000AF6 RID: 2806 RVA: 0x00025AE0 File Offset: 0x00023CE0
		public bool HasOtherMethods
		{
			get
			{
				if (this.other_methods != null)
				{
					return this.other_methods.Count > 0;
				}
				this.InitializeMethods();
				return !this.other_methods.IsNullOrEmpty<MethodDefinition>();
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000AF7 RID: 2807 RVA: 0x00025B0D File Offset: 0x00023D0D
		public Collection<MethodDefinition> OtherMethods
		{
			get
			{
				if (this.other_methods != null)
				{
					return this.other_methods;
				}
				this.InitializeMethods();
				if (this.other_methods != null)
				{
					return this.other_methods;
				}
				Interlocked.CompareExchange<Collection<MethodDefinition>>(ref this.other_methods, new Collection<MethodDefinition>(), null);
				return this.other_methods;
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000AF8 RID: 2808 RVA: 0x00025B4C File Offset: 0x00023D4C
		public bool HasParameters
		{
			get
			{
				this.InitializeMethods();
				if (this.get_method != null)
				{
					return this.get_method.HasParameters;
				}
				return this.set_method != null && this.set_method.HasParameters && this.set_method.Parameters.Count > 1;
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000AF9 RID: 2809 RVA: 0x00025B9F File Offset: 0x00023D9F
		public override Collection<ParameterDefinition> Parameters
		{
			get
			{
				this.InitializeMethods();
				if (this.get_method != null)
				{
					return PropertyDefinition.MirrorParameters(this.get_method, 0);
				}
				if (this.set_method != null)
				{
					return PropertyDefinition.MirrorParameters(this.set_method, 1);
				}
				return new Collection<ParameterDefinition>();
			}
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x00025BD8 File Offset: 0x00023DD8
		private static Collection<ParameterDefinition> MirrorParameters(MethodDefinition method, int bound)
		{
			Collection<ParameterDefinition> collection = new Collection<ParameterDefinition>();
			if (!method.HasParameters)
			{
				return collection;
			}
			Collection<ParameterDefinition> parameters = method.Parameters;
			int num = parameters.Count - bound;
			for (int i = 0; i < num; i++)
			{
				collection.Add(parameters[i]);
			}
			return collection;
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000AFB RID: 2811 RVA: 0x00025C1F File Offset: 0x00023E1F
		// (set) Token: 0x06000AFC RID: 2812 RVA: 0x00025C43 File Offset: 0x00023E43
		public bool HasConstant
		{
			get
			{
				this.ResolveConstant(ref this.constant, this.Module);
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

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000AFD RID: 2813 RVA: 0x00025C53 File Offset: 0x00023E53
		// (set) Token: 0x06000AFE RID: 2814 RVA: 0x00025C65 File Offset: 0x00023E65
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

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000AFF RID: 2815 RVA: 0x00025C6E File Offset: 0x00023E6E
		// (set) Token: 0x06000B00 RID: 2816 RVA: 0x00025C80 File Offset: 0x00023E80
		public bool IsSpecialName
		{
			get
			{
				return this.attributes.GetAttributes(512);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(512, value);
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000B01 RID: 2817 RVA: 0x00025C99 File Offset: 0x00023E99
		// (set) Token: 0x06000B02 RID: 2818 RVA: 0x00025CAB File Offset: 0x00023EAB
		public bool IsRuntimeSpecialName
		{
			get
			{
				return this.attributes.GetAttributes(1024);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(1024, value);
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000B03 RID: 2819 RVA: 0x00025CC4 File Offset: 0x00023EC4
		// (set) Token: 0x06000B04 RID: 2820 RVA: 0x00025CD6 File Offset: 0x00023ED6
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

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000B05 RID: 2821 RVA: 0x0001F1AC File Offset: 0x0001D3AC
		// (set) Token: 0x06000B06 RID: 2822 RVA: 0x0001F1B9 File Offset: 0x0001D3B9
		public new TypeDefinition DeclaringType
		{
			get
			{
				return (TypeDefinition)base.DeclaringType;
			}
			set
			{
				base.DeclaringType = value;
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000B07 RID: 2823 RVA: 0x00010F39 File Offset: 0x0000F139
		public override bool IsDefinition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000B08 RID: 2824 RVA: 0x00025CF0 File Offset: 0x00023EF0
		public override string FullName
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.PropertyType.ToString());
				stringBuilder.Append(' ');
				stringBuilder.Append(base.MemberFullName());
				stringBuilder.Append('(');
				if (this.HasParameters)
				{
					Collection<ParameterDefinition> parameters = this.Parameters;
					for (int i = 0; i < parameters.Count; i++)
					{
						if (i > 0)
						{
							stringBuilder.Append(',');
						}
						stringBuilder.Append(parameters[i].ParameterType.FullName);
					}
				}
				stringBuilder.Append(')');
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x00025D88 File Offset: 0x00023F88
		public PropertyDefinition(string name, PropertyAttributes attributes, TypeReference propertyType)
			: base(name, propertyType)
		{
			this.attributes = (ushort)attributes;
			this.token = new MetadataToken(TokenType.Property);
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x00025DB4 File Offset: 0x00023FB4
		private void InitializeMethods()
		{
			ModuleDefinition module = this.Module;
			if (module == null)
			{
				return;
			}
			object syncRoot = module.SyncRoot;
			lock (syncRoot)
			{
				if (this.get_method == null && this.set_method == null)
				{
					if (module.HasImage())
					{
						module.Read<PropertyDefinition>(this, delegate(PropertyDefinition property, MetadataReader reader)
						{
							reader.ReadMethods(property);
						});
					}
				}
			}
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x00010978 File Offset: 0x0000EB78
		public override PropertyDefinition Resolve()
		{
			return this;
		}

		// Token: 0x0400045E RID: 1118
		private bool? has_this;

		// Token: 0x0400045F RID: 1119
		private ushort attributes;

		// Token: 0x04000460 RID: 1120
		private Collection<CustomAttribute> custom_attributes;

		// Token: 0x04000461 RID: 1121
		internal MethodDefinition get_method;

		// Token: 0x04000462 RID: 1122
		internal MethodDefinition set_method;

		// Token: 0x04000463 RID: 1123
		internal Collection<MethodDefinition> other_methods;

		// Token: 0x04000464 RID: 1124
		private object constant = Mixin.NotResolved;
	}
}
