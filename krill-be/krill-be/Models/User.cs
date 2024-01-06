using MongoDB.Bson.Serialization.Attributes;
using System.Security.Cryptography;
using MongoDB.Bson;

namespace krill_be.Models
{
	public class User : Model 
	{
		public string Username { get; set; }
		public string Password { get; set; }

		public User() { }
		public User(string username, string password)
		{
			this.Username = username;
			this.Password = password;
		}

		public string hashPassword(string password)
		{
			return SHA256.Create(password).ToString();
		}

		public bool areRegistrationFieldsCompiled()
		{
			bool isValid = false;

			if (Username != null || Username == "")
			{
				if(Password != null || Password == "")
				{
					isValid = true;
				}
			}

			return isValid;
		}
	}
}
