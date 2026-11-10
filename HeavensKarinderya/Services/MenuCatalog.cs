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

    public static string ImageFolder => Path.Combine(AppContext.BaseDirectory, "Data", "Images");

    private static Dictionary<string, string>? _imageFiles;

    // Photo for a meal, from Data/Images/manifest.json (falls back to "<id>_*.jpg").
    // Returns null when there is no photo, so the kiosk can keep its placeholder.
    public static string? ImagePath(string mealId)
    {
        _imageFiles ??= ReadManifest();
        if (_imageFiles.TryGetValue(mealId, out var file))
        {
            var path = Path.Combine(ImageFolder, file);
            if (File.Exists(path)) return path;
        }
        if (!Directory.Exists(ImageFolder)) return null;
        return Directory.EnumerateFiles(ImageFolder, mealId + "_*.jpg").FirstOrDefault();
    }

    private static Dictionary<string, string> ReadManifest()
    {
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var path = Path.Combine(ImageFolder, "manifest.json");
        if (!File.Exists(path)) return map;
        try
        {
            using var doc = JsonDocument.Parse(File.ReadAllText(path));
            foreach (var entry in doc.RootElement.EnumerateArray())
            {
                if (entry.TryGetProperty("mealId", out var id) && entry.TryGetProperty("image", out var image))
                    map[id.GetString() ?? ""] = image.GetString() ?? "";
            }
        }
        catch (JsonException) { /* Bad manifest: fall back to file names. */ }
        return map;
    }
}
