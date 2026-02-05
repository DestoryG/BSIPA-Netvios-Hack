using System;
using System.IO;
using System.Text;
using Mono.Cecil.Cil;
using Mono.Cecil.PE;
using Mono.Collections.Generic;

namespace Mono.Cecil.Pdb
{
	// Token: 0x0200000A RID: 10
	internal class CustomMetadataWriter : IDisposable
	{
		// Token: 0x0600011D RID: 285 RVA: 0x00003558 File Offset: 0x00001758
		public CustomMetadataWriter(SymWriter sym_writer)
		{
			this.sym_writer = sym_writer;
			this.stream = new MemoryStream();
			this.writer = new BinaryStreamWriter(this.stream);
			this.writer.WriteByte(4);
			this.writer.WriteByte(0);
			this.writer.Align(4);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x000035B4 File Offset: 0x000017B4
		public void WriteUsingInfo(ImportDebugInformation import_info)
		{
			this.Write(CustomMetadataType.UsingInfo, delegate
			{
				this.writer.WriteUInt16(1);
				this.writer.WriteUInt16((ushort)import_info.Targets.Count);
			});
		}

		// Token: 0x0600011F RID: 287 RVA: 0x000035E8 File Offset: 0x000017E8
		public void WriteForwardInfo(MetadataToken import_parent)
		{
			this.Write(CustomMetadataType.ForwardInfo, delegate
			{
				this.writer.WriteUInt32(import_parent.ToUInt32());
			});
		}

		// Token: 0x06000120 RID: 288 RVA: 0x0000361C File Offset: 0x0000181C
		public void WriteIteratorScopes(StateMachineScopeDebugInformation state_machine, MethodDebugInformation debug_info)
		{
			this.Write(CustomMetadataType.IteratorScopes, delegate
			{
				Collection<StateMachineScope> scopes = state_machine.Scopes;
				this.writer.WriteInt32(scopes.Count);
				foreach (StateMachineScope stateMachineScope in scopes)
				{
					int offset = stateMachineScope.Start.Offset;
					int num = (stateMachineScope.End.IsEndOfMethod ? debug_info.code_size : stateMachineScope.End.Offset);
					this.writer.WriteInt32(offset);
					this.writer.WriteInt32(num - 1);
				}
			});
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00003658 File Offset: 0x00001858
		public void WriteForwardIterator(TypeReference type)
		{
			this.Write(CustomMetadataType.ForwardIterator, delegate
			{
				this.writer.WriteBytes(Encoding.Unicode.GetBytes(type.Name));
			});
		}

		// Token: 0x06000122 RID: 290 RVA: 0x0000368C File Offset: 0x0000188C
		private void Write(CustomMetadataType type, Action write)
		{
			this.count++;
			this.writer.WriteByte(4);
			this.writer.WriteByte((byte)type);
			this.writer.Align(4);
			int position = this.writer.Position;
			this.writer.WriteUInt32(0U);
			write();
			this.writer.Align(4);
			int position2 = this.writer.Position;
			int num = position2 - position + 4;
			this.writer.Position = position;
			this.writer.WriteInt32(num);
			this.writer.Position = position2;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x0000372C File Offset: 0x0000192C
		public void WriteCustomMetadata()
		{
			if (this.count == 0)
			{
				return;
			}
			this.writer.BaseStream.Position = 1L;
			this.writer.WriteByte((byte)this.count);
			this.writer.Flush();
			this.sym_writer.DefineCustomMetadata("MD2", this.stream.ToArray());
		}

		// Token: 0x06000124 RID: 292 RVA: 0x0000378C File Offset: 0x0000198C
		public void Dispose()
		{
			this.stream.Dispose();
		}

		// Token: 0x04000014 RID: 20
		private readonly SymWriter sym_writer;

		// Token: 0x04000015 RID: 21
		private readonly MemoryStream stream;

		// Token: 0x04000016 RID: 22
		private readonly BinaryStreamWriter writer;

		// Token: 0x04000017 RID: 23
		private int count;

		// Token: 0x04000018 RID: 24
		private const byte version = 4;
	}
}
