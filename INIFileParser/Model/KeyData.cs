using System;
using System.Collections.Generic;

namespace IniParser.Model
{
	// Token: 0x02000009 RID: 9
	public class KeyData : ICloneable
	{
		// Token: 0x0600004C RID: 76 RVA: 0x00002FCC File Offset: 0x000011CC
		public KeyData(string keyName)
		{
			bool flag = string.IsNullOrEmpty(keyName);
			if (flag)
			{
				throw new ArgumentException("key name can not be empty");
			}
			this._comments = new List<string>();
			this._value = string.Empty;
			this._keyName = keyName;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003013 File Offset: 0x00001213
		public KeyData(KeyData ori)
		{
			this._value = ori._value;
			this._keyName = ori._keyName;
			this._comments = new List<string>(ori._comments);
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00003048 File Offset: 0x00001248
		// (set) Token: 0x0600004F RID: 79 RVA: 0x00003060 File Offset: 0x00001260
		public List<string> Comments
		{
			get
			{
				return this._comments;
			}
			set
			{
				this._comments = new List<string>(value);
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00003070 File Offset: 0x00001270
		// (set) Token: 0x06000051 RID: 81 RVA: 0x00003088 File Offset: 0x00001288
		public string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00003094 File Offset: 0x00001294
		// (set) Token: 0x06000053 RID: 83 RVA: 0x000030AC File Offset: 0x000012AC
		public string KeyName
		{
			get
			{
				return this._keyName;
			}
			set
			{
				bool flag = value != string.Empty;
				if (flag)
				{
					this._keyName = value;
				}
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000030D0 File Offset: 0x000012D0
		public object Clone()
		{
			return new KeyData(this);
		}

		// Token: 0x0400000E RID: 14
		private List<string> _comments;

		// Token: 0x0400000F RID: 15
		private string _value;

		// Token: 0x04000010 RID: 16
		private string _keyName;
	}
}
