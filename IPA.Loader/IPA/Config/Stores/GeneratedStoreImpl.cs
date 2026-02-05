using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Threading;
using IPA.Config.Data;
using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using IPA.Logging;
using IPA.Utilities;
using IPA.Utilities.Async;

namespace IPA.Config.Stores
{
	// Token: 0x02000064 RID: 100
	internal static class GeneratedStoreImpl
	{
		// Token: 0x060002E7 RID: 743 RVA: 0x0000DCF2 File Offset: 0x0000BEF2
		internal static GeneratedStoreImpl.SerializeObject<T> GetSerializerDelegate<T>()
		{
			GeneratedStoreImpl.SerializeObject<T> serializeObject;
			if ((serializeObject = GeneratedStoreImpl.DelegateStore<T>.Serialize) == null)
			{
				serializeObject = (GeneratedStoreImpl.DelegateStore<T>.Serialize = GeneratedStoreImpl.GetSerializerDelegateInternal<T>());
			}
			return serializeObject;
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000DD08 File Offset: 0x0000BF08
		private static GeneratedStoreImpl.SerializeObject<T> GetSerializerDelegateInternal<T>()
		{
			Type type = typeof(T);
			DynamicMethod dynamicMethod = new DynamicMethod("SerializeType>>" + type.FullName, typeof(Value), new Type[] { type }, GeneratedStoreImpl.Module, true);
			IEnumerable<GeneratedStoreImpl.SerializedMemberInfo> structure = GeneratedStoreImpl.ReadObjectMembers(type);
			Action<ILGenerator> action;
			if (!type.IsValueType)
			{
				action = delegate(ILGenerator il)
				{
					il.Emit(OpCodes.Ldarg_0);
				};
			}
			else
			{
				action = delegate(ILGenerator il)
				{
					il.Emit(OpCodes.Ldarga_S, 0);
				};
			}
			Action<ILGenerator> loadObject = action;
			Action<ILGenerator> action2;
			if (!type.IsValueType)
			{
				action2 = loadObject;
			}
			else
			{
				action2 = delegate(ILGenerator il)
				{
					il.Emit(OpCodes.Ldnull);
				};
			}
			Action<ILGenerator> loadParent = action2;
			ILGenerator il2 = dynamicMethod.GetILGenerator();
			GeneratedStoreImpl.LocalAllocator GetLocal = GeneratedStoreImpl.MakeLocalAllocator(il2);
			if (!type.IsValueType)
			{
				Label notIGeneratedStore = il2.DefineLabel();
				Type IGeneratedStore_t = typeof(GeneratedStoreImpl.IGeneratedStore);
				MethodInfo IGeneratedStore_Serialize = IGeneratedStore_t.GetMethod("Serialize");
				il2.Emit(OpCodes.Ldarg_0);
				il2.Emit(OpCodes.Isinst, IGeneratedStore_t);
				il2.Emit(OpCodes.Brfalse, notIGeneratedStore);
				il2.Emit(OpCodes.Ldarg_0);
				il2.Emit(OpCodes.Castclass, IGeneratedStore_t);
				il2.Emit(OpCodes.Callvirt, IGeneratedStore_Serialize);
				il2.Emit(OpCodes.Ret);
				il2.MarkLabel(notIGeneratedStore);
			}
			GeneratedStoreImpl.EmitSerializeStructure(il2, structure, GetLocal, loadObject, loadParent);
			il2.Emit(OpCodes.Ret);
			return (GeneratedStoreImpl.SerializeObject<T>)dynamicMethod.CreateDelegate(typeof(GeneratedStoreImpl.SerializeObject<T>));
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000DE9A File Offset: 0x0000C09A
		internal static GeneratedStoreImpl.DeserializeObject<T> GetDeserializerDelegate<T>()
		{
			GeneratedStoreImpl.DeserializeObject<T> deserializeObject;
			if ((deserializeObject = GeneratedStoreImpl.DelegateStore<T>.Deserialize) == null)
			{
				deserializeObject = (GeneratedStoreImpl.DelegateStore<T>.Deserialize = GeneratedStoreImpl.GetDeserializerDelegateInternal<T>());
			}
			return deserializeObject;
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000DEB0 File Offset: 0x0000C0B0
		private static GeneratedStoreImpl.DeserializeObject<T> GetDeserializerDelegateInternal<T>()
		{
			Type type = typeof(T);
			DynamicMethod dynamicMethod = new DynamicMethod("DeserializeType>>" + type.FullName, type, new Type[]
			{
				typeof(Value),
				typeof(object)
			}, GeneratedStoreImpl.Module, true);
			IEnumerable<GeneratedStoreImpl.SerializedMemberInfo> structure = GeneratedStoreImpl.ReadObjectMembers(type);
			GeneratedStoreImpl.<>c__DisplayClass6_0<T> CS$<>8__locals1 = new GeneratedStoreImpl.<>c__DisplayClass6_0<T>();
			ILGenerator il2 = dynamicMethod.GetILGenerator();
			GeneratedStoreImpl.LocalAllocator GetLocal = GeneratedStoreImpl.MakeLocalAllocator(il2);
			CS$<>8__locals1.IGeneratedStore_t = typeof(GeneratedStoreImpl.IGeneratedStore);
			MethodInfo IGeneratedStore_Deserialize = CS$<>8__locals1.IGeneratedStore_t.GetMethod("Deserialize");
			if (!type.IsValueType)
			{
				GeneratedStoreImpl.EmitCreateChildGenerated(il2, type, new Action<ILGenerator>(CS$<>8__locals1.<GetDeserializerDelegateInternal>g__ParentObj|0));
				il2.Emit(OpCodes.Dup);
				il2.Emit(OpCodes.Castclass, CS$<>8__locals1.IGeneratedStore_t);
				il2.Emit(OpCodes.Ldarg_0);
				il2.Emit(OpCodes.Callvirt, IGeneratedStore_Deserialize);
				il2.Emit(OpCodes.Ret);
			}
			else
			{
				Type Map_t = typeof(Map);
				Map_t.GetMethod("TryGetValue");
				MethodInfo Object_GetType = typeof(object).GetMethod("GetType");
				LocalBuilder valueLocal = il2.DeclareLocal(typeof(Value));
				LocalBuilder mapLocal = il2.DeclareLocal(typeof(Map));
				LocalBuilder resultLocal = il2.DeclareLocal(type);
				Label nonNull = il2.DefineLabel();
				il2.Emit(OpCodes.Ldarg_0);
				il2.Emit(OpCodes.Brtrue, nonNull);
				GeneratedStoreImpl.EmitLogError(il2, "Attempting to deserialize null", false, null, null);
				il2.Emit(OpCodes.Ldloc, resultLocal);
				il2.Emit(OpCodes.Ret);
				il2.MarkLabel(nonNull);
				il2.Emit(OpCodes.Ldarg_0);
				il2.Emit(OpCodes.Isinst, Map_t);
				il2.Emit(OpCodes.Dup);
				il2.Emit(OpCodes.Stloc, mapLocal);
				Label notMapError = il2.DefineLabel();
				il2.Emit(OpCodes.Brtrue, notMapError);
				GeneratedStoreImpl.EmitLogError(il2, "Invalid root for deserializing " + type.FullName, false, delegate(ILGenerator il)
				{
					GeneratedStoreImpl.EmitTypeof(il, Map_t);
				}, delegate(ILGenerator il)
				{
					il.Emit(OpCodes.Ldarg_0);
					il.Emit(OpCodes.Callvirt, Object_GetType);
				});
				il2.Emit(OpCodes.Ldloc, resultLocal);
				il2.Emit(OpCodes.Ret);
				il2.MarkLabel(notMapError);
				GeneratedStoreImpl.EmitDeserializeStructure(il2, structure, mapLocal, valueLocal, GetLocal, delegate(ILGenerator il)
				{
					il.Emit(OpCodes.Ldloca, resultLocal);
				}, new Action<ILGenerator>(CS$<>8__locals1.<GetDeserializerDelegateInternal>g__ParentObj|0));
				il2.Emit(OpCodes.Ldloc, resultLocal);
				il2.Emit(OpCodes.Ret);
			}
			return (GeneratedStoreImpl.DeserializeObject<T>)dynamicMethod.CreateDelegate(typeof(GeneratedStoreImpl.DeserializeObject<T>));
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000E164 File Offset: 0x0000C364
		private static bool NeedsCorrection(GeneratedStoreImpl.SerializedMemberInfo member)
		{
			if (member.HasConverter)
			{
				return false;
			}
			Type memberType = member.ConversionType;
			Type expectType = GeneratedStoreImpl.GetExpectedValueTypeForType(memberType);
			return expectType == typeof(Map) && (!expectType.IsValueType || GeneratedStoreImpl.ReadObjectMembers(memberType).Any(new Func<GeneratedStoreImpl.SerializedMemberInfo, bool>(GeneratedStoreImpl.NeedsCorrection)));
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000E1C0 File Offset: 0x0000C3C0
		private static void EmitCorrectMember(ILGenerator il, GeneratedStoreImpl.SerializedMemberInfo member, bool shouldLock, bool alwaysNew, GeneratedStoreImpl.LocalAllocator GetLocal, Action<ILGenerator> thisobj, Action<ILGenerator> parentobj)
		{
			if (!GeneratedStoreImpl.NeedsCorrection(member))
			{
				return;
			}
			Label endLabel = il.DefineLabel();
			if (member.IsNullable)
			{
				il.Emit(OpCodes.Dup);
				il.Emit(OpCodes.Call, member.Nullable_HasValue.GetGetMethod());
				il.Emit(OpCodes.Brfalse, endLabel);
				il.Emit(OpCodes.Call, member.Nullable_Value.GetGetMethod());
			}
			Type convType = member.ConversionType;
			if (!convType.IsValueType)
			{
				MethodInfo copyFrom = typeof(GeneratedStoreImpl.IGeneratedStore<>).MakeGenericType(new Type[] { convType }).GetMethod("CopyFrom");
				Label noCreate = il.DefineLabel();
				using (GeneratedStoreImpl.AllocatedLocal valLocal = GetLocal.Allocate(convType))
				{
					if (member.AllowNull)
					{
						il.Emit(OpCodes.Dup);
						il.Emit(OpCodes.Brfalse_S, endLabel);
					}
					if (!alwaysNew)
					{
						il.Emit(OpCodes.Dup);
						il.Emit(OpCodes.Isinst, typeof(GeneratedStoreImpl.IGeneratedStore));
						il.Emit(OpCodes.Brtrue_S, endLabel);
					}
					il.Emit(OpCodes.Stloc, valLocal);
					if (!alwaysNew)
					{
						GeneratedStoreImpl.EmitLoad(il, member, thisobj);
						il.Emit(OpCodes.Dup);
						il.Emit(OpCodes.Isinst, typeof(GeneratedStoreImpl.IGeneratedStore));
						il.Emit(OpCodes.Brtrue_S, noCreate);
						il.Emit(OpCodes.Pop);
					}
					GeneratedStoreImpl.EmitCreateChildGenerated(il, convType, parentobj);
					il.MarkLabel(noCreate);
					il.Emit(OpCodes.Dup);
					il.Emit(OpCodes.Ldloc, valLocal);
					il.Emit(shouldLock ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
					il.Emit(OpCodes.Callvirt, copyFrom);
					goto IL_0263;
				}
			}
			GeneratedStoreImpl.<>c__DisplayClass8_0 CS$<>8__locals1 = new GeneratedStoreImpl.<>c__DisplayClass8_0();
			IEnumerable<GeneratedStoreImpl.SerializedMemberInfo> structure = GeneratedStoreImpl.ReadObjectMembers(convType);
			CS$<>8__locals1.valueLocal = GetLocal.Allocate(convType);
			try
			{
				il.Emit(OpCodes.Stloc, CS$<>8__locals1.valueLocal);
				foreach (GeneratedStoreImpl.SerializedMemberInfo mem in structure)
				{
					if (GeneratedStoreImpl.NeedsCorrection(mem))
					{
						GeneratedStoreImpl.EmitLoadCorrectStore(il, mem, shouldLock, alwaysNew, GetLocal, new Action<ILGenerator>(CS$<>8__locals1.<EmitCorrectMember>g__LdlocaValueLocal|0), new Action<ILGenerator>(CS$<>8__locals1.<EmitCorrectMember>g__LdlocaValueLocal|0), parentobj);
					}
				}
				il.Emit(OpCodes.Ldloc, CS$<>8__locals1.valueLocal);
			}
			finally
			{
				((IDisposable)CS$<>8__locals1.valueLocal).Dispose();
			}
			IL_0263:
			if (member.IsNullable)
			{
				il.Emit(OpCodes.Newobj, member.Nullable_Construct);
			}
			il.MarkLabel(endLabel);
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000E478 File Offset: 0x0000C678
		private static void EmitLoadCorrectStore(ILGenerator il, GeneratedStoreImpl.SerializedMemberInfo member, bool shouldLock, bool alwaysNew, GeneratedStoreImpl.LocalAllocator GetLocal, Action<ILGenerator> loadFrom, Action<ILGenerator> storeTo, Action<ILGenerator> parentobj)
		{
			GeneratedStoreImpl.EmitStore(il, member, delegate(ILGenerator il)
			{
				GeneratedStoreImpl.EmitLoad(il, member, loadFrom);
				GeneratedStoreImpl.EmitCorrectMember(il, member, shouldLock, alwaysNew, GetLocal, storeTo, parentobj);
			}, storeTo);
		}

		// Token: 0x060002EE RID: 750 RVA: 0x0000E4E0 File Offset: 0x0000C6E0
		private static void EmitDeserializeGeneratedValue(ILGenerator il, GeneratedStoreImpl.SerializedMemberInfo member, Type srcType, GeneratedStoreImpl.LocalAllocator GetLocal, Action<ILGenerator> thisarg, Action<ILGenerator> parentobj)
		{
			MethodInfo IGeneratedStore_Deserialize = typeof(GeneratedStoreImpl.IGeneratedStore).GetMethod("Deserialize");
			using (GeneratedStoreImpl.AllocatedLocal valuel = GetLocal.Allocate(srcType))
			{
				Label noCreate = il.DefineLabel();
				il.Emit(OpCodes.Stloc, valuel);
				GeneratedStoreImpl.EmitLoad(il, member, thisarg);
				il.Emit(OpCodes.Dup);
				il.Emit(OpCodes.Isinst, typeof(GeneratedStoreImpl.IGeneratedStore));
				il.Emit(OpCodes.Brtrue_S, noCreate);
				il.Emit(OpCodes.Pop);
				GeneratedStoreImpl.EmitCreateChildGenerated(il, member.Type, parentobj);
				il.MarkLabel(noCreate);
				il.Emit(OpCodes.Dup);
				il.Emit(OpCodes.Ldloc, valuel);
				il.Emit(OpCodes.Callvirt, IGeneratedStore_Deserialize);
			}
		}

		// Token: 0x060002EF RID: 751 RVA: 0x0000E5C0 File Offset: 0x0000C7C0
		private static void EmitDeserializeNullable(ILGenerator il, GeneratedStoreImpl.SerializedMemberInfo member, Type expected, GeneratedStoreImpl.LocalAllocator GetLocal, Action<ILGenerator> thisarg, Action<ILGenerator> parentobj)
		{
			if (thisarg == null)
			{
				thisarg = delegate(ILGenerator il)
				{
					il.Emit(OpCodes.Ldarg_0);
				};
			}
			if (parentobj == null)
			{
				parentobj = thisarg;
			}
			GeneratedStoreImpl.EmitDeserializeValue(il, member, member.NullableWrappedType, expected, GetLocal, thisarg, parentobj);
			il.Emit(OpCodes.Newobj, member.Nullable_Construct);
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0000E620 File Offset: 0x0000C820
		private static void EmitDeserializeValue(ILGenerator il, GeneratedStoreImpl.SerializedMemberInfo member, Type targetType, Type expected, GeneratedStoreImpl.LocalAllocator GetLocal, Action<ILGenerator> thisarg, Action<ILGenerator> parentobj)
		{
			if (typeof(Value).IsAssignableFrom(targetType))
			{
				return;
			}
			if (expected == typeof(Text))
			{
				MethodInfo getter = expected.GetProperty("Value").GetGetMethod();
				il.Emit(OpCodes.Call, getter);
				if (targetType == typeof(char))
				{
					MethodInfo strIndex = typeof(string).GetProperty("Chars").GetGetMethod();
					il.Emit(OpCodes.Ldc_I4_0);
					il.Emit(OpCodes.Call, strIndex);
					return;
				}
			}
			else
			{
				if (expected == typeof(IPA.Config.Data.Boolean))
				{
					MethodInfo getter2 = expected.GetProperty("Value").GetGetMethod();
					il.Emit(OpCodes.Call, getter2);
					return;
				}
				if (expected == typeof(Integer))
				{
					MethodInfo getter3 = expected.GetProperty("Value").GetGetMethod();
					il.Emit(OpCodes.Call, getter3);
					GeneratedStoreImpl.EmitNumberConvertTo(il, targetType, getter3.ReturnType);
					return;
				}
				if (expected == typeof(FloatingPoint))
				{
					MethodInfo getter4 = expected.GetProperty("Value").GetGetMethod();
					il.Emit(OpCodes.Call, getter4);
					GeneratedStoreImpl.EmitNumberConvertTo(il, targetType, getter4.ReturnType);
					return;
				}
				if (expected == typeof(Map))
				{
					if (!targetType.IsValueType)
					{
						GeneratedStoreImpl.EmitDeserializeGeneratedValue(il, member, expected, GetLocal, thisarg, parentobj);
						return;
					}
					using (GeneratedStoreImpl.AllocatedLocal mapLocal = GetLocal.Allocate(typeof(Map)))
					{
						using (GeneratedStoreImpl.AllocatedLocal resultLocal = GetLocal.Allocate(targetType))
						{
							using (GeneratedStoreImpl.AllocatedLocal valueLocal = GetLocal.Allocate(typeof(Value)))
							{
								IEnumerable<GeneratedStoreImpl.SerializedMemberInfo> structure = GeneratedStoreImpl.ReadObjectMembers(targetType);
								if (!structure.Any<GeneratedStoreImpl.SerializedMemberInfo>())
								{
									Logger.config.Warn(string.Concat(new string[]
									{
										"Custom value type ",
										targetType.FullName,
										" (when compiling serialization of ",
										member.Name,
										" on ",
										member.Member.DeclaringType.FullName,
										") has no accessible members"
									}));
									il.Emit(OpCodes.Pop);
									il.Emit(OpCodes.Ldloca, resultLocal);
									il.Emit(OpCodes.Initobj, targetType);
								}
								else
								{
									il.Emit(OpCodes.Stloc, mapLocal);
									GeneratedStoreImpl.EmitLoad(il, member, thisarg);
									il.Emit(OpCodes.Stloc, resultLocal);
									GeneratedStoreImpl.EmitDeserializeStructure(il, structure, mapLocal, valueLocal, GetLocal, delegate(ILGenerator il)
									{
										il.Emit(OpCodes.Ldloca, resultLocal);
									}, parentobj);
								}
								il.Emit(OpCodes.Ldloc, resultLocal);
								return;
							}
						}
					}
				}
				Logger.config.Warn(string.Format("Implicit conversions to {0} are not currently implemented", expected));
				il.Emit(OpCodes.Pop);
				il.Emit(OpCodes.Ldnull);
			}
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0000E984 File Offset: 0x0000CB84
		private static void EmitDeserializeStructure(ILGenerator il, IEnumerable<GeneratedStoreImpl.SerializedMemberInfo> structure, LocalBuilder mapLocal, LocalBuilder valueLocal, GeneratedStoreImpl.LocalAllocator GetLocal, Action<ILGenerator> thisobj, Action<ILGenerator> parentobj)
		{
			MethodInfo Map_TryGetValue = typeof(Map).GetMethod("TryGetValue");
			Action<ILGenerator> <>9__0;
			foreach (GeneratedStoreImpl.SerializedMemberInfo mem in structure)
			{
				Label nextLabel = il.DefineLabel();
				Label endErrorLabel = il.DefineLabel();
				il.Emit(OpCodes.Ldloc, mapLocal);
				il.Emit(OpCodes.Ldstr, mem.Name);
				il.Emit(OpCodes.Ldloca_S, valueLocal);
				il.Emit(OpCodes.Call, Map_TryGetValue);
				il.Emit(OpCodes.Brtrue_S, endErrorLabel);
				GeneratedStoreImpl.EmitLogError(il, "Missing key " + mem.Name, false, null, null);
				il.Emit(OpCodes.Br, nextLabel);
				il.MarkLabel(endErrorLabel);
				il.Emit(OpCodes.Ldloc_S, valueLocal);
				GeneratedStoreImpl.SerializedMemberInfo serializedMemberInfo = mem;
				Label label = nextLabel;
				Action<ILGenerator> action;
				if ((action = <>9__0) == null)
				{
					action = (<>9__0 = delegate(ILGenerator il)
					{
						il.Emit(OpCodes.Ldloc_S, valueLocal);
					});
				}
				GeneratedStoreImpl.EmitDeserializeMember(il, serializedMemberInfo, label, action, GetLocal, thisobj, parentobj);
				il.MarkLabel(nextLabel);
			}
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0000EABC File Offset: 0x0000CCBC
		private static void EmitDeserializeConverter(ILGenerator il, GeneratedStoreImpl.SerializedMemberInfo member, Label nextLabel, GeneratedStoreImpl.LocalAllocator GetLocal, Action<ILGenerator> thisobj, Action<ILGenerator> parentobj)
		{
			using (GeneratedStoreImpl.AllocatedLocal stlocal = GetLocal.Allocate(typeof(Value)))
			{
				using (GeneratedStoreImpl.AllocatedLocal valLocal = GetLocal.Allocate(member.Type))
				{
					il.Emit(OpCodes.Stloc, stlocal);
					il.BeginExceptionBlock();
					il.Emit(OpCodes.Ldsfld, member.ConverterField);
					il.Emit(OpCodes.Ldloc, stlocal);
					parentobj(il);
					if (member.IsGenericConverter)
					{
						MethodInfo fromValueBase2 = member.ConverterBase.GetMethod("FromValue", new Type[]
						{
							typeof(Value),
							typeof(object)
						});
						MethodInfo fromValue = member.Converter.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault((MethodInfo m) => m.GetBaseDefinition() == fromValueBase2) ?? fromValueBase2;
						il.Emit(OpCodes.Call, fromValue);
					}
					else
					{
						MethodInfo fromValueBase = typeof(IValueConverter).GetMethod("FromValue", new Type[]
						{
							typeof(Value),
							typeof(object)
						});
						MethodInfo fromValue2 = member.Converter.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault((MethodInfo m) => m.GetBaseDefinition() == fromValueBase) ?? fromValueBase;
						il.Emit(OpCodes.Call, fromValue2);
						if (member.Type.IsValueType)
						{
							il.Emit(OpCodes.Unbox);
						}
					}
					il.Emit(OpCodes.Stloc, valLocal);
					il.BeginCatchBlock(typeof(Exception));
					GeneratedStoreImpl.EmitWarnException(il, "Error occurred while deserializing");
					il.Emit(OpCodes.Leave, nextLabel);
					il.EndExceptionBlock();
					il.Emit(OpCodes.Ldloc, valLocal);
				}
			}
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0000ECDC File Offset: 0x0000CEDC
		private static void EmitDeserializeMember(ILGenerator il, GeneratedStoreImpl.SerializedMemberInfo member, Label nextLabel, Action<ILGenerator> getValue, GeneratedStoreImpl.LocalAllocator GetLocal, Action<ILGenerator> thisobj, Action<ILGenerator> parentobj)
		{
			MethodInfo Object_GetType = typeof(object).GetMethod("GetType");
			Label implLabel = il.DefineLabel();
			Label passedTypeCheck = il.DefineLabel();
			Type expectType = GeneratedStoreImpl.GetExpectedValueTypeForType(member.ConversionType);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Brtrue_S, implLabel);
			if (!member.AllowNull)
			{
				il.Emit(OpCodes.Pop);
				GeneratedStoreImpl.EmitLogError(il, string.Format("Member {0} ({1}) not nullable", member.Name, member.Type), false, delegate(ILGenerator il)
				{
					GeneratedStoreImpl.EmitTypeof(il, expectType);
				}, null);
				il.Emit(OpCodes.Br, nextLabel);
			}
			else
			{
				if (member.IsNullable)
				{
					il.Emit(OpCodes.Pop);
					using (GeneratedStoreImpl.AllocatedLocal valTLocal = GetLocal.Allocate(member.Type))
					{
						il.Emit(OpCodes.Ldloca, valTLocal);
						il.Emit(OpCodes.Initobj, member.Type);
						GeneratedStoreImpl.EmitStore(il, member, delegate(ILGenerator il)
						{
							il.Emit(OpCodes.Ldloc, valTLocal);
						}, thisobj);
						il.Emit(OpCodes.Br, nextLabel);
						goto IL_017A;
					}
				}
				il.Emit(OpCodes.Pop);
				GeneratedStoreImpl.EmitStore(il, member, delegate(ILGenerator il)
				{
					il.Emit(OpCodes.Ldnull);
				}, thisobj);
				il.Emit(OpCodes.Br, nextLabel);
			}
			IL_017A:
			if (!member.HasConverter)
			{
				il.MarkLabel(implLabel);
				il.Emit(OpCodes.Isinst, expectType);
				il.Emit(OpCodes.Dup);
				il.Emit(OpCodes.Brtrue, passedTypeCheck);
			}
			Label errorHandle = il.DefineLabel();
			if (member.HasConverter)
			{
				il.MarkLabel(implLabel);
			}
			else if (expectType == typeof(FloatingPoint))
			{
				il.DefineLabel();
				il.Emit(OpCodes.Pop);
				getValue(il);
				il.Emit(OpCodes.Isinst, typeof(Integer));
				il.Emit(OpCodes.Dup);
				il.Emit(OpCodes.Brfalse, errorHandle);
				MethodInfo Integer_CoerceToFloat = typeof(Integer).GetMethod("AsFloat");
				il.Emit(OpCodes.Call, Integer_CoerceToFloat);
				il.Emit(OpCodes.Br, passedTypeCheck);
			}
			else if (expectType == typeof(Integer))
			{
				il.DefineLabel();
				il.Emit(OpCodes.Pop);
				getValue(il);
				il.Emit(OpCodes.Isinst, typeof(FloatingPoint));
				il.Emit(OpCodes.Dup);
				il.Emit(OpCodes.Brfalse, errorHandle);
				MethodInfo Float_CoerceToInt = typeof(FloatingPoint).GetMethod("AsInteger");
				il.Emit(OpCodes.Call, Float_CoerceToInt);
				il.Emit(OpCodes.Br, passedTypeCheck);
			}
			if (!member.HasConverter)
			{
				il.MarkLabel(errorHandle);
				il.Emit(OpCodes.Pop);
				GeneratedStoreImpl.EmitLogError(il, "Unexpected type deserializing " + member.Name, false, delegate(ILGenerator il)
				{
					GeneratedStoreImpl.EmitTypeof(il, expectType);
				}, delegate(ILGenerator il)
				{
					getValue(il);
					il.Emit(OpCodes.Callvirt, Object_GetType);
				});
				il.Emit(OpCodes.Br, nextLabel);
			}
			il.MarkLabel(passedTypeCheck);
			using (GeneratedStoreImpl.AllocatedLocal local = GetLocal.Allocate(member.Type))
			{
				if (member.HasConverter)
				{
					GeneratedStoreImpl.EmitDeserializeConverter(il, member, nextLabel, GetLocal, thisobj, parentobj);
				}
				else if (member.IsNullable)
				{
					GeneratedStoreImpl.EmitDeserializeNullable(il, member, expectType, GetLocal, thisobj, parentobj);
				}
				else
				{
					GeneratedStoreImpl.EmitDeserializeValue(il, member, member.Type, expectType, GetLocal, thisobj, parentobj);
				}
				il.Emit(OpCodes.Stloc, local);
				GeneratedStoreImpl.EmitStore(il, member, delegate(ILGenerator il)
				{
					il.Emit(OpCodes.Ldloc, local);
				}, thisobj);
			}
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0000F0F4 File Offset: 0x0000D2F4
		public static T Create<T>() where T : class
		{
			return (T)((object)GeneratedStoreImpl.Create(typeof(T)));
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x0000F10A File Offset: 0x0000D30A
		public static IConfigStore Create(Type type)
		{
			return GeneratedStoreImpl.Create(type, null);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000F113 File Offset: 0x0000D313
		internal static T Create<T>(GeneratedStoreImpl.IGeneratedStore parent) where T : class
		{
			return (T)((object)GeneratedStoreImpl.Create(typeof(T), parent));
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000F12A File Offset: 0x0000D32A
		private static IConfigStore Create(Type type, GeneratedStoreImpl.IGeneratedStore parent)
		{
			return GeneratedStoreImpl.GetCreator(type)(parent);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000F138 File Offset: 0x0000D338
		[return: global::System.Runtime.CompilerServices.TupleElementNames(new string[] { "ctor", "type" })]
		private static global::System.ValueTuple<GeneratedStoreImpl.GeneratedStoreCreator, Type> GetCreatorAndGeneratedType(Type t)
		{
			return GeneratedStoreImpl.generatedCreators.GetOrAdd(t, new Func<Type, global::System.ValueTuple<GeneratedStoreImpl.GeneratedStoreCreator, Type>>(GeneratedStoreImpl.MakeCreator));
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000F151 File Offset: 0x0000D351
		internal static GeneratedStoreImpl.GeneratedStoreCreator GetCreator(Type t)
		{
			return GeneratedStoreImpl.GetCreatorAndGeneratedType(t).Item1;
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000F15E File Offset: 0x0000D35E
		internal static Type GetGeneratedType(Type t)
		{
			return GeneratedStoreImpl.GetCreatorAndGeneratedType(t).Item2;
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060002FB RID: 763 RVA: 0x0000F16C File Offset: 0x0000D36C
		private static AssemblyBuilder Assembly
		{
			get
			{
				if (GeneratedStoreImpl.assembly == null)
				{
					AssemblyName name = new AssemblyName("IPA.Config.Generated");
					GeneratedStoreImpl.assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.RunAndSave);
				}
				return GeneratedStoreImpl.assembly;
			}
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000F1A7 File Offset: 0x0000D3A7
		internal static void DebugSaveAssembly(string file)
		{
			GeneratedStoreImpl.Assembly.Save(file);
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060002FD RID: 765 RVA: 0x0000F1B4 File Offset: 0x0000D3B4
		private static ModuleBuilder Module
		{
			get
			{
				if (GeneratedStoreImpl.module == null)
				{
					GeneratedStoreImpl.module = GeneratedStoreImpl.Assembly.DefineDynamicModule(GeneratedStoreImpl.Assembly.GetName().Name, GeneratedStoreImpl.Assembly.GetName().Name + ".dll");
				}
				return GeneratedStoreImpl.module;
			}
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000F20C File Offset: 0x0000D40C
		private static void CreateAndInitializeConvertersFor(Type type, IEnumerable<GeneratedStoreImpl.SerializedMemberInfo> structure)
		{
			Dictionary<Type, FieldInfo> converters;
			if (!GeneratedStoreImpl.TypeRequiredConverters.TryGetValue(type, out converters))
			{
				TypeBuilder converterFieldType = GeneratedStoreImpl.Module.DefineType(type.FullName + "<Converters>", TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed);
				Type[] array = (from m in structure
					where m.HasConverter
					select m.Converter).Distinct<Type>().ToArray<Type>();
				converters = new Dictionary<Type, FieldInfo>(array.Length);
				foreach (Type convType in array)
				{
					FieldBuilder field = converterFieldType.DefineField(string.Format("<converter>_{0}", convType), convType, FieldAttributes.Private | FieldAttributes.Family | FieldAttributes.Static | FieldAttributes.InitOnly);
					converters.Add(convType, field);
				}
				ILGenerator il = converterFieldType.DefineConstructor(MethodAttributes.Static, CallingConventions.Standard, Type.EmptyTypes).GetILGenerator();
				foreach (KeyValuePair<Type, FieldInfo> kvp in converters)
				{
					ConstructorInfo typeCtor = kvp.Key.GetConstructor(Type.EmptyTypes);
					il.Emit(OpCodes.Newobj, typeCtor);
					il.Emit(OpCodes.Stsfld, kvp.Value);
				}
				il.Emit(OpCodes.Ret);
				GeneratedStoreImpl.TypeRequiredConverters.Add(type, converters);
				converterFieldType.CreateType();
			}
			foreach (GeneratedStoreImpl.SerializedMemberInfo member in structure.Where((GeneratedStoreImpl.SerializedMemberInfo m) => m.HasConverter))
			{
				member.ConverterField = converters[member.Converter];
			}
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000F3F0 File Offset: 0x0000D5F0
		private static void GetMethodThis(ILGenerator il)
		{
			il.Emit(OpCodes.Ldarg_0);
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000F400 File Offset: 0x0000D600
		[return: global::System.Runtime.CompilerServices.TupleElementNames(new string[] { "ctor", "type" })]
		private static global::System.ValueTuple<GeneratedStoreImpl.GeneratedStoreCreator, Type> MakeCreator(Type type)
		{
			if (!type.IsClass)
			{
				throw new ArgumentException("Config type is not a class");
			}
			ConstructorInfo baseCtor = type.GetConstructor(Type.EmptyTypes);
			if (baseCtor == null)
			{
				throw new ArgumentException("Config type does not have a public parameterless constructor");
			}
			MethodInfo baseChanged = type.GetMethod("Changed", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, Array.Empty<ParameterModifier>());
			if (baseChanged != null && GeneratedStoreImpl.IsMethodInvalid(baseChanged, typeof(void)))
			{
				baseChanged = null;
			}
			MethodInfo baseOnReload = type.GetMethod("OnReload", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, Array.Empty<ParameterModifier>());
			if (baseOnReload != null && GeneratedStoreImpl.IsMethodInvalid(baseOnReload, typeof(void)))
			{
				baseOnReload = null;
			}
			MethodInfo baseCopyFrom = type.GetMethod("CopyFrom", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[] { type }, Array.Empty<ParameterModifier>());
			if (baseCopyFrom != null && GeneratedStoreImpl.IsMethodInvalid(baseCopyFrom, typeof(void)))
			{
				baseCopyFrom = null;
			}
			MethodInfo baseChangeTransaction = type.GetMethod("ChangeTransaction", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, Array.Empty<ParameterModifier>());
			if (baseChangeTransaction != null && GeneratedStoreImpl.IsMethodInvalid(baseChangeTransaction, typeof(IDisposable)))
			{
				baseChangeTransaction = null;
			}
			bool isINotifyPropertyChanged = type.FindInterfaces((Type i, object t) => i == (Type)t, typeof(INotifyPropertyChanged)).Length != 0;
			bool hasNotifyAttribute = type.GetCustomAttribute<NotifyPropertyChangesAttribute>() != null;
			IEnumerable<GeneratedStoreImpl.SerializedMemberInfo> structure = GeneratedStoreImpl.ReadObjectMembers(type);
			if (!structure.Any<GeneratedStoreImpl.SerializedMemberInfo>())
			{
				Logger.config.Warn("Custom type " + type.FullName + " has no accessible members");
			}
			TypeBuilder typeBuilder = GeneratedStoreImpl.Module.DefineType(type.FullName + "<Generated>", TypeAttributes.Public | TypeAttributes.Sealed, type);
			FieldBuilder typeField = typeBuilder.DefineField("<>_type", typeof(Type), FieldAttributes.Private | FieldAttributes.InitOnly);
			FieldBuilder implField = typeBuilder.DefineField("<>_impl", typeof(GeneratedStoreImpl.Impl), FieldAttributes.Private | FieldAttributes.InitOnly);
			FieldBuilder parentField = typeBuilder.DefineField("<>_parent", typeof(GeneratedStoreImpl.IGeneratedStore), FieldAttributes.Private | FieldAttributes.InitOnly);
			ConstructorBuilder ctor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { typeof(GeneratedStoreImpl.IGeneratedStore) });
			ILGenerator il8 = ctor.GetILGenerator();
			il8.Emit(OpCodes.Ldarg_0);
			il8.Emit(OpCodes.Dup);
			il8.Emit(OpCodes.Call, baseCtor);
			il8.Emit(OpCodes.Dup);
			il8.Emit(OpCodes.Ldarg_1);
			il8.Emit(OpCodes.Stfld, parentField);
			il8.Emit(OpCodes.Dup);
			GeneratedStoreImpl.EmitTypeof(il8, type);
			il8.Emit(OpCodes.Stfld, typeField);
			Label noImplLabel = il8.DefineLabel();
			il8.Emit(OpCodes.Ldarg_1);
			il8.Emit(OpCodes.Brtrue, noImplLabel);
			il8.Emit(OpCodes.Dup);
			il8.Emit(OpCodes.Dup);
			il8.Emit(OpCodes.Newobj, GeneratedStoreImpl.Impl.Ctor);
			il8.Emit(OpCodes.Stfld, implField);
			il8.MarkLabel(noImplLabel);
			GeneratedStoreImpl.LocalAllocator GetLocal = GeneratedStoreImpl.MakeLocalAllocator(il8);
			foreach (GeneratedStoreImpl.SerializedMemberInfo member in structure)
			{
				if (GeneratedStoreImpl.NeedsCorrection(member))
				{
					GeneratedStoreImpl.EmitLoadCorrectStore(il8, member, false, true, GetLocal, new Action<ILGenerator>(GeneratedStoreImpl.GetMethodThis), new Action<ILGenerator>(GeneratedStoreImpl.GetMethodThis), new Action<ILGenerator>(GeneratedStoreImpl.GetMethodThis));
				}
			}
			il8.Emit(OpCodes.Pop);
			il8.Emit(OpCodes.Ret);
			MethodBuilder notifyChanged = null;
			if (isINotifyPropertyChanged || hasNotifyAttribute)
			{
				Type INotifyPropertyChanged_t = typeof(INotifyPropertyChanged);
				typeBuilder.AddInterfaceImplementation(INotifyPropertyChanged_t);
				EventInfo INotifyPropertyChanged_PropertyChanged = INotifyPropertyChanged_t.GetEvent("PropertyChanged");
				Type PropertyChangedEventHandler_t = typeof(PropertyChangedEventHandler);
				MethodInfo PropertyChangedEventHander_Invoke = PropertyChangedEventHandler_t.GetMethod("Invoke");
				ConstructorInfo PropertyChangedEventArgs_ctor = typeof(PropertyChangedEventArgs).GetConstructor(new Type[] { typeof(string) });
				Type Delegate_t = typeof(Delegate);
				MethodInfo Delegate_Combine = Delegate_t.GetMethod("Combine", BindingFlags.Static | BindingFlags.Public, null, new Type[] { Delegate_t, Delegate_t }, Array.Empty<ParameterModifier>());
				MethodInfo Delegate_Remove = Delegate_t.GetMethod("Remove", BindingFlags.Static | BindingFlags.Public, null, new Type[] { Delegate_t, Delegate_t }, Array.Empty<ParameterModifier>());
				MethodInfo CompareExchange = (from m in typeof(Interlocked).GetMethods()
					where m.Name == "CompareExchange"
					where m.ContainsGenericParameters
					where m.GetParameters().Length == 3
					select m).First<MethodInfo>().MakeGenericMethod(new Type[] { PropertyChangedEventHandler_t });
				EventInfo eventInfo = (from e in type.GetEvents()
					where e.GetAddMethod().GetBaseDefinition().DeclaringType == INotifyPropertyChanged_t
					select e).FirstOrDefault<EventInfo>();
				MethodInfo basePropChangedAdd = ((eventInfo != null) ? eventInfo.GetAddMethod() : null);
				MethodInfo basePropChangedRemove = ((eventInfo != null) ? eventInfo.GetRemoveMethod() : null);
				FieldBuilder PropertyChanged_backing = typeBuilder.DefineField("<event>PropertyChanged", PropertyChangedEventHandler_t, FieldAttributes.Private);
				MethodBuilder add_PropertyChanged = typeBuilder.DefineMethod("<add>PropertyChanged", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.HideBySig, null, new Type[] { PropertyChangedEventHandler_t });
				typeBuilder.DefineMethodOverride(add_PropertyChanged, INotifyPropertyChanged_PropertyChanged.GetAddMethod());
				if (basePropChangedAdd != null)
				{
					typeBuilder.DefineMethodOverride(add_PropertyChanged, basePropChangedAdd);
				}
				ILGenerator ilgenerator = add_PropertyChanged.GetILGenerator();
				Label loopLabel = ilgenerator.DefineLabel();
				LocalBuilder delTemp = ilgenerator.DeclareLocal(PropertyChangedEventHandler_t);
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldfld, PropertyChanged_backing);
				ilgenerator.MarkLabel(loopLabel);
				ilgenerator.Emit(OpCodes.Stloc, delTemp);
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldflda, PropertyChanged_backing);
				ilgenerator.Emit(OpCodes.Ldloc, delTemp);
				ilgenerator.Emit(OpCodes.Ldarg_1);
				ilgenerator.Emit(OpCodes.Call, Delegate_Combine);
				ilgenerator.Emit(OpCodes.Castclass, PropertyChangedEventHandler_t);
				ilgenerator.Emit(OpCodes.Ldloc, delTemp);
				ilgenerator.Emit(OpCodes.Call, CompareExchange);
				ilgenerator.Emit(OpCodes.Dup);
				ilgenerator.Emit(OpCodes.Ldloc, delTemp);
				ilgenerator.Emit(OpCodes.Bne_Un_S, loopLabel);
				ilgenerator.Emit(OpCodes.Ret);
				MethodBuilder remove_PropertyChanged = typeBuilder.DefineMethod("<remove>PropertyChanged", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.HideBySig, null, new Type[] { PropertyChangedEventHandler_t });
				typeBuilder.DefineMethodOverride(remove_PropertyChanged, INotifyPropertyChanged_PropertyChanged.GetRemoveMethod());
				if (basePropChangedRemove != null)
				{
					typeBuilder.DefineMethodOverride(remove_PropertyChanged, basePropChangedRemove);
				}
				ILGenerator ilgenerator2 = remove_PropertyChanged.GetILGenerator();
				Label loopLabel2 = ilgenerator2.DefineLabel();
				LocalBuilder delTemp2 = ilgenerator2.DeclareLocal(PropertyChangedEventHandler_t);
				ilgenerator2.Emit(OpCodes.Ldarg_0);
				ilgenerator2.Emit(OpCodes.Ldfld, PropertyChanged_backing);
				ilgenerator2.MarkLabel(loopLabel2);
				ilgenerator2.Emit(OpCodes.Stloc, delTemp2);
				ilgenerator2.Emit(OpCodes.Ldarg_0);
				ilgenerator2.Emit(OpCodes.Ldflda, PropertyChanged_backing);
				ilgenerator2.Emit(OpCodes.Ldloc, delTemp2);
				ilgenerator2.Emit(OpCodes.Ldarg_1);
				ilgenerator2.Emit(OpCodes.Call, Delegate_Remove);
				ilgenerator2.Emit(OpCodes.Castclass, PropertyChangedEventHandler_t);
				ilgenerator2.Emit(OpCodes.Ldloc, delTemp2);
				ilgenerator2.Emit(OpCodes.Call, CompareExchange);
				ilgenerator2.Emit(OpCodes.Dup);
				ilgenerator2.Emit(OpCodes.Ldloc, delTemp2);
				ilgenerator2.Emit(OpCodes.Bne_Un_S, loopLabel2);
				ilgenerator2.Emit(OpCodes.Ret);
				EventBuilder eventBuilder = typeBuilder.DefineEvent("PropertyChanged", EventAttributes.None, PropertyChangedEventHandler_t);
				eventBuilder.SetAddOnMethod(add_PropertyChanged);
				eventBuilder.SetRemoveOnMethod(remove_PropertyChanged);
				notifyChanged = typeBuilder.DefineMethod("<>NotifyChanged", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Final | MethodAttributes.HideBySig, null, new Type[] { typeof(string) });
				ILGenerator ilgenerator3 = notifyChanged.GetILGenerator();
				Label invokeNonNull = ilgenerator3.DefineLabel();
				ilgenerator3.Emit(OpCodes.Ldarg_0);
				ilgenerator3.Emit(OpCodes.Ldfld, PropertyChanged_backing);
				ilgenerator3.Emit(OpCodes.Dup);
				ilgenerator3.Emit(OpCodes.Brtrue, invokeNonNull);
				ilgenerator3.Emit(OpCodes.Pop);
				ilgenerator3.Emit(OpCodes.Ret);
				ilgenerator3.MarkLabel(invokeNonNull);
				ilgenerator3.Emit(OpCodes.Ldarg_0);
				ilgenerator3.Emit(OpCodes.Ldarg_1);
				ilgenerator3.Emit(OpCodes.Newobj, PropertyChangedEventArgs_ctor);
				ilgenerator3.Emit(OpCodes.Call, PropertyChangedEventHander_Invoke);
				ilgenerator3.Emit(OpCodes.Ret);
			}
			typeBuilder.AddInterfaceImplementation(typeof(GeneratedStoreImpl.IGeneratedStore));
			Type typeFromHandle = typeof(GeneratedStoreImpl.IGeneratedStore);
			MethodInfo IGeneratedStore_GetImpl = typeFromHandle.GetProperty("Impl").GetGetMethod();
			MethodInfo IGeneratedStore_GetType = typeFromHandle.GetProperty("Type").GetGetMethod();
			MethodInfo IGeneratedStore_GetParent = typeFromHandle.GetProperty("Parent").GetGetMethod();
			MethodInfo IGeneratedStore_Serialize = typeFromHandle.GetMethod("Serialize");
			MethodInfo IGeneratedStore_Deserialize = typeFromHandle.GetMethod("Deserialize");
			MethodInfo IGeneratedStore_OnReload = typeFromHandle.GetMethod("OnReload");
			MethodInfo IGeneratedStore_Changed = typeFromHandle.GetMethod("Changed");
			MethodInfo IGeneratedStore_ChangeTransaction = typeFromHandle.GetMethod("ChangeTransaction");
			MethodBuilder onReload = typeBuilder.DefineMethod("<>OnReload", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.HideBySig, null, Type.EmptyTypes);
			typeBuilder.DefineMethodOverride(onReload, IGeneratedStore_OnReload);
			if (baseOnReload != null)
			{
				typeBuilder.DefineMethodOverride(onReload, baseOnReload);
			}
			ILGenerator il2 = onReload.GetILGenerator();
			if (baseOnReload != null)
			{
				il2.Emit(OpCodes.Ldarg_0);
				il2.Emit(OpCodes.Tailcall);
				il2.Emit(OpCodes.Call, baseOnReload);
			}
			il2.Emit(OpCodes.Ret);
			PropertyBuilder implProp = typeBuilder.DefineProperty("Impl", PropertyAttributes.None, typeof(GeneratedStoreImpl.Impl), null);
			MethodBuilder implPropGet = typeBuilder.DefineMethod("<g>Impl", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.SpecialName, implProp.PropertyType, Type.EmptyTypes);
			implProp.SetGetMethod(implPropGet);
			typeBuilder.DefineMethodOverride(implPropGet, IGeneratedStore_GetImpl);
			ILGenerator ilgenerator4 = implPropGet.GetILGenerator();
			ilgenerator4.Emit(OpCodes.Ldarg_0);
			ilgenerator4.Emit(OpCodes.Ldfld, implField);
			ilgenerator4.Emit(OpCodes.Ret);
			PropertyBuilder typeProp = typeBuilder.DefineProperty("Type", PropertyAttributes.None, typeof(Type), null);
			MethodBuilder typePropGet = typeBuilder.DefineMethod("<g>Type", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.SpecialName, typeProp.PropertyType, Type.EmptyTypes);
			typeProp.SetGetMethod(typePropGet);
			typeBuilder.DefineMethodOverride(typePropGet, IGeneratedStore_GetType);
			ILGenerator ilgenerator5 = typePropGet.GetILGenerator();
			ilgenerator5.Emit(OpCodes.Ldarg_0);
			ilgenerator5.Emit(OpCodes.Ldfld, typeField);
			ilgenerator5.Emit(OpCodes.Ret);
			PropertyBuilder parentProp = typeBuilder.DefineProperty("Parent", PropertyAttributes.None, typeof(GeneratedStoreImpl.IGeneratedStore), null);
			MethodBuilder parentPropGet = typeBuilder.DefineMethod("<g>Parent", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.SpecialName, parentProp.PropertyType, Type.EmptyTypes);
			parentProp.SetGetMethod(parentPropGet);
			typeBuilder.DefineMethodOverride(parentPropGet, IGeneratedStore_GetParent);
			ILGenerator ilgenerator6 = parentPropGet.GetILGenerator();
			ilgenerator6.Emit(OpCodes.Ldarg_0);
			ilgenerator6.Emit(OpCodes.Ldfld, parentField);
			ilgenerator6.Emit(OpCodes.Ret);
			MethodBuilder serializeGen = typeBuilder.DefineMethod("<>Serialize", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.SpecialName, IGeneratedStore_Serialize.ReturnType, Type.EmptyTypes);
			typeBuilder.DefineMethodOverride(serializeGen, IGeneratedStore_Serialize);
			ILGenerator ilgenerator7 = serializeGen.GetILGenerator();
			GeneratedStoreImpl.LocalAllocator GetLocal2 = GeneratedStoreImpl.MakeLocalAllocator(ilgenerator7);
			GeneratedStoreImpl.EmitSerializeStructure(ilgenerator7, structure, GetLocal2, new Action<ILGenerator>(GeneratedStoreImpl.GetMethodThis), new Action<ILGenerator>(GeneratedStoreImpl.GetMethodThis));
			ilgenerator7.Emit(OpCodes.Ret);
			MethodBuilder deserializeGen = typeBuilder.DefineMethod("<>Deserialize", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.SpecialName, null, new Type[] { IGeneratedStore_Deserialize.GetParameters()[0].ParameterType });
			typeBuilder.DefineMethodOverride(deserializeGen, IGeneratedStore_Deserialize);
			ILGenerator il3 = deserializeGen.GetILGenerator();
			Type Map_t = typeof(Map);
			Map_t.GetMethod("TryGetValue");
			MethodInfo Object_GetType = typeof(object).GetMethod("GetType");
			LocalBuilder valueLocal = il3.DeclareLocal(typeof(Value));
			LocalBuilder mapLocal = il3.DeclareLocal(typeof(Map));
			Label nonNull = il3.DefineLabel();
			il3.Emit(OpCodes.Ldarg_1);
			il3.Emit(OpCodes.Brtrue, nonNull);
			GeneratedStoreImpl.EmitLogError(il3, "Attempting to deserialize null", true, null, null);
			il3.Emit(OpCodes.Ret);
			il3.MarkLabel(nonNull);
			il3.Emit(OpCodes.Ldarg_1);
			il3.Emit(OpCodes.Isinst, Map_t);
			il3.Emit(OpCodes.Dup);
			il3.Emit(OpCodes.Stloc, mapLocal);
			Label notMapError = il3.DefineLabel();
			il3.Emit(OpCodes.Brtrue, notMapError);
			GeneratedStoreImpl.EmitLogError(il3, "Invalid root for deserializing " + type.FullName, true, delegate(ILGenerator il)
			{
				GeneratedStoreImpl.EmitTypeof(il, Map_t);
			}, delegate(ILGenerator il)
			{
				il.Emit(OpCodes.Ldarg_1);
				il.Emit(OpCodes.Callvirt, Object_GetType);
			});
			il3.Emit(OpCodes.Ret);
			il3.MarkLabel(notMapError);
			GeneratedStoreImpl.LocalAllocator GetLocal3 = GeneratedStoreImpl.MakeLocalAllocator(il3);
			GeneratedStoreImpl.EmitDeserializeStructure(il3, structure, mapLocal, valueLocal, GetLocal3, new Action<ILGenerator>(GeneratedStoreImpl.GetMethodThis), new Action<ILGenerator>(GeneratedStoreImpl.GetMethodThis));
			if (notifyChanged != null)
			{
				foreach (GeneratedStoreImpl.SerializedMemberInfo member2 in structure)
				{
					il3.Emit(OpCodes.Ldarg_0);
					il3.Emit(OpCodes.Ldstr, member2.Name);
					il3.Emit(OpCodes.Call, notifyChanged);
				}
			}
			il3.Emit(OpCodes.Ret);
			typeBuilder.AddInterfaceImplementation(typeof(IConfigStore));
			Type typeFromHandle2 = typeof(IConfigStore);
			MethodInfo IConfigStore_GetSyncObject = typeFromHandle2.GetProperty("SyncObject").GetGetMethod();
			MethodInfo IConfigStore_GetWriteSyncObject = typeFromHandle2.GetProperty("WriteSyncObject").GetGetMethod();
			MethodInfo IConfigStore_WriteTo = typeFromHandle2.GetMethod("WriteTo");
			MethodInfo IConfigStore_ReadFrom = typeFromHandle2.GetMethod("ReadFrom");
			PropertyBuilder syncObjProp = typeBuilder.DefineProperty("SyncObject", PropertyAttributes.None, typeof(WaitHandle), null);
			MethodBuilder syncObjPropGet = typeBuilder.DefineMethod("<g>SyncObject", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.SpecialName, syncObjProp.PropertyType, Type.EmptyTypes);
			syncObjProp.SetGetMethod(syncObjPropGet);
			typeBuilder.DefineMethodOverride(syncObjPropGet, IConfigStore_GetSyncObject);
			ILGenerator ilgenerator8 = syncObjPropGet.GetILGenerator();
			ilgenerator8.Emit(OpCodes.Ldarg_0);
			ilgenerator8.Emit(OpCodes.Tailcall);
			ilgenerator8.Emit(OpCodes.Call, GeneratedStoreImpl.Impl.ImplGetSyncObjectMethod);
			ilgenerator8.Emit(OpCodes.Ret);
			PropertyBuilder writeSyncObjProp = typeBuilder.DefineProperty("WriteSyncObject", PropertyAttributes.None, typeof(WaitHandle), null);
			MethodBuilder writeSyncObjPropGet = typeBuilder.DefineMethod("<g>WriteSyncObject", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.SpecialName, writeSyncObjProp.PropertyType, Type.EmptyTypes);
			writeSyncObjProp.SetGetMethod(writeSyncObjPropGet);
			typeBuilder.DefineMethodOverride(writeSyncObjPropGet, IConfigStore_GetWriteSyncObject);
			ILGenerator ilgenerator9 = writeSyncObjPropGet.GetILGenerator();
			ilgenerator9.Emit(OpCodes.Ldarg_0);
			ilgenerator9.Emit(OpCodes.Tailcall);
			ilgenerator9.Emit(OpCodes.Call, GeneratedStoreImpl.Impl.ImplGetWriteSyncObjectMethod);
			ilgenerator9.Emit(OpCodes.Ret);
			MethodBuilder writeTo = typeBuilder.DefineMethod("<>WriteTo", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.HideBySig, null, new Type[] { typeof(ConfigProvider) });
			typeBuilder.DefineMethodOverride(writeTo, IConfigStore_WriteTo);
			ILGenerator ilgenerator10 = writeTo.GetILGenerator();
			ilgenerator10.Emit(OpCodes.Ldarg_0);
			ilgenerator10.Emit(OpCodes.Ldarg_1);
			ilgenerator10.Emit(OpCodes.Tailcall);
			ilgenerator10.Emit(OpCodes.Call, GeneratedStoreImpl.Impl.ImplWriteToMethod);
			ilgenerator10.Emit(OpCodes.Ret);
			MethodBuilder readFrom = typeBuilder.DefineMethod("<>ReadFrom", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.HideBySig, null, new Type[] { typeof(ConfigProvider) });
			typeBuilder.DefineMethodOverride(readFrom, IConfigStore_ReadFrom);
			ILGenerator ilgenerator11 = readFrom.GetILGenerator();
			ilgenerator11.Emit(OpCodes.Ldarg_0);
			ilgenerator11.Emit(OpCodes.Ldarg_1);
			ilgenerator11.Emit(OpCodes.Tailcall);
			ilgenerator11.Emit(OpCodes.Call, GeneratedStoreImpl.Impl.ImplReadFromMethod);
			ilgenerator11.Emit(OpCodes.Ret);
			MethodBuilder coreChanged = typeBuilder.DefineMethod("<>Changed", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.HideBySig, null, Type.EmptyTypes);
			typeBuilder.DefineMethodOverride(coreChanged, IGeneratedStore_Changed);
			if (baseChanged != null)
			{
				typeBuilder.DefineMethodOverride(coreChanged, baseChanged);
			}
			ILGenerator il4 = coreChanged.GetILGenerator();
			if (baseChanged != null)
			{
				il4.Emit(OpCodes.Ldarg_0);
				il4.Emit(OpCodes.Call, baseChanged);
			}
			il4.Emit(OpCodes.Ldarg_0);
			il4.Emit(OpCodes.Tailcall);
			il4.Emit(OpCodes.Call, GeneratedStoreImpl.Impl.ImplSignalChangedMethod);
			il4.Emit(OpCodes.Ret);
			MethodBuilder coreChangeTransaction = typeBuilder.DefineMethod("<>ChangeTransaction", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.HideBySig, typeof(IDisposable), Type.EmptyTypes);
			typeBuilder.DefineMethodOverride(coreChangeTransaction, IGeneratedStore_ChangeTransaction);
			if (baseChangeTransaction != null)
			{
				typeBuilder.DefineMethodOverride(coreChangeTransaction, baseChangeTransaction);
			}
			ILGenerator il5 = coreChangeTransaction.GetILGenerator();
			il5.Emit(OpCodes.Ldarg_0);
			if (baseChangeTransaction != null)
			{
				il5.Emit(OpCodes.Ldarg_0);
				il5.Emit(OpCodes.Call, baseChangeTransaction);
			}
			else
			{
				il5.Emit(OpCodes.Ldnull);
			}
			il5.Emit(OpCodes.Tailcall);
			il5.Emit(OpCodes.Call, GeneratedStoreImpl.Impl.ImplChangeTransactionMethod);
			il5.Emit(OpCodes.Ret);
			Type IGeneratedStore_T_t = typeof(GeneratedStoreImpl.IGeneratedStore<>).MakeGenericType(new Type[] { type });
			typeBuilder.AddInterfaceImplementation(IGeneratedStore_T_t);
			MethodInfo IGeneratedStore_T_CopyFrom = IGeneratedStore_T_t.GetMethod("CopyFrom");
			MethodBuilder copyFrom = typeBuilder.DefineMethod("<>CopyFrom", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.HideBySig, null, new Type[]
			{
				type,
				typeof(bool)
			});
			typeBuilder.DefineMethodOverride(copyFrom, IGeneratedStore_T_CopyFrom);
			ILGenerator il6 = copyFrom.GetILGenerator();
			LocalBuilder transactionLocal = il6.DeclareLocal(GeneratedStoreImpl.IDisposable_t);
			Label startLock = il6.DefineLabel();
			il6.Emit(OpCodes.Ldarg_2);
			il6.Emit(OpCodes.Brfalse, startLock);
			il6.Emit(OpCodes.Ldarg_0);
			il6.Emit(OpCodes.Call, coreChangeTransaction);
			il6.Emit(OpCodes.Stloc, transactionLocal);
			il6.MarkLabel(startLock);
			GeneratedStoreImpl.LocalAllocator GetLocal4 = GeneratedStoreImpl.MakeLocalAllocator(il6);
			foreach (GeneratedStoreImpl.SerializedMemberInfo member3 in structure)
			{
				il6.BeginExceptionBlock();
				GeneratedStoreImpl.EmitLoadCorrectStore(il6, member3, false, false, GetLocal4, delegate(ILGenerator il)
				{
					il.Emit(OpCodes.Ldarg_1);
				}, new Action<ILGenerator>(GeneratedStoreImpl.GetMethodThis), new Action<ILGenerator>(GeneratedStoreImpl.GetMethodThis));
				il6.BeginCatchBlock(typeof(Exception));
				GeneratedStoreImpl.EmitWarnException(il6, "Error while copying from member " + member3.Name);
				il6.EndExceptionBlock();
			}
			if (notifyChanged != null)
			{
				foreach (GeneratedStoreImpl.SerializedMemberInfo member4 in structure)
				{
					il6.Emit(OpCodes.Ldarg_0);
					il6.Emit(OpCodes.Ldstr, member4.Name);
					il6.Emit(OpCodes.Call, notifyChanged);
				}
			}
			Label endLock = il6.DefineLabel();
			il6.Emit(OpCodes.Ldarg_2);
			il6.Emit(OpCodes.Brfalse, endLock);
			il6.Emit(OpCodes.Ldloc, transactionLocal);
			il6.Emit(OpCodes.Callvirt, GeneratedStoreImpl.IDisposable_Dispose);
			il6.MarkLabel(endLock);
			il6.Emit(OpCodes.Ret);
			if (baseCopyFrom != null)
			{
				MethodBuilder pubCopyFrom = typeBuilder.DefineMethod(baseCopyFrom.Name, MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.HideBySig, null, new Type[] { type });
				typeBuilder.DefineMethodOverride(pubCopyFrom, baseCopyFrom);
				ILGenerator ilgenerator12 = pubCopyFrom.GetILGenerator();
				ilgenerator12.Emit(OpCodes.Ldarg_0);
				ilgenerator12.Emit(OpCodes.Call, coreChangeTransaction);
				ilgenerator12.Emit(OpCodes.Ldarg_0);
				ilgenerator12.Emit(OpCodes.Ldarg_1);
				ilgenerator12.Emit(OpCodes.Ldc_I4_0);
				ilgenerator12.Emit(OpCodes.Call, copyFrom);
				ilgenerator12.Emit(OpCodes.Ldarg_0);
				ilgenerator12.Emit(OpCodes.Ldarg_1);
				ilgenerator12.Emit(OpCodes.Call, baseCopyFrom);
				ilgenerator12.Emit(OpCodes.Tailcall);
				ilgenerator12.Emit(OpCodes.Callvirt, GeneratedStoreImpl.IDisposable_Dispose);
				ilgenerator12.Emit(OpCodes.Ret);
			}
			foreach (GeneratedStoreImpl.SerializedMemberInfo member5 in structure.Where((GeneratedStoreImpl.SerializedMemberInfo m) => m.IsVirtual))
			{
				PropertyInfo propertyInfo = member5.Member as PropertyInfo;
				MethodInfo get = propertyInfo.GetGetMethod(true);
				MethodInfo set = propertyInfo.GetSetMethod(true);
				PropertyBuilder propBuilder = typeBuilder.DefineProperty(member5.Name + "#", PropertyAttributes.None, member5.Type, null);
				MethodBuilder propGet = typeBuilder.DefineMethod("<g>" + propBuilder.Name, MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.SpecialName, member5.Type, Type.EmptyTypes);
				propBuilder.SetGetMethod(propGet);
				typeBuilder.DefineMethodOverride(propGet, get);
				ILGenerator ilgenerator13 = propGet.GetILGenerator();
				LocalBuilder local = ilgenerator13.DeclareLocal(member5.Type);
				ilgenerator13.Emit(OpCodes.Ldarg_0);
				ilgenerator13.Emit(OpCodes.Call, GeneratedStoreImpl.Impl.ImplTakeReadMethod);
				ilgenerator13.BeginExceptionBlock();
				ilgenerator13.Emit(OpCodes.Ldarg_0);
				ilgenerator13.Emit(OpCodes.Call, get);
				ilgenerator13.Emit(OpCodes.Stloc, local);
				ilgenerator13.BeginFinallyBlock();
				ilgenerator13.Emit(OpCodes.Ldarg_0);
				ilgenerator13.Emit(OpCodes.Call, GeneratedStoreImpl.Impl.ImplReleaseReadMethod);
				ilgenerator13.EndExceptionBlock();
				ilgenerator13.Emit(OpCodes.Ldloc, local);
				ilgenerator13.Emit(OpCodes.Ret);
				MethodBuilder propSet = typeBuilder.DefineMethod("<s>" + propBuilder.Name, MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.HideBySig | MethodAttributes.SpecialName, null, new Type[] { member5.Type });
				propBuilder.SetSetMethod(propSet);
				typeBuilder.DefineMethodOverride(propSet, set);
				ILGenerator il7 = propSet.GetILGenerator();
				LocalBuilder transactionLocal2 = il7.DeclareLocal(GeneratedStoreImpl.IDisposable_t);
				GeneratedStoreImpl.LocalAllocator GetLocal5 = GeneratedStoreImpl.MakeLocalAllocator(il7);
				il7.Emit(OpCodes.Ldarg_0);
				il7.Emit(OpCodes.Call, coreChangeTransaction);
				il7.Emit(OpCodes.Stloc, transactionLocal2);
				il7.BeginExceptionBlock();
				il7.Emit(OpCodes.Ldarg_0);
				il7.Emit(OpCodes.Ldarg_1);
				GeneratedStoreImpl.EmitCorrectMember(il7, member5, false, false, GetLocal5, new Action<ILGenerator>(GeneratedStoreImpl.GetMethodThis), new Action<ILGenerator>(GeneratedStoreImpl.GetMethodThis));
				il7.Emit(OpCodes.Call, set);
				il7.BeginFinallyBlock();
				il7.Emit(OpCodes.Ldloc, transactionLocal2);
				il7.Emit(OpCodes.Callvirt, GeneratedStoreImpl.IDisposable_Dispose);
				il7.EndExceptionBlock();
				if (notifyChanged != null)
				{
					il7.Emit(OpCodes.Ldarg_0);
					il7.Emit(OpCodes.Ldstr, member5.Name);
					il7.Emit(OpCodes.Call, notifyChanged);
				}
				il7.Emit(OpCodes.Ret);
			}
			Type genType = typeBuilder.CreateType();
			ParameterExpression parentParam;
			return new global::System.ValueTuple<GeneratedStoreImpl.GeneratedStoreCreator, Type>(Expression.Lambda<GeneratedStoreImpl.GeneratedStoreCreator>(Expression.New(ctor, new Expression[] { parentParam }), new ParameterExpression[] { parentParam }).Compile(), genType);
		}

		// Token: 0x06000301 RID: 769 RVA: 0x00010B68 File Offset: 0x0000ED68
		private static bool IsMethodInvalid(MethodInfo m, Type ret)
		{
			return !m.IsVirtual || m.ReturnType != ret;
		}

		// Token: 0x06000302 RID: 770 RVA: 0x00010B80 File Offset: 0x0000ED80
		private static bool ProcessAttributesFor(Type type, ref GeneratedStoreImpl.SerializedMemberInfo member)
		{
			object[] attrs = member.Member.GetCustomAttributes(true);
			if (attrs.Select((object o) => o as IgnoreAttribute).NonNull<IgnoreAttribute>().Any<IgnoreAttribute>() || typeof(Delegate).IsAssignableFrom(member.Type))
			{
				return false;
			}
			IEnumerable<NonNullableAttribute> nonNullables = attrs.Select((object o) => o as NonNullableAttribute).NonNull<NonNullableAttribute>();
			member.Name = member.Member.Name;
			member.IsNullable = member.Type.IsGenericType && member.Type.GetGenericTypeDefinition() == typeof(Nullable<>);
			member.AllowNull = !nonNullables.Any<NonNullableAttribute>() && (!member.Type.IsValueType || member.IsNullable);
			SerializedNameAttribute nameAttr = attrs.Select((object o) => o as SerializedNameAttribute).NonNull<SerializedNameAttribute>().FirstOrDefault<SerializedNameAttribute>();
			if (nameAttr != null)
			{
				member.Name = nameAttr.Name;
			}
			member.HasConverter = false;
			UseConverterAttribute converterAttr = attrs.Select((object o) => o as UseConverterAttribute).NonNull<UseConverterAttribute>().FirstOrDefault<UseConverterAttribute>();
			if (converterAttr != null)
			{
				if (converterAttr.UseDefaultConverterForType)
				{
					converterAttr = new UseConverterAttribute(Converter.GetDefaultConverterType(member.Type));
				}
				member.Converter = converterAttr.ConverterType;
				member.IsGenericConverter = converterAttr.IsGenericConverter;
				if (member.Converter.GetConstructor(Type.EmptyTypes) == null)
				{
					Logger.config.Warn(type.FullName + "'s member " + member.Member.Name + " requests a converter that is not default-constructible");
				}
				else if (member.Converter.ContainsGenericParameters)
				{
					Logger.config.Warn(type.FullName + "'s member " + member.Member.Name + " requests a converter that has unfilled type parameters");
				}
				else if (member.Converter.IsInterface || member.Converter.IsAbstract)
				{
					Logger.config.Warn(type.FullName + "'s member " + member.Member.Name + " requests a converter that is not constructible");
				}
				else
				{
					Type targetType = converterAttr.ConverterTargetType;
					if (!member.IsGenericConverter)
					{
						try
						{
							targetType = (Activator.CreateInstance(converterAttr.ConverterType) as IValueConverter).Type;
						}
						catch
						{
							Logger.config.Warn(type.FullName + "'s member " + member.Member.Name + " requests a converter who's target type could not be determined");
							return true;
						}
					}
					if (targetType != member.Type)
					{
						Logger.config.Warn(type.FullName + "'s member " + member.Member.Name + " requests a converter that is not of the member's type");
					}
					else
					{
						member.ConverterTarget = targetType;
						if (member.IsGenericConverter)
						{
							member.ConverterBase = typeof(ValueConverter<>).MakeGenericType(new Type[] { targetType });
						}
						else
						{
							member.ConverterBase = typeof(IValueConverter);
						}
						member.HasConverter = true;
					}
				}
			}
			return true;
		}

		// Token: 0x06000303 RID: 771 RVA: 0x00010F00 File Offset: 0x0000F100
		private static IEnumerable<GeneratedStoreImpl.SerializedMemberInfo> ReadObjectMembers(Type type)
		{
			return GeneratedStoreImpl.objectStructureCache.GetOrAdd(type, (Type t) => GeneratedStoreImpl.ReadObjectMembersInternal(type).ToArray<GeneratedStoreImpl.SerializedMemberInfo>());
		}

		// Token: 0x06000304 RID: 772 RVA: 0x00010F38 File Offset: 0x0000F138
		private static IEnumerable<GeneratedStoreImpl.SerializedMemberInfo> ReadObjectMembersInternal(Type type)
		{
			List<GeneratedStoreImpl.SerializedMemberInfo> structure = new List<GeneratedStoreImpl.SerializedMemberInfo>();
			foreach (PropertyInfo prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				MethodInfo setMethod = prop.GetSetMethod(true);
				if (setMethod != null && !setMethod.IsPrivate)
				{
					MethodInfo getMethod = prop.GetGetMethod(true);
					if (getMethod != null && !getMethod.IsPrivate)
					{
						GeneratedStoreImpl.SerializedMemberInfo serializedMemberInfo = new GeneratedStoreImpl.SerializedMemberInfo();
						serializedMemberInfo.Member = prop;
						MethodInfo getMethod2 = prop.GetGetMethod(true);
						bool flag;
						if (getMethod2 == null || !getMethod2.IsVirtual)
						{
							MethodInfo setMethod2 = prop.GetSetMethod(true);
							flag = setMethod2 != null && setMethod2.IsVirtual;
						}
						else
						{
							flag = true;
						}
						serializedMemberInfo.IsVirtual = flag;
						serializedMemberInfo.IsField = false;
						serializedMemberInfo.Type = prop.PropertyType;
						GeneratedStoreImpl.SerializedMemberInfo smi = serializedMemberInfo;
						if (GeneratedStoreImpl.ProcessAttributesFor(type, ref smi))
						{
							structure.Add(smi);
						}
					}
				}
			}
			foreach (FieldInfo field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (!field.IsPrivate)
				{
					GeneratedStoreImpl.SerializedMemberInfo smi2 = new GeneratedStoreImpl.SerializedMemberInfo
					{
						Member = field,
						IsVirtual = false,
						IsField = true,
						Type = field.FieldType
					};
					if (GeneratedStoreImpl.ProcessAttributesFor(type, ref smi2))
					{
						structure.Add(smi2);
					}
				}
			}
			GeneratedStoreImpl.CreateAndInitializeConvertersFor(type, structure);
			return structure;
		}

		// Token: 0x06000305 RID: 773 RVA: 0x00011068 File Offset: 0x0000F268
		private static void EmitSerializeMember(ILGenerator il, GeneratedStoreImpl.SerializedMemberInfo member, GeneratedStoreImpl.LocalAllocator GetLocal, Action<ILGenerator> thisarg, Action<ILGenerator> parentobj)
		{
			GeneratedStoreImpl.EmitLoad(il, member, thisarg);
			Label endSerialize = il.DefineLabel();
			if (member.AllowNull)
			{
				Label passedNull = il.DefineLabel();
				il.Emit(OpCodes.Dup);
				if (member.IsNullable)
				{
					il.Emit(OpCodes.Call, member.Nullable_HasValue.GetGetMethod());
				}
				il.Emit(OpCodes.Brtrue, passedNull);
				il.Emit(OpCodes.Pop);
				il.Emit(OpCodes.Ldnull);
				il.Emit(OpCodes.Br, endSerialize);
				il.MarkLabel(passedNull);
			}
			if (member.IsNullable)
			{
				il.Emit(OpCodes.Call, member.Nullable_Value.GetGetMethod());
			}
			Type memberConversionType = member.ConversionType;
			Type targetType = GeneratedStoreImpl.GetExpectedValueTypeForType(memberConversionType);
			if (member.HasConverter)
			{
				using (GeneratedStoreImpl.AllocatedLocal stlocal2 = GetLocal.Allocate(member.Type))
				{
					using (GeneratedStoreImpl.AllocatedLocal valLocal = GetLocal.Allocate(typeof(Value)))
					{
						il.Emit(OpCodes.Stloc, stlocal2);
						il.BeginExceptionBlock();
						il.Emit(OpCodes.Ldsfld, member.ConverterField);
						il.Emit(OpCodes.Ldloc, stlocal2);
						if (member.IsGenericConverter)
						{
							MethodInfo toValueBase2 = member.ConverterBase.GetMethod("ToValue", new Type[]
							{
								member.ConverterTarget,
								typeof(object)
							});
							MethodInfo toValue = member.Converter.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault((MethodInfo m) => m.GetBaseDefinition() == toValueBase2) ?? toValueBase2;
							il.Emit(OpCodes.Ldarg_0);
							il.Emit(OpCodes.Call, toValue);
						}
						else
						{
							MethodInfo toValueBase = typeof(IValueConverter).GetMethod("ToValue", new Type[]
							{
								typeof(object),
								typeof(object)
							});
							MethodInfo toValue2 = member.Converter.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault((MethodInfo m) => m.GetBaseDefinition() == toValueBase) ?? toValueBase;
							il.Emit(OpCodes.Box);
							il.Emit(OpCodes.Ldarg_0);
							il.Emit(OpCodes.Call, toValue2);
						}
						il.Emit(OpCodes.Stloc, valLocal);
						il.BeginCatchBlock(typeof(Exception));
						GeneratedStoreImpl.EmitWarnException(il, "Error serializing member using converter");
						il.Emit(OpCodes.Ldnull);
						il.Emit(OpCodes.Stloc, valLocal);
						il.EndExceptionBlock();
						il.Emit(OpCodes.Ldloc, valLocal);
						goto IL_06AC;
					}
				}
			}
			if (targetType == typeof(Text))
			{
				MethodInfo TextCreate = typeof(Value).GetMethod("Text");
				if (member.Type == typeof(char))
				{
					MethodInfo strFromChar = typeof(char).GetMethod("ToString", new Type[] { typeof(char) });
					il.Emit(OpCodes.Call, strFromChar);
				}
				il.Emit(OpCodes.Call, TextCreate);
			}
			else if (targetType == typeof(IPA.Config.Data.Boolean))
			{
				MethodInfo BoolCreate = typeof(Value).GetMethod("Bool");
				il.Emit(OpCodes.Call, BoolCreate);
			}
			else if (targetType == typeof(Integer))
			{
				MethodInfo IntCreate = typeof(Value).GetMethod("Integer");
				GeneratedStoreImpl.EmitNumberConvertTo(il, IntCreate.GetParameters()[0].ParameterType, member.Type);
				il.Emit(OpCodes.Call, IntCreate);
			}
			else if (targetType == typeof(FloatingPoint))
			{
				MethodInfo FloatCreate = typeof(Value).GetMethod("Float");
				GeneratedStoreImpl.EmitNumberConvertTo(il, FloatCreate.GetParameters()[0].ParameterType, member.Type);
				il.Emit(OpCodes.Call, FloatCreate);
			}
			else if (targetType == typeof(List))
			{
				Logger.config.Warn(string.Format("Implicit conversions to {0} are not currently implemented", targetType));
				il.Emit(OpCodes.Pop);
				il.Emit(OpCodes.Ldnull);
			}
			else if (targetType == typeof(Map))
			{
				if (!memberConversionType.IsValueType)
				{
					MethodInfo IGeneratedStore_Serialize = typeof(GeneratedStoreImpl.IGeneratedStore).GetMethod("Serialize");
					MethodInfo IGeneratedStoreT_CopyFrom = typeof(GeneratedStoreImpl.IGeneratedStore<>).MakeGenericType(new Type[] { member.Type }).GetMethod("CopyFrom");
					if (!member.IsVirtual)
					{
						Label noCreate = il.DefineLabel();
						using (GeneratedStoreImpl.AllocatedLocal stlocal = GetLocal.Allocate(member.Type))
						{
							il.Emit(OpCodes.Dup);
							il.Emit(OpCodes.Isinst, typeof(GeneratedStoreImpl.IGeneratedStore));
							il.Emit(OpCodes.Brtrue_S, noCreate);
							il.Emit(OpCodes.Stloc, stlocal);
							GeneratedStoreImpl.EmitCreateChildGenerated(il, member.Type, parentobj);
							il.Emit(OpCodes.Dup);
							il.Emit(OpCodes.Ldloc, stlocal);
							il.Emit(OpCodes.Ldc_I4_0);
							il.Emit(OpCodes.Callvirt, IGeneratedStoreT_CopyFrom);
							il.Emit(OpCodes.Dup);
							il.Emit(OpCodes.Stloc, stlocal);
							GeneratedStoreImpl.EmitStore(il, member, delegate(ILGenerator il)
							{
								il.Emit(OpCodes.Ldloc, stlocal);
							}, thisarg);
							il.MarkLabel(noCreate);
						}
					}
					il.Emit(OpCodes.Callvirt, IGeneratedStore_Serialize);
				}
				else
				{
					using (GeneratedStoreImpl.AllocatedLocal valueLocal = GetLocal.Allocate(memberConversionType))
					{
						IEnumerable<GeneratedStoreImpl.SerializedMemberInfo> structure = GeneratedStoreImpl.ReadObjectMembers(memberConversionType);
						if (!structure.Any<GeneratedStoreImpl.SerializedMemberInfo>())
						{
							Logger.config.Warn(string.Concat(new string[]
							{
								"Custom value type ",
								memberConversionType.FullName,
								" (when compiling serialization of ",
								member.Name,
								" on ",
								member.Member.DeclaringType.FullName,
								") has no accessible members"
							}));
							il.Emit(OpCodes.Pop);
						}
						else
						{
							il.Emit(OpCodes.Stloc, valueLocal);
						}
						GeneratedStoreImpl.EmitSerializeStructure(il, structure, GetLocal, delegate(ILGenerator il)
						{
							il.Emit(OpCodes.Ldloca, valueLocal);
						}, parentobj);
					}
				}
			}
			IL_06AC:
			il.MarkLabel(endSerialize);
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0001178C File Offset: 0x0000F98C
		private static void EmitSerializeStructure(ILGenerator il, IEnumerable<GeneratedStoreImpl.SerializedMemberInfo> structure, GeneratedStoreImpl.LocalAllocator GetLocal, Action<ILGenerator> thisarg, Action<ILGenerator> parentobj)
		{
			MethodInfo MapCreate = typeof(Value).GetMethod("Map");
			MethodInfo MapAdd = typeof(Map).GetMethod("Add");
			using (GeneratedStoreImpl.AllocatedLocal mapLocal = GetLocal.Allocate(typeof(Map)))
			{
				using (GeneratedStoreImpl.AllocatedLocal valueLocal = GetLocal.Allocate(typeof(Value)))
				{
					il.Emit(OpCodes.Call, MapCreate);
					il.Emit(OpCodes.Stloc, mapLocal);
					foreach (GeneratedStoreImpl.SerializedMemberInfo mem in structure)
					{
						GeneratedStoreImpl.EmitSerializeMember(il, mem, GetLocal, thisarg, parentobj);
						il.Emit(OpCodes.Stloc, valueLocal);
						il.Emit(OpCodes.Ldloc, mapLocal);
						il.Emit(OpCodes.Ldstr, mem.Name);
						il.Emit(OpCodes.Ldloc, valueLocal);
						il.Emit(OpCodes.Call, MapAdd);
					}
					il.Emit(OpCodes.Ldloc, mapLocal);
				}
			}
		}

		// Token: 0x06000307 RID: 775 RVA: 0x000118E0 File Offset: 0x0000FAE0
		internal static void LogError(Type expected, Type found, string message)
		{
			Logger.config.Notice(message + ((expected == null) ? "" : string.Format(" (expected {0}, found {1})", expected, ((found != null) ? found.ToString() : null) ?? "null")));
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0001192D File Offset: 0x0000FB2D
		internal static void LogWarning(string message)
		{
			Logger.config.Warn(message);
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0001193A File Offset: 0x0000FB3A
		internal static void LogWarningException(Exception exception)
		{
			Logger.config.Warn(exception);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x00011947 File Offset: 0x0000FB47
		private static GeneratedStoreImpl.LocalAllocator MakeLocalAllocator(ILGenerator il)
		{
			return new GeneratedStoreImpl.LocalAllocator(il);
		}

		// Token: 0x0600030B RID: 779 RVA: 0x00011950 File Offset: 0x0000FB50
		private static void EmitLoad(ILGenerator il, GeneratedStoreImpl.SerializedMemberInfo member, Action<ILGenerator> thisarg)
		{
			thisarg(il);
			if (member.IsField)
			{
				il.Emit(OpCodes.Ldfld, member.Member as FieldInfo);
				return;
			}
			MethodInfo getter = (member.Member as PropertyInfo).GetGetMethod();
			if (getter == null)
			{
				throw new InvalidOperationException("Property " + member.Name + " does not have a getter and is not ignored");
			}
			il.Emit(OpCodes.Call, getter);
		}

		// Token: 0x0600030C RID: 780 RVA: 0x000119C4 File Offset: 0x0000FBC4
		private static void EmitStore(ILGenerator il, GeneratedStoreImpl.SerializedMemberInfo member, Action<ILGenerator> value, Action<ILGenerator> thisobj)
		{
			thisobj(il);
			value(il);
			if (member.IsField)
			{
				il.Emit(OpCodes.Stfld, member.Member as FieldInfo);
				return;
			}
			MethodInfo setter = (member.Member as PropertyInfo).GetSetMethod();
			if (setter == null)
			{
				throw new InvalidOperationException("Property " + member.Name + " does not have a setter and is not ignored");
			}
			il.Emit(OpCodes.Call, setter);
		}

		// Token: 0x0600030D RID: 781 RVA: 0x00011A3F File Offset: 0x0000FC3F
		private static void EmitWarnException(ILGenerator il, string v)
		{
			il.Emit(OpCodes.Ldstr, v);
			il.Emit(OpCodes.Call, GeneratedStoreImpl.LogWarningMethod);
			il.Emit(OpCodes.Call, GeneratedStoreImpl.LogWarningExceptionMethod);
		}

		// Token: 0x0600030E RID: 782 RVA: 0x00011A70 File Offset: 0x0000FC70
		private static void EmitLogError(ILGenerator il, string message, bool tailcall = false, Action<ILGenerator> expected = null, Action<ILGenerator> found = null)
		{
			if (expected == null)
			{
				expected = delegate(ILGenerator il)
				{
					il.Emit(OpCodes.Ldnull);
				};
			}
			if (found == null)
			{
				found = delegate(ILGenerator il)
				{
					il.Emit(OpCodes.Ldnull);
				};
			}
			expected(il);
			found(il);
			il.Emit(OpCodes.Ldstr, message);
			if (tailcall)
			{
				il.Emit(OpCodes.Tailcall);
			}
			il.Emit(OpCodes.Call, GeneratedStoreImpl.LogErrorMethod);
		}

		// Token: 0x0600030F RID: 783 RVA: 0x00011AFF File Offset: 0x0000FCFF
		private static void EmitTypeof(ILGenerator il, Type type)
		{
			il.Emit(OpCodes.Ldtoken, type);
			il.Emit(OpCodes.Call, GeneratedStoreImpl.Type_GetTypeFromHandle);
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00011B20 File Offset: 0x0000FD20
		private static void EmitNumberConvertTo(ILGenerator il, Type to, Type from)
		{
			if (to == from)
			{
				return;
			}
			if (to == GeneratedStoreImpl.Decimal_t)
			{
				if (from == typeof(float))
				{
					il.Emit(OpCodes.Newobj, GeneratedStoreImpl.Decimal_FromFloat);
					return;
				}
				if (from == typeof(double))
				{
					il.Emit(OpCodes.Newobj, GeneratedStoreImpl.Decimal_FromDouble);
					return;
				}
				if (from == typeof(long))
				{
					il.Emit(OpCodes.Newobj, GeneratedStoreImpl.Decimal_FromLong);
					return;
				}
				if (from == typeof(ulong))
				{
					il.Emit(OpCodes.Newobj, GeneratedStoreImpl.Decimal_FromULong);
					return;
				}
				if (from == typeof(int))
				{
					il.Emit(OpCodes.Newobj, GeneratedStoreImpl.Decimal_FromInt);
					return;
				}
				if (from == typeof(uint))
				{
					il.Emit(OpCodes.Newobj, GeneratedStoreImpl.Decimal_FromUInt);
					return;
				}
				if (from == typeof(IntPtr))
				{
					GeneratedStoreImpl.EmitNumberConvertTo(il, typeof(long), from);
					GeneratedStoreImpl.EmitNumberConvertTo(il, to, typeof(long));
					return;
				}
				if (from == typeof(UIntPtr))
				{
					GeneratedStoreImpl.EmitNumberConvertTo(il, typeof(ulong), from);
					GeneratedStoreImpl.EmitNumberConvertTo(il, to, typeof(ulong));
					return;
				}
				GeneratedStoreImpl.EmitNumberConvertTo(il, typeof(int), from);
				GeneratedStoreImpl.EmitNumberConvertTo(il, to, typeof(int));
				return;
			}
			else if (from == GeneratedStoreImpl.Decimal_t)
			{
				if (to == typeof(IntPtr))
				{
					GeneratedStoreImpl.EmitNumberConvertTo(il, typeof(long), from);
					GeneratedStoreImpl.EmitNumberConvertTo(il, to, typeof(long));
					return;
				}
				if (to == typeof(UIntPtr))
				{
					GeneratedStoreImpl.EmitNumberConvertTo(il, typeof(ulong), from);
					GeneratedStoreImpl.EmitNumberConvertTo(il, to, typeof(ulong));
					return;
				}
				MethodInfo method = GeneratedStoreImpl.Decimal_t.GetMethod("To" + to.Name);
				il.Emit(OpCodes.Call, method);
				return;
			}
			else
			{
				if (to == typeof(IntPtr))
				{
					il.Emit(OpCodes.Conv_I);
					return;
				}
				if (to == typeof(UIntPtr))
				{
					il.Emit(OpCodes.Conv_U);
					return;
				}
				if (to == typeof(sbyte))
				{
					il.Emit(OpCodes.Conv_I1);
					return;
				}
				if (to == typeof(byte))
				{
					il.Emit(OpCodes.Conv_U1);
					return;
				}
				if (to == typeof(short))
				{
					il.Emit(OpCodes.Conv_I2);
					return;
				}
				if (to == typeof(ushort))
				{
					il.Emit(OpCodes.Conv_U2);
					return;
				}
				if (to == typeof(int))
				{
					il.Emit(OpCodes.Conv_I4);
					return;
				}
				if (to == typeof(uint))
				{
					il.Emit(OpCodes.Conv_U4);
					return;
				}
				if (to == typeof(long))
				{
					il.Emit(OpCodes.Conv_I8);
					return;
				}
				if (to == typeof(ulong))
				{
					il.Emit(OpCodes.Conv_U8);
					return;
				}
				if (to == typeof(float))
				{
					if (from == typeof(byte) || from == typeof(ushort) || from == typeof(uint) || from == typeof(ulong) || from == typeof(UIntPtr))
					{
						il.Emit(OpCodes.Conv_R_Un);
					}
					il.Emit(OpCodes.Conv_R4);
					return;
				}
				if (to == typeof(double))
				{
					if (from == typeof(byte) || from == typeof(ushort) || from == typeof(uint) || from == typeof(ulong) || from == typeof(UIntPtr))
					{
						il.Emit(OpCodes.Conv_R_Un);
					}
					il.Emit(OpCodes.Conv_R8);
				}
				return;
			}
		}

		// Token: 0x06000311 RID: 785 RVA: 0x00011F7C File Offset: 0x0001017C
		private static void EmitCreateChildGenerated(ILGenerator il, Type childType, Action<ILGenerator> parentobj)
		{
			MethodInfo method = GeneratedStoreImpl.CreateGParent.MakeGenericMethod(new Type[] { childType });
			parentobj(il);
			il.Emit(OpCodes.Call, method);
		}

		// Token: 0x06000312 RID: 786 RVA: 0x00011FB4 File Offset: 0x000101B4
		private static Type GetExpectedValueTypeForType(Type valT)
		{
			if (typeof(Value).IsAssignableFrom(valT))
			{
				return valT;
			}
			if (valT == typeof(string) || valT == typeof(char))
			{
				return typeof(Text);
			}
			if (valT == typeof(bool))
			{
				return typeof(IPA.Config.Data.Boolean);
			}
			if (valT == typeof(byte) || valT == typeof(sbyte) || valT == typeof(short) || valT == typeof(ushort) || valT == typeof(int) || valT == typeof(uint) || valT == typeof(long) || valT == typeof(IntPtr))
			{
				return typeof(Integer);
			}
			if (valT == typeof(float) || valT == typeof(double) || valT == typeof(decimal) || valT == typeof(ulong) || valT == typeof(UIntPtr))
			{
				return typeof(FloatingPoint);
			}
			if (typeof(IEnumerable).IsAssignableFrom(valT))
			{
				return typeof(List);
			}
			return typeof(Map);
		}

		// Token: 0x0400010B RID: 267
		private static readonly MethodInfo CreateGParent = typeof(GeneratedStoreImpl).GetMethod("Create", BindingFlags.Static | BindingFlags.NonPublic, null, CallingConventions.Any, new Type[] { typeof(GeneratedStoreImpl.IGeneratedStore) }, Array.Empty<ParameterModifier>());

		// Token: 0x0400010C RID: 268
		[global::System.Runtime.CompilerServices.TupleElementNames(new string[] { "ctor", "type" })]
		private static readonly SingleCreationValueCache<Type, global::System.ValueTuple<GeneratedStoreImpl.GeneratedStoreCreator, Type>> generatedCreators = new SingleCreationValueCache<Type, global::System.ValueTuple<GeneratedStoreImpl.GeneratedStoreCreator, Type>>();

		// Token: 0x0400010D RID: 269
		internal const string GeneratedAssemblyName = "IPA.Config.Generated";

		// Token: 0x0400010E RID: 270
		private static AssemblyBuilder assembly = null;

		// Token: 0x0400010F RID: 271
		private static ModuleBuilder module = null;

		// Token: 0x04000110 RID: 272
		private static readonly Dictionary<Type, Dictionary<Type, FieldInfo>> TypeRequiredConverters = new Dictionary<Type, Dictionary<Type, FieldInfo>>();

		// Token: 0x04000111 RID: 273
		private static readonly SingleCreationValueCache<Type, GeneratedStoreImpl.SerializedMemberInfo[]> objectStructureCache = new SingleCreationValueCache<Type, GeneratedStoreImpl.SerializedMemberInfo[]>();

		// Token: 0x04000112 RID: 274
		private static readonly MethodInfo LogErrorMethod = typeof(GeneratedStoreImpl).GetMethod("LogError", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04000113 RID: 275
		private static readonly MethodInfo LogWarningMethod = typeof(GeneratedStoreImpl).GetMethod("LogWarning", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04000114 RID: 276
		private static readonly MethodInfo LogWarningExceptionMethod = typeof(GeneratedStoreImpl).GetMethod("LogWarningException", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x04000115 RID: 277
		private static readonly MethodInfo Type_GetTypeFromHandle = typeof(Type).GetMethod("GetTypeFromHandle");

		// Token: 0x04000116 RID: 278
		private static readonly Type IDisposable_t = typeof(IDisposable);

		// Token: 0x04000117 RID: 279
		private static readonly MethodInfo IDisposable_Dispose = GeneratedStoreImpl.IDisposable_t.GetMethod("Dispose");

		// Token: 0x04000118 RID: 280
		private static readonly Type Decimal_t = typeof(decimal);

		// Token: 0x04000119 RID: 281
		private static readonly ConstructorInfo Decimal_FromFloat = GeneratedStoreImpl.Decimal_t.GetConstructor(new Type[] { typeof(float) });

		// Token: 0x0400011A RID: 282
		private static readonly ConstructorInfo Decimal_FromDouble = GeneratedStoreImpl.Decimal_t.GetConstructor(new Type[] { typeof(double) });

		// Token: 0x0400011B RID: 283
		private static readonly ConstructorInfo Decimal_FromInt = GeneratedStoreImpl.Decimal_t.GetConstructor(new Type[] { typeof(int) });

		// Token: 0x0400011C RID: 284
		private static readonly ConstructorInfo Decimal_FromUInt = GeneratedStoreImpl.Decimal_t.GetConstructor(new Type[] { typeof(uint) });

		// Token: 0x0400011D RID: 285
		private static readonly ConstructorInfo Decimal_FromLong = GeneratedStoreImpl.Decimal_t.GetConstructor(new Type[] { typeof(long) });

		// Token: 0x0400011E RID: 286
		private static readonly ConstructorInfo Decimal_FromULong = GeneratedStoreImpl.Decimal_t.GetConstructor(new Type[] { typeof(ulong) });

		// Token: 0x02000128 RID: 296
		// (Invoke) Token: 0x060005E6 RID: 1510
		internal delegate Value SerializeObject<T>(T obj);

		// Token: 0x02000129 RID: 297
		// (Invoke) Token: 0x060005EA RID: 1514
		internal delegate T DeserializeObject<T>(Value val, object parent);

		// Token: 0x0200012A RID: 298
		private static class DelegateStore<T>
		{
			// Token: 0x040003E0 RID: 992
			public static GeneratedStoreImpl.SerializeObject<T> Serialize;

			// Token: 0x040003E1 RID: 993
			public static GeneratedStoreImpl.DeserializeObject<T> Deserialize;
		}

		// Token: 0x0200012B RID: 299
		internal interface IGeneratedStore
		{
			// Token: 0x170000E8 RID: 232
			// (get) Token: 0x060005ED RID: 1517
			Type Type { get; }

			// Token: 0x170000E9 RID: 233
			// (get) Token: 0x060005EE RID: 1518
			GeneratedStoreImpl.IGeneratedStore Parent { get; }

			// Token: 0x170000EA RID: 234
			// (get) Token: 0x060005EF RID: 1519
			GeneratedStoreImpl.Impl Impl { get; }

			// Token: 0x060005F0 RID: 1520
			void OnReload();

			// Token: 0x060005F1 RID: 1521
			void Changed();

			// Token: 0x060005F2 RID: 1522
			IDisposable ChangeTransaction();

			// Token: 0x060005F3 RID: 1523
			Value Serialize();

			// Token: 0x060005F4 RID: 1524
			void Deserialize(Value val);
		}

		// Token: 0x0200012C RID: 300
		internal interface IGeneratedStore<T> : GeneratedStoreImpl.IGeneratedStore where T : class
		{
			// Token: 0x060005F5 RID: 1525
			void CopyFrom(T source, bool useLock);
		}

		// Token: 0x0200012D RID: 301
		internal interface IGeneratedPropertyChanged : INotifyPropertyChanged
		{
			// Token: 0x170000EB RID: 235
			// (get) Token: 0x060005F6 RID: 1526
			PropertyChangedEventHandler PropertyChangedEvent { get; }
		}

		// Token: 0x0200012E RID: 302
		internal class Impl : IConfigStore
		{
			// Token: 0x060005F7 RID: 1527 RVA: 0x000179E9 File Offset: 0x00015BE9
			public Impl(GeneratedStoreImpl.IGeneratedStore store)
			{
				this.generated = store;
			}

			// Token: 0x170000EC RID: 236
			// (get) Token: 0x060005F8 RID: 1528 RVA: 0x00017A0F File Offset: 0x00015C0F
			public WaitHandle SyncObject
			{
				get
				{
					return this.resetEvent;
				}
			}

			// Token: 0x060005F9 RID: 1529 RVA: 0x00017A17 File Offset: 0x00015C17
			public static WaitHandle ImplGetSyncObject(GeneratedStoreImpl.IGeneratedStore s)
			{
				return GeneratedStoreImpl.Impl.FindImpl(s).SyncObject;
			}

			// Token: 0x170000ED RID: 237
			// (get) Token: 0x060005FA RID: 1530 RVA: 0x00017A24 File Offset: 0x00015C24
			public ReaderWriterLockSlim WriteSyncObject { get; } = new ReaderWriterLockSlim();

			// Token: 0x060005FB RID: 1531 RVA: 0x00017A2C File Offset: 0x00015C2C
			public static ReaderWriterLockSlim ImplGetWriteSyncObject(GeneratedStoreImpl.IGeneratedStore s)
			{
				GeneratedStoreImpl.Impl impl = GeneratedStoreImpl.Impl.FindImpl(s);
				if (impl == null)
				{
					return null;
				}
				return impl.WriteSyncObject;
			}

			// Token: 0x060005FC RID: 1532 RVA: 0x00017A3F File Offset: 0x00015C3F
			public static void ImplSignalChanged(GeneratedStoreImpl.IGeneratedStore s)
			{
				GeneratedStoreImpl.Impl.FindImpl(s).SignalChanged();
			}

			// Token: 0x060005FD RID: 1533 RVA: 0x00017A4C File Offset: 0x00015C4C
			public void SignalChanged()
			{
				try
				{
					this.resetEvent.Set();
				}
				catch (ObjectDisposedException e)
				{
					Logger config = Logger.config;
					string text = "ObjectDisposedException while signalling a change for generated store {0}";
					GeneratedStoreImpl.IGeneratedStore generatedStore = this.generated;
					config.Error(string.Format(text, (generatedStore != null) ? generatedStore.GetType() : null));
					Logger.config.Error(e);
				}
			}

			// Token: 0x060005FE RID: 1534 RVA: 0x00017AAC File Offset: 0x00015CAC
			public static void ImplInvokeChanged(GeneratedStoreImpl.IGeneratedStore s)
			{
				GeneratedStoreImpl.Impl.FindImpl(s).InvokeChanged();
			}

			// Token: 0x060005FF RID: 1535 RVA: 0x00017AB9 File Offset: 0x00015CB9
			public void InvokeChanged()
			{
				this.generated.Changed();
			}

			// Token: 0x06000600 RID: 1536 RVA: 0x00017AC6 File Offset: 0x00015CC6
			public static void ImplTakeRead(GeneratedStoreImpl.IGeneratedStore s)
			{
				GeneratedStoreImpl.Impl.FindImpl(s).TakeRead();
			}

			// Token: 0x06000601 RID: 1537 RVA: 0x00017AD3 File Offset: 0x00015CD3
			public void TakeRead()
			{
				if (!this.WriteSyncObject.IsWriteLockHeld)
				{
					this.WriteSyncObject.EnterReadLock();
				}
			}

			// Token: 0x06000602 RID: 1538 RVA: 0x00017AED File Offset: 0x00015CED
			public static void ImplReleaseRead(GeneratedStoreImpl.IGeneratedStore s)
			{
				GeneratedStoreImpl.Impl.FindImpl(s).ReleaseRead();
			}

			// Token: 0x06000603 RID: 1539 RVA: 0x00017AFA File Offset: 0x00015CFA
			public void ReleaseRead()
			{
				if (!this.WriteSyncObject.IsWriteLockHeld)
				{
					this.WriteSyncObject.ExitReadLock();
				}
			}

			// Token: 0x06000604 RID: 1540 RVA: 0x00017B14 File Offset: 0x00015D14
			public static void ImplTakeWrite(GeneratedStoreImpl.IGeneratedStore s)
			{
				GeneratedStoreImpl.Impl.FindImpl(s).TakeWrite();
			}

			// Token: 0x06000605 RID: 1541 RVA: 0x00017B21 File Offset: 0x00015D21
			public void TakeWrite()
			{
				this.WriteSyncObject.EnterWriteLock();
			}

			// Token: 0x06000606 RID: 1542 RVA: 0x00017B2E File Offset: 0x00015D2E
			public static void ImplReleaseWrite(GeneratedStoreImpl.IGeneratedStore s)
			{
				GeneratedStoreImpl.Impl.FindImpl(s).ReleaseWrite();
			}

			// Token: 0x06000607 RID: 1543 RVA: 0x00017B3B File Offset: 0x00015D3B
			public void ReleaseWrite()
			{
				this.WriteSyncObject.ExitWriteLock();
			}

			// Token: 0x06000608 RID: 1544 RVA: 0x00017B48 File Offset: 0x00015D48
			public static IDisposable ImplChangeTransaction(GeneratedStoreImpl.IGeneratedStore s, IDisposable nest)
			{
				return GeneratedStoreImpl.Impl.FindImpl(s).ChangeTransaction(nest, true);
			}

			// Token: 0x06000609 RID: 1545 RVA: 0x00017B57 File Offset: 0x00015D57
			public IDisposable ChangeTransaction(IDisposable nest, bool takeWrite = true)
			{
				return this.GetFreeTransaction().InitWith(this, !this.inChangeTransaction, nest, takeWrite && !this.WriteSyncObject.IsWriteLockHeld);
			}

			// Token: 0x0600060A RID: 1546 RVA: 0x00017B83 File Offset: 0x00015D83
			private GeneratedStoreImpl.Impl.ChangeTransactionObj GetFreeTransaction()
			{
				if (GeneratedStoreImpl.Impl.freeTransactionObjs.Count <= 0)
				{
					return new GeneratedStoreImpl.Impl.ChangeTransactionObj();
				}
				return GeneratedStoreImpl.Impl.freeTransactionObjs.Pop();
			}

			// Token: 0x0600060B RID: 1547 RVA: 0x00017BA2 File Offset: 0x00015DA2
			public static GeneratedStoreImpl.Impl FindImpl(GeneratedStoreImpl.IGeneratedStore store)
			{
				while (((store != null) ? store.Parent : null) != null)
				{
					store = store.Parent;
				}
				if (store == null)
				{
					return null;
				}
				return store.Impl;
			}

			// Token: 0x0600060C RID: 1548 RVA: 0x00017BC7 File Offset: 0x00015DC7
			public static void ImplReadFrom(GeneratedStoreImpl.IGeneratedStore s, ConfigProvider provider)
			{
				GeneratedStoreImpl.Impl.FindImpl(s).ReadFrom(provider);
			}

			// Token: 0x0600060D RID: 1549 RVA: 0x00017BD8 File Offset: 0x00015DD8
			public void ReadFrom(ConfigProvider provider)
			{
				Logger.config.Debug(string.Format("Generated impl ReadFrom {0}", this.generated.GetType()));
				Value values = provider.Load();
				this.generated.Deserialize(values);
				using (this.generated.ChangeTransaction())
				{
					this.generated.OnReload();
				}
			}

			// Token: 0x0600060E RID: 1550 RVA: 0x00017C4C File Offset: 0x00015E4C
			public static void ImplWriteTo(GeneratedStoreImpl.IGeneratedStore s, ConfigProvider provider)
			{
				GeneratedStoreImpl.Impl.FindImpl(s).WriteTo(provider);
			}

			// Token: 0x0600060F RID: 1551 RVA: 0x00017C5C File Offset: 0x00015E5C
			public void WriteTo(ConfigProvider provider)
			{
				Logger.config.Debug(string.Format("Generated impl WriteTo {0}", this.generated.GetType()));
				Value values = this.generated.Serialize();
				provider.Store(values);
			}

			// Token: 0x040003E2 RID: 994
			private readonly GeneratedStoreImpl.IGeneratedStore generated;

			// Token: 0x040003E3 RID: 995
			private bool inChangeTransaction;

			// Token: 0x040003E4 RID: 996
			internal static ConstructorInfo Ctor = typeof(GeneratedStoreImpl.Impl).GetConstructor(new Type[] { typeof(GeneratedStoreImpl.IGeneratedStore) });

			// Token: 0x040003E5 RID: 997
			private readonly AutoResetEvent resetEvent = new AutoResetEvent(false);

			// Token: 0x040003E6 RID: 998
			internal static MethodInfo ImplGetSyncObjectMethod = typeof(GeneratedStoreImpl.Impl).GetMethod("ImplGetSyncObject");

			// Token: 0x040003E8 RID: 1000
			internal static MethodInfo ImplGetWriteSyncObjectMethod = typeof(GeneratedStoreImpl.Impl).GetMethod("ImplGetWriteSyncObject");

			// Token: 0x040003E9 RID: 1001
			internal static MethodInfo ImplSignalChangedMethod = typeof(GeneratedStoreImpl.Impl).GetMethod("ImplSignalChanged");

			// Token: 0x040003EA RID: 1002
			internal static MethodInfo ImplInvokeChangedMethod = typeof(GeneratedStoreImpl.Impl).GetMethod("ImplInvokeChanged");

			// Token: 0x040003EB RID: 1003
			internal static MethodInfo ImplTakeReadMethod = typeof(GeneratedStoreImpl.Impl).GetMethod("ImplTakeRead");

			// Token: 0x040003EC RID: 1004
			internal static MethodInfo ImplReleaseReadMethod = typeof(GeneratedStoreImpl.Impl).GetMethod("ImplReleaseRead");

			// Token: 0x040003ED RID: 1005
			internal static MethodInfo ImplTakeWriteMethod = typeof(GeneratedStoreImpl.Impl).GetMethod("ImplTakeWrite");

			// Token: 0x040003EE RID: 1006
			internal static MethodInfo ImplReleaseWriteMethod = typeof(GeneratedStoreImpl.Impl).GetMethod("ImplReleaseWrite");

			// Token: 0x040003EF RID: 1007
			internal static MethodInfo ImplChangeTransactionMethod = typeof(GeneratedStoreImpl.Impl).GetMethod("ImplChangeTransaction");

			// Token: 0x040003F0 RID: 1008
			private static readonly Stack<GeneratedStoreImpl.Impl.ChangeTransactionObj> freeTransactionObjs = new Stack<GeneratedStoreImpl.Impl.ChangeTransactionObj>();

			// Token: 0x040003F1 RID: 1009
			internal static MethodInfo ImplReadFromMethod = typeof(GeneratedStoreImpl.Impl).GetMethod("ImplReadFrom");

			// Token: 0x040003F2 RID: 1010
			internal static MethodInfo ImplWriteToMethod = typeof(GeneratedStoreImpl.Impl).GetMethod("ImplWriteTo");

			// Token: 0x02000162 RID: 354
			private sealed class ChangeTransactionObj : IDisposable
			{
				// Token: 0x060006CA RID: 1738 RVA: 0x00018A81 File Offset: 0x00016C81
				public GeneratedStoreImpl.Impl.ChangeTransactionObj InitWith(GeneratedStoreImpl.Impl impl, bool owning, IDisposable nest, bool takeWrite)
				{
					this.data = new GeneratedStoreImpl.Impl.ChangeTransactionObj.Data(impl, owning, takeWrite, nest);
					if (this.data.owns)
					{
						impl.inChangeTransaction = true;
					}
					if (this.data.ownsWrite)
					{
						impl.TakeWrite();
					}
					return this;
				}

				// Token: 0x060006CB RID: 1739 RVA: 0x00018ABB File Offset: 0x00016CBB
				public void Dispose()
				{
					this.Dispose(true);
				}

				// Token: 0x060006CC RID: 1740 RVA: 0x00018AC4 File Offset: 0x00016CC4
				private void Dispose(bool addToStore)
				{
					if (this.data.owns)
					{
						this.data.impl.inChangeTransaction = false;
						this.data.impl.InvokeChanged();
					}
					IDisposable nested = this.data.nested;
					if (nested != null)
					{
						nested.Dispose();
					}
					try
					{
						if (this.data.ownsWrite)
						{
							this.data.impl.ReleaseWrite();
						}
					}
					catch
					{
					}
					if (addToStore)
					{
						GeneratedStoreImpl.Impl.freeTransactionObjs.Push(this);
					}
				}

				// Token: 0x060006CD RID: 1741 RVA: 0x00018B58 File Offset: 0x00016D58
				~ChangeTransactionObj()
				{
					this.Dispose(false);
				}

				// Token: 0x04000472 RID: 1138
				private GeneratedStoreImpl.Impl.ChangeTransactionObj.Data data;

				// Token: 0x02000164 RID: 356
				private struct Data
				{
					// Token: 0x060006D0 RID: 1744 RVA: 0x00018B98 File Offset: 0x00016D98
					public Data(GeneratedStoreImpl.Impl impl, bool owning, bool takeWrite, IDisposable nest)
					{
						this.impl = impl;
						this.owns = owning;
						this.ownsWrite = takeWrite;
						this.nested = nest;
					}

					// Token: 0x04000475 RID: 1141
					public readonly GeneratedStoreImpl.Impl impl;

					// Token: 0x04000476 RID: 1142
					public readonly bool owns;

					// Token: 0x04000477 RID: 1143
					public readonly bool ownsWrite;

					// Token: 0x04000478 RID: 1144
					public readonly IDisposable nested;
				}
			}
		}

		// Token: 0x0200012F RID: 303
		// (Invoke) Token: 0x06000612 RID: 1554
		internal delegate IConfigStore GeneratedStoreCreator(GeneratedStoreImpl.IGeneratedStore parent);

		// Token: 0x02000130 RID: 304
		private class SerializedMemberInfo
		{
			// Token: 0x170000EE RID: 238
			// (get) Token: 0x06000615 RID: 1557 RVA: 0x00017DED File Offset: 0x00015FED
			public Type NullableWrappedType
			{
				get
				{
					return Nullable.GetUnderlyingType(this.Type);
				}
			}

			// Token: 0x170000EF RID: 239
			// (get) Token: 0x06000616 RID: 1558 RVA: 0x00017DFA File Offset: 0x00015FFA
			public PropertyInfo Nullable_HasValue
			{
				get
				{
					return this.Type.GetProperty("HasValue");
				}
			}

			// Token: 0x170000F0 RID: 240
			// (get) Token: 0x06000617 RID: 1559 RVA: 0x00017E0C File Offset: 0x0001600C
			public PropertyInfo Nullable_Value
			{
				get
				{
					return this.Type.GetProperty("Value");
				}
			}

			// Token: 0x170000F1 RID: 241
			// (get) Token: 0x06000618 RID: 1560 RVA: 0x00017E1E File Offset: 0x0001601E
			public ConstructorInfo Nullable_Construct
			{
				get
				{
					return this.Type.GetConstructor(new Type[] { this.NullableWrappedType });
				}
			}

			// Token: 0x170000F2 RID: 242
			// (get) Token: 0x06000619 RID: 1561 RVA: 0x00017E3A File Offset: 0x0001603A
			public Type ConversionType
			{
				get
				{
					if (!this.IsNullable)
					{
						return this.Type;
					}
					return this.NullableWrappedType;
				}
			}

			// Token: 0x040003F3 RID: 1011
			public string Name;

			// Token: 0x040003F4 RID: 1012
			public MemberInfo Member;

			// Token: 0x040003F5 RID: 1013
			public Type Type;

			// Token: 0x040003F6 RID: 1014
			public bool AllowNull;

			// Token: 0x040003F7 RID: 1015
			public bool IsVirtual;

			// Token: 0x040003F8 RID: 1016
			public bool IsField;

			// Token: 0x040003F9 RID: 1017
			public bool IsNullable;

			// Token: 0x040003FA RID: 1018
			public bool HasConverter;

			// Token: 0x040003FB RID: 1019
			public bool IsGenericConverter;

			// Token: 0x040003FC RID: 1020
			public Type Converter;

			// Token: 0x040003FD RID: 1021
			public Type ConverterBase;

			// Token: 0x040003FE RID: 1022
			public Type ConverterTarget;

			// Token: 0x040003FF RID: 1023
			public FieldInfo ConverterField;
		}

		// Token: 0x02000131 RID: 305
		private struct AllocatedLocal : IDisposable
		{
			// Token: 0x170000F3 RID: 243
			// (get) Token: 0x0600061B RID: 1563 RVA: 0x00017E59 File Offset: 0x00016059
			public readonly LocalBuilder Local { get; }

			// Token: 0x0600061C RID: 1564 RVA: 0x00017E61 File Offset: 0x00016061
			public AllocatedLocal(GeneratedStoreImpl.LocalAllocator alloc, LocalBuilder builder)
			{
				this.allocator = alloc;
				this.Local = builder;
			}

			// Token: 0x0600061D RID: 1565 RVA: 0x00017E71 File Offset: 0x00016071
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public static implicit operator LocalBuilder(GeneratedStoreImpl.AllocatedLocal loc)
			{
				return loc.Local;
			}

			// Token: 0x0600061E RID: 1566 RVA: 0x00017E7A File Offset: 0x0001607A
			public void Dealloc()
			{
				this.allocator.Deallocate(this);
			}

			// Token: 0x0600061F RID: 1567 RVA: 0x00017E8D File Offset: 0x0001608D
			public void Dispose()
			{
				this.Dealloc();
			}

			// Token: 0x04000400 RID: 1024
			internal readonly GeneratedStoreImpl.LocalAllocator allocator;
		}

		// Token: 0x02000132 RID: 306
		private sealed class LocalAllocator
		{
			// Token: 0x06000620 RID: 1568 RVA: 0x00017E95 File Offset: 0x00016095
			public LocalAllocator(ILGenerator il)
			{
				this.ilSource = il;
			}

			// Token: 0x06000621 RID: 1569 RVA: 0x00017EB0 File Offset: 0x000160B0
			private Stack<LocalBuilder> GetLocalListForType(Type type)
			{
				Stack<LocalBuilder> list;
				if (!this.unallocatedLocals.TryGetValue(type, out list))
				{
					this.unallocatedLocals.Add(type, list = new Stack<LocalBuilder>());
				}
				return list;
			}

			// Token: 0x06000622 RID: 1570 RVA: 0x00017EE4 File Offset: 0x000160E4
			public GeneratedStoreImpl.AllocatedLocal Allocate(Type type)
			{
				Stack<LocalBuilder> list = this.GetLocalListForType(type);
				if (list.Count < 1)
				{
					list.Push(this.ilSource.DeclareLocal(type));
				}
				return new GeneratedStoreImpl.AllocatedLocal(this, list.Pop());
			}

			// Token: 0x06000623 RID: 1571 RVA: 0x00017F20 File Offset: 0x00016120
			public void Deallocate(GeneratedStoreImpl.AllocatedLocal loc)
			{
				this.GetLocalListForType(loc.Local.LocalType).Push(loc.Local);
			}

			// Token: 0x04000402 RID: 1026
			private readonly ILGenerator ilSource;

			// Token: 0x04000403 RID: 1027
			private readonly Dictionary<Type, Stack<LocalBuilder>> unallocatedLocals = new Dictionary<Type, Stack<LocalBuilder>>();
		}
	}
}
