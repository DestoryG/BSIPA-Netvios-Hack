using System;
using System.IO;
using IPA.Config;

namespace IPA.Loader.Features
{
	// Token: 0x0200004D RID: 77
	internal class ConfigProviderFeature : Feature
	{
		// Token: 0x06000242 RID: 578 RVA: 0x0000C2AC File Offset: 0x0000A4AC
		public override bool Initialize(PluginMetadata meta, string[] parameters)
		{
			if (parameters.Length != 1)
			{
				this.InvalidMessage = "Incorrect number of parameters";
				return false;
			}
			base.RequireLoaded(meta);
			Type getType;
			try
			{
				getType = meta.Assembly.GetType(parameters[0]);
			}
			catch (ArgumentException)
			{
				this.InvalidMessage = "Invalid type name " + parameters[0];
				return false;
			}
			catch (Exception e) when (e is FileNotFoundException || e is FileLoadException || e is BadImageFormatException)
			{
				FileNotFoundException fn = e as FileNotFoundException;
				string filename;
				if (fn == null)
				{
					FileLoadException fl = e as FileLoadException;
					if (fl == null)
					{
						BadImageFormatException bi = e as BadImageFormatException;
						if (bi == null)
						{
							this.InvalidMessage = string.Format("Error while loading type: {0}", e);
							goto IL_00E3;
						}
						filename = bi.FileName;
					}
					else
					{
						filename = fl.FileName;
					}
				}
				else
				{
					filename = fn.FileName;
				}
				this.InvalidMessage = "Could not find " + filename + " while loading type";
				IL_00E3:
				return false;
			}
			bool flag;
			try
			{
				Config.Register(getType);
				flag = true;
			}
			catch (Exception e2)
			{
				this.InvalidMessage = string.Format("Error while registering config provider: {0}", e2);
				flag = false;
			}
			return flag;
		}
	}
}
