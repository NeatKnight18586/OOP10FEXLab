using System.Drawing;
using HeavensKarinderya.Models;
using HeavensKarinderya.Services;

namespace HeavensKarinderya.UI;

// Customer-facing queue board. F9 opens the staff controls.
public sealed partial class QueueMonitorForm : Form
{
    private static readonly Color Maroon = Color.FromArgb(106, 32, 58);
    private static readonly Color Cream = Color.FromArgb(248, 236, 213);
    private static readonly Color Orange = Color.FromArgb(255, 129, 12);
    private static readonly Color Black = Color.FromArgb(13, 13, 13);
    private static readonly Color LightText = Color.FromArgb(255, 231, 208);

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

        InitializeComponent();
        _callNextButton.Click += (_, _) => MoveOrder(TransactionStore.CallNext, "Preparing");
        _completeButton.Click += (_, _) => MoveOrder(TransactionStore.CompleteOldest, "Now Serving");
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
