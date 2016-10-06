using System;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;

/// <summary>
/// Enables the support for MongoDB.
/// In this case it is MongoLab
/// </summary>
public class MongoDBProvider : IStorageProvider
{
    private IMongoClient _client;
    private IMongoDatabase _database;
    private IMongoCollection<BsonDocument> _bookmarkCollection;
    private List<BsonDocument> _bookmarksList;
    private StorageSettings _settings = SettingsStore.ReadSettings<StorageSettings>("StorageSettings");

    public MongoDBProvider()
    {
        _client = new MongoClient("mongodb://" + _settings.dbUser + ":" + _settings.dbPassword + "@" + _settings.mongoDbURL);
        _database = _client.GetDatabase(_settings.mongoDbURL.Substring(_settings.mongoDbURL.LastIndexOf('/') + 1, _settings.mongoDbURL.Length - _settings.mongoDbURL.LastIndexOf('/') - 1));
        _bookmarkCollection = _database.GetCollection<BsonDocument>("bookmarks");
        if (_bookmarkCollection == null)
        {
            _database.CreateCollection("Bookmarks");
        }
    }

    public Task<bool> DeleteBookmark(string id)
    {
        return Task.Run(async () =>
        {
            var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
            await _bookmarkCollection.DeleteOneAsync(filter);
            return true;
        });
    }

    public Task<IEnumerable<Bookmarks>> ListBookmarks()
    {
        List<Bookmarks> b = new List<Bookmarks>();
        return Task.Run(async () =>
        {
            _bookmarksList = await _bookmarkCollection.Find(new BsonDocument()).ToListAsync();
            foreach (var bookmark in _bookmarksList)
            {
                Bookmarks _bookmark = new Bookmarks();
                _bookmark.Id = Convert.ToString(bookmark.GetElement("_id").Value);
                _bookmark.Title = bookmark.GetElement("Title").Value.AsString;
                _bookmark.URL = bookmark.GetElement("URL").Value.AsString;
                b.Add(_bookmark);
            }

            return b as IEnumerable<Bookmarks>;
        });

    }

    public async Task<bool> SaveBookmark(Bookmarks bookmark)
    {
        Bookmarks _bookmark = new Bookmarks();
        _bookmark.Title = bookmark.Title;
        _bookmark.URL = bookmark.URL;

        BsonDocument document = new BsonDocument
        {
            {"URL", bookmark.URL },
            {"Title", bookmark.Title }
        };

        await _bookmarkCollection.InsertOneAsync(document);
        return true;
    }
}