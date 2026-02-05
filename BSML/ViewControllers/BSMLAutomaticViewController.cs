using System;
using System.IO;
using System.Linq;
using System.Reflection;
using BeatSaberMarkupLanguage.Attributes;
using HMUI;
using IPA.Logging;
using UnityEngine;

namespace BeatSaberMarkupLanguage.ViewControllers
{
	// Token: 0x0200000D RID: 13
	public abstract class BSMLAutomaticViewController : BSMLViewController, WatcherGroup.IHotReloadableController
	{
		// Token: 0x06000066 RID: 102 RVA: 0x0000420C File Offset: 0x0000240C
		private static string GetDefaultResourceName(Type type)
		{
			string @namespace = type.Namespace;
			string name = type.Name;
			string text = ((@namespace.Length > 0) ? (@namespace + ".") : "") + name;
			if (!type.Assembly.GetManifestResourceNames().Contains(text))
			{
				return text + ".bsml";
			}
			return text;
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00004269 File Offset: 0x00002469
		public virtual string FallbackContent
		{
			get
			{
				return "<bg>\r\n                                                     <vertical child-control-height='false' child-control-width='true' child-align='UpperCenter' pref-width='110' pad-left='3' pad-right='3'>\r\n                                                       <horizontal bg='panel-top' pad-left='10' pad-right='10' horizontal-fit='PreferredSize' vertical-fit='PreferredSize'>\r\n                                                         <text text='Invalid BSML' font-size='10'/>\r\n                                                       </horizontal>\r\n                                                     </vertical>\r\n                                                     <text-page text='{0}' anchor-min-x='0.1' anchor-max-x='0.9' anchor-max-y='0.8'/>\r\n                                                   </bg>";
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00004270 File Offset: 0x00002470
		public override string Content
		{
			get
			{
				if (this._resourceName == null)
				{
					ViewDefinitionAttribute customAttribute = base.GetType().GetCustomAttribute<ViewDefinitionAttribute>();
					if (customAttribute != null)
					{
						this._resourceName = customAttribute.Definition;
					}
					else
					{
						this._resourceName = BSMLAutomaticViewController.GetDefaultResourceName(base.GetType());
					}
				}
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
					if (string.IsNullOrEmpty(this._content) && !string.IsNullOrEmpty(this._resourceName))
					{
						string contentFilePath = this.ContentFilePath;
						this._content = Utilities.GetResourceContent(base.GetType().Assembly, this._resourceName);
					}
				}
				return this._content;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000069 RID: 105 RVA: 0x000043A8 File Offset: 0x000025A8
		// (set) Token: 0x0600006A RID: 106 RVA: 0x000043B0 File Offset: 0x000025B0
		public bool ContentChanged { get; protected set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600006B RID: 107 RVA: 0x000043B9 File Offset: 0x000025B9
		public string ContentFilePath { get; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600006C RID: 108 RVA: 0x000043C1 File Offset: 0x000025C1
		string WatcherGroup.IHotReloadableController.Name
		{
			get
			{
				return base.name;
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000043CC File Offset: 0x000025CC
		protected BSMLAutomaticViewController()
		{
			HotReloadAttribute customAttribute = base.GetType().GetCustomAttribute<HotReloadAttribute>();
			if (customAttribute == null)
			{
				this.ContentFilePath = null;
				return;
			}
			this.ContentFilePath = Path.ChangeExtension(customAttribute.Path, ".bsml");
		}

		// Token: 0x0600006E RID: 110 RVA: 0x0000440C File Offset: 0x0000260C
		protected override void DidActivate(bool firstActivation, ViewController.ActivationType type)
		{
			if (!string.IsNullOrEmpty(this.ContentFilePath))
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
			}
			else if (firstActivation)
			{
				this.ParseWithFallback();
			}
			Action<bool, ViewController.ActivationType> didActivate = this.didActivate;
			if (didActivate == null)
			{
				return;
			}
			didActivate(firstActivation, type);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x0000446D File Offset: 0x0000266D
		protected override void DidDeactivate(ViewController.DeactivationType deactivationType)
		{
			if (!string.IsNullOrEmpty(this.ContentFilePath))
			{
				this._content = null;
				WatcherGroup.UnregisterViewController(this);
			}
			base.DidDeactivate(deactivationType);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00004494 File Offset: 0x00002694
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
				PersistentSingleton<BSMLParser>.instance.Parse(string.Format(this.FallbackContent, Utilities.EscapeXml(ex.Message)), base.gameObject, null);
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x0000451C File Offset: 0x0000271C
		void WatcherGroup.IHotReloadableController.MarkDirty()
		{
			this.ContentChanged = true;
			this._content = null;
		}

		// Token: 0x06000072 RID: 114 RVA: 0x0000452C File Offset: 0x0000272C
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

		// Token: 0x06000073 RID: 115 RVA: 0x000045B0 File Offset: 0x000027B0
		int WatcherGroup.IHotReloadableController.GetInstanceID()
		{
			return base.GetInstanceID();
		}

		// Token: 0x0400001D RID: 29
		private string _resourceName;

		// Token: 0x0400001E RID: 30
		private string _content;
	}
}
