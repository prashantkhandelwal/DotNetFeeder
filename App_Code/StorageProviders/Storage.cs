using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Storage
/// </summary>
public sealed class Storage
{
    private static IStorageProvider _storageProvider = null;
    private static LightInject.ServiceContainer container = new LightInject.ServiceContainer();

    private Storage()
    {
        
    }

    public static IStorageProvider GetInstance
    {
        get
        {
            if(_storageProvider == null)
            {
                container.Register<IStorageProvider, MongoProvider>();
                _storageProvider = container.GetInstance<IStorageProvider>();
            }

            return _storageProvider;
        }
    }
}