using Newtonsoft.Json;
using System.Data.SQLite;
using System.Globalization;
using System.Text.RegularExpressions;

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

var addresses = GetSQLiteAddresses(cs, profiles)
	.GroupBy(kv => kv.Profile_Id)
	;

var users = new List<User>();
foreach (var group in addresses)
{
	var profile = profiles[group.Key];
	users.Add(new User
	{
		Id = profile.Id,
		DisplayName = profile.DisplayName,
		EmailAddress = profile.EmailAddress,
		CreatedUtc = profile.CreatedUtc,
		ForwardingAddress = profile.EmailAddress,
		Addresses = group.ToArray(),
	});
};

var folder = "./Exports";
if (!Directory.Exists(folder))
	Directory.CreateDirectory(folder);

foreach (var user in users)
{
	var name = $"{user.DisplayName.ToLowerInvariant()}.json";
    var path = Path.Combine(folder, name);
	using (var output = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.Read))
	using (var writer = new StreamWriter(output))
		writer.Write(JsonConvert.SerializeObject(user));
}

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
	Console.WriteLine("Exporting profiles...");

	var profiles = new List<Profile>();

	const string stm = "SELECT Id, EmailAddress, DisplayName, ForwardingAddress, CreatedUtc FROM Profiles";
	using var con = new SQLiteConnection(connectionString);
	con.Open();

	var cmd = new SQLiteCommand(stm, con);
	using var reader = cmd.ExecuteReader();

	while (reader.Read())
	{
		var displayName = (string)reader["DisplayName"];
		Console.WriteLine($"Exporting profile {displayName}");
		profiles.Add(new Profile
		{
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
	Console.WriteLine("Exporting addresses...");

	var addresses = new List<Address>();

	const string stm = "SELECT Id, Name, Description, EmailAddress, EnableForwarding, Received, CreatedUtc, Profile_Id FROM Addresses";
	using var con = new SQLiteConnection(connectionString);
	con.Open();

	var cmd = new SQLiteCommand(stm, con);
	using var reader = cmd.ExecuteReader();

	while (reader.Read())
	{
		var emailAddress = (string)reader["EmailAddress"];
		Console.WriteLine($"Exporting address {emailAddress}.");

		var address = new Address
		{
			Id = Convert.ToInt32((long)reader["Id"]),
			Name = (string)reader["Name"],
			Description = reader["Description"] as string ?? "",
			EmailAddress = (string)reader["EmailAddress"],
			EnableForwarding = (long)reader["EnableForwarding"] != 0,
			Received = Convert.ToInt32((long)reader["Received"]),
			CreatedUtc = ParseDateTime(reader["CreatedUtc"] as string),
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


static DateTime ParseDateTime(string? dateTime)
{
	if (dateTime == null)
		return new DateTime(2019, 01, 01, 12, 00, 00, 00, DateTimeKind.Utc);

	const string DateTimeRegexPattern = @"^[0-9]{4}(?:\-[0-9]{2}){2}$";
	Regex DateTimeRegex = new Regex(DateTimeRegexPattern, RegexOptions.Singleline);

	var match = DateTimeRegex.Match(dateTime);
	if (match.Success)
		dateTime = dateTime + " 00:00:00.0000000";

	var splits = dateTime.Split(".");
	if (splits.Length == 1)
		dateTime = dateTime + ".";

	splits = dateTime.Split(".");
	Console.WriteLine($"{dateTime} {splits[1].Length}");
	if (splits[1].Length != 7)
	{
		var before = dateTime;
		dateTime = dateTime.PadRight(27, '0');
		Console.WriteLine($"{before} => {dateTime}");
	}

	return DateTime.ParseExact(dateTime, "yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.GetCultureInfo("fr-FR"), DateTimeStyles.None);
}