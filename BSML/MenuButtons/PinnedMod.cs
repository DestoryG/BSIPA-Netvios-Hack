using System;
using System.Runtime.CompilerServices;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Notify;
using IPA.Logging;

namespace BeatSaberMarkupLanguage.MenuButtons
{
	// Token: 0x02000088 RID: 136
	internal class PinnedMod : INotifiableHost
	{
		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060002A4 RID: 676 RVA: 0x0000D68F File Offset: 0x0000B88F
		// (set) Token: 0x060002A5 RID: 677 RVA: 0x0000D697 File Offset: 0x0000B897
		[UIValue("pin-button-text")]
		public string PinButtonText
		{
			get
			{
				return this.pinButtonText;
			}
			set
			{
				this.pinButtonText = value;
				this.NotifyPropertyChanged("PinButtonText");
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060002A6 RID: 678 RVA: 0x0000D6AB File Offset: 0x0000B8AB
		// (set) Token: 0x060002A7 RID: 679 RVA: 0x0000D6B3 File Offset: 0x0000B8B3
		[UIValue("pin-button-stroke-color")]
		public string PinButtonStrokeColor
		{
			get
			{
				return this.pinButtonStrokeColor;
			}
			set
			{
				this.pinButtonStrokeColor = value;
				this.NotifyPropertyChanged("PinButtonStrokeColor");
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060002A8 RID: 680 RVA: 0x0000D6C7 File Offset: 0x0000B8C7
		[UIValue("is-pinned")]
		public bool IsPinned
		{
			get
			{
				return PersistentSingleton<MenuPins>.instance.IsPinned(this.menuButton.Text);
			}
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000D6DE File Offset: 0x0000B8DE
		public PinnedMod(MenuButton menuButton)
		{
			this.menuButton = menuButton;
			this.UpdatePinButtonText();
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000D6F3 File Offset: 0x0000B8F3
		[UIAction("pin-button-click")]
		private void PinButtonClick()
		{
			if (this.IsPinned)
			{
				PersistentSingleton<MenuPins>.instance.UnPinButton(this.menuButton.Text);
			}
			else
			{
				PersistentSingleton<MenuPins>.instance.PinButton(this.menuButton.Text);
			}
			this.UpdatePinButtonText();
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000D72F File Offset: 0x0000B92F
		private void UpdatePinButtonText()
		{
			this.PinButtonText = (this.IsPinned ? "x" : "+");
			this.PinButtonStrokeColor = (this.IsPinned ? "#34eb55" : "white");
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060002AC RID: 684 RVA: 0x0000D768 File Offset: 0x0000B968
		// (remove) Token: 0x060002AD RID: 685 RVA: 0x0000D7A0 File Offset: 0x0000B9A0
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x060002AE RID: 686 RVA: 0x0000D7D8 File Offset: 0x0000B9D8
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

		// Token: 0x04000095 RID: 149
		[UIValue("menu-button")]
		public MenuButton menuButton;

		// Token: 0x04000096 RID: 150
		private string pinButtonText;

		// Token: 0x04000097 RID: 151
		private string pinButtonStrokeColor;
	}
}
