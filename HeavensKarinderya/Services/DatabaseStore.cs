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

    // ---- Save the order to transactions_tbl (sample: btnSave_Click) ----
    // Like the sample's order list, every meal ordered is one row,
    // so a quantity of 2 saves the package name twice.
    public static void SaveTransaction(IEnumerable<OrderItem> items)
    {
        using (var conn = Database().CreateConnection())
        {
            conn.Open();
            using (var tran = conn.BeginTransaction())
            {
                string sql = "INSERT INTO transactions_tbl " +
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
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                tran.Commit();   // all rows saved, or none
            }
        }
    }
}
