using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;
using Mono.Cecil.PE;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x0200014E RID: 334
	internal sealed class ModuleDefinition : ModuleReference, ICustomAttributeProvider, IMetadataTokenProvider, ICustomDebugInformationProvider, IDisposable
	{
		// Token: 0x170001CE RID: 462
		// (get) Token: 0x060009FA RID: 2554 RVA: 0x00023F8A File Offset: 0x0002218A
		public bool IsMain
		{
			get
			{
				return this.kind != ModuleKind.NetModule;
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x060009FB RID: 2555 RVA: 0x00023F98 File Offset: 0x00022198
		// (set) Token: 0x060009FC RID: 2556 RVA: 0x00023FA0 File Offset: 0x000221A0
		public ModuleKind Kind
		{
			get
			{
				return this.kind;
			}
			set
			{
				this.kind = value;
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x060009FD RID: 2557 RVA: 0x00023FA9 File Offset: 0x000221A9
		// (set) Token: 0x060009FE RID: 2558 RVA: 0x00023FB1 File Offset: 0x000221B1
		public MetadataKind MetadataKind
		{
			get
			{
				return this.metadata_kind;
			}
			set
			{
				this.metadata_kind = value;
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x060009FF RID: 2559 RVA: 0x00023FBA File Offset: 0x000221BA
		internal WindowsRuntimeProjections Projections
		{
			get
			{
				if (this.projections == null)
				{
					Interlocked.CompareExchange<WindowsRuntimeProjections>(ref this.projections, new WindowsRuntimeProjections(this), null);
				}
				return this.projections;
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000A00 RID: 2560 RVA: 0x00023FDD File Offset: 0x000221DD
		// (set) Token: 0x06000A01 RID: 2561 RVA: 0x00023FE5 File Offset: 0x000221E5
		public TargetRuntime Runtime
		{
			get
			{
				return this.runtime;
			}
			set
			{
				this.runtime = value;
				this.runtime_version = this.runtime.RuntimeVersionString();
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000A02 RID: 2562 RVA: 0x00023FFF File Offset: 0x000221FF
		// (set) Token: 0x06000A03 RID: 2563 RVA: 0x00024007 File Offset: 0x00022207
		public string RuntimeVersion
		{
			get
			{
				return this.runtime_version;
			}
			set
			{
				this.runtime_version = value;
				this.runtime = this.runtime_version.ParseRuntime();
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000A04 RID: 2564 RVA: 0x00024021 File Offset: 0x00022221
		// (set) Token: 0x06000A05 RID: 2565 RVA: 0x00024029 File Offset: 0x00022229
		public TargetArchitecture Architecture
		{
			get
			{
				return this.architecture;
			}
			set
			{
				this.architecture = value;
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000A06 RID: 2566 RVA: 0x00024032 File Offset: 0x00022232
		// (set) Token: 0x06000A07 RID: 2567 RVA: 0x0002403A File Offset: 0x0002223A
		public ModuleAttributes Attributes
		{
			get
			{
				return this.attributes;
			}
			set
			{
				this.attributes = value;
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000A08 RID: 2568 RVA: 0x00024043 File Offset: 0x00022243
		// (set) Token: 0x06000A09 RID: 2569 RVA: 0x0002404B File Offset: 0x0002224B
		public ModuleCharacteristics Characteristics
		{
			get
			{
				return this.characteristics;
			}
			set
			{
				this.characteristics = value;
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000A0A RID: 2570 RVA: 0x00024054 File Offset: 0x00022254
		[Obsolete("Use FileName")]
		public string FullyQualifiedName
		{
			get
			{
				return this.file_name;
			}
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000A0B RID: 2571 RVA: 0x00024054 File Offset: 0x00022254
		public string FileName
		{
			get
			{
				return this.file_name;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000A0C RID: 2572 RVA: 0x0002405C File Offset: 0x0002225C
		// (set) Token: 0x06000A0D RID: 2573 RVA: 0x00024064 File Offset: 0x00022264
		public Guid Mvid
		{
			get
			{
				return this.mvid;
			}
			set
			{
				this.mvid = value;
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000A0E RID: 2574 RVA: 0x0002406D File Offset: 0x0002226D
		internal bool HasImage
		{
			get
			{
				return this.Image != null;
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x06000A0F RID: 2575 RVA: 0x00024078 File Offset: 0x00022278
		public bool HasSymbols
		{
			get
			{
				return this.symbol_reader != null;
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000A10 RID: 2576 RVA: 0x00024083 File Offset: 0x00022283
		public ISymbolReader SymbolReader
		{
			get
			{
				return this.symbol_reader;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000A11 RID: 2577 RVA: 0x0001347B File Offset: 0x0001167B
		public override MetadataScopeType MetadataScopeType
		{
			get
			{
				return MetadataScopeType.ModuleDefinition;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000A12 RID: 2578 RVA: 0x0002408B File Offset: 0x0002228B
		public AssemblyDefinition Assembly
		{
			get
			{
				return this.assembly;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000A13 RID: 2579 RVA: 0x00024093 File Offset: 0x00022293
		internal IReflectionImporter ReflectionImporter
		{
			get
			{
				if (this.reflection_importer == null)
				{
					Interlocked.CompareExchange<IReflectionImporter>(ref this.reflection_importer, new DefaultReflectionImporter(this), null);
				}
				return this.reflection_importer;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000A14 RID: 2580 RVA: 0x000240B6 File Offset: 0x000222B6
		internal IMetadataImporter MetadataImporter
		{
			get
			{
				if (this.metadata_importer == null)
				{
					Interlocked.CompareExchange<IMetadataImporter>(ref this.metadata_importer, new DefaultMetadataImporter(this), null);
				}
				return this.metadata_importer;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000A15 RID: 2581 RVA: 0x000240DC File Offset: 0x000222DC
		public IAssemblyResolver AssemblyResolver
		{
			get
			{
				if (this.assembly_resolver.value == null)
				{
					object obj = this.module_lock;
					lock (obj)
					{
						this.assembly_resolver = Disposable.Owned<IAssemblyResolver>(new DefaultAssemblyResolver());
					}
				}
				return this.assembly_resolver.value;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000A16 RID: 2582 RVA: 0x00024140 File Offset: 0x00022340
		public IMetadataResolver MetadataResolver
		{
			get
			{
				if (this.metadata_resolver == null)
				{
					Interlocked.CompareExchange<IMetadataResolver>(ref this.metadata_resolver, new MetadataResolver(this.AssemblyResolver), null);
				}
				return this.metadata_resolver;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000A17 RID: 2583 RVA: 0x00024168 File Offset: 0x00022368
		public TypeSystem TypeSystem
		{
			get
			{
				if (this.type_system == null)
				{
					Interlocked.CompareExchange<TypeSystem>(ref this.type_system, TypeSystem.CreateTypeSystem(this), null);
				}
				return this.type_system;
			}
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000A18 RID: 2584 RVA: 0x0002418B File Offset: 0x0002238B
		public bool HasAssemblyReferences
		{
			get
			{
				if (this.references != null)
				{
					return this.references.Count > 0;
				}
				return this.HasImage && this.Image.HasTable(Table.AssemblyRef);
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000A19 RID: 2585 RVA: 0x000241BC File Offset: 0x000223BC
		public Collection<AssemblyNameReference> AssemblyReferences
		{
			get
			{
				if (this.references != null)
				{
					return this.references;
				}
				if (this.HasImage)
				{
					return this.Read<ModuleDefinition, Collection<AssemblyNameReference>>(ref this.references, this, (ModuleDefinition _, MetadataReader reader) => reader.ReadAssemblyReferences());
				}
				Interlocked.CompareExchange<Collection<AssemblyNameReference>>(ref this.references, new Collection<AssemblyNameReference>(), null);
				return this.references;
			}
		}

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000A1A RID: 2586 RVA: 0x00024225 File Offset: 0x00022425
		public bool HasModuleReferences
		{
			get
			{
				if (this.modules != null)
				{
					return this.modules.Count > 0;
				}
				return this.HasImage && this.Image.HasTable(Table.ModuleRef);
			}
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000A1B RID: 2587 RVA: 0x00024258 File Offset: 0x00022458
		public Collection<ModuleReference> ModuleReferences
		{
			get
			{
				if (this.modules != null)
				{
					return this.modules;
				}
				if (this.HasImage)
				{
					return this.Read<ModuleDefinition, Collection<ModuleReference>>(ref this.modules, this, (ModuleDefinition _, MetadataReader reader) => reader.ReadModuleReferences());
				}
				Interlocked.CompareExchange<Collection<ModuleReference>>(ref this.modules, new Collection<ModuleReference>(), null);
				return this.modules;
			}
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000A1C RID: 2588 RVA: 0x000242C4 File Offset: 0x000224C4
		public bool HasResources
		{
			get
			{
				if (this.resources != null)
				{
					return this.resources.Count > 0;
				}
				if (!this.HasImage)
				{
					return false;
				}
				if (!this.Image.HasTable(Table.ManifestResource))
				{
					return this.Read<ModuleDefinition, bool>(this, (ModuleDefinition _, MetadataReader reader) => reader.HasFileResource());
				}
				return true;
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000A1D RID: 2589 RVA: 0x0002432C File Offset: 0x0002252C
		public Collection<Resource> Resources
		{
			get
			{
				if (this.resources != null)
				{
					return this.resources;
				}
				if (this.HasImage)
				{
					return this.Read<ModuleDefinition, Collection<Resource>>(ref this.resources, this, (ModuleDefinition _, MetadataReader reader) => reader.ReadResources());
				}
				Interlocked.CompareExchange<Collection<Resource>>(ref this.resources, new Collection<Resource>(), null);
				return this.resources;
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x06000A1E RID: 2590 RVA: 0x00024395 File Offset: 0x00022595
		public bool HasCustomAttributes
		{
			get
			{
				if (this.custom_attributes != null)
				{
					return this.custom_attributes.Count > 0;
				}
				return this.GetHasCustomAttributes(this);
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000A1F RID: 2591 RVA: 0x000243B5 File Offset: 0x000225B5
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this);
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000A20 RID: 2592 RVA: 0x000243CE File Offset: 0x000225CE
		public bool HasTypes
		{
			get
			{
				if (this.types != null)
				{
					return this.types.Count > 0;
				}
				return this.HasImage && this.Image.HasTable(Table.TypeDef);
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000A21 RID: 2593 RVA: 0x00024400 File Offset: 0x00022600
		public Collection<TypeDefinition> Types
		{
			get
			{
				if (this.types != null)
				{
					return this.types;
				}
				if (this.HasImage)
				{
					return this.Read<ModuleDefinition, TypeDefinitionCollection>(ref this.types, this, (ModuleDefinition _, MetadataReader reader) => reader.ReadTypes());
				}
				Interlocked.CompareExchange<TypeDefinitionCollection>(ref this.types, new TypeDefinitionCollection(this), null);
				return this.types;
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000A22 RID: 2594 RVA: 0x0002446A File Offset: 0x0002266A
		public bool HasExportedTypes
		{
			get
			{
				if (this.exported_types != null)
				{
					return this.exported_types.Count > 0;
				}
				return this.HasImage && this.Image.HasTable(Table.ExportedType);
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000A23 RID: 2595 RVA: 0x0002449C File Offset: 0x0002269C
		public Collection<ExportedType> ExportedTypes
		{
			get
			{
				if (this.exported_types != null)
				{
					return this.exported_types;
				}
				if (this.HasImage)
				{
					return this.Read<ModuleDefinition, Collection<ExportedType>>(ref this.exported_types, this, (ModuleDefinition _, MetadataReader reader) => reader.ReadExportedTypes());
				}
				Interlocked.CompareExchange<Collection<ExportedType>>(ref this.exported_types, new Collection<ExportedType>(), null);
				return this.exported_types;
			}
		}

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000A24 RID: 2596 RVA: 0x00024508 File Offset: 0x00022708
		// (set) Token: 0x06000A25 RID: 2597 RVA: 0x00024563 File Offset: 0x00022763
		public MethodDefinition EntryPoint
		{
			get
			{
				if (this.entry_point != null)
				{
					return this.entry_point;
				}
				if (this.HasImage)
				{
					return this.Read<ModuleDefinition, MethodDefinition>(ref this.entry_point, this, (ModuleDefinition _, MetadataReader reader) => reader.ReadEntryPoint());
				}
				return this.entry_point = null;
			}
			set
			{
				this.entry_point = value;
			}
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000A26 RID: 2598 RVA: 0x0002456C File Offset: 0x0002276C
		public bool HasCustomDebugInformations
		{
			get
			{
				return this.custom_infos != null && this.custom_infos.Count > 0;
			}
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000A27 RID: 2599 RVA: 0x00024586 File Offset: 0x00022786
		public Collection<CustomDebugInformation> CustomDebugInformations
		{
			get
			{
				if (this.custom_infos == null)
				{
					Interlocked.CompareExchange<Collection<CustomDebugInformation>>(ref this.custom_infos, new Collection<CustomDebugInformation>(), null);
				}
				return this.custom_infos;
			}
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x000245A8 File Offset: 0x000227A8
		internal ModuleDefinition()
		{
			this.MetadataSystem = new MetadataSystem();
			this.token = new MetadataToken(TokenType.Module, 1);
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x000245E4 File Offset: 0x000227E4
		internal ModuleDefinition(Image image)
			: this()
		{
			this.Image = image;
			this.kind = image.Kind;
			this.RuntimeVersion = image.RuntimeVersion;
			this.architecture = image.Architecture;
			this.attributes = image.Attributes;
			this.characteristics = image.Characteristics;
			this.linker_version = image.LinkerVersion;
			this.subsystem_major = image.SubSystemMajor;
			this.subsystem_minor = image.SubSystemMinor;
			this.file_name = image.FileName;
			this.timestamp = image.Timestamp;
			this.reader = new MetadataReader(this);
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x00024682 File Offset: 0x00022882
		public void Dispose()
		{
			if (this.Image != null)
			{
				this.Image.Dispose();
			}
			if (this.symbol_reader != null)
			{
				this.symbol_reader.Dispose();
			}
			if (this.assembly_resolver.value != null)
			{
				this.assembly_resolver.Dispose();
			}
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x000246C2 File Offset: 0x000228C2
		public bool HasTypeReference(string fullName)
		{
			return this.HasTypeReference(string.Empty, fullName);
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x000246D0 File Offset: 0x000228D0
		public bool HasTypeReference(string scope, string fullName)
		{
			Mixin.CheckFullName(fullName);
			return this.HasImage && this.GetTypeReference(scope, fullName) != null;
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x000246ED File Offset: 0x000228ED
		public bool TryGetTypeReference(string fullName, out TypeReference type)
		{
			return this.TryGetTypeReference(string.Empty, fullName, out type);
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x000246FC File Offset: 0x000228FC
		public bool TryGetTypeReference(string scope, string fullName, out TypeReference type)
		{
			Mixin.CheckFullName(fullName);
			if (!this.HasImage)
			{
				type = null;
				return false;
			}
			TypeReference typeReference;
			type = (typeReference = this.GetTypeReference(scope, fullName));
			return typeReference != null;
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x0002472C File Offset: 0x0002292C
		private TypeReference GetTypeReference(string scope, string fullname)
		{
			return this.Read<Row<string, string>, TypeReference>(new Row<string, string>(scope, fullname), (Row<string, string> row, MetadataReader reader) => reader.GetTypeReference(row.Col1, row.Col2));
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x0002475A File Offset: 0x0002295A
		public IEnumerable<TypeReference> GetTypeReferences()
		{
			if (!this.HasImage)
			{
				return Empty<TypeReference>.Array;
			}
			return this.Read<ModuleDefinition, IEnumerable<TypeReference>>(this, (ModuleDefinition _, MetadataReader reader) => reader.GetTypeReferences());
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x00024790 File Offset: 0x00022990
		public IEnumerable<MemberReference> GetMemberReferences()
		{
			if (!this.HasImage)
			{
				return Empty<MemberReference>.Array;
			}
			return this.Read<ModuleDefinition, IEnumerable<MemberReference>>(this, (ModuleDefinition _, MetadataReader reader) => reader.GetMemberReferences());
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x000247C6 File Offset: 0x000229C6
		public IEnumerable<CustomAttribute> GetCustomAttributes()
		{
			if (!this.HasImage)
			{
				return Empty<CustomAttribute>.Array;
			}
			return this.Read<ModuleDefinition, IEnumerable<CustomAttribute>>(this, (ModuleDefinition _, MetadataReader reader) => reader.GetCustomAttributes());
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x000247FC File Offset: 0x000229FC
		public TypeReference GetType(string fullName, bool runtimeName)
		{
			if (!runtimeName)
			{
				return this.GetType(fullName);
			}
			return TypeParser.ParseType(this, fullName, true);
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x00024811 File Offset: 0x00022A11
		public TypeDefinition GetType(string fullName)
		{
			Mixin.CheckFullName(fullName);
			if (fullName.IndexOf('/') > 0)
			{
				return this.GetNestedType(fullName);
			}
			return ((TypeDefinitionCollection)this.Types).GetType(fullName);
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x0002483D File Offset: 0x00022A3D
		public TypeDefinition GetType(string @namespace, string name)
		{
			Mixin.CheckName(name);
			return ((TypeDefinitionCollection)this.Types).GetType(@namespace ?? string.Empty, name);
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x00024860 File Offset: 0x00022A60
		public IEnumerable<TypeDefinition> GetTypes()
		{
			return ModuleDefinition.GetTypes(this.Types);
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x0002486D File Offset: 0x00022A6D
		private static IEnumerable<TypeDefinition> GetTypes(Collection<TypeDefinition> types)
		{
			int num;
			for (int i = 0; i < types.Count; i = num + 1)
			{
				TypeDefinition type = types[i];
				yield return type;
				if (type.HasNestedTypes)
				{
					foreach (TypeDefinition typeDefinition in ModuleDefinition.GetTypes(type.NestedTypes))
					{
						yield return typeDefinition;
					}
					IEnumerator<TypeDefinition> enumerator = null;
					type = null;
				}
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x00024880 File Offset: 0x00022A80
		private TypeDefinition GetNestedType(string fullname)
		{
			string[] array = fullname.Split(new char[] { '/' });
			TypeDefinition typeDefinition = this.GetType(array[0]);
			if (typeDefinition == null)
			{
				return null;
			}
			for (int i = 1; i < array.Length; i++)
			{
				TypeDefinition nestedType = typeDefinition.GetNestedType(array[i]);
				if (nestedType == null)
				{
					return null;
				}
				typeDefinition = nestedType;
			}
			return typeDefinition;
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x000248CE File Offset: 0x00022ACE
		internal FieldDefinition Resolve(FieldReference field)
		{
			return this.MetadataResolver.Resolve(field);
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x000248DC File Offset: 0x00022ADC
		internal MethodDefinition Resolve(MethodReference method)
		{
			return this.MetadataResolver.Resolve(method);
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x000248EA File Offset: 0x00022AEA
		internal TypeDefinition Resolve(TypeReference type)
		{
			return this.MetadataResolver.Resolve(type);
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x000248F8 File Offset: 0x00022AF8
		private static void CheckContext(IGenericParameterProvider context, ModuleDefinition module)
		{
			if (context == null)
			{
				return;
			}
			if (context.Module != module)
			{
				throw new ArgumentException();
			}
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x0002490D File Offset: 0x00022B0D
		[Obsolete("Use ImportReference", false)]
		public TypeReference Import(Type type)
		{
			return this.ImportReference(type, null);
		}

		// Token: 0x06000A3E RID: 2622 RVA: 0x0002490D File Offset: 0x00022B0D
		public TypeReference ImportReference(Type type)
		{
			return this.ImportReference(type, null);
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x00024917 File Offset: 0x00022B17
		[Obsolete("Use ImportReference", false)]
		public TypeReference Import(Type type, IGenericParameterProvider context)
		{
			return this.ImportReference(type, context);
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x00024921 File Offset: 0x00022B21
		public TypeReference ImportReference(Type type, IGenericParameterProvider context)
		{
			Mixin.CheckType(type);
			ModuleDefinition.CheckContext(context, this);
			return this.ReflectionImporter.ImportReference(type, context);
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x0002493D File Offset: 0x00022B3D
		[Obsolete("Use ImportReference", false)]
		public FieldReference Import(FieldInfo field)
		{
			return this.ImportReference(field, null);
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x00024947 File Offset: 0x00022B47
		[Obsolete("Use ImportReference", false)]
		public FieldReference Import(FieldInfo field, IGenericParameterProvider context)
		{
			return this.ImportReference(field, context);
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x0002493D File Offset: 0x00022B3D
		public FieldReference ImportReference(FieldInfo field)
		{
			return this.ImportReference(field, null);
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x00024951 File Offset: 0x00022B51
		public FieldReference ImportReference(FieldInfo field, IGenericParameterProvider context)
		{
			Mixin.CheckField(field);
			ModuleDefinition.CheckContext(context, this);
			return this.ReflectionImporter.ImportReference(field, context);
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x0002496D File Offset: 0x00022B6D
		[Obsolete("Use ImportReference", false)]
		public MethodReference Import(MethodBase method)
		{
			return this.ImportReference(method, null);
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x00024977 File Offset: 0x00022B77
		[Obsolete("Use ImportReference", false)]
		public MethodReference Import(MethodBase method, IGenericParameterProvider context)
		{
			return this.ImportReference(method, context);
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x0002496D File Offset: 0x00022B6D
		public MethodReference ImportReference(MethodBase method)
		{
			return this.ImportReference(method, null);
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x00024981 File Offset: 0x00022B81
		public MethodReference ImportReference(MethodBase method, IGenericParameterProvider context)
		{
			Mixin.CheckMethod(method);
			ModuleDefinition.CheckContext(context, this);
			return this.ReflectionImporter.ImportReference(method, context);
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x0002499D File Offset: 0x00022B9D
		[Obsolete("Use ImportReference", false)]
		public TypeReference Import(TypeReference type)
		{
			return this.ImportReference(type, null);
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x000249A7 File Offset: 0x00022BA7
		[Obsolete("Use ImportReference", false)]
		public TypeReference Import(TypeReference type, IGenericParameterProvider context)
		{
			return this.ImportReference(type, context);
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x0002499D File Offset: 0x00022B9D
		public TypeReference ImportReference(TypeReference type)
		{
			return this.ImportReference(type, null);
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x000249B1 File Offset: 0x00022BB1
		public TypeReference ImportReference(TypeReference type, IGenericParameterProvider context)
		{
			Mixin.CheckType(type);
			if (type.Module == this)
			{
				return type;
			}
			ModuleDefinition.CheckContext(context, this);
			return this.MetadataImporter.ImportReference(type, context);
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x000249D8 File Offset: 0x00022BD8
		[Obsolete("Use ImportReference", false)]
		public FieldReference Import(FieldReference field)
		{
			return this.ImportReference(field, null);
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x000249E2 File Offset: 0x00022BE2
		[Obsolete("Use ImportReference", false)]
		public FieldReference Import(FieldReference field, IGenericParameterProvider context)
		{
			return this.ImportReference(field, context);
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x000249D8 File Offset: 0x00022BD8
		public FieldReference ImportReference(FieldReference field)
		{
			return this.ImportReference(field, null);
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x000249EC File Offset: 0x00022BEC
		public FieldReference ImportReference(FieldReference field, IGenericParameterProvider context)
		{
			Mixin.CheckField(field);
			if (field.Module == this)
			{
				return field;
			}
			ModuleDefinition.CheckContext(context, this);
			return this.MetadataImporter.ImportReference(field, context);
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x00024A13 File Offset: 0x00022C13
		[Obsolete("Use ImportReference", false)]
		public MethodReference Import(MethodReference method)
		{
			return this.ImportReference(method, null);
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x00024A1D File Offset: 0x00022C1D
		[Obsolete("Use ImportReference", false)]
		public MethodReference Import(MethodReference method, IGenericParameterProvider context)
		{
			return this.ImportReference(method, context);
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x00024A13 File Offset: 0x00022C13
		public MethodReference ImportReference(MethodReference method)
		{
			return this.ImportReference(method, null);
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x00024A27 File Offset: 0x00022C27
		public MethodReference ImportReference(MethodReference method, IGenericParameterProvider context)
		{
			Mixin.CheckMethod(method);
			if (method.Module == this)
			{
				return method;
			}
			ModuleDefinition.CheckContext(context, this);
			return this.MetadataImporter.ImportReference(method, context);
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x00024A4E File Offset: 0x00022C4E
		public IMetadataTokenProvider LookupToken(int token)
		{
			return this.LookupToken(new MetadataToken((uint)token));
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x00024A5C File Offset: 0x00022C5C
		public IMetadataTokenProvider LookupToken(MetadataToken token)
		{
			return this.Read<MetadataToken, IMetadataTokenProvider>(token, (MetadataToken t, MetadataReader reader) => reader.LookupToken(t));
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000A57 RID: 2647 RVA: 0x00024A84 File Offset: 0x00022C84
		internal object SyncRoot
		{
			get
			{
				return this.module_lock;
			}
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x00024A8C File Offset: 0x00022C8C
		internal void Read<TItem>(TItem item, Action<TItem, MetadataReader> read)
		{
			object obj = this.module_lock;
			lock (obj)
			{
				int position = this.reader.position;
				IGenericContext context = this.reader.context;
				read(item, this.reader);
				this.reader.position = position;
				this.reader.context = context;
			}
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x00024B04 File Offset: 0x00022D04
		internal TRet Read<TItem, TRet>(TItem item, Func<TItem, MetadataReader, TRet> read)
		{
			object obj = this.module_lock;
			TRet tret2;
			lock (obj)
			{
				int position = this.reader.position;
				IGenericContext context = this.reader.context;
				TRet tret = read(item, this.reader);
				this.reader.position = position;
				this.reader.context = context;
				tret2 = tret;
			}
			return tret2;
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x00024B80 File Offset: 0x00022D80
		internal TRet Read<TItem, TRet>(ref TRet variable, TItem item, Func<TItem, MetadataReader, TRet> read) where TRet : class
		{
			object obj = this.module_lock;
			TRet tret;
			lock (obj)
			{
				if (variable != null)
				{
					tret = variable;
				}
				else
				{
					int position = this.reader.position;
					IGenericContext context = this.reader.context;
					TRet tret2 = read(item, this.reader);
					this.reader.position = position;
					this.reader.context = context;
					tret = (variable = tret2);
				}
			}
			return tret;
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000A5B RID: 2651 RVA: 0x00024C24 File Offset: 0x00022E24
		public bool HasDebugHeader
		{
			get
			{
				return this.Image != null && this.Image.DebugHeader != null;
			}
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x00024C3E File Offset: 0x00022E3E
		public ImageDebugHeader GetDebugHeader()
		{
			return this.Image.DebugHeader ?? new ImageDebugHeader();
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x00024C54 File Offset: 0x00022E54
		public static ModuleDefinition CreateModule(string name, ModuleKind kind)
		{
			return ModuleDefinition.CreateModule(name, new ModuleParameters
			{
				Kind = kind
			});
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x00024C68 File Offset: 0x00022E68
		public static ModuleDefinition CreateModule(string name, ModuleParameters parameters)
		{
			Mixin.CheckName(name);
			Mixin.CheckParameters(parameters);
			ModuleDefinition moduleDefinition = new ModuleDefinition
			{
				Name = name,
				kind = parameters.Kind,
				timestamp = (parameters.Timestamp ?? Mixin.GetTimestamp()),
				Runtime = parameters.Runtime,
				architecture = parameters.Architecture,
				mvid = Guid.NewGuid(),
				Attributes = ModuleAttributes.ILOnly,
				Characteristics = (ModuleCharacteristics.DynamicBase | ModuleCharacteristics.NoSEH | ModuleCharacteristics.NXCompat | ModuleCharacteristics.TerminalServerAware)
			};
			if (parameters.AssemblyResolver != null)
			{
				moduleDefinition.assembly_resolver = Disposable.NotOwned<IAssemblyResolver>(parameters.AssemblyResolver);
			}
			if (parameters.MetadataResolver != null)
			{
				moduleDefinition.metadata_resolver = parameters.MetadataResolver;
			}
			if (parameters.MetadataImporterProvider != null)
			{
				moduleDefinition.metadata_importer = parameters.MetadataImporterProvider.GetMetadataImporter(moduleDefinition);
			}
			if (parameters.ReflectionImporterProvider != null)
			{
				moduleDefinition.reflection_importer = parameters.ReflectionImporterProvider.GetReflectionImporter(moduleDefinition);
			}
			if (parameters.Kind != ModuleKind.NetModule)
			{
				AssemblyDefinition assemblyDefinition = new AssemblyDefinition();
				moduleDefinition.assembly = assemblyDefinition;
				moduleDefinition.assembly.Name = ModuleDefinition.CreateAssemblyName(name);
				assemblyDefinition.main_module = moduleDefinition;
			}
			moduleDefinition.Types.Add(new TypeDefinition(string.Empty, "<Module>", TypeAttributes.NotPublic));
			return moduleDefinition;
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x00024D9E File Offset: 0x00022F9E
		private static AssemblyNameDefinition CreateAssemblyName(string name)
		{
			if (name.EndsWith(".dll") || name.EndsWith(".exe"))
			{
				name = name.Substring(0, name.Length - 4);
			}
			return new AssemblyNameDefinition(name, Mixin.ZeroVersion);
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x00024DD8 File Offset: 0x00022FD8
		public void ReadSymbols()
		{
			if (string.IsNullOrEmpty(this.file_name))
			{
				throw new InvalidOperationException();
			}
			DefaultSymbolReaderProvider defaultSymbolReaderProvider = new DefaultSymbolReaderProvider(true);
			this.ReadSymbols(defaultSymbolReaderProvider.GetSymbolReader(this, this.file_name), true);
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x00024E13 File Offset: 0x00023013
		public void ReadSymbols(ISymbolReader reader)
		{
			this.ReadSymbols(reader, true);
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x00024E20 File Offset: 0x00023020
		public void ReadSymbols(ISymbolReader reader, bool throwIfSymbolsAreNotMaching)
		{
			if (reader == null)
			{
				throw new ArgumentNullException("reader");
			}
			this.symbol_reader = reader;
			if (this.symbol_reader.ProcessDebugHeader(this.GetDebugHeader()))
			{
				if (this.HasImage && this.ReadingMode == ReadingMode.Immediate)
				{
					new ImmediateModuleReader(this.Image).ReadSymbols(this);
				}
				return;
			}
			this.symbol_reader = null;
			if (throwIfSymbolsAreNotMaching)
			{
				throw new SymbolsNotMatchingException("Symbols were found but are not matching the assembly");
			}
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x00024E8D File Offset: 0x0002308D
		public static ModuleDefinition ReadModule(string fileName)
		{
			return ModuleDefinition.ReadModule(fileName, new ReaderParameters(ReadingMode.Deferred));
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x00024E9C File Offset: 0x0002309C
		public static ModuleDefinition ReadModule(string fileName, ReaderParameters parameters)
		{
			Stream stream = ModuleDefinition.GetFileStream(fileName, FileMode.Open, parameters.ReadWrite ? FileAccess.ReadWrite : FileAccess.Read, FileShare.Read);
			if (parameters.InMemory)
			{
				MemoryStream memoryStream = new MemoryStream(stream.CanSeek ? ((int)stream.Length) : 0);
				using (stream)
				{
					stream.CopyTo(memoryStream);
				}
				memoryStream.Position = 0L;
				stream = memoryStream;
			}
			ModuleDefinition moduleDefinition;
			try
			{
				moduleDefinition = ModuleDefinition.ReadModule(Disposable.Owned<Stream>(stream), fileName, parameters);
			}
			catch (Exception)
			{
				stream.Dispose();
				throw;
			}
			return moduleDefinition;
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x00024F34 File Offset: 0x00023134
		private static Stream GetFileStream(string fileName, FileMode mode, FileAccess access, FileShare share)
		{
			Mixin.CheckFileName(fileName);
			return new FileStream(fileName, mode, access, share);
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x00024F45 File Offset: 0x00023145
		public static ModuleDefinition ReadModule(Stream stream)
		{
			return ModuleDefinition.ReadModule(stream, new ReaderParameters(ReadingMode.Deferred));
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x00024F53 File Offset: 0x00023153
		public static ModuleDefinition ReadModule(Stream stream, ReaderParameters parameters)
		{
			Mixin.CheckStream(stream);
			Mixin.CheckReadSeek(stream);
			return ModuleDefinition.ReadModule(Disposable.NotOwned<Stream>(stream), stream.GetFileName(), parameters);
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x00024F73 File Offset: 0x00023173
		private static ModuleDefinition ReadModule(Disposable<Stream> stream, string fileName, ReaderParameters parameters)
		{
			Mixin.CheckParameters(parameters);
			return ModuleReader.CreateModule(ImageReader.ReadImage(stream, fileName), parameters);
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x00024F88 File Offset: 0x00023188
		public void Write(string fileName)
		{
			this.Write(fileName, new WriterParameters());
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x00024F98 File Offset: 0x00023198
		public void Write(string fileName, WriterParameters parameters)
		{
			Mixin.CheckParameters(parameters);
			Stream fileStream = ModuleDefinition.GetFileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
			ModuleWriter.WriteModule(this, Disposable.Owned<Stream>(fileStream), parameters);
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x00024FC2 File Offset: 0x000231C2
		public void Write()
		{
			this.Write(new WriterParameters());
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x00024FCF File Offset: 0x000231CF
		public void Write(WriterParameters parameters)
		{
			if (!this.HasImage)
			{
				throw new InvalidOperationException();
			}
			this.Write(this.Image.Stream.value, parameters);
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x00024FF6 File Offset: 0x000231F6
		public void Write(Stream stream)
		{
			this.Write(stream, new WriterParameters());
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x00025004 File Offset: 0x00023204
		public void Write(Stream stream, WriterParameters parameters)
		{
			Mixin.CheckStream(stream);
			Mixin.CheckWriteSeek(stream);
			Mixin.CheckParameters(parameters);
			ModuleWriter.WriteModule(this, Disposable.NotOwned<Stream>(stream), parameters);
		}

		// Token: 0x040003AF RID: 943
		internal Image Image;

		// Token: 0x040003B0 RID: 944
		internal MetadataSystem MetadataSystem;

		// Token: 0x040003B1 RID: 945
		internal ReadingMode ReadingMode;

		// Token: 0x040003B2 RID: 946
		internal ISymbolReaderProvider SymbolReaderProvider;

		// Token: 0x040003B3 RID: 947
		internal ISymbolReader symbol_reader;

		// Token: 0x040003B4 RID: 948
		internal Disposable<IAssemblyResolver> assembly_resolver;

		// Token: 0x040003B5 RID: 949
		internal IMetadataResolver metadata_resolver;

		// Token: 0x040003B6 RID: 950
		internal TypeSystem type_system;

		// Token: 0x040003B7 RID: 951
		internal readonly MetadataReader reader;

		// Token: 0x040003B8 RID: 952
		private readonly string file_name;

		// Token: 0x040003B9 RID: 953
		internal string runtime_version;

		// Token: 0x040003BA RID: 954
		internal ModuleKind kind;

		// Token: 0x040003BB RID: 955
		private WindowsRuntimeProjections projections;

		// Token: 0x040003BC RID: 956
		private MetadataKind metadata_kind;

		// Token: 0x040003BD RID: 957
		private TargetRuntime runtime;

		// Token: 0x040003BE RID: 958
		private TargetArchitecture architecture;

		// Token: 0x040003BF RID: 959
		private ModuleAttributes attributes;

		// Token: 0x040003C0 RID: 960
		private ModuleCharacteristics characteristics;

		// Token: 0x040003C1 RID: 961
		private Guid mvid;

		// Token: 0x040003C2 RID: 962
		internal ushort linker_version = 8;

		// Token: 0x040003C3 RID: 963
		internal ushort subsystem_major = 4;

		// Token: 0x040003C4 RID: 964
		internal ushort subsystem_minor;

		// Token: 0x040003C5 RID: 965
		internal uint timestamp;

		// Token: 0x040003C6 RID: 966
		internal AssemblyDefinition assembly;

		// Token: 0x040003C7 RID: 967
		private MethodDefinition entry_point;

		// Token: 0x040003C8 RID: 968
		internal IReflectionImporter reflection_importer;

		// Token: 0x040003C9 RID: 969
		internal IMetadataImporter metadata_importer;

		// Token: 0x040003CA RID: 970
		private Collection<CustomAttribute> custom_attributes;

		// Token: 0x040003CB RID: 971
		private Collection<AssemblyNameReference> references;

		// Token: 0x040003CC RID: 972
		private Collection<ModuleReference> modules;

		// Token: 0x040003CD RID: 973
		private Collection<Resource> resources;

		// Token: 0x040003CE RID: 974
		private Collection<ExportedType> exported_types;

		// Token: 0x040003CF RID: 975
		private TypeDefinitionCollection types;

		// Token: 0x040003D0 RID: 976
		internal Collection<CustomDebugInformation> custom_infos;

		// Token: 0x040003D1 RID: 977
		internal MetadataBuilder metadata_builder;

		// Token: 0x040003D2 RID: 978
		private readonly object module_lock = new object();
	}
}
