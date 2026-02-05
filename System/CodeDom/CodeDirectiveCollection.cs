using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000631 RID: 1585
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeDirectiveCollection : CollectionBase
	{
		// Token: 0x060039B7 RID: 14775 RVA: 0x000F2F53 File Offset: 0x000F1153
		public CodeDirectiveCollection()
		{
		}

		// Token: 0x060039B8 RID: 14776 RVA: 0x000F2F5B File Offset: 0x000F115B
		public CodeDirectiveCollection(CodeDirectiveCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x060039B9 RID: 14777 RVA: 0x000F2F6A File Offset: 0x000F116A
		public CodeDirectiveCollection(CodeDirective[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x17000DD5 RID: 3541
		public CodeDirective this[int index]
		{
			get
			{
				return (CodeDirective)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x060039BC RID: 14780 RVA: 0x000F2F9B File Offset: 0x000F119B
		public int Add(CodeDirective value)
		{
			return base.List.Add(value);
		}

		// Token: 0x060039BD RID: 14781 RVA: 0x000F2FAC File Offset: 0x000F11AC
		public void AddRange(CodeDirective[] value)
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

		// Token: 0x060039BE RID: 14782 RVA: 0x000F2FE0 File Offset: 0x000F11E0
		public void AddRange(CodeDirectiveCollection value)
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

		// Token: 0x060039BF RID: 14783 RVA: 0x000F301C File Offset: 0x000F121C
		public bool Contains(CodeDirective value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x060039C0 RID: 14784 RVA: 0x000F302A File Offset: 0x000F122A
		public void CopyTo(CodeDirective[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x060039C1 RID: 14785 RVA: 0x000F3039 File Offset: 0x000F1239
		public int IndexOf(CodeDirective value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x060039C2 RID: 14786 RVA: 0x000F3047 File Offset: 0x000F1247
		public void Insert(int index, CodeDirective value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x060039C3 RID: 14787 RVA: 0x000F3056 File Offset: 0x000F1256
		public void Remove(CodeDirective value)
		{
			base.List.Remove(value);
		}
	}
}
