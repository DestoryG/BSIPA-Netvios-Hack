using System;
using System.Text;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020000A7 RID: 167
	public sealed class PropertyDefinition : PropertyReference, IMemberDefinition, ICustomAttributeProvider, IMetadataTokenProvider, IConstantProvider
	{
		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000735 RID: 1845 RVA: 0x00016E10 File Offset: 0x00015010
		// (set) Token: 0x06000736 RID: 1846 RVA: 0x00016E18 File Offset: 0x00015018
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

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000737 RID: 1847 RVA: 0x00016E24 File Offset: 0x00015024
		// (set) Token: 0x06000738 RID: 1848 RVA: 0x00016E73 File Offset: 0x00015073
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

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000739 RID: 1849 RVA: 0x00016E81 File Offset: 0x00015081
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

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x0600073A RID: 1850 RVA: 0x00016EA6 File Offset: 0x000150A6
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.Module);
			}
		}

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x0600073B RID: 1851 RVA: 0x00016EC4 File Offset: 0x000150C4
		// (set) Token: 0x0600073C RID: 1852 RVA: 0x00016EE1 File Offset: 0x000150E1
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

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x0600073D RID: 1853 RVA: 0x00016EEA File Offset: 0x000150EA
		// (set) Token: 0x0600073E RID: 1854 RVA: 0x00016F07 File Offset: 0x00015107
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

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x0600073F RID: 1855 RVA: 0x00016F10 File Offset: 0x00015110
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

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000740 RID: 1856 RVA: 0x00016F40 File Offset: 0x00015140
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
				return this.other_methods = new Collection<MethodDefinition>();
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000741 RID: 1857 RVA: 0x00016F80 File Offset: 0x00015180
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

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000742 RID: 1858 RVA: 0x00016FD3 File Offset: 0x000151D3
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

		// Token: 0x06000743 RID: 1859 RVA: 0x0001700C File Offset: 0x0001520C
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

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000744 RID: 1860 RVA: 0x00017053 File Offset: 0x00015253
		// (set) Token: 0x06000745 RID: 1861 RVA: 0x00017077 File Offset: 0x00015277
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

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000746 RID: 1862 RVA: 0x00017087 File Offset: 0x00015287
		// (set) Token: 0x06000747 RID: 1863 RVA: 0x00017099 File Offset: 0x00015299
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

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000748 RID: 1864 RVA: 0x000170A2 File Offset: 0x000152A2
		// (set) Token: 0x06000749 RID: 1865 RVA: 0x000170B4 File Offset: 0x000152B4
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

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x0600074A RID: 1866 RVA: 0x000170CD File Offset: 0x000152CD
		// (set) Token: 0x0600074B RID: 1867 RVA: 0x000170DF File Offset: 0x000152DF
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

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x0600074C RID: 1868 RVA: 0x000170F8 File Offset: 0x000152F8
		// (set) Token: 0x0600074D RID: 1869 RVA: 0x0001710A File Offset: 0x0001530A
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

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x0600074E RID: 1870 RVA: 0x00010BA8 File Offset: 0x0000EDA8
		// (set) Token: 0x0600074F RID: 1871 RVA: 0x00010BB5 File Offset: 0x0000EDB5
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

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000750 RID: 1872 RVA: 0x00002BE8 File Offset: 0x00000DE8
		public override bool IsDefinition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000751 RID: 1873 RVA: 0x00017124 File Offset: 0x00015324
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

		// Token: 0x06000752 RID: 1874 RVA: 0x000171BC File Offset: 0x000153BC
		public PropertyDefinition(string name, PropertyAttributes attributes, TypeReference propertyType)
			: base(name, propertyType)
		{
			this.attributes = (ushort)attributes;
			this.token = new MetadataToken(TokenType.Property);
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x000171E8 File Offset: 0x000153E8
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

		// Token: 0x06000754 RID: 1876 RVA: 0x00002740 File Offset: 0x00000940
		public override PropertyDefinition Resolve()
		{
			return this;
		}

		// Token: 0x04000226 RID: 550
		private bool? has_this;

		// Token: 0x04000227 RID: 551
		private ushort attributes;

		// Token: 0x04000228 RID: 552
		private Collection<CustomAttribute> custom_attributes;

		// Token: 0x04000229 RID: 553
		internal MethodDefinition get_method;

		// Token: 0x0400022A RID: 554
		internal MethodDefinition set_method;

		// Token: 0x0400022B RID: 555
		internal Collection<MethodDefinition> other_methods;

		// Token: 0x0400022C RID: 556
		private object constant = Mixin.NotResolved;
	}
}
