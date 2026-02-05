using System;
using UnityEngine;

namespace CustomAvatar.Tracking
{
	// Token: 0x02000029 RID: 41
	internal class TrackedDeviceState
	{
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x0000592E File Offset: 0x00003B2E
		// (set) Token: 0x060000A8 RID: 168 RVA: 0x00005936 File Offset: 0x00003B36
		public string name { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x0000593F File Offset: 0x00003B3F
		// (set) Token: 0x060000AA RID: 170 RVA: 0x00005947 File Offset: 0x00003B47
		public string serialNumber { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00005950 File Offset: 0x00003B50
		// (set) Token: 0x060000AC RID: 172 RVA: 0x00005958 File Offset: 0x00003B58
		public Vector3 position { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00005961 File Offset: 0x00003B61
		// (set) Token: 0x060000AE RID: 174 RVA: 0x00005969 File Offset: 0x00003B69
		public Quaternion rotation { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00005972 File Offset: 0x00003B72
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x0000597A File Offset: 0x00003B7A
		public bool found { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00005983 File Offset: 0x00003B83
		// (set) Token: 0x060000B2 RID: 178 RVA: 0x0000598B File Offset: 0x00003B8B
		public bool tracked { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00005994 File Offset: 0x00003B94
		// (set) Token: 0x060000B4 RID: 180 RVA: 0x0000599C File Offset: 0x00003B9C
		public TrackedDeviceRole role { get; set; }
	}
}
