using System;
using System.Diagnostics;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000169 RID: 361
	[DebuggerDisplay("{AttributeType}")]
	internal sealed class SecurityAttribute : ICustomAttribute
	{
		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000B27 RID: 2855 RVA: 0x00025F3F File Offset: 0x0002413F
		// (set) Token: 0x06000B28 RID: 2856 RVA: 0x00025F47 File Offset: 0x00024147
		public TypeReference AttributeType
		{
			get
			{
				return this.attribute_type;
			}
			set
			{
				this.attribute_type = value;
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000B29 RID: 2857 RVA: 0x00025F50 File Offset: 0x00024150
		public bool HasFields
		{
			get
			{
				return !this.fields.IsNullOrEmpty<CustomAttributeNamedArgument>();
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000B2A RID: 2858 RVA: 0x00025F60 File Offset: 0x00024160
		public Collection<CustomAttributeNamedArgument> Fields
		{
			get
			{
				if (this.fields == null)
				{
					Interlocked.CompareExchange<Collection<CustomAttributeNamedArgument>>(ref this.fields, new Collection<CustomAttributeNamedArgument>(), null);
				}
				return this.fields;
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000B2B RID: 2859 RVA: 0x00025F82 File Offset: 0x00024182
		public bool HasProperties
		{
			get
			{
				return !this.properties.IsNullOrEmpty<CustomAttributeNamedArgument>();
			}
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000B2C RID: 2860 RVA: 0x00025F92 File Offset: 0x00024192
		public Collection<CustomAttributeNamedArgument> Properties
		{
			get
			{
				if (this.properties == null)
				{
					Interlocked.CompareExchange<Collection<CustomAttributeNamedArgument>>(ref this.properties, new Collection<CustomAttributeNamedArgument>(), null);
				}
				return this.properties;
			}
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x00025FB4 File Offset: 0x000241B4
		public SecurityAttribute(TypeReference attributeType)
		{
			this.attribute_type = attributeType;
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000B2E RID: 2862 RVA: 0x00010910 File Offset: 0x0000EB10
		bool ICustomAttribute.HasConstructorArguments
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06000B2F RID: 2863 RVA: 0x000039BA File Offset: 0x00001BBA
		Collection<CustomAttributeArgument> ICustomAttribute.ConstructorArguments
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x0400047E RID: 1150
		private TypeReference attribute_type;

		// Token: 0x0400047F RID: 1151
		internal Collection<CustomAttributeNamedArgument> fields;

		// Token: 0x04000480 RID: 1152
		internal Collection<CustomAttributeNamedArgument> properties;
	}
}
