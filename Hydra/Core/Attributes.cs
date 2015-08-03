namespace StockSharp.Hydra.Core
{
	using System;

	using Ecng.Common;

	using StockSharp.Localization;

	/// <summary>
	/// Localized task settings display name.
	/// </summary>
	public class TaskSettingsDisplayNameAttribute : DisplayNameLocAttribute
	{
		/// <summary>
		/// Initizalize new instance.
		/// </summary>
		/// <param name="sourceName">Default name of the task.</param>
		public TaskSettingsDisplayNameAttribute(string sourceName)
			: base(LocalizedStrings.TaskSettings, sourceName)
		{
		}
	}

	/// <summary>
	/// ������������ �������.
	/// </summary>
	public class TaskDocAttribute : Attribute
	{
		/// <summary>
		/// ������ �� ������ ������������.
		/// </summary>
		public string DocUrl { get; private set; }

		/// <summary>
		/// ������� <see cref="TaskDocAttribute"/>.
		/// </summary>
		/// <param name="docUrl">������ �� ������ ������������.</param>
		public TaskDocAttribute(string docUrl)
		{
			if (docUrl.IsEmpty())
				throw new ArgumentNullException("docUrl");

			DocUrl = docUrl;
		}
	}
}