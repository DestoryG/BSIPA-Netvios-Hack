using System;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000108 RID: 264
	internal sealed class EventDefinition : EventReference, IMemberDefinition, ICustomAttributeProvider, IMetadataTokenProvider
	{
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060006D6 RID: 1750 RVA: 0x0001F02C File Offset: 0x0001D22C
		// (set) Token: 0x060006D7 RID: 1751 RVA: 0x0001F034 File Offset: 0x0001D234
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

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060006D8 RID: 1752 RVA: 0x0001F03D File Offset: 0x0001D23D
		// (set) Token: 0x060006D9 RID: 1753 RVA: 0x0001F05A File Offset: 0x0001D25A
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

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060006DA RID: 1754 RVA: 0x0001F063 File Offset: 0x0001D263
		// (set) Token: 0x060006DB RID: 1755 RVA: 0x0001F080 File Offset: 0x0001D280
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

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060006DC RID: 1756 RVA: 0x0001F089 File Offset: 0x0001D289
		// (set) Token: 0x060006DD RID: 1757 RVA: 0x0001F0A6 File Offset: 0x0001D2A6
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

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060006DE RID: 1758 RVA: 0x0001F0AF File Offset: 0x0001D2AF
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

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060006DF RID: 1759 RVA: 0x0001F0DC File Offset: 0x0001D2DC
		public Collection<MethodDefinition> OtherMethods
		{
			get
			{
				if (this.other_methods != null)
				{
					return this.other_methods;
				}
				this.InitializeMethods();
				if (this.other_methods == null)
				{
					Interlocked.CompareExchange<Collection<MethodDefinition>>(ref this.other_methods, new Collection<MethodDefinition>(), null);
				}
				return this.other_methods;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060006E0 RID: 1760 RVA: 0x0001F113 File Offset: 0x0001D313
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

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060006E1 RID: 1761 RVA: 0x0001F138 File Offset: 0x0001D338
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this.Module);
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060006E2 RID: 1762 RVA: 0x0001F156 File Offset: 0x0001D356
		// (set) Token: 0x060006E3 RID: 1763 RVA: 0x0001F168 File Offset: 0x0001D368
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

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x0001F181 File Offset: 0x0001D381
		// (set) Token: 0x060006E5 RID: 1765 RVA: 0x0001F193 File Offset: 0x0001D393
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

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060006E6 RID: 1766 RVA: 0x0001F1AC File Offset: 0x0001D3AC
		// (set) Token: 0x060006E7 RID: 1767 RVA: 0x0001F1B9 File Offset: 0x0001D3B9
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

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060006E8 RID: 1768 RVA: 0x00010F39 File Offset: 0x0000F139
		public override bool IsDefinition
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060006E9 RID: 1769 RVA: 0x0001F1C2 File Offset: 0x0001D3C2
		public EventDefinition(string name, EventAttributes attributes, TypeReference eventType)
			: base(name, eventType)
		{
			this.attributes = (ushort)attributes;
			this.token = new MetadataToken(TokenType.Event);
		}

		// Token: 0x060006EA RID: 1770 RVA: 0x0001F1E4 File Offset: 0x0001D3E4
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

		// Token: 0x060006EB RID: 1771 RVA: 0x00010978 File Offset: 0x0000EB78
		public override EventDefinition Resolve()
		{
			return this;
		}

		// Token: 0x040002A5 RID: 677
		private ushort attributes;

		// Token: 0x040002A6 RID: 678
		private Collection<CustomAttribute> custom_attributes;

		// Token: 0x040002A7 RID: 679
		internal MethodDefinition add_method;

		// Token: 0x040002A8 RID: 680
		internal MethodDefinition invoke_method;

		// Token: 0x040002A9 RID: 681
		internal MethodDefinition remove_method;

		// Token: 0x040002AA RID: 682
		internal Collection<MethodDefinition> other_methods;
	}
}
