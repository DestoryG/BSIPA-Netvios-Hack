using System;
using System.Text;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200004F RID: 79
	public sealed class CallSite : IMethodSignature, IMetadataTokenProvider
	{
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600031E RID: 798 RVA: 0x00010394 File Offset: 0x0000E594
		// (set) Token: 0x0600031F RID: 799 RVA: 0x000103A1 File Offset: 0x0000E5A1
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

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000320 RID: 800 RVA: 0x000103AF File Offset: 0x0000E5AF
		// (set) Token: 0x06000321 RID: 801 RVA: 0x000103BC File Offset: 0x0000E5BC
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

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000322 RID: 802 RVA: 0x000103CA File Offset: 0x0000E5CA
		// (set) Token: 0x06000323 RID: 803 RVA: 0x000103D7 File Offset: 0x0000E5D7
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

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000324 RID: 804 RVA: 0x000103E5 File Offset: 0x0000E5E5
		public bool HasParameters
		{
			get
			{
				return this.signature.HasParameters;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000325 RID: 805 RVA: 0x000103F2 File Offset: 0x0000E5F2
		public Collection<ParameterDefinition> Parameters
		{
			get
			{
				return this.signature.Parameters;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000326 RID: 806 RVA: 0x000103FF File Offset: 0x0000E5FF
		// (set) Token: 0x06000327 RID: 807 RVA: 0x00010411 File Offset: 0x0000E611
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

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000328 RID: 808 RVA: 0x00010424 File Offset: 0x0000E624
		public MethodReturnType MethodReturnType
		{
			get
			{
				return this.signature.MethodReturnType;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000329 RID: 809 RVA: 0x00010431 File Offset: 0x0000E631
		// (set) Token: 0x0600032A RID: 810 RVA: 0x00002C55 File Offset: 0x00000E55
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

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600032B RID: 811 RVA: 0x00010431 File Offset: 0x0000E631
		// (set) Token: 0x0600032C RID: 812 RVA: 0x00002C55 File Offset: 0x00000E55
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

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600032D RID: 813 RVA: 0x00010438 File Offset: 0x0000E638
		public ModuleDefinition Module
		{
			get
			{
				return this.ReturnType.Module;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600032E RID: 814 RVA: 0x00010445 File Offset: 0x0000E645
		public IMetadataScope Scope
		{
			get
			{
				return this.signature.ReturnType.Scope;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600032F RID: 815 RVA: 0x00010457 File Offset: 0x0000E657
		// (set) Token: 0x06000330 RID: 816 RVA: 0x00010464 File Offset: 0x0000E664
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

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000331 RID: 817 RVA: 0x00010474 File Offset: 0x0000E674
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

		// Token: 0x06000332 RID: 818 RVA: 0x000104A6 File Offset: 0x0000E6A6
		internal CallSite()
		{
			this.signature = new MethodReference();
			this.signature.token = new MetadataToken(TokenType.Signature, 0);
		}

		// Token: 0x06000333 RID: 819 RVA: 0x000104CF File Offset: 0x0000E6CF
		public CallSite(TypeReference returnType)
			: this()
		{
			if (returnType == null)
			{
				throw new ArgumentNullException("returnType");
			}
			this.signature.ReturnType = returnType;
		}

		// Token: 0x06000334 RID: 820 RVA: 0x000104F1 File Offset: 0x0000E6F1
		public override string ToString()
		{
			return this.FullName;
		}

		// Token: 0x04000085 RID: 133
		private readonly MethodReference signature;
	}
}
