using AspNetCore.Identity.MongoDbCore.Infrastructure;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TDStore.Models;

namespace TDStore.Service
{
    public class ProductsService
    {
        private readonly IMongoCollection<Product> _productCollection;
        public ProductsService(
        IOptions<MongoDbSettings> MongoDbSettings)
        {
            var mongoClient = new MongoClient(
                MongoDbSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                MongoDbSettings.Value.DatabaseName);

            _productCollection = mongoDatabase.GetCollection<Product>("Products");
        }
        public async Task<List<Product>> GetAllAsync() =>
        await _productCollection.Find(_ => true).ToListAsync();

        public async Task<Product?> GetByIdAsync(string id) =>
            await _productCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Product newProduct) =>
            await _productCollection.InsertOneAsync(newProduct);

        public async Task UpdateAsync(string id, Product updatedProduct) =>
            await _productCollection.ReplaceOneAsync(x => x.Id == id, updatedProduct);

        public async Task RemoveAsync(string id) =>
            await _productCollection.DeleteOneAsync(x => x.Id == id);

    }
}
