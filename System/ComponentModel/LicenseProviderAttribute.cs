using System;

namespace System.ComponentModel
{
	// Token: 0x02000581 RID: 1409
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class LicenseProviderAttribute : Attribute
	{
		// Token: 0x06003414 RID: 13332 RVA: 0x000E453E File Offset: 0x000E273E
		public LicenseProviderAttribute()
			: this(null)
		{
		}

		// Token: 0x06003415 RID: 13333 RVA: 0x000E4547 File Offset: 0x000E2747
		public LicenseProviderAttribute(string typeName)
		{
			this.licenseProviderName = typeName;
		}

		// Token: 0x06003416 RID: 13334 RVA: 0x000E4556 File Offset: 0x000E2756
		public LicenseProviderAttribute(Type type)
		{
			this.licenseProviderType = type;
		}

		// Token: 0x17000CB9 RID: 3257
		// (get) Token: 0x06003417 RID: 13335 RVA: 0x000E4565 File Offset: 0x000E2765
		public Type LicenseProvider
		{
			get
			{
				if (this.licenseProviderType == null && this.licenseProviderName != null)
				{
					this.licenseProviderType = Type.GetType(this.licenseProviderName);
				}
				return this.licenseProviderType;
			}
		}

		// Token: 0x17000CBA RID: 3258
		// (get) Token: 0x06003418 RID: 13336 RVA: 0x000E4594 File Offset: 0x000E2794
		public override object TypeId
		{
			get
			{
				string fullName = this.licenseProviderName;
				if (fullName == null && this.licenseProviderType != null)
				{
					fullName = this.licenseProviderType.FullName;
				}
				return base.GetType().FullName + fullName;
			}
		}

		// Token: 0x06003419 RID: 13337 RVA: 0x000E45D8 File Offset: 0x000E27D8
		public override bool Equals(object value)
		{
			if (value is LicenseProviderAttribute && value != null)
			{
				Type licenseProvider = ((LicenseProviderAttribute)value).LicenseProvider;
				if (licenseProvider == this.LicenseProvider)
				{
					return true;
				}
				if (licenseProvider != null && licenseProvider.Equals(this.LicenseProvider))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600341A RID: 13338 RVA: 0x000E4626 File Offset: 0x000E2826
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x040029C2 RID: 10690
		public static readonly LicenseProviderAttribute Default = new LicenseProviderAttribute();

		// Token: 0x040029C3 RID: 10691
		private Type licenseProviderType;

		// Token: 0x040029C4 RID: 10692
		private string licenseProviderName;
	}
}
