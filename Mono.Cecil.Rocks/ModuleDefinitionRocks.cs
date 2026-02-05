using System;
using System.Collections.Generic;
using System.Linq;

namespace Mono.Cecil.Rocks
{
	// Token: 0x02000008 RID: 8
	public static class ModuleDefinitionRocks
	{
		// Token: 0x0600003D RID: 61 RVA: 0x00003852 File Offset: 0x00001A52
		public static IEnumerable<TypeDefinition> GetAllTypes(this ModuleDefinition self)
		{
			if (self == null)
			{
				throw new ArgumentNullException("self");
			}
			return self.Types.SelectMany(Functional.Y<TypeDefinition, IEnumerable<TypeDefinition>>((Func<TypeDefinition, IEnumerable<TypeDefinition>> f) => (TypeDefinition type) => type.NestedTypes.SelectMany(f).Prepend(type)));
		}
	}
}
