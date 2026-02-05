using System;
using Mono.Cecil.Metadata;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x020000C1 RID: 193
	public abstract class TypeSystem
	{
		// Token: 0x0600084E RID: 2126 RVA: 0x0001917F File Offset: 0x0001737F
		private TypeSystem(ModuleDefinition module)
		{
			this.module = module;
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x0001918E File Offset: 0x0001738E
		internal static TypeSystem CreateTypeSystem(ModuleDefinition module)
		{
			if (module.IsCoreLibrary())
			{
				return new TypeSystem.CoreTypeSystem(module);
			}
			return new TypeSystem.CommonTypeSystem(module);
		}

		// Token: 0x06000850 RID: 2128
		internal abstract TypeReference LookupType(string @namespace, string name);

		// Token: 0x06000851 RID: 2129 RVA: 0x000191A8 File Offset: 0x000173A8
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

		// Token: 0x06000852 RID: 2130 RVA: 0x00019210 File Offset: 0x00017410
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

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000853 RID: 2131 RVA: 0x0001927C File Offset: 0x0001747C
		[Obsolete("Use CoreLibrary")]
		public IMetadataScope Corlib
		{
			get
			{
				return this.CoreLibrary;
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000854 RID: 2132 RVA: 0x00019284 File Offset: 0x00017484
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

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000855 RID: 2133 RVA: 0x000192A8 File Offset: 0x000174A8
		public TypeReference Object
		{
			get
			{
				return this.type_object ?? this.LookupSystemType(ref this.type_object, "Object", ElementType.Object);
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000856 RID: 2134 RVA: 0x000192C7 File Offset: 0x000174C7
		public TypeReference Void
		{
			get
			{
				return this.type_void ?? this.LookupSystemType(ref this.type_void, "Void", ElementType.Void);
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000857 RID: 2135 RVA: 0x000192E5 File Offset: 0x000174E5
		public TypeReference Boolean
		{
			get
			{
				return this.type_bool ?? this.LookupSystemValueType(ref this.type_bool, "Boolean", ElementType.Boolean);
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000858 RID: 2136 RVA: 0x00019303 File Offset: 0x00017503
		public TypeReference Char
		{
			get
			{
				return this.type_char ?? this.LookupSystemValueType(ref this.type_char, "Char", ElementType.Char);
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000859 RID: 2137 RVA: 0x00019321 File Offset: 0x00017521
		public TypeReference SByte
		{
			get
			{
				return this.type_sbyte ?? this.LookupSystemValueType(ref this.type_sbyte, "SByte", ElementType.I1);
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x0600085A RID: 2138 RVA: 0x0001933F File Offset: 0x0001753F
		public TypeReference Byte
		{
			get
			{
				return this.type_byte ?? this.LookupSystemValueType(ref this.type_byte, "Byte", ElementType.U1);
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x0600085B RID: 2139 RVA: 0x0001935D File Offset: 0x0001755D
		public TypeReference Int16
		{
			get
			{
				return this.type_int16 ?? this.LookupSystemValueType(ref this.type_int16, "Int16", ElementType.I2);
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x0600085C RID: 2140 RVA: 0x0001937B File Offset: 0x0001757B
		public TypeReference UInt16
		{
			get
			{
				return this.type_uint16 ?? this.LookupSystemValueType(ref this.type_uint16, "UInt16", ElementType.U2);
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x0600085D RID: 2141 RVA: 0x00019399 File Offset: 0x00017599
		public TypeReference Int32
		{
			get
			{
				return this.type_int32 ?? this.LookupSystemValueType(ref this.type_int32, "Int32", ElementType.I4);
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x0600085E RID: 2142 RVA: 0x000193B7 File Offset: 0x000175B7
		public TypeReference UInt32
		{
			get
			{
				return this.type_uint32 ?? this.LookupSystemValueType(ref this.type_uint32, "UInt32", ElementType.U4);
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x0600085F RID: 2143 RVA: 0x000193D6 File Offset: 0x000175D6
		public TypeReference Int64
		{
			get
			{
				return this.type_int64 ?? this.LookupSystemValueType(ref this.type_int64, "Int64", ElementType.I8);
			}
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000860 RID: 2144 RVA: 0x000193F5 File Offset: 0x000175F5
		public TypeReference UInt64
		{
			get
			{
				return this.type_uint64 ?? this.LookupSystemValueType(ref this.type_uint64, "UInt64", ElementType.U8);
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000861 RID: 2145 RVA: 0x00019414 File Offset: 0x00017614
		public TypeReference Single
		{
			get
			{
				return this.type_single ?? this.LookupSystemValueType(ref this.type_single, "Single", ElementType.R4);
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000862 RID: 2146 RVA: 0x00019433 File Offset: 0x00017633
		public TypeReference Double
		{
			get
			{
				return this.type_double ?? this.LookupSystemValueType(ref this.type_double, "Double", ElementType.R8);
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000863 RID: 2147 RVA: 0x00019452 File Offset: 0x00017652
		public TypeReference IntPtr
		{
			get
			{
				return this.type_intptr ?? this.LookupSystemValueType(ref this.type_intptr, "IntPtr", ElementType.I);
			}
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000864 RID: 2148 RVA: 0x00019471 File Offset: 0x00017671
		public TypeReference UIntPtr
		{
			get
			{
				return this.type_uintptr ?? this.LookupSystemValueType(ref this.type_uintptr, "UIntPtr", ElementType.U);
			}
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000865 RID: 2149 RVA: 0x00019490 File Offset: 0x00017690
		public TypeReference String
		{
			get
			{
				return this.type_string ?? this.LookupSystemType(ref this.type_string, "String", ElementType.String);
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000866 RID: 2150 RVA: 0x000194AF File Offset: 0x000176AF
		public TypeReference TypedReference
		{
			get
			{
				return this.type_typedref ?? this.LookupSystemValueType(ref this.type_typedref, "TypedReference", ElementType.TypedByRef);
			}
		}

		// Token: 0x040002D5 RID: 725
		private readonly ModuleDefinition module;

		// Token: 0x040002D6 RID: 726
		private TypeReference type_object;

		// Token: 0x040002D7 RID: 727
		private TypeReference type_void;

		// Token: 0x040002D8 RID: 728
		private TypeReference type_bool;

		// Token: 0x040002D9 RID: 729
		private TypeReference type_char;

		// Token: 0x040002DA RID: 730
		private TypeReference type_sbyte;

		// Token: 0x040002DB RID: 731
		private TypeReference type_byte;

		// Token: 0x040002DC RID: 732
		private TypeReference type_int16;

		// Token: 0x040002DD RID: 733
		private TypeReference type_uint16;

		// Token: 0x040002DE RID: 734
		private TypeReference type_int32;

		// Token: 0x040002DF RID: 735
		private TypeReference type_uint32;

		// Token: 0x040002E0 RID: 736
		private TypeReference type_int64;

		// Token: 0x040002E1 RID: 737
		private TypeReference type_uint64;

		// Token: 0x040002E2 RID: 738
		private TypeReference type_single;

		// Token: 0x040002E3 RID: 739
		private TypeReference type_double;

		// Token: 0x040002E4 RID: 740
		private TypeReference type_intptr;

		// Token: 0x040002E5 RID: 741
		private TypeReference type_uintptr;

		// Token: 0x040002E6 RID: 742
		private TypeReference type_string;

		// Token: 0x040002E7 RID: 743
		private TypeReference type_typedref;

		// Token: 0x0200014B RID: 331
		private sealed class CoreTypeSystem : TypeSystem
		{
			// Token: 0x06000BCB RID: 3019 RVA: 0x00024F7F File Offset: 0x0002317F
			public CoreTypeSystem(ModuleDefinition module)
				: base(module)
			{
			}

			// Token: 0x06000BCC RID: 3020 RVA: 0x00024F88 File Offset: 0x00023188
			internal override TypeReference LookupType(string @namespace, string name)
			{
				TypeReference typeReference = this.LookupTypeDefinition(@namespace, name) ?? this.LookupTypeForwarded(@namespace, name);
				if (typeReference != null)
				{
					return typeReference;
				}
				throw new NotSupportedException();
			}

			// Token: 0x06000BCD RID: 3021 RVA: 0x00024FB4 File Offset: 0x000231B4
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

			// Token: 0x06000BCE RID: 3022 RVA: 0x00025014 File Offset: 0x00023214
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

			// Token: 0x06000BCF RID: 3023 RVA: 0x00002A0D File Offset: 0x00000C0D
			private static void Initialize(object obj)
			{
			}
		}

		// Token: 0x0200014C RID: 332
		private sealed class CommonTypeSystem : TypeSystem
		{
			// Token: 0x06000BD0 RID: 3024 RVA: 0x00024F7F File Offset: 0x0002317F
			public CommonTypeSystem(ModuleDefinition module)
				: base(module)
			{
			}

			// Token: 0x06000BD1 RID: 3025 RVA: 0x00025079 File Offset: 0x00023279
			internal override TypeReference LookupType(string @namespace, string name)
			{
				return this.CreateTypeReference(@namespace, name);
			}

			// Token: 0x06000BD2 RID: 3026 RVA: 0x00025084 File Offset: 0x00023284
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

			// Token: 0x06000BD3 RID: 3027 RVA: 0x00025114 File Offset: 0x00023314
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

			// Token: 0x06000BD4 RID: 3028 RVA: 0x00025168 File Offset: 0x00023368
			private TypeReference CreateTypeReference(string @namespace, string name)
			{
				return new TypeReference(@namespace, name, this.module, this.GetCoreLibraryReference());
			}

			// Token: 0x04000758 RID: 1880
			private AssemblyNameReference core_library;
		}
	}
}
