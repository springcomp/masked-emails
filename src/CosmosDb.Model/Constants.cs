namespace CosmosDb.Model
{
    public class Constants
    {
        public const string DatabaseId = "MaskedEmails";
        public const string ContainerName = "ProfilesDb";
        public const string PartitionKeyPath = "/id";

        internal const string QueryProfilesStatement = "SELECT * FROM c";
    }
}
