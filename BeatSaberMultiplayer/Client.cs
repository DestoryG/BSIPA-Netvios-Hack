using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using BeatSaberMultiplayer.Configuration;
using BeatSaberMultiplayer.Data;
using BeatSaberMultiplayer.Helper;
using BeatSaberMultiplayer.UI;
using BeatSaberMultiplayer.VOIP;
using Com.Netvios.Proto;
using Com.Netvios.Proto.Inbound;
using Com.Netvios.Proto.Outbound;
using Google.Protobuf;
using Newtonsoft.Json;
using UnityEngine;

namespace BeatSaberMultiplayer
{
	// Token: 0x0200004C RID: 76
	internal class Client : MonoBehaviour
	{
		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000647 RID: 1607 RVA: 0x00019611 File Offset: 0x00017811
		// (set) Token: 0x06000648 RID: 1608 RVA: 0x00019619 File Offset: 0x00017819
		public bool isConnected { get; private set; }

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000649 RID: 1609 RVA: 0x00019622 File Offset: 0x00017822
		// (set) Token: 0x0600064A RID: 1610 RVA: 0x0001962A File Offset: 0x0001782A
		public bool isLogged { get; private set; }

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x00019633 File Offset: 0x00017833
		// (set) Token: 0x0600064C RID: 1612 RVA: 0x0001963B File Offset: 0x0001783B
		public bool isHost { get; private set; }

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x0600064D RID: 1613 RVA: 0x00019644 File Offset: 0x00017844
		// (set) Token: 0x0600064E RID: 1614 RVA: 0x0001964C File Offset: 0x0001784C
		public bool isJoin { get; private set; }

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x0600064F RID: 1615 RVA: 0x00019655 File Offset: 0x00017855
		// (set) Token: 0x06000650 RID: 1616 RVA: 0x0001965D File Offset: 0x0001785D
		public string joinRoomId { get; private set; }

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000651 RID: 1617 RVA: 0x00019666 File Offset: 0x00017866
		// (set) Token: 0x06000652 RID: 1618 RVA: 0x0001966E File Offset: 0x0001786E
		public BeatSaberMultiplayer.Data.Room.RoomStatus roomStatus { get; private set; }

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000653 RID: 1619 RVA: 0x00019677 File Offset: 0x00017877
		// (set) Token: 0x06000654 RID: 1620 RVA: 0x0001967F File Offset: 0x0001787F
		public Com.Netvios.Proto.Outbound.SongCfg songCfg { get; private set; }

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000655 RID: 1621 RVA: 0x00019688 File Offset: 0x00017888
		// (set) Token: 0x06000656 RID: 1622 RVA: 0x00019690 File Offset: 0x00017890
		public bool delayRoomUpdatedNoticeFlag { get; private set; }

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000657 RID: 1623 RVA: 0x00019699 File Offset: 0x00017899
		// (set) Token: 0x06000658 RID: 1624 RVA: 0x000196A0 File Offset: 0x000178A0
		public int TcpReceiveMessageErrorCode
		{
			get
			{
				return -9999;
			}
			private set
			{
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000659 RID: 1625 RVA: 0x000196A4 File Offset: 0x000178A4
		// (remove) Token: 0x0600065A RID: 1626 RVA: 0x000196DC File Offset: 0x000178DC
		public event Action<int, string> ConnectedAndLoggedServerEvent;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600065B RID: 1627 RVA: 0x00019714 File Offset: 0x00017914
		// (remove) Token: 0x0600065C RID: 1628 RVA: 0x0001974C File Offset: 0x0001794C
		public event Action LoginEvent;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600065D RID: 1629 RVA: 0x00019784 File Offset: 0x00017984
		// (remove) Token: 0x0600065E RID: 1630 RVA: 0x000197BC File Offset: 0x000179BC
		public event Action<long, string, string> ChangeRoomOwnerEvent;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600065F RID: 1631 RVA: 0x000197F4 File Offset: 0x000179F4
		// (remove) Token: 0x06000660 RID: 1632 RVA: 0x0001982C File Offset: 0x00017A2C
		public event Action<int, string> OccurredErrorEvent;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000661 RID: 1633 RVA: 0x00019864 File Offset: 0x00017A64
		// (remove) Token: 0x06000662 RID: 1634 RVA: 0x0001989C File Offset: 0x00017A9C
		public event Action<int, string> OccurredTcpConnectErrorEvent;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000663 RID: 1635 RVA: 0x000198D4 File Offset: 0x00017AD4
		// (remove) Token: 0x06000664 RID: 1636 RVA: 0x0001990C File Offset: 0x00017B0C
		public event Action<int, string, Com.Netvios.Proto.Outbound.RoomList> GetRoomsEvent;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000665 RID: 1637 RVA: 0x00019944 File Offset: 0x00017B44
		// (remove) Token: 0x06000666 RID: 1638 RVA: 0x0001997C File Offset: 0x00017B7C
		public event Action<int, string, Com.Netvios.Proto.Outbound.CreateRoom> CreatedRoomEvent;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000667 RID: 1639 RVA: 0x000199B4 File Offset: 0x00017BB4
		// (remove) Token: 0x06000668 RID: 1640 RVA: 0x000199EC File Offset: 0x00017BEC
		public event Action<int, string, Com.Netvios.Proto.Outbound.GetRoom> GetRoomEvent;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000669 RID: 1641 RVA: 0x00019A24 File Offset: 0x00017C24
		// (remove) Token: 0x0600066A RID: 1642 RVA: 0x00019A5C File Offset: 0x00017C5C
		public event Action<int, string, Com.Netvios.Proto.Outbound.JoinRoom> JoinRoomEvent;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600066B RID: 1643 RVA: 0x00019A94 File Offset: 0x00017C94
		// (remove) Token: 0x0600066C RID: 1644 RVA: 0x00019ACC File Offset: 0x00017CCC
		public event Action<int, string, RoomUpdatedNotice> RoomUpdatedNoticeEvent;

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x0600066D RID: 1645 RVA: 0x00019B04 File Offset: 0x00017D04
		// (remove) Token: 0x0600066E RID: 1646 RVA: 0x00019B3C File Offset: 0x00017D3C
		public event Action<int, string, KickedOutRoomNotice> KickedOutRoomNoticeEvent;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x0600066F RID: 1647 RVA: 0x00019B74 File Offset: 0x00017D74
		// (remove) Token: 0x06000670 RID: 1648 RVA: 0x00019BAC File Offset: 0x00017DAC
		public event Action<int, string, StartGameNotice> StartGameNoticeEvent;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000671 RID: 1649 RVA: 0x00019BE4 File Offset: 0x00017DE4
		// (remove) Token: 0x06000672 RID: 1650 RVA: 0x00019C1C File Offset: 0x00017E1C
		public event Action<int, string, RoomSubmitScoreNotice> RoomSubmitScoreNoticeEvent;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000673 RID: 1651 RVA: 0x00019C54 File Offset: 0x00017E54
		// (remove) Token: 0x06000674 RID: 1652 RVA: 0x00019C8C File Offset: 0x00017E8C
		public event Action<int, string, Com.Netvios.Proto.Outbound.RoomBroadcast> RoomBroadcastEvent;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000675 RID: 1653 RVA: 0x00019CC4 File Offset: 0x00017EC4
		// (remove) Token: 0x06000676 RID: 1654 RVA: 0x00019CFC File Offset: 0x00017EFC
		public event Action<int, string, RoomBroadcastNotice> RoomBroadcastNoticeEvent;

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x06000677 RID: 1655 RVA: 0x00019D34 File Offset: 0x00017F34
		// (remove) Token: 0x06000678 RID: 1656 RVA: 0x00019D6C File Offset: 0x00017F6C
		public event Action<int, string, AutoMatchNotice> AutoMatchNoticeEvent;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06000679 RID: 1657 RVA: 0x00019DA4 File Offset: 0x00017FA4
		// (remove) Token: 0x0600067A RID: 1658 RVA: 0x00019DDC File Offset: 0x00017FDC
		public event Action<int, string, Com.Netvios.Proto.Outbound.FastMatch> FastMatchEvent;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x0600067B RID: 1659 RVA: 0x00019E14 File Offset: 0x00018014
		// (remove) Token: 0x0600067C RID: 1660 RVA: 0x00019E4C File Offset: 0x0001804C
		public event Action<int, string, Com.Netvios.Proto.Outbound.ModifyNickname> ModifyNicknameEvent;

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x0600067D RID: 1661 RVA: 0x00019E81 File Offset: 0x00018081
		public static Client Instance
		{
			get
			{
				if (Client.instance == null)
				{
					Client.instance = new GameObject("MultiplayerClient").AddComponent<Client>();
				}
				return Client.instance;
			}
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x00019EAC File Offset: 0x000180AC
		public void Awake()
		{
			Client.instance = this;
			Object.DontDestroyOnLoad(this);
			if (PluginUI.sdkUserInfo.ContainsKey("token"))
			{
				this.player = new Player(PluginUI.sdkUserInfo["app_channel"], PluginUI.sdkUserInfo["token"]);
				return;
			}
			if (PluginUI.sdkUserInfo.ContainsKey("unionid") && !string.IsNullOrEmpty(PluginUI.sdkUserInfo["unionid"]))
			{
				this.player = new Player(PluginUI.sdkUserInfo["app_channel"], PluginUI.sdkUserInfo["appid"], PluginUI.sdkUserInfo["extra_data"], PluginUI.sdkUserInfo["merchant_account"], PluginUI.sdkUserInfo["openid"], PluginUI.sdkUserInfo["sdk_version"], PluginUI.sdkUserInfo["udid"], PluginUI.sdkUserInfo["unionid"]);
				return;
			}
			if (PluginUI.sdkUserInfo.ContainsKey("udid") && !string.IsNullOrEmpty(PluginUI.sdkUserInfo["udid"]))
			{
				this.player = new Player(PluginUI.sdkUserInfo["udid"]);
				this.player.appChannel = "visitor";
				this.player.token = PluginUI.sdkUserInfo["udid"];
				this.player.nickname = PluginConfig.Instance.Nickname;
			}
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x0001A034 File Offset: 0x00018234
		public void Update()
		{
			if (this.dataBuff != null && this.dataBuff.Length != 0)
			{
				int num = new CodedInputStream(this.dataBuff).ReadLength();
				int num2 = CodedOutputStream.ComputeLengthSize(num);
				if (num + num2 <= this.dataBuff.Length)
				{
					byte[] array = new byte[num2 + num];
					Array.Copy(this.dataBuff, 0, array, 0, num2 + num);
					byte[] array2 = new byte[num];
					Array.Copy(this.dataBuff, num2, array2, 0, num);
					try
					{
						Com.Netvios.Proto.Outbound.Body body = Com.Netvios.Proto.Outbound.Body.Parser.ParseFrom(array2);
						if (body.Code != 0)
						{
							Logger.log.Error("ReceiveError:" + body.ToString());
							this.ReceiveErrorHandler(body);
						}
						else if (body.Game == GameType.BeatSaber)
						{
							this.BeatsaberReceiveMessage(body.BeatSaberBody);
						}
						else
						{
							Logger.log.Warn(string.Format("invalid game type: {0}", body.Game));
						}
					}
					catch (Exception ex)
					{
						Logger.log.Error(string.Format("logic error after receive msg: error: {0}", ex));
					}
					byte[] array3 = new byte[this.dataBuff.Length - num - num2];
					Array.Copy(this.dataBuff, num2 + num, array3, 0, array3.Length);
					object syncRoot = this.dataBuff.SyncRoot;
					lock (syncRoot)
					{
						this.dataBuff = array3;
					}
				}
			}
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x0001A1B4 File Offset: 0x000183B4
		public void FixedUpdate()
		{
			if (this.pingTimer < this.pingInterval)
			{
				this.pingTimer += Time.fixedDeltaTime;
				return;
			}
			this.Ping();
			this.pingTimer = 0f;
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x0001A1E8 File Offset: 0x000183E8
		public void Connect()
		{
			if (this.isConnected)
			{
				return;
			}
			try
			{
				this.client = new TcpClient(this.ip, this.port);
				this.isConnected = true;
				this.client.SendTimeout = this.clientSendTimeOut;
				this.client.ReceiveTimeout = this.clientReceiveTimeout;
				this.client.ReceiveBufferSize = this.clientReceiveBufferSize;
				this.receiveBuff = new byte[this.client.ReceiveBufferSize];
				Task.Run(delegate
				{
					this.client.GetStream().BeginRead(this.receiveBuff, 0, this.client.ReceiveBufferSize, new AsyncCallback(this.ReceiveMessage), null);
				});
			}
			catch (Exception ex)
			{
				this.isConnected = false;
				Logger.log.Error("connect server error: " + ex.Message);
				Action<int, string> connectedAndLoggedServerEvent = this.ConnectedAndLoggedServerEvent;
				if (connectedAndLoggedServerEvent != null)
				{
					connectedAndLoggedServerEvent(1, ex.Message);
				}
			}
			this.LoginTcpServer();
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x0001A2CC File Offset: 0x000184CC
		public void Disconnect()
		{
			this.isConnected = false;
			this.isHost = false;
			this.isLogged = false;
			this.isJoin = false;
			this.joinRoomId = "";
			this.roomStatus = BeatSaberMultiplayer.Data.Room.RoomStatus.Waiting;
			this.songCfg = null;
			this.client.Close();
			this.client = null;
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x0001A320 File Offset: 0x00018520
		private void ReceiveMessage(IAsyncResult res)
		{
			try
			{
				if (!this.isConnected)
				{
					Logger.log.Warn("tcp client is offline");
				}
				else
				{
					int num = this.client.GetStream().EndRead(res);
					if (num >= 1)
					{
						if (this.dataBuff == null)
						{
							this.dataBuff = new byte[num];
							Array.Copy(this.receiveBuff, this.dataBuff, num);
						}
						else
						{
							byte[] array = new byte[num + this.dataBuff.Length];
							Array.Copy(this.dataBuff, array, this.dataBuff.Length);
							Array.Copy(this.receiveBuff, 0, array, this.dataBuff.Length, num);
							object syncRoot = this.dataBuff.SyncRoot;
							lock (syncRoot)
							{
								this.dataBuff = array;
							}
						}
						this.client.GetStream().BeginRead(this.receiveBuff, 0, this.client.ReceiveBufferSize, new AsyncCallback(this.ReceiveMessage), null);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.log.Error(string.Format("Receive message error: {0}", ex));
				this.Disconnect();
				Action<int, string> occurredTcpConnectErrorEvent = this.OccurredTcpConnectErrorEvent;
				if (occurredTcpConnectErrorEvent != null)
				{
					occurredTcpConnectErrorEvent(this.TcpReceiveMessageErrorCode, ex.Message);
				}
			}
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x0001A47C File Offset: 0x0001867C
		private void SendMessage(IMessage data)
		{
			if (!this.isConnected)
			{
				Logger.log.Warn("tcp client is offline");
				return;
			}
			try
			{
				CodedOutputStream codedOutputStream = new CodedOutputStream(this.client.GetStream());
				codedOutputStream.WriteMessage(data);
				codedOutputStream.Flush();
			}
			catch (SocketException ex)
			{
				Logger.log.Error("send message error: " + ex.Message);
			}
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x0001A4F0 File Offset: 0x000186F0
		private void BeatsaberReceiveMessage(Com.Netvios.Proto.Outbound.BeatSaberBody body)
		{
			switch (body.Type)
			{
			case DataType.Ping:
			{
				int sequence = body.Ping.Sequence;
				if (!this.pingSequenceDic.ContainsKey(sequence))
				{
					return;
				}
				int num = this.pingSequenceDic[sequence];
				this.pingCounter = DateTime.Now.Millisecond - num;
				return;
			}
			case DataType.Login:
				this.LoginResp(body.Login);
				return;
			case DataType.Logout:
				this.LogoutResp(body.Logout);
				return;
			case DataType.RoomList:
				this.GetRoomsResp(body.RoomList);
				return;
			case DataType.GetRoom:
				this.GetRoomResp(body.GetRoom);
				return;
			case DataType.CreateRoom:
				this.CreateRoomResp(body.CreateRoom);
				return;
			case DataType.JoinRoom:
				this.JoinRoomResp(body.JoinRoom);
				return;
			case DataType.ExitRoom:
				Logger.log.Info("exit room resp:" + body.ExitRoom.ToString());
				return;
			case DataType.KickOutRoomPlayer:
				this.KickOutRoomPlayerResp(body.KickOutRoomPlayer);
				return;
			case DataType.StartGame:
				this.StartGameResp(body.StartGame);
				return;
			case DataType.ChangeRoomOwner:
				this.ChangeRoomOwnerResp(body.ChangeRoomOwner);
				return;
			case DataType.ModifySongCfg:
				this.ModifySongCfgResp(body.ModifySongCfg);
				return;
			case DataType.RoomSubmitScore:
				this.RoomSubmitScoreResp(body.RoomSubmitScore);
				return;
			case DataType.RoomBroadcast:
				this.RoomBroadcastResp(body.RoomBroadcast);
				return;
			case DataType.FastMatch:
				this.FastMatchResp(body.FastMatch);
				return;
			case DataType.ModifyNickname:
				this.ChangeNicknameResp(body.ModifyNickname);
				return;
			case DataType.RoomUpdatedNotice:
				this.RoomUpdatedNoticeResp(body.RoomUpdatedNotice);
				return;
			case DataType.KickedOutRoomNotice:
				this.KickedOutRoomNoticeResp(body.KickedOutRoomNotice);
				return;
			case DataType.StartGameNotice:
				this.StartGameNoticeResp(body.StartGameNotice);
				return;
			case DataType.RoomSubmitScoreNotice:
				this.RoomSubmitScoreNoticeResp(body.RoomSubmitScoreNotice);
				return;
			case DataType.RoomBroadcastNotice:
				this.RoomBroadcastNoticeResp(body.RoomBroadcastNotice);
				return;
			}
			Logger.log.Info("==== BeatsaberReceiveMessage Default ====");
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x0001A70C File Offset: 0x0001890C
		private void BeatsaberSendMessage(Com.Netvios.Proto.Inbound.BeatSaberBody data)
		{
			this.SendMessage(new Com.Netvios.Proto.Inbound.Body
			{
				Game = GameType.BeatSaber,
				BeatSaberBody = data
			});
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x0001A734 File Offset: 0x00018934
		private void ReceiveErrorHandler(Com.Netvios.Proto.Outbound.Body body)
		{
			Logger.log.Error("receive error: " + body.Message);
			switch (body.BeatSaberBody.Type)
			{
			case DataType.Login:
			{
				Action<int, string> connectedAndLoggedServerEvent = this.ConnectedAndLoggedServerEvent;
				if (connectedAndLoggedServerEvent != null)
				{
					connectedAndLoggedServerEvent(body.Code, body.Message);
				}
				this.Disconnect();
				return;
			}
			case DataType.Renew:
			case DataType.GetPlayer:
			case DataType.SongList:
			case DataType.ModifyPersonalCfg:
			case DataType.ModifyRoomCfg:
			case DataType.AutoMatch:
			case (DataType)21:
			case (DataType)23:
			case (DataType)24:
			case (DataType)25:
			case (DataType)26:
			case (DataType)27:
			case (DataType)28:
			case (DataType)29:
			case (DataType)30:
			case (DataType)31:
			case DataType.KickedOutNotice:
			case DataType.RoomSubmitScoreNotice:
				break;
			case DataType.Logout:
				Logger.log.Error("logout tcp server error: " + body.Message);
				return;
			case DataType.RoomList:
			{
				Logger.log.Error("RoomList error: " + body.Message);
				Action<int, string, Com.Netvios.Proto.Outbound.RoomList> getRoomsEvent = this.GetRoomsEvent;
				if (getRoomsEvent == null)
				{
					return;
				}
				getRoomsEvent(body.Code, body.Message, null);
				return;
			}
			case DataType.GetRoom:
			{
				Logger.log.Error("GetRoom error: " + body.Message);
				Action<int, string> occurredErrorEvent = this.OccurredErrorEvent;
				if (occurredErrorEvent == null)
				{
					return;
				}
				occurredErrorEvent(body.Code, body.Message);
				return;
			}
			case DataType.CreateRoom:
			{
				Action<int, string, Com.Netvios.Proto.Outbound.CreateRoom> createdRoomEvent = this.CreatedRoomEvent;
				if (createdRoomEvent == null)
				{
					return;
				}
				createdRoomEvent(body.Code, body.Message, null);
				return;
			}
			case DataType.JoinRoom:
			{
				Action<int, string, Com.Netvios.Proto.Outbound.JoinRoom> joinRoomEvent = this.JoinRoomEvent;
				if (joinRoomEvent == null)
				{
					return;
				}
				joinRoomEvent(body.Code, body.Message, null);
				return;
			}
			case DataType.ExitRoom:
				Logger.log.Error("exit room error: " + body.Message);
				return;
			case DataType.KickOutRoomPlayer:
			{
				Logger.log.Error(string.Format("KickOutRoomPlayer error:{0}|{1}", body.Code, body.Message));
				Action<int, string> occurredErrorEvent2 = this.OccurredErrorEvent;
				if (occurredErrorEvent2 == null)
				{
					return;
				}
				occurredErrorEvent2(body.Code, body.Message);
				return;
			}
			case DataType.StartGame:
			{
				Logger.log.Error(string.Format("StartGame error:{0}|{1}", body.Code, body.Message));
				Action<int, string> occurredErrorEvent3 = this.OccurredErrorEvent;
				if (occurredErrorEvent3 == null)
				{
					return;
				}
				occurredErrorEvent3(body.Code, body.Message);
				return;
			}
			case DataType.ChangeRoomOwner:
			{
				Logger.log.Error(string.Format("ChangeRoomOwner error:{0}|{1}", body.Code, body.Message));
				Action<int, string> occurredErrorEvent4 = this.OccurredErrorEvent;
				if (occurredErrorEvent4 == null)
				{
					return;
				}
				occurredErrorEvent4(body.Code, body.Message);
				return;
			}
			case DataType.ModifySongCfg:
			{
				Logger.log.Error(string.Format("ModifySongCfg error:{0}|{1}", body.Code, body.Message));
				Action<int, string> occurredErrorEvent5 = this.OccurredErrorEvent;
				if (occurredErrorEvent5 == null)
				{
					return;
				}
				occurredErrorEvent5(body.Code, body.Message);
				return;
			}
			case DataType.RoomSubmitScore:
			{
				Logger.log.Error(string.Format("RoomSubmitScore error:{0}|{1}", body.Code, body.Message));
				Action<int, string> occurredErrorEvent6 = this.OccurredErrorEvent;
				if (occurredErrorEvent6 == null)
				{
					return;
				}
				occurredErrorEvent6(body.Code, body.Message);
				return;
			}
			case DataType.RoomBroadcast:
			{
				Logger.log.Error(string.Format("RoomBroadcast error:{0}|{1}", body.Code, body.Message));
				Action<int, string> occurredErrorEvent7 = this.OccurredErrorEvent;
				if (occurredErrorEvent7 == null)
				{
					return;
				}
				occurredErrorEvent7(body.Code, body.Message);
				return;
			}
			case DataType.FastMatch:
			{
				Logger.log.Error(string.Format("FastMatch error:{0}|{1}", body.Code, body.Message));
				Action<int, string, Com.Netvios.Proto.Outbound.FastMatch> fastMatchEvent = this.FastMatchEvent;
				if (fastMatchEvent == null)
				{
					return;
				}
				fastMatchEvent(body.Code, body.Message, null);
				return;
			}
			case DataType.ModifyNickname:
			{
				Logger.log.Error(string.Format("ModifyNickname error:{0}|{1}", body.Code, body.Message));
				Action<int, string, Com.Netvios.Proto.Outbound.ModifyNickname> modifyNicknameEvent = this.ModifyNicknameEvent;
				if (modifyNicknameEvent == null)
				{
					return;
				}
				modifyNicknameEvent(body.Code, body.Message, null);
				break;
			}
			case DataType.RoomUpdatedNotice:
			{
				Logger.log.Error(string.Format("RoomUpdateNotice error:{0}|{1}", body.Code, body.Message));
				Action<int, string> occurredErrorEvent8 = this.OccurredErrorEvent;
				if (occurredErrorEvent8 == null)
				{
					return;
				}
				occurredErrorEvent8(body.Code, body.Message);
				return;
			}
			case DataType.KickedOutRoomNotice:
				Logger.log.Error(string.Format("KickedOutRoomNotice error:{0}|{1}", body.Code, body.Message));
				return;
			case DataType.StartGameNotice:
				Logger.log.Error(string.Format("StartGameNotice error:{0}|{1}", body.Code, body.Message));
				return;
			case DataType.RoomBroadcastNotice:
				Logger.log.Error(string.Format("RoomBroadcastNotice error:{0}|{1}", body.Code, body.Message));
				return;
			default:
				return;
			}
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x0001ABD0 File Offset: 0x00018DD0
		private void LoginTcpServer()
		{
			if (this.player == null)
			{
				return;
			}
			Com.Netvios.Proto.Inbound.Login login = new Com.Netvios.Proto.Inbound.Login();
			login.AppChannel = this.player.appChannel;
			login.Token = this.player.token;
			if (!string.IsNullOrEmpty(this.player.nickname))
			{
				login.Nickname = this.player.nickname;
			}
			this.BeatsaberSendMessage(new Com.Netvios.Proto.Inbound.BeatSaberBody
			{
				Type = DataType.Login,
				Login = login
			});
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x0001AC4C File Offset: 0x00018E4C
		private void LoginResp(Com.Netvios.Proto.Outbound.Login loginData)
		{
			this.player.playerId = loginData.PlayerId;
			this.player.nickname = loginData.Nickname;
			this.player.avatar = loginData.Avatar;
			this.isLogged = true;
			Action<int, string> connectedAndLoggedServerEvent = this.ConnectedAndLoggedServerEvent;
			if (connectedAndLoggedServerEvent != null)
			{
				connectedAndLoggedServerEvent(0, "");
			}
			Action loginEvent = this.LoginEvent;
			if (loginEvent == null)
			{
				return;
			}
			loginEvent();
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x0001ACBC File Offset: 0x00018EBC
		public void LogoutTcpServer()
		{
			if (this.player == null)
			{
				return;
			}
			Com.Netvios.Proto.Inbound.Logout logout = new Com.Netvios.Proto.Inbound.Logout();
			this.BeatsaberSendMessage(new Com.Netvios.Proto.Inbound.BeatSaberBody
			{
				Type = DataType.Logout,
				Logout = logout
			});
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x0001ACF3 File Offset: 0x00018EF3
		private void LogoutResp(Com.Netvios.Proto.Outbound.Logout logoutData)
		{
			this.Disconnect();
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x0001ACFC File Offset: 0x00018EFC
		private void Ping()
		{
			if (!this.isLogged)
			{
				return;
			}
			this.pingSequence++;
			Com.Netvios.Proto.Inbound.Ping ping = new Com.Netvios.Proto.Inbound.Ping();
			Com.Netvios.Proto.Inbound.BeatSaberBody beatSaberBody = new Com.Netvios.Proto.Inbound.BeatSaberBody();
			beatSaberBody.Type = DataType.Ping;
			ping.Sequence = this.pingSequence;
			beatSaberBody.Ping = ping;
			this.pingSequenceDic.Add(this.pingSequence, DateTime.Now.Millisecond);
			while (this.pingSequenceDic.Count > 5)
			{
				int num = this.pingSequenceDic.OrderBy((KeyValuePair<int, int> o) => o.Key).ToDictionary((KeyValuePair<int, int> o) => o.Key, (KeyValuePair<int, int> p) => p.Value).Keys.First<int>();
				this.pingSequenceDic.Remove(num);
			}
			this.BeatsaberSendMessage(beatSaberBody);
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x0001AE08 File Offset: 0x00019008
		public void GetRooms(int page = 0, int size = 100)
		{
			if (!this.isLogged)
			{
				return;
			}
			Com.Netvios.Proto.Inbound.RoomList roomList = new Com.Netvios.Proto.Inbound.RoomList();
			roomList.PageNumber = page;
			roomList.PageSize = size;
			this.BeatsaberSendMessage(new Com.Netvios.Proto.Inbound.BeatSaberBody
			{
				Type = DataType.RoomList,
				RoomList = roomList
			});
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x0001AE4D File Offset: 0x0001904D
		private void GetRoomsResp(Com.Netvios.Proto.Outbound.RoomList roomListData)
		{
			Action<int, string, Com.Netvios.Proto.Outbound.RoomList> getRoomsEvent = this.GetRoomsEvent;
			if (getRoomsEvent == null)
			{
				return;
			}
			getRoomsEvent(0, "", roomListData);
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x0001AE68 File Offset: 0x00019068
		public void CreateRoom(RoomSettings settings, GameplayModifiers modifiers)
		{
			if (!this.isLogged)
			{
				return;
			}
			Com.Netvios.Proto.Inbound.CreateRoom createRoom = new Com.Netvios.Proto.Inbound.CreateRoom();
			createRoom.RoomCfg = new Com.Netvios.Proto.Inbound.RoomCfg
			{
				RoomName = settings.name,
				MaxPlayers = settings.maxPlayers,
				Password = settings.password,
				ResultDisplaySeconds = settings.resultsShowTime
			};
			this.BeatsaberSendMessage(new Com.Netvios.Proto.Inbound.BeatSaberBody
			{
				Type = DataType.CreateRoom,
				CreateRoom = createRoom
			});
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x0001AEDC File Offset: 0x000190DC
		private void CreateRoomResp(Com.Netvios.Proto.Outbound.CreateRoom roomData)
		{
			this.isHost = true;
			this.isJoin = true;
			this.joinRoomId = roomData.RoomId;
			this.roomStatus = BeatSaberMultiplayer.Data.Room.RoomStatus.Waiting;
			Action<int, string, Com.Netvios.Proto.Outbound.CreateRoom> createdRoomEvent = this.CreatedRoomEvent;
			if (createdRoomEvent == null)
			{
				return;
			}
			createdRoomEvent(0, "", roomData);
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x0001AF18 File Offset: 0x00019118
		public void GetRoom()
		{
			if (!this.isLogged)
			{
				return;
			}
			if (string.IsNullOrEmpty(this.joinRoomId))
			{
				return;
			}
			Com.Netvios.Proto.Inbound.GetRoom getRoom = new Com.Netvios.Proto.Inbound.GetRoom();
			getRoom.RoomId = this.joinRoomId;
			this.BeatsaberSendMessage(new Com.Netvios.Proto.Inbound.BeatSaberBody
			{
				Type = DataType.GetRoom,
				GetRoom = getRoom
			});
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x0001AF69 File Offset: 0x00019169
		private void GetRoomResp(Com.Netvios.Proto.Outbound.GetRoom getRoomData)
		{
			this.roomStatus = Utils.ForamtRoomStatus(getRoomData.Status);
			Action<int, string, Com.Netvios.Proto.Outbound.GetRoom> getRoomEvent = this.GetRoomEvent;
			if (getRoomEvent == null)
			{
				return;
			}
			getRoomEvent(0, "", getRoomData);
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x0001AF94 File Offset: 0x00019194
		public void JoinRoom(string roomId, string password)
		{
			if (!this.isLogged)
			{
				return;
			}
			Com.Netvios.Proto.Inbound.JoinRoom joinRoom = new Com.Netvios.Proto.Inbound.JoinRoom();
			joinRoom.RoomId = roomId;
			joinRoom.Password = ((password == null) ? "" : password);
			this.BeatsaberSendMessage(new Com.Netvios.Proto.Inbound.BeatSaberBody
			{
				Type = DataType.JoinRoom,
				JoinRoom = joinRoom
			});
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x0001AFE4 File Offset: 0x000191E4
		private void JoinRoomResp(Com.Netvios.Proto.Outbound.JoinRoom joinRoomData)
		{
			this.isJoin = true;
			this.joinRoomId = joinRoomData.RoomId;
			this.roomStatus = Utils.ForamtRoomStatus(joinRoomData.Status);
			if (joinRoomData.RoomOwner == this.player.playerId)
			{
				this.isHost = true;
			}
			Action<int, string, Com.Netvios.Proto.Outbound.JoinRoom> joinRoomEvent = this.JoinRoomEvent;
			if (joinRoomEvent != null)
			{
				joinRoomEvent(0, "", joinRoomData);
			}
			if (this.delayRoomUpdatedNoticeFlag && this.updatedNoticeData != null)
			{
				this.roomStatus = Utils.ForamtRoomStatus(this.updatedNoticeData.Status);
				this.IsChangeRoomOwner(this.updatedNoticeData);
				Action<int, string, RoomUpdatedNotice> roomUpdatedNoticeEvent = this.RoomUpdatedNoticeEvent;
				if (roomUpdatedNoticeEvent != null)
				{
					roomUpdatedNoticeEvent(0, "", this.updatedNoticeData);
				}
				this.delayRoomUpdatedNoticeFlag = false;
				this.updatedNoticeData = null;
			}
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x0001B0A4 File Offset: 0x000192A4
		private void RoomUpdatedNoticeResp(RoomUpdatedNotice roomUpdatedNoticeData)
		{
			if (!this.isJoin)
			{
				this.delayRoomUpdatedNoticeFlag = true;
				this.updatedNoticeData = roomUpdatedNoticeData;
				return;
			}
			this.songCfg = roomUpdatedNoticeData.SongCfg;
			this.roomStatus = Utils.ForamtRoomStatus(roomUpdatedNoticeData.Status);
			this.IsChangeRoomOwner(roomUpdatedNoticeData);
			Action<int, string, RoomUpdatedNotice> roomUpdatedNoticeEvent = this.RoomUpdatedNoticeEvent;
			if (roomUpdatedNoticeEvent == null)
			{
				return;
			}
			roomUpdatedNoticeEvent(0, "", roomUpdatedNoticeData);
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x0001B104 File Offset: 0x00019304
		private void IsChangeRoomOwner(RoomUpdatedNotice roomUpdatedNoticeData)
		{
			if ((!this.isHost && roomUpdatedNoticeData.RoomOwner == this.player.playerId) || (this.isHost && roomUpdatedNoticeData.RoomOwner != this.player.playerId))
			{
				this.isHost = !this.isHost;
				Action<long, string, string> changeRoomOwnerEvent = this.ChangeRoomOwnerEvent;
				if (changeRoomOwnerEvent == null)
				{
					return;
				}
				changeRoomOwnerEvent(roomUpdatedNoticeData.RoomOwner, roomUpdatedNoticeData.RoomOwnerName, roomUpdatedNoticeData.Status);
			}
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x0001B178 File Offset: 0x00019378
		public void LeaveRoom()
		{
			this.isHost = false;
			this.isJoin = false;
			this.isJoin = false;
			this.joinRoomId = "";
			this.roomStatus = BeatSaberMultiplayer.Data.Room.RoomStatus.Waiting;
			this.songCfg = null;
			Com.Netvios.Proto.Inbound.ExitRoom exitRoom = new Com.Netvios.Proto.Inbound.ExitRoom();
			exitRoom.RoomId = this.joinRoomId;
			this.joinRoomId = "";
			this.BeatsaberSendMessage(new Com.Netvios.Proto.Inbound.BeatSaberBody
			{
				Type = DataType.ExitRoom,
				ExitRoom = exitRoom
			});
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x0001B1EC File Offset: 0x000193EC
		public void ModifySongCfg(string roomId, string songId, string mode, string difficulty, string rules)
		{
			if (!this.isHost)
			{
				return;
			}
			Com.Netvios.Proto.Inbound.ModifySongCfg modifySongCfg = new Com.Netvios.Proto.Inbound.ModifySongCfg();
			modifySongCfg.RoomId = roomId;
			modifySongCfg.SongId = songId;
			modifySongCfg.Mode = mode;
			modifySongCfg.Difficulty = difficulty;
			modifySongCfg.Rules = rules;
			this.BeatsaberSendMessage(new Com.Netvios.Proto.Inbound.BeatSaberBody
			{
				Type = DataType.ModifySongCfg,
				ModifySongCfg = modifySongCfg
			});
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x000196A0 File Offset: 0x000178A0
		private void ModifySongCfgResp(Com.Netvios.Proto.Outbound.ModifySongCfg modifySongCfgData)
		{
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x0001B24C File Offset: 0x0001944C
		public void KickOutRoomPlayer(string roomId, long playerId)
		{
			if (!this.isHost)
			{
				return;
			}
			Com.Netvios.Proto.Inbound.KickOutRoomPlayer kickOutRoomPlayer = new Com.Netvios.Proto.Inbound.KickOutRoomPlayer();
			kickOutRoomPlayer.RoomId = roomId;
			kickOutRoomPlayer.TargetPlayerId = playerId;
			this.BeatsaberSendMessage(new Com.Netvios.Proto.Inbound.BeatSaberBody
			{
				Type = DataType.KickOutRoomPlayer,
				KickOutRoomPlayer = kickOutRoomPlayer
			});
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x000196A0 File Offset: 0x000178A0
		private void KickOutRoomPlayerResp(Com.Netvios.Proto.Outbound.KickOutRoomPlayer kickOutRoomPlayerData)
		{
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x0001B292 File Offset: 0x00019492
		private void KickedOutRoomNoticeResp(KickedOutRoomNotice kickOutRoomPlayerNoticeData)
		{
			Action<int, string, KickedOutRoomNotice> kickedOutRoomNoticeEvent = this.KickedOutRoomNoticeEvent;
			if (kickedOutRoomNoticeEvent == null)
			{
				return;
			}
			kickedOutRoomNoticeEvent(0, "", kickOutRoomPlayerNoticeData);
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x0001B2AC File Offset: 0x000194AC
		public void ChangeRoomOwner(string roomId, long playerId)
		{
			if (!this.isHost)
			{
				return;
			}
			Com.Netvios.Proto.Inbound.ChangeRoomOwner changeRoomOwner = new Com.Netvios.Proto.Inbound.ChangeRoomOwner();
			changeRoomOwner.RoomId = roomId;
			changeRoomOwner.TargetPlayerId = playerId;
			this.BeatsaberSendMessage(new Com.Netvios.Proto.Inbound.BeatSaberBody
			{
				Type = DataType.ChangeRoomOwner,
				ChangeRoomOwner = changeRoomOwner
			});
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x000196A0 File Offset: 0x000178A0
		private void ChangeRoomOwnerResp(Com.Netvios.Proto.Outbound.ChangeRoomOwner changeRoomOwnerData)
		{
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x0001B2F4 File Offset: 0x000194F4
		public void StartGame(string roomId)
		{
			if (!this.isHost)
			{
				return;
			}
			Com.Netvios.Proto.Inbound.StartGame startGame = new Com.Netvios.Proto.Inbound.StartGame();
			startGame.RoomId = roomId;
			this.BeatsaberSendMessage(new Com.Netvios.Proto.Inbound.BeatSaberBody
			{
				Type = DataType.StartGame,
				StartGame = startGame
			});
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x000196A0 File Offset: 0x000178A0
		private void StartGameResp(Com.Netvios.Proto.Outbound.StartGame startGameData)
		{
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x0001B333 File Offset: 0x00019533
		private void StartGameNoticeResp(StartGameNotice startGameNoticeData)
		{
			if (startGameNoticeData.SongCfg != null)
			{
				this.songCfg = startGameNoticeData.SongCfg;
			}
			this.roomStatus = BeatSaberMultiplayer.Data.Room.RoomStatus.Playing;
			Action<int, string, StartGameNotice> startGameNoticeEvent = this.StartGameNoticeEvent;
			if (startGameNoticeEvent == null)
			{
				return;
			}
			startGameNoticeEvent(0, "", startGameNoticeData);
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x0001B368 File Offset: 0x00019568
		public void RoomSubmitScore(bool songDidFinish, int levelBpm, string rank, int maxCombo, int modifiedScore, int goodCutsCount, int badCutsCount, int missedCount, int endSongTime, int songDuration, int leftHandMovementDistance, int rightHandMovementDistance, int leftSaberMovementDistance, int rightSaberMovementDistance, int okCount, int notGoodCount, int rawScore, string levelEndStateType)
		{
			if (!this.isLogged)
			{
				return;
			}
			Com.Netvios.Proto.Inbound.RoomSubmitScore roomSubmitScore = new Com.Netvios.Proto.Inbound.RoomSubmitScore();
			roomSubmitScore.RoomId = this.joinRoomId;
			roomSubmitScore.AppChannel = this.player.appChannel;
			roomSubmitScore.LevelId = this.songCfg.SongId;
			roomSubmitScore.Difficulty = this.songCfg.Difficulty;
			roomSubmitScore.SongDidFinish = songDidFinish;
			roomSubmitScore.LevelBpm = levelBpm;
			roomSubmitScore.Rank = rank;
			roomSubmitScore.MaxCombo = maxCombo;
			roomSubmitScore.ModifiedScore = modifiedScore;
			roomSubmitScore.GoodCutsCount = goodCutsCount;
			roomSubmitScore.BadCutsCount = badCutsCount;
			roomSubmitScore.MissedCount = missedCount;
			roomSubmitScore.EndSongTime = endSongTime;
			roomSubmitScore.SongDuration = songDuration;
			roomSubmitScore.LeftHandMovementDistance = leftHandMovementDistance;
			roomSubmitScore.RightHandMovementDistance = rightHandMovementDistance;
			roomSubmitScore.LeftSaberMovementDistance = leftSaberMovementDistance;
			roomSubmitScore.RightSaberMovementDistance = rightSaberMovementDistance;
			roomSubmitScore.OkCount = okCount;
			roomSubmitScore.NotGoodCount = notGoodCount;
			roomSubmitScore.Mode = this.songCfg.Mode;
			roomSubmitScore.RawScore = rawScore;
			roomSubmitScore.LevelEndStateType = levelEndStateType;
			this.BeatsaberSendMessage(new Com.Netvios.Proto.Inbound.BeatSaberBody
			{
				Type = DataType.RoomSubmitScore,
				RoomSubmitScore = roomSubmitScore
			});
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x0001B47D File Offset: 0x0001967D
		private void RoomSubmitScoreResp(Com.Netvios.Proto.Outbound.RoomSubmitScore roomSubmitScoreData)
		{
			this.GetRoom();
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x0001B485 File Offset: 0x00019685
		private void RoomSubmitScoreNoticeResp(RoomSubmitScoreNotice roomSubmitScoreNoticeData)
		{
			Action<int, string, RoomSubmitScoreNotice> roomSubmitScoreNoticeEvent = this.RoomSubmitScoreNoticeEvent;
			if (roomSubmitScoreNoticeEvent == null)
			{
				return;
			}
			roomSubmitScoreNoticeEvent(0, "", roomSubmitScoreNoticeData);
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x0001B4A0 File Offset: 0x000196A0
		public void RoomBroadcast(RoomBroadcastDataType contentType, string contentData)
		{
			if (!this.isLogged)
			{
				return;
			}
			Com.Netvios.Proto.Inbound.RoomBroadcast roomBroadcast = new Com.Netvios.Proto.Inbound.RoomBroadcast();
			roomBroadcast.RoomId = this.joinRoomId;
			roomBroadcast.Type = contentType.ToString();
			roomBroadcast.Content = contentData;
			this.BeatsaberSendMessage(new Com.Netvios.Proto.Inbound.BeatSaberBody
			{
				Type = DataType.RoomBroadcast,
				RoomBroadcast = roomBroadcast
			});
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x0001B4FE File Offset: 0x000196FE
		public void RoomBroadcastResp(Com.Netvios.Proto.Outbound.RoomBroadcast roomBroadcastData)
		{
			Action<int, string, Com.Netvios.Proto.Outbound.RoomBroadcast> roomBroadcastEvent = this.RoomBroadcastEvent;
			if (roomBroadcastEvent == null)
			{
				return;
			}
			roomBroadcastEvent(0, "", roomBroadcastData);
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x0001B517 File Offset: 0x00019717
		public void RoomBroadcastNoticeResp(RoomBroadcastNotice roomBroadcastNoticeData)
		{
			Action<int, string, RoomBroadcastNotice> roomBroadcastNoticeEvent = this.RoomBroadcastNoticeEvent;
			if (roomBroadcastNoticeEvent == null)
			{
				return;
			}
			roomBroadcastNoticeEvent(0, "", roomBroadcastNoticeData);
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x0001B530 File Offset: 0x00019730
		public void AutoMatch(string songId = null)
		{
			if (!this.isLogged)
			{
				return;
			}
			Com.Netvios.Proto.Inbound.AutoMatch autoMatch = new Com.Netvios.Proto.Inbound.AutoMatch();
			autoMatch.SongId = (string.IsNullOrEmpty(songId) ? "" : songId);
			this.BeatsaberSendMessage(new Com.Netvios.Proto.Inbound.BeatSaberBody
			{
				Type = DataType.AutoMatch,
				AutoMatch = autoMatch
			});
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x000196A0 File Offset: 0x000178A0
		public void AutoMatchResp(Com.Netvios.Proto.Outbound.AutoMatch autoMatchData)
		{
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x0001B57E File Offset: 0x0001977E
		public void AutoMatchNoticeResp(AutoMatchNotice autoMatchNoticeData)
		{
			Action<int, string, AutoMatchNotice> autoMatchNoticeEvent = this.AutoMatchNoticeEvent;
			if (autoMatchNoticeEvent == null)
			{
				return;
			}
			autoMatchNoticeEvent(0, "", autoMatchNoticeData);
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x0001B598 File Offset: 0x00019798
		public void FastMatch(string songId = null)
		{
			if (!this.isLogged)
			{
				return;
			}
			Com.Netvios.Proto.Inbound.FastMatch fastMatch = new Com.Netvios.Proto.Inbound.FastMatch();
			fastMatch.SongId = (string.IsNullOrEmpty(songId) ? "" : songId);
			this.BeatsaberSendMessage(new Com.Netvios.Proto.Inbound.BeatSaberBody
			{
				Type = DataType.FastMatch,
				FastMatch = fastMatch
			});
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x0001B5E8 File Offset: 0x000197E8
		public void FastMatchResp(Com.Netvios.Proto.Outbound.FastMatch fastMatchData)
		{
			if (fastMatchData.RoomOwner == this.player.playerId)
			{
				this.isHost = true;
			}
			this.isJoin = true;
			this.joinRoomId = fastMatchData.RoomId;
			this.roomStatus = Utils.ForamtRoomStatus(fastMatchData.Status);
			Action<int, string, Com.Netvios.Proto.Outbound.FastMatch> fastMatchEvent = this.FastMatchEvent;
			if (fastMatchEvent == null)
			{
				return;
			}
			fastMatchEvent(0, "", fastMatchData);
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x0001B64C File Offset: 0x0001984C
		public void ModifyNickname(string nickname)
		{
			if (!this.isLogged)
			{
				return;
			}
			Com.Netvios.Proto.Inbound.ModifyNickname modifyNickname = new Com.Netvios.Proto.Inbound.ModifyNickname();
			modifyNickname.Nickname = nickname;
			this.BeatsaberSendMessage(new Com.Netvios.Proto.Inbound.BeatSaberBody
			{
				Type = DataType.ModifyNickname,
				ModifyNickname = modifyNickname
			});
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x0001B68B File Offset: 0x0001988B
		private void ChangeNicknameResp(Com.Netvios.Proto.Outbound.ModifyNickname modifyNicknameData)
		{
			Action<int, string, Com.Netvios.Proto.Outbound.ModifyNickname> modifyNicknameEvent = this.ModifyNicknameEvent;
			if (modifyNicknameEvent == null)
			{
				return;
			}
			modifyNicknameEvent(0, "", modifyNicknameData);
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x0001B6A4 File Offset: 0x000198A4
		public void SendVoIPData(VoipFragment audio)
		{
			if (!this.isLogged)
			{
				return;
			}
			Com.Netvios.Proto.Inbound.RoomBroadcast roomBroadcast = new Com.Netvios.Proto.Inbound.RoomBroadcast();
			roomBroadcast.RoomId = this.joinRoomId;
			roomBroadcast.Type = RoomBroadcastDataType.VOIP.ToString();
			string text = JsonConvert.SerializeObject(audio);
			roomBroadcast.Content = text;
			this.BeatsaberSendMessage(new Com.Netvios.Proto.Inbound.BeatSaberBody
			{
				Type = DataType.RoomBroadcast,
				RoomBroadcast = roomBroadcast
			});
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x0001B70C File Offset: 0x0001990C
		public void SendGameStatus(int score = 0, int combo = 0, int maxCombo = 0, int noteWasCutCounter = 0, int noteMissedCounter = 0)
		{
			if (!PluginConfig.Instance.ExposesStatusEnabled || string.IsNullOrEmpty(PluginConfig.Instance.ExposesUrl))
			{
				return;
			}
			ServicePointManager.DefaultConnectionLimit = 50;
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(PluginConfig.Instance.ExposesUrl);
			httpWebRequest.ContentType = "application/json";
			httpWebRequest.Method = "POST";
			try
			{
				using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
				{
					string text = JsonConvert.SerializeObject(new GameStatus(this.joinRoomId, this.player.nickname, combo, maxCombo, noteWasCutCounter, noteMissedCounter, score));
					Logger.log.Info("== post game status json: " + text);
					streamWriter.Write(text);
					streamWriter.Flush();
				}
				using (StreamReader streamReader = new StreamReader(((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream()))
				{
					string text2 = streamReader.ReadToEnd();
					Logger.log.Info("== post game status result: " + text2);
				}
			}
			catch (Exception ex)
			{
				Logger.log.Error(string.Format("post game status result error: {0}", ex));
			}
		}

		// Token: 0x040002F7 RID: 759
		private string ip = "45.253.144.30";

		// Token: 0x040002F8 RID: 760
		private int port = 80;

		// Token: 0x040002F9 RID: 761
		private TcpClient client;

		// Token: 0x040002FA RID: 762
		private int clientSendTimeOut = 10;

		// Token: 0x040002FB RID: 763
		private int clientReceiveTimeout = 10;

		// Token: 0x040002FC RID: 764
		private int clientReceiveBufferSize = 1024;

		// Token: 0x040002FD RID: 765
		private float pingInterval = 5f;

		// Token: 0x040002FE RID: 766
		private int pingSequence;

		// Token: 0x040002FF RID: 767
		private Dictionary<int, int> pingSequenceDic = new Dictionary<int, int>();

		// Token: 0x04000300 RID: 768
		private float pingTimer;

		// Token: 0x04000301 RID: 769
		public int pingCounter;

		// Token: 0x04000302 RID: 770
		private byte[] receiveBuff;

		// Token: 0x04000303 RID: 771
		private byte[] dataBuff;

		// Token: 0x04000304 RID: 772
		private RoomUpdatedNotice updatedNoticeData;

		// Token: 0x0400031F RID: 799
		public Player player;

		// Token: 0x04000320 RID: 800
		private static Client instance;
	}
}
