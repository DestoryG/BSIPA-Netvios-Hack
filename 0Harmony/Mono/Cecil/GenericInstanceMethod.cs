using System;
using System.Text;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000112 RID: 274
	internal sealed class GenericInstanceMethod : MethodSpecification, IGenericInstance, IMetadataTokenProvider, IGenericContext
	{
		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000797 RID: 1943 RVA: 0x0001FFFC File Offset: 0x0001E1FC
		public bool HasGenericArguments
		{
			get
			{
				return !this.arguments.IsNullOrEmpty<TypeReference>();
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000798 RID: 1944 RVA: 0x0002000C File Offset: 0x0001E20C
		public Collection<TypeReference> GenericArguments
		{
			get
			{
				if (this.arguments == null)
				{
					Interlocked.CompareExchange<Collection<TypeReference>>(ref this.arguments, new Collection<TypeReference>(), null);
				}
				return this.arguments;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000799 RID: 1945 RVA: 0x00010F39 File Offset: 0x0000F139
		public override bool IsGenericInstance
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x0600079A RID: 1946 RVA: 0x0002002E File Offset: 0x0001E22E
		IGenericParameterProvider IGenericContext.Method
		{
			get
			{
				return base.ElementMethod;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600079B RID: 1947 RVA: 0x00020036 File Offset: 0x0001E236
		IGenericParameterProvider IGenericContext.Type
		{
			get
			{
				return base.ElementMethod.DeclaringType;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x0600079C RID: 1948 RVA: 0x00020043 File Offset: 0x0001E243
		public override bool ContainsGenericParameter
		{
			get
			{
				return this.ContainsGenericParameter() || base.ContainsGenericParameter;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600079D RID: 1949 RVA: 0x00020058 File Offset: 0x0001E258
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

		// Token: 0x0600079E RID: 1950 RVA: 0x000200C7 File Offset: 0x0001E2C7
		public GenericInstanceMethod(MethodReference method)
			: base(method)
		{
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x000200D0 File Offset: 0x0001E2D0
		internal GenericInstanceMethod(MethodReference method, int arity)
			: this(method)
		{
			this.arguments = new Collection<TypeReference>(arity);
		}

		// Token: 0x040002D8 RID: 728
		private Collection<TypeReference> arguments;
	}
}
