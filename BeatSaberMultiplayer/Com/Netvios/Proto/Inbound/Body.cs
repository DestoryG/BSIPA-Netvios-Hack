using System;
using System.Diagnostics;
using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace Com.Netvios.Proto.Inbound
{
	// Token: 0x0200004B RID: 75
	public sealed class Body : IMessage<Body>, IMessage, IEquatable<Body>, IDeepCloneable<Body>
	{
		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000632 RID: 1586 RVA: 0x000192D4 File Offset: 0x000174D4
		[DebuggerNonUserCode]
		public static MessageParser<Body> Parser
		{
			get
			{
				return Body._parser;
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000633 RID: 1587 RVA: 0x000192DB File Offset: 0x000174DB
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return InboundMessageReflection.Descriptor.MessageTypes[0];
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000634 RID: 1588 RVA: 0x000192ED File Offset: 0x000174ED
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return Body.Descriptor;
			}
		}

		// Token: 0x06000635 RID: 1589 RVA: 0x0000370C File Offset: 0x0000190C
		[DebuggerNonUserCode]
		public Body()
		{
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x000192F4 File Offset: 0x000174F4
		[DebuggerNonUserCode]
		public Body(Body other)
			: this()
		{
			this.game_ = other.game_;
			if (other.DataCase == Body.DataOneofCase.BeatSaberBody)
			{
				this.BeatSaberBody = other.BeatSaberBody.Clone();
			}
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x00019333 File Offset: 0x00017533
		[DebuggerNonUserCode]
		public Body Clone()
		{
			return new Body(this);
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000638 RID: 1592 RVA: 0x0001933B File Offset: 0x0001753B
		// (set) Token: 0x06000639 RID: 1593 RVA: 0x00019343 File Offset: 0x00017543
		[DebuggerNonUserCode]
		public GameType Game
		{
			get
			{
				return this.game_;
			}
			set
			{
				this.game_ = value;
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x0600063A RID: 1594 RVA: 0x0001934C File Offset: 0x0001754C
		// (set) Token: 0x0600063B RID: 1595 RVA: 0x00019364 File Offset: 0x00017564
		[DebuggerNonUserCode]
		public BeatSaberBody BeatSaberBody
		{
			get
			{
				if (this.dataCase_ != Body.DataOneofCase.BeatSaberBody)
				{
					return null;
				}
				return (BeatSaberBody)this.data_;
			}
			set
			{
				this.data_ = value;
				this.dataCase_ = ((value == null) ? Body.DataOneofCase.None : Body.DataOneofCase.BeatSaberBody);
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x0600063C RID: 1596 RVA: 0x0001937A File Offset: 0x0001757A
		[DebuggerNonUserCode]
		public Body.DataOneofCase DataCase
		{
			get
			{
				return this.dataCase_;
			}
		}

		// Token: 0x0600063D RID: 1597 RVA: 0x00019382 File Offset: 0x00017582
		[DebuggerNonUserCode]
		public void ClearData()
		{
			this.dataCase_ = Body.DataOneofCase.None;
			this.data_ = null;
		}

		// Token: 0x0600063E RID: 1598 RVA: 0x00019392 File Offset: 0x00017592
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as Body);
		}

		// Token: 0x0600063F RID: 1599 RVA: 0x000193A0 File Offset: 0x000175A0
		[DebuggerNonUserCode]
		public bool Equals(Body other)
		{
			return other != null && (other == this || (this.Game == other.Game && object.Equals(this.BeatSaberBody, other.BeatSaberBody) && this.DataCase == other.DataCase && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x00019400 File Offset: 0x00017600
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			if (this.Game != GameType.BeatSaber)
			{
				num ^= this.Game.GetHashCode();
			}
			if (this.dataCase_ == Body.DataOneofCase.BeatSaberBody)
			{
				num ^= this.BeatSaberBody.GetHashCode();
			}
			num ^= (int)this.dataCase_;
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x0000460F File Offset: 0x0000280F
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x00019468 File Offset: 0x00017668
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this.Game != GameType.BeatSaber)
			{
				output.WriteRawTag(8);
				output.WriteEnum((int)this.Game);
			}
			if (this.dataCase_ == Body.DataOneofCase.BeatSaberBody)
			{
				output.WriteRawTag(42);
				output.WriteMessage(this.BeatSaberBody);
			}
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x000194C4 File Offset: 0x000176C4
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			if (this.Game != GameType.BeatSaber)
			{
				num += 1 + CodedOutputStream.ComputeEnumSize((int)this.Game);
			}
			if (this.dataCase_ == Body.DataOneofCase.BeatSaberBody)
			{
				num += 1 + CodedOutputStream.ComputeMessageSize(this.BeatSaberBody);
			}
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x0001951C File Offset: 0x0001771C
		[DebuggerNonUserCode]
		public void MergeFrom(Body other)
		{
			if (other == null)
			{
				return;
			}
			if (other.Game != GameType.BeatSaber)
			{
				this.Game = other.Game;
			}
			if (other.DataCase == Body.DataOneofCase.BeatSaberBody)
			{
				if (this.BeatSaberBody == null)
				{
					this.BeatSaberBody = new BeatSaberBody();
				}
				this.BeatSaberBody.MergeFrom(other.BeatSaberBody);
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x00019588 File Offset: 0x00017788
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 8U)
				{
					if (num != 42U)
					{
						this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
					}
					else
					{
						BeatSaberBody beatSaberBody = new BeatSaberBody();
						if (this.dataCase_ == Body.DataOneofCase.BeatSaberBody)
						{
							beatSaberBody.MergeFrom(this.BeatSaberBody);
						}
						input.ReadMessage(beatSaberBody);
						this.BeatSaberBody = beatSaberBody;
					}
				}
				else
				{
					this.Game = (GameType)input.ReadEnum();
				}
			}
		}

		// Token: 0x040002F0 RID: 752
		private static readonly MessageParser<Body> _parser = new MessageParser<Body>(() => new Body());

		// Token: 0x040002F1 RID: 753
		private UnknownFieldSet _unknownFields;

		// Token: 0x040002F2 RID: 754
		public const int GameFieldNumber = 1;

		// Token: 0x040002F3 RID: 755
		private GameType game_;

		// Token: 0x040002F4 RID: 756
		public const int BeatSaberBodyFieldNumber = 5;

		// Token: 0x040002F5 RID: 757
		private object data_;

		// Token: 0x040002F6 RID: 758
		private Body.DataOneofCase dataCase_;

		// Token: 0x020000E4 RID: 228
		public enum DataOneofCase
		{
			// Token: 0x0400057A RID: 1402
			None,
			// Token: 0x0400057B RID: 1403
			BeatSaberBody = 5
		}
	}
}
