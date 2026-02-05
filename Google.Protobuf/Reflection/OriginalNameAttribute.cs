using System;

namespace Google.Protobuf.Reflection
{
	// Token: 0x0200007B RID: 123
	[AttributeUsage(AttributeTargets.Field)]
	public class OriginalNameAttribute : Attribute
	{
		// Token: 0x17000237 RID: 567
		// (get) Token: 0x0600080D RID: 2061 RVA: 0x0001CBE5 File Offset: 0x0001ADE5
		// (set) Token: 0x0600080E RID: 2062 RVA: 0x0001CBED File Offset: 0x0001ADED
		public string Name { get; set; }

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x0600080F RID: 2063 RVA: 0x0001CBF6 File Offset: 0x0001ADF6
		// (set) Token: 0x06000810 RID: 2064 RVA: 0x0001CBFE File Offset: 0x0001ADFE
		public bool PreferredAlias { get; set; }

		// Token: 0x06000811 RID: 2065 RVA: 0x0001CC07 File Offset: 0x0001AE07
		public OriginalNameAttribute(string name)
		{
			this.Name = ProtoPreconditions.CheckNotNull<string>(name, "name");
			this.PreferredAlias = true;
		}
	}
}
