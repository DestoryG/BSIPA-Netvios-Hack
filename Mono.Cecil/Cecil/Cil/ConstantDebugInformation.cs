using System;

namespace Mono.Cecil.Cil
{
	// Token: 0x0200011D RID: 285
	public sealed class ConstantDebugInformation : DebugInformation
	{
		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000AFA RID: 2810 RVA: 0x00023E86 File Offset: 0x00022086
		// (set) Token: 0x06000AFB RID: 2811 RVA: 0x00023E8E File Offset: 0x0002208E
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

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000AFC RID: 2812 RVA: 0x00023E97 File Offset: 0x00022097
		// (set) Token: 0x06000AFD RID: 2813 RVA: 0x00023E9F File Offset: 0x0002209F
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

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000AFE RID: 2814 RVA: 0x00023EA8 File Offset: 0x000220A8
		// (set) Token: 0x06000AFF RID: 2815 RVA: 0x00023EB0 File Offset: 0x000220B0
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

		// Token: 0x06000B00 RID: 2816 RVA: 0x00023EB9 File Offset: 0x000220B9
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

		// Token: 0x040006B0 RID: 1712
		private string name;

		// Token: 0x040006B1 RID: 1713
		private TypeReference constant_type;

		// Token: 0x040006B2 RID: 1714
		private object value;
	}
}
