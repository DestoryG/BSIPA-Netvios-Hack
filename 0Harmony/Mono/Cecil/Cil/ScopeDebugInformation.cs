using System;
using System.Threading;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001DB RID: 475
	internal sealed class ScopeDebugInformation : DebugInformation
	{
		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000EBD RID: 3773 RVA: 0x00032CC6 File Offset: 0x00030EC6
		// (set) Token: 0x06000EBE RID: 3774 RVA: 0x00032CCE File Offset: 0x00030ECE
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

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000EBF RID: 3775 RVA: 0x00032CD7 File Offset: 0x00030ED7
		// (set) Token: 0x06000EC0 RID: 3776 RVA: 0x00032CDF File Offset: 0x00030EDF
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

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000EC1 RID: 3777 RVA: 0x00032CE8 File Offset: 0x00030EE8
		// (set) Token: 0x06000EC2 RID: 3778 RVA: 0x00032CF0 File Offset: 0x00030EF0
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

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000EC3 RID: 3779 RVA: 0x00032CF9 File Offset: 0x00030EF9
		public bool HasScopes
		{
			get
			{
				return !this.scopes.IsNullOrEmpty<ScopeDebugInformation>();
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000EC4 RID: 3780 RVA: 0x00032D09 File Offset: 0x00030F09
		public Collection<ScopeDebugInformation> Scopes
		{
			get
			{
				if (this.scopes == null)
				{
					Interlocked.CompareExchange<Collection<ScopeDebugInformation>>(ref this.scopes, new Collection<ScopeDebugInformation>(), null);
				}
				return this.scopes;
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000EC5 RID: 3781 RVA: 0x00032D2B File Offset: 0x00030F2B
		public bool HasVariables
		{
			get
			{
				return !this.variables.IsNullOrEmpty<VariableDebugInformation>();
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000EC6 RID: 3782 RVA: 0x00032D3B File Offset: 0x00030F3B
		public Collection<VariableDebugInformation> Variables
		{
			get
			{
				if (this.variables == null)
				{
					Interlocked.CompareExchange<Collection<VariableDebugInformation>>(ref this.variables, new Collection<VariableDebugInformation>(), null);
				}
				return this.variables;
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000EC7 RID: 3783 RVA: 0x00032D5D File Offset: 0x00030F5D
		public bool HasConstants
		{
			get
			{
				return !this.constants.IsNullOrEmpty<ConstantDebugInformation>();
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000EC8 RID: 3784 RVA: 0x00032D6D File Offset: 0x00030F6D
		public Collection<ConstantDebugInformation> Constants
		{
			get
			{
				if (this.constants == null)
				{
					Interlocked.CompareExchange<Collection<ConstantDebugInformation>>(ref this.constants, new Collection<ConstantDebugInformation>(), null);
				}
				return this.constants;
			}
		}

		// Token: 0x06000EC9 RID: 3785 RVA: 0x00032D8F File Offset: 0x00030F8F
		internal ScopeDebugInformation()
		{
			this.token = new MetadataToken(TokenType.LocalScope);
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x00032DA7 File Offset: 0x00030FA7
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

		// Token: 0x06000ECB RID: 3787 RVA: 0x00032DD8 File Offset: 0x00030FD8
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

		// Token: 0x040008FD RID: 2301
		internal InstructionOffset start;

		// Token: 0x040008FE RID: 2302
		internal InstructionOffset end;

		// Token: 0x040008FF RID: 2303
		internal ImportDebugInformation import;

		// Token: 0x04000900 RID: 2304
		internal Collection<ScopeDebugInformation> scopes;

		// Token: 0x04000901 RID: 2305
		internal Collection<VariableDebugInformation> variables;

		// Token: 0x04000902 RID: 2306
		internal Collection<ConstantDebugInformation> constants;
	}
}
