using System;

namespace Google.Protobuf
{
	// Token: 0x02000008 RID: 8
	public sealed class RepeatedExtension<TTarget, TValue> : Extension where TTarget : IExtendableMessage<TTarget>
	{
		// Token: 0x060000BC RID: 188 RVA: 0x00004687 File Offset: 0x00002887
		public RepeatedExtension(int fieldNumber, FieldCodec<TValue> codec)
			: base(fieldNumber)
		{
			this.codec = codec;
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00004697 File Offset: 0x00002897
		internal override Type TargetType
		{
			get
			{
				return typeof(TTarget);
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000BE RID: 190 RVA: 0x000046A3 File Offset: 0x000028A3
		internal override bool IsRepeated
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000046A6 File Offset: 0x000028A6
		internal override IExtensionValue CreateValue()
		{
			return new RepeatedExtensionValue<TValue>(this.codec);
		}

		// Token: 0x04000025 RID: 37
		private readonly FieldCodec<TValue> codec;
	}
}
