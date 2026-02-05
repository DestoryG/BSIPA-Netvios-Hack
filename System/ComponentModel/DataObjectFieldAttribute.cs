using System;

namespace System.ComponentModel
{
	// Token: 0x02000535 RID: 1333
	[AttributeUsage(AttributeTargets.Property)]
	public sealed class DataObjectFieldAttribute : Attribute
	{
		// Token: 0x0600324F RID: 12879 RVA: 0x000E1611 File Offset: 0x000DF811
		public DataObjectFieldAttribute(bool primaryKey)
			: this(primaryKey, false, false, -1)
		{
		}

		// Token: 0x06003250 RID: 12880 RVA: 0x000E161D File Offset: 0x000DF81D
		public DataObjectFieldAttribute(bool primaryKey, bool isIdentity)
			: this(primaryKey, isIdentity, false, -1)
		{
		}

		// Token: 0x06003251 RID: 12881 RVA: 0x000E1629 File Offset: 0x000DF829
		public DataObjectFieldAttribute(bool primaryKey, bool isIdentity, bool isNullable)
			: this(primaryKey, isIdentity, isNullable, -1)
		{
		}

		// Token: 0x06003252 RID: 12882 RVA: 0x000E1635 File Offset: 0x000DF835
		public DataObjectFieldAttribute(bool primaryKey, bool isIdentity, bool isNullable, int length)
		{
			this._primaryKey = primaryKey;
			this._isIdentity = isIdentity;
			this._isNullable = isNullable;
			this._length = length;
		}

		// Token: 0x17000C53 RID: 3155
		// (get) Token: 0x06003253 RID: 12883 RVA: 0x000E165A File Offset: 0x000DF85A
		public bool IsIdentity
		{
			get
			{
				return this._isIdentity;
			}
		}

		// Token: 0x17000C54 RID: 3156
		// (get) Token: 0x06003254 RID: 12884 RVA: 0x000E1662 File Offset: 0x000DF862
		public bool IsNullable
		{
			get
			{
				return this._isNullable;
			}
		}

		// Token: 0x17000C55 RID: 3157
		// (get) Token: 0x06003255 RID: 12885 RVA: 0x000E166A File Offset: 0x000DF86A
		public int Length
		{
			get
			{
				return this._length;
			}
		}

		// Token: 0x17000C56 RID: 3158
		// (get) Token: 0x06003256 RID: 12886 RVA: 0x000E1672 File Offset: 0x000DF872
		public bool PrimaryKey
		{
			get
			{
				return this._primaryKey;
			}
		}

		// Token: 0x06003257 RID: 12887 RVA: 0x000E167C File Offset: 0x000DF87C
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DataObjectFieldAttribute dataObjectFieldAttribute = obj as DataObjectFieldAttribute;
			return dataObjectFieldAttribute != null && dataObjectFieldAttribute.IsIdentity == this.IsIdentity && dataObjectFieldAttribute.IsNullable == this.IsNullable && dataObjectFieldAttribute.Length == this.Length && dataObjectFieldAttribute.PrimaryKey == this.PrimaryKey;
		}

		// Token: 0x06003258 RID: 12888 RVA: 0x000E16D3 File Offset: 0x000DF8D3
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x04002964 RID: 10596
		private bool _primaryKey;

		// Token: 0x04002965 RID: 10597
		private bool _isIdentity;

		// Token: 0x04002966 RID: 10598
		private bool _isNullable;

		// Token: 0x04002967 RID: 10599
		private int _length;
	}
}
