using System;
using System.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000105 RID: 261
	internal class DefaultAssemblyResolver : BaseAssemblyResolver
	{
		// Token: 0x060006CB RID: 1739 RVA: 0x0001EDAE File Offset: 0x0001CFAE
		public DefaultAssemblyResolver()
		{
			this.cache = new Dictionary<string, AssemblyDefinition>(StringComparer.Ordinal);
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x0001EDC8 File Offset: 0x0001CFC8
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

		// Token: 0x060006CD RID: 1741 RVA: 0x0001EE10 File Offset: 0x0001D010
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

		// Token: 0x060006CE RID: 1742 RVA: 0x0001EE54 File Offset: 0x0001D054
		protected override void Dispose(bool disposing)
		{
			foreach (AssemblyDefinition assemblyDefinition in this.cache.Values)
			{
				assemblyDefinition.Dispose();
			}
			this.cache.Clear();
			base.Dispose(disposing);
		}

		// Token: 0x0400029C RID: 668
		private readonly IDictionary<string, AssemblyDefinition> cache;
	}
}
