using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Inbound
{
	// Token: 0x02000049 RID: 73
	public sealed class SongCfg : IMessage<SongCfg>, IMessage, IEquatable<SongCfg>, IDeepCloneable<SongCfg>
	{
		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000619 RID: 1561 RVA: 0x00018D5E File Offset: 0x00016F5E
		[DebuggerNonUserCode]
		public static MessageParser<SongCfg> Parser
		{
			get
			{
				return SongCfg._parser;
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x0600061A RID: 1562 RVA: 0x00018D65 File Offset: 0x00016F65
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberInboundMessageReflection.Descriptor.MessageTypes[25];
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x0600061B RID: 1563 RVA: 0x00018D78 File Offset: 0x00016F78
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return SongCfg.Descriptor;
			}
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x00018D7F File Offset: 0x00016F7F
		[DebuggerNonUserCode]
		public SongCfg()
		{
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x00018DB4 File Offset: 0x00016FB4
		[DebuggerNonUserCode]
		public SongCfg(SongCfg other)
			: this()
		{
			this.songId_ = other.songId_;
			this.mode_ = other.mode_;
			this.difficulty_ = other.difficulty_;
			this.rules_ = other.rules_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x00018E08 File Offset: 0x00017008
		[DebuggerNonUserCode]
		public SongCfg Clone()
		{
			return new SongCfg(this);
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x0600061F RID: 1567 RVA: 0x00018E10 File Offset: 0x00017010
		// (set) Token: 0x06000620 RID: 1568 RVA: 0x00018E18 File Offset: 0x00017018
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

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000621 RID: 1569 RVA: 0x00018E2B File Offset: 0x0001702B
		// (set) Token: 0x06000622 RID: 1570 RVA: 0x00018E33 File Offset: 0x00017033
		[DebuggerNonUserCode]
		public string Mode
		{
			get
			{
				return this.mode_;
			}
			set
			{
				this.mode_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000623 RID: 1571 RVA: 0x00018E46 File Offset: 0x00017046
		// (set) Token: 0x06000624 RID: 1572 RVA: 0x00018E4E File Offset: 0x0001704E
		[DebuggerNonUserCode]
		public string Difficulty
		{
			get
			{
				return this.difficulty_;
			}
			set
			{
				this.difficulty_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000625 RID: 1573 RVA: 0x00018E61 File Offset: 0x00017061
		// (set) Token: 0x06000626 RID: 1574 RVA: 0x00018E69 File Offset: 0x00017069
		[DebuggerNonUserCode]
		public string Rules
		{
			get
			{
				return this.rules_;
			}
			set
			{
				this.rules_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x00018E7C File Offset: 0x0001707C
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as SongCfg);
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x00018E8C File Offset: 0x0001708C
		[DebuggerNonUserCode]
		public bool Equals(SongCfg other)
		{
			return other != null && (other == this || (!(this.SongId != other.SongId) && !(this.Mode != other.Mode) && !(this.Difficulty != other.Difficulty) && !(this.Rules != other.Rules) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x00018F0C File Offset: 0x0001710C
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.SongId.Length != 0)
			{
				num ^= this.SongId.GetHashCode();
			}
			if (this.Mode.Length != 0)
			{
				num ^= this.Mode.GetHashCode();
			}
			if (this.Difficulty.Length != 0)
			{
				num ^= this.Difficulty.GetHashCode();
			}
			if (this.Rules.Length != 0)
			{
				num ^= this.Rules.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x00018FA0 File Offset: 0x000171A0
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.SongId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.SongId);
			}
			if (this.Mode.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.Mode);
			}
			if (this.Difficulty.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.Difficulty);
			}
			if (this.Rules.Length != 0)
			{
				output.WriteRawTag(82);
				output.WriteString(this.Rules);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x0600062C RID: 1580 RVA: 0x00019048 File Offset: 0x00017248
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.SongId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.SongId);
			}
			if (this.Mode.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Mode);
			}
			if (this.Difficulty.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Difficulty);
			}
			if (this.Rules.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Rules);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x0600062D RID: 1581 RVA: 0x000190E4 File Offset: 0x000172E4
		[DebuggerNonUserCode]
		public void MergeFrom(SongCfg other)
		{
			if (other == null)
			{
				return;
			}
			if (other.SongId.Length != 0)
			{
				this.SongId = other.SongId;
			}
			if (other.Mode.Length != 0)
			{
				this.Mode = other.Mode;
			}
			if (other.Difficulty.Length != 0)
			{
				this.Difficulty = other.Difficulty;
			}
			if (other.Rules.Length != 0)
			{
				this.Rules = other.Rules;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x0600062E RID: 1582 RVA: 0x00019170 File Offset: 0x00017370
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 18U)
				{
					if (num == 10U)
					{
						this.SongId = input.ReadString();
						continue;
					}
					if (num == 18U)
					{
						this.Mode = input.ReadString();
						continue;
					}
				}
				else
				{
					if (num == 26U)
					{
						this.Difficulty = input.ReadString();
						continue;
					}
					if (num == 82U)
					{
						this.Rules = input.ReadString();
						continue;
					}
				}
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x040002E5 RID: 741
		private static readonly MessageParser<SongCfg> _parser = new MessageParser<SongCfg>(() => new SongCfg());

		// Token: 0x040002E6 RID: 742
		private UnknownFieldSet _unknownFields;

		// Token: 0x040002E7 RID: 743
		public const int SongIdFieldNumber = 1;

		// Token: 0x040002E8 RID: 744
		private string songId_ = "";

		// Token: 0x040002E9 RID: 745
		public const int ModeFieldNumber = 2;

		// Token: 0x040002EA RID: 746
		private string mode_ = "";

		// Token: 0x040002EB RID: 747
		public const int DifficultyFieldNumber = 3;

		// Token: 0x040002EC RID: 748
		private string difficulty_ = "";

		// Token: 0x040002ED RID: 749
		public const int RulesFieldNumber = 10;

		// Token: 0x040002EE RID: 750
		private string rules_ = "";
	}
}
