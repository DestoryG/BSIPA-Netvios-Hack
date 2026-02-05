using System;
using System.Collections.Generic;

namespace System.Runtime.Serialization
{
	// Token: 0x0200009C RID: 156
	internal struct ObjectReferenceStack
	{
		// Token: 0x06000AE9 RID: 2793 RVA: 0x0002DE0C File Offset: 0x0002C00C
		internal void Push(object obj)
		{
			if (this.objectArray == null)
			{
				this.objectArray = new object[4];
				object[] array = this.objectArray;
				int num = this.count;
				this.count = num + 1;
				array[num] = obj;
				return;
			}
			if (this.count < 16)
			{
				if (this.count == this.objectArray.Length)
				{
					Array.Resize<object>(ref this.objectArray, this.objectArray.Length * 2);
				}
				object[] array2 = this.objectArray;
				int num = this.count;
				this.count = num + 1;
				array2[num] = obj;
				return;
			}
			if (this.objectDictionary == null)
			{
				this.objectDictionary = new Dictionary<object, object>();
			}
			this.objectDictionary.Add(obj, null);
			this.count++;
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x0002DEC0 File Offset: 0x0002C0C0
		internal void EnsureSetAsIsReference(object obj)
		{
			if (this.count == 0)
			{
				return;
			}
			if (this.count > 16)
			{
				Dictionary<object, object> dictionary = this.objectDictionary;
				this.objectDictionary.Remove(obj);
				return;
			}
			if (this.objectArray != null && this.objectArray[this.count - 1] == obj)
			{
				if (this.isReferenceArray == null)
				{
					this.isReferenceArray = new bool[4];
				}
				else if (this.count == this.isReferenceArray.Length)
				{
					Array.Resize<bool>(ref this.isReferenceArray, this.isReferenceArray.Length * 2);
				}
				this.isReferenceArray[this.count - 1] = true;
			}
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x0002DF5A File Offset: 0x0002C15A
		internal void Pop(object obj)
		{
			if (this.count > 16)
			{
				Dictionary<object, object> dictionary = this.objectDictionary;
				this.objectDictionary.Remove(obj);
			}
			this.count--;
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x0002DF88 File Offset: 0x0002C188
		internal bool Contains(object obj)
		{
			int num = this.count;
			if (num > 16)
			{
				if (this.objectDictionary != null && this.objectDictionary.ContainsKey(obj))
				{
					return true;
				}
				num = 16;
			}
			for (int i = num - 1; i >= 0; i--)
			{
				if (obj == this.objectArray[i] && this.isReferenceArray != null && !this.isReferenceArray[i])
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000AED RID: 2797 RVA: 0x0002DFEA File Offset: 0x0002C1EA
		internal int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x040004C8 RID: 1224
		private const int MaximumArraySize = 16;

		// Token: 0x040004C9 RID: 1225
		private const int InitialArraySize = 4;

		// Token: 0x040004CA RID: 1226
		private int count;

		// Token: 0x040004CB RID: 1227
		private object[] objectArray;

		// Token: 0x040004CC RID: 1228
		private bool[] isReferenceArray;

		// Token: 0x040004CD RID: 1229
		private Dictionary<object, object> objectDictionary;
	}
}
