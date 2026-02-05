using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Permissions;

namespace System.Diagnostics
{
	// Token: 0x020004B7 RID: 1207
	public class TraceSource
	{
		// Token: 0x06002D03 RID: 11523 RVA: 0x000CA085 File Offset: 0x000C8285
		public TraceSource(string name)
			: this(name, SourceLevels.Off)
		{
		}

		// Token: 0x06002D04 RID: 11524 RVA: 0x000CA090 File Offset: 0x000C8290
		public TraceSource(string name, SourceLevels defaultLevel)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException("name");
			}
			this.sourceName = name;
			this.switchLevel = defaultLevel;
			List<WeakReference> list = TraceSource.tracesources;
			lock (list)
			{
				TraceSource._pruneCachedTraceSources();
				TraceSource.tracesources.Add(new WeakReference(this));
			}
		}

		// Token: 0x06002D05 RID: 11525 RVA: 0x000CA118 File Offset: 0x000C8318
		private static void _pruneCachedTraceSources()
		{
			List<WeakReference> list = TraceSource.tracesources;
			lock (list)
			{
				if (TraceSource.s_LastCollectionCount != GC.CollectionCount(2))
				{
					List<WeakReference> list2 = new List<WeakReference>(TraceSource.tracesources.Count);
					for (int i = 0; i < TraceSource.tracesources.Count; i++)
					{
						TraceSource traceSource = (TraceSource)TraceSource.tracesources[i].Target;
						if (traceSource != null)
						{
							list2.Add(TraceSource.tracesources[i]);
						}
					}
					if (list2.Count < TraceSource.tracesources.Count)
					{
						TraceSource.tracesources.Clear();
						TraceSource.tracesources.AddRange(list2);
						TraceSource.tracesources.TrimExcess();
					}
					TraceSource.s_LastCollectionCount = GC.CollectionCount(2);
				}
			}
		}

		// Token: 0x06002D06 RID: 11526 RVA: 0x000CA1F0 File Offset: 0x000C83F0
		private void Initialize()
		{
			if (!this._initCalled)
			{
				lock (this)
				{
					if (!this._initCalled)
					{
						SourceElementsCollection sources = DiagnosticsConfiguration.Sources;
						if (sources != null)
						{
							SourceElement sourceElement = sources[this.sourceName];
							if (sourceElement != null)
							{
								if (!string.IsNullOrEmpty(sourceElement.SwitchName))
								{
									this.CreateSwitch(sourceElement.SwitchType, sourceElement.SwitchName);
								}
								else
								{
									this.CreateSwitch(sourceElement.SwitchType, this.sourceName);
									if (!string.IsNullOrEmpty(sourceElement.SwitchValue))
									{
										this.internalSwitch.Level = (SourceLevels)Enum.Parse(typeof(SourceLevels), sourceElement.SwitchValue);
									}
								}
								this.listeners = sourceElement.Listeners.GetRuntimeObject();
								this.attributes = new StringDictionary();
								TraceUtils.VerifyAttributes(sourceElement.Attributes, this.GetSupportedAttributes(), this);
								this.attributes.ReplaceHashtable(sourceElement.Attributes);
							}
							else
							{
								this.NoConfigInit();
							}
						}
						else
						{
							this.NoConfigInit();
						}
						this._initCalled = true;
					}
				}
			}
		}

		// Token: 0x06002D07 RID: 11527 RVA: 0x000CA328 File Offset: 0x000C8528
		private void NoConfigInit()
		{
			this.internalSwitch = new SourceSwitch(this.sourceName, this.switchLevel.ToString());
			this.listeners = new TraceListenerCollection();
			this.listeners.Add(new DefaultTraceListener());
			this.attributes = null;
		}

		// Token: 0x06002D08 RID: 11528 RVA: 0x000CA384 File Offset: 0x000C8584
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public void Close()
		{
			if (this.listeners != null)
			{
				object critSec = TraceInternal.critSec;
				lock (critSec)
				{
					foreach (object obj in this.listeners)
					{
						TraceListener traceListener = (TraceListener)obj;
						traceListener.Close();
					}
				}
			}
		}

		// Token: 0x06002D09 RID: 11529 RVA: 0x000CA414 File Offset: 0x000C8614
		public void Flush()
		{
			if (this.listeners != null)
			{
				if (TraceInternal.UseGlobalLock)
				{
					object critSec = TraceInternal.critSec;
					lock (critSec)
					{
						using (IEnumerator enumerator = this.listeners.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								object obj = enumerator.Current;
								TraceListener traceListener = (TraceListener)obj;
								traceListener.Flush();
							}
							return;
						}
					}
				}
				foreach (object obj2 in this.listeners)
				{
					TraceListener traceListener2 = (TraceListener)obj2;
					if (!traceListener2.IsThreadSafe)
					{
						TraceListener traceListener3 = traceListener2;
						lock (traceListener3)
						{
							traceListener2.Flush();
							continue;
						}
					}
					traceListener2.Flush();
				}
			}
		}

		// Token: 0x06002D0A RID: 11530 RVA: 0x000CA53C File Offset: 0x000C873C
		protected internal virtual string[] GetSupportedAttributes()
		{
			return null;
		}

		// Token: 0x06002D0B RID: 11531 RVA: 0x000CA540 File Offset: 0x000C8740
		internal static void RefreshAll()
		{
			List<WeakReference> list = TraceSource.tracesources;
			lock (list)
			{
				TraceSource._pruneCachedTraceSources();
				for (int i = 0; i < TraceSource.tracesources.Count; i++)
				{
					TraceSource traceSource = (TraceSource)TraceSource.tracesources[i].Target;
					if (traceSource != null)
					{
						traceSource.Refresh();
					}
				}
			}
		}

		// Token: 0x06002D0C RID: 11532 RVA: 0x000CA5B4 File Offset: 0x000C87B4
		internal void Refresh()
		{
			if (!this._initCalled)
			{
				this.Initialize();
				return;
			}
			SourceElementsCollection sources = DiagnosticsConfiguration.Sources;
			if (sources != null)
			{
				SourceElement sourceElement = sources[this.Name];
				if (sourceElement != null)
				{
					if ((string.IsNullOrEmpty(sourceElement.SwitchType) && this.internalSwitch.GetType() != typeof(SourceSwitch)) || sourceElement.SwitchType != this.internalSwitch.GetType().AssemblyQualifiedName)
					{
						if (!string.IsNullOrEmpty(sourceElement.SwitchName))
						{
							this.CreateSwitch(sourceElement.SwitchType, sourceElement.SwitchName);
						}
						else
						{
							this.CreateSwitch(sourceElement.SwitchType, this.Name);
							if (!string.IsNullOrEmpty(sourceElement.SwitchValue))
							{
								this.internalSwitch.Level = (SourceLevels)Enum.Parse(typeof(SourceLevels), sourceElement.SwitchValue);
							}
						}
					}
					else if (!string.IsNullOrEmpty(sourceElement.SwitchName))
					{
						if (sourceElement.SwitchName != this.internalSwitch.DisplayName)
						{
							this.CreateSwitch(sourceElement.SwitchType, sourceElement.SwitchName);
						}
						else
						{
							this.internalSwitch.Refresh();
						}
					}
					else if (!string.IsNullOrEmpty(sourceElement.SwitchValue))
					{
						this.internalSwitch.Level = (SourceLevels)Enum.Parse(typeof(SourceLevels), sourceElement.SwitchValue);
					}
					else
					{
						this.internalSwitch.Level = SourceLevels.Off;
					}
					TraceListenerCollection traceListenerCollection = new TraceListenerCollection();
					foreach (object obj in sourceElement.Listeners)
					{
						ListenerElement listenerElement = (ListenerElement)obj;
						TraceListener traceListener = this.listeners[listenerElement.Name];
						if (traceListener != null)
						{
							traceListenerCollection.Add(listenerElement.RefreshRuntimeObject(traceListener));
						}
						else
						{
							traceListenerCollection.Add(listenerElement.GetRuntimeObject());
						}
					}
					TraceUtils.VerifyAttributes(sourceElement.Attributes, this.GetSupportedAttributes(), this);
					this.attributes = new StringDictionary();
					this.attributes.ReplaceHashtable(sourceElement.Attributes);
					this.listeners = traceListenerCollection;
					return;
				}
				this.internalSwitch.Level = this.switchLevel;
				this.listeners.Clear();
				this.attributes = null;
			}
		}

		// Token: 0x06002D0D RID: 11533 RVA: 0x000CA824 File Offset: 0x000C8A24
		[Conditional("TRACE")]
		public void TraceEvent(TraceEventType eventType, int id)
		{
			this.Initialize();
			TraceEventCache traceEventCache = new TraceEventCache();
			if (this.internalSwitch.ShouldTrace(eventType) && this.listeners != null)
			{
				if (TraceInternal.UseGlobalLock)
				{
					object critSec = TraceInternal.critSec;
					lock (critSec)
					{
						for (int i = 0; i < this.listeners.Count; i++)
						{
							TraceListener traceListener = this.listeners[i];
							traceListener.TraceEvent(traceEventCache, this.Name, eventType, id);
							if (Trace.AutoFlush)
							{
								traceListener.Flush();
							}
						}
						return;
					}
				}
				int j = 0;
				while (j < this.listeners.Count)
				{
					TraceListener traceListener2 = this.listeners[j];
					if (!traceListener2.IsThreadSafe)
					{
						TraceListener traceListener3 = traceListener2;
						lock (traceListener3)
						{
							traceListener2.TraceEvent(traceEventCache, this.Name, eventType, id);
							if (Trace.AutoFlush)
							{
								traceListener2.Flush();
							}
							goto IL_0111;
						}
						goto IL_00F3;
					}
					goto IL_00F3;
					IL_0111:
					j++;
					continue;
					IL_00F3:
					traceListener2.TraceEvent(traceEventCache, this.Name, eventType, id);
					if (Trace.AutoFlush)
					{
						traceListener2.Flush();
						goto IL_0111;
					}
					goto IL_0111;
				}
			}
		}

		// Token: 0x06002D0E RID: 11534 RVA: 0x000CA978 File Offset: 0x000C8B78
		[Conditional("TRACE")]
		public void TraceEvent(TraceEventType eventType, int id, string message)
		{
			this.Initialize();
			TraceEventCache traceEventCache = new TraceEventCache();
			if (this.internalSwitch.ShouldTrace(eventType) && this.listeners != null)
			{
				if (TraceInternal.UseGlobalLock)
				{
					object critSec = TraceInternal.critSec;
					lock (critSec)
					{
						for (int i = 0; i < this.listeners.Count; i++)
						{
							TraceListener traceListener = this.listeners[i];
							traceListener.TraceEvent(traceEventCache, this.Name, eventType, id, message);
							if (Trace.AutoFlush)
							{
								traceListener.Flush();
							}
						}
						return;
					}
				}
				int j = 0;
				while (j < this.listeners.Count)
				{
					TraceListener traceListener2 = this.listeners[j];
					if (!traceListener2.IsThreadSafe)
					{
						TraceListener traceListener3 = traceListener2;
						lock (traceListener3)
						{
							traceListener2.TraceEvent(traceEventCache, this.Name, eventType, id, message);
							if (Trace.AutoFlush)
							{
								traceListener2.Flush();
							}
							goto IL_0114;
						}
						goto IL_00F5;
					}
					goto IL_00F5;
					IL_0114:
					j++;
					continue;
					IL_00F5:
					traceListener2.TraceEvent(traceEventCache, this.Name, eventType, id, message);
					if (Trace.AutoFlush)
					{
						traceListener2.Flush();
						goto IL_0114;
					}
					goto IL_0114;
				}
			}
		}

		// Token: 0x06002D0F RID: 11535 RVA: 0x000CAAD0 File Offset: 0x000C8CD0
		[Conditional("TRACE")]
		public void TraceEvent(TraceEventType eventType, int id, string format, params object[] args)
		{
			this.Initialize();
			TraceEventCache traceEventCache = new TraceEventCache();
			if (this.internalSwitch.ShouldTrace(eventType) && this.listeners != null)
			{
				if (TraceInternal.UseGlobalLock)
				{
					object critSec = TraceInternal.critSec;
					lock (critSec)
					{
						for (int i = 0; i < this.listeners.Count; i++)
						{
							TraceListener traceListener = this.listeners[i];
							traceListener.TraceEvent(traceEventCache, this.Name, eventType, id, format, args);
							if (Trace.AutoFlush)
							{
								traceListener.Flush();
							}
						}
						return;
					}
				}
				int j = 0;
				while (j < this.listeners.Count)
				{
					TraceListener traceListener2 = this.listeners[j];
					if (!traceListener2.IsThreadSafe)
					{
						TraceListener traceListener3 = traceListener2;
						lock (traceListener3)
						{
							traceListener2.TraceEvent(traceEventCache, this.Name, eventType, id, format, args);
							if (Trace.AutoFlush)
							{
								traceListener2.Flush();
							}
							goto IL_011D;
						}
						goto IL_00FC;
					}
					goto IL_00FC;
					IL_011D:
					j++;
					continue;
					IL_00FC:
					traceListener2.TraceEvent(traceEventCache, this.Name, eventType, id, format, args);
					if (Trace.AutoFlush)
					{
						traceListener2.Flush();
						goto IL_011D;
					}
					goto IL_011D;
				}
			}
		}

		// Token: 0x06002D10 RID: 11536 RVA: 0x000CAC30 File Offset: 0x000C8E30
		[Conditional("TRACE")]
		public void TraceData(TraceEventType eventType, int id, object data)
		{
			this.Initialize();
			TraceEventCache traceEventCache = new TraceEventCache();
			if (this.internalSwitch.ShouldTrace(eventType) && this.listeners != null)
			{
				if (TraceInternal.UseGlobalLock)
				{
					object critSec = TraceInternal.critSec;
					lock (critSec)
					{
						for (int i = 0; i < this.listeners.Count; i++)
						{
							TraceListener traceListener = this.listeners[i];
							traceListener.TraceData(traceEventCache, this.Name, eventType, id, data);
							if (Trace.AutoFlush)
							{
								traceListener.Flush();
							}
						}
						return;
					}
				}
				int j = 0;
				while (j < this.listeners.Count)
				{
					TraceListener traceListener2 = this.listeners[j];
					if (!traceListener2.IsThreadSafe)
					{
						TraceListener traceListener3 = traceListener2;
						lock (traceListener3)
						{
							traceListener2.TraceData(traceEventCache, this.Name, eventType, id, data);
							if (Trace.AutoFlush)
							{
								traceListener2.Flush();
							}
							goto IL_0114;
						}
						goto IL_00F5;
					}
					goto IL_00F5;
					IL_0114:
					j++;
					continue;
					IL_00F5:
					traceListener2.TraceData(traceEventCache, this.Name, eventType, id, data);
					if (Trace.AutoFlush)
					{
						traceListener2.Flush();
						goto IL_0114;
					}
					goto IL_0114;
				}
			}
		}

		// Token: 0x06002D11 RID: 11537 RVA: 0x000CAD88 File Offset: 0x000C8F88
		[Conditional("TRACE")]
		public void TraceData(TraceEventType eventType, int id, params object[] data)
		{
			this.Initialize();
			TraceEventCache traceEventCache = new TraceEventCache();
			if (this.internalSwitch.ShouldTrace(eventType) && this.listeners != null)
			{
				if (TraceInternal.UseGlobalLock)
				{
					object critSec = TraceInternal.critSec;
					lock (critSec)
					{
						for (int i = 0; i < this.listeners.Count; i++)
						{
							TraceListener traceListener = this.listeners[i];
							traceListener.TraceData(traceEventCache, this.Name, eventType, id, data);
							if (Trace.AutoFlush)
							{
								traceListener.Flush();
							}
						}
						return;
					}
				}
				int j = 0;
				while (j < this.listeners.Count)
				{
					TraceListener traceListener2 = this.listeners[j];
					if (!traceListener2.IsThreadSafe)
					{
						TraceListener traceListener3 = traceListener2;
						lock (traceListener3)
						{
							traceListener2.TraceData(traceEventCache, this.Name, eventType, id, data);
							if (Trace.AutoFlush)
							{
								traceListener2.Flush();
							}
							goto IL_0114;
						}
						goto IL_00F5;
					}
					goto IL_00F5;
					IL_0114:
					j++;
					continue;
					IL_00F5:
					traceListener2.TraceData(traceEventCache, this.Name, eventType, id, data);
					if (Trace.AutoFlush)
					{
						traceListener2.Flush();
						goto IL_0114;
					}
					goto IL_0114;
				}
			}
		}

		// Token: 0x06002D12 RID: 11538 RVA: 0x000CAEE0 File Offset: 0x000C90E0
		[Conditional("TRACE")]
		public void TraceInformation(string message)
		{
			this.TraceEvent(TraceEventType.Information, 0, message, null);
		}

		// Token: 0x06002D13 RID: 11539 RVA: 0x000CAEEC File Offset: 0x000C90EC
		[Conditional("TRACE")]
		public void TraceInformation(string format, params object[] args)
		{
			this.TraceEvent(TraceEventType.Information, 0, format, args);
		}

		// Token: 0x06002D14 RID: 11540 RVA: 0x000CAEF8 File Offset: 0x000C90F8
		[Conditional("TRACE")]
		public void TraceTransfer(int id, string message, Guid relatedActivityId)
		{
			this.Initialize();
			TraceEventCache traceEventCache = new TraceEventCache();
			if (this.internalSwitch.ShouldTrace(TraceEventType.Transfer) && this.listeners != null)
			{
				if (TraceInternal.UseGlobalLock)
				{
					object critSec = TraceInternal.critSec;
					lock (critSec)
					{
						for (int i = 0; i < this.listeners.Count; i++)
						{
							TraceListener traceListener = this.listeners[i];
							traceListener.TraceTransfer(traceEventCache, this.Name, id, message, relatedActivityId);
							if (Trace.AutoFlush)
							{
								traceListener.Flush();
							}
						}
						return;
					}
				}
				int j = 0;
				while (j < this.listeners.Count)
				{
					TraceListener traceListener2 = this.listeners[j];
					if (!traceListener2.IsThreadSafe)
					{
						TraceListener traceListener3 = traceListener2;
						lock (traceListener3)
						{
							traceListener2.TraceTransfer(traceEventCache, this.Name, id, message, relatedActivityId);
							if (Trace.AutoFlush)
							{
								traceListener2.Flush();
							}
							goto IL_0118;
						}
						goto IL_00F9;
					}
					goto IL_00F9;
					IL_0118:
					j++;
					continue;
					IL_00F9:
					traceListener2.TraceTransfer(traceEventCache, this.Name, id, message, relatedActivityId);
					if (Trace.AutoFlush)
					{
						traceListener2.Flush();
						goto IL_0118;
					}
					goto IL_0118;
				}
			}
		}

		// Token: 0x06002D15 RID: 11541 RVA: 0x000CB054 File Offset: 0x000C9254
		private void CreateSwitch(string typename, string name)
		{
			if (!string.IsNullOrEmpty(typename))
			{
				this.internalSwitch = (SourceSwitch)TraceUtils.GetRuntimeObject(typename, typeof(SourceSwitch), name);
				return;
			}
			this.internalSwitch = new SourceSwitch(name, this.switchLevel.ToString());
		}

		// Token: 0x17000AE7 RID: 2791
		// (get) Token: 0x06002D16 RID: 11542 RVA: 0x000CB0A7 File Offset: 0x000C92A7
		public StringDictionary Attributes
		{
			get
			{
				this.Initialize();
				if (this.attributes == null)
				{
					this.attributes = new StringDictionary();
				}
				return this.attributes;
			}
		}

		// Token: 0x17000AE8 RID: 2792
		// (get) Token: 0x06002D17 RID: 11543 RVA: 0x000CB0C8 File Offset: 0x000C92C8
		public string Name
		{
			get
			{
				return this.sourceName;
			}
		}

		// Token: 0x17000AE9 RID: 2793
		// (get) Token: 0x06002D18 RID: 11544 RVA: 0x000CB0D2 File Offset: 0x000C92D2
		public TraceListenerCollection Listeners
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				this.Initialize();
				return this.listeners;
			}
		}

		// Token: 0x17000AEA RID: 2794
		// (get) Token: 0x06002D19 RID: 11545 RVA: 0x000CB0E2 File Offset: 0x000C92E2
		// (set) Token: 0x06002D1A RID: 11546 RVA: 0x000CB0F2 File Offset: 0x000C92F2
		public SourceSwitch Switch
		{
			get
			{
				this.Initialize();
				return this.internalSwitch;
			}
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Switch");
				}
				this.Initialize();
				this.internalSwitch = value;
			}
		}

		// Token: 0x040026F8 RID: 9976
		private static List<WeakReference> tracesources = new List<WeakReference>();

		// Token: 0x040026F9 RID: 9977
		private static int s_LastCollectionCount;

		// Token: 0x040026FA RID: 9978
		private volatile SourceSwitch internalSwitch;

		// Token: 0x040026FB RID: 9979
		private volatile TraceListenerCollection listeners;

		// Token: 0x040026FC RID: 9980
		private StringDictionary attributes;

		// Token: 0x040026FD RID: 9981
		private SourceLevels switchLevel;

		// Token: 0x040026FE RID: 9982
		private volatile string sourceName;

		// Token: 0x040026FF RID: 9983
		internal volatile bool _initCalled;
	}
}
