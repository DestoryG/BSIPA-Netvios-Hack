using System;

namespace System.ComponentModel
{
	// Token: 0x020005AB RID: 1451
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class SettingsBindableAttribute : Attribute
	{
		// Token: 0x06003615 RID: 13845 RVA: 0x000EC2C5 File Offset: 0x000EA4C5
		public SettingsBindableAttribute(bool bindable)
		{
			this._bindable = bindable;
		}

		// Token: 0x17000D30 RID: 3376
		// (get) Token: 0x06003616 RID: 13846 RVA: 0x000EC2D4 File Offset: 0x000EA4D4
		public bool Bindable
		{
			get
			{
				return this._bindable;
			}
		}

		// Token: 0x06003617 RID: 13847 RVA: 0x000EC2DC File Offset: 0x000EA4DC
		public override bool Equals(object obj)
		{
			return obj == this || (obj != null && obj is SettingsBindableAttribute && ((SettingsBindableAttribute)obj).Bindable == this._bindable);
		}

		// Token: 0x06003618 RID: 13848 RVA: 0x000EC304 File Offset: 0x000EA504
		public override int GetHashCode()
		{
			return this._bindable.GetHashCode();
		}

		// Token: 0x04002A8C RID: 10892
		public static readonly SettingsBindableAttribute Yes = new SettingsBindableAttribute(true);

		// Token: 0x04002A8D RID: 10893
		public static readonly SettingsBindableAttribute No = new SettingsBindableAttribute(false);

		// Token: 0x04002A8E RID: 10894
		private bool _bindable;
	}
}
