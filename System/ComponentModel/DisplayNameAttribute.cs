using System;

namespace System.ComponentModel
{
	// Token: 0x02000547 RID: 1351
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event)]
	public class DisplayNameAttribute : Attribute
	{
		// Token: 0x060032CB RID: 13003 RVA: 0x000E2789 File Offset: 0x000E0989
		public DisplayNameAttribute()
			: this(string.Empty)
		{
		}

		// Token: 0x060032CC RID: 13004 RVA: 0x000E2796 File Offset: 0x000E0996
		public DisplayNameAttribute(string displayName)
		{
			this._displayName = displayName;
		}

		// Token: 0x17000C6A RID: 3178
		// (get) Token: 0x060032CD RID: 13005 RVA: 0x000E27A5 File Offset: 0x000E09A5
		public virtual string DisplayName
		{
			get
			{
				return this.DisplayNameValue;
			}
		}

		// Token: 0x17000C6B RID: 3179
		// (get) Token: 0x060032CE RID: 13006 RVA: 0x000E27AD File Offset: 0x000E09AD
		// (set) Token: 0x060032CF RID: 13007 RVA: 0x000E27B5 File Offset: 0x000E09B5
		protected string DisplayNameValue
		{
			get
			{
				return this._displayName;
			}
			set
			{
				this._displayName = value;
			}
		}

		// Token: 0x060032D0 RID: 13008 RVA: 0x000E27C0 File Offset: 0x000E09C0
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DisplayNameAttribute displayNameAttribute = obj as DisplayNameAttribute;
			return displayNameAttribute != null && displayNameAttribute.DisplayName == this.DisplayName;
		}

		// Token: 0x060032D1 RID: 13009 RVA: 0x000E27F0 File Offset: 0x000E09F0
		public override int GetHashCode()
		{
			return this.DisplayName.GetHashCode();
		}

		// Token: 0x060032D2 RID: 13010 RVA: 0x000E27FD File Offset: 0x000E09FD
		public override bool IsDefaultAttribute()
		{
			return this.Equals(DisplayNameAttribute.Default);
		}

		// Token: 0x04002994 RID: 10644
		public static readonly DisplayNameAttribute Default = new DisplayNameAttribute();

		// Token: 0x04002995 RID: 10645
		private string _displayName;
	}
}
