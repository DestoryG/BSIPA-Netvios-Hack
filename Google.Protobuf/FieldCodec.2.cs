using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;

namespace Google.Protobuf
{
	// Token: 0x02000010 RID: 16
	public sealed class FieldCodec<T>
	{
		// Token: 0x0600011E RID: 286 RVA: 0x00005A24 File Offset: 0x00003C24
		static FieldCodec()
		{
			if (typeof(T) == typeof(string))
			{
				FieldCodec<T>.DefaultDefault = (T)((object)"");
				return;
			}
			if (typeof(T) == typeof(ByteString))
			{
				FieldCodec<T>.DefaultDefault = (T)((object)ByteString.Empty);
			}
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00005AA6 File Offset: 0x00003CA6
		internal static bool IsPackedRepeatedField(uint tag)
		{
			return FieldCodec<T>.TypeSupportsPacking && WireFormat.GetTagWireType(tag) == WireFormat.WireType.LengthDelimited;
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000120 RID: 288 RVA: 0x00005ABA File Offset: 0x00003CBA
		internal bool PackedRepeatedField { get; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000121 RID: 289 RVA: 0x00005AC2 File Offset: 0x00003CC2
		internal Action<CodedOutputStream, T> ValueWriter { get; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00005ACA File Offset: 0x00003CCA
		internal Func<T, int> ValueSizeCalculator { get; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00005AD2 File Offset: 0x00003CD2
		internal Func<CodedInputStream, T> ValueReader { get; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00005ADA File Offset: 0x00003CDA
		internal FieldCodec<T>.InputMerger ValueMerger { get; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00005AE2 File Offset: 0x00003CE2
		internal FieldCodec<T>.ValuesMerger FieldMerger { get; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000126 RID: 294 RVA: 0x00005AEA File Offset: 0x00003CEA
		internal int FixedSize { get; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00005AF2 File Offset: 0x00003CF2
		internal uint Tag { get; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00005AFA File Offset: 0x00003CFA
		internal uint EndTag { get; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00005B02 File Offset: 0x00003D02
		internal T DefaultValue { get; }

		// Token: 0x0600012A RID: 298 RVA: 0x00005B0C File Offset: 0x00003D0C
		internal FieldCodec(Func<CodedInputStream, T> reader, Action<CodedOutputStream, T> writer, int fixedSize, uint tag, T defaultValue)
			: this(reader, writer, (T _) => fixedSize, tag, defaultValue)
		{
			this.FixedSize = fixedSize;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00005B4C File Offset: 0x00003D4C
		internal FieldCodec(Func<CodedInputStream, T> reader, Action<CodedOutputStream, T> writer, Func<T, int> sizeCalculator, uint tag, T defaultValue)
			: this(reader, writer, delegate(CodedInputStream i, ref T v)
			{
				v = reader(i);
			}, delegate(ref T v, T v2)
			{
				v = v2;
				return true;
			}, sizeCalculator, tag, 0U, defaultValue)
		{
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00005BA4 File Offset: 0x00003DA4
		internal FieldCodec(Func<CodedInputStream, T> reader, Action<CodedOutputStream, T> writer, FieldCodec<T>.InputMerger inputMerger, FieldCodec<T>.ValuesMerger valuesMerger, Func<T, int> sizeCalculator, uint tag, uint endTag = 0U)
			: this(reader, writer, inputMerger, valuesMerger, sizeCalculator, tag, endTag, FieldCodec<T>.DefaultDefault)
		{
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00005BC8 File Offset: 0x00003DC8
		internal FieldCodec(Func<CodedInputStream, T> reader, Action<CodedOutputStream, T> writer, FieldCodec<T>.InputMerger inputMerger, FieldCodec<T>.ValuesMerger valuesMerger, Func<T, int> sizeCalculator, uint tag, uint endTag, T defaultValue)
		{
			this.ValueReader = reader;
			this.ValueWriter = writer;
			this.ValueMerger = inputMerger;
			this.FieldMerger = valuesMerger;
			this.ValueSizeCalculator = sizeCalculator;
			this.FixedSize = 0;
			this.Tag = tag;
			this.EndTag = endTag;
			this.DefaultValue = defaultValue;
			this.tagSize = CodedOutputStream.ComputeRawVarint32Size(tag);
			if (endTag != 0U)
			{
				this.tagSize += CodedOutputStream.ComputeRawVarint32Size(endTag);
			}
			this.PackedRepeatedField = FieldCodec<T>.IsPackedRepeatedField(tag);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00005C51 File Offset: 0x00003E51
		public void WriteTagAndValue(CodedOutputStream output, T value)
		{
			if (!this.IsDefault(value))
			{
				output.WriteTag(this.Tag);
				this.ValueWriter(output, value);
				if (this.EndTag != 0U)
				{
					output.WriteTag(this.EndTag);
				}
			}
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00005C89 File Offset: 0x00003E89
		public T Read(CodedInputStream input)
		{
			return this.ValueReader(input);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00005C97 File Offset: 0x00003E97
		public int CalculateSizeWithTag(T value)
		{
			if (!this.IsDefault(value))
			{
				return this.ValueSizeCalculator(value) + this.tagSize;
			}
			return 0;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00005CB7 File Offset: 0x00003EB7
		private bool IsDefault(T value)
		{
			return FieldCodec<T>.EqualityComparer.Equals(value, this.DefaultValue);
		}

		// Token: 0x0400002C RID: 44
		private static readonly EqualityComparer<T> EqualityComparer = ProtobufEqualityComparers.GetEqualityComparer<T>();

		// Token: 0x0400002D RID: 45
		private static readonly T DefaultDefault;

		// Token: 0x0400002E RID: 46
		private static readonly bool TypeSupportsPacking = default(T) != null;

		// Token: 0x04000039 RID: 57
		private readonly int tagSize;

		// Token: 0x0200009B RID: 155
		// (Invoke) Token: 0x0600090E RID: 2318
		internal delegate void InputMerger(CodedInputStream input, ref T value);

		// Token: 0x0200009C RID: 156
		// (Invoke) Token: 0x06000912 RID: 2322
		internal delegate bool ValuesMerger(ref T value, T other);
	}
}
