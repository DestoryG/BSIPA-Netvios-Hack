using System;
using Mono.Cecil.PE;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001CE RID: 462
	internal sealed class PortablePdbReader : ISymbolReader, IDisposable
	{
		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000E70 RID: 3696 RVA: 0x0003211C File Offset: 0x0003031C
		private bool IsEmbedded
		{
			get
			{
				return this.reader.image == this.debug_reader.image;
			}
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x00032136 File Offset: 0x00030336
		internal PortablePdbReader(Image image, ModuleDefinition module)
		{
			this.image = image;
			this.module = module;
			this.reader = module.reader;
			this.debug_reader = new MetadataReader(image, module, this.reader);
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x0003216B File Offset: 0x0003036B
		public ISymbolWriterProvider GetWriterProvider()
		{
			return new PortablePdbWriterProvider();
		}

		// Token: 0x06000E73 RID: 3699 RVA: 0x00032174 File Offset: 0x00030374
		public bool ProcessDebugHeader(ImageDebugHeader header)
		{
			if (this.image == this.module.Image)
			{
				return true;
			}
			ImageDebugHeaderEntry codeViewEntry = header.GetCodeViewEntry();
			if (codeViewEntry == null)
			{
				return false;
			}
			byte[] data = codeViewEntry.Data;
			if (data.Length < 24)
			{
				return false;
			}
			if (PortablePdbReader.ReadInt32(data, 0) != 1396986706)
			{
				return false;
			}
			byte[] array = new byte[16];
			Buffer.BlockCopy(data, 4, array, 0, 16);
			Guid guid = new Guid(array);
			Buffer.BlockCopy(this.image.PdbHeap.Id, 0, array, 0, 16);
			Guid guid2 = new Guid(array);
			if (guid != guid2)
			{
				return false;
			}
			this.ReadModule();
			return true;
		}

		// Token: 0x06000E74 RID: 3700 RVA: 0x0003220E File Offset: 0x0003040E
		private static int ReadInt32(byte[] bytes, int start)
		{
			return (int)bytes[start] | ((int)bytes[start + 1] << 8) | ((int)bytes[start + 2] << 16) | ((int)bytes[start + 3] << 24);
		}

		// Token: 0x06000E75 RID: 3701 RVA: 0x0003222D File Offset: 0x0003042D
		private void ReadModule()
		{
			this.module.custom_infos = this.debug_reader.GetCustomDebugInformation(this.module);
		}

		// Token: 0x06000E76 RID: 3702 RVA: 0x0003224C File Offset: 0x0003044C
		public MethodDebugInformation Read(MethodDefinition method)
		{
			MethodDebugInformation methodDebugInformation = new MethodDebugInformation(method);
			this.ReadSequencePoints(methodDebugInformation);
			this.ReadScope(methodDebugInformation);
			this.ReadStateMachineKickOffMethod(methodDebugInformation);
			this.ReadCustomDebugInformations(methodDebugInformation);
			return methodDebugInformation;
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x0003227D File Offset: 0x0003047D
		private void ReadSequencePoints(MethodDebugInformation method_info)
		{
			method_info.sequence_points = this.debug_reader.ReadSequencePoints(method_info.method);
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x00032296 File Offset: 0x00030496
		private void ReadScope(MethodDebugInformation method_info)
		{
			method_info.scope = this.debug_reader.ReadScope(method_info.method);
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x000322AF File Offset: 0x000304AF
		private void ReadStateMachineKickOffMethod(MethodDebugInformation method_info)
		{
			method_info.kickoff_method = this.debug_reader.ReadStateMachineKickoffMethod(method_info.method);
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x000322C8 File Offset: 0x000304C8
		private void ReadCustomDebugInformations(MethodDebugInformation info)
		{
			info.method.custom_infos = this.debug_reader.GetCustomDebugInformation(info.method);
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x000322E6 File Offset: 0x000304E6
		public void Dispose()
		{
			if (this.IsEmbedded)
			{
				return;
			}
			this.image.Dispose();
		}

		// Token: 0x040008D5 RID: 2261
		private readonly Image image;

		// Token: 0x040008D6 RID: 2262
		private readonly ModuleDefinition module;

		// Token: 0x040008D7 RID: 2263
		private readonly MetadataReader reader;

		// Token: 0x040008D8 RID: 2264
		private readonly MetadataReader debug_reader;
	}
}
