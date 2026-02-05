using System;
using System.Configuration;
using System.Threading;

namespace System.Diagnostics
{
	// Token: 0x0200049A RID: 1178
	internal static class DiagnosticsConfiguration
	{
		// Token: 0x17000A85 RID: 2693
		// (get) Token: 0x06002BAB RID: 11179 RVA: 0x000C5A44 File Offset: 0x000C3C44
		internal static SwitchElementsCollection SwitchSettings
		{
			get
			{
				DiagnosticsConfiguration.Initialize();
				SystemDiagnosticsSection systemDiagnosticsSection = DiagnosticsConfiguration.configSection;
				if (systemDiagnosticsSection != null)
				{
					return systemDiagnosticsSection.Switches;
				}
				return null;
			}
		}

		// Token: 0x17000A86 RID: 2694
		// (get) Token: 0x06002BAC RID: 11180 RVA: 0x000C5A6C File Offset: 0x000C3C6C
		internal static bool AssertUIEnabled
		{
			get
			{
				DiagnosticsConfiguration.Initialize();
				SystemDiagnosticsSection systemDiagnosticsSection = DiagnosticsConfiguration.configSection;
				return systemDiagnosticsSection == null || systemDiagnosticsSection.Assert == null || systemDiagnosticsSection.Assert.AssertUIEnabled;
			}
		}

		// Token: 0x17000A87 RID: 2695
		// (get) Token: 0x06002BAD RID: 11181 RVA: 0x000C5AA0 File Offset: 0x000C3CA0
		internal static string ConfigFilePath
		{
			get
			{
				DiagnosticsConfiguration.Initialize();
				SystemDiagnosticsSection systemDiagnosticsSection = DiagnosticsConfiguration.configSection;
				if (systemDiagnosticsSection != null)
				{
					return systemDiagnosticsSection.ElementInformation.Source;
				}
				return string.Empty;
			}
		}

		// Token: 0x17000A88 RID: 2696
		// (get) Token: 0x06002BAE RID: 11182 RVA: 0x000C5AD0 File Offset: 0x000C3CD0
		internal static string LogFileName
		{
			get
			{
				DiagnosticsConfiguration.Initialize();
				SystemDiagnosticsSection systemDiagnosticsSection = DiagnosticsConfiguration.configSection;
				if (systemDiagnosticsSection != null && systemDiagnosticsSection.Assert != null)
				{
					return systemDiagnosticsSection.Assert.LogFileName;
				}
				return string.Empty;
			}
		}

		// Token: 0x17000A89 RID: 2697
		// (get) Token: 0x06002BAF RID: 11183 RVA: 0x000C5B08 File Offset: 0x000C3D08
		internal static bool AutoFlush
		{
			get
			{
				DiagnosticsConfiguration.Initialize();
				SystemDiagnosticsSection systemDiagnosticsSection = DiagnosticsConfiguration.configSection;
				return systemDiagnosticsSection != null && systemDiagnosticsSection.Trace != null && systemDiagnosticsSection.Trace.AutoFlush;
			}
		}

		// Token: 0x17000A8A RID: 2698
		// (get) Token: 0x06002BB0 RID: 11184 RVA: 0x000C5B3C File Offset: 0x000C3D3C
		internal static bool UseGlobalLock
		{
			get
			{
				DiagnosticsConfiguration.Initialize();
				SystemDiagnosticsSection systemDiagnosticsSection = DiagnosticsConfiguration.configSection;
				return systemDiagnosticsSection == null || systemDiagnosticsSection.Trace == null || systemDiagnosticsSection.Trace.UseGlobalLock;
			}
		}

		// Token: 0x17000A8B RID: 2699
		// (get) Token: 0x06002BB1 RID: 11185 RVA: 0x000C5B70 File Offset: 0x000C3D70
		internal static int IndentSize
		{
			get
			{
				DiagnosticsConfiguration.Initialize();
				SystemDiagnosticsSection systemDiagnosticsSection = DiagnosticsConfiguration.configSection;
				if (systemDiagnosticsSection != null && systemDiagnosticsSection.Trace != null)
				{
					return systemDiagnosticsSection.Trace.IndentSize;
				}
				return 4;
			}
		}

		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x06002BB2 RID: 11186 RVA: 0x000C5BA4 File Offset: 0x000C3DA4
		internal static int PerfomanceCountersFileMappingSize
		{
			get
			{
				int num = 0;
				while (!DiagnosticsConfiguration.CanInitialize() && num <= 5)
				{
					if (num == 5)
					{
						return 524288;
					}
					Thread.Sleep(200);
					num++;
				}
				DiagnosticsConfiguration.Initialize();
				SystemDiagnosticsSection systemDiagnosticsSection = DiagnosticsConfiguration.configSection;
				if (systemDiagnosticsSection != null && systemDiagnosticsSection.PerfCounters != null)
				{
					int num2 = systemDiagnosticsSection.PerfCounters.FileMappingSize;
					if (num2 < 32768)
					{
						num2 = 32768;
					}
					if (num2 > 33554432)
					{
						num2 = 33554432;
					}
					return num2;
				}
				return 524288;
			}
		}

		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x06002BB3 RID: 11187 RVA: 0x000C5C20 File Offset: 0x000C3E20
		internal static ListenerElementsCollection SharedListeners
		{
			get
			{
				DiagnosticsConfiguration.Initialize();
				SystemDiagnosticsSection systemDiagnosticsSection = DiagnosticsConfiguration.configSection;
				if (systemDiagnosticsSection != null)
				{
					return systemDiagnosticsSection.SharedListeners;
				}
				return null;
			}
		}

		// Token: 0x17000A8E RID: 2702
		// (get) Token: 0x06002BB4 RID: 11188 RVA: 0x000C5C48 File Offset: 0x000C3E48
		internal static SourceElementsCollection Sources
		{
			get
			{
				DiagnosticsConfiguration.Initialize();
				SystemDiagnosticsSection systemDiagnosticsSection = DiagnosticsConfiguration.configSection;
				if (systemDiagnosticsSection != null && systemDiagnosticsSection.Sources != null)
				{
					return systemDiagnosticsSection.Sources;
				}
				return null;
			}
		}

		// Token: 0x17000A8F RID: 2703
		// (get) Token: 0x06002BB5 RID: 11189 RVA: 0x000C5C75 File Offset: 0x000C3E75
		internal static SystemDiagnosticsSection SystemDiagnosticsSection
		{
			get
			{
				DiagnosticsConfiguration.Initialize();
				return DiagnosticsConfiguration.configSection;
			}
		}

		// Token: 0x06002BB6 RID: 11190 RVA: 0x000C5C84 File Offset: 0x000C3E84
		private static SystemDiagnosticsSection GetConfigSection()
		{
			return (SystemDiagnosticsSection)PrivilegedConfigurationManager.GetSection("system.diagnostics");
		}

		// Token: 0x06002BB7 RID: 11191 RVA: 0x000C5CA2 File Offset: 0x000C3EA2
		internal static bool IsInitializing()
		{
			return DiagnosticsConfiguration.initState == InitState.Initializing;
		}

		// Token: 0x06002BB8 RID: 11192 RVA: 0x000C5CAE File Offset: 0x000C3EAE
		internal static bool IsInitialized()
		{
			return DiagnosticsConfiguration.initState == InitState.Initialized;
		}

		// Token: 0x06002BB9 RID: 11193 RVA: 0x000C5CBA File Offset: 0x000C3EBA
		internal static bool CanInitialize()
		{
			return DiagnosticsConfiguration.initState != InitState.Initializing && !ConfigurationManagerInternalFactory.Instance.SetConfigurationSystemInProgress;
		}

		// Token: 0x06002BBA RID: 11194 RVA: 0x000C5CD8 File Offset: 0x000C3ED8
		internal static void Initialize()
		{
			object critSec = TraceInternal.critSec;
			lock (critSec)
			{
				if (DiagnosticsConfiguration.initState == InitState.NotInitialized && !ConfigurationManagerInternalFactory.Instance.SetConfigurationSystemInProgress)
				{
					DiagnosticsConfiguration.initState = InitState.Initializing;
					try
					{
						DiagnosticsConfiguration.configSection = DiagnosticsConfiguration.GetConfigSection();
					}
					finally
					{
						DiagnosticsConfiguration.initState = InitState.Initialized;
					}
				}
			}
		}

		// Token: 0x06002BBB RID: 11195 RVA: 0x000C5D54 File Offset: 0x000C3F54
		internal static void Refresh()
		{
			ConfigurationManager.RefreshSection("system.diagnostics");
			SystemDiagnosticsSection systemDiagnosticsSection = DiagnosticsConfiguration.configSection;
			if (systemDiagnosticsSection != null)
			{
				if (systemDiagnosticsSection.Switches != null)
				{
					foreach (object obj in systemDiagnosticsSection.Switches)
					{
						SwitchElement switchElement = (SwitchElement)obj;
						switchElement.ResetProperties();
					}
				}
				if (systemDiagnosticsSection.SharedListeners != null)
				{
					foreach (object obj2 in systemDiagnosticsSection.SharedListeners)
					{
						ListenerElement listenerElement = (ListenerElement)obj2;
						listenerElement.ResetProperties();
					}
				}
				if (systemDiagnosticsSection.Sources != null)
				{
					foreach (object obj3 in systemDiagnosticsSection.Sources)
					{
						SourceElement sourceElement = (SourceElement)obj3;
						sourceElement.ResetProperties();
					}
				}
			}
			DiagnosticsConfiguration.configSection = null;
			DiagnosticsConfiguration.initState = InitState.NotInitialized;
			DiagnosticsConfiguration.Initialize();
		}

		// Token: 0x04002688 RID: 9864
		private static volatile SystemDiagnosticsSection configSection;

		// Token: 0x04002689 RID: 9865
		private static volatile InitState initState;
	}
}
