using System;
using BeatSaberMultiplayer.Data;
using BeatSaberMultiplayer.Helper;
using SongCore.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BeatSaberMultiplayer.UI
{
	// Token: 0x02000051 RID: 81
	public class PlayerInfoDisplay : MonoBehaviour
	{
		// Token: 0x060006CC RID: 1740 RVA: 0x0001BD2C File Offset: 0x00019F2C
		public void Awake()
		{
			this.playerNameText = CustomExtensions.CreateWorldText(base.transform, "");
			this.playerNameText.rectTransform.anchoredPosition = new Vector2(44f, -47.5f);
			this.playerNameText.fontSize = 7f;
			this.playerScoreText = CustomExtensions.CreateWorldText(base.transform, "");
			this.playerScoreText.rectTransform.anchoredPosition = new Vector2(55f, -47.5f);
			this.playerScoreText.fontSize = 8f;
			this.playerSpeakerIcon = new GameObject("Player Speaker Icon", new Type[]
			{
				typeof(Canvas),
				typeof(CanvasRenderer)
			}).AddComponent<Image>();
			this.playerSpeakerIcon.GetComponent<Canvas>().renderMode = 2;
			this.playerSpeakerIcon.rectTransform.SetParent(base.transform);
			this.playerSpeakerIcon.rectTransform.localScale = new Vector3(0.008f, 0.008f, 1f);
			this.playerSpeakerIcon.rectTransform.pivot = new Vector2(0.5f, 0.5f);
			this.playerSpeakerIcon.rectTransform.anchoredPosition3D = new Vector3(-8.5f, 2f, 0f);
			this.playerSpeakerIcon.sprite = Sprites.speakerIcon;
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x0001BE98 File Offset: 0x0001A098
		public void UpdatePlayerInfo(Player _player, int _index)
		{
			this._player = _player;
			if (_player != null)
			{
				this.playerNameText.text = _player.nickname;
				this.playerNameText.color = new Color32(byte.MaxValue, 3, 4, 5);
				this._previousScore = this._currentScore;
				this._currentScore = _player.score;
				this.playerScoreText.text = this._currentScore.ToString();
				this._progress = 0f;
				return;
			}
			this.playerNameText.text = "";
			this.playerScoreText.text = "";
			this.playerSpeakerIcon.gameObject.SetActive(false);
		}

		// Token: 0x04000330 RID: 816
		private Player _player;

		// Token: 0x04000331 RID: 817
		public TextMeshPro playerNameText;

		// Token: 0x04000332 RID: 818
		public TextMeshPro playerScoreText;

		// Token: 0x04000333 RID: 819
		public Image playerSpeakerIcon;

		// Token: 0x04000334 RID: 820
		private int _previousScore;

		// Token: 0x04000335 RID: 821
		private int _currentScore;

		// Token: 0x04000336 RID: 822
		private float _progress;

		// Token: 0x04000337 RID: 823
		private HSBColor _color;
	}
}
