using System;

namespace System.Runtime.Serialization
{
	// Token: 0x0200007B RID: 123
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	public sealed class DataMemberAttribute : Attribute
	{
		// Token: 0x1700014E RID: 334
		// (get) Token: 0x0600092F RID: 2351 RVA: 0x0002978D File Offset: 0x0002798D
		// (set) Token: 0x06000930 RID: 2352 RVA: 0x00029795 File Offset: 0x00027995
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
				this.isNameSetExplicitly = true;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000931 RID: 2353 RVA: 0x000297A5 File Offset: 0x000279A5
		public bool IsNameSetExplicitly
		{
			get
			{
				return this.isNameSetExplicitly;
			}
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000932 RID: 2354 RVA: 0x000297AD File Offset: 0x000279AD
		// (set) Token: 0x06000933 RID: 2355 RVA: 0x000297B5 File Offset: 0x000279B5
		public int Order
		{
			get
			{
				return this.order;
			}
			set
			{
				if (value < 0)
				{
					throw DiagnosticUtility.ExceptionUtility.ThrowHelperError(new InvalidDataContractException(SR.GetString("Property 'Order' in DataMemberAttribute attribute cannot be a negative number.")));
				}
				this.order = value;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000934 RID: 2356 RVA: 0x000297D7 File Offset: 0x000279D7
		// (set) Token: 0x06000935 RID: 2357 RVA: 0x000297DF File Offset: 0x000279DF
		public bool IsRequired
		{
			get
			{
				return this.isRequired;
			}
			set
			{
				this.isRequired = value;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000936 RID: 2358 RVA: 0x000297E8 File Offset: 0x000279E8
		// (set) Token: 0x06000937 RID: 2359 RVA: 0x000297F0 File Offset: 0x000279F0
		public bool EmitDefaultValue
		{
			get
			{
				return this.emitDefaultValue;
			}
			set
			{
				this.emitDefaultValue = value;
			}
		}

		// Token: 0x0400034A RID: 842
		private string name;

		// Token: 0x0400034B RID: 843
		private bool isNameSetExplicitly;

		// Token: 0x0400034C RID: 844
		private int order = -1;

		// Token: 0x0400034D RID: 845
		private bool isRequired;

		// Token: 0x0400034E RID: 846
		private bool emitDefaultValue = true;
	}
}
