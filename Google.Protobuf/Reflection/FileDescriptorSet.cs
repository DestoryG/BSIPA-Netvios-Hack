using System;
using System.Diagnostics;
using Google.Protobuf.Collections;

namespace Google.Protobuf.Reflection
{
	// Token: 0x02000050 RID: 80
	public sealed class FileDescriptorSet : IMessage<FileDescriptorSet>, IMessage, IEquatable<FileDescriptorSet>, IDeepCloneable<FileDescriptorSet>
	{
		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000471 RID: 1137 RVA: 0x00012352 File Offset: 0x00010552
		[DebuggerNonUserCode]
		public static MessageParser<FileDescriptorSet> Parser
		{
			get
			{
				return FileDescriptorSet._parser;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000472 RID: 1138 RVA: 0x00012359 File Offset: 0x00010559
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return DescriptorReflection.Descriptor.MessageTypes[0];
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000473 RID: 1139 RVA: 0x0001236B File Offset: 0x0001056B
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return FileDescriptorSet.Descriptor;
			}
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00012372 File Offset: 0x00010572
		[DebuggerNonUserCode]
		public FileDescriptorSet()
		{
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00012385 File Offset: 0x00010585
		[DebuggerNonUserCode]
		public FileDescriptorSet(FileDescriptorSet other)
			: this()
		{
			this.file_ = other.file_.Clone();
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x000123AF File Offset: 0x000105AF
		[DebuggerNonUserCode]
		public FileDescriptorSet Clone()
		{
			return new FileDescriptorSet(this);
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000477 RID: 1143 RVA: 0x000123B7 File Offset: 0x000105B7
		[DebuggerNonUserCode]
		public RepeatedField<FileDescriptorProto> File
		{
			get
			{
				return this.file_;
			}
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x000123BF File Offset: 0x000105BF
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as FileDescriptorSet);
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x000123CD File Offset: 0x000105CD
		[DebuggerNonUserCode]
		public bool Equals(FileDescriptorSet other)
		{
			return other != null && (other == this || (this.file_.Equals(other.file_) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00012400 File Offset: 0x00010600
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			num ^= this.file_.GetHashCode();
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00012434 File Offset: 0x00010634
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0001243C File Offset: 0x0001063C
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			this.file_.WriteTo(output, FileDescriptorSet._repeated_file_codec);
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00012464 File Offset: 0x00010664
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			num += this.file_.CalculateSize(FileDescriptorSet._repeated_file_codec);
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0001249D File Offset: 0x0001069D
		[DebuggerNonUserCode]
		public void MergeFrom(FileDescriptorSet other)
		{
			if (other == null)
			{
				return;
			}
			this.file_.Add(other.file_);
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x000124CC File Offset: 0x000106CC
		[DebuggerNonUserCode]
		public void MergeFrom(CodedInputStream input)
		{
			uint num;
			while ((num = input.ReadTag()) != 0U)
			{
				if (num != 10U)
				{
					this._unknownFields = UnknownFieldSet.MergeFieldFrom(this._unknownFields, input);
				}
				else
				{
					this.file_.AddEntriesFrom(input, FileDescriptorSet._repeated_file_codec);
				}
			}
		}

		// Token: 0x04000153 RID: 339
		private static readonly MessageParser<FileDescriptorSet> _parser = new MessageParser<FileDescriptorSet>(() => new FileDescriptorSet());

		// Token: 0x04000154 RID: 340
		private UnknownFieldSet _unknownFields;

		// Token: 0x04000155 RID: 341
		public const int FileFieldNumber = 1;

		// Token: 0x04000156 RID: 342
		private static readonly FieldCodec<FileDescriptorProto> _repeated_file_codec = FieldCodec.ForMessage<FileDescriptorProto>(10U, FileDescriptorProto.Parser);

		// Token: 0x04000157 RID: 343
		private readonly RepeatedField<FileDescriptorProto> file_ = new RepeatedField<FileDescriptorProto>();
	}
}
