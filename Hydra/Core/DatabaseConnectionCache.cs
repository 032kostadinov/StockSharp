namespace StockSharp.Hydra.Core
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Ecng.Collections;
	using Ecng.Common;
	using Ecng.Data;
	using Ecng.Data.Providers;
	using Ecng.Serialization;
	using Ecng.Xaml.Database;

	/// <summary>
	/// ��� <see cref="DatabaseConnectionPair"/>.
	/// </summary>
	public class DatabaseConnectionCache : IPersistable
	{
		private readonly CachedSynchronizedSet<DatabaseConnectionPair> _connections = new CachedSynchronizedSet<DatabaseConnectionPair>();

		private DatabaseConnectionCache()
		{
		}

		private static readonly Lazy<DatabaseConnectionCache> _instance = new Lazy<DatabaseConnectionCache>(() => new DatabaseConnectionCache());

		/// <summary>
		/// ���.
		/// </summary>
		public static DatabaseConnectionCache Instance
		{
			get { return _instance.Value; }
		}

		/// <summary>
		/// ������ ���� �����������.
		/// </summary>
		public IEnumerable<DatabaseConnectionPair> AllConnections
		{
			get { return _connections.Cache; }
		}

		/// <summary>
		/// ������� �������� ������ �����������.
		/// </summary>
		public event Action<DatabaseConnectionPair> NewConnectionCreated;

		/// <summary>
		/// �������� ����������� � ���� ������.
		/// </summary>
		/// <param name="provider">��������� ��� ������.</param>
		/// <param name="connectionString">������ �����������.</param>
		/// <returns>����������� � ���� ������</returns>
		public DatabaseConnectionPair GetConnection(DatabaseProvider provider, string connectionString)
		{
			var connection = AllConnections.FirstOrDefault(p => p.Provider == provider && p.ConnectionString.CompareIgnoreCase(connectionString));

			if (connection == null)
			{
				connection = new DatabaseConnectionPair { Provider = provider, ConnectionString = connectionString };
				AddConnection(connection);
			}

			return connection;
		}

		/// <summary>
		/// �������� ����� ����������� � ���� ������.
		/// </summary>
		/// <param name="connection">����� �����������.</param>
		public void AddConnection(DatabaseConnectionPair connection)
		{
			if (connection == null)
				throw new ArgumentNullException("connection");

			_connections.Add(connection);
			NewConnectionCreated.SafeInvoke(connection);
		}

		/// <summary>
		/// ��������� ���������.
		/// </summary>
		/// <param name="storage">��������� ��������.</param>
		public void Load(SettingsStorage storage)
		{
			var connections = storage
				.GetValue<IEnumerable<SettingsStorage>>("Connections")
				.Select(s =>
				{
					var providerName = s.GetValue<string>("Provider");
					var provider = DatabaseProviderRegistry.Providers.FirstOrDefault(p => p.Name.CompareIgnoreCase(providerName));

					return provider == null
						? null
						: new DatabaseConnectionPair
						{
							Provider = provider,
							ConnectionString = s.GetValue<string>("ConnectionString")
						};
				})
				.Where(p => p != null)
				.ToArray();

			lock (_connections.SyncRoot)
				_connections.AddRange(connections);
		}

		/// <summary>
		/// ��������� ���������.
		/// </summary>
		/// <param name="storage">��������� ��������.</param>
		public void Save(SettingsStorage storage)
		{
			storage.SetValue("Connections", AllConnections.Select(pair =>
			{
				var conStorage = new SettingsStorage();
				conStorage["Provider"] = pair.Provider.Name;
				conStorage["ConnectionString"] = pair.ConnectionString;
				return conStorage;
			}).ToArray());
		}
	}
}