using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BS_Utils;
using BS_Utils.Gameplay;
using CustomSaber.Data;
using CustomSaber.Settings;
using IPA.Utilities;
using UnityEngine;
using UnityEngine.Events;
using Xft;

namespace CustomSaber.Utilities
{
	// Token: 0x02000011 RID: 17
	internal class SaberScript : MonoBehaviour
	{
		// Token: 0x06000045 RID: 69 RVA: 0x00002F44 File Offset: 0x00001144
		public static void Load()
		{
			bool flag = SaberScript.instance != null;
			if (flag)
			{
				Object.Destroy(SaberScript.instance.leftSaber);
				Object.Destroy(SaberScript.instance.rightSaber);
				Object.Destroy(SaberScript.instance.sabers);
				Object.Destroy(SaberScript.instance.gameObject);
			}
			GameObject gameObject = new GameObject("Saber Loader");
			SaberScript.instance = gameObject.AddComponent<SaberScript>();
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002FB9 File Offset: 0x000011B9
		public void Restart()
		{
			base.CancelInvoke("_Restart");
			base.Invoke("_Restart", 0.5f);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002FDC File Offset: 0x000011DC
		private void _Restart()
		{
			this.OnDestroy();
			bool flag = this.sabers && Configuration.CustomEventsEnabled;
			if (flag)
			{
				this.AddEvents();
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003013 File Offset: 0x00001213
		private void Start()
		{
			this.lastNoteId = -1;
			this.Restart();
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003024 File Offset: 0x00001224
		private void AddEvents()
		{
			GameObject gameObject = this.leftSaber;
			this.leftEventManager = ((gameObject != null) ? gameObject.GetComponent<EventManager>() : null);
			bool flag = !this.leftEventManager;
			if (flag)
			{
				this.leftEventManager = this.leftSaber.AddComponent<EventManager>();
			}
			GameObject gameObject2 = this.rightSaber;
			this.rightEventManager = ((gameObject2 != null) ? gameObject2.GetComponent<EventManager>() : null);
			bool flag2 = !this.rightEventManager;
			if (flag2)
			{
				this.rightEventManager = this.rightSaber.AddComponent<EventManager>();
			}
			EventManager eventManager = this.leftEventManager;
			bool flag3;
			if (((eventManager != null) ? eventManager.OnLevelStart : null) != null)
			{
				EventManager eventManager2 = this.rightEventManager;
				flag3 = ((eventManager2 != null) ? eventManager2.OnLevelStart : null) == null;
			}
			else
			{
				flag3 = true;
			}
			bool flag4 = flag3;
			if (!flag4)
			{
				this.leftEventManager.OnLevelStart.Invoke();
				this.rightEventManager.OnLevelStart.Invoke();
				try
				{
					this.beatmapObjectManager = Resources.FindObjectsOfTypeAll<BeatmapObjectManager>().FirstOrDefault<BeatmapObjectManager>();
					bool flag5 = this.beatmapObjectManager;
					if (flag5)
					{
						this.beatmapObjectManager.noteWasCutEvent += this.SliceCallBack;
						this.beatmapObjectManager.noteWasMissedEvent += this.NoteMissCallBack;
					}
					else
					{
						Logger.log.Warn("Failed to locate a suitable 'BeatmapObjectManager'.");
					}
					this.scoreController = Resources.FindObjectsOfTypeAll<ScoreController>().FirstOrDefault<ScoreController>();
					bool flag6 = this.scoreController;
					if (flag6)
					{
						this.scoreController.multiplierDidChangeEvent += this.MultiplierCallBack;
						this.scoreController.comboDidChangeEvent += this.ComboChangeEvent;
					}
					else
					{
						Logger.log.Warn("Failed to locate a suitable 'ScoreController'.");
					}
					this.saberCollisionManager = Resources.FindObjectsOfTypeAll<ObstacleSaberSparkleEffectManager>().FirstOrDefault<ObstacleSaberSparkleEffectManager>();
					bool flag7 = this.saberCollisionManager;
					if (flag7)
					{
						this.saberCollisionManager.sparkleEffectDidStartEvent += this.SaberStartCollide;
						this.saberCollisionManager.sparkleEffectDidEndEvent += this.SaberEndCollide;
					}
					else
					{
						Logger.log.Warn("Failed to locate a suitable 'ObstacleSaberSparkleEffectManager'.");
					}
					this.gameEnergyCounter = Resources.FindObjectsOfTypeAll<GameEnergyCounter>().FirstOrDefault<GameEnergyCounter>();
					bool flag8 = this.gameEnergyCounter;
					if (flag8)
					{
						this.gameEnergyCounter.gameEnergyDidReach0Event += this.FailLevelCallBack;
					}
					else
					{
						Logger.log.Warn("Failed to locate a suitable 'GameEnergyCounter'.");
					}
					this.beatmapCallback = Resources.FindObjectsOfTypeAll<BeatmapObjectCallbackController>().FirstOrDefault<BeatmapObjectCallbackController>();
					bool flag9 = this.beatmapCallback;
					if (flag9)
					{
						this.beatmapCallback.beatmapEventDidTriggerEvent += this.LightEventCallBack;
					}
					else
					{
						Logger.log.Warn("Failed to locate a suitable 'BeatmapObjectCallbackController'.");
					}
					this.playerHeadAndObstacleInteraction = Resources.FindObjectsOfTypeAll<PlayerHeadAndObstacleInteraction>().FirstOrDefault<PlayerHeadAndObstacleInteraction>();
					bool flag10 = !this.playerHeadAndObstacleInteraction;
					if (flag10)
					{
						Logger.log.Warn("Failed to locate a suitable 'PlayerHeadAndObstacleInteraction'.");
					}
				}
				catch (Exception ex)
				{
					Logger.log.Error(ex);
					throw;
				}
				try
				{
					float num = 0f;
					LevelData levelData = Plugin.LevelData;
					BeatmapData beatmapData = levelData.GameplayCoreSceneSetupData.difficultyBeatmap.beatmapData;
					IEnumerable<BeatmapLineData> beatmapLinesData = beatmapData.beatmapLinesData;
					foreach (BeatmapLineData beatmapLineData in beatmapLinesData)
					{
						IList<BeatmapObjectData> beatmapObjectsData = beatmapLineData.beatmapObjectsData;
						for (int i = beatmapObjectsData.Count - 1; i >= 0; i--)
						{
							BeatmapObjectData beatmapObjectData = beatmapObjectsData[i];
							bool flag11 = beatmapObjectData.beatmapObjectType == null && ((NoteData)beatmapObjectData).noteType != 3;
							if (flag11)
							{
								bool flag12 = beatmapObjectData.time > num;
								if (flag12)
								{
									this.lastNoteId = beatmapObjectData.id;
									num = beatmapObjectData.time;
								}
								break;
							}
						}
					}
				}
				catch (Exception ex2)
				{
					Logger.log.Error(ex2);
					throw;
				}
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x0000346C File Offset: 0x0000166C
		private void OnDestroy()
		{
			bool flag = this.beatmapObjectManager;
			if (flag)
			{
				this.beatmapObjectManager.noteWasCutEvent -= this.SliceCallBack;
				this.beatmapObjectManager.noteWasMissedEvent -= this.NoteMissCallBack;
			}
			bool flag2 = this.scoreController;
			if (flag2)
			{
				this.scoreController.multiplierDidChangeEvent -= this.MultiplierCallBack;
				this.scoreController.comboDidChangeEvent -= this.ComboChangeEvent;
			}
			bool flag3 = this.saberCollisionManager;
			if (flag3)
			{
				this.saberCollisionManager.sparkleEffectDidStartEvent -= this.SaberStartCollide;
				this.saberCollisionManager.sparkleEffectDidEndEvent -= this.SaberEndCollide;
			}
			bool flag4 = this.gameEnergyCounter;
			if (flag4)
			{
				this.gameEnergyCounter.gameEnergyDidReach0Event -= this.FailLevelCallBack;
			}
			bool flag5 = this.beatmapCallback;
			if (flag5)
			{
				this.beatmapCallback.beatmapEventDidTriggerEvent -= this.LightEventCallBack;
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003594 File Offset: 0x00001794
		private void Awake()
		{
			bool flag = this.sabers;
			if (flag)
			{
				Object.Destroy(this.sabers);
				this.sabers = null;
			}
			this.colorManager = Resources.FindObjectsOfTypeAll<ColorManager>().LastOrDefault<ColorManager>();
			this.ResetVanillaTrails();
			CustomSaberData customSaberData = (Configuration.RandomSabersEnabled ? SaberAssetLoader.GetRandomSaber() : SaberAssetLoader.CustomSabers[SaberAssetLoader.SelectedSaber]);
			bool flag2 = customSaberData != null;
			if (flag2)
			{
				bool flag3 = customSaberData.FileName == "DefaultSabers";
				if (flag3)
				{
					base.StartCoroutine(this.WaitToCheckDefault());
				}
				else
				{
					Logger.log.Debug("Replacing sabers");
					bool flag4 = customSaberData.Sabers;
					if (flag4)
					{
						this.sabers = Object.Instantiate<GameObject>(customSaberData.Sabers);
						GameObject gameObject = this.sabers;
						this.rightSaber = ((gameObject != null) ? gameObject.transform.Find("RightSaber").gameObject : null);
						GameObject gameObject2 = this.sabers;
						this.leftSaber = ((gameObject2 != null) ? gameObject2.transform.Find("LeftSaber").gameObject : null);
					}
					base.StartCoroutine(this.WaitForSabers(customSaberData.Sabers));
				}
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x000036C7 File Offset: 0x000018C7
		private IEnumerator WaitForSabers(GameObject saberRoot)
		{
			yield return new WaitUntil(() => Resources.FindObjectsOfTypeAll<Saber>().Any<Saber>());
			bool flag = Configuration.TrailType == TrailType.None;
			if (flag)
			{
				this.HideVanillaTrails();
			}
			IEnumerable<Saber> defaultSabers = Resources.FindObjectsOfTypeAll<Saber>();
			foreach (Saber defaultSaber in defaultSabers)
			{
				Logger.log.Debug(string.Format("Hiding default '{0}'", defaultSaber.saberType));
				IEnumerable<MeshFilter> meshFilters = defaultSaber.transform.GetComponentsInChildren<MeshFilter>();
				foreach (MeshFilter meshFilter in meshFilters)
				{
					meshFilter.gameObject.SetActive(!saberRoot);
					MeshFilter filter = meshFilter.GetComponentInChildren<MeshFilter>();
					MeshFilter meshFilter2 = filter;
					if (meshFilter2 != null)
					{
						meshFilter2.gameObject.SetActive(!saberRoot);
					}
					filter = null;
					meshFilter = null;
				}
				IEnumerator<MeshFilter> enumerator2 = null;
				Logger.log.Debug(string.Format("Attaching custom saber to '{0}'", defaultSaber.saberType));
				GameObject saber = this.GetCustomSaberByType(defaultSaber.saberType);
				bool flag2 = saber;
				if (flag2)
				{
					saber.transform.parent = defaultSaber.transform;
					saber.transform.position = defaultSaber.transform.position;
					saber.transform.rotation = defaultSaber.transform.rotation;
					bool flag3 = Configuration.TrailType == TrailType.Custom;
					if (flag3)
					{
						IEnumerable<CustomTrail> customTrails = saber.GetComponents<CustomTrail>();
						bool flag4 = customTrails.Count<CustomTrail>() == 0 && Configuration.OverrideTrailLength;
						if (flag4)
						{
							this.SetDefaultTrailLength(defaultSaber);
						}
						else
						{
							foreach (CustomTrail trail in customTrails)
							{
								trail.Init(defaultSaber, this.colorManager);
								trail = null;
							}
							IEnumerator<CustomTrail> enumerator3 = null;
						}
						customTrails = null;
					}
					else
					{
						bool flag5 = Configuration.TrailType == TrailType.Vanilla && Configuration.OverrideTrailLength;
						if (flag5)
						{
							this.SetDefaultTrailLength(defaultSaber);
						}
					}
					SaberScript.ApplyColorsToSaber(saber, this.colorManager.ColorForSaberType(defaultSaber.saberType));
					this.ApplyScaleToSabers();
				}
				meshFilters = null;
				saber = null;
				defaultSaber = null;
			}
			IEnumerator<Saber> enumerator = null;
			yield break;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x000036E0 File Offset: 0x000018E0
		private void SetDefaultTrailLength(Saber saber)
		{
			XWeaponTrail componentInChildren = saber.GetComponentInChildren<XWeaponTrail>();
			int num = (int)(Configuration.TrailLength * 30f);
			bool flag = num < 2;
			if (flag)
			{
				this.HideVanillaTrails();
			}
			else
			{
				componentInChildren.SetField("_maxFrame", num);
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003724 File Offset: 0x00001924
		private void ApplyScaleToSabers()
		{
			this.leftSaber.transform.localScale = new Vector3(Configuration.SaberWidthAdjust, Configuration.SaberWidthAdjust, 1f);
			this.rightSaber.transform.localScale = new Vector3(Configuration.SaberWidthAdjust, Configuration.SaberWidthAdjust, 1f);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x0000377C File Offset: 0x0000197C
		public static void ApplyColorsToSaber(GameObject saber, Color color)
		{
			IEnumerable<Renderer> componentsInChildren = saber.GetComponentsInChildren<Renderer>();
			foreach (Renderer renderer in componentsInChildren)
			{
				bool flag = renderer != null;
				if (flag)
				{
					foreach (Material material in renderer.sharedMaterials)
					{
						bool flag2 = material == null;
						if (!flag2)
						{
							bool flag3 = material.HasProperty("_Color");
							if (flag3)
							{
								bool flag4 = material.HasProperty("_CustomColors");
								if (flag4)
								{
									bool flag5 = material.GetFloat("_CustomColors") > 0f;
									if (flag5)
									{
										material.SetColor("_Color", color);
									}
								}
								else
								{
									bool flag6 = (material.HasProperty("_Glow") && material.GetFloat("_Glow") > 0f) || (material.HasProperty("_Bloom") && material.GetFloat("_Bloom") > 0f);
									if (flag6)
									{
										material.SetColor("_Color", color);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000038DC File Offset: 0x00001ADC
		private IEnumerator WaitToCheckDefault()
		{
			yield return new WaitUntil(() => Resources.FindObjectsOfTypeAll<Saber>().Any<Saber>());
			bool flag = Configuration.TrailType == TrailType.None;
			if (flag)
			{
				this.HideVanillaTrails();
			}
			bool hideOneSaber = false;
			SaberType hiddenSaberType = 0;
			bool flag2 = Plugin.LevelData.GameplayCoreSceneSetupData.difficultyBeatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.characteristicNameLocalizationKey.Contains("ONE_SABER");
			if (flag2)
			{
				hideOneSaber = true;
				hiddenSaberType = (Plugin.LevelData.GameplayCoreSceneSetupData.playerSpecificSettings.leftHanded ? 1 : 0);
			}
			Logger.log.Debug("Default Sabers. Not Replacing");
			IEnumerable<Saber> defaultSabers = Resources.FindObjectsOfTypeAll<Saber>();
			foreach (Saber defaultSaber in defaultSabers)
			{
				bool activeState = !hideOneSaber || defaultSaber.saberType != hiddenSaberType;
				defaultSaber.gameObject.SetActive(activeState);
				bool flag3 = defaultSaber.saberType == hiddenSaberType;
				if (flag3)
				{
					IEnumerable<MeshFilter> meshFilters = defaultSaber.transform.GetComponentsInChildren<MeshFilter>();
					foreach (MeshFilter meshFilter in meshFilters)
					{
						meshFilter.gameObject.SetActive(!this.sabers);
						MeshFilter filter = meshFilter.GetComponentInChildren<MeshFilter>();
						MeshFilter meshFilter2 = filter;
						if (meshFilter2 != null)
						{
							meshFilter2.gameObject.SetActive(!this.sabers);
						}
						filter = null;
						meshFilter = null;
					}
					IEnumerator<MeshFilter> enumerator2 = null;
					meshFilters = null;
				}
				defaultSaber = null;
			}
			IEnumerator<Saber> enumerator = null;
			yield break;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000038EC File Offset: 0x00001AEC
		private void Update()
		{
			bool flag = this.playerHeadAndObstacleInteraction != null && this.playerHeadAndObstacleInteraction.intersectingObstacles.Count > 0;
			if (flag)
			{
				bool flag2 = !this.playerHeadWasInObstacle;
				if (flag2)
				{
					EventManager eventManager = this.leftEventManager;
					if (eventManager != null)
					{
						UnityEvent onComboBreak = eventManager.OnComboBreak;
						if (onComboBreak != null)
						{
							onComboBreak.Invoke();
						}
					}
					EventManager eventManager2 = this.rightEventManager;
					if (eventManager2 != null)
					{
						UnityEvent onComboBreak2 = eventManager2.OnComboBreak;
						if (onComboBreak2 != null)
						{
							onComboBreak2.Invoke();
						}
					}
				}
				this.playerHeadWasInObstacle = !this.playerHeadWasInObstacle;
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x0000397C File Offset: 0x00001B7C
		private void HideVanillaTrails()
		{
			this.SetVanillaTrailVisibility(0f);
		}

		// Token: 0x06000053 RID: 83 RVA: 0x0000398A File Offset: 0x00001B8A
		private void ResetVanillaTrails()
		{
			this.SetVanillaTrailVisibility(1.007f);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003998 File Offset: 0x00001B98
		private void SetVanillaTrailVisibility(float trailWidth)
		{
			IEnumerable<XWeaponTrail> enumerable = Resources.FindObjectsOfTypeAll<XWeaponTrail>();
			foreach (XWeaponTrail xweaponTrail in enumerable)
			{
				xweaponTrail.SetField("_trailWidth", trailWidth);
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000039F4 File Offset: 0x00001BF4
		private GameObject GetCustomSaberByType(SaberType saberType)
		{
			GameObject gameObject = null;
			bool flag = saberType == 0;
			if (flag)
			{
				gameObject = this.leftSaber;
			}
			else
			{
				bool flag2 = saberType == 1;
				if (flag2)
				{
					gameObject = this.rightSaber;
				}
			}
			return gameObject;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003A30 File Offset: 0x00001C30
		private EventManager GetEventManagerByType(SaberType saberType)
		{
			EventManager eventManager = null;
			bool flag = saberType == 0;
			if (flag)
			{
				eventManager = this.leftEventManager;
			}
			else
			{
				bool flag2 = saberType == 1;
				if (flag2)
				{
					eventManager = this.rightEventManager;
				}
			}
			return eventManager;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003A6C File Offset: 0x00001C6C
		private void SliceCallBack(INoteController noteController, NoteCutInfo noteCutInfo)
		{
			bool flag = !noteCutInfo.allIsOK;
			if (flag)
			{
				EventManager eventManager = this.leftEventManager;
				if (eventManager != null)
				{
					UnityEvent onComboBreak = eventManager.OnComboBreak;
					if (onComboBreak != null)
					{
						onComboBreak.Invoke();
					}
				}
				EventManager eventManager2 = this.rightEventManager;
				if (eventManager2 != null)
				{
					UnityEvent onComboBreak2 = eventManager2.OnComboBreak;
					if (onComboBreak2 != null)
					{
						onComboBreak2.Invoke();
					}
				}
				base.StartCoroutine(this.CalculateAccuracyAndFireEvents());
			}
			else
			{
				EventManager eventManagerByType = this.GetEventManagerByType(noteCutInfo.saberType);
				if (eventManagerByType != null)
				{
					UnityEvent onSlice = eventManagerByType.OnSlice;
					if (onSlice != null)
					{
						onSlice.Invoke();
					}
				}
				noteCutInfo.swingRatingCounter.didFinishEvent += new SaberSwingRatingCounter.DidFinishDelegate(this.OnSwingRatingCounterFinished);
			}
			bool flag2 = noteController.noteData.id == this.lastNoteId;
			if (flag2)
			{
				EventManager eventManager3 = this.leftEventManager;
				if (eventManager3 != null)
				{
					UnityEvent onLevelEnded = eventManager3.OnLevelEnded;
					if (onLevelEnded != null)
					{
						onLevelEnded.Invoke();
					}
				}
				EventManager eventManager4 = this.rightEventManager;
				if (eventManager4 != null)
				{
					UnityEvent onLevelEnded2 = eventManager4.OnLevelEnded;
					if (onLevelEnded2 != null)
					{
						onLevelEnded2.Invoke();
					}
				}
			}
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003B64 File Offset: 0x00001D64
		private void NoteMissCallBack(INoteController noteController)
		{
			bool flag = noteController.noteData.noteType != 3;
			if (flag)
			{
				EventManager eventManager = this.leftEventManager;
				if (eventManager != null)
				{
					UnityEvent onComboBreak = eventManager.OnComboBreak;
					if (onComboBreak != null)
					{
						onComboBreak.Invoke();
					}
				}
				EventManager eventManager2 = this.rightEventManager;
				if (eventManager2 != null)
				{
					UnityEvent onComboBreak2 = eventManager2.OnComboBreak;
					if (onComboBreak2 != null)
					{
						onComboBreak2.Invoke();
					}
				}
			}
			bool flag2 = noteController.noteData.id == this.lastNoteId;
			if (flag2)
			{
				EventManager eventManager3 = this.leftEventManager;
				if (eventManager3 != null)
				{
					UnityEvent onLevelEnded = eventManager3.OnLevelEnded;
					if (onLevelEnded != null)
					{
						onLevelEnded.Invoke();
					}
				}
				EventManager eventManager4 = this.rightEventManager;
				if (eventManager4 != null)
				{
					UnityEvent onLevelEnded2 = eventManager4.OnLevelEnded;
					if (onLevelEnded2 != null)
					{
						onLevelEnded2.Invoke();
					}
				}
			}
			base.StartCoroutine(this.CalculateAccuracyAndFireEvents());
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003C24 File Offset: 0x00001E24
		private void MultiplierCallBack(int multiplier, float progress)
		{
			bool flag = multiplier > 1 && progress < 0.1f;
			if (flag)
			{
				EventManager eventManager = this.leftEventManager;
				if (eventManager != null)
				{
					UnityEvent multiplierUp = eventManager.MultiplierUp;
					if (multiplierUp != null)
					{
						multiplierUp.Invoke();
					}
				}
				EventManager eventManager2 = this.rightEventManager;
				if (eventManager2 != null)
				{
					UnityEvent multiplierUp2 = eventManager2.MultiplierUp;
					if (multiplierUp2 != null)
					{
						multiplierUp2.Invoke();
					}
				}
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003C84 File Offset: 0x00001E84
		private void SaberStartCollide(SaberType saberType)
		{
			EventManager eventManagerByType = this.GetEventManagerByType(saberType);
			if (eventManagerByType != null)
			{
				UnityEvent saberStartColliding = eventManagerByType.SaberStartColliding;
				if (saberStartColliding != null)
				{
					saberStartColliding.Invoke();
				}
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003CB4 File Offset: 0x00001EB4
		private void SaberEndCollide(SaberType saberType)
		{
			EventManager eventManagerByType = this.GetEventManagerByType(saberType);
			if (eventManagerByType != null)
			{
				UnityEvent saberStopColliding = eventManagerByType.SaberStopColliding;
				if (saberStopColliding != null)
				{
					saberStopColliding.Invoke();
				}
			}
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00003CE1 File Offset: 0x00001EE1
		private void FailLevelCallBack()
		{
			EventManager eventManager = this.leftEventManager;
			if (eventManager != null)
			{
				UnityEvent onLevelFail = eventManager.OnLevelFail;
				if (onLevelFail != null)
				{
					onLevelFail.Invoke();
				}
			}
			EventManager eventManager2 = this.rightEventManager;
			if (eventManager2 != null)
			{
				UnityEvent onLevelFail2 = eventManager2.OnLevelFail;
				if (onLevelFail2 != null)
				{
					onLevelFail2.Invoke();
				}
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00003D20 File Offset: 0x00001F20
		private void LightEventCallBack(BeatmapEventData songEvent)
		{
			bool flag = songEvent.type < 5;
			if (flag)
			{
				bool flag2 = songEvent.value > 0 && songEvent.value < 4;
				if (flag2)
				{
					EventManager eventManager = this.leftEventManager;
					if (eventManager != null)
					{
						UnityEvent onBlueLightOn = eventManager.OnBlueLightOn;
						if (onBlueLightOn != null)
						{
							onBlueLightOn.Invoke();
						}
					}
					EventManager eventManager2 = this.rightEventManager;
					if (eventManager2 != null)
					{
						UnityEvent onBlueLightOn2 = eventManager2.OnBlueLightOn;
						if (onBlueLightOn2 != null)
						{
							onBlueLightOn2.Invoke();
						}
					}
				}
				bool flag3 = songEvent.value > 4 && songEvent.value < 8;
				if (flag3)
				{
					EventManager eventManager3 = this.leftEventManager;
					if (eventManager3 != null)
					{
						UnityEvent onRedLightOn = eventManager3.OnRedLightOn;
						if (onRedLightOn != null)
						{
							onRedLightOn.Invoke();
						}
					}
					EventManager eventManager4 = this.rightEventManager;
					if (eventManager4 != null)
					{
						UnityEvent onRedLightOn2 = eventManager4.OnRedLightOn;
						if (onRedLightOn2 != null)
						{
							onRedLightOn2.Invoke();
						}
					}
				}
			}
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00003DEA File Offset: 0x00001FEA
		private void ComboChangeEvent(int combo)
		{
			EventManager eventManager = this.leftEventManager;
			if (eventManager != null)
			{
				EventManager.ComboChangedEvent onComboChanged = eventManager.OnComboChanged;
				if (onComboChanged != null)
				{
					onComboChanged.Invoke(combo);
				}
			}
			EventManager eventManager2 = this.rightEventManager;
			if (eventManager2 != null)
			{
				EventManager.ComboChangedEvent onComboChanged2 = eventManager2.OnComboChanged;
				if (onComboChanged2 != null)
				{
					onComboChanged2.Invoke(combo);
				}
			}
		}

		// Token: 0x0600005F RID: 95 RVA: 0x00003E29 File Offset: 0x00002029
		private IEnumerator CalculateAccuracyAndFireEvents()
		{
			yield return null;
			int rawScore = this.scoreController.prevFrameRawScore;
			int maximumScore = ScoreModel.MaxRawScoreForNumberOfNotes(this.scoreController.GetField("_cutOrMissedNotes"));
			float accuracy = (float)rawScore / (float)maximumScore;
			EventManager eventManager = this.leftEventManager;
			if (eventManager != null)
			{
				EventManager.AccuracyChangedEvent onAccuracyChanged = eventManager.OnAccuracyChanged;
				if (onAccuracyChanged != null)
				{
					onAccuracyChanged.Invoke(accuracy);
				}
			}
			EventManager eventManager2 = this.rightEventManager;
			if (eventManager2 != null)
			{
				EventManager.AccuracyChangedEvent onAccuracyChanged2 = eventManager2.OnAccuracyChanged;
				if (onAccuracyChanged2 != null)
				{
					onAccuracyChanged2.Invoke(accuracy);
				}
			}
			yield break;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00003E38 File Offset: 0x00002038
		private void OnSwingRatingCounterFinished(SaberSwingRatingCounter afterCutRating)
		{
			afterCutRating.didFinishEvent -= new SaberSwingRatingCounter.DidFinishDelegate(this.OnSwingRatingCounterFinished);
			base.StartCoroutine(this.CalculateAccuracyAndFireEvents());
		}

		// Token: 0x0400003D RID: 61
		private GameObject sabers;

		// Token: 0x0400003E RID: 62
		private GameObject leftSaber;

		// Token: 0x0400003F RID: 63
		private GameObject rightSaber;

		// Token: 0x04000040 RID: 64
		private int lastNoteId;

		// Token: 0x04000041 RID: 65
		private bool playerHeadWasInObstacle;

		// Token: 0x04000042 RID: 66
		private ColorManager colorManager;

		// Token: 0x04000043 RID: 67
		private EventManager leftEventManager;

		// Token: 0x04000044 RID: 68
		private EventManager rightEventManager;

		// Token: 0x04000045 RID: 69
		private BeatmapObjectManager beatmapObjectManager;

		// Token: 0x04000046 RID: 70
		private ScoreController scoreController;

		// Token: 0x04000047 RID: 71
		private ObstacleSaberSparkleEffectManager saberCollisionManager;

		// Token: 0x04000048 RID: 72
		private GameEnergyCounter gameEnergyCounter;

		// Token: 0x04000049 RID: 73
		private BeatmapObjectCallbackController beatmapCallback;

		// Token: 0x0400004A RID: 74
		private PlayerHeadAndObstacleInteraction playerHeadAndObstacleInteraction;

		// Token: 0x0400004B RID: 75
		public static SaberScript instance;
	}
}
