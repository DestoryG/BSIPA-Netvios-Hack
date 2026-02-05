using System;
using System.IO;

namespace Mono.Cecil
{
	// Token: 0x02000055 RID: 85
	public sealed class EmbeddedResource : Resource
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000358 RID: 856 RVA: 0x00002BE8 File Offset: 0x00000DE8
		public override ResourceType ResourceType
		{
			get
			{
				return ResourceType.Embedded;
			}
		}

		// Token: 0x06000359 RID: 857 RVA: 0x000108AC File Offset: 0x0000EAAC
		public EmbeddedResource(string name, ManifestResourceAttributes attributes, byte[] data)
			: base(name, attributes)
		{
			this.data = data;
		}

		// Token: 0x0600035A RID: 858 RVA: 0x000108BD File Offset: 0x0000EABD
		public EmbeddedResource(string name, ManifestResourceAttributes attributes, Stream stream)
			: base(name, attributes)
		{
			this.stream = stream;
		}

		// Token: 0x0600035B RID: 859 RVA: 0x000108CE File Offset: 0x0000EACE
		internal EmbeddedResource(string name, ManifestResourceAttributes attributes, uint offset, MetadataReader reader)
			: base(name, attributes)
		{
			this.offset = new uint?(offset);
			this.reader = reader;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x000108EC File Offset: 0x0000EAEC
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

		// Token: 0x0600035D RID: 861 RVA: 0x0001094C File Offset: 0x0000EB4C
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

		// Token: 0x0600035E RID: 862 RVA: 0x000109A8 File Offset: 0x0000EBA8
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

		// Token: 0x04000093 RID: 147
		private readonly MetadataReader reader;

		// Token: 0x04000094 RID: 148
		private uint? offset;

		// Token: 0x04000095 RID: 149
		private byte[] data;

		// Token: 0x04000096 RID: 150
		private Stream stream;
	}
}
