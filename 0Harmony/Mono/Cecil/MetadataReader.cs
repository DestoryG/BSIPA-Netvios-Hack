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
	// Token: 0x020000C5 RID: 197
	internal sealed class MetadataReader : ByteBuffer
	{
		// Token: 0x0600048C RID: 1164 RVA: 0x00014248 File Offset: 0x00012448
		public MetadataReader(ModuleDefinition module)
			: base(module.Image.TableHeap.data)
		{
			this.image = module.Image;
			this.module = module;
			this.metadata = module.MetadataSystem;
			this.code = new CodeReader(this);
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x00014296 File Offset: 0x00012496
		public MetadataReader(Image image, ModuleDefinition module, MetadataReader metadata_reader)
			: base(image.TableHeap.data)
		{
			this.image = image;
			this.module = module;
			this.metadata = module.MetadataSystem;
			this.metadata_reader = metadata_reader;
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x000142CA File Offset: 0x000124CA
		private int GetCodedIndexSize(CodedIndex index)
		{
			return this.image.GetCodedIndexSize(index);
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x000142D8 File Offset: 0x000124D8
		private uint ReadByIndexSize(int size)
		{
			if (size == 4)
			{
				return base.ReadUInt32();
			}
			return (uint)base.ReadUInt16();
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x000142EC File Offset: 0x000124EC
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

		// Token: 0x06000491 RID: 1169 RVA: 0x00014328 File Offset: 0x00012528
		private byte[] ReadBlob(uint signature)
		{
			BlobHeap blobHeap = this.image.BlobHeap;
			if (blobHeap == null)
			{
				return Empty<byte>.Array;
			}
			return blobHeap.Read(signature);
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00014354 File Offset: 0x00012554
		private uint ReadBlobIndex()
		{
			BlobHeap blobHeap = this.image.BlobHeap;
			return this.ReadByIndexSize((blobHeap != null) ? blobHeap.IndexSize : 2);
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00014380 File Offset: 0x00012580
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

		// Token: 0x06000494 RID: 1172 RVA: 0x000143B4 File Offset: 0x000125B4
		private string ReadString()
		{
			return this.image.StringHeap.Read(this.ReadByIndexSize(this.image.StringHeap.IndexSize));
		}

		// Token: 0x06000495 RID: 1173 RVA: 0x000143DC File Offset: 0x000125DC
		private uint ReadStringIndex()
		{
			return this.ReadByIndexSize(this.image.StringHeap.IndexSize);
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x000143F4 File Offset: 0x000125F4
		private Guid ReadGuid()
		{
			return this.image.GuidHeap.Read(this.ReadByIndexSize(this.image.GuidHeap.IndexSize));
		}

		// Token: 0x06000497 RID: 1175 RVA: 0x0001441C File Offset: 0x0001261C
		private uint ReadTableIndex(Table table)
		{
			return this.ReadByIndexSize(this.image.GetTableIndexSize(table));
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x00014430 File Offset: 0x00012630
		private MetadataToken ReadMetadataToken(CodedIndex index)
		{
			return index.GetMetadataToken(this.ReadByIndexSize(this.GetCodedIndexSize(index)));
		}

		// Token: 0x06000499 RID: 1177 RVA: 0x00014448 File Offset: 0x00012648
		private int MoveTo(Table table)
		{
			TableInformation tableInformation = this.image.TableHeap[table];
			if (tableInformation.Length != 0U)
			{
				this.position = (int)tableInformation.Offset;
			}
			return (int)tableInformation.Length;
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x00014484 File Offset: 0x00012684
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

		// Token: 0x0600049B RID: 1179 RVA: 0x000144CC File Offset: 0x000126CC
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

		// Token: 0x0600049C RID: 1180 RVA: 0x00014512 File Offset: 0x00012712
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

		// Token: 0x0600049D RID: 1181 RVA: 0x00014540 File Offset: 0x00012740
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

		// Token: 0x0600049E RID: 1182 RVA: 0x000145EC File Offset: 0x000127EC
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

		// Token: 0x0600049F RID: 1183 RVA: 0x00014630 File Offset: 0x00012830
		public MethodDefinition ReadEntryPoint()
		{
			if (this.module.Image.EntryPointToken == 0U)
			{
				return null;
			}
			MetadataToken metadataToken = new MetadataToken(this.module.Image.EntryPointToken);
			return this.GetMethodDefinition(metadataToken.RID);
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x00014678 File Offset: 0x00012878
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

		// Token: 0x060004A1 RID: 1185 RVA: 0x00014714 File Offset: 0x00012914
		private string GetModuleFileName(string name)
		{
			if (this.module.FileName == null)
			{
				throw new NotSupportedException();
			}
			return Path.Combine(Path.GetDirectoryName(this.module.FileName), name);
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x00014740 File Offset: 0x00012940
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

		// Token: 0x060004A3 RID: 1187 RVA: 0x000147AD File Offset: 0x000129AD
		public Collection<ModuleReference> ReadModuleReferences()
		{
			this.InitializeModuleReferences();
			return new Collection<ModuleReference>(this.metadata.ModuleReferences);
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x000147C8 File Offset: 0x000129C8
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

		// Token: 0x060004A5 RID: 1189 RVA: 0x00014804 File Offset: 0x00012A04
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

		// Token: 0x060004A6 RID: 1190 RVA: 0x000148EC File Offset: 0x00012AEC
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

		// Token: 0x060004A7 RID: 1191 RVA: 0x00014930 File Offset: 0x00012B30
		public byte[] GetManagedResource(uint offset)
		{
			return this.image.GetReaderAt<uint, byte[]>(this.image.Resources.VirtualAddress, offset, delegate(uint o, BinaryStreamReader reader)
			{
				reader.Advance((int)o);
				return reader.ReadBytes(reader.ReadInt32());
			}) ?? Empty<byte>.Array;
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00014981 File Offset: 0x00012B81
		private void PopulateVersionAndFlags(AssemblyNameReference name)
		{
			name.Version = new Version((int)base.ReadUInt16(), (int)base.ReadUInt16(), (int)base.ReadUInt16(), (int)base.ReadUInt16());
			name.Attributes = (AssemblyAttributes)base.ReadUInt32();
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x000149B2 File Offset: 0x00012BB2
		private void PopulateNameAndCulture(AssemblyNameReference name)
		{
			name.Name = this.ReadString();
			name.Culture = this.ReadString();
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x000149CC File Offset: 0x00012BCC
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

		// Token: 0x060004AB RID: 1195 RVA: 0x00014A5C File Offset: 0x00012C5C
		private void CompleteTypes()
		{
			foreach (TypeDefinition typeDefinition in this.metadata.Types)
			{
				Mixin.Read(typeDefinition.Fields);
				Mixin.Read(typeDefinition.Methods);
			}
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00014A9C File Offset: 0x00012C9C
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

		// Token: 0x060004AD RID: 1197 RVA: 0x00014B30 File Offset: 0x00012D30
		private static bool IsNested(TypeAttributes attributes)
		{
			TypeAttributes typeAttributes = attributes & TypeAttributes.VisibilityMask;
			return typeAttributes - TypeAttributes.NestedPublic <= 5U;
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x00014B4C File Offset: 0x00012D4C
		public bool HasNestedTypes(TypeDefinition type)
		{
			this.InitializeNestedTypes();
			Collection<uint> collection;
			return this.metadata.TryGetNestedTypeMapping(type, out collection) && collection.Count > 0;
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00014B7C File Offset: 0x00012D7C
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

		// Token: 0x060004B0 RID: 1200 RVA: 0x00014BEC File Offset: 0x00012DEC
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

		// Token: 0x060004B1 RID: 1201 RVA: 0x00014C5A File Offset: 0x00012E5A
		private void AddNestedMapping(uint declaring, uint nested)
		{
			this.metadata.SetNestedTypeMapping(declaring, MetadataReader.AddMapping<uint, uint>(this.metadata.NestedTypes, declaring, nested));
			this.metadata.SetReverseNestedTypeMapping(nested, declaring);
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00014C88 File Offset: 0x00012E88
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

		// Token: 0x060004B3 RID: 1203 RVA: 0x00014CB0 File Offset: 0x00012EB0
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

		// Token: 0x060004B4 RID: 1204 RVA: 0x00014D68 File Offset: 0x00012F68
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

		// Token: 0x060004B5 RID: 1205 RVA: 0x00014D9C File Offset: 0x00012F9C
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

		// Token: 0x060004B6 RID: 1206 RVA: 0x00014E44 File Offset: 0x00013044
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

		// Token: 0x060004B7 RID: 1207 RVA: 0x00014EC0 File Offset: 0x000130C0
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

		// Token: 0x060004B8 RID: 1208 RVA: 0x00014F31 File Offset: 0x00013131
		public TypeReference GetTypeDefOrRef(MetadataToken token)
		{
			return (TypeReference)this.LookupToken(token);
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x00014F40 File Offset: 0x00013140
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

		// Token: 0x060004BA RID: 1210 RVA: 0x00014F81 File Offset: 0x00013181
		private TypeDefinition ReadTypeDefinition(uint rid)
		{
			if (!this.MoveTo(Table.TypeDef, rid))
			{
				return null;
			}
			return this.ReadType(rid);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x00014F96 File Offset: 0x00013196
		private void InitializeTypeReferences()
		{
			if (this.metadata.TypeReferences != null)
			{
				return;
			}
			this.metadata.TypeReferences = new TypeReference[this.image.GetTableLength(Table.TypeRef)];
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x00014FC4 File Offset: 0x000131C4
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

		// Token: 0x060004BD RID: 1213 RVA: 0x0001502C File Offset: 0x0001322C
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

		// Token: 0x060004BE RID: 1214 RVA: 0x00015058 File Offset: 0x00013258
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

		// Token: 0x060004BF RID: 1215 RVA: 0x00015130 File Offset: 0x00013330
		private IMetadataScope GetTypeReferenceScope(MetadataToken scope)
		{
			if (scope.TokenType == TokenType.Module)
			{
				return this.module;
			}
			TokenType tokenType = scope.TokenType;
			IMetadataScope[] array2;
			if (tokenType != TokenType.ModuleRef)
			{
				if (tokenType != TokenType.AssemblyRef)
				{
					throw new NotSupportedException();
				}
				this.InitializeAssemblyReferences();
				IMetadataScope[] array = this.metadata.AssemblyReferences;
				array2 = array;
			}
			else
			{
				this.InitializeModuleReferences();
				IMetadataScope[] array = this.metadata.ModuleReferences;
				array2 = array;
			}
			uint num = scope.RID - 1U;
			if (num < 0U || (ulong)num >= (ulong)((long)array2.Length))
			{
				return null;
			}
			return array2[(int)num];
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x000151B4 File Offset: 0x000133B4
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

		// Token: 0x060004C1 RID: 1217 RVA: 0x000151F8 File Offset: 0x000133F8
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

		// Token: 0x060004C2 RID: 1218 RVA: 0x00015243 File Offset: 0x00013443
		private SignatureReader ReadSignature(uint signature)
		{
			return new SignatureReader(signature, this);
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0001524C File Offset: 0x0001344C
		public bool HasInterfaces(TypeDefinition type)
		{
			this.InitializeInterfaces();
			Collection<Row<uint, MetadataToken>> collection;
			return this.metadata.TryGetInterfaceMapping(type, out collection);
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x00015270 File Offset: 0x00013470
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

		// Token: 0x060004C5 RID: 1221 RVA: 0x00015300 File Offset: 0x00013500
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

		// Token: 0x060004C6 RID: 1222 RVA: 0x00015361 File Offset: 0x00013561
		private void AddInterfaceMapping(uint type, Row<uint, MetadataToken> @interface)
		{
			this.metadata.SetInterfaceMapping(type, MetadataReader.AddMapping<uint, Row<uint, MetadataToken>>(this.metadata.Interfaces, type, @interface));
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x00015384 File Offset: 0x00013584
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

		// Token: 0x060004C8 RID: 1224 RVA: 0x00015414 File Offset: 0x00013614
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

		// Token: 0x060004C9 RID: 1225 RVA: 0x00015483 File Offset: 0x00013683
		private void InitializeFields()
		{
			if (this.metadata.Fields != null)
			{
				return;
			}
			this.metadata.Fields = new FieldDefinition[this.image.GetTableLength(Table.Field)];
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x000154AF File Offset: 0x000136AF
		private TypeReference ReadFieldType(uint signature)
		{
			SignatureReader signatureReader = this.ReadSignature(signature);
			if (signatureReader.ReadByte() != 6)
			{
				throw new NotSupportedException();
			}
			return signatureReader.ReadTypeSignature();
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x000154CC File Offset: 0x000136CC
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

		// Token: 0x060004CC RID: 1228 RVA: 0x00015537 File Offset: 0x00013737
		private byte[] GetFieldInitializeValue(int size, uint rva)
		{
			return this.image.GetReaderAt<int, byte[]>(rva, size, (int s, BinaryStreamReader reader) => reader.ReadBytes(s)) ?? Empty<byte>.Array;
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x00015570 File Offset: 0x00013770
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

		// Token: 0x060004CE RID: 1230 RVA: 0x00015650 File Offset: 0x00013850
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

		// Token: 0x060004CF RID: 1231 RVA: 0x000156B0 File Offset: 0x000138B0
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

		// Token: 0x060004D0 RID: 1232 RVA: 0x000156FC File Offset: 0x000138FC
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

		// Token: 0x060004D1 RID: 1233 RVA: 0x0001575C File Offset: 0x0001395C
		public bool HasEvents(TypeDefinition type)
		{
			this.InitializeEvents();
			Range range;
			return this.metadata.TryGetEventsRange(type, out range) && range.Length > 0U;
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x0001578C File Offset: 0x0001398C
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

		// Token: 0x060004D3 RID: 1235 RVA: 0x00015840 File Offset: 0x00013A40
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

		// Token: 0x060004D4 RID: 1236 RVA: 0x00015894 File Offset: 0x00013A94
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

		// Token: 0x060004D5 RID: 1237 RVA: 0x000158F8 File Offset: 0x00013AF8
		public bool HasProperties(TypeDefinition type)
		{
			this.InitializeProperties();
			Range range;
			return this.metadata.TryGetPropertiesRange(type, out range) && range.Length > 0U;
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x00015928 File Offset: 0x00013B28
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

		// Token: 0x060004D7 RID: 1239 RVA: 0x000159DC File Offset: 0x00013BDC
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

		// Token: 0x060004D8 RID: 1240 RVA: 0x00015A60 File Offset: 0x00013C60
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

		// Token: 0x060004D9 RID: 1241 RVA: 0x00015AC4 File Offset: 0x00013CC4
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

		// Token: 0x060004DA RID: 1242 RVA: 0x00015C5E File Offset: 0x00013E5E
		private static EventDefinition GetEvent(TypeDefinition type, MetadataToken token)
		{
			if (token.TokenType != TokenType.Event)
			{
				throw new ArgumentException();
			}
			return MetadataReader.GetMember<EventDefinition>(type.Events, token);
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00015C80 File Offset: 0x00013E80
		private static PropertyDefinition GetProperty(TypeDefinition type, MetadataToken token)
		{
			if (token.TokenType != TokenType.Property)
			{
				throw new ArgumentException();
			}
			return MetadataReader.GetMember<PropertyDefinition>(type.Properties, token);
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x00015CA4 File Offset: 0x00013EA4
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

		// Token: 0x060004DD RID: 1245 RVA: 0x00015CE8 File Offset: 0x00013EE8
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

		// Token: 0x060004DE RID: 1246 RVA: 0x00015D5A File Offset: 0x00013F5A
		public void ReadMethods(PropertyDefinition property)
		{
			this.ReadAllSemantics(property.DeclaringType);
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x00015D68 File Offset: 0x00013F68
		public void ReadMethods(EventDefinition @event)
		{
			this.ReadAllSemantics(@event.DeclaringType);
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x00015D76 File Offset: 0x00013F76
		public void ReadAllSemantics(MethodDefinition method)
		{
			this.ReadAllSemantics(method.DeclaringType);
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x00015D84 File Offset: 0x00013F84
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

		// Token: 0x060004E2 RID: 1250 RVA: 0x00015DD4 File Offset: 0x00013FD4
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

		// Token: 0x060004E3 RID: 1251 RVA: 0x00015E60 File Offset: 0x00014060
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

		// Token: 0x060004E4 RID: 1252 RVA: 0x00015EA9 File Offset: 0x000140A9
		private static bool IsDeleted(IMemberDefinition member)
		{
			return member.IsSpecialName && member.Name == "_Deleted";
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00015EC5 File Offset: 0x000140C5
		private void InitializeMethods()
		{
			if (this.metadata.Methods != null)
			{
				return;
			}
			this.metadata.Methods = new MethodDefinition[this.image.GetTableLength(Table.Method)];
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00015EF4 File Offset: 0x000140F4
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

		// Token: 0x060004E7 RID: 1255 RVA: 0x00015FB8 File Offset: 0x000141B8
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

		// Token: 0x060004E8 RID: 1256 RVA: 0x00016010 File Offset: 0x00014210
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

		// Token: 0x060004E9 RID: 1257 RVA: 0x00016058 File Offset: 0x00014258
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

		// Token: 0x060004EA RID: 1258 RVA: 0x000160B6 File Offset: 0x000142B6
		private void ReadMethodSignature(uint signature, IMethodSignature method)
		{
			this.ReadSignature(signature).ReadMethodSignature(method);
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x000160C8 File Offset: 0x000142C8
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

		// Token: 0x060004EC RID: 1260 RVA: 0x0001614C File Offset: 0x0001434C
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

		// Token: 0x060004ED RID: 1261 RVA: 0x000161DC File Offset: 0x000143DC
		public bool HasGenericParameters(IGenericParameterProvider provider)
		{
			this.InitializeGenericParameters();
			Range[] array;
			return this.metadata.TryGetGenericParameterRanges(provider, out array) && MetadataReader.RangesSize(array) > 0;
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0001620C File Offset: 0x0001440C
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

		// Token: 0x060004EF RID: 1263 RVA: 0x00016270 File Offset: 0x00014470
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

		// Token: 0x060004F0 RID: 1264 RVA: 0x000162E9 File Offset: 0x000144E9
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

		// Token: 0x060004F1 RID: 1265 RVA: 0x00016318 File Offset: 0x00014518
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

		// Token: 0x060004F2 RID: 1266 RVA: 0x000163B0 File Offset: 0x000145B0
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

		// Token: 0x060004F3 RID: 1267 RVA: 0x000163F8 File Offset: 0x000145F8
		public bool HasGenericConstraints(GenericParameter generic_parameter)
		{
			this.InitializeGenericConstraints();
			Collection<Row<uint, MetadataToken>> collection;
			return this.metadata.TryGetGenericConstraintMapping(generic_parameter, out collection) && collection.Count > 0;
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00016428 File Offset: 0x00014628
		public GenericParameterConstraintCollection ReadGenericConstraints(GenericParameter generic_parameter)
		{
			this.InitializeGenericConstraints();
			Collection<Row<uint, MetadataToken>> collection;
			if (!this.metadata.TryGetGenericConstraintMapping(generic_parameter, out collection))
			{
				return new GenericParameterConstraintCollection(generic_parameter);
			}
			GenericParameterConstraintCollection genericParameterConstraintCollection = new GenericParameterConstraintCollection(generic_parameter, collection.Count);
			this.context = (IGenericContext)generic_parameter.Owner;
			for (int i = 0; i < collection.Count; i++)
			{
				genericParameterConstraintCollection.Add(new GenericParameterConstraint(this.GetTypeDefOrRef(collection[i].Col2), new MetadataToken(TokenType.GenericParamConstraint, collection[i].Col1)));
			}
			this.metadata.RemoveGenericConstraintMapping(generic_parameter);
			return genericParameterConstraintCollection;
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x000164C4 File Offset: 0x000146C4
		private void InitializeGenericConstraints()
		{
			if (this.metadata.GenericConstraints != null)
			{
				return;
			}
			int num = this.MoveTo(Table.GenericParamConstraint);
			this.metadata.GenericConstraints = new Dictionary<uint, Collection<Row<uint, MetadataToken>>>(num);
			uint num2 = 1U;
			while ((ulong)num2 <= (ulong)((long)num))
			{
				this.AddGenericConstraintMapping(this.ReadTableIndex(Table.GenericParam), new Row<uint, MetadataToken>(num2, this.ReadMetadataToken(CodedIndex.TypeDefOrRef)));
				num2 += 1U;
			}
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x00016522 File Offset: 0x00014722
		private void AddGenericConstraintMapping(uint generic_parameter, Row<uint, MetadataToken> constraint)
		{
			this.metadata.SetGenericConstraintMapping(generic_parameter, MetadataReader.AddMapping<uint, Row<uint, MetadataToken>>(this.metadata.GenericConstraints, generic_parameter, constraint));
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x00016544 File Offset: 0x00014744
		public bool HasOverrides(MethodDefinition method)
		{
			this.InitializeOverrides();
			Collection<MetadataToken> collection;
			return this.metadata.TryGetOverrideMapping(method, out collection) && collection.Count > 0;
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x00016574 File Offset: 0x00014774
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

		// Token: 0x060004F9 RID: 1273 RVA: 0x000165E8 File Offset: 0x000147E8
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

		// Token: 0x060004FA RID: 1274 RVA: 0x00016663 File Offset: 0x00014863
		private void AddOverrideMapping(uint method_rid, MetadataToken @override)
		{
			this.metadata.SetOverrideMapping(method_rid, MetadataReader.AddMapping<uint, MetadataToken>(this.metadata.Overrides, method_rid, @override));
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00016683 File Offset: 0x00014883
		public MethodBody ReadMethodBody(MethodDefinition method)
		{
			return this.code.ReadMethodBody(method);
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00016691 File Offset: 0x00014891
		public int ReadCodeSize(MethodDefinition method)
		{
			return this.code.ReadCodeSize(method);
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x000166A0 File Offset: 0x000148A0
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

		// Token: 0x060004FE RID: 1278 RVA: 0x000166E0 File Offset: 0x000148E0
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

		// Token: 0x060004FF RID: 1279 RVA: 0x00016750 File Offset: 0x00014950
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

		// Token: 0x06000500 RID: 1280 RVA: 0x00016844 File Offset: 0x00014A44
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

		// Token: 0x06000501 RID: 1281 RVA: 0x00016870 File Offset: 0x00014A70
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

		// Token: 0x06000502 RID: 1282 RVA: 0x000168A8 File Offset: 0x00014AA8
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

		// Token: 0x06000503 RID: 1283 RVA: 0x000168D4 File Offset: 0x00014AD4
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

		// Token: 0x06000504 RID: 1284 RVA: 0x0001690C File Offset: 0x00014B0C
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

		// Token: 0x06000505 RID: 1285 RVA: 0x0001695C File Offset: 0x00014B5C
		private MethodSpecification ReadMethodSpecSignature(uint signature, MethodReference method)
		{
			SignatureReader signatureReader = this.ReadSignature(signature);
			if (signatureReader.ReadByte() != 10)
			{
				throw new NotSupportedException();
			}
			uint num = signatureReader.ReadCompressedUInt32();
			GenericInstanceMethod genericInstanceMethod = new GenericInstanceMethod(method, (int)num);
			signatureReader.ReadGenericInstanceSignature(method, genericInstanceMethod, num);
			return genericInstanceMethod;
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x00016998 File Offset: 0x00014B98
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

		// Token: 0x06000507 RID: 1287 RVA: 0x000169E0 File Offset: 0x00014BE0
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

		// Token: 0x06000508 RID: 1288 RVA: 0x00016A8C File Offset: 0x00014C8C
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

		// Token: 0x06000509 RID: 1289 RVA: 0x00016AC0 File Offset: 0x00014CC0
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

		// Token: 0x0600050A RID: 1290 RVA: 0x00016B24 File Offset: 0x00014D24
		private MemberReference ReadMethodMemberReference(MetadataToken token, string name, uint signature)
		{
			MethodDefinition methodDefinition = this.GetMethodDefinition(token.RID);
			this.context = methodDefinition;
			MemberReference memberReference = this.ReadMemberReferenceSignature(signature, methodDefinition.DeclaringType);
			memberReference.Name = name;
			return memberReference;
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x00016B5A File Offset: 0x00014D5A
		private void InitializeMemberReferences()
		{
			if (this.metadata.MemberReferences != null)
			{
				return;
			}
			this.metadata.MemberReferences = new MemberReference[this.image.GetTableLength(Table.MemberRef)];
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00016B88 File Offset: 0x00014D88
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

		// Token: 0x0600050D RID: 1293 RVA: 0x00016C10 File Offset: 0x00014E10
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

		// Token: 0x0600050E RID: 1294 RVA: 0x00016C82 File Offset: 0x00014E82
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

		// Token: 0x0600050F RID: 1295 RVA: 0x00016CC4 File Offset: 0x00014EC4
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

		// Token: 0x06000510 RID: 1296 RVA: 0x00016D20 File Offset: 0x00014F20
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

		// Token: 0x06000511 RID: 1297 RVA: 0x00016D44 File Offset: 0x00014F44
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

		// Token: 0x06000512 RID: 1298 RVA: 0x00016D7E File Offset: 0x00014F7E
		private object ReadConstantPrimitive(ElementType type, uint signature)
		{
			return this.ReadSignature(signature).ReadConstantSignature(type);
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x00016D8D File Offset: 0x00014F8D
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

		// Token: 0x06000514 RID: 1300 RVA: 0x00016DBC File Offset: 0x00014FBC
		public bool HasCustomAttributes(ICustomAttributeProvider owner)
		{
			this.InitializeCustomAttributes();
			Range[] array;
			return this.metadata.TryGetCustomAttributeRanges(owner, out array) && MetadataReader.RangesSize(array) > 0;
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x00016DEC File Offset: 0x00014FEC
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

		// Token: 0x06000516 RID: 1302 RVA: 0x00016E9C File Offset: 0x0001509C
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

		// Token: 0x06000517 RID: 1303 RVA: 0x00016F00 File Offset: 0x00015100
		private static int RangesSize(Range[] ranges)
		{
			uint num = 0U;
			for (int i = 0; i < ranges.Length; i++)
			{
				num += ranges[i].Length;
			}
			return (int)num;
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x00016F30 File Offset: 0x00015130
		public IEnumerable<CustomAttribute> GetCustomAttributes()
		{
			this.InitializeTypeDefinitions();
			uint length = this.image.TableHeap[Table.CustomAttribute].Length;
			Collection<CustomAttribute> collection = new Collection<CustomAttribute>((int)length);
			this.ReadCustomAttributeRange(new Range(1U, length), collection);
			return collection;
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x00016F71 File Offset: 0x00015171
		public byte[] ReadCustomAttributeBlob(uint signature)
		{
			return this.ReadBlob(signature);
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x00016F7C File Offset: 0x0001517C
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

		// Token: 0x0600051B RID: 1307 RVA: 0x00016FF4 File Offset: 0x000151F4
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

		// Token: 0x0600051C RID: 1308 RVA: 0x0001705D File Offset: 0x0001525D
		public bool HasMarshalInfo(IMarshalInfoProvider owner)
		{
			this.InitializeMarshalInfos();
			return this.metadata.FieldMarshals.ContainsKey(owner.MetadataToken);
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0001707C File Offset: 0x0001527C
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

		// Token: 0x0600051E RID: 1310 RVA: 0x000170CE File Offset: 0x000152CE
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

		// Token: 0x0600051F RID: 1311 RVA: 0x00017100 File Offset: 0x00015300
		public bool HasSecurityDeclarations(ISecurityDeclarationProvider owner)
		{
			this.InitializeSecurityDeclarations();
			Range[] array;
			return this.metadata.TryGetSecurityDeclarationRanges(owner, out array) && MetadataReader.RangesSize(array) > 0;
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00017130 File Offset: 0x00015330
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

		// Token: 0x06000521 RID: 1313 RVA: 0x00017190 File Offset: 0x00015390
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

		// Token: 0x06000522 RID: 1314 RVA: 0x00016F71 File Offset: 0x00015171
		public byte[] ReadSecurityDeclarationBlob(uint signature)
		{
			return this.ReadBlob(signature);
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x000171EC File Offset: 0x000153EC
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

		// Token: 0x06000524 RID: 1316 RVA: 0x00017264 File Offset: 0x00015464
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

		// Token: 0x06000525 RID: 1317 RVA: 0x000172E4 File Offset: 0x000154E4
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

		// Token: 0x06000526 RID: 1318 RVA: 0x000173D4 File Offset: 0x000155D4
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

		// Token: 0x06000527 RID: 1319 RVA: 0x0001743C File Offset: 0x0001563C
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

		// Token: 0x06000528 RID: 1320 RVA: 0x000174B0 File Offset: 0x000156B0
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

		// Token: 0x06000529 RID: 1321 RVA: 0x0001755C File Offset: 0x0001575C
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

		// Token: 0x0600052A RID: 1322 RVA: 0x000175C0 File Offset: 0x000157C0
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

		// Token: 0x0600052B RID: 1323 RVA: 0x000175F0 File Offset: 0x000157F0
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

		// Token: 0x0600052C RID: 1324 RVA: 0x000176A0 File Offset: 0x000158A0
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

		// Token: 0x0600052D RID: 1325 RVA: 0x00017710 File Offset: 0x00015910
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

		// Token: 0x0600052E RID: 1326 RVA: 0x000177CC File Offset: 0x000159CC
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

		// Token: 0x0600052F RID: 1327 RVA: 0x000178FC File Offset: 0x00015AFC
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

		// Token: 0x06000530 RID: 1328 RVA: 0x00017958 File Offset: 0x00015B58
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
			else if (typeReference.etype == ElementType.Object || typeReference.etype == ElementType.None || typeReference.etype == ElementType.Class || typeReference.etype == ElementType.Array)
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

		// Token: 0x06000531 RID: 1329 RVA: 0x00017AB8 File Offset: 0x00015CB8
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

		// Token: 0x06000532 RID: 1330 RVA: 0x00017BA0 File Offset: 0x00015DA0
		public string ReadUTF8StringBlob(uint signature)
		{
			return this.ReadStringBlob(signature, Encoding.UTF8);
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x00017BAE File Offset: 0x00015DAE
		private string ReadUnicodeStringBlob(uint signature)
		{
			return this.ReadStringBlob(signature, Encoding.Unicode);
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x00017BBC File Offset: 0x00015DBC
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

		// Token: 0x06000535 RID: 1333 RVA: 0x00017BE8 File Offset: 0x00015DE8
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

		// Token: 0x06000536 RID: 1334 RVA: 0x00017D4C File Offset: 0x00015F4C
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

		// Token: 0x06000537 RID: 1335 RVA: 0x00017DAC File Offset: 0x00015FAC
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

		// Token: 0x06000538 RID: 1336 RVA: 0x00017DD8 File Offset: 0x00015FD8
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

		// Token: 0x06000539 RID: 1337 RVA: 0x00017E64 File Offset: 0x00016064
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

		// Token: 0x0400023B RID: 571
		internal readonly Image image;

		// Token: 0x0400023C RID: 572
		internal readonly ModuleDefinition module;

		// Token: 0x0400023D RID: 573
		internal readonly MetadataSystem metadata;

		// Token: 0x0400023E RID: 574
		internal CodeReader code;

		// Token: 0x0400023F RID: 575
		internal IGenericContext context;

		// Token: 0x04000240 RID: 576
		private readonly MetadataReader metadata_reader;
	}
}
