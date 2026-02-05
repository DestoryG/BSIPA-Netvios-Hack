using System;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000177 RID: 375
	internal sealed class InterfaceImplementation : ICustomAttributeProvider, IMetadataTokenProvider
	{
		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000BAD RID: 2989 RVA: 0x00026E6F File Offset: 0x0002506F
		// (set) Token: 0x06000BAE RID: 2990 RVA: 0x00026E77 File Offset: 0x00025077
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

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000BAF RID: 2991 RVA: 0x00026E80 File Offset: 0x00025080
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

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000BB0 RID: 2992 RVA: 0x00026EB4 File Offset: 0x000250B4
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				if (this.type == null)
				{
					if (this.custom_attributes == null)
					{
						Interlocked.CompareExchange<Collection<CustomAttribute>>(ref this.custom_attributes, new Collection<CustomAttribute>(), null);
					}
					return this.custom_attributes;
				}
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.type.Module);
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000BB1 RID: 2993 RVA: 0x00026F0B File Offset: 0x0002510B
		// (set) Token: 0x06000BB2 RID: 2994 RVA: 0x00026F13 File Offset: 0x00025113
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

		// Token: 0x06000BB3 RID: 2995 RVA: 0x00026F1C File Offset: 0x0002511C
		internal InterfaceImplementation(TypeReference interfaceType, MetadataToken token)
		{
			this.interface_type = interfaceType;
			this.token = token;
		}

		// Token: 0x06000BB4 RID: 2996 RVA: 0x00026F32 File Offset: 0x00025132
		public InterfaceImplementation(TypeReference interfaceType)
		{
			Mixin.CheckType(interfaceType, Mixin.Argument.interfaceType);
			this.interface_type = interfaceType;
			this.token = new MetadataToken(TokenType.InterfaceImpl);
		}

		// Token: 0x040004EC RID: 1260
		internal TypeDefinition type;

		// Token: 0x040004ED RID: 1261
		internal MetadataToken token;

		// Token: 0x040004EE RID: 1262
		private TypeReference interface_type;

		// Token: 0x040004EF RID: 1263
		private Collection<CustomAttribute> custom_attributes;
	}
}
