using System;
using Mono.Collections.Generic;

namespace Mono.Cecil.Cil
{
	// Token: 0x02000101 RID: 257
	internal class InstructionCollection : Collection<Instruction>
	{
		// Token: 0x06000A6A RID: 2666 RVA: 0x0002196F File Offset: 0x0001FB6F
		internal InstructionCollection(MethodDefinition method)
		{
			this.method = method;
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x0002197E File Offset: 0x0001FB7E
		internal InstructionCollection(MethodDefinition method, int capacity)
			: base(capacity)
		{
			this.method = method;
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x00021990 File Offset: 0x0001FB90
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

		// Token: 0x06000A6D RID: 2669 RVA: 0x000219BC File Offset: 0x0001FBBC
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

		// Token: 0x06000A6E RID: 2670 RVA: 0x00021A20 File Offset: 0x0001FC20
		protected override void OnSet(Instruction item, int index)
		{
			Instruction instruction = this.items[index];
			item.previous = instruction.previous;
			item.next = instruction.next;
			instruction.previous = null;
			instruction.next = null;
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x00021A5C File Offset: 0x0001FC5C
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

		// Token: 0x06000A70 RID: 2672 RVA: 0x00021AAC File Offset: 0x0001FCAC
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

		// Token: 0x0400054B RID: 1355
		private readonly MethodDefinition method;
	}
}
