using System;
using System.Text;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200008B RID: 139
	public class MethodReference : MemberReference, IMethodSignature, IMetadataTokenProvider, IGenericParameterProvider, IGenericContext
	{
		// Token: 0x1700015F RID: 351
		// (get) Token: 0x060005C8 RID: 1480 RVA: 0x00014E8A File Offset: 0x0001308A
		// (set) Token: 0x060005C9 RID: 1481 RVA: 0x00014E92 File Offset: 0x00013092
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

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060005CA RID: 1482 RVA: 0x00014E9B File Offset: 0x0001309B
		// (set) Token: 0x060005CB RID: 1483 RVA: 0x00014EA3 File Offset: 0x000130A3
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

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060005CC RID: 1484 RVA: 0x00014EAC File Offset: 0x000130AC
		// (set) Token: 0x060005CD RID: 1485 RVA: 0x00014EB4 File Offset: 0x000130B4
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

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060005CE RID: 1486 RVA: 0x00014EBD File Offset: 0x000130BD
		public virtual bool HasParameters
		{
			get
			{
				return !this.parameters.IsNullOrEmpty<ParameterDefinition>();
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060005CF RID: 1487 RVA: 0x00014ECD File Offset: 0x000130CD
		public virtual Collection<ParameterDefinition> Parameters
		{
			get
			{
				if (this.parameters == null)
				{
					this.parameters = new ParameterDefinitionCollection(this);
				}
				return this.parameters;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060005D0 RID: 1488 RVA: 0x00014EEC File Offset: 0x000130EC
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

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060005D1 RID: 1489 RVA: 0x00002740 File Offset: 0x00000940
		IGenericParameterProvider IGenericContext.Method
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x060005D2 RID: 1490 RVA: 0x00002BE8 File Offset: 0x00000DE8
		GenericParameterType IGenericParameterProvider.GenericParameterType
		{
			get
			{
				return GenericParameterType.Method;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060005D3 RID: 1491 RVA: 0x00014F12 File Offset: 0x00013112
		public virtual bool HasGenericParameters
		{
			get
			{
				return !this.generic_parameters.IsNullOrEmpty<GenericParameter>();
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060005D4 RID: 1492 RVA: 0x00014F24 File Offset: 0x00013124
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

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x060005D5 RID: 1493 RVA: 0x00014F50 File Offset: 0x00013150
		// (set) Token: 0x060005D6 RID: 1494 RVA: 0x00014F70 File Offset: 0x00013170
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

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x060005D7 RID: 1495 RVA: 0x00014F8E File Offset: 0x0001318E
		// (set) Token: 0x060005D8 RID: 1496 RVA: 0x00014F96 File Offset: 0x00013196
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

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x060005D9 RID: 1497 RVA: 0x00014FA0 File Offset: 0x000131A0
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

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x000026DB File Offset: 0x000008DB
		public virtual bool IsGenericInstance
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x060005DB RID: 1499 RVA: 0x00014FE8 File Offset: 0x000131E8
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

		// Token: 0x060005DC RID: 1500 RVA: 0x00015044 File Offset: 0x00013244
		internal MethodReference()
		{
			this.return_type = new MethodReturnType(this);
			this.token = new MetadataToken(TokenType.MemberRef);
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x00015068 File Offset: 0x00013268
		public MethodReference(string name, TypeReference returnType)
			: base(name)
		{
			Mixin.CheckType(returnType, Mixin.Argument.returnType);
			this.return_type = new MethodReturnType(this);
			this.return_type.ReturnType = returnType;
			this.token = new MetadataToken(TokenType.MemberRef);
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x000150A1 File Offset: 0x000132A1
		public MethodReference(string name, TypeReference returnType, TypeReference declaringType)
			: this(name, returnType)
		{
			Mixin.CheckType(declaringType, Mixin.Argument.declaringType);
			this.DeclaringType = declaringType;
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x00002740 File Offset: 0x00000940
		public virtual MethodReference GetElementMethod()
		{
			return this;
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x000150BA File Offset: 0x000132BA
		protected override IMemberDefinition ResolveDefinition()
		{
			return this.Resolve();
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x000150C2 File Offset: 0x000132C2
		public new virtual MethodDefinition Resolve()
		{
			ModuleDefinition module = this.Module;
			if (module == null)
			{
				throw new NotSupportedException();
			}
			return module.Resolve(this);
		}

		// Token: 0x0400015C RID: 348
		internal ParameterDefinitionCollection parameters;

		// Token: 0x0400015D RID: 349
		private MethodReturnType return_type;

		// Token: 0x0400015E RID: 350
		private bool has_this;

		// Token: 0x0400015F RID: 351
		private bool explicit_this;

		// Token: 0x04000160 RID: 352
		private MethodCallingConvention calling_convention;

		// Token: 0x04000161 RID: 353
		internal Collection<GenericParameter> generic_parameters;
	}
}
