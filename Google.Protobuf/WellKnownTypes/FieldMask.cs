using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;

namespace Google.Protobuf.WellKnownTypes
{
	// Token: 0x02000032 RID: 50
	public sealed class FieldMask : IMessage<FieldMask>, IMessage, IEquatable<FieldMask>, IDeepCloneable<FieldMask>, ICustomDiagnosticMessage
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060002A4 RID: 676 RVA: 0x0000C51B File Offset: 0x0000A71B
		[DebuggerNonUserCode]
		public static MessageParser<FieldMask> Parser
		{
			get
			{
				return FieldMask._parser;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x0000C522 File Offset: 0x0000A722
		[DebuggerNonUserCode]
		public static MessageDescriptor Descriptor
		{
			get
			{
				return FieldMaskReflection.Descriptor.MessageTypes[0];
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060002A6 RID: 678 RVA: 0x0000C534 File Offset: 0x0000A734
		[DebuggerNonUserCode]
		MessageDescriptor IMessage.Descriptor
		{
			get
			{
				return FieldMask.Descriptor;
			}
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000C53B File Offset: 0x0000A73B
		[DebuggerNonUserCode]
		public FieldMask()
		{
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000C54E File Offset: 0x0000A74E
		[DebuggerNonUserCode]
		public FieldMask(FieldMask other)
			: this()
		{
			this.paths_ = other.paths_.Clone();
			this._unknownFields = UnknownFieldSet.Clone(other._unknownFields);
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000C578 File Offset: 0x0000A778
		[DebuggerNonUserCode]
		public FieldMask Clone()
		{
			return new FieldMask(this);
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060002AA RID: 682 RVA: 0x0000C580 File Offset: 0x0000A780
		[DebuggerNonUserCode]
		public RepeatedField<string> Paths
		{
			get
			{
				return this.paths_;
			}
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000C588 File Offset: 0x0000A788
		[DebuggerNonUserCode]
		public override bool Equals(object other)
		{
			return this.Equals(other as FieldMask);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000C596 File Offset: 0x0000A796
		[DebuggerNonUserCode]
		public bool Equals(FieldMask other)
		{
			return other != null && (other == this || (this.paths_.Equals(other.paths_) && object.Equals(this._unknownFields, other._unknownFields)));
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000C5CC File Offset: 0x0000A7CC
		[DebuggerNonUserCode]
		public override int GetHashCode()
		{
			int num = 1;
			num ^= this.paths_.GetHashCode();
			if (this._unknownFields != null)
			{
				num ^= this._unknownFields.GetHashCode();
			}
			return num;
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000C600 File Offset: 0x0000A800
		[DebuggerNonUserCode]
		public override string ToString()
		{
			return JsonFormatter.ToDiagnosticString(this);
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000C608 File Offset: 0x0000A808
		[DebuggerNonUserCode]
		public void WriteTo(CodedOutputStream output)
		{
			this.paths_.WriteTo(output, FieldMask._repeated_paths_codec);
			if (this._unknownFields != null)
			{
				this._unknownFields.WriteTo(output);
			}
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000C630 File Offset: 0x0000A830
		[DebuggerNonUserCode]
		public int CalculateSize()
		{
			int num = 0;
			num += this.paths_.CalculateSize(FieldMask._repeated_paths_codec);
			if (this._unknownFields != null)
			{
				num += this._unknownFields.CalculateSize();
			}
			return num;
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000C669 File Offset: 0x0000A869
		[DebuggerNonUserCode]
		public void MergeFrom(FieldMask other)
		{
			if (other == null)
			{
				return;
			}
			this.paths_.Add(other.paths_);
			this._unknownFields = UnknownFieldSet.MergeFrom(this._unknownFields, other._unknownFields);
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000C698 File Offset: 0x0000A898
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
					this.paths_.AddEntriesFrom(input, FieldMask._repeated_paths_codec);
				}
			}
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000C6DC File Offset: 0x0000A8DC
		internal static string ToJson(IList<string> paths, bool diagnosticOnly)
		{
			string text = paths.FirstOrDefault((string p) => !FieldMask.IsPathValid(p));
			if (text == null)
			{
				StringWriter stringWriter = new StringWriter();
				JsonFormatter.WriteString(stringWriter, string.Join(",", paths.Select(new Func<string, string>(JsonFormatter.ToJsonName))));
				return stringWriter.ToString();
			}
			if (diagnosticOnly)
			{
				StringWriter stringWriter2 = new StringWriter();
				stringWriter2.Write("{ \"@warning\": \"Invalid FieldMask\", \"paths\": ");
				JsonFormatter.Default.WriteList(stringWriter2, (IList)paths);
				stringWriter2.Write(" }");
				return stringWriter2.ToString();
			}
			throw new InvalidOperationException("Invalid field mask to be converted to JSON: " + text);
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000C786 File Offset: 0x0000A986
		public string ToDiagnosticString()
		{
			return FieldMask.ToJson(this.Paths, true);
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000C794 File Offset: 0x0000A994
		public static FieldMask FromString(string value)
		{
			return FieldMask.FromStringEnumerable<Empty>(new List<string>(value.Split(new char[] { ',' })));
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000C7B1 File Offset: 0x0000A9B1
		public static FieldMask FromString<T>(string value) where T : IMessage
		{
			return FieldMask.FromStringEnumerable<T>(new List<string>(value.Split(new char[] { ',' })));
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000C7D0 File Offset: 0x0000A9D0
		public static FieldMask FromStringEnumerable<T>(IEnumerable<string> paths) where T : IMessage
		{
			FieldMask fieldMask = new FieldMask();
			foreach (string text in paths)
			{
				if (text.Length != 0)
				{
					if (typeof(T) != typeof(Empty) && !FieldMask.IsValid<T>(text))
					{
						string text2 = text;
						string text3 = " is not a valid path for ";
						Type typeFromHandle = typeof(T);
						throw new InvalidProtocolBufferException(text2 + text3 + ((typeFromHandle != null) ? typeFromHandle.ToString() : null));
					}
					fieldMask.Paths.Add(text);
				}
			}
			return fieldMask;
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000C878 File Offset: 0x0000AA78
		public static FieldMask FromFieldNumbers<T>(params int[] fieldNumbers) where T : IMessage
		{
			return FieldMask.FromFieldNumbers<T>(fieldNumbers);
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000C880 File Offset: 0x0000AA80
		public static FieldMask FromFieldNumbers<T>(IEnumerable<int> fieldNumbers) where T : IMessage
		{
			T t = Activator.CreateInstance<T>();
			MessageDescriptor descriptor = t.Descriptor;
			FieldMask fieldMask = new FieldMask();
			foreach (int num in fieldNumbers)
			{
				FieldDescriptor fieldDescriptor = descriptor.FindFieldByNumber(num);
				if (fieldDescriptor == null)
				{
					throw new ArgumentNullException(string.Format("{0} is not a valid field number for {1}", num, descriptor.Name));
				}
				fieldMask.Paths.Add(fieldDescriptor.Name);
			}
			return fieldMask;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000C91C File Offset: 0x0000AB1C
		private static bool IsPathValid(string input)
		{
			for (int i = 0; i < input.Length; i++)
			{
				char c = input[i];
				if (c >= 'A' && c <= 'Z')
				{
					return false;
				}
				if (c == '_' && i < input.Length - 1)
				{
					char c2 = input[i + 1];
					if (c2 < 'a' || c2 > 'z')
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000C978 File Offset: 0x0000AB78
		public static bool IsValid<T>(FieldMask fieldMask) where T : IMessage
		{
			T t = Activator.CreateInstance<T>();
			return FieldMask.IsValid(t.Descriptor, fieldMask);
		}

		// Token: 0x060002BC RID: 700 RVA: 0x0000C9A0 File Offset: 0x0000ABA0
		public static bool IsValid(MessageDescriptor descriptor, FieldMask fieldMask)
		{
			foreach (string text in fieldMask.Paths)
			{
				if (!FieldMask.IsValid(descriptor, text))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000C9F8 File Offset: 0x0000ABF8
		public static bool IsValid<T>(string path) where T : IMessage
		{
			T t = Activator.CreateInstance<T>();
			return FieldMask.IsValid(t.Descriptor, path);
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000CA20 File Offset: 0x0000AC20
		public static bool IsValid(MessageDescriptor descriptor, string path)
		{
			string[] array = path.Split(new char[] { '.' });
			if (array.Length == 0)
			{
				return false;
			}
			foreach (string text in array)
			{
				FieldDescriptor fieldDescriptor = ((descriptor != null) ? descriptor.FindFieldByName(text) : null);
				if (fieldDescriptor == null)
				{
					return false;
				}
				if (!fieldDescriptor.IsRepeated && fieldDescriptor.FieldType == FieldType.Message)
				{
					descriptor = fieldDescriptor.MessageType;
				}
				else
				{
					descriptor = null;
				}
			}
			return true;
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000CA91 File Offset: 0x0000AC91
		public FieldMask Normalize()
		{
			return new FieldMaskTree(this).ToFieldMask();
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000CAA0 File Offset: 0x0000ACA0
		public FieldMask Union(params FieldMask[] otherMasks)
		{
			FieldMaskTree fieldMaskTree = new FieldMaskTree(this);
			foreach (FieldMask fieldMask in otherMasks)
			{
				fieldMaskTree.MergeFromFieldMask(fieldMask);
			}
			return fieldMaskTree.ToFieldMask();
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000CAD8 File Offset: 0x0000ACD8
		public FieldMask Intersection(FieldMask additionalMask)
		{
			FieldMaskTree fieldMaskTree = new FieldMaskTree(this);
			FieldMaskTree fieldMaskTree2 = new FieldMaskTree();
			foreach (string text in additionalMask.Paths)
			{
				fieldMaskTree.IntersectFieldPath(text, fieldMaskTree2);
			}
			return fieldMaskTree2.ToFieldMask();
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000CB3C File Offset: 0x0000AD3C
		public void Merge(IMessage source, IMessage destination, FieldMask.MergeOptions options)
		{
			new FieldMaskTree(this).Merge(source, destination, options);
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000CB4C File Offset: 0x0000AD4C
		public void Merge(IMessage source, IMessage destination)
		{
			this.Merge(source, destination, new FieldMask.MergeOptions());
		}

		// Token: 0x040000B2 RID: 178
		private static readonly MessageParser<FieldMask> _parser = new MessageParser<FieldMask>(() => new FieldMask());

		// Token: 0x040000B3 RID: 179
		private UnknownFieldSet _unknownFields;

		// Token: 0x040000B4 RID: 180
		public const int PathsFieldNumber = 1;

		// Token: 0x040000B5 RID: 181
		private static readonly FieldCodec<string> _repeated_paths_codec = FieldCodec.ForString(10U);

		// Token: 0x040000B6 RID: 182
		private readonly RepeatedField<string> paths_ = new RepeatedField<string>();

		// Token: 0x040000B7 RID: 183
		private const char FIELD_PATH_SEPARATOR = ',';

		// Token: 0x040000B8 RID: 184
		private const char FIELD_SEPARATOR_REGEX = '.';

		// Token: 0x020000B1 RID: 177
		public sealed class MergeOptions
		{
			// Token: 0x17000269 RID: 617
			// (get) Token: 0x06000967 RID: 2407 RVA: 0x000200E8 File Offset: 0x0001E2E8
			// (set) Token: 0x06000968 RID: 2408 RVA: 0x000200F0 File Offset: 0x0001E2F0
			public bool ReplaceMessageFields { get; set; }

			// Token: 0x1700026A RID: 618
			// (get) Token: 0x06000969 RID: 2409 RVA: 0x000200F9 File Offset: 0x0001E2F9
			// (set) Token: 0x0600096A RID: 2410 RVA: 0x00020101 File Offset: 0x0001E301
			public bool ReplaceRepeatedFields { get; set; }

			// Token: 0x1700026B RID: 619
			// (get) Token: 0x0600096B RID: 2411 RVA: 0x0002010A File Offset: 0x0001E30A
			// (set) Token: 0x0600096C RID: 2412 RVA: 0x00020112 File Offset: 0x0001E312
			public bool ReplacePrimitiveFields { get; set; }
		}
	}
}
