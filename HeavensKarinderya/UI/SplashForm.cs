namespace HeavensKarinderya.UI;

public sealed class SplashForm : Form
{
    public SplashForm()
    {
        Text = "Heaven's Karinderya";
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.None;
        ClientSize = new Size(700, 340);
        BackColor = Theme.Red;
        var heading = Theme.Text("HEAVEN'S KARINDERYA", 32, Color.White, FontStyle.Bold);
        heading.Dock = DockStyle.Top;
        heading.Height = 190;
        heading.TextAlign = ContentAlignment.BottomCenter;
        var detail = Theme.Text("Lutong bahay, served with care", 16, Theme.PaleGold);
        detail.Dock = DockStyle.Top;
        detail.Height = 65;
        detail.TextAlign = ContentAlignment.MiddleCenter;
        Controls.Add(detail);
        Controls.Add(heading);
        var timer = new System.Windows.Forms.Timer { Interval = 1300 };
        timer.Tick += (_, _) => { timer.Stop(); timer.Dispose(); Close(); };
        Shown += (_, _) => timer.Start();
    }
}
