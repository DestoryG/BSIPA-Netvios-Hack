using System;
using System.Collections.Generic;

namespace Google.Protobuf
{
	// Token: 0x02000025 RID: 37
	public sealed class UnknownFieldSet
	{
		// Token: 0x060001F9 RID: 505 RVA: 0x0000A0EB File Offset: 0x000082EB
		internal UnknownFieldSet()
		{
			this.fields = new Dictionary<int, UnknownField>();
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000A0FE File Offset: 0x000082FE
		internal bool HasField(int field)
		{
			return this.fields.ContainsKey(field);
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000A10C File Offset: 0x0000830C
		public void WriteTo(CodedOutputStream output)
		{
			foreach (KeyValuePair<int, UnknownField> keyValuePair in this.fields)
			{
				keyValuePair.Value.WriteTo(keyValuePair.Key, output);
			}
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000A168 File Offset: 0x00008368
		public int CalculateSize()
		{
			int num = 0;
			foreach (KeyValuePair<int, UnknownField> keyValuePair in this.fields)
			{
				num += keyValuePair.Value.GetSerializedSize(keyValuePair.Key);
			}
			return num;
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000A1C8 File Offset: 0x000083C8
		public override bool Equals(object other)
		{
			if (this == other)
			{
				return true;
			}
			IDictionary<int, UnknownField> dictionary = (other as UnknownFieldSet).fields;
			if (this.fields.Count != dictionary.Count)
			{
				return false;
			}
			foreach (KeyValuePair<int, UnknownField> keyValuePair in this.fields)
			{
				UnknownField unknownField;
				if (!dictionary.TryGetValue(keyValuePair.Key, out unknownField))
				{
					return false;
				}
				if (!keyValuePair.Value.Equals(unknownField))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000A264 File Offset: 0x00008464
		public override int GetHashCode()
		{
			int num = 1;
			foreach (KeyValuePair<int, UnknownField> keyValuePair in this.fields)
			{
				int num2 = keyValuePair.Key.GetHashCode() ^ keyValuePair.Value.GetHashCode();
				num ^= num2;
			}
			return num;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000A2D0 File Offset: 0x000084D0
		private UnknownField GetOrAddField(int number)
		{
			if (this.lastField != null && number == this.lastFieldNumber)
			{
				return this.lastField;
			}
			if (number == 0)
			{
				return null;
			}
			UnknownField unknownField;
			if (this.fields.TryGetValue(number, out unknownField))
			{
				return unknownField;
			}
			this.lastField = new UnknownField();
			this.AddOrReplaceField(number, this.lastField);
			this.lastFieldNumber = number;
			return this.lastField;
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000A332 File Offset: 0x00008532
		internal UnknownFieldSet AddOrReplaceField(int number, UnknownField field)
		{
			if (number == 0)
			{
				throw new ArgumentOutOfRangeException("number", "Zero is not a valid field number.");
			}
			this.fields[number] = field;
			return this;
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000A358 File Offset: 0x00008558
		private bool MergeFieldFrom(CodedInputStream input)
		{
			uint lastTag = input.LastTag;
			int tagFieldNumber = WireFormat.GetTagFieldNumber(lastTag);
			switch (WireFormat.GetTagWireType(lastTag))
			{
			case WireFormat.WireType.Varint:
			{
				ulong num = input.ReadUInt64();
				this.GetOrAddField(tagFieldNumber).AddVarint(num);
				return true;
			}
			case WireFormat.WireType.Fixed64:
			{
				ulong num2 = input.ReadFixed64();
				this.GetOrAddField(tagFieldNumber).AddFixed64(num2);
				return true;
			}
			case WireFormat.WireType.LengthDelimited:
			{
				ByteString byteString = input.ReadBytes();
				this.GetOrAddField(tagFieldNumber).AddLengthDelimited(byteString);
				return true;
			}
			case WireFormat.WireType.StartGroup:
			{
				UnknownFieldSet unknownFieldSet = new UnknownFieldSet();
				input.ReadGroup(tagFieldNumber, unknownFieldSet);
				this.GetOrAddField(tagFieldNumber).AddGroup(unknownFieldSet);
				return true;
			}
			case WireFormat.WireType.EndGroup:
				return false;
			case WireFormat.WireType.Fixed32:
			{
				uint num3 = input.ReadFixed32();
				this.GetOrAddField(tagFieldNumber).AddFixed32(num3);
				return true;
			}
			default:
				throw InvalidProtocolBufferException.InvalidWireType();
			}
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000A423 File Offset: 0x00008623
		internal void MergeGroupFrom(CodedInputStream input)
		{
			while (input.ReadTag() != 0U && this.MergeFieldFrom(input))
			{
			}
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000A436 File Offset: 0x00008636
		public static UnknownFieldSet MergeFieldFrom(UnknownFieldSet unknownFields, CodedInputStream input)
		{
			if (input.DiscardUnknownFields)
			{
				input.SkipLastField();
				return unknownFields;
			}
			if (unknownFields == null)
			{
				unknownFields = new UnknownFieldSet();
			}
			if (!unknownFields.MergeFieldFrom(input))
			{
				throw new InvalidProtocolBufferException("Merge an unknown field of end-group tag, indicating that the corresponding start-group was missing.");
			}
			return unknownFields;
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000A468 File Offset: 0x00008668
		private UnknownFieldSet MergeFrom(UnknownFieldSet other)
		{
			if (other != null)
			{
				foreach (KeyValuePair<int, UnknownField> keyValuePair in other.fields)
				{
					this.MergeField(keyValuePair.Key, keyValuePair.Value);
				}
			}
			return this;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000A4C8 File Offset: 0x000086C8
		public static UnknownFieldSet MergeFrom(UnknownFieldSet unknownFields, UnknownFieldSet other)
		{
			if (other == null)
			{
				return unknownFields;
			}
			if (unknownFields == null)
			{
				unknownFields = new UnknownFieldSet();
			}
			unknownFields.MergeFrom(other);
			return unknownFields;
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000A4E2 File Offset: 0x000086E2
		private UnknownFieldSet MergeField(int number, UnknownField field)
		{
			if (number == 0)
			{
				throw new ArgumentOutOfRangeException("number", "Zero is not a valid field number.");
			}
			if (this.HasField(number))
			{
				this.GetOrAddField(number).MergeFrom(field);
			}
			else
			{
				this.AddOrReplaceField(number, field);
			}
			return this;
		}

		// Token: 0x06000207 RID: 519 RVA: 0x0000A51A File Offset: 0x0000871A
		public static UnknownFieldSet Clone(UnknownFieldSet other)
		{
			if (other == null)
			{
				return null;
			}
			UnknownFieldSet unknownFieldSet = new UnknownFieldSet();
			unknownFieldSet.MergeFrom(other);
			return unknownFieldSet;
		}

		// Token: 0x04000069 RID: 105
		private readonly IDictionary<int, UnknownField> fields;

		// Token: 0x0400006A RID: 106
		private int lastFieldNumber;

		// Token: 0x0400006B RID: 107
		private UnknownField lastField;
	}
}
