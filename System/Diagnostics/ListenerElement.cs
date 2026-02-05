using System;
using System.Collections;
using System.Configuration;
using System.Xml;

namespace System.Diagnostics
{
	// Token: 0x0200049E RID: 1182
	internal class ListenerElement : TypedElement
	{
		// Token: 0x06002BCD RID: 11213 RVA: 0x000C60D4 File Offset: 0x000C42D4
		public ListenerElement(bool allowReferences)
			: base(typeof(TraceListener))
		{
			this._allowReferences = allowReferences;
			ConfigurationPropertyOptions configurationPropertyOptions = ConfigurationPropertyOptions.None;
			if (!this._allowReferences)
			{
				configurationPropertyOptions |= ConfigurationPropertyOptions.IsRequired;
			}
			this._propListenerTypeName = new ConfigurationProperty("type", typeof(string), null, configurationPropertyOptions);
			this._properties.Remove("type");
			this._properties.Add(this._propListenerTypeName);
			this._properties.Add(ListenerElement._propFilter);
			this._properties.Add(ListenerElement._propName);
			this._properties.Add(ListenerElement._propOutputOpts);
		}

		// Token: 0x17000A94 RID: 2708
		// (get) Token: 0x06002BCE RID: 11214 RVA: 0x000C6174 File Offset: 0x000C4374
		public Hashtable Attributes
		{
			get
			{
				if (this._attributes == null)
				{
					this._attributes = new Hashtable(StringComparer.OrdinalIgnoreCase);
				}
				return this._attributes;
			}
		}

		// Token: 0x17000A95 RID: 2709
		// (get) Token: 0x06002BCF RID: 11215 RVA: 0x000C6194 File Offset: 0x000C4394
		[ConfigurationProperty("filter")]
		public FilterElement Filter
		{
			get
			{
				return (FilterElement)base[ListenerElement._propFilter];
			}
		}

		// Token: 0x17000A96 RID: 2710
		// (get) Token: 0x06002BD0 RID: 11216 RVA: 0x000C61A6 File Offset: 0x000C43A6
		// (set) Token: 0x06002BD1 RID: 11217 RVA: 0x000C61B8 File Offset: 0x000C43B8
		[ConfigurationProperty("name", IsRequired = true, IsKey = true)]
		public string Name
		{
			get
			{
				return (string)base[ListenerElement._propName];
			}
			set
			{
				base[ListenerElement._propName] = value;
			}
		}

		// Token: 0x17000A97 RID: 2711
		// (get) Token: 0x06002BD2 RID: 11218 RVA: 0x000C61C6 File Offset: 0x000C43C6
		// (set) Token: 0x06002BD3 RID: 11219 RVA: 0x000C61D8 File Offset: 0x000C43D8
		[ConfigurationProperty("traceOutputOptions", DefaultValue = TraceOptions.None)]
		public TraceOptions TraceOutputOptions
		{
			get
			{
				return (TraceOptions)base[ListenerElement._propOutputOpts];
			}
			set
			{
				base[ListenerElement._propOutputOpts] = value;
			}
		}

		// Token: 0x17000A98 RID: 2712
		// (get) Token: 0x06002BD4 RID: 11220 RVA: 0x000C61EB File Offset: 0x000C43EB
		// (set) Token: 0x06002BD5 RID: 11221 RVA: 0x000C61FE File Offset: 0x000C43FE
		[ConfigurationProperty("type")]
		public override string TypeName
		{
			get
			{
				return (string)base[this._propListenerTypeName];
			}
			set
			{
				base[this._propListenerTypeName] = value;
			}
		}

		// Token: 0x06002BD6 RID: 11222 RVA: 0x000C6210 File Offset: 0x000C4410
		public override bool Equals(object compareTo)
		{
			if (this.Name.Equals("Default") && this.TypeName.Equals(typeof(DefaultTraceListener).FullName))
			{
				ListenerElement listenerElement = compareTo as ListenerElement;
				return listenerElement != null && listenerElement.Name.Equals("Default") && listenerElement.TypeName.Equals(typeof(DefaultTraceListener).FullName);
			}
			return base.Equals(compareTo);
		}

		// Token: 0x06002BD7 RID: 11223 RVA: 0x000C628B File Offset: 0x000C448B
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x06002BD8 RID: 11224 RVA: 0x000C6294 File Offset: 0x000C4494
		public TraceListener GetRuntimeObject()
		{
			if (this._runtimeObject != null)
			{
				return (TraceListener)this._runtimeObject;
			}
			TraceListener traceListener;
			try
			{
				string typeName = this.TypeName;
				if (string.IsNullOrEmpty(typeName))
				{
					if (this._attributes != null || base.ElementInformation.Properties[ListenerElement._propFilter.Name].ValueOrigin == PropertyValueOrigin.SetHere || this.TraceOutputOptions != TraceOptions.None || !string.IsNullOrEmpty(base.InitData))
					{
						throw new ConfigurationErrorsException(SR.GetString("Reference_listener_cant_have_properties", new object[] { this.Name }));
					}
					if (DiagnosticsConfiguration.SharedListeners == null)
					{
						throw new ConfigurationErrorsException(SR.GetString("Reference_to_nonexistent_listener", new object[] { this.Name }));
					}
					ListenerElement listenerElement = DiagnosticsConfiguration.SharedListeners[this.Name];
					if (listenerElement == null)
					{
						throw new ConfigurationErrorsException(SR.GetString("Reference_to_nonexistent_listener", new object[] { this.Name }));
					}
					this._runtimeObject = listenerElement.GetRuntimeObject();
					traceListener = (TraceListener)this._runtimeObject;
				}
				else
				{
					TraceListener traceListener2 = (TraceListener)base.BaseGetRuntimeObject();
					traceListener2.initializeData = base.InitData;
					traceListener2.Name = this.Name;
					traceListener2.SetAttributes(this.Attributes);
					traceListener2.TraceOutputOptions = this.TraceOutputOptions;
					if (this.Filter != null && this.Filter.TypeName != null && this.Filter.TypeName.Length != 0)
					{
						traceListener2.Filter = this.Filter.GetRuntimeObject();
						XmlWriterTraceListener xmlWriterTraceListener = traceListener2 as XmlWriterTraceListener;
						if (xmlWriterTraceListener != null)
						{
							xmlWriterTraceListener.shouldRespectFilterOnTraceTransfer = true;
						}
					}
					this._runtimeObject = traceListener2;
					traceListener = traceListener2;
				}
			}
			catch (ArgumentException ex)
			{
				throw new ConfigurationErrorsException(SR.GetString("Could_not_create_listener", new object[] { this.Name }), ex);
			}
			return traceListener;
		}

		// Token: 0x06002BD9 RID: 11225 RVA: 0x000C6470 File Offset: 0x000C4670
		protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
		{
			this.Attributes.Add(name, value);
			return true;
		}

		// Token: 0x06002BDA RID: 11226 RVA: 0x000C6480 File Offset: 0x000C4680
		protected override void PreSerialize(XmlWriter writer)
		{
			if (this._attributes != null)
			{
				IDictionaryEnumerator enumerator = this._attributes.GetEnumerator();
				while (enumerator.MoveNext())
				{
					string text = (string)enumerator.Value;
					string text2 = (string)enumerator.Key;
					if (text != null && writer != null)
					{
						writer.WriteAttributeString(text2, text);
					}
				}
			}
		}

		// Token: 0x06002BDB RID: 11227 RVA: 0x000C64D4 File Offset: 0x000C46D4
		protected override bool SerializeElement(XmlWriter writer, bool serializeCollectionKey)
		{
			return base.SerializeElement(writer, serializeCollectionKey) || (this._attributes != null && this._attributes.Count > 0);
		}

		// Token: 0x06002BDC RID: 11228 RVA: 0x000C650C File Offset: 0x000C470C
		protected override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
		{
			base.Unmerge(sourceElement, parentElement, saveMode);
			ListenerElement listenerElement = sourceElement as ListenerElement;
			if (listenerElement != null && listenerElement._attributes != null)
			{
				this._attributes = listenerElement._attributes;
			}
		}

		// Token: 0x06002BDD RID: 11229 RVA: 0x000C6540 File Offset: 0x000C4740
		internal void ResetProperties()
		{
			if (this._attributes != null)
			{
				this._attributes.Clear();
				this._properties.Clear();
				this._properties.Add(this._propListenerTypeName);
				this._properties.Add(ListenerElement._propFilter);
				this._properties.Add(ListenerElement._propName);
				this._properties.Add(ListenerElement._propOutputOpts);
			}
		}

		// Token: 0x06002BDE RID: 11230 RVA: 0x000C65AC File Offset: 0x000C47AC
		internal TraceListener RefreshRuntimeObject(TraceListener listener)
		{
			this._runtimeObject = null;
			TraceListener traceListener;
			try
			{
				string typeName = this.TypeName;
				if (string.IsNullOrEmpty(typeName))
				{
					if (this._attributes != null || base.ElementInformation.Properties[ListenerElement._propFilter.Name].ValueOrigin == PropertyValueOrigin.SetHere || this.TraceOutputOptions != TraceOptions.None || !string.IsNullOrEmpty(base.InitData))
					{
						throw new ConfigurationErrorsException(SR.GetString("Reference_listener_cant_have_properties", new object[] { this.Name }));
					}
					if (DiagnosticsConfiguration.SharedListeners == null)
					{
						throw new ConfigurationErrorsException(SR.GetString("Reference_to_nonexistent_listener", new object[] { this.Name }));
					}
					ListenerElement listenerElement = DiagnosticsConfiguration.SharedListeners[this.Name];
					if (listenerElement == null)
					{
						throw new ConfigurationErrorsException(SR.GetString("Reference_to_nonexistent_listener", new object[] { this.Name }));
					}
					this._runtimeObject = listenerElement.RefreshRuntimeObject(listener);
					traceListener = (TraceListener)this._runtimeObject;
				}
				else if (Type.GetType(typeName) != listener.GetType() || base.InitData != listener.initializeData)
				{
					traceListener = this.GetRuntimeObject();
				}
				else
				{
					listener.SetAttributes(this.Attributes);
					listener.TraceOutputOptions = this.TraceOutputOptions;
					if (listener.Filter != null)
					{
						if (base.ElementInformation.Properties[ListenerElement._propFilter.Name].ValueOrigin == PropertyValueOrigin.SetHere || base.ElementInformation.Properties[ListenerElement._propFilter.Name].ValueOrigin == PropertyValueOrigin.Inherited)
						{
							listener.Filter = this.Filter.RefreshRuntimeObject(listener.Filter);
						}
						else
						{
							listener.Filter = null;
						}
					}
					this._runtimeObject = listener;
					traceListener = listener;
				}
			}
			catch (ArgumentException ex)
			{
				throw new ConfigurationErrorsException(SR.GetString("Could_not_create_listener", new object[] { this.Name }), ex);
			}
			return traceListener;
		}

		// Token: 0x0400268A RID: 9866
		private static readonly ConfigurationProperty _propFilter = new ConfigurationProperty("filter", typeof(FilterElement), null, ConfigurationPropertyOptions.None);

		// Token: 0x0400268B RID: 9867
		private static readonly ConfigurationProperty _propName = new ConfigurationProperty("name", typeof(string), null, ConfigurationPropertyOptions.IsRequired | ConfigurationPropertyOptions.IsKey);

		// Token: 0x0400268C RID: 9868
		private static readonly ConfigurationProperty _propOutputOpts = new ConfigurationProperty("traceOutputOptions", typeof(TraceOptions), TraceOptions.None, ConfigurationPropertyOptions.None);

		// Token: 0x0400268D RID: 9869
		private ConfigurationProperty _propListenerTypeName;

		// Token: 0x0400268E RID: 9870
		private bool _allowReferences;

		// Token: 0x0400268F RID: 9871
		private Hashtable _attributes;

		// Token: 0x04002690 RID: 9872
		internal bool _isAddedByDefault;
	}
}
