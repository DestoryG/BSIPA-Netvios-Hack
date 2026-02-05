using System;
using System.Diagnostics;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection
{
	// Token: 0x02000063 RID: 99
	public sealed class SourceCodeInfo : IMessage<SourceCodeInfo>, IMessage, IEquatable<SourceCodeInfo>, IDeepCloneable<SourceCodeInfo>
	{
		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x0600070E RID: 1806 RVA: 0x00019E2E File Offset: 0x0001802E
		[DebuggerNonUserCode]
		public static MessageParser<SourceCodeInfo> Parser
		{
			get
			{
				return SourceCodeInfo._parser;
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x0600070F RID: 1807 RVA: 0x00019E35 File Offset: 0x00018035
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return DescriptorReflection.Descriptor.MessageTypes[19];
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000710 RID: 1808 RVA: 0x00019E48 File Offset: 0x00018048
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return SourceCodeInfo.Descriptor;
			}
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x00019E4F File Offset: 0x0001804F
		[DebuggerNonUserCode]
		public SourceCodeInfo()
		{
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x00019E62 File Offset: 0x00018062
		[DebuggerNonUserCode]
		public SourceCodeInfo(SourceCodeInfo other)
			: this()
		{
			this.location_ = other.location_.Clone();
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x00019E8C File Offset: 0x0001808C
		[DebuggerNonUserCode]
		public SourceCodeInfo Clone()
		{
			return new SourceCodeInfo(this);
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000714 RID: 1812 RVA: 0x00019E94 File Offset: 0x00018094
		[DebuggerNonUserCode]
		public RepeatedField<SourceCodeInfo.Types.Location> Location
		{
			get
			{
				return this.location_;
			}
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x00019E9C File Offset: 0x0001809C
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as SourceCodeInfo);
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x00019EAA File Offset: 0x000180AA
		[DebuggerNonUserCode]
		public bool Equals(SourceCodeInfo other)
		{
			return other != null && (other == this || (this.location_.Equals(other.location_) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x00019EE0 File Offset: 0x000180E0
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			num ^= this.location_.GetHashCode();
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x00019F14 File Offset: 0x00018114
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x00019F1C File Offset: 0x0001811C
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			this.location_.WriteTo(output, SourceCodeInfo._repeated_location_codec);
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x00019F44 File Offset: 0x00018144
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			num += this.location_.CalculateSize(SourceCodeInfo._repeated_location_codec);
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x00019F7D File Offset: 0x0001817D
		[DebuggerNonUserCode]
		public void MergeFrom(SourceCodeInfo other)
		{
			if (other == null)
			{
				return;
			}
			this.location_.Add(other.location_);
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x00019FAC File Offset: 0x000181AC
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
				}
				else
				{
					this.location_.AddEntriesFrom(input, SourceCodeInfo._repeated_location_codec);
				}
			}
		}

		// Token: 0x040002BF RID: 703
		private static readonly MessageParser<SourceCodeInfo> _parser = new MessageParser<SourceCodeInfo>(() => new SourceCodeInfo());

		// Token: 0x040002C0 RID: 704
		private UnknownFieldSet _unknownFields;

		// Token: 0x040002C1 RID: 705
		public const int LocationFieldNumber = 1;

		// Token: 0x040002C2 RID: 706
		private static readonly FieldCodec<SourceCodeInfo.Types.Location> _repeated_location_codec = FieldCodec.ForMessage<SourceCodeInfo.Types.Location>(10U, SourceCodeInfo.Types.Location.Parser);

		// Token: 0x040002C3 RID: 707
		private readonly RepeatedField<SourceCodeInfo.Types.Location> location_ = new RepeatedField<SourceCodeInfo.Types.Location>();

		// Token: 0x020000E3 RID: 227
		[DebuggerNonUserCode]
		public static class Types
		{
			// Token: 0x0200011D RID: 285
			public sealed class Location : IMessage<SourceCodeInfo.Types.Location>, IMessage, IEquatable<SourceCodeInfo.Types.Location>, IDeepCloneable<SourceCodeInfo.Types.Location>
			{
				// Token: 0x17000296 RID: 662
				// (get) Token: 0x06000AE3 RID: 2787 RVA: 0x000222A5 File Offset: 0x000204A5
				[DebuggerNonUserCode]
				public static MessageParser<SourceCodeInfo.Types.Location> Parser
				{
					get
					{
						return SourceCodeInfo.Types.Location._parser;
					}
				}

				// Token: 0x17000297 RID: 663
				// (get) Token: 0x06000AE4 RID: 2788 RVA: 0x000222AC File Offset: 0x000204AC
				[DebuggerNonUserCode]
				public static MessageDescriptor Descriptor
				{
					get
					{
						return SourceCodeInfo.Descriptor.NestedTypes[0];
					}
				}

				// Token: 0x17000298 RID: 664
				// (get) Token: 0x06000AE5 RID: 2789 RVA: 0x000222BE File Offset: 0x000204BE
				[DebuggerNonUserCode]
				MessageDescriptor IMessage.Descriptor
				{
					get
					{
						return SourceCodeInfo.Types.Location.Descriptor;
					}
				}

				// Token: 0x06000AE6 RID: 2790 RVA: 0x000222C5 File Offset: 0x000204C5
				[DebuggerNonUserCode]
				public Location()
				{
				}

				// Token: 0x06000AE7 RID: 2791 RVA: 0x000222F0 File Offset: 0x000204F0
				[DebuggerNonUserCode]
				public Location(SourceCodeInfo.Types.Location other)
					: this()
				{
					this.path_ = other.path_.Clone();
					this.span_ = other.span_.Clone();
					this.leadingComments_ = other.leadingComments_;
					this.trailingComments_ = other.trailingComments_;
					this.leadingDetachedComments_ = other.leadingDetachedComments_.Clone();
					this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
				}

				// Token: 0x06000AE8 RID: 2792 RVA: 0x0002235F File Offset: 0x0002055F
				[DebuggerNonUserCode]
				public SourceCodeInfo.Types.Location Clone()
				{
					return new SourceCodeInfo.Types.Location(this);
				}

				// Token: 0x17000299 RID: 665
				// (get) Token: 0x06000AE9 RID: 2793 RVA: 0x00022367 File Offset: 0x00020567
				[DebuggerNonUserCode]
				public RepeatedField<int> Path
				{
					get
					{
						return this.path_;
					}
				}

				// Token: 0x1700029A RID: 666
				// (get) Token: 0x06000AEA RID: 2794 RVA: 0x0002236F File Offset: 0x0002056F
				[DebuggerNonUserCode]
				public RepeatedField<int> Span
				{
					get
					{
						return this.span_;
					}
				}

				// Token: 0x1700029B RID: 667
				// (get) Token: 0x06000AEB RID: 2795 RVA: 0x00022377 File Offset: 0x00020577
				// (set) Token: 0x06000AEC RID: 2796 RVA: 0x00022388 File Offset: 0x00020588
				[DebuggerNonUserCode]
				public string LeadingComments
				{
					get
					{
						return this.leadingComments_ ?? SourceCodeInfo.Types.Location.LeadingCommentsDefaultValue;
					}
					set
					{
						this.leadingComments_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
					}
				}

				// Token: 0x1700029C RID: 668
				// (get) Token: 0x06000AED RID: 2797 RVA: 0x0002239B File Offset: 0x0002059B
				[DebuggerNonUserCode]
				public bool HasLeadingComments
				{
					get
					{
						return this.leadingComments_ != null;
					}
				}

				// Token: 0x06000AEE RID: 2798 RVA: 0x000223A6 File Offset: 0x000205A6
				[DebuggerNonUserCode]
				public void ClearLeadingComments()
				{
					this.leadingComments_ = null;
				}

				// Token: 0x1700029D RID: 669
				// (get) Token: 0x06000AEF RID: 2799 RVA: 0x000223AF File Offset: 0x000205AF
				// (set) Token: 0x06000AF0 RID: 2800 RVA: 0x000223C0 File Offset: 0x000205C0
				[DebuggerNonUserCode]
				public string TrailingComments
				{
					get
					{
						return this.trailingComments_ ?? SourceCodeInfo.Types.Location.TrailingCommentsDefaultValue;
					}
					set
					{
						this.trailingComments_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
					}
				}

				// Token: 0x1700029E RID: 670
				// (get) Token: 0x06000AF1 RID: 2801 RVA: 0x000223D3 File Offset: 0x000205D3
				[DebuggerNonUserCode]
				public bool HasTrailingComments
				{
					get
					{
						return this.trailingComments_ != null;
					}
				}

				// Token: 0x06000AF2 RID: 2802 RVA: 0x000223DE File Offset: 0x000205DE
				[DebuggerNonUserCode]
				public void ClearTrailingComments()
				{
					this.trailingComments_ = null;
				}

				// Token: 0x1700029F RID: 671
				// (get) Token: 0x06000AF3 RID: 2803 RVA: 0x000223E7 File Offset: 0x000205E7
				[DebuggerNonUserCode]
				public RepeatedField<string> LeadingDetachedComments
				{
					get
					{
						return this.leadingDetachedComments_;
					}
				}

				// Token: 0x06000AF4 RID: 2804 RVA: 0x000223EF File Offset: 0x000205EF
				[DebuggerNonUserCode]
				public override bool Equals(object other)
				{
					return this.Equals(other as SourceCodeInfo.Types.Location);
				}

				// Token: 0x06000AF5 RID: 2805 RVA: 0x00022400 File Offset: 0x00020600
				[DebuggerNonUserCode]
				public bool Equals(SourceCodeInfo.Types.Location other)
				{
					return other != null && (other == this || (this.path_.Equals(other.path_) && this.span_.Equals(other.span_) && !(this.LeadingComments != other.LeadingComments) && !(this.TrailingComments != other.TrailingComments) && this.leadingDetachedComments_.Equals(other.leadingDetachedComments_) && object.Equals(this._unknownFields, other._unknownFields)));
				}

				// Token: 0x06000AF6 RID: 2806 RVA: 0x00022494 File Offset: 0x00020694
				[DebuggerNonUserCode]
				public override int GetHashCode()
				{
					int num = 1;
					num ^= this.path_.GetHashCode();
					num ^= this.span_.GetHashCode();
					if (this.HasLeadingComments)
					{
						num ^= this.LeadingComments.GetHashCode();
					}
					if (this.HasTrailingComments)
					{
						num ^= this.TrailingComments.GetHashCode();
					}
					num ^= this.leadingDetachedComments_.GetHashCode();
					if (this._unknownFields != null)
					{
						num ^= this._unknownFields.GetHashCode();
					}
					return num;
				}

				// Token: 0x06000AF7 RID: 2807 RVA: 0x00022510 File Offset: 0x00020710
				[DebuggerNonUserCode]
				public override string ToString()
				{
					return JsonFormatter.ToDiagnosticString(this);
				}

				// Token: 0x06000AF8 RID: 2808 RVA: 0x00022518 File Offset: 0x00020718
				[DebuggerNonUserCode]
				public void WriteTo(CodedOutputStream output)
				{
					this.path_.WriteTo(output, SourceCodeInfo.Types.Location._repeated_path_codec);
					this.span_.WriteTo(output, SourceCodeInfo.Types.Location._repeated_span_codec);
					if (this.HasLeadingComments)
					{
						output.WriteRawTag(26);
						output.WriteString(this.LeadingComments);
					}
					if (this.HasTrailingComments)
					{
						output.WriteRawTag(34);
						output.WriteString(this.TrailingComments);
					}
					this.leadingDetachedComments_.WriteTo(output, SourceCodeInfo.Types.Location._repeated_leadingDetachedComments_codec);
					if (this._unknownFields != null)
					{
						this._unknownFields.WriteTo(output);
					}
				}

				// Token: 0x06000AF9 RID: 2809 RVA: 0x000225A4 File Offset: 0x000207A4
				[DebuggerNonUserCode]
				public int CalculateSize()
				{
					int num = 0;
					num += this.path_.CalculateSize(SourceCodeInfo.Types.Location._repeated_path_codec);
					num += this.span_.CalculateSize(SourceCodeInfo.Types.Location._repeated_span_codec);
					if (this.HasLeadingComments)
					{
						num += 1 + CodedOutputStream.ComputeStringSize(this.LeadingComments);
					}
					if (this.HasTrailingComments)
					{
						num += 1 + CodedOutputStream.ComputeStringSize(this.TrailingComments);
					}
					num += this.leadingDetachedComments_.CalculateSize(SourceCodeInfo.Types.Location._repeated_leadingDetachedComments_codec);
					if (this._unknownFields != null)
					{
						num += this._unknownFields.CalculateSize();
					}
					return num;
				}

				// Token: 0x06000AFA RID: 2810 RVA: 0x00022634 File Offset: 0x00020834
				[DebuggerNonUserCode]
				public void MergeFrom(SourceCodeInfo.Types.Location other)
				{
					if (other == null)
					{
						return;
					}
					this.path_.Add(other.path_);
					this.span_.Add(other.span_);
					if (other.HasLeadingComments)
					{
						this.LeadingComments = other.LeadingComments;
					}
					if (other.HasTrailingComments)
					{
						this.TrailingComments = other.TrailingComments;
					}
					this.leadingDetachedComments_.Add(other.leadingDetachedComments_);
					this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
				}

				// Token: 0x06000AFB RID: 2811 RVA: 0x000226B8 File Offset: 0x000208B8
				[DebuggerNonUserCode]
				public void MergeFrom(CodedInputStream input)
				{
					uint num;
					while ((num = input.ReadTag()) != 0U)
					{
						if (num <= 16U)
						{
							if (num == 8U || num == 10U)
							{
								this.path_.AddEntriesFrom(input, SourceCodeInfo.Types.Location._repeated_path_codec);
								continue;
							}
							if (num == 16U)
							{
								goto IL_005C;
							}
						}
						else if (num <= 26U)
						{
							if (num == 18U)
							{
								goto IL_005C;
							}
							if (num == 26U)
							{
								this.LeadingComments = input.ReadString();
								continue;
							}
						}
						else
						{
							if (num == 34U)
							{
								this.TrailingComments = input.ReadString();
								continue;
							}
							if (num == 50U)
							{
								this.leadingDetachedComments_.AddEntriesFrom(input, SourceCodeInfo.Types.Location._repeated_leadingDetachedComments_codec);
								continue;
							}
						}
						this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
						continue;
						IL_005C:
						this.span_.AddEntriesFrom(input, SourceCodeInfo.Types.Location._repeated_span_codec);
					}
				}

				// Token: 0x040004BC RID: 1212
				private static readonly MessageParser<SourceCodeInfo.Types.Location> _parser = new MessageParser<SourceCodeInfo.Types.Location>(() => new SourceCodeInfo.Types.Location());

				// Token: 0x040004BD RID: 1213
				private UnknownFieldSet _unknownFields;

				// Token: 0x040004BE RID: 1214
				public const int PathFieldNumber = 1;

				// Token: 0x040004BF RID: 1215
				private static readonly FieldCodec<int> _repeated_path_codec = FieldCodec.ForInt32(10U);

				// Token: 0x040004C0 RID: 1216
				private readonly RepeatedField<int> path_ = new RepeatedField<int>();

				// Token: 0x040004C1 RID: 1217
				public const int SpanFieldNumber = 2;

				// Token: 0x040004C2 RID: 1218
				private static readonly FieldCodec<int> _repeated_span_codec = FieldCodec.ForInt32(18U);

				// Token: 0x040004C3 RID: 1219
				private readonly RepeatedField<int> span_ = new RepeatedField<int>();

				// Token: 0x040004C4 RID: 1220
				public const int LeadingCommentsFieldNumber = 3;

				// Token: 0x040004C5 RID: 1221
				private static readonly string LeadingCommentsDefaultValue = "";

				// Token: 0x040004C6 RID: 1222
				private string leadingComments_;

				// Token: 0x040004C7 RID: 1223
				public const int TrailingCommentsFieldNumber = 4;

				// Token: 0x040004C8 RID: 1224
				private static readonly string TrailingCommentsDefaultValue = "";

				// Token: 0x040004C9 RID: 1225
				private string trailingComments_;

				// Token: 0x040004CA RID: 1226
				public const int LeadingDetachedCommentsFieldNumber = 6;

				// Token: 0x040004CB RID: 1227
				private static readonly FieldCodec<string> _repeated_leadingDetachedComments_codec = FieldCodec.ForString(50U);

				// Token: 0x040004CC RID: 1228
				private readonly RepeatedField<string> leadingDetachedComments_ = new RepeatedField<string>();
			}
		}
	}
}
