using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CameraPlus.SimpleJSON;
using IPA.Logging;
using IPA.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CameraPlus
{
	// Token: 0x02000010 RID: 16
	public class CameraMovement : MonoBehaviour
	{
		// Token: 0x0600007D RID: 125 RVA: 0x000076B4 File Offset: 0x000058B4
		public virtual void OnActiveSceneChanged(Scene from, Scene to)
		{
			if (to.name == "GameCore")
			{
				GamePause gamePause = Resources.FindObjectsOfTypeAll<GamePause>().First<GamePause>();
				if (gamePause && this.dataLoaded && !this.data.ActiveInPauseMenu)
				{
					gamePause.didResumeEvent += this.Resume;
					gamePause.didPauseEvent += this.Pause;
				}
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00007720 File Offset: 0x00005920
		protected void Update()
		{
			if (!this.dataLoaded || this._paused)
			{
				return;
			}
			if (this.movePerc == 1f && this.movementDelayEndTime <= DateTime.Now)
			{
				this.UpdatePosAndRot();
			}
			long ticks = (this.movementEndTime - this.movementStartTime).Ticks;
			long ticks2 = (DateTime.Now - this.movementStartTime).Ticks;
			this.movePerc = Mathf.Clamp((float)ticks2 / (float)ticks, 0f, 1f);
			this._cameraPlus.ThirdPersonPos = this.LerpVector3(this.StartPos, this.EndPos, this.Ease(this.movePerc));
			this._cameraPlus.ThirdPersonRot = this.LerpVector3(this.StartRot, this.EndRot, this.Ease(this.movePerc));
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00007802 File Offset: 0x00005A02
		protected Vector3 LerpVector3(Vector3 from, Vector3 to, float percent)
		{
			return new Vector3(Mathf.LerpAngle(from.x, to.x, percent), Mathf.LerpAngle(from.y, to.y, percent), Mathf.LerpAngle(from.z, to.z, percent));
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00007840 File Offset: 0x00005A40
		public virtual bool Init(CameraPlusBehaviour cameraPlus)
		{
			this._cameraPlus = cameraPlus;
			Plugin instance = Plugin.Instance;
			instance.ActiveSceneChanged = (Action<Scene, Scene>)Delegate.Combine(instance.ActiveSceneChanged, new Action<Scene, Scene>(this.OnActiveSceneChanged));
			return this.LoadCameraData(cameraPlus.Config.movementScriptPath);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x0000767D File Offset: 0x0000587D
		public virtual void Shutdown()
		{
			Plugin instance = Plugin.Instance;
			instance.ActiveSceneChanged = (Action<Scene, Scene>)Delegate.Remove(instance.ActiveSceneChanged, new Action<Scene, Scene>(this.OnActiveSceneChanged));
			Object.Destroy(this);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x0000788C File Offset: 0x00005A8C
		public void Pause()
		{
			if (this._paused)
			{
				return;
			}
			this._paused = true;
			this._pauseTime = DateTime.Now;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x000078AC File Offset: 0x00005AAC
		public void Resume()
		{
			if (!this._paused)
			{
				return;
			}
			TimeSpan timeSpan = DateTime.Now - this._pauseTime;
			this.movementStartTime += timeSpan;
			this.movementEndTime += timeSpan;
			this.movementDelayEndTime += timeSpan;
			this._paused = false;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00007910 File Offset: 0x00005B10
		protected bool LoadCameraData(string path)
		{
			if (File.Exists(path))
			{
				string text = File.ReadAllText(path);
				if (this.data.LoadFromJson(text))
				{
					Logger.Log("Populated CameraData", Logger.Level.Info);
					if (this.data.Movements.Count == 0)
					{
						Logger.Log("No movement data!", Logger.Level.Info);
						return false;
					}
					this.eventID = 0;
					this.UpdatePosAndRot();
					this.dataLoaded = true;
					Logger.Log(string.Format("Found {0} entries in: {1}", this.data.Movements.Count, path), Logger.Level.Info);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000079A4 File Offset: 0x00005BA4
		protected void FindShortestDelta(ref Vector3 from, ref Vector3 to)
		{
			if (Mathf.DeltaAngle(from.x, to.x) < 0f)
			{
				from.x += 360f;
			}
			if (Mathf.DeltaAngle(from.y, to.y) < 0f)
			{
				from.y += 360f;
			}
			if (Mathf.DeltaAngle(from.z, to.z) < 0f)
			{
				from.z += 360f;
			}
		}

		// Token: 0x06000086 RID: 134 RVA: 0x00007A28 File Offset: 0x00005C28
		protected void UpdatePosAndRot()
		{
			this.eventID++;
			if (this.eventID >= this.data.Movements.Count)
			{
				this.eventID = 0;
			}
			this.easeTransition = this.data.Movements[this.eventID].EaseTransition;
			this.StartRot = new Vector3(this.data.Movements[this.eventID].StartRot.x, this.data.Movements[this.eventID].StartRot.y, this.data.Movements[this.eventID].StartRot.z);
			this.StartPos = new Vector3(this.data.Movements[this.eventID].StartPos.x, this.data.Movements[this.eventID].StartPos.y, this.data.Movements[this.eventID].StartPos.z);
			this.EndRot = new Vector3(this.data.Movements[this.eventID].EndRot.x, this.data.Movements[this.eventID].EndRot.y, this.data.Movements[this.eventID].EndRot.z);
			this.EndPos = new Vector3(this.data.Movements[this.eventID].EndPos.x, this.data.Movements[this.eventID].EndPos.y, this.data.Movements[this.eventID].EndPos.z);
			this.FindShortestDelta(ref this.StartRot, ref this.EndRot);
			this.movementStartTime = DateTime.Now;
			this.movementEndTime = this.movementStartTime.AddSeconds((double)this.data.Movements[this.eventID].Duration);
			this.movementDelayEndTime = this.movementStartTime.AddSeconds((double)(this.data.Movements[this.eventID].Duration + this.data.Movements[this.eventID].Delay));
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00007CC4 File Offset: 0x00005EC4
		protected float Ease(float p)
		{
			if (!this.easeTransition)
			{
				return p;
			}
			if (p < 0.5f)
			{
				return 4f * p * p * p;
			}
			float num = 2f * p - 2f;
			return 0.5f * num * num * num + 1f;
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00007D10 File Offset: 0x00005F10
		public static void CreateExampleScript()
		{
			string text = Path.Combine(UnityGame.UserDataPath, Plugin.Name, "Scripts");
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			string text2 = Path.Combine(text, "ExampleMovementScript.json");
			if (!File.Exists(text2))
			{
				File.WriteAllBytes(text2, Utils.GetResource(Assembly.GetExecutingAssembly(), "CameraPlus.Resources.ExampleMovementScript.json"));
			}
		}

		// Token: 0x0400008B RID: 139
		protected CameraPlusBehaviour _cameraPlus;

		// Token: 0x0400008C RID: 140
		protected bool dataLoaded;

		// Token: 0x0400008D RID: 141
		protected CameraMovement.CameraData data = new CameraMovement.CameraData();

		// Token: 0x0400008E RID: 142
		protected Vector3 StartPos = Vector3.zero;

		// Token: 0x0400008F RID: 143
		protected Vector3 EndPos = Vector3.zero;

		// Token: 0x04000090 RID: 144
		protected Vector3 StartRot = Vector3.zero;

		// Token: 0x04000091 RID: 145
		protected Vector3 EndRot = Vector3.zero;

		// Token: 0x04000092 RID: 146
		protected bool easeTransition = true;

		// Token: 0x04000093 RID: 147
		protected float movePerc;

		// Token: 0x04000094 RID: 148
		protected int eventID;

		// Token: 0x04000095 RID: 149
		protected DateTime movementStartTime;

		// Token: 0x04000096 RID: 150
		protected DateTime movementEndTime;

		// Token: 0x04000097 RID: 151
		protected DateTime movementDelayEndTime;

		// Token: 0x04000098 RID: 152
		protected bool _paused;

		// Token: 0x04000099 RID: 153
		protected DateTime _pauseTime;

		// Token: 0x0200002B RID: 43
		public class Movements
		{
			// Token: 0x040000DB RID: 219
			public Vector3 StartPos;

			// Token: 0x040000DC RID: 220
			public Vector3 StartRot;

			// Token: 0x040000DD RID: 221
			public Vector3 EndPos;

			// Token: 0x040000DE RID: 222
			public Vector3 EndRot;

			// Token: 0x040000DF RID: 223
			public float Duration;

			// Token: 0x040000E0 RID: 224
			public float Delay;

			// Token: 0x040000E1 RID: 225
			public bool EaseTransition = true;
		}

		// Token: 0x0200002C RID: 44
		public class CameraData
		{
			// Token: 0x0600016C RID: 364 RVA: 0x00009BFC File Offset: 0x00007DFC
			public bool LoadFromJson(string jsonString)
			{
				this.Movements.Clear();
				JSONNode jsonnode = JSON.Parse(jsonString);
				if (jsonnode != null && !jsonnode["Movements"].IsNull)
				{
					if (jsonnode["ActiveInPauseMenu"].IsBoolean)
					{
						this.ActiveInPauseMenu = jsonnode["ActiveInPauseMenu"].AsBool;
					}
					foreach (KeyValuePair<string, JSONNode> keyValuePair in jsonnode["Movements"].AsArray)
					{
						JSONObject jsonobject = (JSONObject)keyValuePair;
						CameraMovement.Movements movements = new CameraMovement.Movements();
						JSONNode jsonnode2 = jsonobject["StartPos"];
						JSONNode jsonnode3 = jsonobject["StartRot"];
						movements.StartPos = new Vector3(jsonnode2["x"].AsFloat, jsonnode2["y"].AsFloat, jsonnode2["z"].AsFloat);
						movements.StartRot = new Vector3(jsonnode3["x"].AsFloat, jsonnode3["y"].AsFloat, jsonnode3["z"].AsFloat);
						JSONNode jsonnode4 = jsonobject["EndPos"];
						JSONNode jsonnode5 = jsonobject["EndRot"];
						movements.EndPos = new Vector3(jsonnode4["x"].AsFloat, jsonnode4["y"].AsFloat, jsonnode4["z"].AsFloat);
						movements.EndRot = new Vector3(jsonnode5["x"].AsFloat, jsonnode5["y"].AsFloat, jsonnode5["z"].AsFloat);
						movements.Delay = jsonobject["Delay"].AsFloat;
						movements.Duration = Mathf.Clamp(jsonobject["Duration"].AsFloat, 0.01f, float.MaxValue);
						if (jsonobject["EaseTransition"].IsBoolean)
						{
							movements.EaseTransition = jsonobject["EaseTransition"].AsBool;
						}
						this.Movements.Add(movements);
					}
					return true;
				}
				return false;
			}

			// Token: 0x040000E2 RID: 226
			public bool ActiveInPauseMenu = true;

			// Token: 0x040000E3 RID: 227
			public List<CameraMovement.Movements> Movements = new List<CameraMovement.Movements>();
		}
	}
}
