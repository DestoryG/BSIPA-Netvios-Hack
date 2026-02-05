using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001E0 RID: 480
	internal sealed class VariableDebugInformation : DebugInformation
	{
		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000ED8 RID: 3800 RVA: 0x00032F79 File Offset: 0x00031179
		public int Index
		{
			get
			{
				return this.index.Index;
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000ED9 RID: 3801 RVA: 0x00032F86 File Offset: 0x00031186
		// (set) Token: 0x06000EDA RID: 3802 RVA: 0x00032F8E File Offset: 0x0003118E
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000EDB RID: 3803 RVA: 0x00032F97 File Offset: 0x00031197
		// (set) Token: 0x06000EDC RID: 3804 RVA: 0x00032F9F File Offset: 0x0003119F
		public VariableAttributes Attributes
		{
			get
			{
				return (VariableAttributes)this.attributes;
			}
			set
			{
				this.attributes = (ushort)value;
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000EDD RID: 3805 RVA: 0x00032FA8 File Offset: 0x000311A8
		// (set) Token: 0x06000EDE RID: 3806 RVA: 0x00032FB6 File Offset: 0x000311B6
		public bool IsDebuggerHidden
		{
			get
			{
				return this.attributes.GetAttributes(1);
			}
			set
			{
				this.attributes = this.attributes.SetAttributes(1, value);
			}
		}

		// Token: 0x06000EDF RID: 3807 RVA: 0x00032FCB File Offset: 0x000311CB
		internal VariableDebugInformation(int index, string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.index = new VariableIndex(index);
			this.name = name;
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x00032FF4 File Offset: 0x000311F4
		public VariableDebugInformation(VariableDefinition variable, string name)
		{
			if (variable == null)
			{
				throw new ArgumentNullException("variable");
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.index = new VariableIndex(variable);
			this.name = name;
			this.token = new MetadataToken(TokenType.LocalVariable);
		}

		// Token: 0x0400090C RID: 2316
		private string name;

		// Token: 0x0400090D RID: 2317
		private ushort attributes;

		// Token: 0x0400090E RID: 2318
		internal VariableIndex index;
	}
}
