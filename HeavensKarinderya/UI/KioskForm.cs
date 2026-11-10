using HeavensKarinderya.Models;
using HeavensKarinderya.Services;

namespace HeavensKarinderya.UI;

public class KioskForm : Form
{
    static readonly Color Gold = Color.FromArgb(212, 160, 23);

    readonly FlowLayoutPanel _sidebar = new()
    {
        Dock = DockStyle.Left,
        Width = 210,
        BackColor = Color.White,
        FlowDirection = FlowDirection.TopDown,
        WrapContents = false,
        Padding = new Padding(10)
    };
    readonly FlowLayoutPanel _meals = new()
    {
        Dock = DockStyle.Fill,
        AutoScroll = true,
        Padding = new Padding(12)
    };
    List<MenuMeal> _catalog = new();

    public KioskForm()
    {
        Text = "Heaven's Karinderya - Kiosk";
        ClientSize = new Size(1200, 800);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Theme.Cream;

        var header = new Label
        {
            Text = "Heaven's Karinderya",
            Dock = DockStyle.Top,
            Height = 80,
            BackColor = Theme.Red,
            ForeColor = Gold,
            Font = new Font("Georgia", 28, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter
        };
        var cart = new Panel { Dock = DockStyle.Right, Width = 340, BackColor = Color.White };

        Controls.Add(_meals);
        Controls.Add(_sidebar);
        Controls.Add(cart);
        Controls.Add(header);

        Shown += (_, _) => LoadMeals();
    }

    void LoadMeals()
    {
        try { _catalog = MenuCatalog.Load(); }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Menu Error");
            return;
        }

        var names = new[] { "All" }.Concat(_catalog.Select(m => m.Category).Distinct());
        foreach (var name in names)
        {
            var category = name;
            var button = Theme.Action(category, Theme.Ink);
            button.Size = new Size(185, 60);
            button.Margin = new Padding(0, 0, 0, 8);
            Theme.Flat(button);
            button.Click += (_, _) => ShowCategory(category);
            _sidebar.Controls.Add(button);
        }
        ShowCategory("All");
    }

    void ShowCategory(string category)
    {
        _meals.Controls.Clear();
        foreach (var meal in _catalog.Where(m => category == "All" || m.Category == category))
        {
            _meals.Controls.Add(new Label
            {
                Text = $"{meal.Icon} {meal.Name}  -  {meal.Price}",
                AutoSize = true,
                Font = new Font("Segoe UI", 14),
                Margin = new Padding(10)
            });
        }
    }
}