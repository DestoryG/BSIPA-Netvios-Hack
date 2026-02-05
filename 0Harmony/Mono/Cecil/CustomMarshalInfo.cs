using System;

namespace Mono.Cecil
{
	// Token: 0x02000132 RID: 306
	internal sealed class CustomMarshalInfo : MarshalInfo
	{
		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000866 RID: 2150 RVA: 0x000219C6 File Offset: 0x0001FBC6
		// (set) Token: 0x06000867 RID: 2151 RVA: 0x000219CE File Offset: 0x0001FBCE
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

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000868 RID: 2152 RVA: 0x000219D7 File Offset: 0x0001FBD7
		// (set) Token: 0x06000869 RID: 2153 RVA: 0x000219DF File Offset: 0x0001FBDF
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

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x0600086A RID: 2154 RVA: 0x000219E8 File Offset: 0x0001FBE8
		// (set) Token: 0x0600086B RID: 2155 RVA: 0x000219F0 File Offset: 0x0001FBF0
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

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x0600086C RID: 2156 RVA: 0x000219F9 File Offset: 0x0001FBF9
		// (set) Token: 0x0600086D RID: 2157 RVA: 0x00021A01 File Offset: 0x0001FC01
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

		// Token: 0x0600086E RID: 2158 RVA: 0x00021A0A File Offset: 0x0001FC0A
		public CustomMarshalInfo()
			: base(NativeType.CustomMarshaler)
		{
		}

		// Token: 0x0400030B RID: 779
		internal Guid guid;

		// Token: 0x0400030C RID: 780
		internal string unmanaged_type;

		// Token: 0x0400030D RID: 781
		internal TypeReference managed_type;

		// Token: 0x0400030E RID: 782
		internal string cookie;
	}
}
