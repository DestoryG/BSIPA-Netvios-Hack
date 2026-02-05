using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000618 RID: 1560
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeArrayIndexerExpression : CodeExpression
	{
		// Token: 0x06003909 RID: 14601 RVA: 0x000F2349 File Offset: 0x000F0549
		public CodeArrayIndexerExpression()
		{
		}

		// Token: 0x0600390A RID: 14602 RVA: 0x000F2351 File Offset: 0x000F0551
		public CodeArrayIndexerExpression(CodeExpression targetObject, params CodeExpression[] indices)
		{
			this.targetObject = targetObject;
			this.indices = new CodeExpressionCollection();
			this.indices.AddRange(indices);
		}

		// Token: 0x17000DA6 RID: 3494
		// (get) Token: 0x0600390B RID: 14603 RVA: 0x000F2377 File Offset: 0x000F0577
		// (set) Token: 0x0600390C RID: 14604 RVA: 0x000F237F File Offset: 0x000F057F
		public CodeExpression TargetObject
		{
			get
			{
				return this.targetObject;
			}
			set
			{
				this.targetObject = value;
			}
		}

		// Token: 0x17000DA7 RID: 3495
		// (get) Token: 0x0600390D RID: 14605 RVA: 0x000F2388 File Offset: 0x000F0588
		public CodeExpressionCollection Indices
		{
			get
			{
				if (this.indices == null)
				{
					this.indices = new CodeExpressionCollection();
				}
				return this.indices;
			}
		}

		// Token: 0x04002B7F RID: 11135
		private CodeExpression targetObject;

		// Token: 0x04002B80 RID: 11136
		private CodeExpressionCollection indices;
	}
}
