using System;

namespace System.ComponentModel
{
	// Token: 0x020005B0 RID: 1456
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	[Serializable]
	public sealed class ToolboxItemFilterAttribute : Attribute
	{
		// Token: 0x0600362C RID: 13868 RVA: 0x000EC59B File Offset: 0x000EA79B
		public ToolboxItemFilterAttribute(string filterString)
			: this(filterString, ToolboxItemFilterType.Allow)
		{
		}

		// Token: 0x0600362D RID: 13869 RVA: 0x000EC5A5 File Offset: 0x000EA7A5
		public ToolboxItemFilterAttribute(string filterString, ToolboxItemFilterType filterType)
		{
			if (filterString == null)
			{
				filterString = string.Empty;
			}
			this.filterString = filterString;
			this.filterType = filterType;
		}

		// Token: 0x17000D33 RID: 3379
		// (get) Token: 0x0600362E RID: 13870 RVA: 0x000EC5C5 File Offset: 0x000EA7C5
		public string FilterString
		{
			get
			{
				return this.filterString;
			}
		}

		// Token: 0x17000D34 RID: 3380
		// (get) Token: 0x0600362F RID: 13871 RVA: 0x000EC5CD File Offset: 0x000EA7CD
		public ToolboxItemFilterType FilterType
		{
			get
			{
				return this.filterType;
			}
		}

		// Token: 0x17000D35 RID: 3381
		// (get) Token: 0x06003630 RID: 13872 RVA: 0x000EC5D5 File Offset: 0x000EA7D5
		public override object TypeId
		{
			get
			{
				if (this.typeId == null)
				{
					this.typeId = base.GetType().FullName + this.filterString;
				}
				return this.typeId;
			}
		}

		// Token: 0x06003631 RID: 13873 RVA: 0x000EC604 File Offset: 0x000EA804
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			ToolboxItemFilterAttribute toolboxItemFilterAttribute = obj as ToolboxItemFilterAttribute;
			return toolboxItemFilterAttribute != null && toolboxItemFilterAttribute.FilterType.Equals(this.FilterType) && toolboxItemFilterAttribute.FilterString.Equals(this.FilterString);
		}

		// Token: 0x06003632 RID: 13874 RVA: 0x000EC655 File Offset: 0x000EA855
		public override int GetHashCode()
		{
			return this.filterString.GetHashCode();
		}

		// Token: 0x06003633 RID: 13875 RVA: 0x000EC664 File Offset: 0x000EA864
		public override bool Match(object obj)
		{
			ToolboxItemFilterAttribute toolboxItemFilterAttribute = obj as ToolboxItemFilterAttribute;
			return toolboxItemFilterAttribute != null && toolboxItemFilterAttribute.FilterString.Equals(this.FilterString);
		}

		// Token: 0x06003634 RID: 13876 RVA: 0x000EC693 File Offset: 0x000EA893
		public override string ToString()
		{
			return this.filterString + "," + Enum.GetName(typeof(ToolboxItemFilterType), this.filterType);
		}

		// Token: 0x04002A8F RID: 10895
		private ToolboxItemFilterType filterType;

		// Token: 0x04002A90 RID: 10896
		private string filterString;

		// Token: 0x04002A91 RID: 10897
		private string typeId;
	}
}
