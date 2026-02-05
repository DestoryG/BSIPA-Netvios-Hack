using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Inbound
{
	// Token: 0x02000041 RID: 65
	public sealed class ModifySongCfg : IMessage<ModifySongCfg>, IMessage, IEquatable<ModifySongCfg>, IDeepCloneable<ModifySongCfg>
	{
		// Token: 0x170001AF RID: 431
		// (get) Token: 0x0600054C RID: 1356 RVA: 0x00016307 File Offset: 0x00014507
		[DebuggerNonUserCode]
		public static MessageParser<ModifySongCfg> Parser
		{
			get
			{
				return ModifySongCfg._parser;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x0600054D RID: 1357 RVA: 0x0001630E File Offset: 0x0001450E
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberInboundMessageReflection.Descriptor.MessageTypes[17];
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x0600054E RID: 1358 RVA: 0x00016321 File Offset: 0x00014521
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return ModifySongCfg.Descriptor;
			}
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x00016328 File Offset: 0x00014528
		[DebuggerNonUserCode]
		public ModifySongCfg()
		{
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x00016368 File Offset: 0x00014568
		[DebuggerNonUserCode]
		public ModifySongCfg(ModifySongCfg other)
			: this()
		{
			this.roomId_ = other.roomId_;
			this.songId_ = other.songId_;
			this.mode_ = other.mode_;
			this.difficulty_ = other.difficulty_;
			this.rules_ = other.rules_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x000163C8 File Offset: 0x000145C8
		[DebuggerNonUserCode]
		public ModifySongCfg Clone()
		{
			return new ModifySongCfg(this);
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000552 RID: 1362 RVA: 0x000163D0 File Offset: 0x000145D0
		// (set) Token: 0x06000553 RID: 1363 RVA: 0x000163D8 File Offset: 0x000145D8
		[DebuggerNonUserCode]
		public string RoomId
		{
			get
			{
				return this.roomId_;
			}
			set
			{
				this.roomId_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000554 RID: 1364 RVA: 0x000163EB File Offset: 0x000145EB
		// (set) Token: 0x06000555 RID: 1365 RVA: 0x000163F3 File Offset: 0x000145F3
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

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000556 RID: 1366 RVA: 0x00016406 File Offset: 0x00014606
		// (set) Token: 0x06000557 RID: 1367 RVA: 0x0001640E File Offset: 0x0001460E
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

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000558 RID: 1368 RVA: 0x00016421 File Offset: 0x00014621
		// (set) Token: 0x06000559 RID: 1369 RVA: 0x00016429 File Offset: 0x00014629
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

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x0600055A RID: 1370 RVA: 0x0001643C File Offset: 0x0001463C
		// (set) Token: 0x0600055B RID: 1371 RVA: 0x00016444 File Offset: 0x00014644
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

		// Token: 0x0600055C RID: 1372 RVA: 0x00016457 File Offset: 0x00014657
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as ModifySongCfg);
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x00016468 File Offset: 0x00014668
		[DebuggerNonUserCode]
		public bool Equals(ModifySongCfg other)
		{
			return other != null && (other == this || (!(this.RoomId != other.RoomId) && !(this.SongId != other.SongId) && !(this.Mode != other.Mode) && !(this.Difficulty != other.Difficulty) && !(this.Rules != other.Rules) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x000164FC File Offset: 0x000146FC
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.RoomId.Length != 0)
			{
				num ^= this.RoomId.GetHashCode();
			}
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

		// Token: 0x0600055F RID: 1375 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x000165AC File Offset: 0x000147AC
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.RoomId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.RoomId);
			}
			if (this.SongId.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.SongId);
			}
			if (this.Mode.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.Mode);
			}
			if (this.Difficulty.Length != 0)
			{
				output.WriteRawTag(34);
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

		// Token: 0x06000561 RID: 1377 RVA: 0x00016674 File Offset: 0x00014874
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.RoomId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.RoomId);
			}
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

		// Token: 0x06000562 RID: 1378 RVA: 0x0001672C File Offset: 0x0001492C
		[DebuggerNonUserCode]
		public void MergeFrom(ModifySongCfg other)
		{
			if (other == null)
			{
				return;
			}
			if (other.RoomId.Length != 0)
			{
				this.RoomId = other.RoomId;
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

		// Token: 0x06000563 RID: 1379 RVA: 0x000167D4 File Offset: 0x000149D4
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
						this.RoomId = input.ReadString();
						continue;
					}
					if (num == 18U)
					{
						this.SongId = input.ReadString();
						continue;
					}
				}
				else
				{
					if (num == 26U)
					{
						this.Mode = input.ReadString();
						continue;
					}
					if (num == 34U)
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

		// Token: 0x0400027E RID: 638
		private static readonly MessageParser<ModifySongCfg> _parser = new MessageParser<ModifySongCfg>(() => new ModifySongCfg());

		// Token: 0x0400027F RID: 639
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000280 RID: 640
		public const int RoomIdFieldNumber = 1;

		// Token: 0x04000281 RID: 641
		private string roomId_ = "";

		// Token: 0x04000282 RID: 642
		public const int SongIdFieldNumber = 2;

		// Token: 0x04000283 RID: 643
		private string songId_ = "";

		// Token: 0x04000284 RID: 644
		public const int ModeFieldNumber = 3;

		// Token: 0x04000285 RID: 645
		private string mode_ = "";

		// Token: 0x04000286 RID: 646
		public const int DifficultyFieldNumber = 4;

		// Token: 0x04000287 RID: 647
		private string difficulty_ = "";

		// Token: 0x04000288 RID: 648
		public const int RulesFieldNumber = 10;

		// Token: 0x04000289 RID: 649
		private string rules_ = "";
	}
}
