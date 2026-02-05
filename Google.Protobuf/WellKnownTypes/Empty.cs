using System;
using System.Diagnostics;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x02000030 RID: 48
	public sealed class Empty : IMessage<Empty>, IMessage, IEquatable<Empty>, IDeepCloneable<Empty>
	{
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000293 RID: 659 RVA: 0x0000C34A File Offset: 0x0000A54A
		[DebuggerNonUserCode]
		public static MessageParser<Empty> Parser
		{
			get
			{
				return Empty._parser;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000294 RID: 660 RVA: 0x0000C351 File Offset: 0x0000A551
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return EmptyReflection.Descriptor.MessageTypes[0];
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000295 RID: 661 RVA: 0x0000C363 File Offset: 0x0000A563
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return Empty.Descriptor;
			}
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000C36A File Offset: 0x0000A56A
		[DebuggerNonUserCode]
		public Empty()
		{
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000C372 File Offset: 0x0000A572
		[DebuggerNonUserCode]
		public Empty(Empty other)
			: this()
		{
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000C38B File Offset: 0x0000A58B
		[DebuggerNonUserCode]
		public Empty Clone()
		{
			return new Empty(this);
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000C393 File Offset: 0x0000A593
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as Empty);
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000C3A1 File Offset: 0x0000A5A1
		[DebuggerNonUserCode]
		public bool Equals(Empty other)
		{
			return other != null && (other == this || object.Equals(this._unknownFields, other._unknownFields));
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000C3C0 File Offset: 0x0000A5C0
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

		// Token: 0x0600029C RID: 668 RVA: 0x0000C3E6 File Offset: 0x0000A5E6
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000C3EE File Offset: 0x0000A5EE
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000C404 File Offset: 0x0000A604
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

		// Token: 0x0600029F RID: 671 RVA: 0x0000C42A File Offset: 0x0000A62A
		[DebuggerNonUserCode]
		public void MergeFrom(Empty other)
		{
			if (other == null)
			{
				return;
			}
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000C447 File Offset: 0x0000A647
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			while (input.ReadTag() != 0U)
			{
				this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
			}
		}

		// Token: 0x040000AF RID: 175
		private static readonly MessageParser<Empty> _parser = new MessageParser<Empty>(() => new Empty());

		// Token: 0x040000B0 RID: 176
		private UnknownFieldSet _unknownFields;
	}
}
