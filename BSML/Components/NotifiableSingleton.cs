using System;
using System.Runtime.CompilerServices;
using BeatSaberMarkupLanguage.Notify;
using IPA.Logging;
using UnityEngine;

namespace BeatSaberMarkupLanguage.Components
{
	// Token: 0x020000A3 RID: 163
	public class NotifiableSingleton<T> : PersistentSingleton<T>, INotifiableHost where T : MonoBehaviour
	{
		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000353 RID: 851 RVA: 0x00010548 File Offset: 0x0000E748
		// (remove) Token: 0x06000354 RID: 852 RVA: 0x00010580 File Offset: 0x0000E780
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x06000355 RID: 853 RVA: 0x000105B8 File Offset: 0x0000E7B8
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
	}
}
