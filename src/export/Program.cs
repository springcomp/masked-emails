using Newtonsoft.Json;
using System.Data.SQLite;
using System.Globalization;

if (args.Length == 0)
{
    Console.WriteLine("Missing path to SQLite database.");
    Environment.Exit(1);
}

string cs = $"URI=file:{args[0]}";

GetSQLiteVersion(cs);

var profiles = GetSQLiteProfiles(cs).ToDictionary(
    kv => kv.Id,
    kv => kv
    );

var addresses = GetSQLiteAddresses(cs, profiles);

Console.WriteLine(
    JsonConvert.SerializeObject(addresses)
);

static void GetSQLiteVersion(string connectionString)
{
    const string stm = "SELECT SQLITE_VERSION()";
    using var con = new SQLiteConnection(connectionString);
    con.Open();
    var cmd = new SQLiteCommand(stm, con);
    string version = cmd.ExecuteScalar().ToString();

    Console.WriteLine($"SQLite version: {version}");
}
static IEnumerable<Profile> GetSQLiteProfiles(string connectionString)
{
    var profiles = new List<Profile>();

    const string stm = "SELECT Id, EmailAddress, DisplayName, ForwardingAddress, CreatedUtc FROM Profiles";
    using var con = new SQLiteConnection(connectionString);
    con.Open();

    var cmd = new SQLiteCommand(stm, con);
    using var reader = cmd.ExecuteReader();

    while (reader.Read())
    {
        profiles.Add(new Profile { 
            Id = (string)reader["Id"],
            EmailAddress = (string)reader["EmailAddress"],
            DisplayName = (string)reader["DisplayName"],
            ForwardingAddress = (string)reader["ForwardingAddress"],
            CreatedUtc = ParseDateTime((string)reader["CreatedUtc"]),
        });
    }

    return profiles;
}
static IEnumerable<Address> GetSQLiteAddresses(string connectionString, IDictionary<string, Profile> profiles)
{
    var addresses = new List<Address>();

    const string stm = "SELECT Id, Name, Description, EmailAddress, EnableForwarding, Received, CreatedUtc, Profile_Id FROM Addresses";
    using var con = new SQLiteConnection(connectionString);
    con.Open();

    var cmd = new SQLiteCommand(stm, con);
    using var reader = cmd.ExecuteReader();

    while (reader.Read())
    {
        var address = new Address
        {
            Id = Convert.ToInt32((long)reader["Id"]),
            Name = (string)reader["Name"],
            Description = (string)reader["Description"],
            EmailAddress = (string)reader["EmailAddress"],
            EnableForwarding = (long)reader["EnableForwarding"] != 0,
            Received = Convert.ToInt32((long)reader["Received"]),
            CreatedUtc = ParseDateTime((string)reader["CreatedUtc"]),
            Profile_Id = (string)reader["Profile_Id"],
        };
        if (address.EnableForwarding)
        {
            var profile = profiles[address.Profile_Id];
            address.ForwardToEmailAddress = profile.ForwardingAddress;
        }
        addresses.Add(address);
    }

    return addresses;
}

static DateTime ParseDateTime(string dateTime)
{
    return DateTime.ParseExact(dateTime, "yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.GetCultureInfo("fr-FR"), DateTimeStyles.None);
}