using System;
using System.Runtime.CompilerServices;
using BeatSaberMarkupLanguage.Notify;
using HMUI;
using IPA.Logging;

namespace BeatSaberMarkupLanguage.ViewControllers
{
	// Token: 0x0200000F RID: 15
	public abstract class BSMLViewController : ViewController, INotifiableHost
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000077 RID: 119
		public abstract string Content { get; }

		// Token: 0x06000078 RID: 120 RVA: 0x000045D8 File Offset: 0x000027D8
		protected override void DidActivate(bool firstActivation, ViewController.ActivationType type)
		{
			if (firstActivation)
			{
				PersistentSingleton<BSMLParser>.instance.Parse(this.Content, base.gameObject, this);
			}
			Action<bool, ViewController.ActivationType> action = this.didActivate;
			if (action == null)
			{
				return;
			}
			action(firstActivation, type);
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000079 RID: 121 RVA: 0x00004608 File Offset: 0x00002808
		// (remove) Token: 0x0600007A RID: 122 RVA: 0x00004640 File Offset: 0x00002840
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x0600007B RID: 123 RVA: 0x00004678 File Offset: 0x00002878
		protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			try
			{
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged != null)
				{
					propertyChanged(this, new PropertyChangedEventArgs(propertyName));
				}
			}
			catch (Exception ex)
			{
				Logger log = Logger.log;
				if (log != null)
				{
					log.Error("Error Invoking PropertyChanged: " + ex.Message);
				}
				Logger log2 = Logger.log;
				if (log2 != null)
				{
					log2.Error(ex);
				}
			}
		}

		// Token: 0x04000021 RID: 33
		public Action<bool, ViewController.ActivationType> didActivate;
	}
}
