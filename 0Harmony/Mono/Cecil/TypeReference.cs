using System;
using System.Threading;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200017D RID: 381
	internal class TypeReference : MemberReference, IGenericParameterProvider, IMetadataTokenProvider, IGenericContext
	{
		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000BE6 RID: 3046 RVA: 0x00022BE1 File Offset: 0x00020DE1
		// (set) Token: 0x06000BE7 RID: 3047 RVA: 0x00027B54 File Offset: 0x00025D54
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

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000BE8 RID: 3048 RVA: 0x00027B84 File Offset: 0x00025D84
		// (set) Token: 0x06000BE9 RID: 3049 RVA: 0x00027B8C File Offset: 0x00025D8C
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

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000BEA RID: 3050 RVA: 0x00027BBC File Offset: 0x00025DBC
		// (set) Token: 0x06000BEB RID: 3051 RVA: 0x00027BC4 File Offset: 0x00025DC4
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

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000BEC RID: 3052 RVA: 0x00027BD0 File Offset: 0x00025DD0
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

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000BED RID: 3053 RVA: 0x00027BFE File Offset: 0x00025DFE
		// (set) Token: 0x06000BEE RID: 3054 RVA: 0x0001F94A File Offset: 0x0001DB4A
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

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000BEF RID: 3055 RVA: 0x00010978 File Offset: 0x0000EB78
		IGenericParameterProvider IGenericContext.Type
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000BF0 RID: 3056 RVA: 0x0001FFF9 File Offset: 0x0001E1F9
		IGenericParameterProvider IGenericContext.Method
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000BF1 RID: 3057 RVA: 0x00010910 File Offset: 0x0000EB10
		GenericParameterType IGenericParameterProvider.GenericParameterType
		{
			get
			{
				return GenericParameterType.Type;
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000BF2 RID: 3058 RVA: 0x00027C0B File Offset: 0x00025E0B
		public virtual bool HasGenericParameters
		{
			get
			{
				return !this.generic_parameters.IsNullOrEmpty<GenericParameter>();
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000BF3 RID: 3059 RVA: 0x00027C1B File Offset: 0x00025E1B
		public virtual Collection<GenericParameter> GenericParameters
		{
			get
			{
				if (this.generic_parameters == null)
				{
					Interlocked.CompareExchange<Collection<GenericParameter>>(ref this.generic_parameters, new GenericParameterCollection(this), null);
				}
				return this.generic_parameters;
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000BF4 RID: 3060 RVA: 0x00027C40 File Offset: 0x00025E40
		// (set) Token: 0x06000BF5 RID: 3061 RVA: 0x00027C64 File Offset: 0x00025E64
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

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000BF6 RID: 3062 RVA: 0x00027CC2 File Offset: 0x00025EC2
		public bool IsNested
		{
			get
			{
				return this.DeclaringType != null;
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000BF7 RID: 3063 RVA: 0x00027CCD File Offset: 0x00025ECD
		// (set) Token: 0x06000BF8 RID: 3064 RVA: 0x00027CD5 File Offset: 0x00025ED5
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

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000BF9 RID: 3065 RVA: 0x00027D00 File Offset: 0x00025F00
		public override string FullName
		{
			get
			{
				if (this.fullname != null)
				{
					return this.fullname;
				}
				string text = this.TypeFullName();
				if (this.IsNested)
				{
					text = this.DeclaringType.FullName + "/" + text;
				}
				Interlocked.CompareExchange<string>(ref this.fullname, text, null);
				return this.fullname;
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000BFA RID: 3066 RVA: 0x00010910 File Offset: 0x0000EB10
		public virtual bool IsByReference
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000BFB RID: 3067 RVA: 0x00010910 File Offset: 0x0000EB10
		public virtual bool IsPointer
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000BFC RID: 3068 RVA: 0x00010910 File Offset: 0x0000EB10
		public virtual bool IsSentinel
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000BFD RID: 3069 RVA: 0x00010910 File Offset: 0x0000EB10
		public virtual bool IsArray
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000BFE RID: 3070 RVA: 0x00010910 File Offset: 0x0000EB10
		public virtual bool IsGenericParameter
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000BFF RID: 3071 RVA: 0x00010910 File Offset: 0x0000EB10
		public virtual bool IsGenericInstance
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000C00 RID: 3072 RVA: 0x00010910 File Offset: 0x0000EB10
		public virtual bool IsRequiredModifier
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000C01 RID: 3073 RVA: 0x00010910 File Offset: 0x0000EB10
		public virtual bool IsOptionalModifier
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000C02 RID: 3074 RVA: 0x00010910 File Offset: 0x0000EB10
		public virtual bool IsPinned
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000C03 RID: 3075 RVA: 0x00010910 File Offset: 0x0000EB10
		public virtual bool IsFunctionPointer
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000C04 RID: 3076 RVA: 0x00027D56 File Offset: 0x00025F56
		public virtual bool IsPrimitive
		{
			get
			{
				return this.etype.IsPrimitive();
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000C05 RID: 3077 RVA: 0x00027D63 File Offset: 0x00025F63
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

		// Token: 0x06000C06 RID: 3078 RVA: 0x00027D81 File Offset: 0x00025F81
		protected TypeReference(string @namespace, string name)
			: base(name)
		{
			this.@namespace = @namespace ?? string.Empty;
			this.token = new MetadataToken(TokenType.TypeRef, 0);
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x00027DAB File Offset: 0x00025FAB
		public TypeReference(string @namespace, string name, ModuleDefinition module, IMetadataScope scope)
			: this(@namespace, name)
		{
			this.module = module;
			this.scope = scope;
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x00027DC4 File Offset: 0x00025FC4
		public TypeReference(string @namespace, string name, ModuleDefinition module, IMetadataScope scope, bool valueType)
			: this(@namespace, name, module, scope)
		{
			this.value_type = valueType;
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x00027DD9 File Offset: 0x00025FD9
		protected virtual void ClearFullName()
		{
			this.fullname = null;
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x00010978 File Offset: 0x0000EB78
		public virtual TypeReference GetElementType()
		{
			return this;
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x00027DE2 File Offset: 0x00025FE2
		protected override IMemberDefinition ResolveDefinition()
		{
			return this.Resolve();
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x00027DEA File Offset: 0x00025FEA
		public new virtual TypeDefinition Resolve()
		{
			ModuleDefinition moduleDefinition = this.Module;
			if (moduleDefinition == null)
			{
				throw new NotSupportedException();
			}
			return moduleDefinition.Resolve(this);
		}

		// Token: 0x0400051F RID: 1311
		private string @namespace;

		// Token: 0x04000520 RID: 1312
		private bool value_type;

		// Token: 0x04000521 RID: 1313
		internal IMetadataScope scope;

		// Token: 0x04000522 RID: 1314
		internal ModuleDefinition module;

		// Token: 0x04000523 RID: 1315
		internal ElementType etype;

		// Token: 0x04000524 RID: 1316
		private string fullname;

		// Token: 0x04000525 RID: 1317
		protected Collection<GenericParameter> generic_parameters;
	}
}
