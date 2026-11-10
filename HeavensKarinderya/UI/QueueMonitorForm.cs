using System.Drawing;
using HeavensKarinderya.Models;
using HeavensKarinderya.Services;

namespace HeavensKarinderya.UI;

// Customer-facing queue board. F9 opens the staff controls.
public sealed class QueueMonitorForm : Form
{
    private static readonly Color Maroon = Color.FromArgb(106, 32, 58);
    private static readonly Color Cream = Color.FromArgb(248, 236, 213);
    private static readonly Color Orange = Color.FromArgb(255, 129, 12);
    private static readonly Color Black = Color.FromArgb(13, 13, 13);
    private static readonly Color LightText = Color.FromArgb(255, 231, 208);

    private readonly Label _brand = LabelText("HEAVEN'S KARINDERYA", 29, LightText, true);
    private readonly Label _clock = LabelText("", 24, LightText, true);
    private readonly Label _preparingTitle = LabelText("PREPARING", 30, Black, true);
    private readonly Label _servingTitle = LabelText("NOW SERVING", 30, Black, true);
    private readonly Label _pageInfo = LabelText("", 10, Black, false);
    private readonly Label _servingPageInfo = LabelText("", 10, Black, false);
    private readonly Label _footer = LabelText("Pakihintay ang inyong numero sa Now Serving.", 17, LightText, true);
    private readonly Label _staffStatus = LabelText("F9 to hide staff controls", 11, Color.White, false);
    private readonly TableLayoutPanel _preparingNumbers = new();
    private readonly TableLayoutPanel _servingNumbers = new();
    private readonly TableLayoutPanel _root = new();
    private readonly TableLayoutPanel _staffControls = new();
    private readonly System.Windows.Forms.Timer _refreshTimer = new() { Interval = 900 };

    private List<OrderRecord> _preparing = new();
    private List<OrderRecord> _serving = new();
    private long _lastRevision = -1;
    private int _preparingPage;
    private int _servingPage;
    private DateTime _nextPageAt = DateTime.Now.AddSeconds(7);
    private bool _staffOpen;

    private const int PreparingPerPage = 8;
    private const int ServingPerPage = 2;

    public QueueMonitorForm()
    {
        Text = "Heaven's Karinderya - Queue Monitor";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(1366, 768);
        MinimumSize = new Size(950, 580);
        WindowState = FormWindowState.Maximized;
        BackColor = Cream;
        KeyPreview = true;
        DoubleBuffered = true;

        BuildLayout();
        KeyDown += HandleShortcut;
        Resize += (_, _) => ApplyResponsiveSizing();
        Shown += (_, _) =>
        {
            ApplyResponsiveSizing();
            UpdateOrders();
            _refreshTimer.Start();
        };
        _refreshTimer.Tick += (_, _) =>
        {
            _clock.Text = DateTime.Now.ToString("h:mm tt");
            UpdateOrders();
            if (DateTime.Now >= _nextPageAt)
            {
                _nextPageAt = DateTime.Now.AddSeconds(7);
                _preparingPage = (_preparingPage + 1) % PageCount(_preparing.Count, PreparingPerPage);
                _servingPage = (_servingPage + 1) % PageCount(_serving.Count, ServingPerPage);
                DrawNumbers();
            }
        };
        FormClosed += (_, _) => _refreshTimer.Dispose();
    }

    private static Label LabelText(string value, float size, Color foreground, bool bold)
    {
        return new Label
        {
            Text = value,
            Dock = DockStyle.Fill,
            ForeColor = foreground,
            BackColor = Color.Transparent,
            TextAlign = ContentAlignment.MiddleCenter,
            AutoEllipsis = false,
            Font = new Font("Segoe UI", size, bold ? FontStyle.Bold : FontStyle.Regular),
            Margin = Padding.Empty
        };
    }

    private static Panel MakeRule()
    {
        return new Panel { Dock = DockStyle.Fill, BackColor = Black, Margin = Padding.Empty };
    }

    private void BuildLayout()
    {
        _root.Dock = DockStyle.Fill;
        _root.ColumnCount = 1;
        _root.RowCount = 4;
        _root.Margin = Padding.Empty;
        _root.Padding = Padding.Empty;
        _root.RowStyles.Add(new RowStyle(SizeType.Absolute, 86));
        _root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        _root.RowStyles.Add(new RowStyle(SizeType.Absolute, 0));
        _root.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));

        var header = new TableLayoutPanel
        {
            Dock = DockStyle.Fill, BackColor = Maroon,
            ColumnCount = 2, Padding = new Padding(20, 0, 20, 0)
        };
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 72));
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28));
        _brand.TextAlign = ContentAlignment.MiddleLeft;
        _clock.TextAlign = ContentAlignment.MiddleRight;
        header.Controls.Add(_brand, 0, 0);
        header.Controls.Add(_clock, 1, 0);
        _root.Controls.Add(header, 0, 0);

        var body = new TableLayoutPanel
        {
            Dock = DockStyle.Fill, ColumnCount = 2,
            BackColor = Black, Margin = Padding.Empty,
            Padding = new Padding(0, 6, 0, 0)
        };
        body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65));
        body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35));
        body.Controls.Add(MakeSection(_preparingTitle, _pageInfo, _preparingNumbers, Cream), 0, 0);
        body.Controls.Add(MakeSection(_servingTitle, _servingPageInfo, _servingNumbers, Orange), 1, 0);
        _root.Controls.Add(body, 0, 1);

        _staffControls.Dock = DockStyle.Fill;
        _staffControls.BackColor = Color.FromArgb(45, 45, 45);
        _staffControls.ColumnCount = 3;
        _staffControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
        _staffControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
        _staffControls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
        _staffControls.Padding = new Padding(12, 8, 12, 8);
        _staffStatus.TextAlign = ContentAlignment.MiddleLeft;
        _staffControls.Controls.Add(_staffStatus, 0, 0);
        var call = MakeButton("CALL NEXT", Color.FromArgb(54, 121, 73));
        var complete = MakeButton("COMPLETE OLDEST", Maroon);
        call.Click += (_, _) => MoveOrder(TransactionStore.CallNext, "Preparing");
        complete.Click += (_, _) => MoveOrder(TransactionStore.CompleteOldest, "Now Serving");
        _staffControls.Controls.Add(call, 1, 0);
        _staffControls.Controls.Add(complete, 2, 0);
        _staffControls.Visible = false;
        _root.Controls.Add(_staffControls, 0, 2);

        var footerPanel = new Panel { Dock = DockStyle.Fill, BackColor = Black };
        footerPanel.Controls.Add(_footer);
        _root.Controls.Add(footerPanel, 0, 3);
        Controls.Add(_root);
    }

    private static Button MakeButton(string label, Color color)
    {
        var button = new Button
        {
            Text = label, Dock = DockStyle.Fill,
            BackColor = color, ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            Margin = new Padding(5, 0, 5, 0),
            Cursor = Cursors.Hand,
            UseVisualStyleBackColor = false
        };
        button.FlatAppearance.BorderSize = 0;
        return button;
    }

    private static Control MakeSection(Label title, Label pageInfo, TableLayoutPanel numbers, Color background)
    {
        var section = new TableLayoutPanel
        {
            Dock = DockStyle.Fill, BackColor = background,
            Margin = Padding.Empty, Padding = new Padding(20, 12, 20, 12),
            RowCount = 3
        };
        section.RowStyles.Add(new RowStyle(SizeType.Absolute, 72));
        section.RowStyles.Add(new RowStyle(SizeType.Absolute, 7));
        section.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var heading = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 };
        heading.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 78));
        heading.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22));
        title.TextAlign = ContentAlignment.MiddleLeft;
        pageInfo.TextAlign = ContentAlignment.BottomRight;
        heading.Controls.Add(title, 0, 0);
        heading.Controls.Add(pageInfo, 1, 0);
        section.Controls.Add(heading, 0, 0);
        section.Controls.Add(MakeRule(), 0, 1);

        numbers.Dock = DockStyle.Fill;
        numbers.BackColor = background;
        numbers.Margin = Padding.Empty;
        numbers.Padding = new Padding(0, 12, 0, 0);
        section.Controls.Add(numbers, 0, 2);
        return section;
    }

    private void HandleShortcut(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F9)
        {
            _staffOpen = !_staffOpen;
            _staffControls.Visible = _staffOpen;
            _root.RowStyles[2].Height = _staffOpen ? 68 : 0;
            ApplyResponsiveSizing();
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.F11)
        {
            FormBorderStyle = FormBorderStyle == FormBorderStyle.None
                ? FormBorderStyle.Sizable : FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            e.Handled = true;
        }
    }

    private void ApplyResponsiveSizing()
    {
        // Scale the layout with the available client area, not the monitor's DPI.
        var scale = Math.Clamp(Math.Min(ClientSize.Width / 1366f, ClientSize.Height / 768f), .72f, 1.65f);
        _root.RowStyles[0].Height = 86 * scale;
        _root.RowStyles[3].Height = 48 * scale;
        if (_staffOpen) _root.RowStyles[2].Height = 68 * scale;
        _brand.Font = new Font("Segoe UI", 29 * scale, FontStyle.Bold);
        _clock.Font = new Font("Segoe UI", 24 * scale, FontStyle.Bold);
        _preparingTitle.Font = new Font("Segoe UI", 29 * scale, FontStyle.Bold);
        _servingTitle.Font = new Font("Segoe UI", 29 * scale, FontStyle.Bold);
        _footer.Font = new Font("Segoe UI", 17 * scale, FontStyle.Bold);
        _pageInfo.Font = new Font("Segoe UI", 10 * scale);
        _servingPageInfo.Font = new Font("Segoe UI", 10 * scale);
        DrawNumbers();
    }

    private void UpdateOrders()
    {
        try
        {
            var snapshot = TransactionStore.Snapshot();
            if (snapshot.Revision == _lastRevision) return;

            _preparing = snapshot.Orders
                .Where(o => o.Status == "Preparing")
                .OrderBy(o => o.Number).ToList();
            _serving = snapshot.Orders
                .Where(o => o.Status == "Serving")
                .OrderByDescending(o => o.Number).ToList();

            _lastRevision = snapshot.Revision;
            _preparingPage = 0;
            _servingPage = 0;
            _nextPageAt = DateTime.Now.AddSeconds(7);
            _staffStatus.Text = "F9 to hide staff controls";
            _footer.Text = "Pakihintay ang inyong numero sa Now Serving.";
            DrawNumbers();
        }
        catch (Exception ex)
        {
            _staffStatus.Text = "Queue error: " + ex.Message;
            _footer.Text = "Unable to load the queue. Please ask our staff.";
        }
    }

    private static int PageCount(int total, int pageSize) =>
        Math.Max(1, (total + pageSize - 1) / pageSize);

    private void DrawNumbers()
    {
        DrawGrid(_preparingNumbers, _preparing, _preparingPage,
            PreparingPerPage, 2, 4, false, "Wala pang naghihintay.");
        DrawGrid(_servingNumbers, _serving, _servingPage,
            ServingPerPage, 1, 2, true, "Wala pang handang order.");
        _pageInfo.Text = PageCount(_preparing.Count, PreparingPerPage) > 1
            ? $"{_preparingPage + 1}/{PageCount(_preparing.Count, PreparingPerPage)}" : "";
        _servingPageInfo.Text = PageCount(_serving.Count, ServingPerPage) > 1
            ? $"{_servingPage + 1}/{PageCount(_serving.Count, ServingPerPage)}" : "";
    }

    private static void DrawGrid(TableLayoutPanel grid, List<OrderRecord> orders,
        int page, int pageSize, int columns, int rows, bool serving, string emptyMessage)
    {
        grid.SuspendLayout();
        try
        {
            grid.Controls.Clear();
            grid.ColumnStyles.Clear();
            grid.RowStyles.Clear();
            grid.ColumnCount = columns;
            grid.RowCount = rows;
            for (int c = 0; c < columns; c++)
                grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / columns));
            for (int r = 0; r < rows; r++)
                grid.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / rows));

            if (orders.Count == 0)
            {
                var empty = LabelText(emptyMessage, 23, Black, false);
                grid.Controls.Add(empty, 0, 0);
                grid.SetColumnSpan(empty, columns);
                grid.SetRowSpan(empty, rows);
                return;
            }

            var visible = orders.Skip(page * pageSize).Take(pageSize).ToArray();
            var cellWidth = Math.Max(120f, (grid.ClientSize.Width - grid.Padding.Horizontal) / (float)columns);
            var cellHeight = Math.Max(100f, (grid.ClientSize.Height - grid.Padding.Vertical) / (float)rows);

            for (int i = 0; i < visible.Length; i++)
            {
                string number = visible[i].Number.ToString("D3");
                var byWidth = cellWidth / (number.Length * .67f);
                var byHeight = cellHeight * (serving ? .72f : .63f);
                var size = Math.Clamp(Math.Min(byWidth, byHeight), 30f, serving ? 125f : 76f);
                var text = LabelText(number, size, Black, true);
                text.Margin = Padding.Empty;
                grid.Controls.Add(text, i / rows, i % rows);
            }
        }
        finally { grid.ResumeLayout(true); }
    }

    private void MoveOrder(Func<int?> action, string from)
    {
        try
        {
            var number = action();
            _staffStatus.Text = number.HasValue
                ? $"Order {number.Value:D3} updated."
                : $"No orders in {from}.";
            _lastRevision = -1;
            UpdateOrders();
        }
        catch (Exception ex) { _staffStatus.Text = "Could not update: " + ex.Message; }
    }
}
