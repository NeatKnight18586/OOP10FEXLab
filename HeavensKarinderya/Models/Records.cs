namespace HeavensKarinderya.Models;

public sealed class MenuMeal
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal Price { get; set; }
    public string Icon { get; set; } = "🍚";
}

public sealed class OrderItem
{
    public string MealId { get; set; } = "";
    public string Name { get; set; } = "";
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal LineTotal => UnitPrice * Quantity;
}

public sealed class OrderRecord
{
    public int Number { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public string Status { get; set; } = "Preparing";
    public List<OrderItem> Items { get; set; } = new();
    public decimal Total { get; set; }
}

public sealed class TransactionFile
{
    public int NextNumber { get; set; } = 101;
    public long Revision { get; set; }
    public List<OrderRecord> Orders { get; set; } = new();
}
