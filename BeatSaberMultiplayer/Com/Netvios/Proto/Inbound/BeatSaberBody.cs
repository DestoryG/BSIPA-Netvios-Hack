using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Inbound
{
	// Token: 0x02000030 RID: 48
	public sealed class BeatSaberBody : IMessage<BeatSaberBody>, IMessage, IEquatable<BeatSaberBody>, IDeepCloneable<BeatSaberBody>
	{
		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060003E5 RID: 997 RVA: 0x00011D1C File Offset: 0x0000FF1C
		[DebuggerNonUserCode]
		public static MessageParser<BeatSaberBody> Parser
		{
			get
			{
				return BeatSaberBody._parser;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060003E6 RID: 998 RVA: 0x00011D23 File Offset: 0x0000FF23
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberInboundMessageReflection.Descriptor.MessageTypes[0];
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x00011D35 File Offset: 0x0000FF35
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return BeatSaberBody.Descriptor;
			}
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0000370C File Offset: 0x0000190C
		[DebuggerNonUserCode]
		public BeatSaberBody()
		{
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00011D3C File Offset: 0x0000FF3C
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
			}
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x00011FA9 File Offset: 0x000101A9
		[DebuggerNonUserCode]
		public BeatSaberBody Clone()
		{
			return new BeatSaberBody(this);
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060003EB RID: 1003 RVA: 0x00011FB1 File Offset: 0x000101B1
		// (set) Token: 0x060003EC RID: 1004 RVA: 0x00011FB9 File Offset: 0x000101B9
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

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x00011FC2 File Offset: 0x000101C2
		// (set) Token: 0x060003EE RID: 1006 RVA: 0x00011FDA File Offset: 0x000101DA
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

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060003EF RID: 1007 RVA: 0x00011FF0 File Offset: 0x000101F0
		// (set) Token: 0x060003F0 RID: 1008 RVA: 0x00012008 File Offset: 0x00010208
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

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060003F1 RID: 1009 RVA: 0x0001201E File Offset: 0x0001021E
		// (set) Token: 0x060003F2 RID: 1010 RVA: 0x00012036 File Offset: 0x00010236
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

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060003F3 RID: 1011 RVA: 0x0001204C File Offset: 0x0001024C
		// (set) Token: 0x060003F4 RID: 1012 RVA: 0x00012064 File Offset: 0x00010264
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

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060003F5 RID: 1013 RVA: 0x0001207A File Offset: 0x0001027A
		// (set) Token: 0x060003F6 RID: 1014 RVA: 0x00012092 File Offset: 0x00010292
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

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060003F7 RID: 1015 RVA: 0x000120A8 File Offset: 0x000102A8
		// (set) Token: 0x060003F8 RID: 1016 RVA: 0x000120C0 File Offset: 0x000102C0
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

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060003F9 RID: 1017 RVA: 0x000120D6 File Offset: 0x000102D6
		// (set) Token: 0x060003FA RID: 1018 RVA: 0x000120EE File Offset: 0x000102EE
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

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060003FB RID: 1019 RVA: 0x00012104 File Offset: 0x00010304
		// (set) Token: 0x060003FC RID: 1020 RVA: 0x0001211D File Offset: 0x0001031D
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

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060003FD RID: 1021 RVA: 0x00012134 File Offset: 0x00010334
		// (set) Token: 0x060003FE RID: 1022 RVA: 0x0001214D File Offset: 0x0001034D
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

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060003FF RID: 1023 RVA: 0x00012164 File Offset: 0x00010364
		// (set) Token: 0x06000400 RID: 1024 RVA: 0x0001217D File Offset: 0x0001037D
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

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000401 RID: 1025 RVA: 0x00012194 File Offset: 0x00010394
		// (set) Token: 0x06000402 RID: 1026 RVA: 0x000121AD File Offset: 0x000103AD
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

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000403 RID: 1027 RVA: 0x000121C4 File Offset: 0x000103C4
		// (set) Token: 0x06000404 RID: 1028 RVA: 0x000121DD File Offset: 0x000103DD
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

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000405 RID: 1029 RVA: 0x000121F4 File Offset: 0x000103F4
		// (set) Token: 0x06000406 RID: 1030 RVA: 0x0001220D File Offset: 0x0001040D
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

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x00012224 File Offset: 0x00010424
		// (set) Token: 0x06000408 RID: 1032 RVA: 0x0001223D File Offset: 0x0001043D
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

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x00012254 File Offset: 0x00010454
		// (set) Token: 0x0600040A RID: 1034 RVA: 0x0001226D File Offset: 0x0001046D
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

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x00012284 File Offset: 0x00010484
		// (set) Token: 0x0600040C RID: 1036 RVA: 0x0001229D File Offset: 0x0001049D
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

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x000122B4 File Offset: 0x000104B4
		// (set) Token: 0x0600040E RID: 1038 RVA: 0x000122CD File Offset: 0x000104CD
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

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x000122E4 File Offset: 0x000104E4
		// (set) Token: 0x06000410 RID: 1040 RVA: 0x000122FD File Offset: 0x000104FD
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

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x00012314 File Offset: 0x00010514
		// (set) Token: 0x06000412 RID: 1042 RVA: 0x0001232D File Offset: 0x0001052D
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

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000413 RID: 1043 RVA: 0x00012344 File Offset: 0x00010544
		// (set) Token: 0x06000414 RID: 1044 RVA: 0x0001235D File Offset: 0x0001055D
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

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000415 RID: 1045 RVA: 0x00012374 File Offset: 0x00010574
		// (set) Token: 0x06000416 RID: 1046 RVA: 0x0001238D File Offset: 0x0001058D
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

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000417 RID: 1047 RVA: 0x000123A4 File Offset: 0x000105A4
		// (set) Token: 0x06000418 RID: 1048 RVA: 0x000123BD File Offset: 0x000105BD
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

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x000123D4 File Offset: 0x000105D4
		[DebuggerNonUserCode]
		public BeatSaberBody.DataOneofCase DataCase
		{
			get
			{
				return this.dataCase_;
			}
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x000123DC File Offset: 0x000105DC
		[DebuggerNonUserCode]
		public void ClearData()
		{
			this.dataCase_ = BeatSaberBody.DataOneofCase.None;
			this.data_ = null;
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x000123EC File Offset: 0x000105EC
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as BeatSaberBody);
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x000123FC File Offset: 0x000105FC
		[DebuggerNonUserCode]
		public bool Equals(BeatSaberBody other)
		{
			return other != null && (other == this || (this.Type == other.Type && object.Equals(this.Ping, other.Ping) && object.Equals(this.Login, other.Login) && object.Equals(this.Renew, other.Renew) && object.Equals(this.Logout, other.Logout) && object.Equals(this.GetPlayer, other.GetPlayer) && object.Equals(this.SongList, other.SongList) && object.Equals(this.RoomList, other.RoomList) && object.Equals(this.GetRoom, other.GetRoom) && object.Equals(this.CreateRoom, other.CreateRoom) && object.Equals(this.JoinRoom, other.JoinRoom) && object.Equals(this.ExitRoom, other.ExitRoom) && object.Equals(this.KickOutRoomPlayer, other.KickOutRoomPlayer) && object.Equals(this.StartGame, other.StartGame) && object.Equals(this.RoomBroadcast, other.RoomBroadcast) && object.Equals(this.ChangeRoomOwner, other.ChangeRoomOwner) && object.Equals(this.ModifyPersonalCfg, other.ModifyPersonalCfg) && object.Equals(this.ModifyRoomCfg, other.ModifyRoomCfg) && object.Equals(this.ModifySongCfg, other.ModifySongCfg) && object.Equals(this.RoomSubmitScore, other.RoomSubmitScore) && object.Equals(this.FastMatch, other.FastMatch) && object.Equals(this.AutoMatch, other.AutoMatch) && object.Equals(this.ModifyNickname, other.ModifyNickname) && this.DataCase == other.DataCase && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x00012614 File Offset: 0x00010814
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
			num ^= (int)this.dataCase_;
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x0001286C File Offset: 0x00010A6C
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
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00012B58 File Offset: 0x00010D58
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
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00012DCC File Offset: 0x00010FCC
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
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x000131F0 File Offset: 0x000113F0
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 90U)
				{
					if (num <= 42U)
					{
						if (num <= 18U)
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
						}
						else
						{
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
					}
					else if (num <= 66U)
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
					}
					else
					{
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
				else if (num <= 138U)
				{
					if (num <= 114U)
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
					}
					else
					{
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
				}
				else if (num <= 162U)
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
				}
				else
				{
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
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x04000207 RID: 519
		private static readonly MessageParser<BeatSaberBody> _parser = new MessageParser<BeatSaberBody>(() => new BeatSaberBody());

		// Token: 0x04000208 RID: 520
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000209 RID: 521
		public const int TypeFieldNumber = 1;

		// Token: 0x0400020A RID: 522
		private DataType type_;

		// Token: 0x0400020B RID: 523
		public const int PingFieldNumber = 2;

		// Token: 0x0400020C RID: 524
		public const int LoginFieldNumber = 3;

		// Token: 0x0400020D RID: 525
		public const int RenewFieldNumber = 4;

		// Token: 0x0400020E RID: 526
		public const int LogoutFieldNumber = 5;

		// Token: 0x0400020F RID: 527
		public const int GetPlayerFieldNumber = 6;

		// Token: 0x04000210 RID: 528
		public const int SongListFieldNumber = 7;

		// Token: 0x04000211 RID: 529
		public const int RoomListFieldNumber = 8;

		// Token: 0x04000212 RID: 530
		public const int GetRoomFieldNumber = 9;

		// Token: 0x04000213 RID: 531
		public const int CreateRoomFieldNumber = 10;

		// Token: 0x04000214 RID: 532
		public const int JoinRoomFieldNumber = 11;

		// Token: 0x04000215 RID: 533
		public const int ExitRoomFieldNumber = 12;

		// Token: 0x04000216 RID: 534
		public const int KickOutRoomPlayerFieldNumber = 13;

		// Token: 0x04000217 RID: 535
		public const int StartGameFieldNumber = 14;

		// Token: 0x04000218 RID: 536
		public const int RoomBroadcastFieldNumber = 15;

		// Token: 0x04000219 RID: 537
		public const int ChangeRoomOwnerFieldNumber = 16;

		// Token: 0x0400021A RID: 538
		public const int ModifyPersonalCfgFieldNumber = 17;

		// Token: 0x0400021B RID: 539
		public const int ModifyRoomCfgFieldNumber = 18;

		// Token: 0x0400021C RID: 540
		public const int ModifySongCfgFieldNumber = 19;

		// Token: 0x0400021D RID: 541
		public const int RoomSubmitScoreFieldNumber = 20;

		// Token: 0x0400021E RID: 542
		public const int FastMatchFieldNumber = 21;

		// Token: 0x0400021F RID: 543
		public const int AutoMatchFieldNumber = 22;

		// Token: 0x04000220 RID: 544
		public const int ModifyNicknameFieldNumber = 24;

		// Token: 0x04000221 RID: 545
		private object data_;

		// Token: 0x04000222 RID: 546
		private BeatSaberBody.DataOneofCase dataCase_;

		// Token: 0x020000C9 RID: 201
		public enum DataOneofCase
		{
			// Token: 0x04000548 RID: 1352
			None,
			// Token: 0x04000549 RID: 1353
			Ping = 2,
			// Token: 0x0400054A RID: 1354
			Login,
			// Token: 0x0400054B RID: 1355
			Renew,
			// Token: 0x0400054C RID: 1356
			Logout,
			// Token: 0x0400054D RID: 1357
			GetPlayer,
			// Token: 0x0400054E RID: 1358
			SongList,
			// Token: 0x0400054F RID: 1359
			RoomList,
			// Token: 0x04000550 RID: 1360
			GetRoom,
			// Token: 0x04000551 RID: 1361
			CreateRoom,
			// Token: 0x04000552 RID: 1362
			JoinRoom,
			// Token: 0x04000553 RID: 1363
			ExitRoom,
			// Token: 0x04000554 RID: 1364
			KickOutRoomPlayer,
			// Token: 0x04000555 RID: 1365
			StartGame,
			// Token: 0x04000556 RID: 1366
			RoomBroadcast,
			// Token: 0x04000557 RID: 1367
			ChangeRoomOwner,
			// Token: 0x04000558 RID: 1368
			ModifyPersonalCfg,
			// Token: 0x04000559 RID: 1369
			ModifyRoomCfg,
			// Token: 0x0400055A RID: 1370
			ModifySongCfg,
			// Token: 0x0400055B RID: 1371
			RoomSubmitScore,
			// Token: 0x0400055C RID: 1372
			FastMatch,
			// Token: 0x0400055D RID: 1373
			AutoMatch,
			// Token: 0x0400055E RID: 1374
			ModifyNickname = 24
		}
	}
}
