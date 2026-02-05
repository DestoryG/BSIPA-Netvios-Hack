using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Inbound
{
	// Token: 0x0200003F RID: 63
	public sealed class ModifyPersonalCfg : IMessage<ModifyPersonalCfg>, IMessage, IEquatable<ModifyPersonalCfg>, IDeepCloneable<ModifyPersonalCfg>
	{
		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x0600051E RID: 1310 RVA: 0x00015911 File Offset: 0x00013B11
		[DebuggerNonUserCode]
		public static MessageParser<ModifyPersonalCfg> Parser
		{
			get
			{
				return ModifyPersonalCfg._parser;
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x0600051F RID: 1311 RVA: 0x00015918 File Offset: 0x00013B18
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberInboundMessageReflection.Descriptor.MessageTypes[15];
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06000520 RID: 1312 RVA: 0x0001592B File Offset: 0x00013B2B
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return ModifyPersonalCfg.Descriptor;
			}
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x00015932 File Offset: 0x00013B32
		[DebuggerNonUserCode]
		public ModifyPersonalCfg()
		{
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00015945 File Offset: 0x00013B45
		[DebuggerNonUserCode]
		public ModifyPersonalCfg(ModifyPersonalCfg other)
			: this()
		{
			this.roomId_ = other.roomId_;
			this.HeadphoneOn = other.HeadphoneOn;
			this.MicrophoneOn = other.MicrophoneOn;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x00015982 File Offset: 0x00013B82
		[DebuggerNonUserCode]
		public ModifyPersonalCfg Clone()
		{
			return new ModifyPersonalCfg(this);
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x0001598A File Offset: 0x00013B8A
		// (set) Token: 0x06000525 RID: 1317 RVA: 0x00015992 File Offset: 0x00013B92
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

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000526 RID: 1318 RVA: 0x000159A5 File Offset: 0x00013BA5
		// (set) Token: 0x06000527 RID: 1319 RVA: 0x000159AD File Offset: 0x00013BAD
		[DebuggerNonUserCode]
		public bool? HeadphoneOn
		{
			get
			{
				return this.headphoneOn_;
			}
			set
			{
				this.headphoneOn_ = value;
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x000159B6 File Offset: 0x00013BB6
		// (set) Token: 0x06000529 RID: 1321 RVA: 0x000159BE File Offset: 0x00013BBE
		[DebuggerNonUserCode]
		public bool? MicrophoneOn
		{
			get
			{
				return this.microphoneOn_;
			}
			set
			{
				this.microphoneOn_ = value;
			}
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x000159C7 File Offset: 0x00013BC7
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as ModifyPersonalCfg);
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x000159D8 File Offset: 0x00013BD8
		[DebuggerNonUserCode]
		public bool Equals(ModifyPersonalCfg other)
		{
			if (other == null)
			{
				return false;
			}
			if (other == this)
			{
				return true;
			}
			if (this.RoomId != other.RoomId)
			{
				return false;
			}
			bool? flag = this.HeadphoneOn;
			bool? flag2 = other.HeadphoneOn;
			if (!((flag.GetValueOrDefault() == flag2.GetValueOrDefault()) & (flag != null == (flag2 != null))))
			{
				return false;
			}
			flag2 = this.MicrophoneOn;
			flag = other.MicrophoneOn;
			return ((flag2.GetValueOrDefault() == flag.GetValueOrDefault()) & (flag2 != null == (flag != null))) && object.Equals(this._unknownFields, other._unknownFields);
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x00015A7C File Offset: 0x00013C7C
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.RoomId.Length != 0)
			{
				num ^= this.RoomId.GetHashCode();
			}
			if (this.headphoneOn_ != null)
			{
				num ^= this.HeadphoneOn.GetHashCode();
			}
			if (this.microphoneOn_ != null)
			{
				num ^= this.MicrophoneOn.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x00015B08 File Offset: 0x00013D08
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.RoomId.Length != 0)
			{
				output.WriteRawTag(10);
				output.WriteString(this.RoomId);
			}
			if (this.headphoneOn_ != null)
			{
				ModifyPersonalCfg._single_headphoneOn_codec.WriteTagAndValue(output, this.HeadphoneOn);
			}
			if (this.microphoneOn_ != null)
			{
				ModifyPersonalCfg._single_microphoneOn_codec.WriteTagAndValue(output, this.MicrophoneOn);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x00015B88 File Offset: 0x00013D88
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.RoomId.Length != 0)
			{
				num += 1 + CodedOutputStream.ComputeStringSize(this.RoomId);
			}
			if (this.headphoneOn_ != null)
			{
				num += ModifyPersonalCfg._single_headphoneOn_codec.CalculateSizeWithTag(this.HeadphoneOn);
			}
			if (this.microphoneOn_ != null)
			{
				num += ModifyPersonalCfg._single_microphoneOn_codec.CalculateSizeWithTag(this.MicrophoneOn);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x00015C0C File Offset: 0x00013E0C
		[DebuggerNonUserCode]
		public void MergeFrom(ModifyPersonalCfg other)
		{
			if (other == null)
			{
				return;
			}
			if (other.RoomId.Length != 0)
			{
				this.RoomId = other.RoomId;
			}
			if (other.headphoneOn_ != null)
			{
				if (this.headphoneOn_ != null)
				{
					bool? flag = other.HeadphoneOn;
					bool flag2 = false;
					if ((flag.GetValueOrDefault() == flag2) & (flag != null))
					{
						goto IL_0060;
					}
				}
				this.HeadphoneOn = other.HeadphoneOn;
			}
			IL_0060:
			if (other.microphoneOn_ != null)
			{
				if (this.microphoneOn_ != null)
				{
					bool? flag = other.MicrophoneOn;
					bool flag2 = false;
					if ((flag.GetValueOrDefault() == flag2) & (flag != null))
					{
						goto IL_00A3;
					}
				}
				this.MicrophoneOn = other.MicrophoneOn;
			}
			IL_00A3:
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x00015CD4 File Offset: 0x00013ED4
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
							bool? flag = ModifyPersonalCfg._single_microphoneOn_codec.Read(input);
							if (this.microphoneOn_ != null)
							{
								bool? flag2 = flag;
								bool flag3 = false;
								if ((flag2.GetValueOrDefault() == flag3) & (flag2 != null))
								{
									continue;
								}
							}
							this.MicrophoneOn = flag;
						}
					}
					else
					{
						bool? flag4 = ModifyPersonalCfg._single_headphoneOn_codec.Read(input);
						if (this.headphoneOn_ != null)
						{
							bool? flag2 = flag4;
							bool flag3 = false;
							if ((flag2.GetValueOrDefault() == flag3) & (flag2 != null))
							{
								continue;
							}
						}
						this.HeadphoneOn = flag4;
					}
				}
				else
				{
					this.RoomId = input.ReadString();
				}
			}
		}

		// Token: 0x04000267 RID: 615
		private static readonly MessageParser<ModifyPersonalCfg> _parser = new MessageParser<ModifyPersonalCfg>(() => new ModifyPersonalCfg());

		// Token: 0x04000268 RID: 616
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000269 RID: 617
		public const int RoomIdFieldNumber = 1;

		// Token: 0x0400026A RID: 618
		private string roomId_ = "";

		// Token: 0x0400026B RID: 619
		public const int HeadphoneOnFieldNumber = 2;

		// Token: 0x0400026C RID: 620
		private static readonly FieldCodec<bool?> _single_headphoneOn_codec = FieldCodec.ForStructWrapper<bool>(18U);

		// Token: 0x0400026D RID: 621
		private bool? headphoneOn_;

		// Token: 0x0400026E RID: 622
		public const int MicrophoneOnFieldNumber = 3;

		// Token: 0x0400026F RID: 623
		private static readonly FieldCodec<bool?> _single_microphoneOn_codec = FieldCodec.ForStructWrapper<bool>(26U);

		// Token: 0x04000270 RID: 624
		private bool? microphoneOn_;
	}
}
