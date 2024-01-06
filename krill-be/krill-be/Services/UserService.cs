using MongoDB.Driver;
using krill_be.Models;
using Microsoft.Extensions.Options;

namespace krill_be.Services
{
	public class UserService : BaseService<User>
	{

		public UserService(IOptions<KrillDatabaseSettings> krillDatabaseSettings) : base(krillDatabaseSettings)
		{
		}

		public async Task<User?> GetUserByEmail(string email) =>
			await _collection.Find(x => x.Email == email).FirstAsync();

		public async Task<bool> checkIfUserExistByEmail(string email)
		{
			User? userFound = await _collection.Find(x => x.Email == email).FirstAsync();
			if (userFound != null)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

	}
}
