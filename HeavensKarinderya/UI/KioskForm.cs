using HeavensKarinderya.Models;
using HeavensKarinderya.Services;

namespace HeavensKarinderya.UI;

public partial class KioskForm : Form
{
    private List<MenuMeal> _catalog = new();
    private readonly Dictionary<string, OrderItem> _cart = new();
    private readonly Dictionary<string, Image?> _photos = new();
    private static readonly Size PhotoSize = new(230, 130);

    public KioskForm()
    {
        InitializeComponent();
        _cboMeals.SelectedIndexChanged += (_, _) =>
            _addOrder.Enabled = _cboMeals.SelectedItem is MenuMeal;
        _addOrder.Click += (_, _) => AddSelectedMeal();
        _placeOrder.Click += (_, _) => PlaceOrder();
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

        foreach (var button in _categoryButtons)
        {
            var category = button.Tag?.ToString() ?? "All";
            button.Click += (_, _) => ShowCategory(category);
        }
        for (int i = 0; i < _mealCards.Length; i++)
        {
            int index = i;
            _mealButtons[i].Click += (_, _) =>
            {
                if (index < _catalog.Count) SelectMeal(_catalog[index]);
            };
        }
        BindMealCards();
        _cboMeals.BeginUpdate();
        _cboMeals.Items.Clear();
        foreach (var meal in _catalog)
            _cboMeals.Items.Add(meal);
        _cboMeals.SelectedIndex = -1;
        _cboMeals.EndUpdate();
        ShowCategory("All");
    }

    void BindMealCards()
    {
        for (int i = 0; i < _mealCards.Length; i++)
        {
            var visible = i < _catalog.Count;
            _mealCards[i].Visible = visible;
            if (!visible) continue;
            var meal = _catalog[i];
            _mealNames[i].Text = meal.Name;
            _mealPrices[i].Text = Theme.Pesos(meal.Price);
            _mealDescriptions[i].Text = meal.Description;
            var photo = LoadPhoto(meal.Id);
            _photos[meal.Id] = photo;
            _mealPhotos[i].Image = photo;
            _mealPhotos[i].Visible = photo is not null;
            _mealIcons[i].Text = meal.Icon;
            _mealIcons[i].Visible = photo is null;
        }
    }

    void ShowCategory(string category)
    {
        for (int i = 0; i < _mealCards.Length; i++)
            _mealCards[i].Visible = i < _catalog.Count &&
                (category == "All" || _catalog[i].Category == category);
    }

    // Loads the photo without locking the file, cropped to fill the card's photo area.
    static Image? LoadPhoto(string mealId)
    {
        var path = MenuCatalog.ImagePath(mealId);
        if (path is null) return null;
        try
        {
            using var stream = File.OpenRead(path);
            using var source = Image.FromStream(stream);
            var scale = Math.Max(PhotoSize.Width / (float)source.Width, PhotoSize.Height / (float)source.Height);
            var crop = new SizeF(PhotoSize.Width / scale, PhotoSize.Height / scale);
            var from = new RectangleF((source.Width - crop.Width) / 2, (source.Height - crop.Height) / 2,
                crop.Width, crop.Height);

            var photo = new Bitmap(PhotoSize.Width, PhotoSize.Height);
            using var g = Graphics.FromImage(photo);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(source, new RectangleF(0, 0, PhotoSize.Width, PhotoSize.Height), from, GraphicsUnit.Pixel);
            return photo;
        }
        catch (Exception)
        {
            return null;
        }
    }

    // Meal card selects the ComboBox. Add Order is a separate action.
    void SelectMeal(MenuMeal meal)
    {
        _cboMeals.SelectedItem = _catalog.FirstOrDefault(m => m.Id == meal.Id);
        _cboMeals.Focus();
    }

    void AddSelectedMeal()
    {
        if (_cboMeals.SelectedItem is not MenuMeal meal)
        {
            MessageBox.Show("Please select a meal first.", "No Meal Selected");
            return;
        }
        AddMeal(meal);
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

    void ChangeQuantity(string mealId, int change)
    {
        var item = _cart[mealId];
        item.Quantity += change;
        if (item.Quantity <= 0) _cart.Remove(mealId);
        RefreshCart();
    }

    void RemoveMeal(string mealId)
    {
        _cart.Remove(mealId);
        RefreshCart();
    }

    void RefreshCart()
    {
        _cartLines.Controls.Clear();
        foreach (var item in _cart.Values)
            _cartLines.Controls.Add(MakeCartLine(item));
        _total.Text = "Total: " + Theme.Pesos(_cart.Values.Sum(i => i.LineTotal));
    }

    Panel MakeCartLine(OrderItem item)
    {
        var id = item.MealId;
        var line = new Panel { Size = new Size(300, 85), Margin = new Padding(0, 0, 0, 8) };

        var minus = new Button { Text = "-", Size = new Size(40, 32), Location = new Point(0, 48) };
        var qty = new Label
        {
            Text = item.Quantity.ToString(),
            Size = new Size(40, 32),
            Location = new Point(45, 48),
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter
        };
        var plus = new Button { Text = "+", Size = new Size(40, 32), Location = new Point(90, 48) };
        var remove = new Button { Text = "Remove", Size = new Size(90, 32), Location = new Point(150, 48) };

        minus.Click += (_, _) => ChangeQuantity(id, -1);
        plus.Click += (_, _) => ChangeQuantity(id, 1);
        remove.Click += (_, _) => RemoveMeal(id);

        var text = new Label
        {
            Text = item.Name + "\n" + Theme.Pesos(item.LineTotal),
            Dock = DockStyle.Top,
            Height = 44,
            Font = new Font("Segoe UI", 10, FontStyle.Bold)
        };

        line.Controls.Add(minus);
        line.Controls.Add(qty);
        line.Controls.Add(plus);
        line.Controls.Add(remove);
        line.Controls.Add(text);
        return line;
    }

    void PlaceOrder()
    {
        if (_cart.Count == 0)
        {
            MessageBox.Show("Please add at least one meal first.", "Empty Order");
            return;
        }

        try
        {
            var customer = AskCustomerName();
            if (customer is null) return;
            var items = _cart.Values.ToList();
            if (!SaveToDatabase(items, customer)) return;
            var order = TransactionStore.PlaceOrder(items);
            ShowConfirmation(order);
            _cart.Clear();
            RefreshCart();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Order Error");
        }
    }

    // Saves the order to the instructor's transactions_tbl before the order number is issued.
    // Returns false when the customer should stay on the cart.
    bool _savedToDatabase;
    bool _nameSavedToDatabase;

    // Asks who the order is for. Returns null if the customer cancels.
    string? AskCustomerName()
    {
        using var dialog = new Form
        {
            Text = "Customer Name",
            ClientSize = new Size(440, 230),
            StartPosition = FormStartPosition.CenterParent,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox = false,
            MinimizeBox = false,
            BackColor = Theme.Cream
        };
        var prompt = new Label
        {
            Text = "Name for this order",
            Location = new Point(24, 20),
            Size = new Size(392, 36),
            Font = new Font("Segoe UI", 16, FontStyle.Bold)
        };
        var name = new TextBox
        {
            Location = new Point(24, 66),
            Size = new Size(392, 36),
            Font = new Font("Segoe UI", 16),
            MaxLength = 100
        };
        var ok = Theme.Action("CONFIRM ORDER", Theme.Green);
        ok.Location = new Point(24, 130);
        ok.Size = new Size(250, 60);
        Theme.Flat(ok);
        var cancel = Theme.Action("BACK", Theme.Ink);
        cancel.Location = new Point(290, 130);
        cancel.Size = new Size(126, 60);
        Theme.Flat(cancel);

        ok.Click += (_, _) =>
        {
            if (string.IsNullOrWhiteSpace(name.Text))
            {
                MessageBox.Show("Please type a name for the order.", "Name Needed");
                return;
            }
            dialog.DialogResult = DialogResult.OK;
        };
        cancel.DialogResult = DialogResult.Cancel;
        dialog.AcceptButton = ok;
        dialog.CancelButton = cancel;
        dialog.Controls.AddRange(new Control[] { prompt, name, ok, cancel });

        return dialog.ShowDialog(this) == DialogResult.OK ? name.Text.Trim() : null;
    }

    bool SaveToDatabase(List<OrderItem> items, string customer)
    {
        _savedToDatabase = false;
        _nameSavedToDatabase = false;
        while (true)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                _nameSavedToDatabase = DatabaseStore.SaveTransaction(items, customer);
                _savedToDatabase = true;
                return true;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                var choice = MessageBox.Show(
                    "This order could not be saved to the transaction database.\n\n" + ex.Message +
                    "\n\nRetry: try the database again." +
                    "\nIgnore: place the order on this PC only (not saved to the database)." +
                    "\nAbort: go back to the cart.",
                    "Database Error", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Warning);
                if (choice == DialogResult.Ignore) return true;
                if (choice != DialogResult.Retry) return false;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
    }

    void ShowConfirmation(OrderRecord order)
    {
        using var dialog = new Form
        {
            Text = "Order Confirmed",
            ClientSize = new Size(480, 520),
            StartPosition = FormStartPosition.CenterParent,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox = false,
            MinimizeBox = false,
            BackColor = Theme.Cream
        };

        var done = Theme.Action("DONE", Theme.Green);
        done.Dock = DockStyle.Bottom;
        done.Height = 65;
        Theme.Flat(done);
        done.Click += (_, _) => dialog.Close();

        var total = new Label
        {
            Text = "Total: " + Theme.Pesos(order.Total),
            Dock = DockStyle.Bottom,
            Height = 50,
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter
        };
        var items = new Label
        {
            Text = string.Join("\n", order.Items.Select(i => $"{i.Quantity}x {i.Name}"))
                   + "\n\nPlease watch the queue monitor.\n\n"
                   + (_savedToDatabase
                       ? "Saved to the transaction database" + (_nameSavedToDatabase ? "" : " (name not saved)")
                       : "NOT saved to the database (this PC only)"),
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 13),
            TextAlign = ContentAlignment.MiddleCenter
        };
        var number = new Label
        {
            Text = "#" + order.Number,
            Dock = DockStyle.Top,
            Height = 110,
            Font = new Font("Segoe UI", 54, FontStyle.Bold),
            ForeColor = Theme.Red,
            TextAlign = ContentAlignment.MiddleCenter
        };
        var heading = new Label
        {
            Text = "ORDER PLACED! Your number is:",
            Dock = DockStyle.Top,
            Height = 60,
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter
        };

        dialog.Controls.Add(items);
        dialog.Controls.Add(total);
        dialog.Controls.Add(done);
        dialog.Controls.Add(number);
        dialog.Controls.Add(heading);
        dialog.ShowDialog(this);
    }
}