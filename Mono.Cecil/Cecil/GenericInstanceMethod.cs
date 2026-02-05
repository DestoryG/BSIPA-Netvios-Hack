using System;
using System.Text;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200005F RID: 95
	public sealed class GenericInstanceMethod : MethodSpecification, IGenericInstance, IMetadataTokenProvider, IGenericContext
	{
		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x00011948 File Offset: 0x0000FB48
		public bool HasGenericArguments
		{
			get
			{
				return !this.arguments.IsNullOrEmpty<TypeReference>();
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600041A RID: 1050 RVA: 0x00011958 File Offset: 0x0000FB58
		public Collection<TypeReference> GenericArguments
		{
			get
			{
				Collection<TypeReference> collection;
				if ((collection = this.arguments) == null)
				{
					collection = (this.arguments = new Collection<TypeReference>());
				}
				return collection;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600041B RID: 1051 RVA: 0x00002BE8 File Offset: 0x00000DE8
		public override bool IsGenericInstance
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600041C RID: 1052 RVA: 0x0001197D File Offset: 0x0000FB7D
		IGenericParameterProvider IGenericContext.Method
		{
			get
			{
				return base.ElementMethod;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600041D RID: 1053 RVA: 0x00011985 File Offset: 0x0000FB85
		IGenericParameterProvider IGenericContext.Type
		{
			get
			{
				return base.ElementMethod.DeclaringType;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x00011992 File Offset: 0x0000FB92
		public override bool ContainsGenericParameter
		{
			get
			{
				return this.ContainsGenericParameter() || base.ContainsGenericParameter;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x0600041F RID: 1055 RVA: 0x000119A4 File Offset: 0x0000FBA4
		public override string FullName
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				MethodReference elementMethod = base.ElementMethod;
				stringBuilder.Append(elementMethod.ReturnType.FullName).Append(" ").Append(elementMethod.DeclaringType.FullName)
					.Append("::")
					.Append(elementMethod.Name);
				this.GenericInstanceFullName(stringBuilder);
				this.MethodSignatureFullName(stringBuilder);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00011A13 File Offset: 0x0000FC13
		public GenericInstanceMethod(MethodReference method)
			: base(method)
		{
		}

		// Token: 0x040000C9 RID: 201
		private Collection<TypeReference> arguments;
	}
}
