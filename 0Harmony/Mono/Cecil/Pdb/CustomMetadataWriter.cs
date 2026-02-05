using System;
using System.IO;
using System.Text;
using Mono.Cecil.Cil;
using Mono.Cecil.PE;
using Mono.Collections.Generic;

namespace Mono.Cecil.Pdb
{
	// Token: 0x02000231 RID: 561
	internal class CustomMetadataWriter : IDisposable
	{
		// Token: 0x06001184 RID: 4484 RVA: 0x00039388 File Offset: 0x00037588
		public CustomMetadataWriter(SymWriter sym_writer)
		{
			this.sym_writer = sym_writer;
			this.stream = new MemoryStream();
			this.writer = new BinaryStreamWriter(this.stream);
			this.writer.WriteByte(4);
			this.writer.WriteByte(0);
			this.writer.Align(4);
		}

		// Token: 0x06001185 RID: 4485 RVA: 0x000393E4 File Offset: 0x000375E4
		public void WriteUsingInfo(ImportDebugInformation import_info)
		{
			this.Write(CustomMetadataType.UsingInfo, delegate
			{
				this.writer.WriteUInt16(1);
				this.writer.WriteUInt16((ushort)import_info.Targets.Count);
			});
		}

		// Token: 0x06001186 RID: 4486 RVA: 0x00039418 File Offset: 0x00037618
		public void WriteForwardInfo(MetadataToken import_parent)
		{
			this.Write(CustomMetadataType.ForwardInfo, delegate
			{
				this.writer.WriteUInt32(import_parent.ToUInt32());
			});
		}

		// Token: 0x06001187 RID: 4487 RVA: 0x0003944C File Offset: 0x0003764C
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

		// Token: 0x06001188 RID: 4488 RVA: 0x00039488 File Offset: 0x00037688
		public void WriteForwardIterator(TypeReference type)
		{
			this.Write(CustomMetadataType.ForwardIterator, delegate
			{
				this.writer.WriteBytes(Encoding.Unicode.GetBytes(type.Name));
			});
		}

		// Token: 0x06001189 RID: 4489 RVA: 0x000394BC File Offset: 0x000376BC
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

		// Token: 0x0600118A RID: 4490 RVA: 0x0003955C File Offset: 0x0003775C
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

		// Token: 0x0600118B RID: 4491 RVA: 0x000395BC File Offset: 0x000377BC
		public void Dispose()
		{
			this.stream.Dispose();
		}

		// Token: 0x04000A27 RID: 2599
		private readonly SymWriter sym_writer;

		// Token: 0x04000A28 RID: 2600
		private readonly MemoryStream stream;

		// Token: 0x04000A29 RID: 2601
		private readonly BinaryStreamWriter writer;

		// Token: 0x04000A2A RID: 2602
		private int count;

		// Token: 0x04000A2B RID: 2603
		private const byte version = 4;
	}
}
