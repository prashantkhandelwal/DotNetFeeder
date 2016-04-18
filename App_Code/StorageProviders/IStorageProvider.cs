using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

public interface IStorageProvider
{
    Task<IEnumerable<Bookmarks>> ListBookmarks();
    Task<bool> SaveBookmark(Bookmarks bookmark);
    Task<bool> DeleteBookmark(string id);
}