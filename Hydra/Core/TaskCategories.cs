#region S# License
/******************************************************************************************
NOTICE!!!  This program and source code is owned and licensed by
StockSharp, LLC, www.stocksharp.com
Viewing or use of this code requires your acceptance of the license
agreement found at https://github.com/StockSharp/StockSharp/blob/master/LICENSE
Removal of this comment is a violation of the license agreement.

Project: StockSharp.Hydra.Core.CorePublic
File: TaskCategories.cs
Created: 2015, 11, 11, 2:32 PM

Copyright 2010 by StockSharp, LLC
*******************************************************************************************/
#endregion S# License
namespace StockSharp.Hydra.Core
{
	using System;

	/// <summary>
	/// ��������� �����.
	/// </summary>
	[Flags]
	public enum TaskCategories
	{
		/// <summary>
		/// ������.
		/// </summary>
		Russia = 1,

		/// <summary>
		/// �������.
		/// </summary>
		America = Russia << 1,

		/// <summary>
		/// �������� �����.
		/// </summary>
		Stock = America << 1,

		/// <summary>
		/// ������.
		/// </summary>
		Forex = Stock << 1,

		/// <summary>
		/// ������������.
		/// </summary>
		Crypto = Forex << 1,

		/// <summary>
		/// �������.
		/// </summary>
		History = Crypto << 1,

		/// <summary>
		/// ����-����.
		/// </summary>
		RealTime = History << 1,

		/// <summary>
		/// ���������.
		/// </summary>
		Free = RealTime << 1,

		/// <summary>
		/// ������.
		/// </summary>
		Paid = Free << 1,

		/// <summary>
		/// ���� (��������).
		/// </summary>
		Ticks = Paid << 1,

		/// <summary>
		/// ����� (��������).
		/// </summary>
		Candles = Ticks << 1,

		/// <summary>
		/// ������ (��������).
		/// </summary>
		MarketDepth = Candles << 1,

		/// <summary>
		/// Level1 (��������).
		/// </summary>
		Level1 = MarketDepth << 1,

		/// <summary>
		/// ��� ������ (��������).
		/// </summary>
		OrderLog = Level1 << 1,

		/// <summary>
		/// ������� (��������).
		/// </summary>
		News = OrderLog << 1,

		/// <summary>
		/// ���������� (��������).
		/// </summary>
		Transactions = News << 1,

		/// <summary>
		/// ��������������� ������.
		/// </summary>
		Tool = Transactions << 1,
	}

	/// <summary>
	/// �������, �������� ��������� �����.
	/// </summary>
	public class TaskCategoryAttribute : Attribute
	{
		/// <summary>
		/// ��������� �����.
		/// </summary>
		public TaskCategories Categories { get; private set; }

		/// <summary>
		/// ������� <see cref="TaskCategoryAttribute"/>.
		/// </summary>
		/// <param name="categories">��������� �����.</param>
		public TaskCategoryAttribute(TaskCategories categories)
		{
			Categories = categories;
		}
	}
}