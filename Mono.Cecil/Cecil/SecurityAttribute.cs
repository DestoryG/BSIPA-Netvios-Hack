using System;
using System.Diagnostics;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020000AE RID: 174
	[DebuggerDisplay("{AttributeType}")]
	public sealed class SecurityAttribute : ICustomAttribute
	{
		// Token: 0x17000225 RID: 549
		// (get) Token: 0x0600076D RID: 1901 RVA: 0x0001735E File Offset: 0x0001555E
		// (set) Token: 0x0600076E RID: 1902 RVA: 0x00017366 File Offset: 0x00015566
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

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x0600076F RID: 1903 RVA: 0x0001736F File Offset: 0x0001556F
		public bool HasFields
		{
			get
			{
				return !this.fields.IsNullOrEmpty<CustomAttributeNamedArgument>();
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000770 RID: 1904 RVA: 0x00017380 File Offset: 0x00015580
		public Collection<CustomAttributeNamedArgument> Fields
		{
			get
			{
				Collection<CustomAttributeNamedArgument> collection;
				if ((collection = this.fields) == null)
				{
					collection = (this.fields = new Collection<CustomAttributeNamedArgument>());
				}
				return collection;
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000771 RID: 1905 RVA: 0x000173A5 File Offset: 0x000155A5
		public bool HasProperties
		{
			get
			{
				return !this.properties.IsNullOrEmpty<CustomAttributeNamedArgument>();
			}
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000772 RID: 1906 RVA: 0x000173B8 File Offset: 0x000155B8
		public Collection<CustomAttributeNamedArgument> Properties
		{
			get
			{
				Collection<CustomAttributeNamedArgument> collection;
				if ((collection = this.properties) == null)
				{
					collection = (this.properties = new Collection<CustomAttributeNamedArgument>());
				}
				return collection;
			}
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x000173DD File Offset: 0x000155DD
		public SecurityAttribute(TypeReference attributeType)
		{
			this.attribute_type = attributeType;
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000774 RID: 1908 RVA: 0x000026DB File Offset: 0x000008DB
		bool ICustomAttribute.HasConstructorArguments
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000775 RID: 1909 RVA: 0x00011A5E File Offset: 0x0000FC5E
		Collection<CustomAttributeArgument> ICustomAttribute.ConstructorArguments
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x04000244 RID: 580
		private TypeReference attribute_type;

		// Token: 0x04000245 RID: 581
		internal Collection<CustomAttributeNamedArgument> fields;

		// Token: 0x04000246 RID: 582
		internal Collection<CustomAttributeNamedArgument> properties;
	}
}
