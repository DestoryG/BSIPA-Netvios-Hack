using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000645 RID: 1605
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeNamespaceCollection : CollectionBase
	{
		// Token: 0x06003A4D RID: 14925 RVA: 0x000F3CA2 File Offset: 0x000F1EA2
		public CodeNamespaceCollection()
		{
		}

		// Token: 0x06003A4E RID: 14926 RVA: 0x000F3CAA File Offset: 0x000F1EAA
		public CodeNamespaceCollection(CodeNamespaceCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x06003A4F RID: 14927 RVA: 0x000F3CB9 File Offset: 0x000F1EB9
		public CodeNamespaceCollection(CodeNamespace[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x17000E05 RID: 3589
		public CodeNamespace this[int index]
		{
			get
			{
				return (CodeNamespace)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06003A52 RID: 14930 RVA: 0x000F3CEA File Offset: 0x000F1EEA
		public int Add(CodeNamespace value)
		{
			return base.List.Add(value);
		}

		// Token: 0x06003A53 RID: 14931 RVA: 0x000F3CF8 File Offset: 0x000F1EF8
		public void AddRange(CodeNamespace[] value)
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

		// Token: 0x06003A54 RID: 14932 RVA: 0x000F3D2C File Offset: 0x000F1F2C
		public void AddRange(CodeNamespaceCollection value)
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

		// Token: 0x06003A55 RID: 14933 RVA: 0x000F3D68 File Offset: 0x000F1F68
		public bool Contains(CodeNamespace value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06003A56 RID: 14934 RVA: 0x000F3D76 File Offset: 0x000F1F76
		public void CopyTo(CodeNamespace[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06003A57 RID: 14935 RVA: 0x000F3D85 File Offset: 0x000F1F85
		public int IndexOf(CodeNamespace value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x06003A58 RID: 14936 RVA: 0x000F3D93 File Offset: 0x000F1F93
		public void Insert(int index, CodeNamespace value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x06003A59 RID: 14937 RVA: 0x000F3DA2 File Offset: 0x000F1FA2
		public void Remove(CodeNamespace value)
		{
			base.List.Remove(value);
		}
	}
}
