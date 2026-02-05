using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Inbound
{
	// Token: 0x02000045 RID: 69
	public sealed class FastMatch : IMessage<FastMatch>, IMessage, IEquatable<FastMatch>, IDeepCloneable<FastMatch>
	{
		// Token: 0x170001DE RID: 478
		// (get) Token: 0x060005CD RID: 1485 RVA: 0x0001827F File Offset: 0x0001647F
		[DebuggerNonUserCode]
		public static MessageParser<FastMatch> Parser
		{
			get
			{
				return FastMatch._parser;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060005CE RID: 1486 RVA: 0x00018286 File Offset: 0x00016486
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberInboundMessageReflection.Descriptor.MessageTypes[21];
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x060005CF RID: 1487 RVA: 0x00018299 File Offset: 0x00016499
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return FastMatch.Descriptor;
			}
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x000182A0 File Offset: 0x000164A0
		[DebuggerNonUserCode]
		public FastMatch()
		{
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x000182B3 File Offset: 0x000164B3
		[DebuggerNonUserCode]
		public FastMatch(FastMatch other)
			: this()
		{
			this.songId_ = other.songId_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x000182D8 File Offset: 0x000164D8
		[DebuggerNonUserCode]
		public FastMatch Clone()
		{
			return new FastMatch(this);
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x060005D3 RID: 1491 RVA: 0x000182E0 File Offset: 0x000164E0
		// (set) Token: 0x060005D4 RID: 1492 RVA: 0x000182E8 File Offset: 0x000164E8
		[DebuggerNonUserCode]
		public string SongId
		{
			get
			{
				return this.songId_;
			}
			set
			{
				this.songId_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x000182FB File Offset: 0x000164FB
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as FastMatch);
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x00018309 File Offset: 0x00016509
		[DebuggerNonUserCode]
		public bool Equals(FastMatch other)
		{
			return other != null && (other == this || (!(this.SongId != other.SongId) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x0001833C File Offset: 0x0001653C
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.SongId.Length != 0)
			{
				num ^= this.SongId.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x0001837D File Offset: 0x0001657D
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.SongId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.SongId);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x000183B4 File Offset: 0x000165B4
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.SongId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.SongId);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x000183F7 File Offset: 0x000165F7
		[DebuggerNonUserCode]
		public void MergeFrom(FastMatch other)
		{
			if (other == null)
			{
				return;
			}
			if (other.SongId.Length != 0)
			{
				this.SongId = other.SongId;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x00018430 File Offset: 0x00016630
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
					this.SongId = input.ReadString();
				}
			}
		}

		// Token: 0x040002CD RID: 717
		private static readonly MessageParser<FastMatch> _parser = new MessageParser<FastMatch>(() => new FastMatch());

		// Token: 0x040002CE RID: 718
		private UnknownFieldSet _unknownFields;

		// Token: 0x040002CF RID: 719
		public const int SongIdFieldNumber = 1;

		// Token: 0x040002D0 RID: 720
		private string songId_ = "";
	}
}
