using System;

namespace Mono.Cecil.Rocks
{
	// Token: 0x02000007 RID: 7
	public static class MethodDefinitionRocks
	{
		// Token: 0x06000039 RID: 57 RVA: 0x0000379C File Offset: 0x0000199C
		public static MethodDefinition GetBaseMethod(this MethodDefinition self)
		{
			if (self == null)
			{
				throw new ArgumentNullException("self");
			}
			if (!self.IsVirtual)
			{
				return self;
			}
			if (self.IsNewSlot)
			{
				return self;
			}
			for (TypeDefinition typeDefinition = MethodDefinitionRocks.ResolveBaseType(self.DeclaringType); typeDefinition != null; typeDefinition = MethodDefinitionRocks.ResolveBaseType(typeDefinition))
			{
				MethodDefinition matchingMethod = MethodDefinitionRocks.GetMatchingMethod(typeDefinition, self);
				if (matchingMethod != null)
				{
					return matchingMethod;
				}
			}
			return self;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000037F4 File Offset: 0x000019F4
		public static MethodDefinition GetOriginalBaseMethod(this MethodDefinition self)
		{
			if (self == null)
			{
				throw new ArgumentNullException("self");
			}
			for (;;)
			{
				MethodDefinition baseMethod = self.GetBaseMethod();
				if (baseMethod == self)
				{
					break;
				}
				self = baseMethod;
			}
			return self;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003820 File Offset: 0x00001A20
		private static TypeDefinition ResolveBaseType(TypeDefinition type)
		{
			if (type == null)
			{
				return null;
			}
			TypeReference baseType = type.BaseType;
			if (baseType == null)
			{
				return null;
			}
			return baseType.Resolve();
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003844 File Offset: 0x00001A44
		private static MethodDefinition GetMatchingMethod(TypeDefinition type, MethodDefinition method)
		{
			return MetadataResolver.GetMethod(type.Methods, method);
		}
	}
}
