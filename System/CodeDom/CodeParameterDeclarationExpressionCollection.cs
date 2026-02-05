using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200064B RID: 1611
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeParameterDeclarationExpressionCollection : CollectionBase
	{
		// Token: 0x06003A8E RID: 14990 RVA: 0x000F41D2 File Offset: 0x000F23D2
		public CodeParameterDeclarationExpressionCollection()
		{
		}

		// Token: 0x06003A8F RID: 14991 RVA: 0x000F41DA File Offset: 0x000F23DA
		public CodeParameterDeclarationExpressionCollection(CodeParameterDeclarationExpressionCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x06003A90 RID: 14992 RVA: 0x000F41E9 File Offset: 0x000F23E9
		public CodeParameterDeclarationExpressionCollection(CodeParameterDeclarationExpression[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x17000E17 RID: 3607
		public CodeParameterDeclarationExpression this[int index]
		{
			get
			{
				return (CodeParameterDeclarationExpression)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06003A93 RID: 14995 RVA: 0x000F421A File Offset: 0x000F241A
		public int Add(CodeParameterDeclarationExpression value)
		{
			return base.List.Add(value);
		}

		// Token: 0x06003A94 RID: 14996 RVA: 0x000F4228 File Offset: 0x000F2428
		public void AddRange(CodeParameterDeclarationExpression[] value)
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

		// Token: 0x06003A95 RID: 14997 RVA: 0x000F425C File Offset: 0x000F245C
		public void AddRange(CodeParameterDeclarationExpressionCollection value)
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

		// Token: 0x06003A96 RID: 14998 RVA: 0x000F4298 File Offset: 0x000F2498
		public bool Contains(CodeParameterDeclarationExpression value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06003A97 RID: 14999 RVA: 0x000F42A6 File Offset: 0x000F24A6
		public void CopyTo(CodeParameterDeclarationExpression[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06003A98 RID: 15000 RVA: 0x000F42B5 File Offset: 0x000F24B5
		public int IndexOf(CodeParameterDeclarationExpression value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x06003A99 RID: 15001 RVA: 0x000F42C3 File Offset: 0x000F24C3
		public void Insert(int index, CodeParameterDeclarationExpression value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x06003A9A RID: 15002 RVA: 0x000F42D2 File Offset: 0x000F24D2
		public void Remove(CodeParameterDeclarationExpression value)
		{
			base.List.Remove(value);
		}
	}
}
