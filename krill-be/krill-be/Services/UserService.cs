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

	}
}
