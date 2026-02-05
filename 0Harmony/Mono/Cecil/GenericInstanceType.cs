using System;
using System.Text;
using System.Threading;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000113 RID: 275
	internal sealed class GenericInstanceType : TypeSpecification, IGenericInstance, IMetadataTokenProvider, IGenericContext
	{
		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060007A0 RID: 1952 RVA: 0x000200E5 File Offset: 0x0001E2E5
		public bool HasGenericArguments
		{
			get
			{
				return !this.arguments.IsNullOrEmpty<TypeReference>();
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060007A1 RID: 1953 RVA: 0x000200F5 File Offset: 0x0001E2F5
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

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060007A2 RID: 1954 RVA: 0x00020117 File Offset: 0x0001E317
		// (set) Token: 0x060007A3 RID: 1955 RVA: 0x000039BA File Offset: 0x00001BBA
		public override TypeReference DeclaringType
		{
			get
			{
				return base.ElementType.DeclaringType;
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060007A4 RID: 1956 RVA: 0x00020124 File Offset: 0x0001E324
		public override string FullName
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.FullName);
				this.GenericInstanceFullName(stringBuilder);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060007A5 RID: 1957 RVA: 0x00010F39 File Offset: 0x0000F139
		public override bool IsGenericInstance
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060007A6 RID: 1958 RVA: 0x00020151 File Offset: 0x0001E351
		public override bool ContainsGenericParameter
		{
			get
			{
				return this.ContainsGenericParameter() || base.ContainsGenericParameter;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060007A7 RID: 1959 RVA: 0x00020163 File Offset: 0x0001E363
		IGenericParameterProvider IGenericContext.Type
		{
			get
			{
				return base.ElementType;
			}
		}

		// Token: 0x060007A8 RID: 1960 RVA: 0x0002016B File Offset: 0x0001E36B
		public GenericInstanceType(TypeReference type)
			: base(type)
		{
			base.IsValueType = type.IsValueType;
			this.etype = Mono.Cecil.Metadata.ElementType.GenericInst;
		}

		// Token: 0x060007A9 RID: 1961 RVA: 0x00020188 File Offset: 0x0001E388
		internal GenericInstanceType(TypeReference type, int arity)
			: this(type)
		{
			this.arguments = new Collection<TypeReference>(arity);
		}

		// Token: 0x040002D9 RID: 729
		private Collection<TypeReference> arguments;
	}
}
