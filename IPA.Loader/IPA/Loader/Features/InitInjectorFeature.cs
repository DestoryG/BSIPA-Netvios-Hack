using System;
using System.IO;
using System.Reflection;

namespace IPA.Loader.Features
{
	// Token: 0x0200004F RID: 79
	internal class InitInjectorFeature : Feature
	{
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000248 RID: 584 RVA: 0x0000C584 File Offset: 0x0000A784
		protected internal override bool StoreOnPlugin
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000C588 File Offset: 0x0000A788
		public override bool Initialize(PluginMetadata meta, string[] parameters)
		{
			if (parameters.Length != 2)
			{
				this.InvalidMessage = "Incorrect number of parameters";
				return false;
			}
			base.RequireLoaded(meta);
			string[] methodParts = parameters[1].Split(new char[] { ':' });
			Type type = Type.GetType(parameters[0], false);
			if (type == null)
			{
				this.InvalidMessage = "Could not find type " + parameters[0];
				return false;
			}
			Type getType;
			try
			{
				getType = meta.Assembly.GetType(methodParts[0]);
			}
			catch (ArgumentException)
			{
				this.InvalidMessage = "Invalid type name " + methodParts[0];
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
							goto IL_012D;
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
				IL_012D:
				return false;
			}
			MethodInfo method;
			try
			{
				method = getType.GetMethod(methodParts[1], BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[]
				{
					typeof(object),
					typeof(ParameterInfo),
					typeof(PluginMetadata)
				}, Array.Empty<ParameterModifier>());
			}
			catch (Exception e2)
			{
				this.InvalidMessage = string.Format("Error while loading type: {0}", e2);
				return false;
			}
			if (method == null)
			{
				this.InvalidMessage = "Could not find method " + methodParts[1] + " in type " + methodParts[0];
				return false;
			}
			bool flag;
			try
			{
				PluginInitInjector.InjectParameter del = (PluginInitInjector.InjectParameter)Delegate.CreateDelegate(typeof(PluginInitInjector.InjectParameter), null, method);
				PluginInitInjector.AddInjector(type, del);
				flag = true;
			}
			catch (Exception e3)
			{
				this.InvalidMessage = string.Format("Error generated while creating delegate: {0}", e3);
				flag = false;
			}
			return flag;
		}
	}
}
