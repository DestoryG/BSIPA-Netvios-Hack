using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security;
using System.Text;

namespace I18N.Common
{
	// Token: 0x02000007 RID: 7
	public class Manager
	{
		// Token: 0x0600003D RID: 61 RVA: 0x00003A0B File Offset: 0x00001C0B
		private Manager()
		{
			this.handlers = new Hashtable(CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
			this.active = new Hashtable(16);
			this.assemblies = new Hashtable(8);
			this.LoadClassList();
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00003A48 File Offset: 0x00001C48
		public static Manager PrimaryManager
		{
			get
			{
				object obj = Manager.lockobj;
				Manager manager;
				lock (obj)
				{
					if (Manager.manager == null)
					{
						Manager.manager = new Manager();
					}
					manager = Manager.manager;
				}
				return manager;
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003A9C File Offset: 0x00001C9C
		private static string Normalize(string name)
		{
			return name.ToLower(CultureInfo.InvariantCulture).Replace('-', '_');
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003AB2 File Offset: 0x00001CB2
		public Encoding GetEncoding(int codePage)
		{
			return this.Instantiate("CP" + codePage.ToString()) as Encoding;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003AD0 File Offset: 0x00001CD0
		public Encoding GetEncoding(string name)
		{
			if (name == null)
			{
				return null;
			}
			string text = name;
			name = Manager.Normalize(name);
			Encoding encoding = this.Instantiate("ENC" + name) as Encoding;
			if (encoding == null)
			{
				encoding = this.Instantiate(name) as Encoding;
			}
			if (encoding == null)
			{
				string alias = Handlers.GetAlias(name);
				if (alias != null)
				{
					encoding = this.Instantiate("ENC" + alias) as Encoding;
					if (encoding == null)
					{
						encoding = this.Instantiate(alias) as Encoding;
					}
				}
			}
			if (encoding == null)
			{
				return null;
			}
			if (text.IndexOf('_') > 0 && encoding.WebName.IndexOf('-') > 0)
			{
				return null;
			}
			if (text.IndexOf('-') > 0 && encoding.WebName.IndexOf('_') > 0)
			{
				return null;
			}
			return encoding;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003B88 File Offset: 0x00001D88
		public CultureInfo GetCulture(int culture, bool useUserOverride)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("0123456789abcdef"[(culture >> 12) & 15]);
			stringBuilder.Append("0123456789abcdef"[(culture >> 8) & 15]);
			stringBuilder.Append("0123456789abcdef"[(culture >> 4) & 15]);
			stringBuilder.Append("0123456789abcdef"[culture & 15]);
			string text = stringBuilder.ToString();
			if (useUserOverride)
			{
				object obj = this.Instantiate("CIDO" + text);
				if (obj != null)
				{
					return obj as CultureInfo;
				}
			}
			return this.Instantiate("CID" + text) as CultureInfo;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00003C30 File Offset: 0x00001E30
		public CultureInfo GetCulture(string name, bool useUserOverride)
		{
			if (name == null)
			{
				return null;
			}
			name = Manager.Normalize(name);
			if (useUserOverride)
			{
				object obj = this.Instantiate("CNO" + name.ToString());
				if (obj != null)
				{
					return obj as CultureInfo;
				}
			}
			return this.Instantiate("CN" + name.ToString()) as CultureInfo;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003C8C File Offset: 0x00001E8C
		internal object Instantiate(string name)
		{
			object obj2;
			lock (this)
			{
				object obj = this.active[name];
				if (obj != null)
				{
					obj2 = obj;
				}
				else
				{
					string text = (string)this.handlers[name];
					if (text == null)
					{
						obj2 = null;
					}
					else
					{
						Assembly assembly = (Assembly)this.assemblies[text];
						if (assembly == null)
						{
							try
							{
								AssemblyName name2 = typeof(Manager).Assembly.GetName();
								name2.Name = text;
								assembly = Assembly.Load(name2);
							}
							catch (SystemException)
							{
								assembly = null;
							}
							if (assembly == null)
							{
								return null;
							}
							this.assemblies[text] = assembly;
						}
						Type type = assembly.GetType(text + "." + name, false, true);
						if (type == null)
						{
							obj2 = null;
						}
						else
						{
							try
							{
								obj = Activator.CreateInstance(type);
							}
							catch (MissingMethodException)
							{
								return null;
							}
							catch (SecurityException)
							{
								return null;
							}
							this.active.Add(name, obj);
							obj2 = obj;
						}
					}
				}
			}
			return obj2;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003DC8 File Offset: 0x00001FC8
		private void LoadClassList()
		{
			FileStream file;
			try
			{
				file = Assembly.GetExecutingAssembly().GetFile("I18N-handlers.def");
				if (file == null)
				{
					this.LoadInternalClasses();
					return;
				}
			}
			catch (FileLoadException)
			{
				this.LoadInternalClasses();
				return;
			}
			StreamReader streamReader = new StreamReader(file);
			string text;
			while ((text = streamReader.ReadLine()) != null)
			{
				if (text.Length != 0 && text[0] != '#')
				{
					int num = text.LastIndexOf('.');
					if (num != -1)
					{
						string text2 = text.Substring(num + 1);
						if (!this.handlers.Contains(text2))
						{
							this.handlers.Add(text2, text.Substring(0, num));
						}
					}
				}
			}
			streamReader.Close();
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003E74 File Offset: 0x00002074
		private void LoadInternalClasses()
		{
			foreach (string text in Handlers.List)
			{
				int num = text.LastIndexOf('.');
				if (num != -1)
				{
					string text2 = text.Substring(num + 1);
					if (!this.handlers.Contains(text2))
					{
						this.handlers.Add(text2, text.Substring(0, num));
					}
				}
			}
		}

		// Token: 0x04000049 RID: 73
		private static Manager manager;

		// Token: 0x0400004A RID: 74
		private Hashtable handlers;

		// Token: 0x0400004B RID: 75
		private Hashtable active;

		// Token: 0x0400004C RID: 76
		private Hashtable assemblies;

		// Token: 0x0400004D RID: 77
		private static readonly object lockobj = new object();

		// Token: 0x0400004E RID: 78
		private const string hex = "0123456789abcdef";
	}
}
