namespace krill_be.Models
{
	public class LoginData
	{
		public string Email { get; set; }
		public string Password { get; set; }

        public LoginData(string email, string password)
        {
            this.Email = email;
            this.Password = password;
        }

		public bool AreRegistrationFieldsFilled()
		{
			bool isValid = false;

			if (Email != null || Email == "")
			{
				if (Password != null || Password == "")
				{
					isValid = true;
				}
			}

			return isValid;
		}
	}
}
