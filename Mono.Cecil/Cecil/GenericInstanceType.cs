using System;
using System.Text;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000060 RID: 96
	public sealed class GenericInstanceType : TypeSpecification, IGenericInstance, IMetadataTokenProvider, IGenericContext
	{
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000421 RID: 1057 RVA: 0x00011A1C File Offset: 0x0000FC1C
		public bool HasGenericArguments
		{
			get
			{
				return !this.arguments.IsNullOrEmpty<TypeReference>();
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000422 RID: 1058 RVA: 0x00011A2C File Offset: 0x0000FC2C
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

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000423 RID: 1059 RVA: 0x00011A51 File Offset: 0x0000FC51
		// (set) Token: 0x06000424 RID: 1060 RVA: 0x00011A5E File Offset: 0x0000FC5E
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

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000425 RID: 1061 RVA: 0x00011A68 File Offset: 0x0000FC68
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

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000426 RID: 1062 RVA: 0x00002BE8 File Offset: 0x00000DE8
		public override bool IsGenericInstance
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000427 RID: 1063 RVA: 0x00011A95 File Offset: 0x0000FC95
		public override bool ContainsGenericParameter
		{
			get
			{
				return this.ContainsGenericParameter() || base.ContainsGenericParameter;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000428 RID: 1064 RVA: 0x00011AA7 File Offset: 0x0000FCA7
		IGenericParameterProvider IGenericContext.Type
		{
			get
			{
				return base.ElementType;
			}
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x00011AAF File Offset: 0x0000FCAF
		public GenericInstanceType(TypeReference type)
			: base(type)
		{
			base.IsValueType = type.IsValueType;
			this.etype = Mono.Cecil.Metadata.ElementType.GenericInst;
		}

		// Token: 0x040000CA RID: 202
		private Collection<TypeReference> arguments;
	}
}
