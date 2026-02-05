using System;
using System.Collections.Generic;
using System.Linq;

namespace Google.Protobuf.Reflection
{
	// Token: 0x02000081 RID: 129
	public sealed class TypeRegistry
	{
		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000836 RID: 2102 RVA: 0x0001D28A File Offset: 0x0001B48A
		public static TypeRegistry Empty { get; } = new TypeRegistry(new Dictionary<string, MessageDescriptor>());

		// Token: 0x06000837 RID: 2103 RVA: 0x0001D291 File Offset: 0x0001B491
		private TypeRegistry(Dictionary<string, MessageDescriptor> fullNameToMessageMap)
		{
			this.fullNameToMessageMap = fullNameToMessageMap;
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x0001D2A0 File Offset: 0x0001B4A0
		public MessageDescriptor Find(string fullName)
		{
			MessageDescriptor messageDescriptor;
			this.fullNameToMessageMap.TryGetValue(fullName, out messageDescriptor);
			return messageDescriptor;
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x0001D2BD File Offset: 0x0001B4BD
		public static TypeRegistry FromFiles(params FileDescriptor[] fileDescriptors)
		{
			return TypeRegistry.FromFiles(fileDescriptors);
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x0001D2C8 File Offset: 0x0001B4C8
		public static TypeRegistry FromFiles(IEnumerable<FileDescriptor> fileDescriptors)
		{
			ProtoPreconditions.CheckNotNull<IEnumerable<FileDescriptor>>(fileDescriptors, "fileDescriptors");
			TypeRegistry.Builder builder = new TypeRegistry.Builder();
			foreach (FileDescriptor fileDescriptor in fileDescriptors)
			{
				builder.AddFile(fileDescriptor);
			}
			return builder.Build();
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x0001D328 File Offset: 0x0001B528
		public static TypeRegistry FromMessages(params MessageDescriptor[] messageDescriptors)
		{
			return TypeRegistry.FromMessages(messageDescriptors);
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x0001D330 File Offset: 0x0001B530
		public static TypeRegistry FromMessages(IEnumerable<MessageDescriptor> messageDescriptors)
		{
			ProtoPreconditions.CheckNotNull<IEnumerable<MessageDescriptor>>(messageDescriptors, "messageDescriptors");
			return TypeRegistry.FromFiles(messageDescriptors.Select((MessageDescriptor md) => md.File));
		}

		// Token: 0x04000348 RID: 840
		private readonly Dictionary<string, MessageDescriptor> fullNameToMessageMap;

		// Token: 0x020000FF RID: 255
		private class Builder
		{
			// Token: 0x06000A42 RID: 2626 RVA: 0x00020DDA File Offset: 0x0001EFDA
			internal Builder()
			{
				this.types = new Dictionary<string, MessageDescriptor>();
				this.fileDescriptorNames = new HashSet<string>();
			}

			// Token: 0x06000A43 RID: 2627 RVA: 0x00020DF8 File Offset: 0x0001EFF8
			internal void AddFile(FileDescriptor fileDescriptor)
			{
				if (!this.fileDescriptorNames.Add(fileDescriptor.Name))
				{
					return;
				}
				foreach (FileDescriptor fileDescriptor2 in fileDescriptor.Dependencies)
				{
					this.AddFile(fileDescriptor2);
				}
				foreach (MessageDescriptor messageDescriptor in fileDescriptor.MessageTypes)
				{
					this.AddMessage(messageDescriptor);
				}
			}

			// Token: 0x06000A44 RID: 2628 RVA: 0x00020E98 File Offset: 0x0001F098
			private void AddMessage(MessageDescriptor messageDescriptor)
			{
				foreach (MessageDescriptor messageDescriptor2 in messageDescriptor.NestedTypes)
				{
					this.AddMessage(messageDescriptor2);
				}
				this.types[messageDescriptor.FullName] = messageDescriptor;
			}

			// Token: 0x06000A45 RID: 2629 RVA: 0x00020EF8 File Offset: 0x0001F0F8
			internal TypeRegistry Build()
			{
				return new TypeRegistry(this.types);
			}

			// Token: 0x0400042A RID: 1066
			private readonly Dictionary<string, MessageDescriptor> types;

			// Token: 0x0400042B RID: 1067
			private readonly HashSet<string> fileDescriptorNames;
		}
	}
}
