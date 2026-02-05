using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Inbound
{
	// Token: 0x02000040 RID: 64
	public sealed class ModifyRoomCfg : IMessage<ModifyRoomCfg>, IMessage, IEquatable<ModifyRoomCfg>, IDeepCloneable<ModifyRoomCfg>
	{
		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000533 RID: 1331 RVA: 0x00015DD0 File Offset: 0x00013FD0
		[DebuggerNonUserCode]
		public static MessageParser<ModifyRoomCfg> Parser
		{
			get
			{
				return ModifyRoomCfg._parser;
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x00015DD7 File Offset: 0x00013FD7
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberInboundMessageReflection.Descriptor.MessageTypes[16];
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000535 RID: 1333 RVA: 0x00015DEA File Offset: 0x00013FEA
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return ModifyRoomCfg.Descriptor;
			}
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x00015DF1 File Offset: 0x00013FF1
		[DebuggerNonUserCode]
		public ModifyRoomCfg()
		{
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x00015E10 File Offset: 0x00014010
		[DebuggerNonUserCode]
		public ModifyRoomCfg(ModifyRoomCfg other)
			: this()
		{
			this.roomId_ = other.roomId_;
			this.roomName_ = other.roomName_;
			this.maxPlayers_ = other.maxPlayers_;
			this.Password = other.Password;
			this.resultDisplaySeconds_ = other.resultDisplaySeconds_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x00015E70 File Offset: 0x00014070
		[DebuggerNonUserCode]
		public ModifyRoomCfg Clone()
		{
			return new ModifyRoomCfg(this);
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000539 RID: 1337 RVA: 0x00015E78 File Offset: 0x00014078
		// (set) Token: 0x0600053A RID: 1338 RVA: 0x00015E80 File Offset: 0x00014080
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

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x0600053B RID: 1339 RVA: 0x00015E93 File Offset: 0x00014093
		// (set) Token: 0x0600053C RID: 1340 RVA: 0x00015E9B File Offset: 0x0001409B
		[DebuggerNonUserCode]
		public string RoomName
		{
			get
			{
				return this.roomName_;
			}
			set
			{
				this.roomName_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x0600053D RID: 1341 RVA: 0x00015EAE File Offset: 0x000140AE
		// (set) Token: 0x0600053E RID: 1342 RVA: 0x00015EB6 File Offset: 0x000140B6
		[DebuggerNonUserCode]
		public int MaxPlayers
		{
			get
			{
				return this.maxPlayers_;
			}
			set
			{
				this.maxPlayers_ = value;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x0600053F RID: 1343 RVA: 0x00015EBF File Offset: 0x000140BF
		// (set) Token: 0x06000540 RID: 1344 RVA: 0x00015EC7 File Offset: 0x000140C7
		[DebuggerNonUserCode]
		public string Password
		{
			get
			{
				return this.password_;
			}
			set
			{
				this.password_ = value;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000541 RID: 1345 RVA: 0x00015ED0 File Offset: 0x000140D0
		// (set) Token: 0x06000542 RID: 1346 RVA: 0x00015ED8 File Offset: 0x000140D8
		[DebuggerNonUserCode]
		public int ResultDisplaySeconds
		{
			get
			{
				return this.resultDisplaySeconds_;
			}
			set
			{
				this.resultDisplaySeconds_ = value;
			}
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x00015EE1 File Offset: 0x000140E1
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as ModifyRoomCfg);
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x00015EF0 File Offset: 0x000140F0
		[DebuggerNonUserCode]
		public bool Equals(ModifyRoomCfg other)
		{
			return other != null && (other == this || (!(this.RoomId != other.RoomId) && !(this.RoomName != other.RoomName) && this.MaxPlayers == other.MaxPlayers && !(this.Password != other.Password) && this.ResultDisplaySeconds == other.ResultDisplaySeconds && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x00015F78 File Offset: 0x00014178
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.RoomId.Length != 0)
			{
				num ^= this.RoomId.GetHashCode();
			}
			if (this.RoomName.Length != 0)
			{
				num ^= this.RoomName.GetHashCode();
			}
			if (this.MaxPlayers != 0)
			{
				num ^= this.MaxPlayers.GetHashCode();
			}
			if (this.password_ != null)
			{
				num ^= this.Password.GetHashCode();
			}
			if (this.ResultDisplaySeconds != 0)
			{
				num ^= this.ResultDisplaySeconds.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0001601C File Offset: 0x0001421C
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.RoomId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.RoomId);
			}
			if (this.RoomName.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.RoomName);
			}
			if (this.MaxPlayers != 0)
			{
				output.WriteRawTag(24);
				output.WriteInt32(this.MaxPlayers);
			}
			if (this.password_ != null)
			{
				ModifyRoomCfg._single_password_codec.WriteTagAndValue(output, this.Password);
			}
			if (this.ResultDisplaySeconds != 0)
			{
				output.WriteRawTag(40);
				output.WriteInt32(this.ResultDisplaySeconds);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x000160D0 File Offset: 0x000142D0
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.RoomId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.RoomId);
			}
			if (this.RoomName.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.RoomName);
			}
			if (this.MaxPlayers != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.MaxPlayers);
			}
			if (this.password_ != null)
			{
				num += ModifyRoomCfg._single_password_codec.CalculateSizeWithTag(this.Password);
			}
			if (this.ResultDisplaySeconds != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ResultDisplaySeconds);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0001617C File Offset: 0x0001437C
		[DebuggerNonUserCode]
		public void MergeFrom(ModifyRoomCfg other)
		{
			if (other == null)
			{
				return;
			}
			if (other.RoomId.Length != 0)
			{
				this.RoomId = other.RoomId;
			}
			if (other.RoomName.Length != 0)
			{
				this.RoomName = other.RoomName;
			}
			if (other.MaxPlayers != 0)
			{
				this.MaxPlayers = other.MaxPlayers;
			}
			if (other.password_ != null && (this.password_ == null || other.Password != ""))
			{
				this.Password = other.Password;
			}
			if (other.ResultDisplaySeconds != 0)
			{
				this.ResultDisplaySeconds = other.ResultDisplaySeconds;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0001622C File Offset: 0x0001442C
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
						this.RoomName = input.ReadString();
						continue;
					}
				}
				else
				{
					if (num == 24U)
					{
						this.MaxPlayers = input.ReadInt32();
						continue;
					}
					if (num != 34U)
					{
						if (num == 40U)
						{
							this.ResultDisplaySeconds = input.ReadInt32();
							continue;
						}
					}
					else
					{
						string text = ModifyRoomCfg._single_password_codec.Read(input);
						if (this.password_ == null || text != "")
						{
							this.Password = text;
							continue;
						}
						continue;
					}
				}
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x04000271 RID: 625
		private static readonly MessageParser<ModifyRoomCfg> _parser = new MessageParser<ModifyRoomCfg>(() => new ModifyRoomCfg());

		// Token: 0x04000272 RID: 626
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000273 RID: 627
		public const int RoomIdFieldNumber = 1;

		// Token: 0x04000274 RID: 628
		private string roomId_ = "";

		// Token: 0x04000275 RID: 629
		public const int RoomNameFieldNumber = 2;

		// Token: 0x04000276 RID: 630
		private string roomName_ = "";

		// Token: 0x04000277 RID: 631
		public const int MaxPlayersFieldNumber = 3;

		// Token: 0x04000278 RID: 632
		private int maxPlayers_;

		// Token: 0x04000279 RID: 633
		public const int PasswordFieldNumber = 4;

		// Token: 0x0400027A RID: 634
		private static readonly FieldCodec<string> _single_password_codec = FieldCodec.ForClassWrapper<string>(34U);

		// Token: 0x0400027B RID: 635
		private string password_;

		// Token: 0x0400027C RID: 636
		public const int ResultDisplaySecondsFieldNumber = 5;

		// Token: 0x0400027D RID: 637
		private int resultDisplaySeconds_;
	}
}
