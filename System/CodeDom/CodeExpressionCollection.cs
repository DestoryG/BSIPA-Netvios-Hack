using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000635 RID: 1589
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeExpressionCollection : CollectionBase
	{
		// Token: 0x060039CC RID: 14796 RVA: 0x000F30C2 File Offset: 0x000F12C2
		public CodeExpressionCollection()
		{
		}

		// Token: 0x060039CD RID: 14797 RVA: 0x000F30CA File Offset: 0x000F12CA
		public CodeExpressionCollection(CodeExpressionCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x060039CE RID: 14798 RVA: 0x000F30D9 File Offset: 0x000F12D9
		public CodeExpressionCollection(CodeExpression[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x17000DD8 RID: 3544
		public CodeExpression this[int index]
		{
			get
			{
				return (CodeExpression)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x060039D1 RID: 14801 RVA: 0x000F310A File Offset: 0x000F130A
		public int Add(CodeExpression value)
		{
			return base.List.Add(value);
		}

		// Token: 0x060039D2 RID: 14802 RVA: 0x000F3118 File Offset: 0x000F1318
		public void AddRange(CodeExpression[] value)
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

		// Token: 0x060039D3 RID: 14803 RVA: 0x000F314C File Offset: 0x000F134C
		public void AddRange(CodeExpressionCollection value)
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

		// Token: 0x060039D4 RID: 14804 RVA: 0x000F3188 File Offset: 0x000F1388
		public bool Contains(CodeExpression value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x060039D5 RID: 14805 RVA: 0x000F3196 File Offset: 0x000F1396
		public void CopyTo(CodeExpression[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x060039D6 RID: 14806 RVA: 0x000F31A5 File Offset: 0x000F13A5
		public int IndexOf(CodeExpression value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x060039D7 RID: 14807 RVA: 0x000F31B3 File Offset: 0x000F13B3
		public void Insert(int index, CodeExpression value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x060039D8 RID: 14808 RVA: 0x000F31C2 File Offset: 0x000F13C2
		public void Remove(CodeExpression value)
		{
			base.List.Remove(value);
		}
	}
}
