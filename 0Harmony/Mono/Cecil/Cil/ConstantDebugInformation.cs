using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001E1 RID: 481
	internal sealed class ConstantDebugInformation : DebugInformation
	{
		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000EE1 RID: 3809 RVA: 0x00033046 File Offset: 0x00031246
		// (set) Token: 0x06000EE2 RID: 3810 RVA: 0x0003304E File Offset: 0x0003124E
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

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000EE3 RID: 3811 RVA: 0x00033057 File Offset: 0x00031257
		// (set) Token: 0x06000EE4 RID: 3812 RVA: 0x0003305F File Offset: 0x0003125F
		public TypeReference ConstantType
		{
			get
			{
				return this.constant_type;
			}
			set
			{
				this.constant_type = value;
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000EE5 RID: 3813 RVA: 0x00033068 File Offset: 0x00031268
		// (set) Token: 0x06000EE6 RID: 3814 RVA: 0x00033070 File Offset: 0x00031270
		public object Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}

		// Token: 0x06000EE7 RID: 3815 RVA: 0x00033079 File Offset: 0x00031279
		public ConstantDebugInformation(string name, TypeReference constant_type, object value)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			this.name = name;
			this.constant_type = constant_type;
			this.value = value;
			this.token = new MetadataToken(TokenType.LocalConstant);
		}

		// Token: 0x0400090F RID: 2319
		private string name;

		// Token: 0x04000910 RID: 2320
		private TypeReference constant_type;

		// Token: 0x04000911 RID: 2321
		private object value;
	}
}
