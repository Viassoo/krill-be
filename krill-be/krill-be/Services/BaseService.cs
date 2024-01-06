using krill_be.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace krill_be.Services
{
	public class BaseService<T> where T : Model
	{
		public readonly IMongoCollection<T> _collection;

		public MongoClient mongoClient { get; private set; }

		public IMongoDatabase mongoDatabase { get; private set; }

		public BaseService(IOptions<KrillDatabaseSettings> krillDatabaseSettings)
		{
			mongoClient = new MongoClient(
				krillDatabaseSettings.Value.ConnectionString
				);

			mongoDatabase = mongoClient.GetDatabase(
				krillDatabaseSettings.Value.DatabaseName
				);

			_collection = mongoDatabase.GetCollection<T>(
				krillDatabaseSettings.Value.UsersCollectionName
				);
		}

		public async Task<T?> GetOneAsync() =>
			await _collection.Find(_ => true).FirstAsync();

		/*
		 * Come faccio se la classe T non ha un id? Dovrei presupporre che ce l'abbia
		 */
		public async Task<T> FindOneById(string id) =>
			await _collection.Find(Builders<T>.Filter.Eq("id", id)).FirstAsync();

		public async Task CreateOneAync(T objectToCreate) =>
			await _collection.InsertOneAsync(objectToCreate);

		public async Task CreateManyAsync(List<T> objectsToCreate) => 
			await _collection.InsertManyAsync(objectsToCreate);

		public async Task DeleteOneById(string id) =>
			await _collection.DeleteOneAsync(x => x.Id == id);

		public async Task UpdateOneById(string id, T updatedObject) =>
			await _collection.ReplaceOneAsync(x => x.Id == id, updatedObject);
	}
}
