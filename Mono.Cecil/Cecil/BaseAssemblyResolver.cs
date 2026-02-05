using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200004E RID: 78
	public abstract class BaseAssemblyResolver : IAssemblyResolver, IDisposable
	{
		// Token: 0x06000308 RID: 776 RVA: 0x0000FB70 File Offset: 0x0000DD70
		public void AddSearchDirectory(string directory)
		{
			this.directories.Add(directory);
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000FB7E File Offset: 0x0000DD7E
		public void RemoveSearchDirectory(string directory)
		{
			this.directories.Remove(directory);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000FB90 File Offset: 0x0000DD90
		public string[] GetSearchDirectories()
		{
			string[] array = new string[this.directories.size];
			Array.Copy(this.directories.items, array, array.Length);
			return array;
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600030B RID: 779 RVA: 0x0000FBC4 File Offset: 0x0000DDC4
		// (remove) Token: 0x0600030C RID: 780 RVA: 0x0000FBFC File Offset: 0x0000DDFC
		public event AssemblyResolveEventHandler ResolveFailure;

		// Token: 0x0600030D RID: 781 RVA: 0x0000FC31 File Offset: 0x0000DE31
		protected BaseAssemblyResolver()
		{
			this.directories = new Collection<string>(2) { ".", "bin" };
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000FC5B File Offset: 0x0000DE5B
		private AssemblyDefinition GetAssembly(string file, ReaderParameters parameters)
		{
			if (parameters.AssemblyResolver == null)
			{
				parameters.AssemblyResolver = this;
			}
			return ModuleDefinition.ReadModule(file, parameters).Assembly;
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000FC78 File Offset: 0x0000DE78
		public virtual AssemblyDefinition Resolve(AssemblyNameReference name)
		{
			return this.Resolve(name, new ReaderParameters());
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000FC88 File Offset: 0x0000DE88
		public virtual AssemblyDefinition Resolve(AssemblyNameReference name, ReaderParameters parameters)
		{
			Mixin.CheckName(name);
			Mixin.CheckParameters(parameters);
			AssemblyDefinition assemblyDefinition = this.SearchDirectory(name, this.directories, parameters);
			if (assemblyDefinition != null)
			{
				return assemblyDefinition;
			}
			if (name.IsRetargetable)
			{
				name = new AssemblyNameReference(name.Name, Mixin.ZeroVersion)
				{
					PublicKeyToken = Empty<byte>.Array
				};
			}
			string directoryName = Path.GetDirectoryName(typeof(object).Module.FullyQualifiedName);
			string[] array;
			if (!BaseAssemblyResolver.on_mono)
			{
				(array = new string[1])[0] = directoryName;
			}
			else
			{
				string[] array2 = new string[2];
				array2[0] = directoryName;
				array = array2;
				array2[1] = Path.Combine(directoryName, "Facades");
			}
			string[] array3 = array;
			if (BaseAssemblyResolver.IsZero(name.Version))
			{
				assemblyDefinition = this.SearchDirectory(name, array3, parameters);
				if (assemblyDefinition != null)
				{
					return assemblyDefinition;
				}
			}
			if (name.Name == "mscorlib")
			{
				assemblyDefinition = this.GetCorlib(name, parameters);
				if (assemblyDefinition != null)
				{
					return assemblyDefinition;
				}
			}
			assemblyDefinition = this.GetAssemblyInGac(name, parameters);
			if (assemblyDefinition != null)
			{
				return assemblyDefinition;
			}
			assemblyDefinition = this.SearchDirectory(name, array3, parameters);
			if (assemblyDefinition != null)
			{
				return assemblyDefinition;
			}
			if (this.ResolveFailure != null)
			{
				assemblyDefinition = this.ResolveFailure(this, name);
				if (assemblyDefinition != null)
				{
					return assemblyDefinition;
				}
			}
			throw new AssemblyResolutionException(name);
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000FD9C File Offset: 0x0000DF9C
		protected virtual AssemblyDefinition SearchDirectory(AssemblyNameReference name, IEnumerable<string> directories, ReaderParameters parameters)
		{
			string[] array2;
			if (!name.IsWindowsRuntime)
			{
				string[] array = new string[2];
				array[0] = ".exe";
				array2 = array;
				array[1] = ".dll";
			}
			else
			{
				string[] array3 = new string[2];
				array3[0] = ".winmd";
				array2 = array3;
				array3[1] = ".dll";
			}
			string[] array4 = array2;
			foreach (string text in directories)
			{
				foreach (string text2 in array4)
				{
					string text3 = Path.Combine(text, name.Name + text2);
					if (File.Exists(text3))
					{
						try
						{
							return this.GetAssembly(text3, parameters);
						}
						catch (BadImageFormatException)
						{
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000FE70 File Offset: 0x0000E070
		private static bool IsZero(Version version)
		{
			return version.Major == 0 && version.Minor == 0 && version.Build == 0 && version.Revision == 0;
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000FE98 File Offset: 0x0000E098
		private AssemblyDefinition GetCorlib(AssemblyNameReference reference, ReaderParameters parameters)
		{
			Version version = reference.Version;
			if (typeof(object).Assembly.GetName().Version == version || BaseAssemblyResolver.IsZero(version))
			{
				return this.GetAssembly(typeof(object).Module.FullyQualifiedName, parameters);
			}
			string text = Directory.GetParent(Directory.GetParent(typeof(object).Module.FullyQualifiedName).FullName).FullName;
			if (!BaseAssemblyResolver.on_mono)
			{
				switch (version.Major)
				{
				case 1:
					if (version.MajorRevision == 3300)
					{
						text = Path.Combine(text, "v1.0.3705");
						goto IL_016C;
					}
					text = Path.Combine(text, "v1.1.4322");
					goto IL_016C;
				case 2:
					text = Path.Combine(text, "v2.0.50727");
					goto IL_016C;
				case 4:
					text = Path.Combine(text, "v4.0.30319");
					goto IL_016C;
				}
				throw new NotSupportedException("Version not supported: " + version);
			}
			if (version.Major == 1)
			{
				text = Path.Combine(text, "1.0");
			}
			else if (version.Major == 2)
			{
				if (version.MajorRevision == 5)
				{
					text = Path.Combine(text, "2.1");
				}
				else
				{
					text = Path.Combine(text, "2.0");
				}
			}
			else
			{
				if (version.Major != 4)
				{
					throw new NotSupportedException("Version not supported: " + version);
				}
				text = Path.Combine(text, "4.0");
			}
			IL_016C:
			string text2 = Path.Combine(text, "mscorlib.dll");
			if (File.Exists(text2))
			{
				return this.GetAssembly(text2, parameters);
			}
			if (BaseAssemblyResolver.on_mono && Directory.Exists(text + "-api"))
			{
				text2 = Path.Combine(text + "-api", "mscorlib.dll");
				if (File.Exists(text2))
				{
					return this.GetAssembly(text2, parameters);
				}
			}
			return null;
		}

		// Token: 0x06000314 RID: 788 RVA: 0x00010070 File Offset: 0x0000E270
		private static Collection<string> GetGacPaths()
		{
			if (BaseAssemblyResolver.on_mono)
			{
				return BaseAssemblyResolver.GetDefaultMonoGacPaths();
			}
			Collection<string> collection = new Collection<string>(2);
			string environmentVariable = Environment.GetEnvironmentVariable("WINDIR");
			if (environmentVariable == null)
			{
				return collection;
			}
			collection.Add(Path.Combine(environmentVariable, "assembly"));
			collection.Add(Path.Combine(environmentVariable, Path.Combine("Microsoft.NET", "assembly")));
			return collection;
		}

		// Token: 0x06000315 RID: 789 RVA: 0x000100D0 File Offset: 0x0000E2D0
		private static Collection<string> GetDefaultMonoGacPaths()
		{
			Collection<string> collection = new Collection<string>(1);
			string currentMonoGac = BaseAssemblyResolver.GetCurrentMonoGac();
			if (currentMonoGac != null)
			{
				collection.Add(currentMonoGac);
			}
			string environmentVariable = Environment.GetEnvironmentVariable("MONO_GAC_PREFIX");
			if (string.IsNullOrEmpty(environmentVariable))
			{
				return collection;
			}
			foreach (string text in environmentVariable.Split(new char[] { Path.PathSeparator }))
			{
				if (!string.IsNullOrEmpty(text))
				{
					string text2 = Path.Combine(Path.Combine(Path.Combine(text, "lib"), "mono"), "gac");
					if (Directory.Exists(text2) && !collection.Contains(currentMonoGac))
					{
						collection.Add(text2);
					}
				}
			}
			return collection;
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0001017C File Offset: 0x0000E37C
		private static string GetCurrentMonoGac()
		{
			return Path.Combine(Directory.GetParent(Path.GetDirectoryName(typeof(object).Module.FullyQualifiedName)).FullName, "gac");
		}

		// Token: 0x06000317 RID: 791 RVA: 0x000101AB File Offset: 0x0000E3AB
		private AssemblyDefinition GetAssemblyInGac(AssemblyNameReference reference, ReaderParameters parameters)
		{
			if (reference.PublicKeyToken == null || reference.PublicKeyToken.Length == 0)
			{
				return null;
			}
			if (this.gac_paths == null)
			{
				this.gac_paths = BaseAssemblyResolver.GetGacPaths();
			}
			if (BaseAssemblyResolver.on_mono)
			{
				return this.GetAssemblyInMonoGac(reference, parameters);
			}
			return this.GetAssemblyInNetGac(reference, parameters);
		}

		// Token: 0x06000318 RID: 792 RVA: 0x000101EC File Offset: 0x0000E3EC
		private AssemblyDefinition GetAssemblyInMonoGac(AssemblyNameReference reference, ReaderParameters parameters)
		{
			for (int i = 0; i < this.gac_paths.Count; i++)
			{
				string text = this.gac_paths[i];
				string assemblyFile = BaseAssemblyResolver.GetAssemblyFile(reference, string.Empty, text);
				if (File.Exists(assemblyFile))
				{
					return this.GetAssembly(assemblyFile, parameters);
				}
			}
			return null;
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0001023C File Offset: 0x0000E43C
		private AssemblyDefinition GetAssemblyInNetGac(AssemblyNameReference reference, ReaderParameters parameters)
		{
			string[] array = new string[] { "GAC_MSIL", "GAC_32", "GAC_64", "GAC" };
			string[] array2 = new string[]
			{
				string.Empty,
				"v4.0_"
			};
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < array.Length; j++)
				{
					string text = Path.Combine(this.gac_paths[i], array[j]);
					string assemblyFile = BaseAssemblyResolver.GetAssemblyFile(reference, array2[i], text);
					if (Directory.Exists(text) && File.Exists(assemblyFile))
					{
						return this.GetAssembly(assemblyFile, parameters);
					}
				}
			}
			return null;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x000102E4 File Offset: 0x0000E4E4
		private static string GetAssemblyFile(AssemblyNameReference reference, string prefix, string gac)
		{
			StringBuilder stringBuilder = new StringBuilder().Append(prefix).Append(reference.Version).Append("__");
			for (int i = 0; i < reference.PublicKeyToken.Length; i++)
			{
				stringBuilder.Append(reference.PublicKeyToken[i].ToString("x2"));
			}
			return Path.Combine(Path.Combine(Path.Combine(gac, reference.Name), stringBuilder.ToString()), reference.Name + ".dll");
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0001036E File Offset: 0x0000E56E
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600031C RID: 796 RVA: 0x00002A0D File Offset: 0x00000C0D
		protected virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x04000081 RID: 129
		private static readonly bool on_mono = Type.GetType("Mono.Runtime") != null;

		// Token: 0x04000082 RID: 130
		private readonly Collection<string> directories;

		// Token: 0x04000083 RID: 131
		private Collection<string> gac_paths;
	}
}
