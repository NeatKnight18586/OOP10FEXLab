using HeavensKarinderya.Models;
using HeavensKarinderya.Services;

namespace HeavensKarinderya.UI;

// Member 1: customer-facing queue board.
public sealed class QueueMonitorForm : Form
{
    private readonly FlowLayoutPanel _serving = new();
    private readonly FlowLayoutPanel _preparing = new();
    private readonly Label _clock = Theme.Text("", 13, Color.White, FontStyle.Bold);
    private readonly Label _servingCount = Theme.Text("", 13, Theme.PaleGold);
    private readonly Label _preparingCount = Theme.Text("", 13, Theme.RedDark);
    private readonly Label _status = Theme.Text("Waiting for orders", 10, Theme.Muted);
    private readonly FlowLayoutPanel _staff = new();
    private readonly System.Windows.Forms.Timer _timer = new() { Interval = 850 };
    private long _revision = -1;

    public QueueMonitorForm()
    {
        Text = "Heaven's Karinderya — Queue Monitor";
        ClientSize = new Size(1366, 768);
        MinimumSize = new Size(1080, 640);
        WindowState = FormWindowState.Maximized;
        BackColor = Theme.Cream;
        KeyPreview = true;
        MakeLayout();
        KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.F9) _staff.Visible = !_staff.Visible;
            if (e.KeyCode == Keys.F11) FormBorderStyle =
                FormBorderStyle == FormBorderStyle.None ? FormBorderStyle.Sizable : FormBorderStyle.None;
        };
        _timer.Tick += (_, _) => RefreshOrders();
        Shown += (_, _) => { RefreshOrders(); _timer.Start(); };
        FormClosed += (_, _) => _timer.Dispose();
    }

    private void MakeLayout()
    {
        var root = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 3, ColumnCount = 1 };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 110));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 80));

        var header = new TableLayoutPanel { Dock = DockStyle.Fill, BackColor = Theme.Red, ColumnCount = 2, Padding = new Padding(25, 8, 25, 8) };
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 74));
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 26));
        var branding = Theme.Text("HEAVEN'S KARINDERYA", 27, Color.White, FontStyle.Bold);
        branding.Dock = DockStyle.Fill;
        _clock.Dock = DockStyle.Fill;
        _clock.TextAlign = ContentAlignment.MiddleRight;
        header.Controls.Add(branding, 0, 0);
        header.Controls.Add(_clock, 1, 0);

        var sections = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 };
        sections.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        sections.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        sections.Controls.Add(MakeSection("NOW SERVING", _servingCount, _serving, Theme.Ink, Theme.Gold), 0, 0);
        sections.Controls.Add(MakeSection("PREPARING", _preparingCount, _preparing, Theme.Cream, Theme.Ink), 1, 0);

        var footer = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(20, 14, 20, 10) };
        _status.Dock = DockStyle.Left;
        _status.Width = 450;
        _staff.Dock = DockStyle.Right;
        _staff.Width = 410;
        _staff.FlowDirection = FlowDirection.RightToLeft;
        _staff.WrapContents = false;
        _staff.Visible = false;
        var complete = Theme.Action("Complete Oldest", Theme.Green);
        var call = Theme.Action("Call Next", Theme.Red);
        foreach (var button in new[] { complete, call })
        {
            button.Width = 190;
            button.Margin = new Padding(5, 0, 5, 0);
            Theme.Flat(button);
            _staff.Controls.Add(button);
        }
        call.Click += (_, _) => Change(TransactionStore.CallNext);
        complete.Click += (_, _) => Change(TransactionStore.CompleteOldest);
        footer.Controls.Add(_staff);
        footer.Controls.Add(_status);

        root.Controls.Add(header, 0, 0);
        root.Controls.Add(sections, 0, 1);
        root.Controls.Add(footer, 0, 2);
        Controls.Add(root);
    }

    private static Panel MakeSection(string title, Label count, FlowLayoutPanel cards, Color background, Color titleColor)
    {
        var panel = new Panel { Dock = DockStyle.Fill, BackColor = background, Padding = new Padding(20) };
        var top = new TableLayoutPanel { Dock = DockStyle.Top, Height = 83, RowCount = 2 };
        top.RowStyles.Add(new RowStyle(SizeType.Percent, 67));
        top.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
        var heading = Theme.Text(title, 27, titleColor, FontStyle.Bold);
        heading.Dock = DockStyle.Fill;
        count.Dock = DockStyle.Fill;
        top.Controls.Add(heading, 0, 0);
        top.Controls.Add(count, 0, 1);
        cards.Dock = DockStyle.Fill;
        cards.AutoScroll = true;
        cards.WrapContents = true;
        cards.Padding = new Padding(6, 12, 6, 12);
        cards.BackColor = background;
        panel.Controls.Add(cards);
        panel.Controls.Add(top);
        return panel;
    }

    private void RefreshOrders()
    {
        _clock.Text = DateTime.Now.ToString("ddd, MMM d   h:mm:ss tt");
        try
        {
            var snapshot = TransactionStore.Snapshot();
            if (snapshot.Revision == _revision) return;
            _revision = snapshot.Revision;
            var serving = snapshot.Orders.Where(x => x.Status == "Serving").OrderByDescending(x => x.Number).ToList();
            var preparing = snapshot.Orders.Where(x => x.Status == "Preparing").OrderBy(x => x.Number).ToList();
            _servingCount.Text = $"{serving.Count} order(s)";
            _preparingCount.Text = $"{preparing.Count} order(s)";
            DrawCards(_serving, serving, true);
            DrawCards(_preparing, preparing, false);
            _status.Text = "LIVE • Automatic updates • F9 staff controls • F11 fullscreen";
        }
        catch (Exception ex) { _status.Text = "Queue record error: " + ex.Message; }
    }

    private static void DrawCards(FlowLayoutPanel target, List<OrderRecord> orders, bool serving)
    {
        target.SuspendLayout();
        target.Controls.Clear();
        if (orders.Count == 0)
        {
            var empty = Theme.Text(serving ? "No orders being served" : "No orders waiting", 17,
                serving ? Theme.PaleGold : Theme.Muted);
            empty.Width = 420;
            empty.Height = 100;
            target.Controls.Add(empty);
        }
        foreach (var order in orders)
        {
            var card = new Panel { Width = 200, Height = 155, Margin = new Padding(10),
                BackColor = serving ? Theme.Gold : Color.White };
            var number = Theme.Text($"#{order.Number}", 38, Theme.Ink, FontStyle.Bold);
            number.Dock = DockStyle.Top;
            number.Height = 100;
            number.TextAlign = ContentAlignment.MiddleCenter;
            var state = Theme.Text(serving ? "PLEASE CLAIM" : "PREPARING", 13,
                serving ? Theme.RedDark : Theme.Green, FontStyle.Bold);
            state.Dock = DockStyle.Fill;
            state.TextAlign = ContentAlignment.TopCenter;
            card.Controls.Add(state);
            card.Controls.Add(number);
            target.Controls.Add(card);
        }
        target.ResumeLayout();
    }

    private void Change(Func<int?> action)
    {
        try
        {
            var number = action();
            if (number is null) MessageBox.Show("No matching orders.");
            _revision = -1;
            RefreshOrders();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Queue Error"); }
    }
}
