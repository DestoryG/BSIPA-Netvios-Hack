using System;

namespace System.Diagnostics
{
	// Token: 0x02000008 RID: 8
	public static class StackFrameExtensions
	{
		// Token: 0x06000009 RID: 9 RVA: 0x0000209C File Offset: 0x0000029C
		[MonoTODO]
		public static IntPtr GetNativeImageBase(this StackFrame stackFrame)
		{
			if (stackFrame == null)
			{
				throw new ArgumentNullException("stackFrame");
			}
			throw new NotImplementedException();
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000020B1 File Offset: 0x000002B1
		[MonoTODO]
		public static IntPtr GetNativeIP(this StackFrame stackFrame)
		{
			if (stackFrame == null)
			{
				throw new ArgumentNullException("stackFrame");
			}
			throw new NotImplementedException();
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000020C6 File Offset: 0x000002C6
		[MonoTODO]
		public static bool HasNativeImage(this StackFrame stackFrame)
		{
			if (stackFrame == null)
			{
				throw new ArgumentNullException("stackFrame");
			}
			throw new NotImplementedException();
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000020DB File Offset: 0x000002DB
		[MonoTODO]
		public static bool HasMethod(this StackFrame stackFrame)
		{
			if (stackFrame == null)
			{
				throw new ArgumentNullException("stackFrame");
			}
			throw new NotImplementedException();
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000020F0 File Offset: 0x000002F0
		[MonoTODO]
		public static bool HasILOffset(this StackFrame stackFrame)
		{
			if (stackFrame == null)
			{
				throw new ArgumentNullException("stackFrame");
			}
			throw new NotImplementedException();
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002105 File Offset: 0x00000305
		[MonoTODO]
		public static bool HasSource(this StackFrame stackFrame)
		{
			if (stackFrame == null)
			{
				throw new ArgumentNullException("stackFrame");
			}
			throw new NotImplementedException();
		}
	}
}
