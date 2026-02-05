using System;
using System.Text;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000143 RID: 323
	internal class MethodReference : MemberReference, IMethodSignature, IMetadataTokenProvider, IGenericParameterProvider, IGenericContext
	{
		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000961 RID: 2401 RVA: 0x00023738 File Offset: 0x00021938
		// (set) Token: 0x06000962 RID: 2402 RVA: 0x00023740 File Offset: 0x00021940
		public virtual bool HasThis
		{
			get
			{
				return this.has_this;
			}
			set
			{
				this.has_this = value;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000963 RID: 2403 RVA: 0x00023749 File Offset: 0x00021949
		// (set) Token: 0x06000964 RID: 2404 RVA: 0x00023751 File Offset: 0x00021951
		public virtual bool ExplicitThis
		{
			get
			{
				return this.explicit_this;
			}
			set
			{
				this.explicit_this = value;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000965 RID: 2405 RVA: 0x0002375A File Offset: 0x0002195A
		// (set) Token: 0x06000966 RID: 2406 RVA: 0x00023762 File Offset: 0x00021962
		public virtual MethodCallingConvention CallingConvention
		{
			get
			{
				return this.calling_convention;
			}
			set
			{
				this.calling_convention = value;
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000967 RID: 2407 RVA: 0x0002376B File Offset: 0x0002196B
		public virtual bool HasParameters
		{
			get
			{
				return !this.parameters.IsNullOrEmpty<ParameterDefinition>();
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000968 RID: 2408 RVA: 0x0002377B File Offset: 0x0002197B
		public virtual Collection<ParameterDefinition> Parameters
		{
			get
			{
				if (this.parameters == null)
				{
					Interlocked.CompareExchange<ParameterDefinitionCollection>(ref this.parameters, new ParameterDefinitionCollection(this), null);
				}
				return this.parameters;
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x06000969 RID: 2409 RVA: 0x000237A0 File Offset: 0x000219A0
		IGenericParameterProvider IGenericContext.Type
		{
			get
			{
				TypeReference declaringType = this.DeclaringType;
				GenericInstanceType genericInstanceType = declaringType as GenericInstanceType;
				if (genericInstanceType != null)
				{
					return genericInstanceType.ElementType;
				}
				return declaringType;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x0600096A RID: 2410 RVA: 0x00010978 File Offset: 0x0000EB78
		IGenericParameterProvider IGenericContext.Method
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x0600096B RID: 2411 RVA: 0x00010F39 File Offset: 0x0000F139
		GenericParameterType IGenericParameterProvider.GenericParameterType
		{
			get
			{
				return GenericParameterType.Method;
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x0600096C RID: 2412 RVA: 0x000237C6 File Offset: 0x000219C6
		public virtual bool HasGenericParameters
		{
			get
			{
				return !this.generic_parameters.IsNullOrEmpty<GenericParameter>();
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x0600096D RID: 2413 RVA: 0x000237D6 File Offset: 0x000219D6
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

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x0600096E RID: 2414 RVA: 0x000237FC File Offset: 0x000219FC
		// (set) Token: 0x0600096F RID: 2415 RVA: 0x0002381C File Offset: 0x00021A1C
		public TypeReference ReturnType
		{
			get
			{
				MethodReturnType methodReturnType = this.MethodReturnType;
				if (methodReturnType == null)
				{
					return null;
				}
				return methodReturnType.ReturnType;
			}
			set
			{
				MethodReturnType methodReturnType = this.MethodReturnType;
				if (methodReturnType != null)
				{
					methodReturnType.ReturnType = value;
				}
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000970 RID: 2416 RVA: 0x0002383A File Offset: 0x00021A3A
		// (set) Token: 0x06000971 RID: 2417 RVA: 0x00023842 File Offset: 0x00021A42
		public virtual MethodReturnType MethodReturnType
		{
			get
			{
				return this.return_type;
			}
			set
			{
				this.return_type = value;
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000972 RID: 2418 RVA: 0x0002384C File Offset: 0x00021A4C
		public override string FullName
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(this.ReturnType.FullName).Append(" ").Append(base.MemberFullName());
				this.MethodSignatureFullName(stringBuilder);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000973 RID: 2419 RVA: 0x00010910 File Offset: 0x0000EB10
		public virtual bool IsGenericInstance
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000974 RID: 2420 RVA: 0x00023894 File Offset: 0x00021A94
		public override bool ContainsGenericParameter
		{
			get
			{
				if (this.ReturnType.ContainsGenericParameter || base.ContainsGenericParameter)
				{
					return true;
				}
				if (!this.HasParameters)
				{
					return false;
				}
				Collection<ParameterDefinition> collection = this.Parameters;
				for (int i = 0; i < collection.Count; i++)
				{
					if (collection[i].ParameterType.ContainsGenericParameter)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x000238F0 File Offset: 0x00021AF0
		internal MethodReference()
		{
			this.return_type = new MethodReturnType(this);
			this.token = new MetadataToken(TokenType.MemberRef);
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x00023914 File Offset: 0x00021B14
		public MethodReference(string name, TypeReference returnType)
			: base(name)
		{
			Mixin.CheckType(returnType, Mixin.Argument.returnType);
			this.return_type = new MethodReturnType(this);
			this.return_type.ReturnType = returnType;
			this.token = new MetadataToken(TokenType.MemberRef);
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x0002394D File Offset: 0x00021B4D
		public MethodReference(string name, TypeReference returnType, TypeReference declaringType)
			: this(name, returnType)
		{
			Mixin.CheckType(declaringType, Mixin.Argument.declaringType);
			this.DeclaringType = declaringType;
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x00010978 File Offset: 0x0000EB78
		public virtual MethodReference GetElementMethod()
		{
			return this;
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x00023966 File Offset: 0x00021B66
		protected override IMemberDefinition ResolveDefinition()
		{
			return this.Resolve();
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x0002396E File Offset: 0x00021B6E
		public new virtual MethodDefinition Resolve()
		{
			ModuleDefinition module = this.Module;
			if (module == null)
			{
				throw new NotSupportedException();
			}
			return module.Resolve(this);
		}

		// Token: 0x0400037C RID: 892
		internal ParameterDefinitionCollection parameters;

		// Token: 0x0400037D RID: 893
		private MethodReturnType return_type;

		// Token: 0x0400037E RID: 894
		private bool has_this;

		// Token: 0x0400037F RID: 895
		private bool explicit_this;

		// Token: 0x04000380 RID: 896
		private MethodCallingConvention calling_convention;

		// Token: 0x04000381 RID: 897
		internal Collection<GenericParameter> generic_parameters;
	}
}
