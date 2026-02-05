using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IPA.Config;
using IPA.Loader.Features;
using IPA.Logging;
using IPA.Netvios;
using IPA.Utilities;
using Microsoft.CSharp.RuntimeBinder;
using Mono.Cecil;
using Mono.Collections.Generic;
using Newtonsoft.Json;
using SemVer;
using UnityEngine;

namespace IPA.Loader
{
	/// <summary>
	/// A type to manage the loading of plugins.
	/// </summary>
	// Token: 0x02000045 RID: 69
	internal class PluginLoader
	{
		// Token: 0x060001B9 RID: 441 RVA: 0x00007A3D File Offset: 0x00005C3D
		internal static Task LoadTask()
		{
			return Task.Run(delegate
			{
				PluginLoader.YeetIfNeeded();
				PluginLoader.LoadMetadata();
				PluginLoader.Resolve();
				PluginLoader.ComputeLoadOrder();
				PluginLoader.FilterUnauthorized();
				PluginLoader.FilterDisabled();
				PluginLoader.FilterWithoutFiles();
				PluginLoader.ResolveDependencies();
			});
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00007A64 File Offset: 0x00005C64
		internal static void YeetIfNeeded()
		{
			string pluginDir = UnityGame.PluginsPath;
			if (SelfConfig.YeetMods_ && UnityGame.IsGameVersionBoundary)
			{
				string oldPluginsName = Path.Combine(UnityGame.InstallPath, string.Format("Old {0} Plugins", UnityGame.OldVersion));
				string newPluginsName = Path.Combine(UnityGame.InstallPath, string.Format("Old {0} Plugins", UnityGame.GameVersion));
				if (Directory.Exists(oldPluginsName))
				{
					Directory.Delete(oldPluginsName, true);
				}
				Directory.Move(pluginDir, oldPluginsName);
				if (Directory.Exists(newPluginsName))
				{
					Directory.Move(newPluginsName, pluginDir);
					return;
				}
				Directory.CreateDirectory(pluginDir);
			}
		}

		// Token: 0x060001BB RID: 443 RVA: 0x00007AE8 File Offset: 0x00005CE8
		internal static void LoadMetadata()
		{
			string[] plugins = Directory.GetFiles(UnityGame.PluginsPath, "*.dll");
			try
			{
				PluginMetadata selfMeta = new PluginMetadata
				{
					Assembly = Assembly.GetExecutingAssembly(),
					HashStr = IPA.Netvios.Utils.GetMD5HashFromFile(Path.Combine(UnityGame.InstallPath, "IPA.exe")),
					File = new FileInfo(Path.Combine(UnityGame.InstallPath, "IPA.exe")),
					PluginType = null,
					IsSelf = true
				};
				Stream manifestResourceStream = selfMeta.Assembly.GetManifestResourceStream(typeof(PluginLoader), "manifest.json");
				if (manifestResourceStream == null)
				{
					throw new InvalidOperationException();
				}
				string manifest;
				using (StreamReader manifestReader = new StreamReader(manifestResourceStream))
				{
					manifest = manifestReader.ReadToEnd();
				}
				selfMeta.Manifest = JsonConvert.DeserializeObject<PluginManifest>(manifest);
				PluginLoader.PluginsMetadata.Add(selfMeta);
			}
			catch (Exception e)
			{
				Logger.loader.Critical("Error loading own manifest");
				Logger.loader.Critical(e);
				Application.Unload();
			}
			foreach (string plugin in plugins)
			{
				PluginMetadata metadata = new PluginMetadata
				{
					HashStr = IPA.Netvios.Utils.GetMD5HashFromFile(Path.Combine(UnityGame.PluginsPath, plugin)),
					File = new FileInfo(Path.Combine(UnityGame.PluginsPath, plugin)),
					IsSelf = false
				};
				try
				{
					PluginLoader.<>c__DisplayClass6_0 CS$<>8__locals1;
					CS$<>8__locals1.pluginModule = AssemblyDefinition.ReadAssembly(plugin, new ReaderParameters
					{
						ReadingMode = ReadingMode.Immediate,
						ReadWrite = false,
						AssemblyResolver = new CecilLibLoader()
					}).MainModule;
					string pluginNs = "";
					foreach (Resource resource in CS$<>8__locals1.pluginModule.Resources)
					{
						EmbeddedResource embedded = resource as EmbeddedResource;
						if (embedded != null && embedded.Name.EndsWith(".manifest.json"))
						{
							pluginNs = embedded.Name.Substring(0, embedded.Name.Length - ".manifest.json".Length);
							string manifest2;
							using (StreamReader manifestReader2 = new StreamReader(embedded.GetResourceStream()))
							{
								manifest2 = manifestReader2.ReadToEnd();
							}
							metadata.Manifest = JsonConvert.DeserializeObject<PluginManifest>(manifest2);
							break;
						}
					}
					if (metadata.Manifest == null)
					{
						Logger.loader.Notice("No manifest.json in " + Path.GetFileName(plugin));
					}
					else
					{
						PluginManifest.MiscObject misc = metadata.Manifest.Misc;
						string hint = ((misc != null) ? misc.PluginMainHint : null);
						if (hint != null && CS$<>8__locals1.pluginModule.GetType(hint) != null)
						{
							PluginLoader.<LoadMetadata>g__TryGetNamespacedPluginType|6_0(hint, metadata, ref CS$<>8__locals1);
						}
						if (metadata.PluginType == null)
						{
							PluginLoader.<LoadMetadata>g__TryGetNamespacedPluginType|6_0(pluginNs, metadata, ref CS$<>8__locals1);
						}
						if (metadata.PluginType == null)
						{
							Logger.loader.Error(string.Concat(new string[]
							{
								"No plugin found in the manifest ",
								(hint != null) ? ("hint path (" + hint + ") or ") : "",
								"namespace (",
								pluginNs,
								") in ",
								Path.GetFileName(plugin)
							}));
						}
						else
						{
							Logger.loader.Debug("Adding info for " + Path.GetFileName(plugin));
							PluginLoader.PluginsMetadata.Add(metadata);
						}
					}
				}
				catch (Exception e2)
				{
					Logger.loader.Error("Could not load data for plugin " + Path.GetFileName(plugin));
					Logger.loader.Error(e2);
					PluginLoader.ignoredPlugins.Add(metadata, new IgnoreReason(Reason.Error, null, null, null)
					{
						ReasonText = "An error ocurred loading the data",
						Error = e2
					});
					Application.Unload();
				}
			}
			foreach (string manifest3 in Directory.GetFiles(UnityGame.PluginsPath, "*.json").Concat(Directory.GetFiles(UnityGame.PluginsPath, "*.manifest")))
			{
				try
				{
					string hashStr = "";
					string plugin2 = Path.ChangeExtension(Path.Combine(UnityGame.LibraryPath, Path.GetFileName(manifest3)), "dll");
					if (File.Exists(plugin2))
					{
						hashStr = IPA.Netvios.Utils.GetMD5HashFromFile(plugin2);
					}
					PluginMetadata metadata2 = new PluginMetadata
					{
						HashStr = hashStr,
						File = new FileInfo(Path.Combine(UnityGame.PluginsPath, manifest3)),
						IsSelf = false,
						IsBare = true
					};
					metadata2.Manifest = JsonConvert.DeserializeObject<PluginManifest>(File.ReadAllText(manifest3));
					if (metadata2.Manifest.Files.Length < 1)
					{
						Logger.loader.Warn("Bare manifest " + Path.GetFileName(manifest3) + " does not declare any files. Dependency resolution and verification cannot be completed.");
					}
					Logger.loader.Debug("Adding info for bare manifest " + Path.GetFileName(manifest3));
					PluginLoader.PluginsMetadata.Add(metadata2);
				}
				catch (Exception e3)
				{
					Logger.loader.Error("Could not load data for bare manifest " + Path.GetFileName(manifest3));
					Logger.loader.Error(e3);
					Application.Unload();
				}
			}
			foreach (PluginMetadata meta in PluginLoader.PluginsMetadata)
			{
				string[] lines = meta.Manifest.Description.Split(new char[] { '\n' });
				Match i = PluginLoader.embeddedTextDescriptionPattern.Match(lines[0]);
				if (i.Success)
				{
					if (!meta.IsBare)
					{
						string name = i.Groups[1].Value;
						if (meta.IsSelf)
						{
							goto IL_0684;
						}
						EmbeddedResource resc = meta.PluginType.Module.Resources.Select((Resource r) => r as EmbeddedResource).NonNull<EmbeddedResource>().FirstOrDefault((EmbeddedResource r) => r.Name == name);
						if (resc == null)
						{
							Logger.loader.Warn(string.Concat(new string[] { "Could not find description file for plugin ", meta.Name, " (", name, "); ignoring include" }));
							meta.Manifest.Description = string.Join("\n", lines.Skip(1).StrJP());
							continue;
						}
						string description;
						using (StreamReader reader = new StreamReader(resc.GetResourceStream()))
						{
							description = reader.ReadToEnd();
							goto IL_06B5;
						}
						goto IL_0684;
						IL_06B5:
						meta.Manifest.Description = description;
						continue;
						IL_0684:
						using (StreamReader descriptionReader = new StreamReader(meta.Assembly.GetManifestResourceStream(name)))
						{
							description = descriptionReader.ReadToEnd();
						}
						goto IL_06B5;
					}
					Logger.loader.Warn("Bare manifest cannot specify description file");
					meta.Manifest.Description = string.Join("\n", lines.Skip(1).StrJP());
				}
			}
		}

		// Token: 0x060001BC RID: 444 RVA: 0x000082C8 File Offset: 0x000064C8
		internal static void Resolve()
		{
			PluginLoader.PluginsMetadata.Sort((PluginMetadata a, PluginMetadata b) => b.Version.CompareTo(a.Version));
			HashSet<string> ids = new HashSet<string>();
			Dictionary<PluginMetadata, IgnoreReason> ignore = new Dictionary<PluginMetadata, IgnoreReason>();
			List<PluginMetadata> resolved = new List<PluginMetadata>(PluginLoader.PluginsMetadata.Count);
			using (List<PluginMetadata>.Enumerator enumerator = PluginLoader.PluginsMetadata.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PluginMetadata meta = enumerator.Current;
					if (meta.Id != null)
					{
						if (ids.Contains(meta.Id))
						{
							Logger.loader.Warn("Found duplicates of " + meta.Id + ", using newest");
							IgnoreReason ireason = new IgnoreReason(Reason.Duplicate, null, null, null)
							{
								ReasonText = "Duplicate entry of same ID (" + meta.Id + ")",
								RelatedTo = resolved.First((PluginMetadata p) => p.Id == meta.Id)
							};
							ignore.Add(meta, ireason);
							PluginLoader.ignoredPlugins.Add(meta, ireason);
							continue;
						}
						bool processedLater = false;
						foreach (PluginMetadata meta2 in PluginLoader.PluginsMetadata)
						{
							if (!ignore.ContainsKey(meta2))
							{
								if (meta == meta2)
								{
									processedLater = true;
								}
								else if (meta2.Manifest.Conflicts.ContainsKey(meta.Id) && meta2.Manifest.Conflicts[meta.Id].IsSatisfied(meta.Version))
								{
									Logger.loader.Warn(string.Format("{0}@{1} conflicts with {2}", meta.Id, meta.Version, meta2.Id));
									if (!processedLater)
									{
										Logger.loader.Warn("Ignoring " + meta.Name);
										ignore.Add(meta, new IgnoreReason(Reason.Conflict, null, null, null)
										{
											ReasonText = string.Format("{0}@{1} conflicts with {2}", meta2.Id, meta2.Version, meta.Id),
											RelatedTo = meta2
										});
										break;
									}
									Logger.loader.Warn("Ignoring " + meta2.Name);
									ignore.Add(meta2, new IgnoreReason(Reason.Conflict, null, null, null)
									{
										ReasonText = string.Format("{0}@{1} conflicts with {2}", meta.Id, meta.Version, meta2.Id),
										RelatedTo = meta
									});
								}
							}
						}
					}
					IgnoreReason reason;
					if (ignore.TryGetValue(meta, out reason))
					{
						PluginLoader.ignoredPlugins.Add(meta, reason);
					}
					else
					{
						if (meta.Id != null)
						{
							ids.Add(meta.Id);
						}
						resolved.Add(meta);
					}
				}
			}
			PluginLoader.PluginsMetadata = resolved;
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00008670 File Offset: 0x00006870
		private static void RefreshOfficePlugins()
		{
			string url = IPA.Netvios.Config.GetModApiUrlPrefix();
			string res = "";
			try
			{
				res = HttpClientHelper.GetHttpResponseJson(url, null);
				if (string.IsNullOrEmpty(res))
				{
					throw new Exception("response content is empty");
				}
			}
			catch (Exception e)
			{
				Logger loader = Logger.loader;
				string text = "request web api error: ";
				Exception ex = e;
				loader.Error(text + ((ex != null) ? ex.ToString() : null));
				return;
			}
			object pluginJson = JsonConvert.DeserializeObject<object>(res);
			if (PluginLoader.<>o__12.<>p__28 == null)
			{
				PluginLoader.<>o__12.<>p__28 = CallSite<Func<global::System.Runtime.CompilerServices.CallSite, object, IEnumerable>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(IEnumerable), typeof(PluginLoader)));
			}
			foreach (object p in PluginLoader.<>o__12.<>p__28.Target(PluginLoader.<>o__12.<>p__28, pluginJson))
			{
				string text2 = "{0}-{1}";
				if (PluginLoader.<>o__12.<>p__1 == null)
				{
					PluginLoader.<>o__12.<>p__1 = CallSite<Func<global::System.Runtime.CompilerServices.CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(PluginLoader), new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }));
				}
				Func<global::System.Runtime.CompilerServices.CallSite, object, object> target = PluginLoader.<>o__12.<>p__1.Target;
				global::System.Runtime.CompilerServices.CallSite <>p__ = PluginLoader.<>o__12.<>p__1;
				if (PluginLoader.<>o__12.<>p__0 == null)
				{
					PluginLoader.<>o__12.<>p__0 = CallSite<Func<global::System.Runtime.CompilerServices.CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "id", typeof(PluginLoader), new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }));
				}
				object obj = target(<>p__, PluginLoader.<>o__12.<>p__0.Target(PluginLoader.<>o__12.<>p__0, p));
				if (PluginLoader.<>o__12.<>p__3 == null)
				{
					PluginLoader.<>o__12.<>p__3 = CallSite<Func<global::System.Runtime.CompilerServices.CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(PluginLoader), new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }));
				}
				Func<global::System.Runtime.CompilerServices.CallSite, object, object> target2 = PluginLoader.<>o__12.<>p__3.Target;
				global::System.Runtime.CompilerServices.CallSite <>p__2 = PluginLoader.<>o__12.<>p__3;
				if (PluginLoader.<>o__12.<>p__2 == null)
				{
					PluginLoader.<>o__12.<>p__2 = CallSite<Func<global::System.Runtime.CompilerServices.CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "version", typeof(PluginLoader), new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }));
				}
				string key = string.Format(text2, obj, target2(<>p__2, PluginLoader.<>o__12.<>p__2.Target(PluginLoader.<>o__12.<>p__2, p)));
				Dictionary<string, string> dictionary = PluginLoader.officePluginsDictionary;
				string text3 = key;
				if (PluginLoader.<>o__12.<>p__6 == null)
				{
					PluginLoader.<>o__12.<>p__6 = CallSite<Func<global::System.Runtime.CompilerServices.CallSite, object, string>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(PluginLoader)));
				}
				Func<global::System.Runtime.CompilerServices.CallSite, object, string> target3 = PluginLoader.<>o__12.<>p__6.Target;
				global::System.Runtime.CompilerServices.CallSite <>p__3 = PluginLoader.<>o__12.<>p__6;
				if (PluginLoader.<>o__12.<>p__5 == null)
				{
					PluginLoader.<>o__12.<>p__5 = CallSite<Func<global::System.Runtime.CompilerServices.CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(PluginLoader), new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }));
				}
				Func<global::System.Runtime.CompilerServices.CallSite, object, object> target4 = PluginLoader.<>o__12.<>p__5.Target;
				global::System.Runtime.CompilerServices.CallSite <>p__4 = PluginLoader.<>o__12.<>p__5;
				if (PluginLoader.<>o__12.<>p__4 == null)
				{
					PluginLoader.<>o__12.<>p__4 = CallSite<Func<global::System.Runtime.CompilerServices.CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "key", typeof(PluginLoader), new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }));
				}
				dictionary[text3] = target3(<>p__3, target4(<>p__4, PluginLoader.<>o__12.<>p__4.Target(PluginLoader.<>o__12.<>p__4, p)));
				if (PluginLoader.<>o__12.<>p__10 == null)
				{
					PluginLoader.<>o__12.<>p__10 = CallSite<Func<global::System.Runtime.CompilerServices.CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(PluginLoader), new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }));
				}
				Func<global::System.Runtime.CompilerServices.CallSite, object, bool> target5 = PluginLoader.<>o__12.<>p__10.Target;
				global::System.Runtime.CompilerServices.CallSite <>p__5 = PluginLoader.<>o__12.<>p__10;
				if (PluginLoader.<>o__12.<>p__9 == null)
				{
					PluginLoader.<>o__12.<>p__9 = CallSite<Func<global::System.Runtime.CompilerServices.CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(PluginLoader), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<global::System.Runtime.CompilerServices.CallSite, object, string, object> target6 = PluginLoader.<>o__12.<>p__9.Target;
				global::System.Runtime.CompilerServices.CallSite <>p__6 = PluginLoader.<>o__12.<>p__9;
				if (PluginLoader.<>o__12.<>p__8 == null)
				{
					PluginLoader.<>o__12.<>p__8 = CallSite<Func<global::System.Runtime.CompilerServices.CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(PluginLoader), new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }));
				}
				Func<global::System.Runtime.CompilerServices.CallSite, object, object> target7 = PluginLoader.<>o__12.<>p__8.Target;
				global::System.Runtime.CompilerServices.CallSite <>p__7 = PluginLoader.<>o__12.<>p__8;
				if (PluginLoader.<>o__12.<>p__7 == null)
				{
					PluginLoader.<>o__12.<>p__7 = CallSite<Func<global::System.Runtime.CompilerServices.CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "id", typeof(PluginLoader), new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }));
				}
				if (target5(<>p__5, target6(<>p__6, target7(<>p__7, PluginLoader.<>o__12.<>p__7.Target(PluginLoader.<>o__12.<>p__7, p)), "CloudSdkPlugin")))
				{
					if (PluginLoader.<>o__12.<>p__13 == null)
					{
						PluginLoader.<>o__12.<>p__13 = CallSite<Action<global::System.Runtime.CompilerServices.CallSite, List<string>, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(PluginLoader), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Action<global::System.Runtime.CompilerServices.CallSite, List<string>, object> target8 = PluginLoader.<>o__12.<>p__13.Target;
					global::System.Runtime.CompilerServices.CallSite <>p__8 = PluginLoader.<>o__12.<>p__13;
					List<string> cloudSDKPluginHashList = PluginLoader.CloudSDKPluginHashList;
					if (PluginLoader.<>o__12.<>p__12 == null)
					{
						PluginLoader.<>o__12.<>p__12 = CallSite<Func<global::System.Runtime.CompilerServices.CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(PluginLoader), new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }));
					}
					Func<global::System.Runtime.CompilerServices.CallSite, object, object> target9 = PluginLoader.<>o__12.<>p__12.Target;
					global::System.Runtime.CompilerServices.CallSite <>p__9 = PluginLoader.<>o__12.<>p__12;
					if (PluginLoader.<>o__12.<>p__11 == null)
					{
						PluginLoader.<>o__12.<>p__11 = CallSite<Func<global::System.Runtime.CompilerServices.CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "key", typeof(PluginLoader), new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }));
					}
					target8(<>p__8, cloudSDKPluginHashList, target9(<>p__9, PluginLoader.<>o__12.<>p__11.Target(PluginLoader.<>o__12.<>p__11, p)));
				}
				if (PluginLoader.<>o__12.<>p__17 == null)
				{
					PluginLoader.<>o__12.<>p__17 = CallSite<Func<global::System.Runtime.CompilerServices.CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(PluginLoader), new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }));
				}
				Func<global::System.Runtime.CompilerServices.CallSite, object, bool> target10 = PluginLoader.<>o__12.<>p__17.Target;
				global::System.Runtime.CompilerServices.CallSite <>p__10 = PluginLoader.<>o__12.<>p__17;
				if (PluginLoader.<>o__12.<>p__16 == null)
				{
					PluginLoader.<>o__12.<>p__16 = CallSite<Func<global::System.Runtime.CompilerServices.CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(PluginLoader), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<global::System.Runtime.CompilerServices.CallSite, object, string, object> target11 = PluginLoader.<>o__12.<>p__16.Target;
				global::System.Runtime.CompilerServices.CallSite <>p__11 = PluginLoader.<>o__12.<>p__16;
				if (PluginLoader.<>o__12.<>p__15 == null)
				{
					PluginLoader.<>o__12.<>p__15 = CallSite<Func<global::System.Runtime.CompilerServices.CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(PluginLoader), new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }));
				}
				Func<global::System.Runtime.CompilerServices.CallSite, object, object> target12 = PluginLoader.<>o__12.<>p__15.Target;
				global::System.Runtime.CompilerServices.CallSite <>p__12 = PluginLoader.<>o__12.<>p__15;
				if (PluginLoader.<>o__12.<>p__14 == null)
				{
					PluginLoader.<>o__12.<>p__14 = CallSite<Func<global::System.Runtime.CompilerServices.CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "id", typeof(PluginLoader), new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }));
				}
				if (target10(<>p__10, target11(<>p__11, target12(<>p__12, PluginLoader.<>o__12.<>p__14.Target(PluginLoader.<>o__12.<>p__14, p)), "NetviosSdkPlugin")))
				{
					if (PluginLoader.<>o__12.<>p__20 == null)
					{
						PluginLoader.<>o__12.<>p__20 = CallSite<Action<global::System.Runtime.CompilerServices.CallSite, List<string>, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(PluginLoader), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Action<global::System.Runtime.CompilerServices.CallSite, List<string>, object> target13 = PluginLoader.<>o__12.<>p__20.Target;
					global::System.Runtime.CompilerServices.CallSite <>p__13 = PluginLoader.<>o__12.<>p__20;
					List<string> netviosSDKPluginHashList = PluginLoader.NetviosSDKPluginHashList;
					if (PluginLoader.<>o__12.<>p__19 == null)
					{
						PluginLoader.<>o__12.<>p__19 = CallSite<Func<global::System.Runtime.CompilerServices.CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(PluginLoader), new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }));
					}
					Func<global::System.Runtime.CompilerServices.CallSite, object, object> target14 = PluginLoader.<>o__12.<>p__19.Target;
					global::System.Runtime.CompilerServices.CallSite <>p__14 = PluginLoader.<>o__12.<>p__19;
					if (PluginLoader.<>o__12.<>p__18 == null)
					{
						PluginLoader.<>o__12.<>p__18 = CallSite<Func<global::System.Runtime.CompilerServices.CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "key", typeof(PluginLoader), new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }));
					}
					target13(<>p__13, netviosSDKPluginHashList, target14(<>p__14, PluginLoader.<>o__12.<>p__18.Target(PluginLoader.<>o__12.<>p__18, p)));
				}
				if (PluginLoader.<>o__12.<>p__24 == null)
				{
					PluginLoader.<>o__12.<>p__24 = CallSite<Func<global::System.Runtime.CompilerServices.CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof(PluginLoader), new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }));
				}
				Func<global::System.Runtime.CompilerServices.CallSite, object, bool> target15 = PluginLoader.<>o__12.<>p__24.Target;
				global::System.Runtime.CompilerServices.CallSite <>p__15 = PluginLoader.<>o__12.<>p__24;
				if (PluginLoader.<>o__12.<>p__23 == null)
				{
					PluginLoader.<>o__12.<>p__23 = CallSite<Func<global::System.Runtime.CompilerServices.CallSite, object, string, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.Equal, typeof(PluginLoader), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, null)
					}));
				}
				Func<global::System.Runtime.CompilerServices.CallSite, object, string, object> target16 = PluginLoader.<>o__12.<>p__23.Target;
				global::System.Runtime.CompilerServices.CallSite <>p__16 = PluginLoader.<>o__12.<>p__23;
				if (PluginLoader.<>o__12.<>p__22 == null)
				{
					PluginLoader.<>o__12.<>p__22 = CallSite<Func<global::System.Runtime.CompilerServices.CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(PluginLoader), new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }));
				}
				Func<global::System.Runtime.CompilerServices.CallSite, object, object> target17 = PluginLoader.<>o__12.<>p__22.Target;
				global::System.Runtime.CompilerServices.CallSite <>p__17 = PluginLoader.<>o__12.<>p__22;
				if (PluginLoader.<>o__12.<>p__21 == null)
				{
					PluginLoader.<>o__12.<>p__21 = CallSite<Func<global::System.Runtime.CompilerServices.CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "id", typeof(PluginLoader), new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }));
				}
				if (target15(<>p__15, target16(<>p__16, target17(<>p__17, PluginLoader.<>o__12.<>p__21.Target(PluginLoader.<>o__12.<>p__21, p)), "PlayerDataPlugin")))
				{
					if (PluginLoader.<>o__12.<>p__27 == null)
					{
						PluginLoader.<>o__12.<>p__27 = CallSite<Action<global::System.Runtime.CompilerServices.CallSite, List<string>, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "Add", null, typeof(PluginLoader), new CSharpArgumentInfo[]
						{
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null),
							CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
						}));
					}
					Action<global::System.Runtime.CompilerServices.CallSite, List<string>, object> target18 = PluginLoader.<>o__12.<>p__27.Target;
					global::System.Runtime.CompilerServices.CallSite <>p__18 = PluginLoader.<>o__12.<>p__27;
					List<string> playerDataPluginHashList = PluginLoader.PlayerDataPluginHashList;
					if (PluginLoader.<>o__12.<>p__26 == null)
					{
						PluginLoader.<>o__12.<>p__26 = CallSite<Func<global::System.Runtime.CompilerServices.CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.None, "ToString", null, typeof(PluginLoader), new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }));
					}
					Func<global::System.Runtime.CompilerServices.CallSite, object, object> target19 = PluginLoader.<>o__12.<>p__26.Target;
					global::System.Runtime.CompilerServices.CallSite <>p__19 = PluginLoader.<>o__12.<>p__26;
					if (PluginLoader.<>o__12.<>p__25 == null)
					{
						PluginLoader.<>o__12.<>p__25 = CallSite<Func<global::System.Runtime.CompilerServices.CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, "key", typeof(PluginLoader), new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }));
					}
					target18(<>p__18, playerDataPluginHashList, target19(<>p__19, PluginLoader.<>o__12.<>p__25.Target(PluginLoader.<>o__12.<>p__25, p)));
				}
			}
			if ((PluginLoader.CloudSDKPluginHashList.Count == 0 && PluginLoader.NetviosSDKPluginHashList.Count == 0) || PluginLoader.PlayerDataPluginHashList.Count == 0)
			{
				Logger.loader.Error("sdk plugin or playerData plugin not included in download plugin");
				Application.Quit();
			}
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00009060 File Offset: 0x00007260
		private static void FilterUnauthorized()
		{
			PluginLoader.RefreshOfficePlugins();
			bool HasSDKPlugin = false;
			bool HasPlayerDataPlugin = false;
			List<PluginMetadata> enabled = new List<PluginMetadata>(PluginLoader.PluginsMetadata.Count);
			foreach (PluginMetadata meta in PluginLoader.PluginsMetadata)
			{
				if (PluginLoader.CloudSDKPluginHashList.Contains(meta.HashStr) || PluginLoader.NetviosSDKPluginHashList.Contains(meta.HashStr))
				{
					HasSDKPlugin = true;
				}
				if (PluginLoader.PlayerDataPluginHashList.Contains(meta.HashStr))
				{
					HasPlayerDataPlugin = true;
				}
				bool passed = true;
				string key = meta.Id.ToString() + "-" + meta.Version.ToString();
				Logger.log.Info(key + "=" + meta.HashStr);
				if (!meta.IsSelf && meta.Name.ToLower() != "dynamicopenvr" && (!PluginLoader.officePluginsDictionary.ContainsKey(key) || PluginLoader.officePluginsDictionary[key] != meta.HashStr))
				{
					passed = false;
					PluginLoader.ignoredPlugins.Add(meta, new IgnoreReason(Reason.Unsupported, null, null, null)
					{
						ReasonText = meta.Name + " Unauthorized"
					});
					Logger.loader.Warn(meta.Name + " Unauthorized");
				}
				if (passed)
				{
					enabled.Add(meta);
				}
			}
			if (!HasSDKPlugin || !HasPlayerDataPlugin)
			{
				Logger.loader.Error("sdk plugin or playerData plugin not included plugin dir");
				Application.Quit();
			}
			PluginLoader.PluginsMetadata = enabled;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00009220 File Offset: 0x00007420
		private static void FilterDisabled()
		{
			List<PluginMetadata> enabled = new List<PluginMetadata>(PluginLoader.PluginsMetadata.Count);
			HashSet<string> disabled = DisabledConfig.Instance.DisabledModIds;
			foreach (PluginMetadata meta in PluginLoader.PluginsMetadata)
			{
				if (disabled.Contains(meta.Id ?? meta.Name))
				{
					PluginLoader.DisabledPlugins.Add(meta);
				}
				else
				{
					enabled.Add(meta);
				}
			}
			PluginLoader.PluginsMetadata = enabled;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x000092B8 File Offset: 0x000074B8
		private static void FilterWithoutFiles()
		{
			List<PluginMetadata> enabled = new List<PluginMetadata>(PluginLoader.PluginsMetadata.Count);
			foreach (PluginMetadata meta in PluginLoader.PluginsMetadata)
			{
				bool passed = true;
				foreach (FileInfo file in meta.AssociatedFiles)
				{
					if (!file.Exists)
					{
						passed = false;
						PluginLoader.ignoredPlugins.Add(meta, new IgnoreReason(Reason.MissingFiles, null, null, null)
						{
							ReasonText = string.Concat(new string[]
							{
								"File ",
								IPA.Utilities.Utils.GetRelativePath(file.FullName, UnityGame.InstallPath),
								" (declared by ",
								meta.Name,
								") does not exist"
							})
						});
						Logger.loader.Warn(string.Concat(new string[]
						{
							"File ",
							IPA.Utilities.Utils.GetRelativePath(file.FullName, UnityGame.InstallPath),
							" (declared by ",
							meta.Name,
							") does not exist! Mod installation is incomplete, not loading it."
						}));
						break;
					}
				}
				if (passed)
				{
					enabled.Add(meta);
				}
			}
			PluginLoader.PluginsMetadata = enabled;
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00009440 File Offset: 0x00007640
		internal static void ComputeLoadOrder()
		{
			HashSet<PluginMetadata> pluginTree = new HashSet<PluginMetadata>();
			foreach (PluginMetadata meta in PluginLoader.PluginsMetadata)
			{
				PluginLoader.<ComputeLoadOrder>g__InsertInto|16_0(pluginTree, meta, true);
			}
			PluginLoader.PluginsMetadata = new List<PluginMetadata>();
			PluginLoader.<ComputeLoadOrder>g__DeTree|16_1(PluginLoader.PluginsMetadata, pluginTree);
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x000094B0 File Offset: 0x000076B0
		internal static void ResolveDependencies()
		{
			List<PluginMetadata> metadata = new List<PluginMetadata>();
			Dictionary<string, global::SemVer.Version> pluginsToLoad = new Dictionary<string, global::SemVer.Version>();
			Dictionary<string, global::SemVer.Version> disabledLookup = PluginLoader.DisabledPlugins.NonNull((PluginMetadata m) => m.Id).ToDictionary((PluginMetadata m) => m.Id, (PluginMetadata m) => m.Version);
			foreach (PluginMetadata meta in PluginLoader.PluginsMetadata)
			{
				List<global::System.ValueTuple<string, Range, bool>> missingDeps = new List<global::System.ValueTuple<string, Range, bool>>();
				foreach (KeyValuePair<string, Range> dep in meta.Manifest.Dependencies)
				{
					if (!pluginsToLoad.ContainsKey(dep.Key) || !dep.Value.IsSatisfied(pluginsToLoad[dep.Key]))
					{
						if (disabledLookup.ContainsKey(dep.Key) && dep.Value.IsSatisfied(disabledLookup[dep.Key]))
						{
							Logger.loader.Warn(string.Concat(new string[] { "Dependency ", dep.Key, " was found, but disabled. Disabling ", meta.Name, " too." }));
							missingDeps.Add(new global::System.ValueTuple<string, Range, bool>(dep.Key, dep.Value, true));
						}
						else
						{
							Logger.loader.Warn(string.Format("{0} is missing dependency {1}@{2}", meta.Name, dep.Key, dep.Value));
							missingDeps.Add(new global::System.ValueTuple<string, Range, bool>(dep.Key, dep.Value, false));
						}
					}
				}
				if (missingDeps.Count == 0)
				{
					metadata.Add(meta);
					if (meta.Id != null)
					{
						pluginsToLoad.Add(meta.Id, meta.Version);
					}
				}
				else if (missingDeps.Any(([global::System.Runtime.CompilerServices.TupleElementNames(new string[] { "id", "version", "disabled" })] global::System.ValueTuple<string, Range, bool> t) => !t.Item3))
				{
					Dictionary<PluginMetadata, IgnoreReason> dictionary = PluginLoader.ignoredPlugins;
					PluginMetadata pluginMetadata = meta;
					IgnoreReason ignoreReason = new IgnoreReason(Reason.Dependency, null, null, null);
					ignoreReason.ReasonText = "Missing dependencies " + string.Join(", ", (from t in missingDeps
						where !t.Item3
						select string.Format("{0}@{1}", t.Item1, t.Item2)).StrJP());
					dictionary.Add(pluginMetadata, ignoreReason);
				}
				else
				{
					PluginLoader.DisabledPlugins.Add(meta);
					DisabledConfig.Instance.DisabledModIds.Add(meta.Id ?? meta.Name);
				}
			}
			DisabledConfig.Instance.Changed();
			PluginLoader.PluginsMetadata = metadata;
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00009800 File Offset: 0x00007A00
		internal static void InitFeatures()
		{
			global::System.ValueTuple<string, Ref<Feature.FeatureParse?>> feature;
			List<global::System.ValueTuple<PluginMetadata, List<global::System.ValueTuple<string, Ref<Feature.FeatureParse?>>>>> parsedFeatures = PluginLoader.PluginsMetadata.Select((PluginMetadata m) => new global::System.ValueTuple<PluginMetadata, List<global::System.ValueTuple<string, Ref<Feature.FeatureParse?>>>>(m, m.Manifest.Features.Select((string feature) => new global::System.ValueTuple<string, Ref<Feature.FeatureParse?>>(feature, Ref.Create<Feature.FeatureParse?>(null))).ToList<global::System.ValueTuple<string, Ref<Feature.FeatureParse?>>>())).ToList<global::System.ValueTuple<PluginMetadata, List<global::System.ValueTuple<string, Ref<Feature.FeatureParse?>>>>>();
			while (DefineFeature.NewFeature)
			{
				DefineFeature.NewFeature = false;
				foreach (global::System.ValueTuple<PluginMetadata, List<global::System.ValueTuple<string, Ref<Feature.FeatureParse?>>>> valueTuple in parsedFeatures)
				{
					PluginMetadata metadata = valueTuple.Item1;
					List<global::System.ValueTuple<string, Ref<Feature.FeatureParse?>>> features = valueTuple.Item2;
					for (int i = 0; i < features.Count; i++)
					{
						feature = features[i];
						Feature featureObj;
						Exception exception;
						bool valid;
						Feature.FeatureParse parsed;
						bool success = Feature.TryParseFeature(feature.Item1, metadata, out featureObj, out exception, out valid, out parsed, feature.Item2.Value);
						if (!success && !valid && featureObj == null && exception == null)
						{
							feature.Item2.Value = new Feature.FeatureParse?(parsed);
						}
						else if (success)
						{
							if (valid && featureObj.StoreOnPlugin)
							{
								metadata.InternalFeatures.Add(featureObj);
							}
							else if (!valid)
							{
								Logger.features.Warn("Feature not valid on " + metadata.Name + ": " + featureObj.InvalidMessage);
							}
							features.RemoveAt(i--);
						}
						else
						{
							Logger.features.Error("Error parsing feature definition on " + metadata.Name);
							Logger.features.Error(exception);
							features.RemoveAt(i--);
						}
					}
				}
				foreach (PluginMetadata pluginMetadata in PluginLoader.PluginsMetadata)
				{
					foreach (Feature feature3 in pluginMetadata.Features)
					{
						feature3.Evaluate();
					}
				}
			}
			foreach (global::System.ValueTuple<PluginMetadata, List<global::System.ValueTuple<string, Ref<Feature.FeatureParse?>>>> plugin in parsedFeatures)
			{
				if (plugin.Item2.Count > 0)
				{
					Logger.features.Warn("On plugin " + plugin.Item1.Name + ":");
					foreach (global::System.ValueTuple<string, Ref<Feature.FeatureParse?>> feature2 in plugin.Item2)
					{
						Logger.features.Warn("    Feature not found with name " + feature2.Item1);
					}
				}
			}
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00009B18 File Offset: 0x00007D18
		internal static void ReleaseAll(bool full = false)
		{
			if (full)
			{
				PluginLoader.ignoredPlugins = new Dictionary<PluginMetadata, IgnoreReason>();
			}
			else
			{
				foreach (PluginMetadata i in PluginLoader.PluginsMetadata)
				{
					PluginLoader.ignoredPlugins.Add(i, new IgnoreReason(Reason.Released, null, null, null));
				}
				foreach (PluginMetadata pluginMetadata in PluginLoader.ignoredPlugins.Keys)
				{
					pluginMetadata.InternalFeatures.Clear();
					pluginMetadata.PluginType = null;
					pluginMetadata.Assembly = null;
				}
			}
			PluginLoader.PluginsMetadata = new List<PluginMetadata>();
			PluginLoader.DisabledPlugins = new List<PluginMetadata>();
			Feature.Reset();
			GC.Collect();
		}

		// Token: 0x060001C5 RID: 453 RVA: 0x00009C00 File Offset: 0x00007E00
		internal static void Load(PluginMetadata meta)
		{
			if (meta.Assembly == null && meta.PluginType != null)
			{
				meta.Assembly = Assembly.LoadFrom(meta.File.FullName);
			}
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00009C30 File Offset: 0x00007E30
		internal static PluginExecutor InitPlugin(PluginMetadata meta, IEnumerable<PluginMetadata> alreadyLoaded)
		{
			if (meta.Manifest.GameVersion != UnityGame.GameVersion)
			{
				Logger.loader.Warn(string.Format("Mod {0} developed for game version {1}, so it may not work properly.", meta.Name, meta.Manifest.GameVersion));
			}
			if (meta.IsSelf)
			{
				return new PluginExecutor(meta, PluginExecutor.Special.Self);
			}
			foreach (PluginMetadata dep in meta.Dependencies)
			{
				if (!alreadyLoaded.Contains(dep))
				{
					IgnoreReason reason;
					if (PluginLoader.ignoredPlugins.TryGetValue(dep, out reason))
					{
						Dictionary<PluginMetadata, IgnoreReason> dictionary = PluginLoader.ignoredPlugins;
						IgnoreReason ignoreReason = new IgnoreReason(Reason.Dependency, null, null, null)
						{
							ReasonText = "Dependency was ignored at load time: " + reason.ReasonText,
							RelatedTo = dep
						};
						dictionary.Add(meta, ignoreReason);
					}
					else
					{
						Dictionary<PluginMetadata, IgnoreReason> dictionary2 = PluginLoader.ignoredPlugins;
						IgnoreReason ignoreReason = new IgnoreReason(Reason.Dependency, null, null, null)
						{
							ReasonText = "Dependency was not already loaded at load time, but was also not ignored",
							RelatedTo = dep
						};
						dictionary2.Add(meta, ignoreReason);
					}
					return null;
				}
			}
			if (meta.IsBare)
			{
				return new PluginExecutor(meta, PluginExecutor.Special.Bare);
			}
			PluginLoader.Load(meta);
			foreach (Feature feature in meta.Features)
			{
				if (!feature.BeforeLoad(meta))
				{
					Logger.loader.Warn(string.Format("Feature {0} denied plugin {1} from loading! {2}", (feature != null) ? feature.GetType() : null, meta.Name, (feature != null) ? feature.InvalidMessage : null));
					Dictionary<PluginMetadata, IgnoreReason> dictionary3 = PluginLoader.ignoredPlugins;
					IgnoreReason ignoreReason = new IgnoreReason(Reason.Feature, null, null, null)
					{
						ReasonText = string.Format("Denied in {0} of feature {1}:\n\t{2}", "BeforeLoad", (feature != null) ? feature.GetType() : null, (feature != null) ? feature.InvalidMessage : null)
					};
					dictionary3.Add(meta, ignoreReason);
					return null;
				}
			}
			PluginExecutor exec;
			try
			{
				exec = new PluginExecutor(meta, PluginExecutor.Special.None);
			}
			catch (Exception e)
			{
				Logger.loader.Error("Error creating executor for " + meta.Name);
				Logger.loader.Error(e);
				return null;
			}
			foreach (Feature feature2 in meta.Features)
			{
				if (!feature2.BeforeInit(meta))
				{
					Logger.loader.Warn(string.Format("Feature {0} denied plugin {1} from initializing! {2}", (feature2 != null) ? feature2.GetType() : null, meta.Name, (feature2 != null) ? feature2.InvalidMessage : null));
					Dictionary<PluginMetadata, IgnoreReason> dictionary4 = PluginLoader.ignoredPlugins;
					IgnoreReason ignoreReason = new IgnoreReason(Reason.Feature, null, null, null)
					{
						ReasonText = string.Format("Denied in {0} of feature {1}:\n\t{2}", "BeforeInit", (feature2 != null) ? feature2.GetType() : null, (feature2 != null) ? feature2.InvalidMessage : null)
					};
					dictionary4.Add(meta, ignoreReason);
					return null;
				}
			}
			try
			{
				exec.Create();
			}
			catch (Exception e2)
			{
				Logger.loader.Error("Could not init plugin " + meta.Name);
				Logger.loader.Error(e2);
				Dictionary<PluginMetadata, IgnoreReason> dictionary5 = PluginLoader.ignoredPlugins;
				IgnoreReason ignoreReason = new IgnoreReason(Reason.Error, null, null, null)
				{
					ReasonText = "Error ocurred while initializing",
					Error = e2
				};
				dictionary5.Add(meta, ignoreReason);
				return null;
			}
			foreach (Feature feature3 in meta.Features)
			{
				try
				{
					feature3.AfterInit(meta, exec.Instance);
				}
				catch (Exception e3)
				{
					Logger.loader.Critical(string.Format("Feature errored in {0}: {1}", "AfterInit", e3));
				}
			}
			return exec;
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x0000A048 File Offset: 0x00008248
		// (set) Token: 0x060001C8 RID: 456 RVA: 0x0000A04F File Offset: 0x0000824F
		internal static bool IsFirstLoadComplete { get; private set; } = false;

		// Token: 0x060001C9 RID: 457 RVA: 0x0000A058 File Offset: 0x00008258
		internal static List<PluginExecutor> LoadPlugins()
		{
			PluginLoader.InitFeatures();
			PluginLoader.DisabledPlugins.ForEach(new Action<PluginMetadata>(PluginLoader.Load));
			List<PluginExecutor> list = new List<PluginExecutor>();
			HashSet<PluginMetadata> loaded = new HashSet<PluginMetadata>();
			foreach (PluginMetadata meta in PluginLoader.PluginsMetadata)
			{
				PluginExecutor exec = PluginLoader.InitPlugin(meta, loaded);
				if (exec != null)
				{
					list.Add(exec);
					loaded.Add(meta);
				}
			}
			PluginLoader.IsFirstLoadComplete = true;
			return list;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000A0F0 File Offset: 0x000082F0
		internal static string DecodeString(string str)
		{
			PluginsData data = JsonUtility.FromJson<PluginsData>(str);
			byte[] key = Encoding.UTF8.GetBytes("4LsPO3fy144b5j6w");
			byte[] iv = Encoding.UTF8.GetBytes("1234567812345678");
			RijndaelManaged rijndaelManaged = new RijndaelManaged();
			rijndaelManaged.Key = key;
			rijndaelManaged.IV = iv;
			rijndaelManaged.Mode = CipherMode.CBC;
			rijndaelManaged.Padding = PaddingMode.PKCS7;
			rijndaelManaged.BlockSize = 128;
			byte[] encryptedData = Convert.FromBase64String(data.content);
			byte[] plainText = rijndaelManaged.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
			return Encoding.UTF8.GetString(plainText);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000A1F0 File Offset: 0x000083F0
		[CompilerGenerated]
		internal static void <LoadMetadata>g__TryGetNamespacedPluginType|6_0(string ns, PluginMetadata meta, ref PluginLoader.<>c__DisplayClass6_0 A_2)
		{
			foreach (TypeDefinition type in A_2.pluginModule.Types)
			{
				if (!(type.Namespace != ns) && type.HasCustomAttributes)
				{
					CustomAttribute attr = type.CustomAttributes.FirstOrDefault((CustomAttribute a) => a.Constructor.DeclaringType.FullName == typeof(PluginAttribute).FullName);
					if (attr != null)
					{
						if (!attr.HasConstructorArguments)
						{
							Logger.loader.Warn("Attribute plugin found in " + type.FullName + ", but attribute has no arguments");
							break;
						}
						Collection<CustomAttributeArgument> args = attr.ConstructorArguments;
						if (args.Count != 1)
						{
							Logger.loader.Warn("Attribute plugin found in " + type.FullName + ", but attribute has unexpected number of arguments");
							break;
						}
						CustomAttributeArgument rtOptionsArg = args[0];
						if (rtOptionsArg.Type.FullName != typeof(RuntimeOptions).FullName)
						{
							Logger.loader.Warn("Attribute plugin found in " + type.FullName + ", but first argument is of unexpected type " + rtOptionsArg.Type.FullName);
							break;
						}
						int rtOptionsValInt = (int)rtOptionsArg.Value;
						meta.RuntimeOptions = (RuntimeOptions)rtOptionsValInt;
						meta.PluginType = type;
						break;
					}
				}
			}
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000A378 File Offset: 0x00008578
		[CompilerGenerated]
		internal static bool <ComputeLoadOrder>g__InsertInto|16_0(HashSet<PluginMetadata> root, PluginMetadata meta, bool isRoot)
		{
			bool inserted = false;
			foreach (PluginMetadata sr in root)
			{
				inserted = inserted || PluginLoader.<ComputeLoadOrder>g__InsertInto|16_0(sr.Dependencies, meta, false);
				if (meta.Id != null)
				{
					if (sr.Manifest.Dependencies.ContainsKey(meta.Id))
					{
						inserted = inserted || sr.Dependencies.Add(meta);
					}
					else if (sr.Manifest.LoadAfter.Contains(meta.Id))
					{
						inserted = inserted || sr.LoadsAfter.Add(meta);
					}
				}
				if (sr.Id != null && meta.Manifest.LoadBefore.Contains(sr.Id))
				{
					inserted = inserted || sr.LoadsAfter.Add(meta);
				}
			}
			if (isRoot)
			{
				foreach (PluginMetadata sr2 in root)
				{
					PluginLoader.<ComputeLoadOrder>g__InsertInto|16_0(meta.Dependencies, sr2, false);
					if (sr2.Id != null)
					{
						if (meta.Manifest.Dependencies.ContainsKey(sr2.Id))
						{
							meta.Dependencies.Add(sr2);
						}
						else if (meta.Manifest.LoadAfter.Contains(sr2.Id))
						{
							meta.LoadsAfter.Add(sr2);
						}
					}
					if (meta.Id != null && sr2.Manifest.LoadBefore.Contains(meta.Id))
					{
						meta.LoadsAfter.Add(sr2);
					}
				}
				root.Add(meta);
			}
			return inserted;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000A548 File Offset: 0x00008748
		[CompilerGenerated]
		internal static void <ComputeLoadOrder>g__DeTree|16_1(List<PluginMetadata> into, HashSet<PluginMetadata> tree)
		{
			foreach (PluginMetadata st in tree)
			{
				if (!into.Contains(st))
				{
					PluginLoader.<ComputeLoadOrder>g__DeTree|16_1(into, st.Dependencies);
					PluginLoader.<ComputeLoadOrder>g__DeTree|16_1(into, st.LoadsAfter);
					into.Add(st);
				}
			}
		}

		// Token: 0x040000A2 RID: 162
		internal static List<PluginMetadata> PluginsMetadata = new List<PluginMetadata>();

		// Token: 0x040000A3 RID: 163
		internal static List<PluginMetadata> DisabledPlugins = new List<PluginMetadata>();

		// Token: 0x040000A4 RID: 164
		internal static Dictionary<string, string> officePluginsDictionary = new Dictionary<string, string>();

		// Token: 0x040000A5 RID: 165
		private static readonly Regex embeddedTextDescriptionPattern = new Regex("#!\\[(.+)\\]", RegexOptions.Compiled | RegexOptions.Singleline);

		// Token: 0x040000A6 RID: 166
		internal static Dictionary<PluginMetadata, IgnoreReason> ignoredPlugins = new Dictionary<PluginMetadata, IgnoreReason>();

		// Token: 0x040000A7 RID: 167
		internal static List<string> CloudSDKPluginHashList = new List<string>();

		// Token: 0x040000A8 RID: 168
		internal static List<string> NetviosSDKPluginHashList = new List<string>();

		// Token: 0x040000A9 RID: 169
		internal static List<string> PlayerDataPluginHashList = new List<string>();
	}
}
