using System;
using UnityEngine;

namespace SongCore.Utilities
{
	// Token: 0x02000016 RID: 22
	[Serializable]
	public struct HSBColor
	{
		// Token: 0x06000128 RID: 296 RVA: 0x00005C68 File Offset: 0x00003E68
		public HSBColor(float h, float s, float b, float a)
		{
			this.h = h;
			this.s = s;
			this.b = b;
			this.a = a;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00005C87 File Offset: 0x00003E87
		public HSBColor(float h, float s, float b)
		{
			this.h = h;
			this.s = s;
			this.b = b;
			this.a = 1f;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00005CAC File Offset: 0x00003EAC
		public HSBColor(Color col)
		{
			HSBColor hsbcolor = HSBColor.FromColor(col);
			this.h = hsbcolor.h;
			this.s = hsbcolor.s;
			this.b = hsbcolor.b;
			this.a = hsbcolor.a;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00005CF0 File Offset: 0x00003EF0
		public static HSBColor FromColor(Color color)
		{
			HSBColor hsbcolor = new HSBColor(0f, 0f, 0f, color.a);
			float r = color.r;
			float g = color.g;
			float num = color.b;
			float num2 = Mathf.Max(r, Mathf.Max(g, num));
			if (num2 <= 0f)
			{
				return hsbcolor;
			}
			float num3 = Mathf.Min(r, Mathf.Min(g, num));
			float num4 = num2 - num3;
			if (num2 > num3)
			{
				if (g == num2)
				{
					hsbcolor.h = (num - r) / num4 * 60f + 120f;
				}
				else if (num == num2)
				{
					hsbcolor.h = (r - g) / num4 * 60f + 240f;
				}
				else if (num > g)
				{
					hsbcolor.h = (g - num) / num4 * 60f + 360f;
				}
				else
				{
					hsbcolor.h = (g - num) / num4 * 60f;
				}
				if (hsbcolor.h < 0f)
				{
					hsbcolor.h += 360f;
				}
			}
			else
			{
				hsbcolor.h = 0f;
			}
			hsbcolor.h *= 0.0027777778f;
			hsbcolor.s = num4 / num2 * 1f;
			hsbcolor.b = num2;
			return hsbcolor;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00005E34 File Offset: 0x00004034
		public static Color ToColor(HSBColor hsbColor)
		{
			float num = hsbColor.b;
			float num2 = hsbColor.b;
			float num3 = hsbColor.b;
			if (hsbColor.s != 0f)
			{
				float num4 = hsbColor.b;
				float num5 = hsbColor.b * hsbColor.s;
				float num6 = hsbColor.b - num5;
				float num7 = hsbColor.h * 360f;
				if (num7 < 60f)
				{
					num = num4;
					num2 = num7 * num5 / 60f + num6;
					num3 = num6;
				}
				else if (num7 < 120f)
				{
					num = -(num7 - 120f) * num5 / 60f + num6;
					num2 = num4;
					num3 = num6;
				}
				else if (num7 < 180f)
				{
					num = num6;
					num2 = num4;
					num3 = (num7 - 120f) * num5 / 60f + num6;
				}
				else if (num7 < 240f)
				{
					num = num6;
					num2 = -(num7 - 240f) * num5 / 60f + num6;
					num3 = num4;
				}
				else if (num7 < 300f)
				{
					num = (num7 - 240f) * num5 / 60f + num6;
					num2 = num6;
					num3 = num4;
				}
				else if (num7 <= 360f)
				{
					num = num4;
					num2 = num6;
					num3 = -(num7 - 360f) * num5 / 60f + num6;
				}
				else
				{
					num = 0f;
					num2 = 0f;
					num3 = 0f;
				}
			}
			return new Color(Mathf.Clamp01(num), Mathf.Clamp01(num2), Mathf.Clamp01(num3), hsbColor.a);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00005FA8 File Offset: 0x000041A8
		public Color ToColor()
		{
			return HSBColor.ToColor(this);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00005FB8 File Offset: 0x000041B8
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"H:",
				this.h.ToString(),
				" S:",
				this.s.ToString(),
				" B:",
				this.b.ToString()
			});
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00006014 File Offset: 0x00004214
		public static HSBColor Lerp(HSBColor a, HSBColor b, float t)
		{
			float num;
			float num2;
			if (a.b == 0f)
			{
				num = b.h;
				num2 = b.s;
			}
			else if (b.b == 0f)
			{
				num = a.h;
				num2 = a.s;
			}
			else
			{
				if (a.s == 0f)
				{
					num = b.h;
				}
				else if (b.s == 0f)
				{
					num = a.h;
				}
				else
				{
					float num3;
					for (num3 = Mathf.LerpAngle(a.h * 360f, b.h * 360f, t); num3 < 0f; num3 += 360f)
					{
					}
					while (num3 > 360f)
					{
						num3 -= 360f;
					}
					num = num3 / 360f;
				}
				num2 = Mathf.Lerp(a.s, b.s, t);
			}
			return new HSBColor(num, num2, Mathf.Lerp(a.b, b.b, t), Mathf.Lerp(a.a, b.a, t));
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00006118 File Offset: 0x00004318
		public static void Test()
		{
			HSBColor hsbcolor = new HSBColor(Color.red);
			string text = "red: ";
			HSBColor hsbcolor2 = hsbcolor;
			Debug.Log(text + hsbcolor2.ToString());
			hsbcolor = new HSBColor(Color.green);
			string text2 = "green: ";
			hsbcolor2 = hsbcolor;
			Debug.Log(text2 + hsbcolor2.ToString());
			hsbcolor = new HSBColor(Color.blue);
			string text3 = "blue: ";
			hsbcolor2 = hsbcolor;
			Debug.Log(text3 + hsbcolor2.ToString());
			hsbcolor = new HSBColor(Color.grey);
			string text4 = "grey: ";
			hsbcolor2 = hsbcolor;
			Debug.Log(text4 + hsbcolor2.ToString());
			hsbcolor = new HSBColor(Color.white);
			string text5 = "white: ";
			hsbcolor2 = hsbcolor;
			Debug.Log(text5 + hsbcolor2.ToString());
			hsbcolor = new HSBColor(new Color(0.4f, 1f, 0.84f, 1f));
			string text6 = "0.4, 1f, 0.84: ";
			hsbcolor2 = hsbcolor;
			Debug.Log(text6 + hsbcolor2.ToString());
			Debug.Log("164,82,84   .... 0.643137f, 0.321568f, 0.329411f  :" + HSBColor.ToColor(new HSBColor(new Color(0.643137f, 0.321568f, 0.329411f))).ToString());
		}

		// Token: 0x0400006B RID: 107
		public float h;

		// Token: 0x0400006C RID: 108
		public float s;

		// Token: 0x0400006D RID: 109
		public float b;

		// Token: 0x0400006E RID: 110
		public float a;
	}
}
