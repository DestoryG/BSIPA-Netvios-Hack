using System;
using System.Collections;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000553 RID: 1363
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	internal sealed class ExtendedPropertyDescriptor : PropertyDescriptor
	{
		// Token: 0x0600333A RID: 13114 RVA: 0x000E3838 File Offset: 0x000E1A38
		public ExtendedPropertyDescriptor(ReflectPropertyDescriptor extenderInfo, Type receiverType, IExtenderProvider provider, Attribute[] attributes)
			: base(extenderInfo, attributes)
		{
			ArrayList arrayList = new ArrayList(this.AttributeArray);
			arrayList.Add(ExtenderProvidedPropertyAttribute.Create(extenderInfo, receiverType, provider));
			if (extenderInfo.IsReadOnly)
			{
				arrayList.Add(ReadOnlyAttribute.Yes);
			}
			Attribute[] array = new Attribute[arrayList.Count];
			arrayList.CopyTo(array, 0);
			this.AttributeArray = array;
			this.extenderInfo = extenderInfo;
			this.provider = provider;
		}

		// Token: 0x0600333B RID: 13115 RVA: 0x000E38A8 File Offset: 0x000E1AA8
		public ExtendedPropertyDescriptor(PropertyDescriptor extender, Attribute[] attributes)
			: base(extender, attributes)
		{
			ExtenderProvidedPropertyAttribute extenderProvidedPropertyAttribute = extender.Attributes[typeof(ExtenderProvidedPropertyAttribute)] as ExtenderProvidedPropertyAttribute;
			ReflectPropertyDescriptor reflectPropertyDescriptor = extenderProvidedPropertyAttribute.ExtenderProperty as ReflectPropertyDescriptor;
			this.extenderInfo = reflectPropertyDescriptor;
			this.provider = extenderProvidedPropertyAttribute.Provider;
		}

		// Token: 0x0600333C RID: 13116 RVA: 0x000E38F7 File Offset: 0x000E1AF7
		public override bool CanResetValue(object comp)
		{
			return this.extenderInfo.ExtenderCanResetValue(this.provider, comp);
		}

		// Token: 0x17000C84 RID: 3204
		// (get) Token: 0x0600333D RID: 13117 RVA: 0x000E390B File Offset: 0x000E1B0B
		public override Type ComponentType
		{
			get
			{
				return this.extenderInfo.ComponentType;
			}
		}

		// Token: 0x17000C85 RID: 3205
		// (get) Token: 0x0600333E RID: 13118 RVA: 0x000E3918 File Offset: 0x000E1B18
		public override bool IsReadOnly
		{
			get
			{
				return this.Attributes[typeof(ReadOnlyAttribute)].Equals(ReadOnlyAttribute.Yes);
			}
		}

		// Token: 0x17000C86 RID: 3206
		// (get) Token: 0x0600333F RID: 13119 RVA: 0x000E3939 File Offset: 0x000E1B39
		public override Type PropertyType
		{
			get
			{
				return this.extenderInfo.ExtenderGetType(this.provider);
			}
		}

		// Token: 0x17000C87 RID: 3207
		// (get) Token: 0x06003340 RID: 13120 RVA: 0x000E394C File Offset: 0x000E1B4C
		public override string DisplayName
		{
			get
			{
				string text = base.DisplayName;
				DisplayNameAttribute displayNameAttribute = this.Attributes[typeof(DisplayNameAttribute)] as DisplayNameAttribute;
				if (displayNameAttribute == null || displayNameAttribute.IsDefaultAttribute())
				{
					ISite site = MemberDescriptor.GetSite(this.provider);
					if (site != null)
					{
						string name = site.Name;
						if (name != null && name.Length > 0)
						{
							text = SR.GetString("MetaExtenderName", new object[] { text, name });
						}
					}
				}
				return text;
			}
		}

		// Token: 0x06003341 RID: 13121 RVA: 0x000E39C2 File Offset: 0x000E1BC2
		public override object GetValue(object comp)
		{
			return this.extenderInfo.ExtenderGetValue(this.provider, comp);
		}

		// Token: 0x06003342 RID: 13122 RVA: 0x000E39D6 File Offset: 0x000E1BD6
		public override void ResetValue(object comp)
		{
			this.extenderInfo.ExtenderResetValue(this.provider, comp, this);
		}

		// Token: 0x06003343 RID: 13123 RVA: 0x000E39EB File Offset: 0x000E1BEB
		public override void SetValue(object component, object value)
		{
			this.extenderInfo.ExtenderSetValue(this.provider, component, value, this);
		}

		// Token: 0x06003344 RID: 13124 RVA: 0x000E3A01 File Offset: 0x000E1C01
		public override bool ShouldSerializeValue(object comp)
		{
			return this.extenderInfo.ExtenderShouldSerializeValue(this.provider, comp);
		}

		// Token: 0x040029AC RID: 10668
		private readonly ReflectPropertyDescriptor extenderInfo;

		// Token: 0x040029AD RID: 10669
		private readonly IExtenderProvider provider;
	}
}
