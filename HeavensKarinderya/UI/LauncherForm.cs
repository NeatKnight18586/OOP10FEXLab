using System.Diagnostics;
using HeavensKarinderya.Services;

namespace HeavensKarinderya.UI;

public sealed class LauncherForm : Form
{
    private KioskForm? _kiosk;
    private QueueMonitorForm? _monitor;

    public LauncherForm()
    {
        Text = "Heaven's Karinderya — Launcher";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(820, 510);
        MinimumSize = new Size(780, 480);
        BackColor = Theme.Cream;

        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(40), RowCount = 5 };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 90));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var title = Theme.Text("HEAVEN'S KARINDERYA", 26, Theme.Red, FontStyle.Bold);
        title.Dock = DockStyle.Fill;
        var both = Theme.Action("OPEN BOTH SCREENS", Theme.Red);
        var kiosk = Theme.Action("OPEN KIOSK", Theme.Ink);
        var monitor = Theme.Action("OPEN QUEUE MONITOR", Theme.Green);
        foreach (var button in new[] { both, kiosk, monitor })
        {
            button.Dock = DockStyle.Fill;
            button.Margin = new Padding(8);
            Theme.Flat(button);
        }
        both.Click += (_, _) => { OpenMonitor(); OpenKiosk(); };
        kiosk.Click += (_, _) => OpenKiosk();
        monitor.Click += (_, _) => OpenMonitor();

        var folder = LinkLabel("Open saved transactions");
        folder.Dock = DockStyle.Top;
        folder.LinkClicked += (_, _) =>
        {
            Directory.CreateDirectory(TransactionStore.DataFolder);
            Process.Start(new ProcessStartInfo { FileName = TransactionStore.DataFolder, UseShellExecute = true });
        };

        layout.Controls.Add(title, 0, 0);
        layout.Controls.Add(both, 0, 1);
        layout.Controls.Add(kiosk, 0, 2);
        layout.Controls.Add(monitor, 0, 3);
        layout.Controls.Add(folder, 0, 4);
        Controls.Add(layout);
    }

    private static LinkLabel LinkLabel(string text) => new()
    {
        Text = text, Font = Theme.Font(12), LinkColor = Theme.Red,
        AutoSize = false, Height = 40, TextAlign = ContentAlignment.MiddleCenter
    };

    private void OpenKiosk()
    {
        if (_kiosk is null || _kiosk.IsDisposed) { _kiosk = new KioskForm(); _kiosk.Show(this); }
        else { _kiosk.WindowState = FormWindowState.Normal; _kiosk.BringToFront(); }
    }

    private void OpenMonitor()
    {
        if (_monitor is null || _monitor.IsDisposed) { _monitor = new QueueMonitorForm(); _monitor.Show(this); }
        else { _monitor.WindowState = FormWindowState.Maximized; _monitor.BringToFront(); }
    }
}
