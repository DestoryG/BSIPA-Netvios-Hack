using System;

namespace System.ComponentModel
{
	// Token: 0x02000534 RID: 1332
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class DataObjectAttribute : Attribute
	{
		// Token: 0x06003248 RID: 12872 RVA: 0x000E1587 File Offset: 0x000DF787
		public DataObjectAttribute()
			: this(true)
		{
		}

		// Token: 0x06003249 RID: 12873 RVA: 0x000E1590 File Offset: 0x000DF790
		public DataObjectAttribute(bool isDataObject)
		{
			this._isDataObject = isDataObject;
		}

		// Token: 0x17000C52 RID: 3154
		// (get) Token: 0x0600324A RID: 12874 RVA: 0x000E159F File Offset: 0x000DF79F
		public bool IsDataObject
		{
			get
			{
				return this._isDataObject;
			}
		}

		// Token: 0x0600324B RID: 12875 RVA: 0x000E15A8 File Offset: 0x000DF7A8
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DataObjectAttribute dataObjectAttribute = obj as DataObjectAttribute;
			return dataObjectAttribute != null && dataObjectAttribute.IsDataObject == this.IsDataObject;
		}

		// Token: 0x0600324C RID: 12876 RVA: 0x000E15D5 File Offset: 0x000DF7D5
		public override int GetHashCode()
		{
			return this._isDataObject.GetHashCode();
		}

		// Token: 0x0600324D RID: 12877 RVA: 0x000E15E2 File Offset: 0x000DF7E2
		public override bool IsDefaultAttribute()
		{
			return this.Equals(DataObjectAttribute.Default);
		}

		// Token: 0x04002960 RID: 10592
		public static readonly DataObjectAttribute DataObject = new DataObjectAttribute(true);

		// Token: 0x04002961 RID: 10593
		public static readonly DataObjectAttribute NonDataObject = new DataObjectAttribute(false);

		// Token: 0x04002962 RID: 10594
		public static readonly DataObjectAttribute Default = DataObjectAttribute.NonDataObject;

		// Token: 0x04002963 RID: 10595
		private bool _isDataObject;
	}
}
