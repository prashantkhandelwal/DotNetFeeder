using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public interface IStorageProvider
{
    bool SaveBookmark(string URL);
}