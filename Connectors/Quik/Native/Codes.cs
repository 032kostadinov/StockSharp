#region S# License
/******************************************************************************************
NOTICE!!!  This program and source code is owned and licensed by
StockSharp, LLC, www.stocksharp.com
Viewing or use of this code requires your acceptance of the license
agreement found at https://github.com/StockSharp/StockSharp/blob/master/LICENSE
Removal of this comment is a violation of the license agreement.

Project: StockSharp.Quik.Native.QuikPublic
File: Codes.cs
Created: 2015, 11, 11, 2:32 PM

Copyright 2010 by StockSharp, LLC
*******************************************************************************************/
#endregion S# License
namespace StockSharp.Quik.Native
{
	/// <summary>
	/// ��������� ���� (�������� � ���������) ���������� TRANS2QUIK.dll, ������� ���������� Quik.
	/// </summary>
	public enum Codes
	{
		/// <summary>
		/// ����� ������ �������.
		/// </summary>
		Success = 0,

		/// <summary>
		/// ��������� ������.
		/// </summary>
		Failed = 1,

		/// <summary>
		/// � ��������� �������� ���� ����������� INFO.EXE,
		/// ���� � ���� �� ������� ������ ��������� ������� �����������.
		/// </summary>
		QuikTerminalNotFound = 2,

		/// <summary>
		/// ������������ ������ Trans2QUIK.DLL ��������� INFO.EXE �� ��������������.
		/// </summary>
		DllVersionNotSupported = 3,

		/// <summary>
		/// ���������� ��� �����������.
		/// </summary>
		AlreadyConnectedToQuik = 4,

		/// <summary>
		/// ������������ ���������.
		/// </summary>
		WrongSyntax = 5,

		/// <summary>
		/// ����������� � �������� �� �����������.
		/// </summary>
		QuikNotConnected = 6,

		/// <summary>
		/// ����������� � ���������� �� �����������.
		/// </summary>
		DllNotConnected = 7,

		/// <summary>
		/// ����������� � Quik �����������.
		/// </summary>
		QuikConnected = 8,

		/// <summary>
		/// ����������� � Quik ���������.
		/// </summary>
		QuikDisconnected = 9,

		/// <summary>
		/// ����������� � ���������� �����������.
		/// </summary>
		DllConnected = 10,

		/// <summary>
		/// ����������� � ���������� ���������.
		/// </summary>
		DllDisconnected = 11,

		/// <summary>
		/// ������ ��������� ������.
		/// </summary>
		MemoryAllocationError = 12,

		/// <summary>
		/// ������������ ������������� ����������.
		/// </summary>
		WrongConnectionHandle = 13,

		/// <summary>
		/// ������������ ����� ����������.
		/// </summary>
		WrongInputParams = 14,
	}
}