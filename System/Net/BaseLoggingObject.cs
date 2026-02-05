using System;

namespace System.Net
{
	// Token: 0x020001C1 RID: 449
	internal class BaseLoggingObject
	{
		// Token: 0x060011AD RID: 4525 RVA: 0x00060180 File Offset: 0x0005E380
		internal BaseLoggingObject()
		{
		}

		// Token: 0x060011AE RID: 4526 RVA: 0x00060188 File Offset: 0x0005E388
		internal virtual void EnterFunc(string funcname)
		{
		}

		// Token: 0x060011AF RID: 4527 RVA: 0x0006018A File Offset: 0x0005E38A
		internal virtual void LeaveFunc(string funcname)
		{
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x0006018C File Offset: 0x0005E38C
		internal virtual void DumpArrayToConsole()
		{
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x0006018E File Offset: 0x0005E38E
		internal virtual void PrintLine(string msg)
		{
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x00060190 File Offset: 0x0005E390
		internal virtual void DumpArray(bool shouldClose)
		{
		}

		// Token: 0x060011B3 RID: 4531 RVA: 0x00060192 File Offset: 0x0005E392
		internal virtual void DumpArrayToFile(bool shouldClose)
		{
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x00060194 File Offset: 0x0005E394
		internal virtual void Flush()
		{
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x00060196 File Offset: 0x0005E396
		internal virtual void Flush(bool close)
		{
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x00060198 File Offset: 0x0005E398
		internal virtual void LoggingMonitorTick()
		{
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x0006019A File Offset: 0x0005E39A
		internal virtual void Dump(byte[] buffer)
		{
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x0006019C File Offset: 0x0005E39C
		internal virtual void Dump(byte[] buffer, int length)
		{
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x0006019E File Offset: 0x0005E39E
		internal virtual void Dump(byte[] buffer, int offset, int length)
		{
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x000601A0 File Offset: 0x0005E3A0
		internal virtual void Dump(IntPtr pBuffer, int offset, int length)
		{
		}
	}
}
