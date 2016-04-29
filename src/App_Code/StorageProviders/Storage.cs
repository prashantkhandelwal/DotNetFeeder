/// <summary>
/// Summary description for Storage
/// </summary>
public sealed class Storage
{
    private static IStorageProvider _storageProvider = null;
    private static LightInject.ServiceContainer container = new LightInject.ServiceContainer();
    private static StorageSettings _settings = SettingsStore.ReadSettings<StorageSettings>("StorageSettings");

    private Storage() { }

    public static IStorageProvider GetInstance
    {
        get
        {
            if (_storageProvider == null)
            {
                if (_settings.storageType == StorageType.Mongolab)
                {
                    container.Register<IStorageProvider, MongoDBProvider>();
                }
                else if (_settings.storageType == StorageType.RavenDB)
                {
                    container.Register<IStorageProvider, RavenDBProvider>();
                }
                _storageProvider = container.GetInstance<IStorageProvider>();
            }

            return _storageProvider;
        }
    }
}