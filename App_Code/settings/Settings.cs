public class GeneralSettings
{
    public int pageSize { get; set; } 
    public int cacheTime { get; set; }
}

public class StorageSettings
{
    public StorageType storageType { get; set; }
    public string mongoDbURL { get; set; }
    public string dbUser { get; set; }
    public string dbPassword { get; set; }
}