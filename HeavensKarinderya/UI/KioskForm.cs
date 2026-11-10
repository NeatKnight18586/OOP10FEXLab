using HeavensKarinderya.Models;
using HeavensKarinderya.Services;

namespace HeavensKarinderya.UI;

<<<<<<< HEAD
public partial class KioskForm : Form
{
    List<MenuMeal> _catalog = new();
    readonly Dictionary<string, OrderItem> _cart = new();

    public KioskForm()
    {
        InitializeComponent();
    }

    private void KioskForm_Load(object? sender, EventArgs e)
=======
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
        var cart = new Panel
        {
            Dock = DockStyle.Right,
            Width = 340,
            BackColor = Color.White,
            Padding = new Padding(10)
        };
        var cartTitle = new Label
        {
            Text = "YOUR ORDER",
            Dock = DockStyle.Top,
            Height = 50,
            Font = new Font("Segoe UI", 18, FontStyle.Bold),
            ForeColor = Theme.Ink,
            TextAlign = ContentAlignment.MiddleCenter
        };
        cart.Controls.Add(_cartLines);
        cart.Controls.Add(_total);
        var placeOrder = Theme.Action("PLACE ORDER", Theme.Green);
        placeOrder.Dock = DockStyle.Bottom;
        placeOrder.Height = 65;
        Theme.Flat(placeOrder);
        placeOrder.Click += (_, _) => PlaceOrder();
        cart.Controls.Add(placeOrder);
        cart.Controls.Add(cartTitle);

        Controls.Add(_meals);
        Controls.Add(_sidebar);
        Controls.Add(cart);
        Controls.Add(header);

        Shown += (_, _) => LoadMeals();
    }

    void LoadMeals()
>>>>>>> origin/main
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
<<<<<<< HEAD
            var button = new Button { Text = category, Size = new Size(180, 55) };
            button.Click += (_, _) => ShowCategory(category);
            flpCategories.Controls.Add(button);
=======
            var button = Theme.Action(category, Theme.Ink);
            button.Size = new Size(185, 60);
            button.Margin = new Padding(0, 0, 0, 8);
            Theme.Flat(button);
            button.Click += (_, _) => ShowCategory(category);
            _sidebar.Controls.Add(button);
>>>>>>> origin/main
        }
        ShowCategory("All");
    }

    void ShowCategory(string category)
    {
<<<<<<< HEAD
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
=======
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

        var add = Theme.Action("ADD TO ORDER", Theme.Red);
        add.Dock = DockStyle.Bottom;
        add.Height = 45;
        Theme.Flat(add);
        add.Click += (_, _) => AddMeal(meal);

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

    void AddMeal(MenuMeal meal)
    {
>>>>>>> origin/main
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

<<<<<<< HEAD
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
=======
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
>>>>>>> origin/main
    {
        if (_cart.Count == 0)
        {
            MessageBox.Show("Please add at least one meal first.", "Empty Order");
            return;
        }

        try
        {
<<<<<<< HEAD
            var order = TransactionStore.PlaceOrder(_cart.Values.ToList());
            MessageBox.Show(
                $"ORDER PLACED!\n\nYour number is #{order.Number}\n\nTotal: ₱{order.Total:N2}\n\nPlease watch the queue monitor.",
                "Order Confirmed");
=======
            var items = _cart.Values.ToList();
            if (!SaveToDatabase(items)) return;
            var order = TransactionStore.PlaceOrder(items);
            ShowConfirmation(order);
>>>>>>> origin/main
            _cart.Clear();
            RefreshCart();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Order Error");
        }
    }

<<<<<<< HEAD
    private void lstCart_SelectedIndexChanged(object sender, EventArgs e)
    {

=======
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
>>>>>>> origin/main
    }
}