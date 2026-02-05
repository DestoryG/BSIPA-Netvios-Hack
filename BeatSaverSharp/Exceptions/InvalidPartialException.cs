using System;

namespace BeatSaverSharp.Exceptions
{
	// Token: 0x02000016 RID: 22
	public class InvalidPartialException : Exception
	{
		// Token: 0x060000CE RID: 206 RVA: 0x00003AB4 File Offset: 0x00001CB4
		public InvalidPartialException()
		{
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00003ABC File Offset: 0x00001CBC
		public InvalidPartialException(string message)
			: base(message)
		{
		}
	}
}
