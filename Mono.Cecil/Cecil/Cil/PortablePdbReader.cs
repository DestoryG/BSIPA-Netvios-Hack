using System;
using Mono.Cecil.PE;

namespace Mono.Cecil.Cil
{
	// Token: 0x0200010A RID: 266
	public sealed class PortablePdbReader : ISymbolReader, IDisposable
	{
		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000A89 RID: 2697 RVA: 0x00022F10 File Offset: 0x00021110
		private bool IsEmbedded
		{
			get
			{
				return this.reader.image == this.debug_reader.image;
			}
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x00022F2A File Offset: 0x0002112A
		internal PortablePdbReader(Image image, ModuleDefinition module)
		{
			this.image = image;
			this.module = module;
			this.reader = module.reader;
			this.debug_reader = new MetadataReader(image, module, this.reader);
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x00022F5F File Offset: 0x0002115F
		public ISymbolWriterProvider GetWriterProvider()
		{
			return new PortablePdbWriterProvider();
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x00022F68 File Offset: 0x00021168
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

		// Token: 0x06000A8D RID: 2701 RVA: 0x00023002 File Offset: 0x00021202
		private static int ReadInt32(byte[] bytes, int start)
		{
			return (int)bytes[start] | ((int)bytes[start + 1] << 8) | ((int)bytes[start + 2] << 16) | ((int)bytes[start + 3] << 24);
		}

		// Token: 0x06000A8E RID: 2702 RVA: 0x00023021 File Offset: 0x00021221
		private void ReadModule()
		{
			this.module.custom_infos = this.debug_reader.GetCustomDebugInformation(this.module);
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x00023040 File Offset: 0x00021240
		public MethodDebugInformation Read(MethodDefinition method)
		{
			MethodDebugInformation methodDebugInformation = new MethodDebugInformation(method);
			this.ReadSequencePoints(methodDebugInformation);
			this.ReadScope(methodDebugInformation);
			this.ReadStateMachineKickOffMethod(methodDebugInformation);
			this.ReadCustomDebugInformations(methodDebugInformation);
			return methodDebugInformation;
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x00023071 File Offset: 0x00021271
		private void ReadSequencePoints(MethodDebugInformation method_info)
		{
			method_info.sequence_points = this.debug_reader.ReadSequencePoints(method_info.method);
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x0002308A File Offset: 0x0002128A
		private void ReadScope(MethodDebugInformation method_info)
		{
			method_info.scope = this.debug_reader.ReadScope(method_info.method);
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x000230A3 File Offset: 0x000212A3
		private void ReadStateMachineKickOffMethod(MethodDebugInformation method_info)
		{
			method_info.kickoff_method = this.debug_reader.ReadStateMachineKickoffMethod(method_info.method);
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x000230BC File Offset: 0x000212BC
		private void ReadCustomDebugInformations(MethodDebugInformation info)
		{
			info.method.custom_infos = this.debug_reader.GetCustomDebugInformation(info.method);
		}

		// Token: 0x06000A94 RID: 2708 RVA: 0x000230DA File Offset: 0x000212DA
		public void Dispose()
		{
			if (this.IsEmbedded)
			{
				return;
			}
			this.image.Dispose();
		}

		// Token: 0x04000676 RID: 1654
		private readonly Image image;

		// Token: 0x04000677 RID: 1655
		private readonly ModuleDefinition module;

		// Token: 0x04000678 RID: 1656
		private readonly MetadataReader reader;

		// Token: 0x04000679 RID: 1657
		private readonly MetadataReader debug_reader;
	}
}
