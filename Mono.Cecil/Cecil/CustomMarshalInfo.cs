using System;

namespace Mono.Cecil
{
	// Token: 0x0200007B RID: 123
	public sealed class CustomMarshalInfo : MarshalInfo
	{
		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060004D4 RID: 1236 RVA: 0x0001319A File Offset: 0x0001139A
		// (set) Token: 0x060004D5 RID: 1237 RVA: 0x000131A2 File Offset: 0x000113A2
		public Guid Guid
		{
			get
			{
				return this.guid;
			}
			set
			{
				this.guid = value;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060004D6 RID: 1238 RVA: 0x000131AB File Offset: 0x000113AB
		// (set) Token: 0x060004D7 RID: 1239 RVA: 0x000131B3 File Offset: 0x000113B3
		public string UnmanagedType
		{
			get
			{
				return this.unmanaged_type;
			}
			set
			{
				this.unmanaged_type = value;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060004D8 RID: 1240 RVA: 0x000131BC File Offset: 0x000113BC
		// (set) Token: 0x060004D9 RID: 1241 RVA: 0x000131C4 File Offset: 0x000113C4
		public TypeReference ManagedType
		{
			get
			{
				return this.managed_type;
			}
			set
			{
				this.managed_type = value;
			}
		}

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060004DA RID: 1242 RVA: 0x000131CD File Offset: 0x000113CD
		// (set) Token: 0x060004DB RID: 1243 RVA: 0x000131D5 File Offset: 0x000113D5
		public string Cookie
		{
			get
			{
				return this.cookie;
			}
			set
			{
				this.cookie = value;
			}
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x000131DE File Offset: 0x000113DE
		public CustomMarshalInfo()
			: base(NativeType.CustomMarshaler)
		{
		}

		// Token: 0x040000F1 RID: 241
		internal Guid guid;

		// Token: 0x040000F2 RID: 242
		internal string unmanaged_type;

		// Token: 0x040000F3 RID: 243
		internal TypeReference managed_type;

		// Token: 0x040000F4 RID: 244
		internal string cookie;
	}
}
