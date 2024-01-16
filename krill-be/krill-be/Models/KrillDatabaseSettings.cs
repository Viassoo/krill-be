namespace krill_be.Models
{
	public class KrillDatabaseSettings
	{
		public string ConnectionString { get; set; } = null!;
		
		public string DatabaseName { get; set; } = null!;

		public string UsersCollectionName { get; set; } = null!;

		public KrillDatabaseSettings() { }
	}
}