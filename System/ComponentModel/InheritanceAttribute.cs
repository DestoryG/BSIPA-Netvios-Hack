using System;

namespace System.ComponentModel
{
	// Token: 0x020005BD RID: 1469
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event)]
	public sealed class InheritanceAttribute : Attribute
	{
		// Token: 0x06003710 RID: 14096 RVA: 0x000EF91C File Offset: 0x000EDB1C
		public InheritanceAttribute()
		{
			this.inheritanceLevel = InheritanceAttribute.Default.inheritanceLevel;
		}

		// Token: 0x06003711 RID: 14097 RVA: 0x000EF934 File Offset: 0x000EDB34
		public InheritanceAttribute(InheritanceLevel inheritanceLevel)
		{
			this.inheritanceLevel = inheritanceLevel;
		}

		// Token: 0x17000D43 RID: 3395
		// (get) Token: 0x06003712 RID: 14098 RVA: 0x000EF943 File Offset: 0x000EDB43
		public InheritanceLevel InheritanceLevel
		{
			get
			{
				return this.inheritanceLevel;
			}
		}

		// Token: 0x06003713 RID: 14099 RVA: 0x000EF94C File Offset: 0x000EDB4C
		public override bool Equals(object value)
		{
			if (value == this)
			{
				return true;
			}
			if (!(value is InheritanceAttribute))
			{
				return false;
			}
			InheritanceLevel inheritanceLevel = ((InheritanceAttribute)value).InheritanceLevel;
			return inheritanceLevel == this.inheritanceLevel;
		}

		// Token: 0x06003714 RID: 14100 RVA: 0x000EF97E File Offset: 0x000EDB7E
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06003715 RID: 14101 RVA: 0x000EF986 File Offset: 0x000EDB86
		public override bool IsDefaultAttribute()
		{
			return this.Equals(InheritanceAttribute.Default);
		}

		// Token: 0x06003716 RID: 14102 RVA: 0x000EF993 File Offset: 0x000EDB93
		public override string ToString()
		{
			return TypeDescriptor.GetConverter(typeof(InheritanceLevel)).ConvertToString(this.InheritanceLevel);
		}

		// Token: 0x04002AB8 RID: 10936
		private readonly InheritanceLevel inheritanceLevel;

		// Token: 0x04002AB9 RID: 10937
		public static readonly InheritanceAttribute Inherited = new InheritanceAttribute(InheritanceLevel.Inherited);

		// Token: 0x04002ABA RID: 10938
		public static readonly InheritanceAttribute InheritedReadOnly = new InheritanceAttribute(InheritanceLevel.InheritedReadOnly);

		// Token: 0x04002ABB RID: 10939
		public static readonly InheritanceAttribute NotInherited = new InheritanceAttribute(InheritanceLevel.NotInherited);

		// Token: 0x04002ABC RID: 10940
		public static readonly InheritanceAttribute Default = InheritanceAttribute.NotInherited;
	}
}
