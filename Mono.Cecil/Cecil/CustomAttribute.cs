using System;
using System.Diagnostics;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000053 RID: 83
	[DebuggerDisplay("{AttributeType}")]
	public sealed class CustomAttribute : ICustomAttribute
	{
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000342 RID: 834 RVA: 0x00010545 File Offset: 0x0000E745
		// (set) Token: 0x06000343 RID: 835 RVA: 0x0001054D File Offset: 0x0000E74D
		public MethodReference Constructor
		{
			get
			{
				return this.constructor;
			}
			set
			{
				this.constructor = value;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000344 RID: 836 RVA: 0x00010556 File Offset: 0x0000E756
		public TypeReference AttributeType
		{
			get
			{
				return this.constructor.DeclaringType;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000345 RID: 837 RVA: 0x00010563 File Offset: 0x0000E763
		public bool IsResolved
		{
			get
			{
				return this.resolved;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000346 RID: 838 RVA: 0x0001056B File Offset: 0x0000E76B
		public bool HasConstructorArguments
		{
			get
			{
				this.Resolve();
				return !this.arguments.IsNullOrEmpty<CustomAttributeArgument>();
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000347 RID: 839 RVA: 0x00010584 File Offset: 0x0000E784
		public Collection<CustomAttributeArgument> ConstructorArguments
		{
			get
			{
				this.Resolve();
				Collection<CustomAttributeArgument> collection;
				if ((collection = this.arguments) == null)
				{
					collection = (this.arguments = new Collection<CustomAttributeArgument>());
				}
				return collection;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000348 RID: 840 RVA: 0x000105AF File Offset: 0x0000E7AF
		public bool HasFields
		{
			get
			{
				this.Resolve();
				return !this.fields.IsNullOrEmpty<CustomAttributeNamedArgument>();
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000349 RID: 841 RVA: 0x000105C8 File Offset: 0x0000E7C8
		public Collection<CustomAttributeNamedArgument> Fields
		{
			get
			{
				this.Resolve();
				Collection<CustomAttributeNamedArgument> collection;
				if ((collection = this.fields) == null)
				{
					collection = (this.fields = new Collection<CustomAttributeNamedArgument>());
				}
				return collection;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x0600034A RID: 842 RVA: 0x000105F3 File Offset: 0x0000E7F3
		public bool HasProperties
		{
			get
			{
				this.Resolve();
				return !this.properties.IsNullOrEmpty<CustomAttributeNamedArgument>();
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x0600034B RID: 843 RVA: 0x0001060C File Offset: 0x0000E80C
		public Collection<CustomAttributeNamedArgument> Properties
		{
			get
			{
				this.Resolve();
				Collection<CustomAttributeNamedArgument> collection;
				if ((collection = this.properties) == null)
				{
					collection = (this.properties = new Collection<CustomAttributeNamedArgument>());
				}
				return collection;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600034C RID: 844 RVA: 0x00010637 File Offset: 0x0000E837
		internal bool HasImage
		{
			get
			{
				return this.constructor != null && this.constructor.HasImage;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600034D RID: 845 RVA: 0x0001064E File Offset: 0x0000E84E
		internal ModuleDefinition Module
		{
			get
			{
				return this.constructor.Module;
			}
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0001065B File Offset: 0x0000E85B
		internal CustomAttribute(uint signature, MethodReference constructor)
		{
			this.signature = signature;
			this.constructor = constructor;
			this.resolved = false;
		}

		// Token: 0x0600034F RID: 847 RVA: 0x00010678 File Offset: 0x0000E878
		public CustomAttribute(MethodReference constructor)
		{
			this.constructor = constructor;
			this.resolved = true;
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0001068E File Offset: 0x0000E88E
		public CustomAttribute(MethodReference constructor, byte[] blob)
		{
			this.constructor = constructor;
			this.resolved = false;
			this.blob = blob;
		}

		// Token: 0x06000351 RID: 849 RVA: 0x000106AC File Offset: 0x0000E8AC
		public byte[] GetBlob()
		{
			if (this.blob != null)
			{
				return this.blob;
			}
			if (!this.HasImage)
			{
				throw new NotSupportedException();
			}
			return this.Module.Read<CustomAttribute, byte[]>(ref this.blob, this, (CustomAttribute attribute, MetadataReader reader) => reader.ReadCustomAttributeBlob(attribute.signature));
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00010707 File Offset: 0x0000E907
		private void Resolve()
		{
			if (this.resolved || !this.HasImage)
			{
				return;
			}
			this.Module.Read<CustomAttribute>(this, delegate(CustomAttribute attribute, MetadataReader reader)
			{
				try
				{
					reader.ReadCustomAttributeSignature(attribute);
					this.resolved = true;
				}
				catch (ResolutionException)
				{
					if (this.arguments != null)
					{
						this.arguments.Clear();
					}
					if (this.fields != null)
					{
						this.fields.Clear();
					}
					if (this.properties != null)
					{
						this.properties.Clear();
					}
					this.resolved = false;
				}
			});
		}

		// Token: 0x0400008A RID: 138
		internal CustomAttributeValueProjection projection;

		// Token: 0x0400008B RID: 139
		internal readonly uint signature;

		// Token: 0x0400008C RID: 140
		internal bool resolved;

		// Token: 0x0400008D RID: 141
		private MethodReference constructor;

		// Token: 0x0400008E RID: 142
		private byte[] blob;

		// Token: 0x0400008F RID: 143
		internal Collection<CustomAttributeArgument> arguments;

		// Token: 0x04000090 RID: 144
		internal Collection<CustomAttributeNamedArgument> fields;

		// Token: 0x04000091 RID: 145
		internal Collection<CustomAttributeNamedArgument> properties;
	}
}
