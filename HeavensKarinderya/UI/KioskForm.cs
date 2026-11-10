using HeavensKarinderya.Models;
using HeavensKarinderya.Services;

namespace HeavensKarinderya.UI;

public partial class KioskForm : Form
{
    List<MenuMeal> _catalog = new();
    readonly Dictionary<string, OrderItem> _cart = new();

    public KioskForm()
    {
        InitializeComponent();
    }

    private void KioskForm_Load(object? sender, EventArgs e)
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
            var button = new Button { Text = category, Size = new Size(180, 55) };
            button.Click += (_, _) => ShowCategory(category);
            flpCategories.Controls.Add(button);
        }
        ShowCategory("All");
    }

    void ShowCategory(string category)
    {
        flpMeals.Controls.Clear();
        foreach (var meal in _catalog.Where(m => category == "All" || m.Category == category))
        {
            var button = new Button
            {
                Text = $"{meal.Icon} {meal.Name}\n{meal.Description}\n₱{meal.Price:N2}",
                Size = new Size(220, 120)
            };
            button.Click += (_, _) => AddMeal(meal);
            flpMeals.Controls.Add(button);
        }
    }

    void AddMeal(MenuMeal meal)
    {
        if (_cart.TryGetValue(meal.Id, out var item))
            item.Quantity++;
        else
            _cart[meal.Id] = new OrderItem
            {
                MealId = meal.Id,
                Name = meal.Name,
                UnitPrice = meal.Price,
                Quantity = 1
            };
        RefreshCart();
    }

    void RefreshCart()
    {
        var selected = lstCart.SelectedIndex;
        lstCart.Items.Clear();
        foreach (var item in _cart.Values)
            lstCart.Items.Add($"{item.Quantity}x {item.Name}  ₱{item.LineTotal:N2}");
        if (selected >= 0 && selected < lstCart.Items.Count)
            lstCart.SelectedIndex = selected;
        lblTotal.Text = $"Total: ₱{_cart.Values.Sum(i => i.LineTotal):N2}";
    }

    void ChangeSelected(int change)
    {
        if (lstCart.SelectedIndex < 0) return;
        var item = _cart.Values.ElementAt(lstCart.SelectedIndex);
        item.Quantity += change;
        if (item.Quantity <= 0) _cart.Remove(item.MealId);
        RefreshCart();
    }

    private void btnMinus_Click(object? sender, EventArgs e)
    {
        ChangeSelected(-1);
    }

    private void btnPlus_Click(object? sender, EventArgs e)
    {
        ChangeSelected(1);
    }

    private void btnRemove_Click(object? sender, EventArgs e)
    {
        if (lstCart.SelectedIndex < 0) return;
        _cart.Remove(_cart.Values.ElementAt(lstCart.SelectedIndex).MealId);
        RefreshCart();
    }

    private void btnPlaceOrder_Click(object? sender, EventArgs e)
    {
        if (_cart.Count == 0)
        {
            MessageBox.Show("Please add at least one meal first.", "Empty Order");
            return;
        }

        try
        {
            var order = TransactionStore.PlaceOrder(_cart.Values.ToList());
            MessageBox.Show(
                $"ORDER PLACED!\n\nYour number is #{order.Number}\n\nTotal: ₱{order.Total:N2}\n\nPlease watch the queue monitor.",
                "Order Confirmed");
            _cart.Clear();
            RefreshCart();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Order Error");
        }
    }

    private void lstCart_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}