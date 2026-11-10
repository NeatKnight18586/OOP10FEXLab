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
        _preparingSlots = new Label[] { preparingNumber1, preparingNumber2, preparingNumber3, preparingNumber4, preparingNumber5, preparingNumber6, preparingNumber7, preparingNumber8 };
        _servingSlots = new Label[] { servingNumber1, servingNumber2 };
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
        DrawGrid(_preparingNumbers, _preparingSlots, _preparing, _preparingPage,
            PreparingPerPage, false, "Wala pang naghihintay.");
        DrawGrid(_servingNumbers, _servingSlots, _serving, _servingPage,
            ServingPerPage, true, "Wala pang handang order.");
        _pageInfo.Text = PageCount(_preparing.Count, PreparingPerPage) > 1
            ? $"{_preparingPage + 1}/{PageCount(_preparing.Count, PreparingPerPage)}" : "";
        _servingPageInfo.Text = PageCount(_serving.Count, ServingPerPage) > 1
            ? $"{_servingPage + 1}/{PageCount(_serving.Count, ServingPerPage)}" : "";
    }

    private void DrawGrid(TableLayoutPanel grid, Label[] slots,
        List<OrderRecord> orders, int page, int pageSize, bool serving, string emptyMessage)
    {
        var visible = orders.Skip(page * pageSize).Take(pageSize).ToArray();
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Text = i < visible.Length ? visible[i].Number.ToString("D3") : "";
            slots[i].Visible = true;
        }
        // The empty message uses the first designer slot rather than a runtime control.
        if (orders.Count == 0)
        {
            slots[0].Text = emptyMessage;
            slots[0].Font = new Font("Segoe UI", 20F);
        }
        else
        {
            var cellWidth = Math.Max(120f, grid.ClientSize.Width / (float)(serving ? 1 : 2));
            var cellHeight = Math.Max(100f, grid.ClientSize.Height / (float)(serving ? 2 : 4));
            var size = Math.Clamp(Math.Min(cellWidth / (3f * .67f),
                cellHeight * (serving ? .72f : .63f)), 30f, serving ? 125f : 76f);
            foreach (var slot in slots)
            {
                if (Math.Abs(slot.Font.Size - size) > 1)
                {
                    var old = slot.Font;
                    slot.Font = new Font("Segoe UI", size, FontStyle.Bold);
                    old.Dispose();
                }
            }
        }
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
