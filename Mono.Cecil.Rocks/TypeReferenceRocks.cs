using System;

namespace Mono.Cecil.Rocks
{
	// Token: 0x0200000C RID: 12
	public static class TypeReferenceRocks
	{
		// Token: 0x0600004C RID: 76 RVA: 0x00003D53 File Offset: 0x00001F53
		public static ArrayType MakeArrayType(this TypeReference self)
		{
			return new ArrayType(self);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003D5C File Offset: 0x00001F5C
		public static ArrayType MakeArrayType(this TypeReference self, int rank)
		{
			if (rank == 0)
			{
				throw new ArgumentOutOfRangeException("rank");
			}
			ArrayType arrayType = new ArrayType(self);
			for (int i = 1; i < rank; i++)
			{
				arrayType.Dimensions.Add(default(ArrayDimension));
			}
			return arrayType;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003D9F File Offset: 0x00001F9F
		public static PointerType MakePointerType(this TypeReference self)
		{
			return new PointerType(self);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003DA7 File Offset: 0x00001FA7
		public static ByReferenceType MakeByReferenceType(this TypeReference self)
		{
			return new ByReferenceType(self);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003DAF File Offset: 0x00001FAF
		public static OptionalModifierType MakeOptionalModifierType(this TypeReference self, TypeReference modifierType)
		{
			return new OptionalModifierType(modifierType, self);
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003DB8 File Offset: 0x00001FB8
		public static RequiredModifierType MakeRequiredModifierType(this TypeReference self, TypeReference modifierType)
		{
			return new RequiredModifierType(modifierType, self);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003DC4 File Offset: 0x00001FC4
		public static GenericInstanceType MakeGenericInstanceType(this TypeReference self, params TypeReference[] arguments)
		{
			if (self == null)
			{
				throw new ArgumentNullException("self");
			}
			if (arguments == null)
			{
				throw new ArgumentNullException("arguments");
			}
			if (arguments.Length == 0)
			{
				throw new ArgumentException();
			}
			if (self.GenericParameters.Count != arguments.Length)
			{
				throw new ArgumentException();
			}
			GenericInstanceType genericInstanceType = new GenericInstanceType(self);
			foreach (TypeReference typeReference in arguments)
			{
				genericInstanceType.GenericArguments.Add(typeReference);
			}
			return genericInstanceType;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003E35 File Offset: 0x00002035
		public static PinnedType MakePinnedType(this TypeReference self)
		{
			return new PinnedType(self);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003E3D File Offset: 0x0000203D
		public static SentinelType MakeSentinelType(this TypeReference self)
		{
			return new SentinelType(self);
		}
	}
}
