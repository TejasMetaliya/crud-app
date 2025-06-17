namespace CRUDAPI.Models
{
	public class Users
	{
		public string Name { get; set; }
		public string Password { get; set; }
	}

	public class AuthResponse
	{
		public bool IsSuccess { get; set; }
		public string Token { get; set; }
		public DateTime? Expiration { get; set; }
		public string ErrorMessage { get; set; }
	}

	public class AllUsers
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Password { get; set; }
	}
}