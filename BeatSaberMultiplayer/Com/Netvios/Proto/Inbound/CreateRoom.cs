using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Inbound
{
	// Token: 0x02000039 RID: 57
	public sealed class CreateRoom : IMessage<CreateRoom>, IMessage, IEquatable<CreateRoom>, IDeepCloneable<CreateRoom>
	{
		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x00014846 File Offset: 0x00012A46
		[DebuggerNonUserCode]
		public static MessageParser<CreateRoom> Parser
		{
			get
			{
				return CreateRoom._parser;
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060004AF RID: 1199 RVA: 0x0001484D File Offset: 0x00012A4D
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberInboundMessageReflection.Descriptor.MessageTypes[9];
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060004B0 RID: 1200 RVA: 0x00014860 File Offset: 0x00012A60
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return CreateRoom.Descriptor;
			}
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x0000370C File Offset: 0x0000190C
		[DebuggerNonUserCode]
		public CreateRoom()
		{
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00014868 File Offset: 0x00012A68
		[DebuggerNonUserCode]
		public CreateRoom(CreateRoom other)
			: this()
		{
			this.roomCfg_ = ((other.roomCfg_ != null) ? other.roomCfg_.Clone() : null);
			this.songCfg_ = ((other.songCfg_ != null) ? other.songCfg_.Clone() : null);
			this.personalCfg_ = ((other.personalCfg_ != null) ? other.personalCfg_.Clone() : null);
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x000148E0 File Offset: 0x00012AE0
		[DebuggerNonUserCode]
		public CreateRoom Clone()
		{
			return new CreateRoom(this);
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x000148E8 File Offset: 0x00012AE8
		// (set) Token: 0x060004B5 RID: 1205 RVA: 0x000148F0 File Offset: 0x00012AF0
		[DebuggerNonUserCode]
		public RoomCfg RoomCfg
		{
			get
			{
				return this.roomCfg_;
			}
			set
			{
				this.roomCfg_ = value;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060004B6 RID: 1206 RVA: 0x000148F9 File Offset: 0x00012AF9
		// (set) Token: 0x060004B7 RID: 1207 RVA: 0x00014901 File Offset: 0x00012B01
		[DebuggerNonUserCode]
		public SongCfg SongCfg
		{
			get
			{
				return this.songCfg_;
			}
			set
			{
				this.songCfg_ = value;
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060004B8 RID: 1208 RVA: 0x0001490A File Offset: 0x00012B0A
		// (set) Token: 0x060004B9 RID: 1209 RVA: 0x00014912 File Offset: 0x00012B12
		[DebuggerNonUserCode]
		public PersonalCfg PersonalCfg
		{
			get
			{
				return this.personalCfg_;
			}
			set
			{
				this.personalCfg_ = value;
			}
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0001491B File Offset: 0x00012B1B
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as CreateRoom);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0001492C File Offset: 0x00012B2C
		[DebuggerNonUserCode]
		public bool Equals(CreateRoom other)
		{
			return other != null && (other == this || (object.Equals(this.RoomCfg, other.RoomCfg) && object.Equals(this.SongCfg, other.SongCfg) && object.Equals(this.PersonalCfg, other.PersonalCfg) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00014994 File Offset: 0x00012B94
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.roomCfg_ != null)
			{
				num ^= this.RoomCfg.GetHashCode();
			}
			if (this.songCfg_ != null)
			{
				num ^= this.SongCfg.GetHashCode();
			}
			if (this.personalCfg_ != null)
			{
				num ^= this.PersonalCfg.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x000149FC File Offset: 0x00012BFC
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.roomCfg_ != null)
			{
				output.WriteRawTag(10);
				output.WriteMessage(this.RoomCfg);
			}
			if (this.songCfg_ != null)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.SongCfg);
			}
			if (this.personalCfg_ != null)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.PersonalCfg);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x00014A74 File Offset: 0x00012C74
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.roomCfg_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.RoomCfg);
			}
			if (this.songCfg_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.SongCfg);
			}
			if (this.personalCfg_ != null)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.PersonalCfg);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x00014AE4 File Offset: 0x00012CE4
		[DebuggerNonUserCode]
		public void MergeFrom(CreateRoom other)
		{
			if (other == null)
			{
				return;
			}
			if (other.roomCfg_ != null)
			{
				if (this.roomCfg_ == null)
				{
					this.RoomCfg = new RoomCfg();
				}
				this.RoomCfg.MergeFrom(other.RoomCfg);
			}
			if (other.songCfg_ != null)
			{
				if (this.songCfg_ == null)
				{
					this.SongCfg = new SongCfg();
				}
				this.SongCfg.MergeFrom(other.SongCfg);
			}
			if (other.personalCfg_ != null)
			{
				if (this.personalCfg_ == null)
				{
					this.PersonalCfg = new PersonalCfg();
				}
				this.PersonalCfg.MergeFrom(other.PersonalCfg);
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x00014B90 File Offset: 0x00012D90
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					if (num != 18U)
					{
						if (num != 26U)
						{
							this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
						}
						else
						{
							if (this.personalCfg_ == null)
							{
								this.PersonalCfg = new PersonalCfg();
							}
							input.ReadMessage(this.PersonalCfg);
						}
					}
					else
					{
						if (this.songCfg_ == null)
						{
							this.SongCfg = new SongCfg();
						}
						input.ReadMessage(this.SongCfg);
					}
				}
				else
				{
					if (this.roomCfg_ == null)
					{
						this.RoomCfg = new RoomCfg();
					}
					input.ReadMessage(this.RoomCfg);
				}
			}
		}

		// Token: 0x04000245 RID: 581
		private static readonly MessageParser<CreateRoom> _parser = new MessageParser<CreateRoom>(() => new CreateRoom());

		// Token: 0x04000246 RID: 582
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000247 RID: 583
		public const int RoomCfgFieldNumber = 1;

		// Token: 0x04000248 RID: 584
		private RoomCfg roomCfg_;

		// Token: 0x04000249 RID: 585
		public const int SongCfgFieldNumber = 2;

		// Token: 0x0400024A RID: 586
		private SongCfg songCfg_;

		// Token: 0x0400024B RID: 587
		public const int PersonalCfgFieldNumber = 3;

		// Token: 0x0400024C RID: 588
		private PersonalCfg personalCfg_;
	}
}
