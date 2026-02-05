using System;
using System.Text;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020000FF RID: 255
	internal sealed class CallSite : IMethodSignature, IMetadataTokenProvider
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000692 RID: 1682 RVA: 0x0001E94C File Offset: 0x0001CB4C
		// (set) Token: 0x06000693 RID: 1683 RVA: 0x0001E959 File Offset: 0x0001CB59
		public bool HasThis
		{
			get
			{
				return this.signature.HasThis;
			}
			set
			{
				this.signature.HasThis = value;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000694 RID: 1684 RVA: 0x0001E967 File Offset: 0x0001CB67
		// (set) Token: 0x06000695 RID: 1685 RVA: 0x0001E974 File Offset: 0x0001CB74
		public bool ExplicitThis
		{
			get
			{
				return this.signature.ExplicitThis;
			}
			set
			{
				this.signature.ExplicitThis = value;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000696 RID: 1686 RVA: 0x0001E982 File Offset: 0x0001CB82
		// (set) Token: 0x06000697 RID: 1687 RVA: 0x0001E98F File Offset: 0x0001CB8F
		public MethodCallingConvention CallingConvention
		{
			get
			{
				return this.signature.CallingConvention;
			}
			set
			{
				this.signature.CallingConvention = value;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000698 RID: 1688 RVA: 0x0001E99D File Offset: 0x0001CB9D
		public bool HasParameters
		{
			get
			{
				return this.signature.HasParameters;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000699 RID: 1689 RVA: 0x0001E9AA File Offset: 0x0001CBAA
		public Collection<ParameterDefinition> Parameters
		{
			get
			{
				return this.signature.Parameters;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600069A RID: 1690 RVA: 0x0001E9B7 File Offset: 0x0001CBB7
		// (set) Token: 0x0600069B RID: 1691 RVA: 0x0001E9C9 File Offset: 0x0001CBC9
		public TypeReference ReturnType
		{
			get
			{
				return this.signature.MethodReturnType.ReturnType;
			}
			set
			{
				this.signature.MethodReturnType.ReturnType = value;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600069C RID: 1692 RVA: 0x0001E9DC File Offset: 0x0001CBDC
		public MethodReturnType MethodReturnType
		{
			get
			{
				return this.signature.MethodReturnType;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600069D RID: 1693 RVA: 0x0001E9E9 File Offset: 0x0001CBE9
		// (set) Token: 0x0600069E RID: 1694 RVA: 0x00010FA6 File Offset: 0x0000F1A6
		public string Name
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

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x0600069F RID: 1695 RVA: 0x0001E9E9 File Offset: 0x0001CBE9
		// (set) Token: 0x060006A0 RID: 1696 RVA: 0x00010FA6 File Offset: 0x0000F1A6
		public string Namespace
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

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060006A1 RID: 1697 RVA: 0x0001E9F0 File Offset: 0x0001CBF0
		public ModuleDefinition Module
		{
			get
			{
				return this.ReturnType.Module;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060006A2 RID: 1698 RVA: 0x0001E9FD File Offset: 0x0001CBFD
		public IMetadataScope Scope
		{
			get
			{
				return this.signature.ReturnType.Scope;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060006A3 RID: 1699 RVA: 0x0001EA0F File Offset: 0x0001CC0F
		// (set) Token: 0x060006A4 RID: 1700 RVA: 0x0001EA1C File Offset: 0x0001CC1C
		public MetadataToken MetadataToken
		{
			get
			{
				return this.signature.token;
			}
			set
			{
				this.signature.token = value;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060006A5 RID: 1701 RVA: 0x0001EA2C File Offset: 0x0001CC2C
		public string FullName
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(this.ReturnType.FullName);
				this.MethodSignatureFullName(stringBuilder);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x0001EA5E File Offset: 0x0001CC5E
		internal CallSite()
		{
			this.signature = new MethodReference();
			this.signature.token = new MetadataToken(TokenType.Signature, 0);
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x0001EA87 File Offset: 0x0001CC87
		public CallSite(TypeReference returnType)
			: this()
		{
			if (returnType == null)
			{
				throw new ArgumentNullException("returnType");
			}
			this.signature.ReturnType = returnType;
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0001EAA9 File Offset: 0x0001CCA9
		public override string ToString()
		{
			return this.FullName;
		}

		// Token: 0x0400028D RID: 653
		private readonly MethodReference signature;
	}
}
