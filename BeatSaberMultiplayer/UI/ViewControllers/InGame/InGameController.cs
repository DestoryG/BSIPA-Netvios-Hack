using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeatSaberMultiplayer.Configuration;
using BeatSaberMultiplayer.Data;
using BeatSaberMultiplayer.Helper;
using BeatSaberMultiplayer.VOIP;
using BS_Utils.Utilities;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMultiplayer.UI.ViewControllers.InGame
{
	// Token: 0x02000066 RID: 102
	public class InGameController : MonoBehaviour
	{
		// Token: 0x17000227 RID: 551
		// (get) Token: 0x060007B2 RID: 1970 RVA: 0x00020110 File Offset: 0x0001E310
		// (set) Token: 0x060007B3 RID: 1971 RVA: 0x000196A0 File Offset: 0x000178A0
		public bool isPlaying
		{
			get
			{
				return this._currentScene == "GameCore";
			}
			private set
			{
			}
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x00020122 File Offset: 0x0001E322
		public static void OnLoad()
		{
			if (InGameController.instance != null)
			{
				return;
			}
			new GameObject("InGame Online Controller").AddComponent<InGameController>();
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x00020142 File Offset: 0x0001E342
		public void Awake()
		{
			if (InGameController.instance == this)
			{
				return;
			}
			InGameController.instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
			this.ChangeVoiceEnabled(true);
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x0002016C File Offset: 0x0001E36C
		public void ChangeVoiceEnabled(bool first = false)
		{
			if (PluginConfig.Instance.CustomVoiceEnabled)
			{
				this._voiceChatListener = new GameObject("Voice Chat Listener").AddComponent<VoipListener>();
				this._voiceChatListener.OnAudioGenerated -= this.ProcesVoiceFragment;
				this._voiceChatListener.OnAudioGenerated += this.ProcesVoiceFragment;
				Object.DontDestroyOnLoad(this._voiceChatListener.gameObject);
				this._isVoiceChatActive = true;
				if (!first)
				{
					this._voiceChatListener.StopRecording();
					this._voiceChatListener.StartRecording();
					return;
				}
			}
			else if (this._voiceChatListener != null)
			{
				this._voiceChatListener.StopRecording();
				Object.Destroy(this._voiceChatListener.gameObject);
				this._voiceChatListener.OnAudioGenerated -= this.ProcesVoiceFragment;
				this._isRecording = false;
				this._voiceChatListener.isListening = false;
				this._isVoiceChatActive = false;
				this._voiceChatListener = null;
			}
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0002025A File Offset: 0x0001E45A
		public void MenuSceneLoaded()
		{
			if (!Client.Instance.isLogged)
			{
				return;
			}
			this._currentScene = "MenuViewControllers";
			this.isPlaying = false;
			this.PauseRestartButtonSwitch(true);
			this.DestroyScoreScreen();
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x00020288 File Offset: 0x0001E488
		public void GameSceneLoaded()
		{
			if (!Client.Instance.isLogged)
			{
				return;
			}
			this._currentScene = "GameCore";
			this.isPlaying = true;
			this.PauseRestartButtonSwitch(false);
			this.CreateScoreScreen();
			if (this._scoreController == null)
			{
				this._scoreController = Resources.FindObjectsOfTypeAll<ScoreController>().FirstOrDefault<ScoreController>();
			}
			if (this._scoreController != null && Client.Instance.isLogged)
			{
				this._scoreController.scoreDidChangeEvent -= this.SubmitScore;
				this._scoreController.scoreDidChangeEvent += this.SubmitScore;
				if (PluginConfig.Instance.ExposesStatusEnabled)
				{
					this._scoreController.comboDidChangeEvent -= this.SubmitCombo;
					this._scoreController.comboDidChangeEvent += this.SubmitCombo;
					this._scoreController.noteWasMissedEvent -= this.SubmitNoteWasMissed;
					this._scoreController.noteWasMissedEvent += this.SubmitNoteWasMissed;
					this._scoreController.noteWasCutEvent -= this.SubmitNoteWasCut;
					this._scoreController.noteWasCutEvent += this.SubmitNoteWasCut;
				}
			}
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x000203C6 File Offset: 0x0001E5C6
		private void PauseRestartButtonSwitch(bool flg)
		{
			if (this._pauseMenuManager == null)
			{
				this._pauseMenuManager = Object.FindObjectOfType<PauseMenuManager>();
			}
			if (this._pauseMenuManager != null)
			{
				this._pauseMenuManager.GetPrivateField("_restartButton").interactable = flg;
			}
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x00020408 File Offset: 0x0001E608
		private void SubmitScore(int num, int score)
		{
			this._score = score;
			if (PluginConfig.Instance.ExposesStatusEnabled)
			{
				Task.Run(delegate
				{
					this.SendGameStatus();
				});
			}
			Client.Instance.player.score = score;
			RoomBroadcastDataUpdateScoreStruct roomBroadcastDataUpdateScoreStruct = new RoomBroadcastDataUpdateScoreStruct(Client.Instance.player.playerId, score);
			Client.Instance.RoomBroadcast(RoomBroadcastDataType.UpdateScore, JsonConvert.SerializeObject(roomBroadcastDataUpdateScoreStruct));
			foreach (Player player in this._players)
			{
				if (player.playerId == Client.Instance.player.playerId)
				{
					player.score = score;
				}
			}
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x000204D4 File Offset: 0x0001E6D4
		private void SubmitCombo(int num)
		{
			this._combo = num;
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x000204DD File Offset: 0x0001E6DD
		private void SubmitNoteWasMissed(NoteData noteData, int num)
		{
			this._noteMissedCounter++;
			Task.Run(delegate
			{
				this.SendGameStatus();
			});
		}

		// Token: 0x060007BD RID: 1981 RVA: 0x000204FF File Offset: 0x0001E6FF
		private void SubmitNoteWasCut(NoteData noteData, NoteCutInfo noteCutInfo, int num)
		{
			this._noteWasCutCounter++;
		}

		// Token: 0x060007BE RID: 1982 RVA: 0x0002050F File Offset: 0x0001E70F
		private void SendGameStatus()
		{
			Client.Instance.SendGameStatus(this._score, this._combo, this._scoreController.maxCombo, this._noteWasCutCounter, this._noteMissedCounter);
		}

		// Token: 0x060007BF RID: 1983 RVA: 0x0002050F File Offset: 0x0001E70F
		private void SubmitMaxCombo()
		{
			Client.Instance.SendGameStatus(this._score, this._combo, this._scoreController.maxCombo, this._noteWasCutCounter, this._noteMissedCounter);
		}

		// Token: 0x060007C0 RID: 1984 RVA: 0x0002053E File Offset: 0x0001E73E
		public void SetPlayers(Player[] players)
		{
			if (this._players == null)
			{
				this._players = new List<Player>(players.Length);
			}
			else
			{
				this._players.Clear();
			}
			this._players = players.ToList<Player>();
		}

		// Token: 0x060007C1 RID: 1985 RVA: 0x00020570 File Offset: 0x0001E770
		public void SetPlayerControllers(Player[] players, bool fullRefresh = false)
		{
			long[] array = new long[0];
			foreach (Player player in players)
			{
				array.Append(player.playerId);
				if (fullRefresh || !this.playerControllers.ContainsKey(player.playerId))
				{
					MultiPlayerController multiPlayerController = new GameObject("MultiPlayerController").AddComponent<MultiPlayerController>();
					multiPlayerController.player = player;
					this.playerControllers[player.playerId] = multiPlayerController;
				}
			}
			long[] array2 = new long[0];
			foreach (KeyValuePair<long, MultiPlayerController> keyValuePair in this.playerControllers)
			{
				if (!array.Contains(keyValuePair.Key))
				{
					array2.Append(keyValuePair.Key);
				}
			}
			foreach (long num in array2)
			{
				this.playerControllers.Remove(num);
			}
		}

		// Token: 0x060007C2 RID: 1986 RVA: 0x00020678 File Offset: 0x0001E878
		public void PlayVOIPFragment(VoipFragment data)
		{
			if (this._speexDec == null || this._speexDec.mode != data.mode)
			{
				this._speexDec = SpeexCodex.Create(data.mode);
			}
			MultiPlayerController multiPlayerController;
			if (this.playerControllers.TryGetValue(data.playerId, out multiPlayerController))
			{
				if (multiPlayerController != null)
				{
					multiPlayerController.VoIPUpdate();
				}
				if (multiPlayerController != null)
				{
					multiPlayerController.PlayVoIPFragment(this._speexDec.Decode(data.data), data.index);
				}
			}
		}

		// Token: 0x060007C3 RID: 1987 RVA: 0x000206F0 File Offset: 0x0001E8F0
		public void SetVoIPVolume(float volume)
		{
			foreach (KeyValuePair<long, MultiPlayerController> keyValuePair in this.playerControllers)
			{
				keyValuePair.Value.SetVoIPVolume(volume);
			}
		}

		// Token: 0x060007C4 RID: 1988 RVA: 0x0002074C File Offset: 0x0001E94C
		private void CreateScoreScreen()
		{
			if (this._scoreScreen != null)
			{
				return;
			}
			this._scoreScreen = new GameObject("ScoreScreen", new Type[] { typeof(RectTransform) });
			this._scoreScreen.transform.position = new Vector3(0.7f, 3.9f, 12f);
			this._scoreScreen.transform.rotation = Quaternion.Euler(Vector3.zero);
			this._scoreScreen.transform.localScale = Vector3.one;
			Image image = new GameObject("Background", new Type[]
			{
				typeof(Canvas),
				typeof(CanvasRenderer)
			}).AddComponent<Image>();
			image.GetComponent<Canvas>().renderMode = 2;
			image.rectTransform.SetParent(this._scoreScreen.transform);
			image.rectTransform.localScale = new Vector3(0.18f, 0.0525f, 1f);
			image.rectTransform.pivot = new Vector2(0.5f, 0.5f);
			image.rectTransform.anchoredPosition3D = new Vector3(-0.75f, 2.5f, 0.01f);
			image.sprite = Sprites.whitePixel;
			image.material = Sprites.NoGlowMat;
			image.color = new Color(0f, 0f, 0f, 0.5f);
			if (this._scoreDisplays == null)
			{
				this._scoreDisplays = new List<PlayerInfoDisplay>();
				return;
			}
			if (this._scoreDisplays.Count > 0)
			{
				foreach (PlayerInfoDisplay playerInfoDisplay in this._scoreDisplays)
				{
					Object.Destroy(playerInfoDisplay.gameObject);
				}
				this._scoreDisplays.Clear();
			}
		}

		// Token: 0x060007C5 RID: 1989 RVA: 0x00020930 File Offset: 0x0001EB30
		public void UpdatePlayerScore()
		{
			if (this._scoreDisplays.Count > 0)
			{
				foreach (PlayerInfoDisplay playerInfoDisplay in this._scoreDisplays)
				{
					Object.Destroy(playerInfoDisplay.gameObject);
				}
				this._scoreDisplays.Clear();
			}
			for (int i = 0; i < this._players.Count; i++)
			{
				PlayerInfoDisplay playerInfoDisplay2 = new GameObject("ScoreDisplay " + i.ToString()).AddComponent<PlayerInfoDisplay>();
				playerInfoDisplay2.transform.SetParent(this._scoreScreen.transform, false);
				playerInfoDisplay2.transform.localPosition = new Vector3(0f, 2.5f - (float)i, 0f);
				playerInfoDisplay2.transform.localScale = Vector3.one;
				playerInfoDisplay2.transform.localRotation = Quaternion.identity;
				this._scoreDisplays.Add(playerInfoDisplay2);
				this._scoreDisplays[i].UpdatePlayerInfo(this._players[i], i);
			}
		}

		// Token: 0x060007C6 RID: 1990 RVA: 0x00020A5C File Offset: 0x0001EC5C
		private void DestroyScoreScreen()
		{
			try
			{
				int num = 0;
				for (;;)
				{
					int num2 = num;
					List<PlayerInfoDisplay> scoreDisplays = this._scoreDisplays;
					int? num3 = ((scoreDisplays != null) ? new int?(scoreDisplays.Count) : null);
					if (!((num2 < num3.GetValueOrDefault()) & (num3 != null)))
					{
						break;
					}
					if (this._scoreDisplays[num] != null)
					{
						Object.Destroy(this._scoreDisplays[num].gameObject);
					}
					num++;
				}
				List<PlayerInfoDisplay> scoreDisplays2 = this._scoreDisplays;
				if (scoreDisplays2 != null)
				{
					scoreDisplays2.Clear();
				}
				if (this._scoreScreen != null)
				{
					Object.Destroy(this._scoreScreen);
				}
			}
			catch (Exception ex)
			{
				Logger.log.Critical(ex);
			}
		}

		// Token: 0x060007C7 RID: 1991 RVA: 0x00020B1C File Offset: 0x0001ED1C
		private void ProcesVoiceFragment(VoipFragment fragment)
		{
			if (this._voiceChatListener.isListening)
			{
				fragment.playerId = Client.Instance.player.playerId;
				Client.Instance.SendVoIPData(fragment);
			}
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x00020B4B File Offset: 0x0001ED4B
		public void VoiceChatStartRecording()
		{
			if (this._voiceChatListener != null)
			{
				this._voiceChatListener.StartRecording();
			}
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x00020B66 File Offset: 0x0001ED66
		public void VoiceChatStopRecording()
		{
			if (this._voiceChatListener != null)
			{
				this._voiceChatListener.StopRecording();
			}
		}

		// Token: 0x060007CA RID: 1994 RVA: 0x00020B81 File Offset: 0x0001ED81
		private void Update()
		{
			if (!Client.Instance.isLogged)
			{
				return;
			}
			base.StartCoroutine(this.UpdatePlayerScoreCoroutine());
			base.StartCoroutine(this.VOIPRecordCoroutine());
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x00020BAA File Offset: 0x0001EDAA
		private IEnumerator UpdatePlayerScoreCoroutine()
		{
			if (!this.isPlaying)
			{
				yield break;
			}
			if (this._messageDisplayTime > 0f)
			{
				this._messageDisplayTime -= Time.deltaTime;
				yield break;
			}
			this._messageDisplayTime = this._messageDisplayDelayTime;
			if (this._players == null || this._players.Count == 0)
			{
				yield break;
			}
			this.UpdatePlayerScore();
			yield break;
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x00020BB9 File Offset: 0x0001EDB9
		private IEnumerator VOIPRecordCoroutine()
		{
			if (!PluginConfig.Instance.CustomVoiceEnabled || !PluginConfig.Instance.MicphoneEnabled)
			{
				this._isRecording = false;
				if (this._voiceChatListener != null)
				{
					this._voiceChatListener.isListening = this._isRecording;
				}
				yield break;
			}
			this._isRecording = true;
			if (this._isVoiceChatActive && this._voiceChatListener != null)
			{
				if (!this._isRecording && this._voiceChatListener.isListening && !this._waitingForRecordingDelay)
				{
					this._PTTReleaseTime = Time.time;
					this._waitingForRecordingDelay = true;
				}
				else if (this._isRecording || !this._voiceChatListener.isListening || Time.time - this._PTTReleaseTime >= 0.2f)
				{
					this._voiceChatListener.isListening = this._isRecording;
					this._waitingForRecordingDelay = false;
				}
			}
			yield break;
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x00020BC8 File Offset: 0x0001EDC8
		public void SongFinished(StandardLevelScenesTransitionSetupDataSO sender, LevelCompletionResults levelCompletionResults)
		{
			this._score = 0;
			this._combo = 0;
			this._maxCombo = 0;
			this._noteWasCutCounter = 0;
			this._noteMissedCounter = 0;
			PluginUI.instance.multiPlayerFlowCoordinator.roomsFlowCoordinator.roomFlowCoordinator.ShowResult(levelCompletionResults);
			Client.Instance.RoomSubmitScore(true, 0, levelCompletionResults.rank.ToString(), levelCompletionResults.maxCombo, levelCompletionResults.modifiedScore, levelCompletionResults.goodCutsCount, levelCompletionResults.badCutsCount, levelCompletionResults.missedCount, (int)levelCompletionResults.endSongTime, (int)levelCompletionResults.songDuration, (int)levelCompletionResults.leftHandMovementDistance, (int)levelCompletionResults.rightHandMovementDistance, (int)levelCompletionResults.leftSaberMovementDistance, (int)levelCompletionResults.rightSaberMovementDistance, levelCompletionResults.okCount, levelCompletionResults.notGoodCount, levelCompletionResults.rawScore, levelCompletionResults.levelEndStateType.ToString());
		}

		// Token: 0x040003CD RID: 973
		public static InGameController instance;

		// Token: 0x040003CE RID: 974
		public Dictionary<long, MultiPlayerController> playerControllers = new Dictionary<long, MultiPlayerController>();

		// Token: 0x040003CF RID: 975
		private const string _gameSceneName = "GameCore";

		// Token: 0x040003D0 RID: 976
		private const string _menuSceneName = "MenuViewControllers";

		// Token: 0x040003D1 RID: 977
		private float _messageDisplayDelayTime = 1f;

		// Token: 0x040003D2 RID: 978
		private float _messageDisplayTime;

		// Token: 0x040003D3 RID: 979
		private string _currentScene;

		// Token: 0x040003D4 RID: 980
		private GameObject _scoreScreen;

		// Token: 0x040003D5 RID: 981
		private List<PlayerInfoDisplay> _scoreDisplays = new List<PlayerInfoDisplay>();

		// Token: 0x040003D6 RID: 982
		private List<Player> _players;

		// Token: 0x040003D7 RID: 983
		private VoipListener _voiceChatListener;

		// Token: 0x040003D8 RID: 984
		private bool _isVoiceChatActive;

		// Token: 0x040003D9 RID: 985
		private bool _isRecording;

		// Token: 0x040003DA RID: 986
		private bool _waitingForRecordingDelay;

		// Token: 0x040003DB RID: 987
		private float _PTTReleaseTime;

		// Token: 0x040003DC RID: 988
		private const float _PTTReleaseDelay = 0.2f;

		// Token: 0x040003DD RID: 989
		private SpeexCodex _speexDec;

		// Token: 0x040003DE RID: 990
		private PauseMenuManager _pauseMenuManager;

		// Token: 0x040003DF RID: 991
		private ScoreController _scoreController;

		// Token: 0x040003E0 RID: 992
		private int _score;

		// Token: 0x040003E1 RID: 993
		private int _combo;

		// Token: 0x040003E2 RID: 994
		private int _maxCombo;

		// Token: 0x040003E3 RID: 995
		private int _noteWasCutCounter;

		// Token: 0x040003E4 RID: 996
		private int _noteMissedCounter;
	}
}
