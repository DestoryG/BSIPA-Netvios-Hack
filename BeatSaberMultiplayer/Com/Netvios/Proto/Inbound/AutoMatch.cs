using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Inbound
{
	// Token: 0x02000046 RID: 70
	public sealed class AutoMatch : IMessage<AutoMatch>, IMessage, IEquatable<AutoMatch>, IDeepCloneable<AutoMatch>
	{
		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x060005DE RID: 1502 RVA: 0x0001848A File Offset: 0x0001668A
		[DebuggerNonUserCode]
		public static MessageParser<AutoMatch> Parser
		{
			get
			{
				return AutoMatch._parser;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x060005DF RID: 1503 RVA: 0x00018491 File Offset: 0x00016691
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberInboundMessageReflection.Descriptor.MessageTypes[22];
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x060005E0 RID: 1504 RVA: 0x000184A4 File Offset: 0x000166A4
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return AutoMatch.Descriptor;
			}
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x000184AB File Offset: 0x000166AB
		[DebuggerNonUserCode]
		public AutoMatch()
		{
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x000184BE File Offset: 0x000166BE
		[DebuggerNonUserCode]
		public AutoMatch(AutoMatch other)
			: this()
		{
			this.songId_ = other.songId_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x000184E3 File Offset: 0x000166E3
		[DebuggerNonUserCode]
		public AutoMatch Clone()
		{
			return new AutoMatch(this);
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x060005E4 RID: 1508 RVA: 0x000184EB File Offset: 0x000166EB
		// (set) Token: 0x060005E5 RID: 1509 RVA: 0x000184F3 File Offset: 0x000166F3
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

		// Token: 0x060005E6 RID: 1510 RVA: 0x00018506 File Offset: 0x00016706
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as AutoMatch);
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x00018514 File Offset: 0x00016714
		[DebuggerNonUserCode]
		public bool Equals(AutoMatch other)
		{
			return other != null && (other == this || (!(this.SongId != other.SongId) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x00018548 File Offset: 0x00016748
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

		// Token: 0x060005E9 RID: 1513 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x00018589 File Offset: 0x00016789
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

		// Token: 0x060005EB RID: 1515 RVA: 0x000185C0 File Offset: 0x000167C0
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

		// Token: 0x060005EC RID: 1516 RVA: 0x00018603 File Offset: 0x00016803
		[DebuggerNonUserCode]
		public void MergeFrom(AutoMatch other)
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

		// Token: 0x060005ED RID: 1517 RVA: 0x0001863C File Offset: 0x0001683C
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

		// Token: 0x040002D1 RID: 721
		private static readonly MessageParser<AutoMatch> _parser = new MessageParser<AutoMatch>(() => new AutoMatch());

		// Token: 0x040002D2 RID: 722
		private UnknownFieldSet _unknownFields;

		// Token: 0x040002D3 RID: 723
		public const int SongIdFieldNumber = 1;

		// Token: 0x040002D4 RID: 724
		private string songId_ = "";
	}
}
