using System;

namespace System.ComponentModel
{
	// Token: 0x020005BF RID: 1471
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class NotifyParentPropertyAttribute : Attribute
	{
		// Token: 0x06003718 RID: 14104 RVA: 0x000EF9E1 File Offset: 0x000EDBE1
		public NotifyParentPropertyAttribute(bool notifyParent)
		{
			this.notifyParent = notifyParent;
		}

		// Token: 0x17000D44 RID: 3396
		// (get) Token: 0x06003719 RID: 14105 RVA: 0x000EF9F0 File Offset: 0x000EDBF0
		public bool NotifyParent
		{
			get
			{
				return this.notifyParent;
			}
		}

		// Token: 0x0600371A RID: 14106 RVA: 0x000EF9F8 File Offset: 0x000EDBF8
		public override bool Equals(object obj)
		{
			return obj == this || (obj != null && obj is NotifyParentPropertyAttribute && ((NotifyParentPropertyAttribute)obj).NotifyParent == this.notifyParent);
		}

		// Token: 0x0600371B RID: 14107 RVA: 0x000EFA20 File Offset: 0x000EDC20
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0600371C RID: 14108 RVA: 0x000EFA28 File Offset: 0x000EDC28
		public override bool IsDefaultAttribute()
		{
			return this.Equals(NotifyParentPropertyAttribute.Default);
		}

		// Token: 0x04002AC1 RID: 10945
		public static readonly NotifyParentPropertyAttribute Yes = new NotifyParentPropertyAttribute(true);

		// Token: 0x04002AC2 RID: 10946
		public static readonly NotifyParentPropertyAttribute No = new NotifyParentPropertyAttribute(false);

		// Token: 0x04002AC3 RID: 10947
		public static readonly NotifyParentPropertyAttribute Default = NotifyParentPropertyAttribute.No;

		// Token: 0x04002AC4 RID: 10948
		private bool notifyParent;
	}
}
