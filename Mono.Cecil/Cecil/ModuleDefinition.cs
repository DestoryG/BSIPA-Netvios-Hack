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
	// Token: 0x02000096 RID: 150
	public sealed class ModuleDefinition : ModuleReference, ICustomAttributeProvider, IMetadataTokenProvider, ICustomDebugInformationProvider, IDisposable
	{
		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x0600065A RID: 1626 RVA: 0x00015676 File Offset: 0x00013876
		public bool IsMain
		{
			get
			{
				return this.kind != ModuleKind.NetModule;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x0600065B RID: 1627 RVA: 0x00015684 File Offset: 0x00013884
		// (set) Token: 0x0600065C RID: 1628 RVA: 0x0001568C File Offset: 0x0001388C
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

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x0600065D RID: 1629 RVA: 0x00015695 File Offset: 0x00013895
		// (set) Token: 0x0600065E RID: 1630 RVA: 0x0001569D File Offset: 0x0001389D
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

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x000156A6 File Offset: 0x000138A6
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

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000660 RID: 1632 RVA: 0x000156C9 File Offset: 0x000138C9
		// (set) Token: 0x06000661 RID: 1633 RVA: 0x000156D1 File Offset: 0x000138D1
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

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000662 RID: 1634 RVA: 0x000156EB File Offset: 0x000138EB
		// (set) Token: 0x06000663 RID: 1635 RVA: 0x000156F3 File Offset: 0x000138F3
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

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000664 RID: 1636 RVA: 0x0001570D File Offset: 0x0001390D
		// (set) Token: 0x06000665 RID: 1637 RVA: 0x00015715 File Offset: 0x00013915
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

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000666 RID: 1638 RVA: 0x0001571E File Offset: 0x0001391E
		// (set) Token: 0x06000667 RID: 1639 RVA: 0x00015726 File Offset: 0x00013926
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

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000668 RID: 1640 RVA: 0x0001572F File Offset: 0x0001392F
		// (set) Token: 0x06000669 RID: 1641 RVA: 0x00015737 File Offset: 0x00013937
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

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x0600066A RID: 1642 RVA: 0x00015740 File Offset: 0x00013940
		[Obsolete("Use FileName")]
		public string FullyQualifiedName
		{
			get
			{
				return this.file_name;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x0600066B RID: 1643 RVA: 0x00015740 File Offset: 0x00013940
		public string FileName
		{
			get
			{
				return this.file_name;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x0600066C RID: 1644 RVA: 0x00015748 File Offset: 0x00013948
		// (set) Token: 0x0600066D RID: 1645 RVA: 0x00015750 File Offset: 0x00013950
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

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x0600066E RID: 1646 RVA: 0x00015759 File Offset: 0x00013959
		internal bool HasImage
		{
			get
			{
				return this.Image != null;
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x0600066F RID: 1647 RVA: 0x00015764 File Offset: 0x00013964
		public bool HasSymbols
		{
			get
			{
				return this.symbol_reader != null;
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000670 RID: 1648 RVA: 0x0001576F File Offset: 0x0001396F
		public ISymbolReader SymbolReader
		{
			get
			{
				return this.symbol_reader;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000671 RID: 1649 RVA: 0x000050A7 File Offset: 0x000032A7
		public override MetadataScopeType MetadataScopeType
		{
			get
			{
				return MetadataScopeType.ModuleDefinition;
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000672 RID: 1650 RVA: 0x00015777 File Offset: 0x00013977
		public AssemblyDefinition Assembly
		{
			get
			{
				return this.assembly;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000673 RID: 1651 RVA: 0x0001577F File Offset: 0x0001397F
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

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000674 RID: 1652 RVA: 0x000157A2 File Offset: 0x000139A2
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

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000675 RID: 1653 RVA: 0x000157C8 File Offset: 0x000139C8
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

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000676 RID: 1654 RVA: 0x0001582C File Offset: 0x00013A2C
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

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000677 RID: 1655 RVA: 0x00015854 File Offset: 0x00013A54
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

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000678 RID: 1656 RVA: 0x00015877 File Offset: 0x00013A77
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

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000679 RID: 1657 RVA: 0x000158A8 File Offset: 0x00013AA8
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
				return this.references = new Collection<AssemblyNameReference>();
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x0600067A RID: 1658 RVA: 0x00015907 File Offset: 0x00013B07
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

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x0600067B RID: 1659 RVA: 0x00015938 File Offset: 0x00013B38
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
				return this.modules = new Collection<ModuleReference>();
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x0600067C RID: 1660 RVA: 0x00015998 File Offset: 0x00013B98
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

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x0600067D RID: 1661 RVA: 0x00015A00 File Offset: 0x00013C00
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
				return this.resources = new Collection<Resource>();
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x0600067E RID: 1662 RVA: 0x00015A5F File Offset: 0x00013C5F
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

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x0600067F RID: 1663 RVA: 0x00015A7F File Offset: 0x00013C7F
		public Collection<CustomAttribute> CustomAttributes
		{
			get
			{
				return this.custom_attributes ?? this.GetCustomAttributes(ref this.custom_attributes, this);
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000680 RID: 1664 RVA: 0x00015A98 File Offset: 0x00013C98
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

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000681 RID: 1665 RVA: 0x00015AC8 File Offset: 0x00013CC8
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
				return this.types = new TypeDefinitionCollection(this);
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000682 RID: 1666 RVA: 0x00015B28 File Offset: 0x00013D28
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

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000683 RID: 1667 RVA: 0x00015B58 File Offset: 0x00013D58
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
				return this.exported_types = new Collection<ExportedType>();
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000684 RID: 1668 RVA: 0x00015BB8 File Offset: 0x00013DB8
		// (set) Token: 0x06000685 RID: 1669 RVA: 0x00015C13 File Offset: 0x00013E13
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

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000686 RID: 1670 RVA: 0x00015C1C File Offset: 0x00013E1C
		public bool HasCustomDebugInformations
		{
			get
			{
				return this.custom_infos != null && this.custom_infos.Count > 0;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000687 RID: 1671 RVA: 0x00015C38 File Offset: 0x00013E38
		public Collection<CustomDebugInformation> CustomDebugInformations
		{
			get
			{
				Collection<CustomDebugInformation> collection;
				if ((collection = this.custom_infos) == null)
				{
					collection = (this.custom_infos = new Collection<CustomDebugInformation>());
				}
				return collection;
			}
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x00015C5D File Offset: 0x00013E5D
		internal ModuleDefinition()
		{
			this.MetadataSystem = new MetadataSystem();
			this.token = new MetadataToken(TokenType.Module, 1);
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x00015C98 File Offset: 0x00013E98
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

		// Token: 0x0600068A RID: 1674 RVA: 0x00015D36 File Offset: 0x00013F36
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

		// Token: 0x0600068B RID: 1675 RVA: 0x00015D76 File Offset: 0x00013F76
		public bool HasTypeReference(string fullName)
		{
			return this.HasTypeReference(string.Empty, fullName);
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x00015D84 File Offset: 0x00013F84
		public bool HasTypeReference(string scope, string fullName)
		{
			Mixin.CheckFullName(fullName);
			return this.HasImage && this.GetTypeReference(scope, fullName) != null;
		}

		// Token: 0x0600068D RID: 1677 RVA: 0x00015DA1 File Offset: 0x00013FA1
		public bool TryGetTypeReference(string fullName, out TypeReference type)
		{
			return this.TryGetTypeReference(string.Empty, fullName, out type);
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x00015DB0 File Offset: 0x00013FB0
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

		// Token: 0x0600068F RID: 1679 RVA: 0x00015DE0 File Offset: 0x00013FE0
		private TypeReference GetTypeReference(string scope, string fullname)
		{
			return this.Read<Row<string, string>, TypeReference>(new Row<string, string>(scope, fullname), (Row<string, string> row, MetadataReader reader) => reader.GetTypeReference(row.Col1, row.Col2));
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00015E0E File Offset: 0x0001400E
		public IEnumerable<TypeReference> GetTypeReferences()
		{
			if (!this.HasImage)
			{
				return Empty<TypeReference>.Array;
			}
			return this.Read<ModuleDefinition, IEnumerable<TypeReference>>(this, (ModuleDefinition _, MetadataReader reader) => reader.GetTypeReferences());
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x00015E44 File Offset: 0x00014044
		public IEnumerable<MemberReference> GetMemberReferences()
		{
			if (!this.HasImage)
			{
				return Empty<MemberReference>.Array;
			}
			return this.Read<ModuleDefinition, IEnumerable<MemberReference>>(this, (ModuleDefinition _, MetadataReader reader) => reader.GetMemberReferences());
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x00015E7A File Offset: 0x0001407A
		public IEnumerable<CustomAttribute> GetCustomAttributes()
		{
			if (!this.HasImage)
			{
				return Empty<CustomAttribute>.Array;
			}
			return this.Read<ModuleDefinition, IEnumerable<CustomAttribute>>(this, (ModuleDefinition _, MetadataReader reader) => reader.GetCustomAttributes());
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x00015EB0 File Offset: 0x000140B0
		public TypeReference GetType(string fullName, bool runtimeName)
		{
			if (!runtimeName)
			{
				return this.GetType(fullName);
			}
			return TypeParser.ParseType(this, fullName, true);
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x00015EC5 File Offset: 0x000140C5
		public TypeDefinition GetType(string fullName)
		{
			Mixin.CheckFullName(fullName);
			if (fullName.IndexOf('/') > 0)
			{
				return this.GetNestedType(fullName);
			}
			return ((TypeDefinitionCollection)this.Types).GetType(fullName);
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x00015EF1 File Offset: 0x000140F1
		public TypeDefinition GetType(string @namespace, string name)
		{
			Mixin.CheckName(name);
			return ((TypeDefinitionCollection)this.Types).GetType(@namespace ?? string.Empty, name);
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x00015F14 File Offset: 0x00014114
		public IEnumerable<TypeDefinition> GetTypes()
		{
			return ModuleDefinition.GetTypes(this.Types);
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x00015F21 File Offset: 0x00014121
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

		// Token: 0x06000698 RID: 1688 RVA: 0x00015F34 File Offset: 0x00014134
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

		// Token: 0x06000699 RID: 1689 RVA: 0x00015F82 File Offset: 0x00014182
		internal FieldDefinition Resolve(FieldReference field)
		{
			return this.MetadataResolver.Resolve(field);
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x00015F90 File Offset: 0x00014190
		internal MethodDefinition Resolve(MethodReference method)
		{
			return this.MetadataResolver.Resolve(method);
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x00015F9E File Offset: 0x0001419E
		internal TypeDefinition Resolve(TypeReference type)
		{
			return this.MetadataResolver.Resolve(type);
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x00015FAC File Offset: 0x000141AC
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

		// Token: 0x0600069D RID: 1693 RVA: 0x00015FC1 File Offset: 0x000141C1
		[Obsolete("Use ImportReference", false)]
		public TypeReference Import(Type type)
		{
			return this.ImportReference(type, null);
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x00015FC1 File Offset: 0x000141C1
		public TypeReference ImportReference(Type type)
		{
			return this.ImportReference(type, null);
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x00015FCB File Offset: 0x000141CB
		[Obsolete("Use ImportReference", false)]
		public TypeReference Import(Type type, IGenericParameterProvider context)
		{
			return this.ImportReference(type, context);
		}

		// Token: 0x060006A0 RID: 1696 RVA: 0x00015FD5 File Offset: 0x000141D5
		public TypeReference ImportReference(Type type, IGenericParameterProvider context)
		{
			Mixin.CheckType(type);
			ModuleDefinition.CheckContext(context, this);
			return this.ReflectionImporter.ImportReference(type, context);
		}

		// Token: 0x060006A1 RID: 1697 RVA: 0x00015FF1 File Offset: 0x000141F1
		[Obsolete("Use ImportReference", false)]
		public FieldReference Import(FieldInfo field)
		{
			return this.ImportReference(field, null);
		}

		// Token: 0x060006A2 RID: 1698 RVA: 0x00015FFB File Offset: 0x000141FB
		[Obsolete("Use ImportReference", false)]
		public FieldReference Import(FieldInfo field, IGenericParameterProvider context)
		{
			return this.ImportReference(field, context);
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x00015FF1 File Offset: 0x000141F1
		public FieldReference ImportReference(FieldInfo field)
		{
			return this.ImportReference(field, null);
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x00016005 File Offset: 0x00014205
		public FieldReference ImportReference(FieldInfo field, IGenericParameterProvider context)
		{
			Mixin.CheckField(field);
			ModuleDefinition.CheckContext(context, this);
			return this.ReflectionImporter.ImportReference(field, context);
		}

		// Token: 0x060006A5 RID: 1701 RVA: 0x00016021 File Offset: 0x00014221
		[Obsolete("Use ImportReference", false)]
		public MethodReference Import(MethodBase method)
		{
			return this.ImportReference(method, null);
		}

		// Token: 0x060006A6 RID: 1702 RVA: 0x0001602B File Offset: 0x0001422B
		[Obsolete("Use ImportReference", false)]
		public MethodReference Import(MethodBase method, IGenericParameterProvider context)
		{
			return this.ImportReference(method, context);
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x00016021 File Offset: 0x00014221
		public MethodReference ImportReference(MethodBase method)
		{
			return this.ImportReference(method, null);
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x00016035 File Offset: 0x00014235
		public MethodReference ImportReference(MethodBase method, IGenericParameterProvider context)
		{
			Mixin.CheckMethod(method);
			ModuleDefinition.CheckContext(context, this);
			return this.ReflectionImporter.ImportReference(method, context);
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x00016051 File Offset: 0x00014251
		[Obsolete("Use ImportReference", false)]
		public TypeReference Import(TypeReference type)
		{
			return this.ImportReference(type, null);
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x0001605B File Offset: 0x0001425B
		[Obsolete("Use ImportReference", false)]
		public TypeReference Import(TypeReference type, IGenericParameterProvider context)
		{
			return this.ImportReference(type, context);
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x00016051 File Offset: 0x00014251
		public TypeReference ImportReference(TypeReference type)
		{
			return this.ImportReference(type, null);
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x00016065 File Offset: 0x00014265
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

		// Token: 0x060006AD RID: 1709 RVA: 0x0001608C File Offset: 0x0001428C
		[Obsolete("Use ImportReference", false)]
		public FieldReference Import(FieldReference field)
		{
			return this.ImportReference(field, null);
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x00016096 File Offset: 0x00014296
		[Obsolete("Use ImportReference", false)]
		public FieldReference Import(FieldReference field, IGenericParameterProvider context)
		{
			return this.ImportReference(field, context);
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x0001608C File Offset: 0x0001428C
		public FieldReference ImportReference(FieldReference field)
		{
			return this.ImportReference(field, null);
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x000160A0 File Offset: 0x000142A0
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

		// Token: 0x060006B1 RID: 1713 RVA: 0x000160C7 File Offset: 0x000142C7
		[Obsolete("Use ImportReference", false)]
		public MethodReference Import(MethodReference method)
		{
			return this.ImportReference(method, null);
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x000160D1 File Offset: 0x000142D1
		[Obsolete("Use ImportReference", false)]
		public MethodReference Import(MethodReference method, IGenericParameterProvider context)
		{
			return this.ImportReference(method, context);
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x000160C7 File Offset: 0x000142C7
		public MethodReference ImportReference(MethodReference method)
		{
			return this.ImportReference(method, null);
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x000160DB File Offset: 0x000142DB
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

		// Token: 0x060006B5 RID: 1717 RVA: 0x00016102 File Offset: 0x00014302
		public IMetadataTokenProvider LookupToken(int token)
		{
			return this.LookupToken(new MetadataToken((uint)token));
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x00016110 File Offset: 0x00014310
		public IMetadataTokenProvider LookupToken(MetadataToken token)
		{
			return this.Read<MetadataToken, IMetadataTokenProvider>(token, (MetadataToken t, MetadataReader reader) => reader.LookupToken(t));
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x060006B7 RID: 1719 RVA: 0x00016138 File Offset: 0x00014338
		internal object SyncRoot
		{
			get
			{
				return this.module_lock;
			}
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x00016140 File Offset: 0x00014340
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

		// Token: 0x060006B9 RID: 1721 RVA: 0x000161B8 File Offset: 0x000143B8
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

		// Token: 0x060006BA RID: 1722 RVA: 0x00016234 File Offset: 0x00014434
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

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x060006BB RID: 1723 RVA: 0x000162D8 File Offset: 0x000144D8
		public bool HasDebugHeader
		{
			get
			{
				return this.Image != null && this.Image.DebugHeader != null;
			}
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x000162F2 File Offset: 0x000144F2
		public ImageDebugHeader GetDebugHeader()
		{
			return this.Image.DebugHeader ?? new ImageDebugHeader();
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x00016308 File Offset: 0x00014508
		public static ModuleDefinition CreateModule(string name, ModuleKind kind)
		{
			return ModuleDefinition.CreateModule(name, new ModuleParameters
			{
				Kind = kind
			});
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x0001631C File Offset: 0x0001451C
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

		// Token: 0x060006BF RID: 1727 RVA: 0x00016452 File Offset: 0x00014652
		private static AssemblyNameDefinition CreateAssemblyName(string name)
		{
			if (name.EndsWith(".dll") || name.EndsWith(".exe"))
			{
				name = name.Substring(0, name.Length - 4);
			}
			return new AssemblyNameDefinition(name, Mixin.ZeroVersion);
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0001648C File Offset: 0x0001468C
		public void ReadSymbols()
		{
			if (string.IsNullOrEmpty(this.file_name))
			{
				throw new InvalidOperationException();
			}
			DefaultSymbolReaderProvider defaultSymbolReaderProvider = new DefaultSymbolReaderProvider(true);
			this.ReadSymbols(defaultSymbolReaderProvider.GetSymbolReader(this, this.file_name), true);
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x000164C7 File Offset: 0x000146C7
		public void ReadSymbols(ISymbolReader reader)
		{
			this.ReadSymbols(reader, true);
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x000164D4 File Offset: 0x000146D4
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

		// Token: 0x060006C3 RID: 1731 RVA: 0x00016541 File Offset: 0x00014741
		public static ModuleDefinition ReadModule(string fileName)
		{
			return ModuleDefinition.ReadModule(fileName, new ReaderParameters(ReadingMode.Deferred));
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x00016550 File Offset: 0x00014750
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

		// Token: 0x060006C5 RID: 1733 RVA: 0x000165E8 File Offset: 0x000147E8
		private static Stream GetFileStream(string fileName, FileMode mode, FileAccess access, FileShare share)
		{
			Mixin.CheckFileName(fileName);
			return new FileStream(fileName, mode, access, share);
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x000165F9 File Offset: 0x000147F9
		public static ModuleDefinition ReadModule(Stream stream)
		{
			return ModuleDefinition.ReadModule(stream, new ReaderParameters(ReadingMode.Deferred));
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x00016607 File Offset: 0x00014807
		public static ModuleDefinition ReadModule(Stream stream, ReaderParameters parameters)
		{
			Mixin.CheckStream(stream);
			Mixin.CheckReadSeek(stream);
			return ModuleDefinition.ReadModule(Disposable.NotOwned<Stream>(stream), stream.GetFileName(), parameters);
		}

		// Token: 0x060006C8 RID: 1736 RVA: 0x00016627 File Offset: 0x00014827
		private static ModuleDefinition ReadModule(Disposable<Stream> stream, string fileName, ReaderParameters parameters)
		{
			Mixin.CheckParameters(parameters);
			return ModuleReader.CreateModule(ImageReader.ReadImage(stream, fileName), parameters);
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x0001663C File Offset: 0x0001483C
		public void Write(string fileName)
		{
			this.Write(fileName, new WriterParameters());
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0001664C File Offset: 0x0001484C
		public void Write(string fileName, WriterParameters parameters)
		{
			Mixin.CheckParameters(parameters);
			Stream fileStream = ModuleDefinition.GetFileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
			ModuleWriter.WriteModule(this, Disposable.Owned<Stream>(fileStream), parameters);
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x00016676 File Offset: 0x00014876
		public void Write()
		{
			this.Write(new WriterParameters());
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x00016683 File Offset: 0x00014883
		public void Write(WriterParameters parameters)
		{
			if (!this.HasImage)
			{
				throw new InvalidOperationException();
			}
			this.Write(this.Image.Stream.value, parameters);
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x000166AA File Offset: 0x000148AA
		public void Write(Stream stream)
		{
			this.Write(stream, new WriterParameters());
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x000166B8 File Offset: 0x000148B8
		public void Write(Stream stream, WriterParameters parameters)
		{
			Mixin.CheckStream(stream);
			Mixin.CheckWriteSeek(stream);
			Mixin.CheckParameters(parameters);
			ModuleWriter.WriteModule(this, Disposable.NotOwned<Stream>(stream), parameters);
		}

		// Token: 0x0400018C RID: 396
		internal Image Image;

		// Token: 0x0400018D RID: 397
		internal MetadataSystem MetadataSystem;

		// Token: 0x0400018E RID: 398
		internal ReadingMode ReadingMode;

		// Token: 0x0400018F RID: 399
		internal ISymbolReaderProvider SymbolReaderProvider;

		// Token: 0x04000190 RID: 400
		internal ISymbolReader symbol_reader;

		// Token: 0x04000191 RID: 401
		internal Disposable<IAssemblyResolver> assembly_resolver;

		// Token: 0x04000192 RID: 402
		internal IMetadataResolver metadata_resolver;

		// Token: 0x04000193 RID: 403
		internal TypeSystem type_system;

		// Token: 0x04000194 RID: 404
		internal readonly MetadataReader reader;

		// Token: 0x04000195 RID: 405
		private readonly string file_name;

		// Token: 0x04000196 RID: 406
		internal string runtime_version;

		// Token: 0x04000197 RID: 407
		internal ModuleKind kind;

		// Token: 0x04000198 RID: 408
		private WindowsRuntimeProjections projections;

		// Token: 0x04000199 RID: 409
		private MetadataKind metadata_kind;

		// Token: 0x0400019A RID: 410
		private TargetRuntime runtime;

		// Token: 0x0400019B RID: 411
		private TargetArchitecture architecture;

		// Token: 0x0400019C RID: 412
		private ModuleAttributes attributes;

		// Token: 0x0400019D RID: 413
		private ModuleCharacteristics characteristics;

		// Token: 0x0400019E RID: 414
		private Guid mvid;

		// Token: 0x0400019F RID: 415
		internal ushort linker_version = 8;

		// Token: 0x040001A0 RID: 416
		internal ushort subsystem_major = 4;

		// Token: 0x040001A1 RID: 417
		internal ushort subsystem_minor;

		// Token: 0x040001A2 RID: 418
		internal uint timestamp;

		// Token: 0x040001A3 RID: 419
		internal AssemblyDefinition assembly;

		// Token: 0x040001A4 RID: 420
		private MethodDefinition entry_point;

		// Token: 0x040001A5 RID: 421
		internal IReflectionImporter reflection_importer;

		// Token: 0x040001A6 RID: 422
		internal IMetadataImporter metadata_importer;

		// Token: 0x040001A7 RID: 423
		private Collection<CustomAttribute> custom_attributes;

		// Token: 0x040001A8 RID: 424
		private Collection<AssemblyNameReference> references;

		// Token: 0x040001A9 RID: 425
		private Collection<ModuleReference> modules;

		// Token: 0x040001AA RID: 426
		private Collection<Resource> resources;

		// Token: 0x040001AB RID: 427
		private Collection<ExportedType> exported_types;

		// Token: 0x040001AC RID: 428
		private TypeDefinitionCollection types;

		// Token: 0x040001AD RID: 429
		internal Collection<CustomDebugInformation> custom_infos;

		// Token: 0x040001AE RID: 430
		internal MetadataBuilder metadata_builder;

		// Token: 0x040001AF RID: 431
		private readonly object module_lock = new object();
	}
}
