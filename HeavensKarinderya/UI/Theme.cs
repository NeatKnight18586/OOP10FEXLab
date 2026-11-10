using System.Drawing;

namespace HeavensKarinderya.UI;

public static class Theme
{
    public static readonly Color Cream = Color.FromArgb(255, 248, 236);
    public static readonly Color White = Color.White;
    public static readonly Color Ink = Color.FromArgb(36, 35, 31);
    public static readonly Color Muted = Color.FromArgb(115, 110, 102);
    public static readonly Color Red = Color.FromArgb(185, 56, 47);
    public static readonly Color RedDark = Color.FromArgb(146, 40, 34);
    public static readonly Color Gold = Color.FromArgb(245, 184, 65);
    public static readonly Color Green = Color.FromArgb(82, 121, 90);
    public static readonly Color Border = Color.FromArgb(227, 215, 198);
    public static readonly Color PaleGold = Color.FromArgb(255, 239, 194);
    public static readonly Color PaleGreen = Color.FromArgb(223, 237, 221);

    public static Font Font(float size, FontStyle style = FontStyle.Regular) =>
        new("Segoe UI", size, style, GraphicsUnit.Point);

    public static Label Text(string value, float size = 14, Color? color = null,
        FontStyle weight = FontStyle.Regular) => new()
    {
        Text = value, ForeColor = color ?? Ink, Font = Font(size, weight),
        AutoSize = false, TextAlign = ContentAlignment.MiddleLeft
    };

    public static Button Action(string text, Color? background = null, Color? foreground = null) => new()
    {
        Text = text, Font = Font(11.5f, FontStyle.Bold),
        ForeColor = foreground ?? Color.White, BackColor = background ?? Red,
        FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand, Height = 48,
        UseVisualStyleBackColor = false
    };

    public static void Flat(Button button)
    {
        button.FlatAppearance.BorderSize = 0;
        button.FlatAppearance.MouseOverBackColor = button.BackColor;
    }

    public static string Pesos(decimal amount) => $"₱{amount:N2}";
}
