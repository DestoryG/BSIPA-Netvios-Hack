using System;
using System.Collections;
using System.IO;
using System.Linq;
using Google.Protobuf.Reflection;

namespace Google.Protobuf
{
	// Token: 0x0200001F RID: 31
	public static class MessageExtensions
	{
		// Token: 0x060001BD RID: 445 RVA: 0x000094F6 File Offset: 0x000076F6
		public static void MergeFrom(this IMessage message, byte[] data)
		{
			message.MergeFrom(data, false, null);
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00009501 File Offset: 0x00007701
		public static void MergeFrom(this IMessage message, byte[] data, int offset, int length)
		{
			message.MergeFrom(data, offset, length, false, null);
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000950E File Offset: 0x0000770E
		public static void MergeFrom(this IMessage message, ByteString data)
		{
			message.MergeFrom(data, false, null);
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00009519 File Offset: 0x00007719
		public static void MergeFrom(this IMessage message, Stream input)
		{
			message.MergeFrom(input, false, null);
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00009524 File Offset: 0x00007724
		public static void MergeDelimitedFrom(this IMessage message, Stream input)
		{
			message.MergeDelimitedFrom(input, false, null);
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00009530 File Offset: 0x00007730
		public static byte[] ToByteArray(this IMessage message)
		{
			ProtoPreconditions.CheckNotNull<IMessage>(message, "message");
			byte[] array = new byte[message.CalculateSize()];
			CodedOutputStream codedOutputStream = new CodedOutputStream(array);
			message.WriteTo(codedOutputStream);
			codedOutputStream.CheckNoSpaceLeft();
			return array;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00009568 File Offset: 0x00007768
		public static void WriteTo(this IMessage message, Stream output)
		{
			ProtoPreconditions.CheckNotNull<IMessage>(message, "message");
			ProtoPreconditions.CheckNotNull<Stream>(output, "output");
			CodedOutputStream codedOutputStream = new CodedOutputStream(output);
			message.WriteTo(codedOutputStream);
			codedOutputStream.Flush();
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x000095A4 File Offset: 0x000077A4
		public static void WriteDelimitedTo(this IMessage message, Stream output)
		{
			ProtoPreconditions.CheckNotNull<IMessage>(message, "message");
			ProtoPreconditions.CheckNotNull<Stream>(output, "output");
			CodedOutputStream codedOutputStream = new CodedOutputStream(output);
			codedOutputStream.WriteRawVarint32((uint)message.CalculateSize());
			message.WriteTo(codedOutputStream);
			codedOutputStream.Flush();
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x000095E9 File Offset: 0x000077E9
		public static ByteString ToByteString(this IMessage message)
		{
			ProtoPreconditions.CheckNotNull<IMessage>(message, "message");
			return ByteString.AttachBytes(message.ToByteArray());
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00009604 File Offset: 0x00007804
		public static bool IsInitialized(this IMessage message)
		{
			return message.Descriptor.File.Syntax == Syntax.Proto3 || (message.Descriptor.IsExtensionsInitialized(message) && message.Descriptor.Fields.InDeclarationOrder().All(delegate(FieldDescriptor f)
			{
				if (f.IsMap)
				{
					return f.MessageType.Fields[2].FieldType != FieldType.Message || ((IDictionary)f.Accessor.GetValue(message)).Values.Cast<IMessage>().All(new Func<IMessage, bool>(MessageExtensions.IsInitialized));
				}
				if ((f.IsRepeated && f.FieldType == FieldType.Message) || f.FieldType == FieldType.Group)
				{
					return ((IEnumerable)f.Accessor.GetValue(message)).Cast<IMessage>().All(new Func<IMessage, bool>(MessageExtensions.IsInitialized));
				}
				if (f.FieldType != FieldType.Message && f.FieldType != FieldType.Group)
				{
					return !f.IsRequired || f.Accessor.HasValue(message);
				}
				if (f.Accessor.HasValue(message))
				{
					return ((IMessage)f.Accessor.GetValue(message)).IsInitialized();
				}
				return !f.IsRequired;
			}));
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00009678 File Offset: 0x00007878
		internal static void MergeFrom(this IMessage message, byte[] data, bool discardUnknownFields, ExtensionRegistry registry)
		{
			ProtoPreconditions.CheckNotNull<IMessage>(message, "message");
			ProtoPreconditions.CheckNotNull<byte[]>(data, "data");
			CodedInputStream codedInputStream = new CodedInputStream(data);
			codedInputStream.DiscardUnknownFields = discardUnknownFields;
			codedInputStream.ExtensionRegistry = registry;
			message.MergeFrom(codedInputStream);
			codedInputStream.CheckReadEndOfStreamTag();
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x000096C0 File Offset: 0x000078C0
		internal static void MergeFrom(this IMessage message, byte[] data, int offset, int length, bool discardUnknownFields, ExtensionRegistry registry)
		{
			ProtoPreconditions.CheckNotNull<IMessage>(message, "message");
			ProtoPreconditions.CheckNotNull<byte[]>(data, "data");
			CodedInputStream codedInputStream = new CodedInputStream(data, offset, length);
			codedInputStream.DiscardUnknownFields = discardUnknownFields;
			codedInputStream.ExtensionRegistry = registry;
			message.MergeFrom(codedInputStream);
			codedInputStream.CheckReadEndOfStreamTag();
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000970C File Offset: 0x0000790C
		internal static void MergeFrom(this IMessage message, ByteString data, bool discardUnknownFields, ExtensionRegistry registry)
		{
			ProtoPreconditions.CheckNotNull<IMessage>(message, "message");
			ProtoPreconditions.CheckNotNull<ByteString>(data, "data");
			CodedInputStream codedInputStream = data.CreateCodedInput();
			codedInputStream.DiscardUnknownFields = discardUnknownFields;
			codedInputStream.ExtensionRegistry = registry;
			message.MergeFrom(codedInputStream);
			codedInputStream.CheckReadEndOfStreamTag();
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00009754 File Offset: 0x00007954
		internal static void MergeFrom(this IMessage message, Stream input, bool discardUnknownFields, ExtensionRegistry registry)
		{
			ProtoPreconditions.CheckNotNull<IMessage>(message, "message");
			ProtoPreconditions.CheckNotNull<Stream>(input, "input");
			CodedInputStream codedInputStream = new CodedInputStream(input);
			codedInputStream.DiscardUnknownFields = discardUnknownFields;
			codedInputStream.ExtensionRegistry = registry;
			message.MergeFrom(codedInputStream);
			codedInputStream.CheckReadEndOfStreamTag();
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000979C File Offset: 0x0000799C
		internal static void MergeDelimitedFrom(this IMessage message, Stream input, bool discardUnknownFields, ExtensionRegistry registry)
		{
			ProtoPreconditions.CheckNotNull<IMessage>(message, "message");
			ProtoPreconditions.CheckNotNull<Stream>(input, "input");
			int num = (int)CodedInputStream.ReadRawVarint32(input);
			Stream stream = new LimitedInputStream(input, num);
			message.MergeFrom(stream, discardUnknownFields, registry);
		}
	}
}
