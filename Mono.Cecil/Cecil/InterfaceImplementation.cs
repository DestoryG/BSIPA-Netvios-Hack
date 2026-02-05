using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020000BA RID: 186
	public sealed class InterfaceImplementation : ICustomAttributeProvider, IMetadataTokenProvider
	{
		// Token: 0x1700026A RID: 618
		// (get) Token: 0x060007E2 RID: 2018 RVA: 0x00018168 File Offset: 0x00016368
		// (set) Token: 0x060007E3 RID: 2019 RVA: 0x00018170 File Offset: 0x00016370
		public TypeReference InterfaceType
		{
			get
			{
				return this.interface_type;
			}
			set
			{
				this.interface_type = value;
			}
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x060007E4 RID: 2020 RVA: 0x00018179 File Offset: 0x00016379
		public bool HasCustomAttributes
		{
			get
			{
				if (this.custom_attributes != null)
				{
					return this.custom_attributes.Count > 0;
				}
				return this.type != null && this.GetHasCustomAttributes(this.type.Module);
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x060007E5 RID: 2021 RVA: 0x000181B0 File Offset: 0x000163B0
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				if (this.type == null)
				{
					return this.custom_attributes = new Collection<CustomAttribute>();
				}
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.type.Module);
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x060007E6 RID: 2022 RVA: 0x000181F5 File Offset: 0x000163F5
		// (set) Token: 0x060007E7 RID: 2023 RVA: 0x000181FD File Offset: 0x000163FD
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

		// Token: 0x060007E8 RID: 2024 RVA: 0x00018206 File Offset: 0x00016406
		internal InterfaceImplementation(TypeReference interfaceType, MetadataToken token)
		{
			this.interface_type = interfaceType;
			this.token = token;
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x0001821C File Offset: 0x0001641C
		public InterfaceImplementation(TypeReference interfaceType)
		{
			Mixin.CheckType(interfaceType, Mixin.Argument.interfaceType);
			this.interface_type = interfaceType;
			this.token = new MetadataToken(TokenType.InterfaceImpl);
		}

		// Token: 0x040002A3 RID: 675
		internal TypeDefinition type;

		// Token: 0x040002A4 RID: 676
		internal MetadataToken token;

		// Token: 0x040002A5 RID: 677
		private TypeReference interface_type;

		// Token: 0x040002A6 RID: 678
		private Collection<CustomAttribute> custom_attributes;
	}
}
