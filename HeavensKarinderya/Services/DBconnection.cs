using MySql.Data.MySqlClient;

public class DatabaseConnection
{
    private readonly string _connectionString;

    public DatabaseConnection(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>Default settings for this app. Change user / password here.</summary>
    public static DatabaseConnection CreateDefault() =>
        new("Server=172.16.24.11;Port=3306;Database=fastfoodDB;Uid=root;Pwd=1234;");

    /// <summary>Returns a NEW, not-yet-opened connection. Callers open and dispose it.</summary>
    public MySqlConnection CreateConnection() => new(_connectionString);
}