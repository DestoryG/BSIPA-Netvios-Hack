using System;

namespace Valve.VR
{
	// Token: 0x0200008C RID: 140
	public struct VREvent_Keyboard_t
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00004130 File Offset: 0x00002330
		public string cNewInput
		{
			get
			{
				return new string(new char[]
				{
					(char)this.cNewInput0,
					(char)this.cNewInput1,
					(char)this.cNewInput2,
					(char)this.cNewInput3,
					(char)this.cNewInput4,
					(char)this.cNewInput5,
					(char)this.cNewInput6,
					(char)this.cNewInput7
				}).TrimEnd(new char[1]);
			}
		}

		// Token: 0x040005EF RID: 1519
		public byte cNewInput0;

		// Token: 0x040005F0 RID: 1520
		public byte cNewInput1;

		// Token: 0x040005F1 RID: 1521
		public byte cNewInput2;

		// Token: 0x040005F2 RID: 1522
		public byte cNewInput3;

		// Token: 0x040005F3 RID: 1523
		public byte cNewInput4;

		// Token: 0x040005F4 RID: 1524
		public byte cNewInput5;

		// Token: 0x040005F5 RID: 1525
		public byte cNewInput6;

		// Token: 0x040005F6 RID: 1526
		public byte cNewInput7;

		// Token: 0x040005F7 RID: 1527
		public ulong uUserValue;
	}
}
