using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000628 RID: 1576
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeCommentStatementCollection : CollectionBase
	{
		// Token: 0x06003981 RID: 14721 RVA: 0x000F2B64 File Offset: 0x000F0D64
		public CodeCommentStatementCollection()
		{
		}

		// Token: 0x06003982 RID: 14722 RVA: 0x000F2B6C File Offset: 0x000F0D6C
		public CodeCommentStatementCollection(CodeCommentStatementCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x06003983 RID: 14723 RVA: 0x000F2B7B File Offset: 0x000F0D7B
		public CodeCommentStatementCollection(CodeCommentStatement[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x17000DC2 RID: 3522
		public CodeCommentStatement this[int index]
		{
			get
			{
				return (CodeCommentStatement)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06003986 RID: 14726 RVA: 0x000F2BAC File Offset: 0x000F0DAC
		public int Add(CodeCommentStatement value)
		{
			return base.List.Add(value);
		}

		// Token: 0x06003987 RID: 14727 RVA: 0x000F2BBC File Offset: 0x000F0DBC
		public void AddRange(CodeCommentStatement[] value)
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

		// Token: 0x06003988 RID: 14728 RVA: 0x000F2BF0 File Offset: 0x000F0DF0
		public void AddRange(CodeCommentStatementCollection value)
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

		// Token: 0x06003989 RID: 14729 RVA: 0x000F2C2C File Offset: 0x000F0E2C
		public bool Contains(CodeCommentStatement value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x0600398A RID: 14730 RVA: 0x000F2C3A File Offset: 0x000F0E3A
		public void CopyTo(CodeCommentStatement[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x0600398B RID: 14731 RVA: 0x000F2C49 File Offset: 0x000F0E49
		public int IndexOf(CodeCommentStatement value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x0600398C RID: 14732 RVA: 0x000F2C57 File Offset: 0x000F0E57
		public void Insert(int index, CodeCommentStatement value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x0600398D RID: 14733 RVA: 0x000F2C66 File Offset: 0x000F0E66
		public void Remove(CodeCommentStatement value)
		{
			base.List.Remove(value);
		}
	}
}
