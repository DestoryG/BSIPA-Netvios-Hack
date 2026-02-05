using System;
using System.Collections;
using System.Collections.Generic;
using SongCore.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SongCore
{
	// Token: 0x02000014 RID: 20
	public class ProgressBar : MonoBehaviour
	{
		// Token: 0x0600010D RID: 269 RVA: 0x00004E68 File Offset: 0x00003068
		public static ProgressBar Create()
		{
			return new GameObject("Progress Bar").AddComponent<ProgressBar>();
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00004E7C File Offset: 0x0000307C
		public void ShowMessage(string message, float time)
		{
			base.StopAllCoroutines();
			this._showingMessage = true;
			this._headerText.text = message;
			this._loadingBar.enabled = false;
			this._loadingBackg.enabled = false;
			this._canvas.enabled = true;
			base.StartCoroutine(this.DisableCanvasRoutine(time));
		}

		// Token: 0x0600010F RID: 271 RVA: 0x00004ED4 File Offset: 0x000030D4
		public void ShowMessage(string message)
		{
			base.StopAllCoroutines();
			this._showingMessage = true;
			this._headerText.text = message;
			this._loadingBar.enabled = false;
			this._loadingBackg.enabled = false;
			this._canvas.enabled = true;
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00004F13 File Offset: 0x00003113
		private void OnEnable()
		{
			SceneManager.activeSceneChanged += new UnityAction<Scene, Scene>(this.SceneManagerOnActiveSceneChanged);
			Loader.LoadingStartedEvent += this.SongLoaderOnLoadingStartedEvent;
			Loader.SongsLoadedEvent += this.SongLoaderOnSongsLoadedEvent;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x00004F48 File Offset: 0x00003148
		private void OnDisable()
		{
			SceneManager.activeSceneChanged -= new UnityAction<Scene, Scene>(this.SceneManagerOnActiveSceneChanged);
			Loader.LoadingStartedEvent -= this.SongLoaderOnLoadingStartedEvent;
			Loader.SongsLoadedEvent -= this.SongLoaderOnSongsLoadedEvent;
		}

		// Token: 0x06000112 RID: 274 RVA: 0x00004F7D File Offset: 0x0000317D
		private void SceneManagerOnActiveSceneChanged(Scene oldScene, Scene newScene)
		{
			if (newScene.name == "MenuCore")
			{
				if (this._showingMessage)
				{
					this._canvas.enabled = true;
					return;
				}
			}
			else
			{
				this._canvas.enabled = false;
			}
		}

		// Token: 0x06000113 RID: 275 RVA: 0x00004FB4 File Offset: 0x000031B4
		private void SongLoaderOnLoadingStartedEvent(Loader obj)
		{
			base.StopAllCoroutines();
			this._showingMessage = false;
			this._headerText.text = "Loading songs...";
			this._loadingBar.enabled = true;
			this._loadingBackg.enabled = true;
			this._canvas.enabled = true;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x00005004 File Offset: 0x00003204
		private void SongLoaderOnSongsLoadedEvent(Loader arg1, Dictionary<string, CustomPreviewBeatmapLevel> arg2)
		{
			this._showingMessage = false;
			this._headerText.text = arg2.Count.ToString() + " songs loaded.";
			this._loadingBar.enabled = false;
			this._loadingBackg.enabled = false;
			base.StartCoroutine(this.DisableCanvasRoutine(5f));
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00005065 File Offset: 0x00003265
		private IEnumerator DisableCanvasRoutine(float time)
		{
			yield return new WaitForSecondsRealtime(time);
			this._canvas.enabled = false;
			this._showingMessage = false;
			yield break;
		}

		// Token: 0x06000116 RID: 278 RVA: 0x0000507C File Offset: 0x0000327C
		private void Awake()
		{
			base.gameObject.transform.position = ProgressBar.Position;
			base.gameObject.transform.eulerAngles = ProgressBar.Rotation;
			base.gameObject.transform.localScale = ProgressBar.Scale;
			this._canvas = base.gameObject.AddComponent<Canvas>();
			this._canvas.renderMode = 2;
			this._canvas.enabled = false;
			(this._canvas.transform as RectTransform).sizeDelta = ProgressBar.CanvasSize;
			this._authorNameText = Utils.CreateText(this._canvas.transform as RectTransform, "", ProgressBar.AuthorNamePosition);
			RectTransform rectTransform = this._authorNameText.transform as RectTransform;
			rectTransform.SetParent(this._canvas.transform, false);
			rectTransform.anchoredPosition = ProgressBar.AuthorNamePosition;
			rectTransform.sizeDelta = ProgressBar.HeaderSize;
			this._authorNameText.text = "";
			this._authorNameText.fontSize = 7f;
			this._pluginNameText = Utils.CreateText(this._canvas.transform as RectTransform, "SongCore Loader", ProgressBar.PluginNamePosition);
			RectTransform rectTransform2 = this._pluginNameText.transform as RectTransform;
			rectTransform2.SetParent(this._canvas.transform, false);
			rectTransform2.sizeDelta = ProgressBar.HeaderSize;
			rectTransform2.anchoredPosition = ProgressBar.PluginNamePosition;
			this._pluginNameText.text = "SongCore Loader";
			this._pluginNameText.fontSize = 9f;
			this._headerText = Utils.CreateText(this._canvas.transform as RectTransform, "Loading songs...", ProgressBar.HeaderPosition);
			RectTransform rectTransform3 = this._headerText.transform as RectTransform;
			rectTransform3.SetParent(this._canvas.transform, false);
			rectTransform3.anchoredPosition = ProgressBar.HeaderPosition;
			rectTransform3.sizeDelta = ProgressBar.HeaderSize;
			this._headerText.text = "Loading songs...";
			this._headerText.fontSize = 15f;
			this._loadingBackg = new GameObject("Background").AddComponent<Image>();
			RectTransform rectTransform4 = this._loadingBackg.transform as RectTransform;
			rectTransform4.SetParent(this._canvas.transform, false);
			rectTransform4.sizeDelta = ProgressBar.LoadingBarSize;
			this._loadingBackg.color = ProgressBar.BackgroundColor;
			this._loadingBar = new GameObject("Loading Bar").AddComponent<Image>();
			RectTransform rectTransform5 = this._loadingBar.transform as RectTransform;
			rectTransform5.SetParent(this._canvas.transform, false);
			rectTransform5.sizeDelta = ProgressBar.LoadingBarSize;
			Texture2D whiteTexture = Texture2D.whiteTexture;
			Sprite sprite = Sprite.Create(whiteTexture, new Rect(0f, 0f, (float)whiteTexture.width, (float)whiteTexture.height), Vector2.one * 0.5f, 100f, 1U);
			this._loadingBar.sprite = sprite;
			this._loadingBar.type = 3;
			this._loadingBar.fillMethod = 0;
			this._loadingBar.color = new Color(1f, 1f, 1f, 0.5f);
			Object.DontDestroyOnLoad(base.gameObject);
		}

		// Token: 0x06000117 RID: 279 RVA: 0x000053A4 File Offset: 0x000035A4
		private void Update()
		{
			if (!this._canvas.enabled)
			{
				return;
			}
			this._loadingBar.fillAmount = Loader.LoadingProgress;
			this._loadingBar.color = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * 0.35f, 1f), 1f, 1f));
			this._headerText.color = HSBColor.ToColor(new HSBColor(Mathf.PingPong(Time.time * 0.35f, 1f), 1f, 1f));
		}

		// Token: 0x04000052 RID: 82
		private Canvas _canvas;

		// Token: 0x04000053 RID: 83
		private TMP_Text _authorNameText;

		// Token: 0x04000054 RID: 84
		private TMP_Text _pluginNameText;

		// Token: 0x04000055 RID: 85
		private TMP_Text _headerText;

		// Token: 0x04000056 RID: 86
		internal Image _loadingBackg;

		// Token: 0x04000057 RID: 87
		internal Image _loadingBar;

		// Token: 0x04000058 RID: 88
		private static readonly Vector3 Position = new Vector3(0f, 2.5f, 2.5f);

		// Token: 0x04000059 RID: 89
		private static readonly Vector3 Rotation = new Vector3(0f, 0f, 0f);

		// Token: 0x0400005A RID: 90
		private static readonly Vector3 Scale = new Vector3(0.01f, 0.01f, 0.01f);

		// Token: 0x0400005B RID: 91
		private static readonly Vector2 CanvasSize = new Vector2(100f, 50f);

		// Token: 0x0400005C RID: 92
		private const string AuthorNameText = "";

		// Token: 0x0400005D RID: 93
		private const float AuthorNameFontSize = 7f;

		// Token: 0x0400005E RID: 94
		private static readonly Vector2 AuthorNamePosition = new Vector2(10f, 31f);

		// Token: 0x0400005F RID: 95
		private const string PluginNameText = "SongCore Loader";

		// Token: 0x04000060 RID: 96
		private const float PluginNameFontSize = 9f;

		// Token: 0x04000061 RID: 97
		private static readonly Vector2 PluginNamePosition = new Vector2(10f, 23f);

		// Token: 0x04000062 RID: 98
		private static readonly Vector2 HeaderPosition = new Vector2(10f, 15f);

		// Token: 0x04000063 RID: 99
		private static readonly Vector2 HeaderSize = new Vector2(100f, 20f);

		// Token: 0x04000064 RID: 100
		private const string HeaderText = "Loading songs...";

		// Token: 0x04000065 RID: 101
		private const float HeaderFontSize = 15f;

		// Token: 0x04000066 RID: 102
		private static readonly Vector2 LoadingBarSize = new Vector2(100f, 10f);

		// Token: 0x04000067 RID: 103
		private static readonly Color BackgroundColor = new Color(0f, 0f, 0f, 0.2f);

		// Token: 0x04000068 RID: 104
		private bool _showingMessage;
	}
}
