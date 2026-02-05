using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace BeatSaberMarkupLanguage.Attributes
{
	// Token: 0x020000B9 RID: 185
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	[Conditional("DEBUG")]
	[Conditional("USE_HOT_RELOAD")]
	public sealed class HotReloadAttribute : Attribute
	{
		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060003F1 RID: 1009 RVA: 0x000125A9 File Offset: 0x000107A9
		public string GivenPath { get; }

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x000125B1 File Offset: 0x000107B1
		// (set) Token: 0x060003F3 RID: 1011 RVA: 0x000125B9 File Offset: 0x000107B9
		public string[] PathMap { get; set; }

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060003F4 RID: 1012 RVA: 0x000125C4 File Offset: 0x000107C4
		public string Path
		{
			get
			{
				if (this._path == null)
				{
					if (this.PathMap != null)
					{
						int num = 0;
						while (num < this.PathMap.Length && num + 1 < this.PathMap.Length)
						{
							if (this.GivenPath.StartsWith(this.PathMap[num]))
							{
								this._path = this.PathMap[num + 1] + this.GivenPath.Substring(this.PathMap[num].Length);
								break;
							}
							num += 2;
						}
					}
					if (this._path == null)
					{
						this._path = this.GivenPath;
					}
				}
				return this._path;
			}
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x00012662 File Offset: 0x00010862
		public HotReloadAttribute([CallerFilePath] string basePath = null)
		{
			this.GivenPath = basePath;
		}

		// Token: 0x04000137 RID: 311
		private string _path;
	}
}
