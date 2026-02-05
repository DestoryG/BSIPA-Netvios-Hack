using System;
using System.IO;
using System.Reflection;
using Mono.Cecil;

namespace IPA.Loader
{
	// Token: 0x02000043 RID: 67
	internal class CecilLibLoader : BaseAssemblyResolver
	{
		// Token: 0x060001A8 RID: 424 RVA: 0x00007558 File Offset: 0x00005758
		public override AssemblyDefinition Resolve(AssemblyNameReference name, ReaderParameters parameters)
		{
			LibLoader.SetupAssemblyFilenames(false);
			if (name.Name == CecilLibLoader.CurrentAssemblyName)
			{
				return AssemblyDefinition.ReadAssembly(CecilLibLoader.CurrentAssemblyPath, parameters);
			}
			string path;
			if (LibLoader.FilenameLocations.TryGetValue(name.Name + ".dll", out path))
			{
				if (File.Exists(path))
				{
					return AssemblyDefinition.ReadAssembly(path, parameters);
				}
			}
			else if (LibLoader.FilenameLocations.TryGetValue(string.Format("{0}.{1}.dll", name.Name, name.Version), out path) && File.Exists(path))
			{
				return AssemblyDefinition.ReadAssembly(path, parameters);
			}
			return base.Resolve(name, parameters);
		}

		// Token: 0x0400009F RID: 159
		private static readonly string CurrentAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;

		// Token: 0x040000A0 RID: 160
		private static readonly string CurrentAssemblyPath = Assembly.GetExecutingAssembly().Location;
	}
}
