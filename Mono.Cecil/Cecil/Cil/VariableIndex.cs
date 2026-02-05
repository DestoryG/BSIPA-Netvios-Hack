using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x0200011A RID: 282
	public struct VariableIndex
	{
		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000AE9 RID: 2793 RVA: 0x00023CF4 File Offset: 0x00021EF4
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

		// Token: 0x06000AEA RID: 2794 RVA: 0x00023D39 File Offset: 0x00021F39
		public VariableIndex(VariableDefinition variable)
		{
			if (variable == null)
			{
				throw new ArgumentNullException("variable");
			}
			this.variable = variable;
			this.index = null;
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x00023D5C File Offset: 0x00021F5C
		public VariableIndex(int index)
		{
			this.variable = null;
			this.index = new int?(index);
		}

		// Token: 0x040006A9 RID: 1705
		private readonly VariableDefinition variable;

		// Token: 0x040006AA RID: 1706
		private readonly int? index;
	}
}
