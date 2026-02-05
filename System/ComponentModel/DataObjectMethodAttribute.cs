using System;

namespace System.ComponentModel
{
	// Token: 0x02000536 RID: 1334
	[AttributeUsage(AttributeTargets.Method)]
	public sealed class DataObjectMethodAttribute : Attribute
	{
		// Token: 0x06003259 RID: 12889 RVA: 0x000E16DB File Offset: 0x000DF8DB
		public DataObjectMethodAttribute(DataObjectMethodType methodType)
			: this(methodType, false)
		{
		}

		// Token: 0x0600325A RID: 12890 RVA: 0x000E16E5 File Offset: 0x000DF8E5
		public DataObjectMethodAttribute(DataObjectMethodType methodType, bool isDefault)
		{
			this._methodType = methodType;
			this._isDefault = isDefault;
		}

		// Token: 0x17000C57 RID: 3159
		// (get) Token: 0x0600325B RID: 12891 RVA: 0x000E16FB File Offset: 0x000DF8FB
		public bool IsDefault
		{
			get
			{
				return this._isDefault;
			}
		}

		// Token: 0x17000C58 RID: 3160
		// (get) Token: 0x0600325C RID: 12892 RVA: 0x000E1703 File Offset: 0x000DF903
		public DataObjectMethodType MethodType
		{
			get
			{
				return this._methodType;
			}
		}

		// Token: 0x0600325D RID: 12893 RVA: 0x000E170C File Offset: 0x000DF90C
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DataObjectMethodAttribute dataObjectMethodAttribute = obj as DataObjectMethodAttribute;
			return dataObjectMethodAttribute != null && dataObjectMethodAttribute.MethodType == this.MethodType && dataObjectMethodAttribute.IsDefault == this.IsDefault;
		}

		// Token: 0x0600325E RID: 12894 RVA: 0x000E1748 File Offset: 0x000DF948
		public override int GetHashCode()
		{
			int methodType = (int)this._methodType;
			return methodType.GetHashCode() ^ this._isDefault.GetHashCode();
		}

		// Token: 0x0600325F RID: 12895 RVA: 0x000E1770 File Offset: 0x000DF970
		public override bool Match(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DataObjectMethodAttribute dataObjectMethodAttribute = obj as DataObjectMethodAttribute;
			return dataObjectMethodAttribute != null && dataObjectMethodAttribute.MethodType == this.MethodType;
		}

		// Token: 0x04002968 RID: 10600
		private bool _isDefault;

		// Token: 0x04002969 RID: 10601
		private DataObjectMethodType _methodType;
	}
}
