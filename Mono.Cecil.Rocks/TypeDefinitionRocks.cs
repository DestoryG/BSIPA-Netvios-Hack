using System;
using System.Collections.Generic;
using System.Linq;

namespace Mono.Cecil.Rocks
{
	// Token: 0x0200000B RID: 11
	public static class TypeDefinitionRocks
	{
		// Token: 0x06000048 RID: 72 RVA: 0x00003C38 File Offset: 0x00001E38
		public static IEnumerable<MethodDefinition> GetConstructors(this TypeDefinition self)
		{
			if (self == null)
			{
				throw new ArgumentNullException("self");
			}
			if (!self.HasMethods)
			{
				return Empty<MethodDefinition>.Array;
			}
			return self.Methods.Where((MethodDefinition method) => method.IsConstructor);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003C8C File Offset: 0x00001E8C
		public static MethodDefinition GetStaticConstructor(this TypeDefinition self)
		{
			if (self == null)
			{
				throw new ArgumentNullException("self");
			}
			if (!self.HasMethods)
			{
				return null;
			}
			return self.GetConstructors().FirstOrDefault((MethodDefinition ctor) => ctor.IsStatic);
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003CDC File Offset: 0x00001EDC
		public static IEnumerable<MethodDefinition> GetMethods(this TypeDefinition self)
		{
			if (self == null)
			{
				throw new ArgumentNullException("self");
			}
			if (!self.HasMethods)
			{
				return Empty<MethodDefinition>.Array;
			}
			return self.Methods.Where((MethodDefinition method) => !method.IsConstructor);
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003D2F File Offset: 0x00001F2F
		public static TypeReference GetEnumUnderlyingType(this TypeDefinition self)
		{
			if (self == null)
			{
				throw new ArgumentNullException("self");
			}
			if (!self.IsEnum)
			{
				throw new ArgumentException();
			}
			return self.GetEnumUnderlyingType();
		}
	}
}
