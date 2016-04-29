using System;
using System.Collections.Generic;
using Raven.Client.Embedded;
using System.Web.Hosting;
using Raven.Client.Linq;
using System.Threading.Tasks;

/// <summary>
/// Enables the support for RavenDB embedded
/// </summary>
public class RavenDBProvider : IStorageProvider
{
    EmbeddableDocumentStore _store = new EmbeddableDocumentStore
    {
        DataDirectory = HostingEnvironment.MapPath("~/App_Data/Data")
    };

    public RavenDBProvider()
    {
        _store.Initialize();
    }

    public async Task<bool> DeleteBookmark(string id)
    {
        bool isDeleted = false;
        return await Task.Run(() =>
        {
            using (var session = _store.OpenSession())
            {
                Bookmarks _bookmark = session.Load<Bookmarks>(id);
                if (_bookmark != null)
                {
                    session.Delete(_bookmark);
                    session.SaveChanges();
                    isDeleted = true;
                }
            }
            return isDeleted;
        });
    }

    public Task<IEnumerable<Bookmarks>> ListBookmarks()
    {
        IRavenQueryable<Bookmarks> _bookmarksList = null;
        try
        {
            return Task.Run(() =>
            {
                using (var session = _store.OpenSession())
                {
                    _bookmarksList = from bookmarks in session.Query<Bookmarks>() select bookmarks;
                }
                return _bookmarksList as IEnumerable<Bookmarks>;
            });
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> SaveBookmark(Bookmarks bookmark)
    {
        bool isSave = false;
        return await Task.Run(() =>
        {
            using (var session = _store.OpenSession())
            {
                Bookmarks _bookmark = new Bookmarks();
                _bookmark.URL = bookmark.URL;
                _bookmark.Title = bookmark.Title;
                session.Store(bookmark);
                session.SaveChanges();
                isSave = true;
            }
            return isSave;
        });
    }
}