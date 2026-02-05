using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DynamicOpenVR.DefaultBindings;
using DynamicOpenVR.Exceptions;
using DynamicOpenVR.IO;
using DynamicOpenVR.Logging;
using DynamicOpenVR.Manifest;
using Newtonsoft.Json;
using UnityEngine;

namespace DynamicOpenVR
{
	// Token: 0x020000C9 RID: 201
	public class OpenVRActionManager : MonoBehaviour
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600018F RID: 399 RVA: 0x00004A71 File Offset: 0x00002C71
		public static OpenVRActionManager instance
		{
			get
			{
				if (!OpenVRActionManager._instance)
				{
					Logger.Info("Creating instance of OpenVRActionManager");
					GameObject gameObject = new GameObject("OpenVRActionManager");
					Object.DontDestroyOnLoad(gameObject);
					OpenVRActionManager._instance = gameObject.AddComponent<OpenVRActionManager>();
				}
				return OpenVRActionManager._instance;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000190 RID: 400 RVA: 0x00004AA8 File Offset: 0x00002CA8
		public string actionManifestPath
		{
			get
			{
				if (!this.initialized)
				{
					throw new Exception("OpenVRActionManager is not initialized");
				}
				return this._actionManifestPath;
			}
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000191 RID: 401 RVA: 0x00004AC3 File Offset: 0x00002CC3
		// (set) Token: 0x06000192 RID: 402 RVA: 0x00004ACB File Offset: 0x00002CCB
		public bool initialized { get; private set; }

		// Token: 0x06000193 RID: 403 RVA: 0x00004AD4 File Offset: 0x00002CD4
		public void Initialize(string actionManifestPath)
		{
			if (this.initialized)
			{
				throw new InvalidOperationException("Already initialized");
			}
			Logger.Info("Initializing OpenVRActionManager");
			this._actionManifestPath = actionManifestPath;
			this.CombineAndWriteManifest();
			OpenVRWrapper.SetActionManifestPath(actionManifestPath);
			foreach (string text in this._actions.Values.Select((OVRAction action) => action.GetActionSetName()).Distinct<string>())
			{
				this.TryAddActionSet(text);
			}
			foreach (OVRAction ovraction in this._actions.Values.ToList<OVRAction>())
			{
				this.TryUpdateHandle(ovraction);
			}
			this.initialized = true;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00004BD4 File Offset: 0x00002DD4
		public void Update()
		{
			if (!this.initialized)
			{
				return;
			}
			if (this._actionSetHandles != null)
			{
				OpenVRWrapper.UpdateActionState(this._actionSetHandles);
			}
			foreach (OVRInput ovrinput in this._actions.Values.OfType<OVRInput>().ToList<OVRInput>())
			{
				try
				{
					ovrinput.UpdateData();
				}
				catch (OpenVRInputException ex)
				{
					Logger.Error("An unexpected OpenVR error occurred when fetching data for action '" + ovrinput.name + "'. Action has been disabled.");
					Logger.Error(ex);
					this.DeregisterAction(ovrinput);
				}
				catch (NullReferenceException ex2)
				{
					Logger.Error("A null reference exception occurred when fetching data for action '" + ovrinput.name + "'. This is most likely caused by an internal OpenVR issue. Action has been disabled.");
					Logger.Error(ex2);
					this.DeregisterAction(ovrinput);
				}
				catch (Exception ex3)
				{
					Logger.Error("An unexpected error occurred when fetching data for action '" + ovrinput.name + "'. Action has been disabled.");
					Logger.Error(ex3);
					this.DeregisterAction(ovrinput);
				}
			}
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00004CF8 File Offset: 0x00002EF8
		public void RegisterAction(OVRAction action)
		{
			if (this._actions.ContainsKey(action.id))
			{
				throw new InvalidOperationException("Action was already registered.");
			}
			Logger.Trace(string.Concat(new string[] { "Registering action '", action.name, "' (", action.id, ")" }));
			this._actions.Add(action.id, action);
			if (this.initialized)
			{
				string actionSetName = action.GetActionSetName();
				if (!this._actionSetNames.Contains(actionSetName))
				{
					this.TryAddActionSet(actionSetName);
				}
				this.TryUpdateHandle(action);
			}
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00004D9C File Offset: 0x00002F9C
		public void DeregisterAction(OVRAction action)
		{
			Logger.Trace(string.Concat(new string[] { "Deregistering action '", action.name, "' (", action.id, ")" }));
			this._actions.Remove(action.id);
		}

		// Token: 0x06000197 RID: 407 RVA: 0x00004DF8 File Offset: 0x00002FF8
		private void CombineAndWriteManifest()
		{
			string text = Path.Combine(Directory.GetCurrentDirectory(), "DynamicOpenVR", "Actions");
			if (!Directory.Exists(text))
			{
				Logger.Warn("Actions folder does not exist!");
				return;
			}
			Logger.Trace("Reading actions from '" + text + "'");
			string[] files = Directory.GetFiles(text);
			List<ActionManifest> list = new List<ActionManifest>();
			ushort num = 0;
			foreach (string text2 in files)
			{
				try
				{
					Logger.Trace("Reading '" + text2 + "'");
					using (StreamReader streamReader = new StreamReader(text2))
					{
						string text3 = streamReader.ReadToEnd();
						list.Add(JsonConvert.DeserializeObject<ActionManifest>(text3));
						num += BitConverter.ToUInt16(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(text3)), 0);
					}
				}
				catch (Exception ex)
				{
					Logger.Error(string.Concat(new string[]
					{
						"An error of type ",
						ex.GetType().FullName,
						" occured when trying to parse '",
						text2,
						"': ",
						ex.Message
					}));
				}
			}
			List<ManifestDefaultBinding> list2 = this.CombineAndWriteBindings((int)num);
			ActionManifest actionManifest = new ActionManifest();
			actionManifest.version = (ulong)num;
			actionManifest.actions = list.SelectMany((ActionManifest m) => m.actions).ToList<ManifestAction>();
			actionManifest.actionSets = list.SelectMany((ActionManifest m) => m.actionSets).ToList<ManifestActionSet>();
			actionManifest.defaultBindings = list2;
			actionManifest.localization = this.CombineLocalizations(list);
			ActionManifest actionManifest2 = actionManifest;
			foreach (string text4 in actionManifest2.actionSets.Select((ManifestActionSet a) => a.Name))
			{
				Logger.Trace("Found defined action set '" + text4 + "'");
			}
			foreach (string text5 in actionManifest2.actions.Select((ManifestAction a) => a.Name))
			{
				Logger.Trace("Found defined action '" + text5 + "'");
			}
			foreach (string text6 in actionManifest2.defaultBindings.Select((ManifestDefaultBinding a) => a.ControllerType))
			{
				Logger.Trace("Found default binding for controller '" + text6 + "'");
			}
			Logger.Trace("Writing action manifest to '" + this._actionManifestPath + "'");
			using (StreamWriter streamWriter = new StreamWriter(this._actionManifestPath))
			{
				streamWriter.WriteLine(JsonConvert.SerializeObject(actionManifest2, Formatting.Indented));
			}
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000517C File Offset: 0x0000337C
		private void TryUpdateHandle(OVRAction action)
		{
			Logger.Trace(string.Concat(new string[] { "Updating handle for action '", action.name, "' (", action.id, ")" }));
			try
			{
				action.UpdateHandle();
			}
			catch (OpenVRInputException ex)
			{
				Logger.Error("An unexpected OpenVR error occurred when fetching handle for action '" + action.name + "'. Action has been disabled.");
				Logger.Error(ex);
				this.DeregisterAction(action);
			}
			catch (NullReferenceException ex2)
			{
				Logger.Error("A null reference exception occurred when fetching handle for action '" + action.name + "'. This is most likely caused by an internal OpenVR issue. Action has been disabled.");
				Logger.Error(ex2);
				this.DeregisterAction(action);
			}
			catch (Exception ex3)
			{
				Logger.Error("An unexpected error occurred when fetching handle for action '" + action.name + "'. Action has been disabled.");
				Logger.Error(ex3);
				this.DeregisterAction(action);
			}
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000526C File Offset: 0x0000346C
		private void TryAddActionSet(string actionSetName)
		{
			Logger.Trace("Registering action set '" + actionSetName + "'");
			if (this._actionSetNames.Contains(actionSetName))
			{
				throw new InvalidOperationException("Action set '" + actionSetName + "' has already been registered");
			}
			try
			{
				this._actionSetNames.Add(actionSetName);
				this._actionSetHandles.Add(OpenVRWrapper.GetActionSetHandle(actionSetName));
			}
			catch (OpenVRInputException ex)
			{
				Logger.Error("An unexpected OpenVR error occurred when fetching handle for action set '" + actionSetName + "'.");
				Logger.Error(ex);
			}
			catch (NullReferenceException ex2)
			{
				Logger.Error("A null reference exception occured when fetching handle for action set '" + actionSetName + "'. This is most likely caused by an internal OpenVR issue. Action has been disabled.");
				Logger.Error(ex2);
			}
			catch (Exception ex3)
			{
				Logger.Error("An unexpected error occurred when fetching handle for action set '" + actionSetName + "'.");
				Logger.Error(ex3);
			}
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00005350 File Offset: 0x00003550
		private List<ManifestDefaultBinding> CombineAndWriteBindings(int manifestVersion)
		{
			string text = Path.Combine(Directory.GetCurrentDirectory(), "DynamicOpenVR", "Bindings");
			if (!Directory.Exists(text))
			{
				Logger.Warn("Bindings folder does not exist!");
				return new List<ManifestDefaultBinding>();
			}
			Logger.Trace("Reading default bindings from '" + text + "'");
			string[] files = Directory.GetFiles(text);
			List<DefaultBinding> list = new List<DefaultBinding>();
			foreach (string text2 in files)
			{
				try
				{
					Logger.Trace("Reading '" + text2 + "'");
					using (StreamReader streamReader = new StreamReader(text2))
					{
						list.Add(JsonConvert.DeserializeObject<DefaultBinding>(streamReader.ReadToEnd()));
					}
				}
				catch (Exception ex)
				{
					Logger.Error(string.Concat(new string[]
					{
						"An error of type ",
						ex.GetType().FullName,
						" occured when trying to parse '",
						text2,
						"': ",
						ex.Message
					}));
				}
			}
			List<ManifestDefaultBinding> list2 = new List<ManifestDefaultBinding>();
			using (IEnumerator<string> enumerator = list.Select((DefaultBinding b) => b.controllerType).Distinct<string>().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string controllerType = enumerator.Current;
					DefaultBinding defaultBinding = new DefaultBinding
					{
						actionManifestVersion = manifestVersion,
						name = "Default Beat Saber Bindings",
						description = "Action bindings for Beat Saber.",
						controllerType = controllerType,
						category = "steamvr_input",
						bindings = this.MergeBindings(list.Where((DefaultBinding b) => b.controllerType == controllerType))
					};
					string text3 = "default_bindings_" + defaultBinding.controllerType + ".json";
					list2.Add(new ManifestDefaultBinding
					{
						ControllerType = controllerType,
						BindingUrl = text3
					});
					using (StreamWriter streamWriter = new StreamWriter(Path.Combine("DynamicOpenVR", text3)))
					{
						streamWriter.WriteLine(JsonConvert.SerializeObject(defaultBinding, Formatting.Indented));
					}
				}
			}
			return list2;
		}

		// Token: 0x0600019B RID: 411 RVA: 0x000055B8 File Offset: 0x000037B8
		private Dictionary<string, BindingCollection> MergeBindings(IEnumerable<DefaultBinding> bindingSets)
		{
			Dictionary<string, BindingCollection> dictionary = new Dictionary<string, BindingCollection>();
			foreach (DefaultBinding defaultBinding in bindingSets)
			{
				foreach (KeyValuePair<string, BindingCollection> keyValuePair in defaultBinding.bindings)
				{
					string key = keyValuePair.Key;
					BindingCollection value = keyValuePair.Value;
					if (!dictionary.ContainsKey(key))
					{
						dictionary.Add(key, new BindingCollection());
					}
					dictionary[key].chords.AddRange(value.chords);
					dictionary[key].haptics.AddRange(value.haptics);
					dictionary[key].poses.AddRange(value.poses);
					dictionary[key].skeleton.AddRange(value.skeleton);
					dictionary[key].sources.AddRange(value.sources);
				}
			}
			return dictionary;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x000056EC File Offset: 0x000038EC
		private List<Dictionary<string, string>> CombineLocalizations(IEnumerable<ActionManifest> manifests)
		{
			Dictionary<string, Dictionary<string, string>> dictionary = new Dictionary<string, Dictionary<string, string>>();
			foreach (ActionManifest actionManifest in manifests)
			{
				foreach (Dictionary<string, string> dictionary2 in actionManifest.localization)
				{
					if (dictionary2.ContainsKey("language_tag"))
					{
						if (!dictionary.ContainsKey(dictionary2["language_tag"]))
						{
							dictionary.Add(dictionary2["language_tag"], new Dictionary<string, string> { 
							{
								"language_tag",
								dictionary2["language_tag"]
							} });
						}
						foreach (KeyValuePair<string, string> keyValuePair in dictionary2.Where((KeyValuePair<string, string> kvp) => kvp.Key != "language_tag"))
						{
							if (dictionary.ContainsKey(keyValuePair.Key))
							{
								Logger.Warn("Duplicate entry '" + keyValuePair.Key + "'");
							}
							else
							{
								dictionary[dictionary2["language_tag"]].Add(keyValuePair.Key, keyValuePair.Value);
							}
						}
					}
				}
			}
			return dictionary.Values.ToList<Dictionary<string, string>>();
		}

		// Token: 0x04000868 RID: 2152
		private static OpenVRActionManager _instance;

		// Token: 0x0400086A RID: 2154
		private string _actionManifestPath;

		// Token: 0x0400086B RID: 2155
		private readonly Dictionary<string, OVRAction> _actions = new Dictionary<string, OVRAction>();

		// Token: 0x0400086C RID: 2156
		private readonly HashSet<string> _actionSetNames = new HashSet<string>();

		// Token: 0x0400086D RID: 2157
		private readonly List<ulong> _actionSetHandles = new List<ulong>();
	}
}
