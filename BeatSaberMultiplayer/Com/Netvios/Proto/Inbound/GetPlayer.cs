using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Inbound
{
	// Token: 0x02000035 RID: 53
	public sealed class GetPlayer : IMessage<GetPlayer>, IMessage, IEquatable<GetPlayer>, IDeepCloneable<GetPlayer>
	{
		// Token: 0x17000174 RID: 372
		// (get) Token: 0x0600046A RID: 1130 RVA: 0x00014079 File Offset: 0x00012279
		[DebuggerNonUserCode]
		public static MessageParser<GetPlayer> Parser
		{
			get
			{
				return GetPlayer._parser;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x0600046B RID: 1131 RVA: 0x00014080 File Offset: 0x00012280
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return BeatSaberInboundMessageReflection.Descriptor.MessageTypes[5];
			}
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x0600046C RID: 1132 RVA: 0x00014092 File Offset: 0x00012292
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return GetPlayer.Descriptor;
			}
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x0000370C File Offset: 0x0000190C
		[DebuggerNonUserCode]
		public GetPlayer()
		{
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x00014099 File Offset: 0x00012299
		[DebuggerNonUserCode]
		public GetPlayer(GetPlayer other)
			: this()
		{
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x000140B2 File Offset: 0x000122B2
		[DebuggerNonUserCode]
		public GetPlayer Clone()
		{
			return new GetPlayer(this);
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x000140BA File Offset: 0x000122BA
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as GetPlayer);
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x000140C8 File Offset: 0x000122C8
		[DebuggerNonUserCode]
		public bool Equals(GetPlayer other)
		{
			return other != null && (other == this || object.Equals(this._unknownFields, other._unknownFields));
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x000140E8 File Offset: 0x000122E8
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x0001410E File Offset: 0x0001230E
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00014124 File Offset: 0x00012324
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0001414A File Offset: 0x0001234A
		[DebuggerNonUserCode]
		public void MergeFrom(GetPlayer other)
		{
			if (other == null)
			{
				return;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00014167 File Offset: 0x00012367
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			while (input.ReadTag() != 0U)
			{
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x04000235 RID: 565
		private static readonly MessageParser<GetPlayer> _parser = new MessageParser<GetPlayer>(() => new GetPlayer());

		// Token: 0x04000236 RID: 566
		private UnknownFieldSet _unknownFields;
	}
}
