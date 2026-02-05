using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Google.Protobuf.Reflection
{
	// Token: 0x0200006D RID: 109
	public sealed class ExtensionCollection
	{
		// Token: 0x0600076C RID: 1900 RVA: 0x0001AD04 File Offset: 0x00018F04
		internal ExtensionCollection(FileDescriptor file, Extension[] extensions)
		{
			this.UnorderedExtensions = DescriptorUtil.ConvertAndMakeReadOnly<FieldDescriptorProto, FieldDescriptor>(file.Proto.Extension, delegate(FieldDescriptorProto extension, int i)
			{
				Extension[] extensions2 = extensions;
				if (extensions2 == null || extensions2.Length != 0)
				{
					FileDescriptor file2 = file;
					MessageDescriptor messageDescriptor = null;
					string text = null;
					Extension[] extensions3 = extensions;
					return new FieldDescriptor(extension, file2, messageDescriptor, i, text, (extensions3 != null) ? extensions3[i] : null);
				}
				return new FieldDescriptor(extension, file, null, i, null, null);
			});
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0001AD54 File Offset: 0x00018F54
		internal ExtensionCollection(MessageDescriptor message, Extension[] extensions)
		{
			this.UnorderedExtensions = DescriptorUtil.ConvertAndMakeReadOnly<FieldDescriptorProto, FieldDescriptor>(message.Proto.Extension, delegate(FieldDescriptorProto extension, int i)
			{
				Extension[] extensions2 = extensions;
				if (extensions2 == null || extensions2.Length != 0)
				{
					FileDescriptor file = message.File;
					MessageDescriptor message2 = message;
					string text = null;
					Extension[] extensions3 = extensions;
					return new FieldDescriptor(extension, file, message2, i, text, (extensions3 != null) ? extensions3[i] : null);
				}
				return new FieldDescriptor(extension, message.File, message, i, null, null);
			});
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x0600076E RID: 1902 RVA: 0x0001ADA2 File Offset: 0x00018FA2
		public IList<FieldDescriptor> UnorderedExtensions { get; }

		// Token: 0x0600076F RID: 1903 RVA: 0x0001ADAA File Offset: 0x00018FAA
		public IList<FieldDescriptor> GetExtensionsInDeclarationOrder(MessageDescriptor descriptor)
		{
			return this.extensionsByTypeInDeclarationOrder[descriptor];
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x0001ADB8 File Offset: 0x00018FB8
		public IList<FieldDescriptor> GetExtensionsInNumberOrder(MessageDescriptor descriptor)
		{
			return this.extensionsByTypeInNumberOrder[descriptor];
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x0001ADC8 File Offset: 0x00018FC8
		internal void CrossLink()
		{
			Dictionary<MessageDescriptor, IList<FieldDescriptor>> dictionary = new Dictionary<MessageDescriptor, IList<FieldDescriptor>>();
			foreach (FieldDescriptor fieldDescriptor in this.UnorderedExtensions)
			{
				fieldDescriptor.CrossLink();
				IList<FieldDescriptor> list;
				if (!dictionary.TryGetValue(fieldDescriptor.ExtendeeType, out list))
				{
					list = new List<FieldDescriptor>();
					dictionary.Add(fieldDescriptor.ExtendeeType, list);
				}
				list.Add(fieldDescriptor);
			}
			this.extensionsByTypeInDeclarationOrder = dictionary.ToDictionary((KeyValuePair<MessageDescriptor, IList<FieldDescriptor>> kvp) => kvp.Key, (KeyValuePair<MessageDescriptor, IList<FieldDescriptor>> kvp) => new ReadOnlyCollection<FieldDescriptor>(kvp.Value));
			this.extensionsByTypeInNumberOrder = dictionary.ToDictionary((KeyValuePair<MessageDescriptor, IList<FieldDescriptor>> kvp) => kvp.Key, (KeyValuePair<MessageDescriptor, IList<FieldDescriptor>> kvp) => new ReadOnlyCollection<FieldDescriptor>(kvp.Value.OrderBy((FieldDescriptor field) => field.FieldNumber).ToArray<FieldDescriptor>()));
		}

		// Token: 0x040002E4 RID: 740
		private IDictionary<MessageDescriptor, IList<FieldDescriptor>> extensionsByTypeInDeclarationOrder;

		// Token: 0x040002E5 RID: 741
		private IDictionary<MessageDescriptor, IList<FieldDescriptor>> extensionsByTypeInNumberOrder;
	}
}
