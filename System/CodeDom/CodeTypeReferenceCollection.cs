using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000666 RID: 1638
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeTypeReferenceCollection : CollectionBase
	{
		// Token: 0x06003B58 RID: 15192 RVA: 0x000F56C9 File Offset: 0x000F38C9
		public CodeTypeReferenceCollection()
		{
		}

		// Token: 0x06003B59 RID: 15193 RVA: 0x000F56D1 File Offset: 0x000F38D1
		public CodeTypeReferenceCollection(CodeTypeReferenceCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x06003B5A RID: 15194 RVA: 0x000F56E0 File Offset: 0x000F38E0
		public CodeTypeReferenceCollection(CodeTypeReference[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x17000E4D RID: 3661
		public CodeTypeReference this[int index]
		{
			get
			{
				return (CodeTypeReference)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06003B5D RID: 15197 RVA: 0x000F5711 File Offset: 0x000F3911
		public int Add(CodeTypeReference value)
		{
			return base.List.Add(value);
		}

		// Token: 0x06003B5E RID: 15198 RVA: 0x000F571F File Offset: 0x000F391F
		public void Add(string value)
		{
			this.Add(new CodeTypeReference(value));
		}

		// Token: 0x06003B5F RID: 15199 RVA: 0x000F572E File Offset: 0x000F392E
		public void Add(Type value)
		{
			this.Add(new CodeTypeReference(value));
		}

		// Token: 0x06003B60 RID: 15200 RVA: 0x000F5740 File Offset: 0x000F3940
		public void AddRange(CodeTypeReference[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			for (int i = 0; i < value.Length; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x06003B61 RID: 15201 RVA: 0x000F5774 File Offset: 0x000F3974
		public void AddRange(CodeTypeReferenceCollection value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			int count = value.Count;
			for (int i = 0; i < count; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x06003B62 RID: 15202 RVA: 0x000F57B0 File Offset: 0x000F39B0
		public bool Contains(CodeTypeReference value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06003B63 RID: 15203 RVA: 0x000F57BE File Offset: 0x000F39BE
		public void CopyTo(CodeTypeReference[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06003B64 RID: 15204 RVA: 0x000F57CD File Offset: 0x000F39CD
		public int IndexOf(CodeTypeReference value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x06003B65 RID: 15205 RVA: 0x000F57DB File Offset: 0x000F39DB
		public void Insert(int index, CodeTypeReference value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x06003B66 RID: 15206 RVA: 0x000F57EA File Offset: 0x000F39EA
		public void Remove(CodeTypeReference value)
		{
			base.List.Remove(value);
		}
	}
}
