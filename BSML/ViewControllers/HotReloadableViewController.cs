using System;
using System.IO;
using System.Reflection;
using HMUI;
using IPA.Logging;
using UnityEngine;

namespace BeatSaberMarkupLanguage.ViewControllers
{
	// Token: 0x02000010 RID: 16
	[Obsolete("It is now recommended that you use BSMLAutomaticViewController and it's associated attributes", false)]
	public abstract class HotReloadableViewController : BSMLViewController, WatcherGroup.IHotReloadableController
	{
		// Token: 0x0600007D RID: 125 RVA: 0x000046EC File Offset: 0x000028EC
		public static void RefreshViewController(HotReloadableViewController viewController, bool forceReload = false)
		{
			if (viewController == null)
			{
				return;
			}
			((WatcherGroup.IHotReloadableController)viewController).Refresh(forceReload);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00004700 File Offset: 0x00002900
		void WatcherGroup.IHotReloadableController.Refresh(bool forceReload)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (this.ContentChanged || forceReload)
			{
				try
				{
					this.__Deactivate(1, false);
					for (int i = 0; i < base.transform.childCount; i++)
					{
						Object.Destroy(base.transform.GetChild(i).gameObject);
					}
					this.__Activate(1);
				}
				catch (Exception ex)
				{
					Logger log = Logger.log;
					if (log != null)
					{
						log.Error(ex);
					}
				}
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600007F RID: 127
		public abstract string ResourceName { get; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000080 RID: 128
		public abstract string ContentFilePath { get; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00004784 File Offset: 0x00002984
		public virtual string FallbackContent
		{
			get
			{
				return "<vertical child-control-height='false' child-control-width='true' child-align='UpperCenter' pref-width='110' pad-left='3' pad-right='3'>\r\n                                                      <horizontal bg='panel-top' pad-left='10' pad-right='10' horizontal-fit='PreferredSize' vertical-fit='PreferredSize'>\r\n                                                        <text text='Invalid BSML' font-size='10'/>\r\n                                                      </horizontal>\r\n                                                      <text text='{0}' font-size='5'/>\r\n                                                    </vertical>";
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000082 RID: 130 RVA: 0x0000478C File Offset: 0x0000298C
		public override string Content
		{
			get
			{
				if (string.IsNullOrEmpty(this._content))
				{
					if (!string.IsNullOrEmpty(this.ContentFilePath) && File.Exists(this.ContentFilePath))
					{
						try
						{
							this._content = File.ReadAllText(this.ContentFilePath);
						}
						catch (Exception ex)
						{
							Logger log = Logger.log;
							if (log != null)
							{
								log.Warn(string.Concat(new string[] { "Unable to read file ", this.ContentFilePath, " for ", base.name, ": ", ex.Message }));
							}
							Logger log2 = Logger.log;
							if (log2 != null)
							{
								log2.Debug(ex);
							}
						}
					}
					if (string.IsNullOrEmpty(this._content) && !string.IsNullOrEmpty(this.ResourceName))
					{
						this._content = Utilities.GetResourceContent(Assembly.GetAssembly(base.GetType()), this.ResourceName);
					}
				}
				return this._content;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00004888 File Offset: 0x00002A88
		// (set) Token: 0x06000084 RID: 132 RVA: 0x00004890 File Offset: 0x00002A90
		public bool ContentChanged { get; protected set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000085 RID: 133 RVA: 0x000043C1 File Offset: 0x000025C1
		string WatcherGroup.IHotReloadableController.Name
		{
			get
			{
				return base.name;
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00004899 File Offset: 0x00002A99
		protected override void DidActivate(bool firstActivation, ViewController.ActivationType type)
		{
			if (this.ContentChanged && !firstActivation)
			{
				this.ContentChanged = false;
				this.ParseWithFallback();
			}
			else if (firstActivation)
			{
				this.ParseWithFallback();
			}
			WatcherGroup.RegisterViewController(this);
			Action<bool, ViewController.ActivationType> didActivate = this.didActivate;
			if (didActivate == null)
			{
				return;
			}
			didActivate(firstActivation, type);
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000048D7 File Offset: 0x00002AD7
		protected override void DidDeactivate(ViewController.DeactivationType deactivationType)
		{
			this._content = null;
			WatcherGroup.UnregisterViewController(this);
			base.DidDeactivate(deactivationType);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000048F0 File Offset: 0x00002AF0
		private void ParseWithFallback()
		{
			try
			{
				PersistentSingleton<BSMLParser>.instance.Parse(this.Content, base.gameObject, this);
			}
			catch (Exception ex)
			{
				Logger.log.Error("Error parsing BSML: " + ex.Message);
				Logger.log.Debug(ex);
				PersistentSingleton<BSMLParser>.instance.Parse(string.Format(this.FallbackContent, Utilities.EscapeXml(ex.Message)), base.gameObject, this);
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00004978 File Offset: 0x00002B78
		public void MarkDirty()
		{
			this.ContentChanged = true;
			this._content = null;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000045B0 File Offset: 0x000027B0
		int WatcherGroup.IHotReloadableController.GetInstanceID()
		{
			return base.GetInstanceID();
		}

		// Token: 0x04000023 RID: 35
		private string _content;
	}
}
