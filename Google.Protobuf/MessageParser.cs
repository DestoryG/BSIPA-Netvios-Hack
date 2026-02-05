using System;
using System.IO;

namespace Google.Protobuf
{
	// Token: 0x02000020 RID: 32
	public class MessageParser
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060001CC RID: 460 RVA: 0x000097D9 File Offset: 0x000079D9
		internal bool DiscardUnknownFields { get; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060001CD RID: 461 RVA: 0x000097E1 File Offset: 0x000079E1
		internal ExtensionRegistry Extensions { get; }

		// Token: 0x060001CE RID: 462 RVA: 0x000097E9 File Offset: 0x000079E9
		internal MessageParser(Func<IMessage> factory, bool discardUnknownFields, ExtensionRegistry extensions)
		{
			this.factory = factory;
			this.DiscardUnknownFields = discardUnknownFields;
			this.Extensions = extensions;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00009806 File Offset: 0x00007A06
		internal IMessage CreateTemplate()
		{
			return this.factory();
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00009813 File Offset: 0x00007A13
		public IMessage ParseFrom(byte[] data)
		{
			IMessage message = this.factory();
			message.MergeFrom(data, this.DiscardUnknownFields, this.Extensions);
			return message;
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00009833 File Offset: 0x00007A33
		public IMessage ParseFrom(byte[] data, int offset, int length)
		{
			IMessage message = this.factory();
			message.MergeFrom(data, offset, length, this.DiscardUnknownFields, this.Extensions);
			return message;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00009855 File Offset: 0x00007A55
		public IMessage ParseFrom(ByteString data)
		{
			IMessage message = this.factory();
			message.MergeFrom(data, this.DiscardUnknownFields, this.Extensions);
			return message;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00009875 File Offset: 0x00007A75
		public IMessage ParseFrom(Stream input)
		{
			IMessage message = this.factory();
			message.MergeFrom(input, this.DiscardUnknownFields, this.Extensions);
			return message;
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00009895 File Offset: 0x00007A95
		public IMessage ParseDelimitedFrom(Stream input)
		{
			IMessage message = this.factory();
			message.MergeDelimitedFrom(input, this.DiscardUnknownFields, this.Extensions);
			return message;
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x000098B8 File Offset: 0x00007AB8
		public IMessage ParseFrom(CodedInputStream input)
		{
			IMessage message = this.factory();
			this.MergeFrom(message, input);
			return message;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x000098DC File Offset: 0x00007ADC
		public IMessage ParseJson(string json)
		{
			IMessage message = this.factory();
			JsonParser.Default.Merge(message, json);
			return message;
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x00009904 File Offset: 0x00007B04
		internal void MergeFrom(IMessage message, CodedInputStream codedInput)
		{
			bool discardUnknownFields = codedInput.DiscardUnknownFields;
			try
			{
				codedInput.DiscardUnknownFields = this.DiscardUnknownFields;
				message.MergeFrom(codedInput);
			}
			finally
			{
				codedInput.DiscardUnknownFields = discardUnknownFields;
			}
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x00009948 File Offset: 0x00007B48
		public MessageParser WithDiscardUnknownFields(bool discardUnknownFields)
		{
			return new MessageParser(this.factory, discardUnknownFields, this.Extensions);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000995C File Offset: 0x00007B5C
		public MessageParser WithExtensionRegistry(ExtensionRegistry registry)
		{
			return new MessageParser(this.factory, this.DiscardUnknownFields, registry);
		}

		// Token: 0x0400005E RID: 94
		private Func<IMessage> factory;
	}
}
