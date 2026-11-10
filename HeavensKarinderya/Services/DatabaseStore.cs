using HeavensKarinderya.Models;
using MySql.Data.MySqlClient;

namespace HeavensKarinderya.Services;

// Database access built from the instructor's sample project (DBConProj).
// The connection comes from DatabaseConnection (DBconnection.cs, copied unchanged).
// The SELECT and INSERT statements are the same ones used in the sample Form1.
public static class DatabaseStore
{
    // Optional: to test away from school, put another connection string in
    // Data/connection.txt (for example a local XAMPP MySQL). Without that file
    // the instructor's default connection is used.
    public static string OverridePath => Path.Combine(AppContext.BaseDirectory, "Data", "connection.txt");

    private static DatabaseConnection Database()
    {
        if (File.Exists(OverridePath))
        {
            var text = File.ReadAllText(OverridePath).Trim();
            if (text.Length > 0) return new DatabaseConnection(text);
        }
        return DatabaseConnection.CreateDefault();
    }

    // ---- Read package names from package_tbl (sample: LoadPackages) ----
    public static List<string> LoadPackages()
    {
        var packages = new List<string>();
        using (var conn = Database().CreateConnection())
        {
            conn.Open();
            string sql = "SELECT package_name FROM package_tbl";
            using (var cmd = new MySqlCommand(sql, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    packages.Add(reader.GetString("package_name"));
                }
            }
        }
        return packages;
    }

    // Which server and database the app is using, so it can be compared with
    // what the teacher sees in phpMyAdmin.
    public static string Describe()
    {
        var b = new MySqlConnectionStringBuilder(Database().CreateConnection().ConnectionString);
        return $"{b.Server}:{b.Port} / {b.Database}";
    }

    // The column that holds the customer's name in transactions_tbl.
    private const string NameColumn = "customer_name";

    // ---- Save the order to transactions_tbl (sample: btnSave_Click) ----
    // Like the sample's order list, every meal ordered is one row,
    // so a quantity of 2 saves the package name twice.
    // The customer's name is saved in the customer_name column on every row.
    // If the column does not exist yet it is added (nullable, existing rows stay).
    // Returns true when the name was saved too.
    public static bool SaveTransaction(IEnumerable<OrderItem> items, string customerName)
    {
        using (var conn = Database().CreateConnection())
        {
            conn.Open();
            var nameSaved = EnsureNameColumn(conn);

            using (var tran = conn.BeginTransaction())
            {
                string sql = nameSaved
                    ? "INSERT INTO transactions_tbl " +
                      "(package_name, transaction_date, " + NameColumn + ") " +
                      "VALUES (@pkg, @date, @name)"
                    : "INSERT INTO transactions_tbl " +
                      "(package_name, transaction_date) " +
                      "VALUES (@pkg, @date)";

                foreach (var item in items)
                {
                    for (var i = 0; i < item.Quantity; i++)
                    {
                        using (var cmd = new MySqlCommand(sql, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@pkg", item.Name);
                            cmd.Parameters.AddWithValue("@date", DateTime.Now);
                            if (nameSaved) cmd.Parameters.AddWithValue("@name", customerName);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                tran.Commit();   // all rows saved, or none
            }
            return nameSaved;
        }
    }

    // Makes sure transactions_tbl has the name column. Returns false if it
    // is missing and could not be added (for example, no ALTER permission).
    private static bool EnsureNameColumn(MySqlConnection conn)
    {
        const string check =
            "SELECT COUNT(*) FROM information_schema.COLUMNS " +
            "WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'transactions_tbl' AND COLUMN_NAME = @col";
        using (var cmd = new MySqlCommand(check, conn))
        {
            cmd.Parameters.AddWithValue("@col", NameColumn);
            if (Convert.ToInt32(cmd.ExecuteScalar()) > 0) return true;
        }

        try
        {
            using (var cmd = new MySqlCommand(
                "ALTER TABLE transactions_tbl ADD COLUMN " + NameColumn + " VARCHAR(100) NULL", conn))
                cmd.ExecuteNonQuery();
            return true;
        }
        catch (MySqlException)
        {
            return false;
        }
    }
}
