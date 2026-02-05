using System;

namespace IPA.Logging
{
	/// <summary>
	/// A class providing extensions for various loggers.
	/// </summary>
	// Token: 0x02000034 RID: 52
	public static class LoggerExtensions
	{
		/// <summary>
		/// Gets a child logger, if supported. Currently the only defined and supported logger is <see cref="T:IPA.Logging.StandardLogger" />, and most plugins will only ever receive this anyway.
		/// </summary>
		/// <param name="logger">the parent <see cref="T:IPA.Logging.Logger" /></param>
		/// <param name="name">the name of the child</param>
		/// <returns>the child logger</returns>
		// Token: 0x06000151 RID: 337 RVA: 0x00005C58 File Offset: 0x00003E58
		public static Logger GetChildLogger(this Logger logger, string name)
		{
			StandardLogger standardLogger = logger as StandardLogger;
			if (standardLogger != null)
			{
				return standardLogger.GetChild(name);
			}
			throw new InvalidOperationException();
		}
	}
}
