using System;

namespace Mono.Cecil.Metadata
{
	// Token: 0x020000E0 RID: 224
	internal sealed class UserStringHeapBuffer : StringHeapBuffer
	{
		// Token: 0x06000974 RID: 2420 RVA: 0x0001EA74 File Offset: 0x0001CC74
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

		// Token: 0x06000975 RID: 2421 RVA: 0x0001EAB0 File Offset: 0x0001CCB0
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
