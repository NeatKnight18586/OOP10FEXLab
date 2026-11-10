using System.Text.Json;
using HeavensKarinderya.Models;

namespace HeavensKarinderya.Services;

// Text-file transaction storage for one Windows user account.
public static class TransactionStore
{
    private static readonly Mutex Gate = new(false, "HeavensKarinderya_OrderGate_2026");
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    public static string DataFolder => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HeavensKarinderya");

    public static string TransactionPath => Path.Combine(DataFolder, "transactions.json");

    private static T Locked<T>(Func<T> action)
    {
        try { Gate.WaitOne(); }
        catch (AbandonedMutexException) { }
        try { return action(); }
        finally { Gate.ReleaseMutex(); }
    }

    public static TransactionFile Snapshot() => Locked(Read);

    public static OrderRecord PlaceOrder(IReadOnlyCollection<OrderItem> items)
    {
        if (items.Count == 0 || items.Any(i => i.Quantity <= 0 || i.UnitPrice <= 0))
            throw new ArgumentException("Select at least one valid meal.");

        return Locked(() =>
        {
            var db = Read();
            if (db.NextNumber == int.MaxValue)
                throw new InvalidOperationException("No more order numbers available.");

            var order = new OrderRecord
            {
                Number = db.NextNumber++,
                CreatedAt = DateTimeOffset.Now,
                Status = "Preparing",
                Items = items.Select(i => new OrderItem
                {
                    MealId = i.MealId, Name = i.Name,
                    UnitPrice = i.UnitPrice, Quantity = i.Quantity
                }).ToList()
            };
            order.Total = order.Items.Sum(i => i.LineTotal);
            db.Orders.Add(order);
            db.Revision++;
            Save(db);
            return order;
        });
    }

    public static int? CallNext() => MoveOldest("Preparing", "Serving");

    public static int? CompleteOldest() => MoveOldest("Serving", "Completed");

    private static int? MoveOldest(string from, string to)
    {
        return Locked(() =>
        {
            var db = Read();
            var order = db.Orders.Where(o => o.Status == from)
                .OrderBy(o => o.Number).FirstOrDefault();
            if (order is null) return null;
            order.Status = to;
            db.Revision++;
            Save(db);
            return (int?)order.Number;
        });
    }

    private static TransactionFile Read()
    {
        if (!File.Exists(TransactionPath)) return new TransactionFile();
        try
        {
            var data = JsonSerializer.Deserialize<TransactionFile>(
                File.ReadAllText(TransactionPath), Options);
            if (data is null || data.NextNumber < 1 || data.Orders is null)
                throw new InvalidDataException("Transaction file is incomplete.");
            return data;
        }
        catch (JsonException ex)
        {
            throw new InvalidDataException("Transaction file has invalid JSON.", ex);
        }
    }

    private static void Save(TransactionFile data)
    {
        Directory.CreateDirectory(DataFolder);
        var temporary = TransactionPath + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            File.WriteAllText(temporary, JsonSerializer.Serialize(data, Options));
            File.Move(temporary, TransactionPath, true);
        }
        finally { if (File.Exists(temporary)) File.Delete(temporary); }
    }
}
