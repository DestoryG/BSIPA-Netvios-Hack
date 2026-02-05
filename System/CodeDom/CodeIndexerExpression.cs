using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000639 RID: 1593
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeIndexerExpression : CodeExpression
	{
		// Token: 0x060039E7 RID: 14823 RVA: 0x000F3281 File Offset: 0x000F1481
		public CodeIndexerExpression()
		{
		}

		// Token: 0x060039E8 RID: 14824 RVA: 0x000F3289 File Offset: 0x000F1489
		public CodeIndexerExpression(CodeExpression targetObject, params CodeExpression[] indices)
		{
			this.targetObject = targetObject;
			this.indices = new CodeExpressionCollection();
			this.indices.AddRange(indices);
		}

		// Token: 0x17000DDD RID: 3549
		// (get) Token: 0x060039E9 RID: 14825 RVA: 0x000F32AF File Offset: 0x000F14AF
		// (set) Token: 0x060039EA RID: 14826 RVA: 0x000F32B7 File Offset: 0x000F14B7
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

		// Token: 0x17000DDE RID: 3550
		// (get) Token: 0x060039EB RID: 14827 RVA: 0x000F32C0 File Offset: 0x000F14C0
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

		// Token: 0x04002BC2 RID: 11202
		private CodeExpression targetObject;

		// Token: 0x04002BC3 RID: 11203
		private CodeExpressionCollection indices;
	}
}
