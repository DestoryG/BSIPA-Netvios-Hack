using System;
using System.Collections.Generic;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001A0 RID: 416
	internal sealed class GuidHeapBuffer : HeapBuffer
	{
		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000D47 RID: 3399 RVA: 0x0002D91F File Offset: 0x0002BB1F
		public override bool IsEmpty
		{
			get
			{
				return this.length == 0;
			}
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x0002D92A File Offset: 0x0002BB2A
		public GuidHeapBuffer()
			: base(16)
		{
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x0002D940 File Offset: 0x0002BB40
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

		// Token: 0x06000D4A RID: 3402 RVA: 0x0002D982 File Offset: 0x0002BB82
		private void WriteGuid(Guid guid)
		{
			base.WriteBytes(guid.ToByteArray());
		}

		// Token: 0x040005F0 RID: 1520
		private readonly Dictionary<Guid, uint> guids = new Dictionary<Guid, uint>();
	}
}
