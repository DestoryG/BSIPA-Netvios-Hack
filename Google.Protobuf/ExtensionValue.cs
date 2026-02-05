using System;

namespace Google.Protobuf
{
	// Token: 0x0200000D RID: 13
	internal sealed class ExtensionValue<T> : IExtensionValue, IEquatable<IExtensionValue>, IDeepCloneable<IExtensionValue>
	{
		// Token: 0x060000E5 RID: 229 RVA: 0x00004E70 File Offset: 0x00003070
		internal ExtensionValue(FieldCodec<T> codec)
		{
			this.codec = codec;
			this.field = codec.DefaultValue;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00004E8B File Offset: 0x0000308B
		public int CalculateSize()
		{
			return this.codec.CalculateSizeWithTag(this.field);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00004EA0 File Offset: 0x000030A0
		public IExtensionValue Clone()
		{
			return new ExtensionValue<T>(this.codec)
			{
				field = ((this.field is IDeepCloneable<T>) ? (this.field as IDeepCloneable<T>).Clone() : this.field)
			};
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00004EF0 File Offset: 0x000030F0
		public bool Equals(IExtensionValue other)
		{
			return this == other || (other is ExtensionValue<T> && this.codec.Equals((other as ExtensionValue<T>).codec) && object.Equals(this.field, (other as ExtensionValue<T>).field));
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00004F45 File Offset: 0x00003145
		public override int GetHashCode()
		{
			return (17 * 31 + this.field.GetHashCode()) * 31 + this.codec.GetHashCode();
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00004F6D File Offset: 0x0000316D
		public void MergeFrom(CodedInputStream input)
		{
			this.codec.ValueMerger(input, ref this.field);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00004F88 File Offset: 0x00003188
		public void MergeFrom(IExtensionValue value)
		{
			if (value is ExtensionValue<T>)
			{
				ExtensionValue<T> extensionValue = value as ExtensionValue<T>;
				this.codec.FieldMerger(ref this.field, extensionValue.field);
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00004FC4 File Offset: 0x000031C4
		public void WriteTo(CodedOutputStream output)
		{
			output.WriteTag(this.codec.Tag);
			this.codec.ValueWriter(output, this.field);
			if (this.codec.EndTag != 0U)
			{
				output.WriteTag(this.codec.EndTag);
			}
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00005017 File Offset: 0x00003217
		public T GetValue()
		{
			return this.field;
		}

		// Token: 0x060000EE RID: 238 RVA: 0x0000501F File Offset: 0x0000321F
		public void SetValue(T value)
		{
			this.field = value;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00005028 File Offset: 0x00003228
		public bool IsInitialized()
		{
			return !(this.field is IMessage) || (this.field as IMessage).IsInitialized();
		}

		// Token: 0x04000028 RID: 40
		private T field;

		// Token: 0x04000029 RID: 41
		private FieldCodec<T> codec;
	}
}
