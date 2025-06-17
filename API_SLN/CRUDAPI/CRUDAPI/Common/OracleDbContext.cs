using Oracle.ManagedDataAccess.Client;

namespace CRUDAPI.Data
{
	public class OracleDbContext
	{
		private readonly IConfiguration _configuration;
		private readonly string _connectionString;

		public OracleDbContext(IConfiguration configuration)
		{
			_configuration = configuration;
			_connectionString = _configuration.GetConnectionString("OracleConnection");
		}

		public OracleConnection CreateConnection() => new OracleConnection(_connectionString);
	}
}