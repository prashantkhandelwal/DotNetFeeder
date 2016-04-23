using System.Collections.Generic;
using System.Threading.Tasks;

public interface IStorageProvider
{
    Task<IEnumerable<Bookmarks>> ListBookmarks();
    Task<bool> SaveBookmark(Bookmarks bookmark);
    Task<bool> DeleteBookmark(string id);
}