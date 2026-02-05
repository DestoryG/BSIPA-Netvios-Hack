using System;
using System.Collections;
using System.Collections.Generic;

namespace IniParser.Model
{
	// Token: 0x0200000A RID: 10
	public class KeyDataCollection : ICloneable, IEnumerable<KeyData>, IEnumerable
	{
		// Token: 0x06000055 RID: 85 RVA: 0x000030E8 File Offset: 0x000012E8
		public KeyDataCollection()
			: this(EqualityComparer<string>.Default)
		{
		}

		// Token: 0x06000056 RID: 86 RVA: 0x000030F7 File Offset: 0x000012F7
		public KeyDataCollection(IEqualityComparer<string> searchComparer)
		{
			this._searchComparer = searchComparer;
			this._keyData = new Dictionary<string, KeyData>(this._searchComparer);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x0000311C File Offset: 0x0000131C
		public KeyDataCollection(KeyDataCollection ori, IEqualityComparer<string> searchComparer)
			: this(searchComparer)
		{
			foreach (KeyData keyData in ori)
			{
				bool flag = this._keyData.ContainsKey(keyData.KeyName);
				if (flag)
				{
					this._keyData[keyData.KeyName] = (KeyData)keyData.Clone();
				}
				else
				{
					this._keyData.Add(keyData.KeyName, (KeyData)keyData.Clone());
				}
			}
		}

		// Token: 0x17000012 RID: 18
		public string this[string keyName]
		{
			get
			{
				bool flag = this._keyData.ContainsKey(keyName);
				string text;
				if (flag)
				{
					text = this._keyData[keyName].Value;
				}
				else
				{
					text = null;
				}
				return text;
			}
			set
			{
				bool flag = !this._keyData.ContainsKey(keyName);
				if (flag)
				{
					this.AddKey(keyName);
				}
				this._keyData[keyName].Value = value;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00003238 File Offset: 0x00001438
		public int Count
		{
			get
			{
				return this._keyData.Count;
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003258 File Offset: 0x00001458
		public bool AddKey(string keyName)
		{
			bool flag = !this._keyData.ContainsKey(keyName);
			bool flag2;
			if (flag)
			{
				this._keyData.Add(keyName, new KeyData(keyName));
				flag2 = true;
			}
			else
			{
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003298 File Offset: 0x00001498
		[Obsolete("Pottentially buggy method! Use AddKey(KeyData keyData) instead (See comments in code for an explanation of the bug)")]
		public bool AddKey(string keyName, KeyData keyData)
		{
			bool flag = this.AddKey(keyName);
			bool flag2;
			if (flag)
			{
				this._keyData[keyName] = keyData;
				flag2 = true;
			}
			else
			{
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000032CC File Offset: 0x000014CC
		public bool AddKey(KeyData keyData)
		{
			bool flag = this.AddKey(keyData.KeyName);
			bool flag2;
			if (flag)
			{
				this._keyData[keyData.KeyName] = keyData;
				flag2 = true;
			}
			else
			{
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003308 File Offset: 0x00001508
		public bool AddKey(string keyName, string keyValue)
		{
			bool flag = this.AddKey(keyName);
			bool flag2;
			if (flag)
			{
				this._keyData[keyName].Value = keyValue;
				flag2 = true;
			}
			else
			{
				flag2 = false;
			}
			return flag2;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003340 File Offset: 0x00001540
		public void ClearComments()
		{
			foreach (KeyData keyData in this)
			{
				keyData.Comments.Clear();
			}
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003394 File Offset: 0x00001594
		public bool ContainsKey(string keyName)
		{
			return this._keyData.ContainsKey(keyName);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000033B4 File Offset: 0x000015B4
		public KeyData GetKeyData(string keyName)
		{
			bool flag = this._keyData.ContainsKey(keyName);
			KeyData keyData;
			if (flag)
			{
				keyData = this._keyData[keyName];
			}
			else
			{
				keyData = null;
			}
			return keyData;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000033E8 File Offset: 0x000015E8
		public void Merge(KeyDataCollection keyDataToMerge)
		{
			foreach (KeyData keyData in keyDataToMerge)
			{
				this.AddKey(keyData.KeyName);
				this.GetKeyData(keyData.KeyName).Comments.AddRange(keyData.Comments);
				this[keyData.KeyName] = keyData.Value;
			}
		}

		// Token: 0x06000063 RID: 99 RVA: 0x0000346C File Offset: 0x0000166C
		public void RemoveAllKeys()
		{
			this._keyData.Clear();
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000347C File Offset: 0x0000167C
		public bool RemoveKey(string keyName)
		{
			return this._keyData.Remove(keyName);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x0000349C File Offset: 0x0000169C
		public void SetKeyData(KeyData data)
		{
			bool flag = data == null;
			if (!flag)
			{
				bool flag2 = this._keyData.ContainsKey(data.KeyName);
				if (flag2)
				{
					this.RemoveKey(data.KeyName);
				}
				this.AddKey(data);
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000034DE File Offset: 0x000016DE
		public IEnumerator<KeyData> GetEnumerator()
		{
			foreach (string key in this._keyData.Keys)
			{
				yield return this._keyData[key];
				key = null;
			}
			Dictionary<string, KeyData>.KeyCollection.Enumerator enumerator = default(Dictionary<string, KeyData>.KeyCollection.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x000034F0 File Offset: 0x000016F0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._keyData.GetEnumerator();
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00003514 File Offset: 0x00001714
		public object Clone()
		{
			return new KeyDataCollection(this, this._searchComparer);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00003534 File Offset: 0x00001734
		internal KeyData GetLast()
		{
			KeyData keyData = null;
			bool flag = this._keyData.Keys.Count <= 0;
			KeyData keyData2;
			if (flag)
			{
				keyData2 = keyData;
			}
			else
			{
				foreach (string text in this._keyData.Keys)
				{
					keyData = this._keyData[text];
				}
				keyData2 = keyData;
			}
			return keyData2;
		}

		// Token: 0x04000011 RID: 17
		private IEqualityComparer<string> _searchComparer;

		// Token: 0x04000012 RID: 18
		private readonly Dictionary<string, KeyData> _keyData;
	}
}
