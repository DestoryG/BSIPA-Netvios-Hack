using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x02000008 RID: 8
	public sealed class BeatSaberBody : IMessage<BeatSaberBody>, IMessage, IEquatable<BeatSaberBody>, IDeepCloneable<BeatSaberBody>
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000012 RID: 18 RVA: 0x000036EC File Offset: 0x000018EC
		[DebuggerNonUserCode]
		public static MessageParser<BeatSaberBody> Parser
		{
			get
			{
				return BeatSaberBody._parser;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000013 RID: 19 RVA: 0x000036F3 File Offset: 0x000018F3
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[0];
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000014 RID: 20 RVA: 0x00003705 File Offset: 0x00001905
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return BeatSaberBody.Descriptor;
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000370C File Offset: 0x0000190C
		[DebuggerNonUserCode]
		public BeatSaberBody()
		{
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00003714 File Offset: 0x00001914
		[DebuggerNonUserCode]
		public BeatSaberBody(BeatSaberBody other)
			: this()
		{
			this.type_ = other.type_;
			switch (other.DataCase)
			{
			case BeatSaberBody.DataOneofCase.Ping:
				this.Ping = other.Ping.Clone();
				break;
			case BeatSaberBody.DataOneofCase.Login:
				this.Login = other.Login.Clone();
				break;
			case BeatSaberBody.DataOneofCase.Renew:
				this.Renew = other.Renew.Clone();
				break;
			case BeatSaberBody.DataOneofCase.Logout:
				this.Logout = other.Logout.Clone();
				break;
			case BeatSaberBody.DataOneofCase.GetPlayer:
				this.GetPlayer = other.GetPlayer.Clone();
				break;
			case BeatSaberBody.DataOneofCase.SongList:
				this.SongList = other.SongList.Clone();
				break;
			case BeatSaberBody.DataOneofCase.RoomList:
				this.RoomList = other.RoomList.Clone();
				break;
			case BeatSaberBody.DataOneofCase.GetRoom:
				this.GetRoom = other.GetRoom.Clone();
				break;
			case BeatSaberBody.DataOneofCase.CreateRoom:
				this.CreateRoom = other.CreateRoom.Clone();
				break;
			case BeatSaberBody.DataOneofCase.JoinRoom:
				this.JoinRoom = other.JoinRoom.Clone();
				break;
			case BeatSaberBody.DataOneofCase.ExitRoom:
				this.ExitRoom = other.ExitRoom.Clone();
				break;
			case BeatSaberBody.DataOneofCase.KickOutRoomPlayer:
				this.KickOutRoomPlayer = other.KickOutRoomPlayer.Clone();
				break;
			case BeatSaberBody.DataOneofCase.StartGame:
				this.StartGame = other.StartGame.Clone();
				break;
			case BeatSaberBody.DataOneofCase.RoomBroadcast:
				this.RoomBroadcast = other.RoomBroadcast.Clone();
				break;
			case BeatSaberBody.DataOneofCase.ChangeRoomOwner:
				this.ChangeRoomOwner = other.ChangeRoomOwner.Clone();
				break;
			case BeatSaberBody.DataOneofCase.ModifyPersonalCfg:
				this.ModifyPersonalCfg = other.ModifyPersonalCfg.Clone();
				break;
			case BeatSaberBody.DataOneofCase.ModifyRoomCfg:
				this.ModifyRoomCfg = other.ModifyRoomCfg.Clone();
				break;
			case BeatSaberBody.DataOneofCase.ModifySongCfg:
				this.ModifySongCfg = other.ModifySongCfg.Clone();
				break;
			case BeatSaberBody.DataOneofCase.RoomSubmitScore:
				this.RoomSubmitScore = other.RoomSubmitScore.Clone();
				break;
			case BeatSaberBody.DataOneofCase.FastMatch:
				this.FastMatch = other.FastMatch.Clone();
				break;
			case BeatSaberBody.DataOneofCase.AutoMatch:
				this.AutoMatch = other.AutoMatch.Clone();
				break;
			case BeatSaberBody.DataOneofCase.ModifyNickname:
				this.ModifyNickname = other.ModifyNickname.Clone();
				break;
			case BeatSaberBody.DataOneofCase.KickedOutNotice:
				this.KickedOutNotice = other.KickedOutNotice.Clone();
				break;
			case BeatSaberBody.DataOneofCase.RoomUpdatedNotice:
				this.RoomUpdatedNotice = other.RoomUpdatedNotice.Clone();
				break;
			case BeatSaberBody.DataOneofCase.KickedOutRoomNotice:
				this.KickedOutRoomNotice = other.KickedOutRoomNotice.Clone();
				break;
			case BeatSaberBody.DataOneofCase.StartGameNotice:
				this.StartGameNotice = other.StartGameNotice.Clone();
				break;
			case BeatSaberBody.DataOneofCase.RoomSubmitScoreNotice:
				this.RoomSubmitScoreNotice = other.RoomSubmitScoreNotice.Clone();
				break;
			case BeatSaberBody.DataOneofCase.RoomBroadcastNotice:
				this.RoomBroadcastNotice = other.RoomBroadcastNotice.Clone();
				break;
			case BeatSaberBody.DataOneofCase.AutoMatchNotice:
				this.AutoMatchNotice = other.AutoMatchNotice.Clone();
				break;
			case BeatSaberBody.DataOneofCase.HeadphoneOnNotice:
				this.HeadphoneOnNotice = other.HeadphoneOnNotice.Clone();
				break;
			}
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00003A65 File Offset: 0x00001C65
		[DebuggerNonUserCode]
		public BeatSaberBody Clone()
		{
			return new BeatSaberBody(this);
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00003A6D File Offset: 0x00001C6D
		// (set) Token: 0x06000019 RID: 25 RVA: 0x00003A75 File Offset: 0x00001C75
		[DebuggerNonUserCode]
		public DataType Type
		{
			get
			{
				return this.type_;
			}
			set
			{
				this.type_ = value;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001A RID: 26 RVA: 0x00003A7E File Offset: 0x00001C7E
		// (set) Token: 0x0600001B RID: 27 RVA: 0x00003A96 File Offset: 0x00001C96
		[DebuggerNonUserCode]
		public Ping Ping
		{
			get
			{
				if (this.dataCase_ != BeatSaberBody.DataOneofCase.Ping)
				{
					return null;
				}
				return (Ping)this.data_;
			}
			set
			{
				this.data_ = value;
				this.dataCase_ = ((value == null) ? BeatSaberBody.DataOneofCase.None : BeatSaberBody.DataOneofCase.Ping);
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00003AAC File Offset: 0x00001CAC
		// (set) Token: 0x0600001D RID: 29 RVA: 0x00003AC4 File Offset: 0x00001CC4
		[DebuggerNonUserCode]
		public Login Login
		{
			get
			{
				if (this.dataCase_ != BeatSaberBody.DataOneofCase.Login)
				{
					return null;
				}
				return (Login)this.data_;
			}
			set
			{
				this.data_ = value;
				this.dataCase_ = ((value == null) ? BeatSaberBody.DataOneofCase.None : BeatSaberBody.DataOneofCase.Login);
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600001E RID: 30 RVA: 0x00003ADA File Offset: 0x00001CDA
		// (set) Token: 0x0600001F RID: 31 RVA: 0x00003AF2 File Offset: 0x00001CF2
		[DebuggerNonUserCode]
		public Renew Renew
		{
			get
			{
				if (this.dataCase_ != BeatSaberBody.DataOneofCase.Renew)
				{
					return null;
				}
				return (Renew)this.data_;
			}
			set
			{
				this.data_ = value;
				this.dataCase_ = ((value == null) ? BeatSaberBody.DataOneofCase.None : BeatSaberBody.DataOneofCase.Renew);
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000020 RID: 32 RVA: 0x00003B08 File Offset: 0x00001D08
		// (set) Token: 0x06000021 RID: 33 RVA: 0x00003B20 File Offset: 0x00001D20
		[DebuggerNonUserCode]
		public Logout Logout
		{
			get
			{
				if (this.dataCase_ != BeatSaberBody.DataOneofCase.Logout)
				{
					return null;
				}
				return (Logout)this.data_;
			}
			set
			{
				this.data_ = value;
				this.dataCase_ = ((value == null) ? BeatSaberBody.DataOneofCase.None : BeatSaberBody.DataOneofCase.Logout);
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000022 RID: 34 RVA: 0x00003B36 File Offset: 0x00001D36
		// (set) Token: 0x06000023 RID: 35 RVA: 0x00003B4E File Offset: 0x00001D4E
		[DebuggerNonUserCode]
		public GetPlayer GetPlayer
		{
			get
			{
				if (this.dataCase_ != BeatSaberBody.DataOneofCase.GetPlayer)
				{
					return null;
				}
				return (GetPlayer)this.data_;
			}
			set
			{
				this.data_ = value;
				this.dataCase_ = ((value == null) ? BeatSaberBody.DataOneofCase.None : BeatSaberBody.DataOneofCase.GetPlayer);
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000024 RID: 36 RVA: 0x00003B64 File Offset: 0x00001D64
		// (set) Token: 0x06000025 RID: 37 RVA: 0x00003B7C File Offset: 0x00001D7C
		[DebuggerNonUserCode]
		public SongList SongList
		{
			get
			{
				if (this.dataCase_ != BeatSaberBody.DataOneofCase.SongList)
				{
					return null;
				}
				return (SongList)this.data_;
			}
			set
			{
				this.data_ = value;
				this.dataCase_ = ((value == null) ? BeatSaberBody.DataOneofCase.None : BeatSaberBody.DataOneofCase.SongList);
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000026 RID: 38 RVA: 0x00003B92 File Offset: 0x00001D92
		// (set) Token: 0x06000027 RID: 39 RVA: 0x00003BAA File Offset: 0x00001DAA
		[DebuggerNonUserCode]
		public RoomList RoomList
		{
			get
			{
				if (this.dataCase_ != BeatSaberBody.DataOneofCase.RoomList)
				{
					return null;
				}
				return (RoomList)this.data_;
			}
			set
			{
				this.data_ = value;
				this.dataCase_ = ((value == null) ? BeatSaberBody.DataOneofCase.None : BeatSaberBody.DataOneofCase.RoomList);
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00003BC0 File Offset: 0x00001DC0
		// (set) Token: 0x06000029 RID: 41 RVA: 0x00003BD9 File Offset: 0x00001DD9
		[DebuggerNonUserCode]
		public GetRoom GetRoom
		{
			get
			{
				if (this.dataCase_ != BeatSaberBody.DataOneofCase.GetRoom)
				{
					return null;
				}
				return (GetRoom)this.data_;
			}
			set
			{
				this.data_ = value;
				this.dataCase_ = ((value == null) ? BeatSaberBody.DataOneofCase.None : BeatSaberBody.DataOneofCase.GetRoom);
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00003BF0 File Offset: 0x00001DF0
		// (set) Token: 0x0600002B RID: 43 RVA: 0x00003C09 File Offset: 0x00001E09
		[DebuggerNonUserCode]
		public CreateRoom CreateRoom
		{
			get
			{
				if (this.dataCase_ != BeatSaberBody.DataOneofCase.CreateRoom)
				{
					return null;
				}
				return (CreateRoom)this.data_;
			}
			set
			{
				this.data_ = value;
				this.dataCase_ = ((value == null) ? BeatSaberBody.DataOneofCase.None : BeatSaberBody.DataOneofCase.CreateRoom);
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00003C20 File Offset: 0x00001E20
		// (set) Token: 0x0600002D RID: 45 RVA: 0x00003C39 File Offset: 0x00001E39
		[DebuggerNonUserCode]
		public JoinRoom JoinRoom
		{
			get
			{
				if (this.dataCase_ != BeatSaberBody.DataOneofCase.JoinRoom)
				{
					return null;
				}
				return (JoinRoom)this.data_;
			}
			set
			{
				this.data_ = value;
				this.dataCase_ = ((value == null) ? BeatSaberBody.DataOneofCase.None : BeatSaberBody.DataOneofCase.JoinRoom);
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00003C50 File Offset: 0x00001E50
		// (set) Token: 0x0600002F RID: 47 RVA: 0x00003C69 File Offset: 0x00001E69
		[DebuggerNonUserCode]
		public ExitRoom ExitRoom
		{
			get
			{
				if (this.dataCase_ != BeatSaberBody.DataOneofCase.ExitRoom)
				{
					return null;
				}
				return (ExitRoom)this.data_;
			}
			set
			{
				this.data_ = value;
				this.dataCase_ = ((value == null) ? BeatSaberBody.DataOneofCase.None : BeatSaberBody.DataOneofCase.ExitRoom);
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00003C80 File Offset: 0x00001E80
		// (set) Token: 0x06000031 RID: 49 RVA: 0x00003C99 File Offset: 0x00001E99
		[DebuggerNonUserCode]
		public KickOutRoomPlayer KickOutRoomPlayer
		{
			get
			{
				if (this.dataCase_ != BeatSaberBody.DataOneofCase.KickOutRoomPlayer)
				{
					return null;
				}
				return (KickOutRoomPlayer)this.data_;
			}
			set
			{
				this.data_ = value;
				this.dataCase_ = ((value == null) ? BeatSaberBody.DataOneofCase.None : BeatSaberBody.DataOneofCase.KickOutRoomPlayer);
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00003CB0 File Offset: 0x00001EB0
		// (set) Token: 0x06000033 RID: 51 RVA: 0x00003CC9 File Offset: 0x00001EC9
		[DebuggerNonUserCode]
		public StartGame StartGame
		{
			get
			{
				if (this.dataCase_ != BeatSaberBody.DataOneofCase.StartGame)
				{
					return null;
				}
				return (StartGame)this.data_;
			}
			set
			{
				this.data_ = value;
				this.dataCase_ = ((value == null) ? BeatSaberBody.DataOneofCase.None : BeatSaberBody.DataOneofCase.StartGame);
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00003CE0 File Offset: 0x00001EE0
		// (set) Token: 0x06000035 RID: 53 RVA: 0x00003CF9 File Offset: 0x00001EF9
		[DebuggerNonUserCode]
		public RoomBroadcast RoomBroadcast
		{
			get
			{
				if (this.dataCase_ != BeatSaberBody.DataOneofCase.RoomBroadcast)
				{
					return null;
				}
				return (RoomBroadcast)this.data_;
			}
			set
			{
				this.data_ = value;
				this.dataCase_ = ((value == null) ? BeatSaberBody.DataOneofCase.None : BeatSaberBody.DataOneofCase.RoomBroadcast);
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00003D10 File Offset: 0x00001F10
		// (set) Token: 0x06000037 RID: 55 RVA: 0x00003D29 File Offset: 0x00001F29
		[DebuggerNonUserCode]
		public ChangeRoomOwner ChangeRoomOwner
		{
			get
			{
				if (this.dataCase_ != BeatSaberBody.DataOneofCase.ChangeRoomOwner)
				{
					return null;
				}
				return (ChangeRoomOwner)this.data_;
			}
			set
			{
				this.data_ = value;
				this.dataCase_ = ((value == null) ? BeatSaberBody.DataOneofCase.None : BeatSaberBody.DataOneofCase.ChangeRoomOwner);
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00003D40 File Offset: 0x00001F40
		// (set) Token: 0x06000039 RID: 57 RVA: 0x00003D59 File Offset: 0x00001F59
		[DebuggerNonUserCode]
		public ModifyPersonalCfg ModifyPersonalCfg
		{
			get
			{
				if (this.dataCase_ != BeatSaberBody.DataOneofCase.ModifyPersonalCfg)
				{
					return null;
				}
				return (ModifyPersonalCfg)this.data_;
			}
			set
			{
				this.data_ = value;
				this.dataCase_ = ((value == null) ? BeatSaberBody.DataOneofCase.None : BeatSaberBody.DataOneofCase.ModifyPersonalCfg);
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00003D70 File Offset: 0x00001F70
		// (set) Token: 0x0600003B RID: 59 RVA: 0x00003D89 File Offset: 0x00001F89
		[DebuggerNonUserCode]
		public ModifyRoomCfg ModifyRoomCfg
		{
			get
			{
				if (this.dataCase_ != BeatSaberBody.DataOneofCase.ModifyRoomCfg)
				{
					return null;
				}
				return (ModifyRoomCfg)this.data_;
			}
			set
			{
				this.data_ = value;
				this.dataCase_ = ((value == null) ? BeatSaberBody.DataOneofCase.None : BeatSaberBody.DataOneofCase.ModifyRoomCfg);
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00003DA0 File Offset: 0x00001FA0
		// (set) Token: 0x0600003D RID: 61 RVA: 0x00003DB9 File Offset: 0x00001FB9
		[DebuggerNonUserCode]
		public ModifySongCfg ModifySongCfg
		{
			get
			{
				if (this.dataCase_ != BeatSaberBody.DataOneofCase.ModifySongCfg)
				{
					return null;
				}
				return (ModifySongCfg)this.data_;
			}
			set
			{
				this.data_ = value;
				this.dataCase_ = ((value == null) ? BeatSaberBody.DataOneofCase.None : BeatSaberBody.DataOneofCase.ModifySongCfg);
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00003DD0 File Offset: 0x00001FD0
		// (set) Token: 0x0600003F RID: 63 RVA: 0x00003DE9 File Offset: 0x00001FE9
		[DebuggerNonUserCode]
		public RoomSubmitScore RoomSubmitScore
		{
			get
			{
				if (this.dataCase_ != BeatSaberBody.DataOneofCase.RoomSubmitScore)
				{
					return null;
				}
				return (RoomSubmitScore)this.data_;
			}
			set
			{
				this.data_ = value;
				this.dataCase_ = ((value == null) ? BeatSaberBody.DataOneofCase.None : BeatSaberBody.DataOneofCase.RoomSubmitScore);
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00003E00 File Offset: 0x00002000
		// (set) Token: 0x06000041 RID: 65 RVA: 0x00003E19 File Offset: 0x00002019
		[DebuggerNonUserCode]
		public FastMatch FastMatch
		{
			get
			{
				if (this.dataCase_ != BeatSaberBody.DataOneofCase.FastMatch)
				{
					return null;
				}
				return (FastMatch)this.data_;
			}
			set
			{
				this.data_ = value;
				this.dataCase_ = ((value == null) ? BeatSaberBody.DataOneofCase.None : BeatSaberBody.DataOneofCase.FastMatch);
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00003E30 File Offset: 0x00002030
		// (set) Token: 0x06000043 RID: 67 RVA: 0x00003E49 File Offset: 0x00002049
		[DebuggerNonUserCode]
		public AutoMatch AutoMatch
		{
			get
			{
				if (this.dataCase_ != BeatSaberBody.DataOneofCase.AutoMatch)
				{
					return null;
				}
				return (AutoMatch)this.data_;
			}
			set
			{
				this.data_ = value;
				this.dataCase_ = ((value == null) ? BeatSaberBody.DataOneofCase.None : BeatSaberBody.DataOneofCase.AutoMatch);
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00003E60 File Offset: 0x00002060
		// (set) Token: 0x06000045 RID: 69 RVA: 0x00003E79 File Offset: 0x00002079
		[DebuggerNonUserCode]
		public ModifyNickname ModifyNickname
		{
			get
			{
				if (this.dataCase_ != BeatSaberBody.DataOneofCase.ModifyNickname)
				{
					return null;
				}
				return (ModifyNickname)this.data_;
			}
			set
			{
				this.data_ = value;
				this.dataCase_ = ((value == null) ? BeatSaberBody.DataOneofCase.None : BeatSaberBody.DataOneofCase.ModifyNickname);
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00003E90 File Offset: 0x00002090
		// (set) Token: 0x06000047 RID: 71 RVA: 0x00003EA9 File Offset: 0x000020A9
		[DebuggerNonUserCode]
		public KickedOutNotice KickedOutNotice
		{
			get
			{
				if (this.dataCase_ != BeatSaberBody.DataOneofCase.KickedOutNotice)
				{
					return null;
				}
				return (KickedOutNotice)this.data_;
			}
			set
			{
				this.data_ = value;
				this.dataCase_ = ((value == null) ? BeatSaberBody.DataOneofCase.None : BeatSaberBody.DataOneofCase.KickedOutNotice);
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000048 RID: 72 RVA: 0x00003EC0 File Offset: 0x000020C0
		// (set) Token: 0x06000049 RID: 73 RVA: 0x00003ED9 File Offset: 0x000020D9
		[DebuggerNonUserCode]
		public RoomUpdatedNotice RoomUpdatedNotice
		{
			get
			{
				if (this.dataCase_ != BeatSaberBody.DataOneofCase.RoomUpdatedNotice)
				{
					return null;
				}
				return (RoomUpdatedNotice)this.data_;
			}
			set
			{
				this.data_ = value;
				this.dataCase_ = ((value == null) ? BeatSaberBody.DataOneofCase.None : BeatSaberBody.DataOneofCase.RoomUpdatedNotice);
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00003EF0 File Offset: 0x000020F0
		// (set) Token: 0x0600004B RID: 75 RVA: 0x00003F09 File Offset: 0x00002109
		[DebuggerNonUserCode]
		public KickedOutRoomNotice KickedOutRoomNotice
		{
			get
			{
				if (this.dataCase_ != BeatSaberBody.DataOneofCase.KickedOutRoomNotice)
				{
					return null;
				}
				return (KickedOutRoomNotice)this.data_;
			}
			set
			{
				this.data_ = value;
				this.dataCase_ = ((value == null) ? BeatSaberBody.DataOneofCase.None : BeatSaberBody.DataOneofCase.KickedOutRoomNotice);
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600004C RID: 76 RVA: 0x00003F20 File Offset: 0x00002120
		// (set) Token: 0x0600004D RID: 77 RVA: 0x00003F39 File Offset: 0x00002139
		[DebuggerNonUserCode]
		public StartGameNotice StartGameNotice
		{
			get
			{
				if (this.dataCase_ != BeatSaberBody.DataOneofCase.StartGameNotice)
				{
					return null;
				}
				return (StartGameNotice)this.data_;
			}
			set
			{
				this.data_ = value;
				this.dataCase_ = ((value == null) ? BeatSaberBody.DataOneofCase.None : BeatSaberBody.DataOneofCase.StartGameNotice);
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00003F50 File Offset: 0x00002150
		// (set) Token: 0x0600004F RID: 79 RVA: 0x00003F69 File Offset: 0x00002169
		[DebuggerNonUserCode]
		public RoomSubmitScoreNotice RoomSubmitScoreNotice
		{
			get
			{
				if (this.dataCase_ != BeatSaberBody.DataOneofCase.RoomSubmitScoreNotice)
				{
					return null;
				}
				return (RoomSubmitScoreNotice)this.data_;
			}
			set
			{
				this.data_ = value;
				this.dataCase_ = ((value == null) ? BeatSaberBody.DataOneofCase.None : BeatSaberBody.DataOneofCase.RoomSubmitScoreNotice);
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00003F80 File Offset: 0x00002180
		// (set) Token: 0x06000051 RID: 81 RVA: 0x00003F99 File Offset: 0x00002199
		[DebuggerNonUserCode]
		public RoomBroadcastNotice RoomBroadcastNotice
		{
			get
			{
				if (this.dataCase_ != BeatSaberBody.DataOneofCase.RoomBroadcastNotice)
				{
					return null;
				}
				return (RoomBroadcastNotice)this.data_;
			}
			set
			{
				this.data_ = value;
				this.dataCase_ = ((value == null) ? BeatSaberBody.DataOneofCase.None : BeatSaberBody.DataOneofCase.RoomBroadcastNotice);
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00003FB0 File Offset: 0x000021B0
		// (set) Token: 0x06000053 RID: 83 RVA: 0x00003FC9 File Offset: 0x000021C9
		[DebuggerNonUserCode]
		public AutoMatchNotice AutoMatchNotice
		{
			get
			{
				if (this.dataCase_ != BeatSaberBody.DataOneofCase.AutoMatchNotice)
				{
					return null;
				}
				return (AutoMatchNotice)this.data_;
			}
			set
			{
				this.data_ = value;
				this.dataCase_ = ((value == null) ? BeatSaberBody.DataOneofCase.None : BeatSaberBody.DataOneofCase.AutoMatchNotice);
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000054 RID: 84 RVA: 0x00003FE0 File Offset: 0x000021E0
		// (set) Token: 0x06000055 RID: 85 RVA: 0x00003FF9 File Offset: 0x000021F9
		[DebuggerNonUserCode]
		public HeadphoneOnNotice HeadphoneOnNotice
		{
			get
			{
				if (this.dataCase_ != BeatSaberBody.DataOneofCase.HeadphoneOnNotice)
				{
					return null;
				}
				return (HeadphoneOnNotice)this.data_;
			}
			set
			{
				this.data_ = value;
				this.dataCase_ = ((value == null) ? BeatSaberBody.DataOneofCase.None : BeatSaberBody.DataOneofCase.HeadphoneOnNotice);
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000056 RID: 86 RVA: 0x00004010 File Offset: 0x00002210
		[DebuggerNonUserCode]
		public BeatSaberBody.DataOneofCase DataCase
		{
			get
			{
				return this.dataCase_;
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00004018 File Offset: 0x00002218
		[DebuggerNonUserCode]
		public void ClearData()
		{
			this.dataCase_ = BeatSaberBody.DataOneofCase.None;
			this.data_ = null;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00004028 File Offset: 0x00002228
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as BeatSaberBody);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00004038 File Offset: 0x00002238
		[DebuggerNonUserCode]
		public bool Equals(BeatSaberBody other)
		{
			return other != null && (other == this || (this.Type == other.Type && object.Equals(this.Ping, other.Ping) && object.Equals(this.Login, other.Login) && object.Equals(this.Renew, other.Renew) && object.Equals(this.Logout, other.Logout) && object.Equals(this.GetPlayer, other.GetPlayer) && object.Equals(this.SongList, other.SongList) && object.Equals(this.RoomList, other.RoomList) && object.Equals(this.GetRoom, other.GetRoom) && object.Equals(this.CreateRoom, other.CreateRoom) && object.Equals(this.JoinRoom, other.JoinRoom) && object.Equals(this.ExitRoom, other.ExitRoom) && object.Equals(this.KickOutRoomPlayer, other.KickOutRoomPlayer) && object.Equals(this.StartGame, other.StartGame) && object.Equals(this.RoomBroadcast, other.RoomBroadcast) && object.Equals(this.ChangeRoomOwner, other.ChangeRoomOwner) && object.Equals(this.ModifyPersonalCfg, other.ModifyPersonalCfg) && object.Equals(this.ModifyRoomCfg, other.ModifyRoomCfg) && object.Equals(this.ModifySongCfg, other.ModifySongCfg) && object.Equals(this.RoomSubmitScore, other.RoomSubmitScore) && object.Equals(this.FastMatch, other.FastMatch) && object.Equals(this.AutoMatch, other.AutoMatch) && object.Equals(this.ModifyNickname, other.ModifyNickname) && object.Equals(this.KickedOutNotice, other.KickedOutNotice) && object.Equals(this.RoomUpdatedNotice, other.RoomUpdatedNotice) && object.Equals(this.KickedOutRoomNotice, other.KickedOutRoomNotice) && object.Equals(this.StartGameNotice, other.StartGameNotice) && object.Equals(this.RoomSubmitScoreNotice, other.RoomSubmitScoreNotice) && object.Equals(this.RoomBroadcastNotice, other.RoomBroadcastNotice) && object.Equals(this.AutoMatchNotice, other.AutoMatchNotice) && object.Equals(this.HeadphoneOnNotice, other.HeadphoneOnNotice) && this.DataCase == other.DataCase && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x0600005A RID: 90 RVA: 0x000042F8 File Offset: 0x000024F8
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.Type != DataType.Ping)
			{
				num ^= this.Type.GetHashCode();
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.Ping)
			{
				num ^= this.Ping.GetHashCode();
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.Login)
			{
				num ^= this.Login.GetHashCode();
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.Renew)
			{
				num ^= this.Renew.GetHashCode();
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.Logout)
			{
				num ^= this.Logout.GetHashCode();
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.GetPlayer)
			{
				num ^= this.GetPlayer.GetHashCode();
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.SongList)
			{
				num ^= this.SongList.GetHashCode();
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.RoomList)
			{
				num ^= this.RoomList.GetHashCode();
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.GetRoom)
			{
				num ^= this.GetRoom.GetHashCode();
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.CreateRoom)
			{
				num ^= this.CreateRoom.GetHashCode();
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.JoinRoom)
			{
				num ^= this.JoinRoom.GetHashCode();
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.ExitRoom)
			{
				num ^= this.ExitRoom.GetHashCode();
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.KickOutRoomPlayer)
			{
				num ^= this.KickOutRoomPlayer.GetHashCode();
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.StartGame)
			{
				num ^= this.StartGame.GetHashCode();
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.RoomBroadcast)
			{
				num ^= this.RoomBroadcast.GetHashCode();
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.ChangeRoomOwner)
			{
				num ^= this.ChangeRoomOwner.GetHashCode();
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.ModifyPersonalCfg)
			{
				num ^= this.ModifyPersonalCfg.GetHashCode();
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.ModifyRoomCfg)
			{
				num ^= this.ModifyRoomCfg.GetHashCode();
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.ModifySongCfg)
			{
				num ^= this.ModifySongCfg.GetHashCode();
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.RoomSubmitScore)
			{
				num ^= this.RoomSubmitScore.GetHashCode();
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.FastMatch)
			{
				num ^= this.FastMatch.GetHashCode();
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.AutoMatch)
			{
				num ^= this.AutoMatch.GetHashCode();
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.ModifyNickname)
			{
				num ^= this.ModifyNickname.GetHashCode();
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.KickedOutNotice)
			{
				num ^= this.KickedOutNotice.GetHashCode();
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.RoomUpdatedNotice)
			{
				num ^= this.RoomUpdatedNotice.GetHashCode();
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.KickedOutRoomNotice)
			{
				num ^= this.KickedOutRoomNotice.GetHashCode();
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.StartGameNotice)
			{
				num ^= this.StartGameNotice.GetHashCode();
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.RoomSubmitScoreNotice)
			{
				num ^= this.RoomSubmitScoreNotice.GetHashCode();
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.RoomBroadcastNotice)
			{
				num ^= this.RoomBroadcastNotice.GetHashCode();
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.AutoMatchNotice)
			{
				num ^= this.AutoMatchNotice.GetHashCode();
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.HeadphoneOnNotice)
			{
				num ^= this.HeadphoneOnNotice.GetHashCode();
			}
			num ^= (int)this.dataCase_;
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00004618 File Offset: 0x00002818
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Type != DataType.Ping)
			{
				output.WriteRawTag(8);
				output.WriteEnum((int)this.Type);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.Ping)
			{
				output.WriteRawTag(18);
				output.WriteMessage(this.Ping);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.Login)
			{
				output.WriteRawTag(26);
				output.WriteMessage(this.Login);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.Renew)
			{
				output.WriteRawTag(34);
				output.WriteMessage(this.Renew);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.Logout)
			{
				output.WriteRawTag(42);
				output.WriteMessage(this.Logout);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.GetPlayer)
			{
				output.WriteRawTag(50);
				output.WriteMessage(this.GetPlayer);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.SongList)
			{
				output.WriteRawTag(58);
				output.WriteMessage(this.SongList);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.RoomList)
			{
				output.WriteRawTag(66);
				output.WriteMessage(this.RoomList);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.GetRoom)
			{
				output.WriteRawTag(74);
				output.WriteMessage(this.GetRoom);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.CreateRoom)
			{
				output.WriteRawTag(82);
				output.WriteMessage(this.CreateRoom);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.JoinRoom)
			{
				output.WriteRawTag(90);
				output.WriteMessage(this.JoinRoom);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.ExitRoom)
			{
				output.WriteRawTag(98);
				output.WriteMessage(this.ExitRoom);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.KickOutRoomPlayer)
			{
				output.WriteRawTag(106);
				output.WriteMessage(this.KickOutRoomPlayer);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.StartGame)
			{
				output.WriteRawTag(114);
				output.WriteMessage(this.StartGame);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.RoomBroadcast)
			{
				output.WriteRawTag(122);
				output.WriteMessage(this.RoomBroadcast);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.ChangeRoomOwner)
			{
				output.WriteRawTag(130, 1);
				output.WriteMessage(this.ChangeRoomOwner);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.ModifyPersonalCfg)
			{
				output.WriteRawTag(138, 1);
				output.WriteMessage(this.ModifyPersonalCfg);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.ModifyRoomCfg)
			{
				output.WriteRawTag(146, 1);
				output.WriteMessage(this.ModifyRoomCfg);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.ModifySongCfg)
			{
				output.WriteRawTag(154, 1);
				output.WriteMessage(this.ModifySongCfg);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.RoomSubmitScore)
			{
				output.WriteRawTag(162, 1);
				output.WriteMessage(this.RoomSubmitScore);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.FastMatch)
			{
				output.WriteRawTag(170, 1);
				output.WriteMessage(this.FastMatch);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.AutoMatch)
			{
				output.WriteRawTag(178, 1);
				output.WriteMessage(this.AutoMatch);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.ModifyNickname)
			{
				output.WriteRawTag(194, 1);
				output.WriteMessage(this.ModifyNickname);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.KickedOutNotice)
			{
				output.WriteRawTag(242, 1);
				output.WriteMessage(this.KickedOutNotice);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.RoomUpdatedNotice)
			{
				output.WriteRawTag(250, 1);
				output.WriteMessage(this.RoomUpdatedNotice);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.KickedOutRoomNotice)
			{
				output.WriteRawTag(130, 2);
				output.WriteMessage(this.KickedOutRoomNotice);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.StartGameNotice)
			{
				output.WriteRawTag(138, 2);
				output.WriteMessage(this.StartGameNotice);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.RoomSubmitScoreNotice)
			{
				output.WriteRawTag(146, 2);
				output.WriteMessage(this.RoomSubmitScoreNotice);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.RoomBroadcastNotice)
			{
				output.WriteRawTag(154, 2);
				output.WriteMessage(this.RoomBroadcastNotice);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.AutoMatchNotice)
			{
				output.WriteRawTag(162, 2);
				output.WriteMessage(this.AutoMatchNotice);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.HeadphoneOnNotice)
			{
				output.WriteRawTag(170, 2);
				output.WriteMessage(this.HeadphoneOnNotice);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00004A14 File Offset: 0x00002C14
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Type != DataType.Ping)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)this.Type);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.Ping)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.Ping);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.Login)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.Login);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.Renew)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.Renew);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.Logout)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.Logout);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.GetPlayer)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.GetPlayer);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.SongList)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.SongList);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.RoomList)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.RoomList);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.GetRoom)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.GetRoom);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.CreateRoom)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.CreateRoom);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.JoinRoom)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.JoinRoom);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.ExitRoom)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.ExitRoom);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.KickOutRoomPlayer)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.KickOutRoomPlayer);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.StartGame)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.StartGame);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.RoomBroadcast)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.RoomBroadcast);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.ChangeRoomOwner)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.ChangeRoomOwner);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.ModifyPersonalCfg)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.ModifyPersonalCfg);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.ModifyRoomCfg)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.ModifyRoomCfg);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.ModifySongCfg)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.ModifySongCfg);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.RoomSubmitScore)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.RoomSubmitScore);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.FastMatch)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.FastMatch);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.AutoMatch)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.AutoMatch);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.ModifyNickname)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.ModifyNickname);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.KickedOutNotice)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.KickedOutNotice);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.RoomUpdatedNotice)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.RoomUpdatedNotice);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.KickedOutRoomNotice)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.KickedOutRoomNotice);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.StartGameNotice)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.StartGameNotice);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.RoomSubmitScoreNotice)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.RoomSubmitScoreNotice);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.RoomBroadcastNotice)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.RoomBroadcastNotice);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.AutoMatchNotice)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.AutoMatchNotice);
			}
			if (this.dataCase_ == BeatSaberBody.DataOneofCase.HeadphoneOnNotice)
			{
				num += 2 + CodedOutputStream.ComputeMessageSize(this.HeadphoneOnNotice);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00004D58 File Offset: 0x00002F58
		[DebuggerNonUserCode]
		public void MergeFrom(BeatSaberBody other)
		{
			if (other == null)
			{
				return;
			}
			if (other.Type != DataType.Ping)
			{
				this.Type = other.Type;
			}
			switch (other.DataCase)
			{
			case BeatSaberBody.DataOneofCase.Ping:
				if (this.Ping == null)
				{
					this.Ping = new Ping();
				}
				this.Ping.MergeFrom(other.Ping);
				break;
			case BeatSaberBody.DataOneofCase.Login:
				if (this.Login == null)
				{
					this.Login = new Login();
				}
				this.Login.MergeFrom(other.Login);
				break;
			case BeatSaberBody.DataOneofCase.Renew:
				if (this.Renew == null)
				{
					this.Renew = new Renew();
				}
				this.Renew.MergeFrom(other.Renew);
				break;
			case BeatSaberBody.DataOneofCase.Logout:
				if (this.Logout == null)
				{
					this.Logout = new Logout();
				}
				this.Logout.MergeFrom(other.Logout);
				break;
			case BeatSaberBody.DataOneofCase.GetPlayer:
				if (this.GetPlayer == null)
				{
					this.GetPlayer = new GetPlayer();
				}
				this.GetPlayer.MergeFrom(other.GetPlayer);
				break;
			case BeatSaberBody.DataOneofCase.SongList:
				if (this.SongList == null)
				{
					this.SongList = new SongList();
				}
				this.SongList.MergeFrom(other.SongList);
				break;
			case BeatSaberBody.DataOneofCase.RoomList:
				if (this.RoomList == null)
				{
					this.RoomList = new RoomList();
				}
				this.RoomList.MergeFrom(other.RoomList);
				break;
			case BeatSaberBody.DataOneofCase.GetRoom:
				if (this.GetRoom == null)
				{
					this.GetRoom = new GetRoom();
				}
				this.GetRoom.MergeFrom(other.GetRoom);
				break;
			case BeatSaberBody.DataOneofCase.CreateRoom:
				if (this.CreateRoom == null)
				{
					this.CreateRoom = new CreateRoom();
				}
				this.CreateRoom.MergeFrom(other.CreateRoom);
				break;
			case BeatSaberBody.DataOneofCase.JoinRoom:
				if (this.JoinRoom == null)
				{
					this.JoinRoom = new JoinRoom();
				}
				this.JoinRoom.MergeFrom(other.JoinRoom);
				break;
			case BeatSaberBody.DataOneofCase.ExitRoom:
				if (this.ExitRoom == null)
				{
					this.ExitRoom = new ExitRoom();
				}
				this.ExitRoom.MergeFrom(other.ExitRoom);
				break;
			case BeatSaberBody.DataOneofCase.KickOutRoomPlayer:
				if (this.KickOutRoomPlayer == null)
				{
					this.KickOutRoomPlayer = new KickOutRoomPlayer();
				}
				this.KickOutRoomPlayer.MergeFrom(other.KickOutRoomPlayer);
				break;
			case BeatSaberBody.DataOneofCase.StartGame:
				if (this.StartGame == null)
				{
					this.StartGame = new StartGame();
				}
				this.StartGame.MergeFrom(other.StartGame);
				break;
			case BeatSaberBody.DataOneofCase.RoomBroadcast:
				if (this.RoomBroadcast == null)
				{
					this.RoomBroadcast = new RoomBroadcast();
				}
				this.RoomBroadcast.MergeFrom(other.RoomBroadcast);
				break;
			case BeatSaberBody.DataOneofCase.ChangeRoomOwner:
				if (this.ChangeRoomOwner == null)
				{
					this.ChangeRoomOwner = new ChangeRoomOwner();
				}
				this.ChangeRoomOwner.MergeFrom(other.ChangeRoomOwner);
				break;
			case BeatSaberBody.DataOneofCase.ModifyPersonalCfg:
				if (this.ModifyPersonalCfg == null)
				{
					this.ModifyPersonalCfg = new ModifyPersonalCfg();
				}
				this.ModifyPersonalCfg.MergeFrom(other.ModifyPersonalCfg);
				break;
			case BeatSaberBody.DataOneofCase.ModifyRoomCfg:
				if (this.ModifyRoomCfg == null)
				{
					this.ModifyRoomCfg = new ModifyRoomCfg();
				}
				this.ModifyRoomCfg.MergeFrom(other.ModifyRoomCfg);
				break;
			case BeatSaberBody.DataOneofCase.ModifySongCfg:
				if (this.ModifySongCfg == null)
				{
					this.ModifySongCfg = new ModifySongCfg();
				}
				this.ModifySongCfg.MergeFrom(other.ModifySongCfg);
				break;
			case BeatSaberBody.DataOneofCase.RoomSubmitScore:
				if (this.RoomSubmitScore == null)
				{
					this.RoomSubmitScore = new RoomSubmitScore();
				}
				this.RoomSubmitScore.MergeFrom(other.RoomSubmitScore);
				break;
			case BeatSaberBody.DataOneofCase.FastMatch:
				if (this.FastMatch == null)
				{
					this.FastMatch = new FastMatch();
				}
				this.FastMatch.MergeFrom(other.FastMatch);
				break;
			case BeatSaberBody.DataOneofCase.AutoMatch:
				if (this.AutoMatch == null)
				{
					this.AutoMatch = new AutoMatch();
				}
				this.AutoMatch.MergeFrom(other.AutoMatch);
				break;
			case BeatSaberBody.DataOneofCase.ModifyNickname:
				if (this.ModifyNickname == null)
				{
					this.ModifyNickname = new ModifyNickname();
				}
				this.ModifyNickname.MergeFrom(other.ModifyNickname);
				break;
			case BeatSaberBody.DataOneofCase.KickedOutNotice:
				if (this.KickedOutNotice == null)
				{
					this.KickedOutNotice = new KickedOutNotice();
				}
				this.KickedOutNotice.MergeFrom(other.KickedOutNotice);
				break;
			case BeatSaberBody.DataOneofCase.RoomUpdatedNotice:
				if (this.RoomUpdatedNotice == null)
				{
					this.RoomUpdatedNotice = new RoomUpdatedNotice();
				}
				this.RoomUpdatedNotice.MergeFrom(other.RoomUpdatedNotice);
				break;
			case BeatSaberBody.DataOneofCase.KickedOutRoomNotice:
				if (this.KickedOutRoomNotice == null)
				{
					this.KickedOutRoomNotice = new KickedOutRoomNotice();
				}
				this.KickedOutRoomNotice.MergeFrom(other.KickedOutRoomNotice);
				break;
			case BeatSaberBody.DataOneofCase.StartGameNotice:
				if (this.StartGameNotice == null)
				{
					this.StartGameNotice = new StartGameNotice();
				}
				this.StartGameNotice.MergeFrom(other.StartGameNotice);
				break;
			case BeatSaberBody.DataOneofCase.RoomSubmitScoreNotice:
				if (this.RoomSubmitScoreNotice == null)
				{
					this.RoomSubmitScoreNotice = new RoomSubmitScoreNotice();
				}
				this.RoomSubmitScoreNotice.MergeFrom(other.RoomSubmitScoreNotice);
				break;
			case BeatSaberBody.DataOneofCase.RoomBroadcastNotice:
				if (this.RoomBroadcastNotice == null)
				{
					this.RoomBroadcastNotice = new RoomBroadcastNotice();
				}
				this.RoomBroadcastNotice.MergeFrom(other.RoomBroadcastNotice);
				break;
			case BeatSaberBody.DataOneofCase.AutoMatchNotice:
				if (this.AutoMatchNotice == null)
				{
					this.AutoMatchNotice = new AutoMatchNotice();
				}
				this.AutoMatchNotice.MergeFrom(other.AutoMatchNotice);
				break;
			case BeatSaberBody.DataOneofCase.HeadphoneOnNotice:
				if (this.HeadphoneOnNotice == null)
				{
					this.HeadphoneOnNotice = new HeadphoneOnNotice();
				}
				this.HeadphoneOnNotice.MergeFrom(other.HeadphoneOnNotice);
				break;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000052F8 File Offset: 0x000034F8
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 122U)
				{
					if (num <= 58U)
					{
						if (num <= 26U)
						{
							if (num == 8U)
							{
								this.Type = (DataType)input.ReadEnum();
								continue;
							}
							if (num == 18U)
							{
								Ping ping = new Ping();
								if (this.dataCase_ == BeatSaberBody.DataOneofCase.Ping)
								{
									ping.MergeFrom(this.Ping);
								}
								input.ReadMessage(ping);
								this.Ping = ping;
								continue;
							}
							if (num == 26U)
							{
								Login login = new Login();
								if (this.dataCase_ == BeatSaberBody.DataOneofCase.Login)
								{
									login.MergeFrom(this.Login);
								}
								input.ReadMessage(login);
								this.Login = login;
								continue;
							}
						}
						else if (num <= 42U)
						{
							if (num == 34U)
							{
								Renew renew = new Renew();
								if (this.dataCase_ == BeatSaberBody.DataOneofCase.Renew)
								{
									renew.MergeFrom(this.Renew);
								}
								input.ReadMessage(renew);
								this.Renew = renew;
								continue;
							}
							if (num == 42U)
							{
								Logout logout = new Logout();
								if (this.dataCase_ == BeatSaberBody.DataOneofCase.Logout)
								{
									logout.MergeFrom(this.Logout);
								}
								input.ReadMessage(logout);
								this.Logout = logout;
								continue;
							}
						}
						else
						{
							if (num == 50U)
							{
								GetPlayer getPlayer = new GetPlayer();
								if (this.dataCase_ == BeatSaberBody.DataOneofCase.GetPlayer)
								{
									getPlayer.MergeFrom(this.GetPlayer);
								}
								input.ReadMessage(getPlayer);
								this.GetPlayer = getPlayer;
								continue;
							}
							if (num == 58U)
							{
								SongList songList = new SongList();
								if (this.dataCase_ == BeatSaberBody.DataOneofCase.SongList)
								{
									songList.MergeFrom(this.SongList);
								}
								input.ReadMessage(songList);
								this.SongList = songList;
								continue;
							}
						}
					}
					else if (num <= 90U)
					{
						if (num <= 74U)
						{
							if (num == 66U)
							{
								RoomList roomList = new RoomList();
								if (this.dataCase_ == BeatSaberBody.DataOneofCase.RoomList)
								{
									roomList.MergeFrom(this.RoomList);
								}
								input.ReadMessage(roomList);
								this.RoomList = roomList;
								continue;
							}
							if (num == 74U)
							{
								GetRoom getRoom = new GetRoom();
								if (this.dataCase_ == BeatSaberBody.DataOneofCase.GetRoom)
								{
									getRoom.MergeFrom(this.GetRoom);
								}
								input.ReadMessage(getRoom);
								this.GetRoom = getRoom;
								continue;
							}
						}
						else
						{
							if (num == 82U)
							{
								CreateRoom createRoom = new CreateRoom();
								if (this.dataCase_ == BeatSaberBody.DataOneofCase.CreateRoom)
								{
									createRoom.MergeFrom(this.CreateRoom);
								}
								input.ReadMessage(createRoom);
								this.CreateRoom = createRoom;
								continue;
							}
							if (num == 90U)
							{
								JoinRoom joinRoom = new JoinRoom();
								if (this.dataCase_ == BeatSaberBody.DataOneofCase.JoinRoom)
								{
									joinRoom.MergeFrom(this.JoinRoom);
								}
								input.ReadMessage(joinRoom);
								this.JoinRoom = joinRoom;
								continue;
							}
						}
					}
					else if (num <= 106U)
					{
						if (num == 98U)
						{
							ExitRoom exitRoom = new ExitRoom();
							if (this.dataCase_ == BeatSaberBody.DataOneofCase.ExitRoom)
							{
								exitRoom.MergeFrom(this.ExitRoom);
							}
							input.ReadMessage(exitRoom);
							this.ExitRoom = exitRoom;
							continue;
						}
						if (num == 106U)
						{
							KickOutRoomPlayer kickOutRoomPlayer = new KickOutRoomPlayer();
							if (this.dataCase_ == BeatSaberBody.DataOneofCase.KickOutRoomPlayer)
							{
								kickOutRoomPlayer.MergeFrom(this.KickOutRoomPlayer);
							}
							input.ReadMessage(kickOutRoomPlayer);
							this.KickOutRoomPlayer = kickOutRoomPlayer;
							continue;
						}
					}
					else
					{
						if (num == 114U)
						{
							StartGame startGame = new StartGame();
							if (this.dataCase_ == BeatSaberBody.DataOneofCase.StartGame)
							{
								startGame.MergeFrom(this.StartGame);
							}
							input.ReadMessage(startGame);
							this.StartGame = startGame;
							continue;
						}
						if (num == 122U)
						{
							RoomBroadcast roomBroadcast = new RoomBroadcast();
							if (this.dataCase_ == BeatSaberBody.DataOneofCase.RoomBroadcast)
							{
								roomBroadcast.MergeFrom(this.RoomBroadcast);
							}
							input.ReadMessage(roomBroadcast);
							this.RoomBroadcast = roomBroadcast;
							continue;
						}
					}
				}
				else if (num <= 194U)
				{
					if (num <= 154U)
					{
						if (num <= 138U)
						{
							if (num == 130U)
							{
								ChangeRoomOwner changeRoomOwner = new ChangeRoomOwner();
								if (this.dataCase_ == BeatSaberBody.DataOneofCase.ChangeRoomOwner)
								{
									changeRoomOwner.MergeFrom(this.ChangeRoomOwner);
								}
								input.ReadMessage(changeRoomOwner);
								this.ChangeRoomOwner = changeRoomOwner;
								continue;
							}
							if (num == 138U)
							{
								ModifyPersonalCfg modifyPersonalCfg = new ModifyPersonalCfg();
								if (this.dataCase_ == BeatSaberBody.DataOneofCase.ModifyPersonalCfg)
								{
									modifyPersonalCfg.MergeFrom(this.ModifyPersonalCfg);
								}
								input.ReadMessage(modifyPersonalCfg);
								this.ModifyPersonalCfg = modifyPersonalCfg;
								continue;
							}
						}
						else
						{
							if (num == 146U)
							{
								ModifyRoomCfg modifyRoomCfg = new ModifyRoomCfg();
								if (this.dataCase_ == BeatSaberBody.DataOneofCase.ModifyRoomCfg)
								{
									modifyRoomCfg.MergeFrom(this.ModifyRoomCfg);
								}
								input.ReadMessage(modifyRoomCfg);
								this.ModifyRoomCfg = modifyRoomCfg;
								continue;
							}
							if (num == 154U)
							{
								ModifySongCfg modifySongCfg = new ModifySongCfg();
								if (this.dataCase_ == BeatSaberBody.DataOneofCase.ModifySongCfg)
								{
									modifySongCfg.MergeFrom(this.ModifySongCfg);
								}
								input.ReadMessage(modifySongCfg);
								this.ModifySongCfg = modifySongCfg;
								continue;
							}
						}
					}
					else if (num <= 170U)
					{
						if (num == 162U)
						{
							RoomSubmitScore roomSubmitScore = new RoomSubmitScore();
							if (this.dataCase_ == BeatSaberBody.DataOneofCase.RoomSubmitScore)
							{
								roomSubmitScore.MergeFrom(this.RoomSubmitScore);
							}
							input.ReadMessage(roomSubmitScore);
							this.RoomSubmitScore = roomSubmitScore;
							continue;
						}
						if (num == 170U)
						{
							FastMatch fastMatch = new FastMatch();
							if (this.dataCase_ == BeatSaberBody.DataOneofCase.FastMatch)
							{
								fastMatch.MergeFrom(this.FastMatch);
							}
							input.ReadMessage(fastMatch);
							this.FastMatch = fastMatch;
							continue;
						}
					}
					else
					{
						if (num == 178U)
						{
							AutoMatch autoMatch = new AutoMatch();
							if (this.dataCase_ == BeatSaberBody.DataOneofCase.AutoMatch)
							{
								autoMatch.MergeFrom(this.AutoMatch);
							}
							input.ReadMessage(autoMatch);
							this.AutoMatch = autoMatch;
							continue;
						}
						if (num == 194U)
						{
							ModifyNickname modifyNickname = new ModifyNickname();
							if (this.dataCase_ == BeatSaberBody.DataOneofCase.ModifyNickname)
							{
								modifyNickname.MergeFrom(this.ModifyNickname);
							}
							input.ReadMessage(modifyNickname);
							this.ModifyNickname = modifyNickname;
							continue;
						}
					}
				}
				else if (num <= 266U)
				{
					if (num <= 250U)
					{
						if (num == 242U)
						{
							KickedOutNotice kickedOutNotice = new KickedOutNotice();
							if (this.dataCase_ == BeatSaberBody.DataOneofCase.KickedOutNotice)
							{
								kickedOutNotice.MergeFrom(this.KickedOutNotice);
							}
							input.ReadMessage(kickedOutNotice);
							this.KickedOutNotice = kickedOutNotice;
							continue;
						}
						if (num == 250U)
						{
							RoomUpdatedNotice roomUpdatedNotice = new RoomUpdatedNotice();
							if (this.dataCase_ == BeatSaberBody.DataOneofCase.RoomUpdatedNotice)
							{
								roomUpdatedNotice.MergeFrom(this.RoomUpdatedNotice);
							}
							input.ReadMessage(roomUpdatedNotice);
							this.RoomUpdatedNotice = roomUpdatedNotice;
							continue;
						}
					}
					else
					{
						if (num == 258U)
						{
							KickedOutRoomNotice kickedOutRoomNotice = new KickedOutRoomNotice();
							if (this.dataCase_ == BeatSaberBody.DataOneofCase.KickedOutRoomNotice)
							{
								kickedOutRoomNotice.MergeFrom(this.KickedOutRoomNotice);
							}
							input.ReadMessage(kickedOutRoomNotice);
							this.KickedOutRoomNotice = kickedOutRoomNotice;
							continue;
						}
						if (num == 266U)
						{
							StartGameNotice startGameNotice = new StartGameNotice();
							if (this.dataCase_ == BeatSaberBody.DataOneofCase.StartGameNotice)
							{
								startGameNotice.MergeFrom(this.StartGameNotice);
							}
							input.ReadMessage(startGameNotice);
							this.StartGameNotice = startGameNotice;
							continue;
						}
					}
				}
				else if (num <= 282U)
				{
					if (num == 274U)
					{
						RoomSubmitScoreNotice roomSubmitScoreNotice = new RoomSubmitScoreNotice();
						if (this.dataCase_ == BeatSaberBody.DataOneofCase.RoomSubmitScoreNotice)
						{
							roomSubmitScoreNotice.MergeFrom(this.RoomSubmitScoreNotice);
						}
						input.ReadMessage(roomSubmitScoreNotice);
						this.RoomSubmitScoreNotice = roomSubmitScoreNotice;
						continue;
					}
					if (num == 282U)
					{
						RoomBroadcastNotice roomBroadcastNotice = new RoomBroadcastNotice();
						if (this.dataCase_ == BeatSaberBody.DataOneofCase.RoomBroadcastNotice)
						{
							roomBroadcastNotice.MergeFrom(this.RoomBroadcastNotice);
						}
						input.ReadMessage(roomBroadcastNotice);
						this.RoomBroadcastNotice = roomBroadcastNotice;
						continue;
					}
				}
				else
				{
					if (num == 290U)
					{
						AutoMatchNotice autoMatchNotice = new AutoMatchNotice();
						if (this.dataCase_ == BeatSaberBody.DataOneofCase.AutoMatchNotice)
						{
							autoMatchNotice.MergeFrom(this.AutoMatchNotice);
						}
						input.ReadMessage(autoMatchNotice);
						this.AutoMatchNotice = autoMatchNotice;
						continue;
					}
					if (num == 298U)
					{
						HeadphoneOnNotice headphoneOnNotice = new HeadphoneOnNotice();
						if (this.dataCase_ == BeatSaberBody.DataOneofCase.HeadphoneOnNotice)
						{
							headphoneOnNotice.MergeFrom(this.HeadphoneOnNotice);
						}
						input.ReadMessage(headphoneOnNotice);
						this.HeadphoneOnNotice = headphoneOnNotice;
						continue;
					}
				}
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x04000031 RID: 49
		private static readonly MessageParser<BeatSaberBody> _parser = new MessageParser<BeatSaberBody>(() => new BeatSaberBody());

		// Token: 0x04000032 RID: 50
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000033 RID: 51
		public const int TypeFieldNumber = 1;

		// Token: 0x04000034 RID: 52
		private DataType type_;

		// Token: 0x04000035 RID: 53
		public const int PingFieldNumber = 2;

		// Token: 0x04000036 RID: 54
		public const int LoginFieldNumber = 3;

		// Token: 0x04000037 RID: 55
		public const int RenewFieldNumber = 4;

		// Token: 0x04000038 RID: 56
		public const int LogoutFieldNumber = 5;

		// Token: 0x04000039 RID: 57
		public const int GetPlayerFieldNumber = 6;

		// Token: 0x0400003A RID: 58
		public const int SongListFieldNumber = 7;

		// Token: 0x0400003B RID: 59
		public const int RoomListFieldNumber = 8;

		// Token: 0x0400003C RID: 60
		public const int GetRoomFieldNumber = 9;

		// Token: 0x0400003D RID: 61
		public const int CreateRoomFieldNumber = 10;

		// Token: 0x0400003E RID: 62
		public const int JoinRoomFieldNumber = 11;

		// Token: 0x0400003F RID: 63
		public const int ExitRoomFieldNumber = 12;

		// Token: 0x04000040 RID: 64
		public const int KickOutRoomPlayerFieldNumber = 13;

		// Token: 0x04000041 RID: 65
		public const int StartGameFieldNumber = 14;

		// Token: 0x04000042 RID: 66
		public const int RoomBroadcastFieldNumber = 15;

		// Token: 0x04000043 RID: 67
		public const int ChangeRoomOwnerFieldNumber = 16;

		// Token: 0x04000044 RID: 68
		public const int ModifyPersonalCfgFieldNumber = 17;

		// Token: 0x04000045 RID: 69
		public const int ModifyRoomCfgFieldNumber = 18;

		// Token: 0x04000046 RID: 70
		public const int ModifySongCfgFieldNumber = 19;

		// Token: 0x04000047 RID: 71
		public const int RoomSubmitScoreFieldNumber = 20;

		// Token: 0x04000048 RID: 72
		public const int FastMatchFieldNumber = 21;

		// Token: 0x04000049 RID: 73
		public const int AutoMatchFieldNumber = 22;

		// Token: 0x0400004A RID: 74
		public const int ModifyNicknameFieldNumber = 24;

		// Token: 0x0400004B RID: 75
		public const int KickedOutNoticeFieldNumber = 30;

		// Token: 0x0400004C RID: 76
		public const int RoomUpdatedNoticeFieldNumber = 31;

		// Token: 0x0400004D RID: 77
		public const int KickedOutRoomNoticeFieldNumber = 32;

		// Token: 0x0400004E RID: 78
		public const int StartGameNoticeFieldNumber = 33;

		// Token: 0x0400004F RID: 79
		public const int RoomSubmitScoreNoticeFieldNumber = 34;

		// Token: 0x04000050 RID: 80
		public const int RoomBroadcastNoticeFieldNumber = 35;

		// Token: 0x04000051 RID: 81
		public const int AutoMatchNoticeFieldNumber = 36;

		// Token: 0x04000052 RID: 82
		public const int HeadphoneOnNoticeFieldNumber = 37;

		// Token: 0x04000053 RID: 83
		private object data_;

		// Token: 0x04000054 RID: 84
		private BeatSaberBody.DataOneofCase dataCase_;

		// Token: 0x020000A1 RID: 161
		public enum DataOneofCase
		{
			// Token: 0x040004FF RID: 1279
			None,
			// Token: 0x04000500 RID: 1280
			Ping = 2,
			// Token: 0x04000501 RID: 1281
			Login,
			// Token: 0x04000502 RID: 1282
			Renew,
			// Token: 0x04000503 RID: 1283
			Logout,
			// Token: 0x04000504 RID: 1284
			GetPlayer,
			// Token: 0x04000505 RID: 1285
			SongList,
			// Token: 0x04000506 RID: 1286
			RoomList,
			// Token: 0x04000507 RID: 1287
			GetRoom,
			// Token: 0x04000508 RID: 1288
			CreateRoom,
			// Token: 0x04000509 RID: 1289
			JoinRoom,
			// Token: 0x0400050A RID: 1290
			ExitRoom,
			// Token: 0x0400050B RID: 1291
			KickOutRoomPlayer,
			// Token: 0x0400050C RID: 1292
			StartGame,
			// Token: 0x0400050D RID: 1293
			RoomBroadcast,
			// Token: 0x0400050E RID: 1294
			ChangeRoomOwner,
			// Token: 0x0400050F RID: 1295
			ModifyPersonalCfg,
			// Token: 0x04000510 RID: 1296
			ModifyRoomCfg,
			// Token: 0x04000511 RID: 1297
			ModifySongCfg,
			// Token: 0x04000512 RID: 1298
			RoomSubmitScore,
			// Token: 0x04000513 RID: 1299
			FastMatch,
			// Token: 0x04000514 RID: 1300
			AutoMatch,
			// Token: 0x04000515 RID: 1301
			ModifyNickname = 24,
			// Token: 0x04000516 RID: 1302
			KickedOutNotice = 30,
			// Token: 0x04000517 RID: 1303
			RoomUpdatedNotice,
			// Token: 0x04000518 RID: 1304
			KickedOutRoomNotice,
			// Token: 0x04000519 RID: 1305
			StartGameNotice,
			// Token: 0x0400051A RID: 1306
			RoomSubmitScoreNotice,
			// Token: 0x0400051B RID: 1307
			RoomBroadcastNotice,
			// Token: 0x0400051C RID: 1308
			AutoMatchNotice,
			// Token: 0x0400051D RID: 1309
			HeadphoneOnNotice
		}
	}
}
