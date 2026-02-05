using System;
using System.Collections;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.ViewControllers;
using CustomSaber.Data;
using CustomSaber.Utilities;
using HMUI;
using IPA.Utilities;
using TMPro;
using UnityEngine;
using VRUIControls;
using Xft;

namespace CustomSaber.Settings.UI
{
	// Token: 0x02000017 RID: 23
	internal class SaberListViewController : BSMLResourceViewController
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x000047A2 File Offset: 0x000029A2
		public override string ResourceName
		{
			get
			{
				return "CustomSaber.Settings.UI.Views.saberList.bsml";
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x000047AC File Offset: 0x000029AC
		[UIAction("saberSelect")]
		public void Select(TableView _, int row)
		{
			SaberAssetLoader.SelectedSaber = row;
			Configuration.CurrentlySelectedSaber = SaberAssetLoader.CustomSabers[row].FileName;
			Action<CustomSaberData> action = this.customSaberChanged;
			if (action != null)
			{
				action(SaberAssetLoader.CustomSabers[row]);
			}
			base.StartCoroutine(this.GenerateSaberPreview(row));
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00004802 File Offset: 0x00002A02
		[UIAction("reloadSabers")]
		public void ReloadMaterials()
		{
			SaberAssetLoader.Reload();
			this.SetupList();
			this.Select(this.customListTableData.tableView, SaberAssetLoader.SelectedSaber);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x0000482C File Offset: 0x00002A2C
		[UIAction("deleteSaber")]
		public void DeleteCurrentSaber()
		{
			int num = SaberAssetLoader.DeleteCurrentSaber();
			bool flag = num == 0;
			if (!flag)
			{
				this.SetupList();
				this.Select(this.customListTableData.tableView, SaberAssetLoader.SelectedSaber);
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00004868 File Offset: 0x00002A68
		[UIAction("update-confirmation")]
		public void UpdateDeleteConfirmation()
		{
			this.confirmationText.text = "你确定要删除\n<color=\"red\">" + SaberAssetLoader.CustomSabers[SaberAssetLoader.SelectedSaber].Descriptor.SaberName + "</color>?";
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000048A0 File Offset: 0x00002AA0
		[UIAction("#post-parse")]
		public void SetupList()
		{
			this.customListTableData.data.Clear();
			foreach (CustomSaberData customSaberData in SaberAssetLoader.CustomSabers)
			{
				string saberName = customSaberData.Descriptor.SaberName;
				string authorName = customSaberData.Descriptor.AuthorName;
				Sprite coverImage = customSaberData.Descriptor.CoverImage;
				CustomListTableData.CustomCellInfo customCellInfo = new CustomListTableData.CustomCellInfo(saberName, authorName, (coverImage != null) ? coverImage.texture : null);
				this.customListTableData.data.Add(customCellInfo);
			}
			this.customListTableData.tableView.ReloadData();
			int selectedSaber = SaberAssetLoader.SelectedSaber;
			this.customListTableData.tableView.SelectCellWithIdx(selectedSaber, false);
			bool flag = !this.customListTableData.tableView.visibleCells.Where((TableCell x) => x.selected).Any<TableCell>();
			if (flag)
			{
				this.customListTableData.tableView.ScrollToCellWithIdx(selectedSaber, 0, true);
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000049C0 File Offset: 0x00002BC0
		protected override void DidActivate(bool firstActivation, ViewController.ActivationType type)
		{
			base.DidActivate(firstActivation, type);
			SaberListViewController.Instance = this;
			bool flag = !this.preview;
			if (flag)
			{
				this.preview = new GameObject("Preview");
				this.preview.transform.position = new Vector3(2.2f, 1.3f, 1f);
				this.preview.transform.Rotate(0f, 330f, 0f);
			}
			this.Select(this.customListTableData.tableView, SaberAssetLoader.SelectedSaber);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00004A5D File Offset: 0x00002C5D
		protected override void DidDeactivate(ViewController.DeactivationType deactivationType)
		{
			base.DidDeactivate(deactivationType);
			this.ClearPreview();
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00004A6F File Offset: 0x00002C6F
		public IEnumerator GenerateSaberPreview(int selectedSaber)
		{
			bool flag = !this.isGeneratingPreview;
			if (flag)
			{
				yield return new WaitUntil(() => DefaultSaberGrabber.isCompleted);
				try
				{
					this.isGeneratingPreview = true;
					this.ClearSabers();
					CustomSaberData customSaber = SaberAssetLoader.CustomSabers[selectedSaber];
					bool flag2 = customSaber != null && customSaber.Sabers != null;
					if (flag2)
					{
						this.previewSabers = this.CreatePreviewSaber(customSaber.Sabers, this.preview.transform, this.sabersPos);
						Vector3 vector = this.saberLeftPos;
						GameObject gameObject = this.previewSabers;
						this.PositionPreviewSaber(vector, (gameObject != null) ? gameObject.transform.Find("LeftSaber").gameObject : null);
						Vector3 vector2 = this.saberRightPos;
						GameObject gameObject2 = this.previewSabers;
						this.PositionPreviewSaber(vector2, (gameObject2 != null) ? gameObject2.transform.Find("RightSaber").gameObject : null);
						GameObject gameObject3 = this.previewSabers;
						if (gameObject3 != null)
						{
							gameObject3.transform.Find("LeftSaber").gameObject.SetActive(true);
						}
						GameObject gameObject4 = this.previewSabers;
						if (gameObject4 != null)
						{
							gameObject4.transform.Find("LeftSaber").gameObject.gameObject.AddComponent<DummySaber>();
						}
						GameObject gameObject5 = this.previewSabers;
						if (gameObject5 != null)
						{
							gameObject5.transform.Find("RightSaber").gameObject.SetActive(true);
						}
						GameObject gameObject6 = this.previewSabers;
						if (gameObject6 != null)
						{
							gameObject6.transform.Find("RightSaber").gameObject.gameObject.AddComponent<DummySaber>();
						}
						bool showSabersInSaberMenu = Configuration.ShowSabersInSaberMenu;
						if (showSabersInSaberMenu)
						{
							this.GenerateHandheldSaberPreview();
						}
					}
					customSaber = null;
				}
				catch (Exception ex2)
				{
					Exception ex = ex2;
					Logger.log.Error(ex);
				}
				finally
				{
					this.isGeneratingPreview = false;
				}
			}
			yield break;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00004A88 File Offset: 0x00002C88
		private GameObject CreatePreviewSaber(GameObject saber, Transform transform, Vector3 localPosition)
		{
			GameObject gameObject = this.InstantiateGameObject(saber, transform);
			gameObject.name = "Preview Saber Object";
			this.PositionPreviewSaber(localPosition, gameObject);
			return gameObject;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00004ABC File Offset: 0x00002CBC
		public void GenerateHandheldSaberPreview()
		{
			bool flag = Environment.CommandLine.Contains("fpfc");
			if (!flag)
			{
				CustomSaberData customSaberData = SaberAssetLoader.CustomSabers[SaberAssetLoader.SelectedSaber];
				VRController[] array = Resources.FindObjectsOfTypeAll<VRController>();
				GameObject gameObject = this.CreatePreviewSaber(customSaberData.Sabers, this.preview.transform, this.sabersPos);
				ColorManager colorManager = Resources.FindObjectsOfTypeAll<ColorManager>().First<ColorManager>();
				try
				{
					VRController[] array2 = array;
					int i = 0;
					while (i < array2.Length)
					{
						VRController vrcontroller = array2[i];
						bool flag2 = vrcontroller != null && vrcontroller.node == 4;
						if (flag2)
						{
							this.leftSaber = ((gameObject != null) ? gameObject.transform.Find("LeftSaber").gameObject : null);
							bool flag3 = !this.leftSaber;
							if (!flag3)
							{
								this.leftSaber.transform.parent = vrcontroller.transform;
								this.leftSaber.transform.position = vrcontroller.transform.position;
								this.leftSaber.transform.rotation = vrcontroller.transform.rotation;
								this.leftSaber.SetActive(true);
								CustomTrail[] componentsInChildren = this.leftSaber.GetComponentsInChildren<CustomTrail>();
								bool flag4 = componentsInChildren == null || componentsInChildren.Count<CustomTrail>() == 0;
								if (flag4)
								{
									GameObject gameObject2 = Object.Instantiate<GameObject>(DefaultSaberGrabber.defaultLeftSaber, this.leftSaber.transform);
									gameObject2.SetActive(true);
									gameObject2.transform.localPosition = Vector3.zero;
									gameObject2.transform.localRotation = Quaternion.identity;
									gameObject2.transform.Find("BasicSaber").gameObject.SetActive(false);
								}
								else
								{
									foreach (CustomTrail customTrail in componentsInChildren)
									{
										customTrail.Length = (Configuration.OverrideTrailLength ? ((int)((float)customTrail.Length * Configuration.TrailLength)) : customTrail.Length);
										bool flag5 = customTrail.Length < 2 || !customTrail.PointStart || !customTrail.PointEnd;
										if (!flag5)
										{
											this.leftSaber.AddComponent<CustomWeaponTrail>().Init(DefaultSaberGrabber.trail.GetField("_trailRendererPrefab"), colorManager, customTrail.PointStart, customTrail.PointEnd, customTrail.TrailMaterial, customTrail.TrailColor, customTrail.Length, customTrail.MultiplierColor, customTrail.colorType);
										}
									}
								}
								this.leftSaber.AddComponent<DummySaber>();
								Transform transform = vrcontroller.transform.Find("MenuHandle");
								if (transform != null)
								{
									transform.gameObject.SetActive(false);
								}
								goto IL_04FA;
							}
						}
						else
						{
							bool flag6 = vrcontroller != null && vrcontroller.node == 5;
							if (!flag6)
							{
								goto IL_04FA;
							}
							this.rightSaber = ((gameObject != null) ? gameObject.transform.Find("RightSaber").gameObject : null);
							bool flag7 = !this.rightSaber;
							if (!flag7)
							{
								this.rightSaber.transform.parent = vrcontroller.transform;
								this.rightSaber.transform.position = vrcontroller.transform.position;
								this.rightSaber.transform.rotation = vrcontroller.transform.rotation;
								this.rightSaber.SetActive(true);
								CustomTrail[] componentsInChildren2 = this.rightSaber.GetComponentsInChildren<CustomTrail>();
								bool flag8 = componentsInChildren2 == null || componentsInChildren2.Count<CustomTrail>() == 0;
								if (flag8)
								{
									GameObject gameObject3 = Object.Instantiate<GameObject>(DefaultSaberGrabber.defaultRightSaber, this.rightSaber.transform);
									gameObject3.SetActive(true);
									gameObject3.transform.localPosition = Vector3.zero;
									gameObject3.transform.localRotation = Quaternion.identity;
									gameObject3.transform.Find("BasicSaber").gameObject.SetActive(false);
								}
								else
								{
									foreach (CustomTrail customTrail2 in componentsInChildren2)
									{
										customTrail2.Length = (Configuration.OverrideTrailLength ? ((int)((float)customTrail2.Length * Configuration.TrailLength)) : customTrail2.Length);
										bool flag9 = customTrail2.Length < 2 || !customTrail2.PointStart || !customTrail2.PointEnd;
										if (!flag9)
										{
											this.rightSaber.AddComponent<CustomWeaponTrail>().Init(DefaultSaberGrabber.trail.GetField("_trailRendererPrefab"), colorManager, customTrail2.PointStart, customTrail2.PointEnd, customTrail2.TrailMaterial, customTrail2.TrailColor, customTrail2.Length, customTrail2.MultiplierColor, customTrail2.colorType);
										}
									}
								}
								this.rightSaber.AddComponent<DummySaber>();
								Transform transform2 = vrcontroller.transform.Find("MenuHandle");
								if (transform2 != null)
								{
									transform2.gameObject.SetActive(false);
								}
								goto IL_04FA;
							}
						}
						IL_051E:
						i++;
						continue;
						IL_04FA:
						bool flag10 = this.leftSaber && this.rightSaber;
						if (flag10)
						{
							break;
						}
						goto IL_051E;
					}
					base.StartCoroutine(this.HideOrShowPointer(false));
				}
				finally
				{
					this.DestroyGameObject(ref gameObject);
				}
			}
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00005034 File Offset: 0x00003234
		private GameObject InstantiateGameObject(GameObject gameObject, Transform transform = null)
		{
			bool flag = gameObject;
			GameObject gameObject2;
			if (flag)
			{
				gameObject2 = (transform ? Object.Instantiate<GameObject>(gameObject, transform) : Object.Instantiate<GameObject>(gameObject));
			}
			else
			{
				gameObject2 = null;
			}
			return gameObject2;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x0000506C File Offset: 0x0000326C
		private void PositionPreviewSaber(Vector3 vector, GameObject saberObject)
		{
			bool flag = saberObject;
			if (flag)
			{
				saberObject.transform.localPosition = vector;
			}
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00005093 File Offset: 0x00003293
		private void ClearPreview()
		{
			this.ClearSabers();
			this.DestroyGameObject(ref this.preview);
			this.ShowMenuHandles();
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000050B1 File Offset: 0x000032B1
		private void ClearSabers()
		{
			this.DestroyGameObject(ref this.previewSabers);
			this.ClearHandheldSabers();
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x000050C8 File Offset: 0x000032C8
		public void ClearHandheldSabers()
		{
			this.DestroyGameObject(ref this.leftSaber);
			this.DestroyGameObject(ref this.rightSaber);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000050E5 File Offset: 0x000032E5
		private IEnumerator HideOrShowPointer(bool enable = false)
		{
			yield return new WaitUntil(() => this.pointer = Resources.FindObjectsOfTypeAll<VRPointer>().FirstOrDefault<VRPointer>());
			bool flag = this.initialSize == -1f;
			if (flag)
			{
				this.initialSize = this.pointer.GetField("_laserPointerWidth");
			}
			this.pointer.SetField("_laserPointerWidth", enable ? this.initialSize : 0f);
			yield break;
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x000050FC File Offset: 0x000032FC
		public void ShowMenuHandles()
		{
			foreach (VRController vrcontroller in Resources.FindObjectsOfTypeAll<VRController>())
			{
				Transform transform = vrcontroller.transform;
				if (transform != null)
				{
					Transform transform2 = transform.Find("MenuHandle");
					if (transform2 != null)
					{
						GameObject gameObject = transform2.gameObject;
						if (gameObject != null)
						{
							gameObject.SetActive(true);
						}
					}
				}
			}
			base.StartCoroutine(this.HideOrShowPointer(true));
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00005164 File Offset: 0x00003364
		private void DestroyGameObject(ref GameObject gameObject)
		{
			bool flag = gameObject;
			if (flag)
			{
				Object.DestroyImmediate(gameObject);
				gameObject = null;
			}
		}

		// Token: 0x04000065 RID: 101
		public static SaberListViewController Instance;

		// Token: 0x04000066 RID: 102
		private bool isGeneratingPreview;

		// Token: 0x04000067 RID: 103
		private GameObject preview;

		// Token: 0x04000068 RID: 104
		private GameObject previewSabers;

		// Token: 0x04000069 RID: 105
		private GameObject leftSaber;

		// Token: 0x0400006A RID: 106
		private GameObject rightSaber;

		// Token: 0x0400006B RID: 107
		private Vector3 sabersPos = new Vector3(0f, 0f, 0f);

		// Token: 0x0400006C RID: 108
		private Vector3 saberLeftPos = new Vector3(0f, 0f, 0f);

		// Token: 0x0400006D RID: 109
		private Vector3 saberRightPos = new Vector3(0f, 0.5f, 0f);

		// Token: 0x0400006E RID: 110
		public Action<CustomSaberData> customSaberChanged;

		// Token: 0x0400006F RID: 111
		[UIComponent("saberList")]
		public CustomListTableData customListTableData;

		// Token: 0x04000070 RID: 112
		[UIComponent("delete-saber-confirmation-text")]
		public TextMeshProUGUI confirmationText;

		// Token: 0x04000071 RID: 113
		private float initialSize = -1f;

		// Token: 0x04000072 RID: 114
		private VRPointer pointer = null;
	}
}
