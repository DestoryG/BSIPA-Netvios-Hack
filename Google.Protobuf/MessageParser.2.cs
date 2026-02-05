using System;
using System.IO;

namespace Google.Protobuf
{
	// Token: 0x02000021 RID: 33
	public sealed class MessageParser<T> : MessageParser where T : IMessage<T>
	{
		// Token: 0x060001DA RID: 474 RVA: 0x00009970 File Offset: 0x00007B70
		public MessageParser(Func<T> factory)
			: this(factory, false, null)
		{
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000997C File Offset: 0x00007B7C
		internal MessageParser(Func<T> factory, bool discardUnknownFields, ExtensionRegistry extensions)
			: base(() => factory(), discardUnknownFields, extensions)
		{
			this.factory = factory;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x000099B6 File Offset: 0x00007BB6
		internal new T CreateTemplate()
		{
			return this.factory();
		}

		// Token: 0x060001DD RID: 477 RVA: 0x000099C3 File Offset: 0x00007BC3
		public new T ParseFrom(byte[] data)
		{
			T t = this.factory();
			t.MergeFrom(data, base.DiscardUnknownFields, base.Extensions);
			return t;
		}

		// Token: 0x060001DE RID: 478 RVA: 0x000099E8 File Offset: 0x00007BE8
		public new T ParseFrom(byte[] data, int offset, int length)
		{
			T t = this.factory();
			t.MergeFrom(data, offset, length, base.DiscardUnknownFields, base.Extensions);
			return t;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00009A0F File Offset: 0x00007C0F
		public new T ParseFrom(ByteString data)
		{
			T t = this.factory();
			t.MergeFrom(data, base.DiscardUnknownFields, base.Extensions);
			return t;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00009A34 File Offset: 0x00007C34
		public new T ParseFrom(Stream input)
		{
			T t = this.factory();
			t.MergeFrom(input, base.DiscardUnknownFields, base.Extensions);
			return t;
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x00009A59 File Offset: 0x00007C59
		public new T ParseDelimitedFrom(Stream input)
		{
			T t = this.factory();
			t.MergeDelimitedFrom(input, base.DiscardUnknownFields, base.Extensions);
			return t;
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x00009A80 File Offset: 0x00007C80
		public new T ParseFrom(CodedInputStream input)
		{
			T t = this.factory();
			base.MergeFrom(t, input);
			return t;
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x00009AA8 File Offset: 0x00007CA8
		public new T ParseJson(string json)
		{
			T t = this.factory();
			JsonParser.Default.Merge(t, json);
			return t;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x00009AD3 File Offset: 0x00007CD3
		public new MessageParser<T> WithDiscardUnknownFields(bool discardUnknownFields)
		{
			return new MessageParser<T>(this.factory, discardUnknownFields, base.Extensions);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00009AE7 File Offset: 0x00007CE7
		public new MessageParser<T> WithExtensionRegistry(ExtensionRegistry registry)
		{
			return new MessageParser<T>(this.factory, base.DiscardUnknownFields, registry);
		}

		// Token: 0x04000061 RID: 97
		private readonly Func<T> factory;
	}
}
