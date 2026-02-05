using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000647 RID: 1607
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeNamespaceImportCollection : IList, ICollection, IEnumerable
	{
		// Token: 0x17000E08 RID: 3592
		public CodeNamespaceImport this[int index]
		{
			get
			{
				return (CodeNamespaceImport)this.data[index];
			}
			set
			{
				this.data[index] = value;
				this.SyncKeys();
			}
		}

		// Token: 0x17000E09 RID: 3593
		// (get) Token: 0x06003A62 RID: 14946 RVA: 0x000F3E1F File Offset: 0x000F201F
		public int Count
		{
			get
			{
				return this.data.Count;
			}
		}

		// Token: 0x17000E0A RID: 3594
		// (get) Token: 0x06003A63 RID: 14947 RVA: 0x000F3E2C File Offset: 0x000F202C
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000E0B RID: 3595
		// (get) Token: 0x06003A64 RID: 14948 RVA: 0x000F3E2F File Offset: 0x000F202F
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003A65 RID: 14949 RVA: 0x000F3E32 File Offset: 0x000F2032
		public void Add(CodeNamespaceImport value)
		{
			if (!this.keys.ContainsKey(value.Namespace))
			{
				this.keys[value.Namespace] = value;
				this.data.Add(value);
			}
		}

		// Token: 0x06003A66 RID: 14950 RVA: 0x000F3E68 File Offset: 0x000F2068
		public void AddRange(CodeNamespaceImport[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			foreach (CodeNamespaceImport codeNamespaceImport in value)
			{
				this.Add(codeNamespaceImport);
			}
		}

		// Token: 0x06003A67 RID: 14951 RVA: 0x000F3E9E File Offset: 0x000F209E
		public void Clear()
		{
			this.data.Clear();
			this.keys.Clear();
		}

		// Token: 0x06003A68 RID: 14952 RVA: 0x000F3EB8 File Offset: 0x000F20B8
		private void SyncKeys()
		{
			this.keys = new Hashtable(StringComparer.OrdinalIgnoreCase);
			foreach (object obj in this)
			{
				CodeNamespaceImport codeNamespaceImport = (CodeNamespaceImport)obj;
				this.keys[codeNamespaceImport.Namespace] = codeNamespaceImport;
			}
		}

		// Token: 0x06003A69 RID: 14953 RVA: 0x000F3F28 File Offset: 0x000F2128
		public IEnumerator GetEnumerator()
		{
			return this.data.GetEnumerator();
		}

		// Token: 0x17000E0C RID: 3596
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				this[index] = (CodeNamespaceImport)value;
				this.SyncKeys();
			}
		}

		// Token: 0x17000E0D RID: 3597
		// (get) Token: 0x06003A6C RID: 14956 RVA: 0x000F3F53 File Offset: 0x000F2153
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x17000E0E RID: 3598
		// (get) Token: 0x06003A6D RID: 14957 RVA: 0x000F3F5B File Offset: 0x000F215B
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000E0F RID: 3599
		// (get) Token: 0x06003A6E RID: 14958 RVA: 0x000F3F5E File Offset: 0x000F215E
		object ICollection.SyncRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06003A6F RID: 14959 RVA: 0x000F3F61 File Offset: 0x000F2161
		void ICollection.CopyTo(Array array, int index)
		{
			this.data.CopyTo(array, index);
		}

		// Token: 0x06003A70 RID: 14960 RVA: 0x000F3F70 File Offset: 0x000F2170
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06003A71 RID: 14961 RVA: 0x000F3F78 File Offset: 0x000F2178
		int IList.Add(object value)
		{
			return this.data.Add((CodeNamespaceImport)value);
		}

		// Token: 0x06003A72 RID: 14962 RVA: 0x000F3F8B File Offset: 0x000F218B
		void IList.Clear()
		{
			this.Clear();
		}

		// Token: 0x06003A73 RID: 14963 RVA: 0x000F3F93 File Offset: 0x000F2193
		bool IList.Contains(object value)
		{
			return this.data.Contains(value);
		}

		// Token: 0x06003A74 RID: 14964 RVA: 0x000F3FA1 File Offset: 0x000F21A1
		int IList.IndexOf(object value)
		{
			return this.data.IndexOf((CodeNamespaceImport)value);
		}

		// Token: 0x06003A75 RID: 14965 RVA: 0x000F3FB4 File Offset: 0x000F21B4
		void IList.Insert(int index, object value)
		{
			this.data.Insert(index, (CodeNamespaceImport)value);
			this.SyncKeys();
		}

		// Token: 0x06003A76 RID: 14966 RVA: 0x000F3FCE File Offset: 0x000F21CE
		void IList.Remove(object value)
		{
			this.data.Remove((CodeNamespaceImport)value);
			this.SyncKeys();
		}

		// Token: 0x06003A77 RID: 14967 RVA: 0x000F3FE7 File Offset: 0x000F21E7
		void IList.RemoveAt(int index)
		{
			this.data.RemoveAt(index);
			this.SyncKeys();
		}

		// Token: 0x04002BFB RID: 11259
		private ArrayList data = new ArrayList();

		// Token: 0x04002BFC RID: 11260
		private Hashtable keys = new Hashtable(StringComparer.OrdinalIgnoreCase);
	}
}
