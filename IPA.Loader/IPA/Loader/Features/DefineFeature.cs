using System;
using System.IO;

namespace IPA.Loader.Features
{
	// Token: 0x0200004E RID: 78
	internal class DefineFeature : Feature
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000244 RID: 580 RVA: 0x0000C3F4 File Offset: 0x0000A5F4
		protected internal override bool StoreOnPlugin
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000C3F8 File Offset: 0x0000A5F8
		public override bool Initialize(PluginMetadata meta, string[] parameters)
		{
			if (parameters.Length != 2)
			{
				this.InvalidMessage = "Incorrect number of parameters";
				return false;
			}
			base.RequireLoaded(meta);
			Type type;
			try
			{
				type = meta.Assembly.GetType(parameters[1]);
			}
			catch (ArgumentException)
			{
				this.InvalidMessage = "Invalid type name " + parameters[1];
				return false;
			}
			catch (Exception e) when (e is FileNotFoundException || e is FileLoadException || e is BadImageFormatException)
			{
				string filename = "";
				FileNotFoundException fn = e as FileNotFoundException;
				if (fn == null)
				{
					FileLoadException fl = e as FileLoadException;
					if (fl == null)
					{
						BadImageFormatException bi = e as BadImageFormatException;
						if (bi != null)
						{
							filename = bi.FileName;
						}
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
				return false;
			}
			if (type == null)
			{
				this.InvalidMessage = "Invalid type name " + parameters[1];
				return false;
			}
			bool flag;
			try
			{
				if (Feature.RegisterFeature(parameters[0], type))
				{
					flag = (DefineFeature.NewFeature = true);
				}
				else
				{
					this.InvalidMessage = "Feature with name " + parameters[0] + " already exists";
					flag = false;
				}
			}
			catch (ArgumentException)
			{
				this.InvalidMessage = type.FullName + " not a subclass of Feature";
				flag = false;
			}
			return flag;
		}

		// Token: 0x040000E7 RID: 231
		public static bool NewFeature = true;
	}
}
