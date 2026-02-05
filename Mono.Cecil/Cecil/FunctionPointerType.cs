using System;
using System.Text;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200005E RID: 94
	public sealed class FunctionPointerType : TypeSpecification, IMethodSignature, IMetadataTokenProvider
	{
		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06000401 RID: 1025 RVA: 0x000117E2 File Offset: 0x0000F9E2
		// (set) Token: 0x06000402 RID: 1026 RVA: 0x000117EF File Offset: 0x0000F9EF
		public bool HasThis
		{
			get
			{
				return this.function.HasThis;
			}
			set
			{
				this.function.HasThis = value;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000403 RID: 1027 RVA: 0x000117FD File Offset: 0x0000F9FD
		// (set) Token: 0x06000404 RID: 1028 RVA: 0x0001180A File Offset: 0x0000FA0A
		public bool ExplicitThis
		{
			get
			{
				return this.function.ExplicitThis;
			}
			set
			{
				this.function.ExplicitThis = value;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000405 RID: 1029 RVA: 0x00011818 File Offset: 0x0000FA18
		// (set) Token: 0x06000406 RID: 1030 RVA: 0x00011825 File Offset: 0x0000FA25
		public MethodCallingConvention CallingConvention
		{
			get
			{
				return this.function.CallingConvention;
			}
			set
			{
				this.function.CallingConvention = value;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x00011833 File Offset: 0x0000FA33
		public bool HasParameters
		{
			get
			{
				return this.function.HasParameters;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x00011840 File Offset: 0x0000FA40
		public Collection<ParameterDefinition> Parameters
		{
			get
			{
				return this.function.Parameters;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x0001184D File Offset: 0x0000FA4D
		// (set) Token: 0x0600040A RID: 1034 RVA: 0x0001185F File Offset: 0x0000FA5F
		public TypeReference ReturnType
		{
			get
			{
				return this.function.MethodReturnType.ReturnType;
			}
			set
			{
				this.function.MethodReturnType.ReturnType = value;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x00011872 File Offset: 0x0000FA72
		public MethodReturnType MethodReturnType
		{
			get
			{
				return this.function.MethodReturnType;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600040C RID: 1036 RVA: 0x0001187F File Offset: 0x0000FA7F
		// (set) Token: 0x0600040D RID: 1037 RVA: 0x00002C55 File Offset: 0x00000E55
		public override string Name
		{
			get
			{
				return this.function.Name;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x0600040E RID: 1038 RVA: 0x00010431 File Offset: 0x0000E631
		// (set) Token: 0x0600040F RID: 1039 RVA: 0x00002C55 File Offset: 0x00000E55
		public override string Namespace
		{
			get
			{
				return string.Empty;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000410 RID: 1040 RVA: 0x0001188C File Offset: 0x0000FA8C
		public override ModuleDefinition Module
		{
			get
			{
				return this.ReturnType.Module;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x00011899 File Offset: 0x0000FA99
		// (set) Token: 0x06000412 RID: 1042 RVA: 0x00002C55 File Offset: 0x00000E55
		public override IMetadataScope Scope
		{
			get
			{
				return this.function.ReturnType.Scope;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000413 RID: 1043 RVA: 0x00002BE8 File Offset: 0x00000DE8
		public override bool IsFunctionPointer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x000118AB File Offset: 0x0000FAAB
		public override bool ContainsGenericParameter
		{
			get
			{
				return this.function.ContainsGenericParameter;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x000118B8 File Offset: 0x0000FAB8
		public override string FullName
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(this.function.Name);
				stringBuilder.Append(" ");
				stringBuilder.Append(this.function.ReturnType.FullName);
				stringBuilder.Append(" *");
				this.MethodSignatureFullName(stringBuilder);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00011919 File Offset: 0x0000FB19
		public FunctionPointerType()
			: base(null)
		{
			this.function = new MethodReference();
			this.function.Name = "method";
			this.etype = Mono.Cecil.Metadata.ElementType.FnPtr;
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x00011945 File Offset: 0x0000FB45
		public override TypeDefinition Resolve()
		{
			return null;
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00002740 File Offset: 0x00000940
		public override TypeReference GetElementType()
		{
			return this;
		}

		// Token: 0x040000C8 RID: 200
		private readonly MethodReference function;
	}
}
