using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020000AF RID: 175
	public sealed class SecurityDeclaration
	{
		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000776 RID: 1910 RVA: 0x000173EC File Offset: 0x000155EC
		// (set) Token: 0x06000777 RID: 1911 RVA: 0x000173F4 File Offset: 0x000155F4
		public SecurityAction Action
		{
			get
			{
				return this.action;
			}
			set
			{
				this.action = value;
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000778 RID: 1912 RVA: 0x000173FD File Offset: 0x000155FD
		public bool HasSecurityAttributes
		{
			get
			{
				this.Resolve();
				return !this.security_attributes.IsNullOrEmpty<SecurityAttribute>();
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000779 RID: 1913 RVA: 0x00017414 File Offset: 0x00015614
		public Collection<SecurityAttribute> SecurityAttributes
		{
			get
			{
				this.Resolve();
				Collection<SecurityAttribute> collection;
				if ((collection = this.security_attributes) == null)
				{
					collection = (this.security_attributes = new Collection<SecurityAttribute>());
				}
				return collection;
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x0600077A RID: 1914 RVA: 0x0001743F File Offset: 0x0001563F
		internal bool HasImage
		{
			get
			{
				return this.module != null && this.module.HasImage;
			}
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x00017456 File Offset: 0x00015656
		internal SecurityDeclaration(SecurityAction action, uint signature, ModuleDefinition module)
		{
			this.action = action;
			this.signature = signature;
			this.module = module;
		}

		// Token: 0x0600077C RID: 1916 RVA: 0x00017473 File Offset: 0x00015673
		public SecurityDeclaration(SecurityAction action)
		{
			this.action = action;
			this.resolved = true;
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x00017489 File Offset: 0x00015689
		public SecurityDeclaration(SecurityAction action, byte[] blob)
		{
			this.action = action;
			this.resolved = false;
			this.blob = blob;
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x000174A8 File Offset: 0x000156A8
		public byte[] GetBlob()
		{
			if (this.blob != null)
			{
				return this.blob;
			}
			if (!this.HasImage || this.signature == 0U)
			{
				throw new NotSupportedException();
			}
			return this.blob = this.module.Read<SecurityDeclaration, byte[]>(this, (SecurityDeclaration declaration, MetadataReader reader) => reader.ReadSecurityDeclarationBlob(declaration.signature));
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x00017510 File Offset: 0x00015710
		private void Resolve()
		{
			if (this.resolved || !this.HasImage)
			{
				return;
			}
			this.module.Read<SecurityDeclaration>(this, delegate(SecurityDeclaration declaration, MetadataReader reader)
			{
				reader.ReadSecurityDeclarationSignature(declaration);
			});
			this.resolved = true;
		}

		// Token: 0x04000247 RID: 583
		internal readonly uint signature;

		// Token: 0x04000248 RID: 584
		private byte[] blob;

		// Token: 0x04000249 RID: 585
		private readonly ModuleDefinition module;

		// Token: 0x0400024A RID: 586
		internal bool resolved;

		// Token: 0x0400024B RID: 587
		private SecurityAction action;

		// Token: 0x0400024C RID: 588
		internal Collection<SecurityAttribute> security_attributes;
	}
}
