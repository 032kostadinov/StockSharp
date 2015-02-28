namespace StockSharp.Community
{
	using System;

	/// <summary>
	/// ������ ��� ������� � <see cref="IDocService"/>.
	/// </summary>
	public class DocClient : BaseCommunityClient<IDocService>
	{
		/// <summary>
		/// ������� <see cref="DocClient"/>.
		/// </summary>
		public DocClient()
			: this(new Uri("http://stocksharp.com/services/docservice.svc"))
		{
		}

		/// <summary>
		/// ������� <see cref="DocClient"/>.
		/// </summary>
		/// <param name="address">����� �������.</param>
		public DocClient(Uri address)
			: base(address, "doc", true)
		{
		}

		/// <summary>
		/// ��������� �������� ����� ������.
		/// </summary>
		/// <param name="product">��� ��������.</param>
		/// <param name="description">�������� ����� ������.</param>
		public void PostNewVersion(Products product, string description)
		{
			Invoke(f => f.PostNewVersion(SessionId, product, description));
		}
	}
}