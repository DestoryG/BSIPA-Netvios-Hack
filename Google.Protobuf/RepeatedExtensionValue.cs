using System;
using Google.Protobuf.Collections;

namespace Google.Protobuf
{
	// Token: 0x0200000E RID: 14
	internal sealed class RepeatedExtensionValue<T> : IExtensionValue, IEquatable<IExtensionValue>, IDeepCloneable<IExtensionValue>
	{
		// Token: 0x060000F0 RID: 240 RVA: 0x00005053 File Offset: 0x00003253
		internal RepeatedExtensionValue(FieldCodec<T> codec)
		{
			this.codec = codec;
			this.field = new RepeatedField<T>();
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000506D File Offset: 0x0000326D
		public int CalculateSize()
		{
			return this.field.CalculateSize(this.codec);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00005080 File Offset: 0x00003280
		public IExtensionValue Clone()
		{
			return new RepeatedExtensionValue<T>(this.codec)
			{
				field = this.field.Clone()
			};
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000509E File Offset: 0x0000329E
		public bool Equals(IExtensionValue other)
		{
			return this == other || (other is RepeatedExtensionValue<T> && this.field.Equals((other as RepeatedExtensionValue<T>).field) && this.codec.Equals((other as RepeatedExtensionValue<T>).codec));
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x000050DE File Offset: 0x000032DE
		public override int GetHashCode()
		{
			return (17 * 31 + this.field.GetHashCode()) * 31 + this.codec.GetHashCode();
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00005100 File Offset: 0x00003300
		public void MergeFrom(CodedInputStream input)
		{
			this.field.AddEntriesFrom(input, this.codec);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00005114 File Offset: 0x00003314
		public void MergeFrom(IExtensionValue value)
		{
			if (value is RepeatedExtensionValue<T>)
			{
				this.field.Add((value as RepeatedExtensionValue<T>).field);
			}
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00005134 File Offset: 0x00003334
		public void WriteTo(CodedOutputStream output)
		{
			this.field.WriteTo(output, this.codec);
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00005148 File Offset: 0x00003348
		public RepeatedField<T> GetValue()
		{
			return this.field;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00005150 File Offset: 0x00003350
		public bool IsInitialized()
		{
			for (int i = 0; i < this.field.Count; i++)
			{
				T t = this.field[i];
				if (!(t is IMessage))
				{
					break;
				}
				if (!(t as IMessage).IsInitialized())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0400002A RID: 42
		private RepeatedField<T> field;

		// Token: 0x0400002B RID: 43
		private readonly FieldCodec<T> codec;
	}
}
