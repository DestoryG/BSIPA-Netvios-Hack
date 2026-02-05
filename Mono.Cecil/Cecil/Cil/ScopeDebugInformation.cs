using System;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x02000117 RID: 279
	public sealed class ScopeDebugInformation : DebugInformation
	{
		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000AD6 RID: 2774 RVA: 0x00023ABA File Offset: 0x00021CBA
		// (set) Token: 0x06000AD7 RID: 2775 RVA: 0x00023AC2 File Offset: 0x00021CC2
		public InstructionOffset Start
		{
			get
			{
				return this.start;
			}
			set
			{
				this.start = value;
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000AD8 RID: 2776 RVA: 0x00023ACB File Offset: 0x00021CCB
		// (set) Token: 0x06000AD9 RID: 2777 RVA: 0x00023AD3 File Offset: 0x00021CD3
		public InstructionOffset End
		{
			get
			{
				return this.end;
			}
			set
			{
				this.end = value;
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000ADA RID: 2778 RVA: 0x00023ADC File Offset: 0x00021CDC
		// (set) Token: 0x06000ADB RID: 2779 RVA: 0x00023AE4 File Offset: 0x00021CE4
		public ImportDebugInformation Import
		{
			get
			{
				return this.import;
			}
			set
			{
				this.import = value;
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000ADC RID: 2780 RVA: 0x00023AED File Offset: 0x00021CED
		public bool HasScopes
		{
			get
			{
				return !this.scopes.IsNullOrEmpty<ScopeDebugInformation>();
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000ADD RID: 2781 RVA: 0x00023B00 File Offset: 0x00021D00
		public Collection<ScopeDebugInformation> Scopes
		{
			get
			{
				Collection<ScopeDebugInformation> collection;
				if ((collection = this.scopes) == null)
				{
					collection = (this.scopes = new Collection<ScopeDebugInformation>());
				}
				return collection;
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000ADE RID: 2782 RVA: 0x00023B25 File Offset: 0x00021D25
		public bool HasVariables
		{
			get
			{
				return !this.variables.IsNullOrEmpty<VariableDebugInformation>();
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000ADF RID: 2783 RVA: 0x00023B38 File Offset: 0x00021D38
		public Collection<VariableDebugInformation> Variables
		{
			get
			{
				Collection<VariableDebugInformation> collection;
				if ((collection = this.variables) == null)
				{
					collection = (this.variables = new Collection<VariableDebugInformation>());
				}
				return collection;
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000AE0 RID: 2784 RVA: 0x00023B5D File Offset: 0x00021D5D
		public bool HasConstants
		{
			get
			{
				return !this.constants.IsNullOrEmpty<ConstantDebugInformation>();
			}
		}

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000AE1 RID: 2785 RVA: 0x00023B70 File Offset: 0x00021D70
		public Collection<ConstantDebugInformation> Constants
		{
			get
			{
				Collection<ConstantDebugInformation> collection;
				if ((collection = this.constants) == null)
				{
					collection = (this.constants = new Collection<ConstantDebugInformation>());
				}
				return collection;
			}
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x00023B95 File Offset: 0x00021D95
		internal ScopeDebugInformation()
		{
			this.token = new MetadataToken(TokenType.LocalScope);
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x00023BAD File Offset: 0x00021DAD
		public ScopeDebugInformation(Instruction start, Instruction end)
			: this()
		{
			if (start == null)
			{
				throw new ArgumentNullException("start");
			}
			this.start = new InstructionOffset(start);
			if (end != null)
			{
				this.end = new InstructionOffset(end);
			}
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x00023BE0 File Offset: 0x00021DE0
		public bool TryGetName(VariableDefinition variable, out string name)
		{
			name = null;
			if (this.variables == null || this.variables.Count == 0)
			{
				return false;
			}
			for (int i = 0; i < this.variables.Count; i++)
			{
				if (this.variables[i].Index == variable.Index)
				{
					name = this.variables[i].Name;
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400069E RID: 1694
		internal InstructionOffset start;

		// Token: 0x0400069F RID: 1695
		internal InstructionOffset end;

		// Token: 0x040006A0 RID: 1696
		internal ImportDebugInformation import;

		// Token: 0x040006A1 RID: 1697
		internal Collection<ScopeDebugInformation> scopes;

		// Token: 0x040006A2 RID: 1698
		internal Collection<VariableDebugInformation> variables;

		// Token: 0x040006A3 RID: 1699
		internal Collection<ConstantDebugInformation> constants;
	}
}
