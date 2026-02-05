using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Text.RegularExpressions
{
	// Token: 0x02000698 RID: 1688
	[global::__DynamicallyInvokable]
	[Serializable]
	public class Group : Capture
	{
		// Token: 0x06003EC2 RID: 16066 RVA: 0x00105523 File Offset: 0x00103723
		internal Group(string text, int[] caps, int capcount, string name)
			: base(text, (capcount == 0) ? 0 : caps[(capcount - 1) * 2], (capcount == 0) ? 0 : caps[capcount * 2 - 1])
		{
			this._caps = caps;
			this._capcount = capcount;
			this._name = name;
		}

		// Token: 0x17000EBF RID: 3775
		// (get) Token: 0x06003EC3 RID: 16067 RVA: 0x0010555C File Offset: 0x0010375C
		[global::__DynamicallyInvokable]
		public bool Success
		{
			[global::__DynamicallyInvokable]
			get
			{
				return this._capcount != 0;
			}
		}

		// Token: 0x17000EC0 RID: 3776
		// (get) Token: 0x06003EC4 RID: 16068 RVA: 0x00105567 File Offset: 0x00103767
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000EC1 RID: 3777
		// (get) Token: 0x06003EC5 RID: 16069 RVA: 0x0010556F File Offset: 0x0010376F
		[global::__DynamicallyInvokable]
		public CaptureCollection Captures
		{
			[global::__DynamicallyInvokable]
			get
			{
				if (this._capcoll == null)
				{
					this._capcoll = new CaptureCollection(this);
				}
				return this._capcoll;
			}
		}

		// Token: 0x06003EC6 RID: 16070 RVA: 0x0010558C File Offset: 0x0010378C
		[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
		public static Group Synchronized(Group inner)
		{
			if (inner == null)
			{
				throw new ArgumentNullException("inner");
			}
			CaptureCollection captures = inner.Captures;
			if (inner._capcount > 0)
			{
				Capture capture = captures[0];
			}
			return inner;
		}

		// Token: 0x04002DCF RID: 11727
		internal static Group _emptygroup = new Group(string.Empty, new int[0], 0, string.Empty);

		// Token: 0x04002DD0 RID: 11728
		internal int[] _caps;

		// Token: 0x04002DD1 RID: 11729
		internal int _capcount;

		// Token: 0x04002DD2 RID: 11730
		internal CaptureCollection _capcoll;

		// Token: 0x04002DD3 RID: 11731
		[OptionalField]
		internal string _name;
	}
}
