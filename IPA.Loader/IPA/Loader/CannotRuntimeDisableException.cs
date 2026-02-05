using System;
using System.Runtime.Serialization;

namespace IPA.Loader
{
	/// <summary>
	/// Indicates that a plugin cannot be disabled at runtime. Generally not considered an error, however.
	/// </summary>
	// Token: 0x0200003D RID: 61
	[Serializable]
	public class CannotRuntimeDisableException : Exception
	{
		/// <summary>
		/// The plugin that cannot be disabled at runtime.
		/// </summary>
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600017E RID: 382 RVA: 0x00006499 File Offset: 0x00004699
		public PluginMetadata Plugin { get; }

		/// <summary>
		/// Creates an exception for the given plugin metadata.
		/// </summary>
		/// <param name="plugin">the plugin that cannot be disabled</param>
		// Token: 0x0600017F RID: 383 RVA: 0x000064A4 File Offset: 0x000046A4
		public CannotRuntimeDisableException(PluginMetadata plugin)
			: base(string.Concat(new string[] { "Cannot runtime disable plugin \"", plugin.Name, "\" (", plugin.Id, ")" }))
		{
			this.Plugin = plugin;
		}

		/// <summary>
		/// Creats an exception with the given plugin metadata and message information.
		/// </summary>
		/// <param name="plugin">the plugin that cannot be disabled</param>
		/// <param name="message">the message to associate with it</param>
		// Token: 0x06000180 RID: 384 RVA: 0x000064F4 File Offset: 0x000046F4
		public CannotRuntimeDisableException(PluginMetadata plugin, string message)
			: base(string.Concat(new string[] { message, " \"", plugin.Name, "\" (", plugin.Id, ")" }))
		{
			this.Plugin = plugin;
		}

		/// <summary>
		/// Creates an exception from a serialization context. Not currently implemented.
		/// </summary>
		/// <param name="serializationInfo"></param>
		/// <param name="streamingContext"></param>
		/// <exception cref="T:System.NotImplementedException"></exception>
		// Token: 0x06000181 RID: 385 RVA: 0x00006547 File Offset: 0x00004747
		protected CannotRuntimeDisableException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			throw new NotImplementedException();
		}
	}
}
