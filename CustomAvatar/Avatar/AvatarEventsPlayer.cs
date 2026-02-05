using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace CustomAvatar.Avatar
{
	// Token: 0x0200003A RID: 58
	internal class AvatarEventsPlayer : MonoBehaviour
	{
		// Token: 0x0600013F RID: 319 RVA: 0x00009255 File Offset: 0x00007455
		public void Restart()
		{
			base.CancelInvoke("_Restart");
			base.Invoke("_Restart", 0.5f);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00009275 File Offset: 0x00007475
		private void _Restart()
		{
			this.CleanUp();
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0000927F File Offset: 0x0000747F
		private void OnEnable()
		{
			SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(this.SceneManagerOnSceneLoaded);
			this._eventManager = base.gameObject.GetComponent<EventManager>();
		}

		// Token: 0x06000142 RID: 322 RVA: 0x000092A5 File Offset: 0x000074A5
		private void OnDisable()
		{
			SceneManager.sceneLoaded -= new UnityAction<Scene, LoadSceneMode>(this.SceneManagerOnSceneLoaded);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00009275 File Offset: 0x00007475
		private void OnDestroy()
		{
			this.CleanUp();
		}

		// Token: 0x06000144 RID: 324 RVA: 0x000092BC File Offset: 0x000074BC
		private void CleanUp()
		{
			bool flag = this._scoreController;
			if (flag)
			{
				this._scoreController.noteWasCutEvent -= this.SliceCallBack;
				this._scoreController.noteWasMissedEvent -= this.NoteMissCallBack;
				this._scoreController.multiplierDidChangeEvent -= this.MultiplierCallBack;
				this._scoreController.comboDidChangeEvent -= this.ComboChangeEvent;
			}
			bool flag2 = this._saberCollisionManager;
			if (flag2)
			{
				this._saberCollisionManager.sparkleEffectDidStartEvent -= this.SaberStartCollide;
				this._saberCollisionManager.sparkleEffectDidEndEvent -= this.SaberEndCollide;
			}
			bool flag3 = this._gameEnergyCounter;
			if (flag3)
			{
				this._gameEnergyCounter.gameEnergyDidReach0Event -= this.FailLevelCallBack;
			}
			bool flag4 = this._beatmapObjectCallbackController;
			if (flag4)
			{
				this._beatmapObjectCallbackController.beatmapEventDidTriggerEvent -= this.OnBeatmapEventDidTriggerEvent;
			}
		}

		// Token: 0x06000145 RID: 325 RVA: 0x000093CC File Offset: 0x000075CC
		private void SceneManagerOnSceneLoaded(Scene newScene, LoadSceneMode mode)
		{
			this._eventManager = base.gameObject.GetComponent<EventManager>();
			bool flag = this._eventManager == null;
			if (flag)
			{
				this._eventManager = base.gameObject.AddComponent<EventManager>();
			}
			this._scoreController = Resources.FindObjectsOfTypeAll<ScoreController>().FirstOrDefault<ScoreController>();
			bool flag2 = this._scoreController == null;
			if (!flag2)
			{
				this._saberCollisionManager = Resources.FindObjectsOfTypeAll<ObstacleSaberSparkleEffectManager>().FirstOrDefault<ObstacleSaberSparkleEffectManager>();
				this._gameEnergyCounter = Resources.FindObjectsOfTypeAll<GameEnergyCounter>().FirstOrDefault<GameEnergyCounter>();
				this._beatmapObjectCallbackController = Resources.FindObjectsOfTypeAll<BeatmapObjectCallbackController>().FirstOrDefault<BeatmapObjectCallbackController>();
				this._beatmapDataModel = Resources.FindObjectsOfTypeAll<BeatmapDataSO>().FirstOrDefault<BeatmapDataSO>();
				this._scoreController.noteWasCutEvent += this.SliceCallBack;
				this._scoreController.noteWasMissedEvent += this.NoteMissCallBack;
				this._scoreController.multiplierDidChangeEvent += this.MultiplierCallBack;
				this._scoreController.comboDidChangeEvent += this.ComboChangeEvent;
				bool flag3 = this._saberCollisionManager;
				if (flag3)
				{
					this._saberCollisionManager.sparkleEffectDidStartEvent += this.SaberStartCollide;
					this._saberCollisionManager.sparkleEffectDidEndEvent += this.SaberEndCollide;
				}
				bool flag4 = this._gameEnergyCounter;
				if (flag4)
				{
					this._gameEnergyCounter.gameEnergyDidReach0Event += this.FailLevelCallBack;
				}
				bool flag5 = this._beatmapObjectCallbackController;
				if (flag5)
				{
					this._beatmapObjectCallbackController.beatmapEventDidTriggerEvent += this.OnBeatmapEventDidTriggerEvent;
				}
				this._lastNoteId = -1;
			}
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000956C File Offset: 0x0000776C
		private void BeatmapDataChangedCallback()
		{
			bool flag = this._beatmapDataModel.beatmapData == null;
			if (!flag)
			{
				this._lastNoteId = this._beatmapDataModel.beatmapData.beatmapLinesData.Aggregate(new Tuple<float, int>(0f, -1), (Tuple<float, int> maxLine, BeatmapLineData lineData) => lineData.beatmapObjectsData.Where((BeatmapObjectData obj) => obj.beatmapObjectType == null && (((NoteData)obj).noteType == null || ((NoteData)obj).noteType == 1)).Aggregate(maxLine, (Tuple<float, int> maxNote, BeatmapObjectData note) => (maxNote.Item1 < note.time) ? new Tuple<float, int>(note.time, note.id) : maxNote)).Item2;
			}
		}

		// Token: 0x06000147 RID: 327 RVA: 0x000095D8 File Offset: 0x000077D8
		private void SliceCallBack(NoteData noteData, NoteCutInfo noteCutInfo, int multiplier)
		{
			bool flag = !noteCutInfo.allIsOK;
			if (flag)
			{
				EventManager eventManager = this._eventManager;
				if (eventManager != null)
				{
					UnityEvent onComboBreak = eventManager.OnComboBreak;
					if (onComboBreak != null)
					{
						onComboBreak.Invoke();
					}
				}
			}
			else
			{
				EventManager eventManager2 = this._eventManager;
				if (eventManager2 != null)
				{
					UnityEvent onSlice = eventManager2.OnSlice;
					if (onSlice != null)
					{
						onSlice.Invoke();
					}
				}
			}
			bool flag2 = noteData.id == this._lastNoteId;
			if (flag2)
			{
				EventManager eventManager3 = this._eventManager;
				if (eventManager3 != null)
				{
					UnityEvent onLevelFinish = eventManager3.OnLevelFinish;
					if (onLevelFinish != null)
					{
						onLevelFinish.Invoke();
					}
				}
			}
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00009664 File Offset: 0x00007864
		private void NoteMissCallBack(NoteData noteData, int multiplier)
		{
			bool flag = noteData.noteType != 3;
			if (flag)
			{
				EventManager eventManager = this._eventManager;
				if (eventManager != null)
				{
					UnityEvent onComboBreak = eventManager.OnComboBreak;
					if (onComboBreak != null)
					{
						onComboBreak.Invoke();
					}
				}
			}
		}

		// Token: 0x06000149 RID: 329 RVA: 0x000096A4 File Offset: 0x000078A4
		private void MultiplierCallBack(int multiplier, float progress)
		{
			bool flag = multiplier > 1 && progress < 0.1f;
			if (flag)
			{
				EventManager eventManager = this._eventManager;
				if (eventManager != null)
				{
					UnityEvent multiplierUp = eventManager.MultiplierUp;
					if (multiplierUp != null)
					{
						multiplierUp.Invoke();
					}
				}
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x000096E4 File Offset: 0x000078E4
		private void SaberStartCollide(SaberType saber)
		{
			EventManager eventManager = this._eventManager;
			if (eventManager != null)
			{
				UnityEvent saberStartColliding = eventManager.SaberStartColliding;
				if (saberStartColliding != null)
				{
					saberStartColliding.Invoke();
				}
			}
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00009704 File Offset: 0x00007904
		private void SaberEndCollide(SaberType saber)
		{
			EventManager eventManager = this._eventManager;
			if (eventManager != null)
			{
				UnityEvent saberStopColliding = eventManager.SaberStopColliding;
				if (saberStopColliding != null)
				{
					saberStopColliding.Invoke();
				}
			}
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00009724 File Offset: 0x00007924
		private void FailLevelCallBack()
		{
			EventManager eventManager = this._eventManager;
			if (eventManager != null)
			{
				UnityEvent onLevelFail = eventManager.OnLevelFail;
				if (onLevelFail != null)
				{
					onLevelFail.Invoke();
				}
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00009744 File Offset: 0x00007944
		private void OnBeatmapEventDidTriggerEvent(BeatmapEventData beatmapEventData)
		{
			bool flag = beatmapEventData == null || beatmapEventData.type >= 5;
			if (!flag)
			{
				bool flag2 = beatmapEventData.value > 0 && beatmapEventData.value < 4;
				if (flag2)
				{
					EventManager eventManager = this._eventManager;
					if (eventManager != null)
					{
						UnityEvent onBlueLightOn = eventManager.OnBlueLightOn;
						if (onBlueLightOn != null)
						{
							onBlueLightOn.Invoke();
						}
					}
				}
				bool flag3 = beatmapEventData.value > 4 && beatmapEventData.value < 8;
				if (flag3)
				{
					EventManager eventManager2 = this._eventManager;
					if (eventManager2 != null)
					{
						UnityEvent onRedLightOn = eventManager2.OnRedLightOn;
						if (onRedLightOn != null)
						{
							onRedLightOn.Invoke();
						}
					}
				}
			}
		}

		// Token: 0x0600014E RID: 334 RVA: 0x000097DA File Offset: 0x000079DA
		private void ComboChangeEvent(int combo)
		{
			EventManager eventManager = this._eventManager;
			if (eventManager != null)
			{
				EventManager.ComboChangedEvent onComboChanged = eventManager.OnComboChanged;
				if (onComboChanged != null)
				{
					onComboChanged.Invoke(combo);
				}
			}
		}

		// Token: 0x0600014F RID: 335 RVA: 0x000097FB File Offset: 0x000079FB
		public void MenuEnteredEvent()
		{
			EventManager eventManager = this._eventManager;
			if (eventManager != null)
			{
				UnityEvent onMenuEnter = eventManager.OnMenuEnter;
				if (onMenuEnter != null)
				{
					onMenuEnter.Invoke();
				}
			}
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000981B File Offset: 0x00007A1B
		public void LevelStartedEvent()
		{
			EventManager eventManager = this._eventManager;
			if (eventManager != null)
			{
				UnityEvent onLevelStart = eventManager.OnLevelStart;
				if (onLevelStart != null)
				{
					onLevelStart.Invoke();
				}
			}
		}

		// Token: 0x040001B2 RID: 434
		private EventManager _eventManager;

		// Token: 0x040001B3 RID: 435
		private ScoreController _scoreController;

		// Token: 0x040001B4 RID: 436
		private ObstacleSaberSparkleEffectManager _saberCollisionManager;

		// Token: 0x040001B5 RID: 437
		private GameEnergyCounter _gameEnergyCounter;

		// Token: 0x040001B6 RID: 438
		private BeatmapObjectCallbackController _beatmapObjectCallbackController;

		// Token: 0x040001B7 RID: 439
		private BeatmapDataSO _beatmapDataModel;

		// Token: 0x040001B8 RID: 440
		private int _lastNoteId = -1;
	}
}
