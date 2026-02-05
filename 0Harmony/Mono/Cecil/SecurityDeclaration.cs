using System;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200016A RID: 362
	internal sealed class SecurityDeclaration
	{
		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000B30 RID: 2864 RVA: 0x00025FC3 File Offset: 0x000241C3
		// (set) Token: 0x06000B31 RID: 2865 RVA: 0x00025FCB File Offset: 0x000241CB
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

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000B32 RID: 2866 RVA: 0x00025FD4 File Offset: 0x000241D4
		public bool HasSecurityAttributes
		{
			get
			{
				this.Resolve();
				return !this.security_attributes.IsNullOrEmpty<SecurityAttribute>();
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000B33 RID: 2867 RVA: 0x00025FEA File Offset: 0x000241EA
		public Collection<SecurityAttribute> SecurityAttributes
		{
			get
			{
				this.Resolve();
				if (this.security_attributes == null)
				{
					Interlocked.CompareExchange<Collection<SecurityAttribute>>(ref this.security_attributes, new Collection<SecurityAttribute>(), null);
				}
				return this.security_attributes;
			}
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000B34 RID: 2868 RVA: 0x00026012 File Offset: 0x00024212
		internal bool HasImage
		{
			get
			{
				return this.module != null && this.module.HasImage;
			}
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x00026029 File Offset: 0x00024229
		internal SecurityDeclaration(SecurityAction action, uint signature, ModuleDefinition module)
		{
			this.action = action;
			this.signature = signature;
			this.module = module;
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x00026046 File Offset: 0x00024246
		public SecurityDeclaration(SecurityAction action)
		{
			this.action = action;
			this.resolved = true;
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x0002605C File Offset: 0x0002425C
		public SecurityDeclaration(SecurityAction action, byte[] blob)
		{
			this.action = action;
			this.resolved = false;
			this.blob = blob;
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x0002607C File Offset: 0x0002427C
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
			return this.module.Read<SecurityDeclaration, byte[]>(ref this.blob, this, (SecurityDeclaration declaration, MetadataReader reader) => reader.ReadSecurityDeclarationBlob(declaration.signature));
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x000260E0 File Offset: 0x000242E0
		private void Resolve()
		{
			if (this.resolved || !this.HasImage)
			{
				return;
			}
			object syncRoot = this.module.SyncRoot;
			lock (syncRoot)
			{
				if (!this.resolved)
				{
					this.module.Read<SecurityDeclaration>(this, delegate(SecurityDeclaration declaration, MetadataReader reader)
					{
						reader.ReadSecurityDeclarationSignature(declaration);
					});
					this.resolved = true;
				}
			}
		}

		// Token: 0x04000481 RID: 1153
		internal readonly uint signature;

		// Token: 0x04000482 RID: 1154
		private byte[] blob;

		// Token: 0x04000483 RID: 1155
		private readonly ModuleDefinition module;

		// Token: 0x04000484 RID: 1156
		internal bool resolved;

		// Token: 0x04000485 RID: 1157
		private SecurityAction action;

		// Token: 0x04000486 RID: 1158
		internal Collection<SecurityAttribute> security_attributes;
	}
}
