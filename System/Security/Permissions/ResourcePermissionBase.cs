using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Security.Permissions
{
	// Token: 0x0200048A RID: 1162
	[SecurityPermission(SecurityAction.InheritanceDemand, ControlEvidence = true, ControlPolicy = true)]
	[Serializable]
	public abstract class ResourcePermissionBase : CodeAccessPermission, IUnrestrictedPermission
	{
		// Token: 0x06002B09 RID: 11017 RVA: 0x000C3A2A File Offset: 0x000C1C2A
		protected ResourcePermissionBase()
		{
		}

		// Token: 0x06002B0A RID: 11018 RVA: 0x000C3A3D File Offset: 0x000C1C3D
		protected ResourcePermissionBase(PermissionState state)
		{
			if (state == PermissionState.Unrestricted)
			{
				this.isUnrestricted = true;
				return;
			}
			if (state == PermissionState.None)
			{
				this.isUnrestricted = false;
				return;
			}
			throw new ArgumentException(SR.GetString("InvalidPermissionState"), "state");
		}

		// Token: 0x06002B0B RID: 11019 RVA: 0x000C3A7B File Offset: 0x000C1C7B
		private static Hashtable CreateHashtable()
		{
			return new Hashtable(StringComparer.OrdinalIgnoreCase);
		}

		// Token: 0x17000A6A RID: 2666
		// (get) Token: 0x06002B0C RID: 11020 RVA: 0x000C3A88 File Offset: 0x000C1C88
		private string ComputerName
		{
			get
			{
				if (ResourcePermissionBase.computerName == null)
				{
					Type typeFromHandle = typeof(ResourcePermissionBase);
					lock (typeFromHandle)
					{
						if (ResourcePermissionBase.computerName == null)
						{
							StringBuilder stringBuilder = new StringBuilder(256);
							int capacity = stringBuilder.Capacity;
							ResourcePermissionBase.UnsafeNativeMethods.GetComputerName(stringBuilder, ref capacity);
							ResourcePermissionBase.computerName = stringBuilder.ToString();
						}
					}
				}
				return ResourcePermissionBase.computerName;
			}
		}

		// Token: 0x17000A6B RID: 2667
		// (get) Token: 0x06002B0D RID: 11021 RVA: 0x000C3B08 File Offset: 0x000C1D08
		private bool IsEmpty
		{
			get
			{
				return !this.isUnrestricted && this.rootTable.Count == 0;
			}
		}

		// Token: 0x17000A6C RID: 2668
		// (get) Token: 0x06002B0E RID: 11022 RVA: 0x000C3B22 File Offset: 0x000C1D22
		// (set) Token: 0x06002B0F RID: 11023 RVA: 0x000C3B2A File Offset: 0x000C1D2A
		protected Type PermissionAccessType
		{
			get
			{
				return this.permissionAccessType;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (!value.IsEnum)
				{
					throw new ArgumentException(SR.GetString("PermissionBadParameterEnum"), "value");
				}
				this.permissionAccessType = value;
			}
		}

		// Token: 0x17000A6D RID: 2669
		// (get) Token: 0x06002B10 RID: 11024 RVA: 0x000C3B64 File Offset: 0x000C1D64
		// (set) Token: 0x06002B11 RID: 11025 RVA: 0x000C3B6C File Offset: 0x000C1D6C
		protected string[] TagNames
		{
			get
			{
				return this.tagNames;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (value.Length == 0)
				{
					throw new ArgumentException(SR.GetString("PermissionInvalidLength", new object[] { "0" }), "value");
				}
				this.tagNames = value;
			}
		}

		// Token: 0x06002B12 RID: 11026 RVA: 0x000C3BAC File Offset: 0x000C1DAC
		protected void AddPermissionAccess(ResourcePermissionBaseEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			if (entry.PermissionAccessPath.Length != this.TagNames.Length)
			{
				throw new InvalidOperationException(SR.GetString("PermissionNumberOfElements"));
			}
			Hashtable hashtable = this.rootTable;
			string[] permissionAccessPath = entry.PermissionAccessPath;
			for (int i = 0; i < permissionAccessPath.Length - 1; i++)
			{
				if (hashtable.ContainsKey(permissionAccessPath[i]))
				{
					hashtable = (Hashtable)hashtable[permissionAccessPath[i]];
				}
				else
				{
					Hashtable hashtable2 = ResourcePermissionBase.CreateHashtable();
					hashtable[permissionAccessPath[i]] = hashtable2;
					hashtable = hashtable2;
				}
			}
			if (hashtable.ContainsKey(permissionAccessPath[permissionAccessPath.Length - 1]))
			{
				throw new InvalidOperationException(SR.GetString("PermissionItemExists"));
			}
			hashtable[permissionAccessPath[permissionAccessPath.Length - 1]] = entry.PermissionAccess;
		}

		// Token: 0x06002B13 RID: 11027 RVA: 0x000C3C6C File Offset: 0x000C1E6C
		protected void Clear()
		{
			this.rootTable.Clear();
		}

		// Token: 0x06002B14 RID: 11028 RVA: 0x000C3C7C File Offset: 0x000C1E7C
		public override IPermission Copy()
		{
			ResourcePermissionBase resourcePermissionBase = this.CreateInstance();
			resourcePermissionBase.tagNames = this.tagNames;
			resourcePermissionBase.permissionAccessType = this.permissionAccessType;
			resourcePermissionBase.isUnrestricted = this.isUnrestricted;
			resourcePermissionBase.rootTable = this.CopyChildren(this.rootTable, 0);
			return resourcePermissionBase;
		}

		// Token: 0x06002B15 RID: 11029 RVA: 0x000C3CC8 File Offset: 0x000C1EC8
		private Hashtable CopyChildren(object currentContent, int tagIndex)
		{
			IDictionaryEnumerator enumerator = ((Hashtable)currentContent).GetEnumerator();
			Hashtable hashtable = ResourcePermissionBase.CreateHashtable();
			while (enumerator.MoveNext())
			{
				if (tagIndex < this.TagNames.Length - 1)
				{
					hashtable[enumerator.Key] = this.CopyChildren(enumerator.Value, tagIndex + 1);
				}
				else
				{
					hashtable[enumerator.Key] = enumerator.Value;
				}
			}
			return hashtable;
		}

		// Token: 0x06002B16 RID: 11030 RVA: 0x000C3D2E File Offset: 0x000C1F2E
		private ResourcePermissionBase CreateInstance()
		{
			new PermissionSet(PermissionState.Unrestricted).Assert();
			return (ResourcePermissionBase)Activator.CreateInstance(base.GetType(), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, null, null);
		}

		// Token: 0x06002B17 RID: 11031 RVA: 0x000C3D53 File Offset: 0x000C1F53
		protected ResourcePermissionBaseEntry[] GetPermissionEntries()
		{
			return this.GetChildrenAccess(this.rootTable, 0);
		}

		// Token: 0x06002B18 RID: 11032 RVA: 0x000C3D64 File Offset: 0x000C1F64
		private ResourcePermissionBaseEntry[] GetChildrenAccess(object currentContent, int tagIndex)
		{
			IDictionaryEnumerator enumerator = ((Hashtable)currentContent).GetEnumerator();
			ArrayList arrayList = new ArrayList();
			while (enumerator.MoveNext())
			{
				if (tagIndex < this.TagNames.Length - 1)
				{
					ResourcePermissionBaseEntry[] childrenAccess = this.GetChildrenAccess(enumerator.Value, tagIndex + 1);
					for (int i = 0; i < childrenAccess.Length; i++)
					{
						childrenAccess[i].PermissionAccessPath[tagIndex] = (string)enumerator.Key;
					}
					arrayList.AddRange(childrenAccess);
				}
				else
				{
					ResourcePermissionBaseEntry resourcePermissionBaseEntry = new ResourcePermissionBaseEntry((int)enumerator.Value, new string[this.TagNames.Length]);
					resourcePermissionBaseEntry.PermissionAccessPath[tagIndex] = (string)enumerator.Key;
					arrayList.Add(resourcePermissionBaseEntry);
				}
			}
			return (ResourcePermissionBaseEntry[])arrayList.ToArray(typeof(ResourcePermissionBaseEntry));
		}

		// Token: 0x06002B19 RID: 11033 RVA: 0x000C3E30 File Offset: 0x000C2030
		public override void FromXml(SecurityElement securityElement)
		{
			if (securityElement == null)
			{
				throw new ArgumentNullException("securityElement");
			}
			if (!securityElement.Tag.Equals("Permission") && !securityElement.Tag.Equals("IPermission"))
			{
				throw new ArgumentException(SR.GetString("Argument_NotAPermissionElement"));
			}
			string text = securityElement.Attribute("version");
			if (text != null && !text.Equals("1"))
			{
				throw new ArgumentException(SR.GetString("Argument_InvalidXMLBadVersion"));
			}
			string text2 = securityElement.Attribute("Unrestricted");
			if (text2 != null && string.Compare(text2, "true", StringComparison.OrdinalIgnoreCase) == 0)
			{
				this.isUnrestricted = true;
				return;
			}
			this.isUnrestricted = false;
			this.rootTable = (Hashtable)this.ReadChildren(securityElement, 0);
		}

		// Token: 0x06002B1A RID: 11034 RVA: 0x000C3EEC File Offset: 0x000C20EC
		public override IPermission Intersect(IPermission target)
		{
			if (target == null)
			{
				return null;
			}
			if (target.GetType() != base.GetType())
			{
				throw new ArgumentException(SR.GetString("PermissionTypeMismatch"), "target");
			}
			ResourcePermissionBase resourcePermissionBase = (ResourcePermissionBase)target;
			if (this.IsUnrestricted())
			{
				return resourcePermissionBase.Copy();
			}
			if (resourcePermissionBase.IsUnrestricted())
			{
				return this.Copy();
			}
			ResourcePermissionBase resourcePermissionBase2 = null;
			Hashtable hashtable = (Hashtable)this.IntersectContents(this.rootTable, resourcePermissionBase.rootTable);
			if (hashtable != null)
			{
				resourcePermissionBase2 = this.CreateInstance();
				resourcePermissionBase2.rootTable = hashtable;
			}
			return resourcePermissionBase2;
		}

		// Token: 0x06002B1B RID: 11035 RVA: 0x000C3F78 File Offset: 0x000C2178
		private object IntersectContents(object currentContent, object targetContent)
		{
			if (currentContent is int)
			{
				int num = (int)currentContent;
				int num2 = (int)targetContent;
				return num & num2;
			}
			Hashtable hashtable = ResourcePermissionBase.CreateHashtable();
			object obj = ((Hashtable)currentContent)["."];
			object obj2 = ((Hashtable)currentContent)[this.ComputerName];
			if (obj != null || obj2 != null)
			{
				object obj3 = ((Hashtable)targetContent)["."];
				object obj4 = ((Hashtable)targetContent)[this.ComputerName];
				if (obj3 != null || obj4 != null)
				{
					object obj5 = obj;
					if (obj != null && obj2 != null)
					{
						obj5 = this.UnionOfContents(obj, obj2);
					}
					else if (obj2 != null)
					{
						obj5 = obj2;
					}
					object obj6 = obj3;
					if (obj3 != null && obj4 != null)
					{
						obj6 = this.UnionOfContents(obj3, obj4);
					}
					else if (obj4 != null)
					{
						obj6 = obj4;
					}
					object obj7 = this.IntersectContents(obj5, obj6);
					if (this.HasContent(obj7))
					{
						if (obj2 != null || obj4 != null)
						{
							hashtable[this.ComputerName] = obj7;
						}
						else
						{
							hashtable["."] = obj7;
						}
					}
				}
			}
			IDictionaryEnumerator dictionaryEnumerator;
			Hashtable hashtable2;
			if (((Hashtable)currentContent).Count < ((Hashtable)targetContent).Count)
			{
				dictionaryEnumerator = ((Hashtable)currentContent).GetEnumerator();
				hashtable2 = (Hashtable)targetContent;
			}
			else
			{
				dictionaryEnumerator = ((Hashtable)targetContent).GetEnumerator();
				hashtable2 = (Hashtable)currentContent;
			}
			while (dictionaryEnumerator.MoveNext())
			{
				string text = (string)dictionaryEnumerator.Key;
				if (hashtable2.ContainsKey(text) && text != "." && text != this.ComputerName)
				{
					object value = dictionaryEnumerator.Value;
					object obj8 = hashtable2[text];
					object obj9 = this.IntersectContents(value, obj8);
					if (this.HasContent(obj9))
					{
						hashtable[text] = obj9;
					}
				}
			}
			if (hashtable.Count <= 0)
			{
				return null;
			}
			return hashtable;
		}

		// Token: 0x06002B1C RID: 11036 RVA: 0x000C414C File Offset: 0x000C234C
		private bool HasContent(object value)
		{
			if (value == null)
			{
				return false;
			}
			if (value is int)
			{
				int num = (int)value;
				return num != 0;
			}
			Hashtable hashtable = (Hashtable)value;
			IDictionaryEnumerator enumerator = hashtable.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (this.HasContent(enumerator.Value))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002B1D RID: 11037 RVA: 0x000C419C File Offset: 0x000C239C
		private bool IsContentSubset(object currentContent, object targetContent)
		{
			if (currentContent is int)
			{
				int num = (int)currentContent;
				int num2 = (int)targetContent;
				return (num & num2) == num;
			}
			Hashtable hashtable = (Hashtable)currentContent;
			Hashtable hashtable2 = (Hashtable)targetContent;
			object obj = hashtable2["*"];
			if (obj != null)
			{
				foreach (object obj2 in hashtable)
				{
					if (!this.IsContentSubset(((DictionaryEntry)obj2).Value, obj))
					{
						return false;
					}
				}
				return true;
			}
			foreach (object obj3 in hashtable)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj3;
				string text = (string)dictionaryEntry.Key;
				if (this.HasContent(dictionaryEntry.Value) && text != "." && text != this.ComputerName)
				{
					if (!hashtable2.ContainsKey(text))
					{
						return false;
					}
					if (!this.IsContentSubset(dictionaryEntry.Value, hashtable2[text]))
					{
						return false;
					}
				}
			}
			object obj4 = this.MergeContents(hashtable["."], hashtable[this.ComputerName]);
			if (obj4 != null)
			{
				object obj5 = this.MergeContents(hashtable2["."], hashtable2[this.ComputerName]);
				if (obj5 != null)
				{
					return this.IsContentSubset(obj4, obj5);
				}
				if (!this.IsEmpty)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002B1E RID: 11038 RVA: 0x000C435C File Offset: 0x000C255C
		private object MergeContents(object content1, object content2)
		{
			if (content1 == null)
			{
				if (content2 == null)
				{
					return null;
				}
				return content2;
			}
			else
			{
				if (content2 == null)
				{
					return content1;
				}
				return this.UnionOfContents(content1, content2);
			}
		}

		// Token: 0x06002B1F RID: 11039 RVA: 0x000C4378 File Offset: 0x000C2578
		public override bool IsSubsetOf(IPermission target)
		{
			if (target == null)
			{
				return this.IsEmpty;
			}
			if (target.GetType() != base.GetType())
			{
				return false;
			}
			ResourcePermissionBase resourcePermissionBase = (ResourcePermissionBase)target;
			return resourcePermissionBase.IsUnrestricted() || (!this.IsUnrestricted() && this.IsContentSubset(this.rootTable, resourcePermissionBase.rootTable));
		}

		// Token: 0x06002B20 RID: 11040 RVA: 0x000C43D1 File Offset: 0x000C25D1
		public bool IsUnrestricted()
		{
			return this.isUnrestricted;
		}

		// Token: 0x06002B21 RID: 11041 RVA: 0x000C43DC File Offset: 0x000C25DC
		private object ReadChildren(SecurityElement securityElement, int tagIndex)
		{
			Hashtable hashtable = ResourcePermissionBase.CreateHashtable();
			if (securityElement.Children != null)
			{
				for (int i = 0; i < securityElement.Children.Count; i++)
				{
					SecurityElement securityElement2 = (SecurityElement)securityElement.Children[i];
					if (securityElement2.Tag == this.TagNames[tagIndex])
					{
						string text = securityElement2.Attribute("name");
						if (tagIndex < this.TagNames.Length - 1)
						{
							hashtable[text] = this.ReadChildren(securityElement2, tagIndex + 1);
						}
						else
						{
							string text2 = securityElement2.Attribute("access");
							int num = 0;
							if (text2 != null)
							{
								num = (int)Enum.Parse(this.PermissionAccessType, text2);
							}
							hashtable[text] = num;
						}
					}
				}
			}
			return hashtable;
		}

		// Token: 0x06002B22 RID: 11042 RVA: 0x000C44A0 File Offset: 0x000C26A0
		protected void RemovePermissionAccess(ResourcePermissionBaseEntry entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}
			if (entry.PermissionAccessPath.Length != this.TagNames.Length)
			{
				throw new InvalidOperationException(SR.GetString("PermissionNumberOfElements"));
			}
			Hashtable hashtable = this.rootTable;
			string[] permissionAccessPath = entry.PermissionAccessPath;
			for (int i = 0; i < permissionAccessPath.Length; i++)
			{
				if (hashtable == null || !hashtable.ContainsKey(permissionAccessPath[i]))
				{
					throw new InvalidOperationException(SR.GetString("PermissionItemDoesntExist"));
				}
				Hashtable hashtable2 = hashtable;
				if (i < permissionAccessPath.Length - 1)
				{
					hashtable = (Hashtable)hashtable[permissionAccessPath[i]];
					if (hashtable.Count == 1)
					{
						hashtable2.Remove(permissionAccessPath[i]);
					}
				}
				else
				{
					hashtable = null;
					hashtable2.Remove(permissionAccessPath[i]);
				}
			}
		}

		// Token: 0x06002B23 RID: 11043 RVA: 0x000C4550 File Offset: 0x000C2750
		public override SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("IPermission");
			Type type = base.GetType();
			securityElement.AddAttribute("class", type.FullName + ", " + type.Module.Assembly.FullName.Replace('"', '\''));
			securityElement.AddAttribute("version", "1");
			if (this.isUnrestricted)
			{
				securityElement.AddAttribute("Unrestricted", "true");
				return securityElement;
			}
			this.WriteChildren(securityElement, this.rootTable, 0);
			return securityElement;
		}

		// Token: 0x06002B24 RID: 11044 RVA: 0x000C45DC File Offset: 0x000C27DC
		public override IPermission Union(IPermission target)
		{
			if (target == null)
			{
				return this.Copy();
			}
			if (target.GetType() != base.GetType())
			{
				throw new ArgumentException(SR.GetString("PermissionTypeMismatch"), "target");
			}
			ResourcePermissionBase resourcePermissionBase = (ResourcePermissionBase)target;
			ResourcePermissionBase resourcePermissionBase2 = null;
			if (this.IsUnrestricted() || resourcePermissionBase.IsUnrestricted())
			{
				resourcePermissionBase2 = this.CreateInstance();
				resourcePermissionBase2.isUnrestricted = true;
			}
			else
			{
				Hashtable hashtable = (Hashtable)this.UnionOfContents(this.rootTable, resourcePermissionBase.rootTable);
				if (hashtable != null)
				{
					resourcePermissionBase2 = this.CreateInstance();
					resourcePermissionBase2.rootTable = hashtable;
				}
			}
			return resourcePermissionBase2;
		}

		// Token: 0x06002B25 RID: 11045 RVA: 0x000C4670 File Offset: 0x000C2870
		private object UnionOfContents(object currentContent, object targetContent)
		{
			if (currentContent is int)
			{
				int num = (int)currentContent;
				int num2 = (int)targetContent;
				return num | num2;
			}
			Hashtable hashtable = ResourcePermissionBase.CreateHashtable();
			IDictionaryEnumerator enumerator = ((Hashtable)currentContent).GetEnumerator();
			IDictionaryEnumerator enumerator2 = ((Hashtable)targetContent).GetEnumerator();
			while (enumerator.MoveNext())
			{
				hashtable[(string)enumerator.Key] = enumerator.Value;
			}
			while (enumerator2.MoveNext())
			{
				if (!hashtable.ContainsKey(enumerator2.Key))
				{
					hashtable[enumerator2.Key] = enumerator2.Value;
				}
				else
				{
					object obj = hashtable[enumerator2.Key];
					object value = enumerator2.Value;
					hashtable[enumerator2.Key] = this.UnionOfContents(obj, value);
				}
			}
			if (hashtable.Count <= 0)
			{
				return null;
			}
			return hashtable;
		}

		// Token: 0x06002B26 RID: 11046 RVA: 0x000C4748 File Offset: 0x000C2948
		private void WriteChildren(SecurityElement currentElement, object currentContent, int tagIndex)
		{
			IDictionaryEnumerator enumerator = ((Hashtable)currentContent).GetEnumerator();
			while (enumerator.MoveNext())
			{
				SecurityElement securityElement = new SecurityElement(this.TagNames[tagIndex]);
				currentElement.AddChild(securityElement);
				securityElement.AddAttribute("name", (string)enumerator.Key);
				if (tagIndex < this.TagNames.Length - 1)
				{
					this.WriteChildren(securityElement, enumerator.Value, tagIndex + 1);
				}
				else
				{
					int num = (int)enumerator.Value;
					if (this.PermissionAccessType != null && num != 0)
					{
						string text = Enum.Format(this.PermissionAccessType, num, "g");
						securityElement.AddAttribute("access", text);
					}
				}
			}
		}

		// Token: 0x04002662 RID: 9826
		private static volatile string computerName;

		// Token: 0x04002663 RID: 9827
		private string[] tagNames;

		// Token: 0x04002664 RID: 9828
		private Type permissionAccessType;

		// Token: 0x04002665 RID: 9829
		private bool isUnrestricted;

		// Token: 0x04002666 RID: 9830
		private Hashtable rootTable = ResourcePermissionBase.CreateHashtable();

		// Token: 0x04002667 RID: 9831
		public const string Any = "*";

		// Token: 0x04002668 RID: 9832
		public const string Local = ".";

		// Token: 0x0200087C RID: 2172
		[SuppressUnmanagedCodeSecurity]
		private static class UnsafeNativeMethods
		{
			// Token: 0x06004571 RID: 17777
			[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto)]
			internal static extern bool GetComputerName(StringBuilder lpBuffer, ref int nSize);
		}
	}
}
