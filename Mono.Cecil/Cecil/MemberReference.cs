using System;

namespace Mono.Cecil
{
	// Token: 0x02000080 RID: 128
	public abstract class MemberReference : IMetadataTokenProvider
	{
		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060004F1 RID: 1265 RVA: 0x0001333C File Offset: 0x0001153C
		// (set) Token: 0x060004F2 RID: 1266 RVA: 0x00013344 File Offset: 0x00011544
		public virtual string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (this.IsWindowsRuntimeProjection && value != this.name)
				{
					throw new InvalidOperationException();
				}
				this.name = value;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060004F3 RID: 1267
		public abstract string FullName { get; }

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060004F4 RID: 1268 RVA: 0x00013369 File Offset: 0x00011569
		// (set) Token: 0x060004F5 RID: 1269 RVA: 0x00013371 File Offset: 0x00011571
		public virtual TypeReference DeclaringType
		{
			get
			{
				return this.declaring_type;
			}
			set
			{
				this.declaring_type = value;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060004F6 RID: 1270 RVA: 0x0001337A File Offset: 0x0001157A
		// (set) Token: 0x060004F7 RID: 1271 RVA: 0x00013382 File Offset: 0x00011582
		public MetadataToken MetadataToken
		{
			get
			{
				return this.token;
			}
			set
			{
				this.token = value;
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060004F8 RID: 1272 RVA: 0x0001338B File Offset: 0x0001158B
		public bool IsWindowsRuntimeProjection
		{
			get
			{
				return this.projection != null;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060004F9 RID: 1273 RVA: 0x00013396 File Offset: 0x00011596
		// (set) Token: 0x060004FA RID: 1274 RVA: 0x000112F3 File Offset: 0x0000F4F3
		internal MemberReferenceProjection WindowsRuntimeProjection
		{
			get
			{
				return (MemberReferenceProjection)this.projection;
			}
			set
			{
				this.projection = value;
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060004FB RID: 1275 RVA: 0x000133A4 File Offset: 0x000115A4
		internal bool HasImage
		{
			get
			{
				ModuleDefinition module = this.Module;
				return module != null && module.HasImage;
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060004FC RID: 1276 RVA: 0x000133C3 File Offset: 0x000115C3
		public virtual ModuleDefinition Module
		{
			get
			{
				if (this.declaring_type == null)
				{
					return null;
				}
				return this.declaring_type.Module;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060004FD RID: 1277 RVA: 0x000026DB File Offset: 0x000008DB
		public virtual bool IsDefinition
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060004FE RID: 1278 RVA: 0x000133DA File Offset: 0x000115DA
		public virtual bool ContainsGenericParameter
		{
			get
			{
				return this.declaring_type != null && this.declaring_type.ContainsGenericParameter;
			}
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x00004F37 File Offset: 0x00003137
		internal MemberReference()
		{
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x000133F1 File Offset: 0x000115F1
		internal MemberReference(string name)
		{
			this.name = name ?? string.Empty;
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x00013409 File Offset: 0x00011609
		internal string MemberFullName()
		{
			if (this.declaring_type == null)
			{
				return this.name;
			}
			return this.declaring_type.FullName + "::" + this.name;
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x00013435 File Offset: 0x00011635
		public IMemberDefinition Resolve()
		{
			return this.ResolveDefinition();
		}

		// Token: 0x06000503 RID: 1283
		protected abstract IMemberDefinition ResolveDefinition();

		// Token: 0x06000504 RID: 1284 RVA: 0x0001343D File Offset: 0x0001163D
		public override string ToString()
		{
			return this.FullName;
		}

		// Token: 0x040000FA RID: 250
		private string name;

		// Token: 0x040000FB RID: 251
		private TypeReference declaring_type;

		// Token: 0x040000FC RID: 252
		internal MetadataToken token;

		// Token: 0x040000FD RID: 253
		internal object projection;
	}
}
