using System;
using System.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000054 RID: 84
	public class DefaultAssemblyResolver : BaseAssemblyResolver
	{
		// Token: 0x06000354 RID: 852 RVA: 0x000107A4 File Offset: 0x0000E9A4
		public DefaultAssemblyResolver()
		{
			this.cache = new Dictionary<string, AssemblyDefinition>(StringComparer.Ordinal);
		}

		// Token: 0x06000355 RID: 853 RVA: 0x000107BC File Offset: 0x0000E9BC
		public override AssemblyDefinition Resolve(AssemblyNameReference name)
		{
			Mixin.CheckName(name);
			AssemblyDefinition assemblyDefinition;
			if (this.cache.TryGetValue(name.FullName, out assemblyDefinition))
			{
				return assemblyDefinition;
			}
			assemblyDefinition = base.Resolve(name);
			this.cache[name.FullName] = assemblyDefinition;
			return assemblyDefinition;
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00010804 File Offset: 0x0000EA04
		protected void RegisterAssembly(AssemblyDefinition assembly)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}
			string fullName = assembly.Name.FullName;
			if (this.cache.ContainsKey(fullName))
			{
				return;
			}
			this.cache[fullName] = assembly;
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00010848 File Offset: 0x0000EA48
		protected override void Dispose(bool disposing)
		{
			foreach (AssemblyDefinition assemblyDefinition in this.cache.Values)
			{
				assemblyDefinition.Dispose();
			}
			this.cache.Clear();
			base.Dispose(disposing);
		}

		// Token: 0x04000092 RID: 146
		private readonly IDictionary<string, AssemblyDefinition> cache;
	}
}
