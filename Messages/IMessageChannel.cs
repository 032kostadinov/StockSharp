namespace StockSharp.Messages
{
	using System;

	/// <summary>
	/// ���������, ����������� ������������ ����� ���������.
	/// </summary>
	public interface IMessageChannel : IDisposable
	{
		/// <summary>
		/// ������� �����.
		/// </summary>
		void Open();

		/// <summary>
		/// ������� �����.
		/// </summary>
		void Close();

		/// <summary>
		/// ��������� ���������.
		/// </summary>
		/// <param name="message">���������.</param>
		void SendInMessage(Message message);

		/// <summary>
		/// ������� ��������� ������ ���������.
		/// </summary>
		event Action<Message> NewOutMessage;
	}
}