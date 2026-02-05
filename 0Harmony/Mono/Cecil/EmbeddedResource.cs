using System;
using System.IO;

namespace Mono.Cecil
{
	// Token: 0x02000106 RID: 262
	internal sealed class EmbeddedResource : Resource
	{
		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060006CF RID: 1743 RVA: 0x00010F39 File Offset: 0x0000F139
		public override ResourceType ResourceType
		{
			get
			{
				return ResourceType.Embedded;
			}
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x0001EEB8 File Offset: 0x0001D0B8
		public EmbeddedResource(string name, ManifestResourceAttributes attributes, byte[] data)
			: base(name, attributes)
		{
			this.data = data;
		}

		// Token: 0x060006D1 RID: 1745 RVA: 0x0001EEC9 File Offset: 0x0001D0C9
		public EmbeddedResource(string name, ManifestResourceAttributes attributes, Stream stream)
			: base(name, attributes)
		{
			this.stream = stream;
		}

		// Token: 0x060006D2 RID: 1746 RVA: 0x0001EEDA File Offset: 0x0001D0DA
		internal EmbeddedResource(string name, ManifestResourceAttributes attributes, uint offset, MetadataReader reader)
			: base(name, attributes)
		{
			this.offset = new uint?(offset);
			this.reader = reader;
		}

		// Token: 0x060006D3 RID: 1747 RVA: 0x0001EEF8 File Offset: 0x0001D0F8
		public Stream GetResourceStream()
		{
			if (this.stream != null)
			{
				return this.stream;
			}
			if (this.data != null)
			{
				return new MemoryStream(this.data);
			}
			if (this.offset != null)
			{
				return new MemoryStream(this.reader.GetManagedResource(this.offset.Value));
			}
			throw new InvalidOperationException();
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0001EF58 File Offset: 0x0001D158
		public byte[] GetResourceData()
		{
			if (this.stream != null)
			{
				return EmbeddedResource.ReadStream(this.stream);
			}
			if (this.data != null)
			{
				return this.data;
			}
			if (this.offset != null)
			{
				return this.reader.GetManagedResource(this.offset.Value);
			}
			throw new InvalidOperationException();
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0001EFB4 File Offset: 0x0001D1B4
		private static byte[] ReadStream(Stream stream)
		{
			int num3;
			if (stream.CanSeek)
			{
				int num = (int)stream.Length;
				byte[] array = new byte[num];
				int num2 = 0;
				while ((num3 = stream.Read(array, num2, num - num2)) > 0)
				{
					num2 += num3;
				}
				return array;
			}
			byte[] array2 = new byte[8192];
			MemoryStream memoryStream = new MemoryStream();
			while ((num3 = stream.Read(array2, 0, array2.Length)) > 0)
			{
				memoryStream.Write(array2, 0, num3);
			}
			return memoryStream.ToArray();
		}

		// Token: 0x0400029D RID: 669
		private readonly MetadataReader reader;

		// Token: 0x0400029E RID: 670
		private uint? offset;

		// Token: 0x0400029F RID: 671
		private byte[] data;

		// Token: 0x040002A0 RID: 672
		private Stream stream;
	}
}
