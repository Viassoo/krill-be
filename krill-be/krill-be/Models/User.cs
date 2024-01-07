using MongoDB.Bson.Serialization.Attributes;
using System.Security.Cryptography;
using MongoDB.Bson;
using System.Text;

namespace krill_be.Models
{
	public class User : Model 
	{
		[BsonRequired]
		public string Email { get; set; }

		[BsonRequired]
		public string Password { get; set; }

		public byte[]? Salt { get; set; }

		public User() { }
		public User(string email, string password, byte[] salt)
		{
			Email = email;
			Password = password;
			Salt = salt;
		}

		public bool AreRegistrationFieldsFilled()
		{
			bool isValid = false;

			if (Email != null || Email == "")
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
