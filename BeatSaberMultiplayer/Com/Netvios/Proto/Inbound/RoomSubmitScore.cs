using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Inbound
{
	// Token: 0x02000042 RID: 66
	public sealed class RoomSubmitScore : IMessage<RoomSubmitScore>, IMessage, IEquatable<RoomSubmitScore>, IDeepCloneable<RoomSubmitScore>
	{
		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000565 RID: 1381 RVA: 0x00016884 File Offset: 0x00014A84
		[DebuggerNonUserCode]
		public static MessageParser<RoomSubmitScore> Parser
		{
			get
			{
				return RoomSubmitScore._parser;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x0001688B File Offset: 0x00014A8B
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberInboundMessageReflection.Descriptor.MessageTypes[18];
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000567 RID: 1383 RVA: 0x0001689E File Offset: 0x00014A9E
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return RoomSubmitScore.Descriptor;
			}
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x000168A8 File Offset: 0x00014AA8
		[DebuggerNonUserCode]
		public RoomSubmitScore()
		{
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00016908 File Offset: 0x00014B08
		[DebuggerNonUserCode]
		public RoomSubmitScore(RoomSubmitScore other)
			: this()
		{
			this.roomId_ = other.roomId_;
			this.appChannel_ = other.appChannel_;
			this.levelId_ = other.levelId_;
			this.difficulty_ = other.difficulty_;
			this.songDidFinish_ = other.songDidFinish_;
			this.levelBpm_ = other.levelBpm_;
			this.rank_ = other.rank_;
			this.maxCombo_ = other.maxCombo_;
			this.modifiedScore_ = other.modifiedScore_;
			this.goodCutsCount_ = other.goodCutsCount_;
			this.badCutsCount_ = other.badCutsCount_;
			this.missedCount_ = other.missedCount_;
			this.endSongTime_ = other.endSongTime_;
			this.songDuration_ = other.songDuration_;
			this.leftHandMovementDistance_ = other.leftHandMovementDistance_;
			this.rightHandMovementDistance_ = other.rightHandMovementDistance_;
			this.leftSaberMovementDistance_ = other.leftSaberMovementDistance_;
			this.rightSaberMovementDistance_ = other.rightSaberMovementDistance_;
			this.okCount_ = other.okCount_;
			this.notGoodCount_ = other.notGoodCount_;
			this.mode_ = other.mode_;
			this.rawScore_ = other.rawScore_;
			this.levelEndStateType_ = other.levelEndStateType_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00016A40 File Offset: 0x00014C40
		[DebuggerNonUserCode]
		public RoomSubmitScore Clone()
		{
			return new RoomSubmitScore(this);
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x0600056B RID: 1387 RVA: 0x00016A48 File Offset: 0x00014C48
		// (set) Token: 0x0600056C RID: 1388 RVA: 0x00016A50 File Offset: 0x00014C50
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

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x0600056D RID: 1389 RVA: 0x00016A63 File Offset: 0x00014C63
		// (set) Token: 0x0600056E RID: 1390 RVA: 0x00016A6B File Offset: 0x00014C6B
		[DebuggerNonUserCode]
		public string AppChannel
		{
			get
			{
				return this.appChannel_;
			}
			set
			{
				this.appChannel_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x0600056F RID: 1391 RVA: 0x00016A7E File Offset: 0x00014C7E
		// (set) Token: 0x06000570 RID: 1392 RVA: 0x00016A86 File Offset: 0x00014C86
		[DebuggerNonUserCode]
		public string LevelId
		{
			get
			{
				return this.levelId_;
			}
			set
			{
				this.levelId_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000571 RID: 1393 RVA: 0x00016A99 File Offset: 0x00014C99
		// (set) Token: 0x06000572 RID: 1394 RVA: 0x00016AA1 File Offset: 0x00014CA1
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

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000573 RID: 1395 RVA: 0x00016AB4 File Offset: 0x00014CB4
		// (set) Token: 0x06000574 RID: 1396 RVA: 0x00016ABC File Offset: 0x00014CBC
		[DebuggerNonUserCode]
		public bool SongDidFinish
		{
			get
			{
				return this.songDidFinish_;
			}
			set
			{
				this.songDidFinish_ = value;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000575 RID: 1397 RVA: 0x00016AC5 File Offset: 0x00014CC5
		// (set) Token: 0x06000576 RID: 1398 RVA: 0x00016ACD File Offset: 0x00014CCD
		[DebuggerNonUserCode]
		public int LevelBpm
		{
			get
			{
				return this.levelBpm_;
			}
			set
			{
				this.levelBpm_ = value;
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000577 RID: 1399 RVA: 0x00016AD6 File Offset: 0x00014CD6
		// (set) Token: 0x06000578 RID: 1400 RVA: 0x00016ADE File Offset: 0x00014CDE
		[DebuggerNonUserCode]
		public string Rank
		{
			get
			{
				return this.rank_;
			}
			set
			{
				this.rank_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000579 RID: 1401 RVA: 0x00016AF1 File Offset: 0x00014CF1
		// (set) Token: 0x0600057A RID: 1402 RVA: 0x00016AF9 File Offset: 0x00014CF9
		[DebuggerNonUserCode]
		public int MaxCombo
		{
			get
			{
				return this.maxCombo_;
			}
			set
			{
				this.maxCombo_ = value;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x0600057B RID: 1403 RVA: 0x00016B02 File Offset: 0x00014D02
		// (set) Token: 0x0600057C RID: 1404 RVA: 0x00016B0A File Offset: 0x00014D0A
		[DebuggerNonUserCode]
		public int ModifiedScore
		{
			get
			{
				return this.modifiedScore_;
			}
			set
			{
				this.modifiedScore_ = value;
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x0600057D RID: 1405 RVA: 0x00016B13 File Offset: 0x00014D13
		// (set) Token: 0x0600057E RID: 1406 RVA: 0x00016B1B File Offset: 0x00014D1B
		[DebuggerNonUserCode]
		public int GoodCutsCount
		{
			get
			{
				return this.goodCutsCount_;
			}
			set
			{
				this.goodCutsCount_ = value;
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x0600057F RID: 1407 RVA: 0x00016B24 File Offset: 0x00014D24
		// (set) Token: 0x06000580 RID: 1408 RVA: 0x00016B2C File Offset: 0x00014D2C
		[DebuggerNonUserCode]
		public int BadCutsCount
		{
			get
			{
				return this.badCutsCount_;
			}
			set
			{
				this.badCutsCount_ = value;
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000581 RID: 1409 RVA: 0x00016B35 File Offset: 0x00014D35
		// (set) Token: 0x06000582 RID: 1410 RVA: 0x00016B3D File Offset: 0x00014D3D
		[DebuggerNonUserCode]
		public int MissedCount
		{
			get
			{
				return this.missedCount_;
			}
			set
			{
				this.missedCount_ = value;
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x00016B46 File Offset: 0x00014D46
		// (set) Token: 0x06000584 RID: 1412 RVA: 0x00016B4E File Offset: 0x00014D4E
		[DebuggerNonUserCode]
		public int EndSongTime
		{
			get
			{
				return this.endSongTime_;
			}
			set
			{
				this.endSongTime_ = value;
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000585 RID: 1413 RVA: 0x00016B57 File Offset: 0x00014D57
		// (set) Token: 0x06000586 RID: 1414 RVA: 0x00016B5F File Offset: 0x00014D5F
		[DebuggerNonUserCode]
		public int SongDuration
		{
			get
			{
				return this.songDuration_;
			}
			set
			{
				this.songDuration_ = value;
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000587 RID: 1415 RVA: 0x00016B68 File Offset: 0x00014D68
		// (set) Token: 0x06000588 RID: 1416 RVA: 0x00016B70 File Offset: 0x00014D70
		[DebuggerNonUserCode]
		public int LeftHandMovementDistance
		{
			get
			{
				return this.leftHandMovementDistance_;
			}
			set
			{
				this.leftHandMovementDistance_ = value;
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000589 RID: 1417 RVA: 0x00016B79 File Offset: 0x00014D79
		// (set) Token: 0x0600058A RID: 1418 RVA: 0x00016B81 File Offset: 0x00014D81
		[DebuggerNonUserCode]
		public int RightHandMovementDistance
		{
			get
			{
				return this.rightHandMovementDistance_;
			}
			set
			{
				this.rightHandMovementDistance_ = value;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x0600058B RID: 1419 RVA: 0x00016B8A File Offset: 0x00014D8A
		// (set) Token: 0x0600058C RID: 1420 RVA: 0x00016B92 File Offset: 0x00014D92
		[DebuggerNonUserCode]
		public int LeftSaberMovementDistance
		{
			get
			{
				return this.leftSaberMovementDistance_;
			}
			set
			{
				this.leftSaberMovementDistance_ = value;
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x0600058D RID: 1421 RVA: 0x00016B9B File Offset: 0x00014D9B
		// (set) Token: 0x0600058E RID: 1422 RVA: 0x00016BA3 File Offset: 0x00014DA3
		[DebuggerNonUserCode]
		public int RightSaberMovementDistance
		{
			get
			{
				return this.rightSaberMovementDistance_;
			}
			set
			{
				this.rightSaberMovementDistance_ = value;
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x0600058F RID: 1423 RVA: 0x00016BAC File Offset: 0x00014DAC
		// (set) Token: 0x06000590 RID: 1424 RVA: 0x00016BB4 File Offset: 0x00014DB4
		[DebuggerNonUserCode]
		public int OkCount
		{
			get
			{
				return this.okCount_;
			}
			set
			{
				this.okCount_ = value;
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000591 RID: 1425 RVA: 0x00016BBD File Offset: 0x00014DBD
		// (set) Token: 0x06000592 RID: 1426 RVA: 0x00016BC5 File Offset: 0x00014DC5
		[DebuggerNonUserCode]
		public int NotGoodCount
		{
			get
			{
				return this.notGoodCount_;
			}
			set
			{
				this.notGoodCount_ = value;
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000593 RID: 1427 RVA: 0x00016BCE File Offset: 0x00014DCE
		// (set) Token: 0x06000594 RID: 1428 RVA: 0x00016BD6 File Offset: 0x00014DD6
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

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000595 RID: 1429 RVA: 0x00016BE9 File Offset: 0x00014DE9
		// (set) Token: 0x06000596 RID: 1430 RVA: 0x00016BF1 File Offset: 0x00014DF1
		[DebuggerNonUserCode]
		public int RawScore
		{
			get
			{
				return this.rawScore_;
			}
			set
			{
				this.rawScore_ = value;
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000597 RID: 1431 RVA: 0x00016BFA File Offset: 0x00014DFA
		// (set) Token: 0x06000598 RID: 1432 RVA: 0x00016C02 File Offset: 0x00014E02
		[DebuggerNonUserCode]
		public string LevelEndStateType
		{
			get
			{
				return this.levelEndStateType_;
			}
			set
			{
				this.levelEndStateType_ = ProtoPreconditions.CheckNotNull<string>(value, "value");
			}
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x00016C15 File Offset: 0x00014E15
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as RoomSubmitScore);
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x00016C24 File Offset: 0x00014E24
		[DebuggerNonUserCode]
		public bool Equals(RoomSubmitScore other)
		{
			return other != null && (other == this || (!(this.RoomId != other.RoomId) && !(this.AppChannel != other.AppChannel) && !(this.LevelId != other.LevelId) && !(this.Difficulty != other.Difficulty) && this.SongDidFinish == other.SongDidFinish && this.LevelBpm == other.LevelBpm && !(this.Rank != other.Rank) && this.MaxCombo == other.MaxCombo && this.ModifiedScore == other.ModifiedScore && this.GoodCutsCount == other.GoodCutsCount && this.BadCutsCount == other.BadCutsCount && this.MissedCount == other.MissedCount && this.EndSongTime == other.EndSongTime && this.SongDuration == other.SongDuration && this.LeftHandMovementDistance == other.LeftHandMovementDistance && this.RightHandMovementDistance == other.RightHandMovementDistance && this.LeftSaberMovementDistance == other.LeftSaberMovementDistance && this.RightSaberMovementDistance == other.RightSaberMovementDistance && this.OkCount == other.OkCount && this.NotGoodCount == other.NotGoodCount && !(this.Mode != other.Mode) && this.RawScore == other.RawScore && !(this.LevelEndStateType != other.LevelEndStateType) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x00016DE0 File Offset: 0x00014FE0
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.RoomId.Length != 0)
			{
				num ^= this.RoomId.GetHashCode();
			}
			if (this.AppChannel.Length != 0)
			{
				num ^= this.AppChannel.GetHashCode();
			}
			if (this.LevelId.Length != 0)
			{
				num ^= this.LevelId.GetHashCode();
			}
			if (this.Difficulty.Length != 0)
			{
				num ^= this.Difficulty.GetHashCode();
			}
			if (this.SongDidFinish)
			{
				num ^= this.SongDidFinish.GetHashCode();
			}
			if (this.LevelBpm != 0)
			{
				num ^= this.LevelBpm.GetHashCode();
			}
			if (this.Rank.Length != 0)
			{
				num ^= this.Rank.GetHashCode();
			}
			if (this.MaxCombo != 0)
			{
				num ^= this.MaxCombo.GetHashCode();
			}
			if (this.ModifiedScore != 0)
			{
				num ^= this.ModifiedScore.GetHashCode();
			}
			if (this.GoodCutsCount != 0)
			{
				num ^= this.GoodCutsCount.GetHashCode();
			}
			if (this.BadCutsCount != 0)
			{
				num ^= this.BadCutsCount.GetHashCode();
			}
			if (this.MissedCount != 0)
			{
				num ^= this.MissedCount.GetHashCode();
			}
			if (this.EndSongTime != 0)
			{
				num ^= this.EndSongTime.GetHashCode();
			}
			if (this.SongDuration != 0)
			{
				num ^= this.SongDuration.GetHashCode();
			}
			if (this.LeftHandMovementDistance != 0)
			{
				num ^= this.LeftHandMovementDistance.GetHashCode();
			}
			if (this.RightHandMovementDistance != 0)
			{
				num ^= this.RightHandMovementDistance.GetHashCode();
			}
			if (this.LeftSaberMovementDistance != 0)
			{
				num ^= this.LeftSaberMovementDistance.GetHashCode();
			}
			if (this.RightSaberMovementDistance != 0)
			{
				num ^= this.RightSaberMovementDistance.GetHashCode();
			}
			if (this.OkCount != 0)
			{
				num ^= this.OkCount.GetHashCode();
			}
			if (this.NotGoodCount != 0)
			{
				num ^= this.NotGoodCount.GetHashCode();
			}
			if (this.Mode.Length != 0)
			{
				num ^= this.Mode.GetHashCode();
			}
			if (this.RawScore != 0)
			{
				num ^= this.RawScore.GetHashCode();
			}
			if (this.LevelEndStateType.Length != 0)
			{
				num ^= this.LevelEndStateType.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x00017054 File Offset: 0x00015254
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.RoomId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.RoomId);
			}
			if (this.AppChannel.Length != 0)
			{
				output.WriteRawTag(18);
				output.WriteString(this.AppChannel);
			}
			if (this.LevelId.Length != 0)
			{
				output.WriteRawTag(26);
				output.WriteString(this.LevelId);
			}
			if (this.Difficulty.Length != 0)
			{
				output.WriteRawTag(34);
				output.WriteString(this.Difficulty);
			}
			if (this.SongDidFinish)
			{
				output.WriteRawTag(40);
				output.WriteBool(this.SongDidFinish);
			}
			if (this.LevelBpm != 0)
			{
				output.WriteRawTag(48);
				output.WriteInt32(this.LevelBpm);
			}
			if (this.Rank.Length != 0)
			{
				output.WriteRawTag(58);
				output.WriteString(this.Rank);
			}
			if (this.MaxCombo != 0)
			{
				output.WriteRawTag(64);
				output.WriteInt32(this.MaxCombo);
			}
			if (this.ModifiedScore != 0)
			{
				output.WriteRawTag(72);
				output.WriteInt32(this.ModifiedScore);
			}
			if (this.GoodCutsCount != 0)
			{
				output.WriteRawTag(80);
				output.WriteInt32(this.GoodCutsCount);
			}
			if (this.BadCutsCount != 0)
			{
				output.WriteRawTag(88);
				output.WriteInt32(this.BadCutsCount);
			}
			if (this.MissedCount != 0)
			{
				output.WriteRawTag(96);
				output.WriteInt32(this.MissedCount);
			}
			if (this.EndSongTime != 0)
			{
				output.WriteRawTag(104);
				output.WriteInt32(this.EndSongTime);
			}
			if (this.SongDuration != 0)
			{
				output.WriteRawTag(112);
				output.WriteInt32(this.SongDuration);
			}
			if (this.LeftHandMovementDistance != 0)
			{
				output.WriteRawTag(120);
				output.WriteInt32(this.LeftHandMovementDistance);
			}
			if (this.RightHandMovementDistance != 0)
			{
				output.WriteRawTag(128, 1);
				output.WriteInt32(this.RightHandMovementDistance);
			}
			if (this.LeftSaberMovementDistance != 0)
			{
				output.WriteRawTag(136, 1);
				output.WriteInt32(this.LeftSaberMovementDistance);
			}
			if (this.RightSaberMovementDistance != 0)
			{
				output.WriteRawTag(144, 1);
				output.WriteInt32(this.RightSaberMovementDistance);
			}
			if (this.OkCount != 0)
			{
				output.WriteRawTag(152, 1);
				output.WriteInt32(this.OkCount);
			}
			if (this.NotGoodCount != 0)
			{
				output.WriteRawTag(160, 1);
				output.WriteInt32(this.NotGoodCount);
			}
			if (this.Mode.Length != 0)
			{
				output.WriteRawTag(170, 1);
				output.WriteString(this.Mode);
			}
			if (this.RawScore != 0)
			{
				output.WriteRawTag(240, 1);
				output.WriteInt32(this.RawScore);
			}
			if (this.LevelEndStateType.Length != 0)
			{
				output.WriteRawTag(250, 1);
				output.WriteString(this.LevelEndStateType);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x0600059E RID: 1438 RVA: 0x0001733C File Offset: 0x0001553C
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.RoomId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.RoomId);
			}
			if (this.AppChannel.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.AppChannel);
			}
			if (this.LevelId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.LevelId);
			}
			if (this.Difficulty.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Difficulty);
			}
			if (this.SongDidFinish)
			{
				num += 2;
			}
			if (this.LevelBpm != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.LevelBpm);
			}
			if (this.Rank.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.Rank);
			}
			if (this.MaxCombo != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.MaxCombo);
			}
			if (this.ModifiedScore != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.ModifiedScore);
			}
			if (this.GoodCutsCount != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.GoodCutsCount);
			}
			if (this.BadCutsCount != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.BadCutsCount);
			}
			if (this.MissedCount != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.MissedCount);
			}
			if (this.EndSongTime != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.EndSongTime);
			}
			if (this.SongDuration != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.SongDuration);
			}
			if (this.LeftHandMovementDistance != 0)
			{
				num += 1 + CodedOutputStream.ComputeInt32Size(this.LeftHandMovementDistance);
			}
			if (this.RightHandMovementDistance != 0)
			{
				num += 2 + CodedOutputStream.ComputeInt32Size(this.RightHandMovementDistance);
			}
			if (this.LeftSaberMovementDistance != 0)
			{
				num += 2 + CodedOutputStream.ComputeInt32Size(this.LeftSaberMovementDistance);
			}
			if (this.RightSaberMovementDistance != 0)
			{
				num += 2 + CodedOutputStream.ComputeInt32Size(this.RightSaberMovementDistance);
			}
			if (this.OkCount != 0)
			{
				num += 2 + CodedOutputStream.ComputeInt32Size(this.OkCount);
			}
			if (this.NotGoodCount != 0)
			{
				num += 2 + CodedOutputStream.ComputeInt32Size(this.NotGoodCount);
			}
			if (this.Mode.Length != 0)
			{
				num += 2 + CodedOutputStream.ComputeStringSize(this.Mode);
			}
			if (this.RawScore != 0)
			{
				num += 2 + CodedOutputStream.ComputeInt32Size(this.RawScore);
			}
			if (this.LevelEndStateType.Length != 0)
			{
				num += 2 + CodedOutputStream.ComputeStringSize(this.LevelEndStateType);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x0600059F RID: 1439 RVA: 0x000175A4 File Offset: 0x000157A4
		[DebuggerNonUserCode]
		public void MergeFrom(RoomSubmitScore other)
		{
			if (other == null)
			{
				return;
			}
			if (other.RoomId.Length != 0)
			{
				this.RoomId = other.RoomId;
			}
			if (other.AppChannel.Length != 0)
			{
				this.AppChannel = other.AppChannel;
			}
			if (other.LevelId.Length != 0)
			{
				this.LevelId = other.LevelId;
			}
			if (other.Difficulty.Length != 0)
			{
				this.Difficulty = other.Difficulty;
			}
			if (other.SongDidFinish)
			{
				this.SongDidFinish = other.SongDidFinish;
			}
			if (other.LevelBpm != 0)
			{
				this.LevelBpm = other.LevelBpm;
			}
			if (other.Rank.Length != 0)
			{
				this.Rank = other.Rank;
			}
			if (other.MaxCombo != 0)
			{
				this.MaxCombo = other.MaxCombo;
			}
			if (other.ModifiedScore != 0)
			{
				this.ModifiedScore = other.ModifiedScore;
			}
			if (other.GoodCutsCount != 0)
			{
				this.GoodCutsCount = other.GoodCutsCount;
			}
			if (other.BadCutsCount != 0)
			{
				this.BadCutsCount = other.BadCutsCount;
			}
			if (other.MissedCount != 0)
			{
				this.MissedCount = other.MissedCount;
			}
			if (other.EndSongTime != 0)
			{
				this.EndSongTime = other.EndSongTime;
			}
			if (other.SongDuration != 0)
			{
				this.SongDuration = other.SongDuration;
			}
			if (other.LeftHandMovementDistance != 0)
			{
				this.LeftHandMovementDistance = other.LeftHandMovementDistance;
			}
			if (other.RightHandMovementDistance != 0)
			{
				this.RightHandMovementDistance = other.RightHandMovementDistance;
			}
			if (other.LeftSaberMovementDistance != 0)
			{
				this.LeftSaberMovementDistance = other.LeftSaberMovementDistance;
			}
			if (other.RightSaberMovementDistance != 0)
			{
				this.RightSaberMovementDistance = other.RightSaberMovementDistance;
			}
			if (other.OkCount != 0)
			{
				this.OkCount = other.OkCount;
			}
			if (other.NotGoodCount != 0)
			{
				this.NotGoodCount = other.NotGoodCount;
			}
			if (other.Mode.Length != 0)
			{
				this.Mode = other.Mode;
			}
			if (other.RawScore != 0)
			{
				this.RawScore = other.RawScore;
			}
			if (other.LevelEndStateType.Length != 0)
			{
				this.LevelEndStateType = other.LevelEndStateType;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x000177BC File Offset: 0x000159BC
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num <= 88U)
				{
					if (num <= 40U)
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
								this.AppChannel = input.ReadString();
								continue;
							}
						}
						else
						{
							if (num == 26U)
							{
								this.LevelId = input.ReadString();
								continue;
							}
							if (num == 34U)
							{
								this.Difficulty = input.ReadString();
								continue;
							}
							if (num == 40U)
							{
								this.SongDidFinish = input.ReadBool();
								continue;
							}
						}
					}
					else if (num <= 64U)
					{
						if (num == 48U)
						{
							this.LevelBpm = input.ReadInt32();
							continue;
						}
						if (num == 58U)
						{
							this.Rank = input.ReadString();
							continue;
						}
						if (num == 64U)
						{
							this.MaxCombo = input.ReadInt32();
							continue;
						}
					}
					else
					{
						if (num == 72U)
						{
							this.ModifiedScore = input.ReadInt32();
							continue;
						}
						if (num == 80U)
						{
							this.GoodCutsCount = input.ReadInt32();
							continue;
						}
						if (num == 88U)
						{
							this.BadCutsCount = input.ReadInt32();
							continue;
						}
					}
				}
				else if (num <= 136U)
				{
					if (num <= 112U)
					{
						if (num == 96U)
						{
							this.MissedCount = input.ReadInt32();
							continue;
						}
						if (num == 104U)
						{
							this.EndSongTime = input.ReadInt32();
							continue;
						}
						if (num == 112U)
						{
							this.SongDuration = input.ReadInt32();
							continue;
						}
					}
					else
					{
						if (num == 120U)
						{
							this.LeftHandMovementDistance = input.ReadInt32();
							continue;
						}
						if (num == 128U)
						{
							this.RightHandMovementDistance = input.ReadInt32();
							continue;
						}
						if (num == 136U)
						{
							this.LeftSaberMovementDistance = input.ReadInt32();
							continue;
						}
					}
				}
				else if (num <= 160U)
				{
					if (num == 144U)
					{
						this.RightSaberMovementDistance = input.ReadInt32();
						continue;
					}
					if (num == 152U)
					{
						this.OkCount = input.ReadInt32();
						continue;
					}
					if (num == 160U)
					{
						this.NotGoodCount = input.ReadInt32();
						continue;
					}
				}
				else
				{
					if (num == 170U)
					{
						this.Mode = input.ReadString();
						continue;
					}
					if (num == 240U)
					{
						this.RawScore = input.ReadInt32();
						continue;
					}
					if (num == 250U)
					{
						this.LevelEndStateType = input.ReadString();
						continue;
					}
				}
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x0400028A RID: 650
		private static readonly MessageParser<RoomSubmitScore> _parser = new MessageParser<RoomSubmitScore>(() => new RoomSubmitScore());

		// Token: 0x0400028B RID: 651
		private UnknownFieldSet _unknownFields;

		// Token: 0x0400028C RID: 652
		public const int RoomIdFieldNumber = 1;

		// Token: 0x0400028D RID: 653
		private string roomId_ = "";

		// Token: 0x0400028E RID: 654
		public const int AppChannelFieldNumber = 2;

		// Token: 0x0400028F RID: 655
		private string appChannel_ = "";

		// Token: 0x04000290 RID: 656
		public const int LevelIdFieldNumber = 3;

		// Token: 0x04000291 RID: 657
		private string levelId_ = "";

		// Token: 0x04000292 RID: 658
		public const int DifficultyFieldNumber = 4;

		// Token: 0x04000293 RID: 659
		private string difficulty_ = "";

		// Token: 0x04000294 RID: 660
		public const int SongDidFinishFieldNumber = 5;

		// Token: 0x04000295 RID: 661
		private bool songDidFinish_;

		// Token: 0x04000296 RID: 662
		public const int LevelBpmFieldNumber = 6;

		// Token: 0x04000297 RID: 663
		private int levelBpm_;

		// Token: 0x04000298 RID: 664
		public const int RankFieldNumber = 7;

		// Token: 0x04000299 RID: 665
		private string rank_ = "";

		// Token: 0x0400029A RID: 666
		public const int MaxComboFieldNumber = 8;

		// Token: 0x0400029B RID: 667
		private int maxCombo_;

		// Token: 0x0400029C RID: 668
		public const int ModifiedScoreFieldNumber = 9;

		// Token: 0x0400029D RID: 669
		private int modifiedScore_;

		// Token: 0x0400029E RID: 670
		public const int GoodCutsCountFieldNumber = 10;

		// Token: 0x0400029F RID: 671
		private int goodCutsCount_;

		// Token: 0x040002A0 RID: 672
		public const int BadCutsCountFieldNumber = 11;

		// Token: 0x040002A1 RID: 673
		private int badCutsCount_;

		// Token: 0x040002A2 RID: 674
		public const int MissedCountFieldNumber = 12;

		// Token: 0x040002A3 RID: 675
		private int missedCount_;

		// Token: 0x040002A4 RID: 676
		public const int EndSongTimeFieldNumber = 13;

		// Token: 0x040002A5 RID: 677
		private int endSongTime_;

		// Token: 0x040002A6 RID: 678
		public const int SongDurationFieldNumber = 14;

		// Token: 0x040002A7 RID: 679
		private int songDuration_;

		// Token: 0x040002A8 RID: 680
		public const int LeftHandMovementDistanceFieldNumber = 15;

		// Token: 0x040002A9 RID: 681
		private int leftHandMovementDistance_;

		// Token: 0x040002AA RID: 682
		public const int RightHandMovementDistanceFieldNumber = 16;

		// Token: 0x040002AB RID: 683
		private int rightHandMovementDistance_;

		// Token: 0x040002AC RID: 684
		public const int LeftSaberMovementDistanceFieldNumber = 17;

		// Token: 0x040002AD RID: 685
		private int leftSaberMovementDistance_;

		// Token: 0x040002AE RID: 686
		public const int RightSaberMovementDistanceFieldNumber = 18;

		// Token: 0x040002AF RID: 687
		private int rightSaberMovementDistance_;

		// Token: 0x040002B0 RID: 688
		public const int OkCountFieldNumber = 19;

		// Token: 0x040002B1 RID: 689
		private int okCount_;

		// Token: 0x040002B2 RID: 690
		public const int NotGoodCountFieldNumber = 20;

		// Token: 0x040002B3 RID: 691
		private int notGoodCount_;

		// Token: 0x040002B4 RID: 692
		public const int ModeFieldNumber = 21;

		// Token: 0x040002B5 RID: 693
		private string mode_ = "";

		// Token: 0x040002B6 RID: 694
		public const int RawScoreFieldNumber = 30;

		// Token: 0x040002B7 RID: 695
		private int rawScore_;

		// Token: 0x040002B8 RID: 696
		public const int LevelEndStateTypeFieldNumber = 31;

		// Token: 0x040002B9 RID: 697
		private string levelEndStateType_ = "";
	}
}
