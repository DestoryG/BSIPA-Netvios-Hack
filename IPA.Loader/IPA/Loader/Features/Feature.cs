using System;
using System.Collections.Generic;
using System.Text;

namespace IPA.Loader.Features
{
	/// <summary>
	/// The root interface for a mod Feature.
	/// </summary>
	/// <remarks>
	/// Avoid storing any data in any subclasses. If you do, it may result in a failure to load the feature.
	/// </remarks>
	// Token: 0x02000054 RID: 84
	public abstract class Feature
	{
		/// <summary>
		/// Initializes the feature with the parameters provided in the definition.
		///
		/// Note: When no parenthesis are provided, <paramref name="parameters" /> is an empty array.
		/// </summary>
		/// <remarks>
		/// This gets called BEFORE *your* `Init` method.
		///
		/// Returning <see langword="false" /> does *not* prevent the plugin from being loaded. It simply prevents the feature from being used.
		/// </remarks>
		/// <param name="meta">the metadata of the plugin that is being prepared</param>
		/// <param name="parameters">the parameters passed to the feature definition, or null</param>
		/// <returns><see langword="true" /> if the feature is valid for the plugin, <see langword="false" /> otherwise</returns>
		// Token: 0x06000255 RID: 597
		public abstract bool Initialize(PluginMetadata meta, string[] parameters);

		/// <summary>
		/// Evaluates the Feature for use in conditional meta-Features. This should be re-calculated on every call, unless it can be proven to not change.
		///
		/// This will be called on every feature that returns <see langword="true" /> from <see cref="M:IPA.Loader.Features.Feature.Initialize(IPA.Loader.PluginMetadata,System.String[])" />
		/// </summary>
		/// <returns>the truthiness of the Feature.</returns>
		// Token: 0x06000256 RID: 598 RVA: 0x0000C887 File Offset: 0x0000AA87
		public virtual bool Evaluate()
		{
			return true;
		}

		/// <summary>
		/// The message to be logged when the feature is not valid for a plugin.
		/// This should also be set whenever either <see cref="M:IPA.Loader.Features.Feature.BeforeLoad(IPA.Loader.PluginMetadata)" /> or <see cref="M:IPA.Loader.Features.Feature.BeforeInit(IPA.Loader.PluginMetadata)" /> returns false.
		/// </summary>
		/// <value>the message to show when the feature is marked invalid</value>
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000257 RID: 599 RVA: 0x0000C88A File Offset: 0x0000AA8A
		// (set) Token: 0x06000258 RID: 600 RVA: 0x0000C892 File Offset: 0x0000AA92
		public virtual string InvalidMessage { get; protected set; }

		/// <summary>
		/// Called before a plugin is loaded. This should never throw an exception. An exception will abort the loading of the plugin with an error.
		/// </summary>
		/// <remarks>
		/// The assembly will still be loaded, but the plugin will not be constructed if this returns <see langword="false" />.
		/// Any features it defines, for example, will still be loaded.
		/// </remarks>
		/// <param name="plugin">the plugin about to be loaded</param>
		/// <returns>whether or not the plugin should be loaded</returns>
		// Token: 0x06000259 RID: 601 RVA: 0x0000C89B File Offset: 0x0000AA9B
		public virtual bool BeforeLoad(PluginMetadata plugin)
		{
			return true;
		}

		/// <summary>
		/// Called before a plugin's `Init` method is called. This will not be called if there is no `Init` method. This should never throw an exception. An exception will abort the loading of the plugin with an error.
		/// </summary>
		/// <param name="plugin">the plugin to be initialized</param>
		/// <returns>whether or not to call the Init method</returns>
		// Token: 0x0600025A RID: 602 RVA: 0x0000C89E File Offset: 0x0000AA9E
		public virtual bool BeforeInit(PluginMetadata plugin)
		{
			return true;
		}

		/// <summary>
		/// Called after a plugin has been fully initialized, whether or not there is an `Init` method. This should never throw an exception.
		/// </summary>
		/// <param name="plugin">the plugin that was just initialized</param>
		/// <param name="pluginInstance">the instance of the plugin being initialized</param>
		// Token: 0x0600025B RID: 603 RVA: 0x0000C8A1 File Offset: 0x0000AAA1
		public virtual void AfterInit(PluginMetadata plugin, object pluginInstance)
		{
			this.AfterInit(plugin);
		}

		/// <summary>
		/// Called after a plugin has been fully initialized, whether or not there is an `Init` method. This should never throw an exception.
		/// </summary>
		/// <param name="plugin">the plugin that was just initialized</param>
		// Token: 0x0600025C RID: 604 RVA: 0x0000C8AA File Offset: 0x0000AAAA
		public virtual void AfterInit(PluginMetadata plugin)
		{
		}

		/// <summary>
		/// Ensures a plugin's assembly is loaded. Do not use unless you need to.
		/// </summary>
		/// <param name="plugin">the plugin to ensure is loaded.</param>
		// Token: 0x0600025D RID: 605 RVA: 0x0000C8AC File Offset: 0x0000AAAC
		protected void RequireLoaded(PluginMetadata plugin)
		{
			PluginLoader.Load(plugin);
		}

		/// <summary>
		/// Defines whether or not this feature will be accessible from the plugin metadata once loaded.
		/// </summary>
		/// <value><see langword="true" /> if this <see cref="T:IPA.Loader.Features.Feature" /> will be stored on the plugin metadata, <see langword="false" /> otherwise</value>
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600025E RID: 606 RVA: 0x0000C8B4 File Offset: 0x0000AAB4
		protected internal virtual bool StoreOnPlugin
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000C8B7 File Offset: 0x0000AAB7
		static Feature()
		{
			Feature.Reset();
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000C8BE File Offset: 0x0000AABE
		internal static void Reset()
		{
			Feature.featureTypes = new Dictionary<string, Type> { 
			{
				"define-feature",
				typeof(DefineFeature)
			} };
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000C8DF File Offset: 0x0000AADF
		internal static bool HasFeature(string name)
		{
			return Feature.featureTypes.ContainsKey(name);
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000C8EC File Offset: 0x0000AAEC
		internal static bool RegisterFeature(string name, Type type)
		{
			if (!typeof(Feature).IsAssignableFrom(type))
			{
				throw new ArgumentException("Feature type not subclass of Feature", "type");
			}
			if (Feature.featureTypes.ContainsKey(name))
			{
				return false;
			}
			Feature.featureTypes.Add(name, type);
			return true;
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000C92C File Offset: 0x0000AB2C
		internal static bool TryParseFeature(string featureString, PluginMetadata plugin, out Feature feature, out Exception failException, out bool featureValid, out Feature.FeatureParse parsed, Feature.FeatureParse? preParsed = null)
		{
			failException = null;
			feature = null;
			featureValid = false;
			if (preParsed == null)
			{
				StringBuilder builder = new StringBuilder();
				string name = null;
				List<string> parameters = new List<string>();
				bool escape = false;
				int parens = 0;
				bool removeWhitespace = true;
				foreach (char chr in featureString)
				{
					if (escape)
					{
						builder.Append(chr);
						escape = false;
					}
					else
					{
						switch (chr)
						{
						case '(':
							parens++;
							if (parens == 1)
							{
								removeWhitespace = true;
								name = builder.ToString();
								builder.Clear();
								goto IL_00E3;
							}
							goto IL_00C6;
						case ')':
							parens--;
							if (parens != 0)
							{
								goto IL_00C6;
							}
							break;
						case '*':
						case '+':
							goto IL_00C6;
						case ',':
							break;
						default:
							if (chr == '\\')
							{
								escape = true;
								goto IL_00E3;
							}
							goto IL_00C6;
						}
						if (parens <= 1)
						{
							parameters.Add(builder.ToString());
							builder.Clear();
							removeWhitespace = true;
							goto IL_00E3;
						}
						IL_00C6:
						if (removeWhitespace && !char.IsWhiteSpace(chr))
						{
							removeWhitespace = false;
						}
						if (!removeWhitespace)
						{
							builder.Append(chr);
						}
					}
					IL_00E3:;
				}
				if (name == null)
				{
					name = builder.ToString();
				}
				parsed = new Feature.FeatureParse(name, parameters.ToArray());
				if (parens != 0)
				{
					failException = new Exception("Malformed feature definition");
					return false;
				}
			}
			else
			{
				parsed = preParsed.Value;
			}
			Type featureType;
			if (!Feature.featureTypes.TryGetValue(parsed.Name, out featureType))
			{
				return false;
			}
			bool flag;
			try
			{
				Feature aFeature = Activator.CreateInstance(featureType) as Feature;
				if (aFeature == null)
				{
					failException = new InvalidCastException("Feature type not a subtype of Feature");
					flag = false;
				}
				else
				{
					featureValid = aFeature.Initialize(plugin, parsed.Parameters);
					feature = aFeature;
					flag = true;
				}
			}
			catch (Exception e)
			{
				failException = e;
				flag = false;
			}
			return flag;
		}

		// Token: 0x040000EA RID: 234
		private static Dictionary<string, Type> featureTypes;

		// Token: 0x02000118 RID: 280
		internal struct FeatureParse
		{
			// Token: 0x06000597 RID: 1431 RVA: 0x000173E7 File Offset: 0x000155E7
			public FeatureParse(string name, string[] parameters)
			{
				this.Name = name;
				this.Parameters = parameters;
			}

			// Token: 0x040003BE RID: 958
			public readonly string Name;

			// Token: 0x040003BF RID: 959
			public readonly string[] Parameters;
		}
	}
}
