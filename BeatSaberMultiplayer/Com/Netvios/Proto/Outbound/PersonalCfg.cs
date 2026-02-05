using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Outbound
{
	// Token: 0x02000029 RID: 41
	public sealed class PersonalCfg : IMessage<PersonalCfg>, IMessage, IEquatable<PersonalCfg>, IDeepCloneable<PersonalCfg>
	{
		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000360 RID: 864 RVA: 0x0000F80D File Offset: 0x0000DA0D
		[DebuggerNonUserCode]
		public static MessageParser<PersonalCfg> Parser
		{
			get
			{
				return PersonalCfg._parser;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000361 RID: 865 RVA: 0x0000F814 File Offset: 0x0000DA14
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberOutboundMessageReflection.Descriptor.MessageTypes[33];
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000362 RID: 866 RVA: 0x0000F827 File Offset: 0x0000DA27
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return PersonalCfg.Descriptor;
			}
		}

		// Token: 0x06000363 RID: 867 RVA: 0x0000370C File Offset: 0x0000190C
		[DebuggerNonUserCode]
		public PersonalCfg()
		{
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0000F82E File Offset: 0x0000DA2E
		[DebuggerNonUserCode]
		public PersonalCfg(PersonalCfg other)
			: this()
		{
			this.headphoneOn_ = other.headphoneOn_;
			this.microphoneOn_ = other.microphoneOn_;
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x0000F85F File Offset: 0x0000DA5F
		[DebuggerNonUserCode]
		public PersonalCfg Clone()
		{
			return new PersonalCfg(this);
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06000366 RID: 870 RVA: 0x0000F867 File Offset: 0x0000DA67
		// (set) Token: 0x06000367 RID: 871 RVA: 0x0000F86F File Offset: 0x0000DA6F
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

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06000368 RID: 872 RVA: 0x0000F878 File Offset: 0x0000DA78
		// (set) Token: 0x06000369 RID: 873 RVA: 0x0000F880 File Offset: 0x0000DA80
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

		// Token: 0x0600036A RID: 874 RVA: 0x0000F889 File Offset: 0x0000DA89
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as PersonalCfg);
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0000F897 File Offset: 0x0000DA97
		[DebuggerNonUserCode]
		public bool Equals(PersonalCfg other)
		{
			return other != null && (other == this || (this.HeadphoneOn == other.HeadphoneOn && this.MicrophoneOn == other.MicrophoneOn && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x0600036C RID: 876 RVA: 0x0000F8D8 File Offset: 0x0000DAD8
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

		// Token: 0x0600036D RID: 877 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000F930 File Offset: 0x0000DB30
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

		// Token: 0x0600036F RID: 879 RVA: 0x0000F988 File Offset: 0x0000DB88
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

		// Token: 0x06000370 RID: 880 RVA: 0x0000F9C8 File Offset: 0x0000DBC8
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

		// Token: 0x06000371 RID: 881 RVA: 0x0000FA18 File Offset: 0x0000DC18
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

		// Token: 0x040001C6 RID: 454
		private static readonly MessageParser<PersonalCfg> _parser = new MessageParser<PersonalCfg>(() => new PersonalCfg());

		// Token: 0x040001C7 RID: 455
		private UnknownFieldSet _unknownFields;

		// Token: 0x040001C8 RID: 456
		public const int HeadphoneOnFieldNumber = 1;

		// Token: 0x040001C9 RID: 457
		private bool headphoneOn_;

		// Token: 0x040001CA RID: 458
		public const int MicrophoneOnFieldNumber = 2;

		// Token: 0x040001CB RID: 459
		private bool microphoneOn_;
	}
}
