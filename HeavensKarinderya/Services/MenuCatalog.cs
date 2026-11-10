using System.Text.Json;
using HeavensKarinderya.Models;

namespace HeavensKarinderya.Services;

public static class MenuCatalog
{
    public static string MenuPath => Path.Combine(AppContext.BaseDirectory, "Data", "menu.json");

    public static List<MenuMeal> Load()
    {
        if (!File.Exists(MenuPath))
            throw new FileNotFoundException("Menu file not found: Data/menu.json", MenuPath);

        var meals = JsonSerializer.Deserialize<List<MenuMeal>>(
            File.ReadAllText(MenuPath),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? throw new InvalidDataException("Menu file is empty.");

        if (meals.Count == 0 ||
            meals.Any(m => string.IsNullOrWhiteSpace(m.Id) ||
                           string.IsNullOrWhiteSpace(m.Name) || m.Price <= 0))
            throw new InvalidDataException("Menu items must have IDs, names and positive prices.");

        if (meals.Select(m => m.Id).Distinct(StringComparer.OrdinalIgnoreCase).Count() != meals.Count)
            throw new InvalidDataException("Duplicate menu IDs found.");

        return meals;
    }
}
