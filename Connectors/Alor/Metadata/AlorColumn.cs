#region S# License
/******************************************************************************************
NOTICE!!!  This program and source code is owned and licensed by
StockSharp, LLC, www.stocksharp.com
Viewing or use of this code requires your acceptance of the license
agreement found at https://github.com/StockSharp/StockSharp/blob/master/LICENSE
Removal of this comment is a violation of the license agreement.

Project: StockSharp.Alor.Metadata.Alor
File: AlorColumn.cs
Created: 2015, 11, 11, 2:32 PM

Copyright 2010 by StockSharp, LLC
*******************************************************************************************/
#endregion S# License
namespace StockSharp.Alor.Metadata
{
	using System;

	using Ecng.Common;

	/// <summary>
	///  �������� �������.
	/// </summary>
	public class AlorColumn : Equatable<AlorColumn>
	{
		internal AlorColumn(AlorTableTypes tableType, string name, Type dataType, bool isMandatory = true)
		{
			//TableTypeName = TableType.ToString();
			if (name.IsEmpty())
				throw new ArgumentNullException(nameof(name));

			if (dataType == null)
				throw new ArgumentNullException(nameof(dataType));

			TableType = tableType;
			IsMandatory = isMandatory;
			Name = name;
			DataType = dataType;
			//TableTypeName = TableType.ToString();
			AlorManagerColumns.AllAlorColumn.Add(this);
		}

		///// <summary>
		///// getTableTypeName
		///// </summary>
		///// <returns></returns>
		//public string TableTypeName;

		internal AlorTableTypes TableType { get; private set; }

		/// <summary>
		/// ������ �� �������� �� ���������
		/// </summary>
		public bool IsMandatory { get; private set; }

		/// <summary>
		/// �������� �������.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// ��� �������.
		/// </summary>
		public Type DataType { get; private set; }

		/// <summary>
		/// ������� ����� <see cref="AlorColumn"/>.
		/// </summary>
		/// <returns>�����.</returns>
		public override AlorColumn Clone()
		{
			return new AlorColumn(TableType, Name, DataType)
			{
				IsMandatory = IsMandatory
			};
		}

		/// <summary>
		/// �������� <see cref="AlorColumn" /> �� ���������������.
		/// </summary>
		/// <param name="other">������ ��������, � ������� ���������� ����������.</param>
		/// <returns><see langword="true"/>, ���� ������ �������� ����� ��������, �����, <see langword="false"/>.</returns>
		protected override bool OnEquals(AlorColumn other)
		{
			return TableType == other.TableType && Name == other.Name;
		}

		/// <summary>
		/// ���������� ���-��� �������.
		/// </summary>
		/// <returns>���-��� �������.</returns>
		public override int GetHashCode()
		{
			return TableType.GetHashCode() ^ Name.GetHashCode();
		}
	}
}