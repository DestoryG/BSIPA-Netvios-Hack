using System;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x020001C5 RID: 453
	internal class InstructionCollection : Collection<Instruction>
	{
		// Token: 0x06000E51 RID: 3665 RVA: 0x00030B7B File Offset: 0x0002ED7B
		internal InstructionCollection(MethodDefinition method)
		{
			this.method = method;
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x00030B8A File Offset: 0x0002ED8A
		internal InstructionCollection(MethodDefinition method, int capacity)
			: base(capacity)
		{
			this.method = method;
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x00030B9C File Offset: 0x0002ED9C
		protected override void OnAdd(Instruction item, int index)
		{
			if (index == 0)
			{
				return;
			}
			Instruction instruction = this.items[index - 1];
			instruction.next = item;
			item.previous = instruction;
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x00030BC8 File Offset: 0x0002EDC8
		protected override void OnInsert(Instruction item, int index)
		{
			if (this.size == 0)
			{
				return;
			}
			Instruction instruction = this.items[index];
			if (instruction == null)
			{
				Instruction instruction2 = this.items[index - 1];
				instruction2.next = item;
				item.previous = instruction2;
				return;
			}
			Instruction previous = instruction.previous;
			if (previous != null)
			{
				previous.next = item;
				item.previous = previous;
			}
			instruction.previous = item;
			item.next = instruction;
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x00030C2C File Offset: 0x0002EE2C
		protected override void OnSet(Instruction item, int index)
		{
			Instruction instruction = this.items[index];
			item.previous = instruction.previous;
			item.next = instruction.next;
			instruction.previous = null;
			instruction.next = null;
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x00030C68 File Offset: 0x0002EE68
		protected override void OnRemove(Instruction item, int index)
		{
			Instruction previous = item.previous;
			if (previous != null)
			{
				previous.next = item.next;
			}
			Instruction next = item.next;
			if (next != null)
			{
				next.previous = item.previous;
			}
			this.RemoveSequencePoint(item);
			item.previous = null;
			item.next = null;
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x00030CB8 File Offset: 0x0002EEB8
		private void RemoveSequencePoint(Instruction instruction)
		{
			MethodDebugInformation debug_info = this.method.debug_info;
			if (debug_info == null || !debug_info.HasSequencePoints)
			{
				return;
			}
			Collection<SequencePoint> sequence_points = debug_info.sequence_points;
			for (int i = 0; i < sequence_points.Count; i++)
			{
				if (sequence_points[i].Offset == instruction.offset)
				{
					sequence_points.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x040007AA RID: 1962
		private readonly MethodDefinition method;
	}
}
