using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace HarmonyLib
{
	// Token: 0x02000060 RID: 96
	[Serializable]
	public class HarmonyException : Exception
	{
		// Token: 0x060001A5 RID: 421 RVA: 0x00009F70 File Offset: 0x00008170
		internal HarmonyException()
		{
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x00009F78 File Offset: 0x00008178
		internal HarmonyException(string message)
			: base(message)
		{
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00009F81 File Offset: 0x00008181
		internal HarmonyException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00009F8B File Offset: 0x0000818B
		protected HarmonyException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00009F98 File Offset: 0x00008198
		internal HarmonyException(Exception innerException, Dictionary<int, CodeInstruction> instructions, int errorOffset)
			: base("IL Compile Error", innerException)
		{
			this.instructions = instructions;
			this.errorOffset = errorOffset;
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00009FB4 File Offset: 0x000081B4
		internal static Exception Create(Exception ex, Dictionary<int, CodeInstruction> finalInstructions)
		{
			Match match = Regex.Match(ex.Message.TrimEnd(new char[0]), "Reason: Invalid IL code in.+: IL_(\\d{4}): (.+)$");
			if (!match.Success)
			{
				return ex;
			}
			int num = int.Parse(match.Groups[1].Value, NumberStyles.HexNumber);
			Regex.Replace(match.Groups[2].Value, " {2,}", " ");
			HarmonyException ex2 = ex as HarmonyException;
			if (ex2 != null)
			{
				ex2.instructions = finalInstructions;
				ex2.errorOffset = num;
				return ex2;
			}
			return new HarmonyException(ex, finalInstructions, num);
		}

		// Token: 0x060001AB RID: 427 RVA: 0x0000A046 File Offset: 0x00008246
		public List<KeyValuePair<int, CodeInstruction>> GetInstructionsWithOffsets()
		{
			return this.instructions.OrderBy((KeyValuePair<int, CodeInstruction> ins) => ins.Key).ToList<KeyValuePair<int, CodeInstruction>>();
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000A078 File Offset: 0x00008278
		public List<CodeInstruction> GetInstructions()
		{
			return (from ins in this.instructions
				orderby ins.Key
				select ins.Value).ToList<CodeInstruction>();
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000A0D8 File Offset: 0x000082D8
		public int GetErrorOffset()
		{
			return this.errorOffset;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000A0E0 File Offset: 0x000082E0
		public int GetErrorIndex()
		{
			CodeInstruction codeInstruction;
			if (this.instructions.TryGetValue(this.errorOffset, out codeInstruction))
			{
				return this.GetInstructions().IndexOf(codeInstruction);
			}
			return -1;
		}

		// Token: 0x04000106 RID: 262
		private Dictionary<int, CodeInstruction> instructions;

		// Token: 0x04000107 RID: 263
		private int errorOffset;
	}
}
