using System;
using System.ComponentModel;

namespace Microsoft.CSharp.RuntimeBinder
{
	// Token: 0x02000008 RID: 8
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class CSharpArgumentInfo
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000026 RID: 38 RVA: 0x00002A34 File Offset: 0x00000C34
		private CSharpArgumentInfoFlags Flags { get; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000027 RID: 39 RVA: 0x00002A3C File Offset: 0x00000C3C
		internal string Name { get; }

		// Token: 0x06000028 RID: 40 RVA: 0x00002A44 File Offset: 0x00000C44
		private CSharpArgumentInfo(CSharpArgumentInfoFlags flags, string name)
		{
			this.Flags = flags;
			this.Name = name;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002A5A File Offset: 0x00000C5A
		public static CSharpArgumentInfo Create(CSharpArgumentInfoFlags flags, string name)
		{
			return new CSharpArgumentInfo(flags, name);
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00002A63 File Offset: 0x00000C63
		internal bool UseCompileTimeType
		{
			get
			{
				return (this.Flags & CSharpArgumentInfoFlags.UseCompileTimeType) > CSharpArgumentInfoFlags.None;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002A70 File Offset: 0x00000C70
		internal bool LiteralConstant
		{
			get
			{
				return (this.Flags & CSharpArgumentInfoFlags.Constant) > CSharpArgumentInfoFlags.None;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00002A7D File Offset: 0x00000C7D
		internal bool NamedArgument
		{
			get
			{
				return (this.Flags & CSharpArgumentInfoFlags.NamedArgument) > CSharpArgumentInfoFlags.None;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002A8A File Offset: 0x00000C8A
		internal bool IsByRefOrOut
		{
			get
			{
				return (this.Flags & (CSharpArgumentInfoFlags.IsRef | CSharpArgumentInfoFlags.IsOut)) > CSharpArgumentInfoFlags.None;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002A98 File Offset: 0x00000C98
		internal bool IsOut
		{
			get
			{
				return (this.Flags & CSharpArgumentInfoFlags.IsOut) > CSharpArgumentInfoFlags.None;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002AA6 File Offset: 0x00000CA6
		internal bool IsStaticType
		{
			get
			{
				return (this.Flags & CSharpArgumentInfoFlags.IsStaticType) > CSharpArgumentInfoFlags.None;
			}
		}

		// Token: 0x04000090 RID: 144
		internal static readonly CSharpArgumentInfo None = new CSharpArgumentInfo(CSharpArgumentInfoFlags.None, null);
	}
}
