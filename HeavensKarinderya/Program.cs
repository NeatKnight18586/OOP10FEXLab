using HeavensKarinderya.UI;

namespace HeavensKarinderya;

internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();

        if (args.Contains("--monitor", StringComparer.OrdinalIgnoreCase))
        {
            Application.Run(new QueueMonitorForm());
            return;
        }
        if (args.Contains("--kiosk", StringComparer.OrdinalIgnoreCase))
        {
            Application.Run(new KioskForm());
            return;
        }

        using (var splash = new SplashForm()) splash.ShowDialog();
        Application.Run(new LauncherForm());
    }
}
