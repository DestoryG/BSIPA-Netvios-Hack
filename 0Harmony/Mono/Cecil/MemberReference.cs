using System;

namespace Mono.Cecil
{
	// Token: 0x02000137 RID: 311
	internal abstract class MemberReference : IMetadataTokenProvider
	{
		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000883 RID: 2179 RVA: 0x00021B68 File Offset: 0x0001FD68
		// (set) Token: 0x06000884 RID: 2180 RVA: 0x00021B70 File Offset: 0x0001FD70
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

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000885 RID: 2181
		public abstract string FullName { get; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000886 RID: 2182 RVA: 0x00021B95 File Offset: 0x0001FD95
		// (set) Token: 0x06000887 RID: 2183 RVA: 0x00021B9D File Offset: 0x0001FD9D
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

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000888 RID: 2184 RVA: 0x00021BA6 File Offset: 0x0001FDA6
		// (set) Token: 0x06000889 RID: 2185 RVA: 0x00021BAE File Offset: 0x0001FDAE
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

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x0600088A RID: 2186 RVA: 0x00021BB7 File Offset: 0x0001FDB7
		public bool IsWindowsRuntimeProjection
		{
			get
			{
				return this.projection != null;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x0600088B RID: 2187 RVA: 0x00021BC2 File Offset: 0x0001FDC2
		// (set) Token: 0x0600088C RID: 2188 RVA: 0x0001F94A File Offset: 0x0001DB4A
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

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x0600088D RID: 2189 RVA: 0x00021BD0 File Offset: 0x0001FDD0
		internal bool HasImage
		{
			get
			{
				ModuleDefinition module = this.Module;
				return module != null && module.HasImage;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x0600088E RID: 2190 RVA: 0x00021BEF File Offset: 0x0001FDEF
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

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x0600088F RID: 2191 RVA: 0x00010910 File Offset: 0x0000EB10
		public virtual bool IsDefinition
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000890 RID: 2192 RVA: 0x00021C06 File Offset: 0x0001FE06
		public virtual bool ContainsGenericParameter
		{
			get
			{
				return this.declaring_type != null && this.declaring_type.ContainsGenericParameter;
			}
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x00002AB9 File Offset: 0x00000CB9
		internal MemberReference()
		{
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x00021C1D File Offset: 0x0001FE1D
		internal MemberReference(string name)
		{
			this.name = name ?? string.Empty;
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x00021C35 File Offset: 0x0001FE35
		internal string MemberFullName()
		{
			if (this.declaring_type == null)
			{
				return this.name;
			}
			return this.declaring_type.FullName + "::" + this.name;
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x00021C61 File Offset: 0x0001FE61
		public IMemberDefinition Resolve()
		{
			return this.ResolveDefinition();
		}

		// Token: 0x06000895 RID: 2197
		protected abstract IMemberDefinition ResolveDefinition();

		// Token: 0x06000896 RID: 2198 RVA: 0x00021C69 File Offset: 0x0001FE69
		public override string ToString()
		{
			return this.FullName;
		}

		// Token: 0x04000314 RID: 788
		private string name;

		// Token: 0x04000315 RID: 789
		private TypeReference declaring_type;

		// Token: 0x04000316 RID: 790
		internal MetadataToken token;

		// Token: 0x04000317 RID: 791
		internal object projection;
	}
}
