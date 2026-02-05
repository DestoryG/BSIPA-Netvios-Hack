using System;

namespace BeatSaverSharp.Exceptions
{
	// Token: 0x02000018 RID: 24
	public class InvalidPartialHashException : InvalidPartialException
	{
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00003AF5 File Offset: 0x00001CF5
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x00003AFD File Offset: 0x00001CFD
		public string Hash { get; private set; }

		// Token: 0x060000D6 RID: 214 RVA: 0x00003B06 File Offset: 0x00001D06
		public InvalidPartialHashException(string hash)
		{
			this.Hash = hash;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00003B15 File Offset: 0x00001D15
		public InvalidPartialHashException(string hash, string message)
			: base(message)
		{
			this.Hash = hash;
		}
	}
}
