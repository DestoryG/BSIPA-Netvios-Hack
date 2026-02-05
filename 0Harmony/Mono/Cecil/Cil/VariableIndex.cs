using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001DE RID: 478
	internal struct VariableIndex
	{
		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000ED0 RID: 3792 RVA: 0x00032ECA File Offset: 0x000310CA
		public int Index
		{
			get
			{
				if (this.variable != null)
				{
					return this.variable.Index;
				}
				if (this.index != null)
				{
					return this.index.Value;
				}
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000ED1 RID: 3793 RVA: 0x00032EFE File Offset: 0x000310FE
		public VariableIndex(VariableDefinition variable)
		{
			if (variable == null)
			{
				throw new ArgumentNullException("variable");
			}
			this.variable = variable;
			this.index = null;
		}

		// Token: 0x06000ED2 RID: 3794 RVA: 0x00032F21 File Offset: 0x00031121
		public VariableIndex(int index)
		{
			this.variable = null;
			this.index = new int?(index);
		}

		// Token: 0x04000908 RID: 2312
		private readonly VariableDefinition variable;

		// Token: 0x04000909 RID: 2313
		private readonly int? index;
	}
}
