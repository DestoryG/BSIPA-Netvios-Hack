using System;
using System.ComponentModel;

namespace System.Diagnostics
{
	// Token: 0x020004BC RID: 1212
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class StackFrameExtensions
	{
		// Token: 0x06002D4A RID: 11594 RVA: 0x000CBFB8 File Offset: 0x000CA1B8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool HasNativeImage(this StackFrame stackFrame)
		{
			return stackFrame.GetNativeImageBase() != IntPtr.Zero;
		}

		// Token: 0x06002D4B RID: 11595 RVA: 0x000CBFCA File Offset: 0x000CA1CA
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool HasMethod(this StackFrame stackFrame)
		{
			return stackFrame.GetMethod() != null;
		}

		// Token: 0x06002D4C RID: 11596 RVA: 0x000CBFD8 File Offset: 0x000CA1D8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool HasILOffset(this StackFrame stackFrame)
		{
			return stackFrame.GetILOffset() != -1;
		}

		// Token: 0x06002D4D RID: 11597 RVA: 0x000CBFE6 File Offset: 0x000CA1E6
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static bool HasSource(this StackFrame stackFrame)
		{
			return stackFrame.GetFileName() != null;
		}

		// Token: 0x06002D4E RID: 11598 RVA: 0x000CBFF1 File Offset: 0x000CA1F1
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static IntPtr GetNativeIP(this StackFrame stackFrame)
		{
			return IntPtr.Zero;
		}

		// Token: 0x06002D4F RID: 11599 RVA: 0x000CBFF8 File Offset: 0x000CA1F8
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static IntPtr GetNativeImageBase(this StackFrame stackFrame)
		{
			return IntPtr.Zero;
		}
	}
}
