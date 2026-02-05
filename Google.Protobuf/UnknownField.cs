using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;

namespace Google.Protobuf
{
	// Token: 0x02000024 RID: 36
	internal sealed class UnknownField
	{
		// Token: 0x060001ED RID: 493 RVA: 0x00009B98 File Offset: 0x00007D98
		public override bool Equals(object other)
		{
			if (this == other)
			{
				return true;
			}
			UnknownField unknownField = other as UnknownField;
			return unknownField != null && Lists.Equals<ulong>(this.varintList, unknownField.varintList) && Lists.Equals<uint>(this.fixed32List, unknownField.fixed32List) && Lists.Equals<ulong>(this.fixed64List, unknownField.fixed64List) && Lists.Equals<ByteString>(this.lengthDelimitedList, unknownField.lengthDelimitedList) && Lists.Equals<UnknownFieldSet>(this.groupList, unknownField.groupList);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x00009C14 File Offset: 0x00007E14
		public override int GetHashCode()
		{
			return ((((43 * 47 + Lists.GetHashCode<ulong>(this.varintList)) * 47 + Lists.GetHashCode<uint>(this.fixed32List)) * 47 + Lists.GetHashCode<ulong>(this.fixed64List)) * 47 + Lists.GetHashCode<ByteString>(this.lengthDelimitedList)) * 47 + Lists.GetHashCode<UnknownFieldSet>(this.groupList);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x00009C70 File Offset: 0x00007E70
		internal void WriteTo(int fieldNumber, CodedOutputStream output)
		{
			if (this.varintList != null)
			{
				foreach (ulong num in this.varintList)
				{
					output.WriteTag(fieldNumber, WireFormat.WireType.Varint);
					output.WriteUInt64(num);
				}
			}
			if (this.fixed32List != null)
			{
				foreach (uint num2 in this.fixed32List)
				{
					output.WriteTag(fieldNumber, WireFormat.WireType.Fixed32);
					output.WriteFixed32(num2);
				}
			}
			if (this.fixed64List != null)
			{
				foreach (ulong num3 in this.fixed64List)
				{
					output.WriteTag(fieldNumber, WireFormat.WireType.Fixed64);
					output.WriteFixed64(num3);
				}
			}
			if (this.lengthDelimitedList != null)
			{
				foreach (ByteString byteString in this.lengthDelimitedList)
				{
					output.WriteTag(fieldNumber, WireFormat.WireType.LengthDelimited);
					output.WriteBytes(byteString);
				}
			}
			if (this.groupList != null)
			{
				foreach (UnknownFieldSet unknownFieldSet in this.groupList)
				{
					output.WriteTag(fieldNumber, WireFormat.WireType.StartGroup);
					unknownFieldSet.WriteTo(output);
					output.WriteTag(fieldNumber, WireFormat.WireType.EndGroup);
				}
			}
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x00009E28 File Offset: 0x00008028
		internal int GetSerializedSize(int fieldNumber)
		{
			int num = 0;
			if (this.varintList != null)
			{
				num += CodedOutputStream.ComputeTagSize(fieldNumber) * this.varintList.Count;
				foreach (ulong num2 in this.varintList)
				{
					num += CodedOutputStream.ComputeUInt64Size(num2);
				}
			}
			if (this.fixed32List != null)
			{
				num += CodedOutputStream.ComputeTagSize(fieldNumber) * this.fixed32List.Count;
				num += CodedOutputStream.ComputeFixed32Size(1U) * this.fixed32List.Count;
			}
			if (this.fixed64List != null)
			{
				num += CodedOutputStream.ComputeTagSize(fieldNumber) * this.fixed64List.Count;
				num += CodedOutputStream.ComputeFixed64Size(1UL) * this.fixed64List.Count;
			}
			if (this.lengthDelimitedList != null)
			{
				num += CodedOutputStream.ComputeTagSize(fieldNumber) * this.lengthDelimitedList.Count;
				foreach (ByteString byteString in this.lengthDelimitedList)
				{
					num += CodedOutputStream.ComputeBytesSize(byteString);
				}
			}
			if (this.groupList != null)
			{
				num += CodedOutputStream.ComputeTagSize(fieldNumber) * 2 * this.groupList.Count;
				foreach (UnknownFieldSet unknownFieldSet in this.groupList)
				{
					num += unknownFieldSet.CalculateSize();
				}
			}
			return num;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x00009FCC File Offset: 0x000081CC
		internal UnknownField MergeFrom(UnknownField other)
		{
			this.varintList = UnknownField.AddAll<ulong>(this.varintList, other.varintList);
			this.fixed32List = UnknownField.AddAll<uint>(this.fixed32List, other.fixed32List);
			this.fixed64List = UnknownField.AddAll<ulong>(this.fixed64List, other.fixed64List);
			this.lengthDelimitedList = UnknownField.AddAll<ByteString>(this.lengthDelimitedList, other.lengthDelimitedList);
			this.groupList = UnknownField.AddAll<UnknownFieldSet>(this.groupList, other.groupList);
			return this;
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000A04D File Offset: 0x0000824D
		private static List<T> AddAll<T>(List<T> current, IList<T> extras)
		{
			if (extras.Count == 0)
			{
				return current;
			}
			if (current == null)
			{
				current = new List<T>(extras);
			}
			else
			{
				current.AddRange(extras);
			}
			return current;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000A06E File Offset: 0x0000826E
		internal UnknownField AddVarint(ulong value)
		{
			this.varintList = UnknownField.Add<ulong>(this.varintList, value);
			return this;
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000A083 File Offset: 0x00008283
		internal UnknownField AddFixed32(uint value)
		{
			this.fixed32List = UnknownField.Add<uint>(this.fixed32List, value);
			return this;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000A098 File Offset: 0x00008298
		internal UnknownField AddFixed64(ulong value)
		{
			this.fixed64List = UnknownField.Add<ulong>(this.fixed64List, value);
			return this;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000A0AD File Offset: 0x000082AD
		internal UnknownField AddLengthDelimited(ByteString value)
		{
			this.lengthDelimitedList = UnknownField.Add<ByteString>(this.lengthDelimitedList, value);
			return this;
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000A0C2 File Offset: 0x000082C2
		internal UnknownField AddGroup(UnknownFieldSet value)
		{
			this.groupList = UnknownField.Add<UnknownFieldSet>(this.groupList, value);
			return this;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000A0D7 File Offset: 0x000082D7
		private static List<T> Add<T>(List<T> list, T value)
		{
			if (list == null)
			{
				list = new List<T>();
			}
			list.Add(value);
			return list;
		}

		// Token: 0x04000064 RID: 100
		private List<ulong> varintList;

		// Token: 0x04000065 RID: 101
		private List<uint> fixed32List;

		// Token: 0x04000066 RID: 102
		private List<ulong> fixed64List;

		// Token: 0x04000067 RID: 103
		private List<ByteString> lengthDelimitedList;

		// Token: 0x04000068 RID: 104
		private List<UnknownFieldSet> groupList;
	}
}
