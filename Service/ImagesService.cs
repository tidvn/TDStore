using AspNetCore.Identity.MongoDbCore.Infrastructure;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TDStore.Models;

namespace TDStore.Service
{
    public class ImagesService
    {
        private readonly IMongoCollection<ImageData> _imagesCollection;
        public ImagesService(
        IOptions<MongoDbSettings> MongoDbSettings)
        {
            var mongoClient = new MongoClient(
                MongoDbSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                MongoDbSettings.Value.DatabaseName);

            _imagesCollection = mongoDatabase.GetCollection<ImageData>("Images");
        }
        public async Task<List<ImageData>> GetAllAsync() =>
        await _imagesCollection.Find(_ => true).ToListAsync();

        public async Task<ImageData?> GetByIdAsync(string id) =>
            await _imagesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(ImageData newImage) =>
            await _imagesCollection.InsertOneAsync(newImage);

        public async Task UpdateAsync(string id, ImageData updatedImage) =>
            await _imagesCollection.ReplaceOneAsync(x => x.Id == id, updatedImage);

        public async Task RemoveAsync(string id) =>
            await _imagesCollection.DeleteOneAsync(x => x.Id == id);

    }
}
