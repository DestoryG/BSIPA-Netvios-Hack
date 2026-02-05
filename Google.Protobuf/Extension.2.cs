using System;

namespace Google.Protobuf
{
	// Token: 0x02000007 RID: 7
	public sealed class Extension<TTarget, TValue> : Extension where TTarget : IExtendableMessage<TTarget>
	{
		// Token: 0x060000B7 RID: 183 RVA: 0x0000464E File Offset: 0x0000284E
		public Extension(int fieldNumber, FieldCodec<TValue> codec)
			: base(fieldNumber)
		{
			this.codec = codec;
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x0000465E File Offset: 0x0000285E
		internal TValue DefaultValue
		{
			get
			{
				return this.codec.DefaultValue;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x0000466B File Offset: 0x0000286B
		internal override Type TargetType
		{
			get
			{
				return typeof(TTarget);
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00004677 File Offset: 0x00002877
		internal override bool IsRepeated
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0000467A File Offset: 0x0000287A
		internal override IExtensionValue CreateValue()
		{
			return new ExtensionValue<TValue>(this.codec);
		}

		// Token: 0x04000024 RID: 36
		private readonly FieldCodec<TValue> codec;
	}
}
