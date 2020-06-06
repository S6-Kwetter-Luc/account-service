namespace account_service.DatastoreSettings
{
    public class AccountstoreDatabaseSettings : IAccountstoreDatabaseSettings
    {
        public string AccountCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IAccountstoreDatabaseSettings
    {
        string AccountCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}