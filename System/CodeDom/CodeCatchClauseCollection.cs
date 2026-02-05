using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000624 RID: 1572
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeCatchClauseCollection : CollectionBase
	{
		// Token: 0x0600395F RID: 14687 RVA: 0x000F2941 File Offset: 0x000F0B41
		public CodeCatchClauseCollection()
		{
		}

		// Token: 0x06003960 RID: 14688 RVA: 0x000F2949 File Offset: 0x000F0B49
		public CodeCatchClauseCollection(CodeCatchClauseCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x06003961 RID: 14689 RVA: 0x000F2958 File Offset: 0x000F0B58
		public CodeCatchClauseCollection(CodeCatchClause[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x17000DBB RID: 3515
		public CodeCatchClause this[int index]
		{
			get
			{
				return (CodeCatchClause)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06003964 RID: 14692 RVA: 0x000F2989 File Offset: 0x000F0B89
		public int Add(CodeCatchClause value)
		{
			return base.List.Add(value);
		}

		// Token: 0x06003965 RID: 14693 RVA: 0x000F2998 File Offset: 0x000F0B98
		public void AddRange(CodeCatchClause[] value)
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

		// Token: 0x06003966 RID: 14694 RVA: 0x000F29CC File Offset: 0x000F0BCC
		public void AddRange(CodeCatchClauseCollection value)
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

		// Token: 0x06003967 RID: 14695 RVA: 0x000F2A08 File Offset: 0x000F0C08
		public bool Contains(CodeCatchClause value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06003968 RID: 14696 RVA: 0x000F2A16 File Offset: 0x000F0C16
		public void CopyTo(CodeCatchClause[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06003969 RID: 14697 RVA: 0x000F2A25 File Offset: 0x000F0C25
		public int IndexOf(CodeCatchClause value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x0600396A RID: 14698 RVA: 0x000F2A33 File Offset: 0x000F0C33
		public void Insert(int index, CodeCatchClause value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x0600396B RID: 14699 RVA: 0x000F2A42 File Offset: 0x000F0C42
		public void Remove(CodeCatchClause value)
		{
			base.List.Remove(value);
		}
	}
}
