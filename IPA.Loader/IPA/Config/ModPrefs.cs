using System;
using System.Globalization;
using System.IO;
using IPA.Loader;

namespace IPA.Config
{
	/// <inheritdoc />
	/// <summary>
	/// Allows to get and set preferences for your mod. 
	/// </summary>
	// Token: 0x02000063 RID: 99
	[Obsolete("Uses IniFile, which uses 16 bit system calls. Use BS Utils INI system for now.")]
	public class ModPrefs : IModPrefs
	{
		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x0000DA8C File Offset: 0x0000BC8C
		private static IModPrefs StaticInstance
		{
			get
			{
				ModPrefs modPrefs;
				if ((modPrefs = ModPrefs._staticInstance) == null)
				{
					modPrefs = (ModPrefs._staticInstance = new ModPrefs());
				}
				return modPrefs;
			}
		}

		/// <summary>
		/// Constructs a ModPrefs object for the provide plugin.
		/// </summary>
		/// <param name="plugin">the plugin to get the preferences file for</param>
		// Token: 0x060002D3 RID: 723 RVA: 0x0000DAA2 File Offset: 0x0000BCA2
		public ModPrefs(PluginMetadata plugin)
		{
			this._instance = new IniFile(Path.Combine(Environment.CurrentDirectory, "UserData", "ModPrefs", plugin.Name + ".ini"));
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000DAD9 File Offset: 0x0000BCD9
		private ModPrefs()
		{
			this._instance = new IniFile(Path.Combine(Environment.CurrentDirectory, "UserData", "modprefs.ini"));
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000DB00 File Offset: 0x0000BD00
		string IModPrefs.GetString(string section, string name, string defaultValue, bool autoSave)
		{
			string value = this._instance.IniReadValue(section, name);
			if (value != "")
			{
				return value;
			}
			if (autoSave)
			{
				((IModPrefs)this).SetString(section, name, defaultValue);
			}
			return defaultValue;
		}

		/// <summary>
		/// Gets a string from the ini.
		/// </summary>
		/// <param name="section">Section of the key.</param>
		/// <param name="name">Name of the key.</param>
		/// <param name="defaultValue">Value that should be used when no value is found.</param>
		/// <param name="autoSave">Whether or not the default value should be written if no value is found.</param>
		/// <returns></returns>
		// Token: 0x060002D6 RID: 726 RVA: 0x0000DB38 File Offset: 0x0000BD38
		public static string GetString(string section, string name, string defaultValue = "", bool autoSave = false)
		{
			return ModPrefs.StaticInstance.GetString(section, name, defaultValue, autoSave);
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000DB48 File Offset: 0x0000BD48
		int IModPrefs.GetInt(string section, string name, int defaultValue, bool autoSave)
		{
			int value;
			if (int.TryParse(this._instance.IniReadValue(section, name), out value))
			{
				return value;
			}
			if (autoSave)
			{
				((IModPrefs)this).SetInt(section, name, defaultValue);
			}
			return defaultValue;
		}

		/// <summary>
		/// Gets an int from the ini.
		/// </summary>
		/// <param name="section">Section of the key.</param>
		/// <param name="name">Name of the key.</param>
		/// <param name="defaultValue">Value that should be used when no value is found.</param>
		/// <param name="autoSave">Whether or not the default value should be written if no value is found.</param>
		/// <returns></returns>
		// Token: 0x060002D8 RID: 728 RVA: 0x0000DB7B File Offset: 0x0000BD7B
		public static int GetInt(string section, string name, int defaultValue = 0, bool autoSave = false)
		{
			return ModPrefs.StaticInstance.GetInt(section, name, defaultValue, autoSave);
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000DB8C File Offset: 0x0000BD8C
		float IModPrefs.GetFloat(string section, string name, float defaultValue, bool autoSave)
		{
			float value;
			if (float.TryParse(this._instance.IniReadValue(section, name), out value))
			{
				return value;
			}
			if (autoSave)
			{
				((IModPrefs)this).SetFloat(section, name, defaultValue);
			}
			return defaultValue;
		}

		/// <summary>
		/// Gets a float from the ini.
		/// </summary>
		/// <param name="section">Section of the key.</param>
		/// <param name="name">Name of the key.</param>
		/// <param name="defaultValue">Value that should be used when no value is found.</param>
		/// <param name="autoSave">Whether or not the default value should be written if no value is found.</param>
		/// <returns></returns>
		// Token: 0x060002DA RID: 730 RVA: 0x0000DBBF File Offset: 0x0000BDBF
		public static float GetFloat(string section, string name, float defaultValue = 0f, bool autoSave = false)
		{
			return ModPrefs.StaticInstance.GetFloat(section, name, defaultValue, autoSave);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000DBD0 File Offset: 0x0000BDD0
		bool IModPrefs.GetBool(string section, string name, bool defaultValue, bool autoSave)
		{
			string sVal = ModPrefs.GetString(section, name, null, false);
			if (sVal == "1" || sVal == "0")
			{
				return sVal == "1";
			}
			if (autoSave)
			{
				((IModPrefs)this).SetBool(section, name, defaultValue);
			}
			return defaultValue;
		}

		/// <summary>
		/// Gets a bool from the ini.
		/// </summary>
		/// <param name="section">Section of the key.</param>
		/// <param name="name">Name of the key.</param>
		/// <param name="defaultValue">Value that should be used when no value is found.</param>
		/// <param name="autoSave">Whether or not the default value should be written if no value is found.</param>
		/// <returns></returns>
		// Token: 0x060002DC RID: 732 RVA: 0x0000DC1B File Offset: 0x0000BE1B
		public static bool GetBool(string section, string name, bool defaultValue = false, bool autoSave = false)
		{
			return ModPrefs.StaticInstance.GetBool(section, name, defaultValue, autoSave);
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000DC2B File Offset: 0x0000BE2B
		bool IModPrefs.HasKey(string section, string name)
		{
			string text = this._instance.IniReadValue(section, name);
			return ((text != null) ? text.Length : 0) > 0;
		}

		/// <summary>
		/// Checks whether or not a key exists in the ini.
		/// </summary>
		/// <param name="section">Section of the key.</param>
		/// <param name="name">Name of the key.</param>
		/// <returns></returns>
		// Token: 0x060002DE RID: 734 RVA: 0x0000DC49 File Offset: 0x0000BE49
		public static bool HasKey(string section, string name)
		{
			return ModPrefs.StaticInstance.HasKey(section, name);
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000DC57 File Offset: 0x0000BE57
		void IModPrefs.SetFloat(string section, string name, float value)
		{
			this._instance.IniWriteValue(section, name, value.ToString(CultureInfo.InvariantCulture));
		}

		/// <summary>
		/// Sets a float in the ini.
		/// </summary>
		/// <param name="section">Section of the key.</param>
		/// <param name="name">Name of the key.</param>
		/// <param name="value">Value that should be written.</param>
		// Token: 0x060002E0 RID: 736 RVA: 0x0000DC72 File Offset: 0x0000BE72
		public static void SetFloat(string section, string name, float value)
		{
			ModPrefs.StaticInstance.SetFloat(section, name, value);
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000DC81 File Offset: 0x0000BE81
		void IModPrefs.SetInt(string section, string name, int value)
		{
			this._instance.IniWriteValue(section, name, value.ToString());
		}

		/// <summary>
		/// Sets an int in the ini.
		/// </summary>
		/// <param name="section">Section of the key.</param>
		/// <param name="name">Name of the key.</param>
		/// <param name="value">Value that should be written.</param>
		// Token: 0x060002E2 RID: 738 RVA: 0x0000DC97 File Offset: 0x0000BE97
		public static void SetInt(string section, string name, int value)
		{
			ModPrefs.StaticInstance.SetInt(section, name, value);
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000DCA6 File Offset: 0x0000BEA6
		void IModPrefs.SetString(string section, string name, string value)
		{
			this._instance.IniWriteValue(section, name, value);
		}

		/// <summary>
		/// Sets a string in the ini.
		/// </summary>
		/// <param name="section">Section of the key.</param>
		/// <param name="name">Name of the key.</param>
		/// <param name="value">Value that should be written.</param>
		// Token: 0x060002E4 RID: 740 RVA: 0x0000DCB6 File Offset: 0x0000BEB6
		public static void SetString(string section, string name, string value)
		{
			ModPrefs.StaticInstance.SetString(section, name, value);
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000DCC5 File Offset: 0x0000BEC5
		void IModPrefs.SetBool(string section, string name, bool value)
		{
			this._instance.IniWriteValue(section, name, value ? "1" : "0");
		}

		/// <summary>
		/// Sets a bool in the ini.
		/// </summary>
		/// <param name="section">Section of the key.</param>
		/// <param name="name">Name of the key.</param>
		/// <param name="value">Value that should be written.</param>
		// Token: 0x060002E6 RID: 742 RVA: 0x0000DCE3 File Offset: 0x0000BEE3
		public static void SetBool(string section, string name, bool value)
		{
			ModPrefs.StaticInstance.SetBool(section, name, value);
		}

		// Token: 0x04000109 RID: 265
		private static ModPrefs _staticInstance;

		// Token: 0x0400010A RID: 266
		private readonly IniFile _instance;
	}
}
