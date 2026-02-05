using System;
using System.Runtime.CompilerServices;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Notify;
using IPA.Logging;

namespace BeatSaberMarkupLanguage.MenuButtons
{
	// Token: 0x02000085 RID: 133
	public class MenuButton : INotifiableHost
	{
		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000283 RID: 643 RVA: 0x0000D162 File Offset: 0x0000B362
		// (set) Token: 0x06000284 RID: 644 RVA: 0x0000D16A File Offset: 0x0000B36A
		public virtual Action OnClick { get; protected set; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000285 RID: 645 RVA: 0x0000D173 File Offset: 0x0000B373
		// (set) Token: 0x06000286 RID: 646 RVA: 0x0000D17B File Offset: 0x0000B37B
		[UIValue("text")]
		public virtual string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				this._text = value;
				this.NotifyPropertyChanged("Text");
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000287 RID: 647 RVA: 0x0000D18F File Offset: 0x0000B38F
		// (set) Token: 0x06000288 RID: 648 RVA: 0x0000D197 File Offset: 0x0000B397
		[UIValue("hover-hint")]
		public virtual string HoverHint
		{
			get
			{
				return this._hoverHint;
			}
			set
			{
				this._hoverHint = value;
				this.NotifyPropertyChanged("HoverHint");
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000289 RID: 649 RVA: 0x0000D1AB File Offset: 0x0000B3AB
		// (set) Token: 0x0600028A RID: 650 RVA: 0x0000D1B3 File Offset: 0x0000B3B3
		[UIValue("interactable")]
		public virtual bool Interactable
		{
			get
			{
				return this._interactable;
			}
			set
			{
				this._interactable = value;
				this.NotifyPropertyChanged("Interactable");
			}
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0000D1C7 File Offset: 0x0000B3C7
		[UIAction("button-click")]
		public void ButtonClicked()
		{
			Action onClick = this.OnClick;
			if (onClick == null)
			{
				return;
			}
			onClick();
		}

		// Token: 0x0600028C RID: 652 RVA: 0x00003B67 File Offset: 0x00001D67
		protected MenuButton()
		{
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0000D1D9 File Offset: 0x0000B3D9
		public MenuButton(string text, string hoverHint, Action onClick, bool interactable = true)
		{
			this.Text = text;
			this.HoverHint = hoverHint ?? string.Empty;
			this.OnClick = onClick;
			this.Interactable = interactable;
		}

		// Token: 0x0600028E RID: 654 RVA: 0x0000D207 File Offset: 0x0000B407
		public MenuButton(string text, Action onClick)
			: this(text, string.Empty, onClick, true)
		{
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600028F RID: 655 RVA: 0x0000D218 File Offset: 0x0000B418
		// (remove) Token: 0x06000290 RID: 656 RVA: 0x0000D250 File Offset: 0x0000B450
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06000291 RID: 657 RVA: 0x0000D288 File Offset: 0x0000B488
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

		// Token: 0x04000089 RID: 137
		private string _text;

		// Token: 0x0400008A RID: 138
		private string _hoverHint;

		// Token: 0x0400008B RID: 139
		private bool _interactable;
	}
}
