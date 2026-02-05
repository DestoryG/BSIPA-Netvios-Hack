using System;

namespace System.ComponentModel
{
	// Token: 0x02000584 RID: 1412
	[AttributeUsage(AttributeTargets.All)]
	public sealed class ListBindableAttribute : Attribute
	{
		// Token: 0x06003420 RID: 13344 RVA: 0x000E4772 File Offset: 0x000E2972
		public ListBindableAttribute(bool listBindable)
		{
			this.listBindable = listBindable;
		}

		// Token: 0x06003421 RID: 13345 RVA: 0x000E4781 File Offset: 0x000E2981
		public ListBindableAttribute(BindableSupport flags)
		{
			this.listBindable = flags > BindableSupport.No;
			this.isDefault = flags == BindableSupport.Default;
		}

		// Token: 0x17000CBB RID: 3259
		// (get) Token: 0x06003422 RID: 13346 RVA: 0x000E479D File Offset: 0x000E299D
		public bool ListBindable
		{
			get
			{
				return this.listBindable;
			}
		}

		// Token: 0x06003423 RID: 13347 RVA: 0x000E47A8 File Offset: 0x000E29A8
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			ListBindableAttribute listBindableAttribute = obj as ListBindableAttribute;
			return listBindableAttribute != null && listBindableAttribute.ListBindable == this.listBindable;
		}

		// Token: 0x06003424 RID: 13348 RVA: 0x000E47D5 File Offset: 0x000E29D5
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06003425 RID: 13349 RVA: 0x000E47DD File Offset: 0x000E29DD
		public override bool IsDefaultAttribute()
		{
			return this.Equals(ListBindableAttribute.Default) || this.isDefault;
		}

		// Token: 0x040029C8 RID: 10696
		public static readonly ListBindableAttribute Yes = new ListBindableAttribute(true);

		// Token: 0x040029C9 RID: 10697
		public static readonly ListBindableAttribute No = new ListBindableAttribute(false);

		// Token: 0x040029CA RID: 10698
		public static readonly ListBindableAttribute Default = ListBindableAttribute.Yes;

		// Token: 0x040029CB RID: 10699
		private bool listBindable;

		// Token: 0x040029CC RID: 10700
		private bool isDefault;
	}
}
