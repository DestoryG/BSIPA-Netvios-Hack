using System;
using System.Collections.Generic;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020000DD RID: 221
	internal sealed class GuidHeapBuffer : HeapBuffer
	{
		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000965 RID: 2405 RVA: 0x0001E80B File Offset: 0x0001CA0B
		public override bool IsEmpty
		{
			get
			{
				return this.length == 0;
			}
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x0001E816 File Offset: 0x0001CA16
		public GuidHeapBuffer()
			: base(16)
		{
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x0001E82C File Offset: 0x0001CA2C
		public uint GetGuidIndex(Guid guid)
		{
			uint num;
			if (this.guids.TryGetValue(guid, out num))
			{
				return num;
			}
			num = (uint)(this.guids.Count + 1);
			this.WriteGuid(guid);
			this.guids.Add(guid, num);
			return num;
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x0001E86E File Offset: 0x0001CA6E
		private void WriteGuid(Guid guid)
		{
			base.WriteBytes(guid.ToByteArray());
		}

		// Token: 0x04000391 RID: 913
		private readonly Dictionary<Guid, uint> guids = new Dictionary<Guid, uint>();
	}
}
