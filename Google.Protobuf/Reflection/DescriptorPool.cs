using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Google.Protobuf.Reflection
{
	// Token: 0x02000067 RID: 103
	internal sealed class DescriptorPool
	{
		// Token: 0x0600073F RID: 1855 RVA: 0x0001A354 File Offset: 0x00018554
		internal DescriptorPool(IEnumerable<FileDescriptor> dependencyFiles)
		{
			this.dependencies = new HashSet<FileDescriptor>();
			foreach (FileDescriptor fileDescriptor in dependencyFiles)
			{
				this.dependencies.Add(fileDescriptor);
				this.ImportPublicDependencies(fileDescriptor);
			}
			foreach (FileDescriptor fileDescriptor2 in dependencyFiles)
			{
				this.AddPackage(fileDescriptor2.Package, fileDescriptor2);
			}
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x0001A418 File Offset: 0x00018618
		private void ImportPublicDependencies(FileDescriptor file)
		{
			foreach (FileDescriptor fileDescriptor in file.PublicDependencies)
			{
				if (this.dependencies.Add(fileDescriptor))
				{
					this.ImportPublicDependencies(fileDescriptor);
				}
			}
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x0001A474 File Offset: 0x00018674
		internal T FindSymbol<T>(string fullName) where T : class
		{
			IDescriptor descriptor;
			this.descriptorsByName.TryGetValue(fullName, out descriptor);
			T t = descriptor as T;
			if (t != null)
			{
				return t;
			}
			foreach (FileDescriptor fileDescriptor in this.dependencies)
			{
				fileDescriptor.DescriptorPool.descriptorsByName.TryGetValue(fullName, out descriptor);
				t = descriptor as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06000742 RID: 1858 RVA: 0x0001A51C File Offset: 0x0001871C
		internal void AddPackage(string fullName, FileDescriptor file)
		{
			int num = fullName.LastIndexOf('.');
			string text;
			if (num != -1)
			{
				this.AddPackage(fullName.Substring(0, num), file);
				text = fullName.Substring(num + 1);
			}
			else
			{
				text = fullName;
			}
			IDescriptor descriptor;
			if (this.descriptorsByName.TryGetValue(fullName, out descriptor) && !(descriptor is PackageDescriptor))
			{
				throw new DescriptorValidationException(file, string.Concat(new string[]
				{
					"\"",
					text,
					"\" is already defined (as something other than a package) in file \"",
					descriptor.File.Name,
					"\"."
				}));
			}
			this.descriptorsByName[fullName] = new PackageDescriptor(text, fullName, file);
		}

		// Token: 0x06000743 RID: 1859 RVA: 0x0001A5BC File Offset: 0x000187BC
		internal void AddSymbol(IDescriptor descriptor)
		{
			DescriptorPool.ValidateSymbolName(descriptor);
			string fullName = descriptor.FullName;
			IDescriptor descriptor2;
			if (this.descriptorsByName.TryGetValue(fullName, out descriptor2))
			{
				int num = fullName.LastIndexOf('.');
				string text;
				if (descriptor.File == descriptor2.File)
				{
					if (num == -1)
					{
						text = "\"" + fullName + "\" is already defined.";
					}
					else
					{
						text = string.Concat(new string[]
						{
							"\"",
							fullName.Substring(num + 1),
							"\" is already defined in \"",
							fullName.Substring(0, num),
							"\"."
						});
					}
				}
				else
				{
					text = string.Concat(new string[]
					{
						"\"",
						fullName,
						"\" is already defined in file \"",
						descriptor2.File.Name,
						"\"."
					});
				}
				throw new DescriptorValidationException(descriptor, text);
			}
			this.descriptorsByName[fullName] = descriptor;
		}

		// Token: 0x06000744 RID: 1860 RVA: 0x0001A6A0 File Offset: 0x000188A0
		private static void ValidateSymbolName(IDescriptor descriptor)
		{
			if (descriptor.Name == "")
			{
				throw new DescriptorValidationException(descriptor, "Missing name.");
			}
			if (!DescriptorPool.ValidationRegex.IsMatch(descriptor.Name))
			{
				throw new DescriptorValidationException(descriptor, "\"" + descriptor.Name + "\" is not a valid identifier.");
			}
		}

		// Token: 0x06000745 RID: 1861 RVA: 0x0001A6FC File Offset: 0x000188FC
		internal FieldDescriptor FindFieldByNumber(MessageDescriptor messageDescriptor, int number)
		{
			FieldDescriptor fieldDescriptor;
			this.fieldsByNumber.TryGetValue(new ObjectIntPair<IDescriptor>(messageDescriptor, number), out fieldDescriptor);
			return fieldDescriptor;
		}

		// Token: 0x06000746 RID: 1862 RVA: 0x0001A720 File Offset: 0x00018920
		internal EnumValueDescriptor FindEnumValueByNumber(EnumDescriptor enumDescriptor, int number)
		{
			EnumValueDescriptor enumValueDescriptor;
			this.enumValuesByNumber.TryGetValue(new ObjectIntPair<IDescriptor>(enumDescriptor, number), out enumValueDescriptor);
			return enumValueDescriptor;
		}

		// Token: 0x06000747 RID: 1863 RVA: 0x0001A744 File Offset: 0x00018944
		internal void AddFieldByNumber(FieldDescriptor field)
		{
			ObjectIntPair<IDescriptor> objectIntPair = new ObjectIntPair<IDescriptor>(field.Proto.HasExtendee ? field.ExtendeeType : field.ContainingType, field.FieldNumber);
			FieldDescriptor fieldDescriptor;
			if (this.fieldsByNumber.TryGetValue(objectIntPair, out fieldDescriptor))
			{
				throw new DescriptorValidationException(field, string.Concat(new string[]
				{
					"Field number ",
					field.FieldNumber.ToString(),
					"has already been used in \"",
					field.ContainingType.FullName,
					"\" by field \"",
					fieldDescriptor.Name,
					"\"."
				}));
			}
			this.fieldsByNumber[objectIntPair] = field;
		}

		// Token: 0x06000748 RID: 1864 RVA: 0x0001A7F0 File Offset: 0x000189F0
		internal void AddEnumValueByNumber(EnumValueDescriptor enumValue)
		{
			ObjectIntPair<IDescriptor> objectIntPair = new ObjectIntPair<IDescriptor>(enumValue.EnumDescriptor, enumValue.Number);
			if (!this.enumValuesByNumber.ContainsKey(objectIntPair))
			{
				this.enumValuesByNumber[objectIntPair] = enumValue;
			}
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x0001A82C File Offset: 0x00018A2C
		internal IDescriptor LookupSymbol(string name, IDescriptor relativeTo)
		{
			IDescriptor descriptor;
			if (name.StartsWith("."))
			{
				descriptor = this.FindSymbol<IDescriptor>(name.Substring(1));
			}
			else
			{
				int num = name.IndexOf('.');
				string text = ((num == -1) ? name : name.Substring(0, num));
				StringBuilder stringBuilder = new StringBuilder(relativeTo.FullName);
				for (;;)
				{
					int num2 = stringBuilder.ToString().LastIndexOf(".");
					if (num2 == -1)
					{
						break;
					}
					stringBuilder.Length = num2 + 1;
					stringBuilder.Append(text);
					descriptor = this.FindSymbol<IDescriptor>(stringBuilder.ToString());
					if (descriptor != null)
					{
						goto Block_4;
					}
					stringBuilder.Length = num2;
				}
				descriptor = this.FindSymbol<IDescriptor>(name);
				goto IL_00B7;
				Block_4:
				if (num != -1)
				{
					int num2;
					stringBuilder.Length = num2 + 1;
					stringBuilder.Append(name);
					descriptor = this.FindSymbol<IDescriptor>(stringBuilder.ToString());
				}
			}
			IL_00B7:
			if (descriptor == null)
			{
				throw new DescriptorValidationException(relativeTo, "\"" + name + "\" is not defined.");
			}
			return descriptor;
		}

		// Token: 0x040002D4 RID: 724
		private readonly IDictionary<string, IDescriptor> descriptorsByName = new Dictionary<string, IDescriptor>();

		// Token: 0x040002D5 RID: 725
		private readonly IDictionary<ObjectIntPair<IDescriptor>, FieldDescriptor> fieldsByNumber = new Dictionary<ObjectIntPair<IDescriptor>, FieldDescriptor>();

		// Token: 0x040002D6 RID: 726
		private readonly IDictionary<ObjectIntPair<IDescriptor>, EnumValueDescriptor> enumValuesByNumber = new Dictionary<ObjectIntPair<IDescriptor>, EnumValueDescriptor>();

		// Token: 0x040002D7 RID: 727
		private readonly HashSet<FileDescriptor> dependencies;

		// Token: 0x040002D8 RID: 728
		private static readonly Regex ValidationRegex = new Regex("^[_A-Za-z][_A-Za-z0-9]*$", FrameworkPortability.CompiledRegexWhereAvailable);
	}
}
