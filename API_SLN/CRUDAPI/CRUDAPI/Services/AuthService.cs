using CRUDAPI.Common;
using CRUDAPI.Data;
using Microsoft.IdentityModel.Tokens;
using NLog;
using Oracle.ManagedDataAccess.Client;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CRUDAPI.Services
{
	public class AuthService(IConfiguration configuration, OracleDbContext dbContext)
	{
		private static Logger _logger = LogManager.GetCurrentClassLogger();
		private readonly IConfiguration _configuration = configuration;
		private readonly OracleDbContext _dbContext = dbContext;

		public async Task<bool> ValidateUserCredentials(string username, string password)
		{
			//using (var connection = new OracleConnection(_connectionString))
			//{
			//	await connection.OpenAsync();
			//	using (var command = connection.CreateCommand())
			//	{
			//		command.CommandText = "SELECT COUNT(1) FROM USERS WHERE NAME = :username AND PASSWORD = :password";
			//		command.Parameters.Add(new OracleParameter("username", username));
			//		command.Parameters.Add(new OracleParameter("password", password));

			//		var result = (decimal)(await command.ExecuteScalarAsync())!;
			//		return result == 1;
			//	}
			//}
			try
			{
				await using var connection = _dbContext.CreateConnection();
				await connection.OpenAsync();

				using var transaction = connection.BeginTransaction();

				using var command = connection.CreateCommand();
				command.Transaction = transaction;
				command.CommandText = String.Format(CommonSQL.USERSSelectCountSql, username, password);

				var result = (decimal)(await command.ExecuteScalarAsync())!;

				await transaction.CommitAsync();

				_logger.Info("Login attempted for user " + username + "");

				return result == 1;
			}
			catch (OracleException ex)
			{
				_logger.Error(ex, "Oracle error during credential validation");
				throw;
			}
			catch (Exception e)
			{
				_logger.Error(e);
				throw;
			}
		}

		public string GenerateJwtToken(string username)
		{
			try
			{
				var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
				_configuration["Jwt:Key"]!));
				var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

				var claims = new[]
				{
					new Claim(ClaimTypes.Name, username),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
				};

				var token = new JwtSecurityToken(
					issuer: _configuration["Jwt:Issuer"],
					audience: _configuration["Jwt:Audience"],
					claims: claims,
					expires: DateTime.Now.AddMinutes(30),
					signingCredentials: credentials
				);

				return new JwtSecurityTokenHandler().WriteToken(token);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}