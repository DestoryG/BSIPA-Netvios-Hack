using System;
using System.Diagnostics;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000103 RID: 259
	[DebuggerDisplay("{AttributeType}")]
	internal sealed class CustomAttribute : ICustomAttribute
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060006B6 RID: 1718 RVA: 0x0001EAFD File Offset: 0x0001CCFD
		// (set) Token: 0x060006B7 RID: 1719 RVA: 0x0001EB05 File Offset: 0x0001CD05
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

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060006B8 RID: 1720 RVA: 0x0001EB0E File Offset: 0x0001CD0E
		public TypeReference AttributeType
		{
			get
			{
				return this.constructor.DeclaringType;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060006B9 RID: 1721 RVA: 0x0001EB1B File Offset: 0x0001CD1B
		public bool IsResolved
		{
			get
			{
				return this.resolved;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060006BA RID: 1722 RVA: 0x0001EB23 File Offset: 0x0001CD23
		public bool HasConstructorArguments
		{
			get
			{
				this.Resolve();
				return !this.arguments.IsNullOrEmpty<CustomAttributeArgument>();
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060006BB RID: 1723 RVA: 0x0001EB39 File Offset: 0x0001CD39
		public Collection<CustomAttributeArgument> ConstructorArguments
		{
			get
			{
				this.Resolve();
				if (this.arguments == null)
				{
					Interlocked.CompareExchange<Collection<CustomAttributeArgument>>(ref this.arguments, new Collection<CustomAttributeArgument>(), null);
				}
				return this.arguments;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060006BC RID: 1724 RVA: 0x0001EB61 File Offset: 0x0001CD61
		public bool HasFields
		{
			get
			{
				this.Resolve();
				return !this.fields.IsNullOrEmpty<CustomAttributeNamedArgument>();
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060006BD RID: 1725 RVA: 0x0001EB77 File Offset: 0x0001CD77
		public Collection<CustomAttributeNamedArgument> Fields
		{
			get
			{
				this.Resolve();
				if (this.fields == null)
				{
					Interlocked.CompareExchange<Collection<CustomAttributeNamedArgument>>(ref this.fields, new Collection<CustomAttributeNamedArgument>(), null);
				}
				return this.fields;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060006BE RID: 1726 RVA: 0x0001EB9F File Offset: 0x0001CD9F
		public bool HasProperties
		{
			get
			{
				this.Resolve();
				return !this.properties.IsNullOrEmpty<CustomAttributeNamedArgument>();
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060006BF RID: 1727 RVA: 0x0001EBB5 File Offset: 0x0001CDB5
		public Collection<CustomAttributeNamedArgument> Properties
		{
			get
			{
				this.Resolve();
				if (this.properties == null)
				{
					Interlocked.CompareExchange<Collection<CustomAttributeNamedArgument>>(ref this.properties, new Collection<CustomAttributeNamedArgument>(), null);
				}
				return this.properties;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060006C0 RID: 1728 RVA: 0x0001EBDD File Offset: 0x0001CDDD
		internal bool HasImage
		{
			get
			{
				return this.constructor != null && this.constructor.HasImage;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060006C1 RID: 1729 RVA: 0x0001EBF4 File Offset: 0x0001CDF4
		internal ModuleDefinition Module
		{
			get
			{
				return this.constructor.Module;
			}
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x0001EC01 File Offset: 0x0001CE01
		internal CustomAttribute(uint signature, MethodReference constructor)
		{
			this.signature = signature;
			this.constructor = constructor;
			this.resolved = false;
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x0001EC1E File Offset: 0x0001CE1E
		public CustomAttribute(MethodReference constructor)
		{
			this.constructor = constructor;
			this.resolved = true;
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x0001EC34 File Offset: 0x0001CE34
		public CustomAttribute(MethodReference constructor, byte[] blob)
		{
			this.constructor = constructor;
			this.resolved = false;
			this.blob = blob;
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x0001EC54 File Offset: 0x0001CE54
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

		// Token: 0x060006C6 RID: 1734 RVA: 0x0001ECB0 File Offset: 0x0001CEB0
		private void Resolve()
		{
			if (this.resolved || !this.HasImage)
			{
				return;
			}
			object syncRoot = this.Module.SyncRoot;
			lock (syncRoot)
			{
				if (!this.resolved)
				{
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
			}
		}

		// Token: 0x04000292 RID: 658
		internal CustomAttributeValueProjection projection;

		// Token: 0x04000293 RID: 659
		internal readonly uint signature;

		// Token: 0x04000294 RID: 660
		internal bool resolved;

		// Token: 0x04000295 RID: 661
		private MethodReference constructor;

		// Token: 0x04000296 RID: 662
		private byte[] blob;

		// Token: 0x04000297 RID: 663
		internal Collection<CustomAttributeArgument> arguments;

		// Token: 0x04000298 RID: 664
		internal Collection<CustomAttributeNamedArgument> fields;

		// Token: 0x04000299 RID: 665
		internal Collection<CustomAttributeNamedArgument> properties;
	}
}
