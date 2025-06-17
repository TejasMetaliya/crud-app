using CRUDAPI.Common;
using CRUDAPI.Data;
using CRUDAPI.Models;
using NLog;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.Common;
using System.Reflection.Metadata;

namespace CRUDAPI.Repositories
{
	public class UserRepository
	{
		private readonly OracleDbContext _dbContext;
		private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

		public UserRepository(OracleDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		//public async Task<IEnumerable<AllUsers>> GetAllUsers()
		//{
		//	var users = new List<AllUsers>();
		//	OracleTransaction transaction = null;

		//	using (var connection = _dbContext.CreateConnection())
		//	{
		//		await connection.OpenAsync();
		//		using (var command = connection.CreateCommand())
		//		{
		//			command.CommandText = "SELECT ID, NAME, PASSWORD FROM USERS ORDER BY ID";
		//			command.CommandType = CommandType.Text;

		//			using (var reader = await command.ExecuteReaderAsync())
		//			{
		//				while (await reader.ReadAsync())
		//				{
		//					users.Add(new AllUsers
		//					{
		//						Id = reader.GetInt32(0),
		//						Name = reader.GetString(1),
		//						Password = reader.GetString(2)
		//					});
		//				}
		//			}
		//		}
		//	}
		//	return users;
		//}

		public async Task<IEnumerable<AllUsers>> GetAllUsers()
		{
			var users = new List<AllUsers>();
			OracleTransaction transaction = null;

			try
			{
				await using var connection = _dbContext.CreateConnection();
				await connection.OpenAsync();

				// Begin transaction
				using (transaction = connection.BeginTransaction()) {
					_logger.Info("Database transaction started for GetAllUsers");

					await using var command = connection.CreateCommand();
					command.Transaction = transaction;
					command.CommandText = String.Format(CommonSQL.USERSSelectSql);
					command.CommandType = CommandType.Text;

					await using var reader = await command.ExecuteReaderAsync();
					while (await reader.ReadAsync())
					{
						users.Add(new AllUsers
						{
							Id = reader.GetInt32(0),
							Name = reader.GetString(1),
							Password = reader.GetString(2)
						});
					}

					// Commit transaction if successful
					await transaction.CommitAsync();
					_logger.Info($"Retrieved {users.Count} users successfully");
				}				

				return users;
			}
			catch (OracleException ex)
			{
				// Rollback transaction on Oracle errors
				if (transaction != null)
				{
					await transaction.RollbackAsync();
					_logger.Error(ex, "Oracle error occurred. Transaction rolled back");
				}

				_logger.Error(ex, $"Oracle error in GetAllUsers: {ex.Message}");
				throw new DataException("Database operation failed", ex);
			}
			catch (Exception e)
			{
				// Rollback transaction on other errors
				if (transaction != null)
				{
					await transaction.RollbackAsync();
					_logger.Error(e, "Unexpected error occurred. Transaction rolled back");
				}

				_logger.Error(e, $"Unexpected error in GetAllUsers: {e.Message}");
				throw;
			}
			finally
			{
				transaction?.Dispose();
			}
		}
	}
}