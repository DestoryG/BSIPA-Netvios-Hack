using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Text.RegularExpressions
{
	// Token: 0x02000693 RID: 1683
	internal class RegexTypeCompiler : RegexCompiler
	{
		// Token: 0x06003E98 RID: 16024 RVA: 0x0010428C File Offset: 0x0010248C
		internal RegexTypeCompiler(AssemblyName an, CustomAttributeBuilder[] attribs, string resourceFile)
		{
			new ReflectionPermission(PermissionState.Unrestricted).Assert();
			try
			{
				List<CustomAttributeBuilder> list = new List<CustomAttributeBuilder>();
				ConstructorInfo constructor = typeof(SecurityTransparentAttribute).GetConstructor(Type.EmptyTypes);
				CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(constructor, new object[0]);
				list.Add(customAttributeBuilder);
				ConstructorInfo constructor2 = typeof(SecurityRulesAttribute).GetConstructor(new Type[] { typeof(SecurityRuleSet) });
				CustomAttributeBuilder customAttributeBuilder2 = new CustomAttributeBuilder(constructor2, new object[] { SecurityRuleSet.Level2 });
				list.Add(customAttributeBuilder2);
				this._assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.RunAndSave, list);
				this._module = this._assembly.DefineDynamicModule(an.Name + ".dll");
				if (attribs != null)
				{
					for (int i = 0; i < attribs.Length; i++)
					{
						this._assembly.SetCustomAttribute(attribs[i]);
					}
				}
				if (resourceFile != null)
				{
					this._assembly.DefineUnmanagedResource(resourceFile);
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
		}

		// Token: 0x06003E99 RID: 16025 RVA: 0x0010439C File Offset: 0x0010259C
		internal Type FactoryTypeFromCode(RegexCode code, RegexOptions options, string typeprefix)
		{
			this._code = code;
			this._codes = code._codes;
			this._strings = code._strings;
			this._fcPrefix = code._fcPrefix;
			this._bmPrefix = code._bmPrefix;
			this._anchors = code._anchors;
			this._trackcount = code._trackcount;
			this._options = options;
			string text = Interlocked.Increment(ref RegexTypeCompiler._typeCount).ToString(CultureInfo.InvariantCulture);
			string text2 = typeprefix + "Runner" + text;
			string text3 = typeprefix + "Factory" + text;
			this.DefineType(text2, false, typeof(RegexRunner));
			this.DefineMethod("Go", null);
			base.GenerateGo();
			this.BakeMethod();
			this.DefineMethod("FindFirstChar", typeof(bool));
			base.GenerateFindFirstChar();
			this.BakeMethod();
			this.DefineMethod("InitTrackCount", null);
			base.GenerateInitTrackCount();
			this.BakeMethod();
			Type type = this.BakeType();
			this.DefineType(text3, false, typeof(RegexRunnerFactory));
			this.DefineMethod("CreateInstance", typeof(RegexRunner));
			this.GenerateCreateInstance(type);
			this.BakeMethod();
			return this.BakeType();
		}

		// Token: 0x06003E9A RID: 16026 RVA: 0x001044DC File Offset: 0x001026DC
		internal void GenerateRegexType(string pattern, RegexOptions opts, string name, bool ispublic, RegexCode code, RegexTree tree, Type factory, TimeSpan matchTimeout)
		{
			FieldInfo fieldInfo = this.RegexField("pattern");
			FieldInfo fieldInfo2 = this.RegexField("roptions");
			FieldInfo fieldInfo3 = this.RegexField("factory");
			FieldInfo fieldInfo4 = this.RegexField("caps");
			FieldInfo fieldInfo5 = this.RegexField("capnames");
			FieldInfo fieldInfo6 = this.RegexField("capslist");
			FieldInfo fieldInfo7 = this.RegexField("capsize");
			FieldInfo fieldInfo8 = this.RegexField("internalMatchTimeout");
			Type[] array = new Type[0];
			this.DefineType(name, ispublic, typeof(Regex));
			this._methbuilder = null;
			MethodAttributes methodAttributes = MethodAttributes.Public;
			ConstructorBuilder constructorBuilder = this._typebuilder.DefineConstructor(methodAttributes, CallingConventions.Standard, array);
			this._ilg = constructorBuilder.GetILGenerator();
			base.Ldthis();
			this._ilg.Emit(OpCodes.Call, typeof(Regex).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[0], new ParameterModifier[0]));
			base.Ldthis();
			base.Ldstr(pattern);
			base.Stfld(fieldInfo);
			base.Ldthis();
			base.Ldc((int)opts);
			base.Stfld(fieldInfo2);
			base.Ldthis();
			base.LdcI8(matchTimeout.Ticks);
			base.Call(typeof(TimeSpan).GetMethod("FromTicks", BindingFlags.Static | BindingFlags.Public));
			base.Stfld(fieldInfo8);
			base.Ldthis();
			base.Newobj(factory.GetConstructor(array));
			base.Stfld(fieldInfo3);
			if (code._caps != null)
			{
				this.GenerateCreateHashtable(fieldInfo4, code._caps);
			}
			if (tree._capnames != null)
			{
				this.GenerateCreateHashtable(fieldInfo5, tree._capnames);
			}
			if (tree._capslist != null)
			{
				base.Ldthis();
				base.Ldc(tree._capslist.Length);
				this._ilg.Emit(OpCodes.Newarr, typeof(string));
				base.Stfld(fieldInfo6);
				for (int i = 0; i < tree._capslist.Length; i++)
				{
					base.Ldthisfld(fieldInfo6);
					base.Ldc(i);
					base.Ldstr(tree._capslist[i]);
					this._ilg.Emit(OpCodes.Stelem_Ref);
				}
			}
			base.Ldthis();
			base.Ldc(code._capsize);
			base.Stfld(fieldInfo7);
			base.Ldthis();
			base.Call(typeof(Regex).GetMethod("InitializeReferences", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));
			base.Ret();
			this._methbuilder = null;
			methodAttributes = MethodAttributes.Public;
			ConstructorBuilder constructorBuilder2 = this._typebuilder.DefineConstructor(methodAttributes, CallingConventions.Standard, new Type[] { typeof(TimeSpan) });
			this._ilg = constructorBuilder2.GetILGenerator();
			base.Ldthis();
			this._ilg.Emit(OpCodes.Call, constructorBuilder);
			this._ilg.Emit(OpCodes.Ldarg_1);
			base.Call(typeof(Regex).GetMethod("ValidateMatchTimeout", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));
			base.Ldthis();
			this._ilg.Emit(OpCodes.Ldarg_1);
			base.Stfld(fieldInfo8);
			base.Ret();
			this._typebuilder.CreateType();
			this._ilg = null;
			this._typebuilder = null;
		}

		// Token: 0x06003E9B RID: 16027 RVA: 0x001047FC File Offset: 0x001029FC
		internal void GenerateCreateHashtable(FieldInfo field, Hashtable ht)
		{
			MethodInfo method = typeof(Hashtable).GetMethod("Add", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			base.Ldthis();
			base.Newobj(typeof(Hashtable).GetConstructor(new Type[0]));
			base.Stfld(field);
			IDictionaryEnumerator enumerator = ht.GetEnumerator();
			while (enumerator.MoveNext())
			{
				base.Ldthisfld(field);
				if (enumerator.Key is int)
				{
					base.Ldc((int)enumerator.Key);
					this._ilg.Emit(OpCodes.Box, typeof(int));
				}
				else
				{
					base.Ldstr((string)enumerator.Key);
				}
				base.Ldc((int)enumerator.Value);
				this._ilg.Emit(OpCodes.Box, typeof(int));
				base.Callvirt(method);
			}
		}

		// Token: 0x06003E9C RID: 16028 RVA: 0x001048E3 File Offset: 0x00102AE3
		private FieldInfo RegexField(string fieldname)
		{
			return typeof(Regex).GetField(fieldname, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x06003E9D RID: 16029 RVA: 0x001048F7 File Offset: 0x00102AF7
		internal void Save()
		{
			this._assembly.Save(this._assembly.GetName().Name + ".dll");
		}

		// Token: 0x06003E9E RID: 16030 RVA: 0x0010491E File Offset: 0x00102B1E
		internal void GenerateCreateInstance(Type newtype)
		{
			base.Newobj(newtype.GetConstructor(new Type[0]));
			base.Ret();
		}

		// Token: 0x06003E9F RID: 16031 RVA: 0x00104938 File Offset: 0x00102B38
		internal void DefineType(string typename, bool ispublic, Type inheritfromclass)
		{
			if (ispublic)
			{
				this._typebuilder = this._module.DefineType(typename, TypeAttributes.Public, inheritfromclass);
				return;
			}
			this._typebuilder = this._module.DefineType(typename, TypeAttributes.NotPublic, inheritfromclass);
		}

		// Token: 0x06003EA0 RID: 16032 RVA: 0x00104968 File Offset: 0x00102B68
		internal void DefineMethod(string methname, Type returntype)
		{
			MethodAttributes methodAttributes = MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Virtual;
			this._methbuilder = this._typebuilder.DefineMethod(methname, methodAttributes, returntype, null);
			this._ilg = this._methbuilder.GetILGenerator();
		}

		// Token: 0x06003EA1 RID: 16033 RVA: 0x0010499E File Offset: 0x00102B9E
		internal void BakeMethod()
		{
			this._methbuilder = null;
		}

		// Token: 0x06003EA2 RID: 16034 RVA: 0x001049A8 File Offset: 0x00102BA8
		internal Type BakeType()
		{
			Type type = this._typebuilder.CreateType();
			this._typebuilder = null;
			return type;
		}

		// Token: 0x04002DB0 RID: 11696
		private static int _typeCount = 0;

		// Token: 0x04002DB1 RID: 11697
		private static LocalDataStoreSlot _moduleSlot = Thread.AllocateDataSlot();

		// Token: 0x04002DB2 RID: 11698
		private AssemblyBuilder _assembly;

		// Token: 0x04002DB3 RID: 11699
		private ModuleBuilder _module;

		// Token: 0x04002DB4 RID: 11700
		private TypeBuilder _typebuilder;

		// Token: 0x04002DB5 RID: 11701
		private MethodBuilder _methbuilder;
	}
}
