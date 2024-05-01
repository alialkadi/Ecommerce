namespace Ecommerce.APIs.Dtos
{
	public class ResponseAuth
	{
        public ResponseAuth(string? UserName = null, string? Email = null, string? role = null)
        {
			UserData = new UserDto
            {
                DisplayName = UserName!,
                Email = Email!,
                role = role!
            };
        }
        public string status { get; set; }
        public UserDto? UserData { get; set; }
        public string token { get; set; }
    }
}
