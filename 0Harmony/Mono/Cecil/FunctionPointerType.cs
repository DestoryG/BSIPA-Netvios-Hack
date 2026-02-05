using System;
using System.Text;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000111 RID: 273
	internal sealed class FunctionPointerType : TypeSpecification, IMethodSignature, IMetadataTokenProvider
	{
		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600077F RID: 1919 RVA: 0x0001FE96 File Offset: 0x0001E096
		// (set) Token: 0x06000780 RID: 1920 RVA: 0x0001FEA3 File Offset: 0x0001E0A3
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

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000781 RID: 1921 RVA: 0x0001FEB1 File Offset: 0x0001E0B1
		// (set) Token: 0x06000782 RID: 1922 RVA: 0x0001FEBE File Offset: 0x0001E0BE
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

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000783 RID: 1923 RVA: 0x0001FECC File Offset: 0x0001E0CC
		// (set) Token: 0x06000784 RID: 1924 RVA: 0x0001FED9 File Offset: 0x0001E0D9
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

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000785 RID: 1925 RVA: 0x0001FEE7 File Offset: 0x0001E0E7
		public bool HasParameters
		{
			get
			{
				return this.function.HasParameters;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000786 RID: 1926 RVA: 0x0001FEF4 File Offset: 0x0001E0F4
		public Collection<ParameterDefinition> Parameters
		{
			get
			{
				return this.function.Parameters;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000787 RID: 1927 RVA: 0x0001FF01 File Offset: 0x0001E101
		// (set) Token: 0x06000788 RID: 1928 RVA: 0x0001FF13 File Offset: 0x0001E113
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

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000789 RID: 1929 RVA: 0x0001FF26 File Offset: 0x0001E126
		public MethodReturnType MethodReturnType
		{
			get
			{
				return this.function.MethodReturnType;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x0600078A RID: 1930 RVA: 0x0001FF33 File Offset: 0x0001E133
		// (set) Token: 0x0600078B RID: 1931 RVA: 0x00010FA6 File Offset: 0x0000F1A6
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

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x0600078C RID: 1932 RVA: 0x0001E9E9 File Offset: 0x0001CBE9
		// (set) Token: 0x0600078D RID: 1933 RVA: 0x00010FA6 File Offset: 0x0000F1A6
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

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600078E RID: 1934 RVA: 0x0001FF40 File Offset: 0x0001E140
		public override ModuleDefinition Module
		{
			get
			{
				return this.ReturnType.Module;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600078F RID: 1935 RVA: 0x0001FF4D File Offset: 0x0001E14D
		// (set) Token: 0x06000790 RID: 1936 RVA: 0x00010FA6 File Offset: 0x0000F1A6
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

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000791 RID: 1937 RVA: 0x00010F39 File Offset: 0x0000F139
		public override bool IsFunctionPointer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000792 RID: 1938 RVA: 0x0001FF5F File Offset: 0x0001E15F
		public override bool ContainsGenericParameter
		{
			get
			{
				return this.function.ContainsGenericParameter;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000793 RID: 1939 RVA: 0x0001FF6C File Offset: 0x0001E16C
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

		// Token: 0x06000794 RID: 1940 RVA: 0x0001FFCD File Offset: 0x0001E1CD
		public FunctionPointerType()
			: base(null)
		{
			this.function = new MethodReference();
			this.function.Name = "method";
			this.etype = Mono.Cecil.Metadata.ElementType.FnPtr;
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x0001FFF9 File Offset: 0x0001E1F9
		public override TypeDefinition Resolve()
		{
			return null;
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x00010978 File Offset: 0x0000EB78
		public override TypeReference GetElementType()
		{
			return this;
		}

		// Token: 0x040002D7 RID: 727
		private readonly MethodReference function;
	}
}
