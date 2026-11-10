using HeavensKarinderya.Models;
using HeavensKarinderya.Services;

namespace HeavensKarinderya.UI;

// Starter kiosk screen. Member 2 will finish the menu design and cart controls.
public sealed class KioskForm : Form
{
    private readonly FlowLayoutPanel _meals = new() { Dock = DockStyle.Fill, AutoScroll = true, WrapContents = true, Padding = new Padding(12) };
    private readonly FlowLayoutPanel _categories = new() { Dock = DockStyle.Fill, AutoScroll = true, WrapContents = false };
    private readonly ListBox _cartList = new() { Dock = DockStyle.Fill, Font = Theme.Font(12) };
    private readonly Label _total = Theme.Text("Total: ₱0.00", 20, Theme.Red, FontStyle.Bold);
    private readonly Dictionary<string, OrderItem> _cart = new();
    private List<MenuMeal> _catalog = new();

    public KioskForm()
    {
        Text = "Heaven's Karinderya — Ordering Kiosk";
        MinimumSize = new Size(900, 650);
        ClientSize = new Size(1200, 800);
        BackColor = Theme.Cream;
        StartPosition = FormStartPosition.CenterScreen;
        MakeLayout();
        Shown += (_, _) => LoadMeals();
    }

    private void MakeLayout()
    {
        var root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 2 };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 95));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var title = Theme.Text("HEAVEN'S KARINDERYA  •  ORDER HERE", 24, Color.White, FontStyle.Bold);
        title.Dock = DockStyle.Fill;
        title.Padding = new Padding(24, 0, 0, 0);
        title.BackColor = Theme.Red;
        root.Controls.Add(title, 0, 0);
        root.SetColumnSpan(title, 2);

        var left = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2, Padding = new Padding(12) };
        left.RowStyles.Add(new RowStyle(SizeType.Absolute, 65));
        left.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        left.Controls.Add(_categories, 0, 0);
        left.Controls.Add(_meals, 0, 1);
        root.Controls.Add(left, 0, 1);

        var right = new TableLayoutPanel
        {
            Dock = DockStyle.Fill, RowCount = 4,
            Padding = new Padding(18), BackColor = Color.White
        };
        right.RowStyles.Add(new RowStyle(SizeType.Absolute, 65));
        right.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        right.RowStyles.Add(new RowStyle(SizeType.Absolute, 65));
        right.RowStyles.Add(new RowStyle(SizeType.Absolute, 65));
        var heading = Theme.Text("YOUR ORDER", 21, Theme.Ink, FontStyle.Bold);
        heading.Dock = DockStyle.Fill;
        _total.Dock = DockStyle.Fill;
        var checkout = Theme.Action("PLACE ORDER", Theme.Green);
        checkout.Dock = DockStyle.Fill;
        Theme.Flat(checkout);
        checkout.Click += (_, _) => PlaceOrder();
        right.Controls.Add(heading, 0, 0);
        right.Controls.Add(_cartList, 0, 1);
        right.Controls.Add(_total, 0, 2);
        right.Controls.Add(checkout, 0, 3);
        root.Controls.Add(right, 1, 1);
        Controls.Add(root);
    }

    private void LoadMeals()
    {
        try { _catalog = MenuCatalog.Load(); }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Menu Error"); return; }

        _categories.Controls.Clear();
        foreach (var category in new[] { "All" }.Concat(_catalog.Select(m => m.Category).Distinct()))
        {
            var selected = category;
            var button = Theme.Action(selected, Theme.Ink);
            button.AutoSize = true;
            button.Padding = new Padding(12, 0, 12, 0);
            button.Margin = new Padding(4);
            Theme.Flat(button);
            button.Click += (_, _) => ShowCategory(selected);
            _categories.Controls.Add(button);
        }
        ShowCategory("All");
    }

    private void ShowCategory(string category)
    {
        _meals.SuspendLayout();
        _meals.Controls.Clear();
        foreach (var meal in _catalog.Where(x => category == "All" || x.Category == category))
        {
            var tile = new TableLayoutPanel
            {
                Width = 235, Height = 200, RowCount = 3,
                BackColor = Color.White, Padding = new Padding(12), Margin = new Padding(10)
            };
            tile.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            tile.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
            tile.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));

            var name = Theme.Text($"{meal.Icon}  {meal.Name}", 12, Theme.Ink, FontStyle.Bold);
            name.Dock = DockStyle.Fill;
            var price = Theme.Text(Theme.Pesos(meal.Price), 14, Theme.Red, FontStyle.Bold);
            price.Dock = DockStyle.Fill;
            var add = Theme.Action("ADD TO ORDER", Theme.Red);
            add.Dock = DockStyle.Fill;
            Theme.Flat(add);
            add.Click += (_, _) => AddMeal(meal);
            tile.Controls.Add(name, 0, 0);
            tile.Controls.Add(price, 0, 1);
            tile.Controls.Add(add, 0, 2);
            _meals.Controls.Add(tile);
        }
        _meals.ResumeLayout();
    }

    private void AddMeal(MenuMeal meal)
    {
        if (_cart.TryGetValue(meal.Id, out var item)) item.Quantity++;
        else _cart[meal.Id] = new OrderItem
        {
            MealId = meal.Id, Name = meal.Name, UnitPrice = meal.Price, Quantity = 1
        };
        RefreshCart();
    }

    private void RefreshCart()
    {
        _cartList.Items.Clear();
        foreach (var item in _cart.Values)
            _cartList.Items.Add($"{item.Quantity}x {item.Name} — {Theme.Pesos(item.LineTotal)}");
        _total.Text = $"Total: {Theme.Pesos(_cart.Values.Sum(i => i.LineTotal))}";
    }

    private void PlaceOrder()
    {
        if (_cart.Count == 0) { MessageBox.Show("Select a meal first."); return; }
        try
        {
            var order = TransactionStore.PlaceOrder(_cart.Values.ToList());
            MessageBox.Show($"Order #{order.Number} placed. Please watch the queue monitor.", "Order Confirmed");
            _cart.Clear();
            RefreshCart();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Order Error"); }
    }
}
