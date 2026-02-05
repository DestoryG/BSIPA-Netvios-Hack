using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Inbound
{
	// Token: 0x02000047 RID: 71
	public sealed class PersonalCfg : IMessage<PersonalCfg>, IMessage, IEquatable<PersonalCfg>, IDeepCloneable<PersonalCfg>
	{
		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x060005EF RID: 1519 RVA: 0x00018696 File Offset: 0x00016896
		[DebuggerNonUserCode]
		public static MessageParser<PersonalCfg> Parser
		{
			get
			{
				return PersonalCfg._parser;
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x060005F0 RID: 1520 RVA: 0x0001869D File Offset: 0x0001689D
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberInboundMessageReflection.Descriptor.MessageTypes[23];
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x060005F1 RID: 1521 RVA: 0x000186B0 File Offset: 0x000168B0
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return PersonalCfg.Descriptor;
			}
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x0000370C File Offset: 0x0000190C
		[DebuggerNonUserCode]
		public PersonalCfg()
		{
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x000186B7 File Offset: 0x000168B7
		[DebuggerNonUserCode]
		public PersonalCfg(PersonalCfg other)
			: this()
		{
			this.headphoneOn_ = other.headphoneOn_;
			this.microphoneOn_ = other.microphoneOn_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x000186E8 File Offset: 0x000168E8
		[DebuggerNonUserCode]
		public PersonalCfg Clone()
		{
			return new PersonalCfg(this);
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x060005F5 RID: 1525 RVA: 0x000186F0 File Offset: 0x000168F0
		// (set) Token: 0x060005F6 RID: 1526 RVA: 0x000186F8 File Offset: 0x000168F8
		[DebuggerNonUserCode]
		public bool HeadphoneOn
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

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x060005F7 RID: 1527 RVA: 0x00018701 File Offset: 0x00016901
		// (set) Token: 0x060005F8 RID: 1528 RVA: 0x00018709 File Offset: 0x00016909
		[DebuggerNonUserCode]
		public bool MicrophoneOn
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

		// Token: 0x060005F9 RID: 1529 RVA: 0x00018712 File Offset: 0x00016912
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as PersonalCfg);
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x00018720 File Offset: 0x00016920
		[DebuggerNonUserCode]
		public bool Equals(PersonalCfg other)
		{
			return other != null && (other == this || (this.HeadphoneOn == other.HeadphoneOn && this.MicrophoneOn == other.MicrophoneOn && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x00018760 File Offset: 0x00016960
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.HeadphoneOn)
			{
				num ^= this.HeadphoneOn.GetHashCode();
			}
			if (this.MicrophoneOn)
			{
				num ^= this.MicrophoneOn.GetHashCode();
			}
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x000187B8 File Offset: 0x000169B8
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.HeadphoneOn)
			{
				output.WriteRawTag(8);
				output.WriteBool(this.HeadphoneOn);
			}
			if (this.MicrophoneOn)
			{
				output.WriteRawTag(16);
				output.WriteBool(this.MicrophoneOn);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00018810 File Offset: 0x00016A10
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.HeadphoneOn)
			{
				num += 2;
			}
			if (this.MicrophoneOn)
			{
				num += 2;
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x00018850 File Offset: 0x00016A50
		[DebuggerNonUserCode]
		public void MergeFrom(PersonalCfg other)
		{
			if (other == null)
			{
				return;
			}
			if (other.HeadphoneOn)
			{
				this.HeadphoneOn = other.HeadphoneOn;
			}
			if (other.MicrophoneOn)
			{
				this.MicrophoneOn = other.MicrophoneOn;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x000188A0 File Offset: 0x00016AA0
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 8U)
				{
					if (num != 16U)
					{
						this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
					}
					else
					{
						this.MicrophoneOn = input.ReadBool();
					}
				}
				else
				{
					this.HeadphoneOn = input.ReadBool();
				}
			}
		}

		// Token: 0x040002D5 RID: 725
		private static readonly MessageParser<PersonalCfg> _parser = new MessageParser<PersonalCfg>(() => new PersonalCfg());

		// Token: 0x040002D6 RID: 726
		private UnknownFieldSet _unknownFields;

		// Token: 0x040002D7 RID: 727
		public const int HeadphoneOnFieldNumber = 1;

		// Token: 0x040002D8 RID: 728
		private bool headphoneOn_;

		// Token: 0x040002D9 RID: 729
		public const int MicrophoneOnFieldNumber = 2;

		// Token: 0x040002DA RID: 730
		private bool microphoneOn_;
	}
}
