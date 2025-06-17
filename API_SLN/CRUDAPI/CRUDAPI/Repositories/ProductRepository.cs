using CRUDAPI.Common;
using CRUDAPI.Data;
using CRUDAPI.Models;
using NLog;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace CRUDAPI.Repositories
{
	public class ProductRepository
	{
		private readonly OracleDbContext _dbContext;
		private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

		public ProductRepository(OracleDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<IEnumerable<Product>> GetAllProducts()
		{
			var products = new List<Product>();
			OracleTransaction transaction = null;

			try
			{
				await using var connection = _dbContext.CreateConnection();
				await connection.OpenAsync();

				// Begin transaction
				using (transaction = connection.BeginTransaction())
				{
					_logger.Info("Database transaction started for GetAllProducts");

					await using var command = connection.CreateCommand();
					command.Transaction = transaction;
					command.CommandText = String.Format(CommonSQL.M_PRODUCTSelectSql);
					command.CommandType = CommandType.Text;

					await using var reader = await command.ExecuteReaderAsync();
					while (await reader.ReadAsync())
					{
						products.Add(new Product
						{
							ProId = reader.GetInt32(0),
							ProCode = reader.GetString(1),
							ProName = reader.GetString(2),
							ProDescription = reader.GetString(3),
							ProPrice = reader.GetDecimal(4),
							ProCategory = reader.GetString(5),
							ProQuantity = reader.GetInt32(6),
							ProInventoryStatus = reader.GetString(7)
						});
					}

					// Commit transaction if successful
					await transaction.CommitAsync();
					_logger.Info($"Retrieved {products.Count} products successfully");
				}

				return products;
			}
			catch (OracleException ex)
			{
				// Rollback transaction on Oracle errors
				if (transaction != null)
				{
					await transaction.RollbackAsync();
					_logger.Error(ex, "Oracle error occurred. Transaction rolled back");
				}

				_logger.Error(ex, $"Oracle error in GetAllProducts: {ex.Message}");
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

				_logger.Error(e, $"Unexpected error in GetAllProducts: {e.Message}");
				throw;
			}
			finally
			{
				transaction?.Dispose();
			}
		}
	}
}
