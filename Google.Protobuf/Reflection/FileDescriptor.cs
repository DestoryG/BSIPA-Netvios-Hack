using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;

namespace Google.Protobuf.Reflection
{
	// Token: 0x02000072 RID: 114
	public sealed class FileDescriptor : IDescriptor
	{
		// Token: 0x06000794 RID: 1940 RVA: 0x0001B5E5 File Offset: 0x000197E5
		static FileDescriptor()
		{
			FileDescriptor.ForceReflectionInitialization<Syntax>();
			FileDescriptor.ForceReflectionInitialization<NullValue>();
			FileDescriptor.ForceReflectionInitialization<Field.Types.Cardinality>();
			FileDescriptor.ForceReflectionInitialization<Field.Types.Kind>();
			FileDescriptor.ForceReflectionInitialization<Value.KindOneofCase>();
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x0001B600 File Offset: 0x00019800
		private FileDescriptor(ByteString descriptorData, FileDescriptorProto proto, IEnumerable<FileDescriptor> dependencies, DescriptorPool pool, bool allowUnknownDependencies, GeneratedClrTypeInfo generatedCodeInfo)
		{
			FileDescriptor <>4__this = this;
			this.SerializedData = descriptorData;
			this.DescriptorPool = pool;
			this.Proto = proto;
			this.Dependencies = new ReadOnlyCollection<FileDescriptor>(dependencies.ToList<FileDescriptor>());
			this.PublicDependencies = FileDescriptor.DeterminePublicDependencies(this, proto, dependencies, allowUnknownDependencies);
			pool.AddPackage(this.Package, this);
			this.MessageTypes = DescriptorUtil.ConvertAndMakeReadOnly<DescriptorProto, MessageDescriptor>(proto.MessageType, delegate(DescriptorProto message, int index)
			{
				FileDescriptor <>4__this3 = <>4__this;
				MessageDescriptor messageDescriptor = null;
				GeneratedClrTypeInfo generatedCodeInfo3 = generatedCodeInfo;
				return new MessageDescriptor(message, <>4__this3, messageDescriptor, index, (generatedCodeInfo3 != null) ? generatedCodeInfo3.NestedTypes[index] : null);
			});
			this.EnumTypes = DescriptorUtil.ConvertAndMakeReadOnly<EnumDescriptorProto, EnumDescriptor>(proto.EnumType, delegate(EnumDescriptorProto enumType, int index)
			{
				FileDescriptor <>4__this2 = <>4__this;
				MessageDescriptor messageDescriptor2 = null;
				GeneratedClrTypeInfo generatedCodeInfo4 = generatedCodeInfo;
				return new EnumDescriptor(enumType, <>4__this2, messageDescriptor2, index, (generatedCodeInfo4 != null) ? generatedCodeInfo4.NestedEnums[index] : null);
			});
			this.Services = DescriptorUtil.ConvertAndMakeReadOnly<ServiceDescriptorProto, ServiceDescriptor>(proto.Service, (ServiceDescriptorProto service, int index) => new ServiceDescriptor(service, <>4__this, index));
			GeneratedClrTypeInfo generatedCodeInfo2 = generatedCodeInfo;
			this.Extensions = new ExtensionCollection(this, (generatedCodeInfo2 != null) ? generatedCodeInfo2.Extensions : null);
			this.declarations = new Lazy<Dictionary<IDescriptor, DescriptorDeclaration>>(new Func<Dictionary<IDescriptor, DescriptorDeclaration>>(this.CreateDeclarationMap), LazyThreadSafetyMode.ExecutionAndPublication);
			if (!proto.HasSyntax || proto.Syntax == "proto2")
			{
				this.Syntax = Syntax.Proto2;
				return;
			}
			if (proto.Syntax == "proto3")
			{
				this.Syntax = Syntax.Proto3;
				return;
			}
			this.Syntax = Syntax.Unknown;
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x0001B740 File Offset: 0x00019940
		private Dictionary<IDescriptor, DescriptorDeclaration> CreateDeclarationMap()
		{
			Dictionary<IDescriptor, DescriptorDeclaration> dictionary = new Dictionary<IDescriptor, DescriptorDeclaration>();
			SourceCodeInfo sourceCodeInfo = this.Proto.SourceCodeInfo;
			IEnumerable<SourceCodeInfo.Types.Location> enumerable = ((sourceCodeInfo != null) ? sourceCodeInfo.Location : null);
			foreach (SourceCodeInfo.Types.Location location in (enumerable ?? Enumerable.Empty<SourceCodeInfo.Types.Location>()))
			{
				IDescriptor descriptor = this.FindDescriptorForPath(location.Path);
				if (descriptor != null)
				{
					dictionary[descriptor] = DescriptorDeclaration.FromProto(descriptor, location);
				}
			}
			return dictionary;
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x0001B7CC File Offset: 0x000199CC
		private IDescriptor FindDescriptorForPath(IList<int> path)
		{
			if (path.Count == 0 || (path.Count & 1) != 0)
			{
				return null;
			}
			IReadOnlyList<DescriptorBase> nestedDescriptorListForField = this.GetNestedDescriptorListForField(path[0]);
			DescriptorBase descriptorBase = this.GetDescriptorFromList(nestedDescriptorListForField, path[1]);
			int num = 2;
			while (descriptorBase != null && num < path.Count)
			{
				IReadOnlyList<DescriptorBase> nestedDescriptorListForField2 = descriptorBase.GetNestedDescriptorListForField(path[num]);
				descriptorBase = this.GetDescriptorFromList(nestedDescriptorListForField2, path[num + 1]);
				num += 2;
			}
			return descriptorBase;
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0001B83E File Offset: 0x00019A3E
		private DescriptorBase GetDescriptorFromList(IReadOnlyList<DescriptorBase> list, int index)
		{
			if (list == null)
			{
				return null;
			}
			if (index < 0 || index >= list.Count)
			{
				throw new InvalidProtocolBufferException("Invalid descriptor location path: index out of range");
			}
			return list[index];
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x0001B864 File Offset: 0x00019A64
		private IReadOnlyList<DescriptorBase> GetNestedDescriptorListForField(int fieldNumber)
		{
			switch (fieldNumber)
			{
			case 4:
				return (IReadOnlyList<DescriptorBase>)this.MessageTypes;
			case 5:
				return (IReadOnlyList<DescriptorBase>)this.EnumTypes;
			case 6:
				return (IReadOnlyList<DescriptorBase>)this.Services;
			default:
				return null;
			}
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x0001B8A4 File Offset: 0x00019AA4
		internal DescriptorDeclaration GetDeclaration(IDescriptor descriptor)
		{
			DescriptorDeclaration descriptorDeclaration;
			this.declarations.Value.TryGetValue(descriptor, out descriptorDeclaration);
			return descriptorDeclaration;
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x0001B8C6 File Offset: 0x00019AC6
		internal string ComputeFullName(MessageDescriptor parent, string name)
		{
			if (parent != null)
			{
				return parent.FullName + "." + name;
			}
			if (this.Package.Length > 0)
			{
				return this.Package + "." + name;
			}
			return name;
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x0001B900 File Offset: 0x00019B00
		private static IList<FileDescriptor> DeterminePublicDependencies(FileDescriptor @this, FileDescriptorProto proto, IEnumerable<FileDescriptor> dependencies, bool allowUnknownDependencies)
		{
			Dictionary<string, FileDescriptor> dictionary = dependencies.ToDictionary((FileDescriptor file) => file.Name);
			List<FileDescriptor> list = new List<FileDescriptor>();
			for (int i = 0; i < proto.PublicDependency.Count; i++)
			{
				int num = proto.PublicDependency[i];
				if (num < 0 || num >= proto.Dependency.Count)
				{
					throw new DescriptorValidationException(@this, "Invalid public dependency index.");
				}
				string text = proto.Dependency[num];
				FileDescriptor fileDescriptor;
				if (!dictionary.TryGetValue(text, out fileDescriptor))
				{
					if (!allowUnknownDependencies)
					{
						throw new DescriptorValidationException(@this, "Invalid public dependency: " + text);
					}
				}
				else
				{
					list.Add(fileDescriptor);
				}
			}
			return new ReadOnlyCollection<FileDescriptor>(list);
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x0600079D RID: 1949 RVA: 0x0001B9B8 File Offset: 0x00019BB8
		internal FileDescriptorProto Proto { get; }

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x0600079E RID: 1950 RVA: 0x0001B9C0 File Offset: 0x00019BC0
		public Syntax Syntax { get; }

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x0600079F RID: 1951 RVA: 0x0001B9C8 File Offset: 0x00019BC8
		public string Name
		{
			get
			{
				return this.Proto.Name;
			}
		}

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x060007A0 RID: 1952 RVA: 0x0001B9D5 File Offset: 0x00019BD5
		public string Package
		{
			get
			{
				return this.Proto.Package;
			}
		}

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x060007A1 RID: 1953 RVA: 0x0001B9E2 File Offset: 0x00019BE2
		public IList<MessageDescriptor> MessageTypes { get; }

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x060007A2 RID: 1954 RVA: 0x0001B9EA File Offset: 0x00019BEA
		public IList<EnumDescriptor> EnumTypes { get; }

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x060007A3 RID: 1955 RVA: 0x0001B9F2 File Offset: 0x00019BF2
		public IList<ServiceDescriptor> Services { get; }

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x060007A4 RID: 1956 RVA: 0x0001B9FA File Offset: 0x00019BFA
		public ExtensionCollection Extensions { get; }

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x060007A5 RID: 1957 RVA: 0x0001BA02 File Offset: 0x00019C02
		public IList<FileDescriptor> Dependencies { get; }

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x060007A6 RID: 1958 RVA: 0x0001BA0A File Offset: 0x00019C0A
		public IList<FileDescriptor> PublicDependencies { get; }

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x060007A7 RID: 1959 RVA: 0x0001BA12 File Offset: 0x00019C12
		public ByteString SerializedData { get; }

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x060007A8 RID: 1960 RVA: 0x0001BA1A File Offset: 0x00019C1A
		string IDescriptor.FullName
		{
			get
			{
				return this.Name;
			}
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x060007A9 RID: 1961 RVA: 0x0001BA22 File Offset: 0x00019C22
		FileDescriptor IDescriptor.File
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x060007AA RID: 1962 RVA: 0x0001BA25 File Offset: 0x00019C25
		internal DescriptorPool DescriptorPool { get; }

		// Token: 0x060007AB RID: 1963 RVA: 0x0001BA30 File Offset: 0x00019C30
		public T FindTypeByName<T>(string name) where T : class, IDescriptor
		{
			if (name.IndexOf('.') != -1)
			{
				return default(T);
			}
			if (this.Package.Length > 0)
			{
				name = this.Package + "." + name;
			}
			T t = this.DescriptorPool.FindSymbol<T>(name);
			if (t != null && t.File == this)
			{
				return t;
			}
			return default(T);
		}

		// Token: 0x060007AC RID: 1964 RVA: 0x0001BAA4 File Offset: 0x00019CA4
		private static FileDescriptor BuildFrom(ByteString descriptorData, FileDescriptorProto proto, FileDescriptor[] dependencies, bool allowUnknownDependencies, GeneratedClrTypeInfo generatedCodeInfo)
		{
			if (dependencies == null)
			{
				dependencies = new FileDescriptor[0];
			}
			DescriptorPool descriptorPool = new DescriptorPool(dependencies);
			FileDescriptor fileDescriptor = new FileDescriptor(descriptorData, proto, dependencies, descriptorPool, allowUnknownDependencies, generatedCodeInfo);
			if (dependencies.Length != proto.Dependency.Count)
			{
				throw new DescriptorValidationException(fileDescriptor, "Dependencies passed to FileDescriptor.BuildFrom() don't match those listed in the FileDescriptorProto.");
			}
			fileDescriptor.CrossLink();
			return fileDescriptor;
		}

		// Token: 0x060007AD RID: 1965 RVA: 0x0001BAF4 File Offset: 0x00019CF4
		private void CrossLink()
		{
			foreach (MessageDescriptor messageDescriptor in this.MessageTypes)
			{
				messageDescriptor.CrossLink();
			}
			foreach (ServiceDescriptor serviceDescriptor in this.Services)
			{
				serviceDescriptor.CrossLink();
			}
			this.Extensions.CrossLink();
		}

		// Token: 0x060007AE RID: 1966 RVA: 0x0001BB84 File Offset: 0x00019D84
		public static FileDescriptor FromGeneratedCode(byte[] descriptorData, FileDescriptor[] dependencies, GeneratedClrTypeInfo generatedCodeInfo)
		{
			ExtensionRegistry extensionRegistry = new ExtensionRegistry();
			extensionRegistry.AddRange(FileDescriptor.GetAllExtensions(dependencies, generatedCodeInfo));
			FileDescriptorProto fileDescriptorProto;
			try
			{
				fileDescriptorProto = FileDescriptorProto.Parser.WithExtensionRegistry(extensionRegistry).ParseFrom(descriptorData);
			}
			catch (InvalidProtocolBufferException ex)
			{
				throw new ArgumentException("Failed to parse protocol buffer descriptor for generated code.", ex);
			}
			FileDescriptor fileDescriptor;
			try
			{
				fileDescriptor = FileDescriptor.BuildFrom(ByteString.CopyFrom(descriptorData), fileDescriptorProto, dependencies, true, generatedCodeInfo);
			}
			catch (DescriptorValidationException ex2)
			{
				throw new ArgumentException("Invalid embedded descriptor for \"" + fileDescriptorProto.Name + "\".", ex2);
			}
			return fileDescriptor;
		}

		// Token: 0x060007AF RID: 1967 RVA: 0x0001BC14 File Offset: 0x00019E14
		private static IEnumerable<Extension> GetAllExtensions(FileDescriptor[] dependencies, GeneratedClrTypeInfo generatedInfo)
		{
			return dependencies.SelectMany(new Func<FileDescriptor, IEnumerable<Extension>>(FileDescriptor.GetAllDependedExtensions)).Distinct(ExtensionRegistry.ExtensionComparer.Instance).Concat(FileDescriptor.GetAllGeneratedExtensions(generatedInfo));
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x0001BC40 File Offset: 0x00019E40
		private static IEnumerable<Extension> GetAllGeneratedExtensions(GeneratedClrTypeInfo generated)
		{
			return generated.Extensions.Concat(generated.NestedTypes.Where((GeneratedClrTypeInfo t) => t != null).SelectMany(new Func<GeneratedClrTypeInfo, IEnumerable<Extension>>(FileDescriptor.GetAllGeneratedExtensions)));
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x0001BC94 File Offset: 0x00019E94
		private static IEnumerable<Extension> GetAllDependedExtensions(FileDescriptor descriptor)
		{
			return (from s in descriptor.Extensions.UnorderedExtensions
				select s.Extension into e
				where e != null
				select e).Concat(descriptor.Dependencies.Concat(descriptor.PublicDependencies).SelectMany(new Func<FileDescriptor, IEnumerable<Extension>>(FileDescriptor.GetAllDependedExtensions))).Concat(descriptor.MessageTypes.SelectMany(new Func<MessageDescriptor, IEnumerable<Extension>>(FileDescriptor.GetAllDependedExtensionsFromMessage)));
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x0001BD38 File Offset: 0x00019F38
		private static IEnumerable<Extension> GetAllDependedExtensionsFromMessage(MessageDescriptor descriptor)
		{
			return (from s in descriptor.Extensions.UnorderedExtensions
				select s.Extension into e
				where e != null
				select e).Concat(descriptor.NestedTypes.SelectMany(new Func<MessageDescriptor, IEnumerable<Extension>>(FileDescriptor.GetAllDependedExtensionsFromMessage)));
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x0001BDB4 File Offset: 0x00019FB4
		public static IReadOnlyList<FileDescriptor> BuildFromByteStrings(IEnumerable<ByteString> descriptorData)
		{
			ProtoPreconditions.CheckNotNull<IEnumerable<ByteString>>(descriptorData, "descriptorData");
			List<FileDescriptor> list = new List<FileDescriptor>();
			Dictionary<string, FileDescriptor> dictionary = new Dictionary<string, FileDescriptor>();
			foreach (ByteString byteString in descriptorData)
			{
				FileDescriptorProto fileDescriptorProto = FileDescriptorProto.Parser.ParseFrom(byteString);
				List<FileDescriptor> list2 = new List<FileDescriptor>();
				foreach (string text in fileDescriptorProto.Dependency)
				{
					FileDescriptor fileDescriptor;
					if (!dictionary.TryGetValue(text, out fileDescriptor))
					{
						throw new ArgumentException("Dependency missing: " + text);
					}
					list2.Add(fileDescriptor);
				}
				DescriptorPool descriptorPool = new DescriptorPool(list2);
				FileDescriptor fileDescriptor2 = new FileDescriptor(byteString, fileDescriptorProto, list2, descriptorPool, false, null);
				fileDescriptor2.CrossLink();
				list.Add(fileDescriptor2);
				if (dictionary.ContainsKey(fileDescriptor2.Name))
				{
					throw new ArgumentException("Duplicate descriptor name: " + fileDescriptor2.Name);
				}
				dictionary.Add(fileDescriptor2.Name, fileDescriptor2);
			}
			return new ReadOnlyCollection<FileDescriptor>(list);
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x0001BEF0 File Offset: 0x0001A0F0
		public override string ToString()
		{
			return "FileDescriptor for " + this.Name;
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x060007B5 RID: 1973 RVA: 0x0001BF02 File Offset: 0x0001A102
		public static FileDescriptor DescriptorProtoFileDescriptor
		{
			get
			{
				return DescriptorReflection.Descriptor;
			}
		}

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x060007B6 RID: 1974 RVA: 0x0001BF09 File Offset: 0x0001A109
		[Obsolete("CustomOptions are obsolete. Use the GetOptions() method.")]
		public CustomOptions CustomOptions
		{
			get
			{
				FileOptions options = this.Proto.Options;
				IDictionary<int, IExtensionValue> dictionary;
				if (options == null)
				{
					dictionary = null;
				}
				else
				{
					ExtensionSet<FileOptions> extensions = options._extensions;
					dictionary = ((extensions != null) ? extensions.ValuesByNumber : null);
				}
				return new CustomOptions(dictionary);
			}
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0001BF33 File Offset: 0x0001A133
		public FileOptions GetOptions()
		{
			FileOptions options = this.Proto.Options;
			if (options == null)
			{
				return null;
			}
			return options.Clone();
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x0001BF4C File Offset: 0x0001A14C
		[Obsolete("GetOption is obsolete. Use the GetOptions() method.")]
		public T GetOption<T>(Extension<FileOptions, T> extension)
		{
			T extension2 = this.Proto.Options.GetExtension<T>(extension);
			if (!(extension2 is IDeepCloneable<T>))
			{
				return extension2;
			}
			return (extension2 as IDeepCloneable<T>).Clone();
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x0001BF8A File Offset: 0x0001A18A
		[Obsolete("GetOption is obsolete. Use the GetOptions() method.")]
		public RepeatedField<T> GetOption<T>(RepeatedExtension<FileOptions, T> extension)
		{
			return this.Proto.Options.GetExtension<T>(extension).Clone();
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x0001BFA2 File Offset: 0x0001A1A2
		public static void ForceReflectionInitialization<T>()
		{
			ReflectionUtil.ForceInitialize<T>();
		}

		// Token: 0x0400030B RID: 779
		private readonly Lazy<Dictionary<IDescriptor, DescriptorDeclaration>> declarations;
	}
}
