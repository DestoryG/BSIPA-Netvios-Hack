using System;
using System.Linq;
using System.Text.RegularExpressions;
using DynamicOpenVR.Logging;

namespace DynamicOpenVR.IO
{
	// Token: 0x020000DA RID: 218
	public abstract class OVRAction : IDisposable
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x00006108 File Offset: 0x00004308
		public string id
		{
			get
			{
				return ((uint)this.GetHashCode()).ToString();
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x00006123 File Offset: 0x00004323
		public string name { get; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x0000612B File Offset: 0x0000432B
		// (set) Token: 0x060001F5 RID: 501 RVA: 0x00006133 File Offset: 0x00004333
		internal ulong handle { get; private set; }

		// Token: 0x060001F6 RID: 502 RVA: 0x0000613C File Offset: 0x0000433C
		protected OVRAction(string name)
		{
			if (!OVRAction.kNameRegex.IsMatch(name))
			{
				throw new Exception("Unexpected action name '" + name + "'; name should only contain letters, numbers, dashes, and underscores.");
			}
			this.name = name.ToLowerInvariant();
			OpenVRActionManager.instance.RegisterAction(this);
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00006189 File Offset: 0x00004389
		internal string GetActionSetName()
		{
			return string.Join("/", this.name.Split(new char[] { '/' }).Take(3));
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x000061B4 File Offset: 0x000043B4
		internal void UpdateHandle()
		{
			this.handle = OpenVRWrapper.GetActionHandle(this.name);
			if (this.handle <= 0UL)
			{
				Logger.Error("Got invalid handle for action '" + this.name + "'. Make sure it is defined in the action manifest and try again.");
				OpenVRActionManager.instance.DeregisterAction(this);
			}
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00006201 File Offset: 0x00004401
		public void Dispose()
		{
			OpenVRActionManager.instance.DeregisterAction(this);
		}

		// Token: 0x04000881 RID: 2177
		private static readonly Regex kNameRegex = new Regex("^\\/actions\\/[a-z0-9_-]+\\/(?:in|out)\\/[a-z0-9_-]+$", RegexOptions.IgnoreCase);
	}
}
