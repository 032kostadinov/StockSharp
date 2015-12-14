#region S# License
/******************************************************************************************
NOTICE!!!  This program and source code is owned and licensed by
StockSharp Algo Trading, LLC, www.stocksharp.com
Viewing or use of this code requires your acceptance of the license
agreement found at https://github.com/StockSharp/StockSharp/blob/master/LICENSE
Removal of this comment is a violation of the license agreement.

Project: StockSharp.Quik.QuikPublic
File: DdeTable.cs
Created: 2015, 11, 11, 2:32 PM

Copyright 2010 by StockSharp Algo Trading, LLC
*******************************************************************************************/
#endregion S# License
namespace StockSharp.Quik
{
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using System.Reflection;

	using Ecng.Collections;
	using Ecng.Common;
	using Ecng.Serialization;

	using MoreLinq;

	enum DdeTableTypes
	{
		None,
		Security,
		Order,
		StopOrder,
		Trade,
		MyTrade,
		Quote,
		EquityPosition,
		DerivativePosition,
		EquityPortfolio,
		DerivativePortfolio,
		CurrencyPortfolio,
	}

	/// <summary>
	/// �������������� �������, ������������ ��� DDE.
	/// ����� �������� <see cref="Caption"/> ��������, ����� ��������� � Quik-e ����� ����� �������.
	/// ��������, ��� ������� ����������� �� ��������� � Quik-� ������������ �������� '������� ������� �������� ����������',
	/// ��� ���������� ������������� � �����������, ��� �� ������� �������� � ����� ���������:
	/// <example><code>_trader.SecuritiesTable.Caption = "������� ������� �������� ����������";</code></example>
	/// <remarks>�������������, ����� �������� <see cref="Columns"/> ����� ������ ������������ ������� ������� �������.</remarks>
	/// </summary>
	[EntityFactory(typeof(UnitializedEntityFactory<DdeTable>))]
	[TypeSchemaFactory(SearchBy.Properties, VisibleScopes.Both)]
	[Obfuscation(Feature = "Apply to member * when property: renaming", Exclude = true)]
	public sealed class DdeTable : Equatable<DdeTable>
	{
		internal DdeTable(DdeTableTypes type, string caption, string className, IEnumerable<DdeTableColumn> columns)
		{
			columns.ForEach(c => c.IsMandatory = true);
			Init(type, caption, className, columns);
		}

		private void Init(DdeTableTypes type, string caption, string className, IEnumerable<DdeTableColumn> columns)
		{
			if (caption.IsEmpty())
				throw new ArgumentNullException(nameof(caption));

			if (className == null)
				throw new ArgumentNullException(nameof(className));

			if (columns == null)
				throw new ArgumentNullException(nameof(columns));

			Type = type;
			Caption = caption;
			ClassName = className;

			Columns = new DdeTableColumnList();
			Columns.AddRange(columns);
		}

		internal DdeTableTypes Type { get; private set; }

		/// <summary>
		/// ��������� ������� � Quik-e.
		/// </summary>
		public string Caption { get; set; }

		/// <summary>
		/// ����� ������� � Quik-�.
		/// </summary>
		public string ClassName { get; set; }

		private DdeTableColumnList _columns;

		/// <summary>
		/// ���������� � ��������.
		/// </summary>
		/// <remarks>
		/// ���� ��������� ������� ������� � Quik-�, ���������� �������� ������� � � ��������� ����� <see cref="DdeTable"/>.
		/// ����� �������� ������� ������� ������������� ��������� �������:
		/// <example><code>// ������� ����� ����������� 5-�� �� ����� (��������� � ����).
		/// _trader.TradesTable.Columns[4] = DdeTradeColumns.Time;</code></example>
		/// </remarks>
		public DdeTableColumnList Columns
		{
			get { return _columns; }
			private set
			{
				if (value == null)
					throw new ArgumentNullException();

				_columns = value;
				_columns.TableType = () => Type;
			}
		}

		///<summary>
		/// ������� ����� <see cref="DdeTable" />.
		///</summary>
		///<returns>�����.</returns>
		public override DdeTable Clone()
		{
			return new DdeTable(Type, Caption, ClassName, Columns);
		}

		/// <summary>
		/// �������� <see cref="DdeTable" /> �� ���������������.
		/// </summary>
		/// <param name="other">������ ��������, � ������� ���������� ����������.</param>
		/// <returns><see langword="true"/>, ���� ������ �������� ����� ��������, �����, <see langword="false"/>.</returns>
		protected override bool OnEquals(DdeTable other)
		{
			return
				Caption == other.Caption &&
				Type == other.Type &&
				Columns.Count == other.Columns.Count &&
				Columns.SequenceEqual(other.Columns);
		}

		/// <summary>
		/// ���������� ���-��� ������� <see cref="DdeTable"/>.
		/// </summary>
		/// <returns>���-���.</returns>
		public override int GetHashCode()
		{
			return Type.GetHashCode() ^ Caption.GetHashCode();
		}

		/// <summary>
		/// �������� ��������� �������������.
		/// </summary>
		/// <returns>��������� �������������.</returns>
		public override string ToString()
		{
			return Caption;
		}
	}
}