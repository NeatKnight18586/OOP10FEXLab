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
    readonly ComboBox _cboMeals = new()
    {
        Name = "cboMeals",
        Dock = DockStyle.Fill,
        DropDownStyle = ComboBoxStyle.DropDownList,
        Font = new Font("Segoe UI", 11),
        DisplayMember = nameof(MenuMeal.Name),
        AccessibleName = "Selected meal"
    };
    readonly Button _addOrder = Theme.Action("ADD ORDER", Theme.Red);

    readonly Dictionary<string, OrderItem> _cart = new();
    readonly FlowLayoutPanel _cartLines = new()
    {
        Dock = DockStyle.Fill,
        FlowDirection = FlowDirection.TopDown,
        WrapContents = false,
        AutoScroll = true
    };
    readonly Label _total = new()
    {
        Text = "Total: " + Theme.Pesos(0),
        Dock = DockStyle.Bottom,
        Height = 55,
        Font = new Font("Segoe UI", 18, FontStyle.Bold),
        ForeColor = Theme.Red,
        TextAlign = ContentAlignment.MiddleCenter
    };

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
        var cart = new TableLayoutPanel
        {
            Dock = DockStyle.Right,
            Width = 360,
            BackColor = Color.White,
            Padding = new Padding(12),
            ColumnCount = 1,
            RowCount = 5
        };
        cart.RowStyles.Add(new RowStyle(SizeType.Absolute, 56));
        cart.RowStyles.Add(new RowStyle(SizeType.Absolute, 130));
        cart.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        cart.RowStyles.Add(new RowStyle(SizeType.Absolute, 65));
        cart.RowStyles.Add(new RowStyle(SizeType.Absolute, 68));

        var cartTitle = new Label
        {
            Text = "YOUR ORDER",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 18, FontStyle.Bold),
            ForeColor = Theme.Ink,
            TextAlign = ContentAlignment.MiddleCenter
        };

        var chooseMeal = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 3, Padding = new Padding(0, 0, 0, 8) };
        chooseMeal.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
        chooseMeal.RowStyles.Add(new RowStyle(SizeType.Absolute, 43));
        chooseMeal.RowStyles.Add(new RowStyle(SizeType.Absolute, 51));
        var comboLabel = new Label
        {
            Text = "CURRENT SELECTED ITEM",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 9, FontStyle.Bold),
            ForeColor = Theme.Ink
        };
        _addOrder.Dock = DockStyle.Fill;
        _addOrder.Enabled = false;
        Theme.Flat(_addOrder);
        _cboMeals.SelectedIndexChanged += (_, _) => _addOrder.Enabled = _cboMeals.SelectedItem is MenuMeal;
        _addOrder.Click += (_, _) => AddSelectedMeal();
        chooseMeal.Controls.Add(comboLabel, 0, 0);
        chooseMeal.Controls.Add(_cboMeals, 0, 1);
        chooseMeal.Controls.Add(_addOrder, 0, 2);

        var placeOrder = Theme.Action("PLACE ORDER", Theme.Green);
        placeOrder.Dock = DockStyle.Fill;
        Theme.Flat(placeOrder);
        placeOrder.Click += (_, _) => PlaceOrder();

        _cartLines.Dock = DockStyle.Fill;
        _total.Dock = DockStyle.Fill;
        cart.Controls.Add(cartTitle, 0, 0);
        cart.Controls.Add(chooseMeal, 0, 1);
        cart.Controls.Add(_cartLines, 0, 2);
        cart.Controls.Add(_total, 0, 3);
        cart.Controls.Add(placeOrder, 0, 4);

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
        _cboMeals.BeginUpdate();
        _cboMeals.Items.Clear();
        foreach (var meal in _catalog)
            _cboMeals.Items.Add(meal);
        _cboMeals.SelectedIndex = -1;
        _cboMeals.EndUpdate();
        ShowCategory("All");
    }

    void ShowCategory(string category)
    {
        _meals.Controls.Clear();
        foreach (var meal in _catalog.Where(m => category == "All" || m.Category == category))
            _meals.Controls.Add(MakeCard(meal));
    }

    Panel MakeCard(MenuMeal meal)
    {
        var card = new Panel
        {
            Size = new Size(230, 310),
            BackColor = Color.White,
            Margin = new Padding(10)
        };

        var add = Theme.Action("SELECT MEAL", Theme.Red);
        add.Dock = DockStyle.Bottom;
        add.Height = 45;
        Theme.Flat(add);
        add.Click += (_, _) => SelectMeal(meal);

        var price = new Label
        {
            Text = Theme.Pesos(meal.Price),
            Font = new Font("Segoe UI", 14, FontStyle.Bold),
            ForeColor = Theme.Red,
            Dock = DockStyle.Top,
            Height = 32,
            TextAlign = ContentAlignment.MiddleCenter
        };
        var description = new Label
        {
            Text = meal.Description,
            Font = new Font("Segoe UI", 9),
            ForeColor = Color.Gray,
            Dock = DockStyle.Top,
            Height = 48,
            TextAlign = ContentAlignment.TopCenter
        };
        var name = new Label
        {
            Text = meal.Name,
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            Dock = DockStyle.Top,
            Height = 32,
            TextAlign = ContentAlignment.MiddleCenter
        };
        var icon = MakePhoto(meal);

        card.Controls.Add(add);
        card.Controls.Add(price);
        card.Controls.Add(description);
        card.Controls.Add(name);
        card.Controls.Add(icon);
        return card;
    }

    // Food photo from Data/Images. Keeps the emoji placeholder if the photo is missing.
    readonly Dictionary<string, Image?> _photos = new();
    static readonly Size PhotoSize = new(230, 130);

    Control MakePhoto(MenuMeal meal)
    {
        if (!_photos.TryGetValue(meal.Id, out var photo))
        {
            photo = LoadPhoto(meal.Id);
            _photos[meal.Id] = photo;
        }

        if (photo is null)
            return new Label
            {
                Text = meal.Icon,
                Font = new Font("Segoe UI Emoji", 36),
                Dock = DockStyle.Top,
                Height = PhotoSize.Height,
                TextAlign = ContentAlignment.MiddleCenter
            };

        return new PictureBox
        {
            Image = photo,
            Dock = DockStyle.Top,
            Height = PhotoSize.Height,
            SizeMode = PictureBoxSizeMode.StretchImage
        };
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
            var items = _cart.Values.ToList();
            if (!SaveToDatabase(items)) return;
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
    bool SaveToDatabase(List<OrderItem> items)
    {
        while (true)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                DatabaseStore.SaveTransaction(items);
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
                   + "\n\nPlease watch the queue monitor.",
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