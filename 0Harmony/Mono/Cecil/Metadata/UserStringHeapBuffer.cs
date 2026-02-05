using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020001A4 RID: 420
	internal sealed class UserStringHeapBuffer : StringHeapBuffer
	{
		// Token: 0x06000D58 RID: 3416 RVA: 0x0002DC0C File Offset: 0x0002BE0C
		public override uint GetStringIndex(string @string)
		{
			uint position;
			if (this.strings.TryGetValue(@string, out position))
			{
				return position;
			}
			position = (uint)this.position;
			this.WriteString(@string);
			this.strings.Add(@string, position);
			return position;
		}

		// Token: 0x06000D59 RID: 3417 RVA: 0x0002DC48 File Offset: 0x0002BE48
		protected override void WriteString(string @string)
		{
			base.WriteCompressedUInt32((uint)(@string.Length * 2 + 1));
			byte b = 0;
			foreach (char c in @string)
			{
				base.WriteUInt16((ushort)c);
				if (b != 1 && (c < ' ' || c > '~') && (c > '~' || (c >= '\u0001' && c <= '\b') || (c >= '\u000e' && c <= '\u001f') || c == '\'' || c == '-'))
				{
					b = 1;
				}
			}
			base.WriteByte(b);
		}
	}
}
