using System;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200017F RID: 383
	internal abstract class TypeSystem
	{
		// Token: 0x06000C1A RID: 3098 RVA: 0x00027E85 File Offset: 0x00026085
		private TypeSystem(ModuleDefinition module)
		{
			this.module = module;
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x00027E94 File Offset: 0x00026094
		internal static TypeSystem CreateTypeSystem(ModuleDefinition module)
		{
			if (module.IsCoreLibrary())
			{
				return new TypeSystem.CoreTypeSystem(module);
			}
			return new TypeSystem.CommonTypeSystem(module);
		}

		// Token: 0x06000C1C RID: 3100
		internal abstract TypeReference LookupType(string @namespace, string name);

		// Token: 0x06000C1D RID: 3101 RVA: 0x00027EAC File Offset: 0x000260AC
		private TypeReference LookupSystemType(ref TypeReference reference, string name, ElementType element_type)
		{
			object syncRoot = this.module.SyncRoot;
			TypeReference typeReference;
			lock (syncRoot)
			{
				if (reference != null)
				{
					typeReference = reference;
				}
				else
				{
					TypeReference typeReference2 = this.LookupType("System", name);
					typeReference2.etype = element_type;
					TypeReference typeReference3;
					reference = (typeReference3 = typeReference2);
					typeReference = typeReference3;
				}
			}
			return typeReference;
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x00027F14 File Offset: 0x00026114
		private TypeReference LookupSystemValueType(ref TypeReference typeRef, string name, ElementType element_type)
		{
			object syncRoot = this.module.SyncRoot;
			TypeReference typeReference;
			lock (syncRoot)
			{
				if (typeRef != null)
				{
					typeReference = typeRef;
				}
				else
				{
					TypeReference typeReference2 = this.LookupType("System", name);
					typeReference2.etype = element_type;
					typeReference2.KnownValueType();
					TypeReference typeReference3;
					typeRef = (typeReference3 = typeReference2);
					typeReference = typeReference3;
				}
			}
			return typeReference;
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000C1F RID: 3103 RVA: 0x00027F80 File Offset: 0x00026180
		[Obsolete("Use CoreLibrary")]
		public IMetadataScope Corlib
		{
			get
			{
				return this.CoreLibrary;
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000C20 RID: 3104 RVA: 0x00027F88 File Offset: 0x00026188
		public IMetadataScope CoreLibrary
		{
			get
			{
				TypeSystem.CommonTypeSystem commonTypeSystem = this as TypeSystem.CommonTypeSystem;
				if (commonTypeSystem == null)
				{
					return this.module;
				}
				return commonTypeSystem.GetCoreLibraryReference();
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000C21 RID: 3105 RVA: 0x00027FAC File Offset: 0x000261AC
		public TypeReference Object
		{
			get
			{
				return this.type_object ?? this.LookupSystemType(ref this.type_object, "Object", ElementType.Object);
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000C22 RID: 3106 RVA: 0x00027FCB File Offset: 0x000261CB
		public TypeReference Void
		{
			get
			{
				return this.type_void ?? this.LookupSystemType(ref this.type_void, "Void", ElementType.Void);
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000C23 RID: 3107 RVA: 0x00027FE9 File Offset: 0x000261E9
		public TypeReference Boolean
		{
			get
			{
				return this.type_bool ?? this.LookupSystemValueType(ref this.type_bool, "Boolean", ElementType.Boolean);
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000C24 RID: 3108 RVA: 0x00028007 File Offset: 0x00026207
		public TypeReference Char
		{
			get
			{
				return this.type_char ?? this.LookupSystemValueType(ref this.type_char, "Char", ElementType.Char);
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000C25 RID: 3109 RVA: 0x00028025 File Offset: 0x00026225
		public TypeReference SByte
		{
			get
			{
				return this.type_sbyte ?? this.LookupSystemValueType(ref this.type_sbyte, "SByte", ElementType.I1);
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000C26 RID: 3110 RVA: 0x00028043 File Offset: 0x00026243
		public TypeReference Byte
		{
			get
			{
				return this.type_byte ?? this.LookupSystemValueType(ref this.type_byte, "Byte", ElementType.U1);
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000C27 RID: 3111 RVA: 0x00028061 File Offset: 0x00026261
		public TypeReference Int16
		{
			get
			{
				return this.type_int16 ?? this.LookupSystemValueType(ref this.type_int16, "Int16", ElementType.I2);
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000C28 RID: 3112 RVA: 0x0002807F File Offset: 0x0002627F
		public TypeReference UInt16
		{
			get
			{
				return this.type_uint16 ?? this.LookupSystemValueType(ref this.type_uint16, "UInt16", ElementType.U2);
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000C29 RID: 3113 RVA: 0x0002809D File Offset: 0x0002629D
		public TypeReference Int32
		{
			get
			{
				return this.type_int32 ?? this.LookupSystemValueType(ref this.type_int32, "Int32", ElementType.I4);
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000C2A RID: 3114 RVA: 0x000280BB File Offset: 0x000262BB
		public TypeReference UInt32
		{
			get
			{
				return this.type_uint32 ?? this.LookupSystemValueType(ref this.type_uint32, "UInt32", ElementType.U4);
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000C2B RID: 3115 RVA: 0x000280DA File Offset: 0x000262DA
		public TypeReference Int64
		{
			get
			{
				return this.type_int64 ?? this.LookupSystemValueType(ref this.type_int64, "Int64", ElementType.I8);
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000C2C RID: 3116 RVA: 0x000280F9 File Offset: 0x000262F9
		public TypeReference UInt64
		{
			get
			{
				return this.type_uint64 ?? this.LookupSystemValueType(ref this.type_uint64, "UInt64", ElementType.U8);
			}
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000C2D RID: 3117 RVA: 0x00028118 File Offset: 0x00026318
		public TypeReference Single
		{
			get
			{
				return this.type_single ?? this.LookupSystemValueType(ref this.type_single, "Single", ElementType.R4);
			}
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000C2E RID: 3118 RVA: 0x00028137 File Offset: 0x00026337
		public TypeReference Double
		{
			get
			{
				return this.type_double ?? this.LookupSystemValueType(ref this.type_double, "Double", ElementType.R8);
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000C2F RID: 3119 RVA: 0x00028156 File Offset: 0x00026356
		public TypeReference IntPtr
		{
			get
			{
				return this.type_intptr ?? this.LookupSystemValueType(ref this.type_intptr, "IntPtr", ElementType.I);
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000C30 RID: 3120 RVA: 0x00028175 File Offset: 0x00026375
		public TypeReference UIntPtr
		{
			get
			{
				return this.type_uintptr ?? this.LookupSystemValueType(ref this.type_uintptr, "UIntPtr", ElementType.U);
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000C31 RID: 3121 RVA: 0x00028194 File Offset: 0x00026394
		public TypeReference String
		{
			get
			{
				return this.type_string ?? this.LookupSystemType(ref this.type_string, "String", ElementType.String);
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000C32 RID: 3122 RVA: 0x000281B3 File Offset: 0x000263B3
		public TypeReference TypedReference
		{
			get
			{
				return this.type_typedref ?? this.LookupSystemValueType(ref this.type_typedref, "TypedReference", ElementType.TypedByRef);
			}
		}

		// Token: 0x04000527 RID: 1319
		private readonly ModuleDefinition module;

		// Token: 0x04000528 RID: 1320
		private TypeReference type_object;

		// Token: 0x04000529 RID: 1321
		private TypeReference type_void;

		// Token: 0x0400052A RID: 1322
		private TypeReference type_bool;

		// Token: 0x0400052B RID: 1323
		private TypeReference type_char;

		// Token: 0x0400052C RID: 1324
		private TypeReference type_sbyte;

		// Token: 0x0400052D RID: 1325
		private TypeReference type_byte;

		// Token: 0x0400052E RID: 1326
		private TypeReference type_int16;

		// Token: 0x0400052F RID: 1327
		private TypeReference type_uint16;

		// Token: 0x04000530 RID: 1328
		private TypeReference type_int32;

		// Token: 0x04000531 RID: 1329
		private TypeReference type_uint32;

		// Token: 0x04000532 RID: 1330
		private TypeReference type_int64;

		// Token: 0x04000533 RID: 1331
		private TypeReference type_uint64;

		// Token: 0x04000534 RID: 1332
		private TypeReference type_single;

		// Token: 0x04000535 RID: 1333
		private TypeReference type_double;

		// Token: 0x04000536 RID: 1334
		private TypeReference type_intptr;

		// Token: 0x04000537 RID: 1335
		private TypeReference type_uintptr;

		// Token: 0x04000538 RID: 1336
		private TypeReference type_string;

		// Token: 0x04000539 RID: 1337
		private TypeReference type_typedref;

		// Token: 0x02000180 RID: 384
		private sealed class CoreTypeSystem : TypeSystem
		{
			// Token: 0x06000C33 RID: 3123 RVA: 0x000281D2 File Offset: 0x000263D2
			public CoreTypeSystem(ModuleDefinition module)
				: base(module)
			{
			}

			// Token: 0x06000C34 RID: 3124 RVA: 0x000281DC File Offset: 0x000263DC
			internal override TypeReference LookupType(string @namespace, string name)
			{
				TypeReference typeReference = this.LookupTypeDefinition(@namespace, name) ?? this.LookupTypeForwarded(@namespace, name);
				if (typeReference != null)
				{
					return typeReference;
				}
				throw new NotSupportedException();
			}

			// Token: 0x06000C35 RID: 3125 RVA: 0x00028208 File Offset: 0x00026408
			private TypeReference LookupTypeDefinition(string @namespace, string name)
			{
				if (this.module.MetadataSystem.Types == null)
				{
					TypeSystem.CoreTypeSystem.Initialize(this.module.Types);
				}
				return this.module.Read<Row<string, string>, TypeDefinition>(new Row<string, string>(@namespace, name), delegate(Row<string, string> row, MetadataReader reader)
				{
					TypeDefinition[] types = reader.metadata.Types;
					for (int i = 0; i < types.Length; i++)
					{
						if (types[i] == null)
						{
							types[i] = reader.GetTypeDefinition((uint)(i + 1));
						}
						TypeDefinition typeDefinition = types[i];
						if (typeDefinition.Name == row.Col2 && typeDefinition.Namespace == row.Col1)
						{
							return typeDefinition;
						}
					}
					return null;
				});
			}

			// Token: 0x06000C36 RID: 3126 RVA: 0x00028268 File Offset: 0x00026468
			private TypeReference LookupTypeForwarded(string @namespace, string name)
			{
				if (!this.module.HasExportedTypes)
				{
					return null;
				}
				Collection<ExportedType> exportedTypes = this.module.ExportedTypes;
				for (int i = 0; i < exportedTypes.Count; i++)
				{
					ExportedType exportedType = exportedTypes[i];
					if (exportedType.Name == name && exportedType.Namespace == @namespace)
					{
						return exportedType.CreateReference();
					}
				}
				return null;
			}

			// Token: 0x06000C37 RID: 3127 RVA: 0x00010C51 File Offset: 0x0000EE51
			private static void Initialize(object obj)
			{
			}
		}

		// Token: 0x02000182 RID: 386
		private sealed class CommonTypeSystem : TypeSystem
		{
			// Token: 0x06000C3B RID: 3131 RVA: 0x000281D2 File Offset: 0x000263D2
			public CommonTypeSystem(ModuleDefinition module)
				: base(module)
			{
			}

			// Token: 0x06000C3C RID: 3132 RVA: 0x00028341 File Offset: 0x00026541
			internal override TypeReference LookupType(string @namespace, string name)
			{
				return this.CreateTypeReference(@namespace, name);
			}

			// Token: 0x06000C3D RID: 3133 RVA: 0x0002834C File Offset: 0x0002654C
			public AssemblyNameReference GetCoreLibraryReference()
			{
				if (this.core_library != null)
				{
					return this.core_library;
				}
				if (this.module.TryGetCoreLibraryReference(out this.core_library))
				{
					return this.core_library;
				}
				this.core_library = new AssemblyNameReference
				{
					Name = "mscorlib",
					Version = this.GetCorlibVersion(),
					PublicKeyToken = new byte[] { 183, 122, 92, 86, 25, 52, 224, 137 }
				};
				this.module.AssemblyReferences.Add(this.core_library);
				return this.core_library;
			}

			// Token: 0x06000C3E RID: 3134 RVA: 0x000283DC File Offset: 0x000265DC
			private Version GetCorlibVersion()
			{
				switch (this.module.Runtime)
				{
				case TargetRuntime.Net_1_0:
				case TargetRuntime.Net_1_1:
					return new Version(1, 0, 0, 0);
				case TargetRuntime.Net_2_0:
					return new Version(2, 0, 0, 0);
				case TargetRuntime.Net_4_0:
					return new Version(4, 0, 0, 0);
				default:
					throw new NotSupportedException();
				}
			}

			// Token: 0x06000C3F RID: 3135 RVA: 0x00028430 File Offset: 0x00026630
			private TypeReference CreateTypeReference(string @namespace, string name)
			{
				return new TypeReference(@namespace, name, this.module, this.GetCoreLibraryReference());
			}

			// Token: 0x0400053C RID: 1340
			private AssemblyNameReference core_library;
		}
	}
}
