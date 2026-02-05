using System;
using IPA.Logging;
using UnityEngine;

namespace NetviosHelperPlugin
{
	// Token: 0x02000004 RID: 4
	public class NetviosHelperPluginController : MonoBehaviour
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000013 RID: 19 RVA: 0x0000263A File Offset: 0x0000083A
		// (set) Token: 0x06000014 RID: 20 RVA: 0x00002641 File Offset: 0x00000841
		public static NetviosHelperPluginController instance { get; private set; }

		// Token: 0x06000015 RID: 21 RVA: 0x0000264C File Offset: 0x0000084C
		private void Awake()
		{
			bool flag = NetviosHelperPluginController.instance != null;
			if (flag)
			{
				Logger log = Logger.log;
				if (log != null)
				{
					log.Warn("Instance of " + base.GetType().Name + " already exists, destroying.");
				}
				Object.DestroyImmediate(this);
			}
			else
			{
				Object.DontDestroyOnLoad(this);
				NetviosHelperPluginController.instance = this;
				Logger log2 = Logger.log;
				if (log2 != null)
				{
					log2.Debug(base.name + ": Awake()");
				}
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000026CD File Offset: 0x000008CD
		private void Start()
		{
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000026CD File Offset: 0x000008CD
		private void Update()
		{
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000026CD File Offset: 0x000008CD
		private void LateUpdate()
		{
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000026CD File Offset: 0x000008CD
		private void OnEnable()
		{
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000026CD File Offset: 0x000008CD
		private void OnDisable()
		{
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000026D0 File Offset: 0x000008D0
		private void OnDestroy()
		{
			Logger log = Logger.log;
			if (log != null)
			{
				log.Debug(base.name + ": OnDestroy()");
			}
			NetviosHelperPluginController.instance = null;
		}
	}
}
