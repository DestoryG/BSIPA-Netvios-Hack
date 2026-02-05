using System;

namespace IPA.Config
{
	/// <summary>
	/// Allows to get and set preferences for your mod. 
	/// </summary>
	// Token: 0x02000062 RID: 98
	[Obsolete("Uses IniFile, which uses 16 bit system calls. Use BS Utils INI system for now.")]
	public interface IModPrefs
	{
		/// <summary>
		/// Gets a string from the ini.
		/// </summary>
		/// <param name="section">Section of the key.</param>
		/// <param name="name">Name of the key.</param>
		/// <param name="defaultValue">Value that should be used when no value is found.</param>
		/// <param name="autoSave">Whether or not the default value should be written if no value is found.</param>
		/// <returns></returns>
		// Token: 0x060002C9 RID: 713
		string GetString(string section, string name, string defaultValue = "", bool autoSave = false);

		/// <summary>
		/// Gets an int from the ini.
		/// </summary>
		/// <param name="section">Section of the key.</param>
		/// <param name="name">Name of the key.</param>
		/// <param name="defaultValue">Value that should be used when no value is found.</param>
		/// <param name="autoSave">Whether or not the default value should be written if no value is found.</param>
		/// <returns></returns>
		// Token: 0x060002CA RID: 714
		int GetInt(string section, string name, int defaultValue = 0, bool autoSave = false);

		/// <summary>
		/// Gets a float from the ini.
		/// </summary>
		/// <param name="section">Section of the key.</param>
		/// <param name="name">Name of the key.</param>
		/// <param name="defaultValue">Value that should be used when no value is found.</param>
		/// <param name="autoSave">Whether or not the default value should be written if no value is found.</param>
		/// <returns></returns>
		// Token: 0x060002CB RID: 715
		float GetFloat(string section, string name, float defaultValue = 0f, bool autoSave = false);

		/// <summary>
		/// Gets a bool from the ini.
		/// </summary>
		/// <param name="section">Section of the key.</param>
		/// <param name="name">Name of the key.</param>
		/// <param name="defaultValue">Value that should be used when no value is found.</param>
		/// <param name="autoSave">Whether or not the default value should be written if no value is found.</param>
		/// <returns></returns>
		// Token: 0x060002CC RID: 716
		bool GetBool(string section, string name, bool defaultValue = false, bool autoSave = false);

		/// <summary>
		/// Checks whether or not a key exists in the ini.
		/// </summary>
		/// <param name="section">Section of the key.</param>
		/// <param name="name">Name of the key.</param>
		/// <returns></returns>
		// Token: 0x060002CD RID: 717
		bool HasKey(string section, string name);

		/// <summary>
		/// Sets a float in the ini.
		/// </summary>
		/// <param name="section">Section of the key.</param>
		/// <param name="name">Name of the key.</param>
		/// <param name="value">Value that should be written.</param>
		// Token: 0x060002CE RID: 718
		void SetFloat(string section, string name, float value);

		/// <summary>
		/// Sets an int in the ini.
		/// </summary>
		/// <param name="section">Section of the key.</param>
		/// <param name="name">Name of the key.</param>
		/// <param name="value">Value that should be written.</param>
		// Token: 0x060002CF RID: 719
		void SetInt(string section, string name, int value);

		/// <summary>
		/// Sets a string in the ini.
		/// </summary>
		/// <param name="section">Section of the key.</param>
		/// <param name="name">Name of the key.</param>
		/// <param name="value">Value that should be written.</param>
		// Token: 0x060002D0 RID: 720
		void SetString(string section, string name, string value);

		/// <summary>
		/// Sets a bool in the ini.
		/// </summary>
		/// <param name="section">Section of the key.</param>
		/// <param name="name">Name of the key.</param>
		/// <param name="value">Value that should be written.</param>
		// Token: 0x060002D1 RID: 721
		void SetBool(string section, string name, bool value);
	}
}
