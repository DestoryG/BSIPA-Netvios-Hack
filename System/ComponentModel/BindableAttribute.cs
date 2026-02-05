using System;

namespace System.ComponentModel
{
	// Token: 0x0200051A RID: 1306
	[AttributeUsage(AttributeTargets.All)]
	public sealed class BindableAttribute : Attribute
	{
		// Token: 0x06003175 RID: 12661 RVA: 0x000DF6E4 File Offset: 0x000DD8E4
		public BindableAttribute(bool bindable)
			: this(bindable, BindingDirection.OneWay)
		{
		}

		// Token: 0x06003176 RID: 12662 RVA: 0x000DF6EE File Offset: 0x000DD8EE
		public BindableAttribute(bool bindable, BindingDirection direction)
		{
			this.bindable = bindable;
			this.direction = direction;
		}

		// Token: 0x06003177 RID: 12663 RVA: 0x000DF704 File Offset: 0x000DD904
		public BindableAttribute(BindableSupport flags)
			: this(flags, BindingDirection.OneWay)
		{
		}

		// Token: 0x06003178 RID: 12664 RVA: 0x000DF70E File Offset: 0x000DD90E
		public BindableAttribute(BindableSupport flags, BindingDirection direction)
		{
			this.bindable = flags > BindableSupport.No;
			this.isDefault = flags == BindableSupport.Default;
			this.direction = direction;
		}

		// Token: 0x17000C16 RID: 3094
		// (get) Token: 0x06003179 RID: 12665 RVA: 0x000DF731 File Offset: 0x000DD931
		public bool Bindable
		{
			get
			{
				return this.bindable;
			}
		}

		// Token: 0x17000C17 RID: 3095
		// (get) Token: 0x0600317A RID: 12666 RVA: 0x000DF739 File Offset: 0x000DD939
		public BindingDirection Direction
		{
			get
			{
				return this.direction;
			}
		}

		// Token: 0x0600317B RID: 12667 RVA: 0x000DF741 File Offset: 0x000DD941
		public override bool Equals(object obj)
		{
			return obj == this || (obj != null && obj is BindableAttribute && ((BindableAttribute)obj).Bindable == this.bindable);
		}

		// Token: 0x0600317C RID: 12668 RVA: 0x000DF769 File Offset: 0x000DD969
		public override int GetHashCode()
		{
			return this.bindable.GetHashCode();
		}

		// Token: 0x0600317D RID: 12669 RVA: 0x000DF776 File Offset: 0x000DD976
		public override bool IsDefaultAttribute()
		{
			return this.Equals(BindableAttribute.Default) || this.isDefault;
		}

		// Token: 0x04002918 RID: 10520
		public static readonly BindableAttribute Yes = new BindableAttribute(true);

		// Token: 0x04002919 RID: 10521
		public static readonly BindableAttribute No = new BindableAttribute(false);

		// Token: 0x0400291A RID: 10522
		public static readonly BindableAttribute Default = BindableAttribute.No;

		// Token: 0x0400291B RID: 10523
		private bool bindable;

		// Token: 0x0400291C RID: 10524
		private bool isDefault;

		// Token: 0x0400291D RID: 10525
		private BindingDirection direction;
	}
}
