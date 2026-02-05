using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using Mono.Cecil.Cil;
using Mono.Cecil.Metadata;
using Mono.Cecil.PE;
using Mono.Collections.Generic;

namespace Mono.Cecil
{
	// Token: 0x02000018 RID: 24
	internal sealed class MetadataReader : ByteBuffer
	{
		// Token: 0x06000122 RID: 290 RVA: 0x00005E3C File Offset: 0x0000403C
		public MetadataReader(ModuleDefinition module)
			: base(module.Image.TableHeap.data)
		{
			this.image = module.Image;
			this.module = module;
			this.metadata = module.MetadataSystem;
			this.code = new CodeReader(this);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00005E8A File Offset: 0x0000408A
		public MetadataReader(Image image, ModuleDefinition module, MetadataReader metadata_reader)
			: base(image.TableHeap.data)
		{
			this.image = image;
			this.module = module;
			this.metadata = module.MetadataSystem;
			this.metadata_reader = metadata_reader;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00005EBE File Offset: 0x000040BE
		private int GetCodedIndexSize(CodedIndex index)
		{
			return this.image.GetCodedIndexSize(index);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00005ECC File Offset: 0x000040CC
		private uint ReadByIndexSize(int size)
		{
			if (size == 4)
			{
				return base.ReadUInt32();
			}
			return (uint)base.ReadUInt16();
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00005EE0 File Offset: 0x000040E0
		private byte[] ReadBlob()
		{
			BlobHeap blobHeap = this.image.BlobHeap;
			if (blobHeap == null)
			{
				this.position += 2;
				return Empty<byte>.Array;
			}
			return blobHeap.Read(this.ReadBlobIndex());
		}

		// Token: 0x06000127 RID: 295 RVA: 0x00005F1C File Offset: 0x0000411C
		private byte[] ReadBlob(uint signature)
		{
			BlobHeap blobHeap = this.image.BlobHeap;
			if (blobHeap == null)
			{
				return Empty<byte>.Array;
			}
			return blobHeap.Read(signature);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00005F48 File Offset: 0x00004148
		private uint ReadBlobIndex()
		{
			BlobHeap blobHeap = this.image.BlobHeap;
			return this.ReadByIndexSize((blobHeap != null) ? blobHeap.IndexSize : 2);
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00005F74 File Offset: 0x00004174
		private void GetBlobView(uint signature, out byte[] blob, out int index, out int count)
		{
			BlobHeap blobHeap = this.image.BlobHeap;
			if (blobHeap == null)
			{
				blob = null;
				index = (count = 0);
				return;
			}
			blobHeap.GetView(signature, out blob, out index, out count);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00005FA8 File Offset: 0x000041A8
		private string ReadString()
		{
			return this.image.StringHeap.Read(this.ReadByIndexSize(this.image.StringHeap.IndexSize));
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00005FD0 File Offset: 0x000041D0
		private uint ReadStringIndex()
		{
			return this.ReadByIndexSize(this.image.StringHeap.IndexSize);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00005FE8 File Offset: 0x000041E8
		private Guid ReadGuid()
		{
			return this.image.GuidHeap.Read(this.ReadByIndexSize(this.image.GuidHeap.IndexSize));
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00006010 File Offset: 0x00004210
		private uint ReadTableIndex(Table table)
		{
			return this.ReadByIndexSize(this.image.GetTableIndexSize(table));
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00006024 File Offset: 0x00004224
		private MetadataToken ReadMetadataToken(CodedIndex index)
		{
			return index.GetMetadataToken(this.ReadByIndexSize(this.GetCodedIndexSize(index)));
		}

		// Token: 0x0600012F RID: 303 RVA: 0x0000603C File Offset: 0x0000423C
		private int MoveTo(Table table)
		{
			TableInformation tableInformation = this.image.TableHeap[table];
			if (tableInformation.Length != 0U)
			{
				this.position = (int)tableInformation.Offset;
			}
			return (int)tableInformation.Length;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00006078 File Offset: 0x00004278
		private bool MoveTo(Table table, uint row)
		{
			TableInformation tableInformation = this.image.TableHeap[table];
			uint length = tableInformation.Length;
			if (length == 0U || row > length)
			{
				return false;
			}
			this.position = (int)(tableInformation.Offset + tableInformation.RowSize * (row - 1U));
			return true;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x000060C0 File Offset: 0x000042C0
		public AssemblyNameDefinition ReadAssemblyNameDefinition()
		{
			if (this.MoveTo(Table.Assembly) == 0)
			{
				return null;
			}
			AssemblyNameDefinition assemblyNameDefinition = new AssemblyNameDefinition();
			assemblyNameDefinition.HashAlgorithm = (AssemblyHashAlgorithm)base.ReadUInt32();
			this.PopulateVersionAndFlags(assemblyNameDefinition);
			assemblyNameDefinition.PublicKey = this.ReadBlob();
			this.PopulateNameAndCulture(assemblyNameDefinition);
			return assemblyNameDefinition;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00006106 File Offset: 0x00004306
		public ModuleDefinition Populate(ModuleDefinition module)
		{
			if (this.MoveTo(Table.Module) == 0)
			{
				return module;
			}
			base.Advance(2);
			module.Name = this.ReadString();
			module.Mvid = this.ReadGuid();
			return module;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00006134 File Offset: 0x00004334
		private void InitializeAssemblyReferences()
		{
			if (this.metadata.AssemblyReferences != null)
			{
				return;
			}
			int num = this.MoveTo(Table.AssemblyRef);
			AssemblyNameReference[] array = (this.metadata.AssemblyReferences = new AssemblyNameReference[num]);
			uint num2 = 0U;
			while ((ulong)num2 < (ulong)((long)num))
			{
				AssemblyNameReference assemblyNameReference = new AssemblyNameReference();
				assemblyNameReference.token = new MetadataToken(TokenType.AssemblyRef, num2 + 1U);
				this.PopulateVersionAndFlags(assemblyNameReference);
				byte[] array2 = this.ReadBlob();
				if (assemblyNameReference.HasPublicKey)
				{
					assemblyNameReference.PublicKey = array2;
				}
				else
				{
					assemblyNameReference.PublicKeyToken = array2;
				}
				this.PopulateNameAndCulture(assemblyNameReference);
				assemblyNameReference.Hash = this.ReadBlob();
				array[(int)num2] = assemblyNameReference;
				num2 += 1U;
			}
		}

		// Token: 0x06000134 RID: 308 RVA: 0x000061E0 File Offset: 0x000043E0
		public Collection<AssemblyNameReference> ReadAssemblyReferences()
		{
			this.InitializeAssemblyReferences();
			Collection<AssemblyNameReference> collection = new Collection<AssemblyNameReference>(this.metadata.AssemblyReferences);
			if (this.module.IsWindowsMetadata())
			{
				this.module.Projections.AddVirtualReferences(collection);
			}
			return collection;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00006224 File Offset: 0x00004424
		public MethodDefinition ReadEntryPoint()
		{
			if (this.module.Image.EntryPointToken == 0U)
			{
				return null;
			}
			MetadataToken metadataToken = new MetadataToken(this.module.Image.EntryPointToken);
			return this.GetMethodDefinition(metadataToken.RID);
		}

		// Token: 0x06000136 RID: 310 RVA: 0x0000626C File Offset: 0x0000446C
		public Collection<ModuleDefinition> ReadModules()
		{
			Collection<ModuleDefinition> collection = new Collection<ModuleDefinition>(1);
			collection.Add(this.module);
			int num = this.MoveTo(Table.File);
			uint num2 = 1U;
			while ((ulong)num2 <= (ulong)((long)num))
			{
				bool flag = base.ReadUInt32() != 0U;
				string text = this.ReadString();
				this.ReadBlobIndex();
				if (!flag)
				{
					ReaderParameters readerParameters = new ReaderParameters
					{
						ReadingMode = this.module.ReadingMode,
						SymbolReaderProvider = this.module.SymbolReaderProvider,
						AssemblyResolver = this.module.AssemblyResolver
					};
					collection.Add(ModuleDefinition.ReadModule(this.GetModuleFileName(text), readerParameters));
				}
				num2 += 1U;
			}
			return collection;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00006308 File Offset: 0x00004508
		private string GetModuleFileName(string name)
		{
			if (this.module.FileName == null)
			{
				throw new NotSupportedException();
			}
			return Path.Combine(Path.GetDirectoryName(this.module.FileName), name);
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00006334 File Offset: 0x00004534
		private void InitializeModuleReferences()
		{
			if (this.metadata.ModuleReferences != null)
			{
				return;
			}
			int num = this.MoveTo(Table.ModuleRef);
			ModuleReference[] array = (this.metadata.ModuleReferences = new ModuleReference[num]);
			uint num2 = 0U;
			while ((ulong)num2 < (ulong)((long)num))
			{
				array[(int)num2] = new ModuleReference(this.ReadString())
				{
					token = new MetadataToken(TokenType.ModuleRef, num2 + 1U)
				};
				num2 += 1U;
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x000063A1 File Offset: 0x000045A1
		public Collection<ModuleReference> ReadModuleReferences()
		{
			this.InitializeModuleReferences();
			return new Collection<ModuleReference>(this.metadata.ModuleReferences);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x000063BC File Offset: 0x000045BC
		public bool HasFileResource()
		{
			int num = this.MoveTo(Table.File);
			if (num == 0)
			{
				return false;
			}
			uint num2 = 1U;
			while ((ulong)num2 <= (ulong)((long)num))
			{
				if (this.ReadFileRecord(num2).Col1 == FileAttributes.ContainsNoMetaData)
				{
					return true;
				}
				num2 += 1U;
			}
			return false;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x000063F8 File Offset: 0x000045F8
		public Collection<Resource> ReadResources()
		{
			int num = this.MoveTo(Table.ManifestResource);
			Collection<Resource> collection = new Collection<Resource>(num);
			int i = 1;
			while (i <= num)
			{
				uint num2 = base.ReadUInt32();
				ManifestResourceAttributes manifestResourceAttributes = (ManifestResourceAttributes)base.ReadUInt32();
				string text = this.ReadString();
				MetadataToken metadataToken = this.ReadMetadataToken(CodedIndex.Implementation);
				Resource resource;
				if (metadataToken.RID == 0U)
				{
					resource = new EmbeddedResource(text, manifestResourceAttributes, num2, this);
					goto IL_00C6;
				}
				if (metadataToken.TokenType == TokenType.AssemblyRef)
				{
					resource = new AssemblyLinkedResource(text, manifestResourceAttributes)
					{
						Assembly = (AssemblyNameReference)this.GetTypeReferenceScope(metadataToken)
					};
					goto IL_00C6;
				}
				if (metadataToken.TokenType == TokenType.File)
				{
					Row<FileAttributes, string, uint> row = this.ReadFileRecord(metadataToken.RID);
					resource = new LinkedResource(text, manifestResourceAttributes)
					{
						File = row.Col2,
						hash = this.ReadBlob(row.Col3)
					};
					goto IL_00C6;
				}
				IL_00CE:
				i++;
				continue;
				IL_00C6:
				collection.Add(resource);
				goto IL_00CE;
			}
			return collection;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x000064E0 File Offset: 0x000046E0
		private Row<FileAttributes, string, uint> ReadFileRecord(uint rid)
		{
			int position = this.position;
			if (!this.MoveTo(Table.File, rid))
			{
				throw new ArgumentException();
			}
			Row<FileAttributes, string, uint> row = new Row<FileAttributes, string, uint>((FileAttributes)base.ReadUInt32(), this.ReadString(), this.ReadBlobIndex());
			this.position = position;
			return row;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00006524 File Offset: 0x00004724
		public byte[] GetManagedResource(uint offset)
		{
			return this.image.GetReaderAt<uint, byte[]>(this.image.Resources.VirtualAddress, offset, delegate(uint o, BinaryStreamReader reader)
			{
				reader.Advance((int)o);
				return reader.ReadBytes(reader.ReadInt32());
			}) ?? Empty<byte>.Array;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00006575 File Offset: 0x00004775
		private void PopulateVersionAndFlags(AssemblyNameReference name)
		{
			name.Version = new Version((int)base.ReadUInt16(), (int)base.ReadUInt16(), (int)base.ReadUInt16(), (int)base.ReadUInt16());
			name.Attributes = (AssemblyAttributes)base.ReadUInt32();
		}

		// Token: 0x0600013F RID: 319 RVA: 0x000065A6 File Offset: 0x000047A6
		private void PopulateNameAndCulture(AssemblyNameReference name)
		{
			name.Name = this.ReadString();
			name.Culture = this.ReadString();
		}

		// Token: 0x06000140 RID: 320 RVA: 0x000065C0 File Offset: 0x000047C0
		public TypeDefinitionCollection ReadTypes()
		{
			this.InitializeTypeDefinitions();
			TypeDefinition[] types = this.metadata.Types;
			int num = types.Length - this.metadata.NestedTypes.Count;
			TypeDefinitionCollection typeDefinitionCollection = new TypeDefinitionCollection(this.module, num);
			foreach (TypeDefinition typeDefinition in types)
			{
				if (!MetadataReader.IsNested(typeDefinition.Attributes))
				{
					typeDefinitionCollection.Add(typeDefinition);
				}
			}
			if (this.image.HasTable(Table.MethodPtr) || this.image.HasTable(Table.FieldPtr))
			{
				this.CompleteTypes();
			}
			return typeDefinitionCollection;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00006650 File Offset: 0x00004850
		private void CompleteTypes()
		{
			foreach (TypeDefinition typeDefinition in this.metadata.Types)
			{
				Mixin.Read(typeDefinition.Fields);
				Mixin.Read(typeDefinition.Methods);
			}
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00006690 File Offset: 0x00004890
		private void InitializeTypeDefinitions()
		{
			if (this.metadata.Types != null)
			{
				return;
			}
			this.InitializeNestedTypes();
			this.InitializeFields();
			this.InitializeMethods();
			int num = this.MoveTo(Table.TypeDef);
			TypeDefinition[] array = (this.metadata.Types = new TypeDefinition[num]);
			uint num2 = 0U;
			while ((ulong)num2 < (ulong)((long)num))
			{
				if (array[(int)num2] == null)
				{
					array[(int)num2] = this.ReadType(num2 + 1U);
				}
				num2 += 1U;
			}
			if (this.module.IsWindowsMetadata())
			{
				uint num3 = 0U;
				while ((ulong)num3 < (ulong)((long)num))
				{
					WindowsRuntimeProjections.Project(array[(int)num3]);
					num3 += 1U;
				}
			}
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00006724 File Offset: 0x00004924
		private static bool IsNested(TypeAttributes attributes)
		{
			switch (attributes & TypeAttributes.VisibilityMask)
			{
			case TypeAttributes.NestedPublic:
			case TypeAttributes.NestedPrivate:
			case TypeAttributes.NestedFamily:
			case TypeAttributes.NestedAssembly:
			case TypeAttributes.NestedFamANDAssem:
			case TypeAttributes.VisibilityMask:
				return true;
			default:
				return false;
			}
		}

		// Token: 0x06000144 RID: 324 RVA: 0x0000675C File Offset: 0x0000495C
		public bool HasNestedTypes(TypeDefinition type)
		{
			this.InitializeNestedTypes();
			Collection<uint> collection;
			return this.metadata.TryGetNestedTypeMapping(type, out collection) && collection.Count > 0;
		}

		// Token: 0x06000145 RID: 325 RVA: 0x0000678C File Offset: 0x0000498C
		public Collection<TypeDefinition> ReadNestedTypes(TypeDefinition type)
		{
			this.InitializeNestedTypes();
			Collection<uint> collection;
			if (!this.metadata.TryGetNestedTypeMapping(type, out collection))
			{
				return new MemberDefinitionCollection<TypeDefinition>(type);
			}
			MemberDefinitionCollection<TypeDefinition> memberDefinitionCollection = new MemberDefinitionCollection<TypeDefinition>(type, collection.Count);
			for (int i = 0; i < collection.Count; i++)
			{
				TypeDefinition typeDefinition = this.GetTypeDefinition(collection[i]);
				if (typeDefinition != null)
				{
					memberDefinitionCollection.Add(typeDefinition);
				}
			}
			this.metadata.RemoveNestedTypeMapping(type);
			return memberDefinitionCollection;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x000067FC File Offset: 0x000049FC
		private void InitializeNestedTypes()
		{
			if (this.metadata.NestedTypes != null)
			{
				return;
			}
			int num = this.MoveTo(Table.NestedClass);
			this.metadata.NestedTypes = new Dictionary<uint, Collection<uint>>(num);
			this.metadata.ReverseNestedTypes = new Dictionary<uint, uint>(num);
			if (num == 0)
			{
				return;
			}
			for (int i = 1; i <= num; i++)
			{
				uint num2 = this.ReadTableIndex(Table.TypeDef);
				uint num3 = this.ReadTableIndex(Table.TypeDef);
				this.AddNestedMapping(num3, num2);
			}
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0000686A File Offset: 0x00004A6A
		private void AddNestedMapping(uint declaring, uint nested)
		{
			this.metadata.SetNestedTypeMapping(declaring, MetadataReader.AddMapping<uint, uint>(this.metadata.NestedTypes, declaring, nested));
			this.metadata.SetReverseNestedTypeMapping(nested, declaring);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00006898 File Offset: 0x00004A98
		private static Collection<TValue> AddMapping<TKey, TValue>(Dictionary<TKey, Collection<TValue>> cache, TKey key, TValue value)
		{
			Collection<TValue> collection;
			if (!cache.TryGetValue(key, out collection))
			{
				collection = new Collection<TValue>();
			}
			collection.Add(value);
			return collection;
		}

		// Token: 0x06000149 RID: 329 RVA: 0x000068C0 File Offset: 0x00004AC0
		private TypeDefinition ReadType(uint rid)
		{
			if (!this.MoveTo(Table.TypeDef, rid))
			{
				return null;
			}
			TypeAttributes typeAttributes = (TypeAttributes)base.ReadUInt32();
			string text = this.ReadString();
			TypeDefinition typeDefinition = new TypeDefinition(this.ReadString(), text, typeAttributes);
			typeDefinition.token = new MetadataToken(TokenType.TypeDef, rid);
			typeDefinition.scope = this.module;
			typeDefinition.module = this.module;
			this.metadata.AddTypeDefinition(typeDefinition);
			this.context = typeDefinition;
			typeDefinition.BaseType = this.GetTypeDefOrRef(this.ReadMetadataToken(CodedIndex.TypeDefOrRef));
			typeDefinition.fields_range = this.ReadListRange(rid, Table.TypeDef, Table.Field);
			typeDefinition.methods_range = this.ReadListRange(rid, Table.TypeDef, Table.Method);
			if (MetadataReader.IsNested(typeAttributes))
			{
				typeDefinition.DeclaringType = this.GetNestedTypeDeclaringType(typeDefinition);
			}
			return typeDefinition;
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00006978 File Offset: 0x00004B78
		private TypeDefinition GetNestedTypeDeclaringType(TypeDefinition type)
		{
			uint num;
			if (!this.metadata.TryGetReverseNestedTypeMapping(type, out num))
			{
				return null;
			}
			this.metadata.RemoveReverseNestedTypeMapping(type);
			return this.GetTypeDefinition(num);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x000069AC File Offset: 0x00004BAC
		private Range ReadListRange(uint current_index, Table current, Table target)
		{
			Range range = default(Range);
			uint num = this.ReadTableIndex(target);
			if (num == 0U)
			{
				return range;
			}
			TableInformation tableInformation = this.image.TableHeap[current];
			uint num2;
			if (current_index == tableInformation.Length)
			{
				num2 = this.image.TableHeap[target].Length + 1U;
			}
			else
			{
				int position = this.position;
				this.position += (int)((ulong)tableInformation.RowSize - (ulong)((long)this.image.GetTableIndexSize(target)));
				num2 = this.ReadTableIndex(target);
				this.position = position;
			}
			range.Start = num;
			range.Length = num2 - num;
			return range;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00006A54 File Offset: 0x00004C54
		public Row<short, int> ReadTypeLayout(TypeDefinition type)
		{
			this.InitializeTypeLayouts();
			uint rid = type.token.RID;
			Row<ushort, uint> row;
			if (!this.metadata.ClassLayouts.TryGetValue(rid, out row))
			{
				return new Row<short, int>(-1, -1);
			}
			type.PackingSize = (short)row.Col1;
			type.ClassSize = (int)row.Col2;
			this.metadata.ClassLayouts.Remove(rid);
			return new Row<short, int>((short)row.Col1, (int)row.Col2);
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00006AD0 File Offset: 0x00004CD0
		private void InitializeTypeLayouts()
		{
			if (this.metadata.ClassLayouts != null)
			{
				return;
			}
			int num = this.MoveTo(Table.ClassLayout);
			Dictionary<uint, Row<ushort, uint>> dictionary = (this.metadata.ClassLayouts = new Dictionary<uint, Row<ushort, uint>>(num));
			uint num2 = 0U;
			while ((ulong)num2 < (ulong)((long)num))
			{
				ushort num3 = base.ReadUInt16();
				uint num4 = base.ReadUInt32();
				uint num5 = this.ReadTableIndex(Table.TypeDef);
				dictionary.Add(num5, new Row<ushort, uint>(num3, num4));
				num2 += 1U;
			}
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00006B41 File Offset: 0x00004D41
		public TypeReference GetTypeDefOrRef(MetadataToken token)
		{
			return (TypeReference)this.LookupToken(token);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00006B50 File Offset: 0x00004D50
		public TypeDefinition GetTypeDefinition(uint rid)
		{
			this.InitializeTypeDefinitions();
			TypeDefinition typeDefinition = this.metadata.GetTypeDefinition(rid);
			if (typeDefinition != null)
			{
				return typeDefinition;
			}
			typeDefinition = this.ReadTypeDefinition(rid);
			if (this.module.IsWindowsMetadata())
			{
				WindowsRuntimeProjections.Project(typeDefinition);
			}
			return typeDefinition;
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00006B91 File Offset: 0x00004D91
		private TypeDefinition ReadTypeDefinition(uint rid)
		{
			if (!this.MoveTo(Table.TypeDef, rid))
			{
				return null;
			}
			return this.ReadType(rid);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00006BA6 File Offset: 0x00004DA6
		private void InitializeTypeReferences()
		{
			if (this.metadata.TypeReferences != null)
			{
				return;
			}
			this.metadata.TypeReferences = new TypeReference[this.image.GetTableLength(Table.TypeRef)];
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00006BD4 File Offset: 0x00004DD4
		public TypeReference GetTypeReference(string scope, string full_name)
		{
			this.InitializeTypeReferences();
			int num = this.metadata.TypeReferences.Length;
			uint num2 = 1U;
			while ((ulong)num2 <= (ulong)((long)num))
			{
				TypeReference typeReference = this.GetTypeReference(num2);
				if (!(typeReference.FullName != full_name))
				{
					if (string.IsNullOrEmpty(scope))
					{
						return typeReference;
					}
					if (typeReference.Scope.Name == scope)
					{
						return typeReference;
					}
				}
				num2 += 1U;
			}
			return null;
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00006C3C File Offset: 0x00004E3C
		private TypeReference GetTypeReference(uint rid)
		{
			this.InitializeTypeReferences();
			TypeReference typeReference = this.metadata.GetTypeReference(rid);
			if (typeReference != null)
			{
				return typeReference;
			}
			return this.ReadTypeReference(rid);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00006C68 File Offset: 0x00004E68
		private TypeReference ReadTypeReference(uint rid)
		{
			if (!this.MoveTo(Table.TypeRef, rid))
			{
				return null;
			}
			TypeReference typeReference = null;
			MetadataToken metadataToken = this.ReadMetadataToken(CodedIndex.ResolutionScope);
			string text = this.ReadString();
			TypeReference typeReference2 = new TypeReference(this.ReadString(), text, this.module, null);
			typeReference2.token = new MetadataToken(TokenType.TypeRef, rid);
			this.metadata.AddTypeReference(typeReference2);
			IMetadataScope metadataScope3;
			if (metadataToken.TokenType == TokenType.TypeRef)
			{
				if (metadataToken.RID != rid)
				{
					typeReference = this.GetTypeDefOrRef(metadataToken);
					IMetadataScope metadataScope2;
					if (typeReference == null)
					{
						IMetadataScope metadataScope = this.module;
						metadataScope2 = metadataScope;
					}
					else
					{
						metadataScope2 = typeReference.Scope;
					}
					metadataScope3 = metadataScope2;
				}
				else
				{
					metadataScope3 = this.module;
				}
			}
			else
			{
				metadataScope3 = this.GetTypeReferenceScope(metadataToken);
			}
			typeReference2.scope = metadataScope3;
			typeReference2.DeclaringType = typeReference;
			MetadataSystem.TryProcessPrimitiveTypeReference(typeReference2);
			if (typeReference2.Module.IsWindowsMetadata())
			{
				WindowsRuntimeProjections.Project(typeReference2);
			}
			return typeReference2;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00006D40 File Offset: 0x00004F40
		private IMetadataScope GetTypeReferenceScope(MetadataToken scope)
		{
			if (scope.TokenType == TokenType.Module)
			{
				return this.module;
			}
			TokenType tokenType = scope.TokenType;
			IMetadataScope[] array;
			if (tokenType != TokenType.ModuleRef)
			{
				if (tokenType != TokenType.AssemblyRef)
				{
					throw new NotSupportedException();
				}
				this.InitializeAssemblyReferences();
				array = this.metadata.AssemblyReferences;
			}
			else
			{
				this.InitializeModuleReferences();
				array = this.metadata.ModuleReferences;
			}
			uint num = scope.RID - 1U;
			if (num < 0U || (ulong)num >= (ulong)((long)array.Length))
			{
				return null;
			}
			return array[(int)num];
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00006DC0 File Offset: 0x00004FC0
		public IEnumerable<TypeReference> GetTypeReferences()
		{
			this.InitializeTypeReferences();
			int tableLength = this.image.GetTableLength(Table.TypeRef);
			TypeReference[] array = new TypeReference[tableLength];
			uint num = 1U;
			while ((ulong)num <= (ulong)((long)tableLength))
			{
				array[(int)(num - 1U)] = this.GetTypeReference(num);
				num += 1U;
			}
			return array;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00006E04 File Offset: 0x00005004
		private TypeReference GetTypeSpecification(uint rid)
		{
			if (!this.MoveTo(Table.TypeSpec, rid))
			{
				return null;
			}
			TypeReference typeReference = this.ReadSignature(this.ReadBlobIndex()).ReadTypeSignature();
			if (typeReference.token.RID == 0U)
			{
				typeReference.token = new MetadataToken(TokenType.TypeSpec, rid);
			}
			return typeReference;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00006E4F File Offset: 0x0000504F
		private SignatureReader ReadSignature(uint signature)
		{
			return new SignatureReader(signature, this);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00006E58 File Offset: 0x00005058
		public bool HasInterfaces(TypeDefinition type)
		{
			this.InitializeInterfaces();
			Collection<Row<uint, MetadataToken>> collection;
			return this.metadata.TryGetInterfaceMapping(type, out collection);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00006E7C File Offset: 0x0000507C
		public InterfaceImplementationCollection ReadInterfaces(TypeDefinition type)
		{
			this.InitializeInterfaces();
			Collection<Row<uint, MetadataToken>> collection;
			if (!this.metadata.TryGetInterfaceMapping(type, out collection))
			{
				return new InterfaceImplementationCollection(type);
			}
			InterfaceImplementationCollection interfaceImplementationCollection = new InterfaceImplementationCollection(type, collection.Count);
			this.context = type;
			for (int i = 0; i < collection.Count; i++)
			{
				interfaceImplementationCollection.Add(new InterfaceImplementation(this.GetTypeDefOrRef(collection[i].Col2), new MetadataToken(TokenType.InterfaceImpl, collection[i].Col1)));
			}
			this.metadata.RemoveInterfaceMapping(type);
			return interfaceImplementationCollection;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00006F0C File Offset: 0x0000510C
		private void InitializeInterfaces()
		{
			if (this.metadata.Interfaces != null)
			{
				return;
			}
			int num = this.MoveTo(Table.InterfaceImpl);
			this.metadata.Interfaces = new Dictionary<uint, Collection<Row<uint, MetadataToken>>>(num);
			uint num2 = 1U;
			while ((ulong)num2 <= (ulong)((long)num))
			{
				uint num3 = this.ReadTableIndex(Table.TypeDef);
				MetadataToken metadataToken = this.ReadMetadataToken(CodedIndex.TypeDefOrRef);
				this.AddInterfaceMapping(num3, new Row<uint, MetadataToken>(num2, metadataToken));
				num2 += 1U;
			}
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00006F6D File Offset: 0x0000516D
		private void AddInterfaceMapping(uint type, Row<uint, MetadataToken> @interface)
		{
			this.metadata.SetInterfaceMapping(type, MetadataReader.AddMapping<uint, Row<uint, MetadataToken>>(this.metadata.Interfaces, type, @interface));
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00006F90 File Offset: 0x00005190
		public Collection<FieldDefinition> ReadFields(TypeDefinition type)
		{
			Range fields_range = type.fields_range;
			if (fields_range.Length == 0U)
			{
				return new MemberDefinitionCollection<FieldDefinition>(type);
			}
			MemberDefinitionCollection<FieldDefinition> memberDefinitionCollection = new MemberDefinitionCollection<FieldDefinition>(type, (int)fields_range.Length);
			this.context = type;
			if (!this.MoveTo(Table.FieldPtr, fields_range.Start))
			{
				if (!this.MoveTo(Table.Field, fields_range.Start))
				{
					return memberDefinitionCollection;
				}
				for (uint num = 0U; num < fields_range.Length; num += 1U)
				{
					this.ReadField(fields_range.Start + num, memberDefinitionCollection);
				}
			}
			else
			{
				this.ReadPointers<FieldDefinition>(Table.FieldPtr, Table.Field, fields_range, memberDefinitionCollection, new Action<uint, Collection<FieldDefinition>>(this.ReadField));
			}
			return memberDefinitionCollection;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00007020 File Offset: 0x00005220
		private void ReadField(uint field_rid, Collection<FieldDefinition> fields)
		{
			FieldAttributes fieldAttributes = (FieldAttributes)base.ReadUInt16();
			string text = this.ReadString();
			uint num = this.ReadBlobIndex();
			FieldDefinition fieldDefinition = new FieldDefinition(text, fieldAttributes, this.ReadFieldType(num));
			fieldDefinition.token = new MetadataToken(TokenType.Field, field_rid);
			this.metadata.AddFieldDefinition(fieldDefinition);
			if (MetadataReader.IsDeleted(fieldDefinition))
			{
				return;
			}
			fields.Add(fieldDefinition);
			if (this.module.IsWindowsMetadata())
			{
				WindowsRuntimeProjections.Project(fieldDefinition);
			}
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000708F File Offset: 0x0000528F
		private void InitializeFields()
		{
			if (this.metadata.Fields != null)
			{
				return;
			}
			this.metadata.Fields = new FieldDefinition[this.image.GetTableLength(Table.Field)];
		}

		// Token: 0x06000160 RID: 352 RVA: 0x000070BB File Offset: 0x000052BB
		private TypeReference ReadFieldType(uint signature)
		{
			SignatureReader signatureReader = this.ReadSignature(signature);
			if (signatureReader.ReadByte() != 6)
			{
				throw new NotSupportedException();
			}
			return signatureReader.ReadTypeSignature();
		}

		// Token: 0x06000161 RID: 353 RVA: 0x000070D8 File Offset: 0x000052D8
		public int ReadFieldRVA(FieldDefinition field)
		{
			this.InitializeFieldRVAs();
			uint rid = field.token.RID;
			uint num;
			if (!this.metadata.FieldRVAs.TryGetValue(rid, out num))
			{
				return 0;
			}
			int fieldTypeSize = MetadataReader.GetFieldTypeSize(field.FieldType);
			if (fieldTypeSize == 0 || num == 0U)
			{
				return 0;
			}
			this.metadata.FieldRVAs.Remove(rid);
			field.InitialValue = this.GetFieldInitializeValue(fieldTypeSize, num);
			return (int)num;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00007143 File Offset: 0x00005343
		private byte[] GetFieldInitializeValue(int size, uint rva)
		{
			return this.image.GetReaderAt<int, byte[]>(rva, size, (int s, BinaryStreamReader reader) => reader.ReadBytes(s)) ?? Empty<byte>.Array;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0000717C File Offset: 0x0000537C
		private static int GetFieldTypeSize(TypeReference type)
		{
			int num = 0;
			switch (type.etype)
			{
			case ElementType.Boolean:
			case ElementType.I1:
			case ElementType.U1:
				return 1;
			case ElementType.Char:
			case ElementType.I2:
			case ElementType.U2:
				return 2;
			case ElementType.I4:
			case ElementType.U4:
			case ElementType.R4:
				return 4;
			case ElementType.I8:
			case ElementType.U8:
			case ElementType.R8:
				return 8;
			case ElementType.Ptr:
			case ElementType.FnPtr:
				return IntPtr.Size;
			case ElementType.CModReqD:
			case ElementType.CModOpt:
				return MetadataReader.GetFieldTypeSize(((IModifierType)type).ElementType);
			}
			TypeDefinition typeDefinition = type.Resolve();
			if (typeDefinition != null && typeDefinition.HasLayoutInfo)
			{
				num = typeDefinition.ClassSize;
			}
			return num;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000725C File Offset: 0x0000545C
		private void InitializeFieldRVAs()
		{
			if (this.metadata.FieldRVAs != null)
			{
				return;
			}
			int num = this.MoveTo(Table.FieldRVA);
			Dictionary<uint, uint> dictionary = (this.metadata.FieldRVAs = new Dictionary<uint, uint>(num));
			for (int i = 0; i < num; i++)
			{
				uint num2 = base.ReadUInt32();
				uint num3 = this.ReadTableIndex(Table.Field);
				dictionary.Add(num3, num2);
			}
		}

		// Token: 0x06000165 RID: 357 RVA: 0x000072BC File Offset: 0x000054BC
		public int ReadFieldLayout(FieldDefinition field)
		{
			this.InitializeFieldLayouts();
			uint rid = field.token.RID;
			uint num;
			if (!this.metadata.FieldLayouts.TryGetValue(rid, out num))
			{
				return -1;
			}
			this.metadata.FieldLayouts.Remove(rid);
			return (int)num;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x00007308 File Offset: 0x00005508
		private void InitializeFieldLayouts()
		{
			if (this.metadata.FieldLayouts != null)
			{
				return;
			}
			int num = this.MoveTo(Table.FieldLayout);
			Dictionary<uint, uint> dictionary = (this.metadata.FieldLayouts = new Dictionary<uint, uint>(num));
			for (int i = 0; i < num; i++)
			{
				uint num2 = base.ReadUInt32();
				uint num3 = this.ReadTableIndex(Table.Field);
				dictionary.Add(num3, num2);
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00007368 File Offset: 0x00005568
		public bool HasEvents(TypeDefinition type)
		{
			this.InitializeEvents();
			Range range;
			return this.metadata.TryGetEventsRange(type, out range) && range.Length > 0U;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00007398 File Offset: 0x00005598
		public Collection<EventDefinition> ReadEvents(TypeDefinition type)
		{
			this.InitializeEvents();
			Range range;
			if (!this.metadata.TryGetEventsRange(type, out range))
			{
				return new MemberDefinitionCollection<EventDefinition>(type);
			}
			MemberDefinitionCollection<EventDefinition> memberDefinitionCollection = new MemberDefinitionCollection<EventDefinition>(type, (int)range.Length);
			this.metadata.RemoveEventsRange(type);
			if (range.Length == 0U)
			{
				return memberDefinitionCollection;
			}
			this.context = type;
			if (!this.MoveTo(Table.EventPtr, range.Start))
			{
				if (!this.MoveTo(Table.Event, range.Start))
				{
					return memberDefinitionCollection;
				}
				for (uint num = 0U; num < range.Length; num += 1U)
				{
					this.ReadEvent(range.Start + num, memberDefinitionCollection);
				}
			}
			else
			{
				this.ReadPointers<EventDefinition>(Table.EventPtr, Table.Event, range, memberDefinitionCollection, new Action<uint, Collection<EventDefinition>>(this.ReadEvent));
			}
			return memberDefinitionCollection;
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000744C File Offset: 0x0000564C
		private void ReadEvent(uint event_rid, Collection<EventDefinition> events)
		{
			EventAttributes eventAttributes = (EventAttributes)base.ReadUInt16();
			string text = this.ReadString();
			TypeReference typeDefOrRef = this.GetTypeDefOrRef(this.ReadMetadataToken(CodedIndex.TypeDefOrRef));
			EventDefinition eventDefinition = new EventDefinition(text, eventAttributes, typeDefOrRef);
			eventDefinition.token = new MetadataToken(TokenType.Event, event_rid);
			if (MetadataReader.IsDeleted(eventDefinition))
			{
				return;
			}
			events.Add(eventDefinition);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x000074A0 File Offset: 0x000056A0
		private void InitializeEvents()
		{
			if (this.metadata.Events != null)
			{
				return;
			}
			int num = this.MoveTo(Table.EventMap);
			this.metadata.Events = new Dictionary<uint, Range>(num);
			uint num2 = 1U;
			while ((ulong)num2 <= (ulong)((long)num))
			{
				uint num3 = this.ReadTableIndex(Table.TypeDef);
				Range range = this.ReadListRange(num2, Table.EventMap, Table.Event);
				this.metadata.AddEventsRange(num3, range);
				num2 += 1U;
			}
		}

		// Token: 0x0600016B RID: 363 RVA: 0x00007504 File Offset: 0x00005704
		public bool HasProperties(TypeDefinition type)
		{
			this.InitializeProperties();
			Range range;
			return this.metadata.TryGetPropertiesRange(type, out range) && range.Length > 0U;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00007534 File Offset: 0x00005734
		public Collection<PropertyDefinition> ReadProperties(TypeDefinition type)
		{
			this.InitializeProperties();
			Range range;
			if (!this.metadata.TryGetPropertiesRange(type, out range))
			{
				return new MemberDefinitionCollection<PropertyDefinition>(type);
			}
			this.metadata.RemovePropertiesRange(type);
			MemberDefinitionCollection<PropertyDefinition> memberDefinitionCollection = new MemberDefinitionCollection<PropertyDefinition>(type, (int)range.Length);
			if (range.Length == 0U)
			{
				return memberDefinitionCollection;
			}
			this.context = type;
			if (!this.MoveTo(Table.PropertyPtr, range.Start))
			{
				if (!this.MoveTo(Table.Property, range.Start))
				{
					return memberDefinitionCollection;
				}
				for (uint num = 0U; num < range.Length; num += 1U)
				{
					this.ReadProperty(range.Start + num, memberDefinitionCollection);
				}
			}
			else
			{
				this.ReadPointers<PropertyDefinition>(Table.PropertyPtr, Table.Property, range, memberDefinitionCollection, new Action<uint, Collection<PropertyDefinition>>(this.ReadProperty));
			}
			return memberDefinitionCollection;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x000075E8 File Offset: 0x000057E8
		private void ReadProperty(uint property_rid, Collection<PropertyDefinition> properties)
		{
			PropertyAttributes propertyAttributes = (PropertyAttributes)base.ReadUInt16();
			string text = this.ReadString();
			uint num = this.ReadBlobIndex();
			SignatureReader signatureReader = this.ReadSignature(num);
			byte b = signatureReader.ReadByte();
			if ((b & 8) == 0)
			{
				throw new NotSupportedException();
			}
			bool flag = (b & 32) > 0;
			signatureReader.ReadCompressedUInt32();
			PropertyDefinition propertyDefinition = new PropertyDefinition(text, propertyAttributes, signatureReader.ReadTypeSignature());
			propertyDefinition.HasThis = flag;
			propertyDefinition.token = new MetadataToken(TokenType.Property, property_rid);
			if (MetadataReader.IsDeleted(propertyDefinition))
			{
				return;
			}
			properties.Add(propertyDefinition);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000766C File Offset: 0x0000586C
		private void InitializeProperties()
		{
			if (this.metadata.Properties != null)
			{
				return;
			}
			int num = this.MoveTo(Table.PropertyMap);
			this.metadata.Properties = new Dictionary<uint, Range>(num);
			uint num2 = 1U;
			while ((ulong)num2 <= (ulong)((long)num))
			{
				uint num3 = this.ReadTableIndex(Table.TypeDef);
				Range range = this.ReadListRange(num2, Table.PropertyMap, Table.Property);
				this.metadata.AddPropertiesRange(num3, range);
				num2 += 1U;
			}
		}

		// Token: 0x0600016F RID: 367 RVA: 0x000076D0 File Offset: 0x000058D0
		private MethodSemanticsAttributes ReadMethodSemantics(MethodDefinition method)
		{
			this.InitializeMethodSemantics();
			Row<MethodSemanticsAttributes, MetadataToken> row;
			if (!this.metadata.Semantics.TryGetValue(method.token.RID, out row))
			{
				return MethodSemanticsAttributes.None;
			}
			TypeDefinition declaringType = method.DeclaringType;
			MethodSemanticsAttributes col = row.Col1;
			if (col <= MethodSemanticsAttributes.AddOn)
			{
				switch (col)
				{
				case MethodSemanticsAttributes.Setter:
					MetadataReader.GetProperty(declaringType, row.Col2).set_method = method;
					goto IL_016B;
				case MethodSemanticsAttributes.Getter:
					MetadataReader.GetProperty(declaringType, row.Col2).get_method = method;
					goto IL_016B;
				case MethodSemanticsAttributes.Setter | MethodSemanticsAttributes.Getter:
					break;
				case MethodSemanticsAttributes.Other:
				{
					TokenType tokenType = row.Col2.TokenType;
					if (tokenType == TokenType.Event)
					{
						EventDefinition @event = MetadataReader.GetEvent(declaringType, row.Col2);
						if (@event.other_methods == null)
						{
							@event.other_methods = new Collection<MethodDefinition>();
						}
						@event.other_methods.Add(method);
						goto IL_016B;
					}
					if (tokenType != TokenType.Property)
					{
						throw new NotSupportedException();
					}
					PropertyDefinition property = MetadataReader.GetProperty(declaringType, row.Col2);
					if (property.other_methods == null)
					{
						property.other_methods = new Collection<MethodDefinition>();
					}
					property.other_methods.Add(method);
					goto IL_016B;
				}
				default:
					if (col == MethodSemanticsAttributes.AddOn)
					{
						MetadataReader.GetEvent(declaringType, row.Col2).add_method = method;
						goto IL_016B;
					}
					break;
				}
			}
			else
			{
				if (col == MethodSemanticsAttributes.RemoveOn)
				{
					MetadataReader.GetEvent(declaringType, row.Col2).remove_method = method;
					goto IL_016B;
				}
				if (col == MethodSemanticsAttributes.Fire)
				{
					MetadataReader.GetEvent(declaringType, row.Col2).invoke_method = method;
					goto IL_016B;
				}
			}
			throw new NotSupportedException();
			IL_016B:
			this.metadata.Semantics.Remove(method.token.RID);
			return row.Col1;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000786A File Offset: 0x00005A6A
		private static EventDefinition GetEvent(TypeDefinition type, MetadataToken token)
		{
			if (token.TokenType != TokenType.Event)
			{
				throw new ArgumentException();
			}
			return MetadataReader.GetMember<EventDefinition>(type.Events, token);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000788C File Offset: 0x00005A8C
		private static PropertyDefinition GetProperty(TypeDefinition type, MetadataToken token)
		{
			if (token.TokenType != TokenType.Property)
			{
				throw new ArgumentException();
			}
			return MetadataReader.GetMember<PropertyDefinition>(type.Properties, token);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x000078B0 File Offset: 0x00005AB0
		private static TMember GetMember<TMember>(Collection<TMember> members, MetadataToken token) where TMember : IMemberDefinition
		{
			for (int i = 0; i < members.Count; i++)
			{
				TMember tmember = members[i];
				if (tmember.MetadataToken == token)
				{
					return tmember;
				}
			}
			throw new ArgumentException();
		}

		// Token: 0x06000173 RID: 371 RVA: 0x000078F4 File Offset: 0x00005AF4
		private void InitializeMethodSemantics()
		{
			if (this.metadata.Semantics != null)
			{
				return;
			}
			int num = this.MoveTo(Table.MethodSemantics);
			Dictionary<uint, Row<MethodSemanticsAttributes, MetadataToken>> dictionary = (this.metadata.Semantics = new Dictionary<uint, Row<MethodSemanticsAttributes, MetadataToken>>(0));
			uint num2 = 0U;
			while ((ulong)num2 < (ulong)((long)num))
			{
				MethodSemanticsAttributes methodSemanticsAttributes = (MethodSemanticsAttributes)base.ReadUInt16();
				uint num3 = this.ReadTableIndex(Table.Method);
				MetadataToken metadataToken = this.ReadMetadataToken(CodedIndex.HasSemantics);
				dictionary[num3] = new Row<MethodSemanticsAttributes, MetadataToken>(methodSemanticsAttributes, metadataToken);
				num2 += 1U;
			}
		}

		// Token: 0x06000174 RID: 372 RVA: 0x00007966 File Offset: 0x00005B66
		public void ReadMethods(PropertyDefinition property)
		{
			this.ReadAllSemantics(property.DeclaringType);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00007974 File Offset: 0x00005B74
		public void ReadMethods(EventDefinition @event)
		{
			this.ReadAllSemantics(@event.DeclaringType);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00007982 File Offset: 0x00005B82
		public void ReadAllSemantics(MethodDefinition method)
		{
			this.ReadAllSemantics(method.DeclaringType);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00007990 File Offset: 0x00005B90
		private void ReadAllSemantics(TypeDefinition type)
		{
			Collection<MethodDefinition> methods = type.Methods;
			for (int i = 0; i < methods.Count; i++)
			{
				MethodDefinition methodDefinition = methods[i];
				if (!methodDefinition.sem_attrs_ready)
				{
					methodDefinition.sem_attrs = this.ReadMethodSemantics(methodDefinition);
					methodDefinition.sem_attrs_ready = true;
				}
			}
		}

		// Token: 0x06000178 RID: 376 RVA: 0x000079E0 File Offset: 0x00005BE0
		public Collection<MethodDefinition> ReadMethods(TypeDefinition type)
		{
			Range methods_range = type.methods_range;
			if (methods_range.Length == 0U)
			{
				return new MemberDefinitionCollection<MethodDefinition>(type);
			}
			MemberDefinitionCollection<MethodDefinition> memberDefinitionCollection = new MemberDefinitionCollection<MethodDefinition>(type, (int)methods_range.Length);
			if (!this.MoveTo(Table.MethodPtr, methods_range.Start))
			{
				if (!this.MoveTo(Table.Method, methods_range.Start))
				{
					return memberDefinitionCollection;
				}
				for (uint num = 0U; num < methods_range.Length; num += 1U)
				{
					this.ReadMethod(methods_range.Start + num, memberDefinitionCollection);
				}
			}
			else
			{
				this.ReadPointers<MethodDefinition>(Table.MethodPtr, Table.Method, methods_range, memberDefinitionCollection, new Action<uint, Collection<MethodDefinition>>(this.ReadMethod));
			}
			return memberDefinitionCollection;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00007A6C File Offset: 0x00005C6C
		private void ReadPointers<TMember>(Table ptr, Table table, Range range, Collection<TMember> members, Action<uint, Collection<TMember>> reader) where TMember : IMemberDefinition
		{
			for (uint num = 0U; num < range.Length; num += 1U)
			{
				this.MoveTo(ptr, range.Start + num);
				uint num2 = this.ReadTableIndex(table);
				this.MoveTo(table, num2);
				reader(num2, members);
			}
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00007AB5 File Offset: 0x00005CB5
		private static bool IsDeleted(IMemberDefinition member)
		{
			return member.IsSpecialName && member.Name == "_Deleted";
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00007AD1 File Offset: 0x00005CD1
		private void InitializeMethods()
		{
			if (this.metadata.Methods != null)
			{
				return;
			}
			this.metadata.Methods = new MethodDefinition[this.image.GetTableLength(Table.Method)];
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00007B00 File Offset: 0x00005D00
		private void ReadMethod(uint method_rid, Collection<MethodDefinition> methods)
		{
			MethodDefinition methodDefinition = new MethodDefinition();
			methodDefinition.rva = base.ReadUInt32();
			methodDefinition.ImplAttributes = (MethodImplAttributes)base.ReadUInt16();
			methodDefinition.Attributes = (MethodAttributes)base.ReadUInt16();
			methodDefinition.Name = this.ReadString();
			methodDefinition.token = new MetadataToken(TokenType.Method, method_rid);
			if (MetadataReader.IsDeleted(methodDefinition))
			{
				return;
			}
			methods.Add(methodDefinition);
			uint num = this.ReadBlobIndex();
			Range range = this.ReadListRange(method_rid, Table.Method, Table.Param);
			this.context = methodDefinition;
			this.ReadMethodSignature(num, methodDefinition);
			this.metadata.AddMethodDefinition(methodDefinition);
			if (range.Length != 0U)
			{
				int position = this.position;
				this.ReadParameters(methodDefinition, range);
				this.position = position;
			}
			if (this.module.IsWindowsMetadata())
			{
				WindowsRuntimeProjections.Project(methodDefinition);
			}
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00007BC4 File Offset: 0x00005DC4
		private void ReadParameters(MethodDefinition method, Range param_range)
		{
			if (this.MoveTo(Table.ParamPtr, param_range.Start))
			{
				this.ReadParameterPointers(method, param_range);
				return;
			}
			if (!this.MoveTo(Table.Param, param_range.Start))
			{
				return;
			}
			for (uint num = 0U; num < param_range.Length; num += 1U)
			{
				this.ReadParameter(param_range.Start + num, method);
			}
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00007C1C File Offset: 0x00005E1C
		private void ReadParameterPointers(MethodDefinition method, Range range)
		{
			for (uint num = 0U; num < range.Length; num += 1U)
			{
				this.MoveTo(Table.ParamPtr, range.Start + num);
				uint num2 = this.ReadTableIndex(Table.Param);
				this.MoveTo(Table.Param, num2);
				this.ReadParameter(num2, method);
			}
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00007C64 File Offset: 0x00005E64
		private void ReadParameter(uint param_rid, MethodDefinition method)
		{
			ParameterAttributes parameterAttributes = (ParameterAttributes)base.ReadUInt16();
			ushort num = base.ReadUInt16();
			string text = this.ReadString();
			ParameterDefinition parameterDefinition = ((num == 0) ? method.MethodReturnType.Parameter : method.Parameters[(int)(num - 1)]);
			parameterDefinition.token = new MetadataToken(TokenType.Param, param_rid);
			parameterDefinition.Name = text;
			parameterDefinition.Attributes = parameterAttributes;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00007CC2 File Offset: 0x00005EC2
		private void ReadMethodSignature(uint signature, IMethodSignature method)
		{
			this.ReadSignature(signature).ReadMethodSignature(method);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00007CD4 File Offset: 0x00005ED4
		public PInvokeInfo ReadPInvokeInfo(MethodDefinition method)
		{
			this.InitializePInvokes();
			uint rid = method.token.RID;
			Row<PInvokeAttributes, uint, uint> row;
			if (!this.metadata.PInvokes.TryGetValue(rid, out row))
			{
				return null;
			}
			this.metadata.PInvokes.Remove(rid);
			return new PInvokeInfo(row.Col1, this.image.StringHeap.Read(row.Col2), this.module.ModuleReferences[(int)(row.Col3 - 1U)]);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00007D58 File Offset: 0x00005F58
		private void InitializePInvokes()
		{
			if (this.metadata.PInvokes != null)
			{
				return;
			}
			int num = this.MoveTo(Table.ImplMap);
			Dictionary<uint, Row<PInvokeAttributes, uint, uint>> dictionary = (this.metadata.PInvokes = new Dictionary<uint, Row<PInvokeAttributes, uint, uint>>(num));
			for (int i = 1; i <= num; i++)
			{
				PInvokeAttributes pinvokeAttributes = (PInvokeAttributes)base.ReadUInt16();
				MetadataToken metadataToken = this.ReadMetadataToken(CodedIndex.MemberForwarded);
				uint num2 = this.ReadStringIndex();
				uint num3 = this.ReadTableIndex(Table.File);
				if (metadataToken.TokenType == TokenType.Method)
				{
					dictionary.Add(metadataToken.RID, new Row<PInvokeAttributes, uint, uint>(pinvokeAttributes, num2, num3));
				}
			}
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00007DE8 File Offset: 0x00005FE8
		public bool HasGenericParameters(IGenericParameterProvider provider)
		{
			this.InitializeGenericParameters();
			Range[] array;
			return this.metadata.TryGetGenericParameterRanges(provider, out array) && MetadataReader.RangesSize(array) > 0;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00007E18 File Offset: 0x00006018
		public Collection<GenericParameter> ReadGenericParameters(IGenericParameterProvider provider)
		{
			this.InitializeGenericParameters();
			Range[] array;
			if (!this.metadata.TryGetGenericParameterRanges(provider, out array))
			{
				return new GenericParameterCollection(provider);
			}
			this.metadata.RemoveGenericParameterRange(provider);
			GenericParameterCollection genericParameterCollection = new GenericParameterCollection(provider, MetadataReader.RangesSize(array));
			for (int i = 0; i < array.Length; i++)
			{
				this.ReadGenericParametersRange(array[i], provider, genericParameterCollection);
			}
			return genericParameterCollection;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00007E7C File Offset: 0x0000607C
		private void ReadGenericParametersRange(Range range, IGenericParameterProvider provider, GenericParameterCollection generic_parameters)
		{
			if (!this.MoveTo(Table.GenericParam, range.Start))
			{
				return;
			}
			for (uint num = 0U; num < range.Length; num += 1U)
			{
				base.ReadUInt16();
				GenericParameterAttributes genericParameterAttributes = (GenericParameterAttributes)base.ReadUInt16();
				this.ReadMetadataToken(CodedIndex.TypeOrMethodDef);
				generic_parameters.Add(new GenericParameter(this.ReadString(), provider)
				{
					token = new MetadataToken(TokenType.GenericParam, range.Start + num),
					Attributes = genericParameterAttributes
				});
			}
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00007EF5 File Offset: 0x000060F5
		private void InitializeGenericParameters()
		{
			if (this.metadata.GenericParameters != null)
			{
				return;
			}
			this.metadata.GenericParameters = this.InitializeRanges(Table.GenericParam, delegate
			{
				base.Advance(4);
				MetadataToken metadataToken = this.ReadMetadataToken(CodedIndex.TypeOrMethodDef);
				this.ReadStringIndex();
				return metadataToken;
			});
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00007F24 File Offset: 0x00006124
		private Dictionary<MetadataToken, Range[]> InitializeRanges(Table table, Func<MetadataToken> get_next)
		{
			int num = this.MoveTo(table);
			Dictionary<MetadataToken, Range[]> dictionary = new Dictionary<MetadataToken, Range[]>(num);
			if (num == 0)
			{
				return dictionary;
			}
			MetadataToken metadataToken = MetadataToken.Zero;
			Range range = new Range(1U, 0U);
			uint num2 = 1U;
			while ((ulong)num2 <= (ulong)((long)num))
			{
				MetadataToken metadataToken2 = get_next();
				if (num2 == 1U)
				{
					metadataToken = metadataToken2;
					range.Length += 1U;
				}
				else if (metadataToken2 != metadataToken)
				{
					MetadataReader.AddRange(dictionary, metadataToken, range);
					range = new Range(num2, 1U);
					metadataToken = metadataToken2;
				}
				else
				{
					range.Length += 1U;
				}
				num2 += 1U;
			}
			MetadataReader.AddRange(dictionary, metadataToken, range);
			return dictionary;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00007FBC File Offset: 0x000061BC
		private static void AddRange(Dictionary<MetadataToken, Range[]> ranges, MetadataToken owner, Range range)
		{
			if (owner.RID == 0U)
			{
				return;
			}
			Range[] array;
			if (!ranges.TryGetValue(owner, out array))
			{
				ranges.Add(owner, new Range[] { range });
				return;
			}
			ranges[owner] = array.Add(range);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00008004 File Offset: 0x00006204
		public bool HasGenericConstraints(GenericParameter generic_parameter)
		{
			this.InitializeGenericConstraints();
			Collection<MetadataToken> collection;
			return this.metadata.TryGetGenericConstraintMapping(generic_parameter, out collection) && collection.Count > 0;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00008034 File Offset: 0x00006234
		public Collection<TypeReference> ReadGenericConstraints(GenericParameter generic_parameter)
		{
			this.InitializeGenericConstraints();
			Collection<MetadataToken> collection;
			if (!this.metadata.TryGetGenericConstraintMapping(generic_parameter, out collection))
			{
				return new Collection<TypeReference>();
			}
			Collection<TypeReference> collection2 = new Collection<TypeReference>(collection.Count);
			this.context = (IGenericContext)generic_parameter.Owner;
			for (int i = 0; i < collection.Count; i++)
			{
				collection2.Add(this.GetTypeDefOrRef(collection[i]));
			}
			this.metadata.RemoveGenericConstraintMapping(generic_parameter);
			return collection2;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x000080AC File Offset: 0x000062AC
		private void InitializeGenericConstraints()
		{
			if (this.metadata.GenericConstraints != null)
			{
				return;
			}
			int num = this.MoveTo(Table.GenericParamConstraint);
			this.metadata.GenericConstraints = new Dictionary<uint, Collection<MetadataToken>>(num);
			for (int i = 1; i <= num; i++)
			{
				this.AddGenericConstraintMapping(this.ReadTableIndex(Table.GenericParam), this.ReadMetadataToken(CodedIndex.TypeDefOrRef));
			}
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00008102 File Offset: 0x00006302
		private void AddGenericConstraintMapping(uint generic_parameter, MetadataToken constraint)
		{
			this.metadata.SetGenericConstraintMapping(generic_parameter, MetadataReader.AddMapping<uint, MetadataToken>(this.metadata.GenericConstraints, generic_parameter, constraint));
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00008124 File Offset: 0x00006324
		public bool HasOverrides(MethodDefinition method)
		{
			this.InitializeOverrides();
			Collection<MetadataToken> collection;
			return this.metadata.TryGetOverrideMapping(method, out collection) && collection.Count > 0;
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00008154 File Offset: 0x00006354
		public Collection<MethodReference> ReadOverrides(MethodDefinition method)
		{
			this.InitializeOverrides();
			Collection<MetadataToken> collection;
			if (!this.metadata.TryGetOverrideMapping(method, out collection))
			{
				return new Collection<MethodReference>();
			}
			Collection<MethodReference> collection2 = new Collection<MethodReference>(collection.Count);
			this.context = method;
			for (int i = 0; i < collection.Count; i++)
			{
				collection2.Add((MethodReference)this.LookupToken(collection[i]));
			}
			this.metadata.RemoveOverrideMapping(method);
			return collection2;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x000081C8 File Offset: 0x000063C8
		private void InitializeOverrides()
		{
			if (this.metadata.Overrides != null)
			{
				return;
			}
			int num = this.MoveTo(Table.MethodImpl);
			this.metadata.Overrides = new Dictionary<uint, Collection<MetadataToken>>(num);
			for (int i = 1; i <= num; i++)
			{
				this.ReadTableIndex(Table.TypeDef);
				MetadataToken metadataToken = this.ReadMetadataToken(CodedIndex.MethodDefOrRef);
				if (metadataToken.TokenType != TokenType.Method)
				{
					throw new NotSupportedException();
				}
				MetadataToken metadataToken2 = this.ReadMetadataToken(CodedIndex.MethodDefOrRef);
				this.AddOverrideMapping(metadataToken.RID, metadataToken2);
			}
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00008243 File Offset: 0x00006443
		private void AddOverrideMapping(uint method_rid, MetadataToken @override)
		{
			this.metadata.SetOverrideMapping(method_rid, MetadataReader.AddMapping<uint, MetadataToken>(this.metadata.Overrides, method_rid, @override));
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00008263 File Offset: 0x00006463
		public MethodBody ReadMethodBody(MethodDefinition method)
		{
			return this.code.ReadMethodBody(method);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00008271 File Offset: 0x00006471
		public int ReadCodeSize(MethodDefinition method)
		{
			return this.code.ReadCodeSize(method);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00008280 File Offset: 0x00006480
		public CallSite ReadCallSite(MetadataToken token)
		{
			if (!this.MoveTo(Table.StandAloneSig, token.RID))
			{
				return null;
			}
			uint num = this.ReadBlobIndex();
			CallSite callSite = new CallSite();
			this.ReadMethodSignature(num, callSite);
			callSite.MetadataToken = token;
			return callSite;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x000082C0 File Offset: 0x000064C0
		public VariableDefinitionCollection ReadVariables(MetadataToken local_var_token)
		{
			if (!this.MoveTo(Table.StandAloneSig, local_var_token.RID))
			{
				return null;
			}
			SignatureReader signatureReader = this.ReadSignature(this.ReadBlobIndex());
			if (signatureReader.ReadByte() != 7)
			{
				throw new NotSupportedException();
			}
			uint num = signatureReader.ReadCompressedUInt32();
			if (num == 0U)
			{
				return null;
			}
			VariableDefinitionCollection variableDefinitionCollection = new VariableDefinitionCollection((int)num);
			int num2 = 0;
			while ((long)num2 < (long)((ulong)num))
			{
				variableDefinitionCollection.Add(new VariableDefinition(signatureReader.ReadTypeSignature()));
				num2++;
			}
			return variableDefinitionCollection;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00008330 File Offset: 0x00006530
		public IMetadataTokenProvider LookupToken(MetadataToken token)
		{
			uint rid = token.RID;
			if (rid == 0U)
			{
				return null;
			}
			if (this.metadata_reader != null)
			{
				return this.metadata_reader.LookupToken(token);
			}
			int position = this.position;
			IGenericContext genericContext = this.context;
			TokenType tokenType = token.TokenType;
			IMetadataTokenProvider metadataTokenProvider;
			if (tokenType <= TokenType.Field)
			{
				if (tokenType == TokenType.TypeRef)
				{
					metadataTokenProvider = this.GetTypeReference(rid);
					goto IL_00D8;
				}
				if (tokenType == TokenType.TypeDef)
				{
					metadataTokenProvider = this.GetTypeDefinition(rid);
					goto IL_00D8;
				}
				if (tokenType == TokenType.Field)
				{
					metadataTokenProvider = this.GetFieldDefinition(rid);
					goto IL_00D8;
				}
			}
			else if (tokenType <= TokenType.MemberRef)
			{
				if (tokenType == TokenType.Method)
				{
					metadataTokenProvider = this.GetMethodDefinition(rid);
					goto IL_00D8;
				}
				if (tokenType == TokenType.MemberRef)
				{
					metadataTokenProvider = this.GetMemberReference(rid);
					goto IL_00D8;
				}
			}
			else
			{
				if (tokenType == TokenType.TypeSpec)
				{
					metadataTokenProvider = this.GetTypeSpecification(rid);
					goto IL_00D8;
				}
				if (tokenType == TokenType.MethodSpec)
				{
					metadataTokenProvider = this.GetMethodSpecification(rid);
					goto IL_00D8;
				}
			}
			return null;
			IL_00D8:
			this.position = position;
			this.context = genericContext;
			return metadataTokenProvider;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00008424 File Offset: 0x00006624
		public FieldDefinition GetFieldDefinition(uint rid)
		{
			this.InitializeTypeDefinitions();
			FieldDefinition fieldDefinition = this.metadata.GetFieldDefinition(rid);
			if (fieldDefinition != null)
			{
				return fieldDefinition;
			}
			return this.LookupField(rid);
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00008450 File Offset: 0x00006650
		private FieldDefinition LookupField(uint rid)
		{
			TypeDefinition fieldDeclaringType = this.metadata.GetFieldDeclaringType(rid);
			if (fieldDeclaringType == null)
			{
				return null;
			}
			Mixin.Read(fieldDeclaringType.Fields);
			return this.metadata.GetFieldDefinition(rid);
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00008488 File Offset: 0x00006688
		public MethodDefinition GetMethodDefinition(uint rid)
		{
			this.InitializeTypeDefinitions();
			MethodDefinition methodDefinition = this.metadata.GetMethodDefinition(rid);
			if (methodDefinition != null)
			{
				return methodDefinition;
			}
			return this.LookupMethod(rid);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x000084B4 File Offset: 0x000066B4
		private MethodDefinition LookupMethod(uint rid)
		{
			TypeDefinition methodDeclaringType = this.metadata.GetMethodDeclaringType(rid);
			if (methodDeclaringType == null)
			{
				return null;
			}
			Mixin.Read(methodDeclaringType.Methods);
			return this.metadata.GetMethodDefinition(rid);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x000084EC File Offset: 0x000066EC
		private MethodSpecification GetMethodSpecification(uint rid)
		{
			if (!this.MoveTo(Table.MethodSpec, rid))
			{
				return null;
			}
			MethodReference methodReference = (MethodReference)this.LookupToken(this.ReadMetadataToken(CodedIndex.MethodDefOrRef));
			uint num = this.ReadBlobIndex();
			MethodSpecification methodSpecification = this.ReadMethodSpecSignature(num, methodReference);
			methodSpecification.token = new MetadataToken(TokenType.MethodSpec, rid);
			return methodSpecification;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000853C File Offset: 0x0000673C
		private MethodSpecification ReadMethodSpecSignature(uint signature, MethodReference method)
		{
			SignatureReader signatureReader = this.ReadSignature(signature);
			if (signatureReader.ReadByte() != 10)
			{
				throw new NotSupportedException();
			}
			GenericInstanceMethod genericInstanceMethod = new GenericInstanceMethod(method);
			signatureReader.ReadGenericInstanceSignature(method, genericInstanceMethod);
			return genericInstanceMethod;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00008570 File Offset: 0x00006770
		private MemberReference GetMemberReference(uint rid)
		{
			this.InitializeMemberReferences();
			MemberReference memberReference = this.metadata.GetMemberReference(rid);
			if (memberReference != null)
			{
				return memberReference;
			}
			memberReference = this.ReadMemberReference(rid);
			if (memberReference != null && !memberReference.ContainsGenericParameter)
			{
				this.metadata.AddMemberReference(memberReference);
			}
			return memberReference;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x000085B8 File Offset: 0x000067B8
		private MemberReference ReadMemberReference(uint rid)
		{
			if (!this.MoveTo(Table.MemberRef, rid))
			{
				return null;
			}
			MetadataToken metadataToken = this.ReadMetadataToken(CodedIndex.MemberRefParent);
			string text = this.ReadString();
			uint num = this.ReadBlobIndex();
			TokenType tokenType = metadataToken.TokenType;
			MemberReference memberReference;
			if (tokenType <= TokenType.TypeDef)
			{
				if (tokenType != TokenType.TypeRef && tokenType != TokenType.TypeDef)
				{
					goto IL_0073;
				}
			}
			else
			{
				if (tokenType == TokenType.Method)
				{
					memberReference = this.ReadMethodMemberReference(metadataToken, text, num);
					goto IL_0079;
				}
				if (tokenType != TokenType.TypeSpec)
				{
					goto IL_0073;
				}
			}
			memberReference = this.ReadTypeMemberReference(metadataToken, text, num);
			goto IL_0079;
			IL_0073:
			throw new NotSupportedException();
			IL_0079:
			memberReference.token = new MetadataToken(TokenType.MemberRef, rid);
			if (this.module.IsWindowsMetadata())
			{
				WindowsRuntimeProjections.Project(memberReference);
			}
			return memberReference;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00008664 File Offset: 0x00006864
		private MemberReference ReadTypeMemberReference(MetadataToken type, string name, uint signature)
		{
			TypeReference typeDefOrRef = this.GetTypeDefOrRef(type);
			if (!typeDefOrRef.IsArray)
			{
				this.context = typeDefOrRef;
			}
			MemberReference memberReference = this.ReadMemberReferenceSignature(signature, typeDefOrRef);
			memberReference.Name = name;
			return memberReference;
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00008698 File Offset: 0x00006898
		private MemberReference ReadMemberReferenceSignature(uint signature, TypeReference declaring_type)
		{
			SignatureReader signatureReader = this.ReadSignature(signature);
			if (signatureReader.buffer[signatureReader.position] == 6)
			{
				signatureReader.position++;
				return new FieldReference
				{
					DeclaringType = declaring_type,
					FieldType = signatureReader.ReadTypeSignature()
				};
			}
			MethodReference methodReference = new MethodReference();
			methodReference.DeclaringType = declaring_type;
			signatureReader.ReadMethodSignature(methodReference);
			return methodReference;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x000086FC File Offset: 0x000068FC
		private MemberReference ReadMethodMemberReference(MetadataToken token, string name, uint signature)
		{
			MethodDefinition methodDefinition = this.GetMethodDefinition(token.RID);
			this.context = methodDefinition;
			MemberReference memberReference = this.ReadMemberReferenceSignature(signature, methodDefinition.DeclaringType);
			memberReference.Name = name;
			return memberReference;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00008732 File Offset: 0x00006932
		private void InitializeMemberReferences()
		{
			if (this.metadata.MemberReferences != null)
			{
				return;
			}
			this.metadata.MemberReferences = new MemberReference[this.image.GetTableLength(Table.MemberRef)];
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00008760 File Offset: 0x00006960
		public IEnumerable<MemberReference> GetMemberReferences()
		{
			this.InitializeMemberReferences();
			int tableLength = this.image.GetTableLength(Table.MemberRef);
			TypeSystem typeSystem = this.module.TypeSystem;
			MethodDefinition methodDefinition = new MethodDefinition(string.Empty, MethodAttributes.Static, typeSystem.Void);
			methodDefinition.DeclaringType = new TypeDefinition(string.Empty, string.Empty, TypeAttributes.Public);
			MemberReference[] array = new MemberReference[tableLength];
			uint num = 1U;
			while ((ulong)num <= (ulong)((long)tableLength))
			{
				this.context = methodDefinition;
				array[(int)(num - 1U)] = this.GetMemberReference(num);
				num += 1U;
			}
			return array;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x000087E8 File Offset: 0x000069E8
		private void InitializeConstants()
		{
			if (this.metadata.Constants != null)
			{
				return;
			}
			int num = this.MoveTo(Table.Constant);
			Dictionary<MetadataToken, Row<ElementType, uint>> dictionary = (this.metadata.Constants = new Dictionary<MetadataToken, Row<ElementType, uint>>(num));
			uint num2 = 1U;
			while ((ulong)num2 <= (ulong)((long)num))
			{
				ElementType elementType = (ElementType)base.ReadUInt16();
				MetadataToken metadataToken = this.ReadMetadataToken(CodedIndex.HasConstant);
				uint num3 = this.ReadBlobIndex();
				dictionary.Add(metadataToken, new Row<ElementType, uint>(elementType, num3));
				num2 += 1U;
			}
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000885A File Offset: 0x00006A5A
		public TypeReference ReadConstantSignature(MetadataToken token)
		{
			if (token.TokenType != TokenType.Signature)
			{
				throw new NotSupportedException();
			}
			if (token.RID == 0U)
			{
				return null;
			}
			if (!this.MoveTo(Table.StandAloneSig, token.RID))
			{
				return null;
			}
			return this.ReadFieldType(this.ReadBlobIndex());
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000889C File Offset: 0x00006A9C
		public object ReadConstant(IConstantProvider owner)
		{
			this.InitializeConstants();
			Row<ElementType, uint> row;
			if (!this.metadata.Constants.TryGetValue(owner.MetadataToken, out row))
			{
				return Mixin.NoValue;
			}
			this.metadata.Constants.Remove(owner.MetadataToken);
			return this.ReadConstantValue(row.Col1, row.Col2);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x000088F8 File Offset: 0x00006AF8
		private object ReadConstantValue(ElementType etype, uint signature)
		{
			if (etype == ElementType.String)
			{
				return this.ReadConstantString(signature);
			}
			if (etype == ElementType.Class || etype == ElementType.Object)
			{
				return null;
			}
			return this.ReadConstantPrimitive(etype, signature);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000891C File Offset: 0x00006B1C
		private string ReadConstantString(uint signature)
		{
			byte[] array;
			int num;
			int num2;
			this.GetBlobView(signature, out array, out num, out num2);
			if (num2 == 0)
			{
				return string.Empty;
			}
			if ((num2 & 1) == 1)
			{
				num2--;
			}
			return Encoding.Unicode.GetString(array, num, num2);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00008956 File Offset: 0x00006B56
		private object ReadConstantPrimitive(ElementType type, uint signature)
		{
			return this.ReadSignature(signature).ReadConstantSignature(type);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00008965 File Offset: 0x00006B65
		internal void InitializeCustomAttributes()
		{
			if (this.metadata.CustomAttributes != null)
			{
				return;
			}
			this.metadata.CustomAttributes = this.InitializeRanges(Table.CustomAttribute, delegate
			{
				MetadataToken metadataToken = this.ReadMetadataToken(CodedIndex.HasCustomAttribute);
				this.ReadMetadataToken(CodedIndex.CustomAttributeType);
				this.ReadBlobIndex();
				return metadataToken;
			});
		}

		// Token: 0x060001AA RID: 426 RVA: 0x00008994 File Offset: 0x00006B94
		public bool HasCustomAttributes(ICustomAttributeProvider owner)
		{
			this.InitializeCustomAttributes();
			Range[] array;
			return this.metadata.TryGetCustomAttributeRanges(owner, out array) && MetadataReader.RangesSize(array) > 0;
		}

		// Token: 0x060001AB RID: 427 RVA: 0x000089C4 File Offset: 0x00006BC4
		public Collection<CustomAttribute> ReadCustomAttributes(ICustomAttributeProvider owner)
		{
			this.InitializeCustomAttributes();
			Range[] array;
			if (!this.metadata.TryGetCustomAttributeRanges(owner, out array))
			{
				return new Collection<CustomAttribute>();
			}
			Collection<CustomAttribute> collection = new Collection<CustomAttribute>(MetadataReader.RangesSize(array));
			for (int i = 0; i < array.Length; i++)
			{
				this.ReadCustomAttributeRange(array[i], collection);
			}
			this.metadata.RemoveCustomAttributeRange(owner);
			if (this.module.IsWindowsMetadata())
			{
				foreach (CustomAttribute customAttribute in collection)
				{
					WindowsRuntimeProjections.Project(owner, customAttribute);
				}
			}
			return collection;
		}

		// Token: 0x060001AC RID: 428 RVA: 0x00008A74 File Offset: 0x00006C74
		private void ReadCustomAttributeRange(Range range, Collection<CustomAttribute> custom_attributes)
		{
			if (!this.MoveTo(Table.CustomAttribute, range.Start))
			{
				return;
			}
			int num = 0;
			while ((long)num < (long)((ulong)range.Length))
			{
				this.ReadMetadataToken(CodedIndex.HasCustomAttribute);
				MethodReference methodReference = (MethodReference)this.LookupToken(this.ReadMetadataToken(CodedIndex.CustomAttributeType));
				uint num2 = this.ReadBlobIndex();
				custom_attributes.Add(new CustomAttribute(num2, methodReference));
				num++;
			}
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00008AD8 File Offset: 0x00006CD8
		private static int RangesSize(Range[] ranges)
		{
			uint num = 0U;
			for (int i = 0; i < ranges.Length; i++)
			{
				num += ranges[i].Length;
			}
			return (int)num;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00008B08 File Offset: 0x00006D08
		public IEnumerable<CustomAttribute> GetCustomAttributes()
		{
			this.InitializeTypeDefinitions();
			uint length = this.image.TableHeap[Table.CustomAttribute].Length;
			Collection<CustomAttribute> collection = new Collection<CustomAttribute>((int)length);
			this.ReadCustomAttributeRange(new Range(1U, length), collection);
			return collection;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00008B49 File Offset: 0x00006D49
		public byte[] ReadCustomAttributeBlob(uint signature)
		{
			return this.ReadBlob(signature);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00008B54 File Offset: 0x00006D54
		public void ReadCustomAttributeSignature(CustomAttribute attribute)
		{
			SignatureReader signatureReader = this.ReadSignature(attribute.signature);
			if (!signatureReader.CanReadMore())
			{
				return;
			}
			if (signatureReader.ReadUInt16() != 1)
			{
				throw new InvalidOperationException();
			}
			MethodReference constructor = attribute.Constructor;
			if (constructor.HasParameters)
			{
				signatureReader.ReadCustomAttributeConstructorArguments(attribute, constructor.Parameters);
			}
			if (!signatureReader.CanReadMore())
			{
				return;
			}
			ushort num = signatureReader.ReadUInt16();
			if (num == 0)
			{
				return;
			}
			signatureReader.ReadCustomAttributeNamedArguments(num, ref attribute.fields, ref attribute.properties);
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00008BCC File Offset: 0x00006DCC
		private void InitializeMarshalInfos()
		{
			if (this.metadata.FieldMarshals != null)
			{
				return;
			}
			int num = this.MoveTo(Table.FieldMarshal);
			Dictionary<MetadataToken, uint> dictionary = (this.metadata.FieldMarshals = new Dictionary<MetadataToken, uint>(num));
			for (int i = 0; i < num; i++)
			{
				MetadataToken metadataToken = this.ReadMetadataToken(CodedIndex.HasFieldMarshal);
				uint num2 = this.ReadBlobIndex();
				if (metadataToken.RID != 0U)
				{
					dictionary.Add(metadataToken, num2);
				}
			}
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x00008C35 File Offset: 0x00006E35
		public bool HasMarshalInfo(IMarshalInfoProvider owner)
		{
			this.InitializeMarshalInfos();
			return this.metadata.FieldMarshals.ContainsKey(owner.MetadataToken);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00008C54 File Offset: 0x00006E54
		public MarshalInfo ReadMarshalInfo(IMarshalInfoProvider owner)
		{
			this.InitializeMarshalInfos();
			uint num;
			if (!this.metadata.FieldMarshals.TryGetValue(owner.MetadataToken, out num))
			{
				return null;
			}
			SignatureReader signatureReader = this.ReadSignature(num);
			this.metadata.FieldMarshals.Remove(owner.MetadataToken);
			return signatureReader.ReadMarshalInfo();
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00008CA6 File Offset: 0x00006EA6
		private void InitializeSecurityDeclarations()
		{
			if (this.metadata.SecurityDeclarations != null)
			{
				return;
			}
			this.metadata.SecurityDeclarations = this.InitializeRanges(Table.DeclSecurity, delegate
			{
				base.ReadUInt16();
				MetadataToken metadataToken = this.ReadMetadataToken(CodedIndex.HasDeclSecurity);
				this.ReadBlobIndex();
				return metadataToken;
			});
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x00008CD8 File Offset: 0x00006ED8
		public bool HasSecurityDeclarations(ISecurityDeclarationProvider owner)
		{
			this.InitializeSecurityDeclarations();
			Range[] array;
			return this.metadata.TryGetSecurityDeclarationRanges(owner, out array) && MetadataReader.RangesSize(array) > 0;
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00008D08 File Offset: 0x00006F08
		public Collection<SecurityDeclaration> ReadSecurityDeclarations(ISecurityDeclarationProvider owner)
		{
			this.InitializeSecurityDeclarations();
			Range[] array;
			if (!this.metadata.TryGetSecurityDeclarationRanges(owner, out array))
			{
				return new Collection<SecurityDeclaration>();
			}
			Collection<SecurityDeclaration> collection = new Collection<SecurityDeclaration>(MetadataReader.RangesSize(array));
			for (int i = 0; i < array.Length; i++)
			{
				this.ReadSecurityDeclarationRange(array[i], collection);
			}
			this.metadata.RemoveSecurityDeclarationRange(owner);
			return collection;
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00008D68 File Offset: 0x00006F68
		private void ReadSecurityDeclarationRange(Range range, Collection<SecurityDeclaration> security_declarations)
		{
			if (!this.MoveTo(Table.DeclSecurity, range.Start))
			{
				return;
			}
			int num = 0;
			while ((long)num < (long)((ulong)range.Length))
			{
				SecurityAction securityAction = (SecurityAction)base.ReadUInt16();
				this.ReadMetadataToken(CodedIndex.HasDeclSecurity);
				uint num2 = this.ReadBlobIndex();
				security_declarations.Add(new SecurityDeclaration(securityAction, num2, this.module));
				num++;
			}
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00008B49 File Offset: 0x00006D49
		public byte[] ReadSecurityDeclarationBlob(uint signature)
		{
			return this.ReadBlob(signature);
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00008DC4 File Offset: 0x00006FC4
		public void ReadSecurityDeclarationSignature(SecurityDeclaration declaration)
		{
			uint signature = declaration.signature;
			SignatureReader signatureReader = this.ReadSignature(signature);
			if (signatureReader.buffer[signatureReader.position] != 46)
			{
				this.ReadXmlSecurityDeclaration(signature, declaration);
				return;
			}
			signatureReader.position++;
			uint num = signatureReader.ReadCompressedUInt32();
			Collection<SecurityAttribute> collection = new Collection<SecurityAttribute>((int)num);
			int num2 = 0;
			while ((long)num2 < (long)((ulong)num))
			{
				collection.Add(signatureReader.ReadSecurityAttribute());
				num2++;
			}
			declaration.security_attributes = collection;
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00008E3C File Offset: 0x0000703C
		private void ReadXmlSecurityDeclaration(uint signature, SecurityDeclaration declaration)
		{
			declaration.security_attributes = new Collection<SecurityAttribute>(1)
			{
				new SecurityAttribute(this.module.TypeSystem.LookupType("System.Security.Permissions", "PermissionSetAttribute"))
				{
					properties = new Collection<CustomAttributeNamedArgument>(1),
					properties = 
					{
						new CustomAttributeNamedArgument("XML", new CustomAttributeArgument(this.module.TypeSystem.String, this.ReadUnicodeStringBlob(signature)))
					}
				}
			};
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00008EBC File Offset: 0x000070BC
		public Collection<ExportedType> ReadExportedTypes()
		{
			int num = this.MoveTo(Table.ExportedType);
			if (num == 0)
			{
				return new Collection<ExportedType>();
			}
			Collection<ExportedType> collection = new Collection<ExportedType>(num);
			for (int i = 1; i <= num; i++)
			{
				TypeAttributes typeAttributes = (TypeAttributes)base.ReadUInt32();
				uint num2 = base.ReadUInt32();
				string text = this.ReadString();
				string text2 = this.ReadString();
				MetadataToken metadataToken = this.ReadMetadataToken(CodedIndex.Implementation);
				ExportedType exportedType = null;
				IMetadataScope metadataScope = null;
				TokenType tokenType = metadataToken.TokenType;
				if (tokenType != TokenType.AssemblyRef && tokenType != TokenType.File)
				{
					if (tokenType == TokenType.ExportedType)
					{
						exportedType = collection[(int)(metadataToken.RID - 1U)];
					}
				}
				else
				{
					metadataScope = this.GetExportedTypeScope(metadataToken);
				}
				ExportedType exportedType2 = new ExportedType(text2, text, this.module, metadataScope)
				{
					Attributes = typeAttributes,
					Identifier = (int)num2,
					DeclaringType = exportedType
				};
				exportedType2.token = new MetadataToken(TokenType.ExportedType, i);
				collection.Add(exportedType2);
			}
			return collection;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00008FAC File Offset: 0x000071AC
		private IMetadataScope GetExportedTypeScope(MetadataToken token)
		{
			int position = this.position;
			TokenType tokenType = token.TokenType;
			IMetadataScope metadataScope;
			if (tokenType != TokenType.AssemblyRef)
			{
				if (tokenType != TokenType.File)
				{
					throw new NotSupportedException();
				}
				this.InitializeModuleReferences();
				metadataScope = this.GetModuleReferenceFromFile(token);
			}
			else
			{
				this.InitializeAssemblyReferences();
				metadataScope = this.metadata.GetAssemblyNameReference(token.RID);
			}
			this.position = position;
			return metadataScope;
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00009014 File Offset: 0x00007214
		private ModuleReference GetModuleReferenceFromFile(MetadataToken token)
		{
			if (!this.MoveTo(Table.File, token.RID))
			{
				return null;
			}
			base.ReadUInt32();
			string text = this.ReadString();
			Collection<ModuleReference> moduleReferences = this.module.ModuleReferences;
			ModuleReference moduleReference;
			for (int i = 0; i < moduleReferences.Count; i++)
			{
				moduleReference = moduleReferences[i];
				if (moduleReference.Name == text)
				{
					return moduleReference;
				}
			}
			moduleReference = new ModuleReference(text);
			moduleReferences.Add(moduleReference);
			return moduleReference;
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00009088 File Offset: 0x00007288
		private void InitializeDocuments()
		{
			if (this.metadata.Documents != null)
			{
				return;
			}
			int num = this.MoveTo(Table.Document);
			Document[] array = (this.metadata.Documents = new Document[num]);
			uint num2 = 1U;
			while ((ulong)num2 <= (ulong)((long)num))
			{
				uint num3 = this.ReadBlobIndex();
				Guid guid = this.ReadGuid();
				byte[] array2 = this.ReadBlob();
				Guid guid2 = this.ReadGuid();
				string text = this.ReadSignature(num3).ReadDocumentName();
				array[(int)(num2 - 1U)] = new Document(text)
				{
					HashAlgorithmGuid = guid,
					Hash = array2,
					LanguageGuid = guid2,
					token = new MetadataToken(TokenType.Document, num2)
				};
				num2 += 1U;
			}
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00009134 File Offset: 0x00007334
		public Collection<SequencePoint> ReadSequencePoints(MethodDefinition method)
		{
			this.InitializeDocuments();
			if (!this.MoveTo(Table.MethodDebugInformation, method.MetadataToken.RID))
			{
				return new Collection<SequencePoint>(0);
			}
			uint num = this.ReadTableIndex(Table.Document);
			uint num2 = this.ReadBlobIndex();
			if (num2 == 0U)
			{
				return new Collection<SequencePoint>(0);
			}
			Document document = this.GetDocument(num);
			return this.ReadSignature(num2).ReadSequencePoints(document);
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00009198 File Offset: 0x00007398
		public Document GetDocument(uint rid)
		{
			Document document = this.metadata.GetDocument(rid);
			if (document == null)
			{
				return null;
			}
			document.custom_infos = this.GetCustomDebugInformation(document);
			return document;
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x000091C8 File Offset: 0x000073C8
		private void InitializeLocalScopes()
		{
			if (this.metadata.LocalScopes != null)
			{
				return;
			}
			this.InitializeMethods();
			int num = this.MoveTo(Table.LocalScope);
			this.metadata.LocalScopes = new Dictionary<uint, Collection<Row<uint, Range, Range, uint, uint, uint>>>();
			uint num2 = 1U;
			while ((ulong)num2 <= (ulong)((long)num))
			{
				uint num3 = this.ReadTableIndex(Table.Method);
				uint num4 = this.ReadTableIndex(Table.ImportScope);
				Range range = this.ReadListRange(num2, Table.LocalScope, Table.LocalVariable);
				Range range2 = this.ReadListRange(num2, Table.LocalScope, Table.LocalConstant);
				uint num5 = base.ReadUInt32();
				uint num6 = base.ReadUInt32();
				this.metadata.SetLocalScopes(num3, MetadataReader.AddMapping<uint, Row<uint, Range, Range, uint, uint, uint>>(this.metadata.LocalScopes, num3, new Row<uint, Range, Range, uint, uint, uint>(num4, range, range2, num5, num6, num2)));
				num2 += 1U;
			}
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00009278 File Offset: 0x00007478
		public ScopeDebugInformation ReadScope(MethodDefinition method)
		{
			this.InitializeLocalScopes();
			this.InitializeImportScopes();
			Collection<Row<uint, Range, Range, uint, uint, uint>> collection;
			if (!this.metadata.TryGetLocalScopes(method, out collection))
			{
				return null;
			}
			ScopeDebugInformation scopeDebugInformation = null;
			for (int i = 0; i < collection.Count; i++)
			{
				ScopeDebugInformation scopeDebugInformation2 = this.ReadLocalScope(collection[i]);
				if (i == 0)
				{
					scopeDebugInformation = scopeDebugInformation2;
				}
				else if (!MetadataReader.AddScope(scopeDebugInformation.scopes, scopeDebugInformation2))
				{
					scopeDebugInformation.Scopes.Add(scopeDebugInformation2);
				}
			}
			return scopeDebugInformation;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x000092E8 File Offset: 0x000074E8
		private static bool AddScope(Collection<ScopeDebugInformation> scopes, ScopeDebugInformation scope)
		{
			if (scopes.IsNullOrEmpty<ScopeDebugInformation>())
			{
				return false;
			}
			foreach (ScopeDebugInformation scopeDebugInformation in scopes)
			{
				if (scopeDebugInformation.HasScopes && MetadataReader.AddScope(scopeDebugInformation.Scopes, scope))
				{
					return true;
				}
				if (scope.Start.Offset >= scopeDebugInformation.Start.Offset && scope.End.Offset <= scopeDebugInformation.End.Offset)
				{
					scopeDebugInformation.Scopes.Add(scope);
					return true;
				}
			}
			return false;
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x000093A4 File Offset: 0x000075A4
		private ScopeDebugInformation ReadLocalScope(Row<uint, Range, Range, uint, uint, uint> record)
		{
			ScopeDebugInformation scopeDebugInformation = new ScopeDebugInformation
			{
				start = new InstructionOffset((int)record.Col4),
				end = new InstructionOffset((int)(record.Col4 + record.Col5)),
				token = new MetadataToken(TokenType.LocalScope, record.Col6)
			};
			if (record.Col1 > 0U)
			{
				scopeDebugInformation.import = this.metadata.GetImportScope(record.Col1);
			}
			if (record.Col2.Length > 0U)
			{
				scopeDebugInformation.variables = new Collection<VariableDebugInformation>((int)record.Col2.Length);
				for (uint num = 0U; num < record.Col2.Length; num += 1U)
				{
					VariableDebugInformation variableDebugInformation = this.ReadLocalVariable(record.Col2.Start + num);
					if (variableDebugInformation != null)
					{
						scopeDebugInformation.variables.Add(variableDebugInformation);
					}
				}
			}
			if (record.Col3.Length > 0U)
			{
				scopeDebugInformation.constants = new Collection<ConstantDebugInformation>((int)record.Col3.Length);
				for (uint num2 = 0U; num2 < record.Col3.Length; num2 += 1U)
				{
					ConstantDebugInformation constantDebugInformation = this.ReadLocalConstant(record.Col3.Start + num2);
					if (constantDebugInformation != null)
					{
						scopeDebugInformation.constants.Add(constantDebugInformation);
					}
				}
			}
			return scopeDebugInformation;
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x000094D4 File Offset: 0x000076D4
		private VariableDebugInformation ReadLocalVariable(uint rid)
		{
			if (!this.MoveTo(Table.LocalVariable, rid))
			{
				return null;
			}
			VariableAttributes variableAttributes = (VariableAttributes)base.ReadUInt16();
			int num = (int)base.ReadUInt16();
			string text = this.ReadString();
			VariableDebugInformation variableDebugInformation = new VariableDebugInformation(num, text)
			{
				Attributes = variableAttributes,
				token = new MetadataToken(TokenType.LocalVariable, rid)
			};
			variableDebugInformation.custom_infos = this.GetCustomDebugInformation(variableDebugInformation);
			return variableDebugInformation;
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00009530 File Offset: 0x00007730
		private ConstantDebugInformation ReadLocalConstant(uint rid)
		{
			if (!this.MoveTo(Table.LocalConstant, rid))
			{
				return null;
			}
			string text = this.ReadString();
			SignatureReader signatureReader = this.ReadSignature(this.ReadBlobIndex());
			TypeReference typeReference = signatureReader.ReadTypeSignature();
			object obj;
			if (typeReference.etype == ElementType.String)
			{
				if (signatureReader.buffer[signatureReader.position] != 255)
				{
					byte[] array = signatureReader.ReadBytes((int)((ulong)signatureReader.sig_length - (ulong)((long)signatureReader.position - (long)((ulong)signatureReader.start))));
					obj = Encoding.Unicode.GetString(array, 0, array.Length);
				}
				else
				{
					obj = null;
				}
			}
			else if (typeReference.IsTypeOf("System", "Decimal"))
			{
				byte b = signatureReader.ReadByte();
				obj = new decimal(signatureReader.ReadInt32(), signatureReader.ReadInt32(), signatureReader.ReadInt32(), (b & 128) > 0, b & 127);
			}
			else if (typeReference.IsTypeOf("System", "DateTime"))
			{
				obj = new DateTime(signatureReader.ReadInt64());
			}
			else if (typeReference.etype == ElementType.Object || typeReference.etype == ElementType.None || typeReference.etype == ElementType.Class)
			{
				obj = null;
			}
			else
			{
				obj = signatureReader.ReadConstantSignature(typeReference.etype);
			}
			ConstantDebugInformation constantDebugInformation = new ConstantDebugInformation(text, typeReference, obj)
			{
				token = new MetadataToken(TokenType.LocalConstant, rid)
			};
			constantDebugInformation.custom_infos = this.GetCustomDebugInformation(constantDebugInformation);
			return constantDebugInformation;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x00009684 File Offset: 0x00007884
		private void InitializeImportScopes()
		{
			if (this.metadata.ImportScopes != null)
			{
				return;
			}
			int num = this.MoveTo(Table.ImportScope);
			this.metadata.ImportScopes = new ImportDebugInformation[num];
			for (int i = 1; i <= num; i++)
			{
				this.ReadTableIndex(Table.ImportScope);
				ImportDebugInformation importDebugInformation = new ImportDebugInformation();
				importDebugInformation.token = new MetadataToken(TokenType.ImportScope, i);
				SignatureReader signatureReader = this.ReadSignature(this.ReadBlobIndex());
				while (signatureReader.CanReadMore())
				{
					importDebugInformation.Targets.Add(this.ReadImportTarget(signatureReader));
				}
				this.metadata.ImportScopes[i - 1] = importDebugInformation;
			}
			this.MoveTo(Table.ImportScope);
			for (int j = 0; j < num; j++)
			{
				uint num2 = this.ReadTableIndex(Table.ImportScope);
				this.ReadBlobIndex();
				if (num2 != 0U)
				{
					this.metadata.ImportScopes[j].Parent = this.metadata.GetImportScope(num2);
				}
			}
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000976C File Offset: 0x0000796C
		public string ReadUTF8StringBlob(uint signature)
		{
			return this.ReadStringBlob(signature, Encoding.UTF8);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000977A File Offset: 0x0000797A
		private string ReadUnicodeStringBlob(uint signature)
		{
			return this.ReadStringBlob(signature, Encoding.Unicode);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00009788 File Offset: 0x00007988
		private string ReadStringBlob(uint signature, Encoding encoding)
		{
			byte[] array;
			int num;
			int num2;
			this.GetBlobView(signature, out array, out num, out num2);
			if (num2 == 0)
			{
				return string.Empty;
			}
			return encoding.GetString(array, num, num2);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x000097B4 File Offset: 0x000079B4
		private ImportTarget ReadImportTarget(SignatureReader signature)
		{
			AssemblyNameReference assemblyNameReference = null;
			string text = null;
			string text2 = null;
			TypeReference typeReference = null;
			ImportTargetKind importTargetKind = (ImportTargetKind)signature.ReadCompressedUInt32();
			switch (importTargetKind)
			{
			case ImportTargetKind.ImportNamespace:
				text = this.ReadUTF8StringBlob(signature.ReadCompressedUInt32());
				break;
			case ImportTargetKind.ImportNamespaceInAssembly:
				assemblyNameReference = this.metadata.GetAssemblyNameReference(signature.ReadCompressedUInt32());
				text = this.ReadUTF8StringBlob(signature.ReadCompressedUInt32());
				break;
			case ImportTargetKind.ImportType:
				typeReference = signature.ReadTypeToken();
				break;
			case ImportTargetKind.ImportXmlNamespaceWithAlias:
				text2 = this.ReadUTF8StringBlob(signature.ReadCompressedUInt32());
				text = this.ReadUTF8StringBlob(signature.ReadCompressedUInt32());
				break;
			case ImportTargetKind.ImportAlias:
				text2 = this.ReadUTF8StringBlob(signature.ReadCompressedUInt32());
				break;
			case ImportTargetKind.DefineAssemblyAlias:
				text2 = this.ReadUTF8StringBlob(signature.ReadCompressedUInt32());
				assemblyNameReference = this.metadata.GetAssemblyNameReference(signature.ReadCompressedUInt32());
				break;
			case ImportTargetKind.DefineNamespaceAlias:
				text2 = this.ReadUTF8StringBlob(signature.ReadCompressedUInt32());
				text = this.ReadUTF8StringBlob(signature.ReadCompressedUInt32());
				break;
			case ImportTargetKind.DefineNamespaceInAssemblyAlias:
				text2 = this.ReadUTF8StringBlob(signature.ReadCompressedUInt32());
				assemblyNameReference = this.metadata.GetAssemblyNameReference(signature.ReadCompressedUInt32());
				text = this.ReadUTF8StringBlob(signature.ReadCompressedUInt32());
				break;
			case ImportTargetKind.DefineTypeAlias:
				text2 = this.ReadUTF8StringBlob(signature.ReadCompressedUInt32());
				typeReference = signature.ReadTypeToken();
				break;
			}
			return new ImportTarget(importTargetKind)
			{
				alias = text2,
				type = typeReference,
				@namespace = text,
				reference = assemblyNameReference
			};
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00009918 File Offset: 0x00007B18
		private void InitializeStateMachineMethods()
		{
			if (this.metadata.StateMachineMethods != null)
			{
				return;
			}
			int num = this.MoveTo(Table.StateMachineMethod);
			this.metadata.StateMachineMethods = new Dictionary<uint, uint>(num);
			for (int i = 0; i < num; i++)
			{
				this.metadata.StateMachineMethods.Add(this.ReadTableIndex(Table.Method), this.ReadTableIndex(Table.Method));
			}
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00009978 File Offset: 0x00007B78
		public MethodDefinition ReadStateMachineKickoffMethod(MethodDefinition method)
		{
			this.InitializeStateMachineMethods();
			uint num;
			if (!this.metadata.TryGetStateMachineKickOffMethod(method, out num))
			{
				return null;
			}
			return this.GetMethodDefinition(num);
		}

		// Token: 0x060001CE RID: 462 RVA: 0x000099A4 File Offset: 0x00007BA4
		private void InitializeCustomDebugInformations()
		{
			if (this.metadata.CustomDebugInformations != null)
			{
				return;
			}
			int num = this.MoveTo(Table.CustomDebugInformation);
			this.metadata.CustomDebugInformations = new Dictionary<MetadataToken, Row<Guid, uint, uint>[]>();
			uint num2 = 1U;
			while ((ulong)num2 <= (ulong)((long)num))
			{
				MetadataToken metadataToken = this.ReadMetadataToken(CodedIndex.HasCustomDebugInformation);
				Row<Guid, uint, uint> row = new Row<Guid, uint, uint>(this.ReadGuid(), this.ReadBlobIndex(), num2);
				Row<Guid, uint, uint>[] array;
				this.metadata.CustomDebugInformations.TryGetValue(metadataToken, out array);
				this.metadata.CustomDebugInformations[metadataToken] = array.Add(row);
				num2 += 1U;
			}
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00009A30 File Offset: 0x00007C30
		public Collection<CustomDebugInformation> GetCustomDebugInformation(ICustomDebugInformationProvider provider)
		{
			this.InitializeCustomDebugInformations();
			Row<Guid, uint, uint>[] array;
			if (!this.metadata.CustomDebugInformations.TryGetValue(provider.MetadataToken, out array))
			{
				return null;
			}
			Collection<CustomDebugInformation> collection = new Collection<CustomDebugInformation>(array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Col1 == StateMachineScopeDebugInformation.KindIdentifier)
				{
					SignatureReader signatureReader = this.ReadSignature(array[i].Col2);
					Collection<StateMachineScope> collection2 = new Collection<StateMachineScope>();
					while (signatureReader.CanReadMore())
					{
						int num = signatureReader.ReadInt32();
						int num2 = num + signatureReader.ReadInt32();
						collection2.Add(new StateMachineScope(num, num2));
					}
					collection.Add(new StateMachineScopeDebugInformation
					{
						scopes = collection2
					});
				}
				else if (array[i].Col1 == AsyncMethodBodyDebugInformation.KindIdentifier)
				{
					SignatureReader signatureReader2 = this.ReadSignature(array[i].Col2);
					int num3 = signatureReader2.ReadInt32() - 1;
					Collection<InstructionOffset> collection3 = new Collection<InstructionOffset>();
					Collection<InstructionOffset> collection4 = new Collection<InstructionOffset>();
					Collection<MethodDefinition> collection5 = new Collection<MethodDefinition>();
					while (signatureReader2.CanReadMore())
					{
						collection3.Add(new InstructionOffset(signatureReader2.ReadInt32()));
						collection4.Add(new InstructionOffset(signatureReader2.ReadInt32()));
						collection5.Add(this.GetMethodDefinition(signatureReader2.ReadCompressedUInt32()));
					}
					collection.Add(new AsyncMethodBodyDebugInformation(num3)
					{
						yields = collection3,
						resumes = collection4,
						resume_methods = collection5
					});
				}
				else if (array[i].Col1 == EmbeddedSourceDebugInformation.KindIdentifier)
				{
					SignatureReader signatureReader3 = this.ReadSignature(array[i].Col2);
					int num4 = signatureReader3.ReadInt32();
					uint num5 = signatureReader3.sig_length - 4U;
					CustomDebugInformation customDebugInformation = null;
					if (num4 == 0)
					{
						customDebugInformation = new EmbeddedSourceDebugInformation(signatureReader3.ReadBytes((int)num5), false);
					}
					else if (num4 > 0)
					{
						Stream stream = new MemoryStream(signatureReader3.ReadBytes((int)num5));
						byte[] array2 = new byte[num4];
						MemoryStream memoryStream = new MemoryStream(array2);
						using (DeflateStream deflateStream = new DeflateStream(stream, CompressionMode.Decompress, true))
						{
							deflateStream.CopyTo(memoryStream);
						}
						customDebugInformation = new EmbeddedSourceDebugInformation(array2, true);
					}
					else if (num4 < 0)
					{
						customDebugInformation = new BinaryCustomDebugInformation(array[i].Col1, this.ReadBlob(array[i].Col2));
					}
					collection.Add(customDebugInformation);
				}
				else if (array[i].Col1 == SourceLinkDebugInformation.KindIdentifier)
				{
					collection.Add(new SourceLinkDebugInformation(Encoding.UTF8.GetString(this.ReadBlob(array[i].Col2))));
				}
				else
				{
					collection.Add(new BinaryCustomDebugInformation(array[i].Col1, this.ReadBlob(array[i].Col2)));
				}
				collection[i].token = new MetadataToken(TokenType.CustomDebugInformation, array[i].Col3);
			}
			return collection;
		}

		// Token: 0x04000038 RID: 56
		internal readonly Image image;

		// Token: 0x04000039 RID: 57
		internal readonly ModuleDefinition module;

		// Token: 0x0400003A RID: 58
		internal readonly MetadataSystem metadata;

		// Token: 0x0400003B RID: 59
		internal CodeReader code;

		// Token: 0x0400003C RID: 60
		internal IGenericContext context;

		// Token: 0x0400003D RID: 61
		private readonly MetadataReader metadata_reader;
	}
}
