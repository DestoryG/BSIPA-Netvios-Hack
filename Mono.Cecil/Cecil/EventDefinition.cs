using System;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000057 RID: 87
	public sealed class EventDefinition : EventReference, IMemberDefinition, ICustomAttributeProvider, IMetadataTokenProvider
	{
		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600035F RID: 863 RVA: 0x00010A20 File Offset: 0x0000EC20
		// (set) Token: 0x06000360 RID: 864 RVA: 0x00010A28 File Offset: 0x0000EC28
		public EventAttributes Attributes
		{
			get
			{
				return (EventAttributes)this.attributes;
			}
			set
			{
				this.attributes = (ushort)value;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000361 RID: 865 RVA: 0x00010A31 File Offset: 0x0000EC31
		// (set) Token: 0x06000362 RID: 866 RVA: 0x00010A4E File Offset: 0x0000EC4E
		public MethodDefinition AddMethod
		{
			get
			{
				if (this.add_method != null)
				{
					return this.add_method;
				}
				this.InitializeMethods();
				return this.add_method;
			}
			set
			{
				this.add_method = value;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000363 RID: 867 RVA: 0x00010A57 File Offset: 0x0000EC57
		// (set) Token: 0x06000364 RID: 868 RVA: 0x00010A74 File Offset: 0x0000EC74
		public MethodDefinition InvokeMethod
		{
			get
			{
				if (this.invoke_method != null)
				{
					return this.invoke_method;
				}
				this.InitializeMethods();
				return this.invoke_method;
			}
			set
			{
				this.invoke_method = value;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000365 RID: 869 RVA: 0x00010A7D File Offset: 0x0000EC7D
		// (set) Token: 0x06000366 RID: 870 RVA: 0x00010A9A File Offset: 0x0000EC9A
		public MethodDefinition RemoveMethod
		{
			get
			{
				if (this.remove_method != null)
				{
					return this.remove_method;
				}
				this.InitializeMethods();
				return this.remove_method;
			}
			set
			{
				this.remove_method = value;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000367 RID: 871 RVA: 0x00010AA3 File Offset: 0x0000ECA3
		public bool HasOtherMethods
		{
			get
			{
				if (this.other_methods != null)
				{
					return this.other_methods.Count > 0;
				}
				this.InitializeMethods();
				return !this.other_methods.IsNullOrEmpty<MethodDefinition>();
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06000368 RID: 872 RVA: 0x00010AD0 File Offset: 0x0000ECD0
		public Collection<MethodDefinition> OtherMethods
		{
			get
			{
				if (this.other_methods != null)
				{
					return this.other_methods;
				}
				this.InitializeMethods();
				if (this.other_methods != null)
				{
					return this.other_methods;
				}
				return this.other_methods = new Collection<MethodDefinition>();
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000369 RID: 873 RVA: 0x00010B0F File Offset: 0x0000ED0F
		public bool HasCustomAttributes
		{
			get
			{
				if (this.custom_attributes != null)
				{
					return this.custom_attributes.Count > 0;
				}
				return this.GetHasCustomAttributes(this.Module);
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600036A RID: 874 RVA: 0x00010B34 File Offset: 0x0000ED34
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.Module);
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600036B RID: 875 RVA: 0x00010B52 File Offset: 0x0000ED52
		// (set) Token: 0x0600036C RID: 876 RVA: 0x00010B64 File Offset: 0x0000ED64
		public bool IsSpecialName
		{
			get
			{
				return this.attributes.GetAttributes(512);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(512, value);
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600036D RID: 877 RVA: 0x00010B7D File Offset: 0x0000ED7D
		// (set) Token: 0x0600036E RID: 878 RVA: 0x00010B8F File Offset: 0x0000ED8F
		public bool IsRuntimeSpecialName
		{
			get
			{
				return this.attributes.GetAttributes(1024);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(1024, value);
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x0600036F RID: 879 RVA: 0x00010BA8 File Offset: 0x0000EDA8
		// (set) Token: 0x06000370 RID: 880 RVA: 0x00010BB5 File Offset: 0x0000EDB5
		public new TypeDefinition DeclaringType
		{
			get
			{
				return (TypeDefinition)base.DeclaringType;
			}
			set
			{
				base.DeclaringType = value;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000371 RID: 881 RVA: 0x00002BE8 File Offset: 0x00000DE8
		public override bool IsDefinition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00010BBE File Offset: 0x0000EDBE
		public EventDefinition(string name, EventAttributes attributes, TypeReference eventType)
			: base(name, eventType)
		{
			this.attributes = (ushort)attributes;
			this.token = new MetadataToken(TokenType.Event);
		}

		// Token: 0x06000373 RID: 883 RVA: 0x00010BE0 File Offset: 0x0000EDE0
		private void InitializeMethods()
		{
			ModuleDefinition module = this.Module;
			if (module == null)
			{
				return;
			}
			object syncRoot = module.SyncRoot;
			lock (syncRoot)
			{
				if (this.add_method == null && this.invoke_method == null && this.remove_method == null)
				{
					if (module.HasImage())
					{
						module.Read<EventDefinition>(this, delegate(EventDefinition @event, MetadataReader reader)
						{
							reader.ReadMethods(@event);
						});
					}
				}
			}
		}

		// Token: 0x06000374 RID: 884 RVA: 0x00002740 File Offset: 0x00000940
		public override EventDefinition Resolve()
		{
			return this;
		}

		// Token: 0x0400009B RID: 155
		private ushort attributes;

		// Token: 0x0400009C RID: 156
		private Collection<CustomAttribute> custom_attributes;

		// Token: 0x0400009D RID: 157
		internal MethodDefinition add_method;

		// Token: 0x0400009E RID: 158
		internal MethodDefinition invoke_method;

		// Token: 0x0400009F RID: 159
		internal MethodDefinition remove_method;

		// Token: 0x040000A0 RID: 160
		internal Collection<MethodDefinition> other_methods;
	}
}
