using System;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Permissions;

namespace System.Net
{
	// Token: 0x0200020C RID: 524
	internal class SpnDictionary : StringDictionary
	{
		// Token: 0x06001384 RID: 4996 RVA: 0x00066BD3 File Offset: 0x00064DD3
		internal SpnDictionary()
		{
		}

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x06001385 RID: 4997 RVA: 0x00066BEB File Offset: 0x00064DEB
		public override int Count
		{
			get
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				return this.m_SyncTable.Count;
			}
		}

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x06001386 RID: 4998 RVA: 0x00066C02 File Offset: 0x00064E02
		public override bool IsSynchronized
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001387 RID: 4999 RVA: 0x00066C08 File Offset: 0x00064E08
		internal SpnToken InternalGet(string canonicalKey)
		{
			int num = 0;
			string text = null;
			object syncRoot = this.m_SyncTable.SyncRoot;
			lock (syncRoot)
			{
				foreach (object obj in this.m_SyncTable.Keys)
				{
					string text2 = (string)obj;
					if (text2 != null && text2.Length > num && string.Compare(text2, 0, canonicalKey, 0, text2.Length, StringComparison.OrdinalIgnoreCase) == 0)
					{
						num = text2.Length;
						text = text2;
					}
				}
			}
			if (text == null)
			{
				return null;
			}
			return (SpnToken)this.m_SyncTable[text];
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x00066CE0 File Offset: 0x00064EE0
		internal void InternalSet(string canonicalKey, SpnToken spnToken)
		{
			this.m_SyncTable[canonicalKey] = spnToken;
		}

		// Token: 0x17000420 RID: 1056
		public override string this[string key]
		{
			get
			{
				key = SpnDictionary.GetCanonicalKey(key);
				SpnToken spnToken = this.InternalGet(key);
				if (spnToken != null)
				{
					return spnToken.Spn;
				}
				return null;
			}
			set
			{
				key = SpnDictionary.GetCanonicalKey(key);
				this.InternalSet(key, new SpnToken(value));
			}
		}

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x0600138B RID: 5003 RVA: 0x00066D2F File Offset: 0x00064F2F
		public override ICollection Keys
		{
			get
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				return this.m_SyncTable.Keys;
			}
		}

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x0600138C RID: 5004 RVA: 0x00066D46 File Offset: 0x00064F46
		public override object SyncRoot
		{
			[HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
			get
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				return this.m_SyncTable;
			}
		}

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x0600138D RID: 5005 RVA: 0x00066D58 File Offset: 0x00064F58
		public override ICollection Values
		{
			get
			{
				ExceptionHelper.WebPermissionUnrestricted.Demand();
				if (this.m_ValuesWrapper == null)
				{
					this.m_ValuesWrapper = new SpnDictionary.ValueCollection(this);
				}
				return this.m_ValuesWrapper;
			}
		}

		// Token: 0x0600138E RID: 5006 RVA: 0x00066D7E File Offset: 0x00064F7E
		public override void Add(string key, string value)
		{
			key = SpnDictionary.GetCanonicalKey(key);
			this.m_SyncTable.Add(key, new SpnToken(value));
		}

		// Token: 0x0600138F RID: 5007 RVA: 0x00066D9A File Offset: 0x00064F9A
		public override void Clear()
		{
			ExceptionHelper.WebPermissionUnrestricted.Demand();
			this.m_SyncTable.Clear();
		}

		// Token: 0x06001390 RID: 5008 RVA: 0x00066DB1 File Offset: 0x00064FB1
		public override bool ContainsKey(string key)
		{
			key = SpnDictionary.GetCanonicalKey(key);
			return this.m_SyncTable.ContainsKey(key);
		}

		// Token: 0x06001391 RID: 5009 RVA: 0x00066DC8 File Offset: 0x00064FC8
		public override bool ContainsValue(string value)
		{
			ExceptionHelper.WebPermissionUnrestricted.Demand();
			foreach (object obj in this.m_SyncTable.Values)
			{
				SpnToken spnToken = (SpnToken)obj;
				if (spnToken.Spn == value)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x00066E40 File Offset: 0x00065040
		public override void CopyTo(Array array, int index)
		{
			ExceptionHelper.WebPermissionUnrestricted.Demand();
			SpnDictionary.CheckCopyToArguments(array, index, this.Count);
			int num = 0;
			foreach (object obj in this)
			{
				array.SetValue(obj, num + index);
				num++;
			}
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x00066EB0 File Offset: 0x000650B0
		public override IEnumerator GetEnumerator()
		{
			ExceptionHelper.WebPermissionUnrestricted.Demand();
			foreach (object obj in this.m_SyncTable.Keys)
			{
				string text = (string)obj;
				SpnToken spnToken = (SpnToken)this.m_SyncTable[text];
				yield return new DictionaryEntry(text, spnToken.Spn);
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06001394 RID: 5012 RVA: 0x00066EBF File Offset: 0x000650BF
		public override void Remove(string key)
		{
			key = SpnDictionary.GetCanonicalKey(key);
			this.m_SyncTable.Remove(key);
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x00066ED8 File Offset: 0x000650D8
		private static string GetCanonicalKey(string key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			try
			{
				Uri uri = new Uri(key);
				key = uri.GetParts(UriComponents.Scheme | UriComponents.Host | UriComponents.Port | UriComponents.Path, UriFormat.SafeUnescaped);
				new WebPermission(NetworkAccess.Connect, new Uri(key)).Demand();
			}
			catch (UriFormatException ex)
			{
				throw new ArgumentException(SR.GetString("net_mustbeuri", new object[] { "key" }), "key", ex);
			}
			return key;
		}

		// Token: 0x06001396 RID: 5014 RVA: 0x00066F50 File Offset: 0x00065150
		private static void CheckCopyToArguments(Array array, int index, int count)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (array.Rank != 1)
			{
				throw new ArgumentException(SR.GetString("Arg_RankMultiDimNotSupported"));
			}
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", SR.GetString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (array.Length - index < count)
			{
				throw new ArgumentException(SR.GetString("Arg_ArrayPlusOffTooSmall"));
			}
		}

		// Token: 0x04001560 RID: 5472
		private Hashtable m_SyncTable = Hashtable.Synchronized(new Hashtable());

		// Token: 0x04001561 RID: 5473
		private SpnDictionary.ValueCollection m_ValuesWrapper;

		// Token: 0x0200075A RID: 1882
		private class ValueCollection : ICollection, IEnumerable
		{
			// Token: 0x06004208 RID: 16904 RVA: 0x00112423 File Offset: 0x00110623
			internal ValueCollection(SpnDictionary spnDictionary)
			{
				this.spnDictionary = spnDictionary;
			}

			// Token: 0x06004209 RID: 16905 RVA: 0x00112434 File Offset: 0x00110634
			public void CopyTo(Array array, int index)
			{
				SpnDictionary.CheckCopyToArguments(array, index, this.Count);
				int num = 0;
				foreach (object obj in this)
				{
					array.SetValue(obj, num + index);
					num++;
				}
			}

			// Token: 0x17000F16 RID: 3862
			// (get) Token: 0x0600420A RID: 16906 RVA: 0x0011249C File Offset: 0x0011069C
			public int Count
			{
				get
				{
					return this.spnDictionary.m_SyncTable.Values.Count;
				}
			}

			// Token: 0x17000F17 RID: 3863
			// (get) Token: 0x0600420B RID: 16907 RVA: 0x001124B3 File Offset: 0x001106B3
			public bool IsSynchronized
			{
				get
				{
					return true;
				}
			}

			// Token: 0x17000F18 RID: 3864
			// (get) Token: 0x0600420C RID: 16908 RVA: 0x001124B6 File Offset: 0x001106B6
			public object SyncRoot
			{
				get
				{
					return this.spnDictionary.m_SyncTable.SyncRoot;
				}
			}

			// Token: 0x0600420D RID: 16909 RVA: 0x001124C8 File Offset: 0x001106C8
			public IEnumerator GetEnumerator()
			{
				foreach (object obj in this.spnDictionary.m_SyncTable.Values)
				{
					SpnToken spnToken = (SpnToken)obj;
					yield return (spnToken != null) ? spnToken.Spn : null;
				}
				IEnumerator enumerator = null;
				yield break;
				yield break;
			}

			// Token: 0x0400321F RID: 12831
			private SpnDictionary spnDictionary;
		}
	}
}
