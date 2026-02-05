using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using IPA.Loader.Features;
using IPA.Utilities;
using Mono.Cecil;
using SemVer;

namespace IPA.Loader
{
	/// <summary>
	/// A class which describes a loaded plugin.
	/// </summary>
	// Token: 0x02000049 RID: 73
	public class PluginMetadata
	{
		/// <summary>
		/// The assembly the plugin was loaded from.
		/// </summary>
		/// <value>the loaded Assembly that contains the plugin main type</value>
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001DE RID: 478 RVA: 0x0000A769 File Offset: 0x00008969
		// (set) Token: 0x060001DF RID: 479 RVA: 0x0000A771 File Offset: 0x00008971
		public Assembly Assembly { get; internal set; }

		/// <summary>
		/// The TypeDefinition for the main type of the plugin.
		/// </summary>
		/// <value>the Cecil definition for the plugin main type</value>
		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x0000A77A File Offset: 0x0000897A
		// (set) Token: 0x060001E1 RID: 481 RVA: 0x0000A782 File Offset: 0x00008982
		public TypeDefinition PluginType { get; internal set; }

		/// <summary>
		/// The human readable name of the plugin.
		/// </summary>
		/// <value>the name of the plugin</value>
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x0000A78B File Offset: 0x0000898B
		public string Name
		{
			get
			{
				return this.manifest.Name;
			}
		}

		/// <summary>
		/// The BeatMods ID of the plugin, or null if it doesn't have one.
		/// </summary>
		/// <value>the updater ID of the plugin</value>
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x0000A798 File Offset: 0x00008998
		public string Id
		{
			get
			{
				return this.manifest.Id;
			}
		}

		/// <summary>
		/// The name of the author that wrote this plugin.
		/// </summary>
		/// <value>the name of the plugin's author</value>
		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x0000A7A5 File Offset: 0x000089A5
		public string Author
		{
			get
			{
				return this.manifest.Author;
			}
		}

		/// <summary>
		/// The description of this plugin.
		/// </summary>
		/// <value>the description of the plugin</value>
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x0000A7B2 File Offset: 0x000089B2
		public string Description
		{
			get
			{
				return this.manifest.Description;
			}
		}

		/// <summary>
		/// The version of the plugin.
		/// </summary>
		/// <value>the version of the plugin</value>
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x0000A7BF File Offset: 0x000089BF
		public global::SemVer.Version Version
		{
			get
			{
				return this.manifest.Version;
			}
		}

		/// <summary>
		/// The file the plugin was loaded from.
		/// </summary>
		/// <value>the file the plugin was loaded from</value>
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x0000A7CC File Offset: 0x000089CC
		// (set) Token: 0x060001E8 RID: 488 RVA: 0x0000A7D4 File Offset: 0x000089D4
		public FileInfo File { get; internal set; }

		/// <summary>
		/// HashStr
		/// </summary>
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x0000A7DD File Offset: 0x000089DD
		// (set) Token: 0x060001EA RID: 490 RVA: 0x0000A7E5 File Offset: 0x000089E5
		public string HashStr { get; internal set; }

		/// <summary>
		/// The features this plugin requests.
		/// </summary>
		/// <value>the list of features requested by the plugin</value>
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001EB RID: 491 RVA: 0x0000A7EE File Offset: 0x000089EE
		public IReadOnlyList<Feature> Features
		{
			get
			{
				return this.InternalFeatures;
			}
		}

		/// <summary>
		/// A list of files (that aren't <see cref="P:IPA.Loader.PluginMetadata.File" />) that are associated with this plugin.
		/// </summary>
		/// <value>a list of associated files</value>
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060001EC RID: 492 RVA: 0x0000A7F6 File Offset: 0x000089F6
		// (set) Token: 0x060001ED RID: 493 RVA: 0x0000A7FE File Offset: 0x000089FE
		public IReadOnlyList<FileInfo> AssociatedFiles { get; private set; } = new List<FileInfo>();

		/// <summary>
		/// The name of the resource in the plugin assembly containing the plugin's icon.
		/// </summary>
		/// <value>the name of the plugin's icon</value>
		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060001EE RID: 494 RVA: 0x0000A807 File Offset: 0x00008A07
		public string IconName
		{
			get
			{
				return this.manifest.IconPath;
			}
		}

		/// <summary>
		/// A link to this plugin's home page, if any.
		/// </summary>
		/// <value>the <see cref="T:System.Uri" /> of the plugin's home page</value>
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001EF RID: 495 RVA: 0x0000A814 File Offset: 0x00008A14
		public Uri PluginHomeLink
		{
			get
			{
				PluginManifest.LinksObject links = this.manifest.Links;
				if (links == null)
				{
					return null;
				}
				return links.ProjectHome;
			}
		}

		/// <summary>
		/// A link to this plugin's source code, if avaliable.
		/// </summary>
		/// <value>the <see cref="T:System.Uri" /> of the plugin's source code</value>
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x0000A82C File Offset: 0x00008A2C
		public Uri PluginSourceLink
		{
			get
			{
				PluginManifest.LinksObject links = this.manifest.Links;
				if (links == null)
				{
					return null;
				}
				return links.ProjectSource;
			}
		}

		/// <summary>
		/// A link to a donate page for the author of this plugin, if avaliable.
		/// </summary>
		/// <value>the <see cref="T:System.Uri" /> of the author's donate page</value>
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x0000A844 File Offset: 0x00008A44
		public Uri DonateLink
		{
			get
			{
				PluginManifest.LinksObject links = this.manifest.Links;
				if (links == null)
				{
					return null;
				}
				return links.Donate;
			}
		}

		/// <summary>
		/// Whether or not this metadata object represents a bare manifest.
		/// </summary>
		/// <value><see langword="true" /> if it is bare, <see langword="false" /> otherwise</value>
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x0000A85C File Offset: 0x00008A5C
		// (set) Token: 0x060001F3 RID: 499 RVA: 0x0000A864 File Offset: 0x00008A64
		public bool IsBare { get; internal set; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x0000A86D File Offset: 0x00008A6D
		internal HashSet<PluginMetadata> Dependencies { get; } = new HashSet<PluginMetadata>();

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x0000A875 File Offset: 0x00008A75
		internal HashSet<PluginMetadata> LoadsAfter { get; } = new HashSet<PluginMetadata>();

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x0000A87D File Offset: 0x00008A7D
		// (set) Token: 0x060001F7 RID: 503 RVA: 0x0000A888 File Offset: 0x00008A88
		internal PluginManifest Manifest
		{
			get
			{
				return this.manifest;
			}
			set
			{
				this.manifest = value;
				this.AssociatedFiles = (from f in value.Files
					select Path.Combine(UnityGame.InstallPath, f) into p
					select new FileInfo(p)).ToList<FileInfo>();
			}
		}

		/// <summary>
		/// The <see cref="T:IPA.RuntimeOptions" /> that the plugin specified in its <see cref="T:IPA.PluginAttribute" />.
		/// </summary>
		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x0000A8F5 File Offset: 0x00008AF5
		// (set) Token: 0x060001F9 RID: 505 RVA: 0x0000A8FD File Offset: 0x00008AFD
		public RuntimeOptions RuntimeOptions { get; internal set; }

		/// <summary>
		/// Gets all of the metadata as a readable string.
		/// </summary>
		/// <returns>the readable printable metadata string</returns>
		// Token: 0x060001FA RID: 506 RVA: 0x0000A908 File Offset: 0x00008B08
		public override string ToString()
		{
			string text = "{0}({1}@{2})({3}) from '{4}'";
			object[] array = new object[5];
			array[0] = this.Name;
			array[1] = this.Id;
			array[2] = this.Version;
			int num = 3;
			TypeDefinition pluginType = this.PluginType;
			array[num] = ((pluginType != null) ? pluginType.FullName : null);
			int num2 = 4;
			FileInfo file = this.File;
			array[num2] = Utils.GetRelativePath((file != null) ? file.FullName : null, UnityGame.InstallPath);
			return string.Format(text, array);
		}

		// Token: 0x040000CB RID: 203
		internal readonly List<Feature> InternalFeatures = new List<Feature>();

		// Token: 0x040000CD RID: 205
		internal bool IsSelf;

		// Token: 0x040000CF RID: 207
		private PluginManifest manifest;
	}
}
