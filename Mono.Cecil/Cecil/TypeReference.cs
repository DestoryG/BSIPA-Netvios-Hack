using System;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020000BF RID: 191
	public class TypeReference : MemberReference, IGenericParameterProvider, IMetadataTokenProvider, IGenericContext
	{
		// Token: 0x1700026E RID: 622
		// (get) Token: 0x0600081A RID: 2074 RVA: 0x000143A9 File Offset: 0x000125A9
		// (set) Token: 0x0600081B RID: 2075 RVA: 0x00018E38 File Offset: 0x00017038
		public override string Name
		{
			get
			{
				return base.Name;
			}
			set
			{
				if (base.IsWindowsRuntimeProjection && value != base.Name)
				{
					throw new InvalidOperationException("Projected type reference name can't be changed.");
				}
				base.Name = value;
				this.ClearFullName();
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x0600081C RID: 2076 RVA: 0x00018E68 File Offset: 0x00017068
		// (set) Token: 0x0600081D RID: 2077 RVA: 0x00018E70 File Offset: 0x00017070
		public virtual string Namespace
		{
			get
			{
				return this.@namespace;
			}
			set
			{
				if (base.IsWindowsRuntimeProjection && value != this.@namespace)
				{
					throw new InvalidOperationException("Projected type reference namespace can't be changed.");
				}
				this.@namespace = value;
				this.ClearFullName();
			}
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x0600081E RID: 2078 RVA: 0x00018EA0 File Offset: 0x000170A0
		// (set) Token: 0x0600081F RID: 2079 RVA: 0x00018EA8 File Offset: 0x000170A8
		public virtual bool IsValueType
		{
			get
			{
				return this.value_type;
			}
			set
			{
				this.value_type = value;
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06000820 RID: 2080 RVA: 0x00018EB4 File Offset: 0x000170B4
		public override ModuleDefinition Module
		{
			get
			{
				if (this.module != null)
				{
					return this.module;
				}
				TypeReference declaringType = this.DeclaringType;
				if (declaringType != null)
				{
					return declaringType.Module;
				}
				return null;
			}
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06000821 RID: 2081 RVA: 0x00018EE2 File Offset: 0x000170E2
		// (set) Token: 0x06000822 RID: 2082 RVA: 0x000112F3 File Offset: 0x0000F4F3
		internal new TypeReferenceProjection WindowsRuntimeProjection
		{
			get
			{
				return (TypeReferenceProjection)this.projection;
			}
			set
			{
				this.projection = value;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06000823 RID: 2083 RVA: 0x00002740 File Offset: 0x00000940
		IGenericParameterProvider IGenericContext.Type
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06000824 RID: 2084 RVA: 0x00011945 File Offset: 0x0000FB45
		IGenericParameterProvider IGenericContext.Method
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06000825 RID: 2085 RVA: 0x000026DB File Offset: 0x000008DB
		GenericParameterType IGenericParameterProvider.GenericParameterType
		{
			get
			{
				return GenericParameterType.Type;
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000826 RID: 2086 RVA: 0x00018EEF File Offset: 0x000170EF
		public virtual bool HasGenericParameters
		{
			get
			{
				return !this.generic_parameters.IsNullOrEmpty<GenericParameter>();
			}
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000827 RID: 2087 RVA: 0x00018F00 File Offset: 0x00017100
		public virtual Collection<GenericParameter> GenericParameters
		{
			get
			{
				if (this.generic_parameters != null)
				{
					return this.generic_parameters;
				}
				return this.generic_parameters = new GenericParameterCollection(this);
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000828 RID: 2088 RVA: 0x00018F2C File Offset: 0x0001712C
		// (set) Token: 0x06000829 RID: 2089 RVA: 0x00018F50 File Offset: 0x00017150
		public virtual IMetadataScope Scope
		{
			get
			{
				TypeReference declaringType = this.DeclaringType;
				if (declaringType != null)
				{
					return declaringType.Scope;
				}
				return this.scope;
			}
			set
			{
				TypeReference declaringType = this.DeclaringType;
				if (declaringType != null)
				{
					if (base.IsWindowsRuntimeProjection && value != declaringType.Scope)
					{
						throw new InvalidOperationException("Projected type scope can't be changed.");
					}
					declaringType.Scope = value;
					return;
				}
				else
				{
					if (base.IsWindowsRuntimeProjection && value != this.scope)
					{
						throw new InvalidOperationException("Projected type scope can't be changed.");
					}
					this.scope = value;
					return;
				}
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x0600082A RID: 2090 RVA: 0x00018FAE File Offset: 0x000171AE
		public bool IsNested
		{
			get
			{
				return this.DeclaringType != null;
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x0600082B RID: 2091 RVA: 0x00018FB9 File Offset: 0x000171B9
		// (set) Token: 0x0600082C RID: 2092 RVA: 0x00018FC1 File Offset: 0x000171C1
		public override TypeReference DeclaringType
		{
			get
			{
				return base.DeclaringType;
			}
			set
			{
				if (base.IsWindowsRuntimeProjection && value != base.DeclaringType)
				{
					throw new InvalidOperationException("Projected type declaring type can't be changed.");
				}
				base.DeclaringType = value;
				this.ClearFullName();
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x0600082D RID: 2093 RVA: 0x00018FEC File Offset: 0x000171EC
		public override string FullName
		{
			get
			{
				if (this.fullname != null)
				{
					return this.fullname;
				}
				this.fullname = this.TypeFullName();
				if (this.IsNested)
				{
					this.fullname = this.DeclaringType.FullName + "/" + this.fullname;
				}
				return this.fullname;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x0600082E RID: 2094 RVA: 0x000026DB File Offset: 0x000008DB
		public virtual bool IsByReference
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x0600082F RID: 2095 RVA: 0x000026DB File Offset: 0x000008DB
		public virtual bool IsPointer
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000830 RID: 2096 RVA: 0x000026DB File Offset: 0x000008DB
		public virtual bool IsSentinel
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000831 RID: 2097 RVA: 0x000026DB File Offset: 0x000008DB
		public virtual bool IsArray
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000832 RID: 2098 RVA: 0x000026DB File Offset: 0x000008DB
		public virtual bool IsGenericParameter
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000833 RID: 2099 RVA: 0x000026DB File Offset: 0x000008DB
		public virtual bool IsGenericInstance
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000834 RID: 2100 RVA: 0x000026DB File Offset: 0x000008DB
		public virtual bool IsRequiredModifier
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000835 RID: 2101 RVA: 0x000026DB File Offset: 0x000008DB
		public virtual bool IsOptionalModifier
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000836 RID: 2102 RVA: 0x000026DB File Offset: 0x000008DB
		public virtual bool IsPinned
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000837 RID: 2103 RVA: 0x000026DB File Offset: 0x000008DB
		public virtual bool IsFunctionPointer
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000838 RID: 2104 RVA: 0x00019043 File Offset: 0x00017243
		public virtual bool IsPrimitive
		{
			get
			{
				return this.etype.IsPrimitive();
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000839 RID: 2105 RVA: 0x00019050 File Offset: 0x00017250
		public virtual MetadataType MetadataType
		{
			get
			{
				if (this.etype != ElementType.None)
				{
					return (MetadataType)this.etype;
				}
				if (!this.IsValueType)
				{
					return MetadataType.Class;
				}
				return MetadataType.ValueType;
			}
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x0001907B File Offset: 0x0001727B
		protected TypeReference(string @namespace, string name)
			: base(name)
		{
			this.@namespace = @namespace ?? string.Empty;
			this.token = new MetadataToken(TokenType.TypeRef, 0);
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x000190A5 File Offset: 0x000172A5
		public TypeReference(string @namespace, string name, ModuleDefinition module, IMetadataScope scope)
			: this(@namespace, name)
		{
			this.module = module;
			this.scope = scope;
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x000190BE File Offset: 0x000172BE
		public TypeReference(string @namespace, string name, ModuleDefinition module, IMetadataScope scope, bool valueType)
			: this(@namespace, name, module, scope)
		{
			this.value_type = valueType;
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x000190D3 File Offset: 0x000172D3
		protected virtual void ClearFullName()
		{
			this.fullname = null;
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x00002740 File Offset: 0x00000940
		public virtual TypeReference GetElementType()
		{
			return this;
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x000190DC File Offset: 0x000172DC
		protected override IMemberDefinition ResolveDefinition()
		{
			return this.Resolve();
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x000190E4 File Offset: 0x000172E4
		public new virtual TypeDefinition Resolve()
		{
			ModuleDefinition moduleDefinition = this.Module;
			if (moduleDefinition == null)
			{
				throw new NotSupportedException();
			}
			return moduleDefinition.Resolve(this);
		}

		// Token: 0x040002CD RID: 717
		private string @namespace;

		// Token: 0x040002CE RID: 718
		private bool value_type;

		// Token: 0x040002CF RID: 719
		internal IMetadataScope scope;

		// Token: 0x040002D0 RID: 720
		internal ModuleDefinition module;

		// Token: 0x040002D1 RID: 721
		internal ElementType etype;

		// Token: 0x040002D2 RID: 722
		private string fullname;

		// Token: 0x040002D3 RID: 723
		protected Collection<GenericParameter> generic_parameters;
	}
}
