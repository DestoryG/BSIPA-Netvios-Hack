using System;

namespace BeatSaverSharp.Exceptions
{
	// Token: 0x02000017 RID: 23
	public class InvalidPartialKeyException : InvalidPartialException
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00003AC5 File Offset: 0x00001CC5
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x00003ACD File Offset: 0x00001CCD
		public string Key { get; private set; }

		// Token: 0x060000D2 RID: 210 RVA: 0x00003AD6 File Offset: 0x00001CD6
		public InvalidPartialKeyException(string key)
		{
			this.Key = key;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00003AE5 File Offset: 0x00001CE5
		public InvalidPartialKeyException(string key, string message)
			: base(message)
		{
			this.Key = key;
		}
	}
}
